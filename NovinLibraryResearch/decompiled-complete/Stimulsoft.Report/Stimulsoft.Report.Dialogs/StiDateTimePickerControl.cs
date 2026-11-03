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
[StiServiceBitmap(typeof(StiDateTimePickerControl), "Stimulsoft.Report.Dialogs.Bmp.StiDateTimePickerControl.gif")]
[StiGdiPainter(typeof(StiDateTimePickerControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiDateTimePickerControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiDateTimePickerControl : StiReportControl, IStiDateTimePickerControl
{
	private static readonly object EventValueChanged = new object();

	private DateTimePicker control;

	private string valueBinding = "";

	private string maxDateBinding = "";

	private string minDateBinding = "";

	private DateTimePickerFormat format = (DateTimePickerFormat)1;

	private LeftRightAlignment dropDownAlign;

	private bool showUpDown;

	private bool today = true;

	private string customFormat = string.Empty;

	private DateTime maxDate = new DateTime(9998, 12, 31);

	private DateTime minDate = new DateTime(1753, 1, 1);

	private DateTime valueDateTime = DateTime.Now;

	private Image buttonBitmap;

	public override StiComponentId ComponentId => StiComponentId.StiDateTimePickerControl;

	public override int ToolboxPosition => 11;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiDateTimePickerControl");

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
	public DateTimePicker Control
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

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the max date.")]
	[StiOrder(140)]
	public string MaxDateBinding
	{
		get
		{
			return maxDateBinding;
		}
		set
		{
			maxDateBinding = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the min date.")]
	[StiOrder(160)]
	public string MinDateBinding
	{
		get
		{
			return minDateBinding;
		}
		set
		{
			minDateBinding = value;
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
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the format of the date and time displayed in the control.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(350)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual DateTimePickerFormat Format
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return format;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			format = value;
			UpdateReportControl("Format");
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the alignment of the drop-down calendar on the date-time picker control.")]
	[StiOrder(280)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual LeftRightAlignment DropDownAlign
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return dropDownAlign;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			dropDownAlign = value;
			UpdateReportControl("DropDownStyle");
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether an up-down control is used to adjust the date-time value.")]
	[StiOrder(660)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowUpDown
	{
		get
		{
			return showUpDown;
		}
		set
		{
			showUpDown = value;
			UpdateReportControl("ShowUpDown");
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates the date is set equal current date.")]
	[StiOrder(750)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Today
	{
		get
		{
			return today;
		}
		set
		{
			today = value;
			UpdateReportControl("Today");
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the custom date-time format string.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(230)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string CustomFormat
	{
		get
		{
			return customFormat;
		}
		set
		{
			customFormat = value;
			UpdateReportControl("CustomFormat");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the maximum date and time that can be selected in the control.")]
	[StiOrder(490)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual DateTime MaxDate
	{
		get
		{
			return maxDate;
		}
		set
		{
			maxDate = value;
			UpdateReportControl("MaxDate");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the minimum date and time that can be selected in the control.")]
	[StiOrder(530)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual DateTime MinDate
	{
		get
		{
			return minDate;
		}
		set
		{
			minDate = value;
			UpdateReportControl("MinDate");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the date-time value assigned to the control.")]
	[StiOrder(780)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual DateTime Value
	{
		get
		{
			return valueDateTime;
		}
		set
		{
			valueDateTime = value;
			UpdateReportControl("Value");
		}
	}

	[Browsable(false)]
	public Image ButtonBitmap
	{
		get
		{
			if (buttonBitmap == null)
			{
				buttonBitmap = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Images.Dialogs.ComboDown.bmp");
			}
			return buttonBitmap;
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
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "ValueChangedEvent", ValueChangedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueBinding", ValueBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "MaxDateBinding", MaxDateBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "MinDateBinding", MinDateBinding);
		JsonExtensions.AddPropertyEnum(val, "Format", (Enum)(object)Format, (Enum)(object)(DateTimePickerFormat)1);
		JsonExtensions.AddPropertyEnum(val, "DropDownAlign", (Enum)(object)DropDownAlign, (Enum)(object)(LeftRightAlignment)0);
		JsonExtensions.AddPropertyBool(val, "ShowUpDown", ShowUpDown, false, false);
		JsonExtensions.AddPropertyBool(val, "Today", Today, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "CustomFormat", CustomFormat);
		JsonExtensions.AddPropertyDateTime(val, "MaxDate", MaxDate);
		JsonExtensions.AddPropertyDateTime(val, "MinDate", MinDate);
		JsonExtensions.AddPropertyDateTime(val, "Value", Value);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ValueChangedEvent":
			{
				StiValueChangedEvent stiValueChangedEvent = new StiValueChangedEvent();
				stiValueChangedEvent.LoadFromJsonObject((JObject)item.Value);
				ValueChangedEvent = stiValueChangedEvent;
				break;
			}
			case "ValueBinding":
				valueBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "MaxDateBinding":
				maxDateBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "MinDateBinding":
				minDateBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Format":
				format = StiJsonReportObjectHelper.DeserializeEnum<DateTimePickerFormat>(item);
				break;
			case "DropDownAlign":
				dropDownAlign = StiJsonReportObjectHelper.DeserializeEnum<LeftRightAlignment>(item);
				break;
			case "ShowUpDown":
				showUpDown = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Today":
				today = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "CustomFormat":
				customFormat = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "MaxDate":
				maxDate = StiJsonReportObjectHelper.DeserializeDateTime(item);
				break;
			case "MinDate":
				minDate = StiJsonReportObjectHelper.DeserializeDateTime(item);
				break;
			case "Value":
				valueDateTime = StiJsonReportObjectHelper.DeserializeDateTime(item);
				break;
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
		return new StiDateTimePickerControl();
	}

	public StiDateTimePickerControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiDateTimePickerControl(RectangleD rect)
		: base(rect)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		base.BackColor = SystemColors.Window;
		valueDateTime = DateTime.Now;
		PlaceOnToolbox = true;
	}
}
