using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(CheckBoxX), "Controls.CheckBoxX.ico")]
[ToolboxItem(false)]
[DefaultEvent("CheckedChanged")]
[ComVisible(false)]
public class CheckBoxX : BaseItemControl, ICommandSource
{
	private bool m_WindowsXPAware;

	private const string DefaultCheckValueUnchecked = "N";

	private const string DefaultCheckValueChecked = "Y";

	private const object DefaultCheckValueIndeterminate = null;

	private CheckBoxItem m_CheckBox;

	private object m_CheckValue = "N";

	private object m_CheckValueUnchecked = "N";

	private object m_CheckValueChecked = "Y";

	private object m_CheckValueIndeterminate;

	private bool m_ConsiderEmptyStringAsNull = true;

	private bool _EnableMnemonicWithAltKeyOnly;

	private ICommand _Command;

	private object _CommandParameter;

	[Browsable(false)]
	[Category("Behavior")]
	[Description("When running on WindowsXP draws control using the Windows XP Themes if theme manager is enabled.")]
	[DefaultValue(false)]
	public bool ThemeAware
	{
		get
		{
			return m_WindowsXPAware;
		}
		set
		{
			if (m_WindowsXPAware != value)
			{
				m_WindowsXPAware = value;
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates size of the check or radio sign. Default value is 13x13.")]
	public Size CheckSignSize
	{
		get
		{
			return m_CheckBox.CheckSignSize;
		}
		set
		{
			m_CheckBox.CheckSignSize = value;
			RecalcLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for controls Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return m_CheckBox.EnableMarkup;
		}
		set
		{
			m_CheckBox.EnableMarkup = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eCheckBoxStyle.CheckBox)]
	[Category("Appearance")]
	[Description("Indicates appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.")]
	public eCheckBoxStyle CheckBoxStyle
	{
		get
		{
			return m_CheckBox.CheckBoxStyle;
		}
		set
		{
			m_CheckBox.CheckBoxStyle = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eCheckBoxPosition.Left)]
	[Category("Appearance")]
	[Description("Indicates the check box position relative to the text.")]
	public eCheckBoxPosition CheckBoxPosition
	{
		get
		{
			return m_CheckBox.CheckBoxPosition;
		}
		set
		{
			m_CheckBox.CheckBoxPosition = value;
		}
	}

	[Browsable(true)]
	[Bindable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[DefaultValue(false)]
	public virtual bool Checked
	{
		get
		{
			return m_CheckBox.Checked;
		}
		set
		{
			m_CheckBox.Checked = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text assigned to the check box is visible.")]
	public bool TextVisible
	{
		get
		{
			return m_CheckBox.TextVisible;
		}
		set
		{
			m_CheckBox.TextVisible = value;
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates text color.")]
	public Color TextColor
	{
		get
		{
			return m_CheckBox.TextColor;
		}
		set
		{
			m_CheckBox.TextColor = value;
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
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("Indicates text associated with this button..")]
	[Localizable(true)]
	[DefaultValue("")]
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

	[Browsable(true)]
	[DefaultValue(false)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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
				AdjustSize();
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether the CheckBox will allow three check states rather than two.")]
	public bool ThreeState
	{
		get
		{
			return m_CheckBox.ThreeState;
		}
		set
		{
			m_CheckBox.ThreeState = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state")]
	public CheckState CheckState
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_CheckBox.CheckState;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_CheckBox.CheckState = value;
		}
	}

	internal CheckBoxItem CheckBoxItem => m_CheckBox;

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether mnemonic character assigned to control is processed only if Alt key is pressed")]
	public bool EnableMnemonicWithAltKeyOnly
	{
		get
		{
			return _EnableMnemonicWithAltKeyOnly;
		}
		set
		{
			_EnableMnemonicWithAltKeyOnly = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the Checked values and the item appearance are automatically changed when the Check-Box is clicked.")]
	public bool AutoCheck
	{
		get
		{
			return m_CheckBox.AutoCheck;
		}
		set
		{
			m_CheckBox.AutoCheck = value;
		}
	}

	[DefaultValue("N")]
	[Bindable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(StringConverter))]
	[Description("Indicates object that represents the Checked state of control.")]
	public object CheckValue
	{
		get
		{
			return m_CheckValue;
		}
		set
		{
			m_CheckValue = value;
			OnCheckValueChanged();
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Indicates whether empty string is consider as null value during CheckValue value comparison.")]
	public bool ConsiderEmptyStringAsNull
	{
		get
		{
			return m_ConsiderEmptyStringAsNull;
		}
		set
		{
			m_ConsiderEmptyStringAsNull = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the Indeterminate state value of check box when CheckValue property is set to that value")]
	public object CheckValueIndeterminate
	{
		get
		{
			return m_CheckValueIndeterminate;
		}
		set
		{
			m_CheckValueIndeterminate = value;
			OnCheckValueChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue("Y")]
	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the Checked state value of check box when CheckValue property is set to that value")]
	public object CheckValueChecked
	{
		get
		{
			return m_CheckValueChecked;
		}
		set
		{
			m_CheckValueChecked = value;
			OnCheckValueChanged();
		}
	}

	[Browsable(true)]
	[DefaultValue("N")]
	[Category("Behavior")]
	[TypeConverter(typeof(StringConverter))]
	[Description("Represents the Unchecked state value of check box when CheckValue property is set to that value")]
	public object CheckValueUnchecked
	{
		get
		{
			return m_CheckValueUnchecked;
		}
		set
		{
			m_CheckValueUnchecked = value;
			OnCheckValueChanged();
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
	public Image CheckBoxImageChecked
	{
		get
		{
			return m_CheckBox.CheckBoxImageChecked;
		}
		set
		{
			m_CheckBox.CheckBoxImageChecked = value;
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
	public Image CheckBoxImageUnChecked
	{
		get
		{
			return m_CheckBox.CheckBoxImageUnChecked;
		}
		set
		{
			m_CheckBox.CheckBoxImageUnChecked = value;
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
	public Image CheckBoxImageIndeterminate
	{
		get
		{
			return m_CheckBox.CheckBoxImageIndeterminate;
		}
		set
		{
			m_CheckBox.CheckBoxImageIndeterminate = value;
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

	public event CheckBoxXChangeEventHandler CheckedChanging;

	public event CheckBoxXChangeEventHandler CheckedChangedEx;

	public event EventHandler CheckedChanged;

	public event EventHandler CheckValueChanged;

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Invalid comparison between Unknown and I4
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Invalid comparison between Unknown and I4
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Invalid comparison between Unknown and I4
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Invalid comparison between Unknown and I4
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Invalid comparison between Unknown and I4
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		if (!ThemeAware)
		{
			base.OnPaint(e);
			return;
		}
		Brush val = (Brush)new SolidBrush(((Control)this).BackColor);
		try
		{
			e.Graphics.FillRectangle(val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Rectangle rectangle = new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Height, ((Control)this).ClientRectangle.Height);
		bool flag = ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));
		bool flag2 = false;
		if (CheckBoxStyle == eCheckBoxStyle.CheckBox)
		{
			if (VisualStyleInformation.IsEnabledByUser && VisualStyleInformation.IsSupportedByOS)
			{
				VisualStyleElement val2 = null;
				if ((int)CheckState == 1)
				{
					val2 = ((!((Control)this).Enabled) ? CheckBox.CheckedDisabled : ((!flag) ? CheckBox.CheckedNormal : CheckBox.CheckedHot));
				}
				else if ((int)CheckState == 0)
				{
					val2 = ((!((Control)this).Enabled) ? CheckBox.UncheckedDisabled : ((!flag) ? CheckBox.UncheckedNormal : CheckBox.UncheckedHot));
				}
				else if ((int)CheckState == 2)
				{
					val2 = ((!((Control)this).Enabled) ? CheckBox.MixedDisabled : ((!flag) ? CheckBox.MixedNormal : CheckBox.MixedHot));
				}
				try
				{
					if (VisualStyleRenderer.IsElementDefined(val2))
					{
						VisualStyleRenderer val3 = new VisualStyleRenderer(val2);
						val3.DrawBackground((IDeviceContext)(object)e.Graphics, rectangle);
						flag2 = true;
					}
				}
				catch
				{
				}
			}
			if (!flag2)
			{
				ButtonState val4 = (ButtonState)0;
				if (!((Control)this).Enabled)
				{
					val4 = (ButtonState)256;
				}
				else if ((int)CheckState == 1)
				{
					val4 = (ButtonState)1024;
				}
				int num = 13;
				rectangle = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - num) / 2, num, num);
				ControlPaint.DrawCheckBox(e.Graphics, rectangle, val4);
			}
		}
		else
		{
			if (VisualStyleInformation.IsEnabledByUser && VisualStyleInformation.IsSupportedByOS)
			{
				VisualStyleElement val5 = null;
				if ((int)CheckState == 1)
				{
					val5 = ((!((Control)this).Enabled) ? RadioButton.CheckedDisabled : ((!flag) ? RadioButton.CheckedNormal : RadioButton.CheckedHot));
				}
				else if ((int)CheckState == 0)
				{
					val5 = ((!((Control)this).Enabled) ? RadioButton.UncheckedDisabled : ((!flag) ? RadioButton.UncheckedNormal : RadioButton.UncheckedHot));
				}
				try
				{
					if (VisualStyleRenderer.IsElementDefined(val5))
					{
						VisualStyleRenderer val6 = new VisualStyleRenderer(val5);
						val6.DrawBackground((IDeviceContext)(object)e.Graphics, rectangle);
						flag2 = true;
					}
				}
				catch
				{
				}
			}
			if (!flag2)
			{
				ButtonState val7 = (ButtonState)0;
				if (!((Control)this).Enabled)
				{
					val7 = (ButtonState)256;
				}
				else if ((int)CheckState == 1)
				{
					val7 = (ButtonState)1024;
				}
				int num2 = 13;
				rectangle = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - num2) / 2, num2, num2);
				rectangle.Y++;
				ControlPaint.DrawRadioButton(e.Graphics, rectangle, val7);
			}
		}
		StringFormat val8 = new StringFormat();
		try
		{
			Brush val9 = (Brush)new SolidBrush(((Control)this).ForeColor);
			try
			{
				val8.Alignment = (StringAlignment)0;
				val8.LineAlignment = (StringAlignment)1;
				Rectangle rectangle2 = new Rectangle(((Control)this).ClientRectangle.X + rectangle.Width, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width - rectangle.Width, ((Control)this).ClientRectangle.Height);
				val8.HotkeyPrefix = (HotkeyPrefix)1;
				e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, val9, (RectangleF)rectangle2, val8);
			}
			finally
			{
				((IDisposable)val9)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		((Control)this).Invalidate();
	}

	public CheckBoxX()
	{
		((Control)this).SetStyle((ControlStyles)512, true);
		m_CheckBox = new CheckBoxItem();
		m_CheckBox.VerticalPadding = 0;
		m_CheckBox.Style = eDotNetBarStyle.Office2007;
		m_CheckBox.CheckedChanging += OnCheckedChanging;
		m_CheckBox.CheckedChanged += OnCheckedChanged;
		HostItem = m_CheckBox;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCheckSignSize()
	{
		return m_CheckBox.ShouldSerializeCheckSignSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCheckSignSize()
	{
		CheckSignSize = new Size(13, 13);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (!((Control)this).Focused)
		{
			((Control)this).Select();
		}
		base.OnMouseDown(e);
	}

	private void OnCheckedChanged(object sender, CheckBoxChangeEventArgs e)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		CheckBoxX oldchecked = null;
		if (m_CheckBox.CheckBoxStyle == eCheckBoxStyle.RadioButton && m_CheckBox.Checked && ((Control)this).Parent != null)
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
		CheckBoxXChangeEventArgs e2 = new CheckBoxXChangeEventArgs(oldchecked, this, e.EventSource);
		if (this.CheckedChangedEx != null)
		{
			this.CheckedChangedEx(this, e2);
		}
		if (this.CheckedChanged != null)
		{
			this.CheckedChanged(this, new EventArgs());
		}
		if (GetCheckState(m_CheckValue) != CheckState)
		{
			m_CheckValue = GetCheckValue(CheckState);
		}
		if (this.CheckValueChanged != null)
		{
			this.CheckValueChanged(this, new EventArgs());
		}
		ExecuteCommand();
	}

	private void OnCheckedChanging(object sender, CheckBoxChangeEventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		CheckBoxXChangeEventArgs checkBoxXChangeEventArgs = new CheckBoxXChangeEventArgs(null, this, e.EventSource);
		if (m_CheckBox.CheckBoxStyle == eCheckBoxStyle.RadioButton && !m_CheckBox.Checked && ((Control)this).Parent != null)
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
			checkBoxXChangeEventArgs = new CheckBoxXChangeEventArgs(checkBoxX, this, e.EventSource);
		}
		if (this.CheckedChanging != null)
		{
			this.CheckedChanging(this, checkBoxXChangeEventArgs);
			e.Cancel = checkBoxXChangeEventArgs.Cancel;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !m_CheckBox.TextColor.IsEmpty;
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
				SetChecked(!Checked, eEventSource.Keyboard);
			}
			else if (!Checked)
			{
				SetChecked(newValue: true, eEventSource.Keyboard);
			}
		}
		base.OnKeyDown(e);
	}

	private void SetChecked(bool newValue, eEventSource source)
	{
		m_CheckBox.SetChecked(newValue, source);
	}

	private void SetChecked(CheckState newValue, eEventSource source)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_CheckBox.SetChecked(newValue, source);
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
		m_CheckBox.RecalcSize();
		Size size = m_CheckBox.Size;
		size.Width += 2;
		size.Height += 2;
		if (!TextVisible)
		{
			size.Width += 2;
		}
		m_CheckBox.Bounds = GetItemBounds();
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

	private void AdjustSize()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		AdjustSize();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (((Control)this).AutoSize)
		{
			AdjustSize();
		}
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		if (AutoCheck && ((Control)this).CanSelect && Control.IsMnemonic(charCode, ((Control)this).Text) && ((Control)this).Enabled && (!_EnableMnemonicWithAltKeyOnly || (int)Control.ModifierKeys == 262144 || ((Control)this).Focused) && !m_CheckBox.IsUsingTextMarkup && ((Control)this).Focus())
		{
			if (CheckBoxStyle == eCheckBoxStyle.CheckBox)
			{
				SetChecked(!Checked, eEventSource.Keyboard);
			}
			else if (!Checked)
			{
				SetChecked(newValue: true, eEventSource.Keyboard);
			}
			return true;
		}
		return false;
	}

	private void OnCheckValueChanged()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		CheckState checkState = GetCheckState(m_CheckValue);
		CheckState = checkState;
	}

	private bool IsNull(object value)
	{
		if (value != null && (!(value is string) || !((string)value == string.Empty) || !m_ConsiderEmptyStringAsNull))
		{
			return value == DBNull.Value;
		}
		return true;
	}

	private object GetCheckValue(CheckState cs)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if ((int)cs == 2)
		{
			return m_CheckValueIndeterminate;
		}
		if ((int)cs == 0)
		{
			return m_CheckValueUnchecked;
		}
		return m_CheckValueChecked;
	}

	private CheckState GetCheckState(object value)
	{
		if ((m_CheckValueIndeterminate != null || !IsNull(value)) && (m_CheckValueIndeterminate == null || !m_CheckValueIndeterminate.Equals(value)))
		{
			if ((m_CheckValueChecked != null || !IsNull(value)) && (m_CheckValueChecked == null || !m_CheckValueChecked.Equals(value)))
			{
				if ((m_CheckValueUnchecked != null || !IsNull(value)) && (m_CheckValueUnchecked == null || !m_CheckValueUnchecked.Equals(value)))
				{
					if (value is SqlBoolean sqlBoolean)
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
					if (value is int? && ThreeState)
					{
						if (value != null)
						{
							if (((int?)value).Value != 0)
							{
								return (CheckState)1;
							}
							return (CheckState)0;
						}
						return (CheckState)2;
					}
					if (value is int)
					{
						if (ThreeState && (int)value == -1)
						{
							return (CheckState)2;
						}
						if ((int)value == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (value is long)
					{
						if (ThreeState && (long)value == -1)
						{
							return (CheckState)2;
						}
						if ((long)value == 0L)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (value is short)
					{
						if (ThreeState && (short)value == -1)
						{
							return (CheckState)2;
						}
						if ((short)value == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (value is float)
					{
						if (ThreeState && (float)value == -1f)
						{
							return (CheckState)2;
						}
						if ((float)value == 0f)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (value is double)
					{
						if (ThreeState && (double)value == -1.0)
						{
							return (CheckState)2;
						}
						if ((double)value == 0.0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (value is byte)
					{
						if ((byte)value == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (value is uint)
					{
						if ((uint)value == 0)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (value is ulong)
					{
						if ((ulong)value == 0L)
						{
							return (CheckState)0;
						}
						return (CheckState)1;
					}
					if (value is bool)
					{
						if (!(bool)value)
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
		if (_Command != null)
		{
			CommandManager.ExecuteCommand(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
		Command command = Command;
		if (command != null && command.Checked != Checked)
		{
			SetChecked(command.Checked, eEventSource.Code);
		}
	}
}
