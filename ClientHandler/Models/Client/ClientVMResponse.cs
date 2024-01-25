namespace ClientHandler.Models.Client
{
    public class ClientVMResponse
    {
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Salary { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string? Village { get; set; }
    }
}
