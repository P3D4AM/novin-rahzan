using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiPanelControl), "Stimulsoft.Report.Dialogs.Bmp.StiPanelControl.gif")]
[StiGdiPainter(typeof(StiCustomControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiPanelControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiCustomControl : StiReportControl, IStiSerializableCustomControl
{
	private StiBorderStyle borderStyle;

	private object control;

	public override int ToolboxPosition => 19;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => "StiCustomControl";

	public override RectangleD ClientRectangle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.ClientRectangle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			base.ClientRectangle = value;
			if (control != null && control is Control)
			{
				((Control)control).Width = (int)Width;
				((Control)control).Height = (int)Height;
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			return base.ForeColor;
		}
		set
		{
			base.ForeColor = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[DefaultValue(StiBorderStyle.None)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Indicates the border style for the control.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	public StiBorderStyle BorderStyle
	{
		get
		{
			return borderStyle;
		}
		set
		{
			borderStyle = value;
			UpdateReportControl("BorderStyle");
		}
	}

	[Browsable(true)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public object Control
	{
		get
		{
			return control;
		}
		set
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			control = value;
			if (value != null && value is Control)
			{
				((Control)value).Width = (int)Width;
				((Control)value).Height = (int)Height;
			}
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
		JsonExtensions.RemoveProperty(val, "ForeColor");
		JsonExtensions.AddPropertyEnum(val, "BorderStyle", (Enum)BorderStyle, (Enum)StiBorderStyle.None);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "BorderStyle")
			{
				borderStyle = StiJsonReportObjectHelper.DeserializeEnum<StiBorderStyle>(item);
			}
		}
	}

	public override StiEvent GetDefaultEvent()
	{
		return ClickEvent;
	}

	public StiCustomControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiCustomControl(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
