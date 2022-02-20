using System;
using System.Data.Entity;
using System.IO;
using DZ_07_CodeFirstPhoneBook.Helpers;

namespace DZ_07_CodeFirstPhoneBook.DbContext
{
    public class PhoneBookDb : System.Data.Entity.DbContext
    {
        public PhoneBookDb(string connectionString) : base(connectionString) { }
   
        public PhoneBookDb() : base($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Environment.CurrentDirectory}\\PhoneBook.mdf;Integrated Security=True; Connect Timeout=30;") {}

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }
    }

}