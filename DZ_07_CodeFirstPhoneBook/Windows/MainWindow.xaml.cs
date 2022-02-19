using System.Linq;
using System.Windows;
using DZ_07_CodeFirstPhoneBook.DbContext;
using DZ_07_CodeFirstPhoneBook.Entities;
using DZ_07_CodeFirstPhoneBook.Windows;

namespace DZ_07_CodeFirstPhoneBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhoneBookDb database = new PhoneBookDb();
        public MainWindow()
        {

            InitializeComponent();
            ListBoxGroup.ItemsSource = database.Groups.ToList();
        }

        private void ListBoxGroup_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Group group = ListBoxGroup.SelectedItem as Group;

            if (group != null)
            {
                ListBoxContacts.ItemsSource = group.Contacts.ToList();
            }
        }

        private void Button_AllGroups_Click(object sender, RoutedEventArgs e)
        {
            ListBoxContacts.ItemsSource = database.Contacts.ToList();
            ListBoxGroup.SelectedItem = null;
        }

        private void Button_AddContact_Click(object sender, RoutedEventArgs e)
        {
            AddContactWindow window = new AddContactWindow();

            if (window.ShowDialog() == true)
            {
                Contact addContact = new Contact()
                {
                    FirstName = window.FirstName,
                    LastName = window.LastName,
                    Phone = window.Phone,
                    Email = window.Email,
                    Birthday = window.Birthday,
                    Priority = window.Priority,
                    IsBlocking = window.IsBlocking,
                    GroupId = ((Group)ListBoxGroup.SelectedItem).Id
                };

                database.Contacts.Add(addContact);
                database.SaveChanges();
            }
        }

        private void Button_DeleteContact_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxContacts.SelectedItem as Contact == null)
            {
                MessageBox.Show("Выберите контакт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            database.Contacts.Remove(ListBoxContacts.SelectedItem as Contact);
        }

        private void Button_UpdateContact_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxContacts.SelectedItem as Contact == null)
            {
                MessageBox.Show("Выберите контакт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            Contact contact = ListBoxContacts.SelectedItem as Contact;

            AddContactWindow window = new AddContactWindow()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Phone = contact.Phone,
                Email = contact.Email,
                Birthday = contact.Birthday,
                Priority = contact.Priority,
                IsBlocking = contact.IsBlocking,
            };

            if (window.ShowDialog() == true)
            {
                contact.FirstName = window.FirstName;
                contact.LastName = window.LastName;
                contact.Phone = window.Phone;
                contact.Email = window.Email;
                contact.Birthday = window.Birthday;
                contact.Priority = window.Priority;
                contact.IsBlocking = window.IsBlocking;
                contact.GroupId = ((Group)ListBoxGroup.SelectedItem).Id;

                database.SaveChanges();
            }
        }
    }
}
