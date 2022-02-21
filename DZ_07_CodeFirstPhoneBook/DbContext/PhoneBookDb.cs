using System;
using System.Data.Entity;
using System.IO;
using DZ_07_CodeFirstPhoneBook.Helpers;

namespace DZ_07_CodeFirstPhoneBook.DbContext
{
    public class PhoneBookDb : System.Data.Entity.DbContext
    {
        public PhoneBookDb(string connectionString) : base(connectionString) { }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}