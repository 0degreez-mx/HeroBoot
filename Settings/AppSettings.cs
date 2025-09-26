using Winhanced_Shell.Models;

namespace Winhanced_Shell.Settings
{
    public class AppSettings
    {
        public string HeroImagesPath { get; set; }
        public string HeroImagePattern {  get; set; }
        public string LauncherPath {  get; set; }
        public bool OverrideFSE { get; set; }
        public bool PlayIntroVideo { get; set; }
        public AppSettings()
        {
            HeroImagesPath = "";
            HeroImagePattern = "";
            LauncherPath = "";
            OverrideFSE = false;
            PlayIntroVideo = true;
        }
    }
}
