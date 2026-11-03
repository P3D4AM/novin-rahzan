using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiLabelControl), "Stimulsoft.Report.Dialogs.Bmp.StiLabelControl.gif")]
[StiGdiPainter(typeof(StiLabelControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiLabelControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiLabelControl : StiReportControl, IStiLabelControl
{
	private Label control;

	private string textBinding = "";

	private string text = "Label";

	private ContentAlignment textAlign = (ContentAlignment)1;

	public override StiComponentId ComponentId => StiComponentId.StiLabelControl;

	public override int ToolboxPosition => 1;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiLabelControl");

	[Browsable(false)]
	public Label Control
	{
		get
		{
			return control;
		}
		set
		{
			control = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the text.")]
	[StiOrder(220)]
	public string TextBinding
	{
		get
		{
			return textBinding;
		}
		set
		{
			textBinding = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Gets or sets the text associated with this control.")]
	[StiOrder(730)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			UpdateReportControl("Text");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the alignment of text in the label.")]
	[StiOrder(740)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public ContentAlignment TextAlign
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return textAlign;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			textAlign = value;
			UpdateReportControl("TextAlign");
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextBinding", TextBinding);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		JsonExtensions.AddPropertyEnum(val, "TextAlign", (Enum)(object)TextAlign, (Enum)(object)(ContentAlignment)1);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "TextBinding":
				textBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Text":
				text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TextAlign":
				textAlign = StiJsonReportObjectHelper.DeserializeEnum<ContentAlignment>(item);
				break;
			}
		}
	}

	public override StiEvent GetDefaultEvent()
	{
		return ClickEvent;
	}

	public override StiComponent CreateNew()
	{
		return new StiLabelControl();
	}

	public StiLabelControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiLabelControl(RectangleD rect)
		: base(rect)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		Text = LocalizedName;
	}
}
