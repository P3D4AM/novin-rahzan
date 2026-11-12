using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Management.Sdk.Sfc;

public class NamedKey<T> : SfcKey, IEquatable<NamedKey<T>> where T : SfcInstance
{
	private string keyName;

	public string Name => keyName;

	public sealed override Type InstanceType => typeof(T);

	protected virtual string UrnName => typeof(T).Name;

	public NamedKey()
	{
		keyName = string.Empty;
	}

	public NamedKey(NamedKey<T> other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		keyName = other.Name;
	}

	public NamedKey(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		keyName = name ?? string.Empty;
	}

	public NamedKey(IDictionary<string, object> fields)
	{
		keyName = (string)fields["Name"];
		if (keyName == null)
		{
			throw new ArgumentNullException("fields[Name]");
		}
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is NamedKey<T>))
		{
			return false;
		}
		return Equals((NamedKey<T>)obj);
	}

	public override bool Equals(SfcKey other)
	{
		if (other == null || !(other is NamedKey<T>))
		{
			return false;
		}
		return Equals((NamedKey<T>)other);
	}

	public bool Equals(NamedKey<T> other)
	{
		if ((object)this == other)
		{
			return true;
		}
		if ((object)other == null)
		{
			return false;
		}
		return string.CompareOrdinal(Name, other.Name) == 0;
	}

	public override int GetHashCode()
	{
		return Name.GetHashCode();
	}

	public override string ToString()
	{
		return Name;
	}

	public new static bool Equals(object leftOperand, object rightOperand)
	{
		if (leftOperand == null)
		{
			if (rightOperand != null)
			{
				return false;
			}
			return true;
		}
		return leftOperand.Equals(rightOperand);
	}

	public static bool operator ==(NamedKey<T> leftOperand, NamedKey<T> rightOperand)
	{
		if ((object)leftOperand == null)
		{
			if ((object)rightOperand != null)
			{
				return false;
			}
			return true;
		}
		return leftOperand.Equals(rightOperand);
	}

	public static bool operator !=(NamedKey<T> leftOperand, NamedKey<T> rightOperand)
	{
		if ((object)leftOperand == null)
		{
			if ((object)rightOperand != null)
			{
				return true;
			}
			return false;
		}
		return !leftOperand.Equals(rightOperand);
	}

	public override string GetUrnFragment()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}[@Name='{1}']", UrnName, SfcSecureString.EscapeSquote(Name));
	}
}
