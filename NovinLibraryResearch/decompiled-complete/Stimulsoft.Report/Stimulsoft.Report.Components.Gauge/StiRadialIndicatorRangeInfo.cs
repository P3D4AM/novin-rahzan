using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiRadialIndicatorRangeInfo : StiIndicatorRangeInfo
{
	public override StiComponentId ComponentId => StiComponentId.StiRadialIndicatorRangeInfo;

	[StiSerializable]
	[Description("Gets or sets a brush to fill a component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush { get; set; } = (StiBrush)new StiEmptyBrush();


	internal override StiBarRangeListType RangeListType => StiBarRangeListType.RadialBar;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Brush")
			{
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.ValueF() };
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[1] { propertiesHelper.Brush() };
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		return stiPropertyCollection;
	}

	public override StiIndicatorRangeInfo CreateNew()
	{
		return new StiRadialIndicatorRangeInfo();
	}
}
