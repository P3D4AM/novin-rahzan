using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

public class StiDoughnutArea : StiPieArea, IStiDoughnutArea, IStiPieArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[DefaultValue(true)]
	[StiSerializable]
	[Browsable(true)]
	public override bool ColorEach
	{
		get
		{
			return base.ColorEach;
		}
		set
		{
			base.ColorEach = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool ColorEachAllowed => false;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDoughnutArea;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "ColorEach");
		return val;
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiCenterPieLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[2]
		{
			typeof(StiNoneLabels),
			typeof(StiCenterPieLabels)
		};
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiDoughnutSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiDoughnutSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiDoughnutArea();
	}

	[StiUniversalConstructor("Area")]
	public StiDoughnutArea()
	{
		base.Core = new StiDoughnutAreaCoreXF(this);
		ColorEach = true;
	}
}
