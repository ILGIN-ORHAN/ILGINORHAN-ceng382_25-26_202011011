using System.ComponentModel.DataAnnotations;

namespace Week5Project.Models
{
    public class ClassInformationModel
{
    public int Id { get; set; }  // <-- SET eklenmeli

    [Required]
    public string ClassName { get; set; }

    [Range(1, 100)]
    public int StudentCount { get; set; }

    [Required]
    public string Description { get; set; }

    public int HiddenId { get; set; } // Used for actions but not displayed
}

}
