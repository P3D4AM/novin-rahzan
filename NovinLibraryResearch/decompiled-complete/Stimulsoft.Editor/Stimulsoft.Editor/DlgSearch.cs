using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Stimulsoft.Controls;

namespace Stimulsoft.Editor;

public class DlgSearch : Form
{
	public Label laFindWhat;

	public StiDropDownBox cbFindWhat;

	public Button btFindNext;

	public Button btReplace;

	public Button btMarkAll;

	public Button btClose;

	public Button btHelp;

	public Button btPopup;

	public StiDropDownBox cbReplaceWith;

	public Label laReplaceWith;

	public Button btReplaceAll;

	public ContextMenuStrip cmFind;

	public ToolStripMenuItem menuItem4;

	public ToolStripMenuItem menuItem10;

	public ToolStripMenuItem miSingleChar;

	public ToolStripMenuItem miZeroOrMore;

	public ToolStripMenuItem miOneOrMore;

	public ToolStripMenuItem miEndLine;

	public ToolStripMenuItem miOneCharInSet;

	public ToolStripMenuItem miOneCharNotInSet;

	public ToolStripMenuItem miOr;

	public ToolStripMenuItem miEscape;

	public ToolStripMenuItem miTag;

	public CheckBox chbSearchUp;

	public CheckBox chbSearchHiddenText;

	public CheckBox chbMatchWholeWord;

	public CheckBox chbMatchCase;

	public RadioButton rbEntireScope;

	public RadioButton rbFromCursor;

	public GroupBox gbSearch;

	public CheckBox chbUseRegularExpressions;

	public Panel pnlSearch;

	public CheckBox chbPromptOnReplace;

	public ToolStripMenuItem miBeginLine;

	public IContainer components;

	public Button btReplacePopup;

	private ContextMenuStrip cmReplace;

	private ToolStripMenuItem miFindWhatText;

	private ToolStripMenuItem miTaggedExpression1;

	private ToolStripMenuItem miTaggedExpression2;

	private ToolStripMenuItem miTaggedExpression3;

	private ToolStripMenuItem miTaggedExpression4;

	private ToolStripMenuItem miTaggedExpression5;

	private ToolStripMenuItem miTaggedExpression6;

	private ToolStripMenuItem miTaggedExpression7;

	private ToolStripMenuItem miTaggedExpression8;

	private ToolStripMenuItem miTaggedExpression9;

	private bool isReplace;

	private ISearch search;

	private bool findTextAtCursor;

	public RadioButton rbSelectionOnly;

	private SearchOptions options;

	private bool clearBookmarks;

	private bool firstSearch = true;

	private SearchOptions saveOptions;

	private string saveText = string.Empty;

	public ISearch Search
	{
		get
		{
			return search;
		}
		set
		{
			search = value;
		}
	}

	public SearchOptions Options
	{
		get
		{
			return (chbMatchCase.Checked ? SearchOptions.CaseSensitive : SearchOptions.None) | (chbMatchWholeWord.Checked ? SearchOptions.WholeWordsOnly : SearchOptions.None) | (chbSearchHiddenText.Checked ? SearchOptions.SearchHiddenText : SearchOptions.None) | (chbSearchUp.Checked ? SearchOptions.BackwardSearch : SearchOptions.None) | (chbUseRegularExpressions.Checked ? SearchOptions.RegularExpressions : SearchOptions.None) | (rbEntireScope.Checked ? SearchOptions.EntireScope : SearchOptions.None) | (rbSelectionOnly.Checked ? SearchOptions.SelectionOnly : SearchOptions.None) | (findTextAtCursor ? SearchOptions.FindTextAtCursor : SearchOptions.None) | (chbPromptOnReplace.Checked ? SearchOptions.PromptOnReplace : SearchOptions.None) | (((options & SearchOptions.FindSelectedText) != 0) ? SearchOptions.FindSelectedText : SearchOptions.None) | (((options & SearchOptions.CycledSearch) != 0) ? SearchOptions.CycledSearch : SearchOptions.None) | (((options & SearchOptions.SilentSearch) != 0) ? SearchOptions.SilentSearch : SearchOptions.None);
		}
		set
		{
			options = value;
			chbMatchCase.Checked = (value & SearchOptions.CaseSensitive) != 0;
			chbMatchWholeWord.Checked = (value & SearchOptions.WholeWordsOnly) != 0;
			chbSearchHiddenText.Checked = (value & SearchOptions.SearchHiddenText) != 0;
			chbSearchUp.Checked = (value & SearchOptions.BackwardSearch) != 0;
			chbUseRegularExpressions.Checked = (value & SearchOptions.RegularExpressions) != 0;
			rbEntireScope.Checked = (value & SearchOptions.EntireScope) != 0;
			rbSelectionOnly.Checked = (value & SearchOptions.SelectionOnly) != 0;
			((Control)btPopup).Enabled = chbUseRegularExpressions.Checked;
			findTextAtCursor = (value & SearchOptions.FindTextAtCursor) != 0;
			chbPromptOnReplace.Checked = (value & SearchOptions.PromptOnReplace) != 0;
		}
	}

