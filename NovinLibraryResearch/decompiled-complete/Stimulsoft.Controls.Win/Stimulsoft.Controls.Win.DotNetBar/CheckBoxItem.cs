using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("Click")]
public class CheckBoxItem : BaseItem
{
	private bool m_Checked;

	private eCheckBoxStyle m_CheckBoxStyle;

	private Size m_CheckSignSize = new Size((int)Math.Ceiling(13.0 * StiScale2.X), (int)Math.Ceiling(13.0 * StiScale2.Y));

	private eCheckBoxPosition m_CheckBoxPosition;

	private int m_CheckTextSpacing = 6;

	private int m_VerticalPadding = 3;

	private bool m_MouseDown;

	private bool m_MouseOver;

	private bool m_TextVisible = true;

	private Color m_TextColor = Color.Empty;

	private bool m_ThreeState;

	private CheckState m_CheckState;

	private bool _AutoCheck = true;

	private bool _EnableMarkup = true;

	private Image _CheckBoxImageChecked;

	private Image _CheckBoxImageUnChecked;

	private Image _CheckBoxImageIndeterminate;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the Checked values and the item appearance are automatically changed when the Check-Box is clicked.")]
	public bool AutoCheck
	{
		get
		{
			return _AutoCheck;
		}
		set
		{
			if (value != _AutoCheck)
			{
				bool autoCheck = _AutoCheck;
				_AutoCheck = value;
				OnAutoCheckChanged(autoCheck, value);
			}
		}
	}

	internal int CheckTextSpacing => m_CheckTextSpacing;

