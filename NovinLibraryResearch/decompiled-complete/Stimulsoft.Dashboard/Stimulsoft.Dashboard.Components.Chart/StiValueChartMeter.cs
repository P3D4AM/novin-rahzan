using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Chart.Design;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiValueChartMeterConverter))]
public class StiValueChartMeter : StiMeasureMeter, IStiValueMeter
{
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[DefaultValue(StiChartSeriesType.ClusteredColumn)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public virtual StiChartSeriesType SeriesType { get; set; } = StiChartSeriesType.ClusteredColumn;


	[DefaultValue(StiSeriesYAxis.LeftYAxis)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public virtual StiSeriesYAxis YAxis { get; set; }

	[DefaultValue(StiPenStyle.Solid)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public virtual StiPenStyle LineStyle { get; set; }

	[DefaultValue(2f)]
	[StiSerializable]
	[Description("Gets or sets line width of series.")]
	[Browsable(false)]
	public virtual float LineWidth { get; set; } = 2f;


	[StiSerializable]
	[DefaultValue(StiEmptyCellsAs.Zero)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public virtual StiEmptyCellsAs ShowZeros { get; set; } = StiEmptyCellsAs.Zero;


	[StiSerializable]
	[DefaultValue(StiEmptyCellsAs.Zero)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public virtual StiEmptyCellsAs ShowNulls { get; set; } = StiEmptyCellsAs.Zero;


	[Browsable(false)]
	public override StiMeterIdent Ident { get; } = StiMeterIdent.ValueChartMeter;


	[Browsable(false)]
	[JsonIgnore]
	public override string LocalizedName => Loc.Get("PropertyMain", "Value");

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("SeriesType", SeriesType, StiChartSeriesType.ClusteredColumn);
		jObject.AddPropertyEnum("YAxis", YAxis, StiSeriesYAxis.LeftYAxis);
		jObject.AddPropertyEnum("LineStyle", LineStyle, StiPenStyle.Solid);
		jObject.AddPropertyFloat("LineWidth", LineWidth, 2f);
		jObject.AddPropertyEnum("ShowZeros", ShowZeros, StiEmptyCellsAs.Zero);
		jObject.AddPropertyEnum("ShowNulls", ShowNulls, StiEmptyCellsAs.Zero);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "SeriesType":
				SeriesType = item.DeserializeEnum<StiChartSeriesType>();
				break;
			case "YAxis":
				YAxis = item.DeserializeEnum<StiSeriesYAxis>();
				break;
			case "LineStyle":
				LineStyle = item.DeserializeEnum<StiPenStyle>();
				break;
			case "LineWidth":
				LineWidth = item.DeserializeFloat();
				break;
			case "ShowZeros":
				ShowZeros = item.DeserializeEnum<StiEmptyCellsAs>();
				break;
			case "ShowNulls":
				ShowNulls = item.DeserializeEnum<StiEmptyCellsAs>();
				break;
			}
		}
	}

	public override int GetUniqueCode()
	{
		int uniqueCode = base.GetUniqueCode();
		uniqueCode = (uniqueCode * 397) ^ (int)SeriesType;
		uniqueCode = (uniqueCode * 397) ^ (int)YAxis;
		uniqueCode = (uniqueCode * 397) ^ (int)LineStyle;
		uniqueCode = (uniqueCode * 397) ^ LineWidth.GetHashCode();
		uniqueCode = (uniqueCode * 397) ^ (int)ShowNulls;
		return (uniqueCode * 397) ^ (int)ShowZeros;
	}

	public StiValueChartMeter()
	{
	}

	public StiValueChartMeter(string expression, string label, StiChartSeriesType seriesType)
		: base(expression, label)
	{
		SeriesType = seriesType;
	}

	public StiValueChartMeter(string key, string expression, string label, StiChartSeriesType seriesType)
		: base(key, expression, label)
	{
		SeriesType = seriesType;
	}

	public StiValueChartMeter(string key, string expression, string label, StiChartSeriesType seriesType, StiSeriesYAxis yAxis, StiPenStyle lineStyle, float lineWidth)
		: base(key, expression, label)
	{
		SeriesType = seriesType;
		YAxis = yAxis;
		LineStyle = lineStyle;
		LineWidth = lineWidth;
	}

	public StiValueChartMeter(string key, string expression, string label, StiChartSeriesType seriesType, StiSeriesYAxis yAxis, StiPenStyle lineStyle, float lineWidth, StiEmptyCellsAs showZeros, StiEmptyCellsAs showNulls)
		: base(key, expression, label)
	{
		SeriesType = seriesType;
		YAxis = yAxis;
		LineStyle = lineStyle;
		LineWidth = lineWidth;
		ShowZeros = showZeros;
		ShowNulls = showNulls;
	}
}
