using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DefaultEvent("Scroll")]
[DefaultProperty("Value")]
public abstract class ScrollBarAdv : Control, ICommandSource
{
	private ScrollBarCore _ScrollBar;

	[CompilerGenerated]
	private ScrollEventHandler m_Scroll;

	private int _LargeChange = 10;

	private int _Maximum = 100;

	private int _Minimum;

	private int _SmallChange = 1;

	private int _Value;

	private eScrollBarAppearance _Appearance;

	private ICommand _Command;

	private object _CommandParameter;

	[DefaultValue(10)]
	[Category("Behavior")]
	[Description("Indicates value to be added to or subtracted from the Value property when the scroll box is moved a large distance")]
	public int LargeChange
	{
		get
		{
			return _LargeChange;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (_LargeChange != value)
			{
				_LargeChange = value;
				if (_ScrollBar != null)
				{
					_ScrollBar.LargeChange = _LargeChange;
				}
			}
		}
	}

	[DefaultValue(100)]
	[Category("Behavior")]
	[Description("Indicates upper limit of values of the scrollable range.")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public int Maximum
	{
		get
		{
			return _Maximum;
		}
		set
		{
			if (_Maximum != value)
			{
				_Maximum = value;
				if (_ScrollBar != null)
				{
					_ScrollBar.Maximum = _Maximum;
				}
			}
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates lower limit of values of the scrollable range.")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public int Minimum
	{
		get
		{
			return _Minimum;
		}
		set
		{
			if (_Minimum != value)
			{
				_Minimum = value;
				if (_ScrollBar != null)
				{
					_ScrollBar.Minimum = _Minimum;
				}
			}
		}
	}

	[DefaultValue(1)]
	[Category("Behavior")]
	[Description("Indicates value to be added to or subtracted from the Value property when the scroll box is moved a small distance.")]
	public int SmallChange
	{
		get
		{
			return Math.Min(_SmallChange, _LargeChange);
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (_SmallChange != value)
			{
				_SmallChange = value;
				if (_ScrollBar != null)
				{
					_ScrollBar.SmallChange = SmallChange;
				}
			}
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates numeric value that represents the current position of the scroll box on the scroll bar control")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public int Value
	{
		get
		{
			if (_ScrollBar != null)
			{
				return _ScrollBar.Value;
			}
			return 0;
		}
		set
		{
			if (_ScrollBar != null)
			{
				_ScrollBar.Value = value;
			}
		}
	}

	[DefaultValue(eScrollBarAppearance.Default)]
	[Category("Appearance")]
	[Description("Indicates scroll bar appearance style.")]
	[RefreshProperties(RefreshProperties.Repaint)]
	public eScrollBarAppearance Appearance
	{
		get
		{
			return _Appearance;
		}
		set
		{
			if (_Appearance != value)
			{
				_Appearance = value;
				OnAppearanceChanged();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	public event ScrollEventHandler Scroll
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ScrollEventHandler val = this.m_Scroll;
			ScrollEventHandler val2;
			do
			{
				val2 = val;
				ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_Scroll, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			ScrollEventHandler val = this.m_Scroll;
			ScrollEventHandler val2;
			do
			{
				val2 = val;
				ScrollEventHandler value2 = (ScrollEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_Scroll, value2, val2);
			}
			while (val != val2);
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
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

	public void InvalidateCore()
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.InvalidateScrollBar();
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
		CreateScrollBarCore();
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.InvalidateScrollBar();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.Dispose();
			_ScrollBar = null;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.Enabled = ((Control)this).Enabled;
		}
		((Control)this).Refresh();
		((Control)this).OnEnabledChanged(e);
	}

	protected abstract bool IsVertical();

	private void CreateScrollBarCore()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		_ScrollBar = new ScrollBarCore((Control)(object)this, isPassive: false);
		_ScrollBar.Orientation = (IsVertical() ? eOrientation.Vertical : eOrientation.Horizontal);
		_ScrollBar.LargeChange = _LargeChange;
		_ScrollBar.Maximum = _Maximum;
		_ScrollBar.Minimum = _Minimum;
		_ScrollBar.SmallChange = SmallChange;
		_ScrollBar.Value = _Value;
		_ScrollBar.ValueChanged += CoreValueChanged;
		_ScrollBar.Scroll += new ScrollEventHandler(CoreScroll);
	}

	private void CoreScroll(object sender, ScrollEventArgs e)
	{
		OnScroll(e);
	}

	private void CoreValueChanged(object sender, EventArgs e)
	{
		OnValueChanged(e);
		ExecuteCommand();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.Paint(GetItemPaintArgs(e));
		}
		((Control)this).OnPaint(e);
	}

	private ItemPaintArgs GetItemPaintArgs(PaintEventArgs e)
	{
		return new ItemPaintArgs(null, (Control)(object)this, e.Graphics, null);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.MouseMove(e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (TabStop && !((Control)this).Focused)
		{
			((Control)this).Select();
		}
		if (_ScrollBar != null)
		{
			_ScrollBar.MouseDown(e);
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnEnter(EventArgs e)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.InvalidateScrollBar();
		}
		((Control)this).OnEnter(e);
	}

	protected override void OnLeave(EventArgs e)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.InvalidateScrollBar();
		}
		((Control)this).OnLeave(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.MouseUp(e);
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.MouseLeave();
		}
		((Control)this).OnMouseLeave(e);
	}

	protected override void OnResize(EventArgs e)
	{
		if (_ScrollBar != null)
		{
			_ScrollBar.DisplayRectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		}
		((Control)this).OnResize(e);
	}

	internal void DoScroll(int newValue, ScrollEventType eventType)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		int value = Value;
		Value = newValue;
		OnScroll(new ScrollEventArgs(eventType, value, newValue, (ScrollOrientation)(_ScrollBar.Orientation == eOrientation.Vertical)));
	}

	protected virtual void OnScroll(ScrollEventArgs e)
	{
		ScrollEventHandler scroll = this.Scroll;
		if (scroll != null)
		{
			scroll.Invoke((object)this, e);
		}
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		this.ValueChanged?.Invoke(this, e);
	}

	private void OnAppearanceChanged()
	{
		if (_ScrollBar != null)
		{
			if (_Appearance == eScrollBarAppearance.Default)
			{
				_ScrollBar.IsAppScrollBarStyle = false;
			}
			else
			{
				_ScrollBar.IsAppScrollBarStyle = true;
			}
		}
		((Control)this).Invalidate();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (_ScrollBar != null)
		{
			return _ScrollBar.ProcessCmdKey(ref msg, keyData);
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
