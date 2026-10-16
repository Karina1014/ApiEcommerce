using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models;

public class User
{   
    [Key]
    public int IdUser { get; set; }
    [Required]
    public string? Name { get; set; }     
    public string? UserName { get; set; } 
    public string? Password { get; set; }    
    public string? Roles { get; set; } 
}
