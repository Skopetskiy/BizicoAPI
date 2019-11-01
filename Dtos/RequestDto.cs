using Database;
using Database.Models;
using System;

namespace Dtos
{
    public class RequestDto
    {
        public Guid Id { get; set; }
        public string Technology { get; set; }
        public int Experience { get; set; }
        public SkillLevel SkillLevel { get; set; }
    }
}
