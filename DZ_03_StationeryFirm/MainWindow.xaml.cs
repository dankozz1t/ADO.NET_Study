using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DZ_03_StationeryFirm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Data Source=DANKOZZ1;Initial Catalog=StationeryFirm;Integrated Security=True";
        private SqlConnection connection;
        private SqlCommand command;
        private DataTable MainTable;
        private SqlDataAdapter adapter;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection = new SqlConnection(ConnectionString);
            command = connection.CreateCommand();
            TryFillComboBoxTableNames();
        }

        private void TryFillComboBoxTableNames()
        {
            string sqlTale = "select [name] from sys.tables where [name] <> 'sysdiagrams';";

            try
            {
                connection.Open();
                command.CommandText = sqlTale;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        ListTableName.Items.Add(reader[i]);
                    }
                }

                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }

        private void TrySelect(string sql)
        {
            try
            {
                MainTable = new DataTable();

                adapter = new SqlDataAdapter(sql, ConnectionString);
                adapter.Fill(MainTable);

                dataGrid.ItemsSource = MainTable.DefaultView;

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = commandBuilder.GetInsertCommand();
                adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                adapter.DeleteCommand = commandBuilder.GetDeleteCommand();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }

        private int TrySelectFind(string sql)
        {
            int id;
            try
            {
                connection.Open();
                command.CommandText = sql;

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                id = int.Parse(reader[0].ToString());

                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }

            return id;
        }

        private void ListTableName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem.ToString() == "Sales")
            {
                ButtonAddRecord.IsEnabled = false;
                ButtoDeleteRecord.IsEnabled = false;
            }
            else
            {
                ButtonAddRecord.IsEnabled = true;
                ButtoDeleteRecord.IsEnabled = true;
            }

            TrySelect("select * from " + ((ComboBox)sender).SelectedItem);
            ButtonAddRecord.Content = "Добавление в " + ((ComboBox)sender).SelectedItem;
        }

        private void UpdateTable()
        {
            if (adapter == null) return;

            adapter.Update(MainTable);
            dataGrid.ItemsSource = null;

            MainTable = new DataTable(ListTableName.Text);

            adapter.Fill(MainTable);

            dataGrid.ItemsSource = MainTable.DefaultView;

        }
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            if (MainTable == null) return;

            AddRecord addRecord = new AddRecord(ListTableName.Text);

            addRecord.Title = $"Добавление в {ListTableName.Text}";

            if (addRecord.ShowDialog() == true)
            {
                DataRow row = FillRow(addRecord);

                MainTable.Rows.Add(row);
            }

            if (CheckBoxUpdate.IsChecked == true)
                UpdateTable();
        }

        private DataRow FillRow(AddRecord addRecord)
        {
            DataRow row = MainTable.NewRow();

            string tableName = ListTableName.SelectedItem.ToString();

            if (tableName == "Types" || tableName == "Firms")
            {
                row["Name"] = addRecord.NameTypeOrFirm.Text;
            }
            else if (tableName == "Managers" || tableName == "Customers")
            {
                row["FirstName"] = addRecord.FirstNamePerson.Text;
                row["LastName"] = addRecord.LastNamePerson.Text;
                row["Email"] = addRecord.EmailPerson.Text;
                row["Phone"] = addRecord.PhonePerson.Text;
            }
            else if (tableName == "Products")
            {
                row["Name"] = addRecord.NameProduct.Text;
                row["TypeId"] = TrySelectFind($"select Types.Id From Types Where Types.Name = '{addRecord.TypeIdProduct.Text}'");
                row["Count"] = addRecord.CountProduct.Text;
                row["Price"] = addRecord.PriceProduct.Text;
                row["FirmId"] = TrySelectFind($"select Firms.Id From Firms Where Firms.Name = '{addRecord.FirmIdProduct.Text}'");
            }

            return row;
        }
        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            string cellValue = GetSelectedCellValue();
            if (cellValue == null) return;

            string sql = $"delete {ListTableName.Text} where {ListTableName.Text}.Id = {cellValue}";

            try
            {
                connection.Open();
                adapter.DeleteCommand = connection.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;
                adapter.DeleteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            connection.Close();

            if (CheckBoxUpdate.IsChecked == true) 
                UpdateTable();

            string GetSelectedCellValue()
            {
                DataGridCellInfo cellInfo = dataGrid.SelectedCells[0];

                DataGridBoundColumn column = cellInfo.Column as DataGridBoundColumn;
                if (column == null) return null;

                FrameworkElement element = new FrameworkElement() { DataContext = cellInfo.Item };
                BindingOperations.SetBinding(element, TagProperty, column.Binding);

                return element.Tag.ToString();
            }
        }
    }
}
