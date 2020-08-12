namespace ECommerce.Core.Entities.Orders
{
    public class Adress
    {
        public Adress()
        {
            // Ef required
        }

        public Adress(string name, string lastName, string street, string city, string state, string zip)
        {
            Name = name;
            LastName = lastName;
            Street = street;
            City = city;
            State = state;
            Zip = zip;
        }

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}