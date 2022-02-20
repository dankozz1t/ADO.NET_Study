using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace DZ_07_CodeFirstPhoneBook.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddContacts.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        public string FirstName
        {
            get => TextBoxFirstName.Text;
            set => TextBoxFirstName.Text = value;
        }

        public string LastName
        {
            get => TextBoxLastName.Text;
            set => TextBoxLastName.Text = value;
        }
        public string Phone
        {
            get => TextBoxPhone.Text;
            set => TextBoxPhone.Text = value;
        }

        public string Email
        {
            get => TextBoxEmail.Text;
            set => TextBoxEmail.Text = value;
        }

        public DateTime Birthday
        {
            get => DateTime.Parse(TextBoxBirthday.Text);
            set => TextBoxBirthday.Text = value.ToShortDateString();
        }

        public short Priority
        {
            get => short.Parse(TextBoxPriority.Text);
            set => TextBoxPriority.Text = value.ToString();
        }

        public bool IsBlocking
        {
            get => ComboBoxIsBlocking.Text == "True";
            set => ComboBoxIsBlocking.Text = value.ToString();
        }

        public byte[] Photo
        {
            get => BitmapImageToByte(ImagePhoto.Source as BitmapImage);

            set
            {
                if (value != null)
                    ImagePhoto.Source = ByteToBitmapImage(value);
            }
        }

        public AddContactWindow()
        {
            InitializeComponent();
        }

        #region Convert

        public BitmapImage ByteToBitmapImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public byte[] BitmapImageToByte(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        #endregion
        private void Button_OpenPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                TextBoxPathPhoto.Text = openFileDialog.FileName;
            }

            ImagePhoto.Source = new BitmapImage(new Uri(TextBoxPathPhoto.Text));
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxFirstName.Text == "" || TextBoxLastName.Text == "" || TextBoxPhone.Text == "" ||
                TextBoxEmail.Text == "" || TextBoxBirthday.Text == "" || TextBoxPriority.Text == "" ||
                ComboBoxIsBlocking.Text == "")
            {
                MessageBox.Show("Заполните все данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string phoneRegex = @"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}";
            if (!Regex.IsMatch(TextBoxPhone.Text, phoneRegex))
            {
                MessageBox.Show("Неправильно введен Phone! Минимум 10 цифр, можно использовать +38 или -", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string emailRegex = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
            if (!Regex.IsMatch(TextBoxEmail.Text, emailRegex))
            {
                MessageBox.Show("Неправильно введен Email! Формат (*****@****.***)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime birthdayRegex = new DateTime();
            if (!DateTime.TryParse(TextBoxBirthday.Text, out birthdayRegex))
            {
                MessageBox.Show("Заполните правильно дату рождения. Формат (день.месяц.год) (22.02.2022)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            short priorityRegex;
            if (!short.TryParse(TextBoxPriority.Text, out priorityRegex))
            {
                MessageBox.Show("Приоритетность в цифрах!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
