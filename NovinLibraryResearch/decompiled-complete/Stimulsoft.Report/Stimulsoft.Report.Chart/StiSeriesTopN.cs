using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiSeriesTopNConverter))]
public class StiSeriesTopN : IStiSeriesTopN, ICloneable, IStiJsonReportObject, IStiDefault, IStiSerializeToCodeAsClass, IStiPropertyGridObject
{
	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiSeriesTopN;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (Mode == StiTopNMode.None && Count == 5 && ShowOthers)
			{
				return OthersText == "Others";
			}
			return false;
		}
	}

	[DefaultValue(StiTopNMode.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets output values mode.")]
	public StiTopNMode Mode { get; set; }

	[DefaultValue(5)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the number of output values.")]
	public int Count { get; set; } = 5;


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether to display other values.")]
	public bool ShowOthers { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("Others")]
	[Description("Gets or sets signature for other values.")]
	public string OthersText { get; set; } = "Others";


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyEnum(val, "Mode", (Enum)Mode, (Enum)StiTopNMode.None);
		JsonExtensions.AddPropertyInt(val, "Count", Count, 5);
		JsonExtensions.AddPropertyBool(val, "ShowOthers", ShowOthers, true, false);
		JsonExtensions.AddPropertyString(val, "OthersText", OthersText, "Others");
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Mode":
				Mode = StiJsonReportObjectHelper.DeserializeEnum<StiTopNMode>(item);
				break;
			case "Count":
				Count = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "ShowOthers":
				ShowOthers = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "OthersText":
				OthersText = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.ChartTopN() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		return MemberwiseClone() as IStiSeriesTopN;
	}
}
