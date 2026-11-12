using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;

namespace Stimulsoft.Dashboard.Components;

[JsonObject]
[JsonConverter(typeof(StiMeterJsonConverter))]
[TypeConverter(typeof(StiMeterConverter))]
public abstract class StiMeter : StiObject, IStiMeter, IStiLocalizedMeter, IStiJsonReportObject
{
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[DefaultValue("")]
	public string Expression { get; set; } = string.Empty;


	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	[DefaultValue("")]
	public string Label { get; set; } = string.Empty;


	[Browsable(false)]
	[JsonIgnore]
	public abstract string LocalizedName { get; }

	[Browsable(false)]
	public abstract StiMeterIdent Ident { get; }

	[DefaultValue(null)]
	[StiSerializable(StiSerializeTypes.SerializeToAllExceptCode)]
	public string Key { get; set; }

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (Key == null && Expression != null && Expression.Length == 0 && Label != null)
			{
				return Label.Length == 0;
			}
			return false;
		}
	}

	public override object Clone()
	{
		StiMeter stiMeter = base.Clone() as StiMeter;
		stiMeter.Key = StiKeyHelper.GenerateKey();
		return stiMeter;
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyIdent("Ident", Ident.ToString());
		jObject.AddPropertyStringNullOrEmpty("Key", Key);
		jObject.AddPropertyStringNullOrEmpty("Expression", Expression);
		jObject.AddPropertyStringNullOrEmpty("Label", Label);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Key":
				Key = item.DeserializeString();
				break;
			case "Expression":
				Expression = item.DeserializeString();
				break;
			case "Label":
				Label = item.DeserializeString();
				break;
			}
		}
	}

	public virtual int GetUniqueCode()
	{
		int num = ((Expression != null) ? Expression.GetHashCode() : 0);
		num = (num * 397) ^ GetType().GetHashCode();
		num = (num * 397) ^ ((Label != null) ? Label.GetHashCode() : 0);
		return (num * 397) ^ ((Key != null) ? Key.GetHashCode() : 0);
	}

	public override string ToString()
	{
		string name = GetType().Name;
		if (!string.IsNullOrWhiteSpace(Expression))
		{
			return name + "-\"" + Expression + "\"";
		}
		return name;
	}

	public List<StiMeter> ToList()
	{
		return new List<StiMeter> { this };
	}

	public StiMeter()
	{
		Key = StiKeyHelper.GenerateKey();
	}

	public StiMeter(string expression, string label)
	{
		Key = StiKeyHelper.GenerateKey();
		Expression = expression;
		Label = label;
	}

	public StiMeter(string key, string expression, string label)
	{
		Key = key;
		Expression = expression;
		Label = label;
	}
}
