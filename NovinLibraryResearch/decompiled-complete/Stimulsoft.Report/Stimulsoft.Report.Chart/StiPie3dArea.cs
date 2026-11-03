using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Chart;

public class StiPie3dArea : StiPieArea
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPie3dArea;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "ColorEach");
		return val;
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiCenterPie3dLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[2]
		{
			typeof(StiNoneLabels),
			typeof(StiCenterPie3dLabels)
		};
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiPie3dSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiPie3dSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiPie3dArea();
	}

	[StiUniversalConstructor("Area")]
	public StiPie3dArea()
	{
		base.Core = new StiPie3dAreaCoreXF(this);
		ColorEach = true;
	}
}
