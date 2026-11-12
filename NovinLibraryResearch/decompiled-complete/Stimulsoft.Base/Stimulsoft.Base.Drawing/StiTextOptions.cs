using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(StiTextOptionsConverter))]
[RefreshProperties(RefreshProperties.All)]
public sealed class StiTextOptions : ICloneable, IStiJsonReportObject
{
	private sealed class BitsTextOptions : ICloneable
	{
		public bool RightToLeft { get; set; }

		public bool LineLimit { get; set; }

		public float Angle { get; set; }

		public float FirstTabOffset { get; set; }

		public float DistanceBetweenTabs { get; set; }

		public HotkeyPrefix HotkeyPrefix { get; set; }

		public StringTrimming Trimming { get; set; }

		public object Clone()
		{
			return MemberwiseClone();
		}

		private bool Equals(BitsTextOptions other)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			if (RightToLeft == other.RightToLeft && LineLimit == other.LineLimit && Angle.Equals(other.Angle) && FirstTabOffset.Equals(other.FirstTabOffset) && DistanceBetweenTabs.Equals(other.DistanceBetweenTabs) && HotkeyPrefix == other.HotkeyPrefix)
			{
				return Trimming == other.Trimming;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				if (this != obj)
				{
					if (obj.GetType() == GetType())
					{
						return Equals((BitsTextOptions)obj);
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected I4, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected I4, but got Unknown
			int num = (RightToLeft ? 1231 : 1237);
			num = (num * 397) ^ (LineLimit ? 1231 : 1237);
			num = (num * 397) ^ Angle.GetHashCode();
			num = (num * 397) ^ FirstTabOffset.GetHashCode();
			num = (num * 397) ^ DistanceBetweenTabs.GetHashCode();
			num = (num * 397) ^ HotkeyPrefix;
			return (num * 397) ^ Trimming;
		}

		public BitsTextOptions(bool rightToLeft, bool lineLimit, float angle, float firstTabOffset, float distanceBetweenTabs, HotkeyPrefix hotkeyPrefix, StringTrimming trimming)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			RightToLeft = rightToLeft;
			LineLimit = lineLimit;
			Angle = angle;
			FirstTabOffset = firstTabOffset;
			DistanceBetweenTabs = distanceBetweenTabs;
			HotkeyPrefix = hotkeyPrefix;
			Trimming = trimming;
		}
	}