	internal int VerticalPadding
	{
		get
		{
			return m_VerticalPadding;
		}
		set
		{
			m_VerticalPadding = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates size of the check or radio sign. Default value is 13x13.")]
	public Size CheckSignSize
	{
		get
		{
			return m_CheckSignSize;
		}
		set
		{
			if (value.Width < 6)
			{
				value.Width = 6;
			}
			if (value.Height < 6)
			{
				value.Height = 6;
			}
			m_CheckSignSize = value;
			NeedRecalcSize = true;
			Refresh();
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
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text assigned to the check box is visible.")]
	public bool TextVisible
	{
		get
		{
			return m_TextVisible;
		}
		set
		{
			m_TextVisible = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates text color.")]
	public Color TextColor
	{
		get
		{
			return m_TextColor;
		}
		set
		{
			m_TextColor = value;
			OnAppearanceChanged();
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
			return m_CheckBoxStyle;
		}
		set
		{
			m_CheckBoxStyle = value;
			OnAppearanceChanged();
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
			return m_CheckBoxPosition;
		}
		set
		{
			m_CheckBoxPosition = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return m_MouseOver;
		}
		internal set
		{
			m_MouseOver = value;
		}
	}

	[Browsable(false)]
	public bool IsMouseDown
	{
		get
		{
			return m_MouseDown;
		}
		internal set
		{
			m_MouseDown = value;
		}
	}

	[Browsable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[DefaultValue(false)]
	[Bindable(false)]
	public virtual bool Checked
	{
		get
		{
			return m_Checked;
		}
		set
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (m_Checked != value && (!(m_ThreeState && value) || (int)m_CheckState == 0))
			{
				SetChecked(value, eEventSource.Code);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[RefreshProperties(RefreshProperties.All)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[DefaultValue(false)]
	[Bindable(true)]
	public virtual bool CheckedBindable
	{
		get
		{
			return Checked;
		}
		set
		{
			Checked = value;
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
			return m_ThreeState;
		}
		set
		{
			m_ThreeState = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state")]
	[Bindable(true)]
	public CheckState CheckState
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_CheckState;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (value != m_CheckState)
			{
				SetChecked(value, eEventSource.Code);
			}
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
	public Image CheckBoxImageChecked
	{
		get
		{
			return _CheckBoxImageChecked;
		}
		set
		{
			_CheckBoxImageChecked = value;
			OnCheckBoxImageChanged();
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
	public Image CheckBoxImageUnChecked
	{
		get
		{
			return _CheckBoxImageUnChecked;
		}
		set
		{
			_CheckBoxImageUnChecked = value;
			OnCheckBoxImageChanged();
		}
	}

	[DefaultValue(null)]
	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
	public Image CheckBoxImageIndeterminate
	{
		get
		{
			return _CheckBoxImageIndeterminate;
		}
		set
		{
			_CheckBoxImageIndeterminate = value;
			OnCheckBoxImageChanged();
		}
	}

	public event CheckBoxChangeEventHandler CheckedChanging;

	public event CheckBoxChangeEventHandler CheckedChanged;

	public event EventHandler CheckStateChanged;

	public event EventHandler CheckedBindableChanged;

	public CheckBoxItem()
		: this("", "")
	{
	}

	public CheckBoxItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public CheckBoxItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
	}

	public override BaseItem Copy()
	{
		CheckBoxItem checkBoxItem = new CheckBoxItem(m_Name);
		CopyToItem(checkBoxItem);
		return checkBoxItem;
	}

	internal void InternalCopyToItem(CheckBoxItem copy)
	{
		CopyToItem(copy);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (copy is CheckBoxItem checkBoxItem)
		{
			checkBoxItem.CheckBoxPosition = CheckBoxPosition;
			checkBoxItem.CheckBoxStyle = CheckBoxStyle;
			checkBoxItem.Checked = Checked;
			checkBoxItem.CheckState = CheckState;
			checkBoxItem.TextColor = TextColor;
			checkBoxItem.TextVisible = TextVisible;
			checkBoxItem.ThreeState = ThreeState;
			checkBoxItem.EnableMarkup = EnableMarkup;
			checkBoxItem.CheckBoxImageChecked = CheckBoxImageChecked;
			checkBoxItem.CheckBoxImageIndeterminate = CheckBoxImageIndeterminate;
			checkBoxItem.CheckBoxImageUnChecked = CheckBoxImageUnChecked;
			base.CopyToItem(checkBoxItem);
		}
	}

	private void OnAutoCheckChanged(bool oldValue, bool newValue)
	{
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer renderer = p.Renderer;
		if (renderer != null)
		{
			CheckBoxItemRenderEventArgs checkBoxItemRenderEventArgs = new CheckBoxItemRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
			checkBoxItemRenderEventArgs.ItemPaintArgs = p;
			renderer.DrawCheckBoxItem(checkBoxItemRenderEventArgs);
		}
		else
		{
			CheckBoxItemPainter checkBoxItemPainter = PainterFactory.CreateCheckBoxItemPainter(this);
			if (checkBoxItemPainter != null)
			{
				CheckBoxItemRenderEventArgs checkBoxItemRenderEventArgs2 = new CheckBoxItemRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
				checkBoxItemRenderEventArgs2.ItemPaintArgs = p;
				checkBoxItemPainter.Paint(checkBoxItemRenderEventArgs2);
			}
		}
		DrawInsertMarker(p.Graphics);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCheckSignSize()
	{
		if (m_CheckSignSize.Width == 13)
		{
			return m_CheckSignSize.Height != 13;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCheckSignSize()
	{
		CheckSignSize = new Size(13, 13);
	}

	internal Size GetCheckSignSize()
	{
		Size result = m_CheckSignSize;
		if (_CheckBoxImageChecked != null || _CheckBoxImageIndeterminate != null || _CheckBoxImageUnChecked != null)
		{
			result = Size.Empty;
			if (_CheckBoxImageChecked != null)
			{
				result = new Size(Math.Max(result.Width, _CheckBoxImageChecked.Width), Math.Max(result.Height, _CheckBoxImageChecked.Height));
			}
			if (_CheckBoxImageIndeterminate != null)
			{
				result = new Size(Math.Max(result.Width, _CheckBoxImageIndeterminate.Width), Math.Max(result.Height, _CheckBoxImageIndeterminate.Height));
			}
			if (_CheckBoxImageUnChecked != null)
			{
				result = new Size(Math.Max(result.Width, _CheckBoxImageUnChecked.Width), Math.Max(result.Height, _CheckBoxImageUnChecked.Height));
			}
		}
		return result;
	}

	public override void RecalcSize()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.Disposing || val.IsDisposed)
		{
			return;
		}
		int verticalPadding = m_VerticalPadding;
		int checkTextSpacing = m_CheckTextSpacing;
		Graphics val2 = BarFunctions.CreateGraphics(val);
		if (val2 == null)
		{
			return;
		}
		Size checkSignSize = GetCheckSignSize();
		if (m_TextVisible)
		{
			try
			{
				Size size = ButtonItemLayout.MeasureItemText(this, val2, 0, val.Font, eTextFormat.Default, (int)val.RightToLeft == 1);
				size.Width++;
				if (m_CheckBoxPosition == eCheckBoxPosition.Left || m_CheckBoxPosition == eCheckBoxPosition.Right)
				{
					m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, size.Width + checkTextSpacing + checkSignSize.Width, Math.Max(checkSignSize.Height, size.Height) + verticalPadding * 2);
				}
				else
				{
					m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, Math.Max(size.Width, checkSignSize.Width) + verticalPadding * 2, size.Height + checkTextSpacing + checkSignSize.Height);
				}
			}
			finally
			{
				val2.Dispose();
			}
		}
		else
		{
			Size size2 = checkSignSize;
			size2.Width += verticalPadding * 2;
			size2.Height += verticalPadding * 2;
			m_Rect = new Rectangle(m_Rect.Location, size2);
		}
		base.RecalcSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !m_TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!DesignMode && _AutoCheck)
		{
			m_MouseOver = true;
			if (GetEnabled())
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		if (!DesignMode)
		{
			m_MouseOver = false;
			m_MouseDown = false;
			if (GetEnabled())
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if ((int)objArg.Button == 1048576 && !DesignMode && _AutoCheck)
		{
			m_MouseDown = true;
			if (GetEnabled())
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		if (m_MouseDown && !DesignMode)
		{
			m_MouseDown = false;
			if (GetEnabled())
			{
				Refresh();
			}
		}
	}

	public override void RaiseClick(eEventSource source, eShortcut key)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		if (_AutoCheck && base.CanRaiseClick && (CheckBoxStyle != eCheckBoxStyle.RadioButton || !Checked))
		{
			if (ThreeState)
			{
				if ((int)CheckState == 0)
				{
					SetChecked((CheckState)1, source);
				}
				else if ((int)CheckState == 1)
				{
					SetChecked((CheckState)2, source);
				}
				else if ((int)CheckState == 2)
				{
					SetChecked((CheckState)0, source);
				}
			}
			else
			{
				SetChecked(!Checked, source);
			}
			ExecuteCommand();
		}
		base.RaiseClick(source, key);
	}

	public virtual void SetChecked(bool newValue, eEventSource source)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		if (m_CheckBoxStyle == eCheckBoxStyle.RadioButton && newValue && Parent != null)
		{
			CheckBoxItem checkBoxItem = null;
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this)
				{
					checkBoxItem = subItem as CheckBoxItem;
					if (checkBoxItem != null && checkBoxItem.Checked && checkBoxItem.CheckBoxStyle == eCheckBoxStyle.RadioButton)
					{
						break;
					}
				}
			}
			CheckBoxChangeEventArgs checkBoxChangeEventArgs = new CheckBoxChangeEventArgs(checkBoxItem, this, source);
			InvokeCheckedChanging(checkBoxChangeEventArgs);
			if (checkBoxChangeEventArgs.Cancel)
			{
				return;
			}
		}
		else
		{
			CheckBoxChangeEventArgs checkBoxChangeEventArgs2 = new CheckBoxChangeEventArgs(null, this, source);
			InvokeCheckedChanging(checkBoxChangeEventArgs2);
			if (checkBoxChangeEventArgs2.Cancel)
			{
				return;
			}
		}
		m_Checked = newValue;
		if ((m_Checked && (int)m_CheckState == 0) || (!m_Checked && (int)m_CheckState != 0))
		{
			m_CheckState = (CheckState)(m_Checked ? 1 : 0);
		}
		if (Command != null)
		{
			Command.Checked = m_Checked;
		}
		OnCheckedChanged(source);
		OnCheckedBindableChanged(EventArgs.Empty);
		if (ShouldSyncProperties)
		{
			BarFunctions.SyncProperty(this, "Checked");
		}
		if (Displayed)
		{
			Refresh();
		}
	}

	protected override void OnCommandChanged()
	{
		Command command = Command;
		if (command != null && command.Checked != Checked)
		{
			SetChecked(command.Checked, eEventSource.Code);
		}
		base.OnCommandChanged();
	}

	public virtual void SetChecked(CheckState newValue, eEventSource source)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		CheckBoxChangeEventArgs checkBoxChangeEventArgs = new CheckBoxChangeEventArgs(null, this, source);
		InvokeCheckedChanging(checkBoxChangeEventArgs);
		if (!checkBoxChangeEventArgs.Cancel)
		{
			m_CheckState = newValue;
			m_Checked = (int)newValue > 0;
			if (Command != null)
			{
				Command.Checked = m_Checked;
			}
			OnCheckedChanged(source);
			OnCheckStateChanged(EventArgs.Empty);
			if (ShouldSyncProperties)
			{
				BarFunctions.SyncProperty(this, "CheckState");
			}
			if (Displayed)
			{
				Refresh();
			}
		}
	}

	protected virtual void OnCheckStateChanged(EventArgs eventArgs)
	{
		if (this.CheckStateChanged != null)
		{
			this.CheckStateChanged(this, eventArgs);
		}
	}

	protected virtual void OnCheckedBindableChanged(EventArgs eventArgs)
	{
		if (this.CheckedBindableChanged != null)
		{
			this.CheckedBindableChanged(this, eventArgs);
		}
	}

	protected virtual void OnCheckedChanged(eEventSource source)
	{
		CheckBoxItem oldchecked = null;
		if (m_CheckBoxStyle == eCheckBoxStyle.RadioButton && m_Checked && Parent != null)
		{
			BaseItem[] array = new BaseItem[Parent.SubItems.Count];
			Parent.SubItems.CopyTo(array, 0);
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				if (baseItem != this && baseItem is CheckBoxItem { Checked: not false, CheckBoxStyle: eCheckBoxStyle.RadioButton } checkBoxItem)
				{
					checkBoxItem.Checked = false;
					oldchecked = checkBoxItem;
				}
			}
		}
		InvokeCheckedChanged(new CheckBoxChangeEventArgs(oldchecked, this, source));
	}

	protected virtual void InvokeCheckedChanging(CheckBoxChangeEventArgs e)
	{
		if (this.CheckedChanging != null)
		{
			this.CheckedChanging(this, e);
		}
	}

	protected virtual void InvokeCheckedChanged(CheckBoxChangeEventArgs e)
	{
		if (this.CheckedChanged != null)
		{
			this.CheckedChanged(this, e);
		}
	}

	private void OnCheckBoxImageChanged()
	{
		NeedRecalcSize = true;
		Refresh();
	}
}
