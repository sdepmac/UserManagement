namespace UsersApi.Wrappers;

/// <inheritdoc/>
public class GuidGenerator : IGuidGenerator
{
    /// <inheritdoc/>
    public Guid NewGuid()
    {
        return Guid.NewGuid();
    }
}
