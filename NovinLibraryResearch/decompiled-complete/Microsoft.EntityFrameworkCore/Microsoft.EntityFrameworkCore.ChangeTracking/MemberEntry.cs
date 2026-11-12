using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public abstract class MemberEntry : IInfrastructure<InternalEntityEntry>
{
	[EntityFrameworkInternal]
	protected virtual InternalEntityEntry InternalEntry { get; }

	public abstract bool IsModified { get; set; }

	public virtual IPropertyBase Metadata { get; }

	public virtual object CurrentValue
	{
		get
		{
			return InternalEntry[Metadata];
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			InternalEntry[Metadata] = value;
		}
	}

	public virtual EntityEntry EntityEntry => new EntityEntry(InternalEntry);

	[EntityFrameworkInternal]
	InternalEntityEntry IInfrastructure<InternalEntityEntry>.Instance => InternalEntry;

	[EntityFrameworkInternal]
	protected MemberEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] IPropertyBase metadata)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(internalEntry, "internalEntry");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(metadata, "metadata");
		InternalEntry = internalEntry;
		Metadata = metadata;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
