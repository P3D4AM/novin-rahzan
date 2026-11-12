using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Cards;

public class StiMeasureCardsColumn : StiCardsColumn, IStiMeasureMeter, IStiMeter, IStiMeasureColumn, IStiMeasureCardsColumn
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.MeasureCardsColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Dashboard", "Measure");

	public StiMeasureCardsColumn()
	{
	}

	public StiMeasureCardsColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiCardsColumnVisibility visibility, string visibilityExpression, Color foreColor)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor)
	{
	}
}
