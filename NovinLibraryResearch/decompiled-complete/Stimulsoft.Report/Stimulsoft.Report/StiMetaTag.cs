using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report;

[TypeConverter(typeof(StiMetaTagConverter))]
[StiSerializable]
public class StiMetaTag : ICloneable, IStiJsonReportObject
{
	public string Name { get; set; }

	public string Tag { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Tag", Tag);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Name"))
			{
				if (name == "Tag")
				{
					Tag = StiJsonReportObjectHelper.DeserializeString(item);
				}
			}
			else
			{
				Name = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public object Clone()
	{
		return new StiMetaTag(Name, Tag);
	}

	public StiMetaTag(string name, string tag)
	{
		Name = name;
		Tag = tag;
	}
}
