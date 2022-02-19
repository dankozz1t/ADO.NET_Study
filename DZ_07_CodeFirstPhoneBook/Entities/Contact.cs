using System;

namespace DZ_07_CodeFirstPhoneBook.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public short Priority { get; set; }
        public bool IsBlocking { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
