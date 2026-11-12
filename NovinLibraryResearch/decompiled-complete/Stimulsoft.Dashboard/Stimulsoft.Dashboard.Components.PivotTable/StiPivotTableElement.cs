using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Components.PivotTable.Design;
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
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.PivotTable;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiPivotTable.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiPivotTableElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiPivotTableElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiPivotTableElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiPivotTableElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiPivotTableElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiPivotTableElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiPivotTableElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiPivotTableElement : StiElement, IStiPivotTableElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiTransformActions, IStiTransformFilters, IStiTransformSorts, IStiDataTransformationElement, IStiGroupElement, IStiCrossFiltering, IStiDataFilters, IStiConvertibleElement, IStiSimpleShadow, IStiCornerRadius, IStiTitleElement, IStiElementLayout, IStiElementInteraction, IStiGlobalizationProvider
{
	public const int MaxIconSize = 50;

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPivotTableElement;

	[Browsable(false)]
	public override bool IsDefined
	{
		get
		{
			if ((Columns == null || Columns.Count <= 0) && (Rows == null || Rows.Count <= 0))
			{
				if (Summaries != null)
				{
					return Summaries.Count > 0;
				}
				return false;
			}
			return true;
		}
	}

	[StiSerializable]
	[StiCategory("PivotTable", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(102)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("PivotTable", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(100)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool CrossFiltering { get; set; } = true;


	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[Browsable(false)]
	public StiElementLayout Layout { get; set; }

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
	[StiCategory("PivotTable", StiCategorySource.PropertyCategory)]
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

	public override int ToolboxPosition => 303;

	public override string LocalizedName => Loc.Get("Components", "StiPivotTable");

	public override string HelpUrl => "user-manual/dashboards_pivot_table.htm";

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

	[Browsable(false)]
	public bool IsPivotTableExporting { get; set; }

	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiPivotColumn> Columns { get; set; } = new List<StiPivotColumn>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiPivotRow> Rows { get; set; } = new List<StiPivotRow>();


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiMeterListConverter))]
	[Browsable(false)]
	public List<StiPivotSummary> Summaries { get; set; } = new List<StiPivotSummary>();


	[StiSerializable]
	[DefaultValue("")]
	[Browsable(false)]
	public string TotalLabel { get; set; } = "";


	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiPivotTableConditionConverter))]
	[Browsable(false)]
	public List<IStiPivotTableElementCondition> PivotTableConditions { get; set; } = new List<IStiPivotTableElementCondition>();


	[RefreshProperties(RefreshProperties.All)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiPivotTableConditionsEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(140)]
	[StiSerializable(StiSerializationVisibility.List)]
	[TypeConverter(typeof(StiPivotTableConditionConverter))]
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

	[DefaultValue(StiSummaryDirection.UpToDown)]
	[StiSerializable]
	[Browsable(false)]
	public StiSummaryDirection SummaryDirection { get; set; } = StiSummaryDirection.UpToDown;


	public override object Clone(bool cloneProperties)
	{
		StiPivotTableElement stiPivotTableElement = ((StiElement)base.Clone(cloneProperties)) as StiPivotTableElement;
		stiPivotTableElement.Columns = Columns.Select((StiPivotColumn c) => c.Clone() as StiPivotColumn).ToList();
		stiPivotTableElement.Rows = Rows.Select((StiPivotRow c) => c.Clone() as StiPivotRow).ToList();
		stiPivotTableElement.Summaries = Summaries.Select((StiPivotSummary c) => c.Clone() as StiPivotSummary).ToList();
		stiPivotTableElement.TransformActions = TransformActions.Select((StiDataActionRule c) => c.Clone() as StiDataActionRule).ToList();
		stiPivotTableElement.TransformFilters = TransformFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiPivotTableElement.TransformSorts = TransformSorts.Select((StiDataSortRule c) => c.Clone() as StiDataSortRule).ToList();
		stiPivotTableElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiPivotTableElement.Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiPivotTableElement.DashboardInteraction = DashboardInteraction?.Clone() as StiDashboardInteraction;
		stiPivotTableElement.PivotTableConditions = PivotTableConditions.Select((IStiPivotTableElementCondition c) => c.Clone() as IStiPivotTableElementCondition).ToList();
		stiPivotTableElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiPivotTableElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiPivotTableElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("TransformActions", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformActions, mode));
		jObject.AddPropertyJObject("TransformFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformFilters, mode));
		jObject.AddPropertyJObject("TransformSorts", StiJsonReportObjectHelper.Serialize.JObjectCollection(TransformSorts, mode));
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		jObject.AddPropertyJObject("Columns", StiJsonReportObjectHelper.Serialize.JObjectCollection(Columns, mode));
		jObject.AddPropertyJObject("Rows", StiJsonReportObjectHelper.Serialize.JObjectCollection(Rows, mode));
		jObject.AddPropertyJObject("Summaries", StiJsonReportObjectHelper.Serialize.JObjectCollection(Summaries, mode));
		if (DashboardInteraction is StiPivotTableDashboardInteraction stiPivotTableDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiPivotTableDashboardInteraction.SaveToJsonObject(mode));
		}
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		jObject.AddPropertyStringNullOrEmpty("TotalLabel", TotalLabel);
		jObject.AddPropertyJObject("PivotTableConditions", StiJsonReportObjectHelper.Serialize.JObjectCollection(PivotTableConditions, mode));
		jObject.AddPropertyEnum("SummaryDirection", SummaryDirection);
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
				StiPivotTableDashboardInteraction stiPivotTableDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiPivotTableDashboardInteraction;
				DashboardInteraction = stiPivotTableDashboardInteraction ?? null;
				break;
			}
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
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Layout":
				Layout = StiElementLayout.CreateFromJsonObject((JObject)item.Value, this);
				break;
			case "Columns":
				Columns.AddRange(from p in ((JObject)item.Value).Properties()
					select StiMeterLoader.LoadFromJson((JObject)p.Value) as StiPivotColumn into c
					where c != null
					select c);
				break;
			case "Rows":
				Rows.AddRange(from p in ((JObject)item.Value).Properties()
					select StiMeterLoader.LoadFromJson((JObject)p.Value) as StiPivotRow into c
					where c != null
					select c);
				break;
			case "Summaries":
				Summaries.AddRange(from p in ((JObject)item.Value).Properties()
					select StiMeterLoader.LoadFromJson((JObject)p.Value) as StiPivotSummary into c
					where c != null
					select c);
				break;
			case "Style":
				Style = item.DeserializeEnum<StiElementStyleIdent>();
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
			case "SummaryDirection":
				SummaryDirection = item.DeserializeEnum<StiSummaryDirection>();
				break;
			case "TotalLabel":
				TotalLabel = item.DeserializeString();
				break;
			case "PivotTableConditions":
				PivotTableConditions.AddRange(from p in ((JObject)item.Value).Properties()
					select StiPivotTableElementCondition.LoadFromJson((JObject)p.Value));
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

	public override List<string> RetrieveUsedDataNames()
	{
		List<string> many = StiUsedDataHelper.GetMany(Columns);
		many.AddRange(StiUsedDataHelper.GetMany(Rows));
		many.AddRange(StiUsedDataHelper.GetMany(Summaries));
		return many;
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		return GetMeters();
	}

	public override List<IStiMeter> GetMeters()
	{
		List<IStiMeter> list = Columns.Cast<IStiMeter>().ToList();
		list.AddRange(Rows.Cast<IStiMeter>().ToList());
		list.AddRange(Summaries.Cast<IStiMeter>().ToList());
		return list;
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

	public void CreateNextMeter(IStiAppDataCell cell)
	{
		if (Columns.Count == 0)
		{
			Columns.Add(StiMeterHelper.Pivot.GetColumn(cell));
		}
		else if (Rows.Count == 0)
		{
			Rows.Add(StiMeterHelper.Pivot.GetRow(cell));
		}
		else if (Summaries.Count == 0)
		{
			Summaries.Add(StiMeterHelper.Pivot.GetSummary(cell));
		}
		else
		{
			Columns.Add(StiMeterHelper.Pivot.GetColumn(cell));
		}
	}

	public void CreateNewColumn()
	{
		Columns.Add(new StiPivotColumn());
	}

	public IStiMeter GetColumn(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Pivot.GetColumn(cell);
	}

	public IStiMeter GetColumn(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Pivot.GetColumn(meter as StiMeter);
	}

	public IStiMeter GetColumnByIndex(int index)
	{
		if (index >= Columns.Count)
		{
			return null;
		}
		return Columns[index];
	}

	public void InsertColumn(int index, IStiMeter meter)
	{
		if (meter is StiPivotColumn item)
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

	public void RemoveColumn(int index)
	{
		if (index < Columns.Count)
		{
			Columns.RemoveAt(index);
		}
	}

	public void RemoveAllColumns()
	{
		Columns.Clear();
	}

	public void CreateNewRow()
	{
		Rows.Add(new StiPivotRow());
	}

	public IStiMeter GetRow(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Pivot.GetRow(cell);
	}

	public IStiMeter GetRow(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Pivot.GetRow(meter as StiMeter);
	}

	public IStiMeter GetRowByIndex(int index)
	{
		if (index >= Rows.Count)
		{
			return null;
		}
		return Rows[index];
	}

	public void InsertRow(int index, IStiMeter meter)
	{
		if (meter is StiPivotRow item)
		{
			if (index >= 0 && index < Rows.Count)
			{
				Rows.Insert(index, item);
			}
			else
			{
				Rows.Add(item);
			}
		}
	}

	public void RemoveRow(int index)
	{
		if (index < Rows.Count)
		{
			Rows.RemoveAt(index);
		}
	}

	public void RemoveAllRows()
	{
		Rows.Clear();
	}

	public void CreateNewSummary()
	{
		Summaries.Add(new StiPivotSummary
		{
			Expression = "Sum()"
		});
	}

	public IStiMeter GetSummary(IStiAppDataCell cell)
	{
		if (cell == null)
		{
			return null;
		}
		return StiMeterHelper.Pivot.GetSummary(cell);
	}

	public IStiMeter GetSummary(IStiMeter meter)
	{
		if (meter == null)
		{
			return null;
		}
		return StiMeterHelper.Pivot.GetSummary(meter as StiMeter);
	}

	public IStiMeter GetSummaryByIndex(int index)
	{
		if (index >= Summaries.Count)
		{
			return null;
		}
		return Summaries[index];
	}

	public void InsertSummary(int index, IStiMeter meter)
	{
		if (meter is StiPivotSummary item)
		{
			if (index >= 0 && index < Summaries.Count)
			{
				Summaries.Insert(index, item);
			}
			else
			{
				Summaries.Add(item);
			}
		}
	}

	public void RemoveSummary(int index)
	{
		if (index < Summaries.Count)
		{
			Summaries.RemoveAt(index);
		}
	}

	public void RemoveAllSummaries()
	{
		Summaries.Clear();
	}

	public void AddPivotTableCondition(string keyValueMeter, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, Font font, Color textColor, Color backColor, StiConditionPermissions permissions, StiFontIcons icon, StiIconAlignment iconAlignment, byte[] customIcon, Color iconColor)
	{
		PivotTableConditions.Add(new StiPivotTableElementCondition(keyValueMeter, dataType, condition, value, font, textColor, backColor, permissions, icon, iconAlignment, customIcon, iconColor));
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		Columns.Clear();
		Rows.Clear();
		Summaries.Clear();
		int index = 0;
		list?.Cast<StiMeter>().ToList().ForEach(delegate(StiMeter m)
		{
			if (m is IStiDimensionMeter)
			{
				if ((index & 1) == 0)
				{
					StiPivotColumn stiPivotColumn = new StiPivotColumn();
					stiPivotColumn.LoadFromJsonObject(m.SaveToJsonObject(StiJsonSaveMode.Report));
					Columns.Add(stiPivotColumn);
				}
				else
				{
					StiPivotRow stiPivotRow = new StiPivotRow();
					stiPivotRow.LoadFromJsonObject(m.SaveToJsonObject(StiJsonSaveMode.Report));
					Rows.Add(stiPivotRow);
				}
			}
			else if (m is IStiMeasureMeter)
			{
				StiPivotSummary stiPivotSummary = new StiPivotSummary();
				stiPivotSummary.LoadFromJsonObject(m.SaveToJsonObject(StiJsonSaveMode.Report));
				Summaries.Add(stiPivotSummary);
			}
		});
		if (DashboardInteraction == null)
		{
			DashboardInteraction = new StiPivotTableDashboardInteraction();
		}
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		if (!(propertyName == "Title.Text"))
		{
			if (propertyName == "TotalLabel")
			{
				TotalLabel = value;
			}
		}
		else
		{
			Title.Text = value;
		}
		int num = 0;
		foreach (StiPivotColumn column in Columns)
		{
			string text = $"Column{num++}.Label";
			if (propertyName == text)
			{
				column.Label = value;
			}
		}
		num = 0;
		foreach (StiPivotRow row in Rows)
		{
			string text2 = $"Row{num++}.Label";
			if (propertyName == text2)
			{
				row.Label = value;
			}
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		if (!(propertyName == "Title.Text"))
		{
			if (propertyName == "TotalLabel")
			{
				return TotalLabel;
			}
			int num = 0;
			foreach (StiPivotColumn column in Columns)
			{
				string text = $"Column{num++}.Label";
				if (propertyName == text)
				{
					return column.Label;
				}
			}
			num = 0;
			foreach (StiPivotRow row in Rows)
			{
				string text2 = $"Row{num++}.Label";
				if (propertyName == text2)
				{
					return row.Label;
				}
			}
			throw new ArgumentException("Property with name " + propertyName);
		}
		return Title.Text;
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		List<string> list = new List<string> { "Title.Text", "TotalLabel" };
		int num = 0;
		foreach (StiPivotColumn column in Columns)
		{
			list.Add($"Column{num++}.Label");
		}
		num = 0;
		foreach (StiPivotRow row in Rows)
		{
			list.Add($"Row{num++}.Label");
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

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		foreach (IStiPivotTableElementCondition pivotTableCondition in PivotTableConditions)
		{
			fonts.Add(new StiFont(pivotTableCondition.Font));
			if (pivotTableCondition.IconAlignment != 0)
			{
				fonts.Add(StiFontIconsHelper.GetFont());
			}
		}
		fonts.Add(new StiFont(StiElementConsts.Table.Font.GetCachedGdiFont()));
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

	private bool ShouldSerializeRows()
	{
		if (Rows != null)
		{
			return Rows.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeSummaries()
	{
		if (Summaries != null)
		{
			return Summaries.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeConditions()
	{
		if (PivotTableConditions != null)
		{
			return PivotTableConditions.Count > 0;
		}
		return true;
	}

	public override StiComponent CreateNew()
	{
		return new StiPivotTableElement();
	}

	internal override List<object> GetFormatObjects()
	{
		return StiPivotTableElementSelection.GetSelectedObjects(this);
	}

	public List<IStiMeter> GetAllMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		list.AddRange(Columns);
		list.AddRange(Rows);
		list.AddRange(Summaries);
		return list;
	}

	public StiPivotTableElement()
		: this(RectangleD.Empty)
	{
	}

	public StiPivotTableElement(RectangleD rect)
		: base(rect)
	{
		Title.Text = LocalizedName;
		DashboardInteraction = new StiPivotTableDashboardInteraction();
		Layout = new StiElementLayout(this);
	}
}
