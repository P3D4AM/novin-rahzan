using System;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

internal class ExactStringPattern : IPattern<string>, IEquatable<ExactStringPattern>
{
	public string Reference { get; }

	public ExactStringPattern(string reference)
	{
		Reference = reference;
	}

	public bool IsMatch(string value)
	{
		return string.Equals(value, Reference, StringComparison.OrdinalIgnoreCase);
	}

	public bool Equals(ExactStringPattern other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return string.Equals(Reference, other.Reference, StringComparison.OrdinalIgnoreCase);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((ExactStringPattern)obj);
	}

	public override int GetHashCode()
	{
		return Reference?.GetHashCode() ?? 0;
	}

	public override string ToString()
	{
		return Reference;
	}
}
