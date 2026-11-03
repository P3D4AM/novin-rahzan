using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiGdiPainter(typeof(StiViewGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiViewWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiViewV1Builder))]
[StiV2Builder(typeof(StiViewV2Builder))]
public abstract class StiView : StiComponent, IStiHorAlignment, IStiVertAlignment, IStiBorder, IStiExportImageExtended, IStiExportImage, IStiBrush
{
	internal byte[] imageBytesToDraw;

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the horizontal alignment of an object.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiHorAlignment HorAlignment { get; set; } = (StiHorAlignment)1;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the vertical alignment of an object.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiVertAlignment VertAlignment { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets border of the component.")]
	[StiOrder(120)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[DefaultValue(true)]
	[Description("Gets or sets smoothing mode for drawing image.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Smoothing { get; set; } = true;


	[Browsable(false)]
	public bool IsCachedImage { get; set; }

	[Browsable(false)]
	[Description("Gets or sets the WPF visual object that appeared as a result of the component rendering. This property accepts objects of two types: DrawingVisual and ImageSource.")]
	public object ObjectToDraw { get; set; }

	[Browsable(false)]
	[Description("Gets or sets the image that appeared as a result of the component rendering.")]
	public virtual Image ImageToDraw
	{
		get
		{
			return TakeGdiImageToDraw();
		}
		set
		{
			PutImageToDraw(value);
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the image bytes that appeared as a result of the component rendering.")]
	public virtual byte[] ImageBytesToDraw
	{
		get
		{
			if (base.IsDesigning || imageBytesToDraw != null)
			{
				return imageBytesToDraw;
			}
			return StiFileImageCacheV2.LoadImage(Report, base.Guid);
		}
		set
		{
			IsCachedImage = false;
			if (Report != null && !base.IsDesigning && StiOptions.Engine.ImageCache.Enabled && value != null)
			{
				if (base.Guid == null)
				{
					NewGuid();
				}
				StiFileImageCacheV2.SaveImage(Report, base.Guid, value);
				IsCachedImage = true;
			}
			else
			{
				imageBytesToDraw = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that this component will stretch the image till the image will get size equal in its size on the page.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool Stretch { get; set; }

	[Browsable(true)]
	[DefaultValue(1.0)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[Description("Gets or sets value to multiply by it an image size.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public double MultipleFactor { get; set; } = 1.0;


	[Browsable(true)]
	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that the image will save its aspect ratio.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool AspectRatio { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Invalid comparison between Unknown and I4
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)(object)HorAlignment, (Enum)(object)(StiHorAlignment)1);
		JsonExtensions.AddPropertyEnum(val, "VertAlignment", (Enum)(object)VertAlignment, (Enum)(object)(StiVertAlignment)0);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBool(val, "Smoothing", Smoothing, true, false);
		JsonExtensions.AddPropertyBool(val, "Stretch", Stretch, false, false);
		JsonExtensions.AddPropertyDouble(val, "MultipleFactor", MultipleFactor, 1.0);
		JsonExtensions.AddPropertyBool(val, "AspectRatio", AspectRatio, false, false);
		if ((int)mode == 1 && ExistImageToDraw())
		{
			JsonExtensions.AddPropertyByteArray(val, "ImageBytesToDraw", TakeImageToDraw());
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "HorAlignment":
				HorAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiHorAlignment>(item);
				break;
			case "VertAlignment":
				VertAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiVertAlignment>(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Smoothing":
				Smoothing = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Stretch":
				Stretch = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MultipleFactor":
				MultipleFactor = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "AspectRatio":
				AspectRatio = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ImageToDraw":
				PutImageToDraw(StiJsonReportObjectHelper.DeserializeImage(item));
				break;
			case "ImageBytesToDraw":
				PutImageToDraw(StiJsonReportObjectHelper.DeserializeImage(item));
				break;
			}
		}
	}

	public override object Clone(bool cloneProperties)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		StiView stiView = (StiView)base.Clone(cloneProperties);
		if (Border != null)
		{
			stiView.Border = (StiBorder)Border.Clone();
		}
		else
		{
			stiView.Border = null;
		}
		if (Brush != null)
		{
			stiView.Brush = (StiBrush)Brush.Clone();
		}
		else
		{
			stiView.Brush = null;
		}
		stiView.HorAlignment = HorAlignment;
		stiView.VertAlignment = VertAlignment;
		return stiView;
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		if (ObjectToDraw != null)
		{
			StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)1);
			return painter.GetImage(this, ref zoom, format);
		}
		StiPainter painter2 = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter2.GetImage(this, ref zoom, format);
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		return true;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.Transparent);
		}
		return true;
	}

	public override SizeD GetActualSize()
	{
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		SizeD result = default(SizeD);
		((SizeD)(ref result))._002Ector(Width, Height);
		if (!CanGrow && !CanShrink)
		{
			return result;
		}
		if (!ExistImageToDraw())
		{
			return SizeD.Empty;
		}
		StiUnit unit = Page.Unit;
		double num = unit.ConvertToHInches(Width);
		double num2 = unit.ConvertToHInches(Height);
		Image val = TryTakeGdiImageToDraw();
		try
		{
			SizeD val2;
			if (val != null)
			{
				if (CanGrow)
				{
					if ((double)val.Width * MultipleFactor > num)
					{
						((SizeD)(ref result)).Width = unit.ConvertFromHInches((double)val.Width * MultipleFactor);
					}
					if ((double)val.Height * MultipleFactor > num2)
					{
						((SizeD)(ref result)).Height = unit.ConvertFromHInches((double)val.Height * MultipleFactor);
					}
				}
				if (CanShrink)
				{
					if ((double)val.Width * MultipleFactor < num)
					{
						((SizeD)(ref result)).Width = unit.ConvertFromHInches((double)val.Width * MultipleFactor);
					}
					if ((double)val.Height * MultipleFactor < num2)
					{
						((SizeD)(ref result)).Height = unit.ConvertFromHInches((double)val.Height * MultipleFactor);
					}
				}
				if (AspectRatio && CanShrink)
				{
					val2 = MinSize;
					if (!((SizeD)(ref val2)).IsEmpty)
					{
						goto IL_016b;
					}
					val2 = MaxSize;
					if (!((SizeD)(ref val2)).IsEmpty)
					{
						goto IL_016b;
					}
				}
			}
			goto end_IL_0061;
			IL_016b:
			double num3 = ((SizeD)(ref result)).Width;
			val2 = MinSize;
			if (((SizeD)(ref val2)).Width != 0.0)
			{
				double val3 = num3;
				val2 = MinSize;
				num3 = Math.Max(val3, ((SizeD)(ref val2)).Width);
			}
			val2 = MaxSize;
			if (((SizeD)(ref val2)).Width != 0.0)
			{
				double val4 = num3;
				val2 = MaxSize;
				num3 = Math.Min(val4, ((SizeD)(ref val2)).Width);
			}
			double num4 = ((SizeD)(ref result)).Height;
			val2 = MinSize;
			if (((SizeD)(ref val2)).Height != 0.0)
			{
				double val5 = num4;
				val2 = MinSize;
				num4 = Math.Max(val5, ((SizeD)(ref val2)).Height);
			}
			val2 = MaxSize;
			if (((SizeD)(ref val2)).Height != 0.0)
			{
				double val6 = num4;
				val2 = MaxSize;
				num4 = Math.Min(val6, ((SizeD)(ref val2)).Height);
			}
			try
			{
				double num5 = (double)val.Width / (double)val.Height;
				double num6 = num3 / num4;
				if (num5 > num6)
				{
					((SizeD)(ref result)).Height = num3 / num5;
				}
			}
			catch
			{
			}
			end_IL_0061:;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return result;
	}

	public SizeD GetRealSize()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		SizeD result = default(SizeD);
		((SizeD)(ref result))._002Ector(Width, Height);
		if (!CanGrow && !CanShrink)
		{
			return result;
		}
		if (!ExistImageToDraw())
		{
			return SizeD.Empty;
		}
		Image val = TryTakeGdiImageToDraw();
		try
		{
			if (val != null)
			{
				StiUnit unit = Page.Unit;
				double num = unit.ConvertToHInches(Width);
				double num2 = unit.ConvertToHInches(Height);
				if (CanGrow)
				{
					if ((double)val.Width * MultipleFactor > num)
					{
						((SizeD)(ref result)).Width = unit.ConvertFromHInches((double)val.Width * MultipleFactor);
					}
					if ((double)val.Height * MultipleFactor > num2)
					{
						((SizeD)(ref result)).Height = unit.ConvertFromHInches((double)val.Height * MultipleFactor);
					}
				}
				if ((double)val.Width * MultipleFactor < num)
				{
					((SizeD)(ref result)).Width = unit.ConvertFromHInches((double)val.Width * MultipleFactor);
				}
				if ((double)val.Height * MultipleFactor < num2)
				{
					((SizeD)(ref result)).Height = unit.ConvertFromHInches((double)val.Height * MultipleFactor);
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return result;
	}

	public abstract byte[] GetImageFromSource();

	internal bool ExistImageToDraw()
	{
		return ImageBytesToDraw != null;
	}

	internal byte[] TakeImageToDraw()
	{
		return ImageBytesToDraw;
	}

	internal Image TakeGdiImageToDraw(float svgScale = 1f)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (!ExistImageToDraw())
		{
			return null;
		}
		if (ImageToDrawIsSvg())
		{
			RectangleD paintRectangle = GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			try
			{
				return StiSvgHelper.ConvertSvgToImage(ImageBytesToDraw, (int)(((RectangleD)(ref paintRectangle)).Width * (double)svgScale), (int)(((RectangleD)(ref paintRectangle)).Height * (double)svgScale), Stretch, AspectRatio, true);
			}
			catch (OutOfMemoryException)
			{
				if (svgScale > 1f)
				{
					svgScale = (svgScale + 1f) / 2f;
					try
					{
						return StiSvgHelper.ConvertSvgToImage(ImageBytesToDraw, (int)(((RectangleD)(ref paintRectangle)).Width * (double)svgScale), (int)(((RectangleD)(ref paintRectangle)).Height * (double)svgScale), Stretch, AspectRatio, true);
					}
					catch (OutOfMemoryException)
					{
						return StiSvgHelper.ConvertSvgToImage(ImageBytesToDraw, (int)((RectangleD)(ref paintRectangle)).Width, (int)((RectangleD)(ref paintRectangle)).Height, Stretch, AspectRatio, false);
					}
				}
				return (Image)new Bitmap(1, 1);
			}
		}
		Image val = StiImageConverter.BytesToImage(ImageBytesToDraw, 200, 200, true, false);
		if (StiOptions.Engine.Image.RotateImageByExifOrientationData && val != null)
		{
			RotateImageByExifOrientationData(val);
		}
		return val;
	}

	public static void RotateImageByExifOrientationData(Image img, bool updateExifData = true)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int num = 274;
			if (!img.PropertyIdList.Contains(num))
			{
				return;
			}
			PropertyItem propertyItem = img.GetPropertyItem(num);
			RotateFlipType val = (RotateFlipType)0;
			switch (propertyItem.Value[0])
			{
			case 2:
				val = (RotateFlipType)4;
				break;
			case 3:
				val = (RotateFlipType)2;
				break;
			case 4:
				val = (RotateFlipType)6;
				break;
			case 5:
				val = (RotateFlipType)5;
				break;
			case 6:
				val = (RotateFlipType)1;
				break;
			case 7:
				val = (RotateFlipType)7;
				break;
			case 8:
				val = (RotateFlipType)3;
				break;
			}
			if ((int)val != 0)
			{
				img.RotateFlip(val);
				if (updateExifData)
				{
					img.RemovePropertyItem(num);
				}
			}
		}
		catch
		{
		}
	}

	internal Image TryTakeGdiImageToDraw()
	{
		if (!ExistImageToDraw())
		{
			return null;
		}
		return StiImageConverter.TryBytesToImage(ImageBytesToDraw, 200, 200, true, false);
	}

	internal void PutImageToDraw(Image image)
	{
		ImageBytesToDraw = StiImageConverter.ImageToBytes(image, true);
	}

	internal void PutImageToDraw(byte[] image)
	{
		ImageBytesToDraw = image;
	}

	internal void ResetImageToDraw()
	{
		ImageBytesToDraw = null;
	}

	internal bool ImageToDrawIsMetafile()
	{
		return StiImageHelper.IsMetafile(ImageBytesToDraw);
	}

	internal bool ImageToDrawIsSvg()
	{
		return StiSvgHelper.IsSvg(ImageBytesToDraw);
	}

	public StiView()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiView(RectangleD rect)
		: base(rect)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
	//IL_0008: Unknown result type (might be due to invalid IL or missing references)
	//IL_0012: Expected O, but got Unknown
	//IL_0018: Unknown result type (might be due to invalid IL or missing references)
	//IL_0022: Expected O, but got Unknown
	//IL_0039: Unknown result type (might be due to invalid IL or missing references)

}
