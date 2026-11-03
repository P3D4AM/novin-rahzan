using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.CheckBoxItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("Click")]
public class CheckBoxItem : BaseItem
{
	private bool ݟ;

	private eCheckBoxStyle ᢛ;

	private Size ᢜ = new Size(13, 13);

	private eCheckBoxPosition ᢝ;

	private int ᢞ = 6;

	private int _0E77 = 3;

	private bool ݠ;

	private bool ݡ;

	private bool ᢟ = true;

	private Color ޗ = Color.Empty;

	private bool ᢠ;

	private CheckState ᢡ;

	private CheckBoxChangeEventHandler ᢢ;

	private CheckBoxChangeEventHandler _0E65;

	private EventHandler ᢣ;

	private EventHandler ᢤ;

	private bool _ᢥ = true;

	private bool _ᢦ;

	private Size _ᢧ = Size.Empty;

	private bool __0EB9 = true;

	private Image __05ED;

	private Image __05EE;

	private Image _ׯ;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the Checked values and the item appearance are automatically changed when the Check-Box is clicked.")]
	public bool AutoCheck
	{
		get
		{
			return _ᢥ;
		}
		set
		{
			if (value != _ᢥ)
			{
				bool ᢥ = _ᢥ;
				_ᢥ = value;
				ᢨ(ᢥ, value);
			}
		}
	}

	internal int ᢙ => ᢞ;

	internal int _0E62
	{
		get
		{
			return _0E77;
		}
		set
		{
			_0E77 = value;
		}
	}

	[Description("Indicates size of the check or radio sign. Default value is 13x13.")]
	[Category("Appearance")]
	public Size CheckSignSize
	{
		get
		{
			return ᢜ;
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
			ᢜ = value;
			NeedRecalcSize = true;
			Refresh();
		}
	}

	internal bool ᢚ => _ᢦ;

