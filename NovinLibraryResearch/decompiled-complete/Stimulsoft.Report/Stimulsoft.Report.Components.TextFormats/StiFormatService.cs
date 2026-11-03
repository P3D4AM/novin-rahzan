using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Components.TextFormats;

[JsonObject]
[JsonConverter(typeof(StiFormatServiceJsonConverter))]
[StiServiceBitmap(typeof(StiFormatService), "Stimulsoft.Report.Images.TextFormats.FormatCustom.png")]
[StiServiceCategoryBitmap(typeof(StiFormatService), "Stimulsoft.Report.Images.TextFormats.FormatCustom.png")]
public abstract class StiFormatService : StiService, IStiDefault, IStiJsonReportObject
{
	private string stringFormat = string.Empty;

	[JsonIgnore]
	public bool IsDefault => this is StiGeneralFormatService;

	[JsonIgnore]
	public sealed override string ServiceCategory => Loc.Get("Services", "categoryTextFormat");

	[JsonIgnore]
	public sealed override Type ServiceType => typeof(StiFormatService);

	public string Ident => ((object)this).GetType().Name;

	[JsonIgnore]
	public abstract int Position { get; }

	[JsonIgnore]
	public abstract object Sample { get; }

	[JsonIgnore]
	public virtual string NativeFormatString => "{0}";

	[JsonIgnore]
	public bool IsFormatStringFromVariable
	{
		get
		{
			if (this is StiCustomFormatService && StringFormat != null)
			{
				string text = StringFormat.Trim();
				if (text.Length > 2 && StringExt.StartsWithInvariant(text, "{"))
				{
					return StringExt.EndsWithInvariant(text, "}");
				}
				return false;
			}
			return false;
		}
	}

	[StiSerializable]
	public virtual string StringFormat
	{
		get
		{
			return stringFormat;
		}
		set
		{
			stringFormat = value;
		}
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "StringFormat", StringFormat);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "StringFormat")
			{
				stringFormat = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	internal static StiFormatService CreateFromJsonObject(JObject jObject)
	{
		List<StiFormatService> formats = StiOptions.Services.Formats;
		string ident = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		StiFormatService stiFormatService = formats.FirstOrDefault((StiFormatService x) => ((object)x).GetType().Name == ident).CreateNew();
		stiFormatService.LoadFromJsonObject(jObject);
		return stiFormatService;
	}

	public virtual string Format(object arg)
	{
		return Format(StringFormat, arg);
	}

	public virtual string Format(string format, object arg)
	{
		try
		{
			if (arg == null || arg is DBNull)
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(StringFormat))
			{
				return arg.ToString();
			}
			bool flag = false;
			if (arg is TimeSpan && StiOptions.Engine.AllowOldTimeSpanFormatting)
			{
				TimeSpan timeSpan = (TimeSpan)arg;
				DateTime today = DateTime.Today;
				if (timeSpan.Ticks < 0)
				{
					timeSpan = timeSpan.Negate();
					flag = true;
				}
				today = new DateTime(today.Year, today.Month, today.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				arg = today;
			}
			return string.Format("{0}{1:" + format + "}", flag ? "-" : "", arg);
		}
		catch
		{
			return (arg == null) ? string.Empty : arg.ToString();
		}
	}

	public override string ToString()
	{
		return ((StiService)this).ServiceName;
	}

	public abstract StiFormatService CreateNew();
}
