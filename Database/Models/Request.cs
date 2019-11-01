using System;

namespace Database.Models
{
    public class Request
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public string Technology { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public int Experience { get; set; }
        public User User { get; set; }
    }
}