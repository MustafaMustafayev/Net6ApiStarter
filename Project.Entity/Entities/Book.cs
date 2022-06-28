using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Entity.Entities;

public class Book : AuditableEntity
{
    [Key] public int BookId { get; set; }

    [Required] public string Name { get; set; }

    [Required] [Column(TypeName = "Date")] public DateTime PublishedAt { get; set; }

    [Required] public string ISBN { get; set; }

    public string CoverImage { get; set; } // value will come from response of file uploading process

    public virtual Category Category { get; set; }

    [ForeignKey("Category")] public int CategoryId { get; set; }

    public virtual List<Author> Authors { get; set; }
}