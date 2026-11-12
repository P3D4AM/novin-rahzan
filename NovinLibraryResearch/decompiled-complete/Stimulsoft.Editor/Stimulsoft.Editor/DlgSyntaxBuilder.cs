using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Editor;

public class DlgSyntaxBuilder : Form
{
	internal enum NodeKind
	{
		nkNone,
		nkStyles,
		nkStyle,
		nkStates,
		nkState,
		nkSyntaxBlock,
		nkGeneral,
		nkreswordSet
	}

	private Panel pnButtons;

	private Button btClear;

	private Button btSave;

	private TreeView tvLexer;

	private ColorDialog colorDialog1;

	private IContainer components;

	private ContextMenuStrip cmLexer;

	private Button btLoadScheme;

	private ImageList imLexer;

	private ContextMenuStrip cmExpressions;

	private Button btOk;

	private Button btCancel;

	private TabControl tcPanels;

	private TabPage tpGeneral;

	private TabPage tpStyles;

	private TabPage tpStates;

	public Panel pnStyles;

	public Panel pnSample;

	public Label laSample;

	public GroupBox gbStyleProperties;

	public CheckBox chbPlainText;

	public ColorBox clbBkColor;

	public ColorBox clbForeColor;

	public TextBox tbStyleName;

	public Label laStyleName;

	public Label laStyleBkColor;

	public Label laStyleForeColor;

	public TextBox tbStyleDesc;

	public Label laStyleDesc;

	public GroupBox gbFontStyles;

	public CheckBox chbStrikeout;

	public CheckBox chbItalic;

	public CheckBox chbUnderline;

	public CheckBox chbBold;

	public Panel pnStates;

	public GroupBox gbSyntaxBlockProperties;

	public Label laBlockreswords;

	public Label laBlockExpr;

	public TextBox tbBlockDesc;

	public Label laBlockDesc;

	public ComboBox cbreswordStyle;

	public Label lareswordStyle;

	public Label laLeaveState;

	public Label laStyle;

	public ComboBox cbLeaveState;

	public ComboBox cbStyle;

	public GroupBox gbStateProperties;

	public TextBox tbStateName;

	private Label laStateName;

	public Label laStateDesc;

	public TextBox tbStateDesc;

	public CheckBox chbCaseSensitive;

	public Panel pnGeneral;

	public Label laCopyright;

	public Label laDescription;

	public Label laSchemeName;

	public Label laAuthor;

	public TextBox tbCopyright;

	public TextBox tbDescription;

	public TextBox tbSchemeName;

	public TextBox tbAuthor;

	private Panel pnMain;

	private SaveFileDialog saveFileDialog;

	private OpenFileDialog openFileDialog;

	private GroupBox gbreswordsProperties;

	private Button btStyleAdd;

	private Button btStyleEdit;

	private Button btStyleDelete;

	private Panel pnStyleButtons;

	private ImageList ilButtons;

	private Panel pnStateButtons;

	private Button btStateDelete;

	private Button btStateEdit;

	private Button btStateAdd;

	private Panel pnBlockButtons;

	private Button btBlockDelete;

	private Button btBlockEdit;

	private Button btBlockAdd;

	private Panel pnreswordButtons;

	private Button btreswordSetDelete;

	private Button btreswordSetEdit;

	private Button btreswordSetAdd;

	private Label laBlockName;

	private TextBox tbBlockName;

	private Label lareswordSetName;

	private TextBox tbreswordSetName;

	private Lexer lexer;

	private bool updating;

	private TreeNode rootNode;

	private TreeNode generalNode;

	private TreeNode stylesNode;

	private TreeNode statesNode;

	private const int cOpenFolder = 0;

	private const int cCloseFolder = 1;

	private const int cSelItem = 2;

	private const int cUnSelItem = 3;

	public TextBox tbreswords;

	public TextBox tbExpressions;

	private ToolStripMenuItem menuItem1;

	private ToolStripMenuItem menuItem2;

	private ToolStripMenuItem menuItem3;

	private ToolStripMenuItem menuItem4;

	private ToolStripMenuItem menuItem5;

	public Label laFileType;

	public TextBox tbFileType;

	public Label laFileExt;

	public TextBox tbFileExt;

	private const int cState = 4;

	private SyntaxBuilderEditor scriptEditor;

	private string sRemoveScroll = new string('x', 100);

	public LexScheme Scheme
	{
		get
		{
			return (LexScheme)lexer.Scheme;
		}
		set
		{
			lexer.Scheme = value;
			UpdateScheme();
		}
	}

	public DlgSyntaxBuilder()
	{
		InitializeComponent();
		lexer = new Lexer();
		rootNode = tvLexer.Nodes[0];
		generalNode = rootNode.Nodes[0];
		stylesNode = rootNode.Nodes[1];
		statesNode = rootNode.Nodes[2];
	}

