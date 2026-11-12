using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Image.Design;
using Stimulsoft.Dashboard.Components.Text;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Image;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiImage.png")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiImageElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiImageElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiImageElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiImageElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiImageElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiImageElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiImageElement : StiElement, IStiSimpleShadow, IStiCornerRadius, IStiHorAlignment, IStiVertAlignment, IStiImageElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiGroupElement, IStiCrossFiltering, IStiTitleElement, IStiElementInteraction, IStiGlobalizationProvider
{
	private byte[] image;

	private string imageHyperlink;

	private bool aspectRatio = true;

	private StiFontIcons? icon;

	private Color iconColor = Color.FromArgb(68, 114, 196);

	private StiHorAlignment horAlignment = StiHorAlignment.Center;

	private StiVertAlignment vertAlignment = StiVertAlignment.Center;

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiImageElement;

	[Browsable(false)]
	public override bool IsDefined
	{
		get
		{
			if (!IsImageDefined && !IsImageHyperlinkDefined)
			{
				return IsImageIconDefined;
			}
			return true;
		}
	}

	[Browsable(false)]
	public override bool IsQuerable => IsImageHyperlinkDataColumnDefined;

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiImageBytesConverter))]
	public byte[] Image
	{
		get
		{
			return image;
		}
		set
		{
			if (image != value)
			{
				image = value;
				InvokeRefreshControl("Image");
			}
		}
	}

	[DefaultValue(null)]
	[StiSerializable]
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
				InvokeRefreshControl("ImageHyperlink");
			}
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiShowInContextMenu]
	[StiCategory("Image", StiCategorySource.PropertyCategory)]
	[StiOrder(100)]
	[StiPropertyLevel(StiLevel.Basic)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool AspectRatio
	{
		get
		{
			return aspectRatio;
		}
		set
		{
			if (aspectRatio != value)
			{
				aspectRatio = value;
				InvokeRefreshControl("AspectRatio");
			}
		}
	}

	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	[StiCategory("Image", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiNullableEnumConverter))]
	public StiFontIcons? Icon
	{
		get
		{
			return icon;
		}
		set
		{
			if (icon != value)
			{
				icon = value;
				InvokeRefreshControl("Icon");
			}
		}
	}

	[StiSerializable]
	[StiCategory("Image", StiCategorySource.PropertyCategory)]
	[Browsable(false)]
	[TypeConverter(typeof(StiStyleColorConverter))]
	public Color IconColor
	{
		get
		{
			return iconColor;
		}
		set
		{
			if (iconColor != value)
			{
				iconColor = value;
				InvokeRefreshControl("IconColor");
			}
		}
	}

	[StiSerializable]
	[StiCategory("Image", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(102)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("Image", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(100)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool CrossFiltering { get; set; } = true;


	[StiSerializable]
	[DefaultValue(StiHorAlignment.Center)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory("Image", StiCategorySource.PropertyCategory)]
	[StiOrder(110)]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiHorAlignment HorAlignment
	{
		get
		{
			return horAlignment;
		}
		set
		{
			if (horAlignment != value)
			{
				horAlignment = value;
				InvokeRefreshControl("HorAlignment");
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiVertAlignment.Center)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory("Image", StiCategorySource.PropertyCategory)]
	[StiOrder(120)]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiVertAlignment VertAlignment
	{
		get
		{
			return vertAlignment;
		}
		set
		{
			if (vertAlignment != value)
			{
				vertAlignment = value;
				InvokeRefreshControl("VertAlignment");
			}
		}
	}

	[StiSerializable]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(300)]
	[Description("Specifies the interior spacing of the component.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public override StiPadding Padding
	{
		get
		{
			return base.Padding;
		}
		set
		{
			base.Padding = value;
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiImageDashboardInteractionConverter))]
	[Browsable(false)]
	public virtual IStiDashboardInteraction DashboardInteraction { get; set; }

	[Description("Shadow settings of the component.")]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(276)]
	public StiSimpleShadow Shadow
	{
		get
		{
			return shadow;
		}
		set
		{
			if (shadow != value)
			{
				shadow = value;
				InvokeRefreshControl("Shadow");
			}
		}
	}

	[Description("Represents the value to which the corners are rounded.")]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(150)]
	public StiCornerRadius CornerRadius
	{
		get
		{
			return cornerRadius;
		}
		set
		{
			if (cornerRadius != value)
			{
				cornerRadius = value;
				InvokeRefreshControl("CornerRadius");
			}
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiElementInteractionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(205)]
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiDashboardInteractionConverter))]
	[Browsable(true)]
	public override StiInteraction Interaction
	{
		get
		{
			return base.Interaction;
		}
		set
		{
			base.Interaction = value;
		}
	}

	public override int ToolboxPosition => 311;

	public override string LocalizedName => StiLocalization.Get("Components", "StiImage");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 120.0, 120.0);

	public override string HelpUrl => "user-manual/dashboards_image.htm";

	[StiEventHide]
	[StiNonSerialized]
	public override StiClickEvent ClickEvent
	{
		get
		{
			return base.ClickEvent;
		}
		set
		{
			base.ClickEvent = value;
		}
	}

	[Browsable(false)]
	public bool IsImageDefined => Image != null;

	[Browsable(false)]
	public bool IsImageIconDefined => Icon.HasValue;

	[Browsable(false)]
	public bool IsImageHyperlinkDefined => !string.IsNullOrWhiteSpace(ImageHyperlink);

	[Browsable(false)]
	public bool IsImageHyperlinkDataColumnDefined
	{
		get
		{
			if (!IsImageDefined && !IsImageIconDefined && IsImageHyperlinkDefined)
			{
				return !string.IsNullOrWhiteSpace(StiHyperlinkProcessor.GetRealDataColumnFromHyperlink(Report, ImageHyperlink));
			}
			return false;
		}
	}

	public override object Clone(bool cloneProperties)
	{
		StiImageElement stiImageElement = base.Clone(cloneProperties) as StiImageElement;
		((IStiTitleElement)stiImageElement).Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiImageElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiImageElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiImageElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyBool("AspectRatio", AspectRatio, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("ImageHyperlink", ImageHyperlink);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Center);
		jObject.AddPropertyEnum("VertAlignment", VertAlignment, StiVertAlignment.Center);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		if (Image != null)
		{
			jObject.Add("Image", System.Convert.ToBase64String(Image));
		}
		if (Icon.HasValue)
		{
			jObject.AddPropertyEnum("Icon", Icon);
		}
		jObject.AddPropertyColor("IconColor", IconColor, Color.FromArgb(68, 114, 196));
		if (DashboardInteraction is StiImageDashboardInteraction stiImageDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiImageDashboardInteraction.SaveToJsonObject(mode));
		}
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Group":
				Group = item.DeserializeString();
				break;
			case "CrossFiltering":
				CrossFiltering = item.DeserializeBool();
				break;
			case "AspectRatio":
				AspectRatio = item.DeserializeBool();
				break;
			case "ImageHyperlink":
				ImageHyperlink = item.DeserializeString();
				break;
			case "Image":
				Image = item.DeserializeByteArray();
				break;
			case "HorAlignment":
				HorAlignment = item.DeserializeEnum<StiHorAlignment>();
				break;
			case "VertAlignment":
				VertAlignment = item.DeserializeEnum<StiVertAlignment>();
				break;
			case "DashboardInteraction":
			{
				StiImageDashboardInteraction stiImageDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiImageDashboardInteraction;
				DashboardInteraction = stiImageDashboardInteraction ?? null;
				break;
			}
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "IconColor":
				IconColor = item.DeserializeColor();
				break;
			case "Shadow":
				Shadow = item.DeserializeSimpleShadow();
				break;
			case "CornerRadius":
				CornerRadius = item.DeserializeCornerRadius();
				break;
			}
		}
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		return GetMeters();
	}

	public override List<IStiMeter> GetMeters()
	{
		if (!IsImageHyperlinkDataColumnDefined)
		{
			return new List<IStiMeter>();
		}
		string realDataColumnFromHyperlink = StiHyperlinkProcessor.GetRealDataColumnFromHyperlink(Report, ImageHyperlink);
		return new List<IStiMeter>
		{
			new StiImageMeter(realDataColumnFromHyperlink, realDataColumnFromHyperlink, realDataColumnFromHyperlink)
		};
	}

	public override List<string> RetrieveUsedDataNames()
	{
		return StiUsedDataHelper.GetMany(GetMeters()).Distinct().ToList();
	}

	public void CopyAllImageProperties(IStiImageElement image)
	{
		if (image != null)
		{
			Image = image.Image;
			ImageHyperlink = image.ImageHyperlink;
			Icon = image.Icon;
			IconColor = image.IconColor;
		}
	}

	private bool ShouldSerializePadding()
	{
		return !Padding.IsEmpty;
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		if (propertyName == "Title.Text")
		{
			Title.Text = value;
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		if (propertyName == "Title.Text")
		{
			return Title.Text;
		}
		throw new ArgumentException("Property with name " + propertyName);
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		return new string[1] { "Title.Text" };
	}

	private bool ShouldSerializeDashboardInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Title.Font));
		if (Icon.HasValue)
		{
			fonts.Add(StiFontIconsHelper.GetFont());
		}
		return fonts.Distinct().ToList();
	}

	private bool ShouldSerializeShadow()
	{
		if (Shadow != null)
		{
			return !Shadow.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeCornerRadius()
	{
		if (CornerRadius != null)
		{
			return !CornerRadius.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	public override List<StiPage> GetNestedPages()
	{
		if (DashboardInteraction == null || DashboardInteraction.OnClick == StiInteractionOnClick.None)
		{
			return null;
		}
		string pageKey = DashboardInteraction?.DrillDownPageKey;
		if (StiKeyHelper.IsEmptyKey(pageKey))
		{
			return null;
		}
		StiPage stiPage = Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == pageKey);
		if (stiPage == null)
		{
			return null;
		}
		return new List<StiPage> { stiPage };
	}

	public override StiComponent CreateNew()
	{
		return new StiImageElement();
	}

	internal void ResetAllImageProperties()
	{
		Image = null;
		ImageHyperlink = null;
		Icon = null;
	}

	internal byte[] GetImageBytes()
	{
		if (IsImageDefined)
		{
			return Image;
		}
		if (IsImageIconDefined)
		{
			RectangleD paintRectangle = GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			Image imageObject = StiFontIconsHelper.ConvertFontIconToImage(Icon.GetValueOrDefault(), IconColor, (int)(paintRectangle.Width * StiScale.Factor), (int)(paintRectangle.Height * StiScale.Factor));
			return StiImageHelper.GetImageBytesFromObject(imageObject);
		}
		if (!IsImageHyperlinkDefined)
		{
			return null;
		}
		byte[] array = StiDashboardImageHyperlinkCache.Get(ImageHyperlink, Report);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		return array;
	}

	public StiImageElement()
		: this(RectangleD.Empty)
	{
	}

	public StiImageElement(RectangleD rect)
		: base(rect)
	{
		Title.Text = LocalizedName;
		Title.Visible = false;
		Padding = new StiPadding(0.0);
		DashboardInteraction = new StiImageDashboardInteraction();
	}
}
