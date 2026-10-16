using System;

namespace ApiEcommerce.Models.Dtos;

public class UserLoginRegisterDto
{
    public UserRegisterDto? User {get;set;}
    public string? Token {get;set;}
    public string? Message {get;set;}
}
