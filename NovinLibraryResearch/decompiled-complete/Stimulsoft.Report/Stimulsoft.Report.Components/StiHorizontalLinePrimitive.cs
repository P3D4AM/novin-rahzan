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

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiHorizontalLinePrimitive), "Stimulsoft.Report.Images.Components.StiHorizontalLinePrimitive.png")]
[StiDesigner("Stimulsoft.Report.Components.Design.StiHorizontalLinePrimitiveDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfHorizontalLinePrimitiveDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiHorizontalLinePrimitiveGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiHorizontalLinePrimitiveWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiHorizontalLinePrimitive : StiLinePrimitive, IStiHideBorderFromDesigner, IStiBorder
{
	private StiBorder border;

	public override StiComponentId ComponentId => StiComponentId.StiHorizontalLinePrimitive;

	public override string HelpUrl => "user-manual/report_internals_primitives.htm";

	[Browsable(false)]
	[StiNonSerialized]
	public StiBorder Border
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			if (border == null)
			{
				border = new StiBorder((StiBorderSides)1, base.Color, (double)base.Size, base.Style, false, 0.0, (StiBrush)null);
			}
			return border;
		}
		set
		{
		}
	}

	public override int ToolboxPosition => 150;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Shapes;

	public override string LocalizedName => StiLocalization.Get("Components", "StiHorizontalLinePrimitive");

	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets the start cap settings.")]
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
	public override double Height
	{
		get
		{
			if (Page != null && Page.Unit != null)
			{
				return Page.Unit.ConvertFromHInches(1.0);
			}
			return 1.0;
		}
		set
		{
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
				propertiesHelper.Width()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[5]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
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
		StiHorizontalLinePrimitive stiHorizontalLinePrimitive = (StiHorizontalLinePrimitive)base.Clone(cloneProperties);
		if (StartCap != null)
		{
			stiHorizontalLinePrimitive.StartCap = (StiCap)StartCap.Clone();
		}
		else
		{
			stiHorizontalLinePrimitive.StartCap = null;
		}
		if (EndCap != null)
		{
			stiHorizontalLinePrimitive.EndCap = (StiCap)EndCap.Clone();
		}
		else
		{
			stiHorizontalLinePrimitive.EndCap = null;
		}
		return stiHorizontalLinePrimitive;
	}

	public override StiComponent CreateNew()
	{
		return new StiHorizontalLinePrimitive();
	}

	public StiHorizontalLinePrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiHorizontalLinePrimitive(RectangleD rect)
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
