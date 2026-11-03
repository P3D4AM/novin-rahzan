using System.ComponentModel;
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
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiGroupBoxControl), "Stimulsoft.Report.Dialogs.Bmp.StiGroupBoxControl.gif")]
[StiGdiPainter(typeof(StiGroupBoxControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiGroupBoxControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiGroupBoxControl : StiReportControl
{
	private GroupBox control;

	private string textBinding = "";

	private string text = "GroupBox";

	public override StiComponentId ComponentId => StiComponentId.StiGroupBoxControl;

	public override int ToolboxPosition => 3;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiGroupBoxControl");

	[Browsable(false)]
	public override RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.RightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.RightToLeft = value;
		}
	}

	[Browsable(false)]
	public GroupBox Control
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

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 96.0, 96.0);

	[Browsable(false)]
	public override bool IsReportContainer => true;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextBinding", TextBinding);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "TextBinding"))
			{
				if (text == "Text")
				{
					this.text = StiJsonReportObjectHelper.DeserializeString(item);
				}
			}
			else
			{
				textBinding = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiGroupBoxControl();
	}

	public StiGroupBoxControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiGroupBoxControl(RectangleD rect)
		: base(rect)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		Text = LocalizedName;
	}
}
