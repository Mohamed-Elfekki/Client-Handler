using System;
using System.Collections.Generic;

namespace ClientHandler.Entities
{
    public partial class User
    {
        public User()
        {
            Clients = new HashSet<Client>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Client> Clients { get; set; }
    }
}
