using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XmlEditorTool
{
    /// <summary>
    /// Interaction logic for SettingsWindowView.xaml
    /// </summary>
    public partial class SettingsWindowView : Window
    {
        public SettingsWindowView()
        {
            InitializeComponent();
            MapperFileTxtBox.Text = Properties.Settings.Default.MapperFile;
            SourceFileDirTxtBox.Text = Properties.Settings.Default.SourceFileDir;
            MacroPrefixTxtBox.Text = Properties.Settings.Default.MacroPrefix;
            MacroDataFileTxtBox.Text = Properties.Settings.Default.DatatypeMapFile;
        }

        private void OpenMapperDialog(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Comma-Seperated Value (*.csv)|*.csv";
            dlg.InitialDirectory = ValidatePathwayForNullOrEmpty(Properties.Settings.Default.MapperFile);

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                //TODO -- change this to use a Model/ViewModel later
                MapperFileTxtBox.Text = filename;
            }
        }

        private void OpenSourceFileDialog(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.ShowNewFolderButton = true;
                dlg.SelectedPath = ValidatePathwayForNullOrEmpty(Properties.Settings.Default.SourceFileDir);
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    SourceFileDirTxtBox.Text = dlg.SelectedPath;
                }
            }
        }

        private void OpenMacroFileDialog(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Comma-Seperated Value (*.csv)|*.csv";
            dlg.InitialDirectory = ValidatePathwayForNullOrEmpty(Properties.Settings.Default.MapperFile);

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                //TODO -- change this to use a Model/ViewModel later
                MacroDataFileTxtBox.Text = filename;
            }
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MapperFile = MapperFileTxtBox.Text;
            Properties.Settings.Default.SourceFileDir = SourceFileDirTxtBox.Text;
            Properties.Settings.Default.MacroPrefix = MacroPrefixTxtBox.Text;
            Properties.Settings.Default.DatatypeMapFile = MacroDataFileTxtBox.Text;
            try
            {
                Properties.Settings.Default.Save();
                OpMsg.Foreground = Brushes.Green;
                OpMsg.Text = "Settings Saved.";
            }
            catch
            {
                OpMsg.Foreground = Brushes.Red;
                OpMsg.Text = "Failed to save settings.";
            }
        }

        private void CloseSettings(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This method is used to check for a null or empty string for the given pathway, and
        /// default to the user's home folder.
        /// </summary>
        /// <param name="path">a string representation of the path to validate</param>
        /// <returns>the path passed in, or the user home directory if the path is null or empty</returns>
        private string ValidatePathwayForNullOrEmpty(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else
            {
                return path;
            }
        }
    }
}
