using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

public class StiWaterfallSeries : StiClusteredColumnSeries, IStiWaterfallSeries, IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries
{
	public override StiComponentId ComponentId => StiComponentId.StiWaterfallSeries;

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public IStiWaterfallConnectorLine ConnectorLine { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public IStiWaterfallTotal Total { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "ConnectorLine", ((IStiJsonReportObject)ConnectorLine).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Total", ((IStiJsonReportObject)Total).SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "ConnectorLine"))
			{
				if (name == "Total")
				{
					((IStiJsonReportObject)Total).LoadFromJsonObject((JObject)item.Value);
				}
			}
			else
			{
				((IStiJsonReportObject)ConnectorLine).LoadFromJsonObject((JObject)item.Value);
			}
		}
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiWaterfallArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiWaterfallSeries();
	}

	public StiWaterfallSeries()
	{
		base.Core = new StiWaterfallSeriesCoreXF(this);
		ConnectorLine = new StiWaterfallConnectorLine();
		Total = new StiWaterfallTotal();
	}
}
