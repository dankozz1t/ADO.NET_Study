using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DZ_06_ModelFirstGames
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GamesDBContainer db;
        private Developer developerNow;
        public MainWindow()
        {
            InitializeComponent();
            db = new GamesDBContainer();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridDevelopers.ItemsSource = db.Developers.ToList();
        }

        #region Developer
        private void Button_CleanTextDev_Click(object sender, RoutedEventArgs e)
        {
            FirstName.Text = "";
            LastName.Text = "";
            Country.Text = "";
        }

        private void DataGridDevelopers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            developerNow = dataGridDevelopers.SelectedItem as Developer;
            if (developerNow != null)
            {
                dataGridGames.ItemsSource = developerNow.Game.ToList();

                FirstName.Text = developerNow.FirstName;
                LastName.Text = developerNow.LastName;
                Country.Text = developerNow.Country;

                Button_CleanTextGames_Click(null, null);
            }
        }

        private void Button_AddDev_Click(object sender, RoutedEventArgs e)
        {
            Developer developer = new Developer()
            {
                FirstName = FirstName.Text,
                LastName = LastName.Text,
                Country = Country.Text
            };

            db.Developers.Add(developer);
            db.SaveChanges();

            dataGridDevelopers.ItemsSource = db.Developers.ToList();
        }

        private void Button_UpdateDev_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridDevelopers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            if (developerNow != null)
            {
                developerNow.FirstName = FirstName.Text;
                developerNow.LastName = LastName.Text;
                developerNow.Country = Country.Text;

                db.SaveChanges();
                dataGridDevelopers.ItemsSource = db.Developers.ToList();
            }
        }

        private void Button_DeleteDev_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridDevelopers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            db.Developers.Remove(developerNow);
            db.SaveChanges();
            dataGridDevelopers.ItemsSource = db.Developers.ToList();

            Button_CleanTextGames_Click(null, null);
        }

        #endregion


        #region Game 
        private void Button_CleanTextGames_Click(object sender, RoutedEventArgs e)
        {
            Year.Text = "";
            Distributor.Text = "";
            Platform.Text = "";
            Name.Text = "";
        }
        private void DataGridGamesSelection(object sender, SelectionChangedEventArgs e)
        {
            Game game = dataGridGames.SelectedItem as Game;
            if (game != null)
            {
                Year.Text = game.Year.ToString();
                Distributor.Text = game.Distributor;
                Platform.Text = game.Platform;
                Name.Text = game.Name;
            }
        }

        private void Button_AddGame_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game()
            {
                Developer = developerNow,
                Distributor = Distributor.Text,
                Name = Name.Text,
                Platform = Platform.Text,
                Year = short.Parse(Year.Text)
            };

            db.Games.Add(game);
            db.SaveChanges();
            dataGridGames.ItemsSource = developerNow.Game.ToList();
        }
        private void Button_UpdateGame_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridGames.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            Game game = dataGridGames.SelectedItem as Game;
            if (game != null)
            {
                game.Year = short.Parse(Year.Text);
                game.Distributor = Distributor.Text;
                game.Platform = Platform.Text;
                game.Name = Name.Text;

                db.SaveChanges();
                dataGridGames.ItemsSource = developerNow.Game.ToList();
            }
        }

        private void Button_DeleteGame_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridGames.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите строку");
                return;
            }

            Game game = dataGridGames.SelectedItem as Game;
            db.Games.Remove(game);
            db.SaveChanges();
            dataGridGames.ItemsSource = developerNow.Game.ToList();

            Button_CleanTextGames_Click(null, null);
        }

        #endregion
    }
}
