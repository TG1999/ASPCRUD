using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ProductController : BaseController<Product>
    {
        public ProductController() : base("wwwroot/data/products.json") // Specify the file path
        {
            // Ensure the file path is correct
        }
    }
}
