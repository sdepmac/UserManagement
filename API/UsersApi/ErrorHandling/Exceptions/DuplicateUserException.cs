namespace UsersApi.ErrorHandling.Exceptions;

using System.Globalization;

/// <summary>
/// Duplicate user exception.
/// </summary>
public class DuplicateUserException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateUserException"/> class.
    /// </summary>
    public DuplicateUserException()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateUserException"/> class.
    /// </summary>
    /// <param name="message">error message.</param>
    public DuplicateUserException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateUserException"/> class.
    /// </summary>
    /// <param name="message">error message.</param>
    /// <param name="args">error arguments.</param>
    public DuplicateUserException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
