namespace thomasgregcorewebapi.Models
{
    public partial class CustomerAddress
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Complement { get; set; }
        public string? ZipCode { get; set; }
    }
}
