using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Notice> notes = new ObservableCollection<Notice>();
        string path = @"C:\Users\ka-er\Desktop\TestOrdner\Notizen.txt";
        FileInfo file;

        public MainWindow()
        {
            InitializeComponent();

            Notices.ItemsSource = notes;
            file = new FileInfo(path);

            if (!string.IsNullOrEmpty(path))
            {
                string[] FileContent = File.ReadAllLines(path);
                foreach (string FileContentItem in FileContent)
                {
                    notes.Add(new Notice(FileContentItem));
                } 
            }
            if (notes.Count > 0)
            {
                Notices.Visibility = Visibility.Visible;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (newNotice.Text == "")
            {
                return;
            }
            else
            {
                notes.Add(new Notice(newNotice.Text));
                newNotice.Text = "";
            }
            if (notes.Count > 0)
            {
                Notices.Visibility = Visibility.Visible;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            notes.Remove((Notice)Notices.SelectedItem);
            if (notes.Count == 0)
            {
                Notices.Visibility = Visibility.Hidden;
            }
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            (Notices.SelectedItem as Notice).Text = newNotice.Text;
            newNotice.Text = "";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\Users\ka-er\Desktop\TestOrdner";
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(path, "");
                using (StreamWriter sw = file.AppendText())
                {
                    foreach (Notice notice in notes)
                    {
                        sw.WriteLine(notice.Text);
                    }
                }
            }
        }
    }

    class Notice : INotifyPropertyChanged
    {
        private string _text;
        public string Text { get { return this._text; }
            set
            {
                if(this._text != value)
                {
                    this._text = value;
                    this.NotifyPropertyChanged("Text");
                }
            }
        }

        public Notice(string _text)
        {
            Text = _text;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
