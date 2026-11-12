using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class DockContainerItemDesigner : ComponentDesigner
{
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets whether item is visible.")]
	[Browsable(true)]
	[Category("Layout")]
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

	public override void Initialize(IComponent component)
	{
		((ComponentDesigner)this).Initialize(component);
		if (component.Site.DesignMode && component is DockContainerItem dockContainerItem)
		{
			Visible = dockContainerItem.Visible;
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ComponentDesigner)this).PreFilterProperties(properties);
		properties["Visible"] = TypeDescriptor.CreateProperty(typeof(DockContainerItemDesigner), (PropertyDescriptor)properties["Visible"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Layout"));
	}
}
