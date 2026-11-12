using System;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal class UrnTypeKey : IComparable
{
	private string uniqueUrnType;

	private ObjectOrder createOrder = ObjectOrder.uninitialized;

	internal ObjectOrder CreateOrder
	{
		get
		{
			if (createOrder.Equals(ObjectOrder.uninitialized))
			{
				createOrder = SetCreateOrder();
			}
			return createOrder;
		}
	}

	private ObjectOrder SetCreateOrder()
	{
		if (!Enum.TryParse<ObjectOrder>(uniqueUrnType, ignoreCase: true, out var result))
		{
			return ObjectOrder.@default;
		}
		return result;
	}

	public UrnTypeKey(Urn urn)
	{
		uniqueUrnType = GetUniqueUrnType(urn);
	}

	private string GetUniqueUrnType(Urn urn)
	{
		try
		{
			string type = urn.Type;
			string parentUrnType = ((urn.Parent != null) ? urn.Parent.Type : string.Empty);
			return GetUniqueSmoType(type, parentUrnType);
		}
		catch (Exception innerException)
		{
			throw new SmoException("Invalid Urn", innerException);
		}
	}

	public UrnTypeKey(string urnTypeKey)
	{
		uniqueUrnType = urnTypeKey.ToLowerInvariant();
	}

	public UrnTypeKey(string urnType, string parentUrnType)
	{
		uniqueUrnType = GetUniqueSmoType(urnType, parentUrnType);
	}

	private string GetUniqueSmoType(string urnType, string parentUrnType)
	{
		switch (urnType.ToLowerInvariant())
		{
		case "default":
		case "ddltrigger":
		case "role":
			return urnType.ToLowerInvariant() + parentUrnType.ToLowerInvariant();
		default:
			return urnType.ToLowerInvariant();
		}
	}

	public override int GetHashCode()
	{
		return uniqueUrnType.GetHashCode();
	}

	public override string ToString()
	{
		return uniqueUrnType;
	}

	public override bool Equals(object obj)
	{
		if (obj is UrnTypeKey urnTypeKey)
		{
			return uniqueUrnType.Equals(urnTypeKey.uniqueUrnType, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public int CompareTo(object obj)
	{
		if (obj is UrnTypeKey urnTypeKey)
		{
			if (CreateOrder == urnTypeKey.CreateOrder)
			{
				return string.Compare(uniqueUrnType, urnTypeKey.uniqueUrnType, StringComparison.InvariantCultureIgnoreCase);
			}
			return CreateOrder.CompareTo(urnTypeKey.CreateOrder);
		}
		throw new ArgumentException("Object is not a UrnTypeKey.");
	}
}
