using System;
using System.ComponentModel;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Design;

public class StiDataBindingPropertyDescriptor : StiPropertyDescriptor
{
	public override string DisplayName
	{
		get
		{
			string displayName = ((StiPropertyDescriptor)this).PropertyDescriptor.DisplayName;
			displayName = displayName.Substring(0, displayName.IndexOf("Binding", StringComparison.InvariantCulture));
			if (!((StiPropertyDescriptor)this).IsLocalizableProperties)
			{
				return displayName;
			}
			string text = StiLocalization.Get("PropertyMain", ((StiPropertyDescriptor)this).PropertyDescriptor.ComponentType.Name + displayName, false);
			if (text == null)
			{
				text = StiLocalization.Get("PropertyMain", displayName, false);
			}
			return text ?? displayName;
		}
	}

	public override bool CanResetValue(object component)
	{
		return ((StiPropertyDescriptor)this).PropertyDescriptor.CanResetValue(((StiDataBindingsCollection)component).Control);
	}

	public override object GetValue(object component)
	{
		return ((StiPropertyDescriptor)this).PropertyDescriptor.GetValue(((StiDataBindingsCollection)component).Control);
	}

	public override void ResetValue(object component)
	{
		((StiPropertyDescriptor)this).PropertyDescriptor.ResetValue(((StiDataBindingsCollection)component).Control);
	}

	public override void SetValue(object component, object value)
	{
		((StiPropertyDescriptor)this).PropertyDescriptor.SetValue(((StiDataBindingsCollection)component).Control, value);
	}

	public override bool ShouldSerializeValue(object component)
	{
		return ((StiPropertyDescriptor)this).PropertyDescriptor.ShouldSerializeValue(((StiDataBindingsCollection)component).Control);
	}

	public StiDataBindingPropertyDescriptor(PropertyDescriptor propertyDescriptor)
		: base(propertyDescriptor)
	{
	}
}
