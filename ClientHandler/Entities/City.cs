using System;
using System.Collections.Generic;

namespace ClientHandler.Entities
{
    public partial class City
    {
        public City()
        {
            Clients = new HashSet<Client>();
            Villages = new HashSet<Village>();
        }

        public int Id { get; set; }
        public string CityName { get; set; } = null!;
        public int GovernorateId { get; set; }

        public virtual Governorate Governorate { get; set; } = null!;
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Village> Villages { get; set; }
    }
}
