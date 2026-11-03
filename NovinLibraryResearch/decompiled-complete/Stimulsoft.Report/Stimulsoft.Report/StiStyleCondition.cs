using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report;

[RefreshProperties(RefreshProperties.All)]
[TypeConverter(typeof(StiStyleConditionConverter))]
public class StiStyleCondition : ICloneable, IStiJsonReportObject
{
	[StiSerializable]
	[DefaultValue(StiStyleConditionType.Placement)]
	public StiStyleConditionType Type { get; set; } = StiStyleConditionType.Placement;


	[StiSerializable]
	[DefaultValue(StiStyleConditionOperation.EqualTo)]
	public StiStyleConditionOperation OperationPlacement { get; set; }

	[StiSerializable]
	[DefaultValue(StiStyleConditionOperation.EqualTo)]
	public StiStyleConditionOperation OperationPlacementNestedLevel { get; set; }

	[StiSerializable]
	[DefaultValue(StiStyleConditionOperation.EqualTo)]
	public StiStyleConditionOperation OperationComponentType { get; set; }

	[StiSerializable]
	[DefaultValue(StiStyleConditionOperation.EqualTo)]
	public StiStyleConditionOperation OperationLocation { get; set; }

	[StiSerializable]
	[DefaultValue(StiStyleConditionOperation.EqualTo)]
	public StiStyleConditionOperation OperationComponentName { get; set; }

	[StiSerializable]
	[DefaultValue(StiStyleComponentPlacement.None)]
	public StiStyleComponentPlacement Placement { get; set; }

	[StiSerializable]
	[DefaultValue(1)]
	public int PlacementNestedLevel { get; set; } = 1;


	[StiSerializable]
	[DefaultValue(StiStyleComponentType.Text)]
	public StiStyleComponentType ComponentType { get; set; } = StiStyleComponentType.Text;


	[StiSerializable]
	[DefaultValue(StiStyleLocation.None)]
	public StiStyleLocation Location { get; set; }

