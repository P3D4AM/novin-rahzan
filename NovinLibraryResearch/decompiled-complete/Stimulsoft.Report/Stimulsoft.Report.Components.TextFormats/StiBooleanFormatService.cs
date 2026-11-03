using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.TextFormats.Design;

namespace Stimulsoft.Report.Components.TextFormats;

[TypeConverter(typeof(StiBooleanFormatConverter))]
[StiServiceBitmap(typeof(StiBooleanFormatService), "Stimulsoft.Report.Images.TextFormats.FormatBoolean.png")]
[StiFormatEditor("Stimulsoft.Report.Components.TextFormats.Design.StiBooleanEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfFormatEditor("Stimulsoft.Report.WpfDesign.StiBooleanEditor, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiBooleanFormatService : StiFormatService
{
	private class bitsBooleanFormatService : ICloneable
	{
		public string falseValue;

		public string trueValue;

		public string falseDisplay;

		public string trueDisplay;

		public string nullDisplay;

		public object Clone()
		{
			return MemberwiseClone();
		}

		public bitsBooleanFormatService(string falseValue, string trueValue, string falseDisplay, string trueDisplay, string nullDisplay)
		{
			this.falseValue = falseValue;
			this.trueValue = trueValue;
			this.falseDisplay = falseDisplay;
			this.trueDisplay = trueDisplay;
			this.nullDisplay = nullDisplay;
		}
	}

	private bitsBooleanFormatService bits;

	[JsonIgnore]
	public override string ServiceName => Loc.Get("FormFormatEditor", "Boolean");

	[JsonIgnore]
	public override int Position => 7;

	[JsonIgnore]
	public override object Sample => false;

	[StiSerializable]
	[DefaultValue("False")]
	public string FalseValue
	{
		get
		{
			if (bits != null)
			{
				return bits.falseValue;
			}
			return "False";
		}
		set
		{
			if (!(value == "False") || bits != null)
			{
				if (bits != null)
				{
					bits.falseValue = value;
				}
				else
				{
					bits = new bitsBooleanFormatService(value, TrueValue, FalseDisplay, TrueDisplay, NullDisplay);
				}
			}
		}
	}

	[DefaultValue("True")]
	[StiSerializable]
	public string TrueValue
	{
		get
		{
			if (bits != null)
			{
				return bits.trueValue;
			}
			return "True";
		}
		set
		{
			if (!(value == "True") || bits != null)
			{
				if (bits != null)
				{
					bits.trueValue = value;
				}
				else
				{
					bits = new bitsBooleanFormatService(FalseValue, value, FalseDisplay, TrueDisplay, NullDisplay);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue("False")]
	public string FalseDisplay
	{
		get
		{
			if (bits != null)
			{
				return bits.falseDisplay;
			}
			return "False";
		}
		set
		{
			if (!(value == "False") || bits != null)
			{
				if (bits != null)
				{
					bits.falseDisplay = value;
				}
				else
				{
					bits = new bitsBooleanFormatService(FalseValue, TrueValue, value, TrueDisplay, NullDisplay);
				}
			}
		}
	}

	[DefaultValue("True")]
	[StiSerializable]
	public string TrueDisplay
	{
		get
		{
			if (bits != null)
			{
				return bits.trueDisplay;
			}
			return "True";
		}
		set
		{
			if (!(value == "True") || bits != null)
			{
				if (bits != null)
				{
					bits.trueDisplay = value;
				}
				else
				{
					bits = new bitsBooleanFormatService(FalseValue, TrueValue, FalseDisplay, value, NullDisplay);
				}
			}
		}
	}

	[StiSerializable]
	public string NullDisplay
	{
		get
		{
			if (bits != null)
			{
				return bits.nullDisplay;
			}
			return " ";
		}
		set
		{
			if (!(value == " ") || bits != null)
			{
				if (bits != null)
				{
					bits.nullDisplay = value;
				}
				else
				{
					bits = new bitsBooleanFormatService(FalseValue, TrueValue, FalseDisplay, TrueDisplay, value);
				}
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		if (bits != null)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "FalseValue", bits.falseValue);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "TrueValue", bits.trueValue);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "FalseDisplay", bits.falseDisplay);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "TrueDisplay", bits.trueDisplay);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "NullDisplay", bits.nullDisplay);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "FalseValue":
				FalseValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TrueValue":
				TrueValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "FalseDisplay":
				FalseDisplay = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TrueDisplay":
				TrueDisplay = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "NullDisplay":
				NullDisplay = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		StiBooleanFormatService stiBooleanFormatService = ((StiService)this).Clone() as StiBooleanFormatService;
		if (bits != null)
		{
			stiBooleanFormatService.bits = bits.Clone() as bitsBooleanFormatService;
		}
		return stiBooleanFormatService;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StiBooleanFormatService stiBooleanFormatService))
		{
			return false;
		}
		if (FalseValue != stiBooleanFormatService.FalseValue)
		{
			return false;
		}
		if (TrueValue != stiBooleanFormatService.TrueValue)
		{
			return false;
		}
		if (FalseDisplay != stiBooleanFormatService.FalseDisplay)
		{
			return false;
		}
		if (TrueDisplay != stiBooleanFormatService.TrueDisplay)
		{
			return false;
		}
		if (NullDisplay != stiBooleanFormatService.NullDisplay)
		{
			return false;
		}
		if (StringFormat != stiBooleanFormatService.StringFormat)
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
		string text = arg.ToString();
		if (text == TrueValue)
		{
			return TrueDisplay;
		}
		if (text == FalseValue)
		{
			return FalseDisplay;
		}
		return NullDisplay;
	}

	public override StiFormatService CreateNew()
	{
		return new StiBooleanFormatService();
	}

	public StiBooleanFormatService()
		: this("False", "True", StiLocalization.Get("FormFormatEditor", "nameFalse"), StiLocalization.Get("FormFormatEditor", "nameTrue"), " ")
	{
	}

	public StiBooleanFormatService(string falseValue, string trueValue, string falseDisplay, string trueDisplay, string nullDisplay)
	{
		FalseValue = falseValue;
		TrueValue = trueValue;
		FalseDisplay = falseDisplay;
		TrueDisplay = trueDisplay;
		NullDisplay = nullDisplay;
	}
}
