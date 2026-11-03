using System;
using System.ComponentModel;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiIndicatorConverter))]
public abstract class StiIndicator : IStiJsonReportObject
{
	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		return val;
	}

	public abstract void LoadFromJsonObject(JObject jObject);

	internal static StiIndicator CreateFromJsonObject(JObject jObject)
	{
		string text = StiJsonReportObjectHelper.DeserializeString(jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")));
		StiIndicator stiIndicator = null;
		if (!(text == "StiIconSetIndicator"))
		{
			if (text == "StiDataBarIndicator")
			{
				stiIndicator = new StiDataBarIndicator();
			}
		}
		else
		{
			stiIndicator = new StiIconSetIndicator();
		}
		stiIndicator.LoadFromJsonObject(jObject);
		return stiIndicator;
	}
}