	[StiSerializable]
	[DefaultValue("")]
	public string ComponentName { get; set; } = "";


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyEnum(val, "Type", (Enum)Type, (Enum)StiStyleConditionType.Placement);
		JsonExtensions.AddPropertyEnum(val, "OperationPlacement", (Enum)OperationPlacement, (Enum)StiStyleConditionOperation.EqualTo);
		JsonExtensions.AddPropertyEnum(val, "OperationPlacementNestedLevel", (Enum)OperationPlacementNestedLevel, (Enum)StiStyleConditionOperation.EqualTo);
		JsonExtensions.AddPropertyEnum(val, "OperationComponentType", (Enum)OperationComponentType, (Enum)StiStyleConditionOperation.EqualTo);
		JsonExtensions.AddPropertyEnum(val, "OperationLocation", (Enum)OperationLocation, (Enum)StiStyleConditionOperation.EqualTo);
		JsonExtensions.AddPropertyEnum(val, "OperationComponentName", (Enum)OperationComponentName, (Enum)StiStyleConditionOperation.EqualTo);
		JsonExtensions.AddPropertyEnum(val, "Placement", (Enum)Placement, (Enum)StiStyleComponentPlacement.None);
		JsonExtensions.AddPropertyInt(val, "PlacementNestedLevel", PlacementNestedLevel, 1);
		JsonExtensions.AddPropertyEnum(val, "ComponentType", (Enum)ComponentType, (Enum)StiStyleComponentType.Text);
		JsonExtensions.AddPropertyEnum(val, "Location", (Enum)Location, (Enum)StiStyleLocation.None);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ComponentName", ComponentName);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Type":
				Type = StiJsonReportObjectHelper.DeserializeEnum<StiStyleConditionType>(item);
				break;
			case "OperationPlacement":
				OperationPlacement = StiJsonReportObjectHelper.DeserializeEnum<StiStyleConditionOperation>(item);
				break;
			case "OperationPlacementNestedLevel":
				OperationPlacementNestedLevel = StiJsonReportObjectHelper.DeserializeEnum<StiStyleConditionOperation>(item);
				break;
			case "OperationComponentType":
				OperationComponentType = StiJsonReportObjectHelper.DeserializeEnum<StiStyleConditionOperation>(item);
				break;
			case "OperationLocation":
				OperationLocation = StiJsonReportObjectHelper.DeserializeEnum<StiStyleConditionOperation>(item);
				break;
			case "OperationComponentName":
				OperationComponentName = StiJsonReportObjectHelper.DeserializeEnum<StiStyleConditionOperation>(item);
				break;
			case "Placement":
				Placement = StiJsonReportObjectHelper.DeserializeEnum<StiStyleComponentPlacement>(item);
				break;
			case "PlacementNestedLevel":
				PlacementNestedLevel = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "ComponentType":
				ComponentType = StiJsonReportObjectHelper.DeserializeEnum<StiStyleComponentType>(item);
				break;
			case "Location":
				Location = StiJsonReportObjectHelper.DeserializeEnum<StiStyleLocation>(item);
				break;
			case "ComponentName":
				ComponentName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	internal void FromElements(StiStyleConditionElement[] elements)
	{
		Type = (StiStyleConditionType)0;
		foreach (StiStyleConditionElement stiStyleConditionElement in elements)
		{
			if (stiStyleConditionElement is StiStyleConditionComponentNameElement stiStyleConditionComponentNameElement)
			{
				Type |= StiStyleConditionType.ComponentName;
				ComponentName = stiStyleConditionComponentNameElement.ComponentName;
				OperationComponentName = stiStyleConditionComponentNameElement.OperationComponentName;
			}
			if (stiStyleConditionElement is StiStyleConditionComponentTypeElement stiStyleConditionComponentTypeElement)
			{
				Type |= StiStyleConditionType.ComponentType;
				ComponentType = stiStyleConditionComponentTypeElement.ComponentType;
				OperationComponentType = stiStyleConditionComponentTypeElement.OperationComponentType;
			}
			if (stiStyleConditionElement is StiStyleConditionPlacementElement stiStyleConditionPlacementElement)
			{
				Type |= StiStyleConditionType.Placement;
				Placement = stiStyleConditionPlacementElement.Placement;
				OperationPlacement = stiStyleConditionPlacementElement.OperationPlacement;
			}
			if (stiStyleConditionElement is StiStyleConditionPlacementNestedLevelElement stiStyleConditionPlacementNestedLevelElement)
			{
				Type |= StiStyleConditionType.PlacementNestedLevel;
				PlacementNestedLevel = stiStyleConditionPlacementNestedLevelElement.PlacementNestedLevel;
				OperationPlacementNestedLevel = stiStyleConditionPlacementNestedLevelElement.OperationPlacementNestedLevel;
			}
			if (stiStyleConditionElement is StiStyleConditionLocationElement stiStyleConditionLocationElement)
			{
				Type |= StiStyleConditionType.Location;
				Location = stiStyleConditionLocationElement.Location;
				OperationLocation = stiStyleConditionLocationElement.OperationLocation;
			}
		}
	}

	public StiStyleCondition()
	{
	}

	public StiStyleCondition(StiStyleConditionElement[] elements)
	{
		FromElements(elements);
	}

	public StiStyleCondition(StiStyleConditionType type, StiStyleConditionOperation operationPlacement, StiStyleConditionOperation operationPlacementNestedLevel, StiStyleConditionOperation operationComponentType, StiStyleConditionOperation operationLocation, StiStyleConditionOperation operationComponentName, StiStyleComponentPlacement placement, int placementNestedLevel, StiStyleComponentType componentType, StiStyleLocation location, string componentName)
	{
		Type = type;
		OperationPlacement = operationPlacement;
		OperationPlacementNestedLevel = operationPlacementNestedLevel;
		OperationComponentType = operationComponentType;
		OperationLocation = operationLocation;
		OperationComponentName = operationComponentName;
		Placement = placement;
		PlacementNestedLevel = placementNestedLevel;
		ComponentType = componentType;
		Location = location;
		ComponentName = componentName;
	}
}
