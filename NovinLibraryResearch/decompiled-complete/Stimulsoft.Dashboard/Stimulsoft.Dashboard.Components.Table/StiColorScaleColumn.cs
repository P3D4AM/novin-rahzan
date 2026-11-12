using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Table.Design;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Table;

[TypeConverter(typeof(StiColorScaleColumnConverter))]
public class StiColorScaleColumn : StiMeasureColumn, IStiColorScaleColumn
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ColorScaleColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Dashboard", "ColorScale");

	[StiSerializable]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Browsable(false)]
	public Color MinimumColor { get; set; } = Color.Red;


	[StiSerializable]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Browsable(false)]
	public Color MaximumColor { get; set; } = Color.Green;


	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && !ShouldSerializeMinimumColor())
			{
				return !ShouldSerializeMaximumColor();
			}
			return false;
		}
	}

	public override int GetUniqueCode()
	{
		int uniqueCode = base.GetUniqueCode();
		return (uniqueCode * 397) ^ (int)HorAlignment;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyColor("MinimumColor", MinimumColor, Color.Red);
		jObject.AddPropertyColor("MaximumColor", MaximumColor, Color.Green);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "MinimumColor"))
			{
				if (name == "MaximumColor")
				{
					MaximumColor = item.DeserializeColor();
				}
			}
			else
			{
				MinimumColor = item.DeserializeColor();
			}
		}
	}

	private bool ShouldSerializeMinimumColor()
	{
		return MinimumColor != Color.Red;
	}

	private bool ShouldSerializeMaximumColor()
	{
		return MinimumColor != Color.Green;
	}

	public StiColorScaleColumn()
	{
	}

	public StiColorScaleColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(expression, label, horAlignment, textFormat)
	{
	}

	public StiColorScaleColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression)
	{
	}

	public StiColorScaleColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiColorScaleColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiColorScaleColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiColorScaleColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment, Color minimumColor, Color maximumColor)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
		MinimumColor = minimumColor;
		MaximumColor = maximumColor;
	}
}
