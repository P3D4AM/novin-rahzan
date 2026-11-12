using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public abstract class EntryPropertyValues : PropertyValues
{
	private IReadOnlyList<IProperty> _properties;

	public override IReadOnlyList<IProperty> Properties
	{
		[DebuggerStepThrough]
		get
		{
			return _properties ?? (_properties = EntityType.GetProperties().ToList());
		}
	}

	public override object this[string propertyName]
	{
		get
		{
			return GetValueInternal(EntityType.GetProperty(propertyName));
		}
		set
		{
			SetValueInternal(EntityType.GetProperty(propertyName), value);
		}
	}

	public override object this[IProperty property]
	{
		get
		{
			return GetValueInternal(EntityType.CheckPropertyBelongsToType(property));
		}
		set
		{
			SetValueInternal(EntityType.CheckPropertyBelongsToType(property), value);
		}
	}

	protected EntryPropertyValues([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry)
		: base(internalEntry)
	{
	}

	public override object ToObject()
	{
		return Clone().ToObject();
	}

	public override void SetValues(object obj)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(obj, "obj");
		if (obj.GetType() == EntityType.ClrType)
		{
			foreach (IProperty item in Properties.Where((IProperty p) => !p.IsShadowProperty()))
			{
				SetValueInternal(item, ((Property)item).Getter.GetClrValue(obj));
			}
			return;
		}
		foreach (IProperty property in Properties)
		{
			PropertyInfo propertyInfo = obj.GetType().GetAnyProperty(property.Name)?.FindGetterProperty();
			if (propertyInfo != null)
			{
				SetValueInternal(property, propertyInfo.GetValue(obj));
			}
		}
	}

	public override PropertyValues Clone()
	{
		object[] array = new object[Properties.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GetValueInternal(Properties[i]);
		}
		return new ArrayPropertyValues(InternalEntry, array);
	}

	public override void SetValues(PropertyValues propertyValues)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyValues, "propertyValues");
		foreach (IProperty property in Properties)
		{
			SetValueInternal(property, propertyValues[property.Name]);
		}
	}

	protected abstract void SetValueInternal([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object value);

	protected abstract object GetValueInternal([JetBrains.Annotations.NotNull] IProperty property);
}
