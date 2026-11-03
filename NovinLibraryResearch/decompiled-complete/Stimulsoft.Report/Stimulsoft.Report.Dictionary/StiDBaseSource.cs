using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDBaseSourceConverter))]
public class StiDBaseSource : StiFileDataSource
{
	public override StiComponentId ComponentId => StiComponentId.StiDBaseSource;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Path", base.Path);
		JsonExtensions.AddPropertyInt(val, "CodePage", base.CodePage, 0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "Path"))
			{
				if (text == "CodePage")
				{
					base.CodePage = StiJsonReportObjectHelper.DeserializeInt(item);
				}
			}
			else
			{
				base.Path = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiDBaseAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiDBaseSource();
	}

	public StiDBaseSource()
		: this("", "", "", 0)
	{
	}

	public StiDBaseSource(string path, string name, string alias, int codePage)
		: base(path, name, alias, codePage)
	{
	}

	public StiDBaseSource(string path, string name, string alias, int codePage, string key)
		: base(path, name, alias, codePage, key)
	{
	}
}
