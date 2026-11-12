using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public struct MetadataObjectId : IEquatable<MetadataObjectId>
{
	private readonly IList<MetadataObjectSelector> selectors;

	public IList<MetadataObjectSelector> Selectors => selectors;

	public bool IsNull => selectors == null;

	public static MetadataObjectId Null => default(MetadataObjectId);

	public MetadataObjectId(IList<MetadataObjectSelector> selectors)
	{
		this.selectors = selectors;
	}

	public override bool Equals(object obj)
	{
		return Equals((MetadataObjectId)obj);
	}

	public bool Equals(MetadataObjectId other)
	{
		if (IsNull || other.IsNull)
		{
			if (IsNull)
			{
				return other.IsNull;
			}
			return false;
		}
		if (selectors.Count != other.selectors.Count)
		{
			return false;
		}
		for (int num = selectors.Count - 1; num >= 0; num--)
		{
			if (!selectors[num].Equals(other.selectors[num]))
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		if (IsNull)
		{
			return -1;
		}
		return selectors.Count switch
		{
			0 => 0, 
			1 => 1 ^ selectors[0].GetHashCode(), 
			_ => 2 ^ selectors[selectors.Count - 1].GetHashCode() ^ selectors[selectors.Count - 2].GetHashCode(), 
		};
	}

	public static bool operator ==(MetadataObjectId x, MetadataObjectId y)
	{
		return x.Equals(y);
	}

	public static bool operator !=(MetadataObjectId x, MetadataObjectId y)
	{
		return !x.Equals(y);
	}
}
