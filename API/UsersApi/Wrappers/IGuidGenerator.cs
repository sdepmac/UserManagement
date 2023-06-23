namespace UsersApi.Wrappers;

/// <summary>
/// Wrapper to generate guids.
/// </summary>
public interface IGuidGenerator
{
    /// <summary>
    /// Create new guid.
    /// </summary>
    /// <returns>guid.</returns>
    Guid NewGuid();
}