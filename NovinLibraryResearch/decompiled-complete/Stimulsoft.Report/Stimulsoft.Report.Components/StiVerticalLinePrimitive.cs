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
[StiServiceBitmap(typeof(StiVerticalLinePrimitive), "Stimulsoft.Report.Images.Components.StiVerticalLinePrimitive.png")]
[StiDesigner("Stimulsoft.Report.Components.Design.StiVerticalLinePrimitiveDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfVerticalLinePrimitiveDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiVerticalLinePrimitiveGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiVerticalLinePrimitiveWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiVerticalLinePrimitive : StiCrossLinePrimitive, IStiHideBorderFromDesigner, IStiBorder
{
	private StiBorder border;

	public override StiComponentId ComponentId => StiComponentId.StiVerticalLinePrimitive;

	public override string HelpUrl => "user-manual/report_internals_primitives_crossprimitives.htm?zoom_highlightsub=Vertical%2BLine%2BPrimitive";

	[Browsable(false)]
	[StiNonSerialized]
	public virtual StiBorder Border
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			if (border == null)
			{
				border = new StiBorder((StiBorderSides)2, base.Color, (double)base.Size, base.Style, false, 0.0, (StiBrush)null);
			}
			return border;
		}
		set
		{
		}
	}

	public override int ToolboxPosition => 151;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Shapes;

	public override string LocalizedName => StiLocalization.Get("Components", "StiVerticalLinePrimitive");

	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets the end cap settings.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiCap StartCap { get; set; } = new StiCap();


	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[Description("Gets or sets the end cap settings.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiCap EndCap { get; set; } = new StiCap();


	[Browsable(false)]
	public override double Width
	{
		get
		{
			if (Page != null)
			{
				return Page.Unit.ConvertFromHInches(1.0);
			}
			return 0.0;
		}
		set
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			if (base.IsDesigning)
			{
				base.Width = value;
				StiPointPrimitive startPoint = GetStartPoint();
				StiPointPrimitive endPoint = GetEndPoint();
				if (startPoint != null && endPoint != null)
				{
					PointD point = default(PointD);
					((PointD)(ref point))._002Ector(startPoint.Left, startPoint.Top);
					point = startPoint.ComponentToPage(point);
					PointD point2 = default(PointD);
					((PointD)(ref point2))._002Ector(((PointD)(ref point)).X, ((PointD)(ref point)).Y);
					point2 = endPoint.PageToComponent(point2);
					endPoint.Left = ((PointD)(ref point2)).X;
				}
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyCap(val, "StartCap", StartCap);
		JsonExtensions.AddPropertyCap(val, "EndCap", EndCap);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "StartCap"))
			{
				if (text == "EndCap")
				{
					EndCap = StiJsonReportObjectHelper.DeserializeCap(item);
				}
			}
			else
			{
				StartCap = StiJsonReportObjectHelper.DeserializeCap(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Invalid comparison between Unknown and I4
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Invalid comparison between Unknown and I4
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
			stiPropertyCollection.Add(StiPropertyCategories.Primitive, new StiPropertyObject[5]
			{
				propertiesHelper.Color(),
				propertiesHelper.SizeFloat(),
				propertiesHelper.Style(),
				propertiesHelper.StartCap(),
				propertiesHelper.EndCap()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[3]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Height()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[5]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
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

	public override object Clone(bool cloneProperties)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		StiVerticalLinePrimitive stiVerticalLinePrimitive = (StiVerticalLinePrimitive)base.Clone(cloneProperties);
		if (StartCap != null)
		{
			stiVerticalLinePrimitive.StartCap = (StiCap)StartCap.Clone();
		}
		else
		{
			stiVerticalLinePrimitive.StartCap = null;
		}
		if (EndCap != null)
		{
			stiVerticalLinePrimitive.EndCap = (StiCap)EndCap.Clone();
		}
		else
		{
			stiVerticalLinePrimitive.EndCap = null;
		}
		return stiVerticalLinePrimitive;
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
		return new StiVerticalLinePrimitive();
	}

	public StiVerticalLinePrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiVerticalLinePrimitive(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
