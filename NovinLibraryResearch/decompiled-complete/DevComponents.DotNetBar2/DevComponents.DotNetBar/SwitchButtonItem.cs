using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Events;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[DefaultEvent("ValueChanged")]
[DefaultProperty("Value")]
public class SwitchButtonItem : BaseItem
{
	private EventHandler _18AB;

	private EventHandler ᗲ;

	internal static readonly int _18AC = 3;

	private bool __0EB9 = true;

	private bool _ᗸ;

	private Point _ᐨ = Point.Empty;

	private bool __18AD;

	private Rectangle __18AE;

	private Rectangle __18AF;

	private Rectangle _ᢰ;

	private Rectangle _ᢱ;

	private int _ᢲ;

	private bool _ᢳ;

	private int _ᢴ = 28;

	private string _ᢵ = "ON";

	private string _ᢶ = "OFF";

	private bool _ᢷ;

	private eEventSource _ᢸ;

	private int _ᢹ;

	private BackgroundWorker _ᢺ;

	private bool _ᢻ = true;

	private int _ᢼ = 66;

	private int _ᢽ = 22;

	private Padding _ᢾ = new Padding(0, 0, 0, 0);

	private Padding __0AE5 = new Padding(0, 0, 0, 0);

	private Color _ᢿ = Color.Empty;

	private Color _ᣀ = Color.Empty;

	private Color _ᣁ = Color.Empty;

	private Color _ᣂ = Color.Empty;

	private Color _Ꮃ = Color.Empty;

	private Color _ᣃ = Color.Empty;

	private Color _ᣄ = Color.Empty;

	private bool _ᣅ = true;

	private Color __0CD1 = Color.Empty;

	private Font _ᣆ;

	private bool _ᣇ;

	private bool _ᣈ = true;

	private static readonly Color ᣉ = ColorScheme.GetColor(12603469);

	private Color _ᣊ = ᣉ;

