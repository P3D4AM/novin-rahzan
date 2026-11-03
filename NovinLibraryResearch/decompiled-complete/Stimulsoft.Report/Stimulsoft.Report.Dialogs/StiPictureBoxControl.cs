using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiPictureBoxControl), "Stimulsoft.Report.Dialogs.Bmp.StiPictureBoxControl.gif")]
[StiGdiPainter(typeof(StiPictureBoxControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiPictureBoxControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiPictureBoxControl : StiReportControl, IStiPictureBoxControl
{
	private PictureBox control;

	private PictureBoxSizeMode sizeMode;

	private Color transparentColor = Color.Transparent;

	private BorderStyle borderStyle;

	private Image image;

	public override StiComponentId ComponentId => StiComponentId.StiPictureBoxControl;

	public override int ToolboxPosition => 13;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiPictureBoxControl");

	[Browsable(false)]
	public PictureBox Control
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

	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override double Width
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)SizeMode == 2 && image != null)
			{
				return image.Width + GetBorderWidth() * 2;
			}
			return base.Width;
		}
		set
		{
			base.Width = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override double Height
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)SizeMode == 2 && image != null)
			{
				return image.Height + GetBorderHeight() * 2;
			}
			return base.Height;
		}
		set
		{
			base.Height = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Indicates how the image is displayed.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(680)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public PictureBoxSizeMode SizeMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return sizeMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			sizeMode = value;
			UpdateReportControl("SizeMode");
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the transparent color for the image.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(770)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color TransparentColor
	{
		get
		{
			return transparentColor;
		}
		set
		{
			transparentColor = value;
			MakeTransparent();
			UpdateReportControl("TransparentColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Indicates the border style for the control.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return borderStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			borderStyle = value;
			UpdateReportControl("BorderStyle");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the image that the PictureBox displays.")]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(420)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Image Image
	{
		get
		{
			return image;
		}
		set
		{
			image = value;
			MakeTransparent();
			UpdateReportControl("Image");
		}
	}

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
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 96.0, 96.0);

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "SizeMode", (Enum)(object)SizeMode, (Enum)(object)(PictureBoxSizeMode)0);
		JsonExtensions.AddPropertyColor(val, "TransparentColor", TransparentColor, Color.Transparent);
		JsonExtensions.AddPropertyEnum(val, "BorderStyle", (Enum)(object)BorderStyle, (Enum)(object)(BorderStyle)0);
		val.Add("Image", JToken.op_Implicit(StiImageConverter.ImageToString(Image, false)));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "SizeMode":
				sizeMode = StiJsonReportObjectHelper.DeserializeEnum<PictureBoxSizeMode>(item);
				break;
			case "TransparentColor":
				transparentColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderStyle":
				borderStyle = StiJsonReportObjectHelper.DeserializeEnum<BorderStyle>(item);
				break;
			case "Image":
				image = StiJsonReportObjectHelper.DeserializeImage(item);
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
		return new StiPictureBoxControl();
	}

	private void MakeTransparent()
	{
		Image obj = Image;
		Bitmap val = (Bitmap)(object)((obj is Bitmap) ? obj : null);
		if (val != null && TransparentColor != Color.Transparent)
		{
			val.MakeTransparent(TransparentColor);
		}
	}

	public int GetBorderWidth()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		if ((int)BorderStyle == 2)
		{
			return SystemInformation.Border3DSize.Width;
		}
		if ((int)BorderStyle == 1)
		{
			return 1;
		}
		return 0;
	}

	public int GetBorderHeight()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		if ((int)BorderStyle == 2)
		{
			return SystemInformation.Border3DSize.Height;
		}
		if ((int)BorderStyle == 1)
		{
			return 1;
		}
		return 0;
	}

	public StiPictureBoxControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiPictureBoxControl(RectangleD rect)
		: base(rect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
