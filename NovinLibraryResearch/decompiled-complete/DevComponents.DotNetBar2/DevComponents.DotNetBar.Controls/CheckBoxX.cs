using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(CheckBoxX), "Controls.CheckBoxX.ico")]
[DefaultEvent("CheckedChanged")]
[Designer("DevComponents.DotNetBar.Design.CheckBoxXDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[ComVisible(false)]
public class CheckBoxX : BaseItemControl, ICommandSource
{
	private const string _23F6 = "N";

	private const string _23F7 = "Y";

	private const object _23F8 = null;

	private CheckBoxItem _23F9;

	private object _23FA = "N";

	private object _23FB = "N";

	private object _23FC = "Y";

	private object _23FD;

	private bool _23FE = true;

	private CheckBoxXChangeEventHandler ᢢ;

	private CheckBoxXChangeEventHandler _23FF;

	private EventHandler _0E65;

	private EventHandler _2400;

	private bool _ᒓ;

	private ICommand _૦;

	private object _૧;

	[Category("Appearance")]
	[Description("Indicates size of the check or radio sign. Default value is 13x13.")]
	public Size CheckSignSize
	{
		get
		{
			return _23F9.CheckSignSize;
		}
		set
		{
			_23F9.CheckSignSize = value;
			RecalcLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	[Category("Appearance")]
	public bool EnableMarkup
	{
		get
		{
			return _23F9.EnableMarkup;
		}
		set
		{
			_23F9.EnableMarkup = value;
		}
	}

	protected override MouseButtons MouseDownFocusButtons => (MouseButtons)32505856;

	[Description("Indicates appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.")]
	[Browsable(true)]
	[DefaultValue(eCheckBoxStyle.CheckBox)]
	[Category("Appearance")]
	public eCheckBoxStyle CheckBoxStyle
	{
		get
		{
			return _23F9.CheckBoxStyle;
		}
		set
		{
			_23F9.CheckBoxStyle = value;
		}
	}

	[Description("Indicates the check box position relative to the text.")]
	[Browsable(true)]
	[DefaultValue(eCheckBoxPosition.Left)]
	[Category("Appearance")]
	public eCheckBoxPosition CheckBoxPosition
	{
		get
		{
			return _23F9.CheckBoxPosition;
		}
		set
		{
			_23F9.CheckBoxPosition = value;
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether item is checked or not.")]
	[DevCoBrowsable(true)]
	[Bindable(true)]
	[Category("Appearance")]
	[Browsable(true)]
	public virtual bool Checked
	{
		get
		{
			return _23F9.Checked;
		}
		set
		{
			_23F9.Checked = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether text assigned to the check box is visible.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool TextVisible
	{
		get
		{
			return _23F9.TextVisible;
		}
		set
		{
			_23F9.TextVisible = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Description("Indicates text color.")]
	[Category("Appearance")]
	public Color TextColor
	{
		get
		{
			return _23F9.TextColor;
		}
		set
		{
			_23F9.TextColor = value;
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

	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates text associated with this button..")]
	[Localizable(true)]
	[Category("Appearance")]
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

	[Localizable(true)]
	[Browsable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public override bool AutoSize
	{
		get
		{
			return ((Control)this).AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				((Control)this).AutoSize = value;
				_21DF();
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether the CheckBox will allow three check states rather than two.")]
	[Browsable(true)]
	public bool ThreeState
	{
		get
		{
			return _23F9.ThreeState;
		}
		set
		{
			_23F9.ThreeState = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state")]
	[Category("Behavior")]
	public CheckState CheckState
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _23F9.CheckState;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_23F9.CheckState = value;
		}
	}

	internal CheckBoxItem _23F5 => _23F9;

	[Description("Indicates whether mnemonic character assigned to control is processed only if Alt key is pressed")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool EnableMnemonicWithAltKeyOnly
	{
		get
		{
			return _ᒓ;
		}
		set
		{
			_ᒓ = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether the Checked values and the item appearance are automatically changed when the Check-Box is clicked.")]
	[DefaultValue(true)]
	public bool AutoCheck
	{
		get
		{
			return _23F9.AutoCheck;
		}
		set
		{
			_23F9.AutoCheck = value;
		}
	}

	[DefaultValue("N")]
	[Description("Indicates object that represents the Checked state of control.")]
	[Bindable(true)]
	[TypeConverter(typeof(StringConverter))]
	[RefreshProperties(RefreshProperties.All)]
	public object CheckValue
	{
		get
		{
			return _23FA;
		}
		set
		{
			_23FA = value;
			_2402();
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether empty string is consider as null value during CheckValue value comparison.")]
	public bool ConsiderEmptyStringAsNull
	{
		get
		{
			return _23FE;
		}
		set
		{
			_23FE = value;
		}
	}

	[Description("Represents the Indeterminate state value of check box when CheckValue property is set to that value")]
	[TypeConverter(typeof(StringConverter))]
	[Category("Behavior")]
	[DefaultValue(null)]
	[Browsable(true)]
	public object CheckValueIndeterminate
	{
		get
		{
			return _23FD;
		}
		set
		{
			_23FD = value;
			_2402();
		}
	}

	[Description("Represents the Checked state value of check box when CheckValue property is set to that value")]
	[Browsable(true)]
	[DefaultValue("Y")]
	[TypeConverter(typeof(StringConverter))]
	[Category("Behavior")]
	public object CheckValueChecked
	{
		get
		{
			return _23FC;
		}
		set
		{
			_23FC = value;
			_2402();
		}
	}

	[Category("Behavior")]
	[DefaultValue("N")]
	[Browsable(true)]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the Unchecked state value of check box when CheckValue property is set to that value")]
	public object CheckValueUnchecked
	{
		get
		{
			return _23FB;
		}
		set
		{
			_23FB = value;
			_2402();
		}
	}

	[Category("CheckBox Images")]
	[DefaultValue(null)]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
	public Image CheckBoxImageChecked
	{
		get
		{
			return _23F9.CheckBoxImageChecked;
		}
		set
		{
			_23F9.CheckBoxImageChecked = value;
		}
	}

	[Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
	[DefaultValue(null)]
	[Category("CheckBox Images")]
	public Image CheckBoxImageUnChecked
	{
		get
		{
			return _23F9.CheckBoxImageUnChecked;
		}
		set
		{
			_23F9.CheckBoxImageUnChecked = value;
		}
	}

	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
	[DefaultValue(null)]
	public Image CheckBoxImageIndeterminate
	{
		get
		{
			return _23F9.CheckBoxImageIndeterminate;
		}
		set
		{
			_23F9.CheckBoxImageIndeterminate = value;
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

	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	[Category("Commands")]
	[Browsable(true)]
	[DefaultValue(null)]
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

	public event CheckBoxXChangeEventHandler CheckedChanging
	{
		add
		{
			CheckBoxXChangeEventHandler checkBoxXChangeEventHandler = ᢢ;
			CheckBoxXChangeEventHandler checkBoxXChangeEventHandler2;
			do
			{
				checkBoxXChangeEventHandler2 = checkBoxXChangeEventHandler;
				CheckBoxXChangeEventHandler value2 = (CheckBoxXChangeEventHandler)Delegate.Combine(checkBoxXChangeEventHandler2, value);
				checkBoxXChangeEventHandler = Interlocked.CompareExchange(ref ᢢ, value2, checkBoxXChangeEventHandler2);
			}
			while ((object)checkBoxXChangeEventHandler != checkBoxXChangeEventHandler2);
		}
		remove
		{
			CheckBoxXChangeEventHandler checkBoxXChangeEventHandler = ᢢ;
			CheckBoxXChangeEventHandler checkBoxXChangeEventHandler2;
			do
			{
				checkBoxXChangeEventHandler2 = checkBoxXChangeEventHandler;
				CheckBoxXChangeEventHandler value2 = (CheckBoxXChangeEventHandler)Delegate.Remove(checkBoxXChangeEventHandler2, value);
				checkBoxXChangeEventHandler = Interlocked.CompareExchange(ref ᢢ, value2, checkBoxXChangeEventHandler2);
			}
			while ((object)checkBoxXChangeEventHandler != checkBoxXChangeEventHandler2);
		}
	}

	public event CheckBoxXChangeEventHandler CheckedChangedEx
	{
		add
		{
			CheckBoxXChangeEventHandler checkBoxXChangeEventHandler = _23FF;
			CheckBoxXChangeEventHandler checkBoxXChangeEventHandler2;
			do
			{
				checkBoxXChangeEventHandler2 = checkBoxXChangeEventHandler;
				CheckBoxXChangeEventHandler value2 = (CheckBoxXChangeEventHandler)Delegate.Combine(checkBoxXChangeEventHandler2, value);
				checkBoxXChangeEventHandler = Interlocked.CompareExchange(ref _23FF, value2, checkBoxXChangeEventHandler2);
			}
			while ((object)checkBoxXChangeEventHandler != checkBoxXChangeEventHandler2);
		}
		remove
		{
			CheckBoxXChangeEventHandler checkBoxXChangeEventHandler = _23FF;
			CheckBoxXChangeEventHandler checkBoxXChangeEventHandler2;
			do
			{
				checkBoxXChangeEventHandler2 = checkBoxXChangeEventHandler;
				CheckBoxXChangeEventHandler value2 = (CheckBoxXChangeEventHandler)Delegate.Remove(checkBoxXChangeEventHandler2, value);
				checkBoxXChangeEventHandler = Interlocked.CompareExchange(ref _23FF, value2, checkBoxXChangeEventHandler2);
			}
			while ((object)checkBoxXChangeEventHandler != checkBoxXChangeEventHandler2);
		}
	}

	public event EventHandler CheckedChanged
	{
		add
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler CheckValueChanged
	{
		add
		{
			EventHandler eventHandler = _2400;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2400, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2400;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2400, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public CheckBoxX()
	{
		((Control)this).SetStyle((ControlStyles)512, true);
		_23F9 = new CheckBoxItem();
		_23F9._0E62 = 0;
		_23F9.Style = eDotNetBarStyle.Office2007;
		_23F9.CheckedChanging += _2401;
		_23F9.CheckedChanged += ᒚ;
		HostItem = _23F9;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCheckSignSize()
	{
		return _23F9.ShouldSerializeCheckSignSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCheckSignSize()
	{
		CheckSignSize = new Size(13, 13);
	}

	private void ᒚ(object _٠, CheckBoxChangeEventArgs ؿ)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		CheckBoxX oldchecked = null;
		if (_23F9.CheckBoxStyle == eCheckBoxStyle.RadioButton && _23F9.Checked && ((Control)this).Parent != null)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
			{
				Control val = item;
				if (val != this && val is CheckBoxX { Checked: not false, CheckBoxStyle: eCheckBoxStyle.RadioButton } checkBoxX)
				{
					checkBoxX.Checked = false;
					oldchecked = checkBoxX;
				}
			}
		}
		if (Command != null)
		{
			Command.Checked = Checked;
		}
		CheckBoxXChangeEventArgs e = new CheckBoxXChangeEventArgs(oldchecked, this, ؿ.EventSource);
		if (_23FF != null)
		{
			_23FF(this, e);
		}
		if (_0E65 != null)
		{
			_0E65(this, new EventArgs());
		}
		if (_1B3E(_23FA) != CheckState)
		{
			_23FA = _2403(CheckState);
		}
		if (_2400 != null)
		{
			_2400(this, new EventArgs());
		}
		ExecuteCommand();
	}

	private void _2401(object _٠, CheckBoxChangeEventArgs ؿ)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		CheckBoxXChangeEventArgs checkBoxXChangeEventArgs = new CheckBoxXChangeEventArgs(null, this, ؿ.EventSource);
		if (_23F9.CheckBoxStyle == eCheckBoxStyle.RadioButton && !_23F9.Checked && ((Control)this).Parent != null)
		{
			CheckBoxX checkBoxX = null;
			foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
			{
				Control val = item;
				if (val != this)
				{
					checkBoxX = val as CheckBoxX;
					if (checkBoxX != null && checkBoxX.Checked && checkBoxX.CheckBoxStyle == eCheckBoxStyle.RadioButton)
					{
						break;
					}
				}
			}
			checkBoxXChangeEventArgs = new CheckBoxXChangeEventArgs(checkBoxX, this, ؿ.EventSource);
		}
		if (ᢢ != null)
		{
			ᢢ(this, checkBoxXChangeEventArgs);
			ؿ.Cancel = checkBoxXChangeEventArgs.Cancel;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !_23F9.TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && ((Control)this).Enabled && AutoCheck)
		{
			if (CheckBoxStyle == eCheckBoxStyle.CheckBox)
			{
				_0EDB(!Checked, eEventSource.Keyboard);
			}
			else if (!Checked)
			{
				_0EDB(ل: true, eEventSource.Keyboard);
			}
		}
		base.OnKeyDown(e);
	}

	private void _0EDB(bool ل, eEventSource ᎂ)
	{
		_23F9.SetChecked(ل, ᎂ);
	}

	private void _0EDB(CheckState ل, eEventSource ᎂ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_23F9.SetChecked(ل, ᎂ);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		if (((Control)this).Text.Length == 0)
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		_23F9.RecalcSize();
		Size size = _23F9.Size;
		size.Width += 2;
		size.Height += 2;
		if (!TextVisible)
		{
			size.Width += 2;
		}
		_23F9.Bounds = GetItemBounds();
		return size;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			width = preferredSize.Width;
			height = preferredSize.Height;
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	private void _21DF()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		_21DF();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (((Control)this).AutoSize)
		{
			_21DF();
		}
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		if (AutoCheck && ((Control)this).CanSelect && Control.IsMnemonic(charCode, ((Control)this).Text) && ((Control)this).Enabled && (!_ᒓ || (int)Control.ModifierKeys == 262144 || ((Control)this).Focused) && !_23F9.IsUsingTextMarkup && ((Control)this).Focus())
		{
			if (CheckBoxStyle == eCheckBoxStyle.CheckBox)
			{
				_0EDB(!Checked, eEventSource.Keyboard);
			}
			else if (!Checked)
			{
				_0EDB(ل: true, eEventSource.Keyboard);
			}
			return true;
		}
		return false;
	}

	private void _2402()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		CheckState checkState = _1B3E(_23FA);
		CheckState = checkState;
	}

	private bool _1B3F(object ԝ)
	{
		if (ԝ != null && (!(ԝ is string) || !((string)ԝ == string.Empty) || !_23FE))
		{
			return ԝ == DBNull.Value;
		}
		return true;
	}

	private object _2403(CheckState ጎ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if ((int)ጎ == 2)
		{
			return _23FD;
		}
		if ((int)ጎ == 0)
		{
			return _23FB;
		}
		return _23FC;
	}

	private CheckState _1B3E(object ԝ)
	{
		if ((_23FD != null || !_1B3F(ԝ)) && (_23FD == null || !_23FD.Equals(ԝ)))
		{
			if ((_23FC != null || !_1B3F(ԝ)) && (_23FC == null || !_23FC.Equals(ԝ)))
			{
				if ((_23FB != null || !_1B3F(ԝ)) && (_23FB == null || !_23FB.Equals(ԝ)))
				{
					if (ԝ is SqlBoolean sqlBoolean)
					{
						if (!sqlBoolean.IsTrue)
						{
							if (!sqlBoolean.IsNull)
							{
								return (CheckState)0;
							}
							return (CheckState)2;
						}
						return (CheckState)1;
					}
					if (ԝ is int? && ThreeState)
					{
						if (ԝ != null)
						{
							if (((int?)ԝ).Value != 0)
							{
								return (CheckState)1;
							}
							return (CheckState)0;
						}
						return (CheckState)2;
					}
					if (ԝ is int)
					{
						if (ThreeState && (int)ԝ == -1)
						{
							return (CheckState)2;
						}
						if ((int)ԝ == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (ԝ is long)
					{
						if (ThreeState && (long)ԝ == -1)
						{
							return (CheckState)2;
						}
						if ((long)ԝ == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (ԝ is short)
					{
						if (ThreeState && (short)ԝ == -1)
						{
							return (CheckState)2;
						}
						if ((short)ԝ == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (ԝ is float)
					{
						if (ThreeState && (float)ԝ == -1f)
						{
							return (CheckState)2;
						}
						if ((float)ԝ == 0f)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (ԝ is double)
					{
						if (ThreeState && (double)ԝ == -1.0)
						{
							return (CheckState)2;
						}
						if ((double)ԝ == 0.0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (ԝ is byte)
					{
						if ((byte)ԝ == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (ԝ is uint)
					{
						if ((uint)ԝ == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (ԝ is ulong)
					{
						if ((ulong)ԝ == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (ԝ is bool)
					{
						if (!(bool)ԝ)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					return (CheckState)2;
				}
				return (CheckState)0;
			}
			return (CheckState)1;
		}
		return (CheckState)2;
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
		Command command = Command;
		if (command != null && command.Checked != Checked)
		{
			_0EDB(command.Checked, eEventSource.Code);
		}
	}
}
