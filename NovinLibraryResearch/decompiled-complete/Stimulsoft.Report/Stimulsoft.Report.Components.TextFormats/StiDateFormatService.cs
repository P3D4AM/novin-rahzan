using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components.TextFormats.Design;

namespace Stimulsoft.Report.Components.TextFormats;

[TypeConverter(typeof(StiDateFormatConverter))]
[StiServiceBitmap(typeof(StiDateFormatService), "Stimulsoft.Report.Images.TextFormats.FormatDate.png")]
[StiFormatEditor("Stimulsoft.Report.Components.TextFormats.Design.StiDateEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfFormatEditor("Stimulsoft.Report.WpfDesign.StiDateEditor, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiDateFormatService : StiFormatService
{
	[JsonIgnore]
	public override string ServiceName => Loc.Get("FormFormatEditor", "Date");

	[JsonIgnore]
	public override int Position => 4;

	[JsonIgnore]
	public override object Sample => DateTime.Now;

	[JsonIgnore]
	public override string NativeFormatString => "{0:" + Loc.Get("Formats", "date11") + "}";

	[DefaultValue("d")]
	public override string StringFormat
	{
		get
		{
			return base.StringFormat;
		}
		set
		{
			base.StringFormat = value;
		}
	}

	[DefaultValue(" ")]
	[StiSerializable]
	public string NullDisplay { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyString(val, "StringFormat", StringFormat, "d");
		JsonExtensions.AddPropertyString(val, "NullDisplay", NullDisplay, " ");
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "NullDisplay")
			{
				NullDisplay = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StiDateFormatService stiDateFormatService))
		{
			return false;
		}
		if (NullDisplay != stiDateFormatService.NullDisplay)
		{
			return false;
		}
		if (StringFormat != stiDateFormatService.StringFormat)
		{
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return ((object)this).GetHashCode();
	}

	public override string Format(string stringFormat, object arg)
	{
		if (arg == null || arg is DBNull)
		{
			return NullDisplay;
		}
		if (string.IsNullOrEmpty(StringFormat))
		{
			return arg.ToString();
		}
		if (StiOptions.Engine.AllowConvertingInFormatting && !(arg is DateTime) && !(arg is TimeSpan) && !(arg is DateTimeOffset))
		{
			DateTime? dateTime = StiValueHelper.TryToNullableDateTime(arg);
			if (dateTime.HasValue)
			{
				arg = dateTime;
			}
		}
		if (arg is DateTime || arg is TimeSpan || arg is DateTimeOffset)
		{
			switch (stringFormat.ToUpperInvariant())
			{
			case "Q":
			case "QI":
			case "YQ":
			case "YQI":
				return FormatQuarter(stringFormat, arg);
			default:
				return string.Format("{0:" + stringFormat + "}", arg);
			}
		}
		return arg.ToString();
	}

	private static string FormatQuarter(string stringFormat, object arg)
	{
		if (arg is DateTimeOffset dateTimeOffset)
		{
			arg = dateTimeOffset.DateTime;
		}
		if (arg is DateTime value)
		{
			switch (stringFormat)
			{
			case "Q":
				return Funcs.QuarterName((DateTime?)value, true);
			case "QI":
				return Funcs.QuarterIndex((DateTime?)value).ToString();
			case "YQ":
				return $"{value.Year}-{Funcs.QuarterName((DateTime?)value, true)}";
			case "YQI":
				return $"{value.Year}-{Funcs.QuarterIndex((DateTime?)value)}";
			}
		}
		return arg.ToString();
	}

	public override StiFormatService CreateNew()
	{
		return new StiDateFormatService();
	}

	public StiDateFormatService()
		: this("d", " ")
	{
	}

	public StiDateFormatService(string stringFormat, string nullDisplay)
	{
		StringFormat = stringFormat;
		NullDisplay = nullDisplay;
	}
}
