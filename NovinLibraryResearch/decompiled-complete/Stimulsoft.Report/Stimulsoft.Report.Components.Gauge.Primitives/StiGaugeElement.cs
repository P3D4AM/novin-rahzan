using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiGaugeElement : StiElementBase, IStiPropertyGridObject, IStiJsonReportObject
{
	public abstract StiComponentId ComponentId { get; }

	[Browsable(false)]
	public string PropName => null;

	[Browsable(false)]
	public StiAnimation Animation { get; set; }

	[Browsable(false)]
	public virtual StiGaugeElemenType ElementType => StiGaugeElemenType.LinearElement;

	[Browsable(false)]
	public virtual string LocalizeName => "GaugeElement";

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	public StiScaleBase Scale { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "AllowApplyStyle")
			{
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public abstract StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level);

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public virtual StiGaugeElement CreateNew()
	{
		throw new NotImplementedException();
	}

	protected internal virtual void PrepareGaugeElement()
	{
	}
}
