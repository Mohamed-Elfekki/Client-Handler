using System.ComponentModel.DataAnnotations;

namespace ClientHandler.Models.Country
{
    public class VillageVMResponse
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; } = null!;
    }
}
