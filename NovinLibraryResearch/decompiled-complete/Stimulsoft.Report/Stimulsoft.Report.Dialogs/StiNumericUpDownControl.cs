using System;
using System.ComponentModel;
using System.Drawing;
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
[StiServiceBitmap(typeof(StiNumericUpDownControl), "Stimulsoft.Report.Dialogs.Bmp.StiNumericUpDownControl.gif")]
[StiGdiPainter(typeof(StiNumericUpDownControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiNumericUpDownControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiNumericUpDownControl : StiReportControl
{
	private NumericUpDown control;

	private string minimumBinding = "";

	private string maximumBinding = "";

	private string valueBinding = "";

	private int minimum;

	private int maximum = 100;

	private int increment = 1;

	private int valueData;

	private static readonly object EventValueChanged = new object();

	private Bitmap buttonUpBitmap;

	private Bitmap buttonDownBitmap;

	public override StiComponentId ComponentId => StiComponentId.StiNumericUpDownControl;

	public override int ToolboxPosition => 12;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiNumericUpDownControl");

	[Browsable(false)]
	public NumericUpDown Control
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
	[Description("Gets the data bindings for the minimum.")]
	[StiOrder(170)]
	public string MinimumBinding
	{
		get
		{
			return minimumBinding;
		}
		set
		{
			minimumBinding = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the maximum.")]
	[StiOrder(150)]
	public string MaximumBinding
	{
		get
		{
			return maximumBinding;
		}
		set
		{
			maximumBinding = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the value.")]
	[StiOrder(230)]
	public string ValueBinding
	{
		get
		{
			return valueBinding;
		}
		set
		{
			valueBinding = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override double Height
	{
		get
		{
			return 20.0;
		}
		set
		{
			base.Height = 20.0;
		}
	}

	[StiSerializable]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the minimum allowed value for the up-down control.")]
	[StiOrder(540)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int Minimum
	{
		get
		{
			return minimum;
		}
		set
		{
			minimum = value;
			UpdateReportControl("Minimum");
		}
	}

	[StiSerializable]
	[DefaultValue(100)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the maximum value for the up-down control.")]
	[StiOrder(510)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int Maximum
	{
		get
		{
			return maximum;
		}
		set
		{
			maximum = value;
			UpdateReportControl("Maximum");
		}
	}

	[StiSerializable]
	[DefaultValue(1)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the value to increment or decrement the up-down control when the up or down buttons are clicked.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(440)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int Increment
	{
		get
		{
			return increment;
		}
		set
		{
			increment = value;
			UpdateReportControl("Increment");
		}
	}

	[StiSerializable]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the value assigned to the up-down control.")]
	[StiOrder(780)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int Value
	{
		get
		{
			return valueData;
		}
		set
		{
			valueData = value;
			UpdateReportControl("Value");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event ValueChanged.")]
	public virtual StiValueChangedEvent ValueChangedEvent
	{
		get
		{
			return new StiValueChangedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	public Image ButtonUpBitmap
	{
		get
		{
			if (buttonUpBitmap == null)
			{
				buttonUpBitmap = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Images.Dialogs.SpinUp.bmp");
			}
			return (Image)(object)buttonUpBitmap;
		}
	}

	[Browsable(false)]
	public Image ButtonDownBitmap
	{
		get
		{
			if (buttonDownBitmap == null)
			{
				buttonDownBitmap = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Images.Dialogs.SpinDown.bmp");
			}
			return (Image)(object)buttonDownBitmap;
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			base.Events.AddHandler(EventValueChanged, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventValueChanged, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "MinimumBinding", MinimumBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "MaximumBinding", MaximumBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueBinding", ValueBinding);
		JsonExtensions.AddPropertyInt(val, "Minimum", Minimum, 0);
		JsonExtensions.AddPropertyInt(val, "Maximum", Maximum, 100);
		JsonExtensions.AddPropertyInt(val, "Increment", Increment, 1);
		JsonExtensions.AddPropertyInt(val, "Value", Value, 0);
		JsonExtensions.AddPropertyJObject(val, "ValueChangedEvent", ValueChangedEvent.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "MinimumBinding":
				minimumBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "MaximumBinding":
				maximumBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValueBinding":
				valueBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Minimum":
				minimum = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Maximum":
				maximum = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Increment":
				increment = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Value":
				valueData = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "ValueChangedEvent":
			{
				StiValueChangedEvent stiValueChangedEvent = new StiValueChangedEvent();
				stiValueChangedEvent.LoadFromJsonObject((JObject)item.Value);
				ValueChangedEvent = stiValueChangedEvent;
				break;
			}
			}
		}
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (ValueChangedEvent != null)
		{
			stiEventsCollection.Add(ValueChangedEvent);
		}
		return stiEventsCollection;
	}

	public void InvokeValueChanged(EventArgs e)
	{
		if (base.Events[EventValueChanged] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, ValueChangedEvent);
	}

	public override StiEvent GetDefaultEvent()
	{
		return ValueChangedEvent;
	}

	public override StiComponent CreateNew()
	{
		return new StiNumericUpDownControl();
	}

	public StiNumericUpDownControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiNumericUpDownControl(RectangleD rect)
		: base(rect)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		base.BackColor = SystemColors.Window;
		PlaceOnToolbox = true;
	}
}
