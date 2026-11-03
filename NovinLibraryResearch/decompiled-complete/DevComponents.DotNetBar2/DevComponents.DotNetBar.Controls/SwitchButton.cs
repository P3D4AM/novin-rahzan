using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(SwitchButton), "Controls.SwitchButton.ico")]
[DefaultProperty("Value")]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.SwitchButtonDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ComVisible(false)]
[DefaultEvent("ValueChanged")]
[DefaultBindingProperty("Value")]
public class SwitchButton : BaseItemControl, ICommandSource
{
	private const string _2EFA = "N";

	private const string _2EFB = "Y";

	private EventHandler _18AB;

	private EventHandler ᗲ;

	private SwitchButtonItem __2EFC;

	private object __2EFD = "N";

	private object __2EFE = "N";

	private object __2EFF = "Y";

	private EventHandler _៤;

	private ICommand _૦;

	private object _૧;

	[DefaultValue(false)]
	[Category("Appearance")]
	[Bindable(true)]
	[Description("Indicates switch value.")]
	public bool Value
	{
		get
		{
			return __2EFC.Value;
		}
		set
		{
			__2EFC.Value = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates item border color.")]
	public Color BorderColor
	{
		get
		{
			return __2EFC.BorderColor;
		}
		set
		{
			__2EFC.BorderColor = value;
		}
	}

	[Description("Indicates whether state transition animation is enabled.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool AnimationEnabled
	{
		get
		{
			return __2EFC.AnimationEnabled;
		}
		set
		{
			__2EFC.AnimationEnabled = value;
		}
	}

	protected override Size DefaultSize => new Size(66, 22);

	[Description("Indicates color of OFF state background.")]
	[Category("Appearance")]
	public Color OffBackColor
	{
		get
		{
			return __2EFC.OffBackColor;
		}
		set
		{
			__2EFC.OffBackColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of ON state background.")]
	public Color OnBackColor
	{
		get
		{
			return __2EFC.OnBackColor;
		}
		set
		{
			__2EFC.OnBackColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of ON state text.")]
	public Color OnTextColor
	{
		get
		{
			return __2EFC.OnTextColor;
		}
		set
		{
			__2EFC.OnTextColor = value;
		}
	}

	[Description("Indicates color of OFF state text.")]
	[Category("Appearance")]
	public Color OffTextColor
	{
		get
		{
			return __2EFC.OffTextColor;
		}
		set
		{
			__2EFC.OffTextColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates border color of the button switch.")]
	public Color SwitchBorderColor
	{
		get
		{
			return __2EFC.SwitchBorderColor;
		}
		set
		{
			__2EFC.SwitchBorderColor = value;
		}
	}

	[Description("Indicates background color of the switch button.")]
	[Category("Appearance")]
	public Color SwitchBackColor
	{
		get
		{
			return __2EFC.SwitchBackColor;
		}
		set
		{
			__2EFC.SwitchBackColor = value;
		}
	}

	[Browsable(false)]
	[Description("Indicates text color.")]
	[Category("Appearance")]
	public Color TextColor
	{
		get
		{
			return __2EFC.TextColor;
		}
		set
		{
			__2EFC.TextColor = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates font that is used to draw ON/OFF text on the switch button.")]
	[Category("Appearance")]
	public Font SwitchFont
	{
		get
		{
			return __2EFC.SwitchFont;
		}
		set
		{
			__2EFC.SwitchFont = value;
		}
	}

	[Description("Indicates text that is displayed on switch when Value property is set to true.")]
	[Category("Appearance")]
	[DefaultValue("ON")]
	[Localizable(true)]
	public string OnText
	{
		get
		{
			return __2EFC.OnText;
		}
		set
		{
			__2EFC.OnText = value;
		}
	}

	[Description("Indicates text that is displayed on switch when Value property is set to true.")]
	[Category("Appearance")]
	[Localizable(true)]
	[DefaultValue("OFF")]
	public string OffText
	{
		get
		{
			return __2EFC.OffText;
		}
		set
		{
			__2EFC.OffText = value;
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

	[DefaultValue(28)]
	[Category("Appearance")]
	[Description("Indicates width in pixels of the switch part of the button.")]
	public int SwitchWidth
	{
		get
		{
			return __2EFC.SwitchWidth;
		}
		set
		{
			__2EFC.SwitchWidth = value;
		}
	}

	[Browsable(false)]
	public Rectangle SwitchBounds => __2EFC.SwitchBounds;

	[Browsable(false)]
	public Rectangle OnPartBounds => __2EFC.OnPartBounds;

	[Browsable(false)]
	public Rectangle OffPartBounds => __2EFC.OffPartBounds;

	[Description("Indicates whether button is in read-only state meaning that it appears as enabled but user cannot change its state.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool IsReadOnly
	{
		get
		{
			return __2EFC.IsReadOnly;
		}
		set
		{
			__2EFC.IsReadOnly = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether lock marker is visible on face of the control when IsReadOnly is set to true.")]
	[Category("Appearance")]
	public bool ShowReadOnlyMarker
	{
		get
		{
			return __2EFC.ShowReadOnlyMarker;
		}
		set
		{
			__2EFC.ShowReadOnlyMarker = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of read-only marker.")]
	public Color ReadOnlyMarkerColor
	{
		get
		{
			return __2EFC.ReadOnlyMarkerColor;
		}
		set
		{
			__2EFC.ReadOnlyMarkerColor = value;
		}
	}

	[Description("Indicates whether clicking left mouse button on the switch part of the item will toggle the switch Value.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool SwitchClickTogglesValue
	{
		get
		{
			return __2EFC.SwitchClickTogglesValue;
		}
		set
		{
			__2EFC.SwitchClickTogglesValue = value;
		}
	}

	[DefaultValue("N")]
	[Description("Indicates object that represents the Value state of control.")]
	[RefreshProperties(RefreshProperties.All)]
	[Bindable(true)]
	[TypeConverter(typeof(StringConverter))]
	public object ValueObject
	{
		get
		{
			return __2EFD;
		}
		set
		{
			__2EFD = value;
			_2F02();
		}
	}

	[Description("Represents the True state of Value when ValueObject property is set to that value.")]
	[Browsable(true)]
	[TypeConverter(typeof(StringConverter))]
	[Category("Behavior")]
	[DefaultValue("Y")]
	public object ValueTrue
	{
		get
		{
			return __2EFF;
		}
		set
		{
			__2EFF = value;
			_2F02();
		}
	}

	[Description("Represents the False state of control when ValueObject property is set to that value.")]
	[Category("Behavior")]
	[DefaultValue("N")]
	[Browsable(true)]
	[TypeConverter(typeof(StringConverter))]
	public object ValueFalse
	{
		get
		{
			return __2EFE;
		}
		set
		{
			__2EFE = value;
			_2F02();
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

	[Localizable(true)]
	[TypeConverter(typeof(StringConverter))]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
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

	[Description("Occurs after ValueObject property changes.")]
	public event EventHandler ValueObjectChanged
	{
		add
		{
			EventHandler eventHandler = _៤;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៤, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _៤;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _៤, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnValueChanging(EventArgs e)
	{
		_18AB?.Invoke(this, e);
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		ᗲ?.Invoke(this, e);
	}

	public SwitchButton()
	{
		((Control)this).SetStyle((ControlStyles)512, true);
		__2EFC = new SwitchButtonItem();
		__2EFC.TextVisible = false;
		__2EFC.ValueChanging += _2F01;
		__2EFC.ValueChanged += _2F00;
		HostItem = __2EFC;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		RecalcLayout();
		((Control)this).OnHandleCreated(e);
	}

	public override void RecalcLayout()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (__2EFC != null)
		{
			SwitchButtonItem switchButtonItem = __2EFC;
			int width = ((Control)this).Width;
			Padding padding = ((Control)this).Padding;
			switchButtonItem.ButtonWidth = Math.Max(4, Dpi.DescaleWidth(width - ((Padding)(ref padding)).Horizontal));
			SwitchButtonItem switchButtonItem2 = __2EFC;
			int height = ((Control)this).Height;
			Padding padding2 = ((Control)this).Padding;
			switchButtonItem2.ButtonHeight = Math.Max(4, Dpi.DescaleHeight(height - ((Padding)(ref padding2)).Vertical));
			base.RecalcLayout();
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && ((Control)this).Enabled && !IsReadOnly)
		{
			__2EFC.SetValueAndAnimate(!__2EFC.Value, eEventSource.Keyboard);
		}
		base.OnKeyDown(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return __2EFC.ShouldSerializeBorderColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void CancelAnimation()
	{
		__2EFC.ᣎ();
	}

	private void _2F00(object _٠, EventArgs ؿ)
	{
		__2EFD = _2F03(Value);
		ExecuteCommand();
		OnValueChanged(ؿ);
		OnValueObjectChanged(ؿ);
	}

	private void _2F01(object _٠, EventArgs ؿ)
	{
		OnValueChanging(ؿ);
	}

	protected override void OnPaddingChanged(EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		Padding margin = __2EFC.Margin;
		Padding padding = ((Control)this).Padding;
		margin.Bottom = ((Padding)(ref padding)).Bottom;
		Padding margin2 = __2EFC.Margin;
		Padding padding2 = ((Control)this).Padding;
		margin2.Top = ((Padding)(ref padding2)).Top;
		Padding margin3 = __2EFC.Margin;
		Padding padding3 = ((Control)this).Padding;
		margin3.Left = ((Padding)(ref padding3)).Left;
		Padding margin4 = __2EFC.Margin;
		Padding padding4 = ((Control)this).Padding;
		margin4.Right = ((Padding)(ref padding4)).Right;
		((Control)this).OnPaddingChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOffBackColor()
	{
		return __2EFC.ShouldSerializeOffBackColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOffBackColor()
	{
		OffBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOnBackColor()
	{
		return __2EFC.ShouldSerializeOnBackColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOnBackColor()
	{
		OnBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOnTextColor()
	{
		return __2EFC.ShouldSerializeOnTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOnTextColor()
	{
		OnTextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOffTextColor()
	{
		return __2EFC.ShouldSerializeOffTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOffTextColor()
	{
		OffTextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSwitchBorderColor()
	{
		return __2EFC.ShouldSerializeSwitchBorderColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSwitchBorderColor()
	{
		SwitchBorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSwitchBackColor()
	{
		return __2EFC.ShouldSerializeSwitchBackColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSwitchBackColor()
	{
		SwitchBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return __2EFC.ShouldSerializeTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	public void SetValueAndAnimate(bool value, eEventSource source)
	{
		__2EFC.SetValueAndAnimate(value, source);
	}

	public void SetValue(bool newValue, eEventSource source)
	{
		__2EFC.SetValue(newValue, source);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeReadOnlyMarkerColor()
	{
		return __2EFC.ShouldSerializeReadOnlyMarkerColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetReadOnlyMarkerColor()
	{
		__2EFC.ResetReadOnlyMarkerColor();
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (__2EFC != null)
		{
			return __2EFC.ᣋ();
		}
		return ((Control)this).GetPreferredSize(proposedSize);
	}

	protected virtual void OnValueObjectChanged(EventArgs e)
	{
		_៤?.Invoke(this, e);
	}

	private void _2F02()
	{
		bool value = _239B(__2EFD);
		Value = value;
	}

	private bool _1B3F(object ԝ)
	{
		if (ԝ != null && (!(ԝ is string) || !((string)ԝ == string.Empty)))
		{
			return ԝ == DBNull.Value;
		}
		return true;
	}

	private object _2F03(bool ԝ)
	{
		if (!ԝ)
		{
			return __2EFE;
		}
		return __2EFF;
	}

	private bool _239B(object ԝ)
	{
		if (__2EFF != null && __2EFF.Equals(ԝ))
		{
			return true;
		}
		if ((__2EFE == null && _1B3F(ԝ)) || (__2EFE != null && __2EFE.Equals(ԝ)))
		{
			return false;
		}
		if (ԝ is SqlBoolean sqlBoolean)
		{
			return sqlBoolean.IsTrue;
		}
		if (ԝ is int?)
		{
			if (ԝ == null)
			{
				return false;
			}
			if (((int?)ԝ).Value == 0)
			{
				return false;
			}
			return true;
		}
		if (ԝ is int)
		{
			if ((int)ԝ != 0)
			{
				return true;
			}
			return false;
		}
		if (ԝ is long)
		{
			if ((long)ԝ != 0)
			{
				return true;
			}
			return false;
		}
		if (ԝ is short)
		{
			if ((short)ԝ != 0)
			{
				return true;
			}
			return false;
		}
		if (ԝ is float)
		{
			if ((float)ԝ != 0f)
			{
				return true;
			}
			return false;
		}
		if (ԝ is double)
		{
			if ((double)ԝ != 0.0)
			{
				return true;
			}
			return false;
		}
		if (ԝ is byte)
		{
			if ((byte)ԝ != 0)
			{
				return true;
			}
			return false;
		}
		if (ԝ is uint)
		{
			if ((uint)ԝ != 0)
			{
				return true;
			}
			return false;
		}
		if (ԝ is ulong)
		{
			if ((ulong)ԝ != 0)
			{
				return true;
			}
			return false;
		}
		if (ԝ is bool)
		{
			return (bool)ԝ;
		}
		return false;
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
