using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(SwitchButton), "Controls.SwitchButton.ico")]
[ToolboxItem(false)]
[ComVisible(false)]
[DefaultEvent("ValueChanged")]
[DefaultBindingProperty("Value")]
[DefaultProperty("Value")]
public class SwitchButton : BaseItemControl, ICommandSource
{
	private SwitchButtonItem _SwitchButton;

	private const string DefaultValueFalse = "N";

	private const string DefaultValueTrue = "Y";

	private object _ValueObject = "N";

	private object _ValueFalse = "N";

	private object _ValueTrue = "Y";

	private ICommand _Command;

	private object _CommandParameter;

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates switch value.")]
	[Bindable(true)]
	public bool Value
	{
		get
		{
			return _SwitchButton.Value;
		}
		set
		{
			_SwitchButton.Value = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates item border color.")]
	public Color BorderColor
	{
		get
		{
			return _SwitchButton.BorderColor;
		}
		set
		{
			_SwitchButton.BorderColor = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether state transition animation is enabled.")]
	public bool AnimationEnabled
	{
		get
		{
			return _SwitchButton.AnimationEnabled;
		}
		set
		{
			_SwitchButton.AnimationEnabled = value;
		}
	}

	protected override Size DefaultSize => new Size(66, 22);

	[Category("Appearance")]
	[Description("Indicates color of OFF state background.")]
	public Color OffBackColor
	{
		get
		{
			return _SwitchButton.OffBackColor;
		}
		set
		{
			_SwitchButton.OffBackColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of ON state background.")]
	public Color OnBackColor
	{
		get
		{
			return _SwitchButton.OnBackColor;
		}
		set
		{
			_SwitchButton.OnBackColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of ON state text.")]
	public Color OnTextColor
	{
		get
		{
			return _SwitchButton.OnTextColor;
		}
		set
		{
			_SwitchButton.OnTextColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of OFF state text.")]
	public Color OffTextColor
	{
		get
		{
			return _SwitchButton.OffTextColor;
		}
		set
		{
			_SwitchButton.OffTextColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates border color of the button switch.")]
	public Color SwitchBorderColor
	{
		get
		{
			return _SwitchButton.SwitchBorderColor;
		}
		set
		{
			_SwitchButton.SwitchBorderColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates background color of the switch button.")]
	public Color SwitchBackColor
	{
		get
		{
			return _SwitchButton.SwitchBackColor;
		}
		set
		{
			_SwitchButton.SwitchBackColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates text color.")]
	[Browsable(false)]
	public Color TextColor
	{
		get
		{
			return _SwitchButton.TextColor;
		}
		set
		{
			_SwitchButton.TextColor = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates font that is used to draw ON/OFF text on the switch button.")]
	public Font SwitchFont
	{
		get
		{
			return _SwitchButton.SwitchFont;
		}
		set
		{
			_SwitchButton.SwitchFont = value;
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
			return _SwitchButton.OnText;
		}
		set
		{
			_SwitchButton.OnText = value;
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
			return _SwitchButton.OffText;
		}
		set
		{
			_SwitchButton.OffText = value;
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
			return _SwitchButton.SwitchWidth;
		}
		set
		{
			_SwitchButton.SwitchWidth = value;
		}
	}

	[Browsable(false)]
	public Rectangle SwitchBounds => _SwitchButton.SwitchBounds;

	[Browsable(false)]
	public Rectangle OnPartBounds => _SwitchButton.OnPartBounds;

	[Browsable(false)]
	public Rectangle OffPartBounds => _SwitchButton.OffPartBounds;

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether button is in read-only state meaning that it appears as enabled but user cannot change its state.")]
	public bool IsReadOnly
	{
		get
		{
			return _SwitchButton.IsReadOnly;
		}
		set
		{
			_SwitchButton.IsReadOnly = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether lock marker is visible on face of the control when IsReadOnly is set to true.")]
	public bool ShowReadOnlyMarker
	{
		get
		{
			return _SwitchButton.ShowReadOnlyMarker;
		}
		set
		{
			_SwitchButton.ShowReadOnlyMarker = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of read-only marker.")]
	public Color ReadOnlyMarkerColor
	{
		get
		{
			return _SwitchButton.ReadOnlyMarkerColor;
		}
		set
		{
			_SwitchButton.ReadOnlyMarkerColor = value;
		}
	}

	[DefaultValue("N")]
	[Bindable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(StringConverter))]
	[Description("Indicates object that represents the Value state of control.")]
	public object ValueObject
	{
		get
		{
			return _ValueObject;
		}
		set
		{
			_ValueObject = value;
			OnValueObjectChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue("Y")]
	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the True state of Value when ValueObject property is set to that value.")]
	public object ValueTrue
	{
		get
		{
			return _ValueTrue;
		}
		set
		{
			_ValueTrue = value;
			OnValueObjectChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue("N")]
	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the False state of control when ValueObject property is set to that value.")]
	public object ValueFalse
	{
		get
		{
			return _ValueFalse;
		}
		set
		{
			_ValueFalse = value;
			OnValueObjectChanged();
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

	public event EventHandler ValueChanging;

	public event EventHandler ValueChanged;

	[Description("Occurs after ValueObject property changes.")]
	public event EventHandler ValueObjectChanged;

	protected virtual void OnValueChanging(EventArgs e)
	{
		this.ValueChanging?.Invoke(this, e);
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		this.ValueChanged?.Invoke(this, e);
	}

	public SwitchButton()
	{
		((Control)this).SetStyle((ControlStyles)512, true);
		_SwitchButton = new SwitchButtonItem();
		_SwitchButton.TextVisible = false;
		_SwitchButton.ValueChanging += SwitchButtonItemValueChanging;
		_SwitchButton.ValueChanged += SwitchButtonItemValueChanged;
		HostItem = _SwitchButton;
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
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (_SwitchButton != null)
		{
			SwitchButtonItem switchButton = _SwitchButton;
			int width = ((Control)this).Width;
			Padding padding = ((Control)this).Padding;
			switchButton.ButtonWidth = Math.Max(4, width - ((Padding)(ref padding)).Horizontal);
			SwitchButtonItem switchButton2 = _SwitchButton;
			int height = ((Control)this).Height;
			padding = ((Control)this).Padding;
			switchButton2.ButtonHeight = Math.Max(4, height - ((Padding)(ref padding)).Vertical);
			base.RecalcLayout();
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && ((Control)this).Enabled && !IsReadOnly)
		{
			_SwitchButton.SetValueAndAnimate(!_SwitchButton.Value, eEventSource.Keyboard);
		}
		base.OnKeyDown(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return _SwitchButton.ShouldSerializeBorderColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void CancelAnimation()
	{
		_SwitchButton.CancelAnimation();
	}

	private void SwitchButtonItemValueChanged(object sender, EventArgs e)
	{
		_ValueObject = GetValueObject(Value);
		ExecuteCommand();
		OnValueChanged(e);
		OnValueObjectChanged(e);
	}

	private void SwitchButtonItemValueChanging(object sender, EventArgs e)
	{
		OnValueChanging(e);
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
		Padding margin = _SwitchButton.Margin;
		Padding padding = ((Control)this).Padding;
		margin.Bottom = ((Padding)(ref padding)).Bottom;
		Padding margin2 = _SwitchButton.Margin;
		padding = ((Control)this).Padding;
		margin2.Top = ((Padding)(ref padding)).Top;
		Padding margin3 = _SwitchButton.Margin;
		padding = ((Control)this).Padding;
		margin3.Left = ((Padding)(ref padding)).Left;
		Padding margin4 = _SwitchButton.Margin;
		padding = ((Control)this).Padding;
		margin4.Right = ((Padding)(ref padding)).Right;
		((Control)this).OnPaddingChanged(e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOffBackColor()
	{
		return _SwitchButton.ShouldSerializeOffBackColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOffBackColor()
	{
		OffBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOnBackColor()
	{
		return _SwitchButton.ShouldSerializeOnBackColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOnBackColor()
	{
		OnBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOnTextColor()
	{
		return _SwitchButton.ShouldSerializeOnTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOnTextColor()
	{
		OnTextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeOffTextColor()
	{
		return _SwitchButton.ShouldSerializeOffTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetOffTextColor()
	{
		OffTextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSwitchBorderColor()
	{
		return _SwitchButton.ShouldSerializeSwitchBorderColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSwitchBorderColor()
	{
		SwitchBorderColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSwitchBackColor()
	{
		return _SwitchButton.ShouldSerializeSwitchBackColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSwitchBackColor()
	{
		SwitchBackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return _SwitchButton.ShouldSerializeTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	public void SetValueAndAnimate(bool value, eEventSource source)
	{
		_SwitchButton.SetValueAndAnimate(value, source);
	}

	public void SetValue(bool newValue, eEventSource source)
	{
		_SwitchButton.SetValue(newValue, source);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeReadOnlyMarkerColor()
	{
		return _SwitchButton.ShouldSerializeReadOnlyMarkerColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetReadOnlyMarkerColor()
	{
		_SwitchButton.ResetReadOnlyMarkerColor();
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (_SwitchButton != null)
		{
			return _SwitchButton.GetPreferredSize();
		}
		return ((Control)this).GetPreferredSize(proposedSize);
	}

	protected virtual void OnValueObjectChanged(EventArgs e)
	{
		this.ValueObjectChanged?.Invoke(this, e);
	}

	private void OnValueObjectChanged()
	{
		bool value = GetValue(_ValueObject);
		Value = value;
	}

	private bool IsNull(object value)
	{
		if (value != null && (!(value is string) || !((string)value == string.Empty)))
		{
			return value == DBNull.Value;
		}
		return true;
	}

	private object GetValueObject(bool value)
	{
		if (!value)
		{
			return _ValueFalse;
		}
		return _ValueTrue;
	}

	private bool GetValue(object value)
	{
		if (_ValueTrue != null && _ValueTrue.Equals(value))
		{
			return true;
		}
		if ((_ValueFalse == null && IsNull(value)) || (_ValueFalse != null && _ValueFalse.Equals(value)))
		{
			return false;
		}
		if (value is SqlBoolean sqlBoolean)
		{
			return sqlBoolean.IsTrue;
		}
		if (value is int?)
		{
			if (value == null)
			{
				return false;
			}
			if (((int?)value).Value == 0)
			{
				return false;
			}
			return true;
		}
		if (value is int)
		{
			if ((int)value != 0)
			{
				return true;
			}
			return false;
		}
		if (value is long)
		{
			if ((long)value != 0L)
			{
				return true;
			}
			return false;
		}
		if (value is short)
		{
			if ((short)value != 0)
			{
				return true;
			}
			return false;
		}
		if (value is float)
		{
			if ((float)value != 0f)
			{
				return true;
			}
			return false;
		}
		if (value is double)
		{
			if ((double)value != 0.0)
			{
				return true;
			}
			return false;
		}
		if (value is byte)
		{
			if ((byte)value != 0)
			{
				return true;
			}
			return false;
		}
		if (value is uint)
		{
			if ((uint)value != 0)
			{
				return true;
			}
			return false;
		}
		if (value is ulong)
		{
			if ((ulong)value != 0L)
			{
				return true;
			}
			return false;
		}
		if (value is bool)
		{
			return (bool)value;
		}
		return false;
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
