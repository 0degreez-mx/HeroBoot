using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Winhanced_Shell.Settings;

namespace Winhanced_Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const uint SPI_SETWORKAREA = 0x002F;
        private const uint SPIF_UPDATEINIFILE = 0x01 | 0x02;
        private bool playVideo = true;
        private bool useFSE = false;

        private const int PerSourceMax = 5;
        private static readonly TimeSpan FadeDuration = TimeSpan.FromSeconds(2);
        private static readonly TimeSpan HoldDuration = TimeSpan.FromSeconds(3);
        private const double BlurRadius = 25;


        private List<string> _images = new();
        private int _index = 0;
        private DispatcherTimer _cycleTimer;
        private DoubleAnimation _fadeAnim;
        private Process _curLauncher;
        public MainWindow(string[] args)
        {
            SettingsManager.Load();
            InitializeComponent();
            //HideTaskbar();
            playVideo = false;
            if (SettingsManager.Settings != null)
            {
                playVideo = SettingsManager.Settings.PlayIntroVideo;
                useFSE = SettingsManager.Settings.OverrideFSE;
            }
            if (!useFSE)
            {
                if (!playVideo)
                {
                    HideTaskbar();
                    RunApp();
                }
                else
                {
                    //Loaded += MainWindow_Loaded; ;
                    CheckIfVideoPlayable();
                }
            }
            else
            {
                InitializeHeroCycler();
                RunApp();
                if (!playVideo)
                {
                    StartHeroCycler();
                    mediaElement.Visibility = Visibility.Collapsed;
                    loadingText.Visibility = Visibility.Visible;
                }
                else
                {
                    CheckIfVideoPlayable();
                }
            }
        }

        private void CheckIfVideoPlayable()
        {
            if (IsUserLoggedIn())
            {
                PlayBootVideo();
            }
            else
            {
                SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            }
        }

        private void FSEAppKill()
        {
            try
            {
                string[] xboxProcesses = { "XboxPcTray", "XboxPcAppFT", "XboxPcApp" };
                foreach (var procName in xboxProcesses)
                {
                    var processes = Process.GetProcessesByName(procName);
                    foreach (var p in processes)
                    {
                        try
                        {
                            p.Kill();
                            p.WaitForExit();

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            Debug.WriteLine(e.Reason.ToString());
            if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                PlayBootVideo();
            }
        }
        private bool IsUserLoggedIn()
        {
            var query = "SELECT * FROM Win32_ComputerSystem";
            var searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                string? userName = mo["UserName"]?.ToString();
                if (!string.IsNullOrEmpty(userName))
                {
                    return true;
                }
            }
            return false;
        }
        private async Task RunAsAdmin(string appPath, string args)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = appPath,
                UseShellExecute = true,
                //Verb = "runas",
                WorkingDirectory = Path.GetDirectoryName(appPath)
            };
            if (args != null)
            {
                psi.Arguments = args;
            }
            _curLauncher = Process.Start(psi);
            if (_curLauncher != null)
            {
                /*while (_curLauncher.MainWindowHandle == IntPtr.Zero)
                {
                    //Thread.Sleep(100);
                    await Task.Delay(100);
                    _curLauncher.Refresh();
                }*/
                if((!useFSE && !playVideo))
                {
                    await Task.Delay(5000);
                }
                //_curLauncher.WaitForInputIdle(30000);
            }
        }

        private void HideTaskbar()
        {
            return;
            IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", null);
            IntPtr startButtonHandle = FindWindow("Button", "Start");

            ShowWindow(taskbarHandle, SW_HIDE);
            ShowWindow(startButtonHandle, SW_HIDE);

            SystemParametersInfo(SPI_SETWORKAREA, 0, IntPtr.Zero, SPIF_UPDATEINIFILE);
        }

        private void ShowTaskbar()
        {
            IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", null);
            ShowWindow(taskbarHandle, SW_SHOW);
        }

        private async Task StartBackgroundApp(string appPath)
        {
            await RunAsAdmin(appPath, null);
        }

        private async void PlayBootVideo()
        {
            string videoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup-video.mp4");
            mediaElement.Source = new Uri(videoPath);
            mediaElement.MediaEnded += MediaElement_MediaEnded;



            await Task.Delay(1000);
            HideTaskbar();
            await Task.Delay(500);
            this.Topmost = false;
            await Task.Delay(250);
            this.Topmost = true;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (!useFSE)
            {
                RunApp();
            }
            else
            {                 
                StartHeroCycler();
                mediaElement.Visibility = Visibility.Collapsed;
                loadingText.Visibility = Visibility.Visible;
            }
        }

        private async void RunApp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string parentPath = Directory.GetParent(basePath).FullName;

            string appPath = SettingsManager.Settings?.LauncherPath;
            //ShowTaskbar();
            if (appPath != string.Empty && File.Exists(appPath))
            {
                await StartBackgroundApp(appPath);
            }
            if (!useFSE)
            {
                Close();
            }
        }

        private void InitializeHeroCycler()
        {
            if (BlurImage.Effect is BlurEffect be)
                be.Radius = BlurRadius;


            _images = CollectHeroImagesWPF();

            if (_images.Count == 0)
            {
                loadingText.Text = "No Images Found";
                return;
            }
        }
        private void StartHeroCycler()
        {
            SetBothLayers(_images[_index]);
            _fadeAnim = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(FadeDuration),
                FillBehavior = FillBehavior.HoldEnd
            };
            _fadeAnim.Completed += OnFadeCompleted;

            _cycleTimer = new DispatcherTimer { Interval = HoldDuration };
            _cycleTimer.Tick += (s, e) => StartFade();

            StartFade();
        }
        private void StartFade()
        {
            _cycleTimer.Stop();
            BlurImage.Opacity = 0;
            BlurImage.BeginAnimation(UIElement.OpacityProperty, _fadeAnim);
        }
        private int FadesCompleted = 0;
        private void OnFadeCompleted(object? sender, EventArgs e)
        {
            
            FadesCompleted++;
            if (FadesCompleted == 5)
            {
                if (_curLauncher != null)
                {
                    Close();
                }
            }
            _cycleTimer.Stop();
            _cycleTimer = new DispatcherTimer { Interval = HoldDuration };
            _cycleTimer.Tick += (s, e) => StartFade();
            _index = (_index + 1) % _images.Count;
            SetBothLayers(_images[_index]);

            BlurImage.Opacity = 0;
            _cycleTimer.Start();
        }


        private List<string> CollectHeroImagesWPF()
        {
            var result = new List<string>();

            string basePath = SettingsManager.Settings?.HeroImagesPath;
            if(basePath == string.Empty)
            {
                //loadingText.Text = "Hero images path empty.";
                return result;
            }
            
            if(SettingsManager.Settings?.HeroImagePattern == string.Empty)
            {
                //loadingText.Text = "Hero images path empty.";
                return result;
            }
            
            static IEnumerable<string> TakeUpTo(string root, int count)
            {
                if (!Directory.Exists(root)) yield break;

                var files = Directory.EnumerateFiles(root, SettingsManager.Settings?.HeroImagePattern, SearchOption.TopDirectoryOnly)
                .OrderBy(_ => Guid.NewGuid())
                .Take(count);
                foreach (var f in files) yield return f;
            }


            result.AddRange(TakeUpTo(basePath, 10));


            return result;
        }


        private void SetBothLayers(string path)
        {
            try
            {
                var bmp = LoadBitmap(path);
                BaseImage.Source = bmp;
                BlurImage.Source = bmp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando {path}: {ex.Message}");
                _images.Remove(path);
                if (_images.Count > 0)
                {
                    _index %= _images.Count;
                    SetBothLayers(_images[_index]);
                }
            }
        }


        private static BitmapImage LoadBitmap(string path)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmp.UriSource = new Uri(path, UriKind.Absolute);
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (useFSE)
            {
                FSEAppKill();
            }
        }
    }
}