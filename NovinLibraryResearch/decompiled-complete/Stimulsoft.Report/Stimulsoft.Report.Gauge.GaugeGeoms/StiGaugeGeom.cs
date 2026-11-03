using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public abstract class StiGaugeGeom
{
	public abstract StiGaugeGeomType Type { get; }

	public StiAnimation Animation { get; set; }
}
