using System.IO;
using System.Linq;
using System.Windows;
using DZ_07_CodeFirstPhoneBook.DbContext;
using DZ_07_CodeFirstPhoneBook.Helpers;
using DZ_07_CodeFirstPhoneBook.Windows;


namespace DZ_07_CodeFirstPhoneBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhoneBookDb database;


        public MainWindow()
        {
            InitializeComponent();

            string path1 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=";
            string path2 = @"PhoneBook.mdf;Integrated Security=True;";
            string path3 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\Database\");
            DirectoryInfo directoryInfo = new DirectoryInfo(path3);

            database = new PhoneBookDb($"{path1}{directoryInfo.FullName}{path2}");


            UpdateListBoxGroup();
            ComboBoxUpdate();
        }

        private void ComboBoxUpdate()
        {
            ComboBoxEditGroup.Items.Clear();
            foreach (var group in database.Groups.ToList())
            {
                ComboBoxEditGroup.Items.Add(group.Name);
            }
            if (ListBoxGroup.SelectedItem != null)
                ComboBoxEditGroup.SelectedValue = (ListBoxGroup.SelectedItem as Group).Name;
        }

        private void Button_AllGroups_Click(object sender, RoutedEventArgs e)
        {
            ListBoxContacts.ItemsSource = database.Contacts.ToList();
            ListBoxGroup.SelectedItem = null;

            TextBoxNameGroup.Text = "";
        }

        #region Contacts
        private void UpdateListBoxContacts()
        {
            Group group = ListBoxGroup.SelectedItem as Group;

            if (group != null)
            {
                ListBoxContacts.ItemsSource = group.Contacts.ToList();
            }
        }

        private void ListBoxContacts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Contact contact = ListBoxContacts.SelectedItem as Contact;
            if (contact != null)
            {
                ComboBoxEditGroup.SelectedValue = contact.Group.Name;
            }

            UpdateListBoxContacts();
        }

        #region CRUD_Contacts

        private void Button_AddContact_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxGroup.SelectedItem as Group == null)
            {
                MessageBox.Show("Выберите группу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
                    GroupId = ((Group)ListBoxGroup.SelectedItem).Id,
                    Photo = window.Photo
                };

                database.Contacts.Add(addContact);
                database.SaveChanges();

                UpdateListBoxContacts();
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
            database.SaveChanges();

            UpdateListBoxContacts();
        }

        private void Button_UpdateContact_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxContacts.SelectedItem is Contact contact)
            {
                AddContactWindow window = new AddContactWindow()
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Phone = contact.Phone,
                    Email = contact.Email,
                    Birthday = contact.Birthday,
                    Priority = contact.Priority,
                    IsBlocking = contact.IsBlocking,
                    Photo = contact.Photo
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
                    contact.Photo = window.Photo;

                    database.SaveChanges();

                    UpdateListBoxContacts();
                }
            }
            else
            {
                MessageBox.Show("Выберите контакт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_EditGroupContact_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxContacts.SelectedItem is Contact contact)
            {
                foreach (var gGroup in database.Groups)
                {
                    if (gGroup.Name == ComboBoxEditGroup.SelectedValue)
                    {
                        contact.GroupId = gGroup.Id;
                    }
                }

                database.SaveChanges();
                UpdateListBoxContacts();
            }
            else
            {
                MessageBox.Show("Выберите контакт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #endregion

        #region Groups

        private void UpdateListBoxGroup()
        {
            ListBoxGroup.ItemsSource = database.Groups.ToList();
        }

        private void ListBoxGroup_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Group group = ListBoxGroup.SelectedItem as Group;
            if (group != null)
            {
                TextBoxNameGroup.Text = group.Name;
                ComboBoxEditGroup.SelectedValue = group.Name;
            }

            UpdateListBoxContacts();
        }

        #region CRUD_Groups

        #region Helpers

        private bool IsUniqueGroupName(string groupName)
        {
            bool isUnique = false;
            foreach (var itemGroup in database.Groups)
            {
                if (itemGroup.Name == groupName)
                {
                    isUnique = true;
                    break;
                }
            }

            return isUnique;
        }

        #endregion

        private void Button_AddGroup_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNameGroup.Text == "")
            {
                MessageBox.Show("Заполните Name", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsUniqueGroupName(TextBoxNameGroup.Text))
            {
                MessageBox.Show("Такое имя группы уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Group group = new Group()
            {
                Name = TextBoxNameGroup.Text
            };

            ComboBoxEditGroup.Items.Add(group.Name);
            database.Groups.Add(group);
            database.SaveChanges();

            UpdateListBoxGroup();
        }

        private void Button_EditGroup_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxGroup.SelectedItem is Group group)
            {
                if (TextBoxNameGroup.Text == "")
                {
                    MessageBox.Show("Заполните Name", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (IsUniqueGroupName(TextBoxNameGroup.Text))
                {
                    MessageBox.Show("Такое имя группы уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                group.Name = TextBoxNameGroup.Text;
                database.SaveChanges();

                UpdateListBoxGroup();
                ComboBoxUpdate();
            }
        }

        private void Button_DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            Group group = ListBoxGroup.SelectedItem as Group;
            if (group == null)
            {
                MessageBox.Show("Выберите группу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            database.Groups.Remove(group);
            database.SaveChanges();

            UpdateListBoxContacts();
            UpdateListBoxGroup();
        }

        #endregion

        #endregion

    }
}
