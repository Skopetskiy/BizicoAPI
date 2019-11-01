using Database;
using Database.Models;
using System;

namespace Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Technology { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public decimal Price { get; set; }
        public string Brief { get; set; }
    }
}
