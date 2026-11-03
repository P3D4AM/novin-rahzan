using System;
using System.ComponentModel;

namespace DevComponents.DotNetBar;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public abstract class PropertyValueEditor : Attribute
{
	public abstract IPropertyValueEditor CreateEditor(PropertyDescriptor propertyDescriptor, object targetObject);
}
