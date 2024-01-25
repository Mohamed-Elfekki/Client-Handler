using System;
using System.Collections.Generic;

namespace ClientHandler.Entities
{
    public partial class Governorate
    {
        public Governorate()
        {
            Cities = new HashSet<City>();
        }

        public int Id { get; set; }
        public string GovernorateName { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; }
    }
}
