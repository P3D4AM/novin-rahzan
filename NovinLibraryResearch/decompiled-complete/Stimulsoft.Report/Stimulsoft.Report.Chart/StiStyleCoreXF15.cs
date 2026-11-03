using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStyleCoreXF15 : StiStyleCoreXF
{
	public override string LocalizedName => Loc.Get("Chart", "Style") + "15";

	public override Color[] StyleColors { get; } = new Color[6]
	{
		StiColor.Get("#0f6fc6"),
		StiColor.Get("#009dd9"),
		StiColor.Get("#0bd0d9"),
		StiColor.Get("#10cf9b"),
		StiColor.Get("#7cca62"),
		StiColor.Get("#a5c649")
	};


	public override Color BasicStyleColor => StiColor.Get("#dbf5f9");

	public override StiChartStyleId StyleId => StiChartStyleId.StiStyle15;
}
