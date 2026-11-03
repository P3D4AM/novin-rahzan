using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.CrossTab;

[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiCrossTab.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.catCrossBands.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Design.CrossTab.StiCrossTabDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfCrossTabDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiCrossTabGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiCrossTabWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiCrossTabV1Builder))]
[StiV2Builder(typeof(StiCrossTabV2Builder))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiDataSource))]
[StiContextTool(typeof(IStiMasterComponent))]
[StiContextTool(typeof(IStiDataRelation))]
[StiContextTool(typeof(IStiSort))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiPrintIfEmpty))]
[StiEngine(/*Could not decode attribute arguments.*/)]
public class StiCrossTab : StiContainer, IStiDataSource, IStiEnumerator, IStiFilter, IStiCrossTab, IStiSort, IStiDataRelation, IStiPrintIfEmpty, IStiBusinessObject
{
	private string dataSourceName = string.Empty;

	private string businessObjectGuid = string.Empty;

	private int crossTabStyleIndex;

	private bool rightToLeft;

	public override StiComponentId ComponentId => StiComponentId.StiCrossTab;

	public override string HelpUrl => "user-manual/index.html?report_internals_crosstable.htm";

	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the cross-tab is printed if data is not present.")]
	public virtual bool PrintIfEmpty { get; set; } = true;


	[StiNonSerialized]
	[Browsable(false)]
	public override StiShiftMode ShiftMode
	{
		get
		{
			return base.ShiftMode;
		}
		set
		{
			base.ShiftMode = value;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that this object can shrink.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
			base.CanShrink = value;
		}
	}

