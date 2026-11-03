using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiWaterfallTotal : IStiWaterfallTotal, ICloneable, IStiJsonReportObject, IStiSerializeToCodeAsClass
{
	[StiSerializable]
	[DefaultValue("")]
	public string Text { get; set; } = "Total";


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool Visible { get; set; } = true;


	public object Clone()
	{
		return MemberwiseClone();
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Text"))
			{
				if (name == "Visible")
				{
					Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				}
			}
			else
			{
				Text = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public StiWaterfallTotal()
	{
	}

	[StiUniversalConstructor("Total")]
	public StiWaterfallTotal(string text, bool visible)
	{
		Text = text;
		Visible = visible;
	}
}
