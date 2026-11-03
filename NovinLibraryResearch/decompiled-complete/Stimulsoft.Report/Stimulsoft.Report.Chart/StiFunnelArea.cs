using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiFunnelArea : StiArea, IStiFunnelArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFunnelArea;

	[DefaultValue(true)]
	[StiNonSerialized]
	[Browsable(false)]
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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "ColorEach");
		return val;
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.FunnelArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiFunnelSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiFunnelSeries) };
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiNoneLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[4]
		{
			typeof(StiNoneLabels),
			typeof(StiCenterFunnelLabels),
			typeof(StiOutsideRightFunnelLabels),
			typeof(StiOutsideLeftFunnelLabels)
		};
	}

	public override StiArea CreateNew()
	{
		return new StiFunnelArea();
	}

	[StiUniversalConstructor("Area")]
	public StiFunnelArea()
	{
		base.Core = new StiFunnelAreaCoreXF(this);
		ColorEach = true;
	}
}
