using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiWatermarkConverter))]
[RefreshProperties(RefreshProperties.All)]
public class StiWatermark : ICloneable, IStiSerializeToCodeAsClass, IStiJsonReportObject, IStiDefault
{
	private byte[] cachedImage;

	private int imageTransparency;

	private byte[] imageBytes;

	private string imageHyperlink = "";

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Invalid comparison between Unknown and I4
			if (!ShouldSerializeFont() && !ShouldSerializeTextBrush() && Text != null && Text.Length == 0 && Angle == 45f && Enabled && ShowImageBehind && !ShowBehind && !RightToLeft && ImageMultipleFactor == 1.0 && ImageTransparency == 0 && ImageBytes == null && ImageHyperlink != null && ImageHyperlink.Length == 0 && (int)ImageAlignment == 32 && !ImageTiling && !ImageStretch && !AspectRatio && EnabledExpression != null)
			{
				return EnabledExpression.Length == 0;
			}
			return false;
		}
	}

	[StiSerializable]
	[Description("Gets or sets font of watermark.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Font Font { get; set; } = new Font("Arial", 100f);


	[StiSerializable]
	[Description("The brush of the watermark, which is used to display text.")]
	[StiOrder(140)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.FromArgb(50, 0, 0, 0));


	[StiSerializable]
	[DefaultValue("")]
	[Description("Gets or sets text of Watermark.")]
	[StiOrder(110)]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string Text { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue(45f)]
	[Description("Gets or sets angle of Watermark.")]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float Angle { get; set; } = 45f;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates where Watermark should be drawing or not.")]
	[StiOrder(100)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Enabled { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates where Watermark image should be drawing behind or in front of page.")]
	[StiOrder(160)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowImageBehind { get; set; } = true;


	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates where Watermark should be drawing behind or in front of page.")]
	[StiOrder(150)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowBehind { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets watermark's output direction.")]
	[NotifyParentProperty(true)]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool RightToLeft { get; set; }

	[StiSerializable]
	[DefaultValue(1.0)]
	[Description("Gets or sets value to multiply by it an image size.")]
	[StiOrder(190)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double ImageMultipleFactor { get; set; } = 1.0;


	[StiSerializable]
	[Description("Gets or sets the transparency of the watermark's image.")]
	[DefaultValue(0)]
	[StiOrder(220)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
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

	[StiNonSerialized]
	[DefaultValue(null)]
	[Description("Gets or sets value watermark's image.")]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiSimpeImageConverter))]
	[StiOrder(170)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
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

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	[Description("Gets or sets value watermark's image.")]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiSimpeImageConverter))]
	[StiOrder(170)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
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
	[DefaultValue("")]
	[Description("Gets or sets value watermark's image hyperlink.")]
	[Browsable(false)]
	public string ImageHyperlink
	{
		get
		{
			return imageHyperlink;
		}
		set
		{
			if (imageHyperlink != value)
			{
				imageHyperlink = value;
				DisposeCachedImage();
			}
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the watermark's image alignment.")]
	[StiOrder(180)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual ContentAlignment ImageAlignment { get; set; } = (ContentAlignment)32;


	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets the watermark's image should be tiled.")]
	[StiOrder(210)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ImageTiling { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that this watermark's image will stretch on the page.")]
	[StiOrder(200)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ImageStretch { get; set; }

	[Browsable(true)]
	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates this watermark's image will save its aspect ratio.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool AspectRatio { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[StiBrowsable(false)]
	[DefaultValue("")]
	[Description("Gets or sets text of EnabledExpression.")]
	[StiOrder(110)]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string EnabledExpression { get; set; } = string.Empty;


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyFont(val, "Font", Font, "Arial", 100f);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ImageHyperlink", ImageHyperlink);
		JsonExtensions.AddPropertyFloat(val, "Angle", Angle, 45f);
		JsonExtensions.AddPropertyBool(val, "Enabled", Enabled, true, false);
		JsonExtensions.AddPropertyBool(val, "ShowImageBehind", ShowImageBehind, true, false);
		JsonExtensions.AddPropertyBool(val, "ShowBehind", ShowBehind, false, false);
		JsonExtensions.AddPropertyBool(val, "RightToLeft", RightToLeft, false, false);
		JsonExtensions.AddPropertyDouble(val, "ImageMultipleFactor", ImageMultipleFactor, 1.0);
		JsonExtensions.AddPropertyInt(val, "ImageTransparency", ImageTransparency, 0);
		JsonExtensions.AddPropertyEnum(val, "ImageAlignment", (Enum)(object)ImageAlignment, (Enum)(object)(ContentAlignment)32);
		JsonExtensions.AddPropertyBool(val, "ImageTiling", ImageTiling, false, false);
		JsonExtensions.AddPropertyBool(val, "ImageStretch", ImageStretch, false, false);
		JsonExtensions.AddPropertyBool(val, "AspectRatio", AspectRatio, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "EnabledExpression", EnabledExpression);
		if (ExistImage())
		{
			val.Add("ImageBytes", JToken.op_Implicit(Convert.ToBase64String(TakeImage())));
		}
		if (((JContainer)val).Count == 0)
		{
			return null;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "TextBrush":
				TextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Text":
				Text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Angle":
				Angle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Enabled":
				Enabled = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowImageBehind":
				ShowImageBehind = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowBehind":
				ShowBehind = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RightToLeft":
				RightToLeft = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ImageMultipleFactor":
				ImageMultipleFactor = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "ImageTransparency":
				imageTransparency = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Image":
				PutImage(StiJsonReportObjectHelper.DeserializeImage(item));
				break;
			case "ImageBytes":
				PutImage(StiJsonReportObjectHelper.DeserializeImage(item));
				break;
			case "ImageHyperlink":
				ImageHyperlink = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ImageAlignment":
				ImageAlignment = StiJsonReportObjectHelper.DeserializeEnum<ContentAlignment>(item);
				break;
			case "ImageTiling":
				ImageTiling = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ImageStretch":
				ImageStretch = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AspectRatio":
				AspectRatio = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "EnabledExpression":
				EnabledExpression = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public object Clone()
	{
		StiWatermark stiWatermark = (StiWatermark)MemberwiseClone();
		object obj = Font.Clone();
		stiWatermark.Font = (Font)((obj is Font) ? obj : null);
		object obj2 = TextBrush.Clone();
		stiWatermark.TextBrush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		return stiWatermark;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeFont()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		return !((Font.Size == 100f) & ((int)Font.Style == 0) & (Font.Name == "Arial"));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeTextBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TextBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)TextBrush).Color == Color.FromArgb(50, 0, 0, 0));
		}
		return true;
	}

	public byte[] GetImage(StiReport report)
	{
		byte[] array = (string.IsNullOrWhiteSpace(ImageHyperlink) ? TakeImage() : StiHyperlinkProcessor.GetBytes(report, ImageHyperlink));
		if (ImageTransparency == 0)
		{
			return array;
		}
		if (array == null)
		{
			return null;
		}
		if (cachedImage == null)
		{
			Image val = StiImageConverter.BytesToImage(array, 200, 200, true, false);
			Bitmap transparentedImage = StiImageTransparenceHelper.GetTransparentedImage(val, 1f - (float)ImageTransparency / 255f);
			cachedImage = StiImageConverter.ImageToBytes((Image)(object)transparentedImage, false);
		}
		return cachedImage;
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
		return StiImageConverter.BytesToImage(ImageBytes, 200, 200, true, false);
	}

	internal Image TryTakeGdiImage()
	{
		if (!ExistImage())
		{
			return null;
		}
		return StiImageConverter.TryBytesToImage(ImageBytes, 200, 200, true, false);
	}

	internal void PutImage(Image image)
	{
		ImageBytes = StiImageConverter.ImageToBytes(image, true);
	}

	internal void PutImage(byte[] image)
	{
		ImageBytes = image;
	}

	internal void ResetImage()
	{
		ImageBytes = null;
	}

	internal bool ImageIsMetafile()
	{
		return StiImageHelper.IsMetafile(ImageBytes);
	}

	public StiWatermark()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		ImageHyperlink = string.Empty;
	}

	public StiWatermark(StiBrush textBrush, string text, float angle, Font font, bool showBehind)
		: this(textBrush, text, angle, font, showBehind, enabled: true, aspectRatio: false)
	{
	}

	public StiWatermark(StiBrush textBrush, string text, float angle, Font font, bool showBehind, bool enabled, bool aspectRatio)
		: this(textBrush, text, angle, font, showBehind, enabled, aspectRatio, rightToLeft: false)
	{
	}

	public StiWatermark(StiBrush textBrush, string text, float angle, Font font, bool showBehind, bool enabled, bool aspectRatio, bool rightToLeft)
		: this(textBrush, text, angle, font, showBehind, enabled, aspectRatio, rightToLeft, string.Empty)
	{
	}

	public StiWatermark(StiBrush textBrush, string text, float angle, Font font, bool showBehind, bool enabled, bool aspectRatio, bool rightToLeft, string imageHyperlink)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		ImageHyperlink = imageHyperlink;
		TextBrush = textBrush;
		Text = text;
		Angle = angle;
		Font = font;
		ShowBehind = showBehind;
		Enabled = enabled;
		AspectRatio = aspectRatio;
		RightToLeft = rightToLeft;
	}
}
