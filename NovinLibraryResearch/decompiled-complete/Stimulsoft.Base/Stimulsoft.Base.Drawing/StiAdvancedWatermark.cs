using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(StiUniversalConverter))]
[RefreshProperties(RefreshProperties.All)]
public class StiAdvancedWatermark : ICloneable, IStiSerializeToCodeAsClass, IStiJsonReportObject, IStiDefault
{
	private static readonly Color DefaultWeaveMajorColor = StiColor.Get("#77777777");

	private static readonly Color DefaultWeaveMinorColor = StiColor.Get("#55777777");

	private static readonly Color DefaultTextColor = Color.Gray;

	private byte[] cachedImage;

	private byte[] imageBytes;

	private int imageTransparency;

	private int weaveMajorSize = 20;

	private int weaveMinorSize = 10;

	private int weaveAngle = 30;

	private int weaveDistance = 100;

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Invalid comparison between Unknown and I4
			if (!TextEnabled && Text == "Text" && !ShouldSerializeTextFont() && TextColor == DefaultTextColor && TextAngle == 45 && !ImageEnabled && ImageBytes == null && ImageMultipleFactor == 1.0 && ImageTransparency == 0 && (int)ImageAlignment == 32 && !ImageTiling && !ImageStretch && !ImageAspectRatio && !WeaveEnabled && WeaveMajorIcon == StiFontIcons.Star && WeaveMajorSize == 20 && WeaveMajorColor == DefaultWeaveMajorColor && !WeaveMinorIcon.HasValue && WeaveMinorSize == 10 && WeaveMinorColor == DefaultWeaveMinorColor && WeaveAngle == 30)
			{
				return WeaveDistance == 100;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsVisible
	{
		get
		{
			if ((!TextEnabled || string.IsNullOrEmpty(Text)) && (!ImageEnabled || ImageBytes == null))
			{
				if (WeaveEnabled)
				{
					if (!WeaveMajorIcon.HasValue)
					{
						return WeaveMajorIcon.HasValue;
					}
					return true;
				}
				return false;
			}
			return true;
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	public virtual bool TextEnabled { get; set; }

	[StiSerializable]
	[DefaultValue("Text")]
	public virtual string Text { get; set; } = "Text";


	[StiSerializable]
	public Font TextFont { get; set; } = new Font("Arial", 36f);


	[StiSerializable]
	public Color TextColor { get; set; } = DefaultTextColor;


	[StiSerializable]
	[DefaultValue(45)]
	public virtual int TextAngle { get; set; } = 45;


	[StiSerializable]
	[DefaultValue(false)]
	public virtual bool ImageEnabled { get; set; }

	[StiNonSerialized]
	public virtual Image Image
	{
		get
		{
			return TakeGdiImage();
		}
		set
		{
			PutImage(value);
		}
	}

	[StiSerializable]
	[DefaultValue(null)]
	public byte[] ImageBytes
	{
		get
		{
			return imageBytes;
		}
		set
		{
			if (imageBytes != value)
			{
				imageBytes = value;
				DisposeCachedImage();
				PutImage(value);
			}
		}
	}

	[StiSerializable]
	[DefaultValue(1.0)]
	public virtual double ImageMultipleFactor { get; set; } = 1.0;


	[StiSerializable]
	public int ImageTransparency
	{
		get
		{
			return imageTransparency;
		}
		set
		{
			value = Math.Max(0, Math.Min(value, 255));
			if (value != imageTransparency)
			{
				imageTransparency = value;
				DisposeCachedImage();
			}
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public virtual ContentAlignment ImageAlignment { get; set; } = (ContentAlignment)32;


	[StiSerializable]
	[DefaultValue(false)]
	public virtual bool ImageTiling { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	public virtual bool ImageStretch { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	public bool ImageAspectRatio { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	public virtual bool WeaveEnabled { get; set; }

	[StiSerializable]
	[DefaultValue(StiFontIcons.Star)]
	public StiFontIcons? WeaveMajorIcon { get; set; } = StiFontIcons.Star;


	[StiSerializable]
	[DefaultValue(20)]
	public int WeaveMajorSize
	{
		get
		{
			return weaveMajorSize;
		}
		set
		{
			if (weaveMajorSize != value)
			{
				if (value < 5)
				{
					value = 5;
				}
				if (value > 30)
				{
					value = 30;
				}
				weaveMajorSize = value;
			}
		}
	}

	[StiSerializable]
	public Color WeaveMajorColor { get; set; } = DefaultWeaveMajorColor;


	[StiSerializable]
	[DefaultValue(null)]
	public StiFontIcons? WeaveMinorIcon { get; set; }

	[StiSerializable]
	[DefaultValue(10)]
	public int WeaveMinorSize
	{
		get
		{
			return weaveMinorSize;
		}
		set
		{
			if (weaveMinorSize != value)
			{
				if (value < 5)
				{
					value = 5;
				}
				if (value > 30)
				{
					value = 30;
				}
				weaveMinorSize = value;
			}
		}
	}

	[StiSerializable]
	public Color WeaveMinorColor { get; set; } = DefaultWeaveMinorColor;


	[StiSerializable]
	[DefaultValue(30)]
	public int WeaveAngle
	{
		get
		{
			return weaveAngle;
		}
		set
		{
			if (weaveAngle != value)
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 360)
				{
					value = 360;
				}
				weaveAngle = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(100)]
	public int WeaveDistance
	{
		get
		{
			return weaveDistance;
		}
		set
		{
			if (weaveDistance != value)
			{
				if (value < 50)
				{
					value = 50;
				}
				if (value > 200)
				{
					value = 200;
				}
				weaveDistance = value;
			}
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		JObject jObject = new JObject();
		jObject.AddPropertyBool("TextEnabled", TextEnabled);
		if (Text != "Text")
		{
			jObject.AddPropertyStringNullOrEmpty("Text", Text);
		}
		jObject.AddPropertyFont("TextFont", TextFont, "Arial", 36f);
		jObject.AddPropertyColor("TextColor", TextColor, DefaultTextColor);
		jObject.AddPropertyInt("TextAngle", TextAngle, 45);
		jObject.AddPropertyBool("ImageEnabled", ImageEnabled);
		if (ExistImage())
		{
			jObject.AddPropertyByteArray("ImageBytes", TakeImage());
		}
		jObject.AddPropertyDouble("ImageMultipleFactor", ImageMultipleFactor, 1.0);
		jObject.AddPropertyInt("ImageTransparency", ImageTransparency);
		jObject.AddPropertyEnum("ImageAlignment", (Enum)(object)ImageAlignment, (Enum)(object)(ContentAlignment)32);
		jObject.AddPropertyBool("ImageTiling", ImageTiling);
		jObject.AddPropertyBool("ImageStretch", ImageStretch);
		jObject.AddPropertyBool("ImageAspectRatio", ImageAspectRatio);
		jObject.AddPropertyBool("WeaveEnabled", WeaveEnabled);
		jObject.AddPropertyEnum("WeaveMajorIcon", WeaveMajorIcon, StiFontIcons.Star);
		jObject.AddPropertyInt("WeaveMajorSize", WeaveMajorSize, 20);
		jObject.AddPropertyColor("WeaveMajorColor", WeaveMajorColor, DefaultWeaveMajorColor);
		if (WeaveMinorIcon.HasValue)
		{
			jObject.AddPropertyEnum("WeaveMinorIcon", WeaveMinorIcon);
		}
		jObject.AddPropertyInt("WeaveMinorSize", WeaveMinorSize, 10);
		jObject.AddPropertyColor("WeaveMinorColor", WeaveMinorColor, DefaultWeaveMinorColor);
		jObject.AddPropertyInt("WeaveAngle", WeaveAngle, 30);
		jObject.AddPropertyInt("WeaveDistance", WeaveDistance, 100);
		if (jObject.Count == 0)
		{
			return null;
		}
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "TextEnabled":
				TextEnabled = item.DeserializeBool();
				break;
			case "Text":
				Text = item.DeserializeString();
				break;
			case "TextFont":
				TextFont = item.DeserializeFont(TextFont);
				break;
			case "TextColor":
				TextColor = item.DeserializeColor();
				break;
			case "TextAngle":
				TextAngle = item.DeserializeInt();
				break;
			case "ImageEnabled":
				ImageEnabled = item.DeserializeBool();
				break;
			case "ImageBytes":
				PutImage(item.DeserializeImage());
				break;
			case "ImageMultipleFactor":
				ImageMultipleFactor = item.DeserializeDouble();
				break;
			case "ImageTransparency":
				imageTransparency = item.DeserializeInt();
				break;
			case "ImageAlignment":
				ImageAlignment = item.DeserializeEnum<ContentAlignment>();
				break;
			case "ImageTiling":
				ImageTiling = item.DeserializeBool();
				break;
			case "ImageStretch":
				ImageStretch = item.DeserializeBool();
				break;
			case "ImageAspectRatio":
				ImageAspectRatio = item.DeserializeBool();
				break;
			case "WeaveEnabled":
				WeaveEnabled = item.DeserializeBool();
				break;
			case "WeaveMajorIcon":
				WeaveMajorIcon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "WeaveMajorSize":
				WeaveMajorSize = item.DeserializeInt();
				break;
			case "WeaveMajorColor":
				WeaveMajorColor = item.DeserializeColor();
				break;
			case "WeaveMinorIcon":
				WeaveMinorIcon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "WeaveMinorSize":
				WeaveMinorSize = item.DeserializeInt();
				break;
			case "WeaveMinorColor":
				WeaveMinorColor = item.DeserializeColor();
				break;
			case "WeaveAngle":
				WeaveAngle = item.DeserializeInt();
				break;
			case "WeaveDistance":
				WeaveDistance = item.DeserializeInt();
				break;
			}
		}
	}

	public object Clone()
	{
		StiAdvancedWatermark stiAdvancedWatermark = MemberwiseClone() as StiAdvancedWatermark;
		object obj = TextFont.Clone();
		stiAdvancedWatermark.TextFont = (Font)((obj is Font) ? obj : null);
		return stiAdvancedWatermark;
	}

	internal bool ShouldSerializeTextFont()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		return !((TextFont.Size == 36f) & ((int)TextFont.Style == 0) & (TextFont.Name == "Arial"));
	}

	internal bool ShouldSerializeTextColor()
	{
		return TextColor != DefaultTextColor;
	}

	internal bool ShouldSerializeWeaveMajorColor()
	{
		return WeaveMajorColor != DefaultWeaveMajorColor;
	}

	internal bool ShouldSerializeWeaveMinorColor()
	{
		return WeaveMinorColor != DefaultWeaveMinorColor;
	}

	internal byte[] GetCachedImage()
	{
		return cachedImage;
	}

	internal void PutCachedImage(byte[] image)
	{
		cachedImage = image;
	}

	private void DisposeCachedImage()
	{
		cachedImage = null;
	}

	internal bool ExistImage()
	{
		return ImageBytes != null;
	}

	internal byte[] TakeImage()
	{
		return ImageBytes;
	}

	internal Image TakeGdiImage()
	{
		if (!ExistImage())
		{
			return null;
		}
		return StiImageConverter.BytesToImage(ImageBytes);
	}

	internal Image TryTakeGdiImage()
	{
		if (!ExistImage())
		{
			return null;
		}
		return StiImageConverter.TryBytesToImage(ImageBytes);
	}

	internal void PutImage(Image image)
	{
		ImageBytes = StiImageConverter.ImageToBytes(image, allowNulls: true);
	}

	internal void PutImage(byte[] image)
	{
		ImageBytes = image;
	}

	internal void ResetImage()
	{
		ImageBytes = null;
	}

	public StiAdvancedWatermark()
	{
	}//IL_0016: Unknown result type (might be due to invalid IL or missing references)
	//IL_0020: Expected O, but got Unknown
	//IL_0045: Unknown result type (might be due to invalid IL or missing references)


	[StiUniversalConstructor("Watermark")]
	public StiAdvancedWatermark(bool textEnabled, string text, Color textColor, int textAngle, Font textFont, bool imageEnabled, byte[] imageBytes, double imageMultipleFactor, int imageTransparency, ContentAlignment imageAlignment, bool imageTiling, bool imageStretch, bool imageAspectRatio, bool weaveEnabled, StiFontIcons? weaveMajorIcon, int weaveMajorSize, Color weaveMajorColor, StiFontIcons? weaveMinorIcon, int weaveMinorSize, Color weaveMinorColor, int weaveAngle, int weaveDistance)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		TextEnabled = textEnabled;
		Text = text;
		TextColor = textColor;
		TextAngle = textAngle;
		TextFont = textFont;
		ImageEnabled = imageEnabled;
		ImageBytes = imageBytes;
		ImageMultipleFactor = imageMultipleFactor;
		ImageTransparency = imageTransparency;
		ImageAlignment = imageAlignment;
		ImageTiling = imageTiling;
		ImageStretch = imageStretch;
		ImageAspectRatio = imageAspectRatio;
		WeaveEnabled = weaveEnabled;
		WeaveMajorIcon = weaveMajorIcon;
		WeaveMajorSize = weaveMajorSize;
		WeaveMajorColor = weaveMajorColor;
		WeaveMinorIcon = weaveMinorIcon;
		WeaveMinorSize = weaveMinorSize;
		WeaveMinorColor = weaveMinorColor;
		WeaveAngle = weaveAngle;
		WeaveDistance = weaveDistance;
	}
}
