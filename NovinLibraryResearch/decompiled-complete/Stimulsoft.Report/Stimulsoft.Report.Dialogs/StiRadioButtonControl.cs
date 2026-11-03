using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
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
[StiServiceBitmap(typeof(StiRadioButtonControl), "Stimulsoft.Report.Dialogs.Bmp.StiRadioButtonControl.gif")]
[StiGdiPainter(typeof(StiRadioButtonControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiRadioButtonControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiRadioButtonControl : StiReportControl
{
	private static readonly object EventCheckedChanged = new object();

	private RadioButton control;

	private string textBinding = "";

	private string checkedBinding = "";

	private bool checkedValue;

	private string text = "RadioButton";

	public override StiComponentId ComponentId => StiComponentId.StiRadioButtonControl;

	public override int ToolboxPosition => 6;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiRadioButtonControl");

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event CheckedChanged.")]
	public virtual StiCheckedChangedEvent CheckedChangedEvent
	{
		get
		{
			return new StiCheckedChangedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	public RadioButton Control
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

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the text.")]
	[StiOrder(220)]
	public string TextBinding
	{
		get
		{
			return textBinding;
		}
		set
		{
			textBinding = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the checked.")]
	[StiOrder(100)]
	public string CheckedBinding
	{
		get
		{
			return checkedBinding;
		}
		set
		{
			checkedBinding = value;
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value indicating whether the control is checked.")]
	[StiOrder(180)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool Checked
	{
		get
		{
			return checkedValue;
		}
		set
		{
			checkedValue = value;
			UpdateReportControl("Checked");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Gets or sets the text associated with this control.")]
	[StiOrder(730)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			UpdateReportControl("Text");
		}
	}

	public event EventHandler CheckedChanged
	{
		add
		{
			base.Events.AddHandler(EventCheckedChanged, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventCheckedChanged, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "CheckedChangedEvent", CheckedChangedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextBinding", TextBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "CheckedBinding", CheckedBinding);
		JsonExtensions.AddPropertyBool(val, "Checked", Checked, false, false);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CheckedChangedEvent":
			{
				StiCheckedChangedEvent stiCheckedChangedEvent = new StiCheckedChangedEvent();
				stiCheckedChangedEvent.LoadFromJsonObject((JObject)item.Value);
				CheckedChangedEvent = stiCheckedChangedEvent;
				break;
			}
			case "TextBinding":
				textBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Checked":
				checkedValue = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Text":
				text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (CheckedChangedEvent != null)
		{
			stiEventsCollection.Add(CheckedChangedEvent);
		}
		return stiEventsCollection;
	}

	public void InvokeCheckedChanged(EventArgs e)
	{
		if (base.Events[EventCheckedChanged] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, CheckedChangedEvent);
	}

	public override StiEvent GetDefaultEvent()
	{
		return CheckedChangedEvent;
	}

	public override StiComponent CreateNew()
	{
		return new StiRadioButtonControl();
	}

	public StiRadioButtonControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiRadioButtonControl(RectangleD rect)
		: base(rect)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		Text = LocalizedName;
	}
}
