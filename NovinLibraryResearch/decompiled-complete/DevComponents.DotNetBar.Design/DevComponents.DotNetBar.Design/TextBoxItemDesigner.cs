using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;

namespace DevComponents.DotNetBar.Design;

public class TextBoxItemDesigner : SimpleBaseItemDesigner
{
	[DefaultValue("")]
	[Localizable(true)]
	[Editor(typeof(TextMarkupUIEditor), typeof(UITypeEditor))]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates watermark text displayed inside of the control when Text is not set and control does not have input focus.")]
	public string WatermarkText
	{
		get
		{
			return (string)((ComponentDesigner)this).ShadowProperties["WatermarkText"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["WatermarkText"] = value;
		}
	}

	[DefaultValue(eWatermarkBehavior.HideOnFocus)]
	[Description("Indicates watermark hiding behaviour.")]
	[Category("Behavior")]
	public eWatermarkBehavior WatermarkBehavior
	{
		get
		{
			return (eWatermarkBehavior)((ComponentDesigner)this).ShadowProperties["WatermarkBehavior"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["WatermarkBehavior"] = value;
		}
	}

	[Description("Indicates watermark text color.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Color WatermarkColor
	{
		get
		{
			return (Color)((ComponentDesigner)this).ShadowProperties["WatermarkColor"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["WatermarkColor"] = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether watermark text is displayed when control is empty.")]
	public bool WatermarkEnabled
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["WatermarkEnabled"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["WatermarkEnabled"] = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates watermark font.")]
	[DefaultValue(null)]
	public Font WatermarkFont
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			return (Font)((ComponentDesigner)this).ShadowProperties["WatermarkFont"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["WatermarkFont"] = value;
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		base.PreFilterProperties(properties);
		properties["WatermarkText"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkText"], (Attribute[])null);
		properties["WatermarkBehavior"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkBehavior"], (Attribute[])null);
		properties["WatermarkColor"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkColor"], (Attribute[])null);
		properties["WatermarkEnabled"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkEnabled"], (Attribute[])null);
		properties["WatermarkFont"] = TypeDescriptor.CreateProperty(typeof(TextBoxItemDesigner), (PropertyDescriptor)properties["WatermarkFont"], (Attribute[])null);
	}
}
