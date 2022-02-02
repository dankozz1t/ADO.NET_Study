using System;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading;
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
        private DataTable MainTable = new DataTable();
        private SqlDataAdapter adapter;


        private delegate void AsyncAdapter(SqlDataAdapter adapterToUpdate, DataTable tableToUpdate);
        private AsyncAdapter updateAdapter;
        private AsyncAdapter fillAdapter;

        private Action showLoadImage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection = new SqlConnection(ConnectionString);
            command = connection.CreateCommand();
            adapter = new SqlDataAdapter("select * from types", ConnectionString);
            TryFillComboBoxTableNames();

            // для доступа к элементам из другого потока
            showLoadImage = () =>
            {
                //dataGrid.ItemsSource = null;
                ImageLoad.Visibility = Visibility.Visible;
            };

            updateAdapter = new AsyncAdapter((a, t) =>
            {
                Dispatcher.Invoke(showLoadImage);
                a.Update(t);
            });

            fillAdapter = new AsyncAdapter((a, t) =>
            {
                Dispatcher.Invoke(showLoadImage);
          
                t.Clear();
                a.Fill(t);
            });

            ListTableName.SelectedIndex = 0;
        }


        #region ComboBox_ListTableName

        private void TryFillComboBoxTableNames() //Заполняет комбобокс всеми видами таблиц в базе данных
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

            fillAdapter.BeginInvoke(adapter, MainTable, AsyncCbFill, null);

            ButtonAddRecord.Content = "Добавление в " + ((ComboBox)sender).SelectedItem;
        }

        #endregion


        #region CRUD

        private void AsyncCbUpdate(IAsyncResult ar)
        {
            updateAdapter.EndInvoke(ar);
            // завершаем асинхронную операцию
            // выводим результат
            fillAdapter.BeginInvoke(adapter, MainTable, AsyncCbFill, null);
        }
        private void AsyncCbFill(IAsyncResult ar)
        {
            fillAdapter.EndInvoke(ar);

            Action a = () =>
            {
                MainTable = new DataTable();

                string sql = "WAITFOR DELAY '00:00:00.5'; " + "select * from " + ListTableName.SelectedItem;
                adapter = new SqlDataAdapter(sql, ConnectionString);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                adapter.Fill(MainTable);
                dataGrid.ItemsSource = MainTable.DefaultView;

                ImageLoad.Visibility = Visibility.Collapsed;
            };


            // проверяем необходимость переадресации, если работа идет в другом потоке иначе бужет ошибка доступа к элементу UI интерфейса
            if (!CheckAccess())
            {
                Dispatcher.Invoke(a);
            }
            else
            {
                a();
            }

        }

        private void UpdateTable()
        {
            updateAdapter.BeginInvoke(adapter, MainTable, AsyncCbUpdate, null);
        }
        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            if (MainTable == null) return;

            AddRecord addRecord = new AddRecord(ListTableName.Text);

            addRecord.Title = $"Добавление в {ListTableName.Text}";

            if (addRecord.ShowDialog() == true)
            {
                DataRow row = FillRow(addRecord, true);

                MainTable.Rows.Add(row);
            }

            if (CheckBoxAutoSave.IsChecked == true)
                UpdateTable();
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (MainTable == null) return;

            AddRecord addRecord = new AddRecord(ListTableName.Text);

            addRecord.Title = $"Изменение в {ListTableName.Text}";

            FillWindow(addRecord);

            if (addRecord.ShowDialog() == true)
            {
                DataRow row = FillRow(addRecord, false);
            }

            if (CheckBoxAutoSave.IsChecked == true)
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

            if (CheckBoxAutoSave.IsChecked == true)
                UpdateTable();
        }

        #endregion


        #region HelpersMethod

        private string GetSelectedCellValue(int columnIndex = 0) //Достает значение колонки выделеной записи. 0 - Id
        {
            DataGridCellInfo cellInfo = dataGrid.SelectedCells[columnIndex];
            DataGridBoundColumn column = cellInfo.Column as DataGridBoundColumn;

            if (column == null) return null;

            FrameworkElement element = new FrameworkElement() { DataContext = cellInfo.Item };
            BindingOperations.SetBinding(element, TagProperty, column.Binding);

            return element.Tag.ToString();
        }

        private string TrySelectFind(string sql) //Поиск значения по запросу
        {
            string data;
            try
            {
                connection.Open();
                command.CommandText = sql;

                SqlDataReader reader = command.ExecuteReader();

                reader.Read();
                data = reader[0].ToString();

                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
            return data;
        }

        private DataRow FillRow(AddRecord addRecord, bool isAppend) //Заполняет DataRow значениями из окна добавления
        {
            DataRow row = null;
            if (isAppend)
            {
                row = MainTable.NewRow();
            }
            else
            {
                row = MainTable.Rows[dataGrid.SelectedIndex];
            }

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

        private void FillWindow(AddRecord addRecord) //При изменении записи, заполняет окно старыми значениями
        {
            string cellValue = GetSelectedCellValue();
            if (cellValue == null) return;

            string tableName = ListTableName.SelectedItem.ToString();

            if (tableName == "Types" || tableName == "Firms")
            {
                addRecord.NameTypeOrFirm.Text = TrySelectFind($"Select F.Name From {ListTableName.Text} F Where F.Id = {cellValue}");
            }
            else if (tableName == "Managers" || tableName == "Customers")
            {
                addRecord.FirstNamePerson.Text = TrySelectFind($"Select F.FirstName From {ListTableName.Text} F Where F.Id = {cellValue}");
                addRecord.LastNamePerson.Text = TrySelectFind($"Select F.LastName From {ListTableName.Text} F Where F.Id = {cellValue}");
                addRecord.EmailPerson.Text = TrySelectFind($"Select F.Email From {ListTableName.Text} F Where F.Id = {cellValue}");
                addRecord.PhonePerson.Text = TrySelectFind($"Select F.Phone From {ListTableName.Text} F Where F.Id = {cellValue}");

            }
            else if (tableName == "Products")
            {
                addRecord.NameProduct.Text = TrySelectFind($"Select F.Name From {ListTableName.Text} F Where F.Id = {cellValue}");
                addRecord.CountProduct.Text = TrySelectFind($"Select F.Count From {ListTableName.Text} F Where F.Id = {cellValue}");
                addRecord.PriceProduct.Text = TrySelectFind($"Select F.Price From {ListTableName.Text} F Where F.Id = {cellValue}");

                //Нужно привязать TypeIdProduct.SelectedIndex и FirmIdProduct.SelectedIndex к cellValue (Но по индексу КомбоБокса)
            }
        }

        #endregion
    }
}
