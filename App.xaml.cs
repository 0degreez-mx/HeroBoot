using System.Configuration;
using System.Data;
using System.Windows;

namespace Winhanced_Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Aquí accedes a los argumentos
            string[] args = e.Args;

            /*if (args.Length > 0)
            {
                // Por ejemplo, mostrar el primer argumento
                MessageBox.Show($"Argumento recibido: {args[0]}");
            }*/

            // Puedes pasar los argumentos a la ventana principal
            MainWindow mainWindow = new MainWindow(args);
            mainWindow.Show();
        }

    }

}
