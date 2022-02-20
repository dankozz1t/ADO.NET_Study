using System.Collections.Generic;

namespace DZ_07_CodeFirstPhoneBook.Helpers
{
    public class Group
    {
        public Group()
        {
            this.Contacts = new HashSet<Contact>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}