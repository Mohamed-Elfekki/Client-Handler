using System;
using System.Collections.Generic;

namespace ClientHandler.Entities
{
    public partial class Client
    {
        public string Name { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public decimal Salary { get; set; }
        public int UserId { get; set; }
        public int? CityId { get; set; }
        public int? VillageId { get; set; }

        public virtual City? City { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Village? Village { get; set; }
    }
}
