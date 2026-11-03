using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Base.Design;

public class StiServicesTab : PropertyTab
{
	public override Bitmap Bitmap => StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.PropertiesTab.bmp");

	public override string TabName => StiLocalization.Get("Report", "PropertiesTab");

	public override bool CanExtend(object extendee)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
	{
		return ((PropertyTab)this).GetProperties(component, attributes);
	}

	public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
		PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
		foreach (PropertyDescriptor item in properties)
		{
			if (StiPropertyGrid.IsAllowedProperty(component.GetType(), item.Name) && item.Attributes[typeof(StiServiceParamAttribute)] != null)
			{
				propertyDescriptorCollection.Add((PropertyDescriptor)new StiPropertyDescriptor(item));
			}
		}
		return propertyDescriptorCollection;
	}
}
