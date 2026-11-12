using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Events;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[DefaultEvent("ValueChanged")]
[DefaultProperty("Value")]
public class SwitchButtonItem : BaseItem
{
	internal static readonly int TextButtonSpacing = 3;

	private bool _EnableMarkup = true;

	private bool _Value;

	private Point _MouseDownPoint = Point.Empty;

	private Rectangle _SwitchBounds;

	private Rectangle _ButtonBounds;

	private Rectangle _OnPartBounds;

	private Rectangle _OffPartBounds;

	private int _SwitchOffset;

	private bool _SwitchPressed;

	private int _SwitchWidth = 28;

	private string _OnText = "ON";

	private string _OffText = "OFF";

	private bool _AsyncValue;

	private eEventSource _AsyncSource;

	private int _AsyncTotalSwitchOffset;

	private BackgroundWorker _AnimationWorker;

	private bool _AnimationEnabled = true;

	private int _ButtonWidth = 66;

	private int _ButtonHeight = 22;

	private Padding _TextPadding = new Padding(0, 0, 0, 0);

	private Padding _Margin = new Padding(0, 0, 0, 0);

	private Color _OffBackColor = Color.Empty;

	private Color _OnBackColor = Color.Empty;

	private Color _OnTextColor = Color.Empty;

	private Color _OffTextColor = Color.Empty;

	private Color _BorderColor = Color.Empty;

	private Color _SwitchBorderColor = Color.Empty;

	private Color _SwitchBackColor = Color.Empty;

	private bool _TextVisible = true;

	private Color _TextColor = Color.Empty;

	private Font _SwitchFont;

	private bool _IsReadOnly;

	private bool _ShowReadOnlyMarker = true;

	private static readonly Color DefaultReadOnlyMarkerColor = ColorScheme.GetColor(12603469);

	private Color _ReadOnlyMarkerColor = DefaultReadOnlyMarkerColor;

