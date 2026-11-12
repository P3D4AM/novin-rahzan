using System;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

public class MultipleItemChange<TTarget, TValue> : IItemChange<TTarget>, IEquatable<MultipleItemChange<TTarget, TValue>>
{
	public IMultipleChangeAccessor<TTarget, TValue> Accessor { get; }

	public InPlaceTransform<TValue> Transform { get; }

	public MultipleItemChange(IMultipleChangeAccessor<TTarget, TValue> accessor, InPlaceTransform<TValue> transform)
	{
		Accessor = accessor ?? throw new ArgumentNullException("accessor");
		Transform = transform ?? throw new ArgumentNullException("transform");
	}

	public void ApplyTo(TTarget target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		foreach (TValue value in Accessor.GetValues(target))
		{
			Transform.ApplyTo(value);
		}
	}

	public bool Equals(MultipleItemChange<TTarget, TValue> other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (object.Equals(Accessor, other.Accessor))
		{
			return object.Equals(Transform, other.Transform);
		}
		return false;
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
		return Equals((MultipleItemChange<TTarget, TValue>)obj);
	}

	public override int GetHashCode()
	{
		return (((Accessor != null) ? Accessor.GetHashCode() : 0) * 397) ^ ((Transform != null) ? Transform.GetHashCode() : 0);
	}
}
