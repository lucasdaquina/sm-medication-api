using Azure.Core;

namespace SM.Medication.Api.Extensions;

public static class StringExtensions
{
    public static void GuardString(this string? value, string nameOf)
    {
        ArgumentNullException.ThrowIfNull(value, nameOf);

        if(string.IsNullOrEmpty(value))
            throw new Exception($"{nameOf} cannot be null or empty");

        if(string.IsNullOrWhiteSpace(value))
            throw new Exception($"{nameOf} cannot be empty or whitespace");
    }
}
