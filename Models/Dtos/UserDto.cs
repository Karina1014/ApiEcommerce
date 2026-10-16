using System;

namespace ApiEcommerce.Models.Dtos;

public class UserDto
{
    public int IdUser { get; set; }
    public string? Name { get; set; }     
    public string? UserName { get; set; } 
    public string? Password { get; set; }    
    public string? Roles { get; set; } 
}
