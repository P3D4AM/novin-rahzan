using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(Slider), "Controls.Slider.ico")]
[ToolboxItem(false)]
[DefaultEvent("ValueChanged")]
[ComVisible(false)]
public class Slider : BaseItemControl, ICommandSource
{
	private SliderItem m_Slider;

	private ICommand _Command;

	private object _CommandParameter;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return m_Slider.EnableMarkup;
		}
		set
		{
			m_Slider.EnableMarkup = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eSliderLabelPosition.Left)]
	[Category("Layout")]
	[Description("Indicates text label position in relationship to the slider")]
	public eSliderLabelPosition LabelPosition
	{
		get
		{
			return m_Slider.LabelPosition;
		}
		set
		{
			m_Slider.LabelPosition = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets whether the label text is displayed.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool LabelVisible
	{
		get
		{
			return m_Slider.LabelVisible;
		}
		set
		{
			m_Slider.LabelVisible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(38)]
	[Category("Layout")]
	[Description("Indicates width of the label part of the item in pixels.")]
	public int LabelWidth
	{
		get
		{
			return m_Slider.LabelWidth;
		}
		set
		{
			m_Slider.LabelWidth = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(100)]
	public int Maximum
	{
		get
		{
			return m_Slider.Maximum;
		}
		set
		{
			m_Slider.Maximum = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the minimum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Minimum
	{
		get
		{
			return m_Slider.Minimum;
		}
		set
		{
			m_Slider.Minimum = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the current position of the slider.")]
	[Category("Behavior")]
	public int Value
	{
		get
		{
			return m_Slider.Value;
		}
		set
		{
			m_Slider.Value = value;
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider.")]
	[Category("Behavior")]
	[DefaultValue(1)]
	public int Step
	{
		get
		{
			return m_Slider.Step;
		}
		set
		{
			m_Slider.Step = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color of the label text.")]
	public Color TextColor
	{
		get
		{
			return m_Slider.TextColor;
		}
		set
		{
			m_Slider.TextColor = value;
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

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether vertical line track marker is displayed on the slide line.")]
	public virtual bool TrackMarker
	{
		get
		{
			return m_Slider.TrackMarker;
		}
		set
		{
			m_Slider.TrackMarker = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the Increase button of the slider.")]
	public string IncreaseTooltip
	{
		get
		{
			return m_Slider.IncreaseTooltip;
		}
		set
		{
			m_Slider.IncreaseTooltip = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the Increase button of the slider.")]
	public string DecreaseTooltip
	{
		get
		{
			return m_Slider.DecreaseTooltip;
		}
		set
		{
			m_Slider.DecreaseTooltip = value;
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Indicates slider orientation.")]
	public eOrientation SliderOrientation
	{
		get
		{
			return m_Slider.SliderOrientation;
		}
		set
		{
			m_Slider.SliderOrientation = value;
			RecalcLayout();
		}
	}

	internal SliderItem SliderItem => m_Slider;

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
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

	ICommand ICommandSource.Command
	{
		get
		{
			return _Command;
		}
		set
		{
			bool flag = false;
			if (_Command != value)
			{
				flag = true;
			}
			if (_Command != null)
			{
				CommandManager.UnRegisterCommandSource(this, _Command);
			}
			_Command = value;
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

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public object CommandParameter
	{
		get
		{
			return _CommandParameter;
		}
		set
		{
			_CommandParameter = value;
		}
	}

	public event EventHandler ValueChanged;

	public event CancelIntValueEventHandler ValueChanging;

	public Slider()
	{
		m_Slider = new SliderItem();
		m_Slider.Style = eDotNetBarStyle.Office2007;
		m_Slider.ValueChanged += SliderValueChanged;
		m_Slider.ValueChanging += SliderValueChanging;
		HostItem = m_Slider;
	}

	private void SliderValueChanging(object sender, CancelIntValueEventArgs e)
	{
		OnValueChanging(e);
	}

	private void SliderValueChanged(object sender, EventArgs e)
	{
		OnValueChanged(e);
		ExecuteCommand();
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, e);
		}
	}

	protected virtual void OnValueChanging(CancelIntValueEventArgs e)
	{
		if (this.ValueChanging != null)
		{
			this.ValueChanging(this, e);
		}
	}

	public void PerformStep()
	{
		m_Slider.PerformStep();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return m_Slider.ShouldSerializeTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		m_Slider.ResetTextColor();
	}

	public override void RecalcLayout()
	{
		Rectangle itemBounds = GetItemBounds();
		m_Slider.Width = Math.Max(itemBounds.Width - (m_Slider.LabelVisible ? m_Slider.LabelWidth : 0), 30);
		base.RecalcLayout();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 37)
		{
			m_Slider.Increment(-m_Slider.Step);
			return true;
		}
		if ((int)keyData == 39)
		{
			m_Slider.Increment(m_Slider.Step);
			return true;
		}
		return ((Control)this).ProcessCmdKey(ref msg, keyData);
	}

	protected virtual void ExecuteCommand()
	{
		if (_Command != null)
		{
			CommandManager.ExecuteCommand(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
