using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Images;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiStyle : StiBaseStyle, IStiTextFormat, IStiFont
{
	private Font font = new Font("Arial", 8f);

	private float lineSpacing = 1f;

	public override StiComponentId ComponentId => StiComponentId.StiStyle;

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a horizontal alignment of the style.")]
	public virtual StiTextHorAlignment HorAlignment { get; set; }

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a vertical alignment of the style.")]
	public virtual StiVertAlignment VertAlignment { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets a font for drawing this style.")]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			if (font != value)
			{
				if (value == null)
				{
					value = new Font("Arial", 8f);
				}
				font = value;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets a brush to fill the style.")]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("The brush of the style, which is used to display text.")]
	public StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Black);


	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the format of the component.")]
	public StiFormatService TextFormat { get; set; } = new StiGeneralFormatService();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use HorAlignment formatting or not.")]
	public bool AllowUseHorAlignment { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use VertAlignment formatting or not.")]
	public bool AllowUseVertAlignment { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Image formatting or not.")]
	public bool AllowUseImage { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Font formatting or not.")]
	public bool AllowUseFont { get; set; } = true;


	[StiNonSerialized]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Border formatting or not.")]
	[Browsable(false)]
	[Obsolete("AllowUserBorder property is obsolete. Please use AllowUseBorderFormatting and AllowUseBorderSides properties instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool AllowUseBorder
	{
		get
		{
			return AllowUseBorderFormatting & AllowUseBorderSides;
		}
		set
		{
			AllowUseBorderFormatting = value;
			AllowUseBorderSides = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Border formatting or not.")]
	public bool AllowUseBorderFormatting { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Border Sides or not.")]
	public bool AllowUseBorderSides { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can set border sides of a component depending on the component location.")]
	public bool AllowUseBorderSidesFromLocation { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Brush formatting or not.")]
	public bool AllowUseBrush { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use TextBrush formatting or not.")]
	public bool AllowUseTextBrush { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use NegativeTextBrush formatting or not.")]
	public bool AllowUseNegativeTextBrush { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Text Format.")]
	public bool AllowUseTextFormat { get; set; }

	[Obsolete("Please do not use this property")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[StiNonSerialized]
	public bool AllowUseTextOptions
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(StiStyleCode.None)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates on which band in a report this style can be apply when applying style automatically. StyleCode property is obsolete. Do not use it!")]
	[Browsable(false)]
	[Obsolete("StyleCode property is obsolete. Do not use it!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiStyleCode StyleCode { get; set; }

	[Browsable(true)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an image to fill the Image property of the Image component.")]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiSimpeImageConverter))]
	public Image Image { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets a brush to draw the negative values.")]
	public StiBrush NegativeTextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Red);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[DefaultValue(1f)]
	[Description("Gets or sets line spacing of a text.")]
	public virtual float LineSpacing
	{
		get
		{
			return lineSpacing;
		}
		set
		{
			lineSpacing = Math.Max(0.2f, Math.Min(10f, value));
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)(object)HorAlignment, (Enum)(object)(StiTextHorAlignment)0);
		JsonExtensions.AddPropertyEnum(val, "VertAlignment", (Enum)(object)VertAlignment, (Enum)(object)(StiVertAlignment)0);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		JsonExtensions.AddPropertyBrush(val, "NegativeTextBrush", NegativeTextBrush);
		JsonExtensions.AddPropertyFloat(val, "LineSpacing", LineSpacing, 1f);
		if (!(TextFormat is StiGeneralFormatService))
		{
			JsonExtensions.AddPropertyJObject(val, "TextFormat", TextFormat.SaveToJsonObject(mode));
		}
		JsonExtensions.AddPropertyBool(val, "AllowUseHorAlignment", AllowUseHorAlignment, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseVertAlignment", AllowUseVertAlignment, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseImage", AllowUseImage, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseFont", AllowUseFont, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseBorderFormatting", AllowUseBorderFormatting, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseBorderSides", AllowUseBorderSides, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseBorderSidesFromLocation", AllowUseBorderSidesFromLocation, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseBrush", AllowUseBrush, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseTextBrush", AllowUseTextBrush, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseNegativeTextBrush", AllowUseNegativeTextBrush, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseTextFormat", AllowUseTextFormat, false, false);
		JsonExtensions.AddPropertyImage(val, "Image", Image);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "HorAlignment":
				HorAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiTextHorAlignment>(item);
				break;
			case "VertAlignment":
				VertAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiVertAlignment>(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
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
			case "NegativeTextBrush":
				NegativeTextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "LineSpacing":
				LineSpacing = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "AllowUseHorAlignment":
				AllowUseHorAlignment = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseVertAlignment":
				AllowUseVertAlignment = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseImage":
				AllowUseImage = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseFont":
				AllowUseFont = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseBorderFormatting":
				AllowUseBorderFormatting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseBorderSides":
				AllowUseBorderSides = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseBorderSidesFromLocation":
				AllowUseBorderSidesFromLocation = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseBrush":
				AllowUseBrush = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseTextBrush":
				AllowUseTextBrush = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseNegativeTextBrush":
				AllowUseNegativeTextBrush = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseTextFormat":
				AllowUseTextFormat = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Image":
				Image = StiJsonReportObjectHelper.DeserializeImage(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.StyleName(),
			propertiesHelper.Description(),
			propertiesHelper.StylesCollectionName(),
			propertiesHelper.StyleConditions()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		value = new StiPropertyObject[10]
		{
			propertiesHelper.Border(),
			propertiesHelper.Brush(),
			propertiesHelper.Font(),
			propertiesHelper.HorAlignment(),
			propertiesHelper.SimpleImage(),
			propertiesHelper.LineSpacing(),
			propertiesHelper.NegativeTextBrush(),
			propertiesHelper.TextBrush(),
			propertiesHelper.TextFormat(),
			propertiesHelper.VertAlignment()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[12]
		{
			propertiesHelper.AllowUseBorderFormatting(),
			propertiesHelper.AllowUseBorderSides(),
			propertiesHelper.AllowUseBorderSidesFromLocation(),
			propertiesHelper.AllowUseBrush(),
			propertiesHelper.AllowUseFont(),
			propertiesHelper.AllowUseImage(),
			propertiesHelper.AllowUseNegativeTextBrush(),
			propertiesHelper.AllowUseTextBrush(),
			propertiesHelper.AllowUseTextFormat(),
			propertiesHelper.AllowUseTextOptions(),
			propertiesHelper.AllowUseHorAlignment(),
			propertiesHelper.AllowUseVertAlignment()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Parameters, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		StiStyle stiStyle = base.Clone() as StiStyle;
		object obj = Font.Clone();
		stiStyle.Font = (Font)((obj is Font) ? obj : null);
		object obj2 = Border.Clone();
		stiStyle.Border = (StiBorder)((obj2 is StiBorder) ? obj2 : null);
		object obj3 = Brush.Clone();
		stiStyle.Brush = (StiBrush)((obj3 is StiBrush) ? obj3 : null);
		object obj4 = TextBrush.Clone();
		stiStyle.TextBrush = (StiBrush)((obj4 is StiBrush) ? obj4 : null);
		stiStyle.TextFormat = ((StiService)TextFormat).Clone() as StiFormatService;
		object obj5 = NegativeTextBrush.Clone();
		stiStyle.NegativeTextBrush = (StiBrush)((obj5 is StiBrush) ? obj5 : null);
		return stiStyle;
	}

	private bool ShouldSerializeHorAlignment()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		return (int)HorAlignment > 0;
	}

	private bool ShouldSerializeVertAlignment()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		return (int)VertAlignment > 0;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.Size == 8f)
		{
			return (int)Font.Style > 0;
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

	private bool ShouldSerializeTextFormat()
	{
		return !(TextFormat is StiGeneralFormatService);
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Invalid comparison between Unknown and I4
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Invalid comparison between Unknown and I4
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = rect;
		if (AllowUseBrush)
		{
			Brush brush = StiBrush.GetBrush(Brush, rect);
			try
			{
				g.FillRectangle(brush, rect);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		else
		{
			rect.Width++;
			rect.Height++;
			g.FillRectangle(Brushes.White, rect);
			rect.Width--;
			rect.Height--;
		}
		if (!paintValue)
		{
			if (paintImage)
			{
				DrawStyleImage(g, rect);
				if (rect.Height < StiScaleUI.I(32))
				{
					rect.Width -= StiScaleUI.I(24);
					rect.X += StiScaleUI.I(24);
				}
				else
				{
					rect.Width -= StiScaleUI.I(56);
					rect.X += StiScaleUI.I(56);
				}
			}
			StringFormat val = new StringFormat();
			try
			{
				Brush brush2 = StiBrush.GetBrush(TextBrush, rect);
				try
				{
					val.Trimming = (StringTrimming)3;
					val.Alignment = (StringAlignment)0;
					val.LineAlignment = (StringAlignment)1;
					val.FormatFlags = (StringFormatFlags)4096;
					val.Trimming = (StringTrimming)0;
					if (AllowUseHorAlignment)
					{
						StiTextHorAlignment horAlignment = HorAlignment;
						if ((int)horAlignment != 0)
						{
							if ((int)horAlignment == 2)
							{
								val.Alignment = (StringAlignment)2;
							}
							else
							{
								val.Alignment = (StringAlignment)1;
							}
						}
						else
						{
							val.Alignment = (StringAlignment)0;
						}
					}
					if (AllowUseVertAlignment)
					{
						StiVertAlignment vertAlignment = VertAlignment;
						if ((int)vertAlignment != 0)
						{
							if ((int)vertAlignment == 2)
							{
								val.LineAlignment = (StringAlignment)2;
							}
							else
							{
								val.LineAlignment = (StringAlignment)1;
							}
						}
						else
						{
							val.LineAlignment = (StringAlignment)0;
						}
					}
					Brush val2 = brush2;
					if (!AllowUseTextBrush)
					{
						val2 = Brushes.Black;
					}
					SmoothingMode smoothingMode = g.SmoothingMode;
					g.SmoothingMode = (SmoothingMode)4;
					string text = Name;
					if (!string.IsNullOrEmpty(base.Description) && base.Description != Name)
					{
						text = base.Description;
					}
					double num = ((!StiBaseStyle.WindowsScaleInternal.HasValue) ? 1.0 : StiBaseStyle.WindowsScaleInternal.GetValueOrDefault());
					if (AllowUseFont)
					{
						Font val3 = new Font(Font.Name, (float)((double)Font.Size * num), Font.Style);
						try
						{
							Rectangle rectangle2 = rect;
							rectangle2.Inflate(-StiScaleUI.I2, 0);
							SizeF sizeF = g.MeasureString(text, val3);
							g.DrawString(text, val3, val2, (RectangleF)rectangle2, val);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					else
					{
						Font val4 = new Font("Arial", (float)(8.0 * num));
						try
						{
							g.DrawString(text, val4, val2, (RectangleF)rect, val);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					g.SmoothingMode = smoothingMode;
				}
				finally
				{
					((IDisposable)brush2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if ((Border != null && AllowUseBorderFormatting) || AllowUseBorderSides)
		{
			Border.Draw(g, (RectangleF)rectangle, 1f, Color.White, AllowUseBorderFormatting, AllowUseBorderSides);
		}
	}

	public override void DrawStyleImage(Graphics g, Rectangle rect)
	{
		Image val = StiReportImages.Styles.Style((StiImageSize)0);
		Rectangle rectangle = new Rectangle(rect.X + (StiScaleUI.I(24) - val.Width) / 2, rect.Y + (rect.Height - val.Width) / 2, val.Width, val.Height);
		g.DrawImage(val, rectangle);
	}

	public override void DrawBox(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		rect.X++;
		rect.Y++;
		rect.Width -= 2;
		rect.Height -= 3;
		DrawStyle(g, rect, paintValue, paintImage);
		if (Border != null && (int)Border.Side == 0 && !paintValue)
		{
			Pen val = new Pen(Color.FromArgb(150, Color.Gray));
			try
			{
				val.DashStyle = (DashStyle)1;
				g.DrawRectangle(val, rect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (rect.Width < 20)
		{
			rect.X += 4;
			rect.Y += 3;
			rect.Width -= 7;
			rect.Height -= 5;
			Brush brush = StiBrush.GetBrush(TextBrush, rect);
			try
			{
				g.FillRectangle(brush, rect);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
	}

	public override void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements)
	{
		GetStyleFromComponent(component, styleElements, null);
	}

	internal void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements, StiBaseStyle componentStyle)
	{
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected I4, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		StiStyle stiStyle = componentStyle as StiStyle;
		AllowUseBorderFormatting = false;
		AllowUseBorderSides = false;
		AllowUseBorderSidesFromLocation = false;
		AllowUseBrush = false;
		AllowUseTextBrush = false;
		AllowUseFont = false;
		AllowUseHorAlignment = false;
		AllowUseVertAlignment = false;
		AllowUseTextFormat = false;
		bool flag = !StiOptions.Engine.UseParentStylesOldMode;
		if (stiStyle == null)
		{
			flag = false;
		}
		if (component is StiText)
		{
			StiText stiText = component as StiText;
			LineSpacing = stiText.LineSpacing;
		}
		else if (flag)
		{
			LineSpacing = stiStyle.LineSpacing;
		}
		if ((styleElements & StiStyleElements.Font) > (StiStyleElements)0)
		{
			if (component is IStiFont)
			{
				IStiFont stiFont = component as IStiFont;
				object obj = stiFont.Font.Clone();
				Font = (Font)((obj is Font) ? obj : null);
				AllowUseFont = true;
			}
			else if (flag && stiStyle.AllowUseFont)
			{
				object obj2 = stiStyle.Font.Clone();
				Font = (Font)((obj2 is Font) ? obj2 : null);
				AllowUseFont = true;
			}
		}
		if ((styleElements & StiStyleElements.Border) > (StiStyleElements)0)
		{
			if (component is IStiBorder)
			{
				IStiBorder stiBorder = component as IStiBorder;
				object obj3 = stiBorder.Border.Clone();
				Border = (StiBorder)((obj3 is StiBorder) ? obj3 : null);
				AllowUseBorderFormatting = true;
				AllowUseBorderSides = true;
			}
			else if (flag && (stiStyle.AllowUseBorderSides || stiStyle.AllowUseBorderFormatting))
			{
				object obj4 = stiStyle.Border.Clone();
				Border = (StiBorder)((obj4 is StiBorder) ? obj4 : null);
				AllowUseBorderFormatting = true;
				AllowUseBorderSides = true;
			}
		}
		if ((styleElements & StiStyleElements.Brush) > (StiStyleElements)0)
		{
			if (component is IStiBrush)
			{
				IStiBrush stiBrush = component as IStiBrush;
				object obj5 = stiBrush.Brush.Clone();
				Brush = (StiBrush)((obj5 is StiBrush) ? obj5 : null);
				AllowUseBrush = true;
			}
			else if (flag && stiStyle.AllowUseBrush)
			{
				object obj6 = stiStyle.Brush.Clone();
				Brush = (StiBrush)((obj6 is StiBrush) ? obj6 : null);
				AllowUseBrush = true;
			}
		}
		if ((styleElements & StiStyleElements.TextBrush) > (StiStyleElements)0)
		{
			if (component is IStiTextBrush)
			{
				IStiTextBrush stiTextBrush = component as IStiTextBrush;
				object obj7 = stiTextBrush.TextBrush.Clone();
				TextBrush = (StiBrush)((obj7 is StiBrush) ? obj7 : null);
				AllowUseTextBrush = true;
			}
			else if (flag && stiStyle.AllowUseTextBrush)
			{
				object obj8 = stiStyle.TextBrush.Clone();
				TextBrush = (StiBrush)((obj8 is StiBrush) ? obj8 : null);
				AllowUseTextBrush = true;
			}
		}
		if (component is IStiBackColor && (styleElements & StiStyleElements.Brush) > (StiStyleElements)0)
		{
			IStiBackColor stiBackColor = component as IStiBackColor;
			Brush = (StiBrush)new StiSolidBrush(stiBackColor.BackColor);
			AllowUseBrush = true;
		}
		if (component is IStiForeColor && (styleElements & StiStyleElements.TextBrush) > (StiStyleElements)0)
		{
			IStiForeColor stiForeColor = component as IStiForeColor;
			TextBrush = (StiBrush)new StiSolidBrush(stiForeColor.ForeColor);
			AllowUseTextBrush = true;
		}
		if ((styleElements & StiStyleElements.HorAlignment) > (StiStyleElements)0)
		{
			if (component is IStiTextHorAlignment)
			{
				IStiTextHorAlignment stiTextHorAlignment = component as IStiTextHorAlignment;
				HorAlignment = stiTextHorAlignment.HorAlignment;
				AllowUseHorAlignment = true;
			}
			else if (flag && stiStyle.AllowUseHorAlignment)
			{
				HorAlignment = stiStyle.HorAlignment;
				AllowUseHorAlignment = true;
			}
		}
		if (component is IStiHorAlignment && (styleElements & StiStyleElements.HorAlignment) > (StiStyleElements)0)
		{
			IStiHorAlignment stiHorAlignment = component as IStiHorAlignment;
			StiHorAlignment horAlignment = stiHorAlignment.HorAlignment;
			switch (horAlignment - 1)
			{
			case 1:
				HorAlignment = (StiTextHorAlignment)1;
				break;
			case 0:
				HorAlignment = (StiTextHorAlignment)0;
				break;
			case 2:
				HorAlignment = (StiTextHorAlignment)2;
				break;
			}
			AllowUseHorAlignment = true;
		}
		if ((styleElements & StiStyleElements.VertAlignment) > (StiStyleElements)0)
		{
			if (component is IStiVertAlignment)
			{
				IStiVertAlignment stiVertAlignment = component as IStiVertAlignment;
				VertAlignment = stiVertAlignment.VertAlignment;
				AllowUseVertAlignment = true;
			}
			else if (flag && stiStyle.AllowUseVertAlignment)
			{
				VertAlignment = stiStyle.VertAlignment;
				AllowUseVertAlignment = true;
			}
		}
		if (component is StiLinePrimitive stiLinePrimitive)
		{
			Border = new StiBorder(Border.Side, stiLinePrimitive.Color, (double)stiLinePrimitive.Size, stiLinePrimitive.Style);
			AllowUseBorderFormatting = true;
			AllowUseBorderSides = true;
		}
	}

	public override void SetStyleToComponent(StiComponent component)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected I4, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		if (component is StiChart || component is StiCrossTab)
		{
			return;
		}
		StiLinePrimitive stiLinePrimitive = component as StiLinePrimitive;
		StiShape stiShape = component as StiShape;
		if (component is StiText)
		{
			StiText stiText = component as StiText;
			if (stiText.LineSpacing != LineSpacing)
			{
				_003F val = stiText;
				object obj = ((StiService)stiText).Properties.Clone();
				((StiService)val).Properties = (StiRepositoryItems)((obj is StiRepositoryItems) ? obj : null);
				stiText.LineSpacing = LineSpacing;
			}
		}
		if (component is IStiFont && AllowUseFont)
		{
			IStiFont stiFont = component as IStiFont;
			object obj2 = Font.Clone();
			stiFont.Font = (Font)((obj2 is Font) ? obj2 : null);
		}
		if (component is IStiBorder && (AllowUseBorderFormatting || AllowUseBorderSides) && stiLinePrimitive == null)
		{
			IStiBorder stiBorder = component as IStiBorder;
			StiBorderSides side = stiBorder.Border.Side;
			if (AllowUseBorderFormatting)
			{
				object obj3 = Border.Clone();
				stiBorder.Border = (StiBorder)((obj3 is StiBorder) ? obj3 : null);
				stiBorder.Border.Side = side;
			}
			if (AllowUseBorderSides)
			{
				if (AllowUseBorderSidesFromLocation && component.Parent != null)
				{
					stiBorder.Border.Side = StiStylesHelper.GetBorderSidesFromLocation(component);
				}
				else
				{
					stiBorder.Border.Side = Border.Side;
				}
			}
		}
		if (component is IStiBrush && AllowUseBrush)
		{
			IStiBrush stiBrush = component as IStiBrush;
			object obj4 = Brush.Clone();
			stiBrush.Brush = (StiBrush)((obj4 is StiBrush) ? obj4 : null);
		}
		if (component is IStiTextBrush && AllowUseTextBrush)
		{
			IStiTextBrush stiTextBrush = component as IStiTextBrush;
			object obj5 = TextBrush.Clone();
			stiTextBrush.TextBrush = (StiBrush)((obj5 is StiBrush) ? obj5 : null);
		}
		if (component is IStiBackColor && AllowUseBrush)
		{
			IStiBackColor stiBackColor = component as IStiBackColor;
			stiBackColor.BackColor = StiBrush.ToColor(Brush);
		}
		if (component is IStiForeColor && AllowUseTextBrush)
		{
			IStiForeColor stiForeColor = component as IStiForeColor;
			stiForeColor.ForeColor = StiBrush.ToColor(TextBrush);
		}
		if (component is IStiTextHorAlignment && AllowUseHorAlignment)
		{
			IStiTextHorAlignment stiTextHorAlignment = component as IStiTextHorAlignment;
			stiTextHorAlignment.HorAlignment = HorAlignment;
		}
		if (component is IStiHorAlignment && AllowUseHorAlignment)
		{
			IStiHorAlignment stiHorAlignment = component as IStiHorAlignment;
			StiTextHorAlignment horAlignment = HorAlignment;
			switch ((int)horAlignment)
			{
			case 1:
				stiHorAlignment.HorAlignment = (StiHorAlignment)2;
				break;
			case 0:
				stiHorAlignment.HorAlignment = (StiHorAlignment)1;
				break;
			case 2:
				stiHorAlignment.HorAlignment = (StiHorAlignment)3;
				break;
			}
		}
		if (component is IStiVertAlignment && AllowUseVertAlignment)
		{
			IStiVertAlignment stiVertAlignment = component as IStiVertAlignment;
			stiVertAlignment.VertAlignment = VertAlignment;
		}
		if (component is IStiTextFormat && AllowUseTextFormat)
		{
			IStiTextFormat stiTextFormat = component as IStiTextFormat;
			stiTextFormat.TextFormat = ((StiService)TextFormat).Clone() as StiFormatService;
		}
		if (component is StiImage && AllowUseImage)
		{
			StiImage stiImage = component as StiImage;
			stiImage.PutImage(Image);
		}
		if (stiLinePrimitive != null && AllowUseBorderFormatting)
		{
			stiLinePrimitive.Color = Border.Color;
			stiLinePrimitive.Size = (float)Border.Size;
			stiLinePrimitive.Style = Border.Style;
		}
		if (stiShape != null && AllowUseBorderFormatting)
		{
			stiShape.BorderColor = Border.Color;
			stiShape.Size = (float)Border.Size;
			stiShape.Style = Border.Style;
		}
	}

	public override void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiStyle");
	}

	private bool ShouldSerializeImage()
	{
		return Image != null;
	}

	private bool ShouldSerializeNegativeTextBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (NegativeTextBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)NegativeTextBrush).Color == Color.Red);
		}
		return true;
	}

	internal StiStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
	}//IL_000b: Unknown result type (might be due to invalid IL or missing references)
	//IL_0015: Expected O, but got Unknown
	//IL_0016: Unknown result type (might be due to invalid IL or missing references)
	//IL_0020: Expected O, but got Unknown
	//IL_0026: Unknown result type (might be due to invalid IL or missing references)
	//IL_0030: Expected O, but got Unknown
	//IL_0036: Unknown result type (might be due to invalid IL or missing references)
	//IL_0040: Expected O, but got Unknown
	//IL_0074: Unknown result type (might be due to invalid IL or missing references)
	//IL_007e: Expected O, but got Unknown


	public StiStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiStyle(string name)
		: this(name, "")
	{
	}

	public StiStyle()
		: this("")
	{
	}
}
