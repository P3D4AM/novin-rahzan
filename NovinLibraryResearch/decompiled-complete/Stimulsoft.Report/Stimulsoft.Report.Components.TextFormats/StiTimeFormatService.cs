using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.TextFormats.Design;

namespace Stimulsoft.Report.Components.TextFormats;

[TypeConverter(typeof(StiTimeFormatConverter))]
[StiServiceBitmap(typeof(StiTimeFormatService), "Stimulsoft.Report.Images.TextFormats.FormatTime.png")]
[StiFormatEditor("Stimulsoft.Report.Components.TextFormats.Design.StiTimeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfFormatEditor("Stimulsoft.Report.WpfDesign.StiTimeEditor, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiTimeFormatService : StiFormatService
{
	[JsonIgnore]
	public override string ServiceName => Loc.Get("FormFormatEditor", "Time");

	[JsonIgnore]
	public override int Position => 5;

	[DefaultValue("t")]
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

	[JsonIgnore]
	public override string NativeFormatString => "{0:" + Loc.Get("Formats", "time03") + "}";

	[JsonIgnore]
	public override object Sample => DateTime.Now;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyString(val, "StringFormat", StringFormat, "t");
		return val;
	}

	public override bool Equals(object obj)
	{
		if (obj is StiPercentageFormatService stiPercentageFormatService)
		{
			return StringFormat == stiPercentageFormatService.StringFormat;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((object)this).GetHashCode();
	}

	public override string Format(string format, object arg)
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
			if (StiOptions.Engine.AllowConvertingInFormatting && !(arg is DateTime) && !(arg is TimeSpan) && !(arg is DateTimeOffset))
			{
				DateTime? dateTime = StiValueHelper.TryToNullableDateTime(arg);
				if (dateTime.HasValue)
				{
					arg = dateTime;
				}
			}
			return base.Format(format, arg);
		}
		catch
		{
			return (arg == null) ? string.Empty : arg.ToString();
		}
	}

	public override StiFormatService CreateNew()
	{
		return new StiTimeFormatService();
	}

	public StiTimeFormatService()
		: this("t")
	{
	}

	public StiTimeFormatService(string stringFormat)
	{
		StringFormat = stringFormat;
	}
}
