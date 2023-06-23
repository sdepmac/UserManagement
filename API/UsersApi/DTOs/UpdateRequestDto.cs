namespace UsersApi.DTOs;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Update request dto.
/// </summary>
public record UpdateRequestDto
{
    /// <summary>
    /// Gets or sets first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }
}
