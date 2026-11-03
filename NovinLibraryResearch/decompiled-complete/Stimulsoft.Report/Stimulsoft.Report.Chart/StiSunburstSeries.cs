using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiSunburstSeries : StiSeries, IStiSunburstSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness
{
	public override StiComponentId ComponentId => StiComponentId.StiSunburstSeries;

	[Browsable(false)]
	[StiNonSerialized]
	public override StiTrendLinesCollection TrendLines
	{
		get
		{
			return base.TrendLines;
		}
		set
		{
			base.TrendLines = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool TrendLineAllowed => false;

	[StiNonSerialized]
	[Browsable(false)]
	public override StiSeriesYAxis YAxis
	{
		get
		{
			return base.YAxis;
		}
		set
		{
			base.YAxis = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border thickness of series element.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int BorderThickness { get; set; } = 1;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TrendLine");
		JsonExtensions.AddPropertyInt(val, "BorderThickness", BorderThickness, 1);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "BorderThickness")
			{
				BorderThickness = StiJsonReportObjectHelper.DeserializeInt(item);
			}
		}
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiSunburstArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiSunburstSeries();
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return new StiPropertyCollection();
	}

	public StiSunburstSeries()
	{
		base.Core = new StiSunburstSeriesCoreXF(this);
	}
}
