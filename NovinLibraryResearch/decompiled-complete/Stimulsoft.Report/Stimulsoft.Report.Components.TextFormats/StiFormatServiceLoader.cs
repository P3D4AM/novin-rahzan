using System;
using System.Linq;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Components.TextFormats;

public static class StiFormatServiceLoader
{
	public static StiFormatService LoadFromJson(string str)
	{
		JProperty val = JObject.Parse(str).Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident"));
		string identName = val.Value.ToObject<string>();
		StiFormatService stiFormatService = StiFormatServiceCreator.New(identName);
		JsonConvert.PopulateObject(str, (object)stiFormatService);
		return stiFormatService;
	}
}
