using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiStateIndicatorFilter : ICloneable, IStiPropertyGridObject
{
	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiStateIndicatorFilter;

	[Browsable(false)]
	public string PropName => null;

	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets start value of the filter.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float StartValue { get; set; }

	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets end value of the filter.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float EndValue { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush to fill a component.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the border of the component.")]
	public StiBrush BorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyFloat(val, "StartValue", StartValue, 0f);
		JsonExtensions.AddPropertyFloat(val, "EndValue", EndValue, 0f);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBrush(val, "BorderBrush", BorderBrush);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "StartValue":
				StartValue = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "EndValue":
				EndValue = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderBrush":
				BorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.EndValue(),
			propertiesHelper.StartValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.Brush(),
			propertiesHelper.BorderBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		return $"StartValue={StartValue}, EndValue={EndValue}";
	}
}
