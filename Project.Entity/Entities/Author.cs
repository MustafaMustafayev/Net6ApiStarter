using System.ComponentModel.DataAnnotations;

namespace Project.Entity.Entities;

public class Author : AuditableEntity
{
    [Key] public int AuthorId { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Surname { get; set; }

    public virtual List<Book> Books { get; set; }
}