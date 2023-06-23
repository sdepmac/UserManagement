namespace UsersApi.Models;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// User entities.
/// </summary>
[Table("users")]
public class User
{
    /// <summary>
    /// Gets or sets user Id.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets user first name.
    /// </summary>
    [Column("first_name")]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets user Last Name.
    /// </summary>
    [Column("last_name")]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets user email.
    /// </summary>
    [Column("email")]
    public string Email { get; set; }
}