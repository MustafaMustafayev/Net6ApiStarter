namespace Project.DTO.DTOs.AuthorDTOs;

public record AuthorToListDTO
{
    public int AuthorId { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }
}