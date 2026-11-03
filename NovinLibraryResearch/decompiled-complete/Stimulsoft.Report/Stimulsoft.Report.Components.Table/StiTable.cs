using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components.Table;

[StiServiceBitmap(typeof(StiTable), "Stimulsoft.Report.Images.Components.StiTable.png")]
[StiDesigner("Stimulsoft.Report.Components.Design.StiDataBandDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfDataBandDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
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
[StiEngine(/*Could not decode attribute arguments.*/)]
public class StiTable : StiDataBand, IStiTableComponent
{
	private int rowCount = 5;

	private int columnCount = 5;

	private int footerRowsCount;

	private int headerRowsCount;

	private StiTableStyleFX tableStyleFX;

	public override StiComponentId ComponentId => StiComponentId.StiTable;

	public override string HelpUrl => "user-manual/report_internals_table.htm";

	[Browsable(false)]
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

	public override string LocalizedName => StiLocalization.Get("Components", "StiTable");

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int ToolboxPosition => 201;

	[Browsable(false)]
	internal bool IsConverted { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[Description("Gets or sets a value which indicates that the table will be adjusted to the top of the parent component area.")]
	public bool DockableTable { get; set; } = true;


	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(typeof(StiTableAutoWidth), "None")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets which range use table component for adjusting columns width.")]
	public StiTableAutoWidth AutoWidth { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(typeof(StiTableAutoWidthType), "None")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Description("Gets or sets how to table component adjust columns width.")]
	public StiTableAutoWidthType AutoWidthType { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(5)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Get or sets a number of rows in the table. ")]
	public int RowCount
	{
		get
		{
			return rowCount;
		}
		set
		{
			if (value > 0)
			{
				int oldValue = rowCount;
				rowCount = value;
				ChangeRowCount(oldValue, value);
			}
		}
	}

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(5)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Get or sets a number of columns in the table.")]
	public int ColumnCount
	{
		get
		{
			return columnCount;
		}
		set
		{
			if (value > 0)
			{
				int oldValue = columnCount;
				columnCount = value;
				ChangeColumnCount(oldValue, value);
			}
		}
	}

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[Description("Get or sets a number of footer rows in the table.")]
	public int FooterRowsCount
	{
		get
		{
			return footerRowsCount;
		}
		set
		{
			if (!base.IsDesigning || value <= rowCount - headerRowsCount)
			{
				footerRowsCount = value;
				RefreshTableStyle();
			}
		}
	}

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[Description("Get or sets a number of header rows in the table.")]
	public int HeaderRowsCount
	{
		get
		{
			return headerRowsCount;
		}
		set
		{
			if (!base.IsDesigning || value <= rowCount - footerRowsCount)
			{
				headerRowsCount = value;
				RefreshTableStyle();
			}
		}
	}

	[Browsable(false)]
	public double DefaultHeightCell
	{
		get
		{
			if (Page == null || Page.Report == null)
			{
				return 1.0;
			}
			return Page.Report.ReportUnit switch
			{
				StiReportUnitType.Centimeters => 0.8, 
				StiReportUnitType.HundredthsOfInch => 30.0, 
				StiReportUnitType.Inches => 0.3, 
				StiReportUnitType.Millimeters => 8.0, 
				_ => 1.0, 
			};
		}
	}

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(StiPrintOnType.AllPages)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value which indicates how header of table will be print on pages.")]
	public StiPrintOnType HeaderPrintOn { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(110)]
	[Description("Gets or sets value indicates that header of table can grow its height.")]
	public bool HeaderCanGrow { get; set; } = true;


	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(120)]
	[Description("Gets or sets value which indicates that this header can shrink its height.")]
	public bool HeaderCanShrink { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(130)]
	[Description("Gets or sets value which indicates that this header can break its content.")]
	public bool HeaderCanBreak { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(140)]
	[Description("Gets or sets value indicates that the header of table will be print at bottom of page.")]
	public bool HeaderPrintAtBottom { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(150)]
	[Description("Gets or sets value indicates that the header will be print if data not present in table.")]
	public bool HeaderPrintIfEmpty { get; set; } = true;


	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(160)]
	[Description("Gets or sets value indicates that the header of table will be printed on all pages.")]
	public virtual bool HeaderPrintOnAllPages { get; set; } = true;


	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(StiPrintOnEvenOddPagesType.Ignore)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value indicates how the header of table will be printed on even-odd pages.")]
	public virtual StiPrintOnEvenOddPagesType HeaderPrintOnEvenOddPages { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(StiPrintOnType.AllPages)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value which indicates how footer of table will be print on pages.")]
	public StiPrintOnType FooterPrintOn { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(110)]
	[Description("Gets or sets value indicates that footer of table can grow its height.")]
	public bool FooterCanGrow { get; set; } = true;


	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(120)]
	[Description("Gets or sets value which indicates that this footer can shrink its height.")]
	public bool FooterCanShrink { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that this footer can break its content.")]
	public bool FooterCanBreak { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the footer of table will be print at bottom of page.")]
	public bool FooterPrintAtBottom { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the footer will be print if data not present in table.")]
	public bool FooterPrintIfEmpty { get; set; } = true;


	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the footer of table will be printed on all pages.")]
	public virtual bool FooterPrintOnAllPages { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(StiPrintOnEvenOddPagesType.Ignore)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value indicates how the footer of table will be printed on even-odd pages.")]
	public virtual StiPrintOnEvenOddPagesType FooterPrintOnEvenOddPages { get; set; }

	[Editor("Stimulsoft.Report.Design.StiTableStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public override string ComponentStyle
	{
		get
		{
			return base.ComponentStyle;
		}
		set
		{
			base.ComponentStyle = value;
			RefreshTableStyle();
		}
	}

	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override bool RightToLeft
	{
		get
		{
			return base.RightToLeft;
		}
		set
		{
			base.RightToLeft = value;
		}
	}

	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 300.0, 120.0);

	public override Color HeaderStartColor => Color.FromArgb(144, 192, 241);

	public override Color HeaderEndColor => Color.FromArgb(144, 192, 241);

	[StiSerializable]
	[DefaultValue(0)]
	[Browsable(false)]
	public int NumberID { get; set; }

	[Browsable(false)]
	[StiNonSerialized]
	public override int Columns => 0;

	[Browsable(false)]
	[StiNonSerialized]
	public override double ColumnWidth => 0.0;

	[Browsable(false)]
	[StiNonSerialized]
	public override double ColumnGaps => 0.0;

	[Browsable(false)]
	public override StiColumnDirection ColumnDirection => base.ColumnDirection;

	[Browsable(false)]
	[StiNonSerialized]
	public override int MinRowsInColumn => 0;

	[Browsable(false)]
	public override double MinHeight
	{
		get
		{
			return base.MinHeight;
		}
		set
		{
			base.MinHeight = value;
		}
	}

	[Browsable(false)]
	public override double MaxHeight
	{
		get
		{
			return base.MaxHeight;
		}
		set
		{
			base.MaxHeight = value;
		}
	}

	[Browsable(false)]
	public override SizeD MinSize
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return new SizeD(0.0, 0.0);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MinSize = value;
		}
	}

	[Browsable(false)]
	public override double Width
	{
		get
		{
			return base.Width;
		}
		set
		{
			double num = base.Width;
			if (value != base.Width)
			{
				base.Width = value;
				if (value > 0.0 && value != num)
				{
					ResizeWidthCell(num);
				}
			}
		}
	}

	[Browsable(false)]
	public override double Height
	{
		get
		{
			return base.Height;
		}
		set
		{
			base.Height = value;
			if (Components.Count == 0 && Page != null)
			{
				SelectAll();
				CreateCell();
			}
		}
	}

	[Browsable(false)]
	public override bool Dockable
	{
		get
		{
			return DockableTable;
		}
		set
		{
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiTableStyleConverter))]
	[Browsable(false)]
	[StiBrowsable(false)]
	public StiTableStyleFX TableStyleFX
	{
		get
		{
			return tableStyleFX;
		}
		set
		{
			if (tableStyleFX != value)
			{
				tableStyleFX = value;
				RefreshTableStyle();
			}
		}
	}

	[Browsable(false)]
	public StiTableStyle TableStyle
	{
		get
		{
			return StiTableStyle.StyleNone;
		}
		set
		{
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "Columns");
		JsonExtensions.RemoveProperty(val, "ColumnWidth");
		JsonExtensions.RemoveProperty(val, "ColumnGaps");
		JsonExtensions.RemoveProperty(val, "MinRowsInColumn");
		JsonExtensions.AddPropertyBool(val, "DockableTable", DockableTable, true, false);
		JsonExtensions.AddPropertyEnum(val, "AutoWidth", (Enum)AutoWidth, (Enum)StiTableAutoWidth.None);
		JsonExtensions.AddPropertyEnum(val, "AutoWidthType", (Enum)AutoWidthType, (Enum)StiTableAutoWidthType.None);
		JsonExtensions.AddPropertyInt(val, "RowCount", RowCount, 5);
		JsonExtensions.AddPropertyInt(val, "ColumnCount", ColumnCount, 5);
		JsonExtensions.AddPropertyInt(val, "FooterRowsCount", FooterRowsCount, 0);
		JsonExtensions.AddPropertyInt(val, "HeaderRowsCount", HeaderRowsCount, 0);
		JsonExtensions.AddPropertyEnum(val, "HeaderPrintOn", (Enum)HeaderPrintOn, (Enum)StiPrintOnType.AllPages);
		JsonExtensions.AddPropertyBool(val, "HeaderCanGrow", HeaderCanGrow, true, false);
		JsonExtensions.AddPropertyBool(val, "HeaderCanShrink", HeaderCanShrink, false, false);
		JsonExtensions.AddPropertyBool(val, "HeaderCanBreak", HeaderCanBreak, false, false);
		JsonExtensions.AddPropertyBool(val, "HeaderPrintAtBottom", HeaderPrintAtBottom, false, false);
		JsonExtensions.AddPropertyBool(val, "HeaderPrintIfEmpty", HeaderPrintIfEmpty, true, false);
		JsonExtensions.AddPropertyBool(val, "HeaderPrintOnAllPages", HeaderPrintOnAllPages, true, false);
		JsonExtensions.AddPropertyEnum(val, "HeaderPrintOnEvenOddPages", (Enum)HeaderPrintOnEvenOddPages, (Enum)StiPrintOnEvenOddPagesType.Ignore);
		JsonExtensions.AddPropertyEnum(val, "FooterPrintOn", (Enum)FooterPrintOn, (Enum)StiPrintOnType.AllPages);
		JsonExtensions.AddPropertyBool(val, "FooterCanGrow", FooterCanGrow, true, false);
		JsonExtensions.AddPropertyBool(val, "FooterCanShrink", FooterCanShrink, false, false);
		JsonExtensions.AddPropertyBool(val, "FooterCanBreak", FooterCanBreak, false, false);
		JsonExtensions.AddPropertyBool(val, "FooterPrintAtBottom", FooterPrintAtBottom, false, false);
		JsonExtensions.AddPropertyBool(val, "FooterPrintIfEmpty", FooterPrintIfEmpty, true, false);
		JsonExtensions.AddPropertyBool(val, "FooterPrintOnAllPages", FooterPrintOnAllPages, false, false);
		JsonExtensions.AddPropertyEnum(val, "FooterPrintOnEvenOddPages", (Enum)FooterPrintOnEvenOddPages, (Enum)StiPrintOnEvenOddPagesType.Ignore);
		JsonExtensions.AddPropertyInt(val, "NumberID", NumberID, 0);
		JsonExtensions.AddPropertyEnum(val, "TableStyle", (Enum)TableStyle, (Enum)StiTableStyle.StyleNone);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "DockableTable":
				DockableTable = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AutoWidth":
				AutoWidth = StiJsonReportObjectHelper.DeserializeEnum<StiTableAutoWidth>(item);
				break;
			case "AutoWidthType":
				AutoWidthType = StiJsonReportObjectHelper.DeserializeEnum<StiTableAutoWidthType>(item);
				break;
			case "RowCount":
				rowCount = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "ColumnCount":
				columnCount = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "FooterRowsCount":
				footerRowsCount = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "HeaderRowsCount":
				headerRowsCount = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "HeaderPrintOn":
				HeaderPrintOn = StiJsonReportObjectHelper.DeserializeEnum<StiPrintOnType>(item);
				break;
			case "HeaderCanGrow":
				HeaderCanGrow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "HeaderCanShrink":
				HeaderCanShrink = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "HeaderCanBreak":
				HeaderCanBreak = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "HeaderPrintAtBottom":
				HeaderPrintAtBottom = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "HeaderPrintIfEmpty":
				HeaderPrintIfEmpty = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "HeaderPrintOnAllPages":
				HeaderPrintOnAllPages = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "HeaderPrintOnEvenOddPages":
				HeaderPrintOnEvenOddPages = StiJsonReportObjectHelper.DeserializeEnum<StiPrintOnEvenOddPagesType>(item);
				break;
			case "FooterPrintOn":
				FooterPrintOn = StiJsonReportObjectHelper.DeserializeEnum<StiPrintOnType>(item);
				break;
			case "FooterCanGrow":
				FooterCanGrow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "FooterCanShrink":
				FooterCanShrink = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "FooterCanBreak":
				FooterCanBreak = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "FooterPrintAtBottom":
				FooterPrintAtBottom = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "FooterPrintIfEmpty":
				FooterPrintIfEmpty = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "FooterPrintOnAllPages":
				FooterPrintOnAllPages = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "FooterPrintOnEvenOddPages":
				FooterPrintOnEvenOddPages = StiJsonReportObjectHelper.DeserializeEnum<StiPrintOnEvenOddPagesType>(item);
				break;
			case "NumberID":
				NumberID = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Invalid comparison between Unknown and I4
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Data, new StiPropertyObject[1] { propertiesHelper.DataBandEditor() });
		stiPropertyCollection.Add(StiPropertyCategories.Table, new StiPropertyObject[8]
		{
			propertiesHelper.TableAutoWidth(),
			propertiesHelper.AutoWidthType(),
			propertiesHelper.ColumnCount(),
			propertiesHelper.RowCount(),
			propertiesHelper.HeaderRowsCount(),
			propertiesHelper.FooterRowsCount(),
			propertiesHelper.RightToLeft(),
			propertiesHelper.DockableTable()
		});
		stiPropertyCollection.Add(StiPropertyCategories.HeaderTable, new StiPropertyObject[8]
		{
			propertiesHelper.HeaderPrintOn(),
			propertiesHelper.HeaderCanGrow(),
			propertiesHelper.HeaderCanShrink(),
			propertiesHelper.HeaderCanBreak(),
			propertiesHelper.HeaderPrintAtBottom(),
			propertiesHelper.HeaderPrintIfEmpty(),
			propertiesHelper.HeaderPrintOnAllPages(),
			propertiesHelper.HeaderPrintOnEvenOddPages()
		});
		stiPropertyCollection.Add(StiPropertyCategories.FooterTable, new StiPropertyObject[8]
		{
			propertiesHelper.FooterPrintOn(),
			propertiesHelper.FooterCanGrow(),
			propertiesHelper.FooterCanShrink(),
			propertiesHelper.FooterCanBreak(),
			propertiesHelper.FooterPrintAtBottom(),
			propertiesHelper.FooterPrintIfEmpty(),
			propertiesHelper.FooterPrintOnAllPages(),
			propertiesHelper.FooterPrintOnEvenOddPages()
		});
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
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[6]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter(),
				propertiesHelper.BreakIfLessThan(),
				propertiesHelper.SkipFirst()
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[3]
			{
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[11]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.CalcInvisible(),
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

	public override object Clone(bool cloneProperties, bool cloneComponents)
	{
		StiTable stiTable = (StiTable)base.Clone(cloneProperties, cloneComponents: false);
		stiTable.NumberID = NumberID;
		stiTable.rowCount = rowCount;
		stiTable.columnCount = columnCount;
		stiTable.footerRowsCount = footerRowsCount;
		stiTable.headerRowsCount = headerRowsCount;
		stiTable.DockableTable = DockableTable;
		stiTable.HeaderPrintOn = HeaderPrintOn;
		stiTable.HeaderCanGrow = HeaderCanGrow;
		stiTable.HeaderCanShrink = HeaderCanShrink;
		stiTable.HeaderCanBreak = HeaderCanBreak;
		stiTable.HeaderPrintAtBottom = HeaderPrintAtBottom;
		stiTable.HeaderPrintIfEmpty = HeaderPrintIfEmpty;
		stiTable.HeaderPrintOnAllPages = HeaderPrintOnAllPages;
		stiTable.HeaderPrintOnEvenOddPages = HeaderPrintOnEvenOddPages;
		stiTable.FooterPrintOn = FooterPrintOn;
		stiTable.FooterCanGrow = FooterCanGrow;
		stiTable.FooterCanShrink = FooterCanShrink;
		stiTable.FooterCanBreak = FooterCanBreak;
		stiTable.FooterPrintAtBottom = FooterPrintAtBottom;
		stiTable.FooterPrintIfEmpty = FooterPrintIfEmpty;
		stiTable.FooterPrintOnAllPages = FooterPrintOnAllPages;
		stiTable.FooterPrintOnEvenOddPages = FooterPrintOnEvenOddPages;
		stiTable.AutoWidth = AutoWidth;
		stiTable.AutoWidthType = AutoWidthType;
		stiTable.Components = new StiComponentsCollection(stiTable);
		if (!cloneComponents)
		{
			return stiTable;
		}
		foreach (StiComponent component in Components)
		{
			switch (((IStiTableCell)component).CellType)
			{
			case StiTablceCellType.Text:
			{
				StiTableCell stiTableCell = (StiTableCell)component.Clone(cloneProperties: true);
				stiTableCell.Parent = stiTable;
				stiTable.Components.Add(stiTableCell);
				break;
			}
			case StiTablceCellType.Image:
			{
				StiTableCellImage stiTableCellImage = (StiTableCellImage)component.Clone(cloneProperties: true);
				stiTableCellImage.Parent = stiTable;
				stiTable.Components.Add(stiTableCellImage);
				break;
			}
			case StiTablceCellType.CheckBox:
			{
				StiTableCellCheckBox stiTableCellCheckBox = (StiTableCellCheckBox)component.Clone(cloneProperties: true);
				stiTableCellCheckBox.Parent = stiTable;
				stiTable.Components.Add(stiTableCellCheckBox);
				break;
			}
			case StiTablceCellType.RichText:
			{
				StiTableCellRichText stiTableCellRichText = (StiTableCellRichText)component.Clone(cloneProperties: true);
				stiTableCellRichText.Parent = stiTable;
				stiTable.Components.Add(stiTableCellRichText);
				break;
			}
			}
		}
		return stiTable;
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		base.Convert(oldUnit, newUnit, isReportSnapshot);
		DistributeRows();
		DistributeColumns();
	}

	internal void ChangeGrowToHeightAtCell(StiComponent cell)
	{
		int num = Components.IndexOf(cell);
		if (num == -1)
		{
			return;
		}
		int num2 = num / columnCount;
		int num3 = num2 * columnCount;
		bool flag = false;
		for (int i = num3; i < num3 + columnCount; i++)
		{
			StiComponent stiComponent = Components[i];
			if (stiComponent.CanGrow)
			{
				flag = true;
				break;
			}
		}
		for (int j = num3; j < num3 + columnCount; j++)
		{
			StiComponent stiComponent2 = Components[j];
			stiComponent2.GrowToHeight = flag;
		}
	}

	private void RefreshTableStyle()
	{
		ApplyCustomStyle();
	}

	private void ApplyCustomStyle()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Expected O, but got Unknown
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Expected O, but got Unknown
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Expected O, but got Unknown
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Expected O, but got Unknown
		if (Components.Count == 0)
		{
			return;
		}
		Stimulsoft.Report.StiTableStyle stiTableStyle = null;
		if (!string.IsNullOrEmpty(base.ComponentStyle))
		{
			if (Report != null)
			{
				stiTableStyle = Report.Styles[base.ComponentStyle] as Stimulsoft.Report.StiTableStyle;
			}
		}
		else
		{
			stiTableStyle = tableStyleFX;
		}
		if (stiTableStyle == null)
		{
			return;
		}
		StiSolidBrush val = new StiSolidBrush(stiTableStyle.HeaderColor);
		StiSolidBrush val2 = new StiSolidBrush(stiTableStyle.HeaderForeground);
		StiSolidBrush val3 = new StiSolidBrush(stiTableStyle.FooterColor);
		StiSolidBrush val4 = new StiSolidBrush(stiTableStyle.FooterForeground);
		StiBorder val5 = new StiBorder((StiBorderSides)15, stiTableStyle.GridColor, 1.0, (StiPenStyle)0, false, 4.0, (StiBrush)new StiSolidBrush(stiTableStyle.GridColor));
		StiBorder val6 = new StiBorder((StiBorderSides)15, stiTableStyle.GridColor, 1.0, (StiPenStyle)0, false, 4.0, (StiBrush)new StiSolidBrush(stiTableStyle.GridColor));
		StiBorder val7 = new StiBorder((StiBorderSides)15, stiTableStyle.GridColor, 1.0, (StiPenStyle)0, false, 4.0, (StiBrush)new StiSolidBrush(stiTableStyle.GridColor));
		StiSolidBrush val8 = new StiSolidBrush(stiTableStyle.DataColor);
		StiSolidBrush val9 = new StiSolidBrush(stiTableStyle.DataForeground);
		int num = 0;
		if (headerRowsCount > 0)
		{
			int num2 = 0;
			for (int i = 0; i < headerRowsCount; i++)
			{
				for (int j = 0; j < columnCount; j++)
				{
					StiComponent stiComponent = Components[num2];
					if (stiComponent != null)
					{
						switch (((IStiTableCell)stiComponent).CellType)
						{
						case StiTablceCellType.Text:
							((StiTableCell)stiComponent).Border = new StiBorder();
							((StiTableCell)stiComponent).Brush = (StiBrush)((StiBrush)val).Clone();
							((StiTableCell)stiComponent).TextBrush = (StiBrush)(StiSolidBrush)((StiBrush)val2).Clone();
							break;
						case StiTablceCellType.Image:
							((StiTableCellImage)stiComponent).Border = new StiBorder();
							((StiTableCellImage)stiComponent).Brush = (StiBrush)((StiBrush)val).Clone();
							break;
						case StiTablceCellType.CheckBox:
							((StiTableCellCheckBox)stiComponent).Border = new StiBorder();
							((StiTableCellCheckBox)stiComponent).Brush = (StiBrush)((StiBrush)val).Clone();
							break;
						case StiTablceCellType.RichText:
							((StiTableCellRichText)stiComponent).Border = new StiBorder();
							((StiTableCellRichText)stiComponent).BackColor = StiBrush.ToColor((StiBrush)((StiBrush)val).Clone());
							break;
						}
					}
					num2++;
				}
			}
			num = headerRowsCount;
		}
		if (footerRowsCount > 0)
		{
			int num3 = rowCount - footerRowsCount;
			int num4 = num3 * columnCount;
			for (int k = num3; k < rowCount; k++)
			{
				for (int l = 0; l < columnCount; l++)
				{
					StiComponent stiComponent2 = Components[num4];
					if (stiComponent2 != null)
					{
						switch (((IStiTableCell)stiComponent2).CellType)
						{
						case StiTablceCellType.Text:
							((StiTableCell)stiComponent2).Border = new StiBorder();
							((StiTableCell)stiComponent2).Brush = (StiBrush)((StiBrush)val3).Clone();
							((StiTableCell)stiComponent2).TextBrush = (StiBrush)(StiSolidBrush)((StiBrush)val4).Clone();
							break;
						case StiTablceCellType.Image:
							((StiTableCellImage)stiComponent2).Border = new StiBorder();
							((StiTableCellImage)stiComponent2).Brush = (StiBrush)((StiBrush)val3).Clone();
							break;
						case StiTablceCellType.CheckBox:
							((StiTableCellCheckBox)stiComponent2).Border = new StiBorder();
							((StiTableCellCheckBox)stiComponent2).Brush = (StiBrush)((StiBrush)val3).Clone();
							break;
						case StiTablceCellType.RichText:
							((StiTableCellRichText)stiComponent2).Border = new StiBorder();
							((StiTableCellRichText)stiComponent2).BackColor = StiBrush.ToColor((StiBrush)((StiBrush)val3).Clone());
							break;
						}
					}
					num4++;
				}
			}
		}
		int num5 = num * columnCount;
		for (int m = num; m < rowCount - footerRowsCount; m++)
		{
			for (int n = 0; n < columnCount; n++)
			{
				StiComponent stiComponent3 = Components[num5];
				if (stiComponent3 != null)
				{
					switch (((IStiTableCell)stiComponent3).CellType)
					{
					case StiTablceCellType.Text:
						if (n == 0)
						{
							((StiTableCell)stiComponent3).Border = (StiBorder)val6.Clone();
						}
						else if (n == columnCount - 1)
						{
							((StiTableCell)stiComponent3).Border = (StiBorder)val7.Clone();
						}
						else
						{
							((StiTableCell)stiComponent3).Border = (StiBorder)val5.Clone();
						}
						((StiTableCell)stiComponent3).Brush = (StiBrush)((StiBrush)val8).Clone();
						((StiTableCell)stiComponent3).TextBrush = (StiBrush)(StiSolidBrush)((StiBrush)val9).Clone();
						break;
					case StiTablceCellType.Image:
						if (n == 0)
						{
							((StiTableCellImage)stiComponent3).Border = (StiBorder)val6.Clone();
						}
						else if (n == columnCount - 1)
						{
							((StiTableCellImage)stiComponent3).Border = (StiBorder)val7.Clone();
						}
						else
						{
							((StiTableCellImage)stiComponent3).Border = (StiBorder)val5.Clone();
						}
						((StiTableCellImage)stiComponent3).Brush = (StiBrush)((StiBrush)val8).Clone();
						break;
					case StiTablceCellType.CheckBox:
						if (n == 0)
						{
							((StiTableCellCheckBox)stiComponent3).Border = (StiBorder)val6.Clone();
						}
						else if (n == columnCount - 1)
						{
							((StiTableCellCheckBox)stiComponent3).Border = (StiBorder)val7.Clone();
						}
						else
						{
							((StiTableCellCheckBox)stiComponent3).Border = (StiBorder)val5.Clone();
						}
						((StiTableCellCheckBox)stiComponent3).Brush = (StiBrush)((StiBrush)val8).Clone();
						break;
					case StiTablceCellType.RichText:
						if (n == 0)
						{
							((StiTableCellRichText)stiComponent3).Border = (StiBorder)val6.Clone();
						}
						else if (n == columnCount - 1)
						{
							((StiTableCellRichText)stiComponent3).Border = (StiBorder)val7.Clone();
						}
						else
						{
							((StiTableCellRichText)stiComponent3).Border = (StiBorder)val5.Clone();
						}
						((StiTableCellRichText)stiComponent3).BackColor = StiBrush.ToColor((StiBrush)((StiBrush)val8).Clone());
						break;
					}
				}
				num5++;
			}
		}
	}

	private void ChangeRowCount(int oldValue, int value)
	{
		if (Page != null && Report != null && value > 0 && value != oldValue)
		{
			if (value < headerRowsCount + footerRowsCount)
			{
				headerRowsCount = 0;
				footerRowsCount = 0;
			}
			base.MinHeight = Page.GridSize * (double)value;
			int num = value - oldValue;
			if (num > 0)
			{
				AddNewRows(num);
			}
			else
			{
				DeleteLastRows(Math.Abs(num), oldValue);
			}
		}
	}

	private void ChangeColumnCount(int oldValue, int value)
	{
		if (Page != null && Report != null && value > 0 && value != oldValue)
		{
			int num = value - oldValue;
			if (num > 0)
			{
				AddTableNewColumns(num, oldValue);
			}
			else
			{
				DeleteTableColumns(Math.Abs(num), oldValue);
			}
		}
	}

	public int[] CreateJoin(ref double sumWidth, ref double sumHeight, ref int joinWidth, ref int joinHeight)
	{
		int countSelectedCells = GetCountSelectedCells();
		if (countSelectedCells <= 1 || Page == null)
		{
			return new int[0];
		}
		int[] array = new int[countSelectedCells];
		int[] array2 = new int[countSelectedCells];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (StiComponent component in Components)
		{
			if (component.IsSelected)
			{
				array[num3] = num2;
				array2[num3] = num;
				num3++;
				component.IsSelected = false;
			}
			num2++;
			if (num2 == columnCount)
			{
				num2 = 0;
				num++;
			}
		}
		FindLeftSelectedElement(array, out var leftX, out var rightX);
		FindRightSelectedElement(array2, out var leftY, out var rightY);
		joinWidth = rightX - leftX + 1;
		joinHeight = rightY - leftY + 1;
		int[] array3 = new int[(rightY - leftY + 1) * (rightX - leftX + 1)];
		int num4 = 0;
		for (int i = leftY; i <= rightY; i++)
		{
			StiComponent stiComponent2 = null;
			for (int j = leftX; j <= rightX; j++)
			{
				int index = i * columnCount + j;
				stiComponent2 = Components[index];
				if (((IStiTableCell)stiComponent2).Merged)
				{
					StiComponent joinComponentByGuid = ((IStiTableCell)stiComponent2).GetJoinComponentByGuid(((IStiTableCell)stiComponent2).ParentJoin);
					((IStiTableCell)joinComponentByGuid).Join = false;
				}
				array3[num4] = ((IStiTableCell)stiComponent2).ID;
				if (i == leftY)
				{
					sumWidth += stiComponent2.Width;
				}
				num4++;
			}
			sumHeight += stiComponent2.Height;
		}
		return array3;
	}

	private int GetCountSelectedCells()
	{
		int num = 0;
		foreach (StiComponent component in Components)
		{
			if (component.IsSelected)
			{
				num++;
			}
		}
		return num;
	}

	private void FindLeftSelectedElement(int[] allX, out int leftX, out int rightX)
	{
		leftX = allX[0];
		rightX = allX[0];
		for (int i = 1; i < allX.Length; i++)
		{
			if (leftX > allX[i])
			{
				leftX = allX[i];
			}
			if (rightX < allX[i])
			{
				rightX = allX[i];
			}
		}
	}

	private void FindRightSelectedElement(int[] allY, out int leftY, out int rightY)
	{
		leftY = allY[0];
		rightY = allY[0];
		for (int i = 1; i < allY.Length; i++)
		{
			if (leftY > allY[i])
			{
				leftY = allY[i];
			}
			if (rightY < allY[i])
			{
				rightY = allY[i];
			}
		}
	}

	internal void ChangeTableCellContentInImage(StiTableCell cellText)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellText);
		if (num != -1)
		{
			StiTableCellImage stiTableCellImage = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellImage") as StiTableCellImage;
			stiTableCellImage.ClientRectangle = cellText.ClientRectangle;
			stiTableCellImage.Name = cellText.Name;
			stiTableCellImage.Border = cellText.Border;
			stiTableCellImage.Brush = cellText.Brush;
			stiTableCellImage.ID = cellText.ID;
			stiTableCellImage.Restrictions = cellText.Restrictions;
			stiTableCellImage.Page = cellText.Page;
			stiTableCellImage.Parent = cellText.Parent;
			stiTableCellImage.CanBreak = cellText.CanBreak;
			stiTableCellImage.CanGrow = cellText.CanGrow;
			stiTableCellImage.CanShrink = cellText.CanShrink;
			stiTableCellImage.Enabled = cellText.Enabled;
			stiTableCellImage.GrowToHeight = cellText.GrowToHeight;
			stiTableCellImage.Printable = cellText.Printable;
			if (cellText.Text.Value.Length != 0)
			{
				stiTableCellImage.DataColumn = cellText.Text.Value.Substring(1, cellText.Text.Value.Length - 2);
			}
			if (cellText.Join)
			{
				stiTableCellImage.SetJoin(cellText.Join);
				stiTableCellImage.ParentJoin = cellText.ParentJoin;
				stiTableCellImage.JoinCells = cellText.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellImage);
			stiTableCellImage.Select();
		}
	}

	internal void ChangeTableCellContentInImage(StiTableCellCheckBox cellCheckBox)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellCheckBox);
		if (num != -1)
		{
			StiTableCellImage stiTableCellImage = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellImage") as StiTableCellImage;
			stiTableCellImage.ClientRectangle = cellCheckBox.ClientRectangle;
			stiTableCellImage.Name = cellCheckBox.Name;
			stiTableCellImage.Border = cellCheckBox.Border;
			stiTableCellImage.Brush = cellCheckBox.Brush;
			stiTableCellImage.ID = cellCheckBox.ID;
			stiTableCellImage.Restrictions = cellCheckBox.Restrictions;
			stiTableCellImage.Page = cellCheckBox.Page;
			stiTableCellImage.Parent = cellCheckBox.Parent;
			stiTableCellImage.CanGrow = cellCheckBox.CanGrow;
			stiTableCellImage.CanShrink = cellCheckBox.CanShrink;
			stiTableCellImage.Enabled = cellCheckBox.Enabled;
			stiTableCellImage.GrowToHeight = cellCheckBox.GrowToHeight;
			stiTableCellImage.Printable = cellCheckBox.Printable;
			if (cellCheckBox.Checked.Value.Length != 0)
			{
				stiTableCellImage.DataColumn = cellCheckBox.Checked.Value.Substring(1, cellCheckBox.Checked.Value.Length - 2);
			}
			if (cellCheckBox.join)
			{
				stiTableCellImage.SetJoin(cellCheckBox.Join);
				stiTableCellImage.ParentJoin = cellCheckBox.ParentJoin;
				stiTableCellImage.JoinCells = cellCheckBox.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellImage);
			stiTableCellImage.Select();
		}
	}

	internal void ChangeTableCellContentInImage(StiTableCellRichText cellRichText)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		int num = Components.IndexOf(cellRichText);
		if (num != -1)
		{
			StiTableCellImage stiTableCellImage = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellImage") as StiTableCellImage;
			stiTableCellImage.ClientRectangle = cellRichText.ClientRectangle;
			stiTableCellImage.Name = cellRichText.Name;
			stiTableCellImage.Border = cellRichText.Border;
			stiTableCellImage.Brush = (StiBrush)new StiSolidBrush(cellRichText.BackColor);
			stiTableCellImage.ID = cellRichText.ID;
			stiTableCellImage.Restrictions = cellRichText.Restrictions;
			stiTableCellImage.Page = cellRichText.Page;
			stiTableCellImage.Parent = cellRichText.Parent;
			stiTableCellImage.CanGrow = cellRichText.CanGrow;
			stiTableCellImage.CanShrink = cellRichText.CanShrink;
			stiTableCellImage.Enabled = cellRichText.Enabled;
			stiTableCellImage.GrowToHeight = cellRichText.GrowToHeight;
			stiTableCellImage.Printable = cellRichText.Printable;
			if (cellRichText.DataColumn.Length != 0)
			{
				stiTableCellImage.DataColumn = cellRichText.DataColumn;
			}
			if (cellRichText.Join)
			{
				stiTableCellImage.SetJoin(cellRichText.Join);
				stiTableCellImage.ParentJoin = cellRichText.ParentJoin;
				stiTableCellImage.JoinCells = cellRichText.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellImage);
			stiTableCellImage.Select();
		}
	}

	internal void ChangeTableCellContentInText(StiTableCellImage cellImage)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellImage);
		if (num != -1)
		{
			StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
			stiTableCell.ClientRectangle = cellImage.ClientRectangle;
			stiTableCell.Name = cellImage.Name;
			stiTableCell.Border = cellImage.Border;
			stiTableCell.Brush = cellImage.Brush;
			stiTableCell.ID = cellImage.ID;
			stiTableCell.Restrictions = cellImage.Restrictions;
			stiTableCell.Page = cellImage.Page;
			stiTableCell.Parent = cellImage.Parent;
			stiTableCell.CanBreak = cellImage.CanBreak;
			stiTableCell.CanGrow = cellImage.CanGrow;
			stiTableCell.CanShrink = cellImage.CanShrink;
			stiTableCell.Enabled = cellImage.Enabled;
			stiTableCell.GrowToHeight = cellImage.GrowToHeight;
			stiTableCell.Printable = cellImage.Printable;
			if (cellImage.DataColumn.Length != 0)
			{
				stiTableCell.Text.Value = "{" + cellImage.DataColumn + "}";
			}
			if (cellImage.Join)
			{
				stiTableCell.SetJoin(cellImage.Join);
				stiTableCell.ParentJoin = cellImage.ParentJoin;
				stiTableCell.JoinCells = cellImage.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCell);
			stiTableCell.Select();
		}
	}

	internal void ChangeTableCellContentInText(StiTableCellCheckBox cellCheckBox)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellCheckBox);
		if (num != -1)
		{
			StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
			stiTableCell.ClientRectangle = cellCheckBox.ClientRectangle;
			stiTableCell.Name = cellCheckBox.Name;
			stiTableCell.Border = cellCheckBox.Border;
			stiTableCell.Brush = cellCheckBox.Brush;
			stiTableCell.ID = cellCheckBox.ID;
			stiTableCell.Restrictions = cellCheckBox.Restrictions;
			stiTableCell.Page = cellCheckBox.Page;
			stiTableCell.Parent = cellCheckBox.Parent;
			stiTableCell.CanGrow = cellCheckBox.CanGrow;
			stiTableCell.CanShrink = cellCheckBox.CanShrink;
			stiTableCell.Enabled = cellCheckBox.Enabled;
			stiTableCell.GrowToHeight = cellCheckBox.GrowToHeight;
			stiTableCell.Printable = cellCheckBox.Printable;
			if (cellCheckBox.Checked.Value.Length != 0)
			{
				stiTableCell.Text.Value = cellCheckBox.Checked.Value;
			}
			if (cellCheckBox.Join)
			{
				stiTableCell.SetJoin(cellCheckBox.Join);
				stiTableCell.ParentJoin = cellCheckBox.ParentJoin;
				stiTableCell.JoinCells = cellCheckBox.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCell);
			stiTableCell.Select();
		}
	}

	internal void ChangeTableCellContentInText(StiTableCellRichText cellRichText)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		int num = Components.IndexOf(cellRichText);
		if (num != -1)
		{
			StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
			stiTableCell.ClientRectangle = cellRichText.ClientRectangle;
			stiTableCell.Name = cellRichText.Name;
			stiTableCell.Border = cellRichText.Border;
			stiTableCell.Brush = (StiBrush)new StiSolidBrush(cellRichText.BackColor);
			stiTableCell.ID = cellRichText.ID;
			stiTableCell.Restrictions = cellRichText.Restrictions;
			stiTableCell.Page = cellRichText.Page;
			stiTableCell.Parent = cellRichText.Parent;
			stiTableCell.CanGrow = cellRichText.CanGrow;
			stiTableCell.CanShrink = cellRichText.CanShrink;
			stiTableCell.Enabled = cellRichText.Enabled;
			stiTableCell.GrowToHeight = cellRichText.GrowToHeight;
			stiTableCell.Printable = cellRichText.Printable;
			if (cellRichText.DataColumn.Length != 0)
			{
				stiTableCell.Text.Value = "{" + cellRichText.DataColumn + "}";
			}
			if (cellRichText.Join)
			{
				stiTableCell.SetJoin(cellRichText.Join);
				stiTableCell.ParentJoin = cellRichText.ParentJoin;
				stiTableCell.JoinCells = cellRichText.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCell);
			stiTableCell.Select();
		}
	}

	internal void ChangeTableCellContentInCheckBox(StiTableCellImage cellImage)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellImage);
		if (num != -1)
		{
			StiTableCellCheckBox stiTableCellCheckBox = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellCheckBox") as StiTableCellCheckBox;
			stiTableCellCheckBox.ClientRectangle = cellImage.ClientRectangle;
			stiTableCellCheckBox.Name = cellImage.Name;
			stiTableCellCheckBox.Border = cellImage.Border;
			stiTableCellCheckBox.Brush = cellImage.Brush;
			stiTableCellCheckBox.ID = cellImage.ID;
			stiTableCellCheckBox.Restrictions = cellImage.Restrictions;
			stiTableCellCheckBox.Page = cellImage.Page;
			stiTableCellCheckBox.Parent = cellImage.Parent;
			stiTableCellCheckBox.CanGrow = cellImage.CanGrow;
			stiTableCellCheckBox.CanShrink = cellImage.CanShrink;
			stiTableCellCheckBox.Enabled = cellImage.Enabled;
			stiTableCellCheckBox.GrowToHeight = cellImage.GrowToHeight;
			stiTableCellCheckBox.Printable = cellImage.Printable;
			if (cellImage.DataColumn.Length != 0)
			{
				stiTableCellCheckBox.Checked.Value = "{" + cellImage.DataColumn + "}";
			}
			if (cellImage.Join)
			{
				stiTableCellCheckBox.SetJoin(cellImage.Join);
				stiTableCellCheckBox.ParentJoin = cellImage.ParentJoin;
				stiTableCellCheckBox.JoinCells = cellImage.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellCheckBox);
			stiTableCellCheckBox.Select();
		}
	}

	internal void ChangeTableCellContentInCheckBox(StiTableCell cellText)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellText);
		if (num != -1)
		{
			StiTableCellCheckBox stiTableCellCheckBox = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellCheckBox") as StiTableCellCheckBox;
			stiTableCellCheckBox.ClientRectangle = cellText.ClientRectangle;
			stiTableCellCheckBox.Name = cellText.Name;
			stiTableCellCheckBox.Border = cellText.Border;
			stiTableCellCheckBox.Brush = cellText.Brush;
			stiTableCellCheckBox.ID = cellText.ID;
			stiTableCellCheckBox.Restrictions = cellText.Restrictions;
			stiTableCellCheckBox.Page = cellText.Page;
			stiTableCellCheckBox.Parent = cellText.Parent;
			stiTableCellCheckBox.CanGrow = cellText.CanGrow;
			stiTableCellCheckBox.CanShrink = cellText.CanShrink;
			stiTableCellCheckBox.Enabled = cellText.Enabled;
			stiTableCellCheckBox.GrowToHeight = cellText.GrowToHeight;
			stiTableCellCheckBox.Printable = cellText.Printable;
			if (cellText.Text.Value.Length != 0)
			{
				stiTableCellCheckBox.Checked.Value = cellText.Text.Value;
			}
			if (cellText.Join)
			{
				stiTableCellCheckBox.SetJoin(cellText.Join);
				stiTableCellCheckBox.ParentJoin = cellText.ParentJoin;
				stiTableCellCheckBox.JoinCells = cellText.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellCheckBox);
			stiTableCellCheckBox.Select();
		}
	}

	internal void ChangeTableCellContentInCheckBox(StiTableCellRichText cellRichText)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		int num = Components.IndexOf(cellRichText);
		if (num != -1)
		{
			StiTableCellCheckBox stiTableCellCheckBox = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellCheckBox") as StiTableCellCheckBox;
			stiTableCellCheckBox.ClientRectangle = cellRichText.ClientRectangle;
			stiTableCellCheckBox.Name = cellRichText.Name;
			stiTableCellCheckBox.Border = cellRichText.Border;
			stiTableCellCheckBox.Brush = (StiBrush)new StiSolidBrush(cellRichText.BackColor);
			stiTableCellCheckBox.ID = cellRichText.ID;
			stiTableCellCheckBox.Restrictions = cellRichText.Restrictions;
			stiTableCellCheckBox.Page = cellRichText.Page;
			stiTableCellCheckBox.Parent = cellRichText.Parent;
			stiTableCellCheckBox.CanGrow = cellRichText.CanGrow;
			stiTableCellCheckBox.CanShrink = cellRichText.CanShrink;
			stiTableCellCheckBox.Enabled = cellRichText.Enabled;
			stiTableCellCheckBox.GrowToHeight = cellRichText.GrowToHeight;
			stiTableCellCheckBox.Printable = cellRichText.Printable;
			if (cellRichText.DataColumn.Length != 0)
			{
				stiTableCellCheckBox.Checked.Value = "{" + cellRichText.DataColumn + "}";
			}
			if (cellRichText.Join)
			{
				stiTableCellCheckBox.join = cellRichText.Join;
				stiTableCellCheckBox.ParentJoin = cellRichText.ParentJoin;
				stiTableCellCheckBox.JoinCells = cellRichText.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellCheckBox);
			stiTableCellCheckBox.Select();
		}
	}

	internal void ChangeTableCellContentInRichText(StiTableCell cellText)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellText);
		if (num != -1)
		{
			StiTableCellRichText stiTableCellRichText = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellRichText") as StiTableCellRichText;
			stiTableCellRichText.ClientRectangle = cellText.ClientRectangle;
			stiTableCellRichText.Name = cellText.Name;
			stiTableCellRichText.Border = cellText.Border;
			stiTableCellRichText.BackColor = StiBrush.ToColor(cellText.Brush);
			stiTableCellRichText.ID = cellText.ID;
			stiTableCellRichText.Restrictions = cellText.Restrictions;
			stiTableCellRichText.Page = cellText.Page;
			stiTableCellRichText.Parent = cellText.Parent;
			stiTableCellRichText.CanBreak = cellText.CanBreak;
			stiTableCellRichText.CanGrow = cellText.CanGrow;
			stiTableCellRichText.CanShrink = cellText.CanShrink;
			stiTableCellRichText.Enabled = cellText.Enabled;
			stiTableCellRichText.GrowToHeight = cellText.GrowToHeight;
			stiTableCellRichText.Printable = cellText.Printable;
			if (cellText.Text.Value.Length != 0)
			{
				stiTableCellRichText.DataColumn = cellText.Text.Value.Substring(1, cellText.Text.Value.Length - 2);
			}
			if (cellText.Join)
			{
				stiTableCellRichText.SetJoin(cellText.Join);
				stiTableCellRichText.ParentJoin = cellText.ParentJoin;
				stiTableCellRichText.JoinCells = cellText.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellRichText);
			stiTableCellRichText.Select();
		}
	}

	internal void ChangeTableCellContentInRichText(StiTableCellImage cellImage)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellImage);
		if (num != -1)
		{
			StiTableCellRichText stiTableCellRichText = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellRichText") as StiTableCellRichText;
			stiTableCellRichText.ClientRectangle = cellImage.ClientRectangle;
			stiTableCellRichText.Name = cellImage.Name;
			stiTableCellRichText.Border = cellImage.Border;
			stiTableCellRichText.BackColor = StiBrush.ToColor(cellImage.Brush);
			stiTableCellRichText.ID = cellImage.ID;
			stiTableCellRichText.Restrictions = cellImage.Restrictions;
			stiTableCellRichText.Page = cellImage.Page;
			stiTableCellRichText.Parent = cellImage.Parent;
			stiTableCellRichText.CanBreak = cellImage.CanBreak;
			stiTableCellRichText.CanGrow = cellImage.CanGrow;
			stiTableCellRichText.CanShrink = cellImage.CanShrink;
			stiTableCellRichText.Enabled = cellImage.Enabled;
			stiTableCellRichText.GrowToHeight = cellImage.GrowToHeight;
			stiTableCellRichText.Printable = cellImage.Printable;
			if (cellImage.DataColumn.Length != 0)
			{
				stiTableCellRichText.DataColumn = cellImage.DataColumn;
			}
			if (cellImage.Join)
			{
				stiTableCellRichText.SetJoin(cellImage.Join);
				stiTableCellRichText.ParentJoin = cellImage.ParentJoin;
				stiTableCellRichText.JoinCells = cellImage.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellRichText);
			stiTableCellRichText.Select();
		}
	}

	internal void ChangeTableCellContentInRichText(StiTableCellCheckBox cellCheckBox)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = Components.IndexOf(cellCheckBox);
		if (num != -1)
		{
			StiTableCellRichText stiTableCellRichText = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCellRichText") as StiTableCellRichText;
			stiTableCellRichText.ClientRectangle = cellCheckBox.ClientRectangle;
			stiTableCellRichText.Name = cellCheckBox.Name;
			stiTableCellRichText.Border = cellCheckBox.Border;
			stiTableCellRichText.BackColor = StiBrush.ToColor(cellCheckBox.Brush);
			stiTableCellRichText.ID = cellCheckBox.ID;
			stiTableCellRichText.Restrictions = cellCheckBox.Restrictions;
			stiTableCellRichText.Page = cellCheckBox.Page;
			stiTableCellRichText.Parent = cellCheckBox.Parent;
			stiTableCellRichText.CanGrow = cellCheckBox.CanGrow;
			stiTableCellRichText.CanShrink = cellCheckBox.CanShrink;
			stiTableCellRichText.Enabled = cellCheckBox.Enabled;
			stiTableCellRichText.GrowToHeight = cellCheckBox.GrowToHeight;
			stiTableCellRichText.Printable = cellCheckBox.Printable;
			if (cellCheckBox.Checked.Value.Length != 0)
			{
				stiTableCellRichText.DataColumn = cellCheckBox.Checked.Value.Substring(1, cellCheckBox.Checked.Value.Length - 2);
			}
			if (cellCheckBox.join)
			{
				stiTableCellRichText.SetJoin(cellCheckBox.Join);
				stiTableCellRichText.ParentJoin = cellCheckBox.ParentJoin;
				stiTableCellRichText.JoinCells = cellCheckBox.JoinCells;
			}
			Components.RemoveAt(num);
			Components.Insert(num, stiTableCellRichText);
			stiTableCellRichText.Select();
		}
	}

	public int GetColumnForCell(IStiTableCell cell)
	{
		return 0;
	}

	private void SetCellID(IStiTableCell cell)
	{
		cell.ID = NumberID;
		NumberID++;
	}

	public void CreateCell()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		if (Page == null || Parent == null || Page.Report == null)
		{
			return;
		}
		RectangleD clientRectangle = ClientRectangle;
		if (((RectangleD)(ref clientRectangle)).Width < 0.0)
		{
			return;
		}
		base.Height = (double)rowCount * DefaultHeightCell;
		clientRectangle = ClientRectangle;
		if (Components.Count != 0)
		{
			Components.Clear();
		}
		double num = ((RectangleD)(ref clientRectangle)).Width / (double)columnCount;
		int num2 = (int)(num / Page.GridSize);
		double num3 = ((RectangleD)(ref clientRectangle)).Height / (double)rowCount;
		int num4 = (int)(num3 / Page.GridSize);
		double num5 = (double)num2 * Page.GridSize;
		double num6 = ((RectangleD)(ref clientRectangle)).Width - num5 * (double)(columnCount - 1);
		double num7 = (double)num4 * Page.GridSize;
		double num8 = ((RectangleD)(ref clientRectangle)).Height - num7 * (double)(rowCount - 1);
		double num9 = 0.0;
		int num10 = 1;
		for (int i = 0; i < rowCount; i++)
		{
			for (int j = 0; j < columnCount; j++)
			{
				StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
				SetCellID(stiTableCell);
				stiTableCell.Name = $"{Name}_Cell{num10}";
				double num11 = ((j == columnCount - 1) ? num6 : num5);
				double num12 = ((i == rowCount - 1) ? num8 : num7);
				stiTableCell.ClientRectangle = new RectangleD(num9, num7 * (double)i, num11, num12);
				SetStyleForCell(stiTableCell, select: false);
				num9 = stiTableCell.Right;
				Components.Add(stiTableCell);
				num10++;
			}
			num9 = 0.0;
		}
		ResizeHeightCell();
		RefreshTableStyle();
	}

	private void SetStyleForCell(StiTableCell cell, bool select)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		cell.Restrictions = StiRestrictions.AllowMove | StiRestrictions.AllowSelect | StiRestrictions.AllowChange;
		cell.Brush = (StiBrush)new StiSolidBrush(Color.White);
		cell.Font = new Font("Arial", 9f);
		cell.IsSelected = select;
		cell.CanGrow = false;
		cell.GrowToHeight = false;
		cell.Page = Page;
		cell.Parent = this;
	}

	private void AddNewRows(int count)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (Components.Count == 0)
		{
			return;
		}
		int num = Components.Count + 1;
		RectangleD clientRectangle = ClientRectangle;
		((RectangleD)(ref clientRectangle)).Height = ((RectangleD)(ref clientRectangle)).Height + DefaultHeightCell * (double)count;
		base.ClientRectangle = clientRectangle;
		for (int i = 0; i < count; i++)
		{
			for (int j = 0; j < columnCount; j++)
			{
				StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
				StiComponent stiComponent = Components[Components.Count - columnCount];
				stiTableCell.Name = $"{Name}_Cell{num}";
				SetCellID(stiTableCell);
				double realLeft = ((IStiTableCell)stiComponent).GetRealLeft();
				double realWidth = ((IStiTableCell)stiComponent).GetRealWidth();
				stiTableCell.ClientRectangle = new RectangleD(realLeft, stiComponent.Bottom, realWidth, DefaultHeightCell);
				SetStyleForCell(stiTableCell, select: true);
				Components.Add(stiTableCell);
				num++;
			}
		}
		RefreshTableStyle();
	}

	private void DeleteLastRows(int count, int oldValue)
	{
		if (Components.Count == 0)
		{
			return;
		}
		double num = 0.0;
		for (int i = oldValue - count; i < oldValue && i * columnCount < Components.Count; i++)
		{
			for (int j = 0; j < columnCount; j++)
			{
				StiComponent stiComponent = Components[i * columnCount + j];
				if (((IStiTableCell)stiComponent).Merged)
				{
					StiComponent joinComponentByGuid = ((IStiTableCell)stiComponent).GetJoinComponentByGuid(((IStiTableCell)stiComponent).ParentJoin);
					((IStiTableCell)joinComponentByGuid).Join = false;
				}
			}
			num += Components[i * columnCount].Height;
		}
		int num2 = (oldValue - count) * columnCount;
		while (Components.Count > num2)
		{
			Components.RemoveAt(Components.Count - 1);
		}
		base.Height -= num;
		RefreshTableStyle();
	}

	private void AddTableNewColumns(int count, int oldValue)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		if (Components.Count == 0)
		{
			return;
		}
		RectangleD clientRectangle = ClientRectangle;
		int num = (int)(((RectangleD)(ref clientRectangle)).Width / Page.GridSize);
		int num2 = columnCount;
		int num3 = num / num2;
		int num4 = num - num3 * num2;
		double[] array = new double[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = (double)num3 * Page.GridSize;
			if (i < num4)
			{
				array[i] += Page.GridSize;
			}
		}
		array[num2 - 1] += ((RectangleD)(ref clientRectangle)).Width - (double)num * Page.GridSize;
		int num5 = 0;
		int num6 = 0;
		double num7 = 0.0;
		for (int j = 0; j < rowCount; j++)
		{
			for (int k = 0; k < oldValue; k++)
			{
				StiComponent stiComponent = Components[num5];
				stiComponent.Name = $"{Name}_Cell{num5 + 1}";
				stiComponent.ClientRectangle = new RectangleD(num7, stiComponent.Top, array[num6], stiComponent.Height);
				num7 += array[num6];
				num5++;
				num6++;
				if (k == oldValue - 1)
				{
					double realHeight = ((IStiTableCell)stiComponent).GetRealHeight();
					double realTop = ((IStiTableCell)stiComponent).GetRealTop();
					for (int l = 0; l < count; l++)
					{
						StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
						stiTableCell.Name = $"{Name}_Cell{num5 + 1}";
						SetCellID(stiTableCell);
						stiTableCell.ClientRectangle = new RectangleD(num7, realTop, array[num6], realHeight);
						SetStyleForCell(stiTableCell, select: true);
						Components.Insert(num5, stiTableCell);
						num7 += array[num6];
						num5++;
						num6++;
					}
				}
			}
			num7 = 0.0;
			num6 = 0;
		}
		for (int m = 0; m < Components.Count; m++)
		{
			StiComponent stiComponent2 = Components[m];
			if (stiComponent2 != null && ((IStiTableCell)stiComponent2).Join)
			{
				((IStiTableCell)stiComponent2).SetJoinSize();
			}
		}
		RefreshTableStyle();
	}

	private void DeleteTableColumns(int count, int oldValue)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (Components.Count == 0)
		{
			return;
		}
		int num = oldValue - count;
		int num2 = 0;
		for (int i = 0; i < rowCount; i++)
		{
			num2 += num;
			for (int j = 0; j < count; j++)
			{
				Components.RemoveAt(num2);
			}
		}
		RectangleD clientRectangle = ClientRectangle;
		int num3 = (int)(((RectangleD)(ref clientRectangle)).Width / Page.GridSize);
		int num4 = num3 / num;
		int num5 = num3 - num4 * num;
		double[] array = new double[num];
		for (int k = 0; k < num; k++)
		{
			array[k] = (double)num4 * Page.GridSize;
			if (k < num5)
			{
				array[k] += Page.GridSize;
			}
		}
		double num6 = 0.0;
		num2 = 0;
		for (int l = 0; l < rowCount; l++)
		{
			for (int m = 0; m < num; m++)
			{
				StiComponent stiComponent = Components[num2];
				stiComponent.Name = $"{Name}_Cell{num2 + 1}";
				stiComponent.Left = num6;
				stiComponent.Width = array[m];
				num6 += array[m];
				num2++;
			}
			num6 = 0.0;
		}
		for (int n = 0; n < Components.Count; n++)
		{
			StiComponent stiComponent2 = Components[n];
			if (stiComponent2 != null && ((IStiTableCell)stiComponent2).Join)
			{
				((IStiTableCell)stiComponent2).SetJoinSize();
			}
		}
		RefreshTableStyle();
	}

	public void InsertColumnToLeft(int numberColumn)
	{
		if (numberColumn < 0)
		{
			return;
		}
		if (numberColumn != 0)
		{
			int num = numberColumn;
			for (int i = 0; i < rowCount; i++)
			{
				StiComponent stiComponent = Components[num];
				StiComponent stiComponent2 = Components[num - 1];
				if (((IStiTableCell)stiComponent).Merged && ((IStiTableCell)stiComponent2).Merged)
				{
					StiComponent joinComponentByGuid = ((IStiTableCell)stiComponent).GetJoinComponentByGuid(((IStiTableCell)stiComponent).ParentJoin);
					if (((IStiTableCell)joinComponentByGuid).ContainsGuid(((IStiTableCell)stiComponent2).ID))
					{
						((IStiTableCell)joinComponentByGuid).Join = false;
					}
				}
				num += columnCount;
			}
		}
		int num2 = numberColumn;
		for (int j = 0; j < rowCount; j++)
		{
			StiComponent stiComponent3 = Components[num2];
			StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
			SetCellID(stiTableCell);
			stiTableCell.Top = ((IStiTableCell)stiComponent3).GetRealTop();
			stiTableCell.Height = ((IStiTableCell)stiComponent3).GetRealHeight();
			SetStyleForCell(stiTableCell, select: false);
			Components.Insert(num2, stiTableCell);
			num2++;
			num2 += columnCount;
		}
		columnCount++;
		ResizeWidthCellsAfterChanges();
		RefreshTableStyle();
	}

	public void InsertColumnToRight(int numberColumn)
	{
		if (numberColumn < 0)
		{
			return;
		}
		if (numberColumn != columnCount - 1)
		{
			int num = numberColumn;
			for (int i = 0; i < rowCount; i++)
			{
				StiComponent stiComponent = Components[num];
				StiComponent stiComponent2 = Components[num + 1];
				if (((IStiTableCell)stiComponent).Merged && ((IStiTableCell)stiComponent2).Merged)
				{
					StiComponent joinComponentByGuid = ((IStiTableCell)stiComponent).GetJoinComponentByGuid(((IStiTableCell)stiComponent).ParentJoin);
					if (((IStiTableCell)joinComponentByGuid).ContainsGuid(((IStiTableCell)stiComponent2).ID))
					{
						((IStiTableCell)joinComponentByGuid).Join = false;
					}
				}
				num += columnCount;
			}
		}
		int num2 = numberColumn;
		for (int j = 0; j < rowCount; j++)
		{
			StiComponent stiComponent3 = Components[num2];
			StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
			SetCellID(stiTableCell);
			stiTableCell.Top = ((IStiTableCell)stiComponent3).GetRealTop();
			stiTableCell.Height = ((IStiTableCell)stiComponent3).GetRealHeight();
			SetStyleForCell(stiTableCell, select: false);
			Components.Insert(num2 + 1, stiTableCell);
			num2++;
			num2 += columnCount;
		}
		columnCount++;
		ResizeWidthCellsAfterChanges();
		RefreshTableStyle();
	}

	public void InsertRowAbove(int numberRow)
	{
		if (numberRow < 0)
		{
			return;
		}
		if (numberRow != 0)
		{
			int num = numberRow * columnCount;
			for (int i = 0; i < columnCount; i++)
			{
				StiComponent stiComponent = Components[num];
				StiComponent stiComponent2 = Components[num - columnCount];
				if (((IStiTableCell)stiComponent).Merged && ((IStiTableCell)stiComponent2).Merged)
				{
					StiComponent joinComponentByGuid = ((IStiTableCell)stiComponent).GetJoinComponentByGuid(((IStiTableCell)stiComponent).ParentJoin);
					if (((IStiTableCell)joinComponentByGuid).ContainsGuid(((IStiTableCell)stiComponent2).ID))
					{
						((IStiTableCell)joinComponentByGuid).Join = false;
					}
				}
				num++;
			}
		}
		int num2 = numberRow * columnCount;
		for (int j = 0; j < columnCount; j++)
		{
			StiComponent stiComponent3 = Components[num2 + j];
			StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
			SetCellID(stiTableCell);
			stiTableCell.Left = ((IStiTableCell)stiComponent3).GetRealLeft();
			stiTableCell.Width = ((IStiTableCell)stiComponent3).GetRealWidth();
			stiTableCell.Height = DefaultHeightCell;
			SetStyleForCell(stiTableCell, select: false);
			Components.Insert(num2, stiTableCell);
			num2++;
		}
		rowCount++;
		ResizeHeightCellsAfterChanges(DefaultHeightCell);
		RefreshTableStyle();
	}

	public void InsertRowBelow(int numberRow)
	{
		if (numberRow < 0)
		{
			return;
		}
		if (numberRow != rowCount)
		{
			int num = numberRow * columnCount;
			for (int i = 0; i < columnCount; i++)
			{
				StiComponent stiComponent = Components[num];
				StiComponent stiComponent2 = Components[num - columnCount];
				if (((IStiTableCell)stiComponent).Merged && ((IStiTableCell)stiComponent2).Merged)
				{
					StiComponent joinComponentByGuid = ((IStiTableCell)stiComponent).GetJoinComponentByGuid(((IStiTableCell)stiComponent).ParentJoin);
					if (((IStiTableCell)joinComponentByGuid).ContainsGuid(((IStiTableCell)stiComponent2).ID))
					{
						((IStiTableCell)joinComponentByGuid).Join = false;
					}
				}
				num++;
			}
		}
		int num2 = numberRow * columnCount;
		for (int j = 0; j < columnCount; j++)
		{
			StiComponent stiComponent3 = ((numberRow != rowCount) ? Components[num2 + j] : Components[num2 - columnCount]);
			StiTableCell stiTableCell = StiActivator.CreateObject("Stimulsoft.Report.Components.Table.StiTableCell") as StiTableCell;
			SetCellID(stiTableCell);
			stiTableCell.Left = ((IStiTableCell)stiComponent3).GetRealLeft();
			stiTableCell.Width = ((IStiTableCell)stiComponent3).GetRealWidth();
			stiTableCell.Height = DefaultHeightCell;
			SetStyleForCell(stiTableCell, select: false);
			Components.Insert(num2, stiTableCell);
			num2++;
		}
		rowCount++;
		ResizeHeightCellsAfterChanges(DefaultHeightCell);
		RefreshTableStyle();
	}

	public List<StiComponent> DeleteRows(int firstRow, int lastRow)
	{
		int num = lastRow - firstRow + 1;
		if (num == rowCount)
		{
			return null;
		}
		List<StiComponent> list = new List<StiComponent>();
		int index = firstRow * columnCount;
		double num2 = 0.0;
		for (int i = firstRow; i <= lastRow; i++)
		{
			for (int j = 0; j < columnCount; j++)
			{
				StiComponent stiComponent = Components[index];
				if (stiComponent != null)
				{
					if (((IStiTableCell)stiComponent).Merged)
					{
						((IStiTableCell)((IStiTableCell)stiComponent).GetJoinComponentByGuid(((IStiTableCell)stiComponent).ParentJoin)).Join = false;
					}
					if (j == 0)
					{
						num2 += ((IStiTableCell)stiComponent).GetRealHeight();
					}
					Components.RemoveAt(index);
					list.Add(stiComponent);
				}
			}
		}
		rowCount -= num;
		ResizeHeightCellsAfterChanges(num2 * -1.0);
		RefreshTableStyle();
		return list;
	}

	public List<StiComponent> DeleteColumns(int firstColumn, int lastColumn)
	{
		int num = lastColumn - firstColumn + 1;
		if (num == columnCount)
		{
			return null;
		}
		List<StiComponent> list = new List<StiComponent>();
		int num2 = firstColumn;
		for (int i = 0; i < rowCount; i++)
		{
			for (int j = firstColumn; j <= lastColumn; j++)
			{
				StiComponent stiComponent = Components[num2];
				if (stiComponent != null)
				{
					if (((IStiTableCell)stiComponent).Merged)
					{
						((IStiTableCell)((IStiTableCell)stiComponent).GetJoinComponentByGuid(((IStiTableCell)stiComponent).ParentJoin)).Join = false;
					}
					Components.RemoveAt(num2);
					list.Add(stiComponent);
				}
			}
			num2 += columnCount - num;
		}
		columnCount -= num;
		ResizeWidthCellsAfterChanges();
		RefreshTableStyle();
		return list;
	}

	public void DistributeRows()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (IsConverted || (Parent == null && Page == null && Components.Count == 0))
		{
			return;
		}
		RectangleD clientRectangle = ClientRectangle;
		double num = ((RectangleD)(ref clientRectangle)).Height / Page.GridSize;
		double num2 = num / (double)rowCount;
		int num3 = (int)(num - num2 * (double)rowCount);
		double[] array = new double[rowCount];
		for (int i = 0; i < rowCount; i++)
		{
			array[i] = num2 * Page.GridSize;
			if (i < num3)
			{
				array[i] += Page.GridSize;
			}
		}
		array[^1] += ((RectangleD)(ref clientRectangle)).Height - num * Page.GridSize;
		double num4 = 0.0;
		int num5 = 0;
		for (int j = 0; j < rowCount; j++)
		{
			for (int k = 0; k < columnCount; k++)
			{
				StiComponent stiComponent = Components[num5];
				if (stiComponent != null)
				{
					stiComponent.ClientRectangle = new RectangleD(((IStiTableCell)stiComponent).GetRealLeft(), num4, ((IStiTableCell)stiComponent).GetRealWidth(), array[j]);
					if (((IStiTableCell)stiComponent).Join)
					{
						((IStiTableCell)stiComponent).SetJoinSize();
					}
					num5++;
				}
			}
			num4 += array[j];
		}
	}

	public void DistributeColumns()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (IsConverted || (Parent == null && Page == null && Components.Count == 0))
		{
			return;
		}
		RectangleD clientRectangle = ClientRectangle;
		double num = ((RectangleD)(ref clientRectangle)).Width / Page.GridSize;
		double num2 = num / (double)columnCount;
		int num3 = (int)(num - num2 * (double)columnCount);
		double[] array = new double[columnCount];
		for (int i = 0; i < columnCount; i++)
		{
			array[i] = num2 * Page.GridSize;
			if (i < num3)
			{
				array[i] += Page.GridSize;
			}
		}
		array[^1] += ((RectangleD)(ref clientRectangle)).Width - num * Page.GridSize;
		double num4 = 0.0;
		int num5 = 0;
		for (int j = 0; j < rowCount; j++)
		{
			for (int k = 0; k < columnCount; k++)
			{
				StiComponent stiComponent = Components[num5];
				if (stiComponent != null)
				{
					stiComponent.ClientRectangle = new RectangleD(num4, ((IStiTableCell)stiComponent).GetRealTop(), array[k], ((IStiTableCell)stiComponent).GetRealHeight());
					num4 += array[k];
					if (((IStiTableCell)stiComponent).Join)
					{
						((IStiTableCell)stiComponent).SetJoinSize();
					}
					num5++;
				}
			}
			num4 = 0.0;
		}
	}

	public void AutoSizeCells()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		if (IsConverted || Components.Count == 0 || Page == null)
		{
			return;
		}
		RectangleD clientRectangle = ClientRectangle;
		int num = (int)(((RectangleD)(ref clientRectangle)).Width / Page.GridSize);
		int num2 = num / columnCount;
		int num3 = num - num2 * columnCount;
		double[] array = new double[columnCount];
		for (int i = 0; i < columnCount; i++)
		{
			array[i] = (double)num2 * Page.GridSize;
			if (i < num3)
			{
				array[i] += Page.GridSize;
			}
		}
		array[^1] += ((RectangleD)(ref clientRectangle)).Width - (double)num * Page.GridSize;
		int num4 = (int)(((RectangleD)(ref clientRectangle)).Height / Page.GridSize);
		num2 = num4 / rowCount;
		int num5 = num4 - num2 * rowCount;
		double[] array2 = new double[rowCount];
		for (int j = 0; j < rowCount; j++)
		{
			array2[j] = (double)num2 * Page.GridSize;
			if (j < num5)
			{
				array2[j] += Page.GridSize;
			}
		}
		array2[^1] += ((RectangleD)(ref clientRectangle)).Height - (double)num4 * Page.GridSize;
		double num6 = 0.0;
		double num7 = 0.0;
		int num8 = 0;
		for (int k = 0; k < rowCount; k++)
		{
			for (int l = 0; l < columnCount; l++)
			{
				StiComponent stiComponent = Components[num8];
				stiComponent.ClientRectangle = new RectangleD(num6, num7, array[l], array2[k]);
				num6 += array[l];
				num8++;
			}
			num6 = 0.0;
			num7 += array2[k];
		}
	}

	private void ResizeWidthCellsAfterChanges()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		if (IsConverted)
		{
			return;
		}
		RectangleD clientRectangle = ClientRectangle;
		int num = (int)(((RectangleD)(ref clientRectangle)).Width / Page.GridSize);
		int num2 = num / columnCount;
		int num3 = num - num2 * columnCount;
		double[] array = new double[columnCount];
		for (int i = 0; i < columnCount; i++)
		{
			array[i] = (double)num2 * Page.GridSize;
			if (i < num3)
			{
				array[i] += Page.GridSize;
			}
		}
		array[columnCount - 1] += ((RectangleD)(ref clientRectangle)).Width - (double)num * Page.GridSize;
		int num4 = 0;
		int num5 = 0;
		double num6 = 0.0;
		for (int j = 0; j < rowCount; j++)
		{
			for (int k = 0; k < columnCount; k++)
			{
				StiComponent stiComponent = Components[num4];
				stiComponent.Name = $"{Name}_Cell{num4 + 1}";
				if (((IStiTableCell)stiComponent).Join)
				{
					stiComponent.Left = num6;
					stiComponent.Width = array[num5];
					((IStiTableCell)stiComponent).SetJoinSize();
				}
				else
				{
					stiComponent.ClientRectangle = new RectangleD(num6, stiComponent.Top, array[num5], stiComponent.Height);
				}
				num6 += array[num5];
				num4++;
				num5++;
			}
			num5 = 0;
			num6 = 0.0;
		}
	}

	private void ResizeHeightCellsAfterChanges(double changeHeight)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		if (IsConverted)
		{
			return;
		}
		RectangleD clientRectangle = ClientRectangle;
		((RectangleD)(ref clientRectangle)).Height = ((RectangleD)(ref clientRectangle)).Height + changeHeight;
		double num = 0.0;
		int num2 = 0;
		StiComponent stiComponent = null;
		for (int i = 0; i < rowCount; i++)
		{
			for (int j = 0; j < columnCount; j++)
			{
				stiComponent = Components[num2];
				stiComponent.Name = $"{Name}_Cell{num2 + 1}";
				if (((IStiTableCell)stiComponent).Join)
				{
					stiComponent.Height = ((IStiTableCell)stiComponent).GetRealHeightAfterInsertRows();
					stiComponent.Top = num;
					((IStiTableCell)stiComponent).SetJoinSize();
				}
				else
				{
					stiComponent.Top = num;
				}
				num2++;
			}
			if (stiComponent != null)
			{
				num += ((IStiTableCell)stiComponent).GetRealHeight();
			}
		}
		base.ClientRectangle = clientRectangle;
	}

	private void ResizeWidthCell(double oldWidth)
	{
		if (IsConverted || Components.Count == 0 || Page == null)
		{
			return;
		}
		double num = base.Width - oldWidth;
		double num2 = num / (double)columnCount;
		int num3 = (int)(num2 / Page.GridSize);
		double num4 = Page.GridSize * (double)num3;
		double num5 = num - num4 * (double)(columnCount - 1);
		for (int i = 0; i < columnCount; i++)
		{
			for (int j = 0; j < rowCount; j++)
			{
				StiComponent stiComponent = Components[j * columnCount + i];
				if (i == columnCount - 1)
				{
					stiComponent.Left += (double)i * num4;
					stiComponent.Width += num5;
				}
				else
				{
					stiComponent.Left += (double)i * num4;
					stiComponent.Width += num4;
				}
			}
		}
	}

	private void ResizeHeightCell()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (IsConverted || (Page == null && Components.Count == 0))
		{
			return;
		}
		RectangleD clientRectangle = ClientRectangle;
		if (rowCount == 1)
		{
			for (int i = 0; i < columnCount; i++)
			{
				StiComponent stiComponent = Components[i];
				stiComponent.Top = 0.0;
				stiComponent.Height = ((RectangleD)(ref clientRectangle)).Height;
			}
			return;
		}
		double[] array = new double[rowCount];
		int num = (int)(((RectangleD)(ref clientRectangle)).Height / Page.GridSize);
		int num2 = num / rowCount;
		int num3 = num - num2 * rowCount;
		double num4 = (double)num2 * Page.GridSize;
		for (int j = 0; j < rowCount; j++)
		{
			array[j] = num4;
			if (j < num3)
			{
				array[j] += Page.GridSize;
			}
		}
		array[rowCount - 1] += ((RectangleD)(ref clientRectangle)).Height - Page.GridSize * (double)num;
		double num5 = 0.0;
		for (int k = 0; k < rowCount; k++)
		{
			for (int l = 0; l < columnCount; l++)
			{
				StiComponent stiComponent2 = Components[k * columnCount + l];
				if (((IStiTableCell)stiComponent2).Join)
				{
					StiComponent joinComponentByIndex = ((IStiTableCell)stiComponent2).GetJoinComponentByIndex(0);
					stiComponent2.Left = joinComponentByIndex.Left;
					stiComponent2.Top = joinComponentByIndex.Top;
					stiComponent2.Height = num5 + array[k] - joinComponentByIndex.Top;
				}
				else
				{
					stiComponent2.Top = num5;
					stiComponent2.Height = array[k];
				}
			}
			num5 += array[k];
		}
	}

	internal StiDataBand StartRenderTableBand(ref Hashtable newTableComponents)
	{
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		if (!Report.IsInteractionRendering && !Enabled)
		{
			return null;
		}
		StiPage stiPage = Parent as StiPage;
		StiDataBand stiDataBand = Parent as StiDataBand;
		StiPanel stiPanel = Parent as StiPanel;
		if (DockableTable)
		{
			if (stiDataBand != null)
			{
				StiPanel stiPanel2 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPanel") as StiPanel;
				stiPanel2.Name = Name + "_Panel";
				stiPanel2.CanBreak = true;
				stiPanel2.ClientRectangle = stiDataBand.ClientRectangle;
				stiPanel2.DockStyle = StiDockStyle.Fill;
				int index = stiDataBand.Components.IndexOf(this);
				stiDataBand.Components.Insert(index, stiPanel2);
				return StartRenderTable(stiPanel2, 0, ref newTableComponents);
			}
			if (stiPage != null)
			{
				int index2 = Page.Components.IndexOf(this);
				return StartRenderTable(stiPage, index2, ref newTableComponents);
			}
			if (stiPanel != null)
			{
				int index3 = stiPanel.Components.IndexOf(this);
				return StartRenderTable(stiPanel, index3, ref newTableComponents);
			}
		}
		else
		{
			if (stiDataBand != null)
			{
				StiPanel stiPanel3 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPanel") as StiPanel;
				stiPanel3.Name = Name + "_Panel";
				stiPanel3.CanBreak = true;
				stiPanel3.CanGrow = true;
				stiPanel3.ClientRectangle = ClientRectangle;
				int index4 = stiDataBand.Components.IndexOf(this);
				stiDataBand.Components.Insert(index4, stiPanel3);
				return StartRenderTable(stiPanel3, 0, ref newTableComponents);
			}
			if (stiPage != null)
			{
				int index5 = Page.Components.IndexOf(this);
				StiDataBand stiDataBand2 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiDataBand") as StiDataBand;
				stiDataBand2.Name = Name + "_Band";
				stiDataBand2.Left = Left;
				stiDataBand2.Top = Top;
				Page.Components.Insert(index5, stiDataBand2);
				StiPanel stiPanel4 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPanel") as StiPanel;
				stiPanel4.Name = Name + "_Panel";
				stiPanel4.CanBreak = true;
				stiPanel4.CanGrow = true;
				stiPanel4.ClientRectangle = ClientRectangle;
				stiPanel4.Top = 0.0;
				stiDataBand2.Components.Add(stiPanel4);
				return StartRenderTable(stiPanel4, 0, ref newTableComponents);
			}
			if (stiPanel != null)
			{
				int index6 = stiPanel.Parent.Components.IndexOf(stiPanel);
				return StartRenderTable(stiPanel, index6, ref newTableComponents);
			}
		}
		return null;
	}

	private StiDataBand StartRenderTable(StiContainer parentContainer, int index, ref Hashtable newTableComponents)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Expected O, but got Unknown
		//IL_12ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1311: Unknown result type (might be due to invalid IL or missing references)
		//IL_131b: Expected O, but got Unknown
		StiPage page = Page;
		ArrayList arrayList = null;
		if (newTableComponents.ContainsKey(page))
		{
			arrayList = newTableComponents[page] as ArrayList;
		}
		else
		{
			arrayList = new ArrayList();
			newTableComponents.Add(page, arrayList);
		}
		base.Enabled = false;
		int num = 0;
		StiDataBand stiDataBand = null;
		RectangleD clientRectangle = ClientRectangle;
		double num2 = (DockableTable ? ((RectangleD)(ref clientRectangle)).Y : 0.0);
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		int num6 = Components.Count / columnCount;
		ReverseCells(RightToLeft);
		for (int i = 0; i < rowCount; i++)
		{
			for (int j = 0; j < columnCount; j++)
			{
				(Components[columnCount * i + j] as IStiTableCell).Column = j;
				((IStiTableCell)Components[columnCount * i + j]).TableTag = this;
			}
		}
		bool flag = false;
		for (int num7 = Parent.Components.IndexOf(this) - 1; num7 >= 0; num7--)
		{
			StiComponent stiComponent = Parent.Components[num7];
			if (!(stiComponent is StiHeaderBand) && !(stiComponent is StiChildBand) && !(stiComponent is StiEmptyBand))
			{
				if (stiComponent is StiGroupHeaderBand)
				{
					flag = true;
				}
				break;
			}
		}
		if (headerRowsCount > 0)
		{
			for (int k = 0; k < headerRowsCount; k++)
			{
				num3 += ((IStiTableCell)Components[k * columnCount]).GetRealHeight();
			}
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			StiDynamicBand stiDynamicBand = null;
			if (flag)
			{
				stiDynamicBand = StiActivator.CreateObject("Stimulsoft.Report.Components.StiGroupHeaderBand") as StiGroupHeaderBand;
				((StiGroupHeaderBand)stiDynamicBand).GroupHeaderBandInfoV2.IsTableGroupHeader = true;
				stiDynamicBand.Name = Name + "_GrHd";
				((StiGroupHeaderBand)stiDynamicBand).PrintOnAllPages = HeaderPrintOnAllPages;
			}
			else
			{
				stiDynamicBand = StiActivator.CreateObject("Stimulsoft.Report.Components.StiHeaderBand") as StiHeaderBand;
				stiDynamicBand.Name = Name + "_Hd";
				((StiHeaderBand)stiDynamicBand).HeaderBandInfoV2.IsTableHeader = true;
				((StiHeaderBand)stiDynamicBand).PrintOnAllPages = HeaderPrintOnAllPages;
				((StiHeaderBand)stiDynamicBand).PrintIfEmpty = HeaderPrintIfEmpty;
				((StiHeaderBand)stiDynamicBand).PrintOnEvenOddPages = HeaderPrintOnEvenOddPages;
			}
			stiDynamicBand.Height = num3;
			stiDynamicBand.ClientRectangle = new RectangleD(((RectangleD)(ref clientRectangle)).X, num2, ((RectangleD)(ref clientRectangle)).Width, num3);
			stiDynamicBand.Border = Border;
			stiDynamicBand.Brush = Brush;
			stiDynamicBand.Page = Page;
			stiDynamicBand.Parent = Page;
			stiDynamicBand.CanGrow = HeaderCanGrow;
			stiDynamicBand.CanShrink = HeaderCanShrink;
			stiDynamicBand.CanBreak = HeaderCanBreak;
			stiDynamicBand.PrintAtBottom = HeaderPrintAtBottom;
			stiDynamicBand.PrintOn = HeaderPrintOn;
			stiDynamicBand.Conditions = Conditions.Clone() as StiConditionsCollection;
			CopyEventHandlersToComponent(stiDynamicBand, onlyBeforePrint: true);
			double num8 = 0.0;
			for (int l = 0; l < headerRowsCount; l++)
			{
				double realHeight = ((IStiTableCell)Components[num]).GetRealHeight();
				StiPanel stiPanel = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPanel") as StiPanel;
				stiPanel.ClientRectangle = new RectangleD(((RectangleD)(ref clientRectangle)).X, num8, ((RectangleD)(ref clientRectangle)).Width, realHeight);
				stiPanel.Name = stiDynamicBand.Name + "_HPnl" + l;
				stiPanel.CanGrow = true;
				stiPanel.CanShrink = true;
				stiPanel.DockStyle = StiDockStyle.Top;
				stiDynamicBand.Components.Add(stiPanel);
				num8 += realHeight;
				num += columnCount;
				for (int m = columnCount * l; m < num; m++)
				{
					StiComponent stiComponent2 = Components[m];
					bool flag2 = false;
					StiComponent stiComponent3 = null;
					if (((IStiTableCell)stiComponent2).ParentJoin != -1)
					{
						int rowNumber = 0;
						stiComponent3 = GetParentJoin(((IStiTableCell)stiComponent2).ParentJoin, ref rowNumber);
						if (hashtable.ContainsKey(stiComponent3))
						{
							if (l != (int)hashtable[stiComponent3])
							{
								StiText stiText = StiActivator.CreateObject("Stimulsoft.Report.Components.StiText") as StiText;
								stiText.Page = stiComponent2.Page;
								stiText.Conditions = stiComponent3.Conditions;
								CopyEventHandlersToComponent(stiText, onlyBeforePrint: true, stiComponent3);
								stiText.SetPaintRectangle(stiComponent2.GetPaintRectangle());
								stiText.ClientRectangle = new RectangleD(((IStiTableCell)stiComponent2).GetRealLeft(), 0.0, ((IStiTableCell)stiComponent2).GetRealWidth(), ((IStiTableCell)stiComponent2).GetRealHeight());
								stiText.Name = stiComponent2.Name + "_Emp";
								stiText.GrowToHeight = true;
								stiText.Brush = (StiBrush)new StiEmptyBrush();
								stiText.DockStyle = ((IStiTableCell)stiComponent2).CellDockStyle;
								stiPanel.Components.Add(stiText);
								arrayList.Add(stiText);
								if (stiComponent2 == stiComponent3)
								{
									((IStiTableCell)hashtable2[stiComponent3]).ParentJoinCell = stiText;
								}
							}
							continue;
						}
						hashtable.Add(stiComponent3, l);
						stiComponent2 = stiComponent3;
						flag2 = true;
					}
					stiComponent2.Top = 0.0;
					switch (((IStiTableCell)stiComponent2).CellType)
					{
					case StiTablceCellType.Image:
					{
						StiImage stiImage = (StiImage)((StiTableCellImage)stiComponent2).Clone(cloneProperties: true);
						stiImage.Restrictions ^= StiRestrictions.AllowDelete;
						stiImage.DockStyle = ((StiTableCellImage)stiComponent2).CellDockStyle;
						SetInteraction(stiComponent2);
						stiPanel.Components.Add(stiImage);
						arrayList.Add(stiImage);
						if (flag2)
						{
							stiImage.CanGrow = true;
							if (!IsEqualRows(stiComponent2, Components[m]))
							{
								stiImage.Height = ((IStiTableCell)stiComponent2).GetRealHeight();
							}
							hashtable2.Add(stiComponent3, stiImage);
						}
						break;
					}
					case StiTablceCellType.Text:
					{
						StiText stiText2 = (StiText)((StiTableCell)stiComponent2).Clone(cloneProperties: true);
						stiText2.Restrictions ^= StiRestrictions.AllowDelete;
						stiText2.DockStyle = ((StiTableCell)stiComponent2).CellDockStyle;
						SetInteraction(stiComponent2);
						stiPanel.Components.Add(stiText2);
						arrayList.Add(stiText2);
						if (flag2)
						{
							stiText2.CanGrow = true;
							if (!IsEqualRows(stiComponent2, Components[m]))
							{
								stiText2.Height = ((IStiTableCell)stiComponent2).GetRealHeight();
							}
							hashtable2.Add(stiComponent3, stiText2);
						}
						break;
					}
					case StiTablceCellType.CheckBox:
					{
						StiCheckBox stiCheckBox = (StiCheckBox)((StiTableCellCheckBox)stiComponent2).Clone(cloneProperties: true);
						stiCheckBox.Restrictions ^= StiRestrictions.AllowDelete;
						stiCheckBox.DockStyle = ((StiTableCellCheckBox)stiComponent2).CellDockStyle;
						SetInteraction(stiComponent2);
						stiPanel.Components.Add(stiCheckBox);
						arrayList.Add(stiCheckBox);
						if (flag2)
						{
							stiCheckBox.CanGrow = true;
							if (!IsEqualRows(stiComponent2, Components[m]))
							{
								stiCheckBox.Height = ((IStiTableCell)stiComponent2).GetRealHeight();
							}
							hashtable2.Add(stiComponent3, stiCheckBox);
						}
						break;
					}
					case StiTablceCellType.RichText:
					{
						StiRichText stiRichText = (StiRichText)((StiTableCellRichText)stiComponent2).Clone(cloneProperties: true);
						stiRichText.Restrictions ^= StiRestrictions.AllowDelete;
						stiRichText.DockStyle = ((StiTableCellRichText)stiComponent2).CellDockStyle;
						SetInteraction(stiComponent2);
						stiPanel.Components.Add(stiRichText);
						arrayList.Add(stiRichText);
						if (flag2)
						{
							stiRichText.CanGrow = true;
							if (!IsEqualRows(stiComponent2, Components[m]))
							{
								stiRichText.Height = ((IStiTableCell)stiComponent2).GetRealHeight();
							}
							hashtable2.Add(stiComponent3, stiRichText);
						}
						break;
					}
					}
				}
			}
			parentContainer.Components.Insert(index, stiDynamicBand);
			arrayList.Add(stiDynamicBand);
			index++;
			num2 += num3 + 1.0;
		}
		else if (flag && footerRowsCount > 0)
		{
			StiGroupHeaderBand stiGroupHeaderBand = StiActivator.CreateObject("Stimulsoft.Report.Components.StiGroupHeaderBand") as StiGroupHeaderBand;
			stiGroupHeaderBand.Height = 0.0;
			stiGroupHeaderBand.ClientRectangle = new RectangleD(((RectangleD)(ref clientRectangle)).X, num2, ((RectangleD)(ref clientRectangle)).Width, 0.0);
			stiGroupHeaderBand.Name = Name + "_GrHdEmp";
			stiGroupHeaderBand.Page = Page;
			stiGroupHeaderBand.Parent = Page;
			stiGroupHeaderBand.PrintAtBottom = HeaderPrintAtBottom;
			parentContainer.Components.Insert(index, stiGroupHeaderBand);
			arrayList.Add(stiGroupHeaderBand);
			index++;
			num2 += 1.0;
		}
		if (rowCount - headerRowsCount - footerRowsCount > 0)
		{
			IsConverted = true;
			stiDataBand = (StiDataBand)Clone(cloneProperties: true, cloneComponents: false);
			IsConverted = false;
			stiDataBand.Components.Clear();
			SetFilter(stiDataBand);
			for (int n = headerRowsCount; n < num6 - footerRowsCount; n++)
			{
				num5 += ((IStiTableCell)Components[n * columnCount]).GetRealHeight();
			}
			stiDataBand.Parent = Parent;
			stiDataBand.Page = Page;
			stiDataBand.Enabled = true;
			stiDataBand.ClientRectangle = new RectangleD(((RectangleD)(ref clientRectangle)).X, num2, ((RectangleD)(ref clientRectangle)).Width, num5);
			stiDataBand.Name = Name + "_DB";
			stiDataBand.MasterComponent = MasterComponent;
			Hashtable hashtable3 = new Hashtable();
			Hashtable hashtable4 = new Hashtable();
			int num9 = headerRowsCount * columnCount;
			double num10 = 0.0;
			for (int num11 = 0; num11 < rowCount - headerRowsCount - footerRowsCount; num11++)
			{
				double realHeight2 = ((IStiTableCell)Components[num9]).GetRealHeight();
				StiPanel stiPanel2 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPanel") as StiPanel;
				stiPanel2.ClientRectangle = new RectangleD(((RectangleD)(ref clientRectangle)).X, num10, ((RectangleD)(ref clientRectangle)).Width, realHeight2);
				stiPanel2.Name = stiDataBand.Name + "_DPnl" + num11;
				stiPanel2.CanGrow = true;
				stiPanel2.CanBreak = true;
				stiPanel2.CanShrink = true;
				stiPanel2.DockStyle = StiDockStyle.Top;
				stiDataBand.Components.Add(stiPanel2);
				num10 += realHeight2;
				for (int num12 = num9; num12 < num9 + columnCount; num12++)
				{
					StiComponent stiComponent4 = Components[num12];
					bool flag3 = false;
					StiComponent stiComponent5 = null;
					if (((IStiTableCell)stiComponent4).ParentJoin != -1)
					{
						int rowNumber2 = 0;
						stiComponent5 = GetParentJoin(((IStiTableCell)stiComponent4).ParentJoin, ref rowNumber2);
						if (hashtable3.ContainsKey(stiComponent5))
						{
							if (num11 != (int)hashtable3[stiComponent5])
							{
								StiText stiText3 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiText") as StiText;
								stiText3.Page = stiComponent4.Page;
								stiText3.Conditions = stiComponent5.Conditions;
								CopyEventHandlersToComponent(stiText3, onlyBeforePrint: true, stiComponent5);
								stiText3.SetPaintRectangle(stiComponent4.GetPaintRectangle());
								stiText3.ClientRectangle = new RectangleD(((IStiTableCell)stiComponent4).GetRealLeft(), 0.0, ((IStiTableCell)stiComponent4).GetRealWidth(), ((IStiTableCell)stiComponent4).GetRealHeight());
								stiText3.Name = stiComponent4.Name + "_Emp";
								stiText3.GrowToHeight = true;
								stiText3.Brush = (StiBrush)new StiEmptyBrush();
								stiText3.DockStyle = ((IStiTableCell)stiComponent4).CellDockStyle;
								stiPanel2.Components.Add(stiText3);
								arrayList.Add(stiText3);
								if (stiComponent4 == stiComponent5)
								{
									((IStiTableCell)hashtable4[stiComponent5]).ParentJoinCell = stiText3;
								}
							}
							continue;
						}
						hashtable3.Add(stiComponent5, num11);
						stiComponent4 = stiComponent5;
						flag3 = true;
					}
					stiComponent4.Top = 0.0;
					switch (((IStiTableCell)stiComponent4).CellType)
					{
					case StiTablceCellType.Image:
					{
						StiImage stiImage2 = (StiImage)((StiTableCellImage)stiComponent4).Clone(cloneProperties: true);
						stiImage2.Restrictions ^= StiRestrictions.AllowDelete;
						stiImage2.DockStyle = ((StiTableCellImage)stiComponent4).CellDockStyle;
						SetInteraction(stiComponent4);
						stiPanel2.Components.Add(stiImage2);
						arrayList.Add(stiImage2);
						if (flag3)
						{
							stiImage2.CanGrow = true;
							if (!IsEqualRows(stiComponent4, Components[num12]))
							{
								stiImage2.Height = ((IStiTableCell)stiComponent4).GetRealHeight();
							}
							hashtable4.Add(stiComponent5, stiImage2);
						}
						break;
					}
					case StiTablceCellType.Text:
					{
						StiText stiText4 = (StiText)((StiTableCell)stiComponent4).Clone(cloneProperties: true);
						stiText4.Restrictions ^= StiRestrictions.AllowDelete;
						stiText4.DockStyle = ((StiTableCell)stiComponent4).CellDockStyle;
						SetInteraction(stiComponent4);
						stiPanel2.Components.Add(stiText4);
						arrayList.Add(stiText4);
						if (flag3)
						{
							stiText4.CanGrow = true;
							if (!IsEqualRows(stiComponent4, Components[num12]))
							{
								stiPanel2.CanGrow = false;
							}
							hashtable4.Add(stiComponent5, stiText4);
						}
						break;
					}
					case StiTablceCellType.CheckBox:
					{
						StiCheckBox stiCheckBox2 = (StiCheckBox)((StiTableCellCheckBox)stiComponent4).Clone(cloneProperties: true);
						stiCheckBox2.Restrictions ^= StiRestrictions.AllowDelete;
						stiCheckBox2.DockStyle = ((StiTableCellCheckBox)stiComponent4).CellDockStyle;
						SetInteraction(stiComponent4);
						stiPanel2.Components.Add(stiCheckBox2);
						arrayList.Add(stiCheckBox2);
						if (flag3)
						{
							stiCheckBox2.CanGrow = true;
							if (!IsEqualRows(stiComponent4, Components[num12]))
							{
								stiCheckBox2.Height = ((IStiTableCell)stiComponent4).GetRealHeight();
							}
							hashtable4.Add(stiComponent5, stiCheckBox2);
						}
						break;
					}
					case StiTablceCellType.RichText:
					{
						StiRichText stiRichText2 = (StiRichText)((StiTableCellRichText)stiComponent4).Clone(cloneProperties: true);
						stiRichText2.Restrictions ^= StiRestrictions.AllowDelete;
						stiRichText2.DockStyle = ((StiTableCellRichText)stiComponent4).CellDockStyle;
						SetInteraction(stiComponent4);
						stiPanel2.Components.Add(stiRichText2);
						arrayList.Add(stiRichText2);
						if (flag3)
						{
							stiRichText2.CanGrow = true;
							if (!IsEqualRows(stiComponent4, Components[num12]))
							{
								stiRichText2.Height = ((IStiTableCell)stiComponent4).GetRealHeight();
							}
							hashtable4.Add(stiComponent5, stiRichText2);
						}
						break;
					}
					}
				}
				num9 += columnCount;
			}
			parentContainer.Components.Insert(index, stiDataBand);
			arrayList.Add(stiDataBand);
			num2 += num5 + 0.8;
			index++;
		}
		if (footerRowsCount > 0)
		{
			Hashtable hashtable5 = new Hashtable();
			Hashtable hashtable6 = new Hashtable();
			int num13 = Components.Count - footerRowsCount * columnCount;
			for (int num14 = num6 - footerRowsCount; num14 < num6; num14++)
			{
				num4 += ((IStiTableCell)Components[num14 * columnCount]).GetRealHeight();
			}
			StiDynamicBand stiDynamicBand2 = null;
			if (flag)
			{
				stiDynamicBand2 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiGroupFooterBand") as StiGroupFooterBand;
				((StiGroupFooterBand)stiDynamicBand2).GroupFooterBandInfoV2.IsTableGroupFooter = true;
				stiDynamicBand2.Name = Name + "_GrFt";
			}
			else
			{
				stiDynamicBand2 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiFooterBand") as StiFooterBand;
				stiDynamicBand2.Name = Name + "_Ft";
				((StiFooterBand)stiDynamicBand2).FooterBandInfoV2.IsTableFooter = true;
				((StiFooterBand)stiDynamicBand2).PrintOnAllPages = FooterPrintOnAllPages;
				((StiFooterBand)stiDynamicBand2).PrintIfEmpty = FooterPrintIfEmpty;
				((StiFooterBand)stiDynamicBand2).PrintOnEvenOddPages = FooterPrintOnEvenOddPages;
			}
			stiDynamicBand2.ClientRectangle = new RectangleD(((RectangleD)(ref clientRectangle)).X, num2, ((RectangleD)(ref clientRectangle)).Width, num4);
			stiDynamicBand2.Border = Border;
			stiDynamicBand2.Brush = Brush;
			stiDynamicBand2.Interaction = null;
			stiDynamicBand2.Page = Page;
			stiDynamicBand2.Parent = Page;
			stiDynamicBand2.CanGrow = FooterCanGrow;
			stiDynamicBand2.CanShrink = FooterCanShrink;
			stiDynamicBand2.CanBreak = FooterCanBreak;
			stiDynamicBand2.PrintAtBottom = FooterPrintAtBottom;
			stiDynamicBand2.PrintOn = FooterPrintOn;
			stiDynamicBand2.Conditions = Conditions.Clone() as StiConditionsCollection;
			CopyEventHandlersToComponent(stiDynamicBand2, onlyBeforePrint: true);
			double num15 = 0.0;
			for (int num16 = 0; num16 < footerRowsCount; num16++)
			{
				double realHeight3 = ((IStiTableCell)Components[num13]).GetRealHeight();
				StiPanel stiPanel3 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPanel") as StiPanel;
				stiPanel3.ClientRectangle = new RectangleD(((RectangleD)(ref clientRectangle)).X, num15, ((RectangleD)(ref clientRectangle)).Width, realHeight3);
				stiPanel3.Name = stiDynamicBand2.Name + "_FPnl" + num16;
				stiPanel3.CanGrow = true;
				stiPanel3.CanShrink = true;
				stiPanel3.DockStyle = StiDockStyle.Top;
				stiDynamicBand2.Components.Add(stiPanel3);
				num15 += realHeight3;
				for (int num17 = num13; num17 < num13 + columnCount; num17++)
				{
					StiComponent stiComponent6 = Components[num17];
					bool flag4 = false;
					StiComponent stiComponent7 = null;
					if (((IStiTableCell)stiComponent6).ParentJoin != -1)
					{
						int rowNumber3 = 0;
						stiComponent7 = GetParentJoin(((IStiTableCell)stiComponent6).ParentJoin, ref rowNumber3);
						if (hashtable5.ContainsKey(stiComponent7))
						{
							if (num16 != (int)hashtable5[stiComponent7])
							{
								StiText stiText5 = StiActivator.CreateObject("Stimulsoft.Report.Components.StiText") as StiText;
								stiText5.Page = stiComponent6.Page;
								stiText5.Conditions = stiComponent7.Conditions;
								CopyEventHandlersToComponent(stiText5, onlyBeforePrint: true, stiComponent7);
								stiText5.SetPaintRectangle(stiComponent6.GetPaintRectangle());
								stiText5.ClientRectangle = new RectangleD(((IStiTableCell)stiComponent6).GetRealLeft(), 0.0, ((IStiTableCell)stiComponent6).GetRealWidth(), ((IStiTableCell)stiComponent6).GetRealHeight());
								stiText5.Name = stiComponent6.Name + "_Emp";
								stiText5.GrowToHeight = true;
								stiText5.Brush = (StiBrush)new StiEmptyBrush();
								stiText5.DockStyle = ((IStiTableCell)stiComponent6).CellDockStyle;
								stiPanel3.Components.Add(stiText5);
								arrayList.Add(stiText5);
								if (stiComponent6 == stiComponent7)
								{
									((IStiTableCell)hashtable6[stiComponent7]).ParentJoinCell = stiText5;
								}
							}
							continue;
						}
						hashtable5.Add(stiComponent7, num16);
						stiComponent6 = stiComponent7;
						flag4 = true;
					}
					stiComponent6.Top = 0.0;
					switch (((IStiTableCell)stiComponent6).CellType)
					{
					case StiTablceCellType.Image:
					{
						StiImage stiImage3 = (StiImage)((StiTableCellImage)stiComponent6).Clone(cloneProperties: true);
						stiImage3.Restrictions ^= StiRestrictions.AllowDelete;
						stiImage3.DockStyle = ((StiTableCellImage)stiComponent6).CellDockStyle;
						SetInteraction(stiComponent6);
						stiPanel3.Components.Add(stiImage3);
						arrayList.Add(stiImage3);
						if (flag4)
						{
							stiImage3.CanGrow = true;
							if (!IsEqualRows(stiComponent6, Components[num17]))
							{
								stiImage3.Height = ((IStiTableCell)stiComponent6).GetRealHeight();
							}
							hashtable6.Add(stiComponent7, stiImage3);
						}
						break;
					}
					case StiTablceCellType.Text:
					{
						StiText stiText6 = (StiText)((StiTableCell)stiComponent6).Clone(cloneProperties: true);
						stiText6.Restrictions ^= StiRestrictions.AllowDelete;
						stiText6.DockStyle = ((StiTableCell)stiComponent6).CellDockStyle;
						SetInteraction(stiComponent6);
						stiPanel3.Components.Add(stiText6);
						arrayList.Add(stiText6);
						if (flag4)
						{
							stiText6.CanGrow = true;
							if (!IsEqualRows(stiComponent6, Components[num17]))
							{
								stiText6.Height = ((IStiTableCell)stiComponent6).GetRealHeight();
							}
							hashtable6.Add(stiComponent7, stiText6);
						}
						break;
					}
					case StiTablceCellType.CheckBox:
					{
						StiCheckBox stiCheckBox3 = (StiCheckBox)((StiTableCellCheckBox)stiComponent6).Clone(cloneProperties: true);
						stiCheckBox3.Restrictions ^= StiRestrictions.AllowDelete;
						stiCheckBox3.DockStyle = ((StiTableCellCheckBox)stiComponent6).CellDockStyle;
						SetInteraction(stiComponent6);
						stiPanel3.Components.Add(stiCheckBox3);
						arrayList.Add(stiCheckBox3);
						if (flag4)
						{
							stiCheckBox3.CanGrow = true;
							if (!IsEqualRows(stiComponent6, Components[num17]))
							{
								stiCheckBox3.Height = ((IStiTableCell)stiComponent6).GetRealHeight();
							}
							hashtable6.Add(stiComponent7, stiCheckBox3);
						}
						break;
					}
					case StiTablceCellType.RichText:
					{
						StiRichText stiRichText3 = (StiRichText)((StiTableCellRichText)stiComponent6).Clone(cloneProperties: true);
						stiRichText3.Restrictions ^= StiRestrictions.AllowDelete;
						stiRichText3.DockStyle = ((StiTableCellRichText)stiComponent6).CellDockStyle;
						SetInteraction(stiComponent6);
						stiPanel3.Components.Add(stiRichText3);
						arrayList.Add(stiRichText3);
						if (flag4)
						{
							stiRichText3.CanGrow = true;
							if (!IsEqualRows(stiComponent6, Components[num17]))
							{
								stiRichText3.Height = ((IStiTableCell)stiComponent6).GetRealHeight();
							}
							hashtable6.Add(stiComponent7, stiRichText3);
						}
						break;
					}
					}
				}
				num13 += columnCount;
			}
			parentContainer.Components.Insert(index, stiDynamicBand2);
			index++;
			arrayList.Add(stiDynamicBand2);
		}
		else if (flag && headerRowsCount > 0)
		{
			StiGroupFooterBand stiGroupFooterBand = StiActivator.CreateObject("Stimulsoft.Report.Components.StiGroupFooterBand") as StiGroupFooterBand;
			stiGroupFooterBand.Height = 0.0;
			stiGroupFooterBand.ClientRectangle = new RectangleD(((RectangleD)(ref clientRectangle)).X, num2, ((RectangleD)(ref clientRectangle)).Width, 0.0);
			stiGroupFooterBand.Name = Name + "_GrFtEmp";
			stiGroupFooterBand.Page = Page;
			stiGroupFooterBand.Parent = Page;
			stiGroupFooterBand.PrintAtBottom = FooterPrintAtBottom;
			parentContainer.Components.Insert(index, stiGroupFooterBand);
			arrayList.Add(stiGroupFooterBand);
			index++;
		}
		ReverseCells(RightToLeft);
		return stiDataBand;
	}

	private void ReverseCells(bool rightToLeft)
	{
		if (!rightToLeft)
		{
			return;
		}
		for (int i = 0; i < rowCount; i++)
		{
			int num = i * columnCount;
			int num2 = columnCount / 2;
			for (int j = 0; j < num2; j++)
			{
				StiComponent value = Components[num + j];
				Components[num + j] = Components[num + columnCount - 1 - j];
				Components[num + columnCount - 1 - j] = value;
			}
		}
		for (int k = 0; k < Components.Count; k++)
		{
			StiComponent stiComponent = Components[k];
			stiComponent.Left = Width - stiComponent.Right;
		}
	}

	private void SetFilter(StiDataBand comp)
	{
		if (((IStiDataSource)comp).IsDataSourceEmpty || ((IStiFilter)comp).FilterMethodHandler != null || !((IStiFilter)comp).FilterOn)
		{
			return;
		}
		string text = StiNameValidator.CorrectName(Name, comp.Report);
		Type type = comp.Report.GetType();
		MethodInfo method = type.GetMethod(text + "__GetFilter", new Type[2]
		{
			typeof(object),
			typeof(StiFilterEventArgs)
		});
		if (method != null)
		{
			try
			{
				((IStiFilter)comp).FilterMethodHandler = Delegate.CreateDelegate(typeof(StiFilterEventHandler), comp.Report, text + "__GetFilter") as StiFilterEventHandler;
			}
			catch (Exception e)
			{
				StiLogService.Write(((object)comp).GetType(), "StiFilterEventHandler...ERROR");
				StiLogService.Write(((object)comp).GetType(), e);
			}
		}
	}

	private void SetInteraction(StiComponent cell)
	{
		_ = cell.Interaction;
	}

	private StiComponent GetParentJoin(int id, ref int rowNumber)
	{
		int num = 0;
		foreach (IStiTableCell component in Components)
		{
			if (component.ID == id)
			{
				rowNumber = num / columnCount;
				return component as StiComponent;
			}
			num++;
		}
		return null;
	}

	private bool IsEqualRows(StiComponent comp1, StiComponent comp2)
	{
		int num = Components.IndexOf(comp1) / columnCount;
		int num2 = Components.IndexOf(comp2) / columnCount;
		return num == num2;
	}

	public override StiComponent CreateNew()
	{
		return new StiTable();
	}

	public StiTable()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiTable(RectangleD rect)
		: base(rect)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		rowCount = (columnCount = 5);
		PlaceOnToolbox = false;
	}
}
