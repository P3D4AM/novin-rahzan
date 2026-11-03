using System;
using System.ComponentModel;
using System.Text;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.TextFormats.Design;

namespace Stimulsoft.Report.Components.TextFormats;

[TypeConverter(typeof(StiCustomFormatConverter))]
[StiFormatEditor("Stimulsoft.Report.Components.TextFormats.Design.StiCustomEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfFormatEditor("Stimulsoft.Report.WpfDesign.StiCustomEditor, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiServiceBitmap(typeof(StiCurrencyFormatService), "Stimulsoft.Report.Images.TextFormats.FormatCustom.png")]
public class StiCustomFormatService : StiFormatService
{
	[JsonIgnore]
	public override string ServiceName => Loc.Get("FormFormatEditor", "Custom");

	[JsonIgnore]
	public override int Position => 100;

	[JsonIgnore]
	public override object Sample
	{
		get
		{
			if (StringFormat == null)
			{
				return string.Empty;
			}
			string text = StringFormat.ToLowerInvariant().Trim();
			switch (text)
			{
			default:
				if (!text.StartsWith("y"))
				{
					if (text.StartsWith("c") || text.StartsWith("n") || text.StartsWith("#") || text.StartsWith("$"))
					{
						return 123.45;
					}
					if (text.StartsWith("(#"))
					{
						return 1234567890;
					}
					return string.Empty;
				}
				goto case "d";
			case "d":
			case "f":
			case "g":
				return DateTime.Now;
			}
		}
	}

	[DefaultValue("")]
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

	public override bool Equals(object obj)
	{
		if (obj is StiCustomFormatService stiCustomFormatService)
		{
			return StringFormat == stiCustomFormatService.StringFormat;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((object)this).GetHashCode();
	}

	public override StiFormatService CreateNew()
	{
		return new StiCustomFormatService();
	}

	public override string Format(string stringFormat, object arg)
	{
		if (arg == null || arg is DBNull)
		{
			return string.Empty;
		}
		if (string.IsNullOrEmpty(StringFormat))
		{
			return arg.ToString();
		}
		if (arg is string && StringFormat.Contains("#"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			string text = arg.ToString();
			foreach (char c in stringFormat)
			{
				if (c == '#' && num < text.Length)
				{
					stringBuilder.Append(text[num++]);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
		return base.Format(stringFormat, arg);
	}

	public StiCustomFormatService()
		: this(string.Empty)
	{
	}

	public StiCustomFormatService(string stringFormat)
	{
		StringFormat = stringFormat;
	}
}