	[DefaultValue(true)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that this object can grow.")]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
			base.CanGrow = value;
		}
	}

	public override bool CanBreak
	{
		get
		{
			if (StiSubReportsHelper.GetParentBand(this) == null)
			{
				return base.CanBreak;
			}
			return true;
		}
		set
		{
			base.CanBreak = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiAnchorMode Anchor
	{
		get
		{
			return base.Anchor;
		}
		set
		{
			base.Anchor = value;
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
	public string DataRelationName { get; set; } = "";


	[TypeConverter(typeof(StiDataSourceConverter))]
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
	public string DataSourceName
	{
		get
		{
			return dataSourceName;
		}
		set
		{
			if (dataSourceName != value)
			{
				dataSourceName = value;
				StiOptions.Engine.GlobalEvents.InvokeDataSourceAssigned(this, EventArgs.Empty);
			}
		}
	}

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
			if (businessObjectGuid != value)
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

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiSortConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiSortEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[Description("Gets or sets the array of strings that describes rules of sorting.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public virtual string[] Sort { get; set; } = new string[0];


	[Browsable(false)]
	public bool IsEof
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.IsEof;
			}
			if (BusinessObject != null)
			{
				return BusinessObject.IsEof;
			}
			return true;
		}
		set
		{
			if (DataSource != null)
			{
				DataSource.IsEof = value;
			}
			else if (BusinessObject != null)
			{
				BusinessObject.IsEof = value;
			}
		}
	}

	[Browsable(false)]
	public bool IsBof
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.IsBof;
			}
			if (BusinessObject != null)
			{
				return BusinessObject.IsBof;
			}
			return true;
		}
		set
		{
			if (DataSource != null)
			{
				DataSource.IsBof = value;
			}
			else if (BusinessObject != null)
			{
				BusinessObject.IsBof = value;
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.IsEmpty;
			}
			if (BusinessObject != null)
			{
				return BusinessObject.IsEmpty;
			}
			return true;
		}
	}

	[Browsable(false)]
	public virtual int Position
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.Position;
			}
			if (BusinessObject != null)
			{
				return BusinessObject.Position;
			}
			return 0;
		}
		set
		{
			if (DataSource != null)
			{
				DataSource.Position = value;
			}
			else if (BusinessObject != null)
			{
				BusinessObject.Position = value;
			}
		}
	}

	[Browsable(false)]
	public virtual int Count
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.Count;
			}
			if (BusinessObject != null)
			{
				return BusinessObject.Count;
			}
			return 0;
		}
	}

	[Browsable(false)]
	public StiCrossTabInfo CrossTabInfo { get; private set; } = new StiCrossTabInfo();


	[Browsable(false)]
	public StiCrossTabInfoV1 CrossTabInfoV1 { get; private set; } = new StiCrossTabInfoV1();


	[DefaultValue(StiFilterEngine.ReportEngine)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[Description("Gets or sets how a filter will be applied to data - be means of the report generator or by means of changing the SQL query.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFilterEngine FilterEngine { get; set; }

	[DefaultValue(StiFilterMode.And)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(190)]
	[Description("Gets or sets filter mode.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFilterMode FilterMode { get; set; }

	[Browsable(false)]
	public StiFilterEventHandler FilterMethodHandler { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiFiltersCollectionConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiFiltersCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the collection of data filters.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	public StiFiltersCollection Filters { get; set; } = new StiFiltersCollection();


	[Browsable(false)]
	[StiNonSerialized]
	[Obsolete("Filter property is obsolete. Please use Filters property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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


	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossTab");

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override int ToolboxPosition => 202;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Cross;

	public override int Priority => 1500;

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 300.0, 100.0);

	public override StiComponentType ComponentType
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Report != null && (int)Report.EngineVersion == 2)
			{
				return StiComponentType.Simple;
			}
			if (Parent is StiBand)
			{
				return StiComponentType.Simple;
			}
			return StiComponentType.Master;
		}
	}

	[DefaultValue(0)]
	[Browsable(false)]
	[StiSerializable]
	public int CrossTabStyleIndex
	{
		get
		{
			return crossTabStyleIndex;
		}
		set
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			if (value == crossTabStyleIndex)
			{
				return;
			}
			if (value != -1)
			{
				base.ComponentStyle = "";
				CrossTabStyleColor = null;
			}
			crossTabStyleIndex = ((value >= StiOptions.Designer.CrossTab.StyleColors.Length) ? (StiOptions.Designer.CrossTab.StyleColors.Length - 1) : value);
			if (!base.IsDesigning || Report == null || Report.IsSerializing)
			{
				return;
			}
			foreach (StiCrossField component in Components)
			{
				component.ComponentStyle = "CrossTab";
				component.Font = new Font("Arial", 8f);
			}
			UpdateStyles();
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public object CrossTabStyleColor { get; set; }

	[DefaultValue("")]
	[Browsable(false)]
	public string CrossTabStyle
	{
		get
		{
			return ComponentStyle;
		}
		set
		{
			ComponentStyle = value;
		}
	}

	[Editor("Stimulsoft.Report.Design.StiCrossTabStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public override string ComponentStyle
	{
		get
		{
			return base.ComponentStyle;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (base.ComponentStyle == value)
			{
				return;
			}
			base.ComponentStyle = value;
			CrossTabStyleColor = null;
			crossTabStyleIndex = -1;
			UpdateStyles();
			foreach (StiCrossField component in Components)
			{
				component.ComponentStyle = ComponentStyle;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiCrossHorAlignment.None)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the horizontal alignment of an Cross-Tab.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	public StiCrossHorAlignment HorAlignment { get; set; } = StiCrossHorAlignment.None;


	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiOrder(170)]
	[StiSerializable]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public bool PrintTitleOnAllPages { get; set; } = true;


	[DefaultValue(StiSummaryDirection.UpToDown)]
	[StiSerializable]
	[Browsable(false)]
	public StiSummaryDirection SummaryDirection { get; set; } = StiSummaryDirection.UpToDown;


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(220)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that the CrossTab must to be kept together with DataBand on what it is placed.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	public virtual bool KeepCrossTabTogether { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets string value which is used to show the cross-tab in empty cells.")]
	public string EmptyValue { get; set; } = string.Empty;


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that the cross-tab is to be output in one column. At the same time, everything that do not fit by the width is output below. For using this property it is necessary to enable the CanBreak property of the band on which the cross-tab is placed (if it is placed on the band).")]
	public bool Wrap { get; set; }

	[DefaultValue(0.0)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets space between two parts of a wrapped cross-tab. The property is used jointly with the property Wrap.")]
	public double WrapGap { get; set; }

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets horizontal CrossTab direction.")]
	[StiOrder(130)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
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

	[StiNonSerialized]
	[Browsable(false)]
	public override bool Printable
	{
		get
		{
			return base.Printable;
		}
		set
		{
			base.Printable = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool GrowToHeight
	{
		get
		{
			return base.GrowToHeight;
		}
		set
		{
			base.GrowToHeight = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "CanGrow", CanGrow, true, false);
		JsonExtensions.AddPropertyBool(val, "PrintIfEmpty", PrintIfEmpty, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataRelationName", DataRelationName);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataSourceName", DataSourceName);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "BusinessObjectGuid", BusinessObjectGuid);
		JsonExtensions.AddPropertyStringArray(val, "Sort", Sort);
		JsonExtensions.AddPropertyEnum(val, "FilterEngine", (Enum)FilterEngine, (Enum)StiFilterEngine.ReportEngine);
		JsonExtensions.AddPropertyEnum(val, "FilterMode", (Enum)FilterMode, (Enum)StiFilterMode.And);
		JsonExtensions.AddPropertyJObject(val, "Filters", Filters.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "FilterOn", FilterOn, true, false);
		JsonExtensions.AddPropertyInt(val, "CrossTabStyleIndex", CrossTabStyleIndex, 0);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)HorAlignment, (Enum)StiCrossHorAlignment.None);
		JsonExtensions.AddPropertyEnum(val, "SummaryDirection", (Enum)SummaryDirection, (Enum)StiSummaryDirection.UpToDown);
		JsonExtensions.AddPropertyBool(val, "KeepCrossTabTogether", KeepCrossTabTogether, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "EmptyValue", EmptyValue);
		JsonExtensions.AddPropertyBool(val, "Wrap", Wrap, false, false);
		JsonExtensions.AddPropertyDouble(val, "WrapGap", WrapGap);
		JsonExtensions.AddPropertyBool(val, "RightToLeft", RightToLeft, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "PrintIfEmpty":
				PrintIfEmpty = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DataRelationName":
				DataRelationName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DataSourceName":
				dataSourceName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "BusinessObjectGuid":
				businessObjectGuid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Sort":
				Sort = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "FilterEngine":
				FilterEngine = StiJsonReportObjectHelper.DeserializeEnum<StiFilterEngine>(item);
				break;
			case "FilterMode":
				FilterMode = StiJsonReportObjectHelper.DeserializeEnum<StiFilterMode>(item);
				break;
			case "Filters":
				Filters.LoadFromJsonObject((JObject)item.Value);
				break;
			case "FilterOn":
				FilterOn = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "CrossTabStyleIndex":
				crossTabStyleIndex = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "HorAlignment":
				HorAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiCrossHorAlignment>(item);
				break;
			case "SummaryDirection":
				SummaryDirection = StiJsonReportObjectHelper.DeserializeEnum<StiSummaryDirection>(item);
				break;
			case "KeepCrossTabTogether":
				KeepCrossTabTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "EmptyValue":
				EmptyValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Wrap":
				Wrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "WrapGap":
				WrapGap = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "RightToLeft":
				RightToLeft = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Invalid comparison between Unknown and I4
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.CrossTabEditor() });
		if (Wrap)
		{
			stiPropertyCollection.Add(StiPropertyCategories.CrossTab, new StiPropertyObject[6]
			{
				propertiesHelper.EmptyValue(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.PrintIfEmpty(),
				propertiesHelper.RightToLeft(),
				propertiesHelper.Wrap(),
				propertiesHelper.WrapGap()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.CrossTab, new StiPropertyObject[5]
			{
				propertiesHelper.EmptyValue(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.PrintIfEmpty(),
				propertiesHelper.RightToLeft(),
				propertiesHelper.Wrap()
			});
		}
		if ((int)level == 2)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Data, new StiPropertyObject[7]
			{
				propertiesHelper.DataSource(),
				propertiesHelper.DataRelation(),
				propertiesHelper.BusinessObject(),
				propertiesHelper.FilterOn(),
				propertiesHelper.Filters(),
				propertiesHelper.FilterEngine(),
				propertiesHelper.FilterMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Data, new StiPropertyObject[1] { propertiesHelper.DataSource() });
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
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[5]
			{
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.CanBreak(),
				propertiesHelper.KeepCrossTabTogether()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[10]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.KeepCrossTabTogether(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[11]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.KeepCrossTabTogether(),
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
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		base.Convert(oldUnit, newUnit, isReportSnapshot);
		WrapGap = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(WrapGap));
	}

	private bool ShouldSerializeSort()
	{
		if (Sort != null)
		{
			return Sort.Length != 0;
		}
		return true;
	}

	public void First()
	{
		if (DataSource != null)
		{
			DataSource.First();
		}
		else if (BusinessObject != null)
		{
			BusinessObject.First();
		}
	}

	public void Prior()
	{
		if (DataSource != null)
		{
			DataSource.Prior();
		}
		else if (BusinessObject != null)
		{
			BusinessObject.Prior();
		}
	}

	public void Next()
	{
		if (DataSource != null)
		{
			DataSource.Next();
		}
		else if (BusinessObject != null)
		{
			BusinessObject.Next();
		}
	}

	public void Last()
	{
		if (DataSource != null)
		{
			DataSource.Last();
		}
		else if (BusinessObject != null)
		{
			BusinessObject.Last();
		}
	}

	public override object Clone()
	{
		StiCrossTab stiCrossTab = (StiCrossTab)base.Clone();
		stiCrossTab.CrossTabInfo = CrossTabInfo.Clone() as StiCrossTabInfo;
		stiCrossTab.CrossTabInfoV1 = CrossTabInfoV1.Clone() as StiCrossTabInfoV1;
		stiCrossTab.Filters = ((Filters != null) ? (Filters.Clone() as StiFiltersCollection) : null);
		return stiCrossTab;
	}

	public override void PackService()
	{
		base.PackService();
		Filters = null;
	}

	private bool ShouldSerializeFilters()
	{
		if (Filters != null)
		{
			return Filters.Count > 0;
		}
		return true;
	}

	public override bool CanContainIn(StiComponent component)
	{
		if (component is StiCrossTab)
		{
			return false;
		}
		return component is StiContainer;
	}

	public void UpdateStyles()
	{
		foreach (StiCrossField component in Components)
		{
			ApplyFieldStyle(component);
		}
	}

	public Color GetCellColor()
	{
		if (CrossTabStyleColor is Color)
		{
			return (Color)CrossTabStyleColor;
		}
		if (!string.IsNullOrEmpty(CrossTabStyle) && Report != null && Report.Styles[CrossTabStyle] is StiCrossTabStyle stiCrossTabStyle)
		{
			return stiCrossTabStyle.Color;
		}
		if (CrossTabStyleIndex == -1)
		{
			return Color.White;
		}
		return StiOptions.Designer.CrossTab.StyleColors[CrossTabStyleIndex];
	}

	public void ApplyFieldStyle(StiCrossField field)
	{
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		if (field == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(CrossTabStyle) && Report != null && Report.Styles[CrossTabStyle] is StiCrossTabStyle)
		{
			if (field.ComponentStyle != "")
			{
				StiCrossTabStyle stiCrossTabStyle = Report.Styles[CrossTabStyle] as StiCrossTabStyle;
				if (field is StiCrossColumn || field is StiCrossTitle || field is StiCrossColumnTotal || (field is StiCrossSummaryHeader && SummaryDirection == StiSummaryDirection.LeftToRight))
				{
					field.TextBrush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.ColumnHeaderForeColor);
					field.Brush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.ColumnHeaderBackColor);
				}
				else if (field is StiCrossRow || field is StiCrossRowTotal || (field is StiCrossSummaryHeader && SummaryDirection == StiSummaryDirection.UpToDown))
				{
					field.TextBrush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.RowHeaderForeColor);
					field.Brush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.RowHeaderBackColor);
				}
				else
				{
					field.TextBrush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.CellForeColor);
					field.Brush = (StiBrush)new StiSolidBrush(stiCrossTabStyle.CellBackColor);
				}
			}
			return;
		}
		if (field.ComponentStyle != "CrossTab" && !string.IsNullOrEmpty(field.ComponentStyle) && Report != null && Report.Styles[field.ComponentStyle] != null)
		{
			Report.Styles[field.ComponentStyle].SetStyleToComponent(field);
			return;
		}
		field.Brush = (StiBrush)new StiSolidBrush(Color.White);
		field.Border.Color = StiColorUtils.Dark(GetCellColor(), (byte)100);
		field.TextBrush = (StiBrush)new StiSolidBrush(Color.Black);
		if (field is StiCrossTitle || field is StiCrossRow || field is StiCrossColumn || field is StiCrossSummaryHeader)
		{
			field.Brush = (StiBrush)new StiSolidBrush(GetCellColor());
			field.TextBrush = (StiBrush)new StiSolidBrush(StiColorUtils.Dark(GetCellColor(), (byte)150));
		}
		if (field is StiCrossTotal)
		{
			field.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(GetCellColor(), (byte)50));
		}
		if (field is StiCrossSummary)
		{
			field.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(GetCellColor(), (byte)100));
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossTab();
	}

	public StiCrossTab()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiCrossTab(RectangleD rect)
		: base(rect)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
		CanGrow = true;
	}
}
