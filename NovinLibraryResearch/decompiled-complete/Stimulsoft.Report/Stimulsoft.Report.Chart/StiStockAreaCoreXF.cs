using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStockAreaCoreXF : StiCandlestickAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Stock");

	public override int Position => 120;

	public StiStockAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
