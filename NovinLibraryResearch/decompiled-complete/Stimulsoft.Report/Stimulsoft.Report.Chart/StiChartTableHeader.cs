using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartTableHeader : IStiChartTableHeader, ICloneable, IStiJsonReportObject, IStiDefault, IStiFont
{
	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (TextAfter != null && TextAfter.Length == 0 && !ShouldSerializeBrush() && !ShouldSerializeFont() && !ShouldSerializeTextColor() && string.IsNullOrEmpty(Format))
			{
				return !WordWrap;
			}
			return false;
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets string which will be output after text.")]
	public string TextAfter { get; set; } = "";


	[DefaultValue("")]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Chart.Design.StiFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets os sets format string which is used for formating table header.")]
	public string Format { get; set; } = "";


	[StiOrder(150)]
	[StiSerializable]
	[Description("Gets or sets brush which will be used to fill chart table header.")]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable]
	[StiOrder(160)]
	[Description("Gets or sets font which will be used to draw chart table header.")]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets text color.")]
	public Color TextColor { get; set; } = Color.DarkGray;


	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	public bool WordWrap { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextAfter", TextAfter);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		JsonExtensions.AddPropertyColor(val, "TextColor", TextColor, Color.DarkGray);
		JsonExtensions.AddPropertyBool(val, "WordWrap", WordWrap, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "TextAfter":
				TextAfter = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "TextColor":
				TextColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "WordWrap":
				WordWrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Format":
				Format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public object Clone()
	{
		IStiChartTableHeader stiChartTableHeader = MemberwiseClone() as IStiChartTableHeader;
		object obj = Brush.Clone();
		stiChartTableHeader.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		object obj2 = Font.Clone();
		stiChartTableHeader.Font = (Font)((obj2 is Font) ? obj2 : null);
		return stiChartTableHeader;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.White);
		}
		return true;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 8f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeTextColor()
	{
		return TextColor != Color.DarkGray;
	}

	public StiChartTableHeader()
	{
	}//IL_001c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0026: Expected O, but got Unknown
	//IL_0031: Unknown result type (might be due to invalid IL or missing references)
	//IL_003b: Expected O, but got Unknown


	public StiChartTableHeader(StiBrush brush, Font font, Color textColor, bool wordWrap)
		: this(string.Empty, brush, font, textColor, wordWrap)
	{
	}

	public StiChartTableHeader(string textAfter, StiBrush brush, Font font, Color textColor, bool wordWrap)
		: this(string.Empty, brush, font, textColor, wordWrap, string.Empty)
	{
	}

	[StiUniversalConstructor("Header")]
	public StiChartTableHeader(string textAfter, StiBrush brush, Font font, Color textColor, bool wordWrap, string format)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		TextAfter = textAfter;
		Brush = brush;
		Font = font;
		TextColor = textColor;
		WordWrap = wordWrap;
		Format = format;
	}
}
