using ApiEcommerce.Models;
using Microsoft.EntityFrameworkCore; 
public class AplicationDbContext: DbContext {
    public  AplicationDbContext (DbContextOptions<AplicationDbContext> options): base(options)
    {
        
    } 

    //DbSet sera una tabla en DB 
                //tabl
                            //Va por convencion y en plural es la migracios
    public DbSet <Category> Categories { get; set; }
    public DbSet <Product> Products {get; set;}
    public DbSet <User> Users {get; set;}
}