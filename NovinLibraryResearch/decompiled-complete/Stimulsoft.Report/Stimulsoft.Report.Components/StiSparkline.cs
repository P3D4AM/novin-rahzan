using System;
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
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiChart.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiChart.png")]
[StiToolbox(true)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiSparklineDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfSparklineDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Report.Painters.StiSparklineWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiSparklineGdiPainter))]
[StiV2Builder(typeof(StiSparklineV2Builder))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiEngine(/*Could not decode attribute arguments.*/)]
public class StiSparkline : StiComponent, IStiExportImageExtended, IStiExportImage, IStiDataRelation, IStiBorder, IStiBrush
{
	private string valueDataColumn = string.Empty;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSparkline;

	[TypeConverter(typeof(StiDataRelationConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiDataRelationEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(2)]
	[Description("Get link that is used for master-detail reports rendering.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiDataRelation DataRelation
	{
		get
		{
			if (Page == null || Report?.Dictionary?.Relations == null || DataRelationName == null || DataRelationName.Length == 0)
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


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("Gets or sets border of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBorder Border { get; set; } = new StiBorder();


	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

	public override int Priority => 0;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 100.0, 30.0);

	public override StiComponentType ComponentType => StiComponentType.Simple;

	public override int ToolboxPosition => 225;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedName => StiLocalization.Get("Components", "StiSparkline");

	public override string HelpUrl => "user-manual/index.html?report_internals_sparkline.htm";

	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets a name of the column that contains the values.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(1)]
	public virtual string ValueDataColumn
	{
		get
		{
			return valueDataColumn;
		}
		set
		{
			if (valueDataColumn != value)
			{
				valueDataColumn = value;
				Values = null;
			}
		}
	}

	[Browsable(false)]
	public decimal[] Values { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ValuesContainer
	{
		get
		{
			if (Values == null)
			{
				return null;
			}
			return string.Join(";", Values);
		}
		set
		{
			if (value == null || value.Trim().Length == 0)
			{
				Values = null;
				return;
			}
			string[] source = value.Split(new char[1] { ';' });
			Values = source.Select((string s) => StiValueHelper.TryToDecimal((object)s)).ToArray();
		}
	}

	[DefaultValue(StiSparklineType.Area)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable]
	[Browsable(false)]
	public StiSparklineType Type { get; set; } = StiSparklineType.Area;


	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable]
	[Browsable(false)]
	public bool ShowHighLowPoints { get; set; }

	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiSerializable]
	[Browsable(false)]
	public bool ShowFirstLastPoints { get; set; } = true;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Browsable(false)]
	[DefaultValue(null)]
	public Color PositiveColor { get; set; } = StiColor.Get("537eb6");


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Browsable(false)]
	[DefaultValue(null)]
	public Color NegativeColor { get; set; } = StiColor.Get("ff0000");


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Invalid comparison between Unknown and I4
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataRelationName", DataRelationName);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueDataColumn", ValueDataColumn);
		JsonExtensions.AddPropertyEnum(val, "Type", (Enum)Type, (Enum)StiSparklineType.Area);
		JsonExtensions.AddPropertyBool(val, "ShowHighLowPoints", ShowHighLowPoints, false, false);
		JsonExtensions.AddPropertyBool(val, "ShowFirstLastPoints", ShowFirstLastPoints, true, false);
		JsonExtensions.AddPropertyColor(val, "PositiveColor", PositiveColor, "#537eb6");
		JsonExtensions.AddPropertyColor(val, "NegativeColor", NegativeColor, "#ff0000");
		if ((int)mode == 1)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValuesContainer", ValuesContainer);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "DataRelationName":
				DataRelationName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "ValueDataColumn":
				ValueDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Type":
				Type = StiJsonReportObjectHelper.DeserializeEnum<StiSparklineType>(item);
				break;
			case "ShowHighLowPoints":
				ShowHighLowPoints = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowFirstLastPoints":
				ShowFirstLastPoints = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "PositiveColor":
				PositiveColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "NegativeColor":
				NegativeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ValuesContainer":
				ValuesContainer = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Invalid comparison between Unknown and I4
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.ShapeEditor() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Data, new StiPropertyObject[1] { propertiesHelper.ValueDataColumn() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Data, new StiPropertyObject[2]
			{
				propertiesHelper.ValueDataColumn(),
				propertiesHelper.DataRelation()
			});
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[2]
			{
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[8]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
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

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter.GetImage(this, ref zoom, format);
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		return format != StiExportFormat.Pdf;
	}

	public override object Clone(bool cloneProperties)
	{
		StiSparkline stiSparkline = (StiSparkline)base.Clone(cloneProperties);
		StiBorder border = Border;
		object obj = ((border != null) ? border.Clone() : null);
		stiSparkline.Border = (StiBorder)((obj is StiBorder) ? obj : null);
		StiBrush brush = Brush;
		object obj2 = ((brush != null) ? brush.Clone() : null);
		stiSparkline.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		return stiSparkline;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.Transparent);
		}
		return true;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	public override StiComponent CreateNew()
	{
		return new StiSparkline();
	}

	internal object[] FetchValues()
	{
		if (Values == null && base.IsDesigning)
		{
			decimal[] valuesFromValueDataColumn = StiSparklineV2Builder.GetValuesFromValueDataColumn(this, 10);
			if (valuesFromValueDataColumn != null && valuesFromValueDataColumn.Length != 0)
			{
				Values = valuesFromValueDataColumn;
			}
			else
			{
				Values = new decimal[11]
				{
					1m, 2m, 3m, 6m, 3m, 2m, 7m, 4m, 2m, -3m,
					-5m
				};
			}
		}
		return Values?.Cast<object>().ToArray();
	}

	public StiDataSource GetDataSource()
	{
		return StiDataColumn.GetDataSourceFromDataColumn(Report.Dictionary, ValueDataColumn);
	}

	private bool ShouldSerializePositiveColor()
	{
		return PositiveColor != StiColor.Get("537eb6");
	}

	private bool ShouldSerializeNegativeColor()
	{
		return NegativeColor != StiColor.Get("ff0000");
	}

	public StiSparkline()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiSparkline(RectangleD rect)
		: base(rect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