	internal Size ᆾ => _ᢧ;

	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("The text contained in the item.")]
	[DefaultValue("")]
	[Browsable(true)]
	[Category("Appearance")]
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

	protected override bool IsMarkupSupported => __0EB9;

	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	[Category("Appearance")]
	[DefaultValue(true)]
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

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether text assigned to the check box is visible.")]
	[Browsable(true)]
	public bool TextVisible
	{
		get
		{
			return ᢟ;
		}
		set
		{
			ᢟ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Description("Indicates text color.")]
	[Category("Appearance")]
	[Browsable(true)]
	public Color TextColor
	{
		get
		{
			return ޗ;
		}
		set
		{
			ޗ = value;
			OnAppearanceChanged();
		}
	}

	[Description("Indicates appearance style of the item. Default value is CheckBox. Item can also assume the style of radio-button.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eCheckBoxStyle.CheckBox)]
	public eCheckBoxStyle CheckBoxStyle
	{
		get
		{
			return ᢛ;
		}
		set
		{
			ᢛ = value;
			OnAppearanceChanged();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eCheckBoxPosition.Left)]
	[Description("Indicates the check box position relative to the text.")]
	public eCheckBoxPosition CheckBoxPosition
	{
		get
		{
			return ᢝ;
		}
		set
		{
			ᢝ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return ݡ;
		}
		internal set
		{
			ݡ = value;
		}
	}

	[Browsable(false)]
	public bool IsMouseDown
	{
		get
		{
			return ݠ;
		}
		internal set
		{
			ݠ = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Category("Appearance")]
	[Description("Indicates whether item is checked or not.")]
	[Bindable(false)]
	public virtual bool Checked
	{
		get
		{
			return ݟ;
		}
		set
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (ݟ != value && (!ᢠ || !value || (int)ᢡ == 0))
			{
				SetChecked(value, eEventSource.Code);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Indicates whether item is checked or not.")]
	[Browsable(false)]
	[Bindable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
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

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Indicates whether the CheckBox will allow three check states rather than two.")]
	[Browsable(true)]
	public bool ThreeState
	{
		get
		{
			return ᢠ;
		}
		set
		{
			ᢠ = value;
		}
	}

	[Description("Specifies the state of a control, such as a check box, that can be checked, unchecked, or set to an indeterminate state")]
	[RefreshProperties(RefreshProperties.All)]
	[Category("Behavior")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Bindable(true)]
	public CheckState CheckState
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ᢡ;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (value != ᢡ)
			{
				SetChecked(value, eEventSource.Code);
			}
		}
	}

	[Description("Indicates custom image that is displayed instead default check box representation when check box is checked")]
	[DefaultValue(null)]
	[Category("CheckBox Images")]
	public Image CheckBoxImageChecked
	{
		get
		{
			return __05ED;
		}
		set
		{
			__05ED = value;
			_0655();
		}
	}

	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is unchecked")]
	[DefaultValue(null)]
	public Image CheckBoxImageUnChecked
	{
		get
		{
			return __05EE;
		}
		set
		{
			__05EE = value;
			_0655();
		}
	}

	[Category("CheckBox Images")]
	[Description("Indicates custom image that is displayed instead default check box representation when check box is in indeterminate state")]
	[DefaultValue(null)]
	public Image CheckBoxImageIndeterminate
	{
		get
		{
			return _ׯ;
		}
		set
		{
			_ׯ = value;
			_0655();
		}
	}

	public event CheckBoxChangeEventHandler CheckedChanging
	{
		add
		{
			CheckBoxChangeEventHandler checkBoxChangeEventHandler = ᢢ;
			CheckBoxChangeEventHandler checkBoxChangeEventHandler2;
			do
			{
				checkBoxChangeEventHandler2 = checkBoxChangeEventHandler;
				CheckBoxChangeEventHandler value2 = (CheckBoxChangeEventHandler)Delegate.Combine(checkBoxChangeEventHandler2, value);
				checkBoxChangeEventHandler = Interlocked.CompareExchange(ref ᢢ, value2, checkBoxChangeEventHandler2);
			}
			while ((object)checkBoxChangeEventHandler != checkBoxChangeEventHandler2);
		}
		remove
		{
			CheckBoxChangeEventHandler checkBoxChangeEventHandler = ᢢ;
			CheckBoxChangeEventHandler checkBoxChangeEventHandler2;
			do
			{
				checkBoxChangeEventHandler2 = checkBoxChangeEventHandler;
				CheckBoxChangeEventHandler value2 = (CheckBoxChangeEventHandler)Delegate.Remove(checkBoxChangeEventHandler2, value);
				checkBoxChangeEventHandler = Interlocked.CompareExchange(ref ᢢ, value2, checkBoxChangeEventHandler2);
			}
			while ((object)checkBoxChangeEventHandler != checkBoxChangeEventHandler2);
		}
	}

	public event CheckBoxChangeEventHandler CheckedChanged
	{
		add
		{
			CheckBoxChangeEventHandler checkBoxChangeEventHandler = _0E65;
			CheckBoxChangeEventHandler checkBoxChangeEventHandler2;
			do
			{
				checkBoxChangeEventHandler2 = checkBoxChangeEventHandler;
				CheckBoxChangeEventHandler value2 = (CheckBoxChangeEventHandler)Delegate.Combine(checkBoxChangeEventHandler2, value);
				checkBoxChangeEventHandler = Interlocked.CompareExchange(ref _0E65, value2, checkBoxChangeEventHandler2);
			}
			while ((object)checkBoxChangeEventHandler != checkBoxChangeEventHandler2);
		}
		remove
		{
			CheckBoxChangeEventHandler checkBoxChangeEventHandler = _0E65;
			CheckBoxChangeEventHandler checkBoxChangeEventHandler2;
			do
			{
				checkBoxChangeEventHandler2 = checkBoxChangeEventHandler;
				CheckBoxChangeEventHandler value2 = (CheckBoxChangeEventHandler)Delegate.Remove(checkBoxChangeEventHandler2, value);
				checkBoxChangeEventHandler = Interlocked.CompareExchange(ref _0E65, value2, checkBoxChangeEventHandler2);
			}
			while ((object)checkBoxChangeEventHandler != checkBoxChangeEventHandler2);
		}
	}

	public event EventHandler CheckStateChanged
	{
		add
		{
			EventHandler eventHandler = ᢣ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᢣ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᢣ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᢣ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler CheckedBindableChanged
	{
		add
		{
			EventHandler eventHandler = ᢤ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᢤ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᢤ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᢤ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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

	internal void _0EBA(CheckBoxItem _0EBB)
	{
		CopyToItem(_0EBB);
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

	private void ᢨ(bool ك, bool ل)
	{
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer ໝ = p.ໝ;
		if (ໝ != null)
		{
			CheckBoxItemRenderEventArgs checkBoxItemRenderEventArgs = new CheckBoxItemRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
			checkBoxItemRenderEventArgs.က = p;
			ໝ.DrawCheckBoxItem(checkBoxItemRenderEventArgs);
		}
		else
		{
			ᢘ ᢘ = ᣓ._191F(this);
			if (ᢘ != null)
			{
				CheckBoxItemRenderEventArgs checkBoxItemRenderEventArgs2 = new CheckBoxItemRenderEventArgs(p.Graphics, this, p.Colors, p.Font, p.RightToLeft);
				checkBoxItemRenderEventArgs2.က = p;
				ᢘ.Paint(checkBoxItemRenderEventArgs2);
			}
		}
		DrawInsertMarker(p.Graphics);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCheckSignSize()
	{
		if (ᢜ.Width == 13)
		{
			return ᢜ.Height != 13;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCheckSignSize()
	{
		CheckSignSize = new Size(13, 13);
	}

	internal Size _18A9()
	{
		Size size = ᢜ;
		if (__05ED != null || _ׯ != null || __05EE != null)
		{
			size = Size.Empty;
			if (__05ED != null)
			{
				size = new Size(Math.Max(size.Width, __05ED.Width), Math.Max(size.Height, __05ED.Height));
			}
			if (_ׯ != null)
			{
				size = new Size(Math.Max(size.Width, _ׯ.Width), Math.Max(size.Height, _ׯ.Height));
			}
			if (__05EE != null)
			{
				size = new Size(Math.Max(size.Width, __05EE.Width), Math.Max(size.Height, __05EE.Height));
			}
			return Dpi.ImageSize(size);
		}
		return Dpi.Size(size);
	}

	public override void RecalcSize()
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.Disposing || val.IsDisposed)
		{
			return;
		}
		int num = Dpi.Height(_0E77);
		int num2 = Dpi.Width(ᢞ);
		Graphics val2 = BarFunctions.CreateGraphics(val);
		if (val2 == null)
		{
			return;
		}
		Size size = _18A9();
		if (ᢟ)
		{
			try
			{
				Size empty = Size.Empty;
				_ᢦ = false;
				if (val is CheckBoxX && !((Control)(CheckBoxX)(object)val).AutoSize)
				{
					empty = _20B1._20B2(this, val2, m_Rect.Width - size.Width - ᢙ, val.Font, eTextFormat.Default, (int)val.RightToLeft == 1);
					Size size2 = TextDrawing.MeasureString(val2, "Abc", val.Font);
					_ᢦ = empty.Height > size2.Height;
				}
				else
				{
					empty = _20B1._20B2(this, val2, 0, val.Font, eTextFormat.Default, (int)val.RightToLeft == 1);
				}
				empty.Width++;
				_ᢧ = empty;
				if (ᢝ == eCheckBoxPosition.Left || ᢝ == eCheckBoxPosition.Right)
				{
					m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, empty.Width + num2 + size.Width, Math.Max(size.Height, empty.Height) + num * 2);
				}
				else
				{
					m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, Math.Max(empty.Width, size.Width) + num * 2, empty.Height + num2 + size.Height);
				}
			}
			finally
			{
				val2.Dispose();
			}
		}
		else
		{
			Size size3 = size;
			size3.Width += num * 2;
			size3.Height += num * 2;
			m_Rect = new Rectangle(m_Rect.Location, size3);
		}
		base.RecalcSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !ޗ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		if (!DesignMode && _ᢥ)
		{
			ݡ = true;
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
			ݡ = false;
			ݠ = false;
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
		if ((int)objArg.Button == 1048576 && !DesignMode && _ᢥ)
		{
			ݠ = true;
			if (GetEnabled())
			{
				Refresh();
			}
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		if (ݠ && !DesignMode)
		{
			ݠ = false;
			if (GetEnabled())
			{
				Refresh();
			}
		}
	}

	public override void RaiseClick(eEventSource source)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		if (_ᢥ && base.CanRaiseClick && (CheckBoxStyle != eCheckBoxStyle.RadioButton || !Checked))
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
		base.RaiseClick(source);
	}

	public virtual void SetChecked(bool newValue, eEventSource source)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		if (ᢛ == eCheckBoxStyle.RadioButton && newValue && Parent != null)
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
		ݟ = newValue;
		if ((ݟ && (int)ᢡ == 0) || (!ݟ && (int)ᢡ != 0))
		{
			ᢡ = (CheckState)(ݟ ? 1 : 0);
		}
		if (Command != null)
		{
			Command.Checked = ݟ;
		}
		OnCheckedChanged(source);
		OnCheckedBindableChanged(EventArgs.Empty);
		if (ShouldSyncProperties)
		{
			BarFunctions.ढ़(this, "Checked");
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
			ᢡ = newValue;
			ݟ = (int)newValue != 0;
			if (Command != null)
			{
				Command.Checked = ݟ;
			}
			OnCheckedChanged(source);
			OnCheckStateChanged(EventArgs.Empty);
			if (ShouldSyncProperties)
			{
				BarFunctions.ढ़(this, "CheckState");
			}
			if (Displayed)
			{
				Refresh();
			}
		}
	}

	protected virtual void OnCheckStateChanged(EventArgs eventArgs)
	{
		if (ᢣ != null)
		{
			ᢣ(this, eventArgs);
		}
	}

	protected virtual void OnCheckedBindableChanged(EventArgs eventArgs)
	{
		if (ᢤ != null)
		{
			ᢤ(this, eventArgs);
		}
	}

	protected virtual void OnCheckedChanged(eEventSource source)
	{
		CheckBoxItem oldchecked = null;
		if (ᢛ == eCheckBoxStyle.RadioButton && ݟ && Parent != null)
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
		if (ᢢ != null)
		{
			ᢢ(this, e);
		}
	}

	protected virtual void InvokeCheckedChanged(CheckBoxChangeEventArgs e)
	{
		if (_0E65 != null)
		{
			_0E65(this, e);
		}
	}

	private void _0655()
	{
		NeedRecalcSize = true;
		Refresh();
	}
}
