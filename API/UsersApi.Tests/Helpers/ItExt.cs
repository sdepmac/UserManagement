namespace UsersApi.Tests.Helpers;

using FluentAssertions;
using Moq;

public static class ItExt
{
    /// <summary>
    /// Check that two object properties are equivalent.
    /// Example usage: .Verify(x => x.Update(ItExt.IsDeep(expectedObject)), Times.Once);.
    /// </summary>
    /// <typeparam name="T">Type of the expected object.</typeparam>
    /// <param name="expected">Expected object.</param>
    /// <returns>Custom matcher.</returns>
    public static T IsDeep<T>(T expected)
    {
        bool Validate(T actual)
        {
            actual.Should().BeEquivalentTo(expected);
            return true;
        }

        return Match.Create<T>(Validate);
    }
}

