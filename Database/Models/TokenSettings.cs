using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class TokenSettings
    {
        public string JWT_Secret { get; set; }
        public string Client_URL { get; set; }
    }
}
