using System.ComponentModel.DataAnnotations;

namespace DAL.Repository.DTO;

public class CreateProductQuestionAnswer
{
    [Required]
    public string Answer { get; set; }
    [Required]
    public string AuthorName  { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}