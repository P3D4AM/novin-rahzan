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
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiTreeViewControl), "Stimulsoft.Report.Dialogs.Bmp.StiTreeViewControl.gif")]
[StiGdiPainter(typeof(StiTreeViewControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiTreeViewControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGuiMode(/*Could not decode attribute arguments.*/)]
public class StiTreeViewControl : StiReportControl
{
	private TreeView control;

	private static readonly object EventAfterSelect = new object();

	public override StiComponentId ComponentId => StiComponentId.StiTreeViewControl;

	public override int ToolboxPosition => 15;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiTreeViewControl");

	[Browsable(false)]
	public TreeView Control
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
	[Description("Occurs when the AfterSelect property has changed.")]
	public virtual StiAfterSelectEvent AfterSelectEvent
	{
		get
		{
			return new StiAfterSelectEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 96.0, 96.0);

	public event TreeViewEventHandler AfterSelect
	{
		add
		{
			base.Events.AddHandler(EventAfterSelect, (Delegate?)(object)value);
		}
		remove
		{
			base.Events.RemoveHandler(EventAfterSelect, (Delegate?)(object)value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "AfterSelectEvent", AfterSelectEvent.SaveToJsonObject(mode));
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
			if (text == "AfterSelectEvent")
			{
				StiAfterSelectEvent stiAfterSelectEvent = new StiAfterSelectEvent();
				stiAfterSelectEvent.LoadFromJsonObject((JObject)item.Value);
				AfterSelectEvent = stiAfterSelectEvent;
			}
		}
	}

	public override StiEvent GetDefaultEvent()
	{
		return ClickEvent;
	}

	public void InvokeAfterSelect(TreeViewEventArgs e)
	{
		Delegate? @delegate = base.Events[EventAfterSelect];
		TreeViewEventHandler val = (TreeViewEventHandler)(object)((@delegate is TreeViewEventHandler) ? @delegate : null);
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiTreeViewControl();
	}

	public StiTreeViewControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiTreeViewControl(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		base.BackColor = SystemColors.Window;
	}
}
