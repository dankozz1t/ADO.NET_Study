using System.Linq;
using System.Windows;

namespace DZ_04_CountriesAndLINQ
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Countries countries;

        public MainWindow()
        {
            InitializeComponent();
            countries = new Countries();
        }

        private void Button_ALL_INFO_Click(object sender, RoutedEventArgs e)
        {
            var query = from s in countries.Country select s;
            dataGrid.ItemsSource = query;
        }

        private void Button_Name_Countries_Click(object sender, RoutedEventArgs e)
        {
            var query = from country in countries.Country select new { CountryName = country.Name };

            dataGrid.ItemsSource = query;
        }

        private void Button_Capital_Countries_Click(object sender, RoutedEventArgs e)
        {
            var query = from country in countries.Country select new { country.Capital };

            dataGrid.ItemsSource = query;
        }

        private void Button_EU_Countries_Click(object sender, RoutedEventArgs e)
        {
            var query = from country in countries.Country where (country.Continent == "EU") select new { country.Name };

            dataGrid.ItemsSource = query;
        }

        private void Button_AreaMore_Countries_Click(object sender, RoutedEventArgs e)
        {
            if (AreaC.Text == "") return;

            var query = from country in countries.Country where (country.Area >= int.Parse(AreaC.Text)) select new { country.Name, country.Area };

            dataGrid.ItemsSource = query;
        }

        private void Button_Letter_AandU_Click(object sender, RoutedEventArgs e)
        {
            var query = from country in countries.Country where (country.Name.Contains("u") && country.Name.Contains("a")) select new { country.Name};

            dataGrid.ItemsSource = query;

        }

        private void Button_StartLetter_A_Click(object sender, RoutedEventArgs e)
        {
            var query = from country in countries.Country where (country.Name.StartsWith("a")) select new { country.Name };

            dataGrid.ItemsSource = query;
        }

        private void Button_Reange_Area_Click(object sender, RoutedEventArgs e)
        {
            if (AreaFrom.Text == "" || AreaTo.Text == "") return;

            var query = from country in countries.Country where (country.Area >= int.Parse(AreaFrom.Text) && country.Area <= int.Parse(AreaTo.Text)) select new { country.Name, country.Area };

            dataGrid.ItemsSource = query;
        }

        private void Button_Population_Countries_Click(object sender, RoutedEventArgs e)
        {
            if (Population.Text == "") return;

            var query = from country in countries.Country where (country.Population >= int.Parse(Population.Text)) select new { country.Name, country.Population };

            dataGrid.ItemsSource = query;
        }

        private void Button_Top5_Area_Click(object sender, RoutedEventArgs e)
        {
            var query = (from country in countries.Country orderby country.Area descending  select new { country.Name, country.Area }).Take(5);

            dataGrid.ItemsSource = query;
        }

        private void Button_Top5_Population_Click(object sender, RoutedEventArgs e)
        {
            var query = (from country in countries.Country orderby country.Population descending select country).Take(5);

            dataGrid.ItemsSource = query;
        }

        private void Button_Top1_Area_Click(object sender, RoutedEventArgs e)
        {
            var query = (from country in countries.Country orderby country.Area descending select new { country.Name, country.Area }).Take(1);

            dataGrid.ItemsSource = query;
        }
    }
}
