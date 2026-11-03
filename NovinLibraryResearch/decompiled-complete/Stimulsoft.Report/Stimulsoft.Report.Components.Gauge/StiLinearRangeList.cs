using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Gauge;

public class StiLinearRangeList : StiScaleRangeList
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearRangeList;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiLinearRangeList();
	}

	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		for (int i = 0; i < base.Ranges.Count; i++)
		{
			if (base.Ranges[i] is StiLinearRange stiLinearRange)
			{
				stiLinearRange.DrawRange(context, base.Scale);
			}
		}
	}
}
