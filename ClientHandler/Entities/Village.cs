using System;
using System.Collections.Generic;

namespace ClientHandler.Entities
{
    public partial class Village
    {
        public Village()
        {
            Clients = new HashSet<Client>();
        }

        public int Id { get; set; }
        public string VillageName { get; set; } = null!;
        public int CityId { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual ICollection<Client> Clients { get; set; }
    }
}
