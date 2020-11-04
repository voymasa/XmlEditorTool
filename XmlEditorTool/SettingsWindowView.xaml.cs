using System;
using System.Collections.Generic;
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
        }

        private void OpenMapperDialog(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Comma-Seperated Value (*.csv)|*.csv";

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
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    SourceFileDirTxtBox.Text = dlg.SelectedPath;
                }
            }
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MapperFile = MapperFileTxtBox.Text;
            Properties.Settings.Default.SourceFileDir = SourceFileDirTxtBox.Text;
            Properties.Settings.Default.MacroPrefix = MacroPrefixTxtBox.Text;
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
    }
}
