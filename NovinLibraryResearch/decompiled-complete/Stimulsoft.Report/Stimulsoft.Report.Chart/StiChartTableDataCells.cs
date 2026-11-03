using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartTableDataCells : IStiChartTableDataCells, ICloneable, IStiJsonReportObject, IStiDefault, IStiFont
{
	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (!ShrinkFontToFit && ShrinkFontToFitMinimumSize == 1f && !ShouldSerializeFont())
			{
				return !ShouldSerializeTextColor();
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsDefaultExceptTextColor
	{
		get
		{
			if (!ShrinkFontToFit && ShrinkFontToFitMinimumSize == 1f)
			{
				return !ShouldSerializeFont();
			}
			return false;
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	public bool ShrinkFontToFit { get; set; }

	[StiSerializable]
	[DefaultValue(1f)]
	[Description("Gets or sets value that indicates minimum font size for ShrinkFontToFit operation.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float ShrinkFontToFitMinimumSize { get; set; } = 1f;


	[StiSerializable]
	[StiOrder(160)]
	[Description("Gets or sets font which will be used to draw chart table cells.")]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets text color.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public Color TextColor { get; set; } = Color.DarkGray;


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyColor(val, "TextColor", TextColor, Color.DarkGray);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		JsonExtensions.AddPropertyFloat(val, "ShrinkFontToFitMinimumSize", ShrinkFontToFitMinimumSize, 1f);
		JsonExtensions.AddPropertyBool(val, "ShrinkFontToFit", ShrinkFontToFit, false, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "TextColor":
				TextColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ShrinkFontToFitMinimumSize":
				ShrinkFontToFitMinimumSize = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "ShrinkFontToFit":
				ShrinkFontToFit = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public object Clone()
	{
		IStiChartTableDataCells stiChartTableDataCells = MemberwiseClone() as IStiChartTableDataCells;
		object obj = Font.Clone();
		stiChartTableDataCells.Font = (Font)((obj is Font) ? obj : null);
		return stiChartTableDataCells;
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

	public StiChartTableDataCells()
	{
	}//IL_0016: Unknown result type (might be due to invalid IL or missing references)
	//IL_0020: Expected O, but got Unknown


	[StiUniversalConstructor("DataCells")]
	public StiChartTableDataCells(bool shrinkFontToFit, float shrinkFontToFitMinimumSize, Font font, Color textColor)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Font = font;
		ShrinkFontToFit = shrinkFontToFit;
		ShrinkFontToFitMinimumSize = shrinkFontToFitMinimumSize;
		TextColor = textColor;
	}
}