	protected override bool IsMarkupSupported => __0EB9;

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[DefaultValue("")]
	[Localizable(true)]
	[Description("The text contained in the item.")]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[DefaultValue(false)]
	[Bindable(true)]
	[Category("Appearance")]
	[Description("Indicates switch value.")]
	public bool Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			if (_ᗸ != value)
			{
				SetValue(value, eEventSource.Code);
			}
		}
	}

	[Description("Indicates whether clicking left mouse button on the switch part of the item will toggle the switch Value.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool SwitchClickTogglesValue
	{
		get
		{
			return __18AD;
		}
		set
		{
			__18AD = value;
		}
	}

	[Browsable(false)]
	public Rectangle SwitchBounds
	{
		get
		{
			return __18AE;
		}
		internal set
		{
			__18AE = value;
		}
	}

	[Browsable(false)]
	public Rectangle ButtonBounds
	{
		get
		{
			return __18AF;
		}
		internal set
		{
			__18AF = value;
		}
	}

	[Browsable(false)]
	public Rectangle OnPartBounds
	{
		get
		{
			return _ᢰ;
		}
		internal set
		{
			_ᢰ = value;
		}
	}

	[Browsable(false)]
	public Rectangle OffPartBounds
	{
		get
		{
			return _ᢱ;
		}
		internal set
		{
			_ᢱ = value;
		}
	}

	internal int ᢪ
	{
		get
		{
			return _ᢲ;
		}
		set
		{
			_ᢲ = value;
			Refresh();
		}
	}

	[Browsable(false)]
	public bool SwitchPressed
	{
		get
		{
			return _ᢳ;
		}
		internal set
		{
			_ᢳ = value;
			ᢪ = 0;
			Refresh();
		}
	}

	[DefaultValue(28)]
	[Category("Appearance")]
	[Description("Indicates width in pixels of the switch part of the button.")]
	public int SwitchWidth
	{
		get
		{
			return _ᢴ;
		}
		set
		{
			if (value < 6)
			{
				value = 6;
			}
			if (_ᢴ != value)
			{
				_ᢴ = value;
				Refresh();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates text that is displayed on switch when Value property is set to true.")]
	[DefaultValue("ON")]
	[Localizable(true)]
	public string OnText
	{
		get
		{
			return _ᢵ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_ᢵ != value)
			{
				_ᢵ = value;
				Refresh();
			}
		}
	}

	[Description("Indicates text that is displayed on switch when Value property is set to true.")]
	[Localizable(true)]
	[DefaultValue("OFF")]
	[Category("Appearance")]
	public string OffText
	{
		get
		{
			return _ᢶ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_ᢶ != value)
			{
				_ᢶ = value;
				Refresh();
			}
		}
	}

	protected virtual bool IsAnimationEnabled => _ᢻ;

	[Category("Appearance")]
	[Description("Indicates whether state transition animation is enabled.")]
	[DefaultValue(true)]
	public bool AnimationEnabled
	{
		get
		{
			return _ᢻ;
		}
		set
		{
			_ᢻ = value;
		}
	}

	[Description("Indicates width of the switch button. Must be greater than SwitchWidth.")]
	[Category("Appearance")]
	[DefaultValue(66)]
	public int ButtonWidth
	{
		get
		{
			return _ᢼ;
		}
		set
		{
			if (value < _ᢴ)
			{
				value = _ᢴ + 1;
			}
			_ᢼ = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Description("Indicates height of the switch button. Must be greater than 5.")]
	[DefaultValue(22)]
	[Category("Appearance")]
	public int ButtonHeight
	{
		get
		{
			return _ᢽ;
		}
		set
		{
			if (value < 6)
			{
				value = 6;
			}
			_ᢽ = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Description("Gets or sets text padding.")]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public Padding TextPadding => _ᢾ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Appearance")]
	[Description("Gets or sets switch margin.")]
	[Browsable(true)]
	public new Padding Margin => __0AE5;

	[Description("Indicates color of OFF state background.")]
	[Category("Appearance")]
	public Color OffBackColor
	{
		get
		{
			return _ᢿ;
		}
		set
		{
			_ᢿ = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of ON state background.")]
	public Color OnBackColor
	{
		get
		{
			return _ᣀ;
		}
		set
		{
			_ᣀ = value;
			Refresh();
		}
	}

	[Description("Indicates color of ON state text.")]
	[Category("Appearance")]
	public Color OnTextColor
	{
		get
		{
			return _ᣁ;
		}
		set
		{
			_ᣁ = value;
			Refresh();
		}
	}

	[Description("Indicates color of OFF state text.")]
	[Category("Appearance")]
	public Color OffTextColor
	{
		get
		{
			return _ᣂ;
		}
		set
		{
			_ᣂ = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates item border color.")]
	public Color BorderColor
	{
		get
		{
			return _Ꮃ;
		}
		set
		{
			_Ꮃ = value;
			Refresh();
		}
	}

	[Description("Indicates border color of the button switch.")]
	[Category("Appearance")]
	public Color SwitchBorderColor
	{
		get
		{
			return _ᣃ;
		}
		set
		{
			_ᣃ = value;
			Refresh();
		}
	}

	[Description("Indicates background color of the switch button.")]
	[Category("Appearance")]
	public Color SwitchBackColor
	{
		get
		{
			return _ᣄ;
		}
		set
		{
			_ᣄ = value;
			Refresh();
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether caption/label set using Text property is visible.")]
	[Category("Appearance")]
	public bool TextVisible
	{
		get
		{
			return _ᣅ;
		}
		set
		{
			_ᣅ = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Description("Indicates text color.")]
	[Category("Appearance")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
		}
	}

	[Description("Indicates font that is used to draw ON/OFF text on the switch button.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Font SwitchFont
	{
		get
		{
			return _ᣆ;
		}
		set
		{
			_ᣆ = value;
			Refresh();
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether button is in read-only state meaning that it appears as enabled but user cannot change its state.")]
	[Category("Behavior")]
	public bool IsReadOnly
	{
		get
		{
			return _ᣇ;
		}
		set
		{
			_ᣇ = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether lock marker is visible on face of the control when IsReadOnly is set to true.")]
	[DefaultValue(true)]
	public bool ShowReadOnlyMarker
	{
		get
		{
			return _ᣈ;
		}
		set
		{
			_ᣈ = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of read-only marker.")]
	public Color ReadOnlyMarkerColor
	{
		get
		{
			return _ᣊ;
		}
		set
		{
			_ᣊ = value;
			Refresh();
		}
	}

	public event EventHandler ValueChanging
	{
		add
		{
			EventHandler eventHandler = _18AB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _18AB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _18AB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _18AB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
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

	protected virtual void OnValueChanging(CancelableEventSourceArgs e)
	{
		_18AB?.Invoke(this, e);
	}

	protected virtual void OnValueChanged(EventSourceArgs e)
	{
		ᗲ?.Invoke(this, e);
	}

	public SwitchButtonItem()
		: this("", "")
	{
	}

	public SwitchButtonItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public SwitchButtonItem(string sItemName, string itemText)
		: base(sItemName, itemText)
	{
		__0AE5.PropertyChanged += ᣏ;
	}

	public override BaseItem Copy()
	{
		SwitchButtonItem switchButtonItem = new SwitchButtonItem(m_Name);
		CopyToItem(switchButtonItem);
		return switchButtonItem;
	}

	internal void _0EBA(SwitchButtonItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		if (copy is SwitchButtonItem switchButtonItem)
		{
			switchButtonItem.Value = Value;
			switchButtonItem.BorderColor = BorderColor;
			switchButtonItem.OffBackColor = OffBackColor;
			switchButtonItem.OffText = OffText;
			switchButtonItem.OffTextColor = OffTextColor;
			switchButtonItem.OnBackColor = OnBackColor;
			switchButtonItem.OnText = OnText;
			switchButtonItem.OnTextColor = OnTextColor;
			switchButtonItem.SwitchBackColor = SwitchBackColor;
			switchButtonItem.SwitchBorderColor = SwitchBorderColor;
			switchButtonItem.SwitchWidth = SwitchWidth;
			switchButtonItem.ButtonWidth = ButtonWidth;
			switchButtonItem.ButtonHeight = ButtonHeight;
			base.CopyToItem(switchButtonItem);
		}
	}

	protected override void Dispose(bool disposing)
	{
		ᣎ();
		base.Dispose(disposing);
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			SwitchButtonRenderEventArgs switchButtonRenderEventArgs = new SwitchButtonRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
			switchButtonRenderEventArgs.က = p;
			ໝ.DrawSwitchButton(switchButtonRenderEventArgs);
		}
		else
		{
			ᣒ ᣒ = ᣓ._1928(this);
			if (ᣒ != null)
			{
				SwitchButtonRenderEventArgs switchButtonRenderEventArgs2 = new SwitchButtonRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
				switchButtonRenderEventArgs2.က = p;
				ᣒ.Paint(switchButtonRenderEventArgs2);
			}
		}
		DrawInsertMarker(p.Graphics);
	}

	public override void RecalcSize()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		Size size = new Size(Dpi.Width(_ᢼ + __0AE5.Horizontal), Dpi.Height(_ᢽ + __0AE5.Vertical));
		if (_ᣅ && !string.IsNullOrEmpty(Text))
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Font font = val.Font;
				Graphics val2 = val.CreateGraphics();
				try
				{
					Size size2 = _20B1._20B2(this, val2, 0, font, eTextFormat.WordBreak, (int)val.RightToLeft == 1);
					size2.Width += Dpi.Width(_ᢾ.Horizontal + _18AC);
					size2.Height += Dpi.Height(_ᢾ.Vertical);
					size.Width += size2.Width;
					size.Height = Math.Max(size.Height, size2.Height);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		m_Rect.Width = size.Width;
		m_Rect.Height = size.Height;
		base.RecalcSize();
	}

	internal Size ᣋ()
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Invalid comparison between Unknown and I4
		Size result = new Size(Dpi.Width(_ᢴ + __0AE5.Horizontal), Dpi.Height8);
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			Font font = val.Font;
			Graphics val2 = val.CreateGraphics();
			try
			{
				Size size = TextDrawing.MeasureString(val2, _ᢵ, font);
				Size size2 = TextDrawing.MeasureString(val2, _ᢶ, font);
				result.Width += Math.Max(size.Width, size2.Width) + 8;
				result.Height = Math.Max(result.Height, size2.Height + __0AE5.Vertical + 4);
				if (_ᣅ && !string.IsNullOrEmpty(Text))
				{
					Size size3 = _20B1._20B2(this, val2, 0, font, eTextFormat.WordBreak, (int)val.RightToLeft == 1);
					size3.Width += _ᢾ.Horizontal + _18AC;
					size3.Height += _ᢾ.Vertical;
					result.Width += size3.Width;
					result.Height = Math.Max(result.Height, size3.Height);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		return result;
	}

	public void SetValue(bool newValue, eEventSource source)
	{
		CancelableEventSourceArgs cancelableEventSourceArgs = new CancelableEventSourceArgs(source);
		OnValueChanging(cancelableEventSourceArgs);
		if (cancelableEventSourceArgs.Cancel)
		{
			ᢪ = 0;
			Refresh();
			return;
		}
		_ᗸ = newValue;
		_ᢷ = newValue;
		if (ShouldSyncProperties)
		{
			BarFunctions.ढ़(this, "Value");
		}
		ᢪ = 0;
		if (Displayed)
		{
			Refresh();
		}
		ExecuteCommand();
		EventSourceArgs e = new EventSourceArgs(source);
		OnValueChanged(e);
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		if (Enabled && !_ᣇ && !DesignMode && (int)objArg.Button == 1048576)
		{
			if (!Value && _ᢱ.Contains(objArg.X, objArg.Y))
			{
				SetValueAndAnimate(value: true, eEventSource.Mouse);
			}
			else if (Value && _ᢰ.Contains(objArg.X, objArg.Y))
			{
				SetValueAndAnimate(value: false, eEventSource.Mouse);
			}
			else if (__18AE.Contains(objArg.X, objArg.Y))
			{
				_ᐨ = new Point(objArg.X, objArg.Y);
				SwitchPressed = true;
			}
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		if (_ᢳ)
		{
			if ((int)objArg.Button == 1048576)
			{
				Point ᐨ = _ᐨ;
				int num = objArg.X - ᐨ.X;
				if (num != 0)
				{
					if (!Value && num > 0)
					{
						if (__18AE.Right + num >= __18AF.Right)
						{
							SetValue(newValue: true, eEventSource.Mouse);
							_ᐨ = new Point(objArg.X, objArg.Y);
						}
						else
						{
							ᢪ = num;
						}
					}
					else if (Value && num < 0)
					{
						if (__18AE.X + num <= __18AF.X)
						{
							SetValue(newValue: false, eEventSource.Mouse);
							_ᐨ = new Point(objArg.X, objArg.Y);
						}
						else
						{
							ᢪ = Math.Abs(num);
						}
					}
				}
			}
			else
			{
				SwitchPressed = false;
			}
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		if (_ᢳ)
		{
			SwitchPressed = false;
			if (__18AD && (int)objArg.Button == 1048576 && _ᐨ == objArg.Location)
			{
				SetValue(!Value, eEventSource.Mouse);
			}
		}
		base.InternalMouseUp(objArg);
	}

	public void SetValueAndAnimate(bool value, eEventSource source)
	{
		if (!IsAnimationEnabled)
		{
			SetValue(value, source);
			return;
		}
		BackgroundWorker ᢺ = _ᢺ;
		if (ᢺ != null)
		{
			ᢺ.CancelAsync();
			SetValue(value, source);
		}
		else if (Value != value)
		{
			_ᢷ = value;
			_ᢸ = source;
			_ᢹ = __18AF.Width - Dpi.Width(SwitchWidth);
			_ᢺ = new BackgroundWorker();
			_ᢺ.WorkerSupportsCancellation = true;
			_ᢺ.DoWork += ᣍ;
			_ᢺ.RunWorkerCompleted += ᣌ;
			_ᢺ.RunWorkerAsync();
		}
	}

	private void ᣌ(object _٠, RunWorkerCompletedEventArgs ؿ)
	{
		BackgroundWorker ᢺ = _ᢺ;
		_ᢺ = null;
		ᢺ?.Dispose();
		if (!ؿ.Cancelled)
		{
			SetValue(_ᢷ, _ᢸ);
		}
	}

	private void ᣍ(object _٠, DoWorkEventArgs ؿ)
	{
		DateTime now = DateTime.Now;
		int num = Math.Max(9, _ᢹ / 5);
		for (int i = 0; i < _ᢹ; i += num)
		{
			if (_ᢺ.CancellationPending)
			{
				ؿ.Cancel = true;
				break;
			}
			ᢪ = i;
			try
			{
				using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
				manualResetEvent.WaitOne(40);
			}
			catch
			{
				break;
			}
			if (DateTime.Now.Subtract(now).TotalMilliseconds > 400.0)
			{
				break;
			}
		}
	}

	internal void ᣎ()
	{
		BackgroundWorker ᢺ = _ᢺ;
		if (ᢺ != null && ᢺ.IsBusy)
		{
			ᢺ.CancelAsync();
			if (Value != _ᢷ)
			{
				SetValue(_ᢷ, _ᢸ);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextPadding()
	{
		if (_ᢾ.Bottom == 0 && _ᢾ.Top == 0 && _ᢾ.Left == 0)
		{
			return _ᢾ.Right != 0;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetTextPadding()
	{
		_ᢾ = new Padding(0, 0, 0, 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMargin()
	{
		if (__0AE5.Bottom == 0 && __0AE5.Top == 0 && __0AE5.Left == 0)
		{
			return __0AE5.Right != 0;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetMargin()
	{
		__0AE5 = new Padding(0, 0, 0, 0);
	}

	private void ᣏ(object _٠, PropertyChangedEventArgs ؿ)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOffBackColor()
	{
		return !_ᢿ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOffBackColor()
	{
		OffBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOnBackColor()
	{
		return !_ᣀ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOnBackColor()
	{
		OnBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOnTextColor()
	{
		return !_ᣁ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOnTextColor()
	{
		OnTextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOffTextColor()
	{
		return !_ᣂ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOffTextColor()
	{
		OffTextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !_Ꮃ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSwitchBorderColor()
	{
		return !_ᣃ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSwitchBorderColor()
	{
		SwitchBorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSwitchBackColor()
	{
		return !_ᣄ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSwitchBackColor()
	{
		SwitchBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !__0CD1.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeReadOnlyMarkerColor()
	{
		return !_ᣊ.Equals((object?)ᣉ);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetReadOnlyMarkerColor()
	{
		ReadOnlyMarkerColor = ᣉ;
	}
}
