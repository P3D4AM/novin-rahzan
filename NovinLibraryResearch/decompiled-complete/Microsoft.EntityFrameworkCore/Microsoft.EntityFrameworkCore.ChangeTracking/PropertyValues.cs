using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public abstract class PropertyValues
{
	[EntityFrameworkInternal]
	protected virtual InternalEntityEntry InternalEntry
	{
		[DebuggerStepThrough]
		get;
	}

	public abstract IReadOnlyList<IProperty> Properties { get; }

	public virtual IEntityType EntityType
	{
		[DebuggerStepThrough]
		get
		{
			return InternalEntry.EntityType;
		}
	}

	public abstract object this[[JetBrains.Annotations.NotNull] string propertyName]
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public abstract object this[[JetBrains.Annotations.NotNull] IProperty property]
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	[EntityFrameworkInternal]
	protected PropertyValues([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry)
	{
		InternalEntry = internalEntry;
	}

	public abstract object ToObject();

	public abstract void SetValues([JetBrains.Annotations.NotNull] object obj);

	public abstract PropertyValues Clone();

	public abstract void SetValues([JetBrains.Annotations.NotNull] PropertyValues propertyValues);

	public virtual void SetValues([JetBrains.Annotations.NotNull] IDictionary<string, object> values)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(values, "values");
		foreach (IProperty property in Properties)
		{
			if (values.TryGetValue(property.Name, out var value))
			{
				this[property] = value;
			}
		}
	}

	public abstract TValue GetValue<TValue>([JetBrains.Annotations.NotNull] string propertyName);

	public abstract TValue GetValue<TValue>([JetBrains.Annotations.NotNull] IProperty property);

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
