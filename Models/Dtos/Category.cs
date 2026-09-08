using System.ComponentModel.DataAnnotations;

public class  Category
{
    [Key]
    public int IdCategorty {get;set;}
    [Required]
    public string Name {get; set;} = string.Empty;
    [Required]
    public DateTime CreationDate {get;set;}
}