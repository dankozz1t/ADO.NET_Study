using System.Data.Entity;
using DZ_07_CodeFirstPhoneBook.Entities;

namespace DZ_07_CodeFirstPhoneBook.DbContext
{
    public class PhoneBookDb : System.Data.Entity.DbContext
    {
        public PhoneBookDb(string connectionString) : base(connectionString) { }
        public PhoneBookDb() : base("PhoneBookDbConnectionString") { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }
    }

}