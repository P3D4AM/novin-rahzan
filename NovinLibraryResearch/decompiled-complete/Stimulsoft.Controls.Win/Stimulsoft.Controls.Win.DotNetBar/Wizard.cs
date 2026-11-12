using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(Wizard), "Wizard.Wizard.ico")]
[ToolboxItem(false)]
public class Wizard : UserControl
{
	private PanelControl panelHeader;

	private int m_ButtonSpacingMajor = 8;

	private int m_ButtonSpacingMinor = 1;

	private int m_ButtonHeight = 22;

	private FlatStyle m_ButtonFlatStyle = (FlatStyle)3;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PanelControl panelFooter;

	private ButtonX buttonHelp;

	private ButtonX buttonCancel;

	private ButtonX buttonFinish;

	private ButtonX buttonNext;

	private ButtonX buttonBack;

	private bool m_FinishButtonAlwaysVisible;

	private Label labelDescription;

	private Label labelCaption;

	private WizardPageCollection m_WizardPages = new WizardPageCollection();

	private Stack m_PagesHistory = new Stack();

	private int m_SelectedPageIndex;

	private bool m_PageChangeDisableButtons = true;

	private bool m_PageChangeWaitCursor = true;

	private eWizardFormAcceptButton m_FormAcceptButton;

	private eWizardFormCancelButton m_FormCancelButton;

	private PictureBox pictureHeader;

	private Image m_HeaderImage;

	private bool m_HeaderImageChanged;

	private bool m_HeaderImageVisible = true;

	private bool m_HelpButtonVisible = true;

	private Container components;

	private eWizardTitleImageAlignment _HeaderImageAlignment = eWizardTitleImageAlignment.Right;

	private bool _HeaderDescriptionVisible = true;

	private bool _ButtonFocusCuesEnabled = true;

	private eWizardStyle m_WizardStyle;

	[Browsable(true)]
	[Category("Header and Footer")]
	[DefaultValue(null)]
	[Description("Indicates the header image.")]
	public Image HeaderImage
	{
		get
		{
			return m_HeaderImage;
		}
		set
		{
			m_HeaderImage = value;
			m_HeaderImageChanged = true;
			UpdatePageDisplay();
		}
	}

