using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class PropertyEntry : MemberEntry
{
	public override bool IsModified
	{
		get
		{
			return InternalEntry.IsModified(Metadata);
		}
		set
		{
			InternalEntry.SetPropertyModified(Metadata, changeState: true, value);
		}
	}

	public virtual bool IsTemporary
	{
		get
		{
			return InternalEntry.HasTemporaryValue(Metadata);
		}
		set
		{
			if (value)
			{
				InternalEntry.SetTemporaryValue(Metadata, CurrentValue);
			}
			else
			{
				InternalEntry[Metadata] = CurrentValue;
			}
		}
	}

	public new virtual IProperty Metadata => (IProperty)base.Metadata;

	public virtual object OriginalValue
	{
		get
		{
			return InternalEntry.GetOriginalValue(Metadata);
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			InternalEntry.SetOriginalValue(Metadata, value);
		}
	}

	[EntityFrameworkInternal]
	public PropertyEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] string name)
		: this(internalEntry, internalEntry.EntityType.GetProperty(name))
	{
	}

	[EntityFrameworkInternal]
	public PropertyEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] IProperty property)
		: base(internalEntry, property)
	{
	}
}
public class PropertyEntry<TEntity, TProperty> : PropertyEntry where TEntity : class
{
	public new virtual EntityEntry<TEntity> EntityEntry => new EntityEntry<TEntity>(InternalEntry);

	public new virtual TProperty CurrentValue
	{
		get
		{
			return InternalEntry.GetCurrentValue<TProperty>(Metadata);
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			base.CurrentValue = value;
		}
	}

	public new virtual TProperty OriginalValue
	{
		get
		{
			return InternalEntry.GetOriginalValue<TProperty>(Metadata);
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			base.OriginalValue = value;
		}
	}

	[EntityFrameworkInternal]
	public PropertyEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] string name)
		: base(internalEntry, name)
	{
	}

	[EntityFrameworkInternal]
	public PropertyEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] IProperty property)
		: base(internalEntry, property)
	{
	}
}
