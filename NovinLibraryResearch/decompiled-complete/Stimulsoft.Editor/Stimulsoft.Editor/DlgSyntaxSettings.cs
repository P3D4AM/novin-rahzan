using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class DlgSyntaxSettings : Form
{
	private IContainer components;

	public Panel pnButtons;

	public Button btCancel;

	public Button btOK;

	public Panel pnMain;

	public Panel pnTree;

	public TreeView tvProperties;

	public Panel pnManage;

	public ImageList imageList1;

	public TabControl tcMain;

	public TabPage tpGeneral;

	public TabPage tpFontsAndColors;

	public TabPage tpAdditional;

	public Panel pnGeneral;

	public GroupBox gbGutterMargin;

	public CheckBox chbShowMargin;

	public Label laGutterWidth;

	public CheckBox chbShowGutter;

	public Label laMarginPosition;

	public GroupBox gbDocument;

	public CheckBox chbDragAndDrop;

	public CheckBox chbHorzScrollBar;

	public CheckBox chbVertScrollBar;

	public CheckBox chbWordWrap;

	public CheckBox chbHighlightUrls;

	public Panel pnFontsColors;

	public Label laDisplayItems;

	public Panel pnSampleText;

	public Label laSampleText;

	public Label laSample;

	public Label laSize;

	public Label laFont;

	public ComboBox cbFontName;

	public Label laDescription;

	public TextBox tbDescription;

	public Label laBackColor;

	public Label laForeColor;

	public ListBox lbStyles;

	public GroupBox gbFontAttributes;

	public CheckBox chbUnderline;

	public CheckBox chbItalic;

	public CheckBox chbBold;

	public Panel pnAdditional;

	public GroupBox gbTabOptions;

	public TextBox tbTabStops;

	public RadioButton rbKeepTabs;

	public RadioButton rbInsertSpaces;

	public Label laTabSizes;

	public GroupBox gbOutlineOptions;

	public CheckBox chbAllowOutlining;

	public CheckBox chbShowHints;

	public GroupBox gbNavigateOptions;

	public CheckBox chbMoveOnRightButton;

	public CheckBox chbBeyondEof;

	public CheckBox chbBeyondEol;

	public ColorBox cbForeColor;

	public ColorBox cbBackColor;

	public GroupBox gbLineNumbers;

	public CheckBox chbLineNumbers;

	public CheckBox chbLineNumbersOnGutter;

	public TextBox tbGutterWidth;

	public TextBox tbMarginPosition;

	public TextBox tbFontSize;

	private CheckBox chbForced;

	public TabPage tpKeyboard;

	public Panel pnKeyboard;

	private Label laKeyboardMappingScheme;

	private Button btSaveSchemeAs;

	private Button btDeleteScheme;

	private Label laShowCommands;

	private TextBox tbShowCommands;

	private ListBox lbEventHandlers;

	private Label laShortcuts;

	private ComboBox cbShortcuts;

	private Button btRemoveShortcut;

	private Label laUseShortcut;

	private Label laShortcutKeys;

	private ComboBox cbUseShortcutIn;

	private TextBox tbShortcutKeys;

	private Label laShortcutCurrentlyUsed;

	private ComboBox cbShortcutCurrentlyUsed;

	private ComboBox cbKeyboardSchemes;

	private Button btAssignShortcuts;

	private const int openFolderImage = 0;

	private const int closeFolderImage = 1;

	private const int selectedImage = 2;

	private const int unSelectedImage = 3;

	private ISyntaxSettings syntaxSettings;

	private Color curForeColor;

	private Color curBkColor;

	private FontStyle curFontStyle;

	private string curDesc;

	private bool isControlUpdating;

	private bool isFontControlsUpdating;

	private TreeNode rootNode;

	private TreeNode generalNode;

	private TreeNode fontsNode;

	private TreeNode keyboardNode;

	private TreeNode additionalNode;

	private string sControl = "control";

	private string sCtrl = "CTRL";

	private string sAlt = "alt";

	private GroupBox gbColorThemes;

	private ComboBox cbColorThemes;

	private Button btAddColorTheme;

	private Button btDeleteColorTheme;

	private CheckBox chbLineSeparator;

	public CheckBox chbLineModificator;

	public CheckBox chbWhiteSpace;

	private string sShift = "shift";

	public ISyntaxSettings SyntaxSettings
	{
		get
		{
			return syntaxSettings;
		}
		set
		{
			syntaxSettings.Assign(value);
		}
	}

	public DlgSyntaxSettings()
	{
		InitializeComponent();
		syntaxSettings = new SyntaxSettings();
		rootNode = tvProperties.Nodes[0];
		generalNode = rootNode.Nodes[0];
		fontsNode = rootNode.Nodes[1];
		additionalNode = rootNode.Nodes[2];
		keyboardNode = rootNode.Nodes[3];
	}

	public DlgSyntaxSettings(EditorSettingsTab hiddenTabs)
		: this()
	{
		if ((hiddenTabs & EditorSettingsTab.General) != 0)
		{
			rootNode.Nodes.Remove(generalNode);
		}
		if ((hiddenTabs & EditorSettingsTab.FontsAndColors) != 0)
		{
			rootNode.Nodes.Remove(fontsNode);
		}
		if ((hiddenTabs & EditorSettingsTab.Additional) != 0)
		{
			rootNode.Nodes.Remove(additionalNode);
		}
		if ((hiddenTabs & EditorSettingsTab.Keymapping) != 0)
		{
			rootNode.Nodes.Remove(keyboardNode);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Expected O, but got Unknown
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Expected O, but got Unknown
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Expected O, but got Unknown
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Expected O, but got Unknown
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Expected O, but got Unknown
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Expected O, but got Unknown
		//IL_0cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Expected O, but got Unknown
		//IL_3027: Unknown result type (might be due to invalid IL or missing references)
		//IL_3031: Expected O, but got Unknown
		//IL_3043: Unknown result type (might be due to invalid IL or missing references)
		//IL_304d: Expected O, but got Unknown
		//IL_3153: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		TreeNode val = new TreeNode("General");
		TreeNode val2 = new TreeNode("Fonts and Colors");
		TreeNode val3 = new TreeNode("Additional");
		TreeNode val4 = new TreeNode("Keyboard");
		TreeNode val5 = new TreeNode("Options", 1, 0, (TreeNode[])(object)new TreeNode[4] { val, val2, val3, val4 });
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DlgSyntaxSettings));
		pnButtons = new Panel();
		btCancel = new Button();
		btOK = new Button();
		pnMain = new Panel();
		pnManage = new Panel();
		tcMain = new TabControl();
		tpGeneral = new TabPage();
		pnGeneral = new Panel();
		gbLineNumbers = new GroupBox();
		chbLineNumbers = new CheckBox();
		chbLineNumbersOnGutter = new CheckBox();
		gbGutterMargin = new GroupBox();
		tbMarginPosition = new TextBox();
		tbGutterWidth = new TextBox();
		chbShowMargin = new CheckBox();
		laGutterWidth = new Label();
		chbShowGutter = new CheckBox();
		laMarginPosition = new Label();
		gbDocument = new GroupBox();
		chbLineSeparator = new CheckBox();
		chbLineModificator = new CheckBox();
		chbWhiteSpace = new CheckBox();
		chbForced = new CheckBox();
		chbDragAndDrop = new CheckBox();
		chbHorzScrollBar = new CheckBox();
		chbVertScrollBar = new CheckBox();
		chbWordWrap = new CheckBox();
		chbHighlightUrls = new CheckBox();
		tpAdditional = new TabPage();
		pnAdditional = new Panel();
		gbTabOptions = new GroupBox();
		tbTabStops = new TextBox();
		rbKeepTabs = new RadioButton();
		rbInsertSpaces = new RadioButton();
		laTabSizes = new Label();
		gbOutlineOptions = new GroupBox();
		chbAllowOutlining = new CheckBox();
		chbShowHints = new CheckBox();
		gbNavigateOptions = new GroupBox();
		chbMoveOnRightButton = new CheckBox();
		chbBeyondEof = new CheckBox();
		chbBeyondEol = new CheckBox();
		tpFontsAndColors = new TabPage();
		pnFontsColors = new Panel();
		cbForeColor = new ColorBox(components);
		cbBackColor = new ColorBox(components);
		gbColorThemes = new GroupBox();
		btDeleteColorTheme = new Button();
		btAddColorTheme = new Button();
		cbColorThemes = new ComboBox();
		tbFontSize = new TextBox();
		laDisplayItems = new Label();
		pnSampleText = new Panel();
		laSampleText = new Label();
		laSample = new Label();
		laSize = new Label();
		laFont = new Label();
		cbFontName = new ComboBox();
		laDescription = new Label();
		tbDescription = new TextBox();
		laBackColor = new Label();
		laForeColor = new Label();
		lbStyles = new ListBox();
		gbFontAttributes = new GroupBox();
		chbUnderline = new CheckBox();
		chbItalic = new CheckBox();
		chbBold = new CheckBox();
		tpKeyboard = new TabPage();
		pnKeyboard = new Panel();
		cbShortcutCurrentlyUsed = new ComboBox();
		laShortcutCurrentlyUsed = new Label();
		btAssignShortcuts = new Button();
		tbShortcutKeys = new TextBox();
		cbUseShortcutIn = new ComboBox();
		laShortcutKeys = new Label();
		laUseShortcut = new Label();
		btRemoveShortcut = new Button();
		cbShortcuts = new ComboBox();
		laShortcuts = new Label();
		lbEventHandlers = new ListBox();
		tbShowCommands = new TextBox();
		laShowCommands = new Label();
		btDeleteScheme = new Button();
		btSaveSchemeAs = new Button();
		cbKeyboardSchemes = new ComboBox();
		laKeyboardMappingScheme = new Label();
		pnTree = new Panel();
		tvProperties = new TreeView();
		imageList1 = new ImageList(components);
		((Control)pnButtons).SuspendLayout();
		((Control)pnMain).SuspendLayout();
		((Control)pnManage).SuspendLayout();
		((Control)tcMain).SuspendLayout();
		((Control)tpGeneral).SuspendLayout();
		((Control)pnGeneral).SuspendLayout();
		((Control)gbLineNumbers).SuspendLayout();
		((Control)gbGutterMargin).SuspendLayout();
		((Control)gbDocument).SuspendLayout();
		((Control)tpAdditional).SuspendLayout();
		((Control)pnAdditional).SuspendLayout();
		((Control)gbTabOptions).SuspendLayout();
		((Control)gbOutlineOptions).SuspendLayout();
		((Control)gbNavigateOptions).SuspendLayout();
		((Control)tpFontsAndColors).SuspendLayout();
		((Control)pnFontsColors).SuspendLayout();
		((Control)gbColorThemes).SuspendLayout();
		((Control)pnSampleText).SuspendLayout();
		((Control)gbFontAttributes).SuspendLayout();
		((Control)tpKeyboard).SuspendLayout();
		((Control)pnKeyboard).SuspendLayout();
		((Control)pnTree).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)pnButtons).Anchor = (AnchorStyles)14;
		((Control)pnButtons).Controls.Add((Control)(object)btCancel);
		((Control)pnButtons).Controls.Add((Control)(object)btOK);
		((Control)pnButtons).Location = new Point(12, 406);
		((Control)pnButtons).Name = "pnButtons";
		((Control)pnButtons).Size = new Size(546, 40);
		((Control)pnButtons).TabIndex = 6;
		btCancel.DialogResult = (DialogResult)2;
		((ButtonBase)btCancel).FlatStyle = (FlatStyle)3;
		((Control)btCancel).Location = new Point(467, 8);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		btOK.DialogResult = (DialogResult)1;
		((ButtonBase)btOK).FlatStyle = (FlatStyle)3;
		((Control)btOK).Location = new Point(385, 8);
		((Control)btOK).Name = "btOK";
		((Control)btOK).Size = new Size(75, 23);
		((Control)btOK).TabIndex = 0;
		((Control)btOK).Text = "OK";
		((Control)btOK).Click += btOK_Click;
		((Control)pnMain).Anchor = (AnchorStyles)15;
		((Control)pnMain).Controls.Add((Control)(object)pnManage);
		((Control)pnMain).Controls.Add((Control)(object)pnTree);
		((Control)pnMain).Location = new Point(12, 12);
		((Control)pnMain).Name = "pnMain";
		((Control)pnMain).Size = new Size(543, 390);
		((Control)pnMain).TabIndex = 7;
		((Control)pnManage).Anchor = (AnchorStyles)15;
		((Control)pnManage).Controls.Add((Control)(object)tcMain);
		((Control)pnManage).Location = new Point(136, 0);
		((Control)pnManage).Name = "pnManage";
		((Control)pnManage).Size = new Size(407, 390);
		((Control)pnManage).TabIndex = 1;
		((Control)tcMain).Controls.Add((Control)(object)tpGeneral);
		((Control)tcMain).Controls.Add((Control)(object)tpAdditional);
		((Control)tcMain).Controls.Add((Control)(object)tpFontsAndColors);
		((Control)tcMain).Controls.Add((Control)(object)tpKeyboard);
		((Control)tcMain).Dock = (DockStyle)5;
		((Control)tcMain).Location = new Point(0, 0);
		((Control)tcMain).Name = "tcMain";
		tcMain.SelectedIndex = 0;
		((Control)tcMain).Size = new Size(407, 390);
		((Control)tcMain).TabIndex = 0;
		((Control)tcMain).Visible = false;
		((Control)tpGeneral).Controls.Add((Control)(object)pnGeneral);
		tpGeneral.Location = new Point(4, 22);
		((Control)tpGeneral).Name = "tpGeneral";
		((Control)tpGeneral).Size = new Size(399, 364);
		tpGeneral.TabIndex = 0;
		((Control)tpGeneral).Text = "General";
		((Control)pnGeneral).BackColor = Color.White;
		((Control)pnGeneral).Controls.Add((Control)(object)gbLineNumbers);
		((Control)pnGeneral).Controls.Add((Control)(object)gbGutterMargin);
		((Control)pnGeneral).Controls.Add((Control)(object)gbDocument);
		((Control)pnGeneral).Dock = (DockStyle)5;
		((Control)pnGeneral).Location = new Point(0, 0);
		((Control)pnGeneral).Name = "pnGeneral";
		((Control)pnGeneral).Size = new Size(399, 364);
		((Control)pnGeneral).TabIndex = 1;
		((Control)pnGeneral).Visible = false;
		((Control)gbLineNumbers).Controls.Add((Control)(object)chbLineNumbers);
		((Control)gbLineNumbers).Controls.Add((Control)(object)chbLineNumbersOnGutter);
		gbLineNumbers.FlatStyle = (FlatStyle)3;
		((Control)gbLineNumbers).Location = new Point(8, 200);
		((Control)gbLineNumbers).Name = "gbLineNumbers";
		((Control)gbLineNumbers).Size = new Size(384, 72);
		((Control)gbLineNumbers).TabIndex = 39;
		gbLineNumbers.TabStop = false;
		((Control)gbLineNumbers).Text = "Line Numbers";
		((ButtonBase)chbLineNumbers).FlatStyle = (FlatStyle)3;
		((Control)chbLineNumbers).Location = new Point(8, 16);
		((Control)chbLineNumbers).Name = "chbLineNumbers";
		((Control)chbLineNumbers).Size = new Size(200, 25);
		((Control)chbLineNumbers).TabIndex = 0;
		((Control)chbLineNumbers).Text = "Show Line Numbers";
		((ButtonBase)chbLineNumbersOnGutter).FlatStyle = (FlatStyle)3;
		((Control)chbLineNumbersOnGutter).Location = new Point(8, 41);
		((Control)chbLineNumbersOnGutter).Name = "chbLineNumbersOnGutter";
		((Control)chbLineNumbersOnGutter).Size = new Size(200, 25);
		((Control)chbLineNumbersOnGutter).TabIndex = 1;
		((Control)chbLineNumbersOnGutter).Text = "Display on Gutter";
		((Control)gbGutterMargin).Controls.Add((Control)(object)tbMarginPosition);
		((Control)gbGutterMargin).Controls.Add((Control)(object)tbGutterWidth);
		((Control)gbGutterMargin).Controls.Add((Control)(object)chbShowMargin);
		((Control)gbGutterMargin).Controls.Add((Control)(object)laGutterWidth);
		((Control)gbGutterMargin).Controls.Add((Control)(object)chbShowGutter);
		((Control)gbGutterMargin).Controls.Add((Control)(object)laMarginPosition);
		gbGutterMargin.FlatStyle = (FlatStyle)3;
		((Control)gbGutterMargin).Location = new Point(8, 116);
		((Control)gbGutterMargin).Name = "gbGutterMargin";
		((Control)gbGutterMargin).Size = new Size(384, 72);
		((Control)gbGutterMargin).TabIndex = 38;
		gbGutterMargin.TabStop = false;
		((Control)gbGutterMargin).Text = "Gutter&&Margin";
		((Control)tbMarginPosition).Location = new Point(324, 40);
		((Control)tbMarginPosition).Name = "tbMarginPosition";
		((Control)tbMarginPosition).Size = new Size(48, 20);
		((Control)tbMarginPosition).TabIndex = 8;
		((Control)tbMarginPosition).Text = "0";
		((Control)tbMarginPosition).KeyPress += new KeyPressEventHandler(tbMarginPosition_KeyPress);
		((Control)tbGutterWidth).Location = new Point(324, 16);
		((Control)tbGutterWidth).Name = "tbGutterWidth";
		((Control)tbGutterWidth).Size = new Size(48, 20);
		((Control)tbGutterWidth).TabIndex = 7;
		((Control)tbGutterWidth).Text = "0";
		((Control)tbGutterWidth).KeyPress += new KeyPressEventHandler(tbGutterWidth_KeyPress);
		((ButtonBase)chbShowMargin).FlatStyle = (FlatStyle)3;
		((Control)chbShowMargin).Location = new Point(8, 41);
		((Control)chbShowMargin).Name = "chbShowMargin";
		((Control)chbShowMargin).Size = new Size(136, 25);
		((Control)chbShowMargin).TabIndex = 1;
		((Control)chbShowMargin).Text = "Show Margin";
		laGutterWidth.FlatStyle = (FlatStyle)3;
		((Control)laGutterWidth).Location = new Point(160, 16);
		((Control)laGutterWidth).Name = "laGutterWidth";
		((Control)laGutterWidth).Size = new Size(152, 25);
		((Control)laGutterWidth).TabIndex = 3;
		((Control)laGutterWidth).Text = "Gutter width:";
		((ButtonBase)chbShowGutter).FlatStyle = (FlatStyle)3;
		((Control)chbShowGutter).Location = new Point(8, 16);
		((Control)chbShowGutter).Name = "chbShowGutter";
		((Control)chbShowGutter).Size = new Size(136, 25);
		((Control)chbShowGutter).TabIndex = 0;
		((Control)chbShowGutter).Text = "Show Gutter";
		laMarginPosition.FlatStyle = (FlatStyle)3;
		((Control)laMarginPosition).Location = new Point(160, 41);
		((Control)laMarginPosition).Name = "laMarginPosition";
		((Control)laMarginPosition).Size = new Size(152, 25);
		((Control)laMarginPosition).TabIndex = 4;
		((Control)laMarginPosition).Text = "Margin position:";
		((Control)gbDocument).Controls.Add((Control)(object)chbLineSeparator);
		((Control)gbDocument).Controls.Add((Control)(object)chbLineModificator);
		((Control)gbDocument).Controls.Add((Control)(object)chbWhiteSpace);
		((Control)gbDocument).Controls.Add((Control)(object)chbForced);
		((Control)gbDocument).Controls.Add((Control)(object)chbDragAndDrop);
		((Control)gbDocument).Controls.Add((Control)(object)chbHorzScrollBar);
		((Control)gbDocument).Controls.Add((Control)(object)chbVertScrollBar);
		((Control)gbDocument).Controls.Add((Control)(object)chbWordWrap);
		((Control)gbDocument).Controls.Add((Control)(object)chbHighlightUrls);
		gbDocument.FlatStyle = (FlatStyle)3;
		((Control)gbDocument).Location = new Point(8, 8);
		((Control)gbDocument).Name = "gbDocument";
		((Control)gbDocument).Size = new Size(384, 98);
		((Control)gbDocument).TabIndex = 11;
		gbDocument.TabStop = false;
		((Control)gbDocument).Text = "Document";
		((ButtonBase)chbLineSeparator).FlatStyle = (FlatStyle)3;
		((Control)chbLineSeparator).Location = new Point(250, 62);
		((Control)chbLineSeparator).Name = "chbLineSeparator";
		((Control)chbLineSeparator).Size = new Size(122, 25);
		((Control)chbLineSeparator).TabIndex = 8;
		((Control)chbLineSeparator).Text = "Line separator";
		((ButtonBase)chbLineModificator).FlatStyle = (FlatStyle)3;
		((Control)chbLineModificator).Location = new Point(250, 37);
		((Control)chbLineModificator).Name = "chbLineModificator";
		((Control)chbLineModificator).Size = new Size(122, 25);
		((Control)chbLineModificator).TabIndex = 7;
		((Control)chbLineModificator).Text = "Line modificator";
		((ButtonBase)chbWhiteSpace).FlatStyle = (FlatStyle)3;
		((Control)chbWhiteSpace).Location = new Point(250, 12);
		((Control)chbWhiteSpace).Name = "chbWhiteSpace";
		((Control)chbWhiteSpace).Size = new Size(122, 25);
		((Control)chbWhiteSpace).TabIndex = 6;
		((Control)chbWhiteSpace).Text = "White space";
		((ButtonBase)chbForced).FlatStyle = (FlatStyle)3;
		((Control)chbForced).Location = new Point(126, 66);
		((Control)chbForced).Name = "chbForced";
		((Control)chbForced).Size = new Size(122, 25);
		((Control)chbForced).TabIndex = 5;
		((Control)chbForced).Text = "Forced";
		((ButtonBase)chbDragAndDrop).FlatStyle = (FlatStyle)3;
		((Control)chbDragAndDrop).Location = new Point(8, 66);
		((Control)chbDragAndDrop).Name = "chbDragAndDrop";
		((Control)chbDragAndDrop).Size = new Size(144, 25);
		((Control)chbDragAndDrop).TabIndex = 2;
		((Control)chbDragAndDrop).Text = "&Drag and drop text";
		((ButtonBase)chbHorzScrollBar).FlatStyle = (FlatStyle)3;
		((Control)chbHorzScrollBar).Location = new Point(126, 41);
		((Control)chbHorzScrollBar).Name = "chbHorzScrollBar";
		((Control)chbHorzScrollBar).Size = new Size(122, 25);
		((Control)chbHorzScrollBar).TabIndex = 4;
		((Control)chbHorzScrollBar).Text = "&Horizontal scroll bar";
		((ButtonBase)chbVertScrollBar).FlatStyle = (FlatStyle)3;
		((Control)chbVertScrollBar).Location = new Point(126, 16);
		((Control)chbVertScrollBar).Name = "chbVertScrollBar";
		((Control)chbVertScrollBar).Size = new Size(122, 25);
		((Control)chbVertScrollBar).TabIndex = 3;
		((Control)chbVertScrollBar).Text = "&Vertical scroll bar";
		((ButtonBase)chbWordWrap).FlatStyle = (FlatStyle)3;
		((Control)chbWordWrap).Location = new Point(8, 16);
		((Control)chbWordWrap).Name = "chbWordWrap";
		((Control)chbWordWrap).Size = new Size(124, 25);
		((Control)chbWordWrap).TabIndex = 0;
		((Control)chbWordWrap).Text = "Word Wrap";
		((ButtonBase)chbHighlightUrls).FlatStyle = (FlatStyle)3;
		((Control)chbHighlightUrls).Location = new Point(8, 41);
		((Control)chbHighlightUrls).Name = "chbHighlightUrls";
		((Control)chbHighlightUrls).Size = new Size(128, 25);
		((Control)chbHighlightUrls).TabIndex = 1;
		((Control)chbHighlightUrls).Text = "Highlight Urls";
		((Control)tpAdditional).Controls.Add((Control)(object)pnAdditional);
		tpAdditional.Location = new Point(4, 22);
		((Control)tpAdditional).Name = "tpAdditional";
		((Control)tpAdditional).Size = new Size(399, 364);
		tpAdditional.TabIndex = 2;
		((Control)tpAdditional).Text = "Additional";
		((Control)pnAdditional).BackColor = Color.White;
		((Control)pnAdditional).Controls.Add((Control)(object)gbTabOptions);
		((Control)pnAdditional).Controls.Add((Control)(object)gbOutlineOptions);
		((Control)pnAdditional).Controls.Add((Control)(object)gbNavigateOptions);
		((Control)pnAdditional).Dock = (DockStyle)5;
		((Control)pnAdditional).Location = new Point(0, 0);
		((Control)pnAdditional).Name = "pnAdditional";
		((Control)pnAdditional).Size = new Size(399, 364);
		((Control)pnAdditional).TabIndex = 8;
		((Control)pnAdditional).Visible = false;
		((Control)gbTabOptions).Controls.Add((Control)(object)tbTabStops);
		((Control)gbTabOptions).Controls.Add((Control)(object)rbKeepTabs);
		((Control)gbTabOptions).Controls.Add((Control)(object)rbInsertSpaces);
		((Control)gbTabOptions).Controls.Add((Control)(object)laTabSizes);
		gbTabOptions.FlatStyle = (FlatStyle)3;
		((Control)gbTabOptions).Location = new Point(8, 200);
		((Control)gbTabOptions).Name = "gbTabOptions";
		((Control)gbTabOptions).Size = new Size(384, 72);
		((Control)gbTabOptions).TabIndex = 40;
		gbTabOptions.TabStop = false;
		((Control)gbTabOptions).Text = "Tab Options";
		((Control)tbTabStops).Location = new Point(8, 40);
		((Control)tbTabStops).Name = "tbTabStops";
		((Control)tbTabStops).Size = new Size(120, 20);
		((Control)tbTabStops).TabIndex = 5;
		((ButtonBase)rbKeepTabs).FlatStyle = (FlatStyle)3;
		((Control)rbKeepTabs).Location = new Point(160, 41);
		((Control)rbKeepTabs).Name = "rbKeepTabs";
		((Control)rbKeepTabs).Size = new Size(184, 25);
		((Control)rbKeepTabs).TabIndex = 4;
		((Control)rbKeepTabs).Text = "&Keep tabs";
		((ButtonBase)rbInsertSpaces).FlatStyle = (FlatStyle)3;
		((Control)rbInsertSpaces).Location = new Point(160, 16);
		((Control)rbInsertSpaces).Name = "rbInsertSpaces";
		((Control)rbInsertSpaces).Size = new Size(184, 25);
		((Control)rbInsertSpaces).TabIndex = 3;
		((Control)rbInsertSpaces).Text = "Insert s&paces";
		((Control)laTabSizes).AutoSize = true;
		laTabSizes.FlatStyle = (FlatStyle)3;
		((Control)laTabSizes).Location = new Point(8, 21);
		((Control)laTabSizes).Name = "laTabSizes";
		((Control)laTabSizes).Size = new Size(57, 13);
		((Control)laTabSizes).TabIndex = 0;
		((Control)laTabSizes).Text = "Tab Sizes:";
		((Control)gbOutlineOptions).Controls.Add((Control)(object)chbAllowOutlining);
		((Control)gbOutlineOptions).Controls.Add((Control)(object)chbShowHints);
		gbOutlineOptions.FlatStyle = (FlatStyle)3;
		((Control)gbOutlineOptions).Location = new Point(8, 116);
		((Control)gbOutlineOptions).Name = "gbOutlineOptions";
		((Control)gbOutlineOptions).Size = new Size(384, 72);
		((Control)gbOutlineOptions).TabIndex = 39;
		gbOutlineOptions.TabStop = false;
		((Control)gbOutlineOptions).Text = "Outline Options";
		((ButtonBase)chbAllowOutlining).FlatStyle = (FlatStyle)3;
		((Control)chbAllowOutlining).Location = new Point(8, 16);
		((Control)chbAllowOutlining).Name = "chbAllowOutlining";
		((Control)chbAllowOutlining).Size = new Size(336, 25);
		((Control)chbAllowOutlining).TabIndex = 0;
		((Control)chbAllowOutlining).Text = "Allow outlining";
		((ButtonBase)chbShowHints).FlatStyle = (FlatStyle)3;
		((Control)chbShowHints).Location = new Point(8, 41);
		((Control)chbShowHints).Name = "chbShowHints";
		((Control)chbShowHints).Size = new Size(336, 25);
		((Control)chbShowHints).TabIndex = 1;
		((Control)chbShowHints).Text = "Show Hints";
		((Control)gbNavigateOptions).Controls.Add((Control)(object)chbMoveOnRightButton);
		((Control)gbNavigateOptions).Controls.Add((Control)(object)chbBeyondEof);
		((Control)gbNavigateOptions).Controls.Add((Control)(object)chbBeyondEol);
		gbNavigateOptions.FlatStyle = (FlatStyle)3;
		((Control)gbNavigateOptions).Location = new Point(8, 8);
		((Control)gbNavigateOptions).Name = "gbNavigateOptions";
		((Control)gbNavigateOptions).Size = new Size(384, 98);
		((Control)gbNavigateOptions).TabIndex = 38;
		gbNavigateOptions.TabStop = false;
		((Control)gbNavigateOptions).Text = "Navigate Options";
		((ButtonBase)chbMoveOnRightButton).FlatStyle = (FlatStyle)3;
		((Control)chbMoveOnRightButton).Location = new Point(8, 66);
		((Control)chbMoveOnRightButton).Name = "chbMoveOnRightButton";
		((Control)chbMoveOnRightButton).Size = new Size(336, 25);
		((Control)chbMoveOnRightButton).TabIndex = 14;
		((Control)chbMoveOnRightButton).Text = "Move on Right Button";
		((ButtonBase)chbBeyondEof).FlatStyle = (FlatStyle)3;
		((Control)chbBeyondEof).Location = new Point(8, 41);
		((Control)chbBeyondEof).Name = "chbBeyondEof";
		((Control)chbBeyondEof).Size = new Size(336, 25);
		((Control)chbBeyondEof).TabIndex = 13;
		((Control)chbBeyondEof).Text = "Beyond Eof";
		((ButtonBase)chbBeyondEol).FlatStyle = (FlatStyle)3;
		((Control)chbBeyondEol).Location = new Point(8, 16);
		((Control)chbBeyondEol).Name = "chbBeyondEol";
		((Control)chbBeyondEol).Size = new Size(336, 25);
		((Control)chbBeyondEol).TabIndex = 12;
		((Control)chbBeyondEol).Text = "Beyond Eol";
		((Control)tpFontsAndColors).Controls.Add((Control)(object)pnFontsColors);
		tpFontsAndColors.Location = new Point(4, 22);
		((Control)tpFontsAndColors).Name = "tpFontsAndColors";
		((Control)tpFontsAndColors).Size = new Size(399, 364);
		tpFontsAndColors.TabIndex = 1;
		((Control)tpFontsAndColors).Text = "Fonts&&Colors";
		((Control)pnFontsColors).BackColor = Color.White;
		((Control)pnFontsColors).Controls.Add((Control)(object)cbForeColor);
		((Control)pnFontsColors).Controls.Add((Control)(object)cbBackColor);
		((Control)pnFontsColors).Controls.Add((Control)(object)gbColorThemes);
		((Control)pnFontsColors).Controls.Add((Control)(object)tbFontSize);
		((Control)pnFontsColors).Controls.Add((Control)(object)laDisplayItems);
		((Control)pnFontsColors).Controls.Add((Control)(object)pnSampleText);
		((Control)pnFontsColors).Controls.Add((Control)(object)laSample);
		((Control)pnFontsColors).Controls.Add((Control)(object)laSize);
		((Control)pnFontsColors).Controls.Add((Control)(object)laFont);
		((Control)pnFontsColors).Controls.Add((Control)(object)cbFontName);
		((Control)pnFontsColors).Controls.Add((Control)(object)laDescription);
		((Control)pnFontsColors).Controls.Add((Control)(object)tbDescription);
		((Control)pnFontsColors).Controls.Add((Control)(object)laBackColor);
		((Control)pnFontsColors).Controls.Add((Control)(object)laForeColor);
		((Control)pnFontsColors).Controls.Add((Control)(object)lbStyles);
		((Control)pnFontsColors).Controls.Add((Control)(object)gbFontAttributes);
		((Control)pnFontsColors).Dock = (DockStyle)5;
		((Control)pnFontsColors).Location = new Point(0, 0);
		((Control)pnFontsColors).Name = "pnFontsColors";
		((Control)pnFontsColors).Size = new Size(399, 364);
		((Control)pnFontsColors).TabIndex = 7;
		((Control)pnFontsColors).Visible = false;
		((ComboBox)cbForeColor).DrawMode = (DrawMode)1;
		((Control)cbForeColor).Location = new Point(152, 216);
		((Control)cbForeColor).Name = "cbForeColor";
		cbForeColor.SelectedColor = Color.Empty;
		((Control)cbForeColor).Size = new Size(120, 21);
		((Control)cbForeColor).TabIndex = 17;
		((ComboBox)cbForeColor).SelectedIndexChanged += cbForeColor_SelectedIndexChanged;
		((ComboBox)cbBackColor).DrawMode = (DrawMode)1;
		((Control)cbBackColor).Location = new Point(152, 264);
		((Control)cbBackColor).Name = "cbBackColor";
		cbBackColor.SelectedColor = Color.Empty;
		((Control)cbBackColor).Size = new Size(120, 21);
		((Control)cbBackColor).TabIndex = 18;
		((ComboBox)cbBackColor).SelectedIndexChanged += cbBackColor_SelectedIndexChanged;
		((Control)gbColorThemes).Controls.Add((Control)(object)btDeleteColorTheme);
		((Control)gbColorThemes).Controls.Add((Control)(object)btAddColorTheme);
		((Control)gbColorThemes).Controls.Add((Control)(object)cbColorThemes);
		((Control)gbColorThemes).Location = new Point(8, 8);
		((Control)gbColorThemes).Name = "gbColorThemes";
		((Control)gbColorThemes).Size = new Size(392, 85);
		((Control)gbColorThemes).TabIndex = 20;
		gbColorThemes.TabStop = false;
		((Control)gbColorThemes).Text = "Color Themes";
		((Control)btDeleteColorTheme).Location = new Point(237, 56);
		((Control)btDeleteColorTheme).Name = "btDeleteColorTheme";
		((Control)btDeleteColorTheme).Size = new Size(147, 23);
		((Control)btDeleteColorTheme).TabIndex = 2;
		((Control)btDeleteColorTheme).Text = "&Delete Color Theme";
		((Control)btDeleteColorTheme).Click += btDeleteColorTheme_Click;
		((Control)btAddColorTheme).Location = new Point(237, 19);
		((Control)btAddColorTheme).Name = "btAddColorTheme";
		((Control)btAddColorTheme).Size = new Size(147, 23);
		((Control)btAddColorTheme).TabIndex = 1;
		((Control)btAddColorTheme).Text = "&Add Color Theme";
		((Control)btAddColorTheme).Click += btAddColorTheme_Click;
		((Control)cbColorThemes).Location = new Point(10, 19);
		((Control)cbColorThemes).Name = "cbColorThemes";
		((Control)cbColorThemes).Size = new Size(215, 21);
		((Control)cbColorThemes).TabIndex = 0;
		cbColorThemes.SelectedIndexChanged += cbColorThemes_SelectedIndexChanged;
		((Control)cbColorThemes).Leave += cbColorThemes_Leave;
		((Control)tbFontSize).Location = new Point(152, 112);
		((Control)tbFontSize).Name = "tbFontSize";
		((Control)tbFontSize).Size = new Size(32, 20);
		((Control)tbFontSize).TabIndex = 19;
		((Control)tbFontSize).Text = "1";
		((Control)tbFontSize).Leave += tbFontSize_Leave;
		((Control)laDisplayItems).AutoSize = true;
		laDisplayItems.FlatStyle = (FlatStyle)3;
		((Control)laDisplayItems).Location = new Point(8, 144);
		((Control)laDisplayItems).Name = "laDisplayItems";
		((Control)laDisplayItems).Size = new Size(71, 13);
		((Control)laDisplayItems).TabIndex = 4;
		((Control)laDisplayItems).Text = "&Display items:";
		pnSampleText.BorderStyle = (BorderStyle)1;
		((Control)pnSampleText).Controls.Add((Control)(object)laSampleText);
		((Control)pnSampleText).Location = new Point(8, 305);
		((Control)pnSampleText).Name = "pnSampleText";
		((Control)pnSampleText).Size = new Size(384, 48);
		((Control)pnSampleText).TabIndex = 14;
		((Control)laSampleText).AutoSize = true;
		((Control)laSampleText).Location = new Point(160, 16);
		((Control)laSampleText).Name = "laSampleText";
		((Control)laSampleText).Size = new Size(57, 13);
		((Control)laSampleText).TabIndex = 0;
		((Control)laSampleText).Text = "AaBbYyZz";
		((Control)laSample).AutoSize = true;
		laSample.FlatStyle = (FlatStyle)3;
		((Control)laSample).Location = new Point(8, 289);
		((Control)laSample).Name = "laSample";
		((Control)laSample).Size = new Size(45, 13);
		((Control)laSample).TabIndex = 13;
		((Control)laSample).Text = "Sample:";
		((Control)laSize).AutoSize = true;
		laSize.FlatStyle = (FlatStyle)3;
		((Control)laSize).Location = new Point(152, 96);
		((Control)laSize).Name = "laSize";
		((Control)laSize).Size = new Size(30, 13);
		((Control)laSize).TabIndex = 2;
		((Control)laSize).Text = "&Size:";
		((Control)laFont).AutoSize = true;
		laFont.FlatStyle = (FlatStyle)3;
		((Control)laFont).Location = new Point(8, 96);
		((Control)laFont).Name = "laFont";
		((Control)laFont).Size = new Size(31, 13);
		((Control)laFont).TabIndex = 0;
		((Control)laFont).Text = "Font:";
		((Control)cbFontName).Location = new Point(8, 112);
		((Control)cbFontName).Name = "cbFontName";
		((Control)cbFontName).Size = new Size(121, 21);
		((Control)cbFontName).TabIndex = 1;
		cbFontName.SelectedIndexChanged += cbFontName_SelectedIndexChanged;
		((Control)laDescription).AutoSize = true;
		laDescription.FlatStyle = (FlatStyle)3;
		((Control)laDescription).Location = new Point(152, 144);
		((Control)laDescription).Name = "laDescription";
		((Control)laDescription).Size = new Size(63, 13);
		((Control)laDescription).TabIndex = 6;
		((Control)laDescription).Text = "Description:";
		((Control)tbDescription).Location = new Point(152, 160);
		((Control)tbDescription).Name = "tbDescription";
		((Control)tbDescription).Size = new Size(240, 20);
		((Control)tbDescription).TabIndex = 7;
		((Control)laBackColor).AutoSize = true;
		laBackColor.FlatStyle = (FlatStyle)3;
		((Control)laBackColor).Location = new Point(152, 240);
		((Control)laBackColor).Name = "laBackColor";
		((Control)laBackColor).Size = new Size(62, 13);
		((Control)laBackColor).TabIndex = 10;
		((Control)laBackColor).Text = "Back Color:";
		((Control)laForeColor).AutoSize = true;
		laForeColor.FlatStyle = (FlatStyle)3;
		((Control)laForeColor).Location = new Point(152, 192);
		((Control)laForeColor).Name = "laForeColor";
		((Control)laForeColor).Size = new Size(58, 13);
		((Control)laForeColor).TabIndex = 8;
		((Control)laForeColor).Text = "Fore Color:";
		((Control)lbStyles).Location = new Point(8, 160);
		((Control)lbStyles).Name = "lbStyles";
		((Control)lbStyles).Size = new Size(120, 121);
		((Control)lbStyles).TabIndex = 5;
		((Control)gbFontAttributes).Controls.Add((Control)(object)chbUnderline);
		((Control)gbFontAttributes).Controls.Add((Control)(object)chbItalic);
		((Control)gbFontAttributes).Controls.Add((Control)(object)chbBold);
		gbFontAttributes.FlatStyle = (FlatStyle)3;
		((Control)gbFontAttributes).Location = new Point(280, 192);
		((Control)gbFontAttributes).Name = "gbFontAttributes";
		((Control)gbFontAttributes).Size = new Size(112, 92);
		((Control)gbFontAttributes).TabIndex = 12;
		gbFontAttributes.TabStop = false;
		((Control)gbFontAttributes).Text = "Attributes:";
		((ButtonBase)chbUnderline).FlatStyle = (FlatStyle)3;
		((Control)chbUnderline).Location = new Point(8, 64);
		((Control)chbUnderline).Name = "chbUnderline";
		((Control)chbUnderline).Size = new Size(100, 24);
		((Control)chbUnderline).TabIndex = 2;
		((Control)chbUnderline).Text = "Underline";
		((ButtonBase)chbItalic).FlatStyle = (FlatStyle)3;
		((Control)chbItalic).Location = new Point(8, 40);
		((Control)chbItalic).Name = "chbItalic";
		((Control)chbItalic).Size = new Size(100, 24);
		((Control)chbItalic).TabIndex = 1;
		((Control)chbItalic).Text = "Italic";
		((ButtonBase)chbBold).FlatStyle = (FlatStyle)3;
		((Control)chbBold).Location = new Point(8, 16);
		((Control)chbBold).Name = "chbBold";
		((Control)chbBold).Size = new Size(100, 24);
		((Control)chbBold).TabIndex = 0;
		((Control)chbBold).Text = "Bold";
		((Control)tpKeyboard).BackColor = Color.White;
		((Control)tpKeyboard).Controls.Add((Control)(object)pnKeyboard);
		tpKeyboard.Location = new Point(4, 22);
		((Control)tpKeyboard).Name = "tpKeyboard";
		((Control)tpKeyboard).Size = new Size(399, 364);
		tpKeyboard.TabIndex = 3;
		((Control)tpKeyboard).Text = "Keyboard";
		((Control)pnKeyboard).Controls.Add((Control)(object)cbShortcutCurrentlyUsed);
		((Control)pnKeyboard).Controls.Add((Control)(object)laShortcutCurrentlyUsed);
		((Control)pnKeyboard).Controls.Add((Control)(object)btAssignShortcuts);
		((Control)pnKeyboard).Controls.Add((Control)(object)tbShortcutKeys);
		((Control)pnKeyboard).Controls.Add((Control)(object)cbUseShortcutIn);
		((Control)pnKeyboard).Controls.Add((Control)(object)laShortcutKeys);
		((Control)pnKeyboard).Controls.Add((Control)(object)laUseShortcut);
		((Control)pnKeyboard).Controls.Add((Control)(object)btRemoveShortcut);
		((Control)pnKeyboard).Controls.Add((Control)(object)cbShortcuts);
		((Control)pnKeyboard).Controls.Add((Control)(object)laShortcuts);
		((Control)pnKeyboard).Controls.Add((Control)(object)lbEventHandlers);
		((Control)pnKeyboard).Controls.Add((Control)(object)tbShowCommands);
		((Control)pnKeyboard).Controls.Add((Control)(object)laShowCommands);
		((Control)pnKeyboard).Controls.Add((Control)(object)btDeleteScheme);
		((Control)pnKeyboard).Controls.Add((Control)(object)btSaveSchemeAs);
		((Control)pnKeyboard).Controls.Add((Control)(object)cbKeyboardSchemes);
		((Control)pnKeyboard).Controls.Add((Control)(object)laKeyboardMappingScheme);
		((Control)pnKeyboard).Dock = (DockStyle)1;
		((Control)pnKeyboard).Location = new Point(0, 0);
		((Control)pnKeyboard).Name = "pnKeyboard";
		((Control)pnKeyboard).Size = new Size(399, 296);
		((Control)pnKeyboard).TabIndex = 0;
		((Control)pnKeyboard).Visible = false;
		((Control)cbShortcutCurrentlyUsed).Enabled = false;
		((Control)cbShortcutCurrentlyUsed).Location = new Point(8, 264);
		((Control)cbShortcutCurrentlyUsed).Name = "cbShortcutCurrentlyUsed";
		((Control)cbShortcutCurrentlyUsed).Size = new Size(384, 21);
		((Control)cbShortcutCurrentlyUsed).TabIndex = 16;
		((Control)laShortcutCurrentlyUsed).AutoSize = true;
		((Control)laShortcutCurrentlyUsed).Location = new Point(8, 248);
		((Control)laShortcutCurrentlyUsed).Name = "laShortcutCurrentlyUsed";
		((Control)laShortcutCurrentlyUsed).Size = new Size(133, 13);
		((Control)laShortcutCurrentlyUsed).TabIndex = 15;
		((Control)laShortcutCurrentlyUsed).Text = "Shortcut c&urrently used by:";
		((Control)btAssignShortcuts).Enabled = false;
		((Control)btAssignShortcuts).Location = new Point(320, 224);
		((Control)btAssignShortcuts).Name = "btAssignShortcuts";
		((Control)btAssignShortcuts).Size = new Size(73, 23);
		((Control)btAssignShortcuts).TabIndex = 14;
		((Control)btAssignShortcuts).Text = "&Assign";
		((Control)tbShortcutKeys).Enabled = false;
		((Control)tbShortcutKeys).Location = new Point(144, 224);
		((Control)tbShortcutKeys).Name = "tbShortcutKeys";
		((Control)tbShortcutKeys).Size = new Size(168, 20);
		((Control)tbShortcutKeys).TabIndex = 13;
		((Control)cbUseShortcutIn).Enabled = false;
		((Control)cbUseShortcutIn).Location = new Point(8, 224);
		((Control)cbUseShortcutIn).Name = "cbUseShortcutIn";
		((Control)cbUseShortcutIn).Size = new Size(128, 21);
		((Control)cbUseShortcutIn).TabIndex = 12;
		((Control)laShortcutKeys).Location = new Point(144, 192);
		((Control)laShortcutKeys).Name = "laShortcutKeys";
		((Control)laShortcutKeys).Size = new Size(120, 28);
		((Control)laShortcutKeys).TabIndex = 11;
		((Control)laShortcutKeys).Text = "&Press shortcut key(s):";
		laShortcutKeys.TextAlign = (ContentAlignment)16;
		((Control)laUseShortcut).Location = new Point(8, 192);
		((Control)laUseShortcut).Name = "laUseShortcut";
		((Control)laUseShortcut).Size = new Size(128, 28);
		((Control)laUseShortcut).TabIndex = 10;
		((Control)laUseShortcut).Text = "Use &new shortcut in:";
		laUseShortcut.TextAlign = (ContentAlignment)16;
		((Control)btRemoveShortcut).Enabled = false;
		((Control)btRemoveShortcut).Location = new Point(320, 168);
		((Control)btRemoveShortcut).Name = "btRemoveShortcut";
		((Control)btRemoveShortcut).Size = new Size(73, 23);
		((Control)btRemoveShortcut).TabIndex = 9;
		((Control)btRemoveShortcut).Text = "&Remove";
		((Control)cbShortcuts).Location = new Point(8, 168);
		((Control)cbShortcuts).Name = "cbShortcuts";
		((Control)cbShortcuts).Size = new Size(304, 21);
		((Control)cbShortcuts).TabIndex = 8;
		((Control)laShortcuts).AutoSize = true;
		((Control)laShortcuts).Location = new Point(8, 152);
		((Control)laShortcuts).Name = "laShortcuts";
		((Control)laShortcuts).Size = new Size(168, 13);
		((Control)laShortcuts).TabIndex = 7;
		((Control)laShortcuts).Text = "Shor&tcut(s) for selected command:";
		((Control)lbEventHandlers).Location = new Point(8, 88);
		((Control)lbEventHandlers).Name = "lbEventHandlers";
		((Control)lbEventHandlers).Size = new Size(384, 56);
		((Control)lbEventHandlers).TabIndex = 6;
		lbEventHandlers.SelectedIndexChanged += lbEventHandlers_SelectedIndexChanged;
		((Control)tbShowCommands).Location = new Point(8, 64);
		((Control)tbShowCommands).Name = "tbShowCommands";
		((Control)tbShowCommands).Size = new Size(384, 20);
		((Control)tbShowCommands).TabIndex = 5;
		((Control)tbShowCommands).TextChanged += tbShowCommands_TextChanged;
		((Control)laShowCommands).AutoSize = true;
		((Control)laShowCommands).Location = new Point(8, 48);
		((Control)laShowCommands).Name = "laShowCommands";
		((Control)laShowCommands).Size = new Size(143, 13);
		((Control)laShowCommands).TabIndex = 4;
		((Control)laShowCommands).Text = "Show &commands containing:";
		((Control)btDeleteScheme).Enabled = false;
		((Control)btDeleteScheme).Location = new Point(320, 24);
		((Control)btDeleteScheme).Name = "btDeleteScheme";
		((Control)btDeleteScheme).Size = new Size(75, 23);
		((Control)btDeleteScheme).TabIndex = 3;
		((Control)btDeleteScheme).Text = "&Delete";
		((Control)btSaveSchemeAs).Enabled = false;
		((Control)btSaveSchemeAs).Location = new Point(208, 24);
		((Control)btSaveSchemeAs).Name = "btSaveSchemeAs";
		((Control)btSaveSchemeAs).Size = new Size(104, 23);
		((Control)btSaveSchemeAs).TabIndex = 2;
		((Control)btSaveSchemeAs).Text = "&Save As...";
		((Control)cbKeyboardSchemes).Location = new Point(8, 24);
		((Control)cbKeyboardSchemes).Name = "cbKeyboardSchemes";
		((Control)cbKeyboardSchemes).Size = new Size(192, 21);
		((Control)cbKeyboardSchemes).TabIndex = 1;
		((Control)laKeyboardMappingScheme).AutoSize = true;
		((Control)laKeyboardMappingScheme).Location = new Point(8, 8);
		((Control)laKeyboardMappingScheme).Name = "laKeyboardMappingScheme";
		((Control)laKeyboardMappingScheme).Size = new Size(138, 13);
		((Control)laKeyboardMappingScheme).TabIndex = 0;
		((Control)laKeyboardMappingScheme).Text = "Keyboard &mapping scheme:";
		((Control)pnTree).Anchor = (AnchorStyles)7;
		((Control)pnTree).Controls.Add((Control)(object)tvProperties);
		((Control)pnTree).Location = new Point(0, 0);
		((Control)pnTree).Name = "pnTree";
		((Control)pnTree).Size = new Size(136, 390);
		((Control)pnTree).TabIndex = 0;
		((Control)tvProperties).Anchor = (AnchorStyles)15;
		tvProperties.ImageIndex = 3;
		tvProperties.ImageList = imageList1;
		((Control)tvProperties).Location = new Point(0, 0);
		((Control)tvProperties).Name = "tvProperties";
		val.Name = "";
		val.Text = "General";
		val2.Name = "";
		val2.Text = "Fonts and Colors";
		val3.Name = "";
		val3.Text = "Additional";
		val4.Name = "";
		val4.Text = "Keyboard";
		val5.ImageIndex = 1;
		val5.Name = "";
		val5.SelectedImageIndex = 0;
		val5.Text = "Options";
		tvProperties.Nodes.AddRange((TreeNode[])(object)new TreeNode[1] { val5 });
		tvProperties.Scrollable = false;
		tvProperties.SelectedImageIndex = 2;
		tvProperties.ShowLines = false;
		tvProperties.ShowPlusMinus = false;
		tvProperties.ShowRootLines = false;
		((Control)tvProperties).Size = new Size(124, 390);
		((Control)tvProperties).TabIndex = 0;
		tvProperties.AfterSelect += new TreeViewEventHandler(tvProperties_AfterSelect);
		imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
		imageList1.TransparentColor = Color.Red;
		imageList1.Images.SetKeyName(0, "");
		imageList1.Images.SetKeyName(1, "");
		imageList1.Images.SetKeyName(2, "");
		imageList1.Images.SetKeyName(3, "");
		((Form)this).AcceptButton = (IButtonControl)(object)btOK;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(566, 444);
		((Control)this).Controls.Add((Control)(object)pnMain);
		((Control)this).Controls.Add((Control)(object)pnButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "DlgSyntaxSettings";
		((Control)this).Padding = new Padding(12, 12, 12, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Syntax Settings";
		((Form)this).Activated += DlgSyntaxSettings_Activated;
		((Form)this).Load += DlgSyntaxSettings_Load;
		((Control)pnButtons).ResumeLayout(false);
		((Control)pnMain).ResumeLayout(false);
		((Control)pnManage).ResumeLayout(false);
		((Control)tcMain).ResumeLayout(false);
		((Control)tpGeneral).ResumeLayout(false);
		((Control)pnGeneral).ResumeLayout(false);
		((Control)gbLineNumbers).ResumeLayout(false);
		((Control)gbGutterMargin).ResumeLayout(false);
		((Control)gbGutterMargin).PerformLayout();
		((Control)gbDocument).ResumeLayout(false);
		((Control)tpAdditional).ResumeLayout(false);
		((Control)pnAdditional).ResumeLayout(false);
		((Control)gbTabOptions).ResumeLayout(false);
		((Control)gbTabOptions).PerformLayout();
		((Control)gbOutlineOptions).ResumeLayout(false);
		((Control)gbNavigateOptions).ResumeLayout(false);
		((Control)tpFontsAndColors).ResumeLayout(false);
		((Control)pnFontsColors).ResumeLayout(false);
		((Control)pnFontsColors).PerformLayout();
		((Control)gbColorThemes).ResumeLayout(false);
		((Control)pnSampleText).ResumeLayout(false);
		((Control)pnSampleText).PerformLayout();
		((Control)gbFontAttributes).ResumeLayout(false);
		((Control)tpKeyboard).ResumeLayout(false);
		((Control)pnKeyboard).ResumeLayout(false);
		((Control)pnKeyboard).PerformLayout();
		((Control)pnTree).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private string[] GetFonts()
	{
		FontFamily[] families = FontFamily.Families;
		string[] array = new string[families.Length];
		for (int i = 0; i < families.Length; i++)
		{
			array[i] = families[i].Name;
		}
		return array;
	}

	private int GetInt(string s, int DefaultValue)
	{
		try
		{
			return int.Parse(s);
		}
		catch
		{
			return DefaultValue;
		}
	}

	private void FillEventHandlers()
	{
		UpdateEventHandlers();
	}

	private void UpdateEventHandlers()
	{
		lbEventHandlers.BeginUpdate();
		try
		{
			lbEventHandlers.Sorted = false;
			lbEventHandlers.Items.Clear();
			KeyListData[] eventData = syntaxSettings.EventData;
			foreach (KeyListData keyListData in eventData)
			{
				string text = ((keyListData.Param != null) ? $"{keyListData.EventName}{keyListData.Param.ToString()}" : keyListData.EventName);
				if (!(text != string.Empty))
				{
					continue;
				}
				if (((Control)tbShowCommands).Text != string.Empty)
				{
					if (text.IndexOf(((Control)tbShowCommands).Text) >= 0 && lbEventHandlers.Items.IndexOf((object)text) < 0)
					{
						lbEventHandlers.Items.Add((object)text);
					}
				}
				else if (lbEventHandlers.Items.IndexOf((object)text) < 0)
				{
					lbEventHandlers.Items.Add((object)text);
				}
			}
			lbEventHandlers.Sorted = true;
			if (lbEventHandlers.Items.Count > 0)
			{
				((ListControl)lbEventHandlers).SelectedIndex = 0;
			}
		}
		finally
		{
			lbEventHandlers.EndUpdate();
		}
	}

	private void UpdateShortcut(int index)
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		((Control)cbShortcuts).Text = string.Empty;
		cbShortcuts.Items.Clear();
		string text = lbEventHandlers.Items[index].ToString();
		KeyListData[] eventData = syntaxSettings.EventData;
		foreach (KeyListData keyListData in eventData)
		{
			if (keyListData.EventName != string.Empty && text.StartsWith(keyListData.EventName))
			{
				string text2 = ((text.Length > keyListData.EventName.Length) ? text.Remove(0, keyListData.EventName.Length) : string.Empty);
				if (keyListData.Param == null || keyListData.Param.ToString() == text2)
				{
					string text3 = ApplyKeyState(keyListData);
					string text4 = ((text3 != string.Empty) ? $"{text3}, {KeyDataToString(keyListData.KeyData)}" : KeyDataToString(keyListData.KeyData));
					cbShortcuts.Items.Add((object)text4);
				}
			}
		}
		if (cbShortcuts.Items.Count > 0)
		{
			((ListControl)cbShortcuts).SelectedIndex = 0;
		}
	}

	private string KeyDataToString(Keys keyData)
	{
		string text = ((object)(Keys)(ref keyData)).ToString();
		string[] array = text.Split(new char[1] { ',' });
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].ToLower(CultureInfo.InvariantCulture).IndexOf(sControl) >= 0)
			{
				flag = true;
			}
			else if (array[i].ToLower(CultureInfo.InvariantCulture).IndexOf(sAlt) >= 0)
			{
				flag2 = true;
			}
			else if (array[i].ToLower(CultureInfo.InvariantCulture).IndexOf(sShift) >= 0)
			{
				flag3 = true;
			}
			else
			{
				text = ((text != string.Empty) ? $"{text} + {array[i]}" : array[i]);
			}
		}
		if (flag2)
		{
			text = ((text != string.Empty) ? $"{sAlt.ToUpper(CultureInfo.InvariantCulture)} + {text}" : sAlt.ToUpper(CultureInfo.InvariantCulture));
		}
		if (flag3)
		{
			text = ((text != string.Empty) ? $"{sShift.ToUpper(CultureInfo.InvariantCulture)} + {text}" : sShift.ToUpper(CultureInfo.InvariantCulture));
		}
		if (flag)
		{
			text = ((text != string.Empty) ? $"{sCtrl} + {text}" : sCtrl);
		}
		return text;
	}

	private string ApplyKeyState(KeyListData key)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		string result = string.Empty;
		if (key.State > 0)
		{
			KeyListData[] eventData = syntaxSettings.EventData;
			KeyListData[] array = eventData;
			foreach (KeyListData keyListData in array)
			{
				if (keyListData.LeaveState == key.State && keyListData.State == 0)
				{
					result = KeyDataToString(keyListData.KeyData);
					break;
				}
			}
		}
		return result;
	}

	private void ControlsFromSettings()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected I4, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected I4, but got Unknown
		FillStyles();
		FillColorThemes();
		ObjectCollection items = cbFontName.Items;
		object[] fonts = GetFonts();
		items.AddRange(fonts);
		cbKeyboardSchemes.Items.Add((object)"Default Settings");
		((ListControl)cbKeyboardSchemes).SelectedIndex = 0;
		cbUseShortcutIn.Items.Add((object)"Syntax Editor");
		((ListControl)cbUseShortcutIn).SelectedIndex = 0;
		FillEventHandlers();
		chbDragAndDrop.Checked = (syntaxSettings.SelectionOptions & SelectionOptions.DisableDragging) == 0;
		RichTextBoxScrollBars scrollBars = syntaxSettings.ScrollBars;
		switch ((int)scrollBars)
		{
		default:
			switch (scrollBars - 17)
			{
			case 2:
				chbVertScrollBar.Checked = true;
				chbHorzScrollBar.Checked = true;
				chbForced.Checked = true;
				goto end_IL_0096;
			case 0:
				break;
			case 1:
				goto IL_013a;
			default:
				goto end_IL_0096;
			}
			goto case 1;
		case 3:
			chbVertScrollBar.Checked = true;
			chbHorzScrollBar.Checked = true;
			chbForced.Checked = false;
			break;
		case 1:
			chbVertScrollBar.Checked = false;
			chbHorzScrollBar.Checked = true;
			chbForced.Checked = false;
			break;
		case 2:
			goto IL_013a;
		case 0:
			{
				chbVertScrollBar.Checked = false;
				chbHorzScrollBar.Checked = false;
				chbForced.Checked = false;
				break;
			}
			IL_013a:
			chbVertScrollBar.Checked = true;
			chbHorzScrollBar.Checked = false;
			chbForced.Checked = false;
			break;
			end_IL_0096:
			break;
		}
		chbShowMargin.Checked = syntaxSettings.ShowMargin;
		chbWordWrap.Checked = syntaxSettings.WordWrap;
		chbLineNumbers.Checked = (syntaxSettings.GutterOptions & GutterOptions.PaintLineNumbers) != 0;
		chbLineNumbersOnGutter.Checked = (syntaxSettings.GutterOptions & GutterOptions.PaintLinesOnGutter) != 0;
		chbShowGutter.Checked = syntaxSettings.ShowGutter;
		((Control)tbGutterWidth).Text = syntaxSettings.GutterWidth.ToString();
		((Control)tbMarginPosition).Text = syntaxSettings.MarginPos.ToString();
		chbBeyondEol.Checked = (syntaxSettings.NavigateOptions & NavigateOptions.BeyondEol) != 0;
		chbBeyondEof.Checked = (syntaxSettings.NavigateOptions & NavigateOptions.BeyondEof) != 0;
		chbMoveOnRightButton.Checked = (syntaxSettings.NavigateOptions & NavigateOptions.MoveOnRightButton) != 0;
		chbHighlightUrls.Checked = syntaxSettings.HighlightUrls;
		chbAllowOutlining.Checked = syntaxSettings.AllowOutlining;
		chbShowHints.Checked = (syntaxSettings.OutlineOptions & OutlineOptions.ShowHints) != 0;
		rbInsertSpaces.Checked = syntaxSettings.UseSpaces;
		rbKeepTabs.Checked = !syntaxSettings.UseSpaces;
		chbWhiteSpace.Checked = syntaxSettings.WhiteSpaceVisible;
		chbLineModificator.Checked = (((syntaxSettings.GutterOptions & GutterOptions.PaintLineModificators) != 0) ? true : false);
		chbLineSeparator.Checked = (((syntaxSettings.SeparatorOptions & SeparatorOptions.SeparateLines) != 0) ? true : false);
		string[] array = new string[syntaxSettings.TabStops.Length];
		for (int i = 0; i < syntaxSettings.TabStops.Length; i++)
		{
			array[i] = syntaxSettings.TabStops[i].ToString();
		}
		((Control)tbTabStops).Text = string.Join(",", array);
		UpdateFontControls();
	}

	private void UpdateFontControls()
	{
		try
		{
			isFontControlsUpdating = true;
			((ListControl)cbFontName).SelectedIndex = cbFontName.Items.IndexOf((object)syntaxSettings.Font.Name);
			((Control)tbFontSize).Text = syntaxSettings.Font.Size.ToString();
		}
		finally
		{
			isFontControlsUpdating = false;
		}
	}

	private void SettingsFromControl()
	{
		bool @checked = chbVertScrollBar.Checked;
		bool checked2 = chbHorzScrollBar.Checked;
		bool checked3 = chbForced.Checked;
		if (checked2)
		{
			if (@checked)
			{
				syntaxSettings.ScrollBars = (RichTextBoxScrollBars)(checked3 ? 19 : 3);
			}
			else
			{
				syntaxSettings.ScrollBars = (RichTextBoxScrollBars)((!checked3) ? 1 : 17);
			}
		}
		else if (@checked)
		{
			syntaxSettings.ScrollBars = (RichTextBoxScrollBars)(checked3 ? 18 : 2);
		}
		else
		{
			syntaxSettings.ScrollBars = (RichTextBoxScrollBars)0;
		}
		if (chbDragAndDrop.Checked)
		{
			syntaxSettings.SelectionOptions &= ~SelectionOptions.DisableDragging;
		}
		else
		{
			syntaxSettings.SelectionOptions |= SelectionOptions.DisableDragging;
		}
		syntaxSettings.ShowMargin = chbShowMargin.Checked;
		syntaxSettings.WordWrap = chbWordWrap.Checked;
		syntaxSettings.WhiteSpaceVisible = chbWhiteSpace.Checked;
		syntaxSettings.ShowGutter = chbShowGutter.Checked;
		syntaxSettings.GutterWidth = GetInt(((Control)tbGutterWidth).Text, EditConsts.DefaultGutterWidth);
		syntaxSettings.MarginPos = GetInt(((Control)tbMarginPosition).Text, EditConsts.DefaultMarginPosition);
		if (chbLineNumbers.Checked)
		{
			syntaxSettings.GutterOptions |= GutterOptions.PaintLineNumbers;
		}
		else
		{
			syntaxSettings.GutterOptions &= ~GutterOptions.PaintLineNumbers;
		}
		if (chbLineNumbersOnGutter.Checked)
		{
			syntaxSettings.GutterOptions |= GutterOptions.PaintLinesOnGutter;
		}
		else
		{
			syntaxSettings.GutterOptions &= ~GutterOptions.PaintLinesOnGutter;
		}
		if (chbLineModificator.Checked)
		{
			syntaxSettings.GutterOptions |= GutterOptions.PaintLineModificators;
		}
		else
		{
			syntaxSettings.GutterOptions &= ~GutterOptions.PaintLineModificators;
		}
		if (chbLineSeparator.Checked)
		{
			syntaxSettings.SeparatorOptions |= SeparatorOptions.SeparateLines;
		}
		else
		{
			syntaxSettings.SeparatorOptions &= ~SeparatorOptions.SeparateLines;
		}
		if (chbBeyondEol.Checked)
		{
			syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEol;
		}
		else
		{
			syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEol;
		}
		if (chbBeyondEof.Checked)
		{
			syntaxSettings.NavigateOptions |= NavigateOptions.BeyondEof;
		}
		else
		{
			syntaxSettings.NavigateOptions &= ~NavigateOptions.BeyondEof;
		}
		if (chbMoveOnRightButton.Checked)
		{
			syntaxSettings.NavigateOptions |= NavigateOptions.MoveOnRightButton;
		}
		else
		{
			syntaxSettings.NavigateOptions &= ~NavigateOptions.MoveOnRightButton;
		}
		if (chbShowHints.Checked)
		{
			syntaxSettings.OutlineOptions |= OutlineOptions.ShowHints;
		}
		else
		{
			syntaxSettings.OutlineOptions &= ~OutlineOptions.ShowHints;
		}
		syntaxSettings.HighlightUrls = chbHighlightUrls.Checked;
		syntaxSettings.AllowOutlining = chbAllowOutlining.Checked;
		syntaxSettings.UseSpaces = rbInsertSpaces.Checked;
		string[] array = ((Control)tbTabStops).Text.Split(new char[1] { ',' });
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = GetInt(array[i], EditConsts.DefaultTabStop);
		}
		syntaxSettings.TabStops = array2;
	}

	private void FillStyles()
	{
		for (int i = 0; i < syntaxSettings.LexStyles.Length; i++)
		{
			lbStyles.Items.Add((object)syntaxSettings.LexStyles[i].Name);
		}
	}

	private void FillColorThemes()
	{
		cbColorThemes.BeginUpdate();
		cbColorThemes.Items.Clear();
		IColorTheme[] themes = SyntaxSettings.ColorThemes.Themes;
		for (int i = 0; i < themes.Length; i++)
		{
			ColorTheme colorTheme = (ColorTheme)themes[i];
			cbColorThemes.Items.Add((object)colorTheme.Name);
		}
		((ListControl)cbColorThemes).SelectedIndex = SyntaxSettings.ColorThemes.ActiveThemeIndex;
		cbColorThemes.EndUpdate();
	}

	private void tvProperties_AfterSelect(object sender, TreeViewEventArgs e)
	{
		Panel currentPanel = GetCurrentPanel();
		if (currentPanel != null)
		{
			((Control)currentPanel).Visible = true;
			((Control)currentPanel).BringToFront();
			UpdateImages();
		}
	}

	private void UpdateImages()
	{
		if (tvProperties.SelectedNode == rootNode)
		{
			generalNode.ImageIndex = 2;
		}
		else
		{
			generalNode.ImageIndex = 3;
		}
	}

	private int GetNodeLevel(TreeNode node)
	{
		int num = 0;
		if (node != null)
		{
			while (node.Parent != null)
			{
				node = node.Parent;
				num++;
			}
		}
		return num;
	}

	private Panel GetCurrentPanel()
	{
		Panel val = null;
		TreeNode selectedNode = tvProperties.SelectedNode;
		if (selectedNode != null)
		{
			switch (GetNodeLevel(selectedNode))
			{
			case 0:
				val = (rootNode.Nodes.Contains(generalNode) ? pnGeneral : null);
				break;
			case 1:
				val = ((!((object)selectedNode).Equals((object?)generalNode)) ? ((!((object)selectedNode).Equals((object?)fontsNode)) ? ((!((object)selectedNode).Equals((object?)additionalNode)) ? ((!((object)selectedNode).Equals((object?)keyboardNode)) ? null : pnKeyboard) : pnAdditional) : pnFontsColors) : pnGeneral);
				break;
			}
		}
		if (val != null)
		{
			((Control)val).Location = new Point(0, 0);
			((Control)val).Size = ((Control)pnManage).Size;
			((Control)val).Dock = (DockStyle)5;
		}
		return val;
	}

	private void DlgSyntaxSettings_Activated(object sender, EventArgs e)
	{
		((Control)tvProperties).Focus();
	}

	private void DlgSyntaxSettings_Load(object sender, EventArgs e)
	{
		LoadFromResource();
		ControlsFromSettings();
		lbStyles.SelectedIndexChanged += OnStyleSelected;
		((ListControl)lbStyles).SelectedIndex = 0;
		chbBold.CheckedChanged += FontStyleChange;
		chbItalic.CheckedChanged += FontStyleChange;
		chbUnderline.CheckedChanged += FontStyleChange;
		((Control)tbDescription).TextChanged += DescriptionChanged;
		cbFontName.SelectedIndexChanged += FontNameChanged;
		((Control)tbFontSize).TextChanged += FontSizeChanged;
		((Control)pnManage).Controls.Add((Control)(object)pnGeneral);
		((Control)pnManage).Controls.Add((Control)(object)pnFontsColors);
		((Control)pnManage).Controls.Add((Control)(object)pnAdditional);
		((Control)pnManage).Controls.Add((Control)(object)pnKeyboard);
		tvProperties.Nodes[0].ImageIndex = 0;
		tvProperties.Nodes[0].Expand();
		tvProperties.SelectedNode = ((tvProperties.Nodes[0].Nodes.Count > 0) ? tvProperties.Nodes[0].Nodes[0] : tvProperties.Nodes[0]);
	}

	private void LoadFromResource()
	{
		((Control)this).Text = StringConsts.DlgSyntaxSettingsCaption;
		if (tvProperties.Nodes.Count > 0)
		{
			rootNode.Text = StringConsts.PropertiesOptionsCaption;
			if (tvProperties.Nodes[0].Nodes.Count >= 3)
			{
				generalNode.Text = StringConsts.PropertiesGeneralCaption;
				fontsNode.Text = StringConsts.PropertiesFontsColorsCaption;
				additionalNode.Text = StringConsts.PropertiesAdditionalCaption;
				keyboardNode.Text = StringConsts.PropertiesKeyboradCaption;
			}
		}
		((Control)tpGeneral).Text = StringConsts.GeneralCaption;
		((Control)tpFontsAndColors).Text = StringConsts.FontsAndColorsCaption;
		((Control)tpAdditional).Text = StringConsts.AdditionalCaption;
		((Control)gbDocument).Text = StringConsts.DocumentCaption;
		((Control)gbGutterMargin).Text = StringConsts.GutterMarginCaption;
		((Control)gbLineNumbers).Text = StringConsts.GroupBoxLineNumbersCaption_SyntaxSettingsDlg;
		((Control)chbWordWrap).Text = StringConsts.WordWrapCaption_SyntaxSettingsDlg;
		((Control)chbHighlightUrls).Text = StringConsts.HighlightUrlsCaption;
		((Control)chbDragAndDrop).Text = StringConsts.DragAndDropCaption;
		((Control)chbVertScrollBar).Text = StringConsts.VertScrollBarCaption;
		((Control)chbHorzScrollBar).Text = StringConsts.HorzScrollBarCaption;
		((Control)chbForced).Text = StringConsts.ForcedCaption;
		((Control)chbShowGutter).Text = StringConsts.ShowGutterCaption;
		((Control)chbShowMargin).Text = StringConsts.ShowMarginCaption;
		((Control)laGutterWidth).Text = StringConsts.GutterWidthCaption;
		((Control)laMarginPosition).Text = StringConsts.MarginPositionCaption;
		((Control)chbLineNumbers).Text = StringConsts.CheckBoxLineNumbersCaption_SyntaxSettingsDlg;
		((Control)chbLineNumbersOnGutter).Text = StringConsts.LineNumbersOnGutterCaption;
		((Control)laFont).Text = StringConsts.FontCaption;
		((Control)laSize).Text = StringConsts.SizeCaption;
		((Control)btAddColorTheme).Text = StringConsts.AddColorTheme;
		((Control)btDeleteColorTheme).Text = StringConsts.DeleteColorTheme;
		((Control)laDisplayItems).Text = StringConsts.DisplayItemsCaption;
		((Control)laDescription).Text = StringConsts.DescriptionCaption;
		((Control)laForeColor).Text = StringConsts.ForeColorCaption;
		((Control)laBackColor).Text = StringConsts.BackColorCaption;
		((Control)gbFontAttributes).Text = StringConsts.FontAttributesCaption;
		((Control)chbBold).Text = StringConsts.BoldCaption;
		((Control)chbItalic).Text = StringConsts.ItalicCaption;
		((Control)chbUnderline).Text = StringConsts.UnderlineCaption;
		((Control)laSample).Text = StringConsts.SampleCaption;
		((Control)laSampleText).Text = StringConsts.SampleTextCaption;
		((Control)gbNavigateOptions).Text = StringConsts.NavigateOptionsCaption;
		((Control)gbOutlineOptions).Text = StringConsts.OutlineOptionsCaption;
		((Control)gbTabOptions).Text = StringConsts.TabOptionsCaption;
		((Control)chbBeyondEol).Text = StringConsts.BeyondEolCaption;
		((Control)chbBeyondEof).Text = StringConsts.BeyondEofCaption;
		((Control)chbMoveOnRightButton).Text = StringConsts.MoveOnRightButtonCaption;
		((Control)chbAllowOutlining).Text = StringConsts.AllowOutliningCaption;
		((Control)chbShowHints).Text = StringConsts.ShowHintsCaption;
		((Control)laTabSizes).Text = StringConsts.TabSizesCaption;
		((Control)rbInsertSpaces).Text = StringConsts.InsertSpacesCaption;
		((Control)rbKeepTabs).Text = StringConsts.KeepTabsCaption;
		((Control)btOK).Text = StringConsts.OKCaption_SyntaxSettingsDlg;
		((Control)btCancel).Text = StringConsts.CancelCaption_SyntaxSettingsDlg;
		((Control)chbWhiteSpace).Text = StringConsts.WhiteSpaceCaption_SyntaxSettingsDlg;
		((Control)chbLineModificator).Text = StringConsts.LineModificatorCaption_SyntaxSettingsDlg;
		((Control)chbLineSeparator).Text = StringConsts.LineSeparatorCaption_SyntaxSettingsDlg;
		((Control)laKeyboardMappingScheme).Text = StringConsts.KeyboardMappingSchemeCaption;
		((Control)laShowCommands).Text = StringConsts.ShowCommandsCaption;
		((Control)laShortcuts).Text = StringConsts.ShortcutsCaption;
		((Control)laUseShortcut).Text = StringConsts.UseShortcutCaption;
		((Control)laShortcutKeys).Text = StringConsts.ShortcutKeysCaption;
		((Control)laShortcutCurrentlyUsed).Text = StringConsts.ShortcutCurrentlyUsedCaption;
		((Control)btSaveSchemeAs).Text = StringConsts.SaveSchemeAsCaption;
		((Control)btDeleteScheme).Text = StringConsts.DeleteSchemeCaption;
		((Control)btRemoveShortcut).Text = StringConsts.RemoveShortcutCaption;
		((Control)btAssignShortcuts).Text = StringConsts.AssignShortcutsCaption;
	}

	private void DescriptionChanged(object sender, EventArgs e)
	{
		if (!isControlUpdating)
		{
			curDesc = ((Control)tbDescription).Text;
			StyleFromControl();
		}
	}

	private void FontStyleChange(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		if (!isControlUpdating)
		{
			curFontStyle = (FontStyle)0;
			if (chbBold.Checked)
			{
				curFontStyle = (FontStyle)(curFontStyle | 1);
			}
			else
			{
				curFontStyle = (FontStyle)(curFontStyle & -2);
			}
			if (chbItalic.Checked)
			{
				curFontStyle = (FontStyle)(curFontStyle | 2);
			}
			else
			{
				curFontStyle = (FontStyle)(curFontStyle & -3);
			}
			if (chbUnderline.Checked)
			{
				curFontStyle = (FontStyle)(curFontStyle | 4);
			}
			else
			{
				curFontStyle = (FontStyle)(curFontStyle & -5);
			}
			StyleFromControl();
		}
	}

	private void StyleFromControl()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (lbStyles.SelectedItem != null)
		{
			ILexStyle selectedStyle = GetSelectedStyle();
			selectedStyle.ForeColor = curForeColor;
			selectedStyle.BackColor = curBkColor;
			selectedStyle.FontStyle = curFontStyle;
			selectedStyle.Desc = curDesc;
			UpdateStyleControls();
		}
	}

	private void OnStyleSelected(object sender, EventArgs e)
	{
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		((Control)tbDescription).Enabled = syntaxSettings.IsDescriptionEnabled(((ListControl)lbStyles).SelectedIndex);
		((Control)laDescription).Enabled = syntaxSettings.IsDescriptionEnabled(((ListControl)lbStyles).SelectedIndex);
		((Control)gbFontAttributes).Enabled = syntaxSettings.IsFontStyleEnabled(((ListControl)lbStyles).SelectedIndex);
		((Control)laBackColor).Enabled = syntaxSettings.IsBackColorEnabled(((ListControl)lbStyles).SelectedIndex);
		((Control)cbBackColor).Enabled = syntaxSettings.IsBackColorEnabled(((ListControl)lbStyles).SelectedIndex);
		ILexStyle selectedStyle = GetSelectedStyle();
		if (selectedStyle != null)
		{
			((Control)laDescription).Enabled = true;
			((Control)tbDescription).Enabled = true;
			((Control)laForeColor).Enabled = selectedStyle.ForeColorEnabled;
			((Control)cbForeColor).Enabled = selectedStyle.ForeColorEnabled;
			((Control)laBackColor).Enabled = selectedStyle.BackColorEnabled;
			((Control)cbBackColor).Enabled = selectedStyle.BackColorEnabled;
			((Control)chbBold).Enabled = selectedStyle.BoldEnabled;
			((Control)chbItalic).Enabled = selectedStyle.ItalicEnabled;
			((Control)chbUnderline).Enabled = selectedStyle.UnderlineEnabled;
			curForeColor = selectedStyle.ForeColor;
			curBkColor = selectedStyle.BackColor;
			curFontStyle = selectedStyle.FontStyle;
			curDesc = selectedStyle.Desc;
		}
		UpdateStyleControls();
	}

	private ILexStyle GetSelectedStyle()
	{
		if (lbStyles.SelectedItem != null)
		{
			return syntaxSettings.LexStyles[((ListControl)lbStyles).SelectedIndex];
		}
		return null;
	}

	private void UpdateStyleControls()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		isControlUpdating = true;
		try
		{
			chbBold.Checked = (curFontStyle & 1) > 0;
			chbItalic.Checked = (curFontStyle & 2) > 0;
			chbUnderline.Checked = (curFontStyle & 4) > 0;
			((Control)tbDescription).Text = curDesc;
			((Control)laSampleText).Font = new Font(((Control)laSampleText).Font.Name, (float)(int)((Control)laSampleText).Font.Size, curFontStyle);
			cbForeColor.SelectedColor = curForeColor;
			cbBackColor.SelectedColor = curBkColor;
			((Control)laSampleText).ForeColor = curForeColor;
			((Control)pnSampleText).BackColor = ((curBkColor != Color.Empty) ? curBkColor : syntaxSettings.ColorThemes.ActiveTheme[StringConsts.WindowColorInternalName].BackColor);
			WriteSampleText();
		}
		finally
		{
			isControlUpdating = false;
		}
	}

	private void FontNameChanged(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		if (!isControlUpdating && cbFontName.SelectedItem != null)
		{
			try
			{
				((Control)laSampleText).Font = new Font(((Control)cbFontName).Text, ((Control)laSampleText).Font.Size, curFontStyle);
			}
			catch
			{
			}
			WriteSampleText();
		}
	}

	private void FontSizeChanged(object sender, EventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		if (isControlUpdating)
		{
			return;
		}
		isControlUpdating = true;
		try
		{
			int num = Math.Max(Math.Min(GetInt(((Control)tbFontSize).Text, 10), EditConsts.MaxFontSize), 1);
			if (((Control)tbFontSize).Text != num.ToString())
			{
				((Control)tbFontSize).Text = num.ToString();
			}
			((Control)laSampleText).Font = new Font(((Control)laSampleText).Font.Name, (float)num, curFontStyle);
			WriteSampleText();
		}
		finally
		{
			isControlUpdating = false;
		}
	}

	private void WriteSampleText()
	{
		((Control)laSampleText).Location = new Point((((Control)pnSampleText).Width - ((Control)laSampleText).Width) / 2, (((Control)pnSampleText).Height - ((Control)laSampleText).Height) / 2);
	}

	private void btOK_Click(object sender, EventArgs e)
	{
		SettingsFromControl();
	}

	private void cbForeColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!isControlUpdating)
		{
			curForeColor = cbForeColor.SelectedColor;
			StyleFromControl();
		}
	}

	private void cbBackColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!isControlUpdating)
		{
			curBkColor = cbBackColor.SelectedColor;
			StyleFromControl();
		}
	}

	private void tbMarginPosition_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar <= ' ')
		{
			return;
		}
		e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
		if (!e.Handled)
		{
			try
			{
				string text = ((Control)tbMarginPosition).Text;
				int.Parse(text.Insert(Math.Min(((TextBoxBase)tbMarginPosition).SelectionStart, text.Length), e.KeyChar.ToString()));
				return;
			}
			catch
			{
				e.Handled = true;
				OSUtils.MessageBeep();
				return;
			}
		}
		OSUtils.MessageBeep();
	}

	private void tbGutterWidth_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar <= ' ')
		{
			return;
		}
		e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
		if (!e.Handled)
		{
			try
			{
				string text = ((Control)tbGutterWidth).Text;
				int.Parse(text.Insert(Math.Min(((TextBoxBase)tbGutterWidth).SelectionStart, text.Length), e.KeyChar.ToString()));
				return;
			}
			catch
			{
				e.Handled = true;
				OSUtils.MessageBeep();
				return;
			}
		}
		OSUtils.MessageBeep();
	}

	private void tbShowCommands_TextChanged(object sender, EventArgs e)
	{
		UpdateEventHandlers();
	}

	private void lbEventHandlers_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateShortcut(((ListControl)lbEventHandlers).SelectedIndex);
	}

	private void cbColorThemes_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)cbColorThemes).SelectedIndex >= 0 && ((ListControl)cbColorThemes).SelectedIndex < syntaxSettings.ColorThemes.Themes.Length)
		{
			syntaxSettings.ColorThemes.ActiveThemeIndex = ((ListControl)cbColorThemes).SelectedIndex;
			((Control)btDeleteColorTheme).Enabled = !syntaxSettings.ColorThemes.ActiveTheme.ReadOnly;
			UpdateFontControls();
			FontNameChanged(this, new EventArgs());
			OnStyleSelected(this, new EventArgs());
		}
	}

	private void btAddColorTheme_Click(object sender, EventArgs e)
	{
		IColorThemes colorThemes = syntaxSettings.ColorThemes;
		colorThemes.Add((ColorTheme)colorThemes.ActiveTheme.Clone());
		int num = colorThemes.Themes.Length - 1;
		string name = colorThemes.Themes[num].Name;
		colorThemes.Themes[num].Name = "Copy of " + name;
		colorThemes.Themes[num].ReadOnly = false;
		FillColorThemes();
		colorThemes.ActiveThemeIndex = num;
		((ListControl)cbColorThemes).SelectedIndex = num;
	}

	private void btDeleteColorTheme_Click(object sender, EventArgs e)
	{
		IColorThemes colorThemes = syntaxSettings.ColorThemes;
		if (colorThemes.ActiveThemeIndex != -1 && !colorThemes.Themes[colorThemes.ActiveThemeIndex].ReadOnly)
		{
			colorThemes.Remove(colorThemes.ActiveThemeIndex);
			colorThemes.ActiveThemeIndex--;
			FillColorThemes();
			((ListControl)cbColorThemes).SelectedIndex = colorThemes.ActiveThemeIndex;
		}
	}

	private void cbColorThemes_Leave(object sender, EventArgs e)
	{
		if (((ListControl)cbColorThemes).SelectedIndex == -1 && !syntaxSettings.ColorThemes.ActiveTheme.ReadOnly && ((Control)cbColorThemes).Text != string.Empty)
		{
			syntaxSettings.ColorThemes.ActiveTheme.Name = ((Control)cbColorThemes).Text;
			FillColorThemes();
		}
	}

	private void UpdateActiveColorThemeFont()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		if (!isFontControlsUpdating && ((ListControl)cbFontName).SelectedIndex != -1 && ((Control)tbFontSize).Text != null)
		{
			syntaxSettings.Font = new Font(cbFontName.SelectedItem.ToString(), (float)GetInt(((Control)tbFontSize).Text, 10));
		}
	}

	private void cbFontName_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateActiveColorThemeFont();
	}

	private void tbFontSize_Leave(object sender, EventArgs e)
	{
		UpdateActiveColorThemeFont();
	}
}
