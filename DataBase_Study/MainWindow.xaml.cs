using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

namespace DataBase_Study
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string selectSQL = "SELECT * FROM Doctors";

            SqlConnection sqlConnection = new SqlConnection("Data Source=DANKOZZ1;Initial Catalog=Hospital;Integrated Security=True");
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection);

            SqlDataReader reader = sqlCommand.ExecuteReader();

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
                ListBox2.Items.Add(doc);

            }

            reader.Close();

        }
        //public SqlDataReader Select(string selectSQL) // функция подключения 
        //{
        //    DataTable dataTable = new DataTable("dataBase");
        //    SqlConnection sqlConnection = new SqlConnection("Data Source=DANKOZZ1;Initial Catalog=Hospital;Integrated Security=True");
        //    sqlConnection.Open();
        //    SqlCommand sqlCommand = new SqlCommand(selectSQL, sqlConnection);
        //    SqlDataReader reader = sqlCommand.ExecuteReader();
        //    return reader;
        //}
    }
}

