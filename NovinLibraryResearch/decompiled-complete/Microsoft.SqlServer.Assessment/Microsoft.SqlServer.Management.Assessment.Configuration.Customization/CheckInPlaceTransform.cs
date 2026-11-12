using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class CheckInPlaceTransform : InPlaceTransform<Check>, IEquatable<CheckInPlaceTransform>
{
	public List<IItemChange<Check>> Children { get; } = new List<IItemChange<Check>>();


	public CheckInPlaceTransform(IEnumerable<IItemChange<Check>> changes)
	{
		if (changes == null)
		{
			throw new ArgumentNullException("changes");
		}
		Children.AddRange(changes);
	}

	public override void ApplyTo(Check check)
	{
		if (check == null)
		{
			throw new ArgumentNullException("check");
		}
		foreach (IItemChange<Check> child in Children)
		{
			child.ApplyTo(check);
		}
	}

	public bool Equals(CheckInPlaceTransform other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (Children.Count != other.Children.Count)
		{
			return false;
		}
		foreach (IItemChange<Check> child in Children)
		{
			bool flag = true;
			foreach (IItemChange<Check> child2 in other.Children)
			{
				if (child2.Equals(child))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return false;
			}
		}
		return true;
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
		return Equals((CheckInPlaceTransform)obj);
	}

	public override int GetHashCode()
	{
		if (Children == null)
		{
			return 0;
		}
		return Children.GetHashCode();
	}
}
