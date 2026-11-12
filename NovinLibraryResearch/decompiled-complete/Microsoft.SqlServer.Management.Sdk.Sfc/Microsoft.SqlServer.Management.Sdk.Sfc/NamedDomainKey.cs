using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Management.Sdk.Sfc;

public class NamedDomainKey<T> : DomainRootKey, IEquatable<NamedDomainKey<T>> where T : ISfcDomain
{
	private string keyName;

	public string Name => keyName;

	public sealed override Type InstanceType => typeof(T);

	protected virtual string UrnName => typeof(T).Name;

	public NamedDomainKey()
		: this((ISfcDomain)null, string.Empty)
	{
	}

	public NamedDomainKey(ISfcDomain domain)
		: this(domain, domain.DomainInstanceName)
	{
	}

	public NamedDomainKey(ISfcDomain domain, string name)
		: base(domain)
	{
		keyName = name;
	}

	public NamedDomainKey(ISfcDomain domain, IDictionary<string, object> fields)
		: base(domain)
	{
		keyName = (string)fields["Name"];
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is NamedDomainKey<T>))
		{
			return false;
		}
		return Equals((NamedDomainKey<T>)obj);
	}

	public override bool Equals(SfcKey other)
	{
		if (other == null || !(other is NamedDomainKey<T>))
		{
			return false;
		}
		return Equals((NamedDomainKey<T>)other);
	}

	public bool Equals(NamedDomainKey<T> other)
	{
		if ((object)this == other)
		{
			return true;
		}
		if ((object)other == null)
		{
			return false;
		}
		if (base.Domain.Equals(other.Domain))
		{
			return string.CompareOrdinal(Name, other.Name) == 0;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.Domain.GetHashCode() ^ Name.GetHashCode();
	}

	public override string ToString()
	{
		return base.Domain.DomainName + "," + Name;
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
		return ((NamedDomainKey<T>)leftOperand).Equals(rightOperand);
	}

	public static bool operator ==(NamedDomainKey<T> leftOperand, NamedDomainKey<T> rightOperand)
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

	public static bool operator !=(NamedDomainKey<T> leftOperand, NamedDomainKey<T> rightOperand)
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
