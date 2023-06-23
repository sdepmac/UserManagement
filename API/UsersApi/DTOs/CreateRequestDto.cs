namespace UsersApi.DTOs;

using System.ComponentModel.DataAnnotations;
using System.Data;

public record CreateRequestDto
{
    /// <summary>
    /// Gets or sets first name.
    /// </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets last name.
    /// </summary>
    [Required]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
