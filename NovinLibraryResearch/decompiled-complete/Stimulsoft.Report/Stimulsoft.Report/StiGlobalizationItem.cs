using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report;

[TypeConverter(typeof(StiGlobalizationItemConverter))]
[StiSerializable]
public sealed class StiGlobalizationItem : IStiJsonReportObject
{
	[StiSerializable]
	[DefaultValue("")]
	public string PropertyName { get; set; } = "";


	[StiSerializable]
	[DefaultValue("")]
	public string Text { get; set; } = "";


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "PropertyName", PropertyName);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "PropertyName"))
			{
				if (name == "Text")
				{
					Text = StiJsonReportObjectHelper.DeserializeString(item);
				}
			}
			else
			{
				PropertyName = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override string ToString()
	{
		return PropertyName;
	}

	public StiGlobalizationItem()
	{
	}

	public StiGlobalizationItem(string propertyName, string text)
	{
		PropertyName = propertyName;
		Text = text;
	}
}
