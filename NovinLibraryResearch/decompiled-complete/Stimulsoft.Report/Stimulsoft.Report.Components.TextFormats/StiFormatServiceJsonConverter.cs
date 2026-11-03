using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Components.TextFormats;

internal class StiFormatServiceJsonConverter : CustomCreationConverter<StiFormatService>
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)reader.TokenType == 11)
		{
			return null;
		}
		JToken val = JToken.ReadFrom(reader);
		string identName = ((object)((IEnumerable)val).Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "Ident").Value).ToString();
		StiFormatService stiFormatService = StiFormatServiceCreator.New(identName);
		if (stiFormatService == null)
		{
			return null;
		}
		serializer.Populate(val.CreateReader(), (object)stiFormatService);
		return stiFormatService;
	}

	public override StiFormatService Create(Type objectType)
	{
		return Activator.CreateInstance(objectType, nonPublic: true) as StiFormatService;
	}
}
