using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using Dapper;

namespace DZ_08_ShareOnSaleDapper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly string connectionString = "Data Source=DANKOZZ1;Initial Catalog=DankOZetka;Integrated Security=True";
        private IDbConnection connection;
        public MainWindow()
        {
            InitializeComponent();

            connection = new SqlConnection(connectionString);
        }

        #region Part1_Tast3

        private void Button_AllUsers_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = connection.Query("Select * From Users as C Join Cities As Ct ON Ct.Id = C.CityId ").Select(u => new { u.Id, u.FirstName, u.LastName, u.Birthday, u.Sex, u.Email, City = u.Name }).ToList();
        }

        private void Button_AllEmailUser_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = connection.Query("Select Id, Email From Users").Select(u => new { u.Id, u.Email }).ToList();
        }

        private void Button_Categories_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = connection.Query("Select Id, Name From Categories").Select(c => new { c.Id, c.Name }).ToList();
        }

        private void Button_Shares_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = connection.Query("Select Id, Name From Products").Select(c => new { c.Id, c.Name }).ToList();
        }

        private void Button_AllCities_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = connection.Query("Select Id, Name From Cities").Select(c => new { c.Id, c.Name }).ToList();
        }

        private void Button_AllCountries_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = connection.Query("Select Id, Name From Countries").Select(c => new { c.Id, c.Name }).ToList();
        }
        #endregion


        #region Part1_Tast4
        private void Button_SearchUsersByCity_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxCity.Text == "")
                return;

            dataGrid.ItemsSource = connection.Query($"Select * From Users U, Cities Ct Where U.CityId = Ct.Id and Ct.Name = '{TextBoxCity.Text}'").Select(c => new { c.Id, c.FirstName, c.LastName, c.Birthday, c.Sex, c.Email, City = c.Name }).ToList();
        }

        private void Button_SearchUsersByCountry_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxCountry.Text == "")
                return;

            dataGrid.ItemsSource = connection.Query($"Select * From Users U, Cities Ct, Countries Cn Where U.CityId = Ct.Id and Ct.CountryId = Cn.Id and Cn.Name = '{TextBoxCountry.Text}'").Select(u => new { u.Id, u.FirstName, u.LastName, u.Birthday, u.Sex, u.Email, City = u.Name }).ToList();
        }

        private void Button_SearchSharesByShares_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxCountry.Text == "")
                return;

            string sql = $@"Select S.Id, S.Discount, S.StartDate, S.EndDate, P.Name As 'Product', Cn.Name As 'Country', C.Name As 'Category'
From Shares S 
JOIN Countries Cn ON S.CountryId = Cn.Id AND Cn.Name = '{TextBoxCountry.Text}' 
JOIN Products P ON S.ProductId = P.Id 
JOIN Categories C ON S.CountryId = C.Id";

            dataGrid.ItemsSource = connection.Query(sql).Select(s => new { s.Id, s.Discount, s.StartDate, s.EndDate, ProductId = s.Product, CountryId = s.Country, CategoryId = s.Category }).ToList();
        }
        #endregion

    }
}
