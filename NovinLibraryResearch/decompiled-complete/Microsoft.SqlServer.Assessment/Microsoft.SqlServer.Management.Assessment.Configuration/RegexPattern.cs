using System.Text.RegularExpressions;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

internal class RegexPattern : Regex, IPattern<string>
{
	public RegexPattern(string pattern)
		: base(pattern)
	{
	}

	public RegexPattern(string pattern, RegexOptions options)
		: base(pattern, options)
	{
	}

	public RegexPattern(Regex other)
		: this(other.ToString(), other.Options)
	{
	}

	public override bool Equals(object obj)
	{
		if (obj is RegexPattern other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return pattern.GetHashCode();
	}

	protected bool Equals(RegexPattern other)
	{
		return string.CompareOrdinal(pattern, other.pattern) == 0;
	}

	bool IPattern<string>.IsMatch(string value)
	{
		return IsMatch(value);
	}
}
