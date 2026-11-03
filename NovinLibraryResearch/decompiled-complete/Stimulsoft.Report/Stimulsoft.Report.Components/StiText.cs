using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiText), "Stimulsoft.Report.Images.Components.StiText.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiTextDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfTextDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiTextGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiTextWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiAutoWidth))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiContextTool(typeof(IStiText))]
[StiContextTool(typeof(IStiOnlyText))]
[StiContextTool(typeof(IStiEditable))]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiTextOptions))]
[StiContextTool(typeof(IStiTextFormat))]
[StiContextTool(typeof(IStiBreakable))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiTextQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfTextQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiText : StiSimpleText, IStiTextOptions, IStiAutoWidth, IStiTextHorAlignment, IStiVertAlignment, IStiBorder, IStiFont, IStiBrush, IStiTextBrush, IStiTextFormat, IStiRenderTo, IStiSerializable, IStiBreakable, IStiGlobalizationProvider, IStiExportImageExtended, IStiExportImage, IStiIndicator
{
	protected static object PropertyCanBreak = new object();

	protected static object PropertyAutoWidth = new object();

	protected static object PropertyRenderTo = new object();

	private Font font = new Font("Arial", 8f);

	private StiFormatService textFormat;

	private string format = string.Empty;

	private static readonly object EventGetExcelValue = new object();

	protected static object PropertyNullValue = new object();

	protected static object PropertyType = new object();

	protected static object PropertyLineSpacing = new object();

	protected static object PropertyExportAsImage = new object();

	protected static object PropertyTextQuality = new object();

	protected static object PropertyAllowHtmlTags = new object();

	protected static object PropertyShrinkFontToFit = new object();

	protected static object PropertyShrinkFontToFitMinimumSize = new object();

	public override StiComponentId ComponentId => StiComponentId.StiText;

	public override string HelpUrl => "User-Manual/report_internals_output_text_parameters.htm";

	[StiSerializable]
	[Description("Gets or sets special indicator for text component.")]
	[Browsable(false)]
	public virtual StiIndicator Indicator { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool CanBreak
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyCanBreak, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyCanBreak, value, false);
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that this object can change width automatically.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AutoWidth
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyAutoWidth, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyAutoWidth, value, false);
		}
	}

	protected virtual StiTextHorAlignment DefaultHorAlignment => (StiTextHorAlignment)0;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(240)]
	[StiSerializable]
	[DefaultValue("")]
	[Description("In the property specify the Text component in what the text that is out of the current Text component bound will be continued to be output.")]
	[TypeConverter(typeof(StiRenderToConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiRenderToEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string RenderTo
	{
		get
		{
			return ((StiService)this).Properties.Get(PropertyRenderTo, (object)string.Empty) as string;
		}
		set
		{
			((StiService)this).Properties.Set(PropertyRenderTo, (object)string.Intern(value), (object)string.Empty);
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value that indicates that this component is to be output as WYSIWYG concept.")]
	public virtual bool WYSIWYG
	{
		get
		{
			return TextQuality == StiTextQuality.Wysiwyg;
		}
		set
		{
			TextQuality = (value ? StiTextQuality.Typographic : StiTextQuality.Standard);
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets the text horizontal alignment.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual StiTextHorAlignment HorAlignment { get; set; }

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[Description("Gets or sets the vertical alignment of an object.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual StiVertAlignment VertAlignment { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(180)]
	[StiSerializable]
	[Description("Gets or sets font of component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (value != null || !base.IsDesigning)
			{
				font = value;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(300)]
	[StiSerializable]
	[Description("The brush of the component, which is used to display text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Black);


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[Description("Gets or sets the format of the text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiFormatService TextFormat
	{
		get
		{
			if (textFormat == null)
			{
				return StiGeneralFormatService.Default;
			}
			return textFormat;
		}
		set
		{
			if (value is StiGeneralFormatService)
			{
				value = null;
			}
			if (textFormat == value)
			{
				return;
			}
			textFormat = value;
			if (value == null)
			{
				format = "G";
			}
			else if (value is StiPercentageFormatService)
			{
				StiPercentageFormatService stiPercentageFormatService = value as StiPercentageFormatService;
				if (stiPercentageFormatService.UseLocalSetting && stiPercentageFormatService.State == StiTextFormatState.None)
				{
					format = "P";
					return;
				}
				format = "P" + stiPercentageFormatService.DecimalDigits;
				if (stiPercentageFormatService.UseGroupSeparator)
				{
					format += "G";
				}
				if (!stiPercentageFormatService.UseLocalSetting && stiPercentageFormatService.DecimalDigits > 0 && (stiPercentageFormatService.DecimalSeparator[0] == '.' || stiPercentageFormatService.DecimalSeparator[0] == ','))
				{
					format += stiPercentageFormatService.DecimalSeparator[0];
				}
			}
			else if (value is StiCurrencyFormatService)
			{
				StiCurrencyFormatService stiCurrencyFormatService = value as StiCurrencyFormatService;
				if (stiCurrencyFormatService.UseLocalSetting && stiCurrencyFormatService.State == StiTextFormatState.None)
				{
					format = "C";
					return;
				}
				format = "C" + stiCurrencyFormatService.DecimalDigits;
				if (stiCurrencyFormatService.UseGroupSeparator)
				{
					format += "G";
				}
				if (stiCurrencyFormatService.NegativePattern == 0 || stiCurrencyFormatService.NegativePattern == 4 || stiCurrencyFormatService.NegativePattern == 14 || stiCurrencyFormatService.NegativePattern == 15)
				{
					format += "(";
				}
				if (stiCurrencyFormatService.DecimalDigits > 0 && (stiCurrencyFormatService.DecimalSeparator[0] == '.' || stiCurrencyFormatService.DecimalSeparator[0] == ','))
				{
					format += stiCurrencyFormatService.DecimalSeparator[0];
				}
				if (stiCurrencyFormatService.PositivePattern == 0 || stiCurrencyFormatService.PositivePattern == 2)
				{
					format += "+";
				}
				else
				{
					format += "-";
				}
				format += stiCurrencyFormatService.Symbol;
				if (!stiCurrencyFormatService.UseLocalSetting || (stiCurrencyFormatService.State & StiTextFormatState.PositivePattern) > StiTextFormatState.None || (stiCurrencyFormatService.State & StiTextFormatState.NegativePattern) > StiTextFormatState.None)
				{
					format = format + "|" + (char)(65 + stiCurrencyFormatService.PositivePattern) + (char)(65 + stiCurrencyFormatService.NegativePattern);
				}
			}
			else if (value is StiDateFormatService)
			{
				StiDateFormatService stiDateFormatService = value as StiDateFormatService;
				format = "D" + stiDateFormatService.StringFormat;
			}
			else if (value is StiNumberFormatService)
			{
				StiNumberFormatService stiNumberFormatService = value as StiNumberFormatService;
				if (stiNumberFormatService.UseLocalSetting && stiNumberFormatService.State == StiTextFormatState.None)
				{
					format = "N";
					return;
				}
				format = $"N{stiNumberFormatService.DecimalDigits}";
				if (stiNumberFormatService.UseGroupSeparator)
				{
					format += "G";
				}
				if (stiNumberFormatService.NegativePattern == 0)
				{
					format += "(";
				}
				if (stiNumberFormatService.DecimalDigits > 0 && !string.IsNullOrWhiteSpace(stiNumberFormatService.DecimalSeparator) && (stiNumberFormatService.DecimalSeparator[0] == '.' || stiNumberFormatService.DecimalSeparator[0] == ','))
				{
					format += stiNumberFormatService.DecimalSeparator[0];
				}
			}
			else if (value is StiTimeFormatService)
			{
				StiTimeFormatService stiTimeFormatService = value as StiTimeFormatService;
				format = "T" + stiTimeFormatService.StringFormat;
			}
			else if (value is StiCustomFormatService)
			{
				format = "U";
			}
			else
			{
				format = string.Empty;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Browsable(false)]
	public virtual string Format
	{
		get
		{
			return format;
		}
		set
		{
			if (format != value)
			{
				format = string.Intern(value);
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(295)]
	[StiSerializable]
	[DefaultValue(null)]
	[Description("Gets or sets options to control of the text showing.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiTextOptions TextOptions { get; set; } = new StiTextOptions();


	public override int ToolboxPosition => 101;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => StiLocalization.Get("Components", "StiText");

	[Browsable(false)]
	[Description("Gets or sets excel data value.")]
	public string ExcelDataValue { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[Description("Gets or sets an expression used for export data to Excel. Only for numeric values.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiExcelValueExpression ExcelValue
	{
		get
		{
			return new StiExcelValueExpression(this, "ExcelValue");
		}
		set
		{
			value?.Set(this, "ExcelValue", value.Value);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the ExcelValue is calculated.")]
	public StiGetExcelValueEvent GetExcelValueEvent
	{
		get
		{
			return new StiGetExcelValueEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue("")]
	[Description("Gets or sets a value which shows instead null values.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string NullValue
	{
		get
		{
			return ((StiService)this).Properties.Get(PropertyNullValue, (object)string.Empty) as string;
		}
		set
		{
			((StiService)this).Properties.Set(PropertyNullValue, (object)value, (object)string.Empty);
		}
	}

	[StiSerializable]
	[DefaultValue(StiSystemTextType.None)]
	[Browsable(false)]
	public StiSystemTextType Type
	{
		get
		{
			return (StiSystemTextType)((StiService)this).Properties.Get(PropertyType, (object)StiSystemTextType.None);
		}
		set
		{
			((StiService)this).Properties.Set(PropertyType, (object)value, (object)StiSystemTextType.None);
		}
	}

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	[StiOrder(310)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool WordWrap
	{
		get
		{
			return TextOptions.WordWrap;
		}
		set
		{
			TextOptions.WordWrap = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets horizontal output direction.")]
	[StiOrder(250)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool RightToLeft
	{
		get
		{
			return TextOptions.RightToLeft;
		}
		set
		{
			TextOptions.RightToLeft = value;
		}
	}

	[Browsable(false)]
	[StiBrowsable(false)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets type to trim the end of a line.")]
	[StiOrder(300)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StringTrimming Trimming
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return TextOptions.Trimming;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			TextOptions.Trimming = value;
		}
	}

	[DefaultValue(0f)]
	[StiOrder(110)]
	[Description("Gets or sets angle of a text rotation.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float Angle
	{
		get
		{
			return TextOptions.Angle;
		}
		set
		{
			TextOptions.Angle = value;
		}
	}

	[DefaultValue(1f)]
	[StiSerializable]
	[StiOrder(170)]
	[Description("Gets or sets line spacing of a text.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float LineSpacing
	{
		get
		{
			return ((StiService)this).Properties.GetFloat(PropertyLineSpacing, 1f);
		}
		set
		{
			if (value > 0f)
			{
				((StiService)this).Properties.SetFloat(PropertyLineSpacing, value, 1f);
			}
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates how content of text will be exported as image or as text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ExportAsImage
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyExportAsImage, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyExportAsImage, value, false);
		}
	}

	[StiSerializable]
	[DefaultValue(StiTextQuality.Standard)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(280)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value that indicates quality of text.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiTextQuality TextQuality
	{
		get
		{
			if (!StiOptions.Engine.FullTrust)
			{
				return StiTextQuality.Typographic;
			}
			return (StiTextQuality)((StiService)this).Properties.Get(PropertyTextQuality, (object)StiTextQuality.Standard);
		}
		set
		{
			((StiService)this).Properties.Set(PropertyTextQuality, (object)value, (object)StiTextQuality.Standard);
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value that indicates that this component allow Html tags in text.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AllowHtmlTags
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyAllowHtmlTags, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyAllowHtmlTags, value, false);
			if (value)
			{
				TextQuality = StiTextQuality.Wysiwyg;
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[Description("Gets or sets text margins.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiMargins Margins { get; set; } = new StiMargins();


	[DefaultValue(StiExceedMargins.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(135)]
	[TypeConverter(typeof(StiExceedMarginsConverter))]
	[Description("Gets or sets a value to exceed margins of the component background.")]
	[Editor("Stimulsoft.Report.Components.Design.StiExceedMarginsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiExceedMargins ExceedMargins { get; set; }

	[Browsable(false)]
	public override bool ForceRedrawAll => ExceedMargins != StiExceedMargins.None;

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(260)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value that indicates that this component is descrease size of font to fit content of component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShrinkFontToFit
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyShrinkFontToFit, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyShrinkFontToFit, value, false);
		}
	}

	[StiSerializable]
	[DefaultValue(1f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(270)]
	[Description("Gets or sets value that indicates minimum font size for ShrinkFontToFit operation.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float ShrinkFontToFitMinimumSize
	{
		get
		{
			return ((StiService)this).Properties.GetFloat(PropertyShrinkFontToFitMinimumSize, 1f);
		}
		set
		{
			if (value >= 1f)
			{
				((StiService)this).Properties.SetFloat(PropertyShrinkFontToFitMinimumSize, value, 1f);
			}
		}
	}

	public event StiGetExcelValueEventHandler GetExcelValue
	{
		add
		{
			base.Events.AddHandler(EventGetExcelValue, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetExcelValue, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		if (Indicator != null)
		{
			JsonExtensions.AddPropertyJObject(val, "Indicator", Indicator.SaveToJsonObject(mode));
		}
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.AddPropertyBool(val, "AutoWidth", AutoWidth, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "RenderTo", RenderTo);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)(object)HorAlignment, (Enum)(object)(StiTextHorAlignment)0);
		JsonExtensions.AddPropertyEnum(val, "VertAlignment", (Enum)(object)VertAlignment, (Enum)(object)(StiVertAlignment)0);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		JsonExtensions.AddPropertyJObject(val, "TextOptions", TextOptions.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetExcelValueEvent", GetExcelValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "NullValue", NullValue);
		JsonExtensions.AddPropertyBool(val, "ExportAsImage", ExportAsImage, false, false);
		JsonExtensions.AddPropertyEnum(val, "TextQuality", (Enum)TextQuality, (Enum)StiTextQuality.Standard);
		JsonExtensions.AddPropertyBool(val, "AllowHtmlTags", AllowHtmlTags, false, false);
		JsonExtensions.AddPropertyJObject(val, "Margins", Margins.SaveToJsonObject(0.0, 0.0, 0.0, 0.0));
		JsonExtensions.AddPropertyEnum(val, "ExceedMargins", (Enum)ExceedMargins, (Enum)StiExceedMargins.None);
		JsonExtensions.AddPropertyBool(val, "ShrinkFontToFit", ShrinkFontToFit, false, false);
		JsonExtensions.AddPropertyFloat(val, "ShrinkFontToFitMinimumSize", ShrinkFontToFitMinimumSize, 1f);
		JsonExtensions.AddPropertyDouble(val, "LineSpacing", (double)LineSpacing, 1.0);
		if ((int)mode == 0)
		{
			if (!(TextFormat is StiGeneralFormatService))
			{
				JsonExtensions.AddPropertyJObject(val, "TextFormat", TextFormat.SaveToJsonObject(mode));
			}
			JsonExtensions.AddPropertyEnum(val, "Type", (Enum)Type, (Enum)StiSystemTextType.None);
			JsonExtensions.AddPropertyJObject(val, "ExcelValue", ExcelValue.SaveToJsonObject(mode));
		}
		else
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
			if (ExcelDataValue != null)
			{
				JsonExtensions.AddPropertyString(val, "ExcelDataValue", ExcelDataValue);
			}
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Expected O, but got Unknown
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Expected O, but got Unknown
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Indicator":
				Indicator = StiIndicator.CreateFromJsonObject((JObject)item.Value);
				break;
			case "CanBreak":
				CanBreak = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AutoWidth":
				AutoWidth = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RenderTo":
				RenderTo = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "HorAlignment":
				HorAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiTextHorAlignment>(item);
				break;
			case "VertAlignment":
				VertAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiVertAlignment>(item);
				break;
			case "Font":
				font = StiJsonReportObjectHelper.DeserializeFont(item, font);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TextBrush":
				TextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Format":
				format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TextOptions":
				TextOptions.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ExcelValue":
			{
				StiExcelValueExpression stiExcelValueExpression = new StiExcelValueExpression();
				stiExcelValueExpression.LoadFromJsonObject((JObject)item.Value);
				ExcelValue = stiExcelValueExpression;
				break;
			}
			case "ExcelDataValue":
				ExcelDataValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "GetExcelValueEvent":
			{
				StiGetExcelValueEvent stiGetExcelValueEvent = new StiGetExcelValueEvent();
				stiGetExcelValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetExcelValueEvent = stiGetExcelValueEvent;
				break;
			}
			case "NullValue":
				NullValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Type":
				Type = StiJsonReportObjectHelper.DeserializeEnum<StiSystemTextType>(item);
				break;
			case "ExportAsImage":
				ExportAsImage = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "TextQuality":
				TextQuality = StiJsonReportObjectHelper.DeserializeEnum<StiTextQuality>(item);
				break;
			case "LineSpacing":
				LineSpacing = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "AllowHtmlTags":
				AllowHtmlTags = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Margins":
			{
				StiMargins stiMargins = new StiMargins();
				stiMargins.LoadFromJsonObject((JObject)item.Value);
				Margins = stiMargins;
				break;
			}
			case "ExceedMargins":
				ExceedMargins = StiJsonReportObjectHelper.DeserializeEnum<StiExceedMargins>(item);
				break;
			case "ShrinkFontToFit":
				ShrinkFontToFit = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShrinkFontToFitMinimumSize":
				ShrinkFontToFitMinimumSize = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Invalid comparison between Unknown and I4
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Invalid comparison between Unknown and I4
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Invalid comparison between Unknown and I4
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Invalid comparison between Unknown and I4
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.TextEditor() });
		stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[4]
		{
			propertiesHelper.Text(),
			propertiesHelper.HorAlignment(),
			propertiesHelper.VertAlignment(),
			propertiesHelper.TextFormat()
		});
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[4]
			{
				propertiesHelper.TextAngle(),
				propertiesHelper.HideZeros(),
				propertiesHelper.LineSpacing(),
				propertiesHelper.WordWrap()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[12]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextAngle(),
				propertiesHelper.Editable(),
				propertiesHelper.HideZeros(),
				propertiesHelper.LinesOfUnderline(),
				propertiesHelper.LineSpacing(),
				propertiesHelper.Margins(),
				propertiesHelper.MaxNumberOfLines(),
				propertiesHelper.OnlyText(),
				propertiesHelper.ProcessingDuplicates(),
				propertiesHelper.TextQuality(),
				propertiesHelper.WordWrap()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[17]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextAngle(),
				propertiesHelper.Editable(),
				propertiesHelper.HideZeros(),
				propertiesHelper.LinesOfUnderline(),
				propertiesHelper.LineSpacing(),
				propertiesHelper.Margins(),
				propertiesHelper.MaxNumberOfLines(),
				propertiesHelper.OnlyText(),
				propertiesHelper.ProcessAt(),
				propertiesHelper.ProcessingDuplicates(),
				propertiesHelper.RenderTo(),
				propertiesHelper.ShrinkFontToFit(),
				propertiesHelper.ShrinkFontToFitMinimumSize(),
				propertiesHelper.TextQuality(),
				propertiesHelper.TextOptions(),
				propertiesHelper.WordWrap()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[4]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[6]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height(),
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.Font(),
				propertiesHelper.TextBrush()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[7]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.Font(),
				propertiesHelper.TextBrush(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.AutoWidth(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[11]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.AutoWidth(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[12]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.AutoWidth(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.Printable(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[1] { propertiesHelper.Name() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[6]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.GlobalizedName(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
		}
		if ((int)level == 2)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Export, new StiPropertyObject[2]
			{
				propertiesHelper.ExcelValue(),
				propertiesHelper.ExportAsImage()
			});
		}
		return stiPropertyCollection;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetBookmarkEvent,
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetPointerEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.AfterPrintEvent,
			StiPropertyEventId.BeforePrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetExcelValueEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetValueEvent
		});
		return stiEventCollection;
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter.GetImage(this, ref zoom, format);
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		if (Indicator != null && format != StiExportFormat.Pdf)
		{
			return true;
		}
		bool flag = base.IsExportAsImage(format);
		if (flag)
		{
			return flag;
		}
		if (CheckAllowHtmlTags() && format != StiExportFormat.Html && format != StiExportFormat.HtmlDiv && format != StiExportFormat.HtmlSpan && format != StiExportFormat.HtmlTable && format != StiExportFormat.Rtf && format != StiExportFormat.RtfTabbedText && format != StiExportFormat.RtfFrame && format != StiExportFormat.RtfWinWord && format != StiExportFormat.RtfTable && format != StiExportFormat.Text && format != StiExportFormat.Pdf && format != StiExportFormat.ImageSvg && format != StiExportFormat.ImageSvgz && (format != StiExportFormat.Word2007 || StiOptions.Export.Word.RenderHtmlTagsAsImage) && (format != StiExportFormat.Excel2007 || StiOptions.Export.Excel.RenderHtmlTagsAsImage))
		{
			return true;
		}
		if (LinesOfUnderlining && format != StiExportFormat.Dif && format != StiExportFormat.Pdf && format != StiExportFormat.Sylk && format != StiExportFormat.Text)
		{
			return true;
		}
		return ExportAsImage;
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		switch (propertyName)
		{
		case "Text":
			SetTextInternal(value);
			break;
		case "ToolTip":
			ToolTip.Value = value;
			break;
		case "Tag":
			Tag.Value = value;
			break;
		case "Hyperlink":
			Hyperlink.Value = value;
			break;
		default:
			throw new ArgumentException("Property with name " + propertyName);
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		return propertyName switch
		{
			"Text" => GetTextInternal(), 
			"Tag" => Tag.Value, 
			"ToolTip" => ToolTip.Value, 
			"Hyperlink" => Hyperlink.Value, 
			_ => throw new ArgumentException("Property with name " + propertyName), 
		};
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		List<string> list = new List<string>();
		if (StiOptions.Engine.Globalization.AllowUseText)
		{
			list.Add("Text");
		}
		if (StiOptions.Engine.Globalization.AllowUseTag)
		{
			list.Add("Tag");
		}
		if (StiOptions.Engine.Globalization.AllowUseToolTip)
		{
			list.Add("ToolTip");
		}
		if (StiOptions.Engine.Globalization.AllowUseHyperlink)
		{
			list.Add("Hyperlink");
		}
		return list.ToArray();
	}

	public bool Break(StiComponent dividedComponent, double devideFactor, ref double divideLine)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		divideLine = 0.0;
		bool result = true;
		if (GetTextInternal() != null && GetTextInternal().Length > 0)
		{
			string textInternal = GetTextInternal();
			RectangleD rect = Report.Unit.ConvertToHInches(ClientRectangle);
			rect = ConvertTextMargins(rect, convert: false);
			rect = ConvertTextBorders(rect, convert: false);
			string textInternal2 = string.Empty;
			if (((RectangleD)(ref rect)).Height >= 0.0)
			{
				textInternal2 = StiComponentDivider.BreakText(GetMeasureGraphics(), rect, ref textInternal, Font, TextOptions, TextQuality, CheckAllowHtmlTags(), this);
			}
			SetTextInternal(textInternal2);
			if (!GrowToHeight && (GetTextInternal() == null || GetTextInternal().Length == 0))
			{
				result = false;
			}
			((StiText)dividedComponent).SetTextInternal(textInternal);
			if (StiOptions.Engine.MarkBreakedText && dividedComponent.TagValue == null)
			{
				dividedComponent.TagValue = "#breaked#";
			}
		}
		return result;
	}

	protected void Write(StiObjectStringConverter converter, string name, object obj, XmlTextWriter tw)
	{
		string text = converter.ObjectToString(obj);
		if (text != null)
		{
			tw.WriteAttributeString(name, text);
		}
	}

	public void Serialize(StiObjectStringConverter converter, XmlTextWriter tw)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Invalid comparison between Unknown and I4
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		tw.WriteAttributeString("name", Name);
		tw.WriteAttributeString("text", Text);
		if (HorAlignment != DefaultHorAlignment)
		{
			StiTextHorAlignment horAlignment = HorAlignment;
			tw.WriteAttributeString("ha", ((object)(StiTextHorAlignment)(ref horAlignment)).ToString());
		}
		StiVertAlignment vertAlignment;
		if (this is StiCrossField)
		{
			if ((int)VertAlignment != 1)
			{
				vertAlignment = VertAlignment;
				tw.WriteAttributeString("va", ((object)(StiVertAlignment)(ref vertAlignment)).ToString());
			}
		}
		else if ((int)VertAlignment != 0)
		{
			vertAlignment = VertAlignment;
			tw.WriteAttributeString("va", ((object)(StiVertAlignment)(ref vertAlignment)).ToString());
		}
		if (LineSpacing != 1f)
		{
			tw.WriteAttributeString("ls", LineSpacing.ToString(CultureInfo.InvariantCulture));
		}
		if (TextQuality != 0)
		{
			tw.WriteAttributeString("tq", TextQuality.ToString());
		}
		if (Editable)
		{
			tw.WriteAttributeString("ed", Editable ? "true" : "false");
		}
		if (!Enabled)
		{
			tw.WriteAttributeString("enabled", Enabled ? "true" : "false");
		}
		if (!Printable)
		{
			tw.WriteAttributeString("pr", Printable ? "true" : "false");
		}
		if (AllowHtmlTags)
		{
			tw.WriteAttributeString("html", AllowHtmlTags ? "true" : "false");
		}
		if ((int)LinesOfUnderline != 6)
		{
			StiPenStyle linesOfUnderline = LinesOfUnderline;
			tw.WriteAttributeString("lu", ((object)(StiPenStyle)(ref linesOfUnderline)).ToString());
		}
		if (base.Guid != null)
		{
			tw.WriteAttributeString("guid", base.Guid);
		}
		if (TotalValueHelp != null)
		{
			tw.WriteAttributeString("tvh", TotalValueHelp);
		}
		if (base.PointerValue != null)
		{
			tw.WriteAttributeString("pointer", base.PointerValue.ToString());
		}
		if (base.BookmarkValue != null)
		{
			tw.WriteAttributeString("bookmark", base.BookmarkValue.ToString());
		}
		if (base.HyperlinkValue != null)
		{
			tw.WriteAttributeString("hyperlink", base.HyperlinkValue.ToString());
		}
		if (base.TagValue != null)
		{
			tw.WriteAttributeString("tag", base.TagValue.ToString());
		}
		if (base.ToolTipValue != null)
		{
			tw.WriteAttributeString("toolTip", base.ToolTipValue.ToString());
		}
		if (ExcelDataValue != null)
		{
			tw.WriteAttributeString("excelvalue", ExcelDataValue);
		}
		if (Format != null && format.Length > 0)
		{
			tw.WriteAttributeString("format", Format);
		}
		if (!string.IsNullOrEmpty(base.ComponentPlacement))
		{
			tw.WriteAttributeString("pl", base.ComponentPlacement);
		}
		if (!string.IsNullOrEmpty(ComponentStyle))
		{
			tw.WriteAttributeString("style", ComponentStyle);
		}
		Write(converter, "rc", ClientRectangle, tw);
		Write(converter, "fn", Font, tw);
		Write(converter, "tb", TextBrush, tw);
		Write(converter, "bh", Brush, tw);
		Write(converter, "br", Border, tw);
		Write(converter, "to", TextOptions, tw);
		if (Margins.Left != 0.0 || Margins.Right != 0.0 || Margins.Top != 0.0 || Margins.Bottom != 0.0)
		{
			Write(converter, "mr", Margins, tw);
		}
		if (Indicator != null)
		{
			StiIndicatorConverter stiIndicatorConverter = new StiIndicatorConverter();
			string value = stiIndicatorConverter.ConvertToInvariantString(Indicator);
			tw.WriteAttributeString("indicator", value);
		}
		if (Page.Document != null && Page.Document.Report.SaveInteractionParametersToDocument)
		{
			if (Interaction != null)
			{
				string text = ((JToken)Interaction.SaveToJsonObject((StiJsonSaveMode)1)).ToString((Formatting)0, CultureInfo.InvariantCulture, (JsonConverter[])(object)new JsonConverter[0]);
				tw.WriteAttributeString("interaction", text.Substring(1, text.Length - 2));
			}
			if (base.DrillDownParameters != null && base.DrillDownParameters.Count > 0)
			{
				string text2 = JsonConvert.SerializeObject((object)base.DrillDownParameters);
				tw.WriteAttributeString("ddparams", text2.Substring(1, text2.Length - 2));
			}
		}
	}

	public void Deserialize(StiObjectStringConverter converter, XmlTextReader tr)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		string attribute = tr.GetAttribute("name");
		if (attribute != null)
		{
			Name = attribute;
		}
		attribute = tr.GetAttribute("text");
		if (attribute != null)
		{
			Text = attribute;
		}
		attribute = tr.GetAttribute("ha");
		if (attribute != null)
		{
			HorAlignment = (StiTextHorAlignment)Enum.Parse(typeof(StiTextHorAlignment), attribute);
		}
		attribute = tr.GetAttribute("va");
		if (attribute != null)
		{
			VertAlignment = (StiVertAlignment)Enum.Parse(typeof(StiVertAlignment), attribute);
		}
		attribute = tr.GetAttribute("ls");
		if (attribute != null)
		{
			LineSpacing = float.Parse(attribute);
		}
		attribute = tr.GetAttribute("tq");
		if (attribute != null)
		{
			TextQuality = (StiTextQuality)Enum.Parse(typeof(StiTextQuality), attribute);
		}
		attribute = tr.GetAttribute("ed");
		if (attribute != null)
		{
			Editable = attribute.ToLowerInvariant() == "true";
		}
		attribute = tr.GetAttribute("html");
		if (attribute != null)
		{
			AllowHtmlTags = attribute.ToLowerInvariant() == "true";
		}
		attribute = tr.GetAttribute("enabled");
		if (attribute != null)
		{
			Enabled = attribute.ToLowerInvariant() == "true";
		}
		attribute = tr.GetAttribute("ws");
		if (attribute != null)
		{
			WYSIWYG = attribute.ToLowerInvariant() == "true";
		}
		attribute = tr.GetAttribute("pr");
		if (attribute != null)
		{
			Printable = attribute.ToLowerInvariant() != "false";
		}
		attribute = tr.GetAttribute("lou");
		if (attribute != null)
		{
			LinesOfUnderlining = attribute.ToLowerInvariant() == "true";
		}
		attribute = tr.GetAttribute("lu");
		if (attribute != null)
		{
			LinesOfUnderline = (StiPenStyle)Enum.Parse(typeof(StiPenStyle), attribute);
		}
		base.Guid = tr.GetAttribute("guid");
		TotalValueHelp = tr.GetAttribute("tvh");
		base.PointerValue = tr.GetAttribute("pointer");
		base.BookmarkValue = tr.GetAttribute("bookmark");
		base.HyperlinkValue = tr.GetAttribute("hyperlink");
		base.TagValue = tr.GetAttribute("tag");
		base.ToolTipValue = tr.GetAttribute("toolTip");
		ExcelDataValue = tr.GetAttribute("excelvalue");
		format = tr.GetAttribute("format");
		base.ComponentPlacement = tr.GetAttribute("pl");
		ComponentStyle = tr.GetAttribute("style");
		attribute = tr.GetAttribute("rc");
		if (attribute != null)
		{
			ClientRectangle = (RectangleD)converter.StringToObject(attribute, typeof(RectangleD));
		}
		attribute = tr.GetAttribute("fn");
		if (attribute != null)
		{
			object obj = converter.StringToObject(attribute, typeof(Font));
			Font = (Font)((obj is Font) ? obj : null);
		}
		attribute = tr.GetAttribute("tb");
		if (attribute != null)
		{
			object obj2 = converter.StringToObject(attribute, typeof(StiBrush));
			TextBrush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		}
		attribute = tr.GetAttribute("bh");
		if (attribute != null)
		{
			object obj3 = converter.StringToObject(attribute, typeof(StiBrush));
			Brush = (StiBrush)((obj3 is StiBrush) ? obj3 : null);
		}
		attribute = tr.GetAttribute("to");
		if (attribute != null)
		{
			object obj4 = converter.StringToObject(attribute, typeof(StiTextOptions));
			TextOptions = (StiTextOptions)((obj4 is StiTextOptions) ? obj4 : null);
		}
		attribute = tr.GetAttribute("br");
		if (attribute != null)
		{
			object obj5 = converter.StringToObject(attribute, typeof(StiBorder));
			Border = (StiBorder)((obj5 is StiBorder) ? obj5 : null);
		}
		attribute = tr.GetAttribute("mr");
		if (attribute != null)
		{
			Margins = (StiMargins)converter.StringToObject(attribute, typeof(StiMargins));
		}
		attribute = tr.GetAttribute("indicator");
		if (attribute != null)
		{
			StiIndicatorConverter stiIndicatorConverter = new StiIndicatorConverter();
			Indicator = stiIndicatorConverter.ConvertFromInvariantString(attribute) as StiIndicator;
		}
		attribute = tr.GetAttribute("interaction");
		if (attribute != null)
		{
			Interaction = new StiInteraction();
			JObject jObject = (JObject)JsonConvert.DeserializeObject("{" + attribute + "}");
			Interaction.LoadFromJsonObject(jObject);
		}
		attribute = tr.GetAttribute("ddparams");
		if (attribute != null)
		{
			base.DrillDownParameters = new Dictionary<string, object>();
			JsonConvert.PopulateObject("{" + attribute + "}", (object)base.DrillDownParameters);
		}
	}

	public override void InvokeRenderTo(StiSimpleText textFrom)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (!((StiService)textFrom).IsPropertyPresent(PropertyRenderTo))
		{
			return;
		}
		string renderTo = ((StiText)textFrom).RenderTo;
		if (renderTo.Length <= 0)
		{
			return;
		}
		renderTo = renderTo.Trim();
		if (renderTo.Length > 0 && textFrom.Parent.Components[renderTo] is StiSimpleText stiSimpleText)
		{
			string text = textFrom.GetTextInternal();
			if (text == null)
			{
				text = string.Empty;
			}
			string text2 = GetVisibleTextForRenderTo(GetMeasureGraphics(), textFrom.GetPaintRectangle(convertToHInches: true, convertByZoom: false, docking: true), ref text, textFrom as StiText);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = " ";
			}
			textFrom.TextValue = text2;
			textFrom.SetTextInternal(text2);
			stiSimpleText.TextValue = text;
			stiSimpleText.SetTextInternal(text);
			InvokeRenderTo(stiSimpleText);
		}
	}

	private string GetVisibleTextForRenderTo(Graphics g, RectangleD rect, ref string text, StiText checkedText)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		rect = ConvertTextMargins(rect, convert: false);
		rect = ConvertTextBorders(rect, convert: false);
		return StiComponentDivider.BreakText(g, rect, ref text, checkedText.Font, checkedText.TextOptions, checkedText.TextQuality, CheckAllowHtmlTags(), this);
	}

	private bool ShouldSerializeFont()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (Font != null && Font.Name == "Arial" && (int)font.Style == 0)
		{
			return font.Size != 8f;
		}
		return true;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.Transparent);
		}
		return true;
	}

	private bool ShouldSerializeTextBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TextBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)TextBrush).Color == Color.Black);
		}
		return true;
	}

	public override object Clone(bool cloneProperties)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		StiText stiText = (StiText)base.Clone(cloneProperties);
		if (Border != null)
		{
			stiText.Border = (StiBorder)Border.Clone();
		}
		else
		{
			stiText.Border = null;
		}
		if (Brush != null)
		{
			stiText.Brush = (StiBrush)Brush.Clone();
		}
		else
		{
			stiText.Brush = null;
		}
		if (textFormat != null)
		{
			stiText.textFormat = (StiFormatService)((StiService)textFormat).Clone();
		}
		else
		{
			stiText.textFormat = null;
		}
		if (TextBrush != null)
		{
			stiText.TextBrush = (StiBrush)TextBrush.Clone();
		}
		else
		{
			stiText.TextBrush = null;
		}
		if (TextOptions != null)
		{
			stiText.TextOptions = (StiTextOptions)TextOptions.Clone();
		}
		else
		{
			stiText.TextOptions = null;
		}
		stiText.HorAlignment = HorAlignment;
		stiText.VertAlignment = VertAlignment;
		return stiText;
	}

	public override List<StiFont> GetFonts()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		return fonts.Distinct().ToList();
	}

	public RectangleD ConvertTextMargins(RectangleD rect, bool convert)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		StiMargins margins = Margins;
		if (margins.IsEmpty)
		{
			return rect;
		}
		double num = Page.Zoom * StiScale.Factor;
		double num2 = margins.Left;
		double right = margins.Right;
		double num3 = margins.Top;
		double bottom = margins.Bottom;
		if (!convert)
		{
			num = 1.0;
		}
		if (num2 != 0.0)
		{
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + num2 * num;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - num2 * num;
		}
		if (num3 != 0.0)
		{
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + num3 * num;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - num3 * num;
		}
		if (right != 0.0)
		{
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - right * num;
		}
		if (bottom != 0.0)
		{
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - bottom * num;
		}
		return rect;
	}

	public RectangleD ConvertTextBorders(RectangleD rect, bool convert)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		if ((int)Border.Style != 6)
		{
			float sizeOffset = Border.GetSizeOffset();
			if (Border.IsLeftBorderSidePresent)
			{
				num = sizeOffset;
			}
			if (Border.IsRightBorderSidePresent)
			{
				num2 = sizeOffset;
			}
			if (Border.IsTopBorderSidePresent)
			{
				num3 = sizeOffset;
			}
			if (Border.IsBottomBorderSidePresent)
			{
				num4 = sizeOffset;
			}
		}
		StiBorder border = Border;
		StiAdvancedBorder val = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
		if (val != (StiAdvancedBorder)null)
		{
			num = val.LeftSide.GetSizeOffset();
			num2 = val.RightSide.GetSizeOffset();
			num3 = val.TopSide.GetSizeOffset();
			num4 = val.BottomSide.GetSizeOffset();
		}
		if (convert)
		{
			double num5 = (double)(float)Page.Zoom * StiScale.Factor;
			num *= num5;
			num2 *= num5;
			num3 *= num5;
			num4 *= num5;
		}
		((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + num;
		((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + num3;
		((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - (num + num2);
		((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - (num3 + num4);
		if (Report != null && Report.IsWpf)
		{
			((RectangleD)(ref rect)).Inflate(-1.0, -1.0);
		}
		return rect;
	}

	public virtual string GetTextForPaint()
	{
		if (Report != null && Report.Designer != null && base.IsDesigning)
		{
			if (StiOptions.Designer.UseGlobalizationManager && Report.GlobalizationManager != null && !(Report.GlobalizationManager is StiNullGlobalizationManager) && !string.IsNullOrWhiteSpace(GlobalizedName))
			{
				try
				{
					string @string = Report.GlobalizationManager.GetString(GlobalizedName);
					if (@string != null)
					{
						return @string;
					}
				}
				catch
				{
				}
			}
			if (Report.Designer.UseAliases)
			{
				if (Report.Designer.TextAliasesHash != null)
				{
					if (Report.Designer.TextAliasesHash[this] is string result)
					{
						return result;
					}
					string text = StiExpressionPacker.PackExpression(GetTextInternal(), Report.Designer, useBraces: true);
					Report.Designer.TextAliasesHash[this] = text;
					return text;
				}
				return StiExpressionPacker.PackExpression(GetTextInternal(), Report.Designer, useBraces: true);
			}
		}
		return GetTextInternal();
	}

	public bool GetMarkerFieldResult()
	{
		if (!StiOptions.Designer.MarkComponentsWithErrors)
		{
			return false;
		}
		if (!base.IsDesigning || OnlyText || Text == null || GetTextInternal() == null || Report == null)
		{
			return false;
		}
		string textInternal = GetTextInternal();
		int num = StringExt.IndexOfInvariant(textInternal, "{");
		if (num == -1)
		{
			return false;
		}
		int num2 = StringExt.IndexOfInvariant(textInternal, "}");
		if (num2 == -1)
		{
			return true;
		}
		if (StringExt.IndexOfInvariant(textInternal, "?") != -1)
		{
			return false;
		}
		string text = textInternal.Substring(num + 1, num2 - num - 1);
		if (StringExt.IndexOfInvariant(text, ".") == -1)
		{
			return false;
		}
		string[] array = text.Split(new char[1] { '.' });
		if (array.Length > 2)
		{
			return false;
		}
		foreach (StiDataSource dataSource in Report.Dictionary.DataSources)
		{
			string text2 = dataSource.Name;
			if (!StiOptions.Configuration.IsWeb || StiOptions.Engine.FullTrust)
			{
				text2 = StiNameValidator.CorrectName(text2, Report);
			}
			if (text2 != array[0])
			{
				continue;
			}
			StiDataColumnsCollection columns = dataSource.Columns;
			int num3 = 1;
			if (num3 >= array.Length)
			{
				continue;
			}
			string text3 = array[num3];
			foreach (StiDataColumn item in columns)
			{
				string text4 = item.Name;
				if (!StiOptions.Configuration.IsWeb || StiOptions.Engine.FullTrust)
				{
					text4 = StiNameValidator.CorrectName(text4, Report);
				}
				if (text4 == text3)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public override SizeD GetActualSize()
	{
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Invalid comparison between Unknown and I4
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		bool autoWidth = AutoWidth;
		float angle = Angle;
		if ((CanGrow || CanShrink || autoWidth) && Page != null)
		{
			Hashtable hashtable = null;
			if (StiOptions.Engine.AllowCacheForGetActualSize)
			{
				StiReport report = Report;
				if (report != null && report.Engine != null)
				{
					if (report.Engine.HashCheckSize == null)
					{
						report.Engine.HashCheckSize = new Hashtable();
					}
					object obj = report.Engine.HashCheckSize[this];
					if (obj != null)
					{
						return (SizeD)obj;
					}
					hashtable = report.Engine.HashCheckSize;
				}
			}
			StiUnit unit = Page.Unit;
			RectangleD rect = unit.ConvertToHInches(ClientRectangle);
			SizeD size = default(SizeD);
			((SizeD)(ref size))._002Ector(((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
			rect = ConvertTextMargins(rect, convert: false);
			rect = ConvertTextBorders(rect, convert: false);
			StiTextQuality textQuality = TextQuality;
			SizeD val;
			if (Report.IsWpf && !CheckAllowHtmlTags())
			{
				float num = angle;
				Angle = 0f;
				if (num == 90f || num == 270f)
				{
					val = StiWpfTextRender.MeasureString(((RectangleD)(ref rect)).Height, this);
					double num2 = ((SizeD)(ref val)).Width;
					((SizeD)(ref val)).Width = ((SizeD)(ref val)).Height;
					((SizeD)(ref val)).Height = num2;
				}
				else
				{
					val = StiWpfTextRender.MeasureString(((RectangleD)(ref rect)).Width, this);
				}
				Angle = num;
			}
			else if (StiOptions.Engine.UseNewHtmlEngine && AllowHtmlTags)
			{
				float num3 = angle;
				if (num3 == 90f || num3 == 270f)
				{
					val = StiHtmlTextRender.MeasureString(this);
					double num4 = ((SizeD)(ref val)).Width;
					((SizeD)(ref val)).Width = ((SizeD)(ref val)).Height;
					((SizeD)(ref val)).Height = num4;
				}
				else
				{
					val = StiHtmlTextRender.MeasureString(this);
				}
				Angle = num3;
			}
			else if (textQuality != StiTextQuality.Wysiwyg && !CheckAllowHtmlTags())
			{
				val = ((textQuality != StiTextQuality.Typographic) ? ((angle == 90f || angle == 270f) ? StiStandardTextRenderer.MeasureString(((RectangleD)(ref rect)).Height, Font, this) : StiStandardTextRenderer.MeasureString(((RectangleD)(ref rect)).Width, Font, this)) : ((angle == 90f || angle == 270f) ? StiTypographicTextRender.MeasureString(((RectangleD)(ref rect)).Height, Font, this) : StiTypographicTextRender.MeasureString(((RectangleD)(ref rect)).Width, Font, this)));
			}
			else
			{
				float num5 = angle;
				Angle = 0f;
				if (num5 == 90f || num5 == 270f)
				{
					val = StiWysiwygTextRender.MeasureString(((RectangleD)(ref rect)).Height, Font, this);
					double num6 = ((SizeD)(ref val)).Width;
					((SizeD)(ref val)).Width = ((SizeD)(ref val)).Height;
					((SizeD)(ref val)).Height = num6;
				}
				else
				{
					val = StiWysiwygTextRender.MeasureString(((RectangleD)(ref rect)).Width, Font, this);
				}
				Angle = num5;
			}
			StiMargins margins = Margins;
			((SizeD)(ref val)).Width = ((SizeD)(ref val)).Width + (margins.Left + margins.Right);
			((SizeD)(ref val)).Height = ((SizeD)(ref val)).Height + (margins.Top + margins.Bottom);
			double num7 = 0.0;
			double num8 = 0.0;
			double num9 = 0.0;
			double num10 = 0.0;
			if ((int)Border.Style != 6)
			{
				float sizeOffset = Border.GetSizeOffset();
				if (Border.IsLeftBorderSidePresent)
				{
					num7 = sizeOffset;
				}
				if (Border.IsRightBorderSidePresent)
				{
					num8 = sizeOffset;
				}
				if (Border.IsTopBorderSidePresent)
				{
					num9 = sizeOffset;
				}
				if (Border.IsBottomBorderSidePresent)
				{
					num10 = sizeOffset;
				}
			}
			StiBorder border = Border;
			StiAdvancedBorder val2 = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
			if (val2 != (StiAdvancedBorder)null)
			{
				num7 = val2.LeftSide.GetSizeOffset();
				num8 = val2.RightSide.GetSizeOffset();
				num9 = val2.TopSide.GetSizeOffset();
				num10 = val2.BottomSide.GetSizeOffset();
			}
			((SizeD)(ref val)).Width = ((SizeD)(ref val)).Width + (num7 + num8);
			((SizeD)(ref val)).Height = ((SizeD)(ref val)).Height + (num9 + num10);
			if (Report != null && Report.IsWpf)
			{
				((SizeD)(ref val)).Width = ((SizeD)(ref val)).Width + 2.0;
				((SizeD)(ref val)).Height = ((SizeD)(ref val)).Height + 2.0;
			}
			if (autoWidth)
			{
				if (angle == 90f || angle == 270f)
				{
					((SizeD)(ref size)).Height = ((SizeD)(ref val)).Height;
				}
				else
				{
					((SizeD)(ref size)).Width = ((SizeD)(ref val)).Width;
				}
			}
			if (CanGrow)
			{
				if (angle == 90f || angle == 270f)
				{
					if (((SizeD)(ref val)).Width > ((SizeD)(ref size)).Width)
					{
						((SizeD)(ref size)).Width = ((SizeD)(ref val)).Width;
						if (MaxNumberOfLines > 0)
						{
							double num11 = (double)Font.GetHeight() * StiScale.System;
							int num12 = (int)(((SizeD)(ref size)).Width / num11);
							if (num12 > MaxNumberOfLines)
							{
								((SizeD)(ref size)).Width = num11 * (double)MaxNumberOfLines + num7 + num8;
								if (Report.IsWpf && !CheckAllowHtmlTags())
								{
									((SizeD)(ref size)).Width = ((SizeD)(ref size)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWpf;
								}
								else if (textQuality == StiTextQuality.Standard)
								{
									((SizeD)(ref size)).Width = ((SizeD)(ref size)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorStandard;
								}
								else if (textQuality == StiTextQuality.Typographic || (Report.IsWpf && !CheckAllowHtmlTags()))
								{
									((SizeD)(ref size)).Width = ((SizeD)(ref size)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorTypographic;
								}
								else if (textQuality == StiTextQuality.Wysiwyg || CheckAllowHtmlTags())
								{
									((SizeD)(ref size)).Width = ((SizeD)(ref size)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWysiwyg;
								}
							}
						}
					}
				}
				else if (((SizeD)(ref val)).Height > ((SizeD)(ref size)).Height)
				{
					((SizeD)(ref size)).Height = ((SizeD)(ref val)).Height;
					if (MaxNumberOfLines > 0)
					{
						double num13 = (double)Font.GetHeight() * StiScale.System;
						int num14 = (int)(((SizeD)(ref size)).Height / num13);
						if (num14 > MaxNumberOfLines)
						{
							((SizeD)(ref size)).Height = num13 * (double)MaxNumberOfLines + num9 + num10;
							if (Report.IsWpf && !CheckAllowHtmlTags())
							{
								((SizeD)(ref size)).Height = ((SizeD)(ref size)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWpf;
							}
							else if (textQuality == StiTextQuality.Standard)
							{
								((SizeD)(ref size)).Height = ((SizeD)(ref size)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorStandard;
							}
							else if (textQuality == StiTextQuality.Typographic)
							{
								((SizeD)(ref size)).Height = ((SizeD)(ref size)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorTypographic;
							}
							else if (textQuality == StiTextQuality.Wysiwyg || CheckAllowHtmlTags())
							{
								((SizeD)(ref size)).Height = ((SizeD)(ref size)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorWysiwyg;
							}
						}
					}
				}
			}
			if (CanShrink)
			{
				if (angle == 90f || angle == 270f)
				{
					if (GetTextInternal() == null || GetTextInternal().Trim().Length == 0)
					{
						((SizeD)(ref size)).Width = 0.0;
					}
					else
					{
						((SizeD)(ref size)).Width = Math.Min(((SizeD)(ref val)).Width, ((SizeD)(ref size)).Width);
					}
				}
				else if (GetTextInternal() == null || GetTextInternal().Trim().Length == 0)
				{
					((SizeD)(ref size)).Height = 0.0;
				}
				else
				{
					((SizeD)(ref size)).Height = Math.Min(((SizeD)(ref val)).Height, ((SizeD)(ref size)).Height);
				}
			}
			SizeD val3 = unit.ConvertFromHInches(size);
			if (angle == 90f || angle == 270f)
			{
				if (autoWidth && unit.ConvertToHInches(Math.Round(((SizeD)(ref val3)).Height, 2)) < ((SizeD)(ref size)).Height)
				{
					((SizeD)(ref val3)).Height = ((SizeD)(ref val3)).Height + 0.01;
				}
				if ((CanGrow || CanShrink) && unit.ConvertToHInches(Math.Round(((SizeD)(ref val3)).Width, 2)) < ((SizeD)(ref size)).Width)
				{
					((SizeD)(ref val3)).Width = ((SizeD)(ref val3)).Width + 0.01;
				}
			}
			else
			{
				if ((CanGrow || CanShrink) && unit.ConvertToHInches(Math.Round(((SizeD)(ref val3)).Height, 2)) < ((SizeD)(ref size)).Height)
				{
					((SizeD)(ref val3)).Height = ((SizeD)(ref val3)).Height + 0.01;
				}
				if (autoWidth && unit.ConvertToHInches(Math.Round(((SizeD)(ref val3)).Width, 2)) < ((SizeD)(ref size)).Width)
				{
					((SizeD)(ref val3)).Width = ((SizeD)(ref val3)).Width + 0.01;
				}
			}
			if (hashtable != null)
			{
				hashtable[this] = val3;
			}
			return val3;
		}
		return new SizeD(Width, Height);
	}

	public override void PackService()
	{
		base.PackService();
		textFormat = null;
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (GetExcelValueEvent != null)
		{
			stiEventsCollection.Add(GetExcelValueEvent);
		}
		return stiEventsCollection;
	}

	public override void Prepare()
	{
		base.Prepare();
		if (Conditions == null)
		{
			return;
		}
		foreach (StiBaseCondition condition in Conditions)
		{
			if (condition is IStiIndicatorCondition stiIndicatorCondition)
			{
				stiIndicatorCondition.Reset();
			}
		}
	}

	public override void InvokeEvents()
	{
		base.InvokeEvents();
		try
		{
			if ((object)base.Events[EventGetExcelValue] != null)
			{
				StiGetExcelValueEventArgs stiGetExcelValueEventArgs = new StiGetExcelValueEventArgs();
				stiGetExcelValueEventArgs.Value = ExcelDataValue;
				InvokeGetExcelValue(this, stiGetExcelValueEventArgs);
				ExcelDataValue = stiGetExcelValueEventArgs.Value;
			}
			StiReport report = Report;
			if (report != null && report.CalculationMode == StiCalculationMode.Interpretation && ExcelValue.Value.Length > 0)
			{
				ExcelDataValue = StiParser.ParseTextValue(ExcelValue.Value, this)?.ToString();
			}
			if (string.IsNullOrEmpty(ExcelDataValue))
			{
				return;
			}
			if (HideZeros)
			{
				if (decimal.TryParse(ExcelDataValue, out var result))
				{
					if (result == 0m)
					{
						ExcelDataValue = null;
					}
				}
				else
				{
					Report?.WriteToReportRenderingMessages(Name + " FormatException");
				}
			}
			StiNumberFormatService obj = TextFormat as StiNumberFormatService;
			if (obj != null && (obj.State & StiTextFormatState.Abbreviation) > StiTextFormatState.None)
			{
				ExcelDataValue = null;
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoEvents...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
		}
	}

	protected virtual void OnGetExcelValue(StiGetExcelValueEventArgs e)
	{
	}

	public virtual void InvokeGetExcelValue(StiComponent sender, StiGetExcelValueEventArgs e)
	{
		try
		{
			OnGetExcelValue(e);
			if (base.Events[EventGetExcelValue] is StiGetExcelValueEventHandler stiGetExcelValueEventHandler)
			{
				stiGetExcelValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetExcelValueEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in ExcelValue property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report?.WriteToReportRenderingMessages(text);
		}
	}

	private bool ShouldSerializeMargins()
	{
		return !Margins.IsDefault;
	}

	public override StiComponent CreateNew()
	{
		return new StiText();
	}

	internal bool CheckAllowHtmlTags()
	{
		if (!StiOptions.Engine.UseNewHtmlEngine && !StiOptions.Engine.FullTrust && Report != null && !Report.IsSerializing)
		{
			return false;
		}
		return AllowHtmlTags;
	}

	public Font GetActualFont(string text)
	{
		return GetActualFont(text, 1f);
	}

	public Font GetActualFont(string text, float minFontSize)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		RectangleD rect = Page.Unit.ConvertToHInches(ClientRectangle);
		rect = ConvertTextMargins(rect, convert: false);
		rect = ConvertTextBorders(rect, convert: false);
		((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - Border.Size;
		double maxWidth = ((Angle == 90f || Angle == 270f) ? ((RectangleD)(ref rect)).Height : ((RectangleD)(ref rect)).Width);
		double num = Page.Unit.ConvertToHInches(Width) - Margins.Left - Margins.Right;
		double num2 = Page.Unit.ConvertToHInches(Height) - Margins.Top - Margins.Bottom;
		Font val = font;
		Font val2 = font;
		float angle = Angle;
		while (true)
		{
			SizeD val3;
			if (!Report.IsWpf || CheckAllowHtmlTags())
			{
				val3 = ((TextQuality == StiTextQuality.Wysiwyg || CheckAllowHtmlTags()) ? StiWysiwygTextRender.MeasureString(maxWidth, val2, this) : ((TextQuality != StiTextQuality.Typographic) ? StiStandardTextRenderer.MeasureString(maxWidth, val2, this) : StiTypographicTextRender.MeasureString(maxWidth, val2, this)));
			}
			else
			{
				Angle = 0f;
				val3 = StiWpfTextRender.MeasureString(maxWidth, this);
				if (angle == 90f || angle == 270f)
				{
					double num3 = ((SizeD)(ref val3)).Width;
					((SizeD)(ref val3)).Width = ((SizeD)(ref val3)).Height;
					((SizeD)(ref val3)).Height = num3;
				}
				Angle = angle;
			}
			if (!(((SizeD)(ref val3)).Width > num) && !(((SizeD)(ref val3)).Height > num2))
			{
				break;
			}
			float size = val2.Size;
			if (size <= minFontSize)
			{
				break;
			}
			if (val != val2)
			{
				val2.Dispose();
			}
			val2 = (Font = StiFontUtils.ChangeFontSize(font, size - 0.5f));
		}
		font = val;
		return val2;
	}

	public StiText()
		: this(RectangleD.Empty, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiText(RectangleD rect)
		: this(rect, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiText(RectangleD rect, string text)
		: base(rect)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		SetTextInternal(text);
		PlaceOnToolbox = true;
		TextQuality = StiOptions.Engine.DefaultTextQualityMode;
	}
}
