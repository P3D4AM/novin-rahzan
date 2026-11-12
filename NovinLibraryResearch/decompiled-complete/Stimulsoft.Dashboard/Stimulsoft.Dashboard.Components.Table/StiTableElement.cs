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
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Components.Table.Design;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Table;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiTable.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiTableElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiTableElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiTableElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiTableElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiTableElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiTableElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiTableElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiTableElement : StiElement, IStiTableElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiUserSorts, IStiUserFilters, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiFont, IStiConvertibleElement, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiElementLayout, IStiGlobalizationProvider, IStiElementInteraction, IStiSkipOwnFilter
{
	private Color foreColor = Color.Transparent;

	private Font font = new Font("Arial", 10f);

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private List<StiDataFilterRule> userFilters = new List<StiDataFilterRule>();

	private List<StiDataSortRule> userSorts = new List<StiDataSortRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	private int frozenColumns;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiTableElement;

	[Browsable(false)]
	public override bool IsDefined
	{
		get
		{
			if (Columns != null)
			{
				return Columns.Count > 0;
			}
			return false;
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[Browsable(false)]
	public StiElementLayout Layout { get; set; }

	[StiSerializable]
	[StiCategory("Table", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(104)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("Table", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(101)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool CrossFiltering { get; set; } = true;


	[Browsable(false)]
	bool IStiSkipOwnFilter.AllowSkipOwnFilter => !((StiTableDashboardInteraction)DashboardInteraction).DrillDownFiltered;

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
	[StiSerializable(StiSerializationVisibility.List)]
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
	public List<StiDataSortRule> UserSorts
	{
		get
		{
			return userSorts;
		}
		set
		{
			if (userSorts != value)
			{
				userSorts = value;
				InvokeRefreshControl("UserSorts");
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
	[StiCategory("Table", StiCategorySource.PropertyCategory)]
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

	[Browsable(false)]
	[StiSerializable(StiSerializeTypes.SerializeToDesigner)]
	[DefaultValue(false)]
	public override bool IsSelected
	{
		get
		{
			return base.IsSelected;
		}
		set
		{
			base.IsSelected = value;
			if (!value)
			{
				StiTableElementSelection.ResetSelection(this);
			}
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiDashboardInteractionConverter))]
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

	public override int ToolboxPosition => 301;

	public override string LocalizedName => StiLocalization.Get("Components", "StiTable");

	public override string HelpUrl => "user-manual/dashboards_table.htm";

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

	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiTableConditionConverter))]
	[Browsable(false)]
	public List<IStiTableElementCondition> TableConditions { get; set; } = new List<IStiTableElementCondition>();


	[RefreshProperties(RefreshProperties.All)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiTableConditionsEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(140)]
	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiTableConditionConverter))]
	[Browsable(false)]
	public override StiConditionsCollection Conditions
	{
		get
		{
			return base.Conditions;
		}
		set
		{
			base.Conditions = value;
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

	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiTableColumn> Columns { get; set; } = new List<StiTableColumn>();


	[DefaultValue(0)]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Table", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets the specified amount of columns which are visible while scrolling through the rest of the table.")]
	[StiOrder(103)]
	public int FrozenColumns
	{
		get
		{
			return frozenColumns;
		}
		set
		{
			if (value != frozenColumns)
			{
				if (value < 0)
				{
					value = 0;
				}
				frozenColumns = value;
			}
		}
	}

	[DefaultValue(StiTableSizeMode.AutoSize)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Table", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets the column size mode.")]
	[StiOrder(106)]
	public StiTableSizeMode SizeMode { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets fore color for the table header.")]
	[StiOrder(240)]
	public Color HeaderForeColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets the font of the table header.")]
	[StiOrder(230)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font HeaderFont { get; set; } = new Font("Arial", 10f);


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets fore color for the table footer.")]
	[StiOrder(200)]
	public Color FooterForeColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets the font of the table footer.")]
	[StiOrder(190)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Font FooterFont { get; set; } = new Font("Arial", 10f);


	public override object Clone(bool cloneProperties)
	{
		StiTableElement stiTableElement = ((StiElement)base.Clone(cloneProperties)) as StiTableElement;
		stiTableElement.Columns = Columns.Select((StiTableColumn c) => c.Clone() as StiTableColumn).ToList();
		stiTableElement.UserFilters = UserFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiTableElement.UserSorts = UserSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiTableElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiTableElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiTableElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiTableElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiTableElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiTableElement.TableConditions = TableConditions.Select((IStiTableElementCondition c) => ((StiTableElementCondition)c).Clone() as IStiTableElementCondition).ToList();
		stiTableElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiTableElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiTableElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiTableElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("Columns", StiJsonReportObjectHelper.Serialize.JObjectCollection(Columns, mode));
		jObject.AddPropertyJObject("UserFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(UserFilters, mode));
		jObject.AddPropertyJObject("UserSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(UserSorts, mode));
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		jObject.AddPropertyColor("HeaderForeColor", HeaderForeColor, Color.Transparent);
		jObject.AddPropertyColor("FooterForeColor", FooterForeColor, Color.Transparent);
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyFontArial8("HeaderFont", HeaderFont);
		jObject.AddPropertyFontArial8("FooterFont", FooterFont);
		jObject.AddPropertyFontArial10("Font", Font);
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		if (Layout != null)
		{
			jObject.AddPropertyJObject("Layout", Layout.SaveToJsonObject(mode));
		}
		if (DashboardInteraction is StiTableDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", (DashboardInteraction as StiTableDashboardInteraction).SaveToJsonObject(mode));
		}
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyInt("FrozenColumns", FrozenColumns);
		jObject.AddPropertyEnum("SizeMode", SizeMode, StiTableSizeMode.AutoSize);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyJObject("TableConditions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TableConditions.Select((IStiTableElementCondition c) => c as StiTableElementCondition), mode));
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
			case "DashboardInteraction":
			{
				StiTableDashboardInteraction stiTableDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiTableDashboardInteraction;
				DashboardInteraction = stiTableDashboardInteraction ?? null;
				break;
			}
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Layout":
				Layout = StiElementLayout.CreateFromJsonObject((JObject)item.Value, this);
				break;
			case "HeaderForeColor":
				HeaderForeColor = item.DeserializeColor();
				break;
			case "FooterForeColor":
				FooterForeColor = item.DeserializeColor();
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "HeaderFont":
				HeaderFont = item.DeserializeFont(Font);
				break;
			case "FooterFont":
				FooterFont = item.DeserializeFont(FooterFont);
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "Columns":
				Columns.AddRange(from p in ((JObject)item.Value).Properties()
					select StiMeterLoader.LoadFromJson((JObject)p.Value) as StiTableColumn into c
					where c != null
					select c);
				break;
			case "UserSorts":
				UserSorts.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataSortRule.LoadFromJson((JObject)p.Value));
				break;
			case "UserFilters":
				UserFilters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
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
			case "Style":
				Style = item.DeserializeEnum<StiElementStyleIdent>();
				break;
			case "FrozenColumns":
				FrozenColumns = item.DeserializeInt();
				break;
			case "SizeMode":
				SizeMode = item.DeserializeEnum<StiTableSizeMode>();
				break;
			case "Group":
				Group = item.DeserializeString();
				break;
			case "CrossFiltering":
				CrossFiltering = item.DeserializeBool();
				break;
			case "CustomStyleName":
				CustomStyleName = item.DeserializeString();
				break;
			case "TableConditions":
				TableConditions.AddRange(from p in ((JObject)item.Value).Properties()
					select StiTableElementCondition.LoadFromJson((JObject)p.Value));
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

	public void CreateMeters(IStiTableElement source)
	{
		StiTableElement stiTableElement = source as StiTableElement;
		Columns.Clear();
		foreach (StiTableColumn column in stiTableElement.Columns)
		{
			Columns.Add(column);
		}
	}

	public void CreateMeters(StiDataSource dataSource)
	{
		Columns.Clear();
		foreach (StiDataColumn column in dataSource.Columns)
		{
			if (column?.Type != null && column.Type.IsNumericType())
			{
				Columns.Add(StiMeterHelper.Table.GetMeasure(column));
			}
			else
			{
				Columns.Add(StiMeterHelper.Table.GetDimension(column));
			}
		}
	}

	public void CreateMeter(IStiAppDataCell cell)
	{
		StiDataColumn stiDataColumn = cell as StiDataColumn;
		StiVariable stiVariable = cell as StiVariable;
		Type type = ((stiDataColumn != null) ? stiDataColumn.Type : stiVariable?.Type);
		if (type != null && type.IsNumericType())
		{
			StiMeasureColumn measure = StiMeterHelper.Table.GetMeasure(stiDataColumn);
			measure.ShowTotalSummary = true;
			measure.SummaryType = StiSummaryColumnType.Sum;
			Columns.Add(measure);
		}
		else
		{
			Columns.Add(StiMeterHelper.Table.GetDimension(cell));
		}
	}

	public void RemoveMeter(int index)
	{
		if (index < Columns.Count)
		{
			Columns.RemoveAt(index);
		}
	}

	public void RemoveAllMeters()
	{
		Columns.Clear();
	}

	public void InsertMeter(int index, IStiMeter meter)
	{
		if (meter is StiTableColumn item)
		{
			if (index >= 0 && index < Columns.Count)
			{
				Columns.Insert(index, item);
			}
			else
			{
				Columns.Add(item);
			}
		}
	}

	public void InsertNewDimension(int index)
	{
		InsertMeter(index, new StiDimensionColumn
		{
			HorAlignment = StiHorAlignment.Left
		});
	}

	public void InsertNewMeasure(int index)
	{
		InsertMeter(index, new StiMeasureColumn
		{
			Expression = "Sum()",
			HorAlignment = StiHorAlignment.Right
		});
	}

	public IStiMeter GetMeasure(IStiAppDataCell cell)
	{
		return StiMeterHelper.Table.GetMeasure(cell);
	}

	public IStiMeter GetDimension(IStiAppDataCell cell)
	{
		return StiMeterHelper.Table.GetDimension(cell);
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		Columns.Clear();
		list?.Cast<StiMeter>().ToList().ForEach(delegate(StiMeter m)
		{
			StiTableColumn stiTableColumn = null;
			if (m is IStiDimensionMeter)
			{
				stiTableColumn = new StiDimensionColumn();
			}
			else if (m is IStiMeasureMeter)
			{
				stiTableColumn = new StiMeasureColumn
				{
					HorAlignment = StiHorAlignment.Right
				};
			}
			if (stiTableColumn != null)
			{
				stiTableColumn.LoadFromJsonObject(m.SaveToJsonObject(StiJsonSaveMode.Report));
				Columns.Add(stiTableColumn);
			}
		});
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiTableDashboardInteraction();
		}
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		return GetMeters();
	}

	public override List<IStiMeter> GetMeters()
	{
		return Columns.Cast<IStiMeter>().ToList();
	}

	public override List<string> RetrieveUsedDataNames()
	{
		return StiUsedDataHelper.GetMany(Columns);
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 10f)
		{
			return (int)Font.Style > 0;
		}
		return true;
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

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		if (propertyName == "Title.Text")
		{
			Title.Text = value;
		}
		int num = 0;
		foreach (StiTableColumn column in Columns)
		{
			string text = $"Column{num++}.Label";
			if (propertyName == text)
			{
				column.Label = value;
			}
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		if (propertyName == "Title.Text")
		{
			return Title.Text;
		}
		int num = 0;
		foreach (StiTableColumn column in Columns)
		{
			string text = $"Column{num++}.Label";
			if (propertyName == text)
			{
				return column.Label;
			}
		}
		throw new ArgumentException("Property with name " + propertyName);
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		List<string> list = new List<string> { "Title.Text" };
		int num = 0;
		foreach (StiTableColumn column in Columns)
		{
			list.Add($"Column{num++}.Label");
		}
		return list.ToArray();
	}

	private bool ShouldSerializeDashboardInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	public void AddTableCondition(string[] keyDataFiledMeters, string[] keyDestinationMeters, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, StiTableConditionPermissions permissions, Font font, Color foreColor, Color backColor, bool isExpression)
	{
		if (TableConditions == null)
		{
			TableConditions = new List<IStiTableElementCondition>();
		}
		TableConditions.Add(new StiTableElementCondition(keyDataFiledMeters, keyDestinationMeters, dataType, condition, value, permissions, font, foreColor, backColor, isExpression));
	}

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		fonts.Add(new StiFont(HeaderFont));
		fonts.Add(new StiFont(FooterFont));
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

	private bool ShouldSerializeConditions()
	{
		if (TableConditions != null)
		{
			return TableConditions.Count > 0;
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

	private bool ShouldSerializeColumns()
	{
		if (Columns != null)
		{
			return Columns.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeHeaderForeColor()
	{
		return HeaderForeColor != Color.Transparent;
	}

	private bool ShouldSerializeHeaderFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (HeaderFont != null && HeaderFont.Name == "Arial" && HeaderFont.SizeInPoints == 10f)
		{
			return (int)HeaderFont.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeFooterForeColor()
	{
		return FooterForeColor != Color.Transparent;
	}

	private bool ShouldSerializeFooterFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (FooterFont != null && FooterFont.Name == "Arial" && FooterFont.SizeInPoints == 10f)
		{
			return (int)FooterFont.Style > 0;
		}
		return true;
	}

	public override List<StiPage> GetNestedPages()
	{
		List<StiPage> list = new List<StiPage>();
		string pageKey = "";
		if (DashboardInteraction != null && DashboardInteraction.OnClick != 0)
		{
			pageKey = DashboardInteraction?.DrillDownPageKey;
			if (StiKeyHelper.IsKey(pageKey))
			{
				StiPage stiPage = Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == pageKey);
				if (stiPage != null)
				{
					list.Add(stiPage);
				}
			}
		}
		foreach (StiTableColumn column in Columns)
		{
			if (column?.DashboardInteraction == null || column.DashboardInteraction.OnClick == StiInteractionOnClick.None)
			{
				continue;
			}
			pageKey = column?.DashboardInteraction?.DrillDownPageKey;
			if (StiKeyHelper.IsKey(pageKey))
			{
				StiPage stiPage2 = Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == pageKey);
				if (stiPage2 != null)
				{
					list.Add(stiPage2);
				}
			}
		}
		if (list.Count != 0)
		{
			return list.Distinct().ToList();
		}
		return null;
	}

	internal override List<object> GetFormatObjects()
	{
		return StiTableElementSelection.GetSelectedObjects(this);
	}

	public override StiComponent CreateNew()
	{
		return new StiTableElement();
	}

	public StiTableElement()
		: this(RectangleD.Empty)
	{
	}

	public StiTableElement(RectangleD rect)
		: base(rect)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		Title.Text = LocalizedName;
		DashboardInteraction = new StiTableDashboardInteraction();
		Layout = new StiElementLayout(this);
	}
}
