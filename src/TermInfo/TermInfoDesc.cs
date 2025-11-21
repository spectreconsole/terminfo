namespace TermInfo;

/// <summary>
/// Represents a parsed terminfo description.
/// </summary>
public sealed partial class TermInfoDesc
{
    private readonly string[] _names;
    private readonly bool?[] _booleans;
    private readonly int?[] _nums;
    private readonly string?[] _strings;

    /// <summary>
    /// Gets the names of the parsed terminfo description.
    /// </summary>
    public string[] Names => _names;

    /// <summary>
    /// Gets the extended capabilities.
    /// </summary>
    public ExtendedCapabilities Extended { get; }

    internal TermInfoDesc(
        string[] names, bool?[] booleans, int?[] nums,
        string?[] strings, ExtendedCapabilities? extended = null)
    {
        _names = names;
        _booleans = booleans;
        _nums = nums;
        _strings = strings;

        Extended = extended ?? new ExtendedCapabilities();
    }

    /// <summary>
    /// Gets a specific boolean terminfo capability value.
    /// </summary>
    /// <param name="value">The capability to get the value for.</param>
    /// <returns>The terminfo capability value.</returns>
    public bool? GetBoolean(TermInfoCaps.Boolean value)
    {
        var index = (int)value;
        if (index >= _booleans.Length)
        {
            return null;
        }

        return _booleans[index];
    }

    /// <summary>
    /// Gets a specific numeric terminfo capability value.
    /// </summary>
    /// <param name="value">The capability to get the value for.</param>
    /// <returns>The terminfo capability value.</returns>
    public int? GetNum(TermInfoCaps.Num value)
    {
        var index = (int)value;
        if (index >= _nums.Length)
        {
            return null;
        }

        var result = _nums[index];
        if (result == null || result == -1)
        {
            return null;
        }

        return result;
    }

    /// <summary>
    /// Gets a specific string terminfo capability value.
    /// </summary>
    /// <param name="value">The capability to get the value for.</param>
    /// <returns>The terminfo capability value.</returns>
    public string? GetString(TermInfoCaps.String value)
    {
        var index = (int)value;
        if (index >= _strings.Length)
        {
            return null;
        }

        var result = _strings[index];
        if (result == null)
        {
            return null;
        }

        return result;
    }
}