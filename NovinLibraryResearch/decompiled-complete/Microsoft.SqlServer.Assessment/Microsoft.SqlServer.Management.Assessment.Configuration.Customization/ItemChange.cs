using System;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class ItemChange<TTarget, TValue> : IItemChange<TTarget>, IEquatable<ItemChange<TTarget, TValue>>
{
	public IChangeAccessor<TTarget, TValue> Accessor { get; }

	public ChangeTransform<TValue> Transform { get; }

	public ItemChange(IChangeAccessor<TTarget, TValue> accessor, ChangeTransform<TValue> transform)
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
		Accessor.SetValue(target, Transform.Apply(Accessor.GetValue(target)));
	}

	public bool Equals(ItemChange<TTarget, TValue> other)
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
		return Equals((ItemChange<TTarget, TValue>)obj);
	}

	public override int GetHashCode()
	{
		return (Accessor.GetHashCode() * 397) ^ Transform.GetHashCode();
	}
}
