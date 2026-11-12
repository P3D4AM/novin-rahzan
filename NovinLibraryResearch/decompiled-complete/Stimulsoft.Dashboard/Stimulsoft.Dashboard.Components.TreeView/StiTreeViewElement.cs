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

namespace Stimulsoft.Dashboard.Components.TreeView;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiTreeView.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiTreeViewElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiTreeViewElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiTreeViewElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiTreeViewElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiTreeViewElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiTreeViewElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiTreeViewElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiTreeViewElement : StiElement, IStiTreeViewElement, IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiItemElement, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiDataFilters, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiGlobalizationProvider
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
	public override StiComponentId ComponentId => StiComponentId.StiTreeViewElement;

	[StiSerializable]
	[StiCategory("TreeView", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(102)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


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
			if (KeyMeters != null)
			{
				return KeyMeters.Count > 0;
			}
			return false;
		}
	}

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
	[StiCategory("TreeView", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(101)]
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
	[DefaultValue(StiItemSelectionMode.Multi)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public StiItemSelectionMode SelectionMode { get; set; } = StiItemSelectionMode.Multi;


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

	public override int ToolboxPosition => 320;

	public override string LocalizedName => StiLocalization.Get("Components", "StiTreeView");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 200.0, 300.0);

	public override string HelpUrl => "user-manual/dashboards_data_filtering_tree_view.htm";

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

	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiKeyTreeViewMeter> KeyMeters { get; set; } = new List<StiKeyTreeViewMeter>();


	public override object Clone(bool cloneProperties)
	{
		StiTreeViewElement stiTreeViewElement = ((StiElement)base.Clone(cloneProperties)) as StiTreeViewElement;
		stiTreeViewElement.KeyMeters = KeyMeters.Select((StiKeyTreeViewMeter c) => c.Clone() as StiKeyTreeViewMeter).ToList();
		stiTreeViewElement.UserFilters = UserFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiTreeViewElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiTreeViewElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiTreeViewElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiTreeViewElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiTreeViewElement.TextFormat = TextFormat.Clone() as StiFormatService;
		stiTreeViewElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiTreeViewElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiTreeViewElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiTreeViewElement;
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
		jObject.AddPropertyJObject("KeyMeters", StiJsonReportObjectHelper.Serialize.JObjectCollection(KeyMeters, mode));
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		jObject.AddPropertyEnum("SelectionMode", SelectionMode, StiItemSelectionMode.Multi);
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		if (!(TextFormat is StiGeneralFormatService) && TextFormat != null)
		{
			jObject.AddPropertyJObject("TextFormat", TextFormat.SaveToJsonObject(mode));
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
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "KeyMeters":
				KeyMeters.AddRange(from p in ((JObject)item.Value).Properties()
					select (StiKeyTreeViewMeter)StiMeterLoader.LoadFromJson((JObject)p.Value));
				break;
			case "SelectionMode":
				SelectionMode = item.DeserializeEnum<StiItemSelectionMode>();
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
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
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
		List<StiDataFilterRule> list = StiTreeViewHelper.FetchDefaultUserFilters(this);
		if (list != null)
		{
			UserFilters = list;
		}
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		return GetMeters();
	}

	public override List<IStiMeter> GetMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		list.AddRange(KeyMeters);
		return list;
	}

	public override List<string> RetrieveUsedDataNames()
	{
		return StiUsedDataHelper.GetMany(KeyMeters);
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

	public IStiMeter GetKeyMeter(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.TreeView.GetKey(cell);
	}

	public IStiMeter GetKeyMeterByIndex(int index)
	{
		if (index >= KeyMeters.Count)
		{
			return null;
		}
		return KeyMeters[index];
	}

	public void InsertKeyMeter(int index, IStiMeter meter)
	{
		if (meter is StiKeyTreeViewMeter item)
		{
			if (index >= 0 && index < KeyMeters.Count)
			{
				KeyMeters.Insert(index, item);
			}
			else
			{
				KeyMeters.Add(item);
			}
		}
	}

	public void RemoveKeyMeter(int index)
	{
		if (index < KeyMeters.Count)
		{
			KeyMeters.RemoveAt(index);
		}
	}

	public void RemoveAllKeyMeters()
	{
		KeyMeters.Clear();
	}

	public IStiMeter AddNewKeyMeter()
	{
		StiKeyTreeViewMeter stiKeyTreeViewMeter = new StiKeyTreeViewMeter();
		KeyMeters.Add(stiKeyTreeViewMeter);
		return stiKeyTreeViewMeter;
	}

	public void AddKey(IStiAppDataCell cell)
	{
		if (KeyMeters == null)
		{
			KeyMeters = new List<StiKeyTreeViewMeter>();
		}
		KeyMeters.Add(StiMeterHelper.TreeView.GetKey(cell));
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		StiMeter stiMeter = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter = stiMeter ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiMeasureMeter) as StiMeter);
		if (stiMeter != null)
		{
			StiKeyTreeViewMeter stiKeyTreeViewMeter = new StiKeyTreeViewMeter();
			stiKeyTreeViewMeter.LoadFromJsonObject(stiMeter.SaveToJsonObject(StiJsonSaveMode.Report));
			KeyMeters.Clear();
			KeyMeters.Add(stiKeyTreeViewMeter);
		}
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
		return new StiTreeViewElement();
	}

	public StiTreeViewElement()
		: this(RectangleD.Empty)
	{
	}

	public StiTreeViewElement(RectangleD rect)
		: base(rect)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		Title.Text = LocalizedName;
		Title.Visible = false;
	}
}
