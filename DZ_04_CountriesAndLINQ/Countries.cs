using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace DZ_04_CountriesAndLINQ
{
    public class Countries : DataContext
    {
        const string connectionString = "Data Source=DANKOZZ1;Initial Catalog=Countries;Integrated Security=True";
        public Countries() : base(connectionString) { }
        public Countries(string connectionStr) : base(connectionStr) { }
        public Table<Country> Country => this.GetTable<Country>();
    }

    [Table(Name = "Countries")]
    public class Country
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true, CanBeNull = false)]
        public int Id { get; set; }


        [Column(Name = "Name")]
        public string Name { get; set; }


        [Column(Name = "Capital")]
        public string Capital { get; set; }


        [Column(Name = "Population")]
        public int Population { get; set; }


        [Column(Name = "Area")]
        public int Area { get; set; }


        [Column(Name = "Continent")]
        public string Continent { get; set; }
    }
}