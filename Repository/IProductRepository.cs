using System;
using ApiEcommerce.Models;

namespace ApiEcommerce.Repository;

public interface IProductRepository
{
    ICollection<Product> GetProducts();
    ICollection<Product> GetProductsForCategory(int IdCategory);
    ICollection<Product> SearchProduct(string name);
    Product? GetProduct(int IdProduct);
    bool BuyProduct(string name, int quantity);
    bool ProductExists(int IdProduct);
    bool ProductExists(string name);
    bool CreateProduct(Product Product);
    bool UpdateProduct (Product Product);
    bool DeleteProduct (Product Product);
    bool Save();
}
