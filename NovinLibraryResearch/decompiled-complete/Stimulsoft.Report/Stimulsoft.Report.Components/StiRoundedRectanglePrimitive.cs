using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiRoundedRectanglePrimitive), "Stimulsoft.Report.Images.Components.StiRoundedRectanglePrimitive.png")]
[StiGdiPainter(typeof(StiRoundedRectanglePrimitiveGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiRoundedRectanglePrimitiveWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiRoundedRectanglePrimitive : StiRectanglePrimitive
{
	private float round = 0.2f;

	public override StiComponentId ComponentId => StiComponentId.StiRoundedRectanglePrimitive;

	public override string HelpUrl => "user-manual/report_internals_primitives_crossprimitives.htm?zoom_highlightsub=Rounded%2BRectangle";

	public override int ToolboxPosition => 153;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Shapes;

	public override string LocalizedName => StiLocalization.Get("Components", "StiRoundedRectanglePrimitive");

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(145)]
	[StiSerializable]
	[Description("Gets or sets the factor of rounding.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float Round
	{
		get
		{
			return round;
		}
		set
		{
			if (value > 0f && (double)value <= 0.5)
			{
				round = value;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Round", Round, 0.2f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "Round")
			{
				round = StiJsonReportObjectHelper.DeserializeFloat(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Invalid comparison between Unknown and I4
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Invalid comparison between Unknown and I4
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
			stiPropertyCollection.Add(StiPropertyCategories.Primitive, new StiPropertyObject[8]
			{
				propertiesHelper.Color(),
				propertiesHelper.SizeFloat(),
				propertiesHelper.Style(),
				propertiesHelper.Round(),
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

	public override StiComponent CreateNew()
	{
		return new StiRoundedRectanglePrimitive();
	}

	public StiRoundedRectanglePrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiRoundedRectanglePrimitive(RectangleD rect)
		: base(rect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
