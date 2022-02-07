using System.Linq;
using System.Windows;

namespace DZ_05_CountriesEntityFramework
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CountriesEntities dataBase;
        public MainWindow()
        {
            InitializeComponent();

            dataBase = new CountriesEntities();

            dataGrid.ItemsSource = dataBase.Countries.ToList();
        }

        private void Button_ADD_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(Capital.Text) || 
                string.IsNullOrWhiteSpace(Population.Text) || string.IsNullOrWhiteSpace(Area.Text) ||
                string.IsNullOrWhiteSpace(Continent.Text))
            {
                MessageBox.Show("Заполните ввод");
                return;
            }

            dataGrid.ItemsSource = null;
            Country country = new Country()
            {

                Area = int.Parse(Area.Text),
                Capital = Capital.Text,
                Continent = Continent.Text,
                Name = Name.Text,
                Population = int.Parse(Population.Text)
            };

            dataBase.Countries.Add(country);
            dataBase.SaveChanges();
            dataGrid.ItemsSource = dataBase.Countries.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Country country = dataGrid.SelectedCells[0].Item as Country;

            if (country != null)
            {
                Name.Text = country.Name;
                Capital.Text = country.Capital;
                Population.Text = country.Population.ToString();
                Area.Text = country.Area.ToString();
                Continent.Text = country.Continent;
            }
        }

        private void Button_UPDATE_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            Country country = dataGrid.SelectedCells[0].Item as Country;
            if (country != null)
            {
                country.Area = int.Parse(Area.Text);
                country.Capital = Capital.Text;
                country.Continent = Continent.Text;
                country.Name = Name.Text;
                country.Population = int.Parse(Population.Text);

                dataBase.SaveChanges();
                dataGrid.ItemsSource = dataBase.Countries.ToList();
            }
        }

        private void Button_DELETE_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Удалить запись?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            Country country = dataGrid.SelectedCells[0].Item as Country;
            if (country != null)
            {
                dataBase.Countries.Remove(country);

                dataBase.SaveChanges();
                dataGrid.ItemsSource = dataBase.Countries.ToList();
            }

            Button_CleanText_Click(null, null);
        }

        private void Button_CleanText_Click(object sender, RoutedEventArgs e)
        {
            Name.Text = "";
            Capital.Text = "";
            Population.Text = "";
            Area.Text = "";
            Continent.Text = "";
        }
    }
}
