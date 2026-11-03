using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiImage), "Stimulsoft.Report.Images.Components.StiImage.png")]
[StiToolbox(true)]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiDesigner("Stimulsoft.Report.Components.Design.StiImageDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfImageDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiImageGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiImageWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiImageQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfImageQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiImageV1Builder))]
[StiV2Builder(typeof(StiImageV2Builder))]
[StiContextTool(typeof(IStiBreakable))]
public class StiImage : StiView, IStiBreakable, IStiGlobalizedName
{
	protected static object PropertyCanBreak = new object();

	private object imageURLValue;

	private static readonly object EventGetImageURL = new object();

	private static readonly object EventGetImageData = new object();

	protected static object PropertyProcessingDuplicates = new object();

	private byte[] imageBytes;

	protected static object PropertyMargins = new object();

	private string file = string.Empty;

	private string dataColumn = string.Empty;

	private StiFontIcons? icon;

	private Color iconColor = StiColor.Get("4472C4");

	public override StiComponentId ComponentId => StiComponentId.StiImage;

	public override string HelpUrl => "user-manual/report_internals_graphic_information_output.htm";

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[ParenthesizePropertyName(true)]
	[Description("Gets or sets special identificator which will be used for report globalization.")]
	[DefaultValue("")]
	[Editor("Stimulsoft.Report.Design.StiGlobabalizationManagerEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string GlobalizedName { get; set; } = "";


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool CanBreak
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyCanBreak, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyCanBreak, value, false);
		}
	}

	public override int ToolboxPosition => 106;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => StiLocalization.Get("Components", "StiImage");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 100.0, 100.0);

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets image URL.")]
	public object ImageURLValue
	{
		get
		{
			return imageURLValue;
		}
		set
		{
			if (imageURLValue != value)
			{
				imageURLValue = value;
				if (!ExistImage() && (base.IsDesigning || (Report != null && Report.IsRendering)) && value != null)
				{
					PutImageToDraw(GetImageFromUrl());
				}
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a component image URL.")]
	[Editor("Stimulsoft.Report.Components.Design.StiImageExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiImageURLExpression ImageURL
	{
		get
		{
			return new StiImageURLExpression(this, "ImageURL");
		}
		set
		{
			value?.Set(this, "ImageURL", value.Value);
			ResetImageToDraw();
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a component image property.")]
	[Editor("Stimulsoft.Report.Components.Design.StiImageExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiImageDataExpression ImageData
	{
		get
		{
			return new StiImageDataExpression(this, "ImageData");
		}
		set
		{
			value?.Set(this, "ImageData", value.Value);
			ResetImageToDraw();
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting image url for the component.")]
	public virtual StiGetImageURLEvent GetImageURLEvent
	{
		get
		{
			return new StiGetImageURLEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting image for the component.")]
	public virtual StiGetImageDataEvent GetImageDataEvent
	{
		get
		{
			return new StiGetImageDataEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[DefaultValue(StiImageProcessingDuplicatesType.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value which indicates how report engine processes duplicated images.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiImageProcessingDuplicatesType ProcessingDuplicates
	{
		get
		{
			return (StiImageProcessingDuplicatesType)((StiService)this).Properties.Get(PropertyProcessingDuplicates, (object)StiImageProcessingDuplicatesType.None);
		}
		set
		{
			((StiService)this).Properties.Set(PropertyProcessingDuplicates, (object)value, (object)StiImageProcessingDuplicatesType.None);
		}
	}

	[Browsable(true)]
	[DefaultValue(StiImageRotation.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets value which indicates how to rotate an image before output.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiImageRotation ImageRotation { get; set; }

	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets the image.")]
	[Editor("Stimulsoft.Report.Components.Design.StiImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiSimpeImageConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Image Image
	{
		get
		{
			return TakeGdiImage();
		}
		set
		{
			if (value != StiNullImage.Null)
			{
				PutImage(value);
			}
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets the image.")]
	[Editor("Stimulsoft.Report.Components.Design.StiImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiSimpeImageConverter))]
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
				PutImageToDraw(value);
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[Description("Gets or sets image margins.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiMargins Margins { get; set; } = new StiMargins();


	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiImageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[DefaultValue("")]
	[Description("Gets or sets the path to the file that contains the image.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string File
	{
		get
		{
			return file;
		}
		set
		{
			if (file != value)
			{
				file = value;
				if (!ExistImage() && (base.IsDesigning || (Report != null && !Report.IsSerializing)))
				{
					PutImageToDraw(GetImageFromFile());
				}
			}
			else
			{
				ResetImageToDraw();
			}
		}
	}

	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiImageDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[DefaultValue("")]
	[Description("Gets or sets a name of the column that contains the image.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string DataColumn
	{
		get
		{
			return dataColumn;
		}
		set
		{
			dataColumn = value;
			ResetImageToDraw();
		}
	}

	[StiSerializable]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[Description("Gets or sets icon.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	public StiFontIcons? Icon
	{
		get
		{
			return icon;
		}
		set
		{
			icon = value;
			ResetImageToDraw();
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets icon color.")]
	[StiOrder(256)]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public Color IconColor
	{
		get
		{
			return iconColor;
		}
		set
		{
			iconColor = value;
			ResetImageToDraw();
		}
	}

	public event StiValueEventHandler GetImageURL
	{
		add
		{
			base.Events.AddHandler(EventGetImageURL, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetImageURL, value);
		}
	}

	public event StiGetImageDataEventHandler GetImageData
	{
		add
		{
			base.Events.AddHandler(EventGetImageData, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetImageData, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Invalid comparison between Unknown and I4
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "GlobalizedName", GlobalizedName);
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.AddPropertyJObject(val, "GetImageURLEvent", GetImageURLEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetImageDataEvent", GetImageDataEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyEnum(val, "ProcessingDuplicates", (Enum)ProcessingDuplicates, (Enum)StiImageProcessingDuplicatesType.None);
		JsonExtensions.AddPropertyEnum(val, "ImageRotation", (Enum)ImageRotation, (Enum)StiImageRotation.None);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "File", File);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataColumn", DataColumn);
		if (Icon.HasValue)
		{
			JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)(object)Icon);
		}
		JsonExtensions.AddPropertyColor(val, "IconColor", IconColor, Color.FromArgb(68, 114, 196));
		if ((int)mode == 1)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ImageURLValue", ImageURLValue as string);
		}
		else
		{
			JsonExtensions.AddPropertyJObject(val, "ImageURL", ImageURL.SaveToJsonObject(mode));
			JsonExtensions.AddPropertyJObject(val, "ImageData", ImageData.SaveToJsonObject(mode));
			if (ExistImage())
			{
				val.Add("ImageBytes", JToken.op_Implicit(System.Convert.ToBase64String(TakeImage())));
			}
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "GlobalizedName":
				GlobalizedName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "CanBreak":
				CanBreak = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ImageURLValue":
				imageURLValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ImageURL":
			{
				StiImageURLExpression stiImageURLExpression = new StiImageURLExpression();
				stiImageURLExpression.LoadFromJsonObject((JObject)item.Value);
				ImageURL = stiImageURLExpression;
				break;
			}
			case "ImageData":
			{
				StiImageDataExpression stiImageDataExpression = new StiImageDataExpression();
				stiImageDataExpression.LoadFromJsonObject((JObject)item.Value);
				ImageData = stiImageDataExpression;
				break;
			}
			case "GetImageURLEvent":
			{
				StiGetImageURLEvent stiGetImageURLEvent = new StiGetImageURLEvent();
				stiGetImageURLEvent.LoadFromJsonObject((JObject)item.Value);
				GetImageURLEvent = stiGetImageURLEvent;
				break;
			}
			case "GetImageDataEvent":
			{
				StiGetImageDataEvent stiGetImageDataEvent = new StiGetImageDataEvent();
				stiGetImageDataEvent.LoadFromJsonObject((JObject)item.Value);
				GetImageDataEvent = stiGetImageDataEvent;
				break;
			}
			case "ProcessingDuplicates":
				ProcessingDuplicates = StiJsonReportObjectHelper.DeserializeEnum<StiImageProcessingDuplicatesType>(item);
				break;
			case "ImageRotation":
				ImageRotation = StiJsonReportObjectHelper.DeserializeEnum<StiImageRotation>(item);
				break;
			case "Image":
				PutImage(StiJsonReportObjectHelper.DeserializeImage(item));
				break;
			case "ImageBytes":
				PutImage(StiJsonReportObjectHelper.DeserializeImage(item));
				break;
			case "File":
				file = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DataColumn":
				dataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Icon":
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiFontIcons>(item);
				break;
			case "IconColor":
				IconColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Invalid comparison between Unknown and I4
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.ImageEditor() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.ImageAdditional, new StiPropertyObject[4]
			{
				propertiesHelper.AspectRatio(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.Stretch()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.ImageAdditional, new StiPropertyObject[8]
			{
				propertiesHelper.AspectRatio(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.ImageRotation(),
				propertiesHelper.Margins(),
				propertiesHelper.MultipleFactor(),
				propertiesHelper.ProcessingDuplicates(),
				propertiesHelper.Stretch()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.ImageAdditional, new StiPropertyObject[9]
			{
				propertiesHelper.AspectRatio(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.ImageRotation(),
				propertiesHelper.Margins(),
				propertiesHelper.MultipleFactor(),
				propertiesHelper.ProcessingDuplicates(),
				propertiesHelper.Smoothing(),
				propertiesHelper.Stretch()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[4]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[6]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height(),
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.IconColor()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.IconColor(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[5]
			{
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[10]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[11]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.Printable(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[1] { propertiesHelper.Name() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[6]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.GlobalizedName(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
		}
		return stiPropertyCollection;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetBookmarkEvent,
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetPointerEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.AfterPrintEvent,
			StiPropertyEventId.BeforePrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetImageDataEvent,
			StiPropertyEventId.GetImageURLEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override Image GetImage(ref float zoom, StiExportFormat format)
	{
		if (ExistImageToDraw() && format == StiExportFormat.HtmlTable)
		{
			zoom = 1f;
			Image val = StiImageConverter.BytesToImage(ImageBytesToDraw, 200, 200, true, false);
			if (val != null && (((object)val.RawFormat).Equals((object?)ImageFormat.Jpeg) || ((object)val.RawFormat).Equals((object?)ImageFormat.Gif) || ((object)val.RawFormat).Equals((object?)ImageFormat.Png) || ((object)val.RawFormat).Equals((object?)ImageFormat.Bmp)))
			{
				return val;
			}
		}
		return base.GetImage(ref zoom, format);
	}

	protected virtual byte[] GetImageFromFile()
	{
		if (string.IsNullOrEmpty(file))
		{
			return null;
		}
		string text = StiExpressionHelper.ParseText(Page, File);
		if (!string.IsNullOrEmpty(StiOptions.Engine.Image.AbsolutePathOfImages))
		{
			text = Path.Combine(StiOptions.Engine.Image.AbsolutePathOfImages, text);
		}
		if (!System.IO.File.Exists(text))
		{
			return null;
		}
		try
		{
			return System.IO.File.Exists(text) ? System.IO.File.ReadAllBytes(text) : null;
		}
		catch (Exception ex)
		{
			if (base.IsDesigning)
			{
				return new byte[0];
			}
			string text2 = "Image can't be loaded from file '" + file + "' in image component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text2);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text2);
		}
		return null;
	}

	protected virtual byte[] GetImageFromUrl()
	{
		string text = (base.IsDesigning ? ImageURL.Value : (ImageURLValue as string));
		try
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (StiHyperlinkProcessor.IsServerHyperlink(text))
				{
					return StiStimulsoftServerResource.GetImage(this, StiHyperlinkProcessor.GetServerNameFromHyperlink(text));
				}
				string resourceNameFromHyperlink = StiHyperlinkProcessor.GetResourceNameFromHyperlink(text);
				if (resourceNameFromHyperlink != null)
				{
					return StiHyperlinkProcessor.GetBytes(Report, text);
				}
				string variableNameFromHyperlink = StiHyperlinkProcessor.GetVariableNameFromHyperlink(text);
				if (variableNameFromHyperlink != null)
				{
					return StiHyperlinkProcessor.GetBytes(Report, text);
				}
				if (base.IsDesigning && !StiOptions.Configuration.IsWPF && StiImageHyperlinkLoader.AllowAsyncLoading)
				{
					StiImageHyperlinkLoader.Load(this, text);
					return null;
				}
				return StiDownloadCache.Get(text, Report?.CookieContainer, Report?.HttpHeadersContainer);
			}
		}
		catch (Exception ex)
		{
			if (base.IsDesigning)
			{
				return new byte[0];
			}
			string text2 = "Image can't be loaded from URL '" + text + "' in image component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text2);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text2);
		}
		return null;
	}

	protected virtual byte[] GetImageFromDataColumn()
	{
		try
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, DataColumn);
			return StiImageHelper.GetImageBytesFromObject(dataFromDataColumn);
		}
		catch (Exception ex)
		{
			if (base.IsDesigning)
			{
				return new byte[0];
			}
			string text = "Image can't be loaded from data column '" + DataColumn + "' in image component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
		return null;
	}

	protected virtual byte[] GetImageFromIcon()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!Icon.HasValue)
			{
				return null;
			}
			RectangleD paintRectangle = GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			Image imageObject = StiFontIconsHelper.ConvertFontIconToImage(Icon.GetValueOrDefault(), IconColor, (int)(((RectangleD)(ref paintRectangle)).Width * StiScale.Factor), (int)(((RectangleD)(ref paintRectangle)).Height * StiScale.Factor));
			return StiImageHelper.GetImageBytesFromObject(imageObject);
		}
		catch (Exception ex)
		{
			if (base.IsDesigning)
			{
				return new byte[0];
			}
			string text = "Image can't be loaded from data column '" + DataColumn + "' in image component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
		return null;
	}

	public override byte[] GetImageFromSource()
	{
		byte[] imageFromFile = GetImageFromFile();
		if (imageFromFile != null)
		{
			return imageFromFile;
		}
		imageFromFile = GetImageFromUrl();
		if (imageFromFile != null)
		{
			return imageFromFile;
		}
		imageFromFile = GetImageFromIcon();
		if (imageFromFile != null)
		{
			return imageFromFile;
		}
		return GetImageFromDataColumn();
	}

	public bool Break(StiComponent dividedComponent, double divideFactor, ref double divideLine)
	{
		divideLine = 0.0;
		bool result = true;
		if (ExistImageToDraw())
		{
			byte[] image = TakeImageToDraw();
			PutImageToDraw(StiComponentDivider.BreakImage(dividedComponent as StiImage, ref image, divideFactor));
			((StiImage)dividedComponent).PutImageToDraw(image);
		}
		return result;
	}

	public override void OnResizeComponent(SizeD oldSize, SizeD newSize)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		base.OnResizeComponent(oldSize, newSize);
		string value = (base.IsDesigning ? ImageURL.Value : (ImageURLValue as string));
		if (string.IsNullOrEmpty(file) && string.IsNullOrEmpty(value) && Icon.HasValue)
		{
			ResetImageToDraw();
		}
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (GetImageURLEvent != null)
		{
			stiEventsCollection.Add(GetImageURLEvent);
		}
		if (GetImageDataEvent != null)
		{
			stiEventsCollection.Add(GetImageDataEvent);
		}
		return stiEventsCollection;
	}

	public override void InvokeEvents()
	{
		try
		{
			base.InvokeEvents();
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				if ((object)base.Events[EventGetImageURL] != null && (ImageURLValue == null || (ImageURLValue is string && ((string)ImageURLValue).Length == 0)))
				{
					StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
					InvokeGetImageURL(this, stiValueEventArgs);
					ImageURLValue = stiValueEventArgs.Value;
				}
			}
			else if (ImageURLValue == null || (ImageURLValue is string && ((string)ImageURLValue).Length == 0))
			{
				StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
				InvokeGetImageURL(this, stiValueEventArgs2);
				ImageURLValue = stiValueEventArgs2.Value;
			}
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				if ((object)base.Events[EventGetImageData] != null)
				{
					StiGetImageDataEventArgs stiGetImageDataEventArgs = new StiGetImageDataEventArgs();
					InvokeGetImageData(this, stiGetImageDataEventArgs);
					if (stiGetImageDataEventArgs.Value != null)
					{
						PutImage(stiGetImageDataEventArgs.Value);
					}
				}
			}
			else if (!string.IsNullOrEmpty(ImageData.Value))
			{
				StiGetImageDataEventArgs stiGetImageDataEventArgs2 = new StiGetImageDataEventArgs();
				InvokeGetImageData(this, stiGetImageDataEventArgs2);
				if (stiGetImageDataEventArgs2.Value != null)
				{
					PutImage(stiGetImageDataEventArgs2.Value);
				}
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoEvents...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
		}
	}

	protected virtual void OnGetImageURL(StiValueEventArgs e)
	{
	}

	public void InvokeGetImageURL(object sender, StiValueEventArgs e)
	{
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				OnGetImageURL(e);
				if (base.Events[EventGetImageURL] is StiValueEventHandler stiValueEventHandler)
				{
					stiValueEventHandler(sender, e);
				}
			}
			else
			{
				OnGetImageURL(e);
				object obj = StiParser.ParseTextValue(ImageURL.Value, this, sender);
				if (obj != null)
				{
					e.Value = obj.ToString();
				}
				if (base.Events[EventGetImageURL] is StiValueEventHandler stiValueEventHandler2)
				{
					stiValueEventHandler2(sender, e);
				}
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetImageURLEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in ImageURL property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetImageData(StiGetImageDataEventArgs e)
	{
	}

	public void InvokeGetImageData(object sender, StiGetImageDataEventArgs e)
	{
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				OnGetImageData(e);
				if (base.Events[EventGetImageData] is StiGetImageDataEventHandler stiGetImageDataEventHandler)
				{
					stiGetImageDataEventHandler(sender, e);
				}
			}
			else
			{
				OnGetImageData(e);
				object obj = StiParser.ParseTextValue(ImageData.Value, this, sender);
				if (obj is Image)
				{
					e.Value = (Image)((obj is Image) ? obj : null);
				}
				if (base.Events[EventGetImageData] is StiGetImageDataEventHandler stiGetImageDataEventHandler2)
				{
					stiGetImageDataEventHandler2(sender, e);
				}
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetImageDataEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in ImageData property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	private bool ShouldSerializeImage()
	{
		return Image != null;
	}

	private bool ShouldSerializeMargins()
	{
		return !Margins.IsDefault;
	}

	private bool ShouldSerializeIconColor()
	{
		return IconColor != StiColor.Get("4472C4");
	}

	public void UpdateImageToDrawInDesigner()
	{
		if (ExistImageToDraw() || Report == null || Report.IsPageDesigner)
		{
			return;
		}
		if (Report != null && Report.Dictionary != null && base.IsDesigning && !Report.Info.IsComponentsMoving)
		{
			try
			{
				if (ImageData != null)
				{
					PutImageToDraw(StiVariableImageProcessor.GetImage(Report, ImageData.Value));
				}
				if (!ExistImageToDraw())
				{
					PutImageToDraw(GetImageFromSource());
				}
			}
			catch (Exception ex)
			{
				ResetImageToDraw();
				Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
		if (!StiOptions.Designer.UseGlobalizationManager || !ExistImage() || Report.GlobalizationManager == null || Report.GlobalizationManager is StiNullGlobalizationManager || string.IsNullOrWhiteSpace(GlobalizedName))
		{
			return;
		}
		try
		{
			object @object = Report.GlobalizationManager.GetObject(GlobalizedName);
			PutImageToDraw((Image)((@object is Image) ? @object : null));
		}
		catch
		{
		}
	}

	public RectangleD ConvertImageMargins(RectangleD rect, bool convert)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		StiMargins margins = Margins;
		if (margins.IsEmpty)
		{
			return rect;
		}
		double num = (convert ? Page.Zoom : 1.0);
		double num2 = margins.Left;
		double right = margins.Right;
		double num3 = margins.Top;
		double bottom = margins.Bottom;
		if (num2 != 0.0)
		{
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + num2 * num;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - num2 * num;
		}
		if (num3 != 0.0)
		{
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + num3 * num;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - num3 * num;
		}
		if (right != 0.0)
		{
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - right * num;
		}
		if (bottom != 0.0)
		{
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - bottom * num;
		}
		return rect;
	}

	public override StiComponent CreateNew()
	{
		return new StiImage();
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
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (!ExistImage())
		{
			return null;
		}
		if (ImageIsSvg())
		{
			RectangleD paintRectangle = GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			return StiImageConverter.BytesToImage(ImageBytes, (int)((RectangleD)(ref paintRectangle)).Width, (int)((RectangleD)(ref paintRectangle)).Height, base.Stretch, base.AspectRatio);
		}
		return StiImageConverter.BytesToImage(ImageBytes, 200, 200, true, false);
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

	internal bool ImageIsSvg()
	{
		return StiSvgHelper.IsSvg(ImageBytes);
	}

	internal void ResetAllImageProperties()
	{
		ResetImage();
		DataColumn = string.Empty;
		ImageURL.Value = string.Empty;
		ImageData.Value = string.Empty;
		File = string.Empty;
	}

	internal void CopyAllImageProperties(StiImage image)
	{
		if (image != null)
		{
			ImageBytes = image.ImageBytes;
			DataColumn = image.DataColumn;
			ImageURL.Value = image.ImageURL.Value;
			ImageData.Value = image.ImageData.Value;
			File = image.File;
		}
	}

	public StiImage()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiImage(RectangleD rect)
		: base(rect)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
