using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.ScrollBar;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DefaultEvent("Scroll")]
[DefaultProperty("Value")]
public abstract class ScrollBarAdv : Control, ICommandSource
{
	private ScrollBarCore _ᗱ;

	private EventHandler ᗲ;

	private ScrollEventHandler ᗳ;

	private int _ᗴ = 10;

	private int _ᗵ = 100;

	private int _ᗶ;

	private int _ᗷ = 1;

	private int _ᗸ;

	private eScrollBarAppearance _ᗹ;

	private ICommand _૦;

	private object _૧;

	[Description("Indicates value to be added to or subtracted from the Value property when the scroll box is moved a large distance")]
	[Category("Behavior")]
	[DefaultValue(10)]
	public int LargeChange
	{
		get
		{
			return _ᗴ;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (_ᗴ != value)
			{
				_ᗴ = value;
				if (_ᗱ != null)
				{
					_ᗱ.LargeChange = _ᗴ;
				}
			}
		}
	}

	[DefaultValue(100)]
	[Description("Indicates upper limit of values of the scrollable range.")]
	[Category("Behavior")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public int Maximum
	{
		get
		{
			return _ᗵ;
		}
		set
		{
			if (_ᗵ != value)
			{
				_ᗵ = value;
				if (_ᗱ != null)
				{
					_ᗱ.Maximum = _ᗵ;
				}
			}
		}
	}

	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Indicates lower limit of values of the scrollable range.")]
	[DefaultValue(0)]
	[Category("Behavior")]
	public int Minimum
	{
		get
		{
			return _ᗶ;
		}
		set
		{
			if (_ᗶ != value)
			{
				_ᗶ = value;
				if (_ᗱ != null)
				{
					_ᗱ.Minimum = _ᗶ;
				}
			}
		}
	}

	[DefaultValue(1)]
	[Description("Indicates value to be added to or subtracted from the Value property when the scroll box is moved a small distance.")]
	[Category("Behavior")]
	public int SmallChange
	{
		get
		{
			return Math.Min(_ᗷ, _ᗴ);
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (_ᗷ != value)
			{
				_ᗷ = value;
				if (_ᗱ != null)
				{
					_ᗱ.SmallChange = SmallChange;
				}
			}
		}
	}

	[DefaultValue(0)]
	[Description("Indicates numeric value that represents the current position of the scroll box on the scroll bar control")]
	[Category("Behavior")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public int Value
	{
		get
		{
			if (_ᗱ != null)
			{
				return _ᗱ.Value;
			}
			return 0;
		}
		set
		{
			if (value < Minimum)
			{
				throw new ArgumentOutOfRangeException("Value must be greater or equal than Minimum property value.");
			}
			if (value > Maximum)
			{
				throw new ArgumentOutOfRangeException("Value must be less or equal than Maximum property value.");
			}
			if (_ᗱ != null)
			{
				_ᗱ.Value = value;
			}
		}
	}

	[DefaultValue(eScrollBarAppearance.Default)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[Description("Indicates scroll bar appearance style.")]
	[Category("Appearance")]
	public eScrollBarAppearance Appearance
	{
		get
		{
			return _ᗹ;
		}
		set
		{
			if (_ᗹ != value)
			{
				_ᗹ = value;
				ᘀ();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool AutoSize
	{
		get
		{
			return ((Control)this).AutoSize;
		}
		set
		{
			((Control)this).AutoSize = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Font Font
	{
		get
		{
			return ((Control)this).Font;
		}
		set
		{
			((Control)this).Font = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImeMode ImeMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).ImeMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).ImeMode = value;
		}
	}

	[DefaultValue(false)]
	public bool TabStop
	{
		get
		{
			return ((Control)this).TabStop;
		}
		set
		{
			((Control)this).TabStop = value;
		}
	}

	[Bindable(false)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override RightToLeft RightToLeft
	{
		get
		{
			return (RightToLeft)0;
		}
		set
		{
		}
	}

	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
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

	[DefaultValue(null)]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[Category("Commands")]
	[TypeConverter(typeof(StringConverter))]
	[Browsable(true)]
	[Localizable(true)]
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

	public event ScrollEventHandler Scroll
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ScrollEventHandler val = ᗳ;
			ScrollEventHandler val2;
			do
			{
				val2 = val;
				ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᗳ, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ScrollEventHandler val = ᗳ;
			ScrollEventHandler val2;
			do
			{
				val2 = val;
				ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref ᗳ, value2, val2);
			}
			while (val != val2);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event EventHandler AutoSizeChanged
	{
		add
		{
			((Control)this).AutoSizeChanged += value;
		}
		remove
		{
			((Control)this).AutoSizeChanged -= value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event MouseEventHandler MouseClick
	{
		add
		{
			((Control)this).MouseClick += value;
		}
		remove
		{
			((Control)this).MouseClick -= value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event MouseEventHandler MouseDoubleClick
	{
		add
		{
			((Control)this).MouseDoubleClick += value;
		}
		remove
		{
			((Control)this).MouseDoubleClick -= value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event EventHandler BackColorChanged
	{
		add
		{
			((Control)this).BackColorChanged += value;
		}
		remove
		{
			((Control)this).BackColorChanged -= value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event EventHandler BackgroundImageChanged
	{
		add
		{
			((Control)this).BackgroundImageChanged += value;
		}
		remove
		{
			((Control)this).BackgroundImageChanged -= value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler BackgroundImageLayoutChanged
	{
		add
		{
			((Control)this).BackgroundImageLayoutChanged += value;
		}
		remove
		{
			((Control)this).BackgroundImageLayoutChanged -= value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event EventHandler Click
	{
		add
		{
			((Control)this).Click += value;
		}
		remove
		{
			((Control)this).Click -= value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler DoubleClick
	{
		add
		{
			((Control)this).DoubleClick += value;
		}
		remove
		{
			((Control)this).DoubleClick -= value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event EventHandler FontChanged
	{
		add
		{
			((Control)this).FontChanged += value;
		}
		remove
		{
			((Control)this).FontChanged -= value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler ForeColorChanged
	{
		add
		{
			((Control)this).ForeColorChanged += value;
		}
		remove
		{
			((Control)this).ForeColorChanged -= value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler ImeModeChanged
	{
		add
		{
			((Control)this).ImeModeChanged += value;
		}
		remove
		{
			((Control)this).ImeModeChanged -= value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event MouseEventHandler MouseDown
	{
		add
		{
			((Control)this).MouseDown += value;
		}
		remove
		{
			((Control)this).MouseDown -= value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event MouseEventHandler MouseMove
	{
		add
		{
			((Control)this).MouseMove += value;
		}
		remove
		{
			((Control)this).MouseMove -= value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event MouseEventHandler MouseUp
	{
		add
		{
			((Control)this).MouseUp += value;
		}
		remove
		{
			((Control)this).MouseUp -= value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event PaintEventHandler Paint
	{
		add
		{
			((Control)this).Paint += value;
		}
		remove
		{
			((Control)this).Paint -= value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public event EventHandler TextChanged
	{
		add
		{
			((Control)this).TextChanged += value;
		}
		remove
		{
			((Control)this).TextChanged -= value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public event EventHandler RightToLeftChanged
	{
		add
		{
			((Control)this).RightToLeftChanged += value;
		}
		remove
		{
			((Control)this).RightToLeftChanged -= value;
		}
	}

	public ScrollBarAdv()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		TabStop = false;
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, false);
		((Control)this).SetStyle((ControlStyles)1, false);
		((Control)this).SetStyle((ControlStyles)512, true);
		ᗻ();
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		if (_ᗱ != null)
		{
			_ᗱ.ᘰ();
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		if (_ᗱ != null)
		{
			_ᗱ.ValueChanged -= ᗽ;
			_ᗱ.Scroll -= new ScrollEventHandler(ᗼ);
			_ᗱ.Dispose();
			_ᗱ = null;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		ᗺ(factor);
		((Control)this).ScaleControl(factor, specified);
	}

	internal void ᗺ(SizeF _0659)
	{
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(_0659);
		}
		_ᗱ.ScaleItem(_0659);
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		_ᗱ.Enabled = ((Control)this).Enabled;
		((Control)this).Refresh();
		((Control)this).OnEnabledChanged(e);
	}

	protected abstract bool IsVertical();

	private void ᗻ()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		_ᗱ = new ScrollBarCore((Control)(object)this, isPassive: false);
		_ᗱ.Orientation = (IsVertical() ? eOrientation.Vertical : eOrientation.Horizontal);
		_ᗱ.LargeChange = _ᗴ;
		_ᗱ.Maximum = _ᗵ;
		_ᗱ.Minimum = _ᗶ;
		_ᗱ.SmallChange = SmallChange;
		_ᗱ.Value = _ᗸ;
		_ᗱ.ValueChanged += ᗽ;
		_ᗱ.Scroll += new ScrollEventHandler(ᗼ);
	}

	private void ᗼ(object _٠, ScrollEventArgs ؿ)
	{
		OnScroll(ؿ);
	}

	private void ᗽ(object _٠, EventArgs ؿ)
	{
		OnValueChanged(ؿ);
		ExecuteCommand();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_ᗱ != null)
		{
			_ᗱ.Paint(_0A92(e));
		}
		((Control)this).OnPaint(e);
	}

	private ItemPaintArgs _0A92(PaintEventArgs ؿ)
	{
		return new ItemPaintArgs(null, (Control)(object)this, ؿ.Graphics, null);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_ᗱ != null)
		{
			_ᗱ.MouseMove(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (TabStop && !((Control)this).Focused)
		{
			((Control)this).Select();
		}
		if (_ᗱ != null)
		{
			_ᗱ.MouseDown(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnEnter(EventArgs e)
	{
		if (_ᗱ != null)
		{
			_ᗱ.ᘰ();
		}
		((Control)this).OnEnter(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		if (_ᗱ != null)
		{
			_ᗱ.ᘰ();
		}
		((Control)this).OnLeave(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_ᗱ != null)
		{
			_ᗱ.MouseUp(e);
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_ᗱ != null)
		{
			_ᗱ.MouseLeave();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnResize(EventArgs e)
	{
		if (_ᗱ != null)
		{
			_ᗱ.DisplayRectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		}
		((Control)this).OnResize(e);
	}

	internal void ᗾ(int ل, ScrollEventType ᗿ)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		int value = Value;
		Value = ل;
		OnScroll(new ScrollEventArgs(ᗿ, value, ل, (ScrollOrientation)(_ᗱ.Orientation == eOrientation.Vertical)));
	}

	protected virtual void OnScroll(ScrollEventArgs e)
	{
		ScrollEventHandler val = ᗳ;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		ᗲ?.Invoke(this, e);
	}

	private void ᘀ()
	{
		if (_ᗱ != null)
		{
			if (_ᗹ == eScrollBarAppearance.Default)
			{
				_ᗱ.IsAppScrollBarStyle = false;
			}
			else
			{
				_ᗱ.IsAppScrollBarStyle = true;
			}
		}
		((Control)this).Invalidate();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (_ᗱ != null)
		{
			return _ᗱ.ᘭ(ref msg, keyData);
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
