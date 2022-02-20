using System;
using System.Data.Entity;
using System.IO;
using DZ_07_CodeFirstPhoneBook.Helpers;

namespace DZ_07_CodeFirstPhoneBook.DbContext
{
    public class PhoneBookDb : System.Data.Entity.DbContext
    {
        public PhoneBookDb(string connectionString) : base(connectionString) { }
        public static string MDF_Directory
        {
            get
            {
                var directoryPath = AppDomain.CurrentDomain.BaseDirectory;
                return Path.GetFullPath(Path.Combine(directoryPath, "..//PhoneBook.mdf"));
            }
        }
        public PhoneBookDb() : base($@"Data Source=(LocalDB)\MSSQLLocalDB; " +
                                    "AttachDbFilename=" + MDF_Directory +
                                    " Integrated Security=True; Connect Timeout=30;") { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }
    }

}