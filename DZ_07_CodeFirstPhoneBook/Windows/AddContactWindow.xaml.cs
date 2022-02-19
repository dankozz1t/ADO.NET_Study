using System;
using System.Windows;

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
            get => (TextBoxIsBlocking.Text == "True");
            set => TextBoxIsBlocking.Text = value.ToString();
        }

        public AddContactWindow()
        {
            InitializeComponent();
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
