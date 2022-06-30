using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

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
        private const string jsonFileName = "Notes.json";

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(jsonFileName))
            {
                File.Create("Notes.json").Close();
            }
            string jsonFile = File.ReadAllText(jsonFileName);
            if (!string.IsNullOrEmpty(jsonFile))
            {
                notes = JsonSerializer.Deserialize<ObservableCollection<Notice>>(jsonFile);
            }

            Notices.ItemsSource = notes;
            
            if (notes.Count > 0)
            {
                Notices.Visibility = Visibility.Visible;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(newNoticeTitel.Text) && !string.IsNullOrEmpty(newNoticeText.Text))
            {
                notes.Add(new Notice(newNoticeTitel.Text, newNoticeText.Text));
                newNoticeTitel.Text = "";
                newNoticeText.Text = "";
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

        private void overwriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(newNoticeTitel.Text) && !string.IsNullOrEmpty(newNoticeText.Text))
            {
                (Notices.SelectedItem as Notice).Text = newNoticeTitel.Text;
                (Notices.SelectedItem as Notice).Note = newNoticeText.Text;
                newNoticeTitel.Text = "";
                newNoticeText.Text = "";
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            newNoticeTitel.Text = (Notices.SelectedItem as Notice).Text;
            newNoticeText.Text = (Notices.SelectedItem as Notice).Note;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string jsonFile = JsonSerializer.Serialize(notes);
            File.WriteAllText(jsonFileName, jsonFile);
        }

        private void Notices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Notices.SelectedItem != null)
            {
                deleteButton.IsEnabled = true;
                overwriteButton.IsEnabled = true;
                openButton.IsEnabled = true;
            }
            else
            {
                deleteButton.IsEnabled = false;
                overwriteButton.IsEnabled = false;
                openButton.IsEnabled = false;
            }
        }
    }

    [Serializable]
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
        public string Note { set; get; }

        public Notice(string text, string note)
        {
            Text = text;
            Note = note;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null && this._text != "")
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
