using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotePadApp
{
    public partial class MainWindow : Window
    {
        private string _saveUri;
        private Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            _saveUri = "NewFile_" + Guid.NewGuid().ToString() + ".txt";

            timer = new Timer(SaveFile, _saveUri, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60));

        }

        private void CreateMenuItemClick(object sender, RoutedEventArgs e)
        {
            if ((MessageBox.Show("Сохранить файл?", "", MessageBoxButton.YesNo)) == MessageBoxResult.Yes)
            {
                if (_saveUri == null)
                {
                    SaveAsMenuItemClick(null, null);
                }
                else
                {
                    SaveFile(_saveUri);
                }

            }
            richTextBox.Document.Blocks.Clear();
        }

        private void OpenMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog();

                bool? result = openDialog.ShowDialog();

                if (result == true)
                {
                    _saveUri = openDialog.FileName;
                }

                richTextBox.Document.Blocks.Clear();

                using (StreamReader reader = new StreamReader(_saveUri))
                {
                    richTextBox.Document.Blocks.Add(new Paragraph(new Run(reader.ReadToEnd())));
                }
            }
            catch (Exception)
            {}
        }

        private void SaveFile(object path)
        {
            try
            {
                if (!File.Exists((string)path))
                {
                    File.Create((string)path);
                }

                string text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

                if (text.Length <= 0 || text == "\r\n") return;

                using (StreamWriter writer = new StreamWriter((string)path))
                {
                    writer.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка записи\n" + e.Message);
            }
        }

        private void SaveMenuItemClick(object sender, RoutedEventArgs e)
        {
            SaveFile(_saveUri);
        }

        private void SaveAsMenuItemClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            bool? result = openDialog.ShowDialog();

            if (result == true)
            {
                _saveUri = openDialog.FileName;
                timer = null;
                timer = new Timer(SaveFile, _saveUri, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30));
            }

            SaveFile(_saveUri);
        }

        private void PrintMenuItemClick(object sender, RoutedEventArgs e)
        {
            SaveFile(_saveUri);
            new PrintDialog().ShowDialog();
        }

        private void ExitMenuItemClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
