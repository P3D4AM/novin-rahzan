using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class ArrayPropertyValues : PropertyValues
{
	private readonly object[] _values;

	private IReadOnlyList<IProperty> _properties;

	public override IReadOnlyList<IProperty> Properties => _properties ?? (_properties = EntityType.GetProperties().ToList());

	public override object this[string propertyName]
	{
		get
		{
			return _values[EntityType.GetProperty(propertyName).GetIndex()];
		}
		set
		{
			SetValue(EntityType.GetProperty(propertyName).GetIndex(), value);
		}
	}

	public override object this[IProperty property]
	{
		get
		{
			return _values[EntityType.CheckPropertyBelongsToType(property).GetIndex()];
		}
		set
		{
			SetValue(EntityType.CheckPropertyBelongsToType(property).GetIndex(), value);
		}
	}

	private IEntityMaterializerSource MaterializerSource => InternalEntry.StateManager.EntityMaterializerSource;

	public ArrayPropertyValues([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] object[] values)
		: base(internalEntry)
	{
		_values = values;
	}

	public override object ToObject()
	{
		Func<MaterializationContext, object> materializer = MaterializerSource.GetMaterializer(EntityType);
		ValueBuffer valueBuffer = new ValueBuffer(_values);
		return materializer(new MaterializationContext(in valueBuffer, InternalEntry.StateManager.Context));
	}

	public override void SetValues(object obj)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(obj, "obj");
		if (obj.GetType() == EntityType.ClrType)
		{
			for (int i = 0; i < _values.Length; i++)
			{
				if (!Properties[i].IsShadowProperty())
				{
					SetValue(i, ((Property)Properties[i]).Getter.GetClrValue(obj));
				}
			}
			return;
		}
		for (int j = 0; j < _values.Length; j++)
		{
			PropertyInfo propertyInfo = obj.GetType().GetAnyProperty(Properties[j].Name)?.FindGetterProperty();
			if (propertyInfo != null)
			{
				SetValue(j, propertyInfo.GetValue(obj));
			}
		}
	}

	public override PropertyValues Clone()
	{
		object[] array = new object[_values.Length];
		Array.Copy(_values, array, _values.Length);
		return new ArrayPropertyValues(InternalEntry, array);
	}

	public override void SetValues(PropertyValues propertyValues)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyValues, "propertyValues");
		for (int i = 0; i < _values.Length; i++)
		{
			SetValue(i, propertyValues[Properties[i].Name]);
		}
	}

	public override TValue GetValue<TValue>(string propertyName)
	{
		return (TValue)this[propertyName];
	}

	public override TValue GetValue<TValue>(IProperty property)
	{
		return (TValue)this[property];
	}

	private void SetValue(int index, object value)
	{
		IProperty property = Properties[index];
		if (value != null)
		{
			if (!property.ClrType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
			{
				throw new InvalidCastException(CoreStrings.InvalidType(property.Name, property.DeclaringEntityType.DisplayName(), value.GetType().DisplayName(), property.ClrType.DisplayName()));
			}
		}
		else if (!property.ClrType.IsNullableType())
		{
			throw new InvalidOperationException(CoreStrings.ValueCannotBeNull(property.Name, property.DeclaringEntityType.DisplayName(), property.ClrType.DisplayName()));
		}
		_values[index] = value;
	}
}
