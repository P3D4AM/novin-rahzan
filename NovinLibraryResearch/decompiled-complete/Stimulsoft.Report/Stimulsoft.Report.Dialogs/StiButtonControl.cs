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
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiButtonControl), "Stimulsoft.Report.Dialogs.Bmp.StiButtonControl.gif")]
[StiGdiPainter(typeof(StiButtonControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiButtonControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiButtonControl : StiReportControl
{
	private Image image;

	private bool defaultValue;

	private DialogResult dialogResult;

	private bool cancel;

	private string text = "Button";

	public override StiComponentId ComponentId => StiComponentId.StiButtonControl;

	public override int ToolboxPosition => 4;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiButtonControl");

	[StiNonSerialized]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			return base.BackColor;
		}
		set
		{
			base.BackColor = value;
		}
	}

	[Browsable(false)]
	public Button Control { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the image that is displayed on a button control.")]
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
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			image = value;
			if (value != null && value is Bitmap)
			{
				StiImageUtils.MakeImageBackgroundAlphaZero((Bitmap)value);
			}
			UpdateReportControl("Image");
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the value which indicates which button is clicked when the user presses the ENTER key.")]
	[StiOrder(250)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool Default
	{
		get
		{
			return defaultValue;
		}
		set
		{
			defaultValue = value;
			UpdateReportControl("Default");
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value that is returned to the parent form when the button is clicked.")]
	[StiOrder(260)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public DialogResult DialogResult
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return dialogResult;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			dialogResult = value;
			UpdateReportControl("DialogResult");
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the value which indicates which button is clicked when the user presses the ESCAPE key.")]
	[StiOrder(170)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool Cancel
	{
		get
		{
			return cancel;
		}
		set
		{
			cancel = value;
			UpdateReportControl("Cancel");
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the alignment of the image on the button control.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(430)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public ContentAlignment ImageAlign { get; set; } = (ContentAlignment)32;


	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the alignment of the text on the button control.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiOrder(740)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public ContentAlignment TextAlign { get; set; } = (ContentAlignment)32;


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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "BackColor");
		JsonExtensions.AddPropertyImage(val, "Image", Image);
		JsonExtensions.AddPropertyBool(val, "Default", Default, false, false);
		JsonExtensions.AddPropertyEnum(val, "DialogResult", (Enum)(object)DialogResult, (Enum)(object)(DialogResult)0);
		JsonExtensions.AddPropertyBool(val, "Cancel", Cancel, false, false);
		JsonExtensions.AddPropertyEnum(val, "ImageAlign", (Enum)(object)ImageAlign, (Enum)(object)(ContentAlignment)32);
		JsonExtensions.AddPropertyEnum(val, "TextAlign", (Enum)(object)TextAlign, (Enum)(object)(ContentAlignment)32);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Image":
				Image = StiJsonReportObjectHelper.DeserializeImage(item);
				break;
			case "Default":
				defaultValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DialogResult":
				dialogResult = StiJsonReportObjectHelper.DeserializeEnum<DialogResult>(item);
				break;
			case "Cancel":
				cancel = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ImageAlign":
				ImageAlign = StiJsonReportObjectHelper.DeserializeEnum<ContentAlignment>(item);
				break;
			case "TextAlign":
				TextAlign = StiJsonReportObjectHelper.DeserializeEnum<ContentAlignment>(item);
				break;
			case "Text":
				text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiButtonControl();
	}

	public StiButtonControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiButtonControl(RectangleD rect)
		: base(rect)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		Text = LocalizedName;
	}
}
