using System;

namespace Database.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public string Technology { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public decimal Price { get; set; }
        public string Brief { get; set; }
        public User User { get; set; }
    }
}