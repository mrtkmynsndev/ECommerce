namespace ECommerce.Core.Entities.Identity
{
    public class Adress
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}