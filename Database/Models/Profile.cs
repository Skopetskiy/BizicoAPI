using System;

namespace Database.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Experience { get; set; }
        public string Summary { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
