using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Table.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Table;

[TypeConverter(typeof(StiDimensionColumnConverter))]
public class StiDimensionColumn : StiTableColumn, IStiDimensionMeter, IStiMeter, IStiDimensionColumn, IStiMeterRules, IStiTableColumnSize
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.DimensionColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Dashboard", "Dimension");

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable]
	public bool ShowHyperlink { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public string HyperlinkPattern { get; set; } = "";


	[Description("Gets or sets settings of the legend.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	public StiTableColumnSize Size { get; set; } = new StiTableColumnSize();


	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && !ShowHyperlink && string.IsNullOrEmpty(HyperlinkPattern) && Size != null)
			{
				return Size.IsDefault;
			}
			return false;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyBool("ShowHyperlink", ShowHyperlink);
		jObject.AddPropertyStringNullOrEmpty("HyperlinkPattern", HyperlinkPattern);
		if (Size != null)
		{
			jObject.AddPropertyJObject("Size", Size.SaveToJsonObject(mode));
		}
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowHyperlink":
				ShowHyperlink = item.DeserializeBool();
				break;
			case "HyperlinkPattern":
				HyperlinkPattern = item.DeserializeString();
				break;
			case "Size":
				Size.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override object Clone()
	{
		StiDimensionColumn stiDimensionColumn = base.Clone() as StiDimensionColumn;
		stiDimensionColumn.Size = Size?.Clone() as StiTableColumnSize;
		return stiDimensionColumn;
	}

	public void CheckRules()
	{
		Size?.CheckRules();
	}

	private bool ShouldSerializeSize()
	{
		if (Size != null)
		{
			return !Size.IsDefault;
		}
		return true;
	}

	public StiDimensionColumn()
	{
	}

	public StiDimensionColumn(string expression)
	{
		base.Expression = expression;
	}

	public StiDimensionColumn(string expression, string label)
	{
		base.Expression = expression;
		base.Label = label;
	}

	public StiDimensionColumn(string key, string expression, string label)
		: base(key, expression, label)
	{
	}

	public StiDimensionColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat)
		: base(expression, label, horAlignment, textFormat)
	{
	}

	public StiDimensionColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression)
	{
	}

	public StiDimensionColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiDimensionColumn(string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiDimensionColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
	}

	public StiDimensionColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showHyperlink, string hyperlinkPattern, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
		ShowHyperlink = showHyperlink;
		HyperlinkPattern = hyperlinkPattern;
	}

	public StiDimensionColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiTableColumnVisibility visibility, string visibilityExpression, Color foreColor, bool showHyperlink, string hyperlinkPattern, bool showTotalSummary, StiSummaryColumnType summaryType, StiHorAlignment summaryAlignment, StiTableColumnSize size)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor, showTotalSummary, summaryType, summaryAlignment)
	{
		ShowHyperlink = showHyperlink;
		HyperlinkPattern = hyperlinkPattern;
		Size = size;
	}
}
