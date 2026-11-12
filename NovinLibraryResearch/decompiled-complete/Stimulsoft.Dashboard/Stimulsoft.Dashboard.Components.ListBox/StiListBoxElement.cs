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
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.ListBox;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiListBox.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiListBoxElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiListBoxElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiListBoxElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiListBoxElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiListBoxElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiListBoxElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiListBoxElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiListBoxElement : StiElement, IStiListBoxElement, IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiItemElement, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiDataFilters, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiGlobalizationProvider
{
	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private List<StiDataFilterRule> userFilters = new List<StiDataFilterRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private Font font = new Font("Arial", 8f);

	private Color foreColor = Color.Transparent;

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiListBoxElement;

	[StiSerializable]
	[StiCategory("ListBox", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(103)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[DefaultValue(StiElementStyleIdent.Auto)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(280)]
	[StiPropertyLevel(StiLevel.Basic)]
	[TypeConverter(typeof(StiElementStyleConverter))]
	[Description("Specifies the visual style used to draw this component.")]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementStyleEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiElementStyleIdent Style
	{
		get
		{
			return style;
		}
		set
		{
			if (style != value)
			{
				style = value;
				if (value != StiElementStyleIdent.Custom)
				{
					CustomStyleName = string.Empty;
				}
				InvokeRefreshControl("Style");
			}
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[Browsable(false)]
	public string CustomStyleName
	{
		get
		{
			return customStyleName;
		}
		set
		{
			if (customStyleName != value)
			{
				customStyleName = value;
				if (!string.IsNullOrWhiteSpace(value))
				{
					style = StiElementStyleIdent.Custom;
				}
				InvokeRefreshControl("CustomStyleName");
			}
		}
	}

	[Browsable(false)]
	public override bool IsDefined
	{
		get
		{
			if (NameMeter == null)
			{
				return KeyMeter != null;
			}
			return true;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ShowAllValue { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[Browsable(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ShowBlanks { get; set; }

	[StiSerializable]
	[DefaultValue(StiItemSelectionMode.One)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public StiItemSelectionMode SelectionMode { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[Browsable(false)]
	public virtual List<StiDataFilterRule> UserFilters
	{
		get
		{
			return userFilters;
		}
		set
		{
			if (userFilters != value)
			{
				userFilters = value;
				InvokeRefreshControl("UserFilters");
			}
		}
	}

	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public List<StiDataActionRule> TransformActions { get; set; } = new List<StiDataActionRule>();


	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public virtual List<StiDataFilterRule> TransformFilters { get; set; } = new List<StiDataFilterRule>();


	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public List<StiDataSortRule> TransformSorts { get; set; } = new List<StiDataSortRule>();


	[Editor("Stimulsoft.Report.Components.Design.StiDataTransformationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiDataTransformationConverter))]
	[StiCategory("ListBox", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(102)]
	public object DataTransformation { get; set; } = new object();


	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public virtual List<StiDataFilterRule> DataFilters
	{
		get
		{
			return dataFilters;
		}
		set
		{
			if (dataFilters != value)
			{
				dataFilters = value;
				InvokeRefreshControl("DataFilters");
			}
		}
	}

	[StiSerializable]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The font used to display text in the component.")]
	[StiOrder(180)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (font != value)
			{
				font = value;
				InvokeRefreshControl("Font");
			}
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The foreground color of this component, which is used to display text.")]
	[StiOrder(210)]
	[StiExpressionAllowed]
	public Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			if (foreColor != value)
			{
				foreColor = value;
				InvokeRefreshControl("ForeColor");
			}
		}
	}

	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(480)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiFormatService TextFormat { get; set; } = new StiGeneralFormatService();


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

	public override int ToolboxPosition => 318;

	public override string LocalizedName => StiLocalization.Get("Components", "StiListBox");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 200.0, 300.0);

	public override string HelpUrl => "user-manual/dashboards_data_filtering_list_box.htm";

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

	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	public string ParentKey { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiNameListBoxMeter NameMeter { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiKeyListBoxMeter KeyMeter { get; set; }

	[StiSerializable]
	[DefaultValue(StiItemOrientation.Vertical)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public StiItemOrientation Orientation { get; set; }

	public override object Clone(bool cloneProperties)
	{
		StiListBoxElement stiListBoxElement = ((StiElement)base.Clone(cloneProperties)) as StiListBoxElement;
		stiListBoxElement.KeyMeter = KeyMeter?.Clone() as StiKeyListBoxMeter;
		stiListBoxElement.NameMeter = NameMeter?.Clone() as StiNameListBoxMeter;
		stiListBoxElement.UserFilters = UserFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiListBoxElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiListBoxElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiListBoxElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiListBoxElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiListBoxElement.TextFormat = TextFormat.Clone() as StiFormatService;
		stiListBoxElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiListBoxElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiListBoxElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiListBoxElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyStringNullOrEmpty("ParentKey", ParentKey);
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyBool("ShowAllValue", ShowAllValue);
		jObject.AddPropertyBool("ShowBlanks", ShowBlanks);
		jObject.AddPropertyEnum("SelectionMode", SelectionMode, StiItemSelectionMode.One);
		jObject.AddPropertyEnum("Orientation", Orientation, StiItemOrientation.Vertical);
		if (NameMeter != null)
		{
			jObject.AddPropertyJObject("NameMeter", NameMeter.SaveToJsonObject(mode));
		}
		if (KeyMeter != null)
		{
			jObject.AddPropertyJObject("KeyMeter", KeyMeter.SaveToJsonObject(mode));
		}
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		if (!(TextFormat is StiGeneralFormatService) && TextFormat != null)
		{
			jObject.AddPropertyJObject("TextFormat", TextFormat.SaveToJsonObject(mode));
		}
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ParentKey":
				ParentKey = item.DeserializeString();
				break;
			case "TransformActions":
				TransformActions.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataActionRule.LoadFromJson((JObject)p.Value));
				break;
			case "TransformFilters":
				TransformFilters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
				break;
			case "TransformSorts":
				TransformSorts.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataSortRule.LoadFromJson((JObject)p.Value));
				break;
			case "DataFilters":
				DataFilters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "ShowAllValue":
				ShowAllValue = item.DeserializeBool();
				break;
			case "ShowBlanks":
				ShowBlanks = item.DeserializeBool();
				break;
			case "SelectionMode":
				SelectionMode = item.DeserializeEnum<StiItemSelectionMode>();
				break;
			case "Orientation":
				Orientation = item.DeserializeEnum<StiItemOrientation>();
				break;
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "NameMeter":
			{
				StiNameListBoxMeter stiNameListBoxMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiNameListBoxMeter;
				NameMeter = stiNameListBoxMeter ?? null;
				break;
			}
			case "KeyMeter":
			{
				StiKeyListBoxMeter stiKeyListBoxMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiKeyListBoxMeter;
				KeyMeter = stiKeyListBoxMeter ?? null;
				break;
			}
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Style":
				Style = item.DeserializeEnum<StiElementStyleIdent>();
				break;
			case "Group":
				Group = item.DeserializeString();
				break;
			case "CustomStyleName":
				CustomStyleName = item.DeserializeString();
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
		List<IStiMeter> list = new List<IStiMeter>();
		if (NameMeter != null)
		{
			list.Add(NameMeter);
		}
		if (KeyMeter != null)
		{
			list.Add(KeyMeter);
		}
		return list;
	}

	public override List<string> RetrieveUsedDataNames()
	{
		return StiUsedDataHelper.GetMany(NameMeter, KeyMeter);
	}

	string IStiFilterElement.GetParentKey()
	{
		return ParentKey;
	}

	void IStiFilterElement.SetParentKey(string key)
	{
		ParentKey = key;
	}

	void IStiFilterElement.ApplyDefaultFilters()
	{
		List<StiDataFilterRule> list = StiListBoxHelper.FetchDefaultUserFilters(this);
		if (list != null)
		{
			UserFilters = list;
		}
	}

	public void AddKeyMeter(IStiAppDataCell cell)
	{
		KeyMeter = ((cell != null) ? StiMeterHelper.ListBox.GetKey(cell) : null);
	}

	public void AddKeyMeter(IStiMeter meter)
	{
		KeyMeter = ((meter != null) ? StiMeterHelper.ListBox.GetKey(meter as StiMeter) : null);
	}

	public IStiMeter GetKeyMeter()
	{
		return KeyMeter;
	}

	public void RemoveKeyMeter()
	{
		KeyMeter = null;
	}

	public void CreateNewKeyMeter()
	{
		KeyMeter = new StiKeyListBoxMeter();
	}

	public void AddNameMeter(IStiAppDataCell cell)
	{
		NameMeter = ((cell != null) ? StiMeterHelper.ListBox.GetName(cell) : null);
	}

	public void AddNameMeter(IStiMeter meter)
	{
		NameMeter = ((meter != null) ? StiMeterHelper.ListBox.GetName(meter as StiMeter) : null);
	}

	public IStiMeter GetNameMeter()
	{
		return NameMeter;
	}

	public void RemoveNameMeter()
	{
		NameMeter = null;
	}

	public void CreateNewNameMeter()
	{
		NameMeter = new StiNameListBoxMeter();
	}

	public void CreateNextMeter(IStiAppDataCell cell)
	{
		if (KeyMeter == null)
		{
			KeyMeter = StiMeterHelper.ListBox.GetKey(cell);
		}
		else if (NameMeter == null)
		{
			NameMeter = StiMeterHelper.ListBox.GetName(cell);
		}
		else
		{
			KeyMeter = StiMeterHelper.ListBox.GetKey(cell);
		}
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		StiMeter stiMeter = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter = stiMeter ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiMeasureMeter) as StiMeter);
		if (stiMeter != null)
		{
			KeyMeter = KeyMeter ?? new StiKeyListBoxMeter();
			KeyMeter.LoadFromJsonObject(stiMeter.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		StiMeter stiMeter2 = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter2 = stiMeter2 ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiDimensionMeter) as StiMeter);
		if (stiMeter2 != null)
		{
			NameMeter = NameMeter ?? new StiNameListBoxMeter();
			NameMeter.LoadFromJsonObject(stiMeter2.SaveToJsonObject(StiJsonSaveMode.Report));
		}
	}

	private bool ShouldSerializeDataTransformation()
	{
		if (TransformActions != null && !TransformActions.Any() && TransformFilters != null && !TransformFilters.Any() && TransformSorts != null)
		{
			return TransformSorts.Any();
		}
		return true;
	}

	public bool IsDefaultDataTransformation()
	{
		return !ShouldSerializeDataTransformation();
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 8f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	private bool ShouldSerializeTextFormat()
	{
		return !(TextFormat is StiGeneralFormatService);
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

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		fonts.Add(new StiFont(Title.Font));
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

	public override StiComponent CreateNew()
	{
		return new StiListBoxElement();
	}

	public StiListBoxElement()
		: this(RectangleD.Empty)
	{
	}

	public StiListBoxElement(RectangleD rect)
		: base(rect)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		Title.Text = LocalizedName;
		Title.Visible = false;
	}
}
