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
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiEmptyBand), "Stimulsoft.Report.Images.Components.StiEmptyBand.png")]
[StiToolbox(true)]
[StiDesigner(typeof(StiComponentDesigner))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiEmptyBand : StiBand, IStiOddEvenStyles
{
	private static readonly object EventBeginRender = new object();

	private static readonly object EventRendering = new object();

	private static readonly object EventEndRender = new object();

	public override StiComponentId ComponentId => StiComponentId.StiEmptyBand;

	public override string HelpUrl => "user-manual/report_internals_empty_band.htm";

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(186, 235, 137);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(186, 235, 137);

	public override int ToolboxPosition => 15;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority => 300;

	public override string LocalizedName => StiLocalization.Get("Components", "StiEmptyBand");

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[Description("Gets or sets value, indicates style of even lines.")]
	[Editor("Stimulsoft.Report.Design.StiStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string EvenStyle
	{
		get
		{
			return ((StiService)this).Properties.Get(StiDataBand.PropertyEvenStyle, (object)string.Empty) as string;
		}
		set
		{
			((StiService)this).Properties.Set(StiDataBand.PropertyEvenStyle, (object)value, (object)string.Empty);
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets value, indicates style of odd lines.")]
	[Editor("Stimulsoft.Report.Design.StiStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string OddStyle
	{
		get
		{
			return ((StiService)this).Properties.Get(StiDataBand.PropertyOddStyle, (object)string.Empty) as string;
		}
		set
		{
			((StiService)this).Properties.Set(StiDataBand.PropertyOddStyle, (object)value, (object)string.Empty);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when band begin render.")]
	public StiBeginRenderEvent BeginRenderEvent
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
	public StiRenderingEvent RenderingEvent
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
	public StiEndRenderEvent EndRenderEvent
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

	[DefaultValue(StiEmptySizeMode.AlignFooterToTop)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(430)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("This property allows to indicate how to change the size of the last row on a page.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiEmptySizeMode SizeMode { get; set; } = StiEmptySizeMode.AlignFooterToTop;


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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "EvenStyle", EvenStyle);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "OddStyle", OddStyle);
		JsonExtensions.AddPropertyJObject(val, "BeginRenderEvent", BeginRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "RenderingEvent", RenderingEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "EndRenderEvent", EndRenderEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyEnum(val, "SizeMode", (Enum)SizeMode, (Enum)StiEmptySizeMode.AlignFooterToTop);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
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
			case "SizeMode":
				SizeMode = StiJsonReportObjectHelper.DeserializeEnum<StiEmptySizeMode>(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[1] { propertiesHelper.Height() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[3]
			{
				propertiesHelper.Height(),
				propertiesHelper.MinHeight(),
				propertiesHelper.MaxHeight()
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
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ResetPageNumber(),
				propertiesHelper.SizeMode()
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
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override string GetHeaderText()
	{
		return ((object)this).ToString();
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
		return stiEventsCollection;
	}

	protected virtual void OnBeginRender(EventArgs e)
	{
	}

	public void InvokeBeginRender()
	{
		OnBeginRender(EventArgs.Empty);
		if (base.Events[EventBeginRender] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
	}

	protected virtual void OnRendering(EventArgs e)
	{
	}

	public void InvokeRendering()
	{
		OnRendering(EventArgs.Empty);
		if (base.Events[EventRendering] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, RenderingEvent);
	}

	protected virtual void OnEndRender(EventArgs e)
	{
	}

	public void InvokeEndRender()
	{
		OnEndRender(EventArgs.Empty);
		if (base.Events[EventEndRender] is EventHandler eventHandler)
		{
			eventHandler(this, EventArgs.Empty);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, EndRenderEvent);
	}

	public override StiComponent CreateNew()
	{
		return new StiEmptyBand();
	}

	public StiEmptyBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiEmptyBand(RectangleD rect)
		: base(rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
