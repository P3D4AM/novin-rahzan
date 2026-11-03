using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public abstract class StiIndicatorRangeInfo : ICloneable, IStiPropertyGridObject
{
	public abstract StiComponentId ComponentId { get; }

	[Browsable(false)]
	public string PropName => null;

	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the value of the indicator.")]
	public float Value { get; set; }

	[Browsable(false)]
	internal abstract StiBarRangeListType RangeListType { get; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyFloat(val, "Value", Value, 0f);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Value")
			{
				Value = StiJsonReportObjectHelper.DeserializeFloat(item);
			}
		}
	}

	public abstract StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level);

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual StiIndicatorRangeInfo CreateNew()
	{
		throw new NotImplementedException();
	}
}
