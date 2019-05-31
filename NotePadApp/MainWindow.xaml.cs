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

        public MainWindow()
        {
            InitializeComponent();

            Timer timer = new Timer(SaveFile, _saveUri, TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(1));

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

        private void SaveFile(object path)
        {
            try
            {
                string text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

                using (StreamWriter writer = new StreamWriter((string)path))
                {
                    writer.WriteLine(text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка записи");
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
            }

            SaveFile(_saveUri);
        }
    }
}
