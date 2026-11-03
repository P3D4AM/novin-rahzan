using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiChartTable : IStiChartTable, ICloneable, IStiJsonReportObject, IStiDefault, IStiSerializeToCodeAsClass, IStiPropertyGridObject
{
	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiChartTable;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (AllowApplyStyle && MarkerVisible && GridLinesHor && GridLinesVert && GridOutline && Format != null && Format.Length == 0 && !ShouldSerializeDataCells() && !ShouldSerializeHeader())
			{
				return !Visible;
			}
			return false;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	[Obsolete("Font property is obsolete. Please use DataCells.Font property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Font Font
	{
		get
		{
			return DataCells.Font;
		}
		set
		{
			DataCells.Font = value;
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of table.")]
	public bool Visible { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of markers.")]
	public bool MarkerVisible { get; set; } = true;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets grid lines color.")]
	public Color GridLineColor { get; set; } = Color.Gray;


	[Browsable(false)]
	[StiNonSerialized]
	[Obsolete("TextColor property is obsolete. Please use DataCells.TextColor property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Color TextColor
	{
		get
		{
			return DataCells.TextColor;
		}
		set
		{
			DataCells.TextColor = value;
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of grid lines horizontal.")]
	public bool GridLinesHor { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of grid lines vertical.")]
	public bool GridLinesVert { get; set; } = true;


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of grid outline.")]
	public bool GridOutline { get; set; } = true;


	[DefaultValue("")]
	[Description("Gets or sets format string which used for formating of the chart table.")]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Chart.Design.StiFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Format { get; set; } = "";


	[Description("Gets or sets header settings.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiChartTableHeader Header { get; set; }

	[Description("Gets or sets data cells settings.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiChartTableDataCells DataCells { get; set; }

	[Browsable(false)]
	public StiChartTableCore Core { get; set; }

	[Browsable(false)]
	public IStiChart Chart { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "MarkerVisible", MarkerVisible, true, false);
		JsonExtensions.AddPropertyColor(val, "GridLineColor", GridLineColor, Color.Gray);
		JsonExtensions.AddPropertyBool(val, "GridLinesHor", GridLinesHor, true, false);
		JsonExtensions.AddPropertyBool(val, "GridLinesVert", GridLinesVert, true, false);
		JsonExtensions.AddPropertyBool(val, "GridOutline", GridOutline, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
		JsonExtensions.AddPropertyJObject(val, "Header", ((IStiJsonReportObject)Header).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DataCells", ((IStiJsonReportObject)DataCells).SaveToJsonObject(mode));
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Font":
				DataCells.Font = StiJsonReportObjectHelper.DeserializeFont(item, DataCells.Font);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MarkerVisible":
				MarkerVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "GridLineColor":
				GridLineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TextColor":
				DataCells.TextColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "GridLinesHor":
				GridLinesHor = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "GridLinesVert":
				GridLinesVert = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "GridOutline":
				GridOutline = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Format":
				Format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Header":
				((IStiJsonReportObject)Header).LoadFromJsonObject((JObject)item.Value);
				break;
			case "DataCells":
				((IStiJsonReportObject)DataCells).LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.ChartTable() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		IStiChartTable stiChartTable = MemberwiseClone() as IStiChartTable;
		stiChartTable.Header = Header.Clone() as IStiChartTableHeader;
		stiChartTable.DataCells = DataCells.Clone() as IStiChartTableDataCells;
		if (Core != null)
		{
			stiChartTable.Core = Core.Clone() as StiChartTableCore;
			stiChartTable.Core.ChartTable = stiChartTable;
		}
		return stiChartTable;
	}

	private bool ShouldSerializeGridLineColor()
	{
		return GridLineColor != Color.Gray;
	}

	private bool ShouldSerializeHeader()
	{
		if (Header != null)
		{
			return !((IStiDefault)Header).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeDataCells()
	{
		if (DataCells != null)
		{
			return !(AllowApplyStyle ? DataCells.IsDefaultExceptTextColor : ((IStiDefault)DataCells).IsDefault);
		}
		return true;
	}

	[StiUniversalConstructor("Table")]
	public StiChartTable()
	{
		Header = new StiChartTableHeader();
		DataCells = new StiChartTableDataCells();
		Core = new StiChartTableCore(this);
	}
}
