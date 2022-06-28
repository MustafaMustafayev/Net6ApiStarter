using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Entity.Entities;

public class Category : AuditableEntity
{
    [Key] public int CategoryId { get; set; }

    [Required] public string Name { get; set; }

    public virtual Category ParentCategory { get; set; }

    [ForeignKey("ParentCategory")] public int? ParentCategoryId { get; set; }
}