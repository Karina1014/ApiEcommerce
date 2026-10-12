using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEcommerce.Models;

public class Product
{
    [Key]
    public int IdProduct { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Range(0,double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public string ImgUrl { get; set; } = string.Empty;
    [Required]
    public string SKU { get; set; } = string.Empty;//PROD-001-BLK-M

    [Range(0,int.MaxValue)]
    public int Stock { get; set; }

    public DateTime CreateDate {get;set;} = DateTime.Now;
    public DateTime? UpdateDte {get;set;} = null;
    
    //Relación con el modelo Category
    public int IdCategory { get; set; }
    [ForeignKey("IdCategory")]

    public required Category category { get; set; }


}
