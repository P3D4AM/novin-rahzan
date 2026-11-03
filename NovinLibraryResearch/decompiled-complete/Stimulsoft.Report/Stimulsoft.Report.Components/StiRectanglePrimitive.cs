using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiRectanglePrimitive), "Stimulsoft.Report.Images.Components.StiRectanglePrimitive.png")]
[StiDesigner("Stimulsoft.Report.Components.Design.StiRectanglePrimitiveDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfRectanglePrimitiveDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiRectanglePrimitiveGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiRectanglePrimitiveWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiRectanglePrimitive : StiCrossLinePrimitive, IStiHideBorderFromDesigner, IStiBorder
{
	private StiBorder border;

	public override StiComponentId ComponentId => StiComponentId.StiRectanglePrimitive;

	public override string HelpUrl => "user-manual/getting_started_report_with_cross_primitives.htm?zoom_highlightsub=Rectangle%2BPrimitive";

	[Browsable(false)]
	[StiNonSerialized]
	public StiBorder Border
	{
		get
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			if (border == null)
			{
				border = new StiBorder((StiBorderSides)15, base.Color, (double)base.Size, base.Style, false, 0.0, (StiBrush)null);
			}
			return border;
		}
		set
		{
		}
	}

	public override int ToolboxPosition => 152;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Shapes;

	public override string LocalizedName => StiLocalization.Get("Components", "StiRectanglePrimitive");

	public override double Width
	{
		get
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			StiPointPrimitive startPoint = GetStartPoint();
			StiPointPrimitive endPoint = GetEndPoint();
			if (startPoint == null || endPoint == null)
			{
				return base.Width;
			}
			PointD point = default(PointD);
			((PointD)(ref point))._002Ector(startPoint.Left, startPoint.Top);
			PointD point2 = default(PointD);
			((PointD)(ref point2))._002Ector(endPoint.Left, endPoint.Top);
			point = startPoint.ComponentToPage(point);
			point2 = endPoint.ComponentToPage(point2);
			return Math.Round(((PointD)(ref point2)).X - ((PointD)(ref point)).X, 2);
		}
		set
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			base.Width = value;
			StiStartPointPrimitive startPoint = GetStartPoint();
			StiEndPointPrimitive endPoint = GetEndPoint();
			if (startPoint != null && endPoint != null)
			{
				PointD point = default(PointD);
				((PointD)(ref point))._002Ector(startPoint.Left, startPoint.Top);
				point = startPoint.ComponentToPage(point);
				PointD point2 = default(PointD);
				((PointD)(ref point2))._002Ector(((PointD)(ref point)).X + value, ((PointD)(ref point)).Y);
				point2 = endPoint.PageToComponent(point2);
				endPoint.Left = ((PointD)(ref point2)).X;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets os sets property which indicates to draw top side of the rectangle or no.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool TopSide { get; set; } = true;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets os sets property which indicates to draw left side of the rectangle or no.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool LeftSide { get; set; } = true;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets os sets property which indicates to draw bottom side of the rectangle or no.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool BottomSide { get; set; } = true;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets os sets property which indicates to draw right side of the rectangle or no.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool RightSide { get; set; } = true;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "TopSide", TopSide, true, false);
		JsonExtensions.AddPropertyBool(val, "LeftSide", LeftSide, true, false);
		JsonExtensions.AddPropertyBool(val, "BottomSide", BottomSide, true, false);
		JsonExtensions.AddPropertyBool(val, "RightSide", RightSide, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "TopSide":
				TopSide = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LeftSide":
				LeftSide = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "BottomSide":
				BottomSide = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RightSide":
				RightSide = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Invalid comparison between Unknown and I4
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Primitive, new StiPropertyObject[3]
			{
				propertiesHelper.Color(),
				propertiesHelper.SizeFloat(),
				propertiesHelper.Style()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Primitive, new StiPropertyObject[7]
			{
				propertiesHelper.Color(),
				propertiesHelper.SizeFloat(),
				propertiesHelper.Style(),
				propertiesHelper.TopSide(),
				propertiesHelper.LeftSide(),
				propertiesHelper.BottomSide(),
				propertiesHelper.RightSide()
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
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[1] { propertiesHelper.ComponentStyle() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[1] { propertiesHelper.Enabled() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[5]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.Enabled(),
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
		if (GetStartPoint() == null && GetEndPoint() == null)
		{
			base.Convert(oldUnit, newUnit, isReportSnapshot);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiRectanglePrimitive();
	}

	public StiRectanglePrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiRectanglePrimitive(RectangleD rect)
		: base(rect)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
