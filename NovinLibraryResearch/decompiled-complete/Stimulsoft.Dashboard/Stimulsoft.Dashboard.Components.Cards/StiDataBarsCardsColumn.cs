using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Dashboard.Components.Cards;

public class StiDataBarsCardsColumn : StiMeasureCardsColumn, IStiDataBarsColumn
{
	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.DataBarsCardsColumn;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("Dashboard", "DataBars");

	public StiDataBarsCardsColumn()
	{
	}

	public StiDataBarsCardsColumn(string key, string expression, string label, StiHorAlignment horAlignment, StiFormatService textFormat, StiCardsColumnVisibility visibility, string visibilityExpression, Color foreColor)
		: base(key, expression, label, horAlignment, textFormat, visibility, visibilityExpression, foreColor)
	{
	}
}
