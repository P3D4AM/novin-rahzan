using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report;

[TypeConverter(typeof(StiReportResourceConverter))]
[StiSerializable]
public class StiReportResource : ICloneable, IStiJsonReportObject
{
	public string Resource { get; set; }

	public string Component { get; set; }

	public string Property { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Resource", Resource);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Component", Component);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Property", Property);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Resource":
				Resource = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Component":
				Component = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Property":
				Property = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return new StiReportResource(Resource, Component, Property);
	}

	public StiReportResource(string resource, string component, string property)
	{
		Resource = resource;
		Component = component;
		Property = property;
	}
}
