using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.ColorPickerItem;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[DefaultEvent("SelectedColorChanged")]
public class ColorPickerDropDown : ButtonItem
{
	private bool m_ColorsInitialized;

	private bool m_DisplayThemeColors = true;

	private bool m_DisplayStandardColors = true;

	private bool m_DisplayMoreColors = true;

	private bool m_Localized;

	private string m_ThemeColorsLabel = "Theme Colors";

	private string m_StandardColorsLabel = "Standard Colors";

	private string m_MoreColorsLabel = "&More Colors...";

	private string m_NoFillLabel = "&No Fill";

	private ButtonItem m_NoFillButton;

	private Color m_SelectedColor = Color.Empty;

	private Rectangle m_SelectedColorRectangle = Rectangle.Empty;

	private bool m_SelectedImageCreated;

	private bool lockSelectedColor;

	private IWin32Window _OwnerWindow;

	private ColorItem[][] _CustomThemeColors;

	private ColorItem[][] _CustomStandardColors;

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsFromStyle { get; set; }

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectedColor
	{
		get
		{
			return m_SelectedColor;
		}
		set
		{
			if (!lockSelectedColor)
			{
				lockSelectedColor = true;
				try
				{
					SetSelectedColor(value, raiseClick: false);
				}
				finally
				{
					lockSelectedColor = false;
				}
				Invalidate();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	[Description("Indicates whether theme colors are displayed on drop-down.")]
	public bool DisplayThemeColors
	{
		get
		{
			return m_DisplayThemeColors;
		}
		set
		{
			m_DisplayThemeColors = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	[Description("Indicates whether standard colors are displayed on drop-down.")]
	public bool DisplayStandardColors
	{
		get
		{
			return m_DisplayStandardColors;
		}
		set
		{
			m_DisplayStandardColors = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[DevCoBrowsable(true)]
	[DevCoSerialize]
	[Category("Appearance")]
	[Description("Indicates more colors menu item is visible which allows user to open Custom Colors dialog box.")]
	public bool DisplayMoreColors
	{
		get
		{
			return m_DisplayMoreColors;
		}
		set
		{
			m_DisplayMoreColors = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates rectangle in Image coordinates where selected color will be painted. Property will have effect only if Image property is used to set the image.")]
	[Category("Behaviour")]
	public Rectangle SelectedColorImageRectangle
	{
		get
		{
			return m_SelectedColorRectangle;
		}
		set
		{
			m_SelectedColorRectangle = value;
			UpdateSelectedColorImage();
		}
	}

	[Browsable(false)]
	public bool ColorsInitialized => m_ColorsInitialized;

	[Browsable(false)]
	[DefaultValue(null)]
	public IWin32Window OwnerWindow
	{
		get
		{
			return _OwnerWindow;
		}
		set
		{
			_OwnerWindow = value;
		}
	}

	[DefaultValue(null)]
	[Category("Colors")]
	[Description("Array of ColorItem objects that will be used as theme colors instead of built-in color palette.")]
	public ColorItem[][] CustomThemeColors
	{
		get
		{
			return _CustomThemeColors;
		}
		set
		{
			_CustomThemeColors = value;
			m_ColorsInitialized = false;
		}
	}

	[DefaultValue(null)]
	[Category("Colors")]
	[Description("Array of ColorItem objects that will be used as standard colors instead of built-in color palette.")]
	public ColorItem[][] CustomStandardColors
	{
		get
		{
			return _CustomStandardColors;
		}
		set
		{
			_CustomStandardColors = value;
			m_ColorsInitialized = false;
		}
	}

	protected override bool ShouldAutoExpandOnClick
	{
		get
		{
			if (!base.ShouldAutoExpandOnClick)
			{
				return m_SelectedColor.IsEmpty;
			}
			return true;
		}
	}

	[Description("Occurs when color is chosen from drop-down color picker or from Custom Colors dialog box.")]
	public event EventHandler SelectedColorChanged;

	[Description("Occurs when mouse is moving over the colors presented by the color picker")]
	public event ColorPreviewEventHandler ColorPreview;

	public event CancelObjectValueEventHandler BeforeColorDialog;

	public void Localize()
	{
		m_ThemeColorsLabel = StiLocalization.Get("Gui", "colorpicker_themecolorslabel");
		m_StandardColorsLabel = StiLocalization.Get("Gui", "colorpicker_standardcolorslabel");
		m_MoreColorsLabel = StiLocalization.Get("Gui", "colorpicker_morecolors");
		m_NoFillLabel = (IsFromStyle ? StiLocalization.Get("FormStyleDesigner", "FromStyle") : StiLocalization.Get("Gui", "colorpicker_nofill"));
		m_ColorsInitialized = false;
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
		colorPickerDropDown.SelectedColorChanged = this.SelectedColorChanged;
		colorPickerDropDown.CustomStandardColors = CustomStandardColors;
		colorPickerDropDown.CustomThemeColors = CustomThemeColors;
		colorPickerDropDown.SelectedColorImageRectangle = SelectedColorImageRectangle;
		colorPickerDropDown.SelectedColor = SelectedColor;
		base.CopyToItem((BaseItem)colorPickerDropDown);
	}

	protected virtual void OnColorPreview(ColorPreviewEventArgs e)
	{
		if (this.ColorPreview != null)
		{
			this.ColorPreview(this, e);
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
		return !m_SelectedColorRectangle.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedColorImageRectangle()
	{
		SelectedColorImageRectangle = Rectangle.Empty;
	}

	protected override void OnStyleChanged()
	{
		m_ColorsInitialized = false;
		base.OnStyleChanged();
	}

	protected override void OnPopupOpen(PopupOpenEventArgs e)
	{
		Localize();
		if (!m_ColorsInitialized)
		{
			InitializeColorDropDown();
		}
		m_NoFillButton.Checked = SelectedColor == Color.Transparent;
		base.OnPopupOpen(e);
	}

	private void InitializeColorDropDown()
	{
		SubItems.Clear();
		LoadResources();
		if (m_DisplayThemeColors)
		{
			LabelItem labelItem = new LabelItem();
			labelItem.CanCustomize = false;
			labelItem.Text = m_ThemeColorsLabel;
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
			AddColors(themeColors);
		}
		if (m_DisplayStandardColors)
		{
			LabelItem labelItem2 = new LabelItem();
			labelItem2.CanCustomize = false;
			labelItem2.Text = m_StandardColorsLabel;
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
			AddColors(standardColors);
		}
		string stepName = StiScale2.StepName;
		ButtonItem buttonItem = new ButtonItem();
		buttonItem.Text = m_NoFillLabel;
		buttonItem.CanCustomize = false;
		buttonItem.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.NoFill" + stepName + ".png");
		buttonItem.Click += NoFillButtonClick;
		buttonItem.BeginGroup = true;
		m_NoFillButton = buttonItem;
		SubItems.Add(buttonItem);
		if (m_DisplayMoreColors)
		{
			string text = "";
			if (StiScale2.Factor >= 4.0)
			{
				text = "_x4";
			}
			else if (StiScale2.Factor >= 3.0)
			{
				text = "_x3";
			}
			else if (StiScale2.Factor >= 2.0)
			{
				text = "_x2";
			}
			else if (StiScale2.Factor >= 1.5)
			{
				text = "_x1_5";
			}
			ButtonItem buttonItem2 = new ButtonItem("sys_CPMoreColors", m_MoreColorsLabel);
			buttonItem2.CanCustomize = false;
			buttonItem2.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.ColorPickerCustomColors" + text + ".png");
			buttonItem2.Click += MoreColorsButtonClick;
			buttonItem2.BeginGroup = true;
			SubItems.Add(buttonItem2);
		}
		m_ColorsInitialized = true;
	}

	private void MoreColorsButtonClick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		DisplayMoreColorsDialog();
	}

	private void NoFillButtonClick(object sender, EventArgs e)
	{
		SetSelectedColor(Color.Transparent);
		BaseItem.CollapseAll(this);
	}

	public DialogResult DisplayMoreColorsDialog()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		DialogResult val = (DialogResult)2;
		CustomColorDialogMetro customColorDialogMetro = new CustomColorDialogMetro();
		((Form)customColorDialogMetro).StartPosition = (FormStartPosition)1;
		if (!SelectedColor.IsEmpty)
		{
			customColorDialogMetro.SetNewColor(SelectedColor);
			customColorDialogMetro.CurrentColor = SelectedColor;
		}
		LocalizeDialog(customColorDialogMetro);
		CancelObjectValueEventArgs cancelObjectValueEventArgs = new CancelObjectValueEventArgs(customColorDialogMetro);
		OnBeforeColorDialog(cancelObjectValueEventArgs);
		if (cancelObjectValueEventArgs.Cancel)
		{
			((Component)(object)customColorDialogMetro).Dispose();
			return (DialogResult)0;
		}
		if (_OwnerWindow != null)
		{
			if (_OwnerWindow is Form && ((Form)_OwnerWindow).TopMost)
			{
				((Form)customColorDialogMetro).TopMost = true;
			}
			((Form)customColorDialogMetro).ShowDialog(_OwnerWindow);
		}
		else
		{
			((Form)customColorDialogMetro).ShowDialog();
		}
		val = ((Form)customColorDialogMetro).DialogResult;
		if ((int)val == 1 && !customColorDialogMetro.NewColor.IsEmpty)
		{
			SetSelectedColor(customColorDialogMetro.NewColor);
		}
		((Component)(object)customColorDialogMetro).Dispose();
		return val;
	}

	protected virtual void OnBeforeColorDialog(CancelObjectValueEventArgs e)
	{
		this.BeforeColorDialog?.Invoke(this, e);
	}

	private void LocalizeDialog(CustomColorDialogMetro d)
	{
		if (GetOwner() != null)
		{
			m_Localized = true;
		}
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogOKButton);
		if (localizedString != "")
		{
			((Control)d.buttonOK).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCancelButton);
		if (localizedString != "")
		{
			((Control)d.buttonCancel).Text = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogNewColorLabel);
		if (localizedString != "")
		{
			((Control)d.labelNewColor).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCurrentColorLabel);
		if (localizedString != "")
		{
			((Control)d.labelCurrentColor).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogStandardColorsLabel);
		if (localizedString != "")
		{
			((Control)d.labelStandardColors).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogCustomColorsLabel);
		if (localizedString != "")
		{
			((Control)d.labelCustomColors).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogGreenLabel);
		if (localizedString != "")
		{
			((Control)d.labelGreen).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogBlueLabel);
		if (localizedString != "")
		{
			((Control)d.labelBlue).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogRedLabel);
		if (localizedString != "")
		{
			((Control)d.labelRed).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerDialogAlphaLabel);
		if (localizedString != "")
		{
			((Control)d.labelAlpha).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerTabStandard);
		if (localizedString != "")
		{
			((Control)d.tabPageStandard).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerTabCustom);
		if (localizedString != "")
		{
			((Control)d.tabPageCustom).Text = localizedString.Replace(":", "");
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerCaption);
		if (localizedString != "")
		{
			((Control)d).Text = localizedString.Replace(":", "");
		}
	}

	private void AddColors(ArrayList colors)
	{
		int itemSpacing = 6;
		for (int i = 0; i < colors.Count; i++)
		{
			ItemContainer itemContainer = new ItemContainer();
			itemContainer.Orientation = eOrientation.Horizontal;
			itemContainer.ItemSpacing = itemSpacing;
			if (i == 0)
			{
				itemContainer.BackgroundStyle.MarginBottom = 9;
				itemContainer.BackgroundStyle.MarginTop = 6;
			}
			if (i == colors.Count - 1)
			{
				itemContainer.BackgroundStyle.MarginBottom = 6;
			}
			ColorItem[] array = (ColorItem[])colors[i];
			ColorItem[] array2 = array;
			foreach (ColorItem colorItem in array2)
			{
				itemContainer.SubItems.Add(colorItem);
				colorItem.Click += ColorItemClick;
				colorItem.AutoCollapseOnClick = false;
				colorItem.DesiredSize = new Size((int)Math.Ceiling(13.0 * StiScale2.X), (int)Math.Ceiling(13.0 * StiScale2.Y));
			}
			SubItems.Add(itemContainer);
		}
	}

	private void ColorItemClick(object sender, EventArgs e)
	{
		if (sender is ColorItem colorItem)
		{
			SetSelectedColor(colorItem.Color);
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

	private void SetSelectedColor(Color c)
	{
		SetSelectedColor(c, raiseClick: true);
	}

	private void SetSelectedColor(Color c, bool raiseClick)
	{
		Color[] colors = StiColors.Colors;
		for (int i = 0; i < colors.Length; i++)
		{
			Color color = colors[i];
			if (color.A == c.A && color.R == c.R && color.G == c.G && color.B == c.B)
			{
				c = color;
				break;
			}
		}
		bool flag = m_SelectedColor != c;
		m_SelectedColor = c;
		OnSelectedColorChanged(new EventArgs());
		if (raiseClick)
		{
			RaiseClick();
		}
		if (flag && ShouldSyncProperties)
		{
			BarFunctions.SyncProperty(this, "SelectedColor");
		}
		UpdateSelectedColorImage();
	}

	public void UpdateSelectedColorImage()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		if (m_SelectedColorRectangle.IsEmpty || m_SelectedColorRectangle.Width <= 0 || m_SelectedColorRectangle.Height <= 0 || base.Image == null || DesignMode)
		{
			return;
		}
		Bitmap val = new Bitmap(base.Image.Width, base.Image.Height, (PixelFormat)2498570);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			Rectangle selectedColorRectangle = m_SelectedColorRectangle;
			val2.DrawImage(base.Image, 0, 0);
			Color color = SelectedColor;
			if (color.IsEmpty || color == Color.Transparent)
			{
				color = Color.White;
			}
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				val2.FillRectangle((Brush)(object)val3, selectedColorRectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (m_SelectedImageCreated)
		{
			Image image = base.Image;
			base.Image = (Image)(object)val;
			image.Dispose();
		}
		else
		{
			base.Image = (Image)(object)val;
		}
		m_SelectedImageCreated = true;
	}

	protected virtual ArrayList GetThemeColors()
	{
		ArrayList arrayList = new ArrayList();
		if (_CustomThemeColors == null || _CustomThemeColors.Length == 0)
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro)
			{
				arrayList.Add(new ColorItem[10]
				{
					GetColorItem("FFFFFF"),
					GetColorItem("000000"),
					GetColorItem("EEECE1"),
					GetColorItem("1F497D"),
					GetColorItem("4F81BD"),
					GetColorItem("C0504D"),
					GetColorItem("9BBB59"),
					GetColorItem("8064A2"),
					GetColorItem("4BACC6"),
					GetColorItem("F79646")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemTop("F2F2F2"),
					GetColorItemTop("7F7F7F"),
					GetColorItemTop("DDD9C3"),
					GetColorItemTop("C6D9F0"),
					GetColorItemTop("DBE5F1"),
					GetColorItemTop("F2DCDB"),
					GetColorItemTop("EBF1DD"),
					GetColorItemTop("E5E0EC"),
					GetColorItemTop("DBEEF3"),
					GetColorItemTop("FDEADA")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemMiddle("D8D8D8"),
					GetColorItemMiddle("595959"),
					GetColorItemMiddle("C4BD97"),
					GetColorItemMiddle("8DB3E2"),
					GetColorItemMiddle("B8CCE4"),
					GetColorItemMiddle("E5B9B7"),
					GetColorItemMiddle("D7E3BC"),
					GetColorItemMiddle("CCC1D9"),
					GetColorItemMiddle("B7DDE8"),
					GetColorItemMiddle("FBD5B5")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemMiddle("BFBFBF"),
					GetColorItemMiddle("3F3F3F"),
					GetColorItemMiddle("938953"),
					GetColorItemMiddle("548DD4"),
					GetColorItemMiddle("95B3D7"),
					GetColorItemMiddle("D99694"),
					GetColorItemMiddle("C3D69B"),
					GetColorItemMiddle("B2A1C7"),
					GetColorItemMiddle("92CDDC"),
					GetColorItemMiddle("FAC08F")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemMiddle("A5A5A5"),
					GetColorItemMiddle("262626"),
					GetColorItemMiddle("494429"),
					GetColorItemMiddle("17365D"),
					GetColorItemMiddle("366092"),
					GetColorItemMiddle("953734"),
					GetColorItemMiddle("76923C"),
					GetColorItemMiddle("5F497A"),
					GetColorItemMiddle("31859B"),
					GetColorItemMiddle("E36C09")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemBottom("7F7F7F"),
					GetColorItemBottom("0C0C0C"),
					GetColorItemBottom("1D1B10"),
					GetColorItemBottom("0F243E"),
					GetColorItemBottom("244061"),
					GetColorItemBottom("632423"),
					GetColorItemBottom("4F6128"),
					GetColorItemBottom("3F3151"),
					GetColorItemBottom("205867"),
					GetColorItemBottom("974806")
				});
			}
			else
			{
				arrayList.Add(new ColorItem[10]
				{
					GetColorItem("FFFFFF"),
					GetColorItem("000000"),
					GetColorItem("FAF3E8"),
					GetColorItem("1F497D"),
					GetColorItem("5C83B4"),
					GetColorItem("C0504D"),
					GetColorItem("9DBB61"),
					GetColorItem("8066A0"),
					GetColorItem("4BACC6"),
					GetColorItem("F59D56")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemTop("D8D8D8"),
					GetColorItemTop("7F7F7F"),
					GetColorItemTop("BBB6AE"),
					GetColorItemTop("C7D1DE"),
					GetColorItemTop("D6E0EC"),
					GetColorItemTop("EFD3D2"),
					GetColorItemTop("E6EED7"),
					GetColorItemTop("DFD8E7"),
					GetColorItemTop("D2EAF0"),
					GetColorItemTop("FCE6D4")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemMiddle("BFBFBF"),
					GetColorItemMiddle("727272"),
					GetColorItemMiddle("A29D96"),
					GetColorItemMiddle("8FA4BE"),
					GetColorItemMiddle("ADC1D9"),
					GetColorItemMiddle("DFA7A6"),
					GetColorItemMiddle("CEDDB0"),
					GetColorItemMiddle("BFB2CF"),
					GetColorItemMiddle("A5D5E2"),
					GetColorItemMiddle("FACEAA")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemMiddle("A5A5A5"),
					GetColorItemMiddle("595959"),
					GetColorItemMiddle("7D7974"),
					GetColorItemMiddle("57769D"),
					GetColorItemMiddle("84A2C6"),
					GetColorItemMiddle("CF7B79"),
					GetColorItemMiddle("B5CC88"),
					GetColorItemMiddle("9F8CB7"),
					GetColorItemMiddle("78C0D4"),
					GetColorItemMiddle("F7B580")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemMiddle("8C8C8C"),
					GetColorItemMiddle("3F3F3F"),
					GetColorItemMiddle("575551"),
					GetColorItemMiddle("17365D"),
					GetColorItemMiddle("456287"),
					GetColorItemMiddle("903C39"),
					GetColorItemMiddle("758C48"),
					GetColorItemMiddle("604C78"),
					GetColorItemMiddle("388194"),
					GetColorItemMiddle("B77540")
				});
				arrayList.Add(new ColorItem[10]
				{
					GetColorItemBottom("7F7F7F"),
					GetColorItemBottom("262626"),
					GetColorItemBottom("3E3C3A"),
					GetColorItemBottom("0F243E"),
					GetColorItemBottom("2E415A"),
					GetColorItemBottom("602826"),
					GetColorItemBottom("4E5D30"),
					GetColorItemBottom("403350"),
					GetColorItemBottom("255663"),
					GetColorItemBottom("7A4E2B")
				});
			}
		}
		else
		{
			arrayList.AddRange(_CustomThemeColors);
		}
		return arrayList;
	}

	protected virtual ArrayList GetStandardColors()
	{
		ArrayList arrayList = new ArrayList();
		if (_CustomStandardColors == null || _CustomStandardColors.Length == 0)
		{
			eDotNetBarStyle effectiveStyle = EffectiveStyle;
			if (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Metro)
			{
				arrayList.Add(new ColorItem[10]
				{
					GetColorItem("C00000"),
					GetColorItem("FF0000"),
					GetColorItem("FFC000"),
					GetColorItem("FFFF00"),
					GetColorItem("92D050"),
					GetColorItem("00B050"),
					GetColorItem("00B0F0"),
					GetColorItem("0070C0"),
					GetColorItem("002060"),
					GetColorItem("7030A0")
				});
			}
			else
			{
				arrayList.Add(new ColorItem[10]
				{
					GetColorItem("BA1419"),
					GetColorItem("ED1C24"),
					GetColorItem("FFC20E"),
					GetColorItem("FFF200"),
					GetColorItem("9DDA4E"),
					GetColorItem("22B14C"),
					GetColorItem("00B7EF"),
					GetColorItem("0072BC"),
					GetColorItem("2F3699"),
					GetColorItem("6F3198")
				});
			}
		}
		else
		{
			arrayList.AddRange(_CustomStandardColors);
		}
		return arrayList;
	}

	private ColorItem GetColorItem(string color)
	{
		return new ColorItem("", "", ColorScheme.GetColor(color));
	}

	private ColorItem GetColorItemTop(string color)
	{
		ColorItem colorItem = new ColorItem("", "", ColorScheme.GetColor(color));
		eColorItemBorder border = eColorItemBorder.Left | eColorItemBorder.Right | eColorItemBorder.Top;
		colorItem.Border = border;
		return colorItem;
	}

	private ColorItem GetColorItemMiddle(string color)
	{
		ColorItem colorItem = new ColorItem("", "", ColorScheme.GetColor(color));
		eColorItemBorder border = eColorItemBorder.Left | eColorItemBorder.Right;
		colorItem.Border = border;
		return colorItem;
	}

	private ColorItem GetColorItemBottom(string color)
	{
		ColorItem colorItem = new ColorItem("", "", ColorScheme.GetColor(color));
		eColorItemBorder border = eColorItemBorder.Left | eColorItemBorder.Right | eColorItemBorder.Bottom;
		colorItem.Border = border;
		return colorItem;
	}

	private void LoadResources()
	{
		if (m_Localized)
		{
			return;
		}
		if (GetOwner() != null)
		{
			m_Localized = true;
		}
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		string localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerThemeColorsLabel);
		if (localizedString != "")
		{
			m_ThemeColorsLabel = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerStandardColorsLabel);
		if (localizedString != "")
		{
			m_StandardColorsLabel = localizedString;
		}
		localizedString = localizationManager.GetLocalizedString(LocalizationKeys.ColorPickerMoreColorsMenuItem);
		if (localizedString != "")
		{
			m_MoreColorsLabel = localizedString;
		}
	}

	protected virtual void OnSelectedColorChanged(EventArgs e)
	{
		if (this.SelectedColorChanged != null)
		{
			this.SelectedColorChanged(this, e);
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
