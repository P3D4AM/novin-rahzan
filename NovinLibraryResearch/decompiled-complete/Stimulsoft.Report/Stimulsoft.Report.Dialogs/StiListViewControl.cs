using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiListViewControl), "Stimulsoft.Report.Dialogs.Bmp.StiListViewControl.gif")]
[StiGdiPainter(typeof(StiListViewControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiListViewControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGuiMode(/*Could not decode attribute arguments.*/)]
public class StiListViewControl : StiReportControl
{
	private ListView control;

	private static readonly object EventSelectedIndexChanged = new object();

	public override StiComponentId ComponentId => StiComponentId.StiListViewControl;

	public override int ToolboxPosition => 16;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiListViewControl");

	[Browsable(false)]
	public ListView Control
	{
		get
		{
			return control;
		}
		set
		{
			control = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the SelectedIndex property has changed.")]
	public virtual StiSelectedIndexChangedEvent SelectedIndexChangedEvent
	{
		get
		{
			return new StiSelectedIndexChangedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 96.0, 96.0);

	public event EventHandler SelectedIndexChanged
	{
		add
		{
			base.Events.AddHandler(EventSelectedIndexChanged, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventSelectedIndexChanged, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "SelectedIndexChangedEvent", SelectedIndexChangedEvent.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "SelectedIndexChangedEvent")
			{
				StiSelectedIndexChangedEvent stiSelectedIndexChangedEvent = new StiSelectedIndexChangedEvent();
				stiSelectedIndexChangedEvent.LoadFromJsonObject((JObject)item.Value);
				SelectedIndexChangedEvent = stiSelectedIndexChangedEvent;
			}
		}
	}

	public override StiEvent GetDefaultEvent()
	{
		return ClickEvent;
	}

	public void InvokeSelectedIndexChanged(EventArgs e)
	{
		if (base.Events[EventSelectedIndexChanged] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, SelectedIndexChangedEvent);
	}

	public override StiComponent CreateNew()
	{
		return new StiListViewControl();
	}

	public StiListViewControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiListViewControl(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		base.BackColor = SystemColors.Window;
	}
}
