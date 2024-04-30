namespace WebApplication2.Models
{
    public class Product
    {   
        public Product(string Name, decimal Price)
        {
            this.Name = Name;
            this.Price = Price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