	[Browsable(true)]
	[Category("Header and Footer")]
	[DefaultValue(true)]
	[Description("Indicates whether header image is visible.")]
	public bool HeaderImageVisible
	{
		get
		{
			return m_HeaderImageVisible;
		}
		set
		{
			m_HeaderImageVisible = value;
			if (((Control)pictureHeader).Visible != m_HeaderImageVisible)
			{
				((Control)pictureHeader).Visible = m_HeaderImageVisible;
				LayoutHeader();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ControlCollection Controls => ((Control)this).Controls;

	[Browsable(true)]
	[DefaultValue(eWizardFormAcceptButton.FinishAndNext)]
	[Category("Wizard Behavior")]
	[Description("Indicates wizard button that is clicked when ENTER key is pressed.")]
	public eWizardFormAcceptButton FormAcceptButton
	{
		get
		{
			return m_FormAcceptButton;
		}
		set
		{
			m_FormAcceptButton = value;
			SetFormAcceptButton();
		}
	}

	[Browsable(true)]
	[DefaultValue(eWizardFormCancelButton.Cancel)]
	[Category("Wizard Behavior")]
	[Description("Indicates wizard button that is clicked when ESCAPE key is pressed.")]
	public eWizardFormCancelButton FormCancelButton
	{
		get
		{
			return m_FormCancelButton;
		}
		set
		{
			m_FormCancelButton = value;
			SetFormCancelButton();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Behavior")]
	[Description("Indicates whether all buttons are disabled while wizard page is changed.")]
	public bool PageChangeDisableButtons
	{
		get
		{
			return m_PageChangeDisableButtons;
		}
		set
		{
			m_PageChangeDisableButtons = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Behavior")]
	[Description("Indicates whether wait cursor is displayed while page is changed.")]
	public bool PageChangeWaitCursor
	{
		get
		{
			return m_PageChangeWaitCursor;
		}
		set
		{
			m_PageChangeWaitCursor = value;
		}
	}

	[Browsable(false)]
	[Category("Wizard Pages")]
	[Description("Indicates selected page index.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedPageIndex
	{
		get
		{
			return m_SelectedPageIndex;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value < m_WizardPages.Count)
			{
				ShowPage(m_WizardPages[value], eWizardPageChangeSource.Code);
			}
			else
			{
				m_SelectedPageIndex = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public WizardPage SelectedPage
	{
		get
		{
			if (m_SelectedPageIndex < m_WizardPages.Count)
			{
				return m_WizardPages[m_SelectedPageIndex];
			}
			return null;
		}
		set
		{
			if (!m_WizardPages.Contains(value))
			{
				throw new InvalidOperationException("WizardPage is not member of WizardPages collection. Add page to the WizardPages collection before setting this property");
			}
			ShowPage(value, eWizardPageChangeSource.Code);
		}
	}

	[Browsable(true)]
	[Category("Wizard Pages")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public WizardPageCollection WizardPages => m_WizardPages;

	[Browsable(false)]
	[Category("Wizard Pages")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Stack PagesHistory => m_PagesHistory;

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates flat style for buttons")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("Property is obsolete and no longer applies")]
	public FlatStyle ButtonFlatStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_ButtonFlatStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			m_ButtonFlatStyle = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(22)]
	[Category("Wizard Buttons")]
	[Description("Indicates the height of the wizard command buttons")]
	public int ButtonHeight
	{
		get
		{
			return m_ButtonHeight;
		}
		set
		{
			if (m_ButtonHeight != value)
			{
				m_ButtonHeight = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether back button causes validation to be performed on any controls that require validation when it receives focus.")]
	public bool BackButtonCausesValidation
	{
		get
		{
			return ((Control)buttonBack).CausesValidation;
		}
		set
		{
			((Control)buttonBack).CausesValidation = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(1)]
	[Category("Wizard Buttons")]
	[Description("Indicates tab index of back button.")]
	public int BackButtonTabIndex
	{
		get
		{
			return ((Control)buttonBack).TabIndex;
		}
		set
		{
			((Control)buttonBack).TabIndex = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether the user can give the focus to this back button using the TAB key.")]
	public bool BackButtonTabStop
	{
		get
		{
			return ((Control)buttonBack).TabStop;
		}
		set
		{
			((Control)buttonBack).TabStop = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue("< Back")]
	[Category("Wizard Buttons")]
	[Description("Indicates caption of the button")]
	public string BackButtonText
	{
		get
		{
			return ((Control)buttonBack).Text;
		}
		set
		{
			((Control)buttonBack).Text = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(74)]
	[Category("Wizard Buttons")]
	[Description("Indicates width of button")]
	public int BackButtonWidth
	{
		get
		{
			return ((Control)buttonBack).Width;
		}
		set
		{
			if (((Control)buttonBack).Width != value)
			{
				((Control)buttonBack).Width = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button is auto sized")]
	public bool BackButtonAutoSize
	{
		get
		{
			return ((Control)buttonBack).AutoSize;
		}
		set
		{
			if (((Control)buttonBack).AutoSize != value)
			{
				((Control)buttonBack).AutoSize = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Wizard Buttons")]
	[Description("Indicates button auto-size mode")]
	public AutoSizeMode BackButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return buttonBack.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (buttonBack.AutoSizeMode != value)
			{
				buttonBack.AutoSizeMode = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
	public bool NextButtonCausesValidation
	{
		get
		{
			return ((Control)buttonNext).CausesValidation;
		}
		set
		{
			((Control)buttonNext).CausesValidation = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(2)]
	[Category("Wizard Buttons")]
	[Description("Indicates tab index of next button.")]
	public int NextButtonTabIndex
	{
		get
		{
			return ((Control)buttonNext).TabIndex;
		}
		set
		{
			((Control)buttonNext).TabIndex = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether the user can give the focus to button using the TAB key.")]
	public bool NextButtonTabStop
	{
		get
		{
			return ((Control)buttonNext).TabStop;
		}
		set
		{
			((Control)buttonNext).TabStop = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue("Next >")]
	[Category("Wizard Buttons")]
	[Description("Indicates caption of the button")]
	public string NextButtonText
	{
		get
		{
			return ((Control)buttonNext).Text;
		}
		set
		{
			((Control)buttonNext).Text = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(74)]
	[Category("Wizard Buttons")]
	[Description("Indicates width of button")]
	public int NextButtonWidth
	{
		get
		{
			return ((Control)buttonNext).Width;
		}
		set
		{
			if (((Control)buttonNext).Width != value)
			{
				((Control)buttonNext).Width = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button is auto sized")]
	public bool NextButtonAutoSize
	{
		get
		{
			return ((Control)buttonNext).AutoSize;
		}
		set
		{
			if (((Control)buttonNext).AutoSize != value)
			{
				((Control)buttonNext).AutoSize = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Wizard Buttons")]
	[Description("Indicates button auto-size mode")]
	public AutoSizeMode NextButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return buttonNext.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (buttonNext.AutoSizeMode != value)
			{
				buttonNext.AutoSizeMode = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
	public bool CancelButtonCausesValidation
	{
		get
		{
			return ((Control)buttonCancel).CausesValidation;
		}
		set
		{
			((Control)buttonCancel).CausesValidation = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(4)]
	[Category("Wizard Buttons")]
	[Description("Indicates tab index of the button.")]
	public int CancelButtonTabIndex
	{
		get
		{
			return ((Control)buttonCancel).TabIndex;
		}
		set
		{
			((Control)buttonCancel).TabIndex = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether the user can give the focus to button using the TAB key.")]
	public bool CancelButtonTabStop
	{
		get
		{
			return ((Control)buttonCancel).TabStop;
		}
		set
		{
			((Control)buttonCancel).TabStop = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates caption of the button")]
	public string CancelButtonText
	{
		get
		{
			return ((Control)buttonCancel).Text;
		}
		set
		{
			((Control)buttonCancel).Text = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(74)]
	[Category("Wizard Buttons")]
	[Description("Indicates width of button")]
	public int CancelButtonWidth
	{
		get
		{
			return ((Control)buttonCancel).Width;
		}
		set
		{
			if (((Control)buttonCancel).Width != value)
			{
				((Control)buttonCancel).Width = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button is auto sized")]
	public bool CancelButtonAutoSize
	{
		get
		{
			return ((Control)buttonCancel).AutoSize;
		}
		set
		{
			if (((Control)buttonCancel).AutoSize != value)
			{
				((Control)buttonCancel).AutoSize = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Wizard Buttons")]
	[Description("Indicates button auto-size mode")]
	public AutoSizeMode CancelButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return buttonCancel.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (buttonCancel.AutoSizeMode != value)
			{
				buttonCancel.AutoSizeMode = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
	public bool FinishButtonCausesValidation
	{
		get
		{
			return ((Control)buttonFinish).CausesValidation;
		}
		set
		{
			((Control)buttonFinish).CausesValidation = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(4)]
	[Category("Wizard Buttons")]
	[Description("Indicates tab index of the button.")]
	public int FinishButtonTabIndex
	{
		get
		{
			return ((Control)buttonFinish).TabIndex;
		}
		set
		{
			((Control)buttonFinish).TabIndex = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether the user can give the focus to button using the TAB key.")]
	public bool FinishButtonTabStop
	{
		get
		{
			return ((Control)buttonFinish).TabStop;
		}
		set
		{
			((Control)buttonFinish).TabStop = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue("Finish")]
	[Category("Wizard Buttons")]
	[Description("Indicates caption of the button")]
	public string FinishButtonText
	{
		get
		{
			return ((Control)buttonFinish).Text;
		}
		set
		{
			((Control)buttonFinish).Text = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(74)]
	[Category("Wizard Buttons")]
	[Description("Indicates width of button")]
	public int FinishButtonWidth
	{
		get
		{
			return ((Control)buttonFinish).Width;
		}
		set
		{
			if (((Control)buttonFinish).Width != value)
			{
				((Control)buttonFinish).Width = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether Finish button is always visible or only when needed.")]
	public bool FinishButtonAlwaysVisible
	{
		get
		{
			return m_FinishButtonAlwaysVisible;
		}
		set
		{
			if (m_FinishButtonAlwaysVisible != value)
			{
				m_FinishButtonAlwaysVisible = value;
				OnFinishButtonAlwaysVisibleChanged();
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button is auto sized")]
	public bool FinishButtonAutoSize
	{
		get
		{
			return ((Control)buttonFinish).AutoSize;
		}
		set
		{
			if (((Control)buttonFinish).AutoSize != value)
			{
				((Control)buttonFinish).AutoSize = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Wizard Buttons")]
	[Description("Indicates button auto-size mode")]
	public AutoSizeMode FinishButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return buttonFinish.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (buttonFinish.AutoSizeMode != value)
			{
				buttonFinish.AutoSizeMode = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
	public bool HelpButtonCausesValidation
	{
		get
		{
			return ((Control)buttonHelp).CausesValidation;
		}
		set
		{
			((Control)buttonHelp).CausesValidation = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button is visible")]
	public bool HelpButtonVisible
	{
		get
		{
			return m_HelpButtonVisible;
		}
		set
		{
			m_HelpButtonVisible = value;
			((Control)buttonHelp).Visible = m_HelpButtonVisible;
			RepositionButtons();
		}
	}

	[Browsable(true)]
	[DefaultValue(5)]
	[Category("Wizard Buttons")]
	[Description("Indicates tab index of the button.")]
	public int HelpButtonTabIndex
	{
		get
		{
			return ((Control)buttonHelp).TabIndex;
		}
		set
		{
			((Control)buttonHelp).TabIndex = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether the user can give the focus to button using the TAB key.")]
	public bool HelpButtonTabStop
	{
		get
		{
			return ((Control)buttonHelp).TabStop;
		}
		set
		{
			((Control)buttonHelp).TabStop = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue("Help")]
	[Category("Wizard Buttons")]
	[Description("Indicates caption of the button")]
	public string HelpButtonText
	{
		get
		{
			return ((Control)buttonHelp).Text;
		}
		set
		{
			((Control)buttonHelp).Text = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(74)]
	[Category("Wizard Buttons")]
	[Description("Indicates width of button")]
	public int HelpButtonWidth
	{
		get
		{
			return ((Control)buttonHelp).Width;
		}
		set
		{
			if (((Control)buttonHelp).Width != value)
			{
				((Control)buttonHelp).Width = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button is auto sized")]
	public bool HelpButtonAutoSize
	{
		get
		{
			return ((Control)buttonHelp).AutoSize;
		}
		set
		{
			if (((Control)buttonHelp).AutoSize != value)
			{
				((Control)buttonHelp).AutoSize = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Wizard Buttons")]
	[Description("Indicates button auto-size mode")]
	public AutoSizeMode HelpButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return buttonHelp.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (buttonHelp.AutoSizeMode != value)
			{
				buttonHelp.AutoSizeMode = value;
				RepositionButtons();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(46)]
	[Category("Header and Footer")]
	[Description("Indicates height of the wizard footer.")]
	public int FooterHeight
	{
		get
		{
			return ((Control)panelFooter).Height;
		}
		set
		{
			if (((Control)panelFooter).Height != value)
			{
				((Control)panelFooter).Height = value;
				RepositionButtons();
				SetupPage(SelectedPage);
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(60)]
	[Category("Header and Footer")]
	[Description("Indicates height of the wizard header.")]
	public int HeaderHeight
	{
		get
		{
			return ((Control)panelHeader).Height;
		}
		set
		{
			if (((Control)panelHeader).Height != value)
			{
				((Control)panelHeader).Height = value;
				SetupPage(SelectedPage);
			}
		}
	}

	[DefaultValue(eWizardTitleImageAlignment.Right)]
	[Category("Header and Footer")]
	[Description("Indicates header image alignment.")]
	public eWizardTitleImageAlignment HeaderImageAlignment
	{
		get
		{
			return _HeaderImageAlignment;
		}
		set
		{
			_HeaderImageAlignment = value;
			LayoutHeader();
		}
	}

	[Browsable(true)]
	[Category("Header and Footer")]
	[Description("Indicates header image size for interior wizard pages.")]
	public Size HeaderImageSize
	{
		get
		{
			return ((Control)pictureHeader).Size;
		}
		set
		{
			if (((Control)pictureHeader).Size != value)
			{
				((Control)pictureHeader).Size = value;
				LayoutHeader();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(16)]
	[Category("Header and Footer")]
	[Description("Indicates indentation of header title label.")]
	public int HeaderTitleIndent
	{
		get
		{
			return ((Control)labelCaption).Left;
		}
		set
		{
			if (((Control)labelCaption).Left != value)
			{
				((Control)labelCaption).Left = value;
				LayoutHeader();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(44)]
	[Category("Header and Footer")]
	[Description("Indicates indentation of header description label.")]
	public int HeaderDescriptionIndent
	{
		get
		{
			return ((Control)labelDescription).Left;
		}
		set
		{
			if (((Control)labelDescription).Left != value)
			{
				((Control)labelDescription).Left = value;
				LayoutHeader();
			}
		}
	}

	[Category("Header and Footer")]
	[Description(" Indicates the font used to render header description text.")]
	public Font HeaderDescriptionFont
	{
		get
		{
			return ((Control)labelDescription).Font;
		}
		set
		{
			((Control)labelDescription).Font = value;
			LayoutHeader();
		}
	}

	[Category("Header and Footer")]
	[Description("Indicates the font used to render caption header text.")]
	public Font HeaderCaptionFont
	{
		get
		{
			return ((Control)labelCaption).Font;
		}
		set
		{
			((Control)labelCaption).Font = value;
			LayoutHeader();
		}
	}

	[DefaultValue(true)]
	[Category("Header and Footer")]
	[Description("Indicates whether description text displayed in wizard header is visible.")]
	public bool HeaderDescriptionVisible
	{
		get
		{
			return _HeaderDescriptionVisible;
		}
		set
		{
			_HeaderDescriptionVisible = value;
			((Control)labelDescription).Visible = value;
			LayoutHeader();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets header background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle HeaderStyle => panelHeader.Style;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets footer background style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle FooterStyle => panelFooter.Style;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ButtonX NextButtonControl => buttonNext;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ButtonX BackButtonControl => buttonBack;

	private bool IsRightToLeft => (int)((Control)this).RightToLeft == 1;

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether Focus cues on wizard navigation buttons are enabled.")]
	public bool ButtonFocusCuesEnabled
	{
		get
		{
			return _ButtonFocusCuesEnabled;
		}
		set
		{
			if (_ButtonFocusCuesEnabled != value)
			{
				_ButtonFocusCuesEnabled = value;
				buttonHelp.FocusCuesEnabled = value;
				buttonCancel.FocusCuesEnabled = value;
				buttonFinish.FocusCuesEnabled = value;
				buttonNext.FocusCuesEnabled = value;
				buttonBack.FocusCuesEnabled = value;
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(eWizardStyle.Default)]
	public eWizardStyle ButtonStyle
	{
		get
		{
			return m_WizardStyle;
		}
		set
		{
			if (m_WizardStyle != value)
			{
				m_WizardStyle = value;
				OnWizardStyleChanged();
			}
		}
	}

	[Description("Occurs when Back button is clicked.")]
	public event CancelEventHandler BackButtonClick;

	[Description("Occurs when Next button is clicked.")]
	public event CancelEventHandler NextButtonClick;

	[Description("Occurs when Finish button is clicked.")]
	public event CancelEventHandler FinishButtonClick;

	[Description("Occurs when Cancel button is clicked.")]
	public event CancelEventHandler CancelButtonClick;

	[Description("Occurs when Help button is clicked.")]
	public event CancelEventHandler HelpButtonClick;

	[Description("Occurs before wizard page has changed and gives you opportunity to cancel the change.")]
	public event WizardCancelPageChangeEventHandler WizardPageChanging;

	[Description("Occurs after wizard page has changed.")]
	public event WizardPageChangeEventHandler WizardPageChanged;

	[Description("Occurs when wizard buttons (Back, Next, Finish etc) are positioned and resized.")]
	public event WizardButtonsLayoutEventHandler LayoutWizardButtons;

	public Wizard()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		m_WizardPages.ParentWizard = this;
		((Control)pictureHeader).BackgroundImage = (Image)(object)BarFunctions.LoadBitmap("SystemImages.WizardHeaderImage.png");
		panelFooter.Style.StyleChanged += FooterStyleStyleChanged;
	}

	private void FooterStyleStyleChanged(object sender, EventArgs e)
	{
		if (panelFooter.Style.BackColor == Color.Transparent && panelFooter.Style.BackColor2.IsEmpty && panelFooter.Style.BackColorBlend.Count == 0)
		{
			((Control)panelFooter).BackColor = Color.Transparent;
		}
		else if (((Control)panelFooter).BackColor != SystemColors.Control)
		{
			((Control)panelFooter).BackColor = SystemColors.Control;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		if (BarUtilities.DisposeItemImages && !((Component)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref m_HeaderImage);
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		panelHeader = new PanelControl();
		pictureHeader = new PictureBox();
		labelDescription = new Label();
		labelCaption = new Label();
		panelFooter = new PanelControl();
		buttonHelp = new ButtonX();
		buttonCancel = new ButtonX();
		buttonFinish = new ButtonX();
		buttonNext = new ButtonX();
		buttonBack = new ButtonX();
		((Control)panelHeader).SuspendLayout();
		((Control)panelFooter).SuspendLayout();
		((Control)this).SuspendLayout();
		panelHeader.AntiAlias = false;
		((Control)panelHeader).BackColor = Color.Transparent;
		panelHeader.CanvasColor = SystemColors.Control;
		((Control)panelHeader).Controls.Add((Control)(object)pictureHeader);
		((Control)panelHeader).Controls.Add((Control)(object)labelDescription);
		((Control)panelHeader).Controls.Add((Control)(object)labelCaption);
		((Control)panelHeader).Dock = (DockStyle)1;
		((Control)panelHeader).Location = new Point(0, 0);
		((Control)panelHeader).Name = "panelHeader";
		((Control)panelHeader).Size = new Size(548, 60);
		panelHeader.Style.BackColor = Color.FromArgb(255, 255, 255);
		panelHeader.Style.BackColorGradientAngle = 90;
		panelHeader.Style.BorderBottom = eStyleBorderType.Etched;
		panelHeader.Style.BorderBottomWidth = 1;
		panelHeader.Style.BorderColor = SystemColors.Control;
		panelHeader.Style.BorderLeftWidth = 1;
		panelHeader.Style.BorderRightWidth = 1;
		panelHeader.Style.BorderTopWidth = 1;
		panelHeader.Style.TextAlignment = eStyleTextAlignment.Center;
		panelHeader.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		((Control)panelHeader).TabIndex = 5;
		((Control)pictureHeader).Anchor = (AnchorStyles)9;
		((Control)pictureHeader).Location = new Point(496, 6);
		((Control)pictureHeader).Name = "pictureHeader";
		((Control)pictureHeader).Size = new Size(48, 48);
		pictureHeader.TabIndex = 7;
		pictureHeader.TabStop = false;
		((Control)labelDescription).Anchor = (AnchorStyles)15;
		((Control)labelDescription).Location = new Point(44, 22);
		((Control)labelDescription).Name = "labelDescription";
		((Control)labelDescription).Size = new Size(446, 32);
		((Control)labelDescription).TabIndex = 1;
		((Control)labelCaption).Anchor = (AnchorStyles)13;
		((Control)labelCaption).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelCaption).Location = new Point(16, 5);
		((Control)labelCaption).Name = "labelCaption";
		((Control)labelCaption).Size = new Size(474, 17);
		((Control)labelCaption).TabIndex = 0;
		((Control)panelFooter).BackColor = SystemColors.Control;
		panelFooter.CanvasColor = SystemColors.Control;
		((Control)panelFooter).Controls.Add((Control)(object)buttonHelp);
		((Control)panelFooter).Controls.Add((Control)(object)buttonCancel);
		((Control)panelFooter).Controls.Add((Control)(object)buttonFinish);
		((Control)panelFooter).Controls.Add((Control)(object)buttonNext);
		((Control)panelFooter).Controls.Add((Control)(object)buttonBack);
		((Control)panelFooter).Dock = (DockStyle)2;
		((Control)panelFooter).Location = new Point(0, 329);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(548, 46);
		panelFooter.AntiAlias = false;
		((Control)panelFooter).TabIndex = 6;
		((Control)panelFooter).Resize += panelFooter_Resize;
		((Control)buttonHelp).CausesValidation = false;
		((Control)buttonHelp).Location = new Point(462, 13);
		((Control)buttonHelp).Name = "buttonHelp";
		((Control)buttonHelp).Size = new Size(74, 22);
		((Control)buttonHelp).TabIndex = 5;
		((Control)buttonHelp).Text = "Help";
		((Control)buttonHelp).VisibleChanged += CommandButtonVisibleChanged;
		((Control)buttonHelp).Click += buttonHelp_Click;
		buttonHelp.ThemeAware = true;
		buttonHelp.Style = eDotNetBarStyle.Office2000;
		buttonHelp.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)buttonCancel).CausesValidation = false;
		((Control)buttonCancel).Location = new Point(382, 13);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(74, 22);
		((Control)buttonCancel).TabIndex = 4;
		((Control)buttonCancel).Text = "Cancel";
		((Control)buttonCancel).VisibleChanged += CommandButtonVisibleChanged;
		((Control)buttonCancel).Click += buttonCancel_Click;
		buttonCancel.ThemeAware = true;
		buttonCancel.Style = eDotNetBarStyle.Office2000;
		buttonCancel.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)buttonFinish).Location = new Point(148, 13);
		((Control)buttonFinish).Name = "buttonFinish";
		((Control)buttonFinish).Size = new Size(74, 22);
		((Control)buttonFinish).TabIndex = 3;
		((Control)buttonFinish).Text = "Finish";
		((Control)buttonFinish).VisibleChanged += CommandButtonVisibleChanged;
		((Control)buttonFinish).Click += buttonFinish_Click;
		buttonFinish.ThemeAware = true;
		buttonFinish.Style = eDotNetBarStyle.Office2000;
		buttonFinish.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)buttonNext).Location = new Point(302, 13);
		((Control)buttonNext).Name = "buttonNext";
		((Control)buttonNext).Size = new Size(74, 22);
		((Control)buttonNext).TabIndex = 2;
		((Control)buttonNext).Text = "Next >";
		((Control)buttonNext).VisibleChanged += CommandButtonVisibleChanged;
		((Control)buttonNext).Click += buttonNext_Click;
		buttonNext.ThemeAware = true;
		buttonNext.Style = eDotNetBarStyle.Office2000;
		buttonNext.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)buttonBack).CausesValidation = false;
		((Control)buttonBack).Location = new Point(228, 13);
		((Control)buttonBack).Name = "buttonBack";
		((Control)buttonBack).Size = new Size(74, 22);
		((Control)buttonBack).TabIndex = 1;
		((Control)buttonBack).Text = "< Back";
		((Control)buttonBack).VisibleChanged += CommandButtonVisibleChanged;
		((Control)buttonBack).Click += buttonBack_Click;
		buttonBack.ThemeAware = true;
		buttonBack.Style = eDotNetBarStyle.Office2000;
		buttonBack.ColorTable = eButtonColor.Office2007WithBackground;
		Controls.Add((Control)(object)panelHeader);
		Controls.Add((Control)(object)panelFooter);
		((Control)this).Name = "Wizard";
		((Control)this).Size = new Size(548, 375);
		((Control)panelHeader).ResumeLayout(false);
		((Control)panelFooter).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private bool ShouldSerializeHeaderImageSize()
	{
		if (((Control)pictureHeader).Size.Width == 48)
		{
			return ((Control)pictureHeader).Size.Height != 48;
		}
		return true;
	}

	private void ResetHeaderImageSize()
	{
		TypeDescriptor.GetProperties(this)["HeaderImageSize"].SetValue(this, new Size(48, 48));
	}

	private void ResetHeaderStyle()
	{
		panelHeader.ResetStyle();
	}

	private void ResetFooterStyle()
	{
		panelFooter.ResetStyle();
	}

	public void NavigateBack()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnBackButtonClick(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return;
		}
		WizardPage selectedPage = SelectedPage;
		if (selectedPage != null)
		{
			selectedPage.InvokeBackButtonClick(cancelEventArgs);
			if (cancelEventArgs.Cancel)
			{
				return;
			}
		}
		selectedPage = GetBackPage();
		if (selectedPage != null)
		{
			ShowPage(selectedPage, eWizardPageChangeSource.BackButton);
		}
	}

	public void NavigateNext()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnNextButtonClick(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return;
		}
		WizardPage selectedPage = SelectedPage;
		if (selectedPage != null)
		{
			selectedPage.InvokeNextButtonClick(cancelEventArgs);
			if (cancelEventArgs.Cancel)
			{
				return;
			}
		}
		selectedPage = GetNextPage();
		if (selectedPage != null)
		{
			WizardPage selectedPage2 = SelectedPage;
			if (ShowPage(selectedPage, eWizardPageChangeSource.NextButton) && !((Component)this).DesignMode)
			{
				m_PagesHistory.Push(selectedPage2);
			}
		}
	}

	public void NavigateCancel()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnCancelButtonClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			WizardPage selectedPage = SelectedPage;
			if (selectedPage != null)
			{
				selectedPage.InvokeCancelButtonClick(cancelEventArgs);
				_ = cancelEventArgs.Cancel;
			}
		}
	}

	public void NavigateFinish()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnFinishButtonClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			WizardPage selectedPage = SelectedPage;
			if (selectedPage != null)
			{
				selectedPage.InvokeFinishButtonClick(cancelEventArgs);
				_ = cancelEventArgs.Cancel;
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (((Component)this).DesignMode && WizardPages.Count == 0)
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			Graphics graphics = e.Graphics;
			clientRectangle.Inflate(6, 6);
			TextDrawing.DrawString(graphics, "Right-click control and use context menu commands to create, delete, navigate and re-order wizard pages. You can use Next and Back button to navigate wizard just like in run-time.", ((Control)this).Font, SystemColors.ControlDarkDark, clientRectangle, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak);
		}
		((Control)this).OnPaint(e);
	}

	protected override void OnResize(EventArgs e)
	{
		((UserControl)this).OnResize(e);
		if (((Component)this).DesignMode)
		{
			((Control)this).Invalidate();
		}
	}

	private void LayoutHeader()
	{
		int num = 4;
		int num2 = ((Control)this).Width - ((Control)labelCaption).Left - num;
		int num3 = ((Control)this).Width - ((Control)labelDescription).Left - num;
		if (m_HeaderImageVisible || ((Component)this).DesignMode)
		{
			if (_HeaderImageAlignment == eWizardTitleImageAlignment.Right)
			{
				((Control)pictureHeader).Anchor = (AnchorStyles)9;
				((Control)pictureHeader).Location = new Point(((Control)this).Width - ((Control)pictureHeader).Width - num, (((Control)panelHeader).Height - ((Control)pictureHeader).Height) / 2);
			}
			else
			{
				((Control)pictureHeader).Anchor = (AnchorStyles)5;
				((Control)pictureHeader).Location = new Point(num, (((Control)panelHeader).Height - ((Control)pictureHeader).Height) / 2);
			}
			num2 -= ((Control)pictureHeader).Width + num;
			num3 -= ((Control)pictureHeader).Width + num;
		}
		((Control)labelCaption).Width = num2;
		((Control)labelDescription).Width = num3;
		((Control)labelCaption).Top = 5;
		((Control)labelCaption).Height = ((Control)labelCaption).Font.Height + 4;
		if (_HeaderImageAlignment == eWizardTitleImageAlignment.Right)
		{
			((Control)labelCaption).Left = 16;
		}
		else
		{
			((Control)labelCaption).Left = ((Control)pictureHeader).Bounds.Right + 10;
			if (!_HeaderDescriptionVisible)
			{
				((Control)labelCaption).Top = (((Control)panelHeader).Height - ((Control)labelCaption).Height) / 2;
			}
		}
		((Control)labelDescription).Left = ((Control)labelCaption).Left;
		if (IsRightToLeft)
		{
			((Control)pictureHeader).Left = ((Control)this).Width - ((Control)pictureHeader).Bounds.Right;
			((Control)labelCaption).Left = ((Control)this).Width - ((Control)labelCaption).Bounds.Right;
			((Control)labelDescription).Left = ((Control)this).Width - ((Control)labelDescription).Bounds.Right;
		}
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		RepositionButtons();
		LayoutHeader();
		((ScrollableControl)this).OnRightToLeftChanged(e);
	}

	private void RepositionButtons()
	{
		int num = ((Control)panelFooter).Width - m_ButtonSpacingMajor;
		int y = (((Control)panelFooter).Height - m_ButtonHeight) / 2;
		WizardButtonsLayoutEventArgs wizardButtonsLayoutEventArgs = new WizardButtonsLayoutEventArgs();
		if (((Control)buttonHelp).Visible)
		{
			num -= ((Control)buttonHelp).Width;
			wizardButtonsLayoutEventArgs.HelpButtonBounds = new Rectangle(num, y, ((Control)buttonHelp).Width, m_ButtonHeight);
			num -= m_ButtonSpacingMajor;
		}
		if (((Control)buttonCancel).Visible)
		{
			num -= ((Control)buttonCancel).Width;
			wizardButtonsLayoutEventArgs.CancelButtonBounds = new Rectangle(num, y, ((Control)buttonCancel).Width, m_ButtonHeight);
			num -= m_ButtonSpacingMajor;
		}
		if (((Control)buttonFinish).Visible)
		{
			num -= ((Control)buttonFinish).Width;
			wizardButtonsLayoutEventArgs.FinishButtonBounds = new Rectangle(num, y, ((Control)buttonFinish).Width, m_ButtonHeight);
			num = ((!m_FinishButtonAlwaysVisible) ? (num - m_ButtonSpacingMinor) : (num - m_ButtonSpacingMajor));
		}
		if (((Control)buttonNext).Visible)
		{
			num -= ((Control)buttonNext).Width;
			wizardButtonsLayoutEventArgs.NextButtonBounds = new Rectangle(num, y, ((Control)buttonNext).Width, m_ButtonHeight);
			num -= m_ButtonSpacingMinor;
		}
		if (((Control)buttonBack).Visible)
		{
			num -= ((Control)buttonBack).Width;
			wizardButtonsLayoutEventArgs.BackButtonBounds = new Rectangle(num, y, ((Control)buttonBack).Width, m_ButtonHeight);
			num -= m_ButtonSpacingMinor;
		}
		if (IsRightToLeft)
		{
			int num2 = ((Control)panelFooter).Width - num;
			if (!wizardButtonsLayoutEventArgs.HelpButtonBounds.IsEmpty)
			{
				Rectangle helpButtonBounds = wizardButtonsLayoutEventArgs.HelpButtonBounds;
				wizardButtonsLayoutEventArgs.HelpButtonBounds = new Rectangle(((Control)panelFooter).Width - helpButtonBounds.Right, helpButtonBounds.Y, helpButtonBounds.Width, helpButtonBounds.Height);
			}
			if (!wizardButtonsLayoutEventArgs.CancelButtonBounds.IsEmpty)
			{
				Rectangle cancelButtonBounds = wizardButtonsLayoutEventArgs.CancelButtonBounds;
				wizardButtonsLayoutEventArgs.CancelButtonBounds = new Rectangle(((Control)panelFooter).Width - cancelButtonBounds.Right, cancelButtonBounds.Y, cancelButtonBounds.Width, cancelButtonBounds.Height);
			}
			if (!wizardButtonsLayoutEventArgs.FinishButtonBounds.IsEmpty)
			{
				Rectangle finishButtonBounds = wizardButtonsLayoutEventArgs.FinishButtonBounds;
				wizardButtonsLayoutEventArgs.FinishButtonBounds = new Rectangle(((Control)panelFooter).Width - finishButtonBounds.Right, finishButtonBounds.Y, finishButtonBounds.Width, finishButtonBounds.Height);
			}
			if (!wizardButtonsLayoutEventArgs.NextButtonBounds.IsEmpty)
			{
				Rectangle nextButtonBounds = wizardButtonsLayoutEventArgs.NextButtonBounds;
				wizardButtonsLayoutEventArgs.NextButtonBounds = new Rectangle(((Control)panelFooter).Width - nextButtonBounds.Right, nextButtonBounds.Y, nextButtonBounds.Width, nextButtonBounds.Height);
			}
			if (!wizardButtonsLayoutEventArgs.BackButtonBounds.IsEmpty)
			{
				Rectangle backButtonBounds = wizardButtonsLayoutEventArgs.BackButtonBounds;
				wizardButtonsLayoutEventArgs.BackButtonBounds = new Rectangle(((Control)panelFooter).Width - backButtonBounds.Right, backButtonBounds.Y, backButtonBounds.Width, backButtonBounds.Height);
			}
		}
		OnLayoutWizardButtons(wizardButtonsLayoutEventArgs);
		if (((Control)buttonHelp).Visible)
		{
			((Control)buttonHelp).Bounds = wizardButtonsLayoutEventArgs.HelpButtonBounds;
		}
		if (((Control)buttonCancel).Visible)
		{
			((Control)buttonCancel).Bounds = wizardButtonsLayoutEventArgs.CancelButtonBounds;
		}
		if (((Control)buttonNext).Visible)
		{
			((Control)buttonNext).Bounds = wizardButtonsLayoutEventArgs.NextButtonBounds;
		}
		if (((Control)buttonFinish).Visible)
		{
			((Control)buttonFinish).Bounds = wizardButtonsLayoutEventArgs.FinishButtonBounds;
		}
		if (((Control)buttonBack).Visible)
		{
			((Control)buttonBack).Bounds = wizardButtonsLayoutEventArgs.BackButtonBounds;
		}
	}

	private void OnFinishButtonAlwaysVisibleChanged()
	{
	}

	private void panelFooter_Resize(object sender, EventArgs e)
	{
		RepositionButtons();
	}

	private void CommandButtonVisibleChanged(object sender, EventArgs e)
	{
		RepositionButtons();
	}

	private void buttonBack_Click(object sender, EventArgs e)
	{
		NavigateBack();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public WizardPage GetBackPage()
	{
		WizardPage result = null;
		if (m_PagesHistory.Count > 0)
		{
			result = m_PagesHistory.Pop() as WizardPage;
		}
		else if (SelectedPageIndex > 0)
		{
			result = m_WizardPages[SelectedPageIndex - 1];
		}
		return result;
	}

	private void buttonNext_Click(object sender, EventArgs e)
	{
		NavigateNext();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public WizardPage GetNextPage()
	{
		if (SelectedPageIndex + 1 < m_WizardPages.Count)
		{
			return m_WizardPages[SelectedPageIndex + 1];
		}
		return null;
	}

	private void buttonFinish_Click(object sender, EventArgs e)
	{
		NavigateFinish();
	}

	private void buttonCancel_Click(object sender, EventArgs e)
	{
		NavigateCancel();
	}

	private void buttonHelp_Click(object sender, EventArgs e)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnHelpButtonClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			WizardPage selectedPage = SelectedPage;
			if (selectedPage != null)
			{
				selectedPage.InvokeHelpButtonClick(cancelEventArgs);
				_ = cancelEventArgs.Cancel;
			}
		}
	}

	internal void OnWizardPageAdded(WizardPage page)
	{
		SetupPage(page);
		if (m_WizardPages.IndexOf(page) == m_SelectedPageIndex)
		{
			ShowPage(page, eWizardPageChangeSource.Code);
		}
		else
		{
			page.Visible = false;
		}
		Controls.Add((Control)(object)page);
		SetupButtons(canSetFocus: true);
	}

	internal void OnWizardPageRemoved(WizardPage page)
	{
		Controls.Remove((Control)(object)page);
		if (page.Visible && SelectedPage != null && !SelectedPage.Visible)
		{
			ShowPage(SelectedPage, eWizardPageChangeSource.Code);
		}
	}

	private bool ShowPage(WizardPage page, eWizardPageChangeSource changeSource)
	{
		WizardPage selectedPage = SelectedPage;
		WizardCancelPageChangeEventArgs wizardCancelPageChangeEventArgs = new WizardCancelPageChangeEventArgs(page, selectedPage, changeSource);
		OnWizardPageChanging(wizardCancelPageChangeEventArgs);
		if (wizardCancelPageChangeEventArgs.Cancel || wizardCancelPageChangeEventArgs.NewPage == null)
		{
			return false;
		}
		page = wizardCancelPageChangeEventArgs.NewPage;
		if (page != null)
		{
			page.InvokeBeforePageDisplayed(wizardCancelPageChangeEventArgs);
			if (wizardCancelPageChangeEventArgs.Cancel || wizardCancelPageChangeEventArgs.NewPage == null)
			{
				return false;
			}
			page = wizardCancelPageChangeEventArgs.NewPage;
		}
		if (m_PageChangeDisableButtons)
		{
			((Control)buttonBack).Enabled = false;
			((Control)buttonNext).Enabled = false;
			((Control)buttonFinish).Enabled = false;
			((Control)buttonCancel).Enabled = false;
		}
		if (m_PageChangeWaitCursor)
		{
			((Control)this).Cursor = Cursors.WaitCursor;
		}
		if (selectedPage != null)
		{
			if (selectedPage != page)
			{
				if (((Component)this).DesignMode)
				{
					TypeDescriptor.GetProperties(selectedPage)["Visible"].SetValue(selectedPage, false);
				}
				selectedPage.Visible = false;
				selectedPage.InvokeAfterPageHidden(new WizardPageChangeEventArgs(page, selectedPage, changeSource));
			}
			else
			{
				foreach (WizardPage wizardPage in WizardPages)
				{
					if (wizardPage != page)
					{
						if (((Component)this).DesignMode)
						{
							TypeDescriptor.GetProperties(wizardPage)["Visible"].SetValue(wizardPage, false);
						}
						wizardPage.Visible = false;
					}
				}
			}
		}
		if (page != null)
		{
			if (((Component)this).DesignMode)
			{
				TypeDescriptor.GetProperties(page)["Visible"].SetValue(page, true);
			}
			page.Visible = true;
		}
		m_SelectedPageIndex = m_WizardPages.IndexOf(page);
		SetupButtons(canSetFocus: true);
		wizardCancelPageChangeEventArgs.Cancel = false;
		OnWizardPageChanged(wizardCancelPageChangeEventArgs);
		page?.InvokeAfterPageDisplayed(new WizardPageChangeEventArgs(page, selectedPage, changeSource));
		UpdatePageDisplay();
		if (m_PageChangeWaitCursor)
		{
			((Control)this).Cursor = Cursors.Default;
		}
		return true;
	}

	internal void UpdatePageDisplay()
	{
		WizardPage selectedPage = SelectedPage;
		if (selectedPage == null)
		{
			return;
		}
		if (selectedPage.InteriorPage)
		{
			if (!((Control)panelHeader).Visible)
			{
				((Control)panelHeader).Visible = true;
			}
			if (((Control)pictureHeader).Visible != m_HeaderImageVisible)
			{
				((Control)pictureHeader).Visible = m_HeaderImageVisible;
				LayoutHeader();
			}
		}
		else if (((Control)panelHeader).Visible)
		{
			((Control)panelHeader).Visible = false;
		}
		((Control)labelCaption).Text = selectedPage.PageTitle;
		((Control)labelDescription).Text = selectedPage.PageDescription;
		if (selectedPage.PageHeaderImage != null)
		{
			((Control)pictureHeader).BackgroundImage = selectedPage.PageHeaderImage;
			m_HeaderImageChanged = true;
		}
		else if (m_HeaderImageChanged)
		{
			if (m_HeaderImage != null)
			{
				((Control)pictureHeader).BackgroundImage = m_HeaderImage;
			}
			else
			{
				((Control)pictureHeader).BackgroundImage = (Image)(object)BarFunctions.LoadBitmap("SystemImages.WizardHeaderImage.png");
			}
			m_HeaderImageChanged = false;
		}
		if (selectedPage.FormCaption != "")
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				((Control)val).Text = selectedPage.FormCaption;
			}
		}
	}

	internal void SetupButtons(bool canSetFocus)
	{
		eWizardButtonState eWizardButtonState2 = eWizardButtonState.Auto;
		eWizardButtonState eWizardButtonState3 = eWizardButtonState.Auto;
		eWizardButtonState eWizardButtonState4 = eWizardButtonState.Auto;
		eWizardButtonState eWizardButtonState5 = eWizardButtonState.Auto;
		eWizardButtonState eWizardButtonState6 = eWizardButtonState.Auto;
		eWizardButtonState eWizardButtonState7 = eWizardButtonState.Auto;
		eWizardButtonState eWizardButtonState8 = eWizardButtonState.Auto;
		eWizardButtonState eWizardButtonState9 = eWizardButtonState.Auto;
		eWizardButtonState eWizardButtonState10 = eWizardButtonState.Auto;
		WizardPage selectedPage = SelectedPage;
		if (selectedPage != null)
		{
			eWizardButtonState2 = selectedPage.BackButtonEnabled;
			eWizardButtonState3 = selectedPage.BackButtonVisible;
			eWizardButtonState4 = selectedPage.NextButtonEnabled;
			eWizardButtonState5 = selectedPage.NextButtonVisible;
			eWizardButtonState6 = selectedPage.FinishButtonEnabled;
			eWizardButtonState7 = selectedPage.CancelButtonEnabled;
			eWizardButtonState8 = selectedPage.CancelButtonVisible;
			eWizardButtonState9 = selectedPage.HelpButtonEnabled;
			eWizardButtonState10 = selectedPage.HelpButtonVisible;
		}
		if (!m_HelpButtonVisible)
		{
			eWizardButtonState10 = eWizardButtonState.False;
		}
		if (WizardPages.Count == 0 && ((Component)this).DesignMode)
		{
			eWizardButtonState2 = eWizardButtonState.False;
			eWizardButtonState4 = eWizardButtonState.False;
			eWizardButtonState6 = eWizardButtonState.False;
			eWizardButtonState7 = eWizardButtonState.False;
		}
		if (eWizardButtonState10 == eWizardButtonState.False)
		{
			((Control)buttonHelp).Visible = false;
		}
		else if (!((Control)buttonHelp).Visible)
		{
			((Control)buttonHelp).Visible = true;
		}
		if (eWizardButtonState9 == eWizardButtonState.False)
		{
			((Control)buttonHelp).Enabled = false;
		}
		else if (!((Control)buttonHelp).Enabled)
		{
			((Control)buttonHelp).Enabled = true;
		}
		if (eWizardButtonState8 == eWizardButtonState.False)
		{
			((Control)buttonCancel).Visible = false;
		}
		else if (!((Control)buttonCancel).Visible)
		{
			((Control)buttonCancel).Visible = true;
		}
		if (eWizardButtonState7 == eWizardButtonState.False)
		{
			((Control)buttonCancel).Enabled = false;
		}
		else if (!((Control)buttonCancel).Enabled)
		{
			((Control)buttonCancel).Enabled = true;
		}
		if (m_FinishButtonAlwaysVisible || SelectedPageIndex == m_WizardPages.Count - 1 || eWizardButtonState6 == eWizardButtonState.True)
		{
			if (!((Control)buttonFinish).Visible)
			{
				((Control)buttonFinish).Visible = true;
			}
		}
		else
		{
			((Control)buttonFinish).Visible = false;
		}
		if (eWizardButtonState6 == eWizardButtonState.False || (eWizardButtonState6 == eWizardButtonState.Auto && SelectedPageIndex < m_WizardPages.Count - 1))
		{
			((Control)buttonFinish).Enabled = false;
		}
		else if (!((Control)buttonFinish).Enabled)
		{
			((Control)buttonFinish).Enabled = true;
		}
		switch (eWizardButtonState5)
		{
		case eWizardButtonState.True:
			if (!((Control)buttonNext).Visible)
			{
				((Control)buttonNext).Visible = true;
			}
			break;
		case eWizardButtonState.Auto:
			if (((Control)buttonFinish).Visible && ((Control)buttonFinish).Enabled && !m_FinishButtonAlwaysVisible)
			{
				((Control)buttonNext).Visible = false;
			}
			else if (!((Control)buttonNext).Visible)
			{
				((Control)buttonNext).Visible = true;
			}
			break;
		default:
			if (((Control)buttonNext).Visible)
			{
				((Control)buttonNext).Visible = false;
			}
			break;
		}
		switch (eWizardButtonState4)
		{
		case eWizardButtonState.True:
			if (!((Control)buttonNext).Enabled)
			{
				((Control)buttonNext).Enabled = true;
			}
			break;
		case eWizardButtonState.Auto:
			if ((((Control)buttonFinish).Visible && ((Control)buttonFinish).Enabled) || SelectedPageIndex == m_WizardPages.Count - 1)
			{
				((Control)buttonNext).Enabled = false;
			}
			else if (!((Control)buttonNext).Enabled)
			{
				((Control)buttonNext).Enabled = true;
			}
			break;
		default:
			if (((Control)buttonNext).Enabled)
			{
				((Control)buttonNext).Enabled = false;
			}
			break;
		}
		if (canSetFocus)
		{
			if (((Control)buttonNext).Enabled && ((Control)buttonNext).Visible)
			{
				((Control)buttonNext).Select();
			}
			else if (((Control)buttonFinish).Enabled && ((Control)buttonFinish).Visible)
			{
				((Control)buttonFinish).Select();
			}
		}
		if (eWizardButtonState3 == eWizardButtonState.True || eWizardButtonState3 == eWizardButtonState.Auto)
		{
			if (!((Control)buttonBack).Visible)
			{
				((Control)buttonBack).Visible = true;
			}
		}
		else if (((Control)buttonBack).Visible)
		{
			((Control)buttonBack).Visible = false;
		}
		switch (eWizardButtonState2)
		{
		case eWizardButtonState.True:
			if (!((Control)buttonBack).Enabled)
			{
				((Control)buttonBack).Enabled = true;
			}
			break;
		case eWizardButtonState.Auto:
			if (m_PagesHistory.Count > 0 || SelectedPageIndex > 0)
			{
				if (!((Control)buttonBack).Enabled)
				{
					((Control)buttonBack).Enabled = true;
				}
			}
			else if (((Control)buttonBack).Enabled)
			{
				((Control)buttonBack).Enabled = false;
			}
			break;
		case eWizardButtonState.False:
			if (((Control)buttonBack).Enabled)
			{
				((Control)buttonBack).Enabled = false;
			}
			break;
		}
		SetFormAcceptButton();
		SetFormCancelButton();
		RepositionButtons();
	}

	private void SetFormAcceptButton()
	{
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return;
		}
		if (m_FormAcceptButton == eWizardFormAcceptButton.FinishAndNext)
		{
			if (((Control)buttonFinish).Visible && ((Control)buttonFinish).Enabled)
			{
				if (val.AcceptButton != buttonFinish)
				{
					val.AcceptButton = (IButtonControl)(object)buttonFinish;
				}
			}
			else if (val.AcceptButton != buttonNext)
			{
				val.AcceptButton = (IButtonControl)(object)buttonNext;
			}
		}
		else if (m_FormAcceptButton == eWizardFormAcceptButton.Next)
		{
			if (val.AcceptButton != buttonNext)
			{
				val.AcceptButton = (IButtonControl)(object)buttonNext;
			}
		}
		else if (m_FormAcceptButton == eWizardFormAcceptButton.Finish)
		{
			if (val.AcceptButton != buttonFinish)
			{
				val.AcceptButton = (IButtonControl)(object)buttonFinish;
			}
		}
		else if (m_FormAcceptButton == eWizardFormAcceptButton.None && (val.AcceptButton == buttonFinish || val.AcceptButton == buttonNext))
		{
			val.AcceptButton = null;
		}
	}

	private void SetFormCancelButton()
	{
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return;
		}
		if (m_FormCancelButton == eWizardFormCancelButton.Cancel)
		{
			if (val.CancelButton != buttonCancel)
			{
				val.CancelButton = (IButtonControl)(object)buttonCancel;
			}
		}
		else if (val.CancelButton == buttonCancel)
		{
			val.CancelButton = null;
		}
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((ContainerControl)this).OnParentChanged(e);
		SetFormCancelButton();
		SetFormAcceptButton();
	}

	internal void SetupPage(WizardPage page)
	{
		if (page == null)
		{
			return;
		}
		Size size = new Size(7, 12);
		((Control)this).SuspendLayout();
		try
		{
			Rectangle rectangle = Rectangle.Empty;
			rectangle = ((!page.InteriorPage) ? new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height - ((Control)panelFooter).Height) : new Rectangle(((Control)this).ClientRectangle.X + size.Width, ((Control)this).ClientRectangle.Y + ((Control)panelHeader).Height + size.Height, ((Control)this).ClientRectangle.Width - size.Width * 2, ((Control)this).ClientRectangle.Height - (((Control)panelHeader).Height + ((Control)panelFooter).Height + size.Height * 2)));
			TypeDescriptor.GetProperties(page)["Bounds"].SetValue(page, rectangle);
			TypeDescriptor.GetProperties(page)["Anchor"].SetValue(page, (object)(AnchorStyles)15);
		}
		finally
		{
			((Control)this).ResumeLayout();
		}
	}

	private void OnWizardStyleChanged()
	{
		if (m_WizardStyle == eWizardStyle.Default)
		{
			buttonBack.Style = eDotNetBarStyle.Office2000;
			buttonCancel.Style = eDotNetBarStyle.Office2000;
			buttonFinish.Style = eDotNetBarStyle.Office2000;
			buttonHelp.Style = eDotNetBarStyle.Office2000;
			buttonNext.Style = eDotNetBarStyle.Office2000;
			buttonBack.ThemeAware = true;
			buttonCancel.ThemeAware = true;
			buttonFinish.ThemeAware = true;
			buttonHelp.ThemeAware = true;
			buttonNext.ThemeAware = true;
		}
		else
		{
			buttonBack.Style = eDotNetBarStyle.Office2007;
			buttonCancel.Style = eDotNetBarStyle.Office2007;
			buttonFinish.Style = eDotNetBarStyle.Office2007;
			buttonHelp.Style = eDotNetBarStyle.Office2007;
			buttonNext.Style = eDotNetBarStyle.Office2007;
			buttonBack.ThemeAware = false;
			buttonCancel.ThemeAware = false;
			buttonFinish.ThemeAware = false;
			buttonHelp.ThemeAware = false;
			buttonNext.ThemeAware = false;
		}
	}

	protected virtual void OnBackButtonClick(CancelEventArgs e)
	{
		if (this.BackButtonClick != null)
		{
			this.BackButtonClick(this, e);
		}
	}

	protected virtual void OnNextButtonClick(CancelEventArgs e)
	{
		if (this.NextButtonClick != null)
		{
			this.NextButtonClick(this, e);
		}
	}

	protected virtual void OnFinishButtonClick(CancelEventArgs e)
	{
		if (this.FinishButtonClick != null)
		{
			this.FinishButtonClick(this, e);
		}
	}

	protected virtual void OnCancelButtonClick(CancelEventArgs e)
	{
		if (this.CancelButtonClick != null)
		{
			this.CancelButtonClick(this, e);
		}
	}

	protected virtual void OnHelpButtonClick(CancelEventArgs e)
	{
		if (this.HelpButtonClick != null)
		{
			this.HelpButtonClick(this, e);
		}
	}

	protected virtual void OnWizardPageChanging(WizardCancelPageChangeEventArgs e)
	{
		if (this.WizardPageChanging != null)
		{
			this.WizardPageChanging(this, e);
		}
	}

	protected virtual void OnWizardPageChanged(WizardCancelPageChangeEventArgs e)
	{
		if (this.WizardPageChanged != null)
		{
			this.WizardPageChanged(this, e);
		}
	}

	protected virtual void OnLayoutWizardButtons(WizardButtonsLayoutEventArgs e)
	{
		if (this.LayoutWizardButtons != null)
		{
			this.LayoutWizardButtons(this, e);
		}
	}
}