	public bool IsReplace
	{
		get
		{
			return isReplace;
		}
		set
		{
			if (isReplace != value)
			{
				isReplace = value;
				ReplaceChanged();
			}
		}
	}

	public bool ClearBookmarks
	{
		get
		{
			return clearBookmarks;
		}
		set
		{
			clearBookmarks = value;
		}
	}

	public DlgSearch()
	{
		InitializeComponent();
		ReplaceChanged();
	}

	private string ExcludeAmpersand(string AText)
	{
		string text = AText;
		for (int num = text.IndexOf("&"); num >= 0; num = text.IndexOf("&"))
		{
			text = text.Remove(num, 1);
		}
		return text;
	}

	protected void ReplaceChanged()
	{
		int num = (isReplace ? (((Control)cbReplaceWith).Height + 4) : (-(((Control)cbReplaceWith).Height + 4)));
		((Control)this).Text = ExcludeAmpersand(isReplace ? StringConsts.DlgReplaceCaption : StringConsts.DlgSearchCaption);
		((Control)laReplaceWith).Visible = isReplace;
		((Control)cbReplaceWith).Visible = isReplace;
		((Control)btReplacePopup).Visible = isReplace;
		((Control)btReplaceAll).Visible = isReplace;
		((Control)btReplace).Text = (isReplace ? StringConsts.ReplaceCaption : (StringConsts.ReplaceCaption + "  »"));
		((Control)btPopup).Enabled = chbUseRegularExpressions.Checked;
		((Control)btReplacePopup).Enabled = chbUseRegularExpressions.Checked;
		Panel obj = pnlSearch;
		((Control)obj).Top = ((Control)obj).Top + num;
		Button obj2 = btMarkAll;
		((Control)obj2).Top = ((Control)obj2).Top + num;
		Button obj3 = btClose;
		((Control)obj3).Top = ((Control)obj3).Top + num;
		Button obj4 = btHelp;
		((Control)obj4).Top = ((Control)obj4).Top + num;
		((Control)this).Height = ((Control)this).Height + num;
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
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
		laFindWhat = new Label();
		cbFindWhat = new StiDropDownBox();
		btPopup = new Button();
		btFindNext = new Button();
		btReplace = new Button();
		btMarkAll = new Button();
		btClose = new Button();
		btHelp = new Button();
		cbReplaceWith = new StiDropDownBox();
		laReplaceWith = new Label();
		btReplaceAll = new Button();
		cmFind = new ContextMenuStrip();
		miSingleChar = new ToolStripMenuItem();
		miZeroOrMore = new ToolStripMenuItem();
		miOneOrMore = new ToolStripMenuItem();
		menuItem4 = new ToolStripMenuItem();
		miBeginLine = new ToolStripMenuItem();
		miEndLine = new ToolStripMenuItem();
		menuItem10 = new ToolStripMenuItem();
		miOneCharInSet = new ToolStripMenuItem();
		miOneCharNotInSet = new ToolStripMenuItem();
		miOr = new ToolStripMenuItem();
		miEscape = new ToolStripMenuItem();
		miTag = new ToolStripMenuItem();
		pnlSearch = new Panel();
		chbPromptOnReplace = new CheckBox();
		gbSearch = new GroupBox();
		rbSelectionOnly = new RadioButton();
		rbEntireScope = new RadioButton();
		rbFromCursor = new RadioButton();
		chbSearchUp = new CheckBox();
		chbSearchHiddenText = new CheckBox();
		chbMatchWholeWord = new CheckBox();
		chbMatchCase = new CheckBox();
		chbUseRegularExpressions = new CheckBox();
		btReplacePopup = new Button();
		cmReplace = new ContextMenuStrip();
		miFindWhatText = new ToolStripMenuItem();
		miTaggedExpression1 = new ToolStripMenuItem();
		miTaggedExpression2 = new ToolStripMenuItem();
		miTaggedExpression3 = new ToolStripMenuItem();
		miTaggedExpression4 = new ToolStripMenuItem();
		miTaggedExpression5 = new ToolStripMenuItem();
		miTaggedExpression6 = new ToolStripMenuItem();
		miTaggedExpression7 = new ToolStripMenuItem();
		miTaggedExpression8 = new ToolStripMenuItem();
		miTaggedExpression9 = new ToolStripMenuItem();
		((Control)pnlSearch).SuspendLayout();
		((Control)gbSearch).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)laFindWhat).AutoSize = true;
		laFindWhat.FlatStyle = (FlatStyle)3;
		((Control)laFindWhat).ForeColor = Color.DimGray;
		((Control)laFindWhat).Location = new Point(12, 16);
		((Control)laFindWhat).Name = "laFindWhat";
		((Control)laFindWhat).Size = new Size(56, 13);
		((Control)laFindWhat).TabIndex = 0;
		((Control)laFindWhat).Text = "Fi&nd what:";
		((Control)cbFindWhat).Location = new Point(99, 12);
		((Control)cbFindWhat).Name = "cbFindWhat";
		((Control)cbFindWhat).Size = new Size(274, 21);
		((Control)cbFindWhat).TabIndex = 1;
		((Control)btPopup).BackColor = SystemColors.Control;
		((ButtonBase)btPopup).FlatStyle = (FlatStyle)3;
		((ButtonBase)btPopup).ImageIndex = 0;
		((Control)btPopup).Location = new Point(377, 12);
		((Control)btPopup).Name = "btPopup";
		((Control)btPopup).Size = new Size(17, 20);
		((Control)btPopup).TabIndex = 2;
		((Control)btPopup).Text = ">";
		((ButtonBase)btPopup).UseVisualStyleBackColor = false;
		((Control)btPopup).Click += btPopup_Click;
		((ButtonBase)btFindNext).FlatStyle = (FlatStyle)3;
		((Control)btFindNext).Location = new Point(404, 12);
		((Control)btFindNext).Name = "btFindNext";
		((Control)btFindNext).Size = new Size(90, 23);
		((Control)btFindNext).TabIndex = 12;
		((Control)btFindNext).Text = "&Find Next";
		((Control)btFindNext).Click += btFindNext_Click;
		((ButtonBase)btReplace).FlatStyle = (FlatStyle)3;
		((ButtonBase)btReplace).ImageAlign = (ContentAlignment)64;
		((Control)btReplace).Location = new Point(404, 38);
		((Control)btReplace).Name = "btReplace";
		((Control)btReplace).Size = new Size(90, 23);
		((Control)btReplace).TabIndex = 13;
		((Control)btReplace).Text = "&Replace";
		((Control)btReplace).Click += btReplace_Click;
		((ButtonBase)btMarkAll).FlatStyle = (FlatStyle)3;
		((Control)btMarkAll).Location = new Point(404, 92);
		((Control)btMarkAll).Name = "btMarkAll";
		((Control)btMarkAll).Size = new Size(90, 23);
		((Control)btMarkAll).TabIndex = 15;
		((Control)btMarkAll).Text = "&Mark All";
		((Control)btMarkAll).Click += btMarkAll_Click;
		btClose.DialogResult = (DialogResult)2;
		((ButtonBase)btClose).FlatStyle = (FlatStyle)3;
		((Control)btClose).Location = new Point(404, 118);
		((Control)btClose).Name = "btClose";
		((Control)btClose).Size = new Size(90, 23);
		((Control)btClose).TabIndex = 16;
		((Control)btClose).Text = "Close";
		((Control)btClose).Click += btClose_Click;
		((ButtonBase)btHelp).FlatStyle = (FlatStyle)3;
		((Control)btHelp).Location = new Point(404, 144);
		((Control)btHelp).Name = "btHelp";
		((Control)btHelp).Size = new Size(90, 23);
		((Control)btHelp).TabIndex = 17;
		((Control)btHelp).Text = "Help";
		((Control)btHelp).Visible = false;
		((Control)cbReplaceWith).Location = new Point(99, 36);
		((Control)cbReplaceWith).Name = "cbReplaceWith";
		((Control)cbReplaceWith).Size = new Size(274, 21);
		((Control)cbReplaceWith).TabIndex = 3;
		((Control)laReplaceWith).AutoSize = true;
		laReplaceWith.FlatStyle = (FlatStyle)3;
		((Control)laReplaceWith).ForeColor = Color.DimGray;
		((Control)laReplaceWith).Location = new Point(12, 39);
		((Control)laReplaceWith).Name = "laReplaceWith";
		((Control)laReplaceWith).Size = new Size(72, 13);
		((Control)laReplaceWith).TabIndex = 2;
		((Control)laReplaceWith).Text = "Re&place with:";
		((ButtonBase)btReplaceAll).FlatStyle = (FlatStyle)3;
		((Control)btReplaceAll).Location = new Point(404, 64);
		((Control)btReplaceAll).Name = "btReplaceAll";
		((Control)btReplaceAll).Size = new Size(90, 23);
		((Control)btReplaceAll).TabIndex = 14;
		((Control)btReplaceAll).Text = "Replace &All";
		((Control)btReplaceAll).Click += btReplaceAll_Click;
		((ToolStrip)cmFind).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[12]
		{
			(ToolStripItem)miSingleChar,
			(ToolStripItem)miZeroOrMore,
			(ToolStripItem)miOneOrMore,
			(ToolStripItem)menuItem4,
			(ToolStripItem)miBeginLine,
			(ToolStripItem)miEndLine,
			(ToolStripItem)menuItem10,
			(ToolStripItem)miOneCharInSet,
			(ToolStripItem)miOneCharNotInSet,
			(ToolStripItem)miOr,
			(ToolStripItem)miEscape,
			(ToolStripItem)miTag
		});
		((ToolStripItem)miSingleChar).Text = ". Any single character";
		((ToolStripItem)miSingleChar).Click += miBeginWord_Click;
		((ToolStripItem)miZeroOrMore).Text = "* Zero or more";
		((ToolStripItem)miZeroOrMore).Click += miBeginWord_Click;
		((ToolStripItem)miOneOrMore).Text = "+ One or more";
		((ToolStripItem)miOneOrMore).Click += miBeginWord_Click;
		((ToolStripItem)menuItem4).Text = "-";
		((ToolStripItem)menuItem4).Click += miBeginWord_Click;
		((ToolStripItem)miBeginLine).Text = "^ Beginning of line";
		((ToolStripItem)miBeginLine).Click += miBeginWord_Click;
		((ToolStripItem)miEndLine).Text = "/r/n End of line";
		((ToolStripItem)miEndLine).Click += miBeginWord_Click;
		((ToolStripItem)menuItem10).Text = "-";
		((ToolStripItem)menuItem10).Click += miBeginWord_Click;
		((ToolStripItem)miOneCharInSet).Text = "[] Any one character in the set";
		((ToolStripItem)miOneCharInSet).Click += miBeginWord_Click;
		((ToolStripItem)miOneCharNotInSet).Text = "[^] Any one character not in the set";
		((ToolStripItem)miOneCharNotInSet).Click += miBeginWord_Click;
		((ToolStripItem)miOr).Text = "| Or";
		((ToolStripItem)miOr).Click += miBeginWord_Click;
		((ToolStripItem)miEscape).Text = "\\ Escape Special Character";
		((ToolStripItem)miEscape).Click += miBeginWord_Click;
		((ToolStripItem)miTag).Text = "{} Tag expression";
		((ToolStripItem)miTag).Click += miBeginWord_Click;
		((Control)pnlSearch).Controls.Add((Control)(object)chbPromptOnReplace);
		((Control)pnlSearch).Controls.Add((Control)(object)gbSearch);
		((Control)pnlSearch).Controls.Add((Control)(object)chbSearchUp);
		((Control)pnlSearch).Controls.Add((Control)(object)chbSearchHiddenText);
		((Control)pnlSearch).Controls.Add((Control)(object)chbMatchWholeWord);
		((Control)pnlSearch).Controls.Add((Control)(object)chbMatchCase);
		((Control)pnlSearch).Controls.Add((Control)(object)chbUseRegularExpressions);
		((Control)pnlSearch).Location = new Point(12, 60);
		((Control)pnlSearch).Name = "pnlSearch";
		((Control)pnlSearch).Size = new Size(386, 154);
		((Control)pnlSearch).TabIndex = 18;
		((ButtonBase)chbPromptOnReplace).FlatStyle = (FlatStyle)3;
		((Control)chbPromptOnReplace).Location = new Point(12, 127);
		((Control)chbPromptOnReplace).Name = "chbPromptOnReplace";
		((Control)chbPromptOnReplace).Size = new Size(180, 25);
		((Control)chbPromptOnReplace).TabIndex = 19;
		((Control)chbPromptOnReplace).Text = "Prompt on replace";
		((Control)gbSearch).Controls.Add((Control)(object)rbSelectionOnly);
		((Control)gbSearch).Controls.Add((Control)(object)rbEntireScope);
		((Control)gbSearch).Controls.Add((Control)(object)rbFromCursor);
		gbSearch.FlatStyle = (FlatStyle)3;
		((Control)gbSearch).Location = new Point(208, 6);
		((Control)gbSearch).Name = "gbSearch";
		((Control)gbSearch).Size = new Size(176, 103);
		((Control)gbSearch).TabIndex = 17;
		gbSearch.TabStop = false;
		((Control)gbSearch).Text = "Search";
		((ButtonBase)rbSelectionOnly).FlatStyle = (FlatStyle)3;
		((ButtonBase)rbSelectionOnly).ImageIndex = 0;
		((Control)rbSelectionOnly).Location = new Point(8, 68);
		((Control)rbSelectionOnly).Name = "rbSelectionOnly";
		((Control)rbSelectionOnly).Size = new Size(160, 18);
		((Control)rbSelectionOnly).TabIndex = 3;
		((Control)rbSelectionOnly).Text = "&Selection Only";
		((ButtonBase)rbEntireScope).FlatStyle = (FlatStyle)3;
		((ButtonBase)rbEntireScope).ImageIndex = 0;
		((Control)rbEntireScope).Location = new Point(8, 41);
		((Control)rbEntireScope).Name = "rbEntireScope";
		((Control)rbEntireScope).Size = new Size(160, 18);
		((Control)rbEntireScope).TabIndex = 1;
		((Control)rbEntireScope).Text = "&Entire scope";
		rbFromCursor.Checked = true;
		((ButtonBase)rbFromCursor).FlatStyle = (FlatStyle)3;
		((Control)rbFromCursor).Location = new Point(8, 16);
		((Control)rbFromCursor).Name = "rbFromCursor";
		((Control)rbFromCursor).Size = new Size(160, 18);
		((Control)rbFromCursor).TabIndex = 0;
		rbFromCursor.TabStop = true;
		((Control)rbFromCursor).Text = "From cursor";
		((ButtonBase)chbSearchUp).FlatStyle = (FlatStyle)3;
		((Control)chbSearchUp).Location = new Point(12, 77);
		((Control)chbSearchUp).Name = "chbSearchUp";
		((Control)chbSearchUp).Size = new Size(180, 25);
		((Control)chbSearchUp).TabIndex = 14;
		((Control)chbSearchUp).Text = "Search &up";
		((ButtonBase)chbSearchHiddenText).FlatStyle = (FlatStyle)3;
		((Control)chbSearchHiddenText).Location = new Point(12, 52);
		((Control)chbSearchHiddenText).Name = "chbSearchHiddenText";
		((Control)chbSearchHiddenText).Size = new Size(180, 25);
		((Control)chbSearchHiddenText).TabIndex = 13;
		((Control)chbSearchHiddenText).Text = "Search &hidden text";
		((ButtonBase)chbMatchWholeWord).FlatStyle = (FlatStyle)3;
		((Control)chbMatchWholeWord).Location = new Point(12, 27);
		((Control)chbMatchWholeWord).Name = "chbMatchWholeWord";
		((Control)chbMatchWholeWord).Size = new Size(172, 25);
		((Control)chbMatchWholeWord).TabIndex = 12;
		((Control)chbMatchWholeWord).Text = "Match &whole word";
		((ButtonBase)chbMatchCase).FlatStyle = (FlatStyle)3;
		((Control)chbMatchCase).Location = new Point(12, 2);
		((Control)chbMatchCase).Name = "chbMatchCase";
		((Control)chbMatchCase).Size = new Size(180, 25);
		((Control)chbMatchCase).TabIndex = 11;
		((Control)chbMatchCase).Text = "Match &case";
		((ButtonBase)chbUseRegularExpressions).FlatStyle = (FlatStyle)3;
		((Control)chbUseRegularExpressions).Location = new Point(12, 102);
		((Control)chbUseRegularExpressions).Name = "chbUseRegularExpressions";
		((Control)chbUseRegularExpressions).Size = new Size(180, 25);
		((Control)chbUseRegularExpressions).TabIndex = 15;
		((Control)chbUseRegularExpressions).Text = "Use &regular expressions";
		((Control)chbUseRegularExpressions).Click += chbUseRegularExpressions_Click;
		((Control)btReplacePopup).BackColor = SystemColors.Control;
		((ButtonBase)btReplacePopup).FlatStyle = (FlatStyle)3;
		((ButtonBase)btReplacePopup).ImageIndex = 0;
		((Control)btReplacePopup).Location = new Point(377, 36);
		((Control)btReplacePopup).Name = "btReplacePopup";
		((Control)btReplacePopup).Size = new Size(17, 20);
		((Control)btReplacePopup).TabIndex = 19;
		((Control)btReplacePopup).Text = ">";
		((ButtonBase)btReplacePopup).UseVisualStyleBackColor = false;
		((Control)btReplacePopup).Click += btReplacePopup_Click;
		((ToolStrip)cmReplace).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[10]
		{
			(ToolStripItem)miFindWhatText,
			(ToolStripItem)miTaggedExpression1,
			(ToolStripItem)miTaggedExpression2,
			(ToolStripItem)miTaggedExpression3,
			(ToolStripItem)miTaggedExpression4,
			(ToolStripItem)miTaggedExpression5,
			(ToolStripItem)miTaggedExpression6,
			(ToolStripItem)miTaggedExpression7,
			(ToolStripItem)miTaggedExpression8,
			(ToolStripItem)miTaggedExpression9
		});
		((ToolStripItem)miFindWhatText).Text = "$0 Find What Text";
		((ToolStripItem)miFindWhatText).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression1).Text = "$1 Tagged Expression 1";
		((ToolStripItem)miTaggedExpression1).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression2).Text = "$2 Tagged Expression 2";
		((ToolStripItem)miTaggedExpression2).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression3).Text = "$3 Tagged Expression 3";
		((ToolStripItem)miTaggedExpression3).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression4).Text = "$4 Tagged Expression 4";
		((ToolStripItem)miTaggedExpression4).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression5).Text = "$5 Tagged Expression 5";
		((ToolStripItem)miTaggedExpression5).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression6).Text = "$6 Tagged Expression 6";
		((ToolStripItem)miTaggedExpression6).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression7).Text = "$7 Tagged Expression 7";
		((ToolStripItem)miTaggedExpression7).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression8).Text = "$8 Tagged Expression 8";
		((ToolStripItem)miTaggedExpression8).Click += miFindWhatText_Click;
		((ToolStripItem)miTaggedExpression9).Text = "$9 Tagged Expression 9";
		((ToolStripItem)miTaggedExpression9).Click += miFindWhatText_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)btFindNext;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btClose;
		((Form)this).ClientSize = new Size(505, 226);
		((Control)this).Controls.Add((Control)(object)btReplacePopup);
		((Control)this).Controls.Add((Control)(object)pnlSearch);
		((Control)this).Controls.Add((Control)(object)btReplaceAll);
		((Control)this).Controls.Add((Control)(object)cbReplaceWith);
		((Control)this).Controls.Add((Control)(object)laReplaceWith);
		((Control)this).Controls.Add((Control)(object)laFindWhat);
		((Control)this).Controls.Add((Control)(object)btHelp);
		((Control)this).Controls.Add((Control)(object)btClose);
		((Control)this).Controls.Add((Control)(object)btMarkAll);
		((Control)this).Controls.Add((Control)(object)btReplace);
		((Control)this).Controls.Add((Control)(object)btFindNext);
		((Control)this).Controls.Add((Control)(object)btPopup);
		((Control)this).Controls.Add((Control)(object)cbFindWhat);
		((Form)this).FormBorderStyle = (FormBorderStyle)5;
		((Control)this).Name = "DlgSearch";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Find";
		((Form)this).Load += DlgSearch_Load;
		((Control)this).Resize += DlgSearch_Resize;
		((Control)pnlSearch).ResumeLayout(false);
		((Control)gbSearch).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private RegexOptions GetRegexOptions(string text)
	{
		return (RegexOptions)((int)(chbSearchUp.Checked ? RegexOptions.RightToLeft : RegexOptions.None) | ((!chbMatchCase.Checked) ? 1 : 0) | (int)((text != null && (text.IndexOf("\\r") >= 0 || text.IndexOf("\\n") >= 0)) ? RegexOptions.Multiline : RegexOptions.None));
	}

	private Regex GetExpression()
	{
		Regex result = null;
		if (chbUseRegularExpressions.Checked)
		{
			try
			{
				result = new Regex(((Control)cbFindWhat).Text, GetRegexOptions(((Control)cbFindWhat).Text));
			}
			catch (Exception exception)
			{
				result = null;
				ErrorHandler.Error(exception);
			}
		}
		return result;
	}

	private void AddToHistory(IList List, string s)
	{
		if (s != string.Empty)
		{
			for (int num = List.IndexOf(s); num >= 0; num = List.IndexOf(s))
			{
				List.RemoveAt(num);
			}
			List.Insert(0, s);
		}
	}

	private void AddToHistory()
	{
		if (((Control)cbFindWhat).Text != string.Empty)
		{
			AddToHistory((IList)cbFindWhat.Items, ((Control)cbFindWhat).Text);
			cbFindWhat.SelectedIndex = 0;
		}
		if (isReplace && ((Control)cbReplaceWith).Text != string.Empty)
		{
			AddToHistory((IList)cbReplaceWith.Items, ((Control)cbReplaceWith).Text);
			cbReplaceWith.SelectedIndex = 0;
		}
	}

	private void TextFound()
	{
		firstSearch = false;
		saveOptions = Options;
		saveText = ((Control)cbFindWhat).Text;
	}

	private void btPopup_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		((ToolStripDropDown)cmFind).Show((Control)sender, new Point(0, 0));
	}

	private void btFindNext_Click(object sender, EventArgs e)
	{
		AddToHistory();
		if (search != null)
		{
			SearchOptions searchOptions = Options;
			if ((search.FirstSearch || firstSearch || !(((Control)cbFindWhat).Text == saveText) || searchOptions != saveOptions) ? search.Find(((Control)cbFindWhat).Text, searchOptions, GetExpression()) : (((searchOptions & SearchOptions.BackwardSearch) == 0) ? search.FindNext() : search.FindPrevious()))
			{
				TextFound();
			}
			else
			{
				ShowNotFound();
			}
		}
	}

	private void btReplace_Click(object sender, EventArgs e)
	{
		if (!isReplace)
		{
			IsReplace = true;
			return;
		}
		AddToHistory();
		if (search == null)
		{
			return;
		}
		SearchOptions searchOptions = Options;
		if (!search.FirstSearch && ((Control)cbFindWhat).Text == saveText && search.NeedReplaceCurrent(out var match))
		{
			if (search.ReplaceCurrent(((Control)cbReplaceWith).Text, searchOptions, match))
			{
				if ((Options & SearchOptions.BackwardSearch) != 0)
				{
					search.FindPrevious();
				}
				else
				{
					search.FindNext();
				}
			}
		}
		else if (!search.Find(((Control)cbFindWhat).Text, searchOptions, GetExpression()))
		{
			ShowNotFound();
		}
		else
		{
			TextFound();
		}
	}

	private void btReplaceAll_Click(object sender, EventArgs e)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		AddToHistory();
		if (search != null)
		{
			SearchOptions searchOptions = Options;
			if (!search.FirstSearch && ((Control)cbFindWhat).Text == saveText && search.NeedReplaceCurrent(out var match))
			{
				search.ReplaceCurrent(((Control)cbReplaceWith).Text, searchOptions, match);
			}
			if (!search.ReplaceAll(((Control)cbFindWhat).Text, ((Control)cbReplaceWith).Text, Options, GetExpression(), out var count))
			{
				ShowNotFound();
			}
			else
			{
				MessageBox.Show(string.Format(StringConsts.OccurencesReplaced, count), ((Control)this).Text);
			}
		}
	}

	private void btMarkAll_Click(object sender, EventArgs e)
	{
		if (search != null)
		{
			search.MarkAll(((Control)cbFindWhat).Text, Options, GetExpression(), clearBookmarks);
		}
	}

	private void chbUseRegularExpressions_Click(object sender, EventArgs e)
	{
		((Control)btPopup).Enabled = chbUseRegularExpressions.Checked;
		((Control)btReplacePopup).Enabled = chbUseRegularExpressions.Checked;
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected void ShowNotFound()
	{
		search.ShowNotFound(((Control)this).Text);
		((Control)cbFindWhat).Focus();
	}

	protected override void OnLoad(EventArgs e)
	{
		((Form)this).OnLoad(e);
		if (((Control)cbFindWhat).Text == string.Empty && cbFindWhat.Items.Count > 0)
		{
			cbFindWhat.SelectedIndex = 0;
		}
	}

	private void miBeginWord_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		string text = ((ToolStripItem)(ToolStripMenuItem)sender).Text.Trim();
		int num = text.IndexOf(' ');
		if (num > 0)
		{
			text = text.Substring(0, num);
		}
		((Control)cbFindWhat).Text = ((Control)cbFindWhat).Text + text;
	}

	public void Init()
	{
		firstSearch = true;
		saveOptions = SearchOptions.None;
		saveText = string.Empty;
	}

	private void DlgSearch_Load(object sender, EventArgs e)
	{
		LoadFromResource();
	}

	private void LoadFromResource()
	{
		((Control)laFindWhat).Text = StringConsts.FindWhatCaption;
		((Control)laReplaceWith).Text = StringConsts.ReplaceWithCaption;
		((Control)chbMatchCase).Text = StringConsts.MatchCaseCaption;
		((Control)chbMatchWholeWord).Text = StringConsts.MatchWholeWordCaption;
		((Control)chbSearchHiddenText).Text = StringConsts.SearchHiddenTextCaption;
		((Control)chbSearchUp).Text = StringConsts.SearchUpCaption;
		((Control)chbUseRegularExpressions).Text = StringConsts.UseRegularExpressionsCaption;
		((Control)chbPromptOnReplace).Text = StringConsts.PromptOnReplaceCaption;
		((Control)gbSearch).Text = StringConsts.SearchCaption;
		((Control)rbFromCursor).Text = StringConsts.FromCursorCaption;
		((Control)rbEntireScope).Text = StringConsts.EntireScopeCaption;
		((Control)rbSelectionOnly).Text = StringConsts.SelectionOnlyCaption;
		((Control)btFindNext).Text = StringConsts.FindNextCaption;
		((Control)btReplaceAll).Text = StringConsts.ReplaceAllCaption;
		((Control)btMarkAll).Text = StringConsts.MarkAllCaption;
		((Control)btClose).Text = StringConsts.CloseCaption;
		((Control)btHelp).Text = StringConsts.HelpCaption;
		((ToolStripItem)miSingleChar).Text = StringConsts.SingleCharCaption;
		((ToolStripItem)miZeroOrMore).Text = StringConsts.ZeroOrMoreCaption;
		((ToolStripItem)miOneOrMore).Text = StringConsts.OneOrMoreCaption;
		((ToolStripItem)miBeginLine).Text = StringConsts.BeginLineCaption;
		((ToolStripItem)miEndLine).Text = StringConsts.EndLineCaption;
		((ToolStripItem)miOneCharInSet).Text = StringConsts.OneCharInSetCaption;
		((ToolStripItem)miOneCharNotInSet).Text = StringConsts.OneCharNotInSetCaption;
		((ToolStripItem)miOr).Text = StringConsts.OrCaption;
		((ToolStripItem)miEscape).Text = StringConsts.EscapeCaption;
		((ToolStripItem)miTag).Text = StringConsts.TagCaption;
		((ToolStripItem)miFindWhatText).Text = StringConsts.FindWhatTextCaption;
		((ToolStripItem)miTaggedExpression1).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 1);
		((ToolStripItem)miTaggedExpression2).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 2);
		((ToolStripItem)miTaggedExpression3).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 3);
		((ToolStripItem)miTaggedExpression4).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 4);
		((ToolStripItem)miTaggedExpression5).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 5);
		((ToolStripItem)miTaggedExpression6).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 6);
		((ToolStripItem)miTaggedExpression7).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 7);
		((ToolStripItem)miTaggedExpression8).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 8);
		((ToolStripItem)miTaggedExpression9).Text = string.Format("${1} {0} {1}", StringConsts.TagExpressionCaption, 9);
	}

	private void btReplacePopup_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		((ToolStripDropDown)cmReplace).Show((Control)sender, new Point(0, 0));
	}

	private void miFindWhatText_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		string text = ((ToolStripItem)(ToolStripMenuItem)sender).Text.Trim();
		int num = text.IndexOf(' ');
		if (num > 0)
		{
			text = text.Substring(0, num);
		}
		((Control)cbReplaceWith).Text = ((Control)cbReplaceWith).Text + text;
	}

	private void DlgSearch_Resize(object sender, EventArgs e)
	{
	}
}
