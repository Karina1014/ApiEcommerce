using System;
using ApiEcommerce.Models;

namespace ApiEcommerce.Repository;

public class ProductRepository : IProductRepository
{
    //Creamos una instancia del contexto
    private readonly AplicationDbContext _db;
    //Constructor
    public ProductRepository(AplicationDbContext db)
    {
        _db = db;
    }
    public bool BuyProduct(string name, int quantity)
    {
       if (string.IsNullOrWhiteSpace(name) ||  quantity ==0)
       {
         return false;
       }
       var product =_db.Products.FirstOrDefault(p=>p.Name.ToLower().Trim()== name.ToLower().Trim());
       if (product == null || product.Stock < quantity)
       {
         return false;
       }
       product.Stock -= quantity;
       _db.Products.Update(product);
       return Save();
    }

    public bool CreateProduct(Product product)
    {
        if (product == null)
        {
            return false;
        }
        product.CreateDate = DateTime.Now;
        product.UpdateDte = DateTime.Now;
        _db.Products.Add(product);
        return Save();
    }

    public bool DeleteProduct(Product product)
    {
        if (product == null)
        {
            return false;
        }
        _db.Products.Remove(product);
        return Save();
    }

    public Product? GetProduct(int IdProduct)
    {
        if (IdProduct <=0)
        {
            return null;
        }
        return _db.Products.FirstOrDefault(p => p.IdProduct == IdProduct);
    }

    public ICollection<Product> GetProducts()
    {
        return _db.Products.OrderBy(p=>p.Name).ToList();
    }

    public ICollection<Product> GetProductsForCategory(int IdCategory)
    {
        if (IdCategory <= 0)
        {
            return new List<Product>();
        }
        return _db.Products.Where(p=>p.IdCategory == IdCategory).OrderBy(p=>p.Name).ToList();
    }

    public bool ProductExists(int IdProduct)
    {
        if (IdProduct <= 0)
        {
            return false;
        }
        return _db.Products.Any(p=>p.IdProduct == IdProduct);
    }

    public bool ProductExists(string name)
    {
       if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        return _db.Products.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
    }
        
    

    public bool Save()
    {
        return _db.SaveChanges()>= 0;
    }

    public ICollection<Product> SearchProducts(string name)
    {
        IQueryable<Product>  query = _db.Products;
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        return query.OrderBy(p => p.Name).ToList();
    }

    public bool UpdateProduct(Product product)
    {
        if (product == null)
        {
            return false;
        }
        product.UpdateDte = DateTime.Now;
        _db.Products.Add(product);
        return Save();
    }
}
