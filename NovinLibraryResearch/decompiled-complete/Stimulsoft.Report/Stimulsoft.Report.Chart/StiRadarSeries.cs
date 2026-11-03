using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Chart;

public abstract class StiRadarSeries : StiSeries, IStiRadarSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries
{
	private bool showNulls = true;

	private IStiMarker marker = new StiMarker();

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

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[Description("Gets or sets value which indicates whether it is necessary to show the series element, if the series value of this bar is null.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ShowNulls
	{
		get
		{
			return showNulls;
		}
		set
		{
			showNulls = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets marker settings.")]
	[Browsable(false)]
	[TypeConverter(typeof(StiLineMarkerConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual IStiMarker Marker
	{
		get
		{
			return marker;
		}
		set
		{
			marker = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TrendLine");
		JsonExtensions.AddPropertyBool(val, "ShowNulls", showNulls, true, false);
		JsonExtensions.AddPropertyJObject(val, "Marker", ((IStiJsonReportObject)Marker).SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "ShowNulls"))
			{
				if (name == "Marker")
				{
					((IStiJsonReportObject)Marker).LoadFromJsonObject((JObject)item.Value);
				}
			}
			else
			{
				showNulls = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override object Clone()
	{
		IStiRadarSeries stiRadarSeries = base.Clone() as IStiRadarSeries;
		if (marker != null)
		{
			stiRadarSeries.Marker = marker.Clone() as IStiMarker;
		}
		return stiRadarSeries;
	}
}
