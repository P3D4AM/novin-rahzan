using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Chart.Design;

public sealed class StiChartCommon
{
	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(90)]
	[DefaultValue(true)]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	public bool AllowApplyStyle
	{
		get
		{
			return Chart.AllowApplyStyle;
		}
		set
		{
			Chart.AllowApplyStyle = value;
		}
	}

	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(160)]
	[DefaultValue(false)]
	[Description("Gets or sets value indicates that a chart is processed at the end of the report execution.")]
	public bool ProcessAtEnd
	{
		get
		{
			return Chart.ProcessAtEnd;
		}
		set
		{
			Chart.ProcessAtEnd = value;
		}
	}

	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[DefaultValue(10)]
	[StiOrder(130)]
	[Description("Gets os sets horizontal space between border of the chart and the chart.")]
	public int HorSpacing
	{
		get
		{
			return Chart.HorSpacing;
		}
		set
		{
			Chart.HorSpacing = value;
		}
	}

	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[DefaultValue(10)]
	[StiOrder(230)]
	[Description("Gets os sets vertical space between border of the chart and the chart.")]
	public int VertSpacing
	{
		get
		{
			return Chart.VertSpacing;
		}
		set
		{
			Chart.VertSpacing = value;
		}
	}

	[StiCategory("Chart", StiCategorySource.PropertyCategory)]
	[DefaultValue(StiImageRotation.None)]
	[StiOrder(170)]
	[Description("Gets or sets value which indicates how to rotate an chart before output.")]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiImageRotation Rotation
	{
		get
		{
			return Chart.Rotation;
		}
		set
		{
			Chart.Rotation = value;
		}
	}

	[StiCategory("Data", StiCategorySource.PropertyCategory)]
	[StiOrder(100)]
	[StiPropertyLevel(StiLevel.Basic)]
	[TypeConverter(typeof(StiDataSourceConverter))]
	[DefaultValue(null)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Get data source that is used for getting data.")]
	public StiDataSource DataSource => Chart.DataSource;

	[Editor("Stimulsoft.Report.Components.Design.StiDataRelationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Data", StiCategorySource.PropertyCategory)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiDataRelationConverter))]
	[DefaultValue(null)]
	[Description("Get link that is used for master-detail reports rendering.")]
	[StiPropertyLevel(StiLevel.Professional)]
	public StiDataRelation DataRelation => Chart.DataRelation;

	[TypeConverter(typeof(StiBusinessObjectConverter))]
	[Description("Get Business Object that is used for getting data.")]
	[StiCategory("Data", StiCategorySource.PropertyCategory)]
	[StiOrder(125)]
	[RefreshProperties(RefreshProperties.All)]
	[StiEngine(StiEngineVersion.EngineV2)]
	[StiPropertyLevel(StiLevel.Professional)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiBusinessObject BusinessObject => Chart.BusinessObject;

	[Description("Gets or sets the master-component.")]
	[StiOrder(130)]
	[StiPropertyLevel(StiLevel.Professional)]
	[TypeConverter(typeof(StiMasterComponentConverter))]
	[DefaultValue(null)]
	[StiCategory("Data", StiCategorySource.PropertyCategory)]
	[Editor("Stimulsoft.Report.Components.Design.StiMasterComponentEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiComponent MasterComponent
	{
		get
		{
			return Chart.MasterComponent;
		}
		set
		{
			Chart.MasterComponent = value;
		}
	}

	[StiCategory("Data", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Professional)]
	[StiOrder(140)]
	[DefaultValue(0)]
	[Description("Gets or sets the count of rows for virtual data.")]
	public int CountData
	{
		get
		{
			return Chart.CountData;
		}
		set
		{
			Chart.CountData = value;
		}
	}

	[StiCategory("Data", StiCategorySource.PropertyCategory)]
	[StiOrder(160)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates, that filter is turn on.")]
	[StiPropertyLevel(StiLevel.Professional)]
	[DefaultValue(true)]
	public bool FilterOn
	{
		get
		{
			return Chart.FilterOn;
		}
		set
		{
			Chart.FilterOn = value;
		}
	}

	[StiOrder(170)]
	[StiCategory("Data", StiCategorySource.PropertyCategory)]
	[Editor("Stimulsoft.Report.Components.Design.StiFiltersCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the collection of data filters.")]
	[StiPropertyLevel(StiLevel.Professional)]
	[TypeConverter(typeof(StiFiltersCollectionConverter))]
	public StiFiltersCollection Filters
	{
		get
		{
			return Chart.Filters;
		}
		set
		{
			Chart.Filters = value;
		}
	}

	[TypeConverter(typeof(StiChartCommonSortConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiSortEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Data", StiCategorySource.PropertyCategory)]
	[StiOrder(210)]
	[Description("Gets or sets the array of strings that describes rules of sorting.")]
	[StiPropertyLevel(StiLevel.Professional)]
	public string[] Sort
	{
		get
		{
			return Chart.Sort;
		}
		set
		{
			Chart.Sort = value;
		}
	}

	[Browsable(false)]
	public StiChart Chart { get; set; }

	private bool ShouldSerializeFilters()
	{
		if (Filters != null)
		{
			return Filters.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeSort()
	{
		if (Sort != null)
		{
			return Sort.Length != 0;
		}
		return true;
	}

	public StiChartCommon(StiChart chart)
	{
		Chart = chart;
	}
}
