using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class ExpandableSplitterDesigner : ControlDesigner
{
	[Description("Indicates visual style of the control.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eSplitterStyle.Office2003)]
	public eSplitterStyle Style
	{
		get
		{
			return ((ExpandableSplitter)(object)((ControlDesigner)this).Control).Style;
		}
		set
		{
			((ExpandableSplitter)(object)((ControlDesigner)this).Control).Style = value;
			if (!(((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: not false }))
			{
				((ExpandableSplitter)(object)((ControlDesigner)this).Control).ApplyStyle(value);
				OnStyleChanged();
			}
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		Style = eSplitterStyle.Office2007;
		((Control)(ExpandableSplitter)(object)((ControlDesigner)this).Control).Width = 6;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		properties["Style"] = TypeDescriptor.CreateProperty(typeof(ExpandableSplitterDesigner), (PropertyDescriptor)properties["Style"]);
	}

	private void OnStyleChanged()
	{
		ExpandableSplitter component = ((ControlDesigner)this).Control as ExpandableSplitter;
		if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
		{
			componentChangeService.OnComponentChanging(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["Style"]);
			componentChangeService.OnComponentChanged(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["Style"], null, null);
			componentChangeService.OnComponentChanging(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["BackColor"]);
			componentChangeService.OnComponentChanged(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["BackColor"], null, null);
			componentChangeService.OnComponentChanging(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["BackColor2"]);
			componentChangeService.OnComponentChanged(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["BackColor2"], null, null);
			componentChangeService.OnComponentChanging(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["GripLightColor"]);
			componentChangeService.OnComponentChanged(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["GripLightColor"], null, null);
			componentChangeService.OnComponentChanging(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["GripDarkColor"]);
			componentChangeService.OnComponentChanged(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["GripDarkColor"], null, null);
			componentChangeService.OnComponentChanging(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["EpxandFillColor"]);
			componentChangeService.OnComponentChanged(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["ExpandFillColor"], null, null);
			componentChangeService.OnComponentChanging(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["EpxandLineColor"]);
			componentChangeService.OnComponentChanged(component, TypeDescriptor.GetProperties(typeof(ExpandableSplitter))["ExpandLineColor"], null, null);
		}
	}

	private IDesignerHost GetIDesignerHost()
	{
		try
		{
			return (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		}
		catch
		{
		}
		return null;
	}
}
