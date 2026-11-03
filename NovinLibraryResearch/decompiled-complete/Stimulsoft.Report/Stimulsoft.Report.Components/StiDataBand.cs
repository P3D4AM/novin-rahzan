using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiDataBand), "Stimulsoft.Report.Images.Components.StiDataBand.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiDataBandDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfDataBandDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiDataBandV1Builder))]
[StiV2Builder(typeof(StiDataBandV2Builder))]
[StiContextTool(typeof(IStiDataSource))]
[StiContextTool(typeof(IStiMasterComponent))]
[StiContextTool(typeof(IStiDataRelation))]
[StiContextTool(typeof(IStiSort))]
[StiContextTool(typeof(IStiKeepGroupTogether))]
[StiContextTool(typeof(IStiKeepChildTogether))]
[StiContextTool(typeof(IStiKeepHeaderTogether))]
[StiContextTool(typeof(IStiKeepFooterTogether))]
[StiContextTool(typeof(IStiPrintOnAllPages))]
[StiContextTool(typeof(IStiPrintIfDetailEmpty))]
[StiContextTool(typeof(IStiStartNewPage))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiBreakable))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiDataSourceQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 0)]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiDataRelationQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 1)]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiMasterComponentQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 2)]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiBandQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 3)]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 4)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfDataSourceQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 0)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfDataRelationQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 1)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfMasterComponentQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 2)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfBandQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 3)]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", 4)]
public class StiDataBand : StiDynamicBand, IStiDataSource, IStiEnumerator, IStiMasterComponent, IStiDataRelation, IStiOddEvenStyles, IStiSort, IStiFilter, IStiStartNewPage, IStiPrintOnAllPages, IStiPrintIfDetailEmpty, IStiKeepDetailsTogether, IStiKeepFooterTogether, IStiKeepGroupTogether, IStiKeepHeaderTogether, IStiSelectedLine, IStiRenderMaster, IStiBusinessObject
{
	internal string jsonMasterComponentTemp;

	private StiKeepDetails keepDetails;

	private string dataSourceName = string.Empty;

	private string businessObjectGuid = string.Empty;

	internal bool isEofValue;

	internal bool isBofValue;

	internal int positionValue;

	public static object PropertyEvenStyle = new object();

	public static object PropertyOddStyle = new object();

	private StiDataBandInfoV1 dataBandInfoV1;

	private StiDataBandInfoV2 dataBandInfoV2;

	private static readonly object EventBeginRender = new object();

	private static readonly object EventRendering = new object();

	private static readonly object EventEndRender = new object();

	private static readonly object EventGetCollapsed = new object();

	private bool rightToLeft;

	private double columnGaps;

	private double columnWidth;

	private int columns;

	private int minRowsInColumn;

	private StiColumnDirection columnDirection = StiColumnDirection.AcrossThenDown;

	private int line = 1;

	private int countData;

	public override StiComponentId ComponentId => StiComponentId.StiDataBand;

