using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class RadialMenuItemDesigner : BaseItemDesigner
{
	private void RemoveDescriptors(IDictionary properties, string[] propNames)
	{
		foreach (string key in propNames)
		{
			if (properties.Contains(key))
			{
				if (properties[key] is PropertyDescriptor)
				{
					properties[key] = TypeDescriptor.CreateProperty(((ComponentDesigner)this).Component.GetType(), (PropertyDescriptor)properties[key], new BrowsableAttribute(browsable: false));
				}
				else if (properties[key] is EventDescriptor)
				{
					properties[key] = TypeDescriptor.CreateEvent(((ComponentDesigner)this).Component.GetType(), (EventDescriptor)properties[key], new BrowsableAttribute(browsable: false));
				}
				else
				{
					properties.Remove(key);
				}
			}
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		base.PreFilterProperties(properties);
		RemoveDescriptors(properties, new string[14]
		{
			"ItemAlignment", "GlobalItem", "AccessibleDefaultActionDescription", "AccessibleDescription", "AccessibleName", "AccessibleRole", "CanCustomize", "Category", "KeyTips", "Shortcuts",
			"ThemeAware", "GlobalName", "Stretch", "BeginGroup"
		});
	}
}
