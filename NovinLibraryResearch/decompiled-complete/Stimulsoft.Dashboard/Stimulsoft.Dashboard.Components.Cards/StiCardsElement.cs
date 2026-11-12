using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
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

namespace Stimulsoft.Dashboard.Components.Cards;

[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiCardsElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiCardsElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiCardsElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiCardsElement : StiElement, IStiCardsElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiConvertibleElement, IStiDataFilters, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiElementLayout, IStiGlobalizationProvider
{
	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	private StiMargin cardMargin = new StiMargin(12.0);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCardsElement;

	public override int ToolboxPosition => 302;

	public override string LocalizedName => StiLocalization.Get("Components", "StiCards");

	public override string HelpUrl => "user-manual/dashboards_cards.htm";

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

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


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

	[Browsable(false)]
	public StiElementLayout Layout { get; set; }

	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiDashboardInteractionConverter))]
	[Browsable(false)]
	public virtual IStiDashboardInteraction DashboardInteraction { get; set; }

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

	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[DefaultValue(StiItemOrientation.Horizontal)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiItemOrientation Orientation { get; set; } = StiItemOrientation.Horizontal;


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiCardsColumn> Columns { get; set; } = new List<StiCardsColumn>();


	[StiSerializable]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(280)]
	[Description("Gets or sets card margin.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiMargin CardMargin
	{
		get
		{
			return cardMargin;
		}
		set
		{
			if (cardMargin != value)
			{
				cardMargin = value;
				InvokeRefreshControl("Margin");
			}
		}
	}

	[StiSerializable]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(300)]
	[Description("Specifies the interior spacing of the component.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiPadding CardPadding { get; set; } = new StiPadding(12.0);


	[StiSerializable]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[DefaultValue(0)]
	[Description("Gets or sets amount of columns.")]
	[StiPropertyLevel(StiLevel.Standard)]
	public int ColumnCount { get; set; }

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

	public override object Clone(bool cloneProperties)
	{
		StiCardsElement stiCardsElement = ((StiElement)base.Clone(cloneProperties)) as StiCardsElement;
		stiCardsElement.Columns = Columns.Select((StiCardsColumn c) => c.Clone() as StiCardsColumn).ToList();
		stiCardsElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiCardsElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiCardsElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiCardsElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiCardsElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiCardsElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("Columns", StiJsonReportObjectHelper.Serialize.JObjectCollection(Columns, mode));
		jObject.AddPropertyEnum("Orientation", Orientation, StiItemOrientation.Horizontal);
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		if (Layout != null)
		{
			jObject.AddPropertyJObject("Layout", Layout.SaveToJsonObject(mode));
		}
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyInt("ColumnCount", ColumnCount);
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		if (CardMargin != null)
		{
			jObject.AddPropertyJObject("CardMargin", CardMargin.SaveToJsonObject(mode, 12.0, 12.0, 12.0, 12.0));
		}
		if (CardPadding != null)
		{
			jObject.AddPropertyJObject("CardPadding", CardPadding.SaveToJsonObject(mode, 12.0, 12.0, 12.0, 12.0));
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
			case "Columns":
				Columns.AddRange(from p in ((JObject)item.Value).Properties()
					select StiMeterLoader.LoadFromJson((JObject)p.Value) as StiCardsColumn into c
					where c != null
					select c);
				break;
			case "Orientation":
				Orientation = item.DeserializeEnum<StiItemOrientation>();
				break;
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Layout":
				Layout = StiElementLayout.CreateFromJsonObject((JObject)item.Value, this);
				break;
			case "Style":
				Style = item.DeserializeEnum<StiElementStyleIdent>();
				break;
			case "CustomStyleName":
				CustomStyleName = item.DeserializeString();
				break;
			case "ColumnCount":
				ColumnCount = item.DeserializeInt();
				break;
			case "DataFilters":
				DataFilters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
				break;
			case "CardMargin":
				CardMargin.LoadFromJsonObject((JObject)item.Value);
				break;
			case "CardPadding":
				CardPadding.LoadFromJsonObject((JObject)item.Value);
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
		StiCardsElement stiCardsElement = source as StiCardsElement;
		Columns.Clear();
		foreach (StiCardsColumn column in stiCardsElement.Columns)
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
				Columns.Add(StiMeterHelper.Cards.GetMeasure(column));
			}
			else
			{
				Columns.Add(StiMeterHelper.Cards.GetDimension(column));
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
			StiMeasureCardsColumn measure = StiMeterHelper.Cards.GetMeasure(stiDataColumn);
			Columns.Add(measure);
		}
		else
		{
			Columns.Add(StiMeterHelper.Cards.GetDimension(cell));
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
		if (meter is StiCardsColumn item)
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
		InsertMeter(index, new StiDimensionCardsColumn
		{
			HorAlignment = StiHorAlignment.Left
		});
	}

	public void InsertNewMeasure(int index)
	{
		InsertMeter(index, new StiMeasureCardsColumn
		{
			Expression = "Sum()",
			HorAlignment = StiHorAlignment.Right
		});
	}

	public IStiMeter GetMeasure(IStiAppDataCell cell)
	{
		return StiMeterHelper.Cards.GetMeasure(cell);
	}

	public IStiMeter GetDimension(IStiAppDataCell cell)
	{
		return StiMeterHelper.Cards.GetDimension(cell);
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		Columns.Clear();
		list?.Cast<StiMeter>().ToList().ForEach(delegate(StiMeter m)
		{
			StiCardsColumn stiCardsColumn = null;
			if (m is IStiDimensionMeter)
			{
				stiCardsColumn = new StiDimensionCardsColumn();
			}
			else if (m is IStiMeasureMeter)
			{
				stiCardsColumn = new StiMeasureCardsColumn
				{
					HorAlignment = StiHorAlignment.Right
				};
			}
			if (stiCardsColumn != null)
			{
				stiCardsColumn.LoadFromJsonObject(m.SaveToJsonObject(StiJsonSaveMode.Report));
				Columns.Add(stiCardsColumn);
			}
		});
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiCardsDashboardInteraction();
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
		List<string> list = new List<string> { "Title.Text" };
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

	private bool ShouldSerializeColumns()
	{
		if (Columns != null)
		{
			return Columns.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeCardMargin()
	{
		if (CardMargin.Left == 12.0 && CardMargin.Top == 12.0 && CardMargin.Right == 12.0)
		{
			return CardMargin.Bottom != 12.0;
		}
		return true;
	}

	private bool ShouldSerializeCardPadding()
	{
		if (CardPadding.Left == 12.0 && CardPadding.Top == 12.0 && CardPadding.Right == 12.0)
		{
			return CardPadding.Bottom != 12.0;
		}
		return true;
	}

	private bool ShouldSerializeColumnCount()
	{
		return ColumnCount > 0;
	}

	internal override List<object> GetFormatObjects()
	{
		return StiCardsElementSelection.GetSelectedObjects(this);
	}

	public override StiComponent CreateNew()
	{
		return new StiCardsElement();
	}

	public StiCardsElement()
		: this(RectangleD.Empty)
	{
	}

	public StiCardsElement(RectangleD rect)
		: base(rect)
	{
		Title.Text = LocalizedName;
		Layout = new StiElementLayout(this);
	}
}
