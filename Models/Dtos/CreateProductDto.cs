
using System;

namespace ApiEcommerce.Models.Dtos;

public class CreateProductDto
{
    public int IdProduct { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImgUrl { get; set; } = string.Empty;
    public int Stock { get; set; }
    public DateTime? UpdateDte {get;set;} = null;
    public int IdCategory { get; set; }
   
}
