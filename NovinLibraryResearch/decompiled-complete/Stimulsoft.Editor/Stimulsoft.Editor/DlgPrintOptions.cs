using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class DlgPrintOptions : Form, IPrintOptionsDialog
{
	private Button btCancel;

	private Button btOk;

	private GroupBox gbOptions;

	private CheckBox chbDisplayProgress;

	private CheckBox chbUseColors;

	private CheckBox chbUseSyntax;

	private CheckBox chbWordWrap;

	private CheckBox chbPageNumbers;

	private CheckBox chbLineNumbers;

	private CheckBox chbUseFooter;

	private CheckBox chbUseHeader;

	private GroupBox gbFileToPrint;

	private CheckBox chbPrintSelection;

	private Label laFileName;

	private Container components;

	private PrintOptions options;

	private PrintOptions allowedOptions;

	private string fileName;

	public PrintOptions Options
	{
		get
		{
			return options;
		}
		set
		{
			options = value;
		}
	}

	public PrintOptions AllowedOptions
	{
		get
		{
			return allowedOptions;
		}
		set
		{
			allowedOptions = value;
		}
	}

	public string FileName
	{
		get
		{
			return fileName;
		}
		set
		{
			fileName = value;
		}
	}

	public DlgPrintOptions()
	{
		InitializeComponent();
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
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
		btCancel = new Button();
		btOk = new Button();
		gbOptions = new GroupBox();
		chbDisplayProgress = new CheckBox();
		chbUseColors = new CheckBox();
		chbUseSyntax = new CheckBox();
		chbWordWrap = new CheckBox();
		chbPageNumbers = new CheckBox();
		chbLineNumbers = new CheckBox();
		chbUseFooter = new CheckBox();
		chbUseHeader = new CheckBox();
		gbFileToPrint = new GroupBox();
		chbPrintSelection = new CheckBox();
		laFileName = new Label();
		((Control)gbOptions).SuspendLayout();
		((Control)gbFileToPrint).SuspendLayout();
		((Control)this).SuspendLayout();
		btCancel.DialogResult = (DialogResult)2;
		((ButtonBase)btCancel).FlatStyle = (FlatStyle)3;
		((Control)btCancel).Location = new Point(257, 229);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 9;
		((Control)btCancel).Text = "Cancel";
		btOk.DialogResult = (DialogResult)1;
		((ButtonBase)btOk).FlatStyle = (FlatStyle)3;
		((Control)btOk).Location = new Point(177, 229);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 23);
		((Control)btOk).TabIndex = 8;
		((Control)btOk).Text = "Ok";
		((Control)btOk).Click += btOk_Click;
		((Control)gbOptions).Controls.Add((Control)(object)chbDisplayProgress);
		((Control)gbOptions).Controls.Add((Control)(object)chbUseColors);
		((Control)gbOptions).Controls.Add((Control)(object)chbUseSyntax);
		((Control)gbOptions).Controls.Add((Control)(object)chbWordWrap);
		((Control)gbOptions).Controls.Add((Control)(object)chbPageNumbers);
		((Control)gbOptions).Controls.Add((Control)(object)chbLineNumbers);
		((Control)gbOptions).Controls.Add((Control)(object)chbUseFooter);
		((Control)gbOptions).Controls.Add((Control)(object)chbUseHeader);
		((Control)gbOptions).Location = new Point(12, 92);
		((Control)gbOptions).Name = "gbOptions";
		((Control)gbOptions).Size = new Size(319, 125);
		((Control)gbOptions).TabIndex = 6;
		gbOptions.TabStop = false;
		((Control)gbOptions).Text = "Options";
		((ButtonBase)chbDisplayProgress).FlatStyle = (FlatStyle)3;
		((Control)chbDisplayProgress).Location = new Point(129, 91);
		((Control)chbDisplayProgress).Name = "chbDisplayProgress";
		((Control)chbDisplayProgress).Size = new Size(187, 25);
		((Control)chbDisplayProgress).TabIndex = 7;
		((Control)chbDisplayProgress).Text = "Display progress";
		((ButtonBase)chbUseColors).FlatStyle = (FlatStyle)3;
		((Control)chbUseColors).Location = new Point(129, 66);
		((Control)chbUseColors).Name = "chbUseColors";
		((Control)chbUseColors).Size = new Size(187, 25);
		((Control)chbUseColors).TabIndex = 6;
		((Control)chbUseColors).Text = "Color print";
		((ButtonBase)chbUseSyntax).FlatStyle = (FlatStyle)3;
		((Control)chbUseSyntax).Location = new Point(129, 41);
		((Control)chbUseSyntax).Name = "chbUseSyntax";
		((Control)chbUseSyntax).Size = new Size(187, 25);
		((Control)chbUseSyntax).TabIndex = 5;
		((Control)chbUseSyntax).Text = "Syntax print";
		((ButtonBase)chbWordWrap).FlatStyle = (FlatStyle)3;
		((Control)chbWordWrap).Location = new Point(129, 16);
		((Control)chbWordWrap).Name = "chbWordWrap";
		((Control)chbWordWrap).Size = new Size(187, 25);
		((Control)chbWordWrap).TabIndex = 4;
		((Control)chbWordWrap).Text = "Word Wrap";
		((ButtonBase)chbPageNumbers).FlatStyle = (FlatStyle)3;
		((Control)chbPageNumbers).Location = new Point(16, 91);
		((Control)chbPageNumbers).Name = "chbPageNumbers";
		((Control)chbPageNumbers).Size = new Size(99, 25);
		((Control)chbPageNumbers).TabIndex = 3;
		((Control)chbPageNumbers).Text = "Page Numbers";
		((ButtonBase)chbLineNumbers).FlatStyle = (FlatStyle)3;
		((Control)chbLineNumbers).Location = new Point(16, 66);
		((Control)chbLineNumbers).Name = "chbLineNumbers";
		((Control)chbLineNumbers).Size = new Size(99, 25);
		((Control)chbLineNumbers).TabIndex = 2;
		((Control)chbLineNumbers).Text = "Line Numbers";
		((ButtonBase)chbUseFooter).FlatStyle = (FlatStyle)3;
		((Control)chbUseFooter).Location = new Point(16, 41);
		((Control)chbUseFooter).Name = "chbUseFooter";
		((Control)chbUseFooter).Size = new Size(99, 25);
		((Control)chbUseFooter).TabIndex = 1;
		((Control)chbUseFooter).Text = "Footer";
		((ButtonBase)chbUseHeader).FlatStyle = (FlatStyle)3;
		((Control)chbUseHeader).Location = new Point(16, 16);
		((Control)chbUseHeader).Name = "chbUseHeader";
		((Control)chbUseHeader).Size = new Size(99, 25);
		((Control)chbUseHeader).TabIndex = 0;
		((Control)chbUseHeader).Text = "Header";
		((Control)gbFileToPrint).Controls.Add((Control)(object)chbPrintSelection);
		((Control)gbFileToPrint).Controls.Add((Control)(object)laFileName);
		gbFileToPrint.FlatStyle = (FlatStyle)3;
		((Control)gbFileToPrint).Location = new Point(12, 12);
		((Control)gbFileToPrint).Name = "gbFileToPrint";
		((Control)gbFileToPrint).Size = new Size(319, 72);
		((Control)gbFileToPrint).TabIndex = 5;
		gbFileToPrint.TabStop = false;
		((Control)gbFileToPrint).Text = "File to Print";
		((ButtonBase)chbPrintSelection).FlatStyle = (FlatStyle)3;
		((Control)chbPrintSelection).Location = new Point(16, 42);
		((Control)chbPrintSelection).Name = "chbPrintSelection";
		((Control)chbPrintSelection).Size = new Size(104, 25);
		((Control)chbPrintSelection).TabIndex = 1;
		((Control)chbPrintSelection).Text = "Print selection";
		((Control)laFileName).AutoSize = true;
		((Control)laFileName).ForeColor = Color.DimGray;
		((Control)laFileName).Location = new Point(16, 24);
		((Control)laFileName).Name = "laFileName";
		((Control)laFileName).Size = new Size(54, 13);
		((Control)laFileName).TabIndex = 0;
		((Control)laFileName).Text = "File Name";
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(344, 261);
		((Control)this).Controls.Add((Control)(object)btCancel);
		((Control)this).Controls.Add((Control)(object)btOk);
		((Control)this).Controls.Add((Control)(object)gbOptions);
		((Control)this).Controls.Add((Control)(object)gbFileToPrint);
		((Form)this).FormBorderStyle = (FormBorderStyle)5;
		((Control)this).Name = "DlgPrintOptions";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Print Settings";
		((Form)this).Load += DlgPrintOptions_Load;
		((Control)gbOptions).ResumeLayout(false);
		((Control)gbFileToPrint).ResumeLayout(false);
		((Control)gbFileToPrint).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	private void ControlsFromOptions()
	{
		if (FileName != string.Empty && FileName != null)
		{
			((Control)laFileName).Text = ((Control)laFileName).Text + ": " + FileName;
		}
		((Control)chbPrintSelection).Enabled = (AllowedOptions & PrintOptions.PrintSelection) > PrintOptions.None;
		((Control)chbUseHeader).Enabled = (AllowedOptions & PrintOptions.UseHeader) > PrintOptions.None;
		((Control)chbUseFooter).Enabled = (AllowedOptions & PrintOptions.UseFooter) > PrintOptions.None;
		((Control)chbLineNumbers).Enabled = (AllowedOptions & PrintOptions.LineNumbers) > PrintOptions.None;
		((Control)chbPageNumbers).Enabled = (AllowedOptions & PrintOptions.PageNumbers) > PrintOptions.None;
		((Control)chbWordWrap).Enabled = (AllowedOptions & PrintOptions.WordWrap) > PrintOptions.None;
		((Control)chbUseSyntax).Enabled = (AllowedOptions & PrintOptions.UseSyntax) > PrintOptions.None;
		((Control)chbUseColors).Enabled = (AllowedOptions & PrintOptions.UseColors) > PrintOptions.None;
		((Control)chbDisplayProgress).Enabled = (AllowedOptions & PrintOptions.DisplayProgress) > PrintOptions.None;
		chbPrintSelection.Checked = (Options & PrintOptions.PrintSelection) > PrintOptions.None;
		chbUseHeader.Checked = (Options & PrintOptions.UseHeader) > PrintOptions.None;
		chbUseFooter.Checked = (Options & PrintOptions.UseFooter) > PrintOptions.None;
		chbLineNumbers.Checked = (Options & PrintOptions.LineNumbers) > PrintOptions.None;
		chbPageNumbers.Checked = (Options & PrintOptions.PageNumbers) > PrintOptions.None;
		chbWordWrap.Checked = (Options & PrintOptions.WordWrap) > PrintOptions.None;
		chbUseSyntax.Checked = (Options & PrintOptions.UseSyntax) > PrintOptions.None;
		chbUseColors.Checked = (Options & PrintOptions.UseColors) > PrintOptions.None;
		chbDisplayProgress.Checked = (Options & PrintOptions.DisplayProgress) > PrintOptions.None;
	}

	private void OptionsFromControls()
	{
		Options = (chbPrintSelection.Checked ? (Options | PrintOptions.PrintSelection) : (Options & ~PrintOptions.PrintSelection));
		Options = (chbUseHeader.Checked ? (Options | PrintOptions.UseHeader) : (Options & ~PrintOptions.UseHeader));
		Options = (chbUseFooter.Checked ? (Options | PrintOptions.UseFooter) : (Options & ~PrintOptions.UseFooter));
		Options = (chbLineNumbers.Checked ? (Options | PrintOptions.LineNumbers) : (Options & ~PrintOptions.LineNumbers));
		Options = (chbPageNumbers.Checked ? (Options | PrintOptions.PageNumbers) : (Options & ~PrintOptions.PageNumbers));
		Options = (chbWordWrap.Checked ? (Options | PrintOptions.WordWrap) : (Options & ~PrintOptions.WordWrap));
		Options = (chbUseSyntax.Checked ? (Options | PrintOptions.UseSyntax) : (Options & ~PrintOptions.UseSyntax));
		Options = (chbUseColors.Checked ? (Options | PrintOptions.UseColors) : (Options & ~PrintOptions.UseColors));
		Options = (chbDisplayProgress.Checked ? (Options | PrintOptions.DisplayProgress) : (Options & ~PrintOptions.DisplayProgress));
	}

	private void DlgPrintOptions_Load(object sender, EventArgs e)
	{
		LoadFromResource();
		ControlsFromOptions();
	}

	private void LoadFromResource()
	{
		((Control)this).Text = StringConsts.DlgPrintOptionsCaption;
		((Control)gbFileToPrint).Text = StringConsts.FileToPrintCaption;
		((Control)gbOptions).Text = StringConsts.OptionsCaption;
		((Control)laFileName).Text = StringConsts.FileNameCaption;
		((Control)chbPrintSelection).Text = StringConsts.PrintSelectionCaption;
		((Control)chbUseHeader).Text = StringConsts.UseHeaderCaption;
		((Control)chbUseFooter).Text = StringConsts.UseFooterCaption;
		((Control)chbLineNumbers).Text = StringConsts.LineNumbersCaption_PrintOptionsDlg;
		((Control)chbPageNumbers).Text = StringConsts.PageNumbersCaption;
		((Control)chbWordWrap).Text = StringConsts.WordWrapCaption_PrintOptionsDlg;
		((Control)chbUseSyntax).Text = StringConsts.UseSyntaxCaption;
		((Control)chbUseColors).Text = StringConsts.UseColorsCaption;
		((Control)chbDisplayProgress).Text = StringConsts.DisplayProgressCaption;
		((Control)btOk).Text = StringConsts.OkCaption_PrintOptionsDlg;
		((Control)btCancel).Text = StringConsts.CancelCaption_PrintOptionsDlg;
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		OptionsFromControls();
	}

	public virtual void ResetOptions()
	{
		Options = EditConsts.DefaultPrintOptions;
	}

	public virtual void ResetAllowedOptions()
	{
		AllowedOptions = EditConsts.DefaultPrintOptions;
	}

	DialogResult IPrintOptionsDialog.ShowDialog()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((Form)this).ShowDialog();
	}
}
