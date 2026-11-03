using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiTwoColumnsPieLabels : StiOutsidePieLabels, IStiTwoColumnsPieLabels, IStiOutsidePieLabels, IStiCenterPieLabels, IStiPieSeriesLabels, IStiSeriesLabels, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiTwoColumnsPieLabels;

	[StiNonSerialized]
	[Browsable(false)]
	public override bool AutoRotate
	{
		get
		{
			return base.AutoRotate;
		}
		set
		{
			base.AutoRotate = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "AutoRotate");
		return val;
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.SeriesTwoColumnsPieLabels() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiSeriesLabels CreateNew()
	{
		return new StiTwoColumnsPieLabels();
	}

	public StiTwoColumnsPieLabels()
	{
		base.Core = new StiTwoColumnsPieLabelsCoreXF(this);
	}
}