	public DlgSyntaxBuilder(SyntaxBuilderEditor editor)
		: this()
	{
		scriptEditor = editor;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Form)this).TopLevel = true;
		((Form)this).ShowInTaskbar = false;
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Expected O, but got Unknown
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Expected O, but got Unknown
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Expected O, but got Unknown
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Expected O, but got Unknown
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Expected O, but got Unknown
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6b: Expected O, but got Unknown
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Expected O, but got Unknown
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Expected O, but got Unknown
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Expected O, but got Unknown
		//IL_14c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cc: Expected O, but got Unknown
		//IL_16a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16af: Expected O, but got Unknown
		components = new Container();
		TreeNode val = new TreeNode("General", 3, 2);
		TreeNode val2 = new TreeNode("Styles", 1, 0);
		TreeNode val3 = new TreeNode("States", 1, 0);
		TreeNode val4 = new TreeNode("Syntax Scheme Editor", 0, 0, (TreeNode[])(object)new TreeNode[3] { val, val2, val3 });
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DlgSyntaxBuilder));
		pnButtons = new Panel();
		btCancel = new Button();
		btOk = new Button();
		btLoadScheme = new Button();
		btClear = new Button();
		btSave = new Button();
		tvLexer = new TreeView();
		imLexer = new ImageList(components);
		pnMain = new Panel();
		tcPanels = new TabControl();
		tpGeneral = new TabPage();
		pnGeneral = new Panel();
		laFileExt = new Label();
		tbFileExt = new TextBox();
		laFileType = new Label();
		tbFileType = new TextBox();
		laCopyright = new Label();
		laDescription = new Label();
		laSchemeName = new Label();
		laAuthor = new Label();
		tbCopyright = new TextBox();
		tbDescription = new TextBox();
		tbSchemeName = new TextBox();
		tbAuthor = new TextBox();
		tpStyles = new TabPage();
		pnStyles = new Panel();
		pnStyleButtons = new Panel();
		btStyleDelete = new Button();
		ilButtons = new ImageList(components);
		btStyleEdit = new Button();
		btStyleAdd = new Button();
		pnSample = new Panel();
		laSample = new Label();
		gbStyleProperties = new GroupBox();
		chbPlainText = new CheckBox();
		tbStyleName = new TextBox();
		laStyleName = new Label();
		laStyleBkColor = new Label();
		laStyleForeColor = new Label();
		tbStyleDesc = new TextBox();
		laStyleDesc = new Label();
		clbBkColor = new ColorBox(components);
		clbForeColor = new ColorBox(components);
		gbFontStyles = new GroupBox();
		chbStrikeout = new CheckBox();
		chbItalic = new CheckBox();
		chbUnderline = new CheckBox();
		chbBold = new CheckBox();
		tpStates = new TabPage();
		pnStates = new Panel();
		gbreswordsProperties = new GroupBox();
		tbreswordSetName = new TextBox();
		lareswordSetName = new Label();
		pnreswordButtons = new Panel();
		btreswordSetDelete = new Button();
		btreswordSetEdit = new Button();
		btreswordSetAdd = new Button();
		cbreswordStyle = new ComboBox();
		tbreswords = new TextBox();
		laBlockreswords = new Label();
		lareswordStyle = new Label();
		gbSyntaxBlockProperties = new GroupBox();
		tbExpressions = new TextBox();
		tbBlockName = new TextBox();
		laBlockName = new Label();
		pnBlockButtons = new Panel();
		btBlockDelete = new Button();
		btBlockEdit = new Button();
		btBlockAdd = new Button();
		laBlockExpr = new Label();
		tbBlockDesc = new TextBox();
		laBlockDesc = new Label();
		laLeaveState = new Label();
		laStyle = new Label();
		cbLeaveState = new ComboBox();
		cbStyle = new ComboBox();
		gbStateProperties = new GroupBox();
		pnStateButtons = new Panel();
		btStateDelete = new Button();
		btStateEdit = new Button();
		btStateAdd = new Button();
		tbStateName = new TextBox();
		laStateName = new Label();
		laStateDesc = new Label();
		tbStateDesc = new TextBox();
		chbCaseSensitive = new CheckBox();
		colorDialog1 = new ColorDialog();
		saveFileDialog = new SaveFileDialog();
		cmLexer = new ContextMenuStrip();
		openFileDialog = new OpenFileDialog();
		cmExpressions = new ContextMenuStrip();
		menuItem1 = new ToolStripMenuItem();
		menuItem2 = new ToolStripMenuItem();
		menuItem3 = new ToolStripMenuItem();
		menuItem4 = new ToolStripMenuItem();
		menuItem5 = new ToolStripMenuItem();
		((Control)pnButtons).SuspendLayout();
		((Control)pnMain).SuspendLayout();
		((Control)tcPanels).SuspendLayout();
		((Control)tpGeneral).SuspendLayout();
		((Control)pnGeneral).SuspendLayout();
		((Control)tpStyles).SuspendLayout();
		((Control)pnStyles).SuspendLayout();
		((Control)pnStyleButtons).SuspendLayout();
		((Control)pnSample).SuspendLayout();
		((Control)gbStyleProperties).SuspendLayout();
		((Control)gbFontStyles).SuspendLayout();
		((Control)tpStates).SuspendLayout();
		((Control)pnStates).SuspendLayout();
		((Control)gbreswordsProperties).SuspendLayout();
		((Control)pnreswordButtons).SuspendLayout();
		((Control)gbSyntaxBlockProperties).SuspendLayout();
		((Control)pnBlockButtons).SuspendLayout();
		((Control)gbStateProperties).SuspendLayout();
		((Control)pnStateButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)pnButtons).BackColor = SystemColors.Control;
		pnButtons.BorderStyle = (BorderStyle)2;
		((Control)pnButtons).Controls.Add((Control)(object)btCancel);
		((Control)pnButtons).Controls.Add((Control)(object)btOk);
		((Control)pnButtons).Controls.Add((Control)(object)btLoadScheme);
		((Control)pnButtons).Controls.Add((Control)(object)btClear);
		((Control)pnButtons).Controls.Add((Control)(object)btSave);
		((Control)pnButtons).Dock = (DockStyle)2;
		((Control)pnButtons).Location = new Point(0, 480);
		((Control)pnButtons).Name = "pnButtons";
		((Control)pnButtons).Size = new Size(584, 40);
		((Control)pnButtons).TabIndex = 12;
		((Control)btCancel).Anchor = (AnchorStyles)9;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(502, 10);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 33;
		((Control)btCancel).Text = "Cancel";
		((Control)btCancel).Click += btCancel_Click;
		((Control)btOk).Anchor = (AnchorStyles)9;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(422, 10);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 23);
		((Control)btOk).TabIndex = 32;
		((Control)btOk).Text = "&OK";
		((Control)btOk).Click += btOk_Click;
		((Control)btLoadScheme).Anchor = (AnchorStyles)9;
		((Control)btLoadScheme).Location = new Point(166, 10);
		((Control)btLoadScheme).Name = "btLoadScheme";
		((Control)btLoadScheme).Size = new Size(75, 23);
		((Control)btLoadScheme).TabIndex = 28;
		((Control)btLoadScheme).Text = "Load";
		((Control)btLoadScheme).Click += btLoadScheme_Click;
		((Control)btClear).Anchor = (AnchorStyles)9;
		((Control)btClear).Location = new Point(326, 10);
		((Control)btClear).Name = "btClear";
		((Control)btClear).Size = new Size(75, 23);
		((Control)btClear).TabIndex = 31;
		((Control)btClear).Text = "Clear";
		((Control)btClear).Click += btClear_Click;
		((Control)btSave).Anchor = (AnchorStyles)9;
		((Control)btSave).Location = new Point(246, 10);
		((Control)btSave).Name = "btSave";
		((Control)btSave).Size = new Size(75, 23);
		((Control)btSave).TabIndex = 29;
		((Control)btSave).Text = "Save";
		((Control)btSave).Click += btSave_Click;
		((Control)tvLexer).BackColor = SystemColors.Control;
		((Control)tvLexer).Dock = (DockStyle)3;
		tvLexer.HideSelection = false;
		tvLexer.ImageIndex = 3;
		tvLexer.ImageList = imLexer;
		tvLexer.LabelEdit = true;
		((Control)tvLexer).Location = new Point(0, 0);
		((Control)tvLexer).Name = "tvLexer";
		val.ImageIndex = 3;
		val.Name = "";
		val.SelectedImageIndex = 2;
		val.Text = "General";
		val2.ImageIndex = 1;
		val2.Name = "";
		val2.SelectedImageIndex = 0;
		val2.Text = "Styles";
		val3.ImageIndex = 1;
		val3.Name = "";
		val3.SelectedImageIndex = 0;
		val3.Text = "States";
		val4.ImageIndex = 0;
		val4.Name = "";
		val4.SelectedImageIndex = 0;
		val4.Text = "Syntax Scheme Editor";
		tvLexer.Nodes.AddRange((TreeNode[])(object)new TreeNode[1] { val4 });
		tvLexer.SelectedImageIndex = 2;
		tvLexer.ShowLines = false;
		tvLexer.ShowRootLines = false;
		((Control)tvLexer).Size = new Size(184, 480);
		((Control)tvLexer).TabIndex = 34;
		tvLexer.BeforeLabelEdit += new NodeLabelEditEventHandler(tvLexer_BeforeLabelEdit);
		tvLexer.AfterLabelEdit += new NodeLabelEditEventHandler(tvLexer_AfterLabelEdit);
		tvLexer.AfterCollapse += new TreeViewEventHandler(tvLexer_AfterCollapse);
		tvLexer.AfterExpand += new TreeViewEventHandler(tvLexer_AfterExpand);
		tvLexer.AfterSelect += new TreeViewEventHandler(tvLexer_AfterSelect);
		((Control)tvLexer).MouseDown += new MouseEventHandler(tvLexer_MouseDown);
		imLexer.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imLexer.ImageStream");
		imLexer.TransparentColor = Color.Red;
		imLexer.Images.SetKeyName(0, "");
		imLexer.Images.SetKeyName(1, "");
		imLexer.Images.SetKeyName(2, "");
		imLexer.Images.SetKeyName(3, "");
		imLexer.Images.SetKeyName(4, "");
		((Control)pnMain).BackColor = SystemColors.Control;
		((Control)pnMain).Controls.Add((Control)(object)tcPanels);
		((Control)pnMain).Dock = (DockStyle)5;
		((Control)pnMain).Location = new Point(184, 0);
		((Control)pnMain).Name = "pnMain";
		((Control)pnMain).Size = new Size(400, 480);
		((Control)pnMain).TabIndex = 35;
		((Control)tcPanels).Controls.Add((Control)(object)tpGeneral);
		((Control)tcPanels).Controls.Add((Control)(object)tpStyles);
		((Control)tcPanels).Controls.Add((Control)(object)tpStates);
		((Control)tcPanels).Location = new Point(0, 0);
		((Control)tcPanels).Name = "tcPanels";
		tcPanels.SelectedIndex = 0;
		((Control)tcPanels).Size = new Size(394, 488);
		((Control)tcPanels).TabIndex = 0;
		((Control)tcPanels).Visible = false;
		((Control)tpGeneral).Controls.Add((Control)(object)pnGeneral);
		tpGeneral.Location = new Point(4, 22);
		((Control)tpGeneral).Name = "tpGeneral";
		((Control)tpGeneral).Size = new Size(386, 462);
		tpGeneral.TabIndex = 0;
		((Control)tpGeneral).Text = "General";
		((Control)pnGeneral).Controls.Add((Control)(object)laFileExt);
		((Control)pnGeneral).Controls.Add((Control)(object)tbFileExt);
		((Control)pnGeneral).Controls.Add((Control)(object)laFileType);
		((Control)pnGeneral).Controls.Add((Control)(object)tbFileType);
		((Control)pnGeneral).Controls.Add((Control)(object)laCopyright);
		((Control)pnGeneral).Controls.Add((Control)(object)laDescription);
		((Control)pnGeneral).Controls.Add((Control)(object)laSchemeName);
		((Control)pnGeneral).Controls.Add((Control)(object)laAuthor);
		((Control)pnGeneral).Controls.Add((Control)(object)tbCopyright);
		((Control)pnGeneral).Controls.Add((Control)(object)tbDescription);
		((Control)pnGeneral).Controls.Add((Control)(object)tbSchemeName);
		((Control)pnGeneral).Controls.Add((Control)(object)tbAuthor);
		((Control)pnGeneral).Location = new Point(0, 0);
		((Control)pnGeneral).Name = "pnGeneral";
		((Control)pnGeneral).Size = new Size(384, 448);
		((Control)pnGeneral).TabIndex = 1;
		((Control)pnGeneral).Visible = false;
		((Control)laFileExt).AutoSize = true;
		((Control)laFileExt).Location = new Point(16, 256);
		((Control)laFileExt).Name = "laFileExt";
		((Control)laFileExt).Size = new Size(75, 13);
		((Control)laFileExt).TabIndex = 10;
		((Control)laFileExt).Text = "File Extension:";
		((Control)tbFileExt).Anchor = (AnchorStyles)13;
		((Control)tbFileExt).Location = new Point(16, 280);
		((Control)tbFileExt).Name = "tbFileExt";
		((Control)tbFileExt).Size = new Size(360, 20);
		((Control)tbFileExt).TabIndex = 11;
		((Control)laFileType).AutoSize = true;
		((Control)laFileType).Location = new Point(16, 202);
		((Control)laFileType).Name = "laFileType";
		((Control)laFileType).Size = new Size(53, 13);
		((Control)laFileType).TabIndex = 8;
		((Control)laFileType).Text = "File Type:";
		((Control)tbFileType).Anchor = (AnchorStyles)13;
		((Control)tbFileType).Location = new Point(16, 226);
		((Control)tbFileType).Name = "tbFileType";
		((Control)tbFileType).Size = new Size(360, 20);
		((Control)tbFileType).TabIndex = 9;
		((Control)laCopyright).AutoSize = true;
		((Control)laCopyright).Location = new Point(16, 152);
		((Control)laCopyright).Name = "laCopyright";
		((Control)laCopyright).Size = new Size(54, 13);
		((Control)laCopyright).TabIndex = 6;
		((Control)laCopyright).Text = "Copyright:";
		((Control)laDescription).AutoSize = true;
		((Control)laDescription).Location = new Point(16, 104);
		((Control)laDescription).Name = "laDescription";
		((Control)laDescription).Size = new Size(63, 13);
		((Control)laDescription).TabIndex = 4;
		((Control)laDescription).Text = "Description:";
		((Control)laSchemeName).AutoSize = true;
		((Control)laSchemeName).Location = new Point(16, 56);
		((Control)laSchemeName).Name = "laSchemeName";
		((Control)laSchemeName).Size = new Size(80, 13);
		((Control)laSchemeName).TabIndex = 2;
		((Control)laSchemeName).Text = "Scheme Name:";
		((Control)laAuthor).AutoSize = true;
		((Control)laAuthor).Location = new Point(16, 8);
		((Control)laAuthor).Name = "laAuthor";
		((Control)laAuthor).Size = new Size(41, 13);
		((Control)laAuthor).TabIndex = 0;
		((Control)laAuthor).Text = "Author:";
		((Control)tbCopyright).Anchor = (AnchorStyles)13;
		((Control)tbCopyright).Location = new Point(16, 176);
		((Control)tbCopyright).Name = "tbCopyright";
		((Control)tbCopyright).Size = new Size(360, 20);
		((Control)tbCopyright).TabIndex = 7;
		((Control)tbDescription).Anchor = (AnchorStyles)13;
		((Control)tbDescription).Location = new Point(16, 128);
		((Control)tbDescription).Name = "tbDescription";
		((Control)tbDescription).Size = new Size(360, 20);
		((Control)tbDescription).TabIndex = 5;
		((Control)tbSchemeName).Anchor = (AnchorStyles)13;
		((Control)tbSchemeName).Location = new Point(16, 80);
		((Control)tbSchemeName).Name = "tbSchemeName";
		((Control)tbSchemeName).Size = new Size(360, 20);
		((Control)tbSchemeName).TabIndex = 3;
		((Control)tbAuthor).Anchor = (AnchorStyles)13;
		((Control)tbAuthor).Location = new Point(16, 32);
		((Control)tbAuthor).Name = "tbAuthor";
		((Control)tbAuthor).Size = new Size(360, 20);
		((Control)tbAuthor).TabIndex = 1;
		((Control)tpStyles).Controls.Add((Control)(object)pnStyles);
		tpStyles.Location = new Point(4, 22);
		((Control)tpStyles).Name = "tpStyles";
		((Control)tpStyles).Size = new Size(386, 462);
		tpStyles.TabIndex = 1;
		((Control)tpStyles).Text = "Styles";
		((Control)pnStyles).BackColor = SystemColors.Control;
		((Control)pnStyles).Controls.Add((Control)(object)pnStyleButtons);
		((Control)pnStyles).Controls.Add((Control)(object)pnSample);
		((Control)pnStyles).Controls.Add((Control)(object)gbStyleProperties);
		((Control)pnStyles).Location = new Point(0, 0);
		((Control)pnStyles).Name = "pnStyles";
		((Control)pnStyles).Size = new Size(456, 480);
		((Control)pnStyles).TabIndex = 4;
		((Control)pnStyles).Visible = false;
		((Control)pnStyleButtons).Controls.Add((Control)(object)btStyleDelete);
		((Control)pnStyleButtons).Controls.Add((Control)(object)btStyleEdit);
		((Control)pnStyleButtons).Controls.Add((Control)(object)btStyleAdd);
		((Control)pnStyleButtons).Location = new Point(8, 304);
		((Control)pnStyleButtons).Name = "pnStyleButtons";
		((Control)pnStyleButtons).Size = new Size(52, 18);
		((Control)pnStyleButtons).TabIndex = 7;
		((ButtonBase)btStyleDelete).ImageIndex = 2;
		((ButtonBase)btStyleDelete).ImageList = ilButtons;
		((Control)btStyleDelete).Location = new Point(17, 0);
		((Control)btStyleDelete).Name = "btStyleDelete";
		((Control)btStyleDelete).Size = new Size(17, 17);
		((Control)btStyleDelete).TabIndex = 1;
		((Control)btStyleDelete).Click += DeleteStyleClick;
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = SystemColors.Control;
		ilButtons.Images.SetKeyName(0, "");
		ilButtons.Images.SetKeyName(1, "");
		ilButtons.Images.SetKeyName(2, "");
		((ButtonBase)btStyleEdit).ImageIndex = 1;
		((ButtonBase)btStyleEdit).ImageList = ilButtons;
		((Control)btStyleEdit).Location = new Point(34, 0);
		((Control)btStyleEdit).Name = "btStyleEdit";
		((Control)btStyleEdit).Size = new Size(17, 17);
		((Control)btStyleEdit).TabIndex = 2;
		((Control)btStyleEdit).Click += btStyleEdit_Click;
		((ButtonBase)btStyleAdd).ImageIndex = 0;
		((ButtonBase)btStyleAdd).ImageList = ilButtons;
		((Control)btStyleAdd).Location = new Point(0, 0);
		((Control)btStyleAdd).Name = "btStyleAdd";
		((Control)btStyleAdd).Size = new Size(17, 17);
		((Control)btStyleAdd).TabIndex = 0;
		((Control)btStyleAdd).Click += AddStyleClick;
		pnSample.BorderStyle = (BorderStyle)2;
		((Control)pnSample).Controls.Add((Control)(object)laSample);
		((Control)pnSample).Location = new Point(8, 232);
		((Control)pnSample).Name = "pnSample";
		((Control)pnSample).Size = new Size(378, 64);
		((Control)pnSample).TabIndex = 6;
		((Control)laSample).AutoSize = true;
		laSample.BorderStyle = (BorderStyle)2;
		((Control)laSample).Font = new Font("Microsoft Sans Serif", 18f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)laSample).Location = new Point(120, 16);
		((Control)laSample).Name = "laSample";
		((Control)laSample).Size = new Size(125, 31);
		((Control)laSample).TabIndex = 0;
		((Control)laSample).Text = "AaBbYyZz";
		((Control)gbStyleProperties).Controls.Add((Control)(object)chbPlainText);
		((Control)gbStyleProperties).Controls.Add((Control)(object)tbStyleName);
		((Control)gbStyleProperties).Controls.Add((Control)(object)laStyleName);
		((Control)gbStyleProperties).Controls.Add((Control)(object)laStyleBkColor);
		((Control)gbStyleProperties).Controls.Add((Control)(object)laStyleForeColor);
		((Control)gbStyleProperties).Controls.Add((Control)(object)tbStyleDesc);
		((Control)gbStyleProperties).Controls.Add((Control)(object)laStyleDesc);
		((Control)gbStyleProperties).Controls.Add((Control)(object)clbBkColor);
		((Control)gbStyleProperties).Controls.Add((Control)(object)clbForeColor);
		((Control)gbStyleProperties).Controls.Add((Control)(object)gbFontStyles);
		((Control)gbStyleProperties).Location = new Point(8, 8);
		((Control)gbStyleProperties).Name = "gbStyleProperties";
		((Control)gbStyleProperties).Size = new Size(380, 216);
		((Control)gbStyleProperties).TabIndex = 5;
		gbStyleProperties.TabStop = false;
		((Control)gbStyleProperties).Text = "Style Properties";
		((Control)chbPlainText).Location = new Point(256, 160);
		((Control)chbPlainText).Name = "chbPlainText";
		((Control)chbPlainText).Size = new Size(80, 24);
		((Control)chbPlainText).TabIndex = 8;
		((Control)chbPlainText).Text = "Plain Text";
		((Control)tbStyleName).Location = new Point(16, 32);
		((Control)tbStyleName).Name = "tbStyleName";
		((Control)tbStyleName).Size = new Size(216, 20);
		((Control)tbStyleName).TabIndex = 1;
		((Control)laStyleName).AutoSize = true;
		((Control)laStyleName).Location = new Point(16, 16);
		((Control)laStyleName).Name = "laStyleName";
		((Control)laStyleName).Size = new Size(38, 13);
		((Control)laStyleName).TabIndex = 0;
		((Control)laStyleName).Text = "Name:";
		((Control)laStyleBkColor).AutoSize = true;
		((Control)laStyleBkColor).Location = new Point(16, 144);
		((Control)laStyleBkColor).Name = "laStyleBkColor";
		((Control)laStyleBkColor).Size = new Size(62, 13);
		((Control)laStyleBkColor).TabIndex = 6;
		((Control)laStyleBkColor).Text = "Back Color:";
		((Control)laStyleForeColor).AutoSize = true;
		((Control)laStyleForeColor).Location = new Point(16, 98);
		((Control)laStyleForeColor).Name = "laStyleForeColor";
		((Control)laStyleForeColor).Size = new Size(58, 13);
		((Control)laStyleForeColor).TabIndex = 4;
		((Control)laStyleForeColor).Text = "Fore Color:";
		((Control)tbStyleDesc).Location = new Point(16, 72);
		((Control)tbStyleDesc).Name = "tbStyleDesc";
		((Control)tbStyleDesc).Size = new Size(216, 20);
		((Control)tbStyleDesc).TabIndex = 3;
		((Control)laStyleDesc).AutoSize = true;
		((Control)laStyleDesc).Location = new Point(16, 56);
		((Control)laStyleDesc).Name = "laStyleDesc";
		((Control)laStyleDesc).Size = new Size(63, 13);
		((Control)laStyleDesc).TabIndex = 2;
		((Control)laStyleDesc).Text = "Description:";
		((ComboBox)clbBkColor).DrawMode = (DrawMode)1;
		((ComboBox)clbBkColor).DropDownStyle = (ComboBoxStyle)2;
		((Control)clbBkColor).Location = new Point(16, 160);
		((Control)clbBkColor).Name = "clbBkColor";
		clbBkColor.SelectedColor = Color.Empty;
		((Control)clbBkColor).Size = new Size(216, 21);
		((Control)clbBkColor).TabIndex = 7;
		((ComboBox)clbForeColor).DrawMode = (DrawMode)1;
		((ComboBox)clbForeColor).DropDownStyle = (ComboBoxStyle)2;
		((Control)clbForeColor).Location = new Point(16, 114);
		((Control)clbForeColor).Name = "clbForeColor";
		clbForeColor.SelectedColor = Color.Empty;
		((Control)clbForeColor).Size = new Size(216, 21);
		((Control)clbForeColor).TabIndex = 5;
		((Control)gbFontStyles).Controls.Add((Control)(object)chbStrikeout);
		((Control)gbFontStyles).Controls.Add((Control)(object)chbItalic);
		((Control)gbFontStyles).Controls.Add((Control)(object)chbUnderline);
		((Control)gbFontStyles).Controls.Add((Control)(object)chbBold);
		((Control)gbFontStyles).Location = new Point(256, 32);
		((Control)gbFontStyles).Name = "gbFontStyles";
		((Control)gbFontStyles).Size = new Size(112, 120);
		((Control)gbFontStyles).TabIndex = 9;
		gbFontStyles.TabStop = false;
		((Control)gbFontStyles).Text = "Font Styles";
		((Control)chbStrikeout).Location = new Point(16, 88);
		((Control)chbStrikeout).Name = "chbStrikeout";
		((Control)chbStrikeout).Size = new Size(80, 24);
		((Control)chbStrikeout).TabIndex = 3;
		((Control)chbStrikeout).Text = "StrikeOut";
		((Control)chbItalic).Location = new Point(16, 40);
		((Control)chbItalic).Name = "chbItalic";
		((Control)chbItalic).Size = new Size(80, 24);
		((Control)chbItalic).TabIndex = 1;
		((Control)chbItalic).Text = "Italic";
		((Control)chbUnderline).Location = new Point(16, 64);
		((Control)chbUnderline).Name = "chbUnderline";
		((Control)chbUnderline).Size = new Size(80, 24);
		((Control)chbUnderline).TabIndex = 2;
		((Control)chbUnderline).Text = "Underline";
		((Control)chbBold).Location = new Point(16, 16);
		((Control)chbBold).Name = "chbBold";
		((Control)chbBold).Size = new Size(80, 24);
		((Control)chbBold).TabIndex = 0;
		((Control)chbBold).Text = "Bold";
		((Control)tpStates).Controls.Add((Control)(object)pnStates);
		tpStates.Location = new Point(4, 22);
		((Control)tpStates).Name = "tpStates";
		((Control)tpStates).Size = new Size(386, 462);
		tpStates.TabIndex = 2;
		((Control)tpStates).Text = "States";
		((Control)pnStates).BackColor = SystemColors.Control;
		((Control)pnStates).Controls.Add((Control)(object)gbreswordsProperties);
		((Control)pnStates).Controls.Add((Control)(object)gbSyntaxBlockProperties);
		((Control)pnStates).Controls.Add((Control)(object)gbStateProperties);
		((Control)pnStates).Location = new Point(0, 0);
		((Control)pnStates).Name = "pnStates";
		((Control)pnStates).Size = new Size(390, 490);
		((Control)pnStates).TabIndex = 3;
		((Control)pnStates).Visible = false;
		((Control)gbreswordsProperties).Controls.Add((Control)(object)tbreswordSetName);
		((Control)gbreswordsProperties).Controls.Add((Control)(object)lareswordSetName);
		((Control)gbreswordsProperties).Controls.Add((Control)(object)pnreswordButtons);
		((Control)gbreswordsProperties).Controls.Add((Control)(object)cbreswordStyle);
		((Control)gbreswordsProperties).Controls.Add((Control)(object)tbreswords);
		((Control)gbreswordsProperties).Controls.Add((Control)(object)laBlockreswords);
		((Control)gbreswordsProperties).Controls.Add((Control)(object)lareswordStyle);
		((Control)gbreswordsProperties).Location = new Point(8, 312);
		((Control)gbreswordsProperties).Name = "gbreswordsProperties";
		((Control)gbreswordsProperties).Size = new Size(380, 144);
		((Control)gbreswordsProperties).TabIndex = 2;
		gbreswordsProperties.TabStop = false;
		((Control)gbreswordsProperties).Text = "resword Set Properties";
		((Control)tbreswordSetName).Location = new Point(112, 16);
		((Control)tbreswordSetName).Name = "tbreswordSetName";
		((Control)tbreswordSetName).Size = new Size(256, 20);
		((Control)tbreswordSetName).TabIndex = 1;
		((Control)lareswordSetName).AutoSize = true;
		((Control)lareswordSetName).Location = new Point(8, 19);
		((Control)lareswordSetName).Name = "lareswordSetName";
		((Control)lareswordSetName).Size = new Size(38, 13);
		((Control)lareswordSetName).TabIndex = 0;
		((Control)lareswordSetName).Text = "Name:";
		((Control)pnreswordButtons).Controls.Add((Control)(object)btreswordSetDelete);
		((Control)pnreswordButtons).Controls.Add((Control)(object)btreswordSetEdit);
		((Control)pnreswordButtons).Controls.Add((Control)(object)btreswordSetAdd);
		((Control)pnreswordButtons).Location = new Point(8, 124);
		((Control)pnreswordButtons).Name = "pnreswordButtons";
		((Control)pnreswordButtons).Size = new Size(52, 18);
		((Control)pnreswordButtons).TabIndex = 7;
		((ButtonBase)btreswordSetDelete).ImageIndex = 2;
		((ButtonBase)btreswordSetDelete).ImageList = ilButtons;
		((Control)btreswordSetDelete).Location = new Point(17, 0);
		((Control)btreswordSetDelete).Name = "btreswordSetDelete";
		((Control)btreswordSetDelete).Size = new Size(17, 17);
		((Control)btreswordSetDelete).TabIndex = 1;
		((Control)btreswordSetDelete).Click += DeletereswordSetClick;
		((ButtonBase)btreswordSetEdit).ImageIndex = 1;
		((ButtonBase)btreswordSetEdit).ImageList = ilButtons;
		((Control)btreswordSetEdit).Location = new Point(34, 0);
		((Control)btreswordSetEdit).Name = "btreswordSetEdit";
		((Control)btreswordSetEdit).Size = new Size(17, 17);
		((Control)btreswordSetEdit).TabIndex = 2;
		((Control)btreswordSetEdit).Click += btreswordSetEdit_Click;
		((ButtonBase)btreswordSetAdd).ImageIndex = 0;
		((ButtonBase)btreswordSetAdd).ImageList = ilButtons;
		((Control)btreswordSetAdd).Location = new Point(0, 0);
		((Control)btreswordSetAdd).Name = "btreswordSetAdd";
		((Control)btreswordSetAdd).Size = new Size(17, 17);
		((Control)btreswordSetAdd).TabIndex = 0;
		((Control)btreswordSetAdd).Click += AddreswordSetClick;
		cbreswordStyle.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbreswordStyle).Location = new Point(112, 40);
		((Control)cbreswordStyle).Name = "cbreswordStyle";
		((Control)cbreswordStyle).Size = new Size(256, 21);
		((Control)cbreswordStyle).TabIndex = 3;
		((Control)tbreswords).Location = new Point(112, 64);
		((TextBoxBase)tbreswords).Multiline = true;
		((Control)tbreswords).Name = "tbreswords";
		tbreswords.ScrollBars = (ScrollBars)2;
		((Control)tbreswords).Size = new Size(256, 64);
		((Control)tbreswords).TabIndex = 5;
		((Control)laBlockreswords).AutoSize = true;
		((Control)laBlockreswords).Location = new Point(8, 67);
		((Control)laBlockreswords).Name = "laBlockreswords";
		((Control)laBlockreswords).Size = new Size(52, 13);
		((Control)laBlockreswords).TabIndex = 4;
		((Control)laBlockreswords).Text = "reswords:";
		((Control)lareswordStyle).AutoSize = true;
		((Control)lareswordStyle).Location = new Point(8, 43);
		((Control)lareswordStyle).Name = "lareswordStyle";
		((Control)lareswordStyle).Size = new Size(73, 13);
		((Control)lareswordStyle).TabIndex = 2;
		((Control)lareswordStyle).Text = "resword Style:";
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)tbExpressions);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)tbBlockName);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)laBlockName);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)pnBlockButtons);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)laBlockExpr);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)tbBlockDesc);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)laBlockDesc);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)laLeaveState);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)laStyle);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)cbLeaveState);
		((Control)gbSyntaxBlockProperties).Controls.Add((Control)(object)cbStyle);
		((Control)gbSyntaxBlockProperties).Location = new Point(8, 102);
		((Control)gbSyntaxBlockProperties).Name = "gbSyntaxBlockProperties";
		((Control)gbSyntaxBlockProperties).Size = new Size(380, 194);
		((Control)gbSyntaxBlockProperties).TabIndex = 1;
		gbSyntaxBlockProperties.TabStop = false;
		((Control)gbSyntaxBlockProperties).Text = "Block Properties";
		((Control)tbExpressions).Location = new Point(112, 112);
		((TextBoxBase)tbExpressions).Multiline = true;
		((Control)tbExpressions).Name = "tbExpressions";
		((Control)tbExpressions).Size = new Size(256, 64);
		((Control)tbExpressions).TabIndex = 12;
		((Control)tbBlockName).Location = new Point(112, 16);
		((Control)tbBlockName).Name = "tbBlockName";
		((Control)tbBlockName).Size = new Size(256, 20);
		((Control)tbBlockName).TabIndex = 1;
		((Control)laBlockName).AutoSize = true;
		((Control)laBlockName).Location = new Point(8, 19);
		((Control)laBlockName).Name = "laBlockName";
		((Control)laBlockName).Size = new Size(38, 13);
		((Control)laBlockName).TabIndex = 0;
		((Control)laBlockName).Text = "Name:";
		((Control)pnBlockButtons).Controls.Add((Control)(object)btBlockDelete);
		((Control)pnBlockButtons).Controls.Add((Control)(object)btBlockEdit);
		((Control)pnBlockButtons).Controls.Add((Control)(object)btBlockAdd);
		((Control)pnBlockButtons).Location = new Point(8, 170);
		((Control)pnBlockButtons).Name = "pnBlockButtons";
		((Control)pnBlockButtons).Size = new Size(52, 18);
		((Control)pnBlockButtons).TabIndex = 11;
		((ButtonBase)btBlockDelete).ImageIndex = 2;
		((ButtonBase)btBlockDelete).ImageList = ilButtons;
		((Control)btBlockDelete).Location = new Point(17, 0);
		((Control)btBlockDelete).Name = "btBlockDelete";
		((Control)btBlockDelete).Size = new Size(17, 17);
		((Control)btBlockDelete).TabIndex = 1;
		((Control)btBlockDelete).Click += DeleteBlockClick;
		((ButtonBase)btBlockEdit).ImageIndex = 1;
		((ButtonBase)btBlockEdit).ImageList = ilButtons;
		((Control)btBlockEdit).Location = new Point(34, 0);
		((Control)btBlockEdit).Name = "btBlockEdit";
		((Control)btBlockEdit).Size = new Size(17, 17);
		((Control)btBlockEdit).TabIndex = 2;
		((Control)btBlockEdit).Click += btBlockEdit_Click;
		((ButtonBase)btBlockAdd).ImageIndex = 0;
		((ButtonBase)btBlockAdd).ImageList = ilButtons;
		((Control)btBlockAdd).Location = new Point(0, 0);
		((Control)btBlockAdd).Name = "btBlockAdd";
		((Control)btBlockAdd).Size = new Size(17, 17);
		((Control)btBlockAdd).TabIndex = 0;
		((Control)btBlockAdd).Click += AddBlockClick;
		((Control)laBlockExpr).AutoSize = true;
		((Control)laBlockExpr).Location = new Point(8, 115);
		((Control)laBlockExpr).Name = "laBlockExpr";
		((Control)laBlockExpr).Size = new Size(66, 13);
		((Control)laBlockExpr).TabIndex = 8;
		((Control)laBlockExpr).Text = "Expressions:";
		((Control)tbBlockDesc).Location = new Point(112, 40);
		((Control)tbBlockDesc).Name = "tbBlockDesc";
		((Control)tbBlockDesc).Size = new Size(256, 20);
		((Control)tbBlockDesc).TabIndex = 3;
		((Control)laBlockDesc).AutoSize = true;
		((Control)laBlockDesc).Location = new Point(8, 43);
		((Control)laBlockDesc).Name = "laBlockDesc";
		((Control)laBlockDesc).Size = new Size(63, 13);
		((Control)laBlockDesc).TabIndex = 2;
		((Control)laBlockDesc).Text = "Description:";
		((Control)laLeaveState).AutoSize = true;
		((Control)laLeaveState).Location = new Point(8, 67);
		((Control)laLeaveState).Name = "laLeaveState";
		((Control)laLeaveState).Size = new Size(68, 13);
		((Control)laLeaveState).TabIndex = 4;
		((Control)laLeaveState).Text = "Leave State:";
		((Control)laStyle).AutoSize = true;
		((Control)laStyle).Location = new Point(8, 91);
		((Control)laStyle).Name = "laStyle";
		((Control)laStyle).Size = new Size(33, 13);
		((Control)laStyle).TabIndex = 6;
		((Control)laStyle).Text = "Style:";
		cbLeaveState.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbLeaveState).Location = new Point(112, 64);
		((Control)cbLeaveState).Name = "cbLeaveState";
		((Control)cbLeaveState).Size = new Size(256, 21);
		((Control)cbLeaveState).TabIndex = 5;
		cbStyle.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbStyle).Location = new Point(112, 88);
		((Control)cbStyle).Name = "cbStyle";
		((Control)cbStyle).Size = new Size(256, 21);
		((Control)cbStyle).TabIndex = 7;
		((Control)gbStateProperties).Controls.Add((Control)(object)pnStateButtons);
		((Control)gbStateProperties).Controls.Add((Control)(object)tbStateName);
		((Control)gbStateProperties).Controls.Add((Control)(object)laStateName);
		((Control)gbStateProperties).Controls.Add((Control)(object)laStateDesc);
		((Control)gbStateProperties).Controls.Add((Control)(object)tbStateDesc);
		((Control)gbStateProperties).Controls.Add((Control)(object)chbCaseSensitive);
		((Control)gbStateProperties).Location = new Point(8, 8);
		((Control)gbStateProperties).Name = "gbStateProperties";
		((Control)gbStateProperties).Size = new Size(380, 92);
		((Control)gbStateProperties).TabIndex = 0;
		gbStateProperties.TabStop = false;
		((Control)gbStateProperties).Text = "State Properties";
		((Control)pnStateButtons).Controls.Add((Control)(object)btStateDelete);
		((Control)pnStateButtons).Controls.Add((Control)(object)btStateEdit);
		((Control)pnStateButtons).Controls.Add((Control)(object)btStateAdd);
		((Control)pnStateButtons).Location = new Point(8, 68);
		((Control)pnStateButtons).Name = "pnStateButtons";
		((Control)pnStateButtons).Size = new Size(52, 18);
		((Control)pnStateButtons).TabIndex = 8;
		((ButtonBase)btStateDelete).ImageIndex = 2;
		((ButtonBase)btStateDelete).ImageList = ilButtons;
		((Control)btStateDelete).Location = new Point(17, 0);
		((Control)btStateDelete).Name = "btStateDelete";
		((Control)btStateDelete).Size = new Size(17, 17);
		((Control)btStateDelete).TabIndex = 1;
		((Control)btStateDelete).Click += DeleteStateClick;
		((ButtonBase)btStateEdit).ImageIndex = 1;
		((ButtonBase)btStateEdit).ImageList = ilButtons;
		((Control)btStateEdit).Location = new Point(34, 0);
		((Control)btStateEdit).Name = "btStateEdit";
		((Control)btStateEdit).Size = new Size(17, 17);
		((Control)btStateEdit).TabIndex = 2;
		((Control)btStateEdit).Click += btStateEdit_Click;
		((ButtonBase)btStateAdd).ImageIndex = 0;
		((ButtonBase)btStateAdd).ImageList = ilButtons;
		((Control)btStateAdd).Location = new Point(0, 0);
		((Control)btStateAdd).Name = "btStateAdd";
		((Control)btStateAdd).Size = new Size(17, 17);
		((Control)btStateAdd).TabIndex = 0;
		((Control)btStateAdd).Click += AddStateClick;
		((Control)tbStateName).Location = new Point(112, 16);
		((Control)tbStateName).Name = "tbStateName";
		((Control)tbStateName).Size = new Size(256, 20);
		((Control)tbStateName).TabIndex = 1;
		((Control)laStateName).AutoSize = true;
		((Control)laStateName).Location = new Point(8, 19);
		((Control)laStateName).Name = "laStateName";
		((Control)laStateName).Size = new Size(38, 13);
		((Control)laStateName).TabIndex = 0;
		((Control)laStateName).Text = "Name:";
		((Control)laStateDesc).AutoSize = true;
		((Control)laStateDesc).Location = new Point(8, 43);
		((Control)laStateDesc).Name = "laStateDesc";
		((Control)laStateDesc).Size = new Size(63, 13);
		((Control)laStateDesc).TabIndex = 2;
		((Control)laStateDesc).Text = "Description:";
		((Control)tbStateDesc).Location = new Point(112, 40);
		((Control)tbStateDesc).Name = "tbStateDesc";
		((Control)tbStateDesc).Size = new Size(256, 20);
		((Control)tbStateDesc).TabIndex = 3;
		((Control)chbCaseSensitive).Location = new Point(264, 64);
		((Control)chbCaseSensitive).Name = "chbCaseSensitive";
		((Control)chbCaseSensitive).Size = new Size(104, 24);
		((Control)chbCaseSensitive).TabIndex = 4;
		((Control)chbCaseSensitive).Text = "CaseSensitive";
		((ToolStrip)cmExpressions).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[5]
		{
			(ToolStripItem)menuItem1,
			(ToolStripItem)menuItem2,
			(ToolStripItem)menuItem3,
			(ToolStripItem)menuItem4,
			(ToolStripItem)menuItem5
		});
		((ToolStripItem)menuItem1).Text = "Identifiers   [a-zA-Z_][a-zA-Z0-9_]*";
		((ToolStripItem)menuItem1).Click += menuItem5_Click;
		((ToolStripItem)menuItem2).Text = "Comments   //.";
		((ToolStripItem)menuItem2).Click += menuItem5_Click;
		((ToolStripItem)menuItem3).Text = "Numbers     ([0-9]+\\.[0-9]*(e|E)(\\+|\\-)?[0-9]+)|([0-9]+\\.[0-9]*)|([0-9]+)";
		((ToolStripItem)menuItem3).Click += menuItem5_Click;
		((ToolStripItem)menuItem4).Text = "Strings        '[^']*'";
		((ToolStripItem)menuItem4).Click += menuItem5_Click;
		((ToolStripItem)menuItem5).Text = "Whitespace (\\s)*";
		((ToolStripItem)menuItem5).Click += menuItem5_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(584, 520);
		((Control)this).Controls.Add((Control)(object)pnMain);
		((Control)this).Controls.Add((Control)(object)tvLexer);
		((Control)this).Controls.Add((Control)(object)pnButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Control)this).Name = "DlgSyntaxBuilder";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Syntax Scheme Builder";
		((Form)this).Closing += DlgSyntaxBuilder_Closing;
		((Form)this).Load += MainForm_Load;
		((Control)pnButtons).ResumeLayout(false);
		((Control)pnMain).ResumeLayout(false);
		((Control)tcPanels).ResumeLayout(false);
		((Control)tpGeneral).ResumeLayout(false);
		((Control)pnGeneral).ResumeLayout(false);
		((Control)pnGeneral).PerformLayout();
		((Control)tpStyles).ResumeLayout(false);
		((Control)pnStyles).ResumeLayout(false);
		((Control)pnStyleButtons).ResumeLayout(false);
		((Control)pnSample).ResumeLayout(false);
		((Control)pnSample).PerformLayout();
		((Control)gbStyleProperties).ResumeLayout(false);
		((Control)gbStyleProperties).PerformLayout();
		((Control)gbFontStyles).ResumeLayout(false);
		((Control)tpStates).ResumeLayout(false);
		((Control)pnStates).ResumeLayout(false);
		((Control)gbreswordsProperties).ResumeLayout(false);
		((Control)gbreswordsProperties).PerformLayout();
		((Control)pnreswordButtons).ResumeLayout(false);
		((Control)gbSyntaxBlockProperties).ResumeLayout(false);
		((Control)gbSyntaxBlockProperties).PerformLayout();
		((Control)pnBlockButtons).ResumeLayout(false);
		((Control)gbStateProperties).ResumeLayout(false);
		((Control)gbStateProperties).PerformLayout();
		((Control)pnStateButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private bool IsSyntaxBlockComplete(LexScheme scheme, ILexState state, ILexSyntaxBlock block, out string errorMsg)
	{
		errorMsg = string.Empty;
		bool result = true;
		if (block.LeaveState == null)
		{
			errorMsg = string.Format(SyntaxBuilderConsts.IncompleteLeaveState, state.Name, block.Name);
			return false;
		}
		if (block.Style == null)
		{
			errorMsg = string.Format(SyntaxBuilderConsts.IncompleteLeaveStyle, state.Name, block.Name);
			return false;
		}
		foreach (LexReswordSet lexReswordSet in block.LexReswordSets)
		{
			if (lexReswordSet.Reswords.Length != 0 && lexReswordSet.ReswordStyle == null)
			{
				errorMsg = string.Format(SyntaxBuilderConsts.IncompleteReswordStyle, state.Name, block.Name, lexReswordSet.Name);
				return false;
			}
		}
		return result;
	}

	private bool IsStateComplete(LexScheme scheme, ILexState state, out string errorMsg)
	{
		errorMsg = string.Empty;
		bool result = true;
		foreach (LexSyntaxBlock lexSyntaxBlock in state.LexSyntaxBlocks)
		{
			if (!IsSyntaxBlockComplete(scheme, state, lexSyntaxBlock, out errorMsg))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private bool IsSchemeComplete(LexScheme scheme, out string errorMsg)
	{
		errorMsg = string.Empty;
		bool result = true;
		for (int i = 0; i < scheme.LexStates.Count; i++)
		{
			if (!IsStateComplete(scheme, scheme.LexStates[i], out errorMsg))
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private int GetNodeLevel(TreeNode node)
	{
		if (node == null)
		{
			return -1;
		}
		TreeNode val = node;
		int num = -1;
		while (val != null)
		{
			val = val.Parent;
			num++;
		}
		return num;
	}

	private NodeKind GetNodeKind(TreeNode node)
	{
		int nodeLevel = GetNodeLevel(node);
		TreeNode parent = node.Parent;
		switch (nodeLevel)
		{
		case 0:
			return NodeKind.nkGeneral;
		case 1:
			return node.Index switch
			{
				0 => NodeKind.nkGeneral, 
				1 => NodeKind.nkStyles, 
				2 => NodeKind.nkStates, 
				_ => NodeKind.nkNone, 
			};
		case 2:
			if (parent == stylesNode)
			{
				return NodeKind.nkStyle;
			}
			if (parent == statesNode)
			{
				return NodeKind.nkState;
			}
			return NodeKind.nkNone;
		case 3:
			return NodeKind.nkSyntaxBlock;
		case 4:
			return NodeKind.nkreswordSet;
		default:
			return NodeKind.nkNone;
		}
	}

	private TreeNode GetStyleNode(TreeNode node)
	{
		switch (GetNodeKind(node))
		{
		case NodeKind.nkStyle:
			return node;
		case NodeKind.nkStyles:
			if (node.Nodes.Count > 0)
			{
				return node.Nodes[0];
			}
			return null;
		default:
			return null;
		}
	}

	private TreeNode GetStateNode(TreeNode node)
	{
		switch (GetNodeKind(node))
		{
		case NodeKind.nkStates:
			if (statesNode.Nodes.Count > 0)
			{
				return node.Nodes[0];
			}
			return null;
		case NodeKind.nkState:
			return node;
		case NodeKind.nkSyntaxBlock:
			return node.Parent;
		case NodeKind.nkreswordSet:
			return node.Parent.Parent;
		default:
			return null;
		}
	}

	private TreeNode GetSyntaxBlockNode(TreeNode node)
	{
		return (TreeNode)(GetNodeKind(node) switch
		{
			NodeKind.nkSyntaxBlock => node, 
			NodeKind.nkreswordSet => node.Parent, 
			_ => null, 
		});
	}

	private ILexSyntaxBlock GetLexSyntaxBlock(TreeNode node, LexScheme scheme)
	{
		NodeKind nodeKind = GetNodeKind(node);
		if (nodeKind != NodeKind.nkSyntaxBlock && nodeKind != NodeKind.nkreswordSet)
		{
			return null;
		}
		ILexState lexState = GetLexState(node.Parent, scheme);
		if (nodeKind == NodeKind.nkreswordSet)
		{
			node = node.Parent;
		}
		if (lexState == null)
		{
			return null;
		}
		return ((LexState)lexState).LexSyntaxBlocks[node.Index];
	}

	private ILexReswordSet GetLexreswordSet(TreeNode node, LexScheme scheme)
	{
		NodeKind nodeKind = GetNodeKind(node);
		if (nodeKind != NodeKind.nkreswordSet)
		{
			return null;
		}
		return GetLexSyntaxBlock(node.Parent, scheme)?.LexReswordSets[node.Index];
	}

	private void UpdateStylePanel(bool isStyle)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Invalid comparison between Unknown and I4
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Invalid comparison between Unknown and I4
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Invalid comparison between Unknown and I4
		UpdateEvents(update: false);
		try
		{
			((Control)pnStyles).Visible = isStyle || (!isStyle && stylesNode.Nodes.Count > 0);
			if (((Control)pnStyles).Visible)
			{
				ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
				if (lexStyle != null)
				{
					((Control)tbStyleName).Text = lexStyle.Name;
					((Control)tbStyleDesc).Text = lexStyle.Desc;
					clbForeColor.SelectedColor = lexStyle.ForeColor;
					clbBkColor.SelectedColor = lexStyle.BackColor;
					chbBold.Checked = (lexStyle.FontStyle & 1) > 0;
					chbItalic.Checked = (lexStyle.FontStyle & 2) > 0;
					chbUnderline.Checked = (lexStyle.FontStyle & 4) > 0;
					chbStrikeout.Checked = (lexStyle.FontStyle & 8) > 0;
					chbPlainText.Checked = lexStyle.PlainText;
				}
				((Control)btStyleDelete).Enabled = !IsStyleUsed();
				UpdateSample();
			}
		}
		finally
		{
			UpdateEvents(update: true);
		}
	}

	private void UpdateGeneralPanel()
	{
		((Control)pnGeneral).Visible = true;
		UpdateEvents(update: false);
		try
		{
			((Control)tbAuthor).Text = Scheme.Author;
			((Control)tbDescription).Text = Scheme.Desc;
			((Control)tbCopyright).Text = Scheme.Copyright;
			((Control)tbSchemeName).Text = Scheme.Name;
			((Control)tbFileType).Text = Scheme.FileType;
			((Control)tbFileExt).Text = Scheme.FileExtension;
		}
		finally
		{
			UpdateEvents(update: false);
		}
	}

	private void UpdateStatePanel()
	{
		UpdateEvents(update: false);
		try
		{
			NodeKind nodeKind = GetNodeKind(tvLexer.SelectedNode);
			((Control)pnStates).Visible = nodeKind != NodeKind.nkStates || (nodeKind == NodeKind.nkStates && statesNode.Nodes.Count > 0);
			switch (nodeKind)
			{
			case NodeKind.nkState:
				((Control)gbStateProperties).Visible = true;
				((Control)gbSyntaxBlockProperties).Visible = false;
				((Control)gbreswordsProperties).Visible = false;
				break;
			case NodeKind.nkStates:
				((Control)gbStateProperties).Visible = statesNode.Nodes.Count > 0;
				((Control)gbSyntaxBlockProperties).Visible = false;
				((Control)gbreswordsProperties).Visible = false;
				break;
			case NodeKind.nkSyntaxBlock:
				((Control)gbStateProperties).Visible = true;
				((Control)gbSyntaxBlockProperties).Visible = true;
				((Control)gbreswordsProperties).Visible = false;
				break;
			case NodeKind.nkreswordSet:
				((Control)gbStateProperties).Visible = true;
				((Control)gbSyntaxBlockProperties).Visible = true;
				((Control)gbreswordsProperties).Visible = true;
				break;
			}
			ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
			ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
			ILexReswordSet lexreswordSet = GetLexreswordSet(tvLexer.SelectedNode, Scheme);
			if (lexState != null)
			{
				chbCaseSensitive.Checked = lexState.CaseSensitive;
				((Control)tbStateDesc).Text = lexState.Desc;
				((Control)tbStateName).Text = lexState.Name;
			}
			((ListControl)cbStyle).SelectedIndex = GetStyleIndex(lexSyntaxBlock);
			((Control)btStateDelete).Enabled = !IsStateUsed();
			((ListControl)cbLeaveState).SelectedIndex = GetLeaveStateIndex(lexSyntaxBlock);
			((Control)tbExpressions).Text = string.Empty;
			((Control)tbreswords).Text = string.Empty;
			if (lexSyntaxBlock != null)
			{
				((Control)tbBlockName).Text = lexSyntaxBlock.Name;
				((Control)tbBlockDesc).Text = lexSyntaxBlock.Desc;
				string[] expressions = lexSyntaxBlock.Expressions;
				foreach (string text in expressions)
				{
					((Control)tbExpressions).Text = ((((Control)tbExpressions).Text == string.Empty) ? text : (((Control)tbExpressions).Text + "\r\n" + text));
				}
			}
			if (lexreswordSet != null)
			{
				((Control)tbreswordSetName).Text = lexreswordSet.Name;
				((Control)cbreswordStyle).Enabled = lexreswordSet.Reswords.Length != 0;
				((ListControl)cbreswordStyle).SelectedIndex = GetreswordStyleIndex(lexreswordSet);
				string[] reswords = lexreswordSet.Reswords;
				foreach (string text2 in reswords)
				{
					((Control)tbreswords).Text = ((((Control)tbreswords).Text == string.Empty) ? text2 : (((Control)tbreswords).Text + "\r\n" + text2));
				}
			}
		}
		finally
		{
			UpdateEvents(update: true);
		}
	}

	private void UpdatePanelControls(NodeKind kind)
	{
		switch (kind)
		{
		case NodeKind.nkStyle:
			UpdateStylePanel(isStyle: true);
			break;
		case NodeKind.nkStyles:
			UpdateStylePanel(isStyle: false);
			break;
		case NodeKind.nkGeneral:
			UpdateGeneralPanel();
			break;
		case NodeKind.nkStates:
		case NodeKind.nkState:
		case NodeKind.nkSyntaxBlock:
		case NodeKind.nkreswordSet:
			UpdateStatePanel();
			break;
		}
	}

	private void UpdateVisible(NodeKind kind)
	{
		Panel currentPanel = GetCurrentPanel(kind);
		if (currentPanel != null)
		{
			((Control)pnMain).Controls.Add((Control)(object)currentPanel);
		}
		for (int i = 0; i < ((ArrangedElementCollection)((Control)pnMain).Controls).Count; i++)
		{
			if (!((object)((Control)pnMain).Controls[i]).Equals((object?)currentPanel) && ((Control)pnMain).Controls[i] is Panel)
			{
				((Control)pnMain).Controls[i].Visible = false;
			}
		}
		((Control)currentPanel).Dock = (DockStyle)5;
		((Control)currentPanel).BringToFront();
		UpdatePanelControls(kind);
	}

	private void UpdateImages()
	{
		TreeNode folderNode = GetFolderNode();
		if (folderNode == rootNode)
		{
			generalNode.ImageIndex = 2;
		}
		else
		{
			generalNode.ImageIndex = 3;
		}
		if (folderNode == stylesNode)
		{
			stylesNode.ImageIndex = 0;
		}
		else
		{
			stylesNode.ImageIndex = 1;
		}
		if (folderNode == statesNode)
		{
			statesNode.ImageIndex = 0;
		}
		else
		{
			statesNode.ImageIndex = 1;
		}
		if (stylesNode.Nodes.Count > 0)
		{
			if (stylesNode == tvLexer.SelectedNode)
			{
				stylesNode.Nodes[0].ImageIndex = 2;
			}
			else
			{
				stylesNode.Nodes[0].ImageIndex = 3;
			}
		}
	}

	private void UpdateTree(TreeNode node)
	{
		updating = true;
		try
		{
			UpdateImages();
			UpdateVisible(GetNodeKind(node));
		}
		finally
		{
			updating = false;
		}
	}

	private void Updatereswords()
	{
		ILexReswordSet lexreswordSet = GetLexreswordSet(tvLexer.SelectedNode, Scheme);
		if (lexreswordSet != null)
		{
			lexreswordSet.Reswords = new string[0];
			string[] lines = ((TextBoxBase)tbreswords).Lines;
			foreach (string text in lines)
			{
				string text2 = text.Trim();
				if (text2 != string.Empty && !lexreswordSet.FindResword(text2))
				{
					lexreswordSet.AddResword(text2);
				}
			}
		}
		((Control)cbreswordStyle).Enabled = lexreswordSet != null && lexreswordSet.Reswords.Length != 0;
	}

	private void UpdateBlockExpressions()
	{
		ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
		if (lexSyntaxBlock == null)
		{
			return;
		}
		lexSyntaxBlock.Expressions = new string[0];
		string[] lines = ((TextBoxBase)tbExpressions).Lines;
		foreach (string text in lines)
		{
			string text2 = text.Trim();
			if (text2 != string.Empty)
			{
				lexSyntaxBlock.AddExpression(text2);
			}
		}
	}

	private void UpdateLexStyles()
	{
		cbStyle.Items.Clear();
		cbreswordStyle.Items.Clear();
		for (int i = 0; i < Scheme.LexStyles.Count; i++)
		{
			if (Scheme.LexStyles[i].Name != string.Empty)
			{
				cbStyle.Items.Add((object)Scheme.LexStyles[i].Name);
				cbreswordStyle.Items.Add((object)Scheme.LexStyles[i].Name);
			}
			else
			{
				cbStyle.Items.Add((object)"");
				cbreswordStyle.Items.Add((object)"");
			}
		}
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		((Control)tbStyleName).Text = ((lexStyle != null) ? lexStyle.Name : string.Empty);
	}

	private void UpdateStates(int index)
	{
		cbLeaveState.Items.Clear();
		for (int i = 0; i < Scheme.LexStates.Count; i++)
		{
			if (Scheme.LexStates[i].Name != null)
			{
				cbLeaveState.Items.Add((object)Scheme.LexStates[i].Name);
			}
			else
			{
				cbLeaveState.Items.Add((object)"");
			}
		}
		if (cbLeaveState.Items.Count > index)
		{
			((ListControl)cbLeaveState).SelectedIndex = index;
		}
		ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
		((Control)tbStateName).Text = ((lexState != null) ? lexState.Name : string.Empty);
	}

	private void UpdateFontStyle()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		NodeKind nodeKind = GetNodeKind(tvLexer.SelectedNode);
		if (nodeKind != NodeKind.nkStyle && nodeKind != NodeKind.nkStyles)
		{
			return;
		}
		updating = true;
		try
		{
			ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
			if (lexStyle != null)
			{
				lexStyle.FontStyle = (FontStyle)0;
				if (chbBold.Checked)
				{
					lexStyle.FontStyle = (FontStyle)(lexStyle.FontStyle | 1);
				}
				if (chbItalic.Checked)
				{
					lexStyle.FontStyle = (FontStyle)(lexStyle.FontStyle | 2);
				}
				if (chbUnderline.Checked)
				{
					lexStyle.FontStyle = (FontStyle)(lexStyle.FontStyle | 4);
				}
				if (chbStrikeout.Checked)
				{
					lexStyle.FontStyle = (FontStyle)(lexStyle.FontStyle | 8);
				}
			}
		}
		finally
		{
			updating = false;
		}
		UpdateSample();
	}

	private void UpdateSample()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		if (lexStyle != null)
		{
			((Control)laSample).Font = new Font(((Control)laSample).Font.Name, ((Control)laSample).Font.Size, lexStyle.FontStyle);
		}
		else
		{
			((Control)laSample).Font = new Font(((Control)laSample).Font.Name, ((Control)laSample).Font.Size, (FontStyle)0);
		}
		((Control)laSample).ForeColor = clbForeColor.SelectedColor;
		((Control)laSample).BackColor = clbBkColor.SelectedColor;
		int x = (((Control)pnSample).Width - ((Control)laSample).Width) / 2;
		int y = (((Control)pnSample).Height - ((Control)laSample).Height) / 2;
		((Control)laSample).Location = new Point(x, y);
	}

	private void ClearScheme()
	{
		Scheme.ClearScheme();
		statesNode.Nodes.Clear();
		stylesNode.Nodes.Clear();
		((Control)this).Text = SyntaxBuilderConsts.SyntaxFormCaption;
	}

	private void LoadScheme()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog).ShowDialog((IWin32Window)(object)this) == 1)
		{
			ClearScheme();
			lexer.LoadScheme(((FileDialog)openFileDialog).FileName);
			((Control)this).Text = SyntaxBuilderConsts.SyntaxFormCaption + " " + ((FileDialog)openFileDialog).FileName;
			UpdateScheme();
		}
	}

	private void SaveScheme()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		if ((IsSchemeComplete(Scheme, out var errorMsg) || (int)MessageBox.Show(SyntaxBuilderConsts.InvalidScheme + ": " + errorMsg + "\n" + SyntaxBuilderConsts.QueryInvalidScheme, StringConsts.ErrorCaption, (MessageBoxButtons)1, (MessageBoxIcon)48) == 1) && (int)((CommonDialog)saveFileDialog).ShowDialog((IWin32Window)(object)this) == 1)
		{
			lexer.SaveScheme(((FileDialog)saveFileDialog).FileName);
			((Control)this).Text = SyntaxBuilderConsts.SyntaxFormCaption + " " + ((FileDialog)saveFileDialog).FileName;
		}
	}

	private void UpdateScheme()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		cbStyle.Items.Clear();
		cbreswordStyle.Items.Clear();
		cbLeaveState.Items.Clear();
		stylesNode.Nodes.Clear();
		statesNode.Nodes.Clear();
		for (int i = 0; i < Scheme.LexStyles.Count; i++)
		{
			stylesNode.Nodes.Add(new TreeNode(Scheme.LexStyles[i].Name));
			cbStyle.Items.Add((object)Scheme.LexStyles[i].Name);
			cbreswordStyle.Items.Add((object)Scheme.LexStyles[i].Name);
		}
		for (int j = 0; j < Scheme.LexStates.Count; j++)
		{
			statesNode.Nodes.Add(new TreeNode(Scheme.LexStates[j].Name, 4, 4));
			TreeNode val = statesNode.Nodes[j];
			for (int k = 0; k < Scheme.LexStates[j].LexSyntaxBlocks.Count; k++)
			{
				val.Nodes.Add(new TreeNode(Scheme.LexStates[j].LexSyntaxBlocks[k].Name));
				for (int l = 0; l < Scheme.LexStates[j].LexSyntaxBlocks[k].LexReswordSets.Count; l++)
				{
					string text = Scheme.LexStates[j].LexSyntaxBlocks[k].LexReswordSets[l].Name;
					if (text == null || text == string.Empty || text.Trim() == "")
					{
						text = SyntaxBuilderConsts.ReswordSetText + (l + 1);
					}
					val.Nodes[k].Nodes.Add(new TreeNode(text));
				}
			}
			cbLeaveState.Items.Add((object)Scheme.LexStates[j].Name);
		}
		tvLexer.CollapseAll();
		tvLexer.SelectedNode = generalNode;
	}

	private void AddStateClick(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		string newStateName = GetNewStateName();
		ILexState lexState = Scheme.LexStates.AddLexState();
		TreeNode val = new TreeNode(newStateName);
		val.ImageIndex = 4;
		val.SelectedImageIndex = 4;
		statesNode.Nodes.Add(val);
		statesNode.Expand();
		tvLexer.SelectedNode = statesNode.LastNode;
		lexState.Name = newStateName;
		UpdateStates(tvLexer.SelectedNode.Index);
	}

	private void DeleteStateClick(object sender, EventArgs e)
	{
		ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
		TreeNode stateNode = GetStateNode(tvLexer.SelectedNode);
		stateNode.Remove();
		if (lexState != null)
		{
			Scheme.LexStates.Remove(lexState);
		}
		stateNode = GetStateNode(tvLexer.SelectedNode);
		UpdateStates((stateNode != null) ? stateNode.Index : (-1));
	}

	private void AddStyleClick(object sender, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		string newStyleName = GetNewStyleName();
		ILexStyle lexStyle = Scheme.LexStyles.AddLexStyle();
		lexStyle.FontStyle = (FontStyle)0;
		lexStyle.ForeColor = Color.Black;
		stylesNode.Nodes.Add(new TreeNode(newStyleName));
		stylesNode.Expand();
		tvLexer.SelectedNode = stylesNode.LastNode;
		lexStyle.Name = newStyleName;
		UpdateLexStyles();
	}

	private void DeleteStyleClick(object sender, EventArgs e)
	{
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		TreeNode styleNode = GetStyleNode(tvLexer.SelectedNode);
		if (styleNode != null)
		{
			styleNode.Remove();
		}
		if (lexStyle != null)
		{
			Scheme.LexStyles.Remove(lexStyle);
		}
		if (!((Control)tvLexer).Focused)
		{
			((Control)tvLexer).Focus();
		}
		UpdateLexStyles();
	}

	private void AddreswordSetClick(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
		string newReswordSetName = GetNewReswordSetName(lexSyntaxBlock);
		ILexReswordSet lexReswordSet = lexSyntaxBlock.LexReswordSets.AddLexReswordSet();
		lexReswordSet.Name = newReswordSetName;
		TreeNode syntaxBlockNode = GetSyntaxBlockNode(tvLexer.SelectedNode);
		if (syntaxBlockNode != null)
		{
			syntaxBlockNode.Nodes.Add(new TreeNode(newReswordSetName));
		}
		syntaxBlockNode.Expand();
		tvLexer.SelectedNode = syntaxBlockNode.LastNode;
	}

	private void DeletereswordSetClick(object sender, EventArgs e)
	{
		ILexReswordSet lexreswordSet = GetLexreswordSet(tvLexer.SelectedNode, Scheme);
		tvLexer.SelectedNode.Remove();
		if (lexreswordSet != null)
		{
			LexSyntaxBlock lexSyntaxBlock = (LexSyntaxBlock)GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
			lexSyntaxBlock.LexReswordSets.Remove(lexreswordSet);
		}
	}

	private void AddBlockClick(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
		string newSyntaxBlockName = GetNewSyntaxBlockName(lexState);
		ILexSyntaxBlock lexSyntaxBlock = lexState.LexSyntaxBlocks.AddLexSyntaxBlock();
		lexSyntaxBlock.Name = newSyntaxBlockName;
		TreeNode stateNode = GetStateNode(tvLexer.SelectedNode);
		if (stateNode != null)
		{
			stateNode.Nodes.Add(new TreeNode(newSyntaxBlockName));
		}
		stateNode.Expand();
		tvLexer.SelectedNode = stateNode.LastNode;
	}

	private void DeleteBlockClick(object sender, EventArgs e)
	{
		ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
		tvLexer.SelectedNode.Remove();
		if (lexSyntaxBlock != null)
		{
			LexState lexState = (LexState)GetLexState(tvLexer.SelectedNode, Scheme);
			lexState.LexSyntaxBlocks.Remove(lexSyntaxBlock);
		}
	}

	private string GetNewStyleName()
	{
		string[] array = new string[Scheme.LexStyles.Count];
		for (int i = 0; i < Scheme.LexStyles.Count; i++)
		{
			array[i] = Scheme.LexStyles[i].Name;
		}
		int firstNumber = GetFirstNumber(array, SyntaxBuilderConsts.StyleText);
		return SyntaxBuilderConsts.StyleText + firstNumber;
	}

	private string GetNewStateName()
	{
		string[] array = new string[Scheme.LexStates.Count];
		for (int i = 0; i < Scheme.LexStates.Count; i++)
		{
			array[i] = Scheme.LexStates[i].Name;
		}
		int firstNumber = GetFirstNumber(array, SyntaxBuilderConsts.StateText);
		return SyntaxBuilderConsts.StateText + firstNumber;
	}

	private string GetNewSyntaxBlockName(ILexState lexState)
	{
		ILexSyntaxBlocks lexSyntaxBlocks = lexState.LexSyntaxBlocks;
		string[] array = new string[lexSyntaxBlocks.Count];
		for (int i = 0; i < lexSyntaxBlocks.Count; i++)
		{
			array[i] = lexSyntaxBlocks[i].Name;
		}
		int firstNumber = GetFirstNumber(array, SyntaxBuilderConsts.BlockText);
		return SyntaxBuilderConsts.BlockText + firstNumber;
	}

	private string GetNewReswordSetName(ILexSyntaxBlock lexBlock)
	{
		ILexReswordSets lexReswordSets = lexBlock.LexReswordSets;
		string[] array = new string[lexReswordSets.Count];
		for (int i = 0; i < lexReswordSets.Count; i++)
		{
			array[i] = lexReswordSets[i].Name;
		}
		int firstNumber = GetFirstNumber(array, SyntaxBuilderConsts.ReswordSetText);
		return SyntaxBuilderConsts.ReswordSetText + firstNumber;
	}

	private int GetFirstNumber(string[] list, string start)
	{
		List<int> list2 = new List<int>();
		foreach (string text in list)
		{
			if (text != null && text.StartsWith(start))
			{
				string s = text.Substring(start.Length);
				try
				{
					int item = int.Parse(s);
					list2.Add(item);
				}
				catch
				{
				}
			}
		}
		list2.Sort();
		int num = 0;
		for (int j = 0; j < list2.Count; j++)
		{
			if (list2[j] == num)
			{
				num++;
			}
		}
		return num;
	}

	private int GetFirstNumber(List<int> list)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == num)
			{
				num++;
			}
		}
		return num;
	}

	private string GetSampleExpression(string text)
	{
		string text2 = text;
		int num = text2.IndexOf(" ");
		if (num >= 0)
		{
			text2 = text2.Remove(0, num);
			text2 = text2.Trim();
		}
		return text2;
	}

	private Panel GetCurrentPanel(NodeKind kind)
	{
		switch (kind)
		{
		case NodeKind.nkStates:
		case NodeKind.nkState:
		case NodeKind.nkSyntaxBlock:
		case NodeKind.nkreswordSet:
			return pnStates;
		case NodeKind.nkStyles:
		case NodeKind.nkStyle:
			return pnStyles;
		case NodeKind.nkGeneral:
			return pnGeneral;
		default:
			return pnGeneral;
		}
	}

	private int GetreswordStyleIndex(ILexReswordSet reswordSet)
	{
		if (reswordSet != null && reswordSet.ReswordStyle != null)
		{
			for (int i = 0; i < cbreswordStyle.Items.Count; i++)
			{
				if (string.Compare(cbreswordStyle.Items[i].ToString(), reswordSet.ReswordStyle.Name) == 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int GetLeaveStateIndex(ILexSyntaxBlock block)
	{
		if (block != null && block.LeaveState != null)
		{
			for (int i = 0; i < cbLeaveState.Items.Count; i++)
			{
				if (string.Compare(cbLeaveState.Items[i].ToString(), block.LeaveState.Name) == 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int GetStyleIndex(ILexSyntaxBlock block)
	{
		if (block != null && block.Style != null)
		{
			for (int i = 0; i < cbStyle.Items.Count; i++)
			{
				if (string.Compare(cbStyle.Items[i].ToString(), block.Style.Name) == 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private TreeNode GetParent(TreeNode node)
	{
		TreeNode val = node;
		while (val.Parent != null)
		{
			val = val.Parent;
		}
		return val;
	}

	private TreeNode GetFolderNode()
	{
		TreeNode val = tvLexer.SelectedNode;
		while (val != null)
		{
			if (val.ImageIndex != 0 && val.ImageIndex != 1)
			{
				val = val.Parent;
				continue;
			}
			return val;
		}
		return val;
	}

	private bool IsStyleUsed()
	{
		bool result = false;
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		if (lexStyle != null)
		{
			for (int i = 0; i < Scheme.LexStates.Count; i++)
			{
				for (int j = 0; j < Scheme.LexStates[i].LexSyntaxBlocks.Count; j++)
				{
					if (lexStyle.Equals(Scheme.LexStates[i].LexSyntaxBlocks[j].Style))
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	private bool IsStateUsed()
	{
		bool result = false;
		ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
		if (lexState != null)
		{
			for (int i = 0; i < Scheme.LexStates.Count; i++)
			{
				if (lexState == Scheme.LexStates[i])
				{
					continue;
				}
				for (int j = 0; j < Scheme.LexStates[i].LexSyntaxBlocks.Count; j++)
				{
					if (lexState.Equals(Scheme.LexStates[i].LexSyntaxBlocks[j].LeaveState))
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	private void btColor_Click(object sender, EventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Invalid comparison between Unknown and I4
		if (GetNodeKind(tvLexer.SelectedNode) == NodeKind.nkStyle)
		{
			ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
			colorDialog1.FullOpen = true;
			colorDialog1.Color = lexStyle.ForeColor;
			colorDialog1.CustomColors = StiColorDialogHelper.Load();
			if ((int)((CommonDialog)colorDialog1).ShowDialog() == 1)
			{
				StiColorDialogHelper.Save(colorDialog1.CustomColors);
				lexStyle.ForeColor = colorDialog1.Color;
			}
		}
	}

	private void MainForm_Load(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		TreeNode val = new TreeNode(sRemoveScroll);
		tvLexer.Nodes.Add(val);
		tvLexer.Nodes.Remove(val);
		((Control)tbExpressions).ContextMenuStrip = cmExpressions;
		tvLexer.SelectedNode = rootNode;
		rootNode.Expand();
		((FileDialog)saveFileDialog).Filter = "xml files (*.xml)|*.xml";
		((FileDialog)openFileDialog).Filter = "Scheme files (*.xml)|*.xml";
		LoadFromResource();
		UpdateEvents(update: true);
	}

	private void UpdateEvents(bool update)
	{
		if (update)
		{
			cbStyle.SelectedIndexChanged += cbStyle_SelectedIndexChanged;
			cbreswordStyle.SelectedIndexChanged += cbreswordStyle_SelectedIndexChanged;
			cbLeaveState.SelectedIndexChanged += cbLeaveState_SelectedIndexChanged;
			chbBold.CheckedChanged += chbBold_CheckedChanged;
			chbItalic.CheckedChanged += chbItalic_CheckedChanged;
			chbUnderline.CheckedChanged += chbUnderline_CheckedChanged;
			chbStrikeout.CheckedChanged += chbStrikeout_CheckedChanged;
			chbPlainText.CheckedChanged += chbPlainText_CheckedChanged;
			((ComboBox)clbBkColor).SelectedIndexChanged += clbBkColor_SelectedIndexChanged;
			((ComboBox)clbForeColor).SelectedIndexChanged += clbForeColor_SelectedIndexChanged;
			chbCaseSensitive.CheckedChanged += chbCaseSensitive_TextChanged;
			((Control)tbAuthor).TextChanged += tbAuthor_TextChanged;
			((Control)tbDescription).TextChanged += tbDescription_TextChanged;
			((Control)tbCopyright).TextChanged += tbCopyright_TextChanged;
			((Control)tbFileType).TextChanged += tbFileType_TextChanged;
			((Control)tbFileExt).TextChanged += tbFileExt_TextChanged;
			((Control)tbSchemeName).TextChanged += tbSchemeName_TextChanged;
			((Control)tbStateDesc).TextChanged += tbStateDesc_TextChanged;
			((Control)tbStyleName).TextChanged += tbStyleName_TextChanged;
			((Control)tbStateName).TextChanged += tbStateName_TextChanged;
			((Control)tbBlockName).TextChanged += tbBlockName_TextChanged;
			((Control)tbreswordSetName).TextChanged += tbreswordSetName_TextChanged;
			((Control)tbreswords).TextChanged += tbreswords_TextChanged;
			((Control)tbExpressions).TextChanged += tbExpressions_TextChanged;
			((Control)tbStyleDesc).TextChanged += tbStyleDesc_TextChanged;
			((Control)tbBlockDesc).TextChanged += tbBlockDesc_TextChanged;
		}
		else
		{
			cbStyle.SelectedIndexChanged -= cbStyle_SelectedIndexChanged;
			cbreswordStyle.SelectedIndexChanged -= cbreswordStyle_SelectedIndexChanged;
			cbLeaveState.SelectedIndexChanged -= cbLeaveState_SelectedIndexChanged;
			chbBold.CheckedChanged -= chbBold_CheckedChanged;
			chbItalic.CheckedChanged -= chbItalic_CheckedChanged;
			chbUnderline.CheckedChanged -= chbUnderline_CheckedChanged;
			chbStrikeout.CheckedChanged -= chbStrikeout_CheckedChanged;
			chbPlainText.CheckedChanged -= chbPlainText_CheckedChanged;
			((ComboBox)clbBkColor).SelectedIndexChanged -= clbBkColor_SelectedIndexChanged;
			((ComboBox)clbForeColor).SelectedIndexChanged -= clbForeColor_SelectedIndexChanged;
			chbCaseSensitive.CheckedChanged -= chbCaseSensitive_TextChanged;
			((Control)tbAuthor).TextChanged -= tbAuthor_TextChanged;
			((Control)tbDescription).TextChanged -= tbDescription_TextChanged;
			((Control)tbCopyright).TextChanged -= tbCopyright_TextChanged;
			((Control)tbFileType).TextChanged -= tbFileType_TextChanged;
			((Control)tbFileExt).TextChanged -= tbFileExt_TextChanged;
			((Control)tbSchemeName).TextChanged -= tbSchemeName_TextChanged;
			((Control)tbStateDesc).TextChanged -= tbStateDesc_TextChanged;
			((Control)tbStyleName).TextChanged -= tbStyleName_TextChanged;
			((Control)tbStateName).TextChanged -= tbStateName_TextChanged;
			((Control)tbBlockName).TextChanged -= tbBlockName_TextChanged;
			((Control)tbreswordSetName).TextChanged -= tbreswordSetName_TextChanged;
			((Control)tbreswords).TextChanged -= tbreswords_TextChanged;
			((Control)tbExpressions).TextChanged -= tbExpressions_TextChanged;
			((Control)tbStyleDesc).TextChanged -= tbStyleDesc_TextChanged;
			((Control)tbBlockDesc).TextChanged -= tbBlockDesc_TextChanged;
		}
	}

	private void LoadFromResource()
	{
		((Control)this).Text = SyntaxBuilderConsts.DlgSyntaxBuilderCaption;
		if (tvLexer.Nodes.Count > 0)
		{
			tvLexer.Nodes[0].Text = SyntaxBuilderConsts.SyntaxSchemeEditorCaption;
			if (tvLexer.Nodes[0].Nodes.Count >= 3)
			{
				tvLexer.Nodes[0].Nodes[0].Text = SyntaxBuilderConsts.SyntaxSchemeGeneralCaption;
				tvLexer.Nodes[0].Nodes[1].Text = SyntaxBuilderConsts.SyntaxSchemeStylesCaption;
				tvLexer.Nodes[0].Nodes[2].Text = SyntaxBuilderConsts.SyntaxSchemeStatesCaption;
			}
			((Control)tpGeneral).Text = SyntaxBuilderConsts.GeneralCaption_SyntaxBuilderDlg;
			((Control)tpStyles).Text = SyntaxBuilderConsts.StylesCaption;
			((Control)tpStates).Text = SyntaxBuilderConsts.StatesCaption;
			((Control)laAuthor).Text = SyntaxBuilderConsts.AuthorCaption;
			((Control)laSchemeName).Text = SyntaxBuilderConsts.SchemeNameCaption;
			((Control)laDescription).Text = SyntaxBuilderConsts.DescriptionCaption_SyntaxBuilderDlg;
			((Control)laCopyright).Text = SyntaxBuilderConsts.CopyrightCaption;
			((Control)gbStyleProperties).Text = SyntaxBuilderConsts.StylePropertiesCaption;
			((Control)laStyleName).Text = SyntaxBuilderConsts.StyleNameCaption;
			((Control)laStyleDesc).Text = SyntaxBuilderConsts.StyleDescriptionCaption;
			((Control)laStyleForeColor).Text = SyntaxBuilderConsts.StyleForeColorCaption;
			((Control)laStyleBkColor).Text = SyntaxBuilderConsts.StyleBackColorCaption;
			((Control)gbFontStyles).Text = SyntaxBuilderConsts.FontStylesCaption;
			((Control)chbBold).Text = SyntaxBuilderConsts.BoldCaption_SyntaxBuilderDlg;
			((Control)chbItalic).Text = SyntaxBuilderConsts.ItalicCaption_SyntaxBuilderDlg;
			((Control)chbUnderline).Text = SyntaxBuilderConsts.UnderlineCaption_SyntaxBuilderDlg;
			((Control)chbStrikeout).Text = SyntaxBuilderConsts.StrikeOutCaption_SyntaxBuilderDlg;
			((Control)chbPlainText).Text = SyntaxBuilderConsts.PlainTextCaption;
			((Control)laSample).Text = SyntaxBuilderConsts.SampleCaption_SyntaxBuilderDlg;
			((Control)gbStateProperties).Text = SyntaxBuilderConsts.StatePropertiesCaption;
			((Control)laStateName).Text = SyntaxBuilderConsts.StateNameCaption;
			((Control)laStateDesc).Text = SyntaxBuilderConsts.StateDescriptionCaption;
			((Control)chbCaseSensitive).Text = SyntaxBuilderConsts.CaseSensitiveCaption;
			((Control)gbSyntaxBlockProperties).Text = SyntaxBuilderConsts.BlockPropertiesCaption;
			((Control)laBlockName).Text = SyntaxBuilderConsts.BlockNameCaption;
			((Control)laBlockDesc).Text = SyntaxBuilderConsts.BlockDescriptionCaption;
			((Control)laLeaveState).Text = SyntaxBuilderConsts.LeaveStateCaption;
			((Control)laStyle).Text = SyntaxBuilderConsts.BlockStyleCaption;
			((Control)laBlockExpr).Text = SyntaxBuilderConsts.BlockExpressionCaption;
			((Control)gbreswordsProperties).Text = SyntaxBuilderConsts.ReswordSetPropertiesCaption;
			((Control)lareswordSetName).Text = SyntaxBuilderConsts.ReswordSetNameCaption;
			((Control)lareswordStyle).Text = SyntaxBuilderConsts.ReswordStyleCaption;
			((Control)laBlockreswords).Text = SyntaxBuilderConsts.ReswordsCaption;
			((Control)btLoadScheme).Text = SyntaxBuilderConsts.LoadCaption;
			((Control)btSave).Text = SyntaxBuilderConsts.SaveCaption;
			((Control)btClear).Text = SyntaxBuilderConsts.ClearCaption;
			((Control)btOk).Text = SyntaxBuilderConsts.OKCaption_SyntaxBuilderDlg;
			((Control)btCancel).Text = SyntaxBuilderConsts.CancelCaption_SyntaxBuilderDlg;
		}
	}

	private void tbStyleName_TextChanged(object sender, EventArgs e)
	{
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		if (lexStyle != null)
		{
			TreeNode styleNode = GetStyleNode(tvLexer.SelectedNode);
			if (styleNode != null)
			{
				styleNode.Text = ((Control)tbStyleName).Text;
			}
			lexStyle.Name = ((Control)tbStyleName).Text;
			UpdateLexStyles();
		}
	}

	private void tbStateName_TextChanged(object sender, EventArgs e)
	{
		ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
		if (lexState != null)
		{
			TreeNode stateNode = GetStateNode(tvLexer.SelectedNode);
			if (stateNode != null)
			{
				stateNode.Text = ((Control)tbStateName).Text;
			}
			lexState.Name = ((Control)tbStateName).Text;
			UpdateStates((stateNode != null) ? stateNode.Index : (-1));
		}
	}

	private void tbExpressions_TextChanged(object sender, EventArgs e)
	{
		UpdateBlockExpressions();
	}

	private void tbreswords_TextChanged(object sender, EventArgs e)
	{
		Updatereswords();
	}

	private void tbBlockDesc_TextChanged(object sender, EventArgs e)
	{
		ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
		if (lexSyntaxBlock != null)
		{
			lexSyntaxBlock.Desc = ((Control)tbBlockDesc).Text;
		}
	}

	private void chbCaseSensitive_TextChanged(object sender, EventArgs e)
	{
		ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
		if (lexState != null)
		{
			lexState.CaseSensitive = chbCaseSensitive.Checked;
		}
	}

	private void tbStateDesc_TextChanged(object sender, EventArgs e)
	{
		ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
		if (lexState != null)
		{
			lexState.Desc = ((Control)tbStateDesc).Text;
		}
	}

	private void tbStyleDesc_TextChanged(object sender, EventArgs e)
	{
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		if (lexStyle != null)
		{
			lexStyle.Desc = ((Control)tbStyleDesc).Text;
		}
	}

	private void clbForeColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		if (lexStyle != null)
		{
			lexStyle.ForeColor = clbForeColor.SelectedColor;
		}
		UpdateSample();
	}

	private void clbBkColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		if (lexStyle != null)
		{
			lexStyle.BackColor = clbBkColor.SelectedColor;
		}
		UpdateSample();
	}

	private void chbBold_CheckedChanged(object sender, EventArgs e)
	{
		if (!updating)
		{
			UpdateFontStyle();
		}
	}

	private void chbUnderline_CheckedChanged(object sender, EventArgs e)
	{
		if (!updating)
		{
			UpdateFontStyle();
		}
	}

	private void chbItalic_CheckedChanged(object sender, EventArgs e)
	{
		if (!updating)
		{
			UpdateFontStyle();
		}
	}

	private void chbStrikeout_CheckedChanged(object sender, EventArgs e)
	{
		if (!updating)
		{
			UpdateFontStyle();
		}
	}

	private void chbPlainText_CheckedChanged(object sender, EventArgs e)
	{
		ILexStyle lexStyle = GetLexStyle(tvLexer.SelectedNode, Scheme);
		if (lexStyle != null)
		{
			lexStyle.PlainText = chbPlainText.Checked;
		}
	}

	private void chbCaseSensitive_CheckedChanged(object sender, EventArgs e)
	{
		if (!updating)
		{
			NodeKind nodeKind = GetNodeKind(tvLexer.SelectedNode);
			if (nodeKind == NodeKind.nkState || nodeKind == NodeKind.nkSyntaxBlock)
			{
				ILexState lexState = GetLexState(tvLexer.SelectedNode, Scheme);
				lexState.CaseSensitive = chbCaseSensitive.Checked;
			}
		}
	}

	private void btSave_Click(object sender, EventArgs e)
	{
		SaveScheme();
	}

	private void btClear_Click(object sender, EventArgs e)
	{
		ClearScheme();
	}

	private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!updating)
		{
			ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
			if (lexSyntaxBlock != null && ((ListControl)cbStyle).SelectedIndex >= 0)
			{
				lexSyntaxBlock.Style = Scheme.LexStyles[((ListControl)cbStyle).SelectedIndex];
			}
		}
	}

	private void cbLeaveState_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!updating)
		{
			ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
			if (lexSyntaxBlock != null && ((ListControl)cbLeaveState).SelectedIndex >= 0)
			{
				lexSyntaxBlock.LeaveState = Scheme.LexStates[((ListControl)cbLeaveState).SelectedIndex];
			}
		}
	}

	private void tvLexer_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		if ((int)e.Button != 2097152)
		{
			return;
		}
		TreeNode nodeAt = tvLexer.GetNodeAt(e.X, e.Y);
		if (nodeAt != null)
		{
			tvLexer.SelectedNode = nodeAt;
		}
		((ToolStrip)cmLexer).Items.Clear();
		NodeKind nodeKind = GetNodeKind(tvLexer.SelectedNode);
		switch (nodeKind)
		{
		case NodeKind.nkStates:
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StateText, (Image)null, (EventHandler)AddStateClick));
			break;
		case NodeKind.nkStyle:
		{
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StyleText, (Image)null, (EventHandler)AddStyleClick));
			ToolStripMenuItem val2 = new ToolStripMenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.StyleText, (Image)null, (EventHandler)DeleteStyleClick);
			((ToolStripItem)val2).Enabled = !IsStyleUsed();
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)(object)val2);
			break;
		}
		case NodeKind.nkStyles:
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StyleText, (Image)null, (EventHandler)AddStyleClick));
			break;
		case NodeKind.nkState:
		{
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StateText, (Image)null, (EventHandler)AddStateClick));
			ToolStripMenuItem val = new ToolStripMenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.StateText, (Image)null, (EventHandler)DeleteStateClick);
			((ToolStripItem)val).Enabled = !IsStateUsed();
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)(object)val);
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.BlockText, (Image)null, (EventHandler)AddBlockClick));
			break;
		}
		case NodeKind.nkSyntaxBlock:
		case NodeKind.nkreswordSet:
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.BlockText, (Image)null, (EventHandler)AddBlockClick));
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.BlockText, (Image)null, (EventHandler)DeleteBlockClick));
			((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.ReswordSetText, (Image)null, (EventHandler)AddreswordSetClick));
			if (nodeKind == NodeKind.nkreswordSet)
			{
				((ToolStrip)cmLexer).Items.Add((ToolStripItem)new ToolStripMenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.ReswordSetText, (Image)null, (EventHandler)DeletereswordSetClick));
			}
			break;
		}
		((ToolStripDropDown)cmLexer).Show((Control)(object)tvLexer, new Point(e.X, e.Y));
	}

	private void cbreswordStyle_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!updating)
		{
			ILexReswordSet lexreswordSet = GetLexreswordSet(tvLexer.SelectedNode, Scheme);
			if (lexreswordSet != null && ((ListControl)cbreswordStyle).SelectedIndex >= 0)
			{
				lexreswordSet.ReswordStyle = Scheme.LexStyles[((ListControl)cbreswordStyle).SelectedIndex];
			}
		}
	}

	private void btLoadScheme_Click(object sender, EventArgs e)
	{
		LoadScheme();
	}

	private void tbAuthor_TextChanged(object sender, EventArgs e)
	{
		Scheme.Author = ((Control)tbAuthor).Text;
	}

	private void tbSchemeName_TextChanged(object sender, EventArgs e)
	{
		Scheme.Name = ((Control)tbSchemeName).Text;
	}

	private void tbDescription_TextChanged(object sender, EventArgs e)
	{
		Scheme.Desc = ((Control)tbDescription).Text;
	}

	private void tbCopyright_TextChanged(object sender, EventArgs e)
	{
		Scheme.Copyright = ((Control)tbCopyright).Text;
	}

	private void tbFileType_TextChanged(object sender, EventArgs e)
	{
		Scheme.FileType = ((Control)tbFileType).Text;
	}

	private void tbFileExt_TextChanged(object sender, EventArgs e)
	{
		Scheme.FileExtension = ((Control)tbFileExt).Text;
	}

	private void tvLexer_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
	{
		int nodeLevel = GetNodeLevel(e.Node);
		e.CancelEdit = nodeLevel <= 1;
	}

	private void tvLexer_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
	{
		if (e.Label == null)
		{
			return;
		}
		switch (GetNodeKind(e.Node))
		{
		case NodeKind.nkStyle:
			Scheme.LexStyles[e.Node.Index].Name = e.Label;
			UpdateLexStyles();
			break;
		case NodeKind.nkState:
			Scheme.LexStates[e.Node.Index].Name = e.Label;
			UpdateStates(e.Node.Index);
			break;
		case NodeKind.nkSyntaxBlock:
		{
			ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
			if (lexSyntaxBlock != null)
			{
				lexSyntaxBlock.Name = e.Label;
			}
			break;
		}
		case NodeKind.nkStates:
			break;
		}
	}

	private void tvLexer_AfterCollapse(object sender, TreeViewEventArgs e)
	{
		if (((object)e.Node).Equals((object?)stylesNode) || ((object)e.Node).Equals((object?)statesNode))
		{
			e.Node.SelectedImageIndex = 1;
		}
	}

	private void tvLexer_AfterExpand(object sender, TreeViewEventArgs e)
	{
		if (((object)e.Node).Equals((object?)stylesNode) || ((object)e.Node).Equals((object?)statesNode))
		{
			e.Node.SelectedImageIndex = 0;
		}
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void DoExit()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if ((int)((Form)this).DialogResult == 2 && (int)MessageBox.Show(SyntaxBuilderConsts.CancelText, SyntaxBuilderConsts.CancelCaption, (MessageBoxButtons)4, (MessageBoxIcon)32) != 6)
		{
			((Form)this).DialogResult = (DialogResult)0;
		}
	}

	private void btCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void tvLexer_AfterSelect(object sender, TreeViewEventArgs e)
	{
		UpdateTree(e.Node);
	}

	private void btStyleEdit_Click(object sender, EventArgs e)
	{
		((Control)tbStyleName).Focus();
	}

	private void btStateEdit_Click(object sender, EventArgs e)
	{
		((Control)tbStateName).Focus();
	}

	private void btBlockEdit_Click(object sender, EventArgs e)
	{
		((Control)tbBlockName).Focus();
	}

	private void btreswordSetEdit_Click(object sender, EventArgs e)
	{
		((Control)tbreswordSetName).Focus();
	}

	private void tbBlockName_TextChanged(object sender, EventArgs e)
	{
		ILexSyntaxBlock lexSyntaxBlock = GetLexSyntaxBlock(tvLexer.SelectedNode, Scheme);
		if (lexSyntaxBlock != null)
		{
			GetSyntaxBlockNode(tvLexer.SelectedNode).Text = ((Control)tbBlockName).Text;
			lexSyntaxBlock.Name = ((Control)tbBlockName).Text;
		}
	}

	private void tbreswordSetName_TextChanged(object sender, EventArgs e)
	{
		ILexReswordSet lexreswordSet = GetLexreswordSet(tvLexer.SelectedNode, Scheme);
		if (lexreswordSet != null)
		{
			tvLexer.SelectedNode.Text = ((Control)tbreswordSetName).Text;
			lexreswordSet.Name = ((Control)tbreswordSetName).Text;
		}
	}

	private void DlgSyntaxBuilder_Closing(object sender, CancelEventArgs e)
	{
		DoExit();
	}

	private void menuItem5_Click(object sender, EventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		string sampleExpression = GetSampleExpression(((ToolStripItem)(ToolStripMenuItem)sender).Text);
		((Control)tbExpressions).Text = ((((Control)tbExpressions).Text == string.Empty) ? sampleExpression : (((Control)tbExpressions).Text + "\r\n" + sampleExpression));
	}

	protected ILexStyle GetLexStyle(TreeNode node, LexScheme scheme)
	{
		switch (GetNodeKind(node))
		{
		case NodeKind.nkStyles:
			if (node.Nodes.Count > 0 && scheme.LexStyles.Count > 0)
			{
				return scheme.LexStyles[0];
			}
			return null;
		case NodeKind.nkStyle:
			return scheme.LexStyles[node.Index];
		default:
			return null;
		}
	}

	protected ILexState GetLexState(TreeNode node, LexScheme scheme)
	{
		switch (GetNodeKind(node))
		{
		case NodeKind.nkStates:
			if (statesNode.Nodes.Count > 0 && scheme.LexStates.Count > 0)
			{
				return scheme.LexStates[0];
			}
			return null;
		case NodeKind.nkState:
			return scheme.LexStates[node.Index];
		case NodeKind.nkSyntaxBlock:
			return scheme.LexStates[node.Parent.Index];
		case NodeKind.nkreswordSet:
			return scheme.LexStates[node.Parent.Parent.Index];
		default:
			return null;
		}
	}
}
