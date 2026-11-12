using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class TabItemDesigner : ComponentDesigner
{
	[Description("Gets or sets whether item is visible.")]
	[Category("Layout")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool Visible
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["Visible"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["Visible"] = value;
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ComponentDesigner)this).PreFilterProperties(properties);
		properties["Visible"] = TypeDescriptor.CreateProperty(typeof(TabItemDesigner), (PropertyDescriptor)properties["Visible"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Layout"));
	}
}
