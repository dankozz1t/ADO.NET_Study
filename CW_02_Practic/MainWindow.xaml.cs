using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
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

namespace CW_02_Practic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string connectionString = "Data Source=DANKOZZ1;Initial Catalog=Academy2;Integrated Security=True";
        private SqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }

            string sqlTale = "select [name] from sys.tables where [name] <> 'sysdiagrams';";
            SqlCommand commandd = connection.CreateCommand();
            commandd.CommandText = sqlTale;
            SqlDataReader reader = commandd.ExecuteReader();
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

        private void ShowTable(string tableName)
        {
            string sql = "Select * From " + tableName;
            SqlCommand commandd = connection.CreateCommand();
            commandd.CommandText = sql;

            SqlDataReader reader = commandd.ExecuteReader();

            bool firstTime = true;
            while (reader.Read())
            {
                if (firstTime)
                {
                    string header = null;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        header += reader.GetName(i) + " | ";
                        firstTime = false;
                    }

                    TextBlock.Text = header;
                }

                string doc = null;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    doc += reader[i];
                    doc += " | ";
                }
                ListDataTable.Items.Add(doc);

            }
            reader.Close();

        }

        private void Button_LOAD_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();
            connection.Open();
            ShowTable(ListTableName.Text);
            connection.Close();

        }

        private void Button_DELETE_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();

            int id = ListDataTable.SelectedIndex;
            id++;
            
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $"delete from {ListTableName.SelectedItem} where [id] = @id";
            SqlParameter parameterId = new SqlParameter("@id", id);
            command.Parameters.Add(parameterId);
            command.ExecuteNonQuery();

            connection.Close();
        }

        private void Button_ADD_Click(object sender, RoutedEventArgs e)
        {
            //string sql;
            //SqlCommand command = connection.CreateCommand();

            //switch (ListTableName.SelectedItem)
            //{
            //    case "Teachers":
            //        string name;
            //        string surname
            //        return;
            //}

            //connection.Open();

            //int id = 7;
            //string tableName = "Teachers";

            //SqlCommand command = connection.CreateCommand();
            //command.CommandText = $"delete from {tableName} where [id] = @id";
            //SqlParameter parameterId = new SqlParameter("@id", id);
            //command.Parameters.Add(parameterId);
            //command.ExecuteNonQuery();

            //connection.Close();
        }
    }
}
