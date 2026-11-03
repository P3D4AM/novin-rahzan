using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[RefreshProperties(RefreshProperties.All)]
[TypeConverter(typeof(StiIconSetItemConverter))]
public class StiIconSetItem : IStiJsonReportObject
{
	[StiSerializable]
	public StiIcon Icon { get; set; }

	[StiSerializable]
	[DefaultValue(StiIconSetOperation.MoreThan)]
	public StiIconSetOperation Operation { get; set; }

	[StiSerializable]
	[DefaultValue(StiIconSetValueType.Percent)]
	public StiIconSetValueType ValueType { get; set; } = StiIconSetValueType.Percent;


	[StiSerializable]
	[DefaultValue(0f)]
	public float Value { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)Icon, (Enum)StiIcon.None);
		JsonExtensions.AddPropertyEnum(val, "Operation", (Enum)Operation, (Enum)StiIconSetOperation.MoreThan);
		JsonExtensions.AddPropertyEnum(val, "ValueType", (Enum)ValueType, (Enum)StiIconSetValueType.Percent);
		JsonExtensions.AddPropertyFloat(val, "Value", Value, 0f);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Icon":
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiIcon>(item);
				break;
			case "Operation":
				Operation = StiJsonReportObjectHelper.DeserializeEnum<StiIconSetOperation>(item);
				break;
			case "ValueType":
				ValueType = StiJsonReportObjectHelper.DeserializeEnum<StiIconSetValueType>(item);
				break;
			case "Value":
				Value = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public StiIconSetItem()
	{
	}

	public StiIconSetItem(StiIcon icon, StiIconSetOperation operation, StiIconSetValueType valueType, float value)
	{
		Icon = icon;
		Operation = operation;
		ValueType = valueType;
		Value = value;
	}
}
