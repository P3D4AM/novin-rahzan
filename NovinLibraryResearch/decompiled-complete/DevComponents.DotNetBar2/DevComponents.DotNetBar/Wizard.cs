using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxBitmap(typeof(Wizard), "Wizard.Wizard.ico")]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.WizardDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class Wizard : UserControl
{
	private PanelControl _301C;

	private int _301D = 8;

	private int _301E = 1;

	private int _301F = 22;

	private FlatStyle _3020 = (FlatStyle)3;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public PanelControl panelFooter;

	private ButtonX _3021;

	private ButtonX ẘ;

	private ButtonX _3022;

	private ButtonX _3023;

	private ButtonX _3024;

	private bool _3025;

	private Label _3026;

	private Label _3027;

	private WizardPageCollection _3028 = new WizardPageCollection();

	private Stack _3029 = new Stack();

	private int _302A;

	private bool _302B = true;

	private bool _302C = true;

	private eWizardFormAcceptButton _302D;

	private eWizardFormCancelButton _302E;

	private PictureBox _302F;

	private Image _3030;

	private bool 〱;

	private bool 〲 = true;

	private bool 〳 = true;

	private Container Պ;

	private CancelEventHandler _1BFE;

	private CancelEventHandler 〴;

	private CancelEventHandler 〵;

	private CancelEventHandler _3036;

	private CancelEventHandler Ꮹ;

	private WizardCancelPageChangeEventHandler _3037;

	private WizardPageChangeEventHandler _3038;

	private WizardButtonsLayoutEventHandler _3039;

	private eWizardTitleImageAlignment __303A = eWizardTitleImageAlignment.Right;

	private bool _〻 = true;

	private bool _〼 = true;

	private eWizardStyle _303D;

	[Browsable(true)]
	[Description("Indicates the header image.")]
	[Category("Header and Footer")]
	[DefaultValue(null)]
	public Image HeaderImage
	{
		get
		{
			return _3030;
		}
		set
		{
			_3030 = value;
			〱 = true;
			か();
		}
	}

	[Browsable(true)]
	[Description("Indicates whether header image is visible.")]
	[DefaultValue(true)]
	[Category("Header and Footer")]
	public bool HeaderImageVisible
	{
		get
		{
			return 〲;
		}
		set
		{
			〲 = value;
			if (((Control)_302F).Visible != 〲)
			{
				((Control)_302F).Visible = 〲;
				_303F();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ControlCollection Controls => ((Control)this).Controls;

	[Browsable(true)]
	[Category("Wizard Behavior")]
	[DefaultValue(eWizardFormAcceptButton.FinishAndNext)]
	[Description("Indicates wizard button that is clicked when ENTER key is pressed.")]
	public eWizardFormAcceptButton FormAcceptButton
	{
		get
		{
			return _302D;
		}
		set
		{
			_302D = value;
			ぎ();
		}
	}

	[DefaultValue(eWizardFormCancelButton.Cancel)]
	[Category("Wizard Behavior")]
	[Description("Indicates wizard button that is clicked when ESCAPE key is pressed.")]
	[Browsable(true)]
	public eWizardFormCancelButton FormCancelButton
	{
		get
		{
			return _302E;
		}
		set
		{
			_302E = value;
			く();
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
			return _302B;
		}
		set
		{
			_302B = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether wait cursor is displayed while page is changed.")]
	[Category("Wizard Behavior")]
	public bool PageChangeWaitCursor
	{
		get
		{
			return _302C;
		}
		set
		{
			_302C = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates selected page index.")]
	[Category("Wizard Pages")]
	public int SelectedPageIndex
	{
		get
		{
			return _302A;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value < _3028.Count)
			{
				ぉ(_3028[value], eWizardPageChangeSource.Code);
			}
			else
			{
				_302A = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public WizardPage SelectedPage
	{
		get
		{
			if (_302A < _3028.Count)
			{
				return _3028[_302A];
			}
			return null;
		}
		set
		{
			if (!_3028.Contains(value))
			{
				throw new InvalidOperationException("WizardPage is not member of WizardPages collection. Add page to the WizardPages collection before setting this property");
			}
			ぉ(value, eWizardPageChangeSource.Code);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Wizard Pages")]
	[Browsable(true)]
	public WizardPageCollection WizardPages => _3028;

	[Browsable(false)]
	[Category("Wizard Pages")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Stack PagesHistory => _3029;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete("Property is obsolete and no longer applies")]
	[Description("Indicates flat style for buttons")]
	[Category("Wizard Buttons")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public FlatStyle ButtonFlatStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _3020;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_3020 = value;
		}
	}

	[Browsable(true)]
	[Category("Wizard Buttons")]
	[DefaultValue(22)]
	[Description("Indicates the height of the wizard command buttons")]
	public int ButtonHeight
	{
		get
		{
			return _301F;
		}
		set
		{
			if (_301F != value)
			{
				_301F = value;
				ᦗ();
			}
		}
	}

	[Description("Indicates whether back button causes validation to be performed on any controls that require validation when it receives focus.")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Wizard Buttons")]
	public bool BackButtonCausesValidation
	{
		get
		{
			return ((Control)_3024).CausesValidation;
		}
		set
		{
			((Control)_3024).CausesValidation = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(1)]
	[Description("Indicates tab index of back button.")]
	[Category("Wizard Buttons")]
	public int BackButtonTabIndex
	{
		get
		{
			return ((Control)_3024).TabIndex;
		}
		set
		{
			((Control)_3024).TabIndex = value;
		}
	}

	[Description("Indicates whether the user can give the focus to this back button using the TAB key.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Wizard Buttons")]
	public bool BackButtonTabStop
	{
		get
		{
			return ((Control)_3024).TabStop;
		}
		set
		{
			((Control)_3024).TabStop = value;
		}
	}

	[DefaultValue("< Back")]
	[Browsable(true)]
	[Localizable(true)]
	[Description("Indicates caption of the button")]
	[Category("Wizard Buttons")]
	public string BackButtonText
	{
		get
		{
			return ((Control)_3024).Text;
		}
		set
		{
			((Control)_3024).Text = value;
		}
	}

	[Category("Wizard Buttons")]
	[Description("Indicates width of button")]
	[DefaultValue(74)]
	[Browsable(true)]
	public int BackButtonWidth
	{
		get
		{
			return ((Control)_3024).Width;
		}
		set
		{
			if (((Control)_3024).Width != value)
			{
				((Control)_3024).Width = value;
				ᦗ();
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether button is auto sized")]
	[Browsable(true)]
	[Category("Wizard Buttons")]
	public bool BackButtonAutoSize
	{
		get
		{
			return ((Control)_3024).AutoSize;
		}
		set
		{
			if (((Control)_3024).AutoSize != value)
			{
				((Control)_3024).AutoSize = value;
				ᦗ();
			}
		}
	}

	[Category("Wizard Buttons")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates button auto-size mode")]
	[Browsable(true)]
	public AutoSizeMode BackButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _3024.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (_3024.AutoSizeMode != value)
			{
				_3024.AutoSizeMode = value;
				ᦗ();
			}
		}
	}

	[Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Wizard Buttons")]
	public bool NextButtonCausesValidation
	{
		get
		{
			return ((Control)_3023).CausesValidation;
		}
		set
		{
			((Control)_3023).CausesValidation = value;
		}
	}

	[Browsable(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates tab index of next button.")]
	[DefaultValue(2)]
	public int NextButtonTabIndex
	{
		get
		{
			return ((Control)_3023).TabIndex;
		}
		set
		{
			((Control)_3023).TabIndex = value;
		}
	}

	[Browsable(true)]
	[Category("Wizard Buttons")]
	[DefaultValue(true)]
	[Description("Indicates whether the user can give the focus to button using the TAB key.")]
	public bool NextButtonTabStop
	{
		get
		{
			return ((Control)_3023).TabStop;
		}
		set
		{
			((Control)_3023).TabStop = value;
		}
	}

	[DefaultValue("Next >")]
	[Description("Indicates caption of the button")]
	[Category("Wizard Buttons")]
	[Browsable(true)]
	[Localizable(true)]
	public string NextButtonText
	{
		get
		{
			return ((Control)_3023).Text;
		}
		set
		{
			((Control)_3023).Text = value;
		}
	}

	[Description("Indicates width of button")]
	[DefaultValue(74)]
	[Browsable(true)]
	[Category("Wizard Buttons")]
	public int NextButtonWidth
	{
		get
		{
			return ((Control)_3023).Width;
		}
		set
		{
			if (((Control)_3023).Width != value)
			{
				((Control)_3023).Width = value;
				ᦗ();
			}
		}
	}

	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether button is auto sized")]
	[Browsable(true)]
	public bool NextButtonAutoSize
	{
		get
		{
			return ((Control)_3023).AutoSize;
		}
		set
		{
			if (((Control)_3023).AutoSize != value)
			{
				((Control)_3023).AutoSize = value;
				ᦗ();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Buttons")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates button auto-size mode")]
	public AutoSizeMode NextButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _3023.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (_3023.AutoSizeMode != value)
			{
				_3023.AutoSizeMode = value;
				ᦗ();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
	[Category("Wizard Buttons")]
	public bool CancelButtonCausesValidation
	{
		get
		{
			return ((Control)ẘ).CausesValidation;
		}
		set
		{
			((Control)ẘ).CausesValidation = value;
		}
	}

	[Description("Indicates tab index of the button.")]
	[Browsable(true)]
	[DefaultValue(4)]
	[Category("Wizard Buttons")]
	public int CancelButtonTabIndex
	{
		get
		{
			return ((Control)ẘ).TabIndex;
		}
		set
		{
			((Control)ẘ).TabIndex = value;
		}
	}

	[Category("Wizard Buttons")]
	[Browsable(true)]
	[Description("Indicates whether the user can give the focus to button using the TAB key.")]
	[DefaultValue(true)]
	public bool CancelButtonTabStop
	{
		get
		{
			return ((Control)ẘ).TabStop;
		}
		set
		{
			((Control)ẘ).TabStop = value;
		}
	}

	[Category("Wizard Buttons")]
	[Description("Indicates caption of the button")]
	[Browsable(true)]
	[Localizable(true)]
	public string CancelButtonText
	{
		get
		{
			return ((Control)ẘ).Text;
		}
		set
		{
			((Control)ẘ).Text = value;
		}
	}

	[Category("Wizard Buttons")]
	[Browsable(true)]
	[Description("Indicates width of button")]
	[DefaultValue(74)]
	public int CancelButtonWidth
	{
		get
		{
			return ((Control)ẘ).Width;
		}
		set
		{
			if (((Control)ẘ).Width != value)
			{
				((Control)ẘ).Width = value;
				ᦗ();
			}
		}
	}

	[Category("Wizard Buttons")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether button is auto sized")]
	public bool CancelButtonAutoSize
	{
		get
		{
			return ((Control)ẘ).AutoSize;
		}
		set
		{
			if (((Control)ẘ).AutoSize != value)
			{
				((Control)ẘ).AutoSize = value;
				ᦗ();
			}
		}
	}

	[Description("Indicates button auto-size mode")]
	[Browsable(true)]
	[Category("Wizard Buttons")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public AutoSizeMode CancelButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ẘ.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (ẘ.AutoSizeMode != value)
			{
				ẘ.AutoSizeMode = value;
				ᦗ();
			}
		}
	}

	[Category("Wizard Buttons")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
	public bool FinishButtonCausesValidation
	{
		get
		{
			return ((Control)_3022).CausesValidation;
		}
		set
		{
			((Control)_3022).CausesValidation = value;
		}
	}

	[DefaultValue(4)]
	[Browsable(true)]
	[Description("Indicates tab index of the button.")]
	[Category("Wizard Buttons")]
	public int FinishButtonTabIndex
	{
		get
		{
			return ((Control)_3022).TabIndex;
		}
		set
		{
			((Control)_3022).TabIndex = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates whether the user can give the focus to button using the TAB key.")]
	[Category("Wizard Buttons")]
	[DefaultValue(true)]
	public bool FinishButtonTabStop
	{
		get
		{
			return ((Control)_3022).TabStop;
		}
		set
		{
			((Control)_3022).TabStop = value;
		}
	}

	[Localizable(true)]
	[Category("Wizard Buttons")]
	[DefaultValue("Finish")]
	[Description("Indicates caption of the button")]
	[Browsable(true)]
	public string FinishButtonText
	{
		get
		{
			return ((Control)_3022).Text;
		}
		set
		{
			((Control)_3022).Text = value;
		}
	}

	[Browsable(true)]
	[Category("Wizard Buttons")]
	[DefaultValue(74)]
	[Description("Indicates width of button")]
	public int FinishButtonWidth
	{
		get
		{
			return ((Control)_3022).Width;
		}
		set
		{
			if (((Control)_3022).Width != value)
			{
				((Control)_3022).Width = value;
				ᦗ();
			}
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Wizard Buttons")]
	[Description("Indicates whether Finish button is always visible or only when needed.")]
	public bool FinishButtonAlwaysVisible
	{
		get
		{
			return _3025;
		}
		set
		{
			if (_3025 != value)
			{
				_3025 = value;
				_3040();
				ᦗ();
			}
		}
	}

	[Description("Indicates whether button is auto sized")]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	[Browsable(true)]
	public bool FinishButtonAutoSize
	{
		get
		{
			return ((Control)_3022).AutoSize;
		}
		set
		{
			if (((Control)_3022).AutoSize != value)
			{
				((Control)_3022).AutoSize = value;
				ᦗ();
			}
		}
	}

	[Description("Indicates button auto-size mode")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Wizard Buttons")]
	[Browsable(true)]
	public AutoSizeMode FinishButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _3022.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (_3022.AutoSizeMode != value)
			{
				_3022.AutoSizeMode = value;
				ᦗ();
			}
		}
	}

	[Description("Indicates whether button causes validation to be performed on any controls that require validation when it receives focus.")]
	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Wizard Buttons")]
	public bool HelpButtonCausesValidation
	{
		get
		{
			return ((Control)_3021).CausesValidation;
		}
		set
		{
			((Control)_3021).CausesValidation = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether button is visible")]
	[Browsable(true)]
	[Category("Wizard Buttons")]
	public bool HelpButtonVisible
	{
		get
		{
			return 〳;
		}
		set
		{
			〳 = value;
			((Control)_3021).Visible = 〳;
			ᦗ();
		}
	}

	[Browsable(true)]
	[Description("Indicates tab index of the button.")]
	[Category("Wizard Buttons")]
	[DefaultValue(5)]
	public int HelpButtonTabIndex
	{
		get
		{
			return ((Control)_3021).TabIndex;
		}
		set
		{
			((Control)_3021).TabIndex = value;
		}
	}

	[Browsable(true)]
	[Category("Wizard Buttons")]
	[DefaultValue(true)]
	[Description("Indicates whether the user can give the focus to button using the TAB key.")]
	public bool HelpButtonTabStop
	{
		get
		{
			return ((Control)_3021).TabStop;
		}
		set
		{
			((Control)_3021).TabStop = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[Description("Indicates caption of the button")]
	[DefaultValue("Help")]
	[Category("Wizard Buttons")]
	public string HelpButtonText
	{
		get
		{
			return ((Control)_3021).Text;
		}
		set
		{
			((Control)_3021).Text = value;
		}
	}

	[DefaultValue(74)]
	[Browsable(true)]
	[Description("Indicates width of button")]
	[Category("Wizard Buttons")]
	public int HelpButtonWidth
	{
		get
		{
			return ((Control)_3021).Width;
		}
		set
		{
			if (((Control)_3021).Width != value)
			{
				((Control)_3021).Width = value;
				ᦗ();
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
			return ((Control)_3021).AutoSize;
		}
		set
		{
			if (((Control)_3021).AutoSize != value)
			{
				((Control)_3021).AutoSize = value;
				ᦗ();
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Wizard Buttons")]
	[Browsable(true)]
	[Description("Indicates button auto-size mode")]
	public AutoSizeMode HelpButtonAutoSizeMode
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return _3021.AutoSizeMode;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (_3021.AutoSizeMode != value)
			{
				_3021.AutoSizeMode = value;
				ᦗ();
			}
		}
	}

	[DefaultValue(46)]
	[Category("Header and Footer")]
	[Browsable(true)]
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
				ᦗ();
				ぐ(SelectedPage);
			}
		}
	}

	[DefaultValue(60)]
	[Description("Indicates height of the wizard header.")]
	[Browsable(true)]
	[Category("Header and Footer")]
	public int HeaderHeight
	{
		get
		{
			return ((Control)_301C).Height;
		}
		set
		{
			if (((Control)_301C).Height != value)
			{
				((Control)_301C).Height = value;
				ぐ(SelectedPage);
			}
		}
	}

	[Description("Indicates header image alignment.")]
	[DefaultValue(eWizardTitleImageAlignment.Right)]
	[Category("Header and Footer")]
	public eWizardTitleImageAlignment HeaderImageAlignment
	{
		get
		{
			return __303A;
		}
		set
		{
			__303A = value;
			_303F();
		}
	}

	[Description("Indicates header image size for interior wizard pages.")]
	[Category("Header and Footer")]
	[Browsable(true)]
	public Size HeaderImageSize
	{
		get
		{
			return ((Control)_302F).Size;
		}
		set
		{
			if (((Control)_302F).Size != value)
			{
				((Control)_302F).Size = value;
				_303F();
			}
		}
	}

	[DefaultValue(16)]
	[Description("Indicates indentation of header title label.")]
	[Category("Header and Footer")]
	[Browsable(true)]
	public int HeaderTitleIndent
	{
		get
		{
			return ((Control)_3027).Left;
		}
		set
		{
			if (((Control)_3027).Left != value)
			{
				((Control)_3027).Left = value;
				_303F();
			}
		}
	}

	[Browsable(true)]
	[Description("Indicates indentation of header description label.")]
	[DefaultValue(44)]
	[Category("Header and Footer")]
	public int HeaderDescriptionIndent
	{
		get
		{
			return ((Control)_3026).Left;
		}
		set
		{
			if (((Control)_3026).Left != value)
			{
				((Control)_3026).Left = value;
				_303F();
			}
		}
	}

	[Category("Header and Footer")]
	[Description(" Indicates the font used to render header description text.")]
	public Font HeaderDescriptionFont
	{
		get
		{
			return ((Control)_3026).Font;
		}
		set
		{
			((Control)_3026).Font = value;
			_303F();
		}
	}

	[Description("Indicates the font used to render caption header text.")]
	[Category("Header and Footer")]
	public Font HeaderCaptionFont
	{
		get
		{
			return ((Control)_3027).Font;
		}
		set
		{
			((Control)_3027).Font = value;
			_303F();
		}
	}

	[Description("Indicates whether description text displayed in wizard header is visible.")]
	[DefaultValue(true)]
	[Category("Header and Footer")]
	public bool HeaderDescriptionVisible
	{
		get
		{
			return _〻;
		}
		set
		{
			_〻 = value;
			((Control)_3026).Visible = value;
			_303F();
		}
	}

	[Category("Style")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets header background style.")]
	[Browsable(true)]
	[NotifyParentProperty(true)]
	[DevCoBrowsable(true)]
	public ElementStyle HeaderStyle => _301C.Style;

	[Category("Style")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets footer background style.")]
	[NotifyParentProperty(true)]
	[DevCoBrowsable(true)]
	public ElementStyle FooterStyle => panelFooter.Style;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ButtonX NextButtonControl => _3023;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public ButtonX CancelButtonControl => ẘ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public ButtonX FinishButtonControl => _3022;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ButtonX HelpButtonControl => _3021;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public ButtonX BackButtonControl => _3024;

	private bool ᓛ => (int)((Control)this).RightToLeft == 1;

	[Description("Indicates whether Focus cues on wizard navigation buttons are enabled.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ButtonFocusCuesEnabled
	{
		get
		{
			return _〼;
		}
		set
		{
			if (_〼 != value)
			{
				_〼 = value;
				_3021.FocusCuesEnabled = value;
				ẘ.FocusCuesEnabled = value;
				_3022.FocusCuesEnabled = value;
				_3023.FocusCuesEnabled = value;
				_3024.FocusCuesEnabled = value;
			}
		}
	}

	[DefaultValue(eWizardStyle.Default)]
	[Browsable(false)]
	public eWizardStyle ButtonStyle
	{
		get
		{
			return _303D;
		}
		set
		{
			if (_303D != value)
			{
				_303D = value;
				け();
			}
		}
	}

	[Description("Occurs when Back button is clicked.")]
	public event CancelEventHandler BackButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _1BFE;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _1BFE, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _1BFE;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _1BFE, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when Next button is clicked.")]
	public event CancelEventHandler NextButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = 〴;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref 〴, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = 〴;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref 〴, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when Finish button is clicked.")]
	public event CancelEventHandler FinishButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = 〵;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref 〵, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = 〵;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref 〵, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when Cancel button is clicked.")]
	public event CancelEventHandler CancelButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = _3036;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _3036, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _3036;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _3036, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs when Help button is clicked.")]
	public event CancelEventHandler HelpButtonClick
	{
		add
		{
			CancelEventHandler cancelEventHandler = Ꮹ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref Ꮹ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = Ꮹ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref Ꮹ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs before wizard page has changed and gives you opportunity to cancel the change.")]
	public event WizardCancelPageChangeEventHandler WizardPageChanging
	{
		add
		{
			WizardCancelPageChangeEventHandler wizardCancelPageChangeEventHandler = _3037;
			WizardCancelPageChangeEventHandler wizardCancelPageChangeEventHandler2;
			do
			{
				wizardCancelPageChangeEventHandler2 = wizardCancelPageChangeEventHandler;
				WizardCancelPageChangeEventHandler value2 = (WizardCancelPageChangeEventHandler)Delegate.Combine(wizardCancelPageChangeEventHandler2, value);
				wizardCancelPageChangeEventHandler = Interlocked.CompareExchange(ref _3037, value2, wizardCancelPageChangeEventHandler2);
			}
			while ((object)wizardCancelPageChangeEventHandler != wizardCancelPageChangeEventHandler2);
		}
		remove
		{
			WizardCancelPageChangeEventHandler wizardCancelPageChangeEventHandler = _3037;
			WizardCancelPageChangeEventHandler wizardCancelPageChangeEventHandler2;
			do
			{
				wizardCancelPageChangeEventHandler2 = wizardCancelPageChangeEventHandler;
				WizardCancelPageChangeEventHandler value2 = (WizardCancelPageChangeEventHandler)Delegate.Remove(wizardCancelPageChangeEventHandler2, value);
				wizardCancelPageChangeEventHandler = Interlocked.CompareExchange(ref _3037, value2, wizardCancelPageChangeEventHandler2);
			}
			while ((object)wizardCancelPageChangeEventHandler != wizardCancelPageChangeEventHandler2);
		}
	}

	[Description("Occurs after wizard page has changed.")]
	public event WizardPageChangeEventHandler WizardPageChanged
	{
		add
		{
			WizardPageChangeEventHandler wizardPageChangeEventHandler = _3038;
			WizardPageChangeEventHandler wizardPageChangeEventHandler2;
			do
			{
				wizardPageChangeEventHandler2 = wizardPageChangeEventHandler;
				WizardPageChangeEventHandler value2 = (WizardPageChangeEventHandler)Delegate.Combine(wizardPageChangeEventHandler2, value);
				wizardPageChangeEventHandler = Interlocked.CompareExchange(ref _3038, value2, wizardPageChangeEventHandler2);
			}
			while ((object)wizardPageChangeEventHandler != wizardPageChangeEventHandler2);
		}
		remove
		{
			WizardPageChangeEventHandler wizardPageChangeEventHandler = _3038;
			WizardPageChangeEventHandler wizardPageChangeEventHandler2;
			do
			{
				wizardPageChangeEventHandler2 = wizardPageChangeEventHandler;
				WizardPageChangeEventHandler value2 = (WizardPageChangeEventHandler)Delegate.Remove(wizardPageChangeEventHandler2, value);
				wizardPageChangeEventHandler = Interlocked.CompareExchange(ref _3038, value2, wizardPageChangeEventHandler2);
			}
			while ((object)wizardPageChangeEventHandler != wizardPageChangeEventHandler2);
		}
	}

	[Description("Occurs when wizard buttons (Back, Next, Finish etc) are positioned and resized.")]
	public event WizardButtonsLayoutEventHandler LayoutWizardButtons
	{
		add
		{
			WizardButtonsLayoutEventHandler wizardButtonsLayoutEventHandler = _3039;
			WizardButtonsLayoutEventHandler wizardButtonsLayoutEventHandler2;
			do
			{
				wizardButtonsLayoutEventHandler2 = wizardButtonsLayoutEventHandler;
				WizardButtonsLayoutEventHandler value2 = (WizardButtonsLayoutEventHandler)Delegate.Combine(wizardButtonsLayoutEventHandler2, value);
				wizardButtonsLayoutEventHandler = Interlocked.CompareExchange(ref _3039, value2, wizardButtonsLayoutEventHandler2);
			}
			while ((object)wizardButtonsLayoutEventHandler != wizardButtonsLayoutEventHandler2);
		}
		remove
		{
			WizardButtonsLayoutEventHandler wizardButtonsLayoutEventHandler = _3039;
			WizardButtonsLayoutEventHandler wizardButtonsLayoutEventHandler2;
			do
			{
				wizardButtonsLayoutEventHandler2 = wizardButtonsLayoutEventHandler;
				WizardButtonsLayoutEventHandler value2 = (WizardButtonsLayoutEventHandler)Delegate.Remove(wizardButtonsLayoutEventHandler2, value);
				wizardButtonsLayoutEventHandler = Interlocked.CompareExchange(ref _3039, value2, wizardButtonsLayoutEventHandler2);
			}
			while ((object)wizardButtonsLayoutEventHandler != wizardButtonsLayoutEventHandler2);
		}
	}

	public Wizard()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		ـ();
		_3028.に = this;
		((Control)_302F).BackgroundImage = (Image)(object)BarFunctions.ই("SystemImages.WizardHeaderImage.png");
		panelFooter.Style.StyleChanged += _303E;
	}

	private void _303E(object _٠, EventArgs ؿ)
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
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		if (BarUtilities.DisposeItemImages && !((Component)this).DesignMode)
		{
			BarUtilities.ᆟ(ref _3030);
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void ـ()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		_301C = new PanelControl();
		_302F = new PictureBox();
		_3026 = new Label();
		_3027 = new Label();
		panelFooter = new PanelControl();
		_3021 = new ButtonX();
		ẘ = new ButtonX();
		_3022 = new ButtonX();
		_3023 = new ButtonX();
		_3024 = new ButtonX();
		((Control)_301C).SuspendLayout();
		((Control)panelFooter).SuspendLayout();
		((Control)this).SuspendLayout();
		_301C.AntiAlias = false;
		((Control)_301C).BackColor = Color.Transparent;
		_301C.CanvasColor = SystemColors.Control;
		((Control)_301C).Controls.Add((Control)(object)_302F);
		((Control)_301C).Controls.Add((Control)(object)_3026);
		((Control)_301C).Controls.Add((Control)(object)_3027);
		((Control)_301C).Dock = (DockStyle)1;
		((Control)_301C).Location = new Point(0, 0);
		((Control)_301C).Name = "panelHeader";
		((Control)_301C).Size = new Size(548, 60);
		_301C.Style.BackColor = Color.FromArgb(255, 255, 255);
		_301C.Style.BackColorGradientAngle = 90;
		_301C.Style.BorderBottom = eStyleBorderType.Etched;
		_301C.Style.BorderBottomWidth = 1;
		_301C.Style.BorderColor = SystemColors.Control;
		_301C.Style.BorderLeftWidth = 1;
		_301C.Style.BorderRightWidth = 1;
		_301C.Style.BorderTopWidth = 1;
		_301C.Style.TextAlignment = eStyleTextAlignment.Center;
		_301C.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		((Control)_301C).TabIndex = 5;
		((Control)_302F).Anchor = (AnchorStyles)9;
		((Control)_302F).Location = new Point(496, 6);
		((Control)_302F).Name = "pictureHeader";
		((Control)_302F).Size = new Size(48, 48);
		_302F.TabIndex = 7;
		_302F.TabStop = false;
		((Control)_302F).BackgroundImageLayout = (ImageLayout)0;
		((Control)_3026).Anchor = (AnchorStyles)15;
		((Control)_3026).Location = new Point(44, 22);
		((Control)_3026).Name = "labelDescription";
		((Control)_3026).Size = new Size(446, 32);
		((Control)_3026).TabIndex = 1;
		((Control)_3027).Anchor = (AnchorStyles)13;
		((Control)_3027).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_3027).Location = new Point(16, 5);
		((Control)_3027).Name = "labelCaption";
		((Control)_3027).Size = new Size(474, 17);
		((Control)_3027).TabIndex = 0;
		((Control)panelFooter).BackColor = SystemColors.Control;
		panelFooter.CanvasColor = SystemColors.Control;
		((Control)panelFooter).Controls.Add((Control)(object)_3021);
		((Control)panelFooter).Controls.Add((Control)(object)ẘ);
		((Control)panelFooter).Controls.Add((Control)(object)_3022);
		((Control)panelFooter).Controls.Add((Control)(object)_3023);
		((Control)panelFooter).Controls.Add((Control)(object)_3024);
		((Control)panelFooter).Dock = (DockStyle)2;
		((Control)panelFooter).Location = new Point(0, 329);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(548, 46);
		panelFooter.AntiAlias = false;
		((Control)panelFooter).TabIndex = 6;
		((Control)panelFooter).Resize += ぁ;
		((Control)_3021).CausesValidation = false;
		((Control)_3021).Location = new Point(462, 13);
		((Control)_3021).Name = "buttonHelp";
		((Control)_3021).Size = new Size(74, 22);
		((Control)_3021).TabIndex = 5;
		((Control)_3021).Text = "Help";
		((Control)_3021).VisibleChanged += あ;
		((Control)_3021).Click += う;
		_3021.ThemeAware = true;
		_3021.Style = eDotNetBarStyle.Office2000;
		_3021.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)ẘ).CausesValidation = false;
		((Control)ẘ).Location = new Point(382, 13);
		((Control)ẘ).Name = "buttonCancel";
		((Control)ẘ).Size = new Size(74, 22);
		((Control)ẘ).TabIndex = 4;
		((Control)ẘ).Text = "Cancel";
		((Control)ẘ).VisibleChanged += あ;
		((Control)ẘ).Click += _2F23;
		ẘ.ThemeAware = true;
		ẘ.Style = eDotNetBarStyle.Office2000;
		ẘ.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)_3022).Location = new Point(148, 13);
		((Control)_3022).Name = "buttonFinish";
		((Control)_3022).Size = new Size(74, 22);
		((Control)_3022).TabIndex = 3;
		((Control)_3022).Text = "Finish";
		((Control)_3022).VisibleChanged += あ;
		((Control)_3022).Click += ぅ;
		_3022.ThemeAware = true;
		_3022.Style = eDotNetBarStyle.Office2000;
		_3022.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)_3023).Location = new Point(302, 13);
		((Control)_3023).Name = "buttonNext";
		((Control)_3023).Size = new Size(74, 22);
		((Control)_3023).TabIndex = 2;
		((Control)_3023).Text = "Next >";
		((Control)_3023).VisibleChanged += あ;
		((Control)_3023).Click += い;
		_3023.ThemeAware = true;
		_3023.Style = eDotNetBarStyle.Office2000;
		_3023.ColorTable = eButtonColor.Office2007WithBackground;
		((Control)_3024).CausesValidation = false;
		((Control)_3024).Location = new Point(228, 13);
		((Control)_3024).Name = "buttonBack";
		((Control)_3024).Size = new Size(74, 22);
		((Control)_3024).TabIndex = 1;
		((Control)_3024).Text = "< Back";
		((Control)_3024).VisibleChanged += あ;
		((Control)_3024).Click += ぃ;
		_3024.ThemeAware = true;
		_3024.Style = eDotNetBarStyle.Office2000;
		_3024.ColorTable = eButtonColor.Office2007WithBackground;
		Controls.Add((Control)(object)_301C);
		Controls.Add((Control)(object)panelFooter);
		((Control)this).Name = "Wizard";
		((Control)this).Size = new Size(548, 375);
		((Control)_301C).ResumeLayout(false);
		((Control)panelFooter).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private bool ShouldSerializeHeaderImageSize()
	{
		if (((Control)_302F).Size.Width == 48)
		{
			return ((Control)_302F).Size.Height != 48;
		}
		return true;
	}

	private void ResetHeaderImageSize()
	{
		TypeDescriptor.GetProperties(this)["HeaderImageSize"].SetValue(this, new Size(48, 48));
	}

	private void ResetHeaderStyle()
	{
		_301C.ResetStyle();
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
			selectedPage.で(cancelEventArgs);
			if (cancelEventArgs.Cancel)
			{
				return;
			}
		}
		selectedPage = GetBackPage();
		if (selectedPage != null)
		{
			ぉ(selectedPage, eWizardPageChangeSource.BackButton);
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
			selectedPage.と(cancelEventArgs);
			if (cancelEventArgs.Cancel)
			{
				return;
			}
		}
		selectedPage = GetNextPage();
		if (selectedPage != null)
		{
			WizardPage selectedPage2 = SelectedPage;
			if (ぉ(selectedPage, eWizardPageChangeSource.NextButton) && !((Component)this).DesignMode)
			{
				_3029.Push(selectedPage2);
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
				selectedPage.な(cancelEventArgs);
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
				selectedPage.ど(cancelEventArgs);
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

	private void _303F()
	{
		int num = 4;
		int num2 = ((Control)this).Width - ((Control)_3027).Left - num;
		int num3 = ((Control)this).Width - ((Control)_3026).Left - num;
		if (〲 || ((Component)this).DesignMode)
		{
			if (__303A == eWizardTitleImageAlignment.Right)
			{
				((Control)_302F).Anchor = (AnchorStyles)9;
				((Control)_302F).Location = new Point(((Control)this).Width - ((Control)_302F).Width - num, (((Control)_301C).Height - ((Control)_302F).Height) / 2);
			}
			else
			{
				((Control)_302F).Anchor = (AnchorStyles)5;
				((Control)_302F).Location = new Point(num, (((Control)_301C).Height - ((Control)_302F).Height) / 2);
			}
			num2 -= ((Control)_302F).Width + num;
			num3 -= ((Control)_302F).Width + num;
		}
		((Control)_3027).Width = num2;
		((Control)_3026).Width = num3;
		((Control)_3027).Top = 5;
		((Control)_3027).Height = ((Control)_3027).Font.Height + 4;
		if (__303A == eWizardTitleImageAlignment.Right)
		{
			((Control)_3027).Left = 16;
		}
		else
		{
			((Control)_3027).Left = ((Control)_302F).Bounds.Right + 10;
			if (!_〻)
			{
				((Control)_3027).Top = (((Control)_301C).Height - ((Control)_3027).Height) / 2;
			}
		}
		((Control)_3026).Left = ((Control)_3027).Left;
		if (ᓛ)
		{
			((Control)_302F).Left = ((Control)this).Width - ((Control)_302F).Bounds.Right;
			((Control)_3027).Left = ((Control)this).Width - ((Control)_3027).Bounds.Right;
			((Control)_3026).Left = ((Control)this).Width - ((Control)_3026).Bounds.Right;
		}
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		ᦗ();
		_303F();
		((ScrollableControl)this).OnRightToLeftChanged(e);
	}

	private void ᦗ()
	{
		int num = Dpi.Width(_301D);
		int num2 = ((Control)panelFooter).Width - num;
		int num3 = Dpi.Height(_301F);
		int y = (((Control)panelFooter).Height - num3) / 2;
		WizardButtonsLayoutEventArgs wizardButtonsLayoutEventArgs = new WizardButtonsLayoutEventArgs();
		if (((Control)_3021).Visible)
		{
			num2 -= ((Control)_3021).Width;
			wizardButtonsLayoutEventArgs.HelpButtonBounds = new Rectangle(num2, y, ((Control)_3021).Width, num3);
			num2 -= num;
		}
		if (((Control)ẘ).Visible)
		{
			num2 -= ((Control)ẘ).Width;
			wizardButtonsLayoutEventArgs.CancelButtonBounds = new Rectangle(num2, y, ((Control)ẘ).Width, num3);
			num2 -= num;
		}
		if (((Control)_3022).Visible)
		{
			num2 -= ((Control)_3022).Width;
			wizardButtonsLayoutEventArgs.FinishButtonBounds = new Rectangle(num2, y, ((Control)_3022).Width, num3);
			num2 = ((!_3025) ? (num2 - _301E) : (num2 - num));
		}
		if (((Control)_3023).Visible)
		{
			num2 -= ((Control)_3023).Width;
			wizardButtonsLayoutEventArgs.NextButtonBounds = new Rectangle(num2, y, ((Control)_3023).Width, num3);
			num2 -= _301E;
		}
		if (((Control)_3024).Visible)
		{
			num2 -= ((Control)_3024).Width;
			wizardButtonsLayoutEventArgs.BackButtonBounds = new Rectangle(num2, y, ((Control)_3024).Width, num3);
			num2 -= _301E;
		}
		if (ᓛ)
		{
			_ = ((Control)panelFooter).Width;
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
		if (((Control)_3021).Visible)
		{
			((Control)_3021).Bounds = wizardButtonsLayoutEventArgs.HelpButtonBounds;
		}
		if (((Control)ẘ).Visible)
		{
			((Control)ẘ).Bounds = wizardButtonsLayoutEventArgs.CancelButtonBounds;
		}
		if (((Control)_3023).Visible)
		{
			((Control)_3023).Bounds = wizardButtonsLayoutEventArgs.NextButtonBounds;
		}
		if (((Control)_3022).Visible)
		{
			((Control)_3022).Bounds = wizardButtonsLayoutEventArgs.FinishButtonBounds;
		}
		if (((Control)_3024).Visible)
		{
			((Control)_3024).Bounds = wizardButtonsLayoutEventArgs.BackButtonBounds;
		}
	}

	private void _3040()
	{
	}

	private void ぁ(object _٠, EventArgs ؿ)
	{
		ᦗ();
	}

	private void あ(object _٠, EventArgs ؿ)
	{
		ᦗ();
	}

	private void ぃ(object _٠, EventArgs ؿ)
	{
		NavigateBack();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public WizardPage GetBackPage()
	{
		WizardPage result = null;
		if (_3029.Count > 0)
		{
			result = _3029.Pop() as WizardPage;
		}
		else if (SelectedPageIndex > 0)
		{
			result = _3028[SelectedPageIndex - 1];
		}
		return result;
	}

	private void い(object _٠, EventArgs ؿ)
	{
		NavigateNext();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public WizardPage GetNextPage()
	{
		if (SelectedPageIndex + 1 < _3028.Count)
		{
			return _3028[SelectedPageIndex + 1];
		}
		return null;
	}

	private void ぅ(object _٠, EventArgs ؿ)
	{
		NavigateFinish();
	}

	private void _2F23(object _٠, EventArgs ؿ)
	{
		NavigateCancel();
	}

	private void う(object _٠, EventArgs ؿ)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnHelpButtonClick(cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			WizardPage selectedPage = SelectedPage;
			if (selectedPage != null)
			{
				selectedPage.ᏻ(cancelEventArgs);
				_ = cancelEventArgs.Cancel;
			}
		}
	}

	internal void ぇ(WizardPage _2889)
	{
		ぐ(_2889);
		if (_3028.IndexOf(_2889) == _302A)
		{
			ぉ(_2889, eWizardPageChangeSource.Code);
		}
		else
		{
			_2889.Visible = false;
		}
		Controls.Add((Control)(object)_2889);
		が(き: true);
	}

	internal void え(WizardPage _2889)
	{
		Controls.Remove((Control)(object)_2889);
		if (_2889.Visible && SelectedPage != null && !SelectedPage.Visible)
		{
			ぉ(SelectedPage, eWizardPageChangeSource.Code);
		}
	}

	private bool ぉ(WizardPage _2889, eWizardPageChangeSource お)
	{
		WizardPage selectedPage = SelectedPage;
		WizardCancelPageChangeEventArgs wizardCancelPageChangeEventArgs = new WizardCancelPageChangeEventArgs(_2889, selectedPage, お);
		OnWizardPageChanging(wizardCancelPageChangeEventArgs);
		if (wizardCancelPageChangeEventArgs.Cancel || wizardCancelPageChangeEventArgs.NewPage == null)
		{
			return false;
		}
		_2889 = wizardCancelPageChangeEventArgs.NewPage;
		if (_2889 != null)
		{
			_2889.つ(wizardCancelPageChangeEventArgs);
			if (wizardCancelPageChangeEventArgs.Cancel || wizardCancelPageChangeEventArgs.NewPage == null)
			{
				return false;
			}
			_2889 = wizardCancelPageChangeEventArgs.NewPage;
		}
		if (_302B)
		{
			((Control)_3024).Enabled = false;
			((Control)_3023).Enabled = false;
			((Control)_3022).Enabled = false;
			((Control)ẘ).Enabled = false;
		}
		if (_302C)
		{
			((Control)this).Cursor = Cursors.WaitCursor;
		}
		if (selectedPage != null)
		{
			if (selectedPage != _2889)
			{
				if (((Component)this).DesignMode)
				{
					TypeDescriptor.GetProperties(selectedPage)["Visible"].SetValue(selectedPage, false);
				}
				selectedPage.Visible = false;
				selectedPage.て(new WizardPageChangeEventArgs(_2889, selectedPage, お));
			}
			else
			{
				foreach (WizardPage wizardPage in WizardPages)
				{
					if (wizardPage != _2889)
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
		if (_2889 != null)
		{
			if (((Component)this).DesignMode)
			{
				TypeDescriptor.GetProperties(_2889)["Visible"].SetValue(_2889, true);
			}
			_2889.Visible = true;
		}
		_302A = _3028.IndexOf(_2889);
		が(き: true);
		か();
		wizardCancelPageChangeEventArgs.Cancel = false;
		OnWizardPageChanged(wizardCancelPageChangeEventArgs);
		_2889?.づ(new WizardPageChangeEventArgs(_2889, selectedPage, お));
		if (_302C)
		{
			((Control)this).Cursor = Cursors.Default;
		}
		return true;
	}

	internal void か()
	{
		WizardPage selectedPage = SelectedPage;
		if (selectedPage == null)
		{
			return;
		}
		if (selectedPage.InteriorPage)
		{
			if (!((Control)_301C).Visible)
			{
				((Control)_301C).Visible = true;
			}
			if (((Control)_302F).Visible != 〲)
			{
				((Control)_302F).Visible = 〲;
				_303F();
			}
		}
		else if (((Control)_301C).Visible)
		{
			((Control)_301C).Visible = false;
		}
		((Control)_3027).Text = selectedPage.PageTitle;
		((Control)_3026).Text = selectedPage.PageDescription;
		if (selectedPage.PageHeaderImage != null)
		{
			((Control)_302F).BackgroundImage = selectedPage.PageHeaderImage;
			〱 = true;
		}
		else if (〱)
		{
			if (_3030 != null)
			{
				((Control)_302F).BackgroundImage = _3030;
			}
			else
			{
				((Control)_302F).BackgroundImage = (Image)(object)BarFunctions.ই("SystemImages.WizardHeaderImage.png");
			}
			〱 = false;
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

	internal void が(bool き)
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
		if (!〳)
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
			((Control)_3021).Visible = false;
		}
		else if (!((Control)_3021).Visible)
		{
			((Control)_3021).Visible = true;
		}
		if (eWizardButtonState9 == eWizardButtonState.False)
		{
			((Control)_3021).Enabled = false;
		}
		else if (!((Control)_3021).Enabled)
		{
			((Control)_3021).Enabled = true;
		}
		if (eWizardButtonState8 == eWizardButtonState.False)
		{
			((Control)ẘ).Visible = false;
		}
		else if (!((Control)ẘ).Visible)
		{
			((Control)ẘ).Visible = true;
		}
		if (eWizardButtonState7 == eWizardButtonState.False)
		{
			((Control)ẘ).Enabled = false;
		}
		else if (!((Control)ẘ).Enabled)
		{
			((Control)ẘ).Enabled = true;
		}
		if (_3025 || SelectedPageIndex == _3028.Count - 1 || eWizardButtonState6 == eWizardButtonState.True)
		{
			if (!((Control)_3022).Visible)
			{
				((Control)_3022).Visible = true;
			}
		}
		else
		{
			((Control)_3022).Visible = false;
		}
		if (eWizardButtonState6 == eWizardButtonState.False || (eWizardButtonState6 == eWizardButtonState.Auto && SelectedPageIndex < _3028.Count - 1))
		{
			((Control)_3022).Enabled = false;
		}
		else if (!((Control)_3022).Enabled)
		{
			((Control)_3022).Enabled = true;
		}
		switch (eWizardButtonState5)
		{
		case eWizardButtonState.True:
			if (!((Control)_3023).Visible)
			{
				((Control)_3023).Visible = true;
			}
			break;
		case eWizardButtonState.Auto:
			if (((Control)_3022).Visible && ((Control)_3022).Enabled && !_3025)
			{
				((Control)_3023).Visible = false;
			}
			else if (!((Control)_3023).Visible)
			{
				((Control)_3023).Visible = true;
			}
			break;
		default:
			if (((Control)_3023).Visible)
			{
				((Control)_3023).Visible = false;
			}
			break;
		}
		switch (eWizardButtonState4)
		{
		case eWizardButtonState.True:
			if (!((Control)_3023).Enabled)
			{
				((Control)_3023).Enabled = true;
			}
			break;
		case eWizardButtonState.Auto:
			if ((((Control)_3022).Visible && ((Control)_3022).Enabled) || SelectedPageIndex == _3028.Count - 1)
			{
				((Control)_3023).Enabled = false;
			}
			else if (!((Control)_3023).Enabled)
			{
				((Control)_3023).Enabled = true;
			}
			break;
		default:
			if (((Control)_3023).Enabled)
			{
				((Control)_3023).Enabled = false;
			}
			break;
		}
		if (き)
		{
			if (((Control)_3023).Enabled && ((Control)_3023).Visible)
			{
				((Control)_3023).Select();
			}
			else if (((Control)_3022).Enabled && ((Control)_3022).Visible)
			{
				((Control)_3022).Select();
			}
		}
		if (eWizardButtonState3 == eWizardButtonState.True || eWizardButtonState3 == eWizardButtonState.Auto)
		{
			if (!((Control)_3024).Visible)
			{
				((Control)_3024).Visible = true;
			}
		}
		else if (((Control)_3024).Visible)
		{
			((Control)_3024).Visible = false;
		}
		switch (eWizardButtonState2)
		{
		case eWizardButtonState.True:
			if (!((Control)_3024).Enabled)
			{
				((Control)_3024).Enabled = true;
			}
			break;
		case eWizardButtonState.Auto:
			if (_3029.Count > 0 || SelectedPageIndex > 0)
			{
				if (!((Control)_3024).Enabled)
				{
					((Control)_3024).Enabled = true;
				}
			}
			else if (((Control)_3024).Enabled)
			{
				((Control)_3024).Enabled = false;
			}
			break;
		case eWizardButtonState.False:
			if (((Control)_3024).Enabled)
			{
				((Control)_3024).Enabled = false;
			}
			break;
		}
		ぎ();
		く();
		ᦗ();
	}

	private void ぎ()
	{
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return;
		}
		if (_302D == eWizardFormAcceptButton.FinishAndNext)
		{
			if (((Control)_3022).Visible && ((Control)_3022).Enabled)
			{
				if (val.AcceptButton != _3022)
				{
					val.AcceptButton = (IButtonControl)(object)_3022;
				}
			}
			else if (val.AcceptButton != _3023)
			{
				val.AcceptButton = (IButtonControl)(object)_3023;
			}
		}
		else if (_302D == eWizardFormAcceptButton.Next)
		{
			if (val.AcceptButton != _3023)
			{
				val.AcceptButton = (IButtonControl)(object)_3023;
			}
		}
		else if (_302D == eWizardFormAcceptButton.Finish)
		{
			if (val.AcceptButton != _3022)
			{
				val.AcceptButton = (IButtonControl)(object)_3022;
			}
		}
		else if (_302D == eWizardFormAcceptButton.None && (val.AcceptButton == _3022 || val.AcceptButton == _3023))
		{
			val.AcceptButton = null;
		}
	}

	private void く()
	{
		Form val = ((Control)this).FindForm();
		if (val == null)
		{
			return;
		}
		if (_302E == eWizardFormCancelButton.Cancel)
		{
			if (val.CancelButton != ẘ)
			{
				val.CancelButton = (IButtonControl)(object)ẘ;
			}
		}
		else if (val.CancelButton == ẘ)
		{
			val.CancelButton = null;
		}
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((ContainerControl)this).OnParentChanged(e);
		く();
		ぎ();
	}

	internal void ぐ(WizardPage _2889)
	{
		if (_2889 == null)
		{
			return;
		}
		Size size = new Size(7, 12);
		((Control)this).SuspendLayout();
		try
		{
			Rectangle rectangle = Rectangle.Empty;
			rectangle = ((!_2889.InteriorPage) ? new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height - ((Control)panelFooter).Height) : new Rectangle(((Control)this).ClientRectangle.X + size.Width, ((Control)this).ClientRectangle.Y + ((Control)_301C).Height + size.Height, ((Control)this).ClientRectangle.Width - size.Width * 2, ((Control)this).ClientRectangle.Height - (((Control)_301C).Height + ((Control)panelFooter).Height + size.Height * 2)));
			TypeDescriptor.GetProperties(_2889)["Bounds"].SetValue(_2889, rectangle);
			TypeDescriptor.GetProperties(_2889)["Anchor"].SetValue(_2889, (object)(AnchorStyles)15);
		}
		finally
		{
			((Control)this).ResumeLayout();
		}
	}

	private void け()
	{
		if (_303D == eWizardStyle.Default)
		{
			_3024.Style = eDotNetBarStyle.Office2000;
			ẘ.Style = eDotNetBarStyle.Office2000;
			_3022.Style = eDotNetBarStyle.Office2000;
			_3021.Style = eDotNetBarStyle.Office2000;
			_3023.Style = eDotNetBarStyle.Office2000;
			_3024.ThemeAware = true;
			ẘ.ThemeAware = true;
			_3022.ThemeAware = true;
			_3021.ThemeAware = true;
			_3023.ThemeAware = true;
		}
		else
		{
			_3024.Style = eDotNetBarStyle.Office2007;
			ẘ.Style = eDotNetBarStyle.Office2007;
			_3022.Style = eDotNetBarStyle.Office2007;
			_3021.Style = eDotNetBarStyle.Office2007;
			_3023.Style = eDotNetBarStyle.Office2007;
			_3024.ThemeAware = false;
			ẘ.ThemeAware = false;
			_3022.ThemeAware = false;
			_3021.ThemeAware = false;
			_3023.ThemeAware = false;
		}
	}

	protected virtual void OnBackButtonClick(CancelEventArgs e)
	{
		if (_1BFE != null)
		{
			_1BFE(this, e);
		}
	}

	protected virtual void OnNextButtonClick(CancelEventArgs e)
	{
		if (〴 != null)
		{
			〴(this, e);
		}
	}

	protected virtual void OnFinishButtonClick(CancelEventArgs e)
	{
		if (〵 != null)
		{
			〵(this, e);
		}
	}

	protected virtual void OnCancelButtonClick(CancelEventArgs e)
	{
		if (_3036 != null)
		{
			_3036(this, e);
		}
	}

	protected virtual void OnHelpButtonClick(CancelEventArgs e)
	{
		if (Ꮹ != null)
		{
			Ꮹ(this, e);
		}
	}

	protected virtual void OnWizardPageChanging(WizardCancelPageChangeEventArgs e)
	{
		if (_3037 != null)
		{
			_3037(this, e);
		}
	}

	protected virtual void OnWizardPageChanged(WizardCancelPageChangeEventArgs e)
	{
		if (_3038 != null)
		{
			_3038(this, e);
		}
	}

	protected virtual void OnLayoutWizardButtons(WizardButtonsLayoutEventArgs e)
	{
		if (_3039 != null)
		{
			_3039(this, e);
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		Ӕ ӕ2 = new Ӕ();
		ӕ2.Ḕ();
		((Component)(object)ӕ2).Dispose();
	}
}