	protected override bool IsMarkupSupported => _EnableMarkup;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _EnableMarkup;
		}
		set
		{
			if (_EnableMarkup != value)
			{
				_EnableMarkup = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	[Browsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	[DefaultValue("")]
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
	[Category("Appearance")]
	[Description("Indicates switch value.")]
	[Bindable(true)]
	public bool Value
	{
		get
		{
			return _Value;
		}
		set
		{
			if (_Value != value)
			{
				SetValue(value, eEventSource.Code);
			}
		}
	}

	[Browsable(false)]
	public Rectangle SwitchBounds
	{
		get
		{
			return _SwitchBounds;
		}
		internal set
		{
			_SwitchBounds = value;
		}
	}

	[Browsable(false)]
	public Rectangle ButtonBounds
	{
		get
		{
			return _ButtonBounds;
		}
		internal set
		{
			_ButtonBounds = value;
		}
	}

	[Browsable(false)]
	public Rectangle OnPartBounds
	{
		get
		{
			return _OnPartBounds;
		}
		internal set
		{
			_OnPartBounds = value;
		}
	}

	[Browsable(false)]
	public Rectangle OffPartBounds
	{
		get
		{
			return _OffPartBounds;
		}
		internal set
		{
			_OffPartBounds = value;
		}
	}

	internal int SwitchOffset
	{
		get
		{
			return _SwitchOffset;
		}
		set
		{
			_SwitchOffset = value;
			Refresh();
		}
	}

	[Browsable(false)]
	public bool SwitchPressed
	{
		get
		{
			return _SwitchPressed;
		}
		internal set
		{
			_SwitchPressed = value;
			SwitchOffset = 0;
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
			return _SwitchWidth;
		}
		set
		{
			if (value < 6)
			{
				value = 6;
			}
			if (_SwitchWidth != value)
			{
				_SwitchWidth = value;
				Refresh();
			}
		}
	}

	[DefaultValue("ON")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates text that is displayed on switch when Value property is set to true.")]
	public string OnText
	{
		get
		{
			return _OnText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_OnText != value)
			{
				_OnText = value;
				Refresh();
			}
		}
	}

	[DefaultValue("OFF")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates text that is displayed on switch when Value property is set to true.")]
	public string OffText
	{
		get
		{
			return _OffText;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (_OffText != value)
			{
				_OffText = value;
				Refresh();
			}
		}
	}

	protected virtual bool IsAnimationEnabled => _AnimationEnabled;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether state transition animation is enabled.")]
	public bool AnimationEnabled
	{
		get
		{
			return _AnimationEnabled;
		}
		set
		{
			_AnimationEnabled = value;
		}
	}

	[DefaultValue(66)]
	[Category("Appearance")]
	[Description("Indicates width of the switch button. Must be greater than SwitchWidth.")]
	public int ButtonWidth
	{
		get
		{
			return _ButtonWidth;
		}
		set
		{
			if (value < _SwitchWidth)
			{
				value = _SwitchWidth + 1;
			}
			_ButtonWidth = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[DefaultValue(22)]
	[Category("Appearance")]
	[Description("Indicates height of the switch button. Must be greater than 5.")]
	public int ButtonHeight
	{
		get
		{
			return _ButtonHeight;
		}
		set
		{
			if (value < 6)
			{
				value = 6;
			}
			_ButtonHeight = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets text padding.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Padding TextPadding => _TextPadding;

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets switch margin.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public new Padding Margin => _Margin;

	[Category("Appearance")]
	[Description("Indicates color of OFF state background.")]
	public Color OffBackColor
	{
		get
		{
			return _OffBackColor;
		}
		set
		{
			_OffBackColor = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of ON state background.")]
	public Color OnBackColor
	{
		get
		{
			return _OnBackColor;
		}
		set
		{
			_OnBackColor = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of ON state text.")]
	public Color OnTextColor
	{
		get
		{
			return _OnTextColor;
		}
		set
		{
			_OnTextColor = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of OFF state text.")]
	public Color OffTextColor
	{
		get
		{
			return _OffTextColor;
		}
		set
		{
			_OffTextColor = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates item border color.")]
	public Color BorderColor
	{
		get
		{
			return _BorderColor;
		}
		set
		{
			_BorderColor = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates border color of the button switch.")]
	public Color SwitchBorderColor
	{
		get
		{
			return _SwitchBorderColor;
		}
		set
		{
			_SwitchBorderColor = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates background color of the switch button.")]
	public Color SwitchBackColor
	{
		get
		{
			return _SwitchBackColor;
		}
		set
		{
			_SwitchBackColor = value;
			Refresh();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether caption/label set using Text property is visible.")]
	public bool TextVisible
	{
		get
		{
			return _TextVisible;
		}
		set
		{
			_TextVisible = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates text color.")]
	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			_TextColor = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates font that is used to draw ON/OFF text on the switch button.")]
	public Font SwitchFont
	{
		get
		{
			return _SwitchFont;
		}
		set
		{
			_SwitchFont = value;
			Refresh();
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether button is in read-only state meaning that it appears as enabled but user cannot change its state.")]
	public bool IsReadOnly
	{
		get
		{
			return _IsReadOnly;
		}
		set
		{
			_IsReadOnly = value;
			Refresh();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether lock marker is visible on face of the control when IsReadOnly is set to true.")]
	public bool ShowReadOnlyMarker
	{
		get
		{
			return _ShowReadOnlyMarker;
		}
		set
		{
			_ShowReadOnlyMarker = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of read-only marker.")]
	public Color ReadOnlyMarkerColor
	{
		get
		{
			return _ReadOnlyMarkerColor;
		}
		set
		{
			_ReadOnlyMarkerColor = value;
			Refresh();
		}
	}

	public event EventHandler ValueChanging;

	public event EventHandler ValueChanged;

	protected virtual void OnValueChanging(CancelableEventSourceArgs e)
	{
		this.ValueChanging?.Invoke(this, e);
	}

	protected virtual void OnValueChanged(EventSourceArgs e)
	{
		this.ValueChanged?.Invoke(this, e);
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
		_Margin.PropertyChanged += MarginPropertyChanged;
	}

	public override BaseItem Copy()
	{
		SwitchButtonItem switchButtonItem = new SwitchButtonItem(m_Name);
		CopyToItem(switchButtonItem);
		return switchButtonItem;
	}

	internal void InternalCopyToItem(SwitchButtonItem copy)
	{
		CopyToItem(copy);
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
		CancelAnimation();
		base.Dispose(disposing);
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer renderer = p.Renderer;
		if (renderer != null)
		{
			SwitchButtonRenderEventArgs switchButtonRenderEventArgs = new SwitchButtonRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
			switchButtonRenderEventArgs.ItemPaintArgs = p;
			renderer.DrawSwitchButton(switchButtonRenderEventArgs);
		}
		else
		{
			SwitchButtonPainter switchButtonPainter = PainterFactory.CreateSwitchButtonPainter(this);
			if (switchButtonPainter != null)
			{
				SwitchButtonRenderEventArgs switchButtonRenderEventArgs2 = new SwitchButtonRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
				switchButtonRenderEventArgs2.ItemPaintArgs = p;
				switchButtonPainter.Paint(switchButtonRenderEventArgs2);
			}
		}
		DrawInsertMarker(p.Graphics);
	}

	public override void RecalcSize()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		Size size = new Size(_ButtonWidth + _Margin.Horizontal, _ButtonHeight + _Margin.Vertical);
		if (_TextVisible && !string.IsNullOrEmpty(Text))
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Font font = val.Font;
				Graphics val2 = val.CreateGraphics();
				try
				{
					Size size2 = ButtonItemLayout.MeasureItemText(this, val2, 0, font, eTextFormat.WordBreak, (int)val.RightToLeft == 1);
					size2.Width += _TextPadding.Horizontal + TextButtonSpacing;
					size2.Height += _TextPadding.Vertical;
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

	internal Size GetPreferredSize()
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Invalid comparison between Unknown and I4
		Size result = new Size(_SwitchWidth + _Margin.Horizontal, 8);
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			Font font = val.Font;
			Graphics val2 = val.CreateGraphics();
			try
			{
				Size size = TextDrawing.MeasureString(val2, _OnText, font);
				Size size2 = TextDrawing.MeasureString(val2, _OffText, font);
				result.Width += Math.Max(size.Width, size2.Width) + 8;
				result.Height = Math.Max(result.Height, size2.Height + _Margin.Vertical + 4);
				if (_TextVisible && !string.IsNullOrEmpty(Text))
				{
					Size size3 = ButtonItemLayout.MeasureItemText(this, val2, 0, font, eTextFormat.WordBreak, (int)val.RightToLeft == 1);
					size3.Width += _TextPadding.Horizontal + TextButtonSpacing;
					size3.Height += _TextPadding.Vertical;
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
			SwitchOffset = 0;
			Refresh();
			return;
		}
		_Value = newValue;
		_AsyncValue = newValue;
		if (ShouldSyncProperties)
		{
			BarFunctions.SyncProperty(this, "Value");
		}
		SwitchOffset = 0;
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
		if (Enabled && !_IsReadOnly && !DesignMode && (int)objArg.Button == 1048576)
		{
			if (!Value && _OffPartBounds.Contains(objArg.X, objArg.Y))
			{
				SetValueAndAnimate(value: true, eEventSource.Mouse);
			}
			else if (Value && _OnPartBounds.Contains(objArg.X, objArg.Y))
			{
				SetValueAndAnimate(value: false, eEventSource.Mouse);
			}
			else if (_SwitchBounds.Contains(objArg.X, objArg.Y))
			{
				_MouseDownPoint = new Point(objArg.X, objArg.Y);
				SwitchPressed = true;
			}
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		if (_SwitchPressed)
		{
			if ((int)objArg.Button == 1048576)
			{
				Point mouseDownPoint = _MouseDownPoint;
				int num = objArg.X - mouseDownPoint.X;
				if (num != 0)
				{
					if (!Value && num > 0)
					{
						if (_SwitchBounds.Right + num >= _ButtonBounds.Right)
						{
							SetValue(newValue: true, eEventSource.Mouse);
							_MouseDownPoint = new Point(objArg.X, objArg.Y);
						}
						else
						{
							SwitchOffset = num;
						}
					}
					else if (Value && num < 0)
					{
						if (_SwitchBounds.X + num <= _ButtonBounds.X)
						{
							SetValue(newValue: false, eEventSource.Mouse);
							_MouseDownPoint = new Point(objArg.X, objArg.Y);
						}
						else
						{
							SwitchOffset = Math.Abs(num);
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
		if (_SwitchPressed)
		{
			SwitchPressed = false;
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
		BackgroundWorker animationWorker = _AnimationWorker;
		if (animationWorker != null)
		{
			animationWorker.CancelAsync();
			SetValue(value, source);
		}
		else if (Value != value)
		{
			_AsyncValue = value;
			_AsyncSource = source;
			_AsyncTotalSwitchOffset = _ButtonBounds.Width - SwitchWidth;
			_AnimationWorker = new BackgroundWorker();
			_AnimationWorker.WorkerSupportsCancellation = true;
			_AnimationWorker.DoWork += AnimationWorkerDoWork;
			_AnimationWorker.RunWorkerCompleted += AnimationWorkerRunWorkerCompleted;
			_AnimationWorker.RunWorkerAsync();
		}
	}

	private void AnimationWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		BackgroundWorker animationWorker = _AnimationWorker;
		_AnimationWorker = null;
		animationWorker?.Dispose();
		if (!e.Cancelled)
		{
			SetValue(_AsyncValue, _AsyncSource);
		}
	}

	private void AnimationWorkerDoWork(object sender, DoWorkEventArgs e)
	{
		DateTime now = DateTime.Now;
		int num = Math.Max(9, _AsyncTotalSwitchOffset / 5);
		for (int i = 0; i < _AsyncTotalSwitchOffset; i += num)
		{
			if (_AnimationWorker.CancellationPending)
			{
				e.Cancel = true;
				break;
			}
			SwitchOffset = i;
			Thread.Sleep(40);
			if (DateTime.Now.Subtract(now).TotalMilliseconds > 400.0)
			{
				break;
			}
		}
	}

	internal void CancelAnimation()
	{
		BackgroundWorker animationWorker = _AnimationWorker;
		if (animationWorker != null && animationWorker.IsBusy)
		{
			animationWorker.CancelAsync();
			if (Value != _AsyncValue)
			{
				SetValue(_AsyncValue, _AsyncSource);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextPadding()
	{
		if (_TextPadding.Bottom == 0 && _TextPadding.Top == 0 && _TextPadding.Left == 0)
		{
			return _TextPadding.Right != 0;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetTextPadding()
	{
		_TextPadding = new Padding(0, 0, 0, 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMargin()
	{
		if (_Margin.Bottom == 0 && _Margin.Top == 0 && _Margin.Left == 0)
		{
			return _Margin.Right != 0;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetMargin()
	{
		_Margin = new Padding(0, 0, 0, 0);
	}

	private void MarginPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOffBackColor()
	{
		return !_OffBackColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOffBackColor()
	{
		OffBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOnBackColor()
	{
		return !_OnBackColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOnBackColor()
	{
		OnBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOnTextColor()
	{
		return !_OnTextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOnTextColor()
	{
		OnTextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOffTextColor()
	{
		return !_OffTextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOffTextColor()
	{
		OffTextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !_BorderColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSwitchBorderColor()
	{
		return !_SwitchBorderColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSwitchBorderColor()
	{
		SwitchBorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSwitchBackColor()
	{
		return !_SwitchBackColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSwitchBackColor()
	{
		SwitchBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !_TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeReadOnlyMarkerColor()
	{
		return !_ReadOnlyMarkerColor.Equals((object?)DefaultReadOnlyMarkerColor);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetReadOnlyMarkerColor()
	{
		ReadOnlyMarkerColor = DefaultReadOnlyMarkerColor;
	}
}
