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

namespace Stimulsoft.Dashboard.Components.ComboBox;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiComboBox.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiComboBoxElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiComboBoxElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiComboBoxElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiComboBoxElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiComboBoxElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiComboBoxElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiComboBoxElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiComboBoxElement : StiElement, IStiComboBoxElement, IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiItemElement, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiDataFilters, IStiSimpleShadow, IStiCornerRadius, IStiFixedHeightElement
{
	private List<StiDataFilterRule> userFilters = new List<StiDataFilterRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private Font font = new Font("Arial", 8f);

	private Color foreColor = Color.Transparent;

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiComboBoxElement;

	[StiSerializable]
	[StiCategory("ComboBox", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(103)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[Browsable(false)]
	public bool IsFixedHeight => true;

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
	[StiCategory("ComboBox", StiCategorySource.PropertyCategory)]
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

	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class, StiSerializeTypes.SerializeToAll)]
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

	public override int ToolboxPosition => 316;

	public override string LocalizedName => StiLocalization.Get("Components", "StiComboBox");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 200.0, 40.0);

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MinSize
	{
		get
		{
			StiPage stiPage = Report?.GetCurrentPage();
			if (stiPage == null)
			{
				return SizeD.Empty;
			}
			double num = StiAlignValue.AlignToMaxGrid(DefaultClientRectangle.Height, stiPage.GridSize, aligningToGrid: true);
			return new SizeD(0.0, num);
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MaxSize
	{
		get
		{
			StiPage stiPage = Report?.GetCurrentPage();
			if (stiPage == null)
			{
				return SizeD.Empty;
			}
			double num = StiAlignValue.AlignToMaxGrid(DefaultClientRectangle.Height, stiPage.GridSize, aligningToGrid: true);
			return new SizeD(0.0, num);
		}
		set
		{
		}
	}

	public override string HelpUrl => "user-manual/dashboards_data_filtering_combo_box.htm";

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
	[StiPropertyLevel(StiLevel.Basic)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string ParentKey { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiNameComboBoxMeter NameMeter { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiKeyComboBoxMeter KeyMeter { get; set; }

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
		if (NameMeter != null)
		{
			jObject.AddPropertyJObject("NameMeter", NameMeter.SaveToJsonObject(mode));
		}
		if (KeyMeter != null)
		{
			jObject.AddPropertyJObject("KeyMeter", KeyMeter.SaveToJsonObject(mode));
		}
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
			case "SelectionMode":
				SelectionMode = item.DeserializeEnum<StiItemSelectionMode>();
				break;
			case "NameMeter":
			{
				StiNameComboBoxMeter stiNameComboBoxMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiNameComboBoxMeter;
				NameMeter = stiNameComboBoxMeter ?? null;
				break;
			}
			case "KeyMeter":
			{
				StiKeyComboBoxMeter stiKeyComboBoxMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiKeyComboBoxMeter;
				KeyMeter = stiKeyComboBoxMeter ?? null;
				break;
			}
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

	public override object Clone(bool cloneProperties)
	{
		StiComboBoxElement stiComboBoxElement = ((StiElement)base.Clone(cloneProperties)) as StiComboBoxElement;
		stiComboBoxElement.NameMeter = NameMeter?.Clone() as StiNameComboBoxMeter;
		stiComboBoxElement.KeyMeter = KeyMeter?.Clone() as StiKeyComboBoxMeter;
		stiComboBoxElement.UserFilters = UserFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiComboBoxElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiComboBoxElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiComboBoxElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiComboBoxElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiComboBoxElement.TextFormat = TextFormat.Clone() as StiFormatService;
		stiComboBoxElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiComboBoxElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiComboBoxElement;
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
		List<StiDataFilterRule> list = StiComboBoxHelper.FetchDefaultUserFilters(this);
		if (list != null)
		{
			UserFilters = list;
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

	public void AddKeyMeter(IStiAppVariable variable)
	{
		KeyMeter = ((variable != null) ? StiMeterHelper.ComboBox.GetKey(variable) : null);
	}

	public void AddKeyMeter(IStiAppDataCell cell)
	{
		KeyMeter = ((cell != null) ? StiMeterHelper.ComboBox.GetKey(cell) : null);
	}

	public void AddKeyMeter(IStiMeter meter)
	{
		KeyMeter = ((meter != null) ? StiMeterHelper.ComboBox.GetKey(meter as StiMeter) : null);
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
		KeyMeter = new StiKeyComboBoxMeter();
	}

	public void AddNameMeter(IStiAppDataCell cell)
	{
		NameMeter = ((cell != null) ? StiMeterHelper.ComboBox.GetName(cell) : null);
	}

	public void AddNameMeter(IStiMeter meter)
	{
		NameMeter = ((meter != null) ? StiMeterHelper.ComboBox.GetName(meter as StiMeter) : null);
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
		NameMeter = new StiNameComboBoxMeter();
	}

	public void CreateNextMeter(IStiAppDataCell cell)
	{
		if (KeyMeter == null)
		{
			KeyMeter = StiMeterHelper.ComboBox.GetKey(cell);
		}
		else if (NameMeter == null)
		{
			NameMeter = StiMeterHelper.ComboBox.GetName(cell);
		}
		else
		{
			KeyMeter = StiMeterHelper.ComboBox.GetKey(cell);
		}
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		StiMeter stiMeter = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter = stiMeter ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiMeasureMeter) as StiMeter);
		if (stiMeter != null)
		{
			KeyMeter = KeyMeter ?? new StiKeyComboBoxMeter();
			KeyMeter.LoadFromJsonObject(stiMeter.SaveToJsonObject(StiJsonSaveMode.Report));
		}
		StiMeter stiMeter2 = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter2 = stiMeter2 ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiDimensionMeter) as StiMeter);
		if (stiMeter2 != null)
		{
			NameMeter = NameMeter ?? new StiNameComboBoxMeter();
			NameMeter.LoadFromJsonObject(stiMeter2.SaveToJsonObject(StiJsonSaveMode.Report));
		}
	}

	private bool ShouldSerializeTextFormat()
	{
		return !(TextFormat is StiGeneralFormatService);
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
		return new StiComboBoxElement();
	}

	public StiComboBoxElement()
		: this(RectangleD.Empty)
	{
	}

	public StiComboBoxElement(RectangleD rect)
		: base(rect)
	{
	}//IL_006a: Unknown result type (might be due to invalid IL or missing references)
	//IL_0074: Expected O, but got Unknown

}
