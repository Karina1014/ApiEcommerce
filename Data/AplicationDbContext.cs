using Microsoft.EntityFrameworkCore; //La base de datos es
public class AplicationDbContext: DbContext //DbContext hereda de EntityFrameworkCore de la db, clase base 
{
    //creamos el constructor
                                //pones opciones del contexto
                                                 //tipo 
    public  AplicationDbContext (DbContextOptions<AplicationDbContext> options): base(options)
    {
        
    } 

    //DbSet sera una tabla en DB 
                //tabl
                            //Va por convencion y en plural es la migracios
    public DbSet <Category> Categories { get; set; }
}