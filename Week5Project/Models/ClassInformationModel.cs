using System.ComponentModel.DataAnnotations;


    public class ClassInformationModel
{
    public int Id { get; set; }  // <-- SET eklenmeli

    [Required]
    public string ClassName { get; set; }

    [Range(1, 100)]
    public int StudentCount { get; set; }

    [Required]
    public string Description { get; set; }


}


