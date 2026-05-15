using System.ComponentModel.DataAnnotations;

namespace SimpleAuth.Model;

public class Credential
{
    [Required]
    [Display(Name = "User Name")]
    public string UserName { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}