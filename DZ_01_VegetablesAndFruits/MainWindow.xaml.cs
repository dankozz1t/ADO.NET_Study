using System;
using System.Data.SqlClient;
using System.Windows;

namespace DZ_01_VegetablesAndFruits
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string connectionString = "Data Source=DANKOZZ1;Initial Catalog=VegetablesAndFruits;Integrated Security=True";
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
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
            
            connection.Close();
        }

        private void Select(string sql)
        {
            SqlCommand commandd = connection.CreateCommand();
            commandd.CommandText = sql;
            connection.Open();
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
                    doc += ",  ";
                }
                ListDataTable.Items.Add(doc);

            }
            reader.Close();
            connection.Close();
        }

        #region CRUD
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

        private void Button_UPDATE_Click(object sender, RoutedEventArgs e)
        {

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

        #endregion

        #region Button_DZ_01_3

        private void Button_ALL_INFO_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();

            Select("Select * From " + ListTableName.Text);
        }

        private void Button_NAME_INFO_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();
            Select("Select Id, Name From " + ListTableName.Text);
        }

        private void Button_COLOR_INFO_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();
            Select("Select Id, Color From " + ListTableName.Text);
        }

        private void Button_MAX_CALORIES_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();
            Select("Select max(Calories) AS 'Max Calories', min(Calories) AS 'Min Calories',  AVG(Calories) AS 'AVG Calories' From " + ListTableName.Text);
        }

        #endregion

        #region Button_DZ_02_2

        private void Button_COUNT_VEGETABLES_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();
            //Select count(Id)From Products Where Type = 'Овощь'
            Select("Select count(Id) AS 'Count Vegetables' From " + ListTableName.Text + " Where Type = 'Овощь'");
        }

        private void Button_COUNT_FRUINTS_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();
            Select("Select count(Id) AS 'Count Fruits' From " + ListTableName.Text + " Where Type = 'Фрукт'");
        }

        private void Button_COUNT_COLOR_Click(object sender, RoutedEventArgs e)
        {
            if (VF_Color.Text != "")
            {
                ListDataTable.Items.Clear();
                Select("Select count(Id) AS 'Count Color' From " + ListTableName.Text + " Where Color = '" +
                       VF_Color.Text + "'");
            }
        }

        private void Button_COUNT_ALL_COLORS_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();

            //Select Color AS 'Цвет', count(Id) AS 'Количество'From Products  Group By  Color
            Select("Select Color, count(Id) AS 'Count Color' From " + ListTableName.Text +" Group By Color");
        }

        private void Button_COLORIES_DOWN_Click(object sender, RoutedEventArgs e)
        {
            if (VF_Calories.Text != "")
            {
                ListDataTable.Items.Clear();
                Select("Select * From " + ListTableName.Text + " Where Calories <= " + VF_Calories.Text + "");
            }
        }

        private void Button_COLORIES_UP_Click(object sender, RoutedEventArgs e)
        {
            if (VF_Calories.Text != "")
            {
                ListDataTable.Items.Clear();
                Select("Select * From " + ListTableName.Text + " Where Calories >= " + VF_Calories.Text + "");
            }
        }

        private void Button_COLORIES_FROM_TO_Click(object sender, RoutedEventArgs e)
        {
            if (VF_Calories_From.Text != "" && VF_Calories_TO.Text != "")
            {
                ListDataTable.Items.Clear();
                Select("Select * From " + ListTableName.Text + " Where Calories >= " + VF_Calories_From.Text +
                       " AND Calories <= " + VF_Calories_TO.Text);
            }
        }
        private void Button_RED_OR_YELLOW_Click(object sender, RoutedEventArgs e)
        {
            ListDataTable.Items.Clear();
            Select("Select Id, Name, Color From " + ListTableName.Text + " Where Color = 'Желтый' OR Color = 'Красный'");
        }

        #endregion
    }
}
