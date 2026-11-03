using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[DefaultEvent("ValueChanged")]
[ToolboxBitmap(typeof(CircularProgress), "Controls.CircularProgress.ico")]
[ToolboxItem(true)]
public class CircularProgress : BaseItemControl
{
	private CircularProgressItem _ᕹ;

	private EventHandler ᗲ;

	[Category("Appearance")]
	[Description("Indicates circular progress bar type.")]
	[DefaultValue(eCircularProgressType.Line)]
	public eCircularProgressType ProgressBarType
	{
		get
		{
			return _ᕹ.ProgressBarType;
		}
		set
		{
			_ᕹ.ProgressBarType = value;
		}
	}

	[Description("Indicates maximum value of the progress bar.")]
	[DefaultValue(100)]
	[Category("Behavior")]
	public int Maximum
	{
		get
		{
			return _ᕹ.Maximum;
		}
		set
		{
			_ᕹ.Maximum = value;
		}
	}

	[Description("Indicates minimum value of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Minimum
	{
		get
		{
			return _ᕹ.Minimum;
		}
		set
		{
			_ᕹ.Minimum = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of progress percentage text")]
	public Color ProgressTextColor
	{
		get
		{
			return _ᕹ.ProgressTextColor;
		}
		set
		{
			_ᕹ.ProgressTextColor = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether text that displays the progress bar completion percentage text is visible")]
	[Category("Appearance")]
	public bool ProgressTextVisible
	{
		get
		{
			return _ᕹ.ProgressTextVisible;
		}
		set
		{
			_ᕹ.ProgressTextVisible = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates text displayed on top of the circular progress bar.")]
	public string ProgressText
	{
		get
		{
			return _ᕹ.ProgressText;
		}
		set
		{
			_ᕹ.ProgressText = value;
		}
	}

	[DefaultValue(0)]
	[Description("Indicates current value of the progress bar.")]
	[Category("Behavior")]
	public int Value
	{
		get
		{
			return _ᕹ.Value;
		}
		set
		{
			_ᕹ.Value = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(false)]
	public bool IsRunning
	{
		get
		{
			return _ᕹ.IsRunning;
		}
		set
		{
			_ᕹ.IsRunning = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of progress indicator.")]
	public Color ProgressColor
	{
		get
		{
			return _ᕹ.ProgressColor;
		}
		set
		{
			_ᕹ.ProgressColor = value;
		}
	}

	[Category("Pie")]
	[Description("Indicates color of pie progress bar dark border.")]
	public Color PieBorderDark
	{
		get
		{
			return _ᕹ.PieBorderDark;
		}
		set
		{
			_ᕹ.PieBorderDark = value;
		}
	}

	[Category("Pie")]
	[Description("Indicates color of pie progress bar light border. ")]
	public Color PieBorderLight
	{
		get
		{
			return _ᕹ.PieBorderLight;
		}
		set
		{
			_ᕹ.PieBorderLight = value;
		}
	}

	[Category("Spoke")]
	[Description("Indicates color of spoke progress bar dark border.")]
	public Color SpokeBorderDark
	{
		get
		{
			return _ᕹ.SpokeBorderDark;
		}
		set
		{
			_ᕹ.SpokeBorderDark = value;
		}
	}

	[Category("Spoke")]
	[Description("Indicates color of spoke progress bar light border..")]
	public Color SpokeBorderLight
	{
		get
		{
			return _ᕹ.SpokeBorderLight;
		}
		set
		{
			_ᕹ.SpokeBorderLight = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates format string for progress value.")]
	[DefaultValue("{0}%")]
	public string ProgressTextFormat
	{
		get
		{
			return _ᕹ.ProgressTextFormat;
		}
		set
		{
			_ᕹ.ProgressTextFormat = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(100)]
	[Description("Indicates the animation speed for endless running progress. Lower number means faster running.")]
	public int AnimationSpeed
	{
		get
		{
			return _ᕹ.AnimationSpeed;
		}
		set
		{
			_ᕹ.AnimationSpeed = value;
		}
	}

	[Browsable(false)]
	public override eDotNetBarStyle Style
	{
		get
		{
			return base.Style;
		}
		set
		{
			base.Style = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).RightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RightToLeft = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ImageLayout BackgroundImageLayout
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).BackgroundImageLayout;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).BackgroundImageLayout = value;
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

	public CircularProgress()
	{
		_ᕹ = new CircularProgressItem();
		_ᕹ.ValueChanged += _237B;
		_ᕹ.TextVisible = false;
		HostItem = _ᕹ;
	}

	protected override void Dispose(bool disposing)
	{
		_ᕹ.Dispose();
		base.Dispose(disposing);
	}

	protected override void RecalcSize()
	{
		_ᕹ.Diameter = Math.Min(((Control)this).Width, ((Control)this).Height);
		base.RecalcSize();
	}

	private void _237B(object _٠, EventArgs ؿ)
	{
		OnValueChanged(ؿ);
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		ᗲ?.Invoke(this, e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeProgressTextColor()
	{
		return _ᕹ.ShouldSerializeProgressTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetProgressTextColor()
	{
		_ᕹ.ResetProgressTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeProgressColor()
	{
		return _ᕹ.ShouldSerializeProgressColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetProgressColor()
	{
		_ᕹ.ResetProgressColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePieBorderDark()
	{
		return _ᕹ.ShouldSerializePieBorderDark();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPieBorderDark()
	{
		_ᕹ.ResetPieBorderDark();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePieBorderLight()
	{
		return _ᕹ.ShouldSerializePieBorderLight();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPieBorderLight()
	{
		_ᕹ.ResetPieBorderLight();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSpokeBorderDark()
	{
		return _ᕹ.ShouldSerializeSpokeBorderDark();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSpokeBorderDark()
	{
		_ᕹ.ResetSpokeBorderDark();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSpokeBorderLight()
	{
		return _ᕹ.ShouldSerializeSpokeBorderLight();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSpokeBorderLight()
	{
		_ᕹ.ResetSpokeBorderLight();
	}

	public void Start()
	{
		_ᕹ.Start();
	}

	public void Stop()
	{
		_ᕹ.Stop(disposing: false);
	}
}
