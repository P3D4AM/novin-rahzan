using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(Slider), "Controls.Slider.ico")]
[DefaultEvent("ValueChanged")]
[Designer("DevComponents.DotNetBar.Design.SliderDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[ComVisible(false)]
public class Slider : BaseItemControl, ICommandSource
{
	private SliderItem _2520;

	private EventHandler ᗲ;

	private CancelIntValueEventHandler _18AB;

	private EventHandler _24D1;

	private EventHandler _24D2;

	private ICommand _૦;

	private object _૧;

	[DefaultValue(true)]
	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	[Category("Appearance")]
	public bool EnableMarkup
	{
		get
		{
			return _2520.EnableMarkup;
		}
		set
		{
			_2520.EnableMarkup = value;
		}
	}

	[Category("Layout")]
	[Browsable(true)]
	[Description("Indicates text label position in relationship to the slider")]
	[DefaultValue(eSliderLabelPosition.Left)]
	public eSliderLabelPosition LabelPosition
	{
		get
		{
			return _2520.LabelPosition;
		}
		set
		{
			_2520.LabelPosition = value;
		}
	}

	[Description("Gets or sets whether the label text is displayed.")]
	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	public bool LabelVisible
	{
		get
		{
			return _2520.LabelVisible;
		}
		set
		{
			_2520.LabelVisible = value;
		}
	}

	[Category("Layout")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DefaultValue(38)]
	[Description("Indicates width of the label part of the item in pixels.")]
	public int LabelWidth
	{
		get
		{
			return _2520.LabelWidth;
		}
		set
		{
			_2520.LabelWidth = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(100)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	public int Maximum
	{
		get
		{
			return _2520.Maximum;
		}
		set
		{
			_2520.Maximum = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(0)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Gets or sets the minimum value of the range of the control.")]
	public int Minimum
	{
		get
		{
			return _2520.Minimum;
		}
		set
		{
			_2520.Minimum = value;
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets the current position of the slider.")]
	public int Value
	{
		get
		{
			return _2520.Value;
		}
		set
		{
			_2520.Value = value;
		}
	}

	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(1)]
	[DevCoBrowsable(true)]
	public int Step
	{
		get
		{
			return _2520.Step;
		}
		set
		{
			_2520.Step = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of the label text.")]
	[Browsable(true)]
	public Color TextColor
	{
		get
		{
			return _2520.TextColor;
		}
		set
		{
			_2520.TextColor = value;
		}
	}

	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether vertical line track marker is displayed on the slide line.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public virtual bool TrackMarker
	{
		get
		{
			return _2520.TrackMarker;
		}
		set
		{
			_2520.TrackMarker = value;
		}
	}

	[Description("Indicates tooltip for the Increase button of the slider.")]
	[DefaultValue("")]
	[Browsable(true)]
	[Localizable(true)]
	[Category("Appearance")]
	public string IncreaseTooltip
	{
		get
		{
			return _2520.IncreaseTooltip;
		}
		set
		{
			_2520.IncreaseTooltip = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates tooltip for the Increase button of the slider.")]
	[Category("Appearance")]
	[Localizable(true)]
	public string DecreaseTooltip
	{
		get
		{
			return _2520.DecreaseTooltip;
		}
		set
		{
			_2520.DecreaseTooltip = value;
		}
	}

	[Description("Indicates slider orientation.")]
	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	public eOrientation SliderOrientation
	{
		get
		{
			return _2520.SliderOrientation;
		}
		set
		{
			_2520.SliderOrientation = value;
			RecalcLayout();
		}
	}

	internal SliderItem _251F => _2520;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Office2007SliderColorTable ColorTable
	{
		get
		{
			return _2520.ColorTable;
		}
		set
		{
			_2520.ColorTable = value;
		}
	}

	[Description("Indicates the command assigned to the item.")]
	[Category("Commands")]
	[DefaultValue(null)]
	public Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
		}
	}

	ICommand ICommandSource.ᒍ
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[Localizable(true)]
	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Category("Commands")]
	[TypeConverter(typeof(StringConverter))]
	public object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event CancelIntValueEventHandler ValueChanging
	{
		add
		{
			CancelIntValueEventHandler cancelIntValueEventHandler = _18AB;
			CancelIntValueEventHandler cancelIntValueEventHandler2;
			do
			{
				cancelIntValueEventHandler2 = cancelIntValueEventHandler;
				CancelIntValueEventHandler value2 = (CancelIntValueEventHandler)Delegate.Combine(cancelIntValueEventHandler2, value);
				cancelIntValueEventHandler = Interlocked.CompareExchange(ref _18AB, value2, cancelIntValueEventHandler2);
			}
			while ((object)cancelIntValueEventHandler != cancelIntValueEventHandler2);
		}
		remove
		{
			CancelIntValueEventHandler cancelIntValueEventHandler = _18AB;
			CancelIntValueEventHandler cancelIntValueEventHandler2;
			do
			{
				cancelIntValueEventHandler2 = cancelIntValueEventHandler;
				CancelIntValueEventHandler value2 = (CancelIntValueEventHandler)Delegate.Remove(cancelIntValueEventHandler2, value);
				cancelIntValueEventHandler = Interlocked.CompareExchange(ref _18AB, value2, cancelIntValueEventHandler2);
			}
			while ((object)cancelIntValueEventHandler != cancelIntValueEventHandler2);
		}
	}

	public event EventHandler IncreaseButtonClick
	{
		add
		{
			EventHandler eventHandler = _24D1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _24D1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _24D1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _24D1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler DecreaseButtonClick
	{
		add
		{
			EventHandler eventHandler = _24D2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _24D2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _24D2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _24D2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public Slider()
	{
		_2520 = new SliderItem();
		_2520.Style = eDotNetBarStyle.Office2007;
		_2520.ValueChanged += _2524;
		_2520.ValueChanging += _2523;
		_2520.IncreaseButtonClick += _2522;
		_2520.DecreaseButtonClick += _2521;
		HostItem = _2520;
	}

	private void _2521(object _٠, EventArgs ؿ)
	{
		OnDecreaseButtonClick(ؿ);
	}

	private void _2522(object _٠, EventArgs ؿ)
	{
		OnIncreaseButtonClick(ؿ);
	}

	protected virtual void OnIncreaseButtonClick(EventArgs e)
	{
		_24D1?.Invoke(this, e);
	}

	protected virtual void OnDecreaseButtonClick(EventArgs e)
	{
		_24D2?.Invoke(this, e);
	}

	private void _2523(object _٠, CancelIntValueEventArgs ؿ)
	{
		OnValueChanging(ؿ);
	}

	private void _2524(object _٠, EventArgs ؿ)
	{
		OnValueChanged(ؿ);
		ExecuteCommand();
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		if (ᗲ != null)
		{
			ᗲ(this, e);
		}
	}

	protected virtual void OnValueChanging(CancelIntValueEventArgs e)
	{
		if (_18AB != null)
		{
			_18AB(this, e);
		}
	}

	public void PerformStep()
	{
		_2520.PerformStep();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return _2520.ShouldSerializeTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		_2520.ResetTextColor();
	}

	public override void RecalcLayout()
	{
		Rectangle itemBounds = GetItemBounds();
		_2520.Width = Math.Max(itemBounds.Width - (_2520.LabelVisible ? _2520.LabelWidth : 0), 30);
		base.RecalcLayout();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 37)
		{
			_2520.Increment(-_2520.Step, eEventSource.Keyboard);
			return true;
		}
		if ((int)keyData == 39)
		{
			_2520.Increment(_2520.Step, eEventSource.Keyboard);
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null)
		{
			CommandManager._૭(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
