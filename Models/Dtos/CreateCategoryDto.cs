using System;

namespace ApiEcommerce.Models.Dtos;

public class CreateCategoryDto
{
    public int IdCategory {get;set;}
    public string Name {get; set;} = string.Empty;
}