	private BitsTextOptions bits;

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets horizontal output direction.")]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	public bool RightToLeft
	{
		get
		{
			if (bits != null)
			{
				return bits.RightToLeft;
			}
			return false;
		}
		set
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			if (value || bits != null)
			{
				if (bits != null)
				{
					bits.RightToLeft = value;
				}
				else
				{
					bits = new BitsTextOptions(value, LineLimit, Angle, FirstTabOffset, DistanceBetweenTabs, HotkeyPrefix, Trimming);
				}
			}
		}
	}

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, that show compleleted lines only.")]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	public bool LineLimit
	{
		get
		{
			if (bits != null)
			{
				return bits.LineLimit;
			}
			return false;
		}
		set
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			if (value || bits != null)
			{
				if (bits != null)
				{
					bits.LineLimit = value;
				}
				else
				{
					bits = new BitsTextOptions(RightToLeft, value, Angle, FirstTabOffset, DistanceBetweenTabs, HotkeyPrefix, Trimming);
				}
			}
		}
	}

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	public bool WordWrap { get; set; }

	[DefaultValue(0f)]
	[Description("Gets or sets angle of a text rotation.")]
	[Browsable(false)]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	public float Angle
	{
		get
		{
			if (bits != null)
			{
				return bits.Angle;
			}
			return 0f;
		}
		set
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (value != 0f || bits != null)
			{
				if (bits != null)
				{
					bits.Angle = value;
				}
				else
				{
					bits = new BitsTextOptions(RightToLeft, LineLimit, value, FirstTabOffset, DistanceBetweenTabs, HotkeyPrefix, Trimming);
				}
			}
		}
	}

	[DefaultValue(40f)]
	[Description("Gets or sets first tab offset.")]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	public float FirstTabOffset
	{
		get
		{
			if (bits != null)
			{
				return bits.FirstTabOffset;
			}
			return 40f;
		}
		set
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			if (!(value < 0f) && (value != 40f || bits != null))
			{
				if (bits != null)
				{
					bits.FirstTabOffset = value;
				}
				else
				{
					bits = new BitsTextOptions(RightToLeft, LineLimit, Angle, value, DistanceBetweenTabs, HotkeyPrefix, Trimming);
				}
			}
		}
	}

	[DefaultValue(20f)]
	[Description("Gets or sets distance between tabs.")]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	public float DistanceBetweenTabs
	{
		get
		{
			if (bits != null)
			{
				return bits.DistanceBetweenTabs;
			}
			return 20f;
		}
		set
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			if (!(value < 0f) && (value != 20f || bits != null))
			{
				if (bits != null)
				{
					bits.DistanceBetweenTabs = value;
				}
				else
				{
					bits = new BitsTextOptions(RightToLeft, LineLimit, Angle, FirstTabOffset, value, HotkeyPrefix, Trimming);
				}
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets type of drawing hot keys.")]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	public HotkeyPrefix HotkeyPrefix
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (bits != null)
			{
				return bits.HotkeyPrefix;
			}
			return (HotkeyPrefix)0;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if ((int)value != 0 || bits != null)
			{
				if (bits != null)
				{
					bits.HotkeyPrefix = value;
				}
				else
				{
					bits = new BitsTextOptions(RightToLeft, LineLimit, Angle, FirstTabOffset, DistanceBetweenTabs, value, Trimming);
				}
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets type to trim the end of a line.")]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	public StringTrimming Trimming
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (bits != null)
			{
				return bits.Trimming;
			}
			return (StringTrimming)0;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if ((int)value != 0 || bits != null)
			{
				if (bits != null)
				{
					bits.Trimming = value;
				}
				else
				{
					bits = new BitsTextOptions(RightToLeft, LineLimit, Angle, FirstTabOffset, DistanceBetweenTabs, HotkeyPrefix, value);
				}
			}
		}
	}

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (!WordWrap)
			{
				return bits == null;
			}
			return false;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		JObject jObject = new JObject();
		jObject.AddPropertyBool("RightToLeft", RightToLeft);
		jObject.AddPropertyBool("LineLimit", LineLimit);
		jObject.AddPropertyFloat("Angle", Angle);
		jObject.AddPropertyFloat("FirstTabOffset", FirstTabOffset, 40f);
		jObject.AddPropertyFloat("DistanceBetweenTabs", DistanceBetweenTabs, 20f);
		jObject.AddPropertyEnum("HotkeyPrefix", (Enum)(object)HotkeyPrefix, (Enum)(object)(HotkeyPrefix)0);
		jObject.AddPropertyEnum("Trimming", (Enum)(object)Trimming, (Enum)(object)(StringTrimming)0);
		jObject.AddPropertyBool("WordWrap", WordWrap);
		if (jObject.Count > 0)
		{
			return jObject;
		}
		return null;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "RightToLeft":
				RightToLeft = item.DeserializeBool();
				break;
			case "LineLimit":
				LineLimit = item.DeserializeBool();
				break;
			case "Angle":
				Angle = item.DeserializeFloat();
				break;
			case "FirstTabOffset":
				FirstTabOffset = item.DeserializeFloat();
				break;
			case "DistanceBetweenTabs":
				DistanceBetweenTabs = item.DeserializeFloat();
				break;
			case "HotkeyPrefix":
				HotkeyPrefix = item.DeserializeEnum<HotkeyPrefix>();
				break;
			case "Trimming":
				Trimming = item.DeserializeEnum<StringTrimming>();
				break;
			case "WordWrap":
				WordWrap = item.DeserializeBool();
				break;
			}
		}
	}

	public object Clone()
	{
		StiTextOptions stiTextOptions = new StiTextOptions();
		if (bits != null)
		{
			stiTextOptions.bits = bits.Clone() as BitsTextOptions;
		}
		stiTextOptions.WordWrap = WordWrap;
		return stiTextOptions;
	}

	private bool Equals(StiTextOptions other)
	{
		if (object.Equals(bits, other.bits))
		{
			return WordWrap.Equals(other.WordWrap);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is StiTextOptions)
				{
					return Equals((StiTextOptions)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((("StiTextOptions".GetHashCode() * 397) ^ ((bits != null) ? bits.GetHashCode() : 0)) * 397) ^ (WordWrap ? 1231 : 1237);
	}

	public StringFormat GetStringFormat()
	{
		return GetStringFormat(antialiasing: false, 1f);
	}

	public StringFormat GetStringFormat(bool antialiasing, float zoom)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		StringFormat val = (antialiasing ? new StringFormat(StringFormat.GenericTypographic) : new StringFormat());
		val.FormatFlags = (StringFormatFlags)0;
		if (!WordWrap)
		{
			val.FormatFlags = (StringFormatFlags)4096;
		}
		if (RightToLeft)
		{
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
		}
		if (LineLimit)
		{
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x2000);
		}
		val.Trimming = Trimming;
		val.HotkeyPrefix = HotkeyPrefix;
		val.SetTabStops(FirstTabOffset * zoom, new float[1] { DistanceBetweenTabs * zoom });
		return val;
	}

	public StiTextOptions()
		: this(rightToLeft: false, lineLimit: false, wordWrap: false, 0f, (HotkeyPrefix)0, (StringTrimming)0)
	{
	}

	public StiTextOptions(bool rightToLeft, bool lineLimit, bool wordWrap, float angle, HotkeyPrefix hotkeyPrefix, StringTrimming trimming)
		: this(rightToLeft, lineLimit, wordWrap, angle, hotkeyPrefix, trimming, 40f, 20f)
	{
	}//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_0008: Unknown result type (might be due to invalid IL or missing references)


	public StiTextOptions(bool rightToLeft, bool lineLimit, bool wordWrap, float angle, HotkeyPrefix hotkeyPrefix, StringTrimming trimming, float firstTabOffset, float distanceBetweenTabs)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		WordWrap = wordWrap;
		if (!rightToLeft && !lineLimit && angle == 0f && (int)hotkeyPrefix == 0 && (int)trimming == 0 && firstTabOffset == 40f && distanceBetweenTabs == 20f)
		{
			bits = null;
		}
		else
		{
			bits = new BitsTextOptions(rightToLeft, lineLimit, angle, firstTabOffset, distanceBetweenTabs, hotkeyPrefix, trimming);
		}
	}
}
