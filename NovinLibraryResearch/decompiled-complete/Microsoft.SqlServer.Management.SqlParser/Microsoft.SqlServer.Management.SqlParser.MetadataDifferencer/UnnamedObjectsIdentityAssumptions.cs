using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class UnnamedObjectsIdentityAssumptions
{
	private struct UnorderedPair<T>
	{
		private readonly T first;

		private readonly T second;

		public T First => first;

		public T Second => second;

		public UnorderedPair(T first, T second)
		{
			this.first = first;
			this.second = second;
		}

		public bool Equals(UnorderedPair<T> other)
		{
			if (!first.Equals(other.first) || !second.Equals(other.second))
			{
				if (first.Equals(other.second))
				{
					return second.Equals(other.first);
				}
				return false;
			}
			return true;
		}

		public override bool Equals(object other)
		{
			return Equals((UnorderedPair<T>)other);
		}

		public override int GetHashCode()
		{
			return first.GetHashCode() ^ second.GetHashCode();
		}
	}

	private readonly HashSet<IMetadataObject> allObjects;

	private readonly HashSet<UnorderedPair<IMetadataObject>> equalObjects;

	public UnnamedObjectsIdentityAssumptions()
	{
		allObjects = new HashSet<IMetadataObject>();
		equalObjects = new HashSet<UnorderedPair<IMetadataObject>>();
	}

	public void AddEqualityAssumption(IMetadataObject left, IMetadataObject right)
	{
		UnorderedPair<IMetadataObject> item = new UnorderedPair<IMetadataObject>(left, right);
		equalObjects.Add(item);
		allObjects.Add(left);
		allObjects.Add(right);
	}

	public void RemoveEqualityAssumption(IMetadataObject left, IMetadataObject right)
	{
		UnorderedPair<IMetadataObject> item = new UnorderedPair<IMetadataObject>(left, right);
		equalObjects.Remove(item);
		allObjects.Remove(left);
		allObjects.Remove(right);
	}

	public bool AreAssumedEqual(IMetadataObject left, IMetadataObject right)
	{
		UnorderedPair<IMetadataObject> item = new UnorderedPair<IMetadataObject>(left, right);
		return equalObjects.Contains(item);
	}

	public bool CanBeAssumedEqual(IMetadataObject left, IMetadataObject right)
	{
		new UnorderedPair<IMetadataObject>(left, right);
		if (!AreAssumedEqual(left, right))
		{
			if (!allObjects.Contains(left))
			{
				return !allObjects.Contains(right);
			}
			return false;
		}
		return true;
	}
}
