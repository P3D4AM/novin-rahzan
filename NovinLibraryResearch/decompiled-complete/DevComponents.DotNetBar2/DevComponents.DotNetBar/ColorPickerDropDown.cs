using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.ColorPickerItem;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[DefaultEvent("SelectedColorChanged")]
public class ColorPickerDropDown : ButtonItem
{
	private EventHandler Ṳ;

	private ColorPreviewEventHandler ṳ;

	private CancelObjectValueEventHandler Ṵ;

	private bool ṵ;

	private bool Ṷ = true;

	private bool ṷ = true;

	private bool Ṹ = true;

	private bool _0DF2;

	private string ṹ = "Theme Colors";

	private string Ṻ = "Standard Colors";

	private string ṻ = "&More Colors...";

	private Color Ṽ = Color.Empty;

	private Rectangle ṽ = Rectangle.Empty;

	private bool Ṿ;

	private EventHandler ṿ;

	private IWin32Window _Ẁ;

	private ColorItem[][] _ẁ;

	private ColorItem[][] _Ẃ;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectedColor
	{
		get
		{
			return Ṽ;
		}
		set
		{
			Ẋ(value, ẋ: false);
		}
	}

	[Browsable(true)]
	[DevCoSerialize]
	[Description("Indicates whether theme colors are displayed on drop-down.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool DisplayThemeColors
	{
		get
		{
			return Ṷ;
		}
		set
		{
			Ṷ = value;
		}
	}

	[Category("Appearance")]
	[DevCoSerialize]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Indicates whether standard colors are displayed on drop-down.")]
	public bool DisplayStandardColors
	{
		get
		{
			return ṷ;
		}
		set
		{
			ṷ = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Description("Indicates more colors menu item is visible which allows user to open Custom Colors dialog box.")]
	[Browsable(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	public bool DisplayMoreColors
	{
		get
		{
			return Ṹ;
		}
		set
		{
			Ṹ = value;
		}
	}

	[Category("Behaviour")]
	[Browsable(true)]
	[Description("Indicates rectangle in Image coordinates where selected color will be painted. Property will have effect only if Image property is used to set the image.")]
	public Rectangle SelectedColorImageRectangle
	{
		get
		{
			return ṽ;
		}
		set
		{
			ṽ = value;
			UpdateSelectedColorImage();
		}
	}

	[Browsable(false)]
	public bool ColorsInitialized => ṵ;

	[DefaultValue(null)]
	[Browsable(false)]
	public IWin32Window OwnerWindow
	{
		get
		{
			return _Ẁ;
		}
		set
		{
			_Ẁ = value;
		}
	}

	[DefaultValue(null)]
	[Category("Colors")]
	[Description("Array of ColorItem objects that will be used as theme colors instead of built-in color palette.")]
	public ColorItem[][] CustomThemeColors
	{
		get
		{
			return _ẁ;
		}
		set
		{
			_ẁ = value;
			ṵ = false;
		}
	}

	[Category("Colors")]
	[DefaultValue(null)]
	[Description("Array of ColorItem objects that will be used as standard colors instead of built-in color palette.")]
	public ColorItem[][] CustomStandardColors
	{
		get
		{
			return _Ẃ;
		}
		set
		{
			_Ẃ = value;
			ṵ = false;
		}
	}

	protected override bool ShouldAutoExpandOnClick
	{
		get
		{
			if (!base.ShouldAutoExpandOnClick)
			{
				return Ṽ.IsEmpty;
			}
			return true;
		}
	}

	[Description("Occurs when color is chosen from drop-down color picker or from Custom Colors dialog box.")]
	public event EventHandler SelectedColorChanged
	{
		add
		{
			EventHandler eventHandler = Ṳ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ṳ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ṳ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ṳ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when mouse is moving over the colors presented by the color picker")]
	public event ColorPreviewEventHandler ColorPreview
	{
		add
		{
			ColorPreviewEventHandler colorPreviewEventHandler = ṳ;
			ColorPreviewEventHandler colorPreviewEventHandler2;
			do
			{
				colorPreviewEventHandler2 = colorPreviewEventHandler;
				ColorPreviewEventHandler value2 = (ColorPreviewEventHandler)Delegate.Combine(colorPreviewEventHandler2, value);
				colorPreviewEventHandler = Interlocked.CompareExchange(ref ṳ, value2, colorPreviewEventHandler2);
			}
			while ((object)colorPreviewEventHandler != colorPreviewEventHandler2);
		}
		remove
		{
			ColorPreviewEventHandler colorPreviewEventHandler = ṳ;
			ColorPreviewEventHandler colorPreviewEventHandler2;
			do
			{
				colorPreviewEventHandler2 = colorPreviewEventHandler;
				ColorPreviewEventHandler value2 = (ColorPreviewEventHandler)Delegate.Remove(colorPreviewEventHandler2, value);
				colorPreviewEventHandler = Interlocked.CompareExchange(ref ṳ, value2, colorPreviewEventHandler2);
			}
			while ((object)colorPreviewEventHandler != colorPreviewEventHandler2);
		}
	}

	public event CancelObjectValueEventHandler BeforeColorDialog
	{
		add
		{
			CancelObjectValueEventHandler cancelObjectValueEventHandler = Ṵ;
			CancelObjectValueEventHandler cancelObjectValueEventHandler2;
			do
			{
				cancelObjectValueEventHandler2 = cancelObjectValueEventHandler;
				CancelObjectValueEventHandler value2 = (CancelObjectValueEventHandler)Delegate.Combine(cancelObjectValueEventHandler2, value);
				cancelObjectValueEventHandler = Interlocked.CompareExchange(ref Ṵ, value2, cancelObjectValueEventHandler2);
			}
			while ((object)cancelObjectValueEventHandler != cancelObjectValueEventHandler2);
		}
		remove
		{
			CancelObjectValueEventHandler cancelObjectValueEventHandler = Ṵ;
			CancelObjectValueEventHandler cancelObjectValueEventHandler2;
			do
			{
				cancelObjectValueEventHandler2 = cancelObjectValueEventHandler;
				CancelObjectValueEventHandler value2 = (CancelObjectValueEventHandler)Delegate.Remove(cancelObjectValueEventHandler2, value);
				cancelObjectValueEventHandler = Interlocked.CompareExchange(ref Ṵ, value2, cancelObjectValueEventHandler2);
			}
			while ((object)cancelObjectValueEventHandler != cancelObjectValueEventHandler2);
		}
	}

	[Description("Occurs after popup colors have been added to the SubItems collection. This event can be used to effectively add custom items to the popup.")]
	public event EventHandler ColorItemsInitialized
	{
		add
		{
			EventHandler eventHandler = ṿ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ṿ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ṿ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ṿ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public ColorPickerDropDown()
		: this("", "")
	{
	}

	public ColorPickerDropDown(string sItemName)
		: this(sItemName, "")
	{
	}

	public ColorPickerDropDown(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		SubItems.Add(new ButtonItem("tempColorPickerItem"));
	}

	public override BaseItem Copy()
	{
		ColorPickerDropDown colorPickerDropDown = new ColorPickerDropDown();
		CopyToItem(colorPickerDropDown);
		return colorPickerDropDown;
	}

	protected override void CopyToItem(BaseItem c)
	{
		ColorPickerDropDown colorPickerDropDown = c as ColorPickerDropDown;
		colorPickerDropDown.DisplayMoreColors = DisplayMoreColors;
		colorPickerDropDown.DisplayStandardColors = DisplayStandardColors;
		colorPickerDropDown.DisplayThemeColors = DisplayThemeColors;
		colorPickerDropDown.Ṳ = Ṳ;
		colorPickerDropDown.CustomStandardColors = ẃ(CustomStandardColors);
		colorPickerDropDown.CustomThemeColors = ẃ(CustomThemeColors);
		colorPickerDropDown.SelectedColorImageRectangle = SelectedColorImageRectangle;
		colorPickerDropDown.SelectedColor = SelectedColor;
		base.CopyToItem((BaseItem)colorPickerDropDown);
	}

	private ColorItem[][] ẃ(ColorItem[][] Ẅ)
	{
		if (Ẅ == null || Ẅ.Length == 0)
		{
			return null;
		}
		ColorItem[][] array = new ColorItem[Ẅ.Length][];
		for (int i = 0; i < Ẅ.Length; i++)
		{
			ColorItem[] array2 = Ẅ[i];
			array[i] = new ColorItem[array2.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				array[i][j] = (ColorItem)array2[j].Copy();
			}
		}
		return array;
	}

	protected virtual void OnColorPreview(ColorPreviewEventArgs e)
	{
		if (ṳ != null)
		{
			ṳ(this, e);
		}
	}

	public void InvokeColorPreview(ColorPreviewEventArgs e)
	{
		OnColorPreview(e);
	}

	protected override bool ShouldSerializeSubItems()
	{
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSelectedColorImageRectangle()
	{
		return !ṽ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedColorImageRectangle()
	{
		SelectedColorImageRectangle = Rectangle.Empty;
	}

	protected override void OnStyleChanged()
	{
		ṵ = false;
		base.OnStyleChanged();
	}

	protected override void OnPopupOpen(PopupOpenEventArgs e)
	{
		base.OnPopupOpen(e);
		if (!ṵ)
		{
			ẅ();
		}
	}

	private void ẅ()
	{
		SubItems.Clear();
		Ẑ();
		if (Ṷ)
		{
			LabelItem labelItem = new LabelItem();
			labelItem.CanCustomize = false;
			labelItem.Text = ṹ;
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				labelItem.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Gallery.GroupLabelBackground;
				labelItem.ForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Gallery.GroupLabelText;
				labelItem.SingleLineColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Gallery.GroupLabelBorder;
			}
			else
			{
				labelItem.BackColor = ColorScheme.GetColor("DDE7EE");
				labelItem.ForeColor = ColorScheme.GetColor("00156E");
				labelItem.SingleLineColor = ColorScheme.GetColor("C5C5C5");
			}
			labelItem.PaddingBottom = 1;
			labelItem.PaddingLeft = 0;
			labelItem.PaddingRight = 0;
			labelItem.PaddingTop = 1;
			labelItem.BorderSide = eBorderSide.Bottom;
			labelItem.BorderType = eBorderType.SingleLine;
			SubItems.Add(labelItem);
			ArrayList themeColors = GetThemeColors();
			Ẉ(themeColors);
		}
		if (ṷ)
		{
			LabelItem labelItem2 = new LabelItem();
			labelItem2.CanCustomize = false;
			labelItem2.Text = Ṻ;
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				labelItem2.BackColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.QuickAccessToolbar.QatCustomizeMenuLabelBackground;
				labelItem2.ForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.QuickAccessToolbar.QatCustomizeMenuLabelText;
				labelItem2.SingleLineColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Gallery.GroupLabelBorder;
			}
			else
			{
				labelItem2.BackColor = ColorScheme.GetColor("DDE7EE");
				labelItem2.ForeColor = ColorScheme.GetColor("00156E");
				labelItem2.SingleLineColor = ColorScheme.GetColor("C5C5C5");
			}
			labelItem2.PaddingBottom = 1;
			labelItem2.PaddingLeft = 0;
			labelItem2.PaddingRight = 0;
			labelItem2.PaddingTop = 1;
			labelItem2.BorderSide = eBorderSide.Bottom;
			labelItem2.BorderType = eBorderType.SingleLine;
			SubItems.Add(labelItem2);
			ArrayList standardColors = GetStandardColors();
			Ẉ(standardColors);
		}
		if (Ṹ)
		{
			ButtonItem buttonItem = new ButtonItem("sys_CPMoreColors", ṻ);
			buttonItem.CanCustomize = false;
			buttonItem.Image = (Image)(object)BarFunctions.ই("SystemImages.ColorPickerCustomColors.png");
			buttonItem.Click += Ẇ;
			buttonItem.BeginGroup = true;
			SubItems.Add(buttonItem);
		}
		ṵ = true;
		OnColorItemsInitialized(EventArgs.Empty);
	}

	protected virtual void OnColorItemsInitialized(EventArgs e)
	{
		ṿ?.Invoke(this, e);
	}

	private void Ẇ(object _٠, EventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		DisplayMoreColorsDialog();
	}

	public DialogResult DisplayMoreColorsDialog()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Invalid comparison between Unknown and I4
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		DialogResult val = (DialogResult)2;
		if (StyleManager.IsMetro(EffectiveStyle))
		{
			Ẕ ẕ = new Ẕ();
			ẕ._᱐(EffectiveStyle);
			((Form)ẕ).StartPosition = (FormStartPosition)1;
			if (!SelectedColor.IsEmpty)
			{
				ẕ.ẕ = SelectedColor;
			}
			ẇ(ẕ);
			CancelObjectValueEventArgs cancelObjectValueEventArgs = new CancelObjectValueEventArgs(ẕ);
			OnBeforeColorDialog(cancelObjectValueEventArgs);
			if (cancelObjectValueEventArgs.Cancel)
			{
				((Component)(object)ẕ).Dispose();
				return (DialogResult)0;
			}
			if (_Ẁ != null)
			{
				if (_Ẁ is Form && ((Form)_Ẁ).TopMost)
				{
					((Form)ẕ).TopMost = true;
				}
				((Form)ẕ).ShowDialog(_Ẁ);
			}
			else
			{
				((Form)ẕ).ShowDialog();
			}
			val = ((Form)ẕ).DialogResult;
			if ((int)val == 1 && !ẕ.ẖ.IsEmpty)
			{
				Ẋ(ẕ.ẖ);
			}
			((Component)(object)ẕ).Dispose();
		}
		else
		{
			_235F _235F = new _235F();
			_235F._᱐(EffectiveStyle);
			((Form)_235F).StartPosition = (FormStartPosition)1;
			if (!SelectedColor.IsEmpty)
			{
				_235F.ẕ = SelectedColor;
			}
			ẇ(_235F);
			CancelObjectValueEventArgs cancelObjectValueEventArgs2 = new CancelObjectValueEventArgs(_235F);
			OnBeforeColorDialog(cancelObjectValueEventArgs2);
			if (cancelObjectValueEventArgs2.Cancel)
			{
				((Component)(object)_235F).Dispose();
				return (DialogResult)0;
			}
			if (_Ẁ != null)
			{
				if (_Ẁ is Form && ((Form)_Ẁ).TopMost)
				{
					((Form)_235F).TopMost = true;
				}
				((Form)_235F).ShowDialog(_Ẁ);
			}
			else
			{
				((Form)_235F).ShowDialog();
			}
			val = ((Form)_235F).DialogResult;
			if ((int)val == 1 && !_235F.ẖ.IsEmpty)
			{
				Ẋ(_235F.ẖ);
			}
			((Component)(object)_235F).Dispose();
		}
		return val;
	}

	protected virtual void OnBeforeColorDialog(CancelObjectValueEventArgs e)
	{
		Ṵ?.Invoke(this, e);
	}

	private void ẇ(_235F _0CC8)
	{
		if (GetOwner() != null)
		{
			_0DF2 = true;
		}
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogOKButton);
		if (localizedString != "")
		{
			((Control)_0CC8.ẗ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCancelButton);
		if (localizedString != "")
		{
			((Control)_0CC8.ẘ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogNewColorLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẙ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCurrentColorLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẚ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogStandardColorsLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẞ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCustomColorsLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẟ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogGreenLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.Ạ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogBlueLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ạ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogRedLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.Ẩ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerTabStandard);
		if (localizedString != "")
		{
			_0CC8.ả.Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerTabCustom);
		if (localizedString != "")
		{
			_0CC8.ấ.Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerCaption);
		if (localizedString != "")
		{
			((Control)_0CC8).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogColorModelLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẩ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogRgbLabel);
		if (localizedString != "")
		{
			_0CC8.Ẫ.Items[0] = localizedString;
		}
	}

	private void ẇ(Ẕ _0CC8)
	{
		if (GetOwner() != null)
		{
			_0DF2 = true;
		}
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogOKButton);
		if (localizedString != "")
		{
			((Control)_0CC8.ẗ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCancelButton);
		if (localizedString != "")
		{
			((Control)_0CC8.ẘ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogNewColorLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẙ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCurrentColorLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẚ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogStandardColorsLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẞ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCustomColorsLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẟ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogGreenLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.Ạ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogBlueLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ạ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogRedLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.Ẩ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerTabStandard);
		if (localizedString != "")
		{
			_0CC8.ả.Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerTabCustom);
		if (localizedString != "")
		{
			_0CC8.ấ.Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerCaption);
		if (localizedString != "")
		{
			((Control)_0CC8).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogColorModelLabel);
		if (localizedString != "")
		{
			((Control)_0CC8.ẩ).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogRgbLabel);
		if (localizedString != "")
		{
			_0CC8.Ẫ.Items[0] = localizedString;
		}
	}

	private void Ẉ(ArrayList ᛪ)
	{
		int itemSpacing = 3;
		for (int i = 0; i < ᛪ.Count; i++)
		{
			ItemContainer itemContainer = new ItemContainer();
			itemContainer.Orientation = eOrientation.Horizontal;
			itemContainer.ItemSpacing = itemSpacing;
			if (i == 0)
			{
				itemContainer.BackgroundStyle.MarginBottom = 5;
				itemContainer.BackgroundStyle.MarginTop = 3;
			}
			if (i == ᛪ.Count - 1)
			{
				itemContainer.BackgroundStyle.MarginBottom = 3;
			}
			ColorItem[] array = (ColorItem[])ᛪ[i];
			ColorItem[] array2 = array;
			foreach (ColorItem colorItem in array2)
			{
				itemContainer.SubItems.Add(colorItem);
				colorItem.Click += ẉ;
				colorItem.AutoCollapseOnClick = false;
			}
			SubItems.Add(itemContainer);
		}
	}

	private void ẉ(object _٠, EventArgs ؿ)
	{
		if (_٠ is ColorItem colorItem)
		{
			Ẋ(colorItem.Color);
			if (AutoCollapseOnClick)
			{
				BaseItem.CollapseAll(this);
			}
			else if (base.PopupControl != null)
			{
				base.PopupControl.Invalidate();
			}
		}
	}

	private void Ẋ(Color _0BDE)
	{
		Ẋ(_0BDE, ẋ: true);
	}

	private void Ẋ(Color _0BDE, bool ẋ)
	{
		bool flag = Ṽ != _0BDE;
		Ṽ = _0BDE;
		OnSelectedColorChanged(new EventArgs());
		if (ẋ)
		{
			RaiseClick();
		}
		if (flag && ShouldSyncProperties)
		{
			BarFunctions.ढ़(this, "SelectedColor");
		}
		UpdateSelectedColorImage();
	}

	protected override void ScaleItem(SizeF factor)
	{
		base.ScaleItem(factor);
		UpdateSelectedColorImage();
	}

	public void UpdateSelectedColorImage()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		if (ṽ.IsEmpty || ṽ.Width <= 0 || ṽ.Height <= 0 || base.Image == null || DesignMode)
		{
			return;
		}
		Bitmap val = new Bitmap(base.Image.Width, base.Image.Height, (PixelFormat)2498570);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			Rectangle rectangle = ṽ;
			val2.DrawImage(base.Image, 0, 0, base.Image.Width, base.Image.Height);
			Color color = SelectedColor;
			bool flag = false;
			if (color.IsEmpty || color == Color.Transparent)
			{
				color = Color.White;
				flag = true;
			}
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				val2.FillRectangle((Brush)(object)val3, rectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			if (flag && rectangle.Height > 5)
			{
				rectangle.Width--;
				rectangle.Height--;
				Pen val4 = new Pen(Color.Black);
				try
				{
					val2.SmoothingMode = (SmoothingMode)2;
					val2.DrawLine(val4, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
					val2.DrawLine(val4, rectangle.Right, rectangle.Y, rectangle.X, rectangle.Bottom);
					val2.SmoothingMode = (SmoothingMode)0;
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (Ṿ)
		{
			Image image = base.Image;
			base.Image = (Image)(object)val;
			image.Dispose();
		}
		else
		{
			base.Image = (Image)(object)val;
		}
		Ṿ = true;
	}

	protected virtual ArrayList GetThemeColors()
	{
		ArrayList arrayList = new ArrayList();
		if (_ẁ == null || _ẁ.Length == 0)
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (effectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(effectiveStyle))
			{
				arrayList.Add(new ColorItem[10]
				{
					Ẍ("FFFFFF"),
					Ẍ("000000"),
					Ẍ("EEECE1"),
					Ẍ("1F497D"),
					Ẍ("4F81BD"),
					Ẍ("C0504D"),
					Ẍ("9BBB59"),
					Ẍ("8064A2"),
					Ẍ("4BACC6"),
					Ẍ("F79646")
				});
				arrayList.Add(new ColorItem[10]
				{
					ẍ("F2F2F2"),
					ẍ("7F7F7F"),
					ẍ("DDD9C3"),
					ẍ("C6D9F0"),
					ẍ("DBE5F1"),
					ẍ("F2DCDB"),
					ẍ("EBF1DD"),
					ẍ("E5E0EC"),
					ẍ("DBEEF3"),
					ẍ("FDEADA")
				});
				arrayList.Add(new ColorItem[10]
				{
					Ẏ("D8D8D8"),
					Ẏ("595959"),
					Ẏ("C4BD97"),
					Ẏ("8DB3E2"),
					Ẏ("B8CCE4"),
					Ẏ("E5B9B7"),
					Ẏ("D7E3BC"),
					Ẏ("CCC1D9"),
					Ẏ("B7DDE8"),
					Ẏ("FBD5B5")
				});
				arrayList.Add(new ColorItem[10]
				{
					Ẏ("BFBFBF"),
					Ẏ("3F3F3F"),
					Ẏ("938953"),
					Ẏ("548DD4"),
					Ẏ("95B3D7"),
					Ẏ("D99694"),
					Ẏ("C3D69B"),
					Ẏ("B2A1C7"),
					Ẏ("92CDDC"),
					Ẏ("FAC08F")
				});
				arrayList.Add(new ColorItem[10]
				{
					Ẏ("A5A5A5"),
					Ẏ("262626"),
					Ẏ("494429"),
					Ẏ("17365D"),
					Ẏ("366092"),
					Ẏ("953734"),
					Ẏ("76923C"),
					Ẏ("5F497A"),
					Ẏ("31859B"),
					Ẏ("E36C09")
				});
				arrayList.Add(new ColorItem[10]
				{
					ẏ("7F7F7F"),
					ẏ("0C0C0C"),
					ẏ("1D1B10"),
					ẏ("0F243E"),
					ẏ("244061"),
					ẏ("632423"),
					ẏ("4F6128"),
					ẏ("3F3151"),
					ẏ("205867"),
					ẏ("974806")
				});
			}
			else
			{
				arrayList.Add(new ColorItem[10]
				{
					Ẍ("FFFFFF"),
					Ẍ("000000"),
					Ẍ("FAF3E8"),
					Ẍ("1F497D"),
					Ẍ("5C83B4"),
					Ẍ("C0504D"),
					Ẍ("9DBB61"),
					Ẍ("8066A0"),
					Ẍ("4BACC6"),
					Ẍ("F59D56")
				});
				arrayList.Add(new ColorItem[10]
				{
					ẍ("D8D8D8"),
					ẍ("7F7F7F"),
					ẍ("BBB6AE"),
					ẍ("C7D1DE"),
					ẍ("D6E0EC"),
					ẍ("EFD3D2"),
					ẍ("E6EED7"),
					ẍ("DFD8E7"),
					ẍ("D2EAF0"),
					ẍ("FCE6D4")
				});
				arrayList.Add(new ColorItem[10]
				{
					Ẏ("BFBFBF"),
					Ẏ("727272"),
					Ẏ("A29D96"),
					Ẏ("8FA4BE"),
					Ẏ("ADC1D9"),
					Ẏ("DFA7A6"),
					Ẏ("CEDDB0"),
					Ẏ("BFB2CF"),
					Ẏ("A5D5E2"),
					Ẏ("FACEAA")
				});
				arrayList.Add(new ColorItem[10]
				{
					Ẏ("A5A5A5"),
					Ẏ("595959"),
					Ẏ("7D7974"),
					Ẏ("57769D"),
					Ẏ("84A2C6"),
					Ẏ("CF7B79"),
					Ẏ("B5CC88"),
					Ẏ("9F8CB7"),
					Ẏ("78C0D4"),
					Ẏ("F7B580")
				});
				arrayList.Add(new ColorItem[10]
				{
					Ẏ("8C8C8C"),
					Ẏ("3F3F3F"),
					Ẏ("575551"),
					Ẏ("17365D"),
					Ẏ("456287"),
					Ẏ("903C39"),
					Ẏ("758C48"),
					Ẏ("604C78"),
					Ẏ("388194"),
					Ẏ("B77540")
				});
				arrayList.Add(new ColorItem[10]
				{
					ẏ("7F7F7F"),
					ẏ("262626"),
					ẏ("3E3C3A"),
					ẏ("0F243E"),
					ẏ("2E415A"),
					ẏ("602826"),
					ẏ("4E5D30"),
					ẏ("403350"),
					ẏ("255663"),
					ẏ("7A4E2B")
				});
			}
		}
		else
		{
			arrayList.AddRange(_ẁ);
		}
		return arrayList;
	}

	protected virtual ArrayList GetStandardColors()
	{
		ArrayList arrayList = new ArrayList();
		if (_Ẃ == null || _Ẃ.Length == 0)
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (effectiveStyle == eDotNetBarStyle.Office2010 || StyleManager.IsMetro(effectiveStyle))
			{
				arrayList.Add(new ColorItem[10]
				{
					Ẍ("C00000"),
					Ẍ("FF0000"),
					Ẍ("FFC000"),
					Ẍ("FFFF00"),
					Ẍ("92D050"),
					Ẍ("00B050"),
					Ẍ("00B0F0"),
					Ẍ("0070C0"),
					Ẍ("002060"),
					Ẍ("7030A0")
				});
			}
			else
			{
				arrayList.Add(new ColorItem[10]
				{
					Ẍ("BA1419"),
					Ẍ("ED1C24"),
					Ẍ("FFC20E"),
					Ẍ("FFF200"),
					Ẍ("9DDA4E"),
					Ẍ("22B14C"),
					Ẍ("00B7EF"),
					Ẍ("0072BC"),
					Ẍ("2F3699"),
					Ẍ("6F3198")
				});
			}
		}
		else
		{
			arrayList.AddRange(_Ẃ);
		}
		return arrayList;
	}

	private ColorItem Ẍ(string _065B)
	{
		return new ColorItem("", "", ColorScheme.GetColor(_065B));
	}

	private ColorItem ẍ(string _065B)
	{
		ColorItem colorItem = new ColorItem("", "", ColorScheme.GetColor(_065B));
		eColorItemBorder border = eColorItemBorder.Left | eColorItemBorder.Right | eColorItemBorder.Top;
		colorItem.Border = border;
		return colorItem;
	}

	private ColorItem Ẏ(string _065B)
	{
		ColorItem colorItem = new ColorItem("", "", ColorScheme.GetColor(_065B));
		eColorItemBorder border = eColorItemBorder.Left | eColorItemBorder.Right;
		colorItem.Border = border;
		return colorItem;
	}

	private ColorItem ẏ(string _065B)
	{
		ColorItem colorItem = new ColorItem("", "", ColorScheme.GetColor(_065B));
		eColorItemBorder border = eColorItemBorder.Left | eColorItemBorder.Right | eColorItemBorder.Bottom;
		colorItem.Border = border;
		return colorItem;
	}

	private void Ẑ()
	{
		if (_0DF2)
		{
			return;
		}
		if (GetOwner() != null)
		{
			_0DF2 = true;
		}
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerThemeColorsLabel);
		if (localizedString != "")
		{
			ṹ = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerStandardColorsLabel);
		if (localizedString != "")
		{
			Ṻ = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerMoreColorsMenuItem);
		if (localizedString != "")
		{
			ṻ = localizedString;
		}
	}

	protected virtual void OnSelectedColorChanged(EventArgs e)
	{
		if (Ṳ != null)
		{
			Ṳ(this, e);
		}
		if (GetOwner() is DotNetBarManager dotNetBarManager)
		{
			dotNetBarManager.InvokeColorPickerSelectedColorChanged(this);
		}
	}

	protected override void OnCommandChanged()
	{
	}
}