	public override string HelpUrl => "User-Manual/report_internals_creating_lists_data_band.htm";

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(420)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Allows to reset page number on this component.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool ResetPageNumber
	{
		get
		{
			return base.ResetPageNumber;
		}
		set
		{
			base.ResetPageNumber = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiMasterComponentConverter))]
	[DefaultValue(null)]
	[Description("Gets or sets the master-component.")]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiMasterComponentEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiComponent MasterComponent { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(460)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that it is necessary to print every new string on a new page.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool StartNewPage { get; set; }

	[DefaultValue(100f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(470)]
	[Description("Gets or sets value which indicates how much free space on a page (in percentage terms) should be reserved for formation of a new page. The value should be set in the range from 0 to 100. If the value is 100 then, in any case, a new page will be formed. This property is used together with the StartNewPage property.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float StartNewPageIfLessThan
	{
		get
		{
			return BreakIfLessThan;
		}
		set
		{
			BreakIfLessThan = value;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(240)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that group is to be kept together.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepGroupTogether { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(260)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that header is printed with data together.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepHeaderTogether { get; set; } = true;


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(250)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the footer is printed with data.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepFooterTogether { get; set; } = true;


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that childs are to be kept together.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepChildTogether
	{
		get
		{
			return KeepDetailsTogether;
		}
		set
		{
			KeepDetailsTogether = value;
		}
	}

	[DefaultValue(false)]
	[Description("Gets or sets value indicates that details are to be kept together with this DataBand.")]
	[Browsable(false)]
	public virtual bool KeepDetailsTogether
	{
		get
		{
			return keepDetails == StiKeepDetails.KeepDetailsTogether;
		}
		set
		{
			StiKeepDetails stiKeepDetails = (value ? StiKeepDetails.KeepDetailsTogether : StiKeepDetails.None);
			if (keepDetails != stiKeepDetails)
			{
				CheckBlockedException("KeepDetailsTogether");
				keepDetails = stiKeepDetails;
			}
		}
	}

	[DefaultValue(StiKeepDetails.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(230)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value indicates how details are to be kept together with this DataBand.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiKeepDetails KeepDetails
	{
		get
		{
			return keepDetails;
		}
		set
		{
			if (keepDetails != value)
			{
				CheckBlockedException("KeepDetails");
				keepDetails = value;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiSortConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiSortEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[Description("Gets or sets the array of strings that describes rules of sorting.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string[] Sort { get; set; } = new string[0];


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(370)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the component is printed on all pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintOnAllPages { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(340)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that if detail is empty then the master data must be printed anyway.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintIfDetailEmpty { get; set; }

	[Browsable(false)]
	public bool IsDataSourceEmpty
	{
		get
		{
			if (!string.IsNullOrEmpty(DataSourceName))
			{
				return DataSource == null;
			}
			return true;
		}
	}

	[TypeConverter(typeof(StiDataSourceConverter))]
	[RefreshProperties(RefreshProperties.All)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Get data source that is used for getting data.")]
	public virtual StiDataSource DataSource
	{
		get
		{
			if (Page == null || Report == null || Report.DataSources == null || DataSourceName == null || DataSourceName.Length == 0)
			{
				return null;
			}
			return Report.DataSources[DataSourceName];
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue("")]
	public string DataSourceName
	{
		get
		{
			return dataSourceName;
		}
		set
		{
			if (!(dataSourceName == value))
			{
				dataSourceName = value;
				if (!string.IsNullOrEmpty(value))
				{
					BusinessObjectGuid = null;
				}
				if (!string.IsNullOrEmpty(value))
				{
					CountData = 0;
				}
				StiOptions.Engine.GlobalEvents.InvokeDataSourceAssigned(this, EventArgs.Empty);
			}
		}
	}

	[Browsable(false)]
	public bool IsBusinessObjectEmpty
	{
		get
		{
			if (!string.IsNullOrEmpty(BusinessObjectGuid))
			{
				return BusinessObject == null;
			}
			return true;
		}
	}

	[TypeConverter(typeof(StiBusinessObjectConverter))]
	[Description("Get Business Object that is used for getting data.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(125)]
	[RefreshProperties(RefreshProperties.All)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual StiBusinessObject BusinessObject
	{
		get
		{
			if (Page == null || Report == null || BusinessObjectGuid == null || BusinessObjectGuid.Length == 0)
			{
				return null;
			}
			return StiBusinessObjectHelper.GetBusinessObjectFromGuid(Report, BusinessObjectGuid);
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue("")]
	public string BusinessObjectGuid
	{
		get
		{
			return businessObjectGuid;
		}
		set
		{
			if (!(businessObjectGuid == value))
			{
				businessObjectGuid = value;
				if (!string.IsNullOrEmpty(value))
				{
					DataSourceName = null;
				}
				StiOptions.Engine.GlobalEvents.InvokeBusinessObjectAssigned(this, EventArgs.Empty);
			}
		}
	}

	[Browsable(false)]
	public int SelectedLine { get; set; } = -1;


	[Browsable(false)]
	public virtual bool IsEof
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.IsEof;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.IsEof;
			}
			return isEofValue;
		}
		set
		{
			if (!IsBusinessObjectEmpty)
			{
				BusinessObject.IsEof = value;
			}
			else if (!IsDataSourceEmpty)
			{
				DataSource.IsEof = value;
			}
			else
			{
				isEofValue = value;
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsBof
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.IsBof;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.IsBof;
			}
			return isBofValue;
		}
		set
		{
			if (!IsBusinessObjectEmpty)
			{
				BusinessObject.IsBof = value;
			}
			else if (!IsDataSourceEmpty)
			{
				DataSource.IsBof = value;
			}
			else
			{
				isBofValue = value;
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsEmpty
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.IsEmpty;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.IsEmpty;
			}
			return CountData == 0;
		}
	}

	[Browsable(false)]
	public virtual int Position
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.Position;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.Position;
			}
			return positionValue;
		}
		set
		{
			if (!IsBusinessObjectEmpty)
			{
				BusinessObject.Position = value;
			}
			else if (!IsDataSourceEmpty)
			{
				DataSource.Position = value;
			}
			else
			{
				positionValue = value;
			}
		}
	}

	[Browsable(false)]
	public virtual int Count
	{
		get
		{
			if (!IsBusinessObjectEmpty)
			{
				return BusinessObject.Count;
			}
			if (!IsDataSourceEmpty)
			{
				return DataSource.Count;
			}
			return countData;
		}
	}

	[TypeConverter(typeof(StiDataRelationConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiDataRelationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Get link that is used for master-detail reports rendering.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiDataRelation DataRelation
	{
		get
		{
			if (Page == null || Report == null || Report.Dictionary == null || Report.Dictionary.Relations == null || DataRelationName == null || DataRelationName.Length == 0)
			{
				return null;
			}
			return Report.Dictionary.Relations[DataRelationName];
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue("")]
	public string DataRelationName { get; set; } = "";


	[DefaultValue(StiFilterMode.And)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(190)]
	[Description("Gets or sets filter mode.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiFilterMode FilterMode { get; set; }

	[DefaultValue(StiFilterEngine.ReportEngine)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[Browsable(true)]
	[Description("Gets or sets how a filter will be applied to data - be means of the report generator or by means of changing the SQL query.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiFilterEngine FilterEngine { get; set; }

	[Browsable(false)]
	public StiFilterEventHandler FilterMethodHandler { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[TypeConverter(typeof(StiFiltersCollectionConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiFiltersCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the collection of data filters.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiFiltersCollection Filters { get; set; } = new StiFiltersCollection();


	[Browsable(false)]
	[StiNonSerialized]
	[Obsolete("Filter property is obsolete. Please use Filters property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiExpression Filter
	{
		get
		{
			if (Filters.Count == 0)
			{
				Filters.Add(new StiFilter());
			}
			Filters[0].Item = StiFilterItem.Expression;
			return Filters[0].Expression;
		}
		set
		{
			if (Filters.Count == 0)
			{
				Filters.Add(new StiFilter());
			}
			Filters[0].Item = StiFilterItem.Expression;
			Filters[0].Expression = value;
		}
	}

	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets value indicates, that filter is turn on.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool FilterOn { get; set; } = true;


	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[Description("Gets or sets value, indicates style of even lines.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiExpressionStyleConverter))]
	[StiExpressionAllowed]
	public virtual string EvenStyle
	{
		get
		{
			return ((StiService)this).Properties.Get(PropertyEvenStyle, (object)string.Empty) as string;
		}
		set
		{
			((StiService)this).Properties.Set(PropertyEvenStyle, (object)value, (object)string.Empty);
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets value, indicates style of odd lines.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiExpressionStyleConverter))]
	[StiExpressionAllowed]
	public virtual string OddStyle
	{
		get
		{
			return ((StiService)this).Properties.Get(PropertyOddStyle, (object)string.Empty) as string;
		}
		set
		{
			((StiService)this).Properties.Set(PropertyOddStyle, (object)value, (object)string.Empty);
		}
	}

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(90, 147, 204);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(84, 175, 220);

	[Browsable(false)]
	public StiDataBandInfoV1 DataBandInfoV1 => dataBandInfoV1 ?? (dataBandInfoV1 = new StiDataBandInfoV1());

	[Browsable(false)]
	public StiDataBandInfoV2 DataBandInfoV2 => dataBandInfoV2 ?? (dataBandInfoV2 = new StiDataBandInfoV2());

	public override int ToolboxPosition => 11;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority => 300;

	public override StiComponentType ComponentType
	{
		get
		{
			if (MasterComponent == null)
			{
				return StiComponentType.Master;
			}
			if (MasterComponent != null)
			{
				if (Page.Skip && MasterComponent.Page != Page)
				{
					return StiComponentType.Master;
				}
				if (Parent != MasterComponent.Parent)
				{
					return StiComponentType.Master;
				}
			}
			return StiComponentType.Detail;
		}
	}

	public override string LocalizedName => StiLocalization.Get("Components", "StiDataBand");

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when band begin render.")]
	public virtual StiBeginRenderEvent BeginRenderEvent
	{
		get
		{
			return new StiBeginRenderEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when a data row rendering.")]
	public virtual StiRenderingEvent RenderingEvent
	{
		get
		{
			return new StiRenderingEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when band rendering is finished.")]
	public virtual StiEndRenderEvent EndRenderEvent
	{
		get
		{
			return new StiEndRenderEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the Collapsed value is calculated.")]
	public virtual StiGetCollapsedEvent GetCollapsedEvent
	{
		get
		{
			return new StiGetCollapsedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an expression to fill a collapsed value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiCollapsedExpression Collapsed
	{
		get
		{
			return new StiCollapsedExpression(this, "Collapsed");
		}
		set
		{
			value?.Set(this, "Collapsed", value.Value);
		}
	}

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets horizontal column direction.")]
	[StiOrder(150)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool RightToLeft
	{
		get
		{
			return rightToLeft;
		}
		set
		{
			if (rightToLeft != value)
			{
				CheckBlockedException("RightToLeft");
				rightToLeft = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(0.0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Gets or sets distance between two columns.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double ColumnGaps
	{
		get
		{
			return columnGaps;
		}
		set
		{
			if (columnGaps != value)
			{
				CheckBlockedException("ColumnGaps");
				if (value < 0.0)
				{
					throw new ArgumentOutOfRangeException("ColumnGaps", $"Value of '{value}' is not valid for 'ColumnGaps'. 'ColumnGaps' must be greater than or equal to 0.");
				}
				columnGaps = Math.Round(value, 2);
			}
		}
	}

	[StiSerializable]
	[DefaultValue(0.0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Description("Gets or sets width of column.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double ColumnWidth
	{
		get
		{
			return columnWidth;
		}
		set
		{
			if (columnWidth != value)
			{
				CheckBlockedException("ColumnWidth");
				if (value < 0.0)
				{
					throw new ArgumentOutOfRangeException("ColumnWidth", $"Value of '{value}' is not valid for 'ColumnWidth'. 'ColumnWidth' must be greater than or equal to 0.");
				}
				columnWidth = Math.Round(value, 2);
			}
		}
	}

	[StiSerializable]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets columns count.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int Columns
	{
		get
		{
			return columns;
		}
		set
		{
			if (columns != value)
			{
				CheckBlockedException("Columns");
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("Columns", $"Value of '{value}' is not valid for 'Columns'. 'Columns' must be greater than or equal to 0.");
				}
				columns = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[Description("Gets or sets minimum count of the rows in one column.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int MinRowsInColumn
	{
		get
		{
			return minRowsInColumn;
		}
		set
		{
			if (minRowsInColumn != value)
			{
				CheckBlockedException("MinRowsInColumn");
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MinRowsInColumn", $"Value of '{value}' is not valid for 'MinRowsInColumn'. 'MinRowsInColumn' must be greater than or equal to 0.");
				}
				minRowsInColumn = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiColumnDirection.AcrossThenDown)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets direction of the rendeting columns.")]
	public virtual StiColumnDirection ColumnDirection
	{
		get
		{
			return columnDirection;
		}
		set
		{
			if (columnDirection != value)
			{
				CheckBlockedException("ColumnDirection");
				columnDirection = value;
			}
		}
	}

	[Browsable(false)]
	public int LineThrough { get; set; } = 1;


	[Browsable(false)]
	public int Line
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			if (Report == null)
			{
				return line;
			}
			if ((int)Report.EngineVersion == 1)
			{
				return DataBandInfoV1.RuntimeLine - DataBandInfoV1.StartLine;
			}
			return line;
		}
		set
		{
			line = value;
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[Description("Gets or sets value, indicates to reset Data Source postion to begin when preparation for rendering.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ResetDataSource { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[Description("Gets or sets value which indicates that, when aggregate functions calculation, it is nesessary to take into consideration invisible data bands.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool CalcInvisible { get; set; }

	[StiSerializable]
	[DefaultValue(0)]
	[RefreshProperties(RefreshProperties.All)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[Description("Gets or sets the count of rows for virtual data.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int CountData
	{
		get
		{
			return countData;
		}
		set
		{
			if (countData != value)
			{
				countData = value;
				if (value > 0)
				{
					DataSourceName = string.Empty;
				}
			}
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets the limit of rows for break.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string LimitRows { get; set; } = string.Empty;


	public event EventHandler BeginRender
	{
		add
		{
			base.Events.AddHandler(EventBeginRender, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventBeginRender, value);
		}
	}

	public event EventHandler Rendering
	{
		add
		{
			base.Events.AddHandler(EventRendering, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventRendering, value);
		}
	}

	public event EventHandler EndRender
	{
		add
		{
			base.Events.AddHandler(EventEndRender, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventEndRender, value);
		}
	}

	public event StiValueEventHandler GetCollapsed
	{
		add
		{
			base.Events.AddHandler(EventGetCollapsed, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetCollapsed, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "StartNewPage", StartNewPage, false, false);
		JsonExtensions.AddPropertyFloat(val, "StartNewPageIfLessThan", StartNewPageIfLessThan, 100f);
		JsonExtensions.AddPropertyBool(val, "KeepGroupTogether", KeepGroupTogether, false, false);
		JsonExtensions.AddPropertyBool(val, "KeepHeaderTogether", KeepHeaderTogether, true, false);
		JsonExtensions.AddPropertyBool(val, "KeepFooterTogether", KeepFooterTogether, true, false);
		JsonExtensions.AddPropertyBool(val, "KeepChildTogether", KeepChildTogether, false, false);
		JsonExtensions.AddPropertyEnum(val, "KeepDetails", (Enum)KeepDetails, (Enum)StiKeepDetails.None);
		JsonExtensions.AddPropertyBool(val, "PrintOnAllPages", PrintOnAllPages, false, false);
		JsonExtensions.AddPropertyBool(val, "PrintIfDetailEmpty", PrintIfDetailEmpty, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataSourceName", DataSourceName);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "BusinessObjectGuid", BusinessObjectGuid);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataRelationName", DataRelationName);
		JsonExtensions.AddPropertyEnum(val, "FilterMode", (Enum)FilterMode, (Enum)StiFilterMode.And);
		JsonExtensions.AddPropertyEnum(val, "FilterEngine", (Enum)FilterEngine, (Enum)StiFilterEngine.ReportEngine);
		JsonExtensions.AddPropertyBool(val, "FilterOn", FilterOn, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "EvenStyle", EvenStyle);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "OddStyle", OddStyle);
		JsonExtensions.AddPropertyJObject(val, "BeginRenderEvent", BeginRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "RenderingEvent", RenderingEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "EndRenderEvent", EndRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetCollapsedEvent", GetCollapsedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "RightToLeft", RightToLeft, false, false);
		JsonExtensions.AddPropertyDouble(val, "ColumnGaps", ColumnGaps, 0.0);
		JsonExtensions.AddPropertyDouble(val, "ColumnWidth", ColumnWidth, 0.0);
		JsonExtensions.AddPropertyInt(val, "Columns", Columns, 0);
		JsonExtensions.AddPropertyInt(val, "MinRowsInColumn", MinRowsInColumn, 0);
		JsonExtensions.AddPropertyEnum(val, "ColumnDirection", (Enum)ColumnDirection, (Enum)StiColumnDirection.AcrossThenDown);
		JsonExtensions.AddPropertyBool(val, "ResetDataSource", ResetDataSource, false, false);
		JsonExtensions.AddPropertyBool(val, "CalcInvisible", CalcInvisible, false, false);
		JsonExtensions.AddPropertyInt(val, "CountData", CountData, 0);
		JsonExtensions.AddPropertyBool(val, "KeepDetailsTogether", KeepDetailsTogether, false, false);
		if (MasterComponent != null)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "MasterComponent", MasterComponent.Name);
		}
		if ((int)mode == 0)
		{
			JsonExtensions.AddPropertyStringArray(val, "Sort", Sort);
			JsonExtensions.AddPropertyJObject(val, "Filters", Filters.SaveToJsonObject(mode));
			JsonExtensions.AddPropertyJObject(val, "Collapsed", Collapsed.SaveToJsonObject(mode));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Expected O, but got Unknown
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "StartNewPage":
				StartNewPage = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartNewPageIfLessThan":
				StartNewPageIfLessThan = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "KeepGroupTogether":
				KeepGroupTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "KeepHeaderTogether":
				KeepHeaderTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "KeepFooterTogether":
				KeepFooterTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "KeepChildTogether":
				KeepChildTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "KeepDetails":
				keepDetails = StiJsonReportObjectHelper.DeserializeEnum<StiKeepDetails>(item);
				break;
			case "Sort":
				Sort = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "PrintOnAllPages":
				PrintOnAllPages = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "PrintIfDetailEmpty":
				PrintIfDetailEmpty = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DataSourceName":
				dataSourceName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "BusinessObjectGuid":
				businessObjectGuid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DataRelationName":
				DataRelationName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "FilterMode":
				FilterMode = StiJsonReportObjectHelper.DeserializeEnum<StiFilterMode>(item);
				break;
			case "FilterEngine":
				FilterEngine = StiJsonReportObjectHelper.DeserializeEnum<StiFilterEngine>(item);
				break;
			case "Filters":
				if (Filters == null)
				{
					Filters = new StiFiltersCollection();
				}
				Filters.LoadFromJsonObject((JObject)item.Value);
				break;
			case "FilterOn":
				FilterOn = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "EvenStyle":
				EvenStyle = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "OddStyle":
				OddStyle = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "BeginRenderEvent":
			{
				StiBeginRenderEvent stiBeginRenderEvent = new StiBeginRenderEvent();
				stiBeginRenderEvent.LoadFromJsonObject((JObject)item.Value);
				BeginRenderEvent = stiBeginRenderEvent;
				break;
			}
			case "RenderingEvent":
			{
				StiRenderingEvent stiRenderingEvent = new StiRenderingEvent();
				stiRenderingEvent.LoadFromJsonObject((JObject)item.Value);
				RenderingEvent = stiRenderingEvent;
				break;
			}
			case "EndRenderEvent":
			{
				StiEndRenderEvent stiEndRenderEvent = new StiEndRenderEvent();
				stiEndRenderEvent.LoadFromJsonObject((JObject)item.Value);
				EndRenderEvent = stiEndRenderEvent;
				break;
			}
			case "GetCollapsedEvent":
			{
				StiGetCollapsedEvent stiGetCollapsedEvent = new StiGetCollapsedEvent();
				stiGetCollapsedEvent.LoadFromJsonObject((JObject)item.Value);
				GetCollapsedEvent = stiGetCollapsedEvent;
				break;
			}
			case "Collapsed":
			{
				StiCollapsedExpression stiCollapsedExpression = new StiCollapsedExpression();
				stiCollapsedExpression.LoadFromJsonObject((JObject)item.Value);
				Collapsed = stiCollapsedExpression;
				break;
			}
			case "RightToLeft":
				rightToLeft = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ColumnGaps":
				columnGaps = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "ColumnWidth":
				columnWidth = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Columns":
				columns = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MinRowsInColumn":
				minRowsInColumn = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "ColumnDirection":
				columnDirection = StiJsonReportObjectHelper.DeserializeEnum<StiColumnDirection>(item);
				break;
			case "ResetDataSource":
				ResetDataSource = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "CalcInvisible":
				CalcInvisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "CountData":
				countData = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MasterComponent":
				jsonMasterComponentTemp = StiJsonReportObjectHelper.DeserializeString(item);
				Report.jsonLoaderHelper.MasterComponents.Add(this);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.DataBandEditor() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[4]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[5]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter(),
				propertiesHelper.SkipFirst()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[7]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter(),
				propertiesHelper.BreakIfLessThan(),
				propertiesHelper.SkipFirst(),
				propertiesHelper.LimitRows()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Columns, new StiPropertyObject[5]
			{
				propertiesHelper.Columns(),
				propertiesHelper.ColumnWidth(),
				propertiesHelper.ColumnGaps(),
				propertiesHelper.ColumnDirection(),
				propertiesHelper.RightToLeft()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Columns, new StiPropertyObject[6]
			{
				propertiesHelper.Columns(),
				propertiesHelper.ColumnWidth(),
				propertiesHelper.ColumnGaps(),
				propertiesHelper.ColumnDirection(),
				propertiesHelper.MinRowsInColumn(),
				propertiesHelper.RightToLeft()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[1] { propertiesHelper.Height() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[3]
			{
				propertiesHelper.Height(),
				propertiesHelper.MaxHeight(),
				propertiesHelper.MinHeight()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.OddStyle(),
				propertiesHelper.EvenStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[7]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.OddStyle(),
				propertiesHelper.EvenStyle(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[4]
			{
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[12]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.CalcInvisible(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.KeepDetailsTogether(),
				propertiesHelper.PrintAtBottom(),
				propertiesHelper.PrintIfDetailEmpty(),
				propertiesHelper.PrintOn(),
				propertiesHelper.PrintOnAllPages(),
				propertiesHelper.ResetPageNumber()
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
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[5]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
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
		stiEventCollection.Add(StiPropertyCategories.RenderEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.BeginRenderEvent,
			StiPropertyEventId.EndRenderEvent,
			StiPropertyEventId.RenderingEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.GetCollapsedEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override void SaveState(string stateName)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 2)
		{
			base.States.PushInt(stateName, this, "positionValue", positionValue);
			base.States.PushBool(stateName, this, "isEofValue", isEofValue);
			base.States.PushBool(stateName, this, "isBofValue", isBofValue);
			if (DataSource != null)
			{
				DataSource.SaveState(stateName);
			}
			if (!string.IsNullOrEmpty(BusinessObjectGuid))
			{
				BusinessObject.SaveState(stateName);
			}
			return;
		}
		base.SaveState(stateName);
		base.States.PushInt(stateName, this, "startLine", DataBandInfoV1.StartLine);
		base.States.PushInt(stateName, this, "positionValue", positionValue);
		base.States.PushBool(stateName, this, "isEofValue", isEofValue);
		base.States.PushBool(stateName, this, "isBofValue", isBofValue);
		base.States.PushInt(stateName, this, "runtimeLine", DataBandInfoV1.RuntimeLine);
		base.States.PushDouble(stateName, this, "freeSpace", DataBandInfoV1.FreeSpace);
		base.States.PushInt(stateName, this, "lineThrough", LineThrough);
		base.States.PushBool(stateName, this, "firstCall", DataBandInfoV1.FirstCall);
		base.States.PushBool(stateName, this, "firstGroupOnPass", DataBandInfoV1.FirstGroupOnPass);
		base.States.Push(stateName, this, "CurrentDetailComponent", DataBandInfoV1.CurrentDetailComponent);
		base.States.PushInt(stateName, this, "lastPositionRendering", DataBandInfoV1.LastPositionRendering);
		if (DataSource != null)
		{
			DataSource.SaveState(stateName);
		}
		if (!string.IsNullOrEmpty(BusinessObjectGuid))
		{
			BusinessObject.SaveState(stateName);
		}
	}

	public override void RestoreState(string stateName)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 2)
		{
			isBofValue = base.States.PopBool(stateName, this, "isBofValue");
			isEofValue = base.States.PopBool(stateName, this, "isEofValue");
			positionValue = base.States.PopInt(stateName, this, "positionValue");
			if (DataSource != null)
			{
				DataSource.RestoreState(stateName);
			}
			if (!string.IsNullOrEmpty(BusinessObjectGuid))
			{
				BusinessObject.RestoreState(stateName);
			}
			StiDataBandV2Builder.PrepareGroupResults(this);
			return;
		}
		base.RestoreState(stateName);
		if (base.States.IsExist(stateName, this))
		{
			DataBandInfoV1.FirstCall = base.States.PopBool(stateName, this, "firstCall");
			LineThrough = base.States.PopInt(stateName, this, "lineThrough");
			DataBandInfoV1.RuntimeLine = base.States.PopInt(stateName, this, "runtimeLine");
			isBofValue = base.States.PopBool(stateName, this, "isBofValue");
			isEofValue = base.States.PopBool(stateName, this, "isEofValue");
			positionValue = base.States.PopInt(stateName, this, "positionValue");
			DataBandInfoV1.StartLine = base.States.PopInt(stateName, this, "startLine");
			DataBandInfoV1.FreeSpace = base.States.PopDouble(stateName, this, "freeSpace");
			DataBandInfoV1.FirstGroupOnPass = base.States.PopBool(stateName, this, "firstGroupOnPass");
			DataBandInfoV1.LastPositionRendering = base.States.PopInt(stateName, this, "lastPositionRendering");
		}
		if (DataSource != null)
		{
			DataSource.RestoreState(stateName);
		}
		if (!string.IsNullOrEmpty(BusinessObjectGuid))
		{
			BusinessObject.RestoreState(stateName);
		}
		if (Page.PageInfoV1.RestoreCurrentDetailComponent && base.States.IsExist(stateName, this))
		{
			DataBandInfoV1.CurrentDetailComponent = base.States.Pop(stateName, this, "currentDetailComponent") as StiComponent;
		}
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		base.Convert(oldUnit, newUnit, isReportSnapshot);
		ColumnGaps = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(ColumnGaps));
		ColumnWidth = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(ColumnWidth));
	}

	private bool ShouldSerializeSort()
	{
		if (Sort != null)
		{
			return Sort.Length != 0;
		}
		return true;
	}

	public override object Clone(bool cloneProperties, bool cloneComponents)
	{
		StiDataBand stiDataBand = (StiDataBand)base.Clone(cloneProperties, cloneComponents);
		stiDataBand.dataBandInfoV1 = DataBandInfoV1.Clone() as StiDataBandInfoV1;
		stiDataBand.dataBandInfoV2 = DataBandInfoV2.Clone() as StiDataBandInfoV2;
		stiDataBand.MasterComponent = null;
		if (Sort != null)
		{
			stiDataBand.Sort = (string[])Sort.Clone();
		}
		else
		{
			stiDataBand.Sort = null;
		}
		if (Filters != null)
		{
			stiDataBand.Filters = Filters.Clone() as StiFiltersCollection;
		}
		else
		{
			stiDataBand.Filters = null;
		}
		return stiDataBand;
	}

	public virtual void First()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		if (!IsBusinessObjectEmpty)
		{
			BusinessObject.First();
		}
		else if (!IsDataSourceEmpty)
		{
			DataSource.First();
		}
		else
		{
			isEofValue = false;
			isBofValue = true;
			positionValue = 0;
		}
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			StiDataBandV1Builder.SetDetails(this);
		}
		else
		{
			StiDataBandV2Builder.SetDetails(this);
		}
	}

	public virtual void Prior()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		if (!IsBusinessObjectEmpty)
		{
			BusinessObject.Prior();
		}
		else if (!IsDataSourceEmpty)
		{
			DataSource.Prior();
		}
		else
		{
			isBofValue = false;
			isEofValue = false;
			if (positionValue <= 0)
			{
				isBofValue = true;
			}
			else
			{
				positionValue--;
			}
		}
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			StiDataBandV1Builder.SetDetails(this);
		}
		else
		{
			StiDataBandV2Builder.SetDetails(this);
		}
	}

	public virtual void Next()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		if (!IsBusinessObjectEmpty)
		{
			BusinessObject.Next();
		}
		else if (!IsDataSourceEmpty)
		{
			DataSource.Next();
		}
		else
		{
			isBofValue = false;
			isEofValue = false;
			if (positionValue >= countData - 1)
			{
				isEofValue = true;
			}
			else
			{
				positionValue++;
			}
		}
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			StiDataBandV1Builder.SetDetails(this);
		}
		else
		{
			StiDataBandV2Builder.SetDetails(this);
		}
	}

	public virtual void Last()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		if (!IsBusinessObjectEmpty)
		{
			BusinessObject.Last();
		}
		else if (!IsDataSourceEmpty)
		{
			DataSource.Last();
		}
		else
		{
			isEofValue = true;
			isBofValue = false;
			positionValue = countData - 1;
		}
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			StiDataBandV1Builder.SetDetails(this);
		}
		else
		{
			StiDataBandV2Builder.SetDetails(this);
		}
	}

	private bool ShouldSerializeFilters()
	{
		if (Filters != null)
		{
			return Filters.Count > 0;
		}
		return true;
	}

	public override void PackService()
	{
		base.PackService();
		Filters = null;
	}

	public override string GetHeaderText()
	{
		StiBusinessObject businessObject = BusinessObject;
		if (businessObject != null)
		{
			return ((object)this).ToString() + "; " + Loc.Get("PropertyMain", "BusinessObject") + ": " + businessObject.GetFullName();
		}
		string text = ((object)this).ToString() + "; " + Loc.Get("PropertyMain", "DataSource") + ": ";
		if (DataSource != null)
		{
			return $"{text}{DataSource}";
		}
		if (CountData > 0)
		{
			return $"{text}{CountData}";
		}
		return text + Loc.Get("Report", "NotAssigned");
	}

	public override bool DoBookmark()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 2)
		{
			return base.DoBookmark();
		}
		return false;
	}

	public override void DoPointer(bool createNewGuid = true)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 2)
		{
			base.DoPointer(createNewGuid);
		}
	}

	public void InvokeGroupRendering()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		if (Report == null)
		{
			return;
		}
		if ((int)Report.EngineVersion == 1)
		{
			StiComponentsCollection groupHeaderComponents = DataBandInfoV1.GroupHeaderComponents;
			if (this is StiCrossDataBand && ((StiCrossDataBand)this).ColumnMode)
			{
				groupHeaderComponents = ((StiDataBand)Parent).DataBandInfoV1.GroupHeaderComponents;
			}
			if (groupHeaderComponents == null)
			{
				return;
			}
			{
				foreach (StiGroupHeaderBand item in groupHeaderComponents)
				{
					item.InvokeRendering(Position);
				}
				return;
			}
		}
		if (DataBandInfoV2.GroupHeaders == null)
		{
			return;
		}
		foreach (StiGroupHeaderBand groupHeader in DataBandInfoV2.GroupHeaders)
		{
			groupHeader.InvokeRendering();
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public override StiComponent Render()
	{
		return null;
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public virtual void RenderMaster()
	{
		StiDataBandV2Builder stiDataBandV2Builder = StiV2Builder.GetBuilder(((object)this).GetType()) as StiDataBandV2Builder;
		stiDataBandV2Builder.RenderMaster(this);
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (BeginRenderEvent != null)
		{
			stiEventsCollection.Add(BeginRenderEvent);
		}
		if (RenderingEvent != null)
		{
			stiEventsCollection.Add(RenderingEvent);
		}
		if (EndRenderEvent != null)
		{
			stiEventsCollection.Add(EndRenderEvent);
		}
		if (GetCollapsedEvent != null)
		{
			stiEventsCollection.Add(GetCollapsedEvent);
		}
		return stiEventsCollection;
	}

	protected virtual void OnBeginRender(EventArgs e)
	{
	}

	public void InvokeBeginRender()
	{
		try
		{
			DataBandInfoV1.LastPositionRendering = -1;
			DataBandInfoV1.RuntimeLine = 0;
			OnBeginRender(EventArgs.Empty);
			if (base.Events[EventBeginRender] is EventHandler eventHandler)
			{
				eventHandler(this, EventArgs.Empty);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, BeginRenderEvent);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), Name + " InvokeBeginRender...ERROR");
			StiLogService.Write(((object)this).GetType(), Name + " " + ex.Message);
			Report?.WriteToReportRenderingMessages(Name + ".BeginRender event error: " + ex.Message);
		}
	}

	protected virtual void OnRendering(EventArgs e)
	{
	}

	public void InvokeRendering()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			if (Report != null && (int)Report.EngineVersion == 2)
			{
				OnRendering(EventArgs.Empty);
				if (base.Events[EventRendering] is EventHandler eventHandler)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}
			else if (DataBandInfoV1.LastPositionRendering != Position)
			{
				if (ResetPageNumber)
				{
					Report.EngineV1.ProcessResetPageNumber(Report);
				}
				DataBandInfoV1.RuntimeLine++;
				LineThrough++;
				DataBandInfoV1.LastPositionRendering = Position;
				OnRendering(EventArgs.Empty);
				if (base.Events[EventRendering] is EventHandler eventHandler2)
				{
					eventHandler2(this, EventArgs.Empty);
				}
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, RenderingEvent);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), Name + " InvokeRendering...ERROR");
			StiLogService.Write(((object)this).GetType(), Name + " " + ex.Message);
			Report?.WriteToReportRenderingMessages("Error in the 'Rendering' event of '" + Name + "' component: " + ex.Message);
		}
	}

	protected virtual void OnEndRender(EventArgs e)
	{
	}

	public void InvokeEndRender()
	{
		try
		{
			OnEndRender(EventArgs.Empty);
			if (base.Events[EventEndRender] is EventHandler eventHandler)
			{
				eventHandler(this, EventArgs.Empty);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, EndRenderEvent);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), Name + " InvokeEndRender...ERROR");
			StiLogService.Write(((object)this).GetType(), Name + " " + ex.Message);
			Report?.WriteToReportRenderingMessages("Error in the 'EndRender' event of '" + Name + "' component: " + ex.Message);
		}
	}

	protected virtual void OnGetCollapsed(StiValueEventArgs e)
	{
	}

	public void InvokeGetCollapsed(StiValueEventArgs e)
	{
		try
		{
			OnGetCollapsed(e);
			if (Report != null && Report.CalculationMode == StiCalculationMode.Interpretation && Collapsed.Value.Length > 0)
			{
				e.Value = StiParser.ParseTextValue(Collapsed.Value, this);
			}
			if (base.Events[EventGetCollapsed] is StiValueEventHandler stiValueEventHandler)
			{
				stiValueEventHandler(this, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, GetCollapsedEvent, e);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoGetCollapsed...ERROR");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
		}
	}

	public double GetColumnWidth()
	{
		double num = ColumnWidth;
		if (num != 0.0)
		{
			return num;
		}
		if (Columns == 0)
		{
			return Width;
		}
		return Width / (double)Columns - ColumnGaps;
	}

	public override StiComponent CreateNew()
	{
		return new StiDataBand();
	}

	public StiDataBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiDataBand(RectangleD rect)
		: base(rect)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		PrintIfDetailEmpty = StiOptions.Engine.PrintIfDetailEmptyDefaultValue;
	}
}
