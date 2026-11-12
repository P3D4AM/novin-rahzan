using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components.Chart;

public static class StiChartUserViewStatesHelper
{
	public static JObject SaveToJsonObject(StiChartElement chart)
	{
		JObject jObject = new JObject();
		StiJsonSaveMode mode = StiJsonSaveMode.Report;
		jObject.AddPropertyJObject("Values", StiJsonReportObjectHelper.Serialize.JObjectCollection(chart.Values, mode));
		jObject.AddPropertyJObject("EndValues", StiJsonReportObjectHelper.Serialize.JObjectCollection(chart.EndValues, mode));
		jObject.AddPropertyJObject("CloseValues", StiJsonReportObjectHelper.Serialize.JObjectCollection(chart.CloseValues, mode));
		jObject.AddPropertyJObject("LowValues", StiJsonReportObjectHelper.Serialize.JObjectCollection(chart.LowValues, mode));
		jObject.AddPropertyJObject("HighValues", StiJsonReportObjectHelper.Serialize.JObjectCollection(chart.HighValues, mode));
		jObject.AddPropertyJObject("Arguments", StiJsonReportObjectHelper.Serialize.JObjectCollection(chart.Arguments, mode));
		jObject.AddPropertyJObject("Weights", StiJsonReportObjectHelper.Serialize.JObjectCollection(chart.Weights, mode));
		if (chart.Series != null)
		{
			jObject.AddPropertyJObject("Series", chart.Series.SaveToJsonObject(mode));
		}
		if (chart.Icon.HasValue)
		{
			jObject.AddPropertyEnum("Icon", chart.Icon);
		}
		jObject.AddPropertyBool("RoundValues", chart.RoundValues, defaultValue: true);
		return jObject;
	}

	public static void LoadFromJsonObject(StiChartElement chart, JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Values":
				chart.Values.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "EndValues":
				chart.EndValues.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiEndValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "CloseValues":
				chart.CloseValues.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiCloseValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "LowValues":
				chart.LowValues.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiLowValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "HighValues":
				chart.HighValues.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiHighValueChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "Arguments":
				chart.Arguments.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiArgumentChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "Weights":
				chart.Weights.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiWeightChartMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "Series":
			{
				StiSeriesChartMeter stiSeriesChartMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiSeriesChartMeter;
				chart.Series = stiSeriesChartMeter ?? null;
				break;
			}
			case "Icon":
				chart.Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "RoundValues":
				chart.RoundValues = item.DeserializeBool();
				break;
			}
		}
	}

	public static void ClearVisualState(StiChartElement chart)
	{
		chart.Values.Clear();
		chart.EndValues.Clear();
		chart.CloseValues.Clear();
		chart.LowValues.Clear();
		chart.HighValues.Clear();
		chart.Arguments.Clear();
		chart.Weights.Clear();
		chart.Series = null;
		chart.Icon = null;
	}

	public static void SwitchSelectedViewState(StiChartElement chart, string newKey)
	{
		string text = chart.SelectedViewStateKey;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = chart.UserViewStates.FirstOrDefault()?.Key;
		}
		SaveVisualState(chart, text);
		LoadVisualState(chart, newKey);
		chart.SelectedViewStateKey = newKey;
	}

	public static void SaveVisualState(StiChartElement chart, string key)
	{
		if (!string.IsNullOrWhiteSpace(key))
		{
			StiUserViewState stiUserViewState = chart.UserViewStates.FirstOrDefault((StiUserViewState v) => v.Key == key);
			if (stiUserViewState != null)
			{
				string s = SaveToJsonObject(chart).ToString();
				stiUserViewState.State = StiPacker.PackToString(Encoding.UTF8.GetBytes(s));
			}
		}
	}

	public static void LoadVisualState(StiChartElement chart, string key)
	{
		ClearVisualState(chart);
		if (!string.IsNullOrWhiteSpace(key))
		{
			StiUserViewState stiUserViewState = chart.UserViewStates.FirstOrDefault((StiUserViewState v) => v.Key == key);
			if (!string.IsNullOrWhiteSpace(stiUserViewState?.State))
			{
				string @string = Encoding.UTF8.GetString(StiPacker.UnpackFromString(stiUserViewState.State));
				JObject jObject = JObject.Parse(@string);
				LoadFromJsonObject(chart, jObject);
			}
		}
	}
}
