using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiPieArea : StiArea, IStiPieArea, IStiArea, ICloneable, IStiJsonReportObject
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPieArea;

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
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.PieArea() });
		return stiPropertyCollection;
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiCenterPieLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[5]
		{
			typeof(StiNoneLabels),
			typeof(StiInsideEndPieLabels),
			typeof(StiCenterPieLabels),
			typeof(StiOutsidePieLabels),
			typeof(StiTwoColumnsPieLabels)
		};
	}

	public override Type GetDefaultSeriesType()
	{
		return typeof(StiPieSeries);
	}

	public override Type[] GetSeriesTypes()
	{
		return new Type[1] { typeof(StiPieSeries) };
	}

	public override StiArea CreateNew()
	{
		return new StiPieArea();
	}

	[StiUniversalConstructor("Area")]
	public StiPieArea()
	{
		base.Core = new StiPieAreaCoreXF(this);
		ColorEach = true;
	}
}
