using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Viewer;

public class StiPageSizeForm : Form
{
	private StiGroupLine glButtons;

	private Button btOk;

	private Button btCancel;

	private GroupBox gbMargins;

	private TextBox ebBottom;

	private Label lbBottom;

	private TextBox ebTop;

	private Label lbTop;

	private TextBox ebRight;

	private Label lbRight;

	private TextBox ebLeft;

	private Label lbLeft;

	private GroupBox gbPaper;

	private Label lbSize;

	private ComboBox cbSize;

	private TextBox tbHeight;

	private Label lbHeight;

	private TextBox tbWidth;

	private Label lbWidth;

	private GroupBox gbOrientation;

	private RadioButton rbLandscape;

	private RadioButton rbPortrait;

	public CheckBox cbScaleContent;

	private GroupBox gbApplyTo;

	public RadioButton rbCurrentPage;

	public RadioButton rbAll;

	private Container components;

	public CheckBox checkBoxRebuildReport;

	protected bool textLock;

	private PrinterSettings printerSetting = new PrinterSettings();

	private StiPage page;

	public double PageWidth => double.Parse(((Control)tbWidth).Text);

	public double PageHeight => double.Parse(((Control)tbHeight).Text);

	public StiMargins Margins => new StiMargins(double.Parse(((Control)ebLeft).Text), double.Parse(((Control)ebRight).Text), double.Parse(((Control)ebTop).Text), double.Parse(((Control)ebBottom).Text));

	public StiPageOrientation PageOrientation
	{
		get
		{
			if (rbPortrait.Checked)
			{
				return StiPageOrientation.Portrait;
			}
			return StiPageOrientation.Landscape;
		}
	}

	public PaperKind PaperKind
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			PaperKind result = (PaperKind)0;
			int num = 0;
			int num2 = 0;
			try
			{
				if (PageOrientation == StiPageOrientation.Landscape)
				{
					num = (int)Math.Round(page.Unit.ConvertToHInches(PageHeight), 0);
					num2 = (int)Math.Round(page.Unit.ConvertToHInches(PageWidth), 0);
				}
				else
				{
					num = (int)Math.Round(page.Unit.ConvertToHInches(PageWidth), 0);
					num2 = (int)Math.Round(page.Unit.ConvertToHInches(PageHeight), 0);
				}
			}
			catch
			{
				num = (int)Math.Round(page.Unit.ConvertToHInches(page.PageWidth), 0);
				num2 = (int)Math.Round(page.Unit.ConvertToHInches(page.PageHeight), 0);
			}
			foreach (PaperSize paperSize in printerSetting.PaperSizes)
			{
				PaperSize val = paperSize;
				if ((Math.Abs(val.Width - num) < 10 && Math.Abs(val.Height - num2) < 10) || (Math.Abs(val.Height - num) < 10 && Math.Abs(val.Width - num2) < 10))
				{
					result = val.Kind;
				}
			}
			return result;
		}
	}

	public bool IsPageOrientationChanged => page.Orientation != PageOrientation;

	public StiPageSizeForm(StiPage page)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.page = page;
			rbPortrait.Checked = page.Orientation == StiPageOrientation.Portrait;
			rbLandscape.Checked = page.Orientation == StiPageOrientation.Landscape;
			((Control)tbWidth).Text = page.PageWidth.ToString();
			((Control)tbHeight).Text = page.PageHeight.ToString();
			((Control)ebLeft).Text = page.Margins.Left.ToString();
			((Control)ebRight).Text = page.Margins.Right.ToString();
			((Control)ebTop).Text = page.Margins.Top.ToString();
			((Control)ebBottom).Text = page.Margins.Bottom.ToString();
			cbSize.Items.Clear();
			((Control)cbSize).Text = "Custom";
			Scan();
			Localize();
			StiSettings.Load();
			cbScaleContent.Checked = StiSettings.GetBool(((Control)this).Name, "ScaleContent", cbScaleContent.Checked);
			rbAll.Checked = StiSettings.GetBool(((Control)this).Name, "All", rbAll.Checked);
			rbCurrentPage.Checked = StiSettings.GetBool(((Control)this).Name, "CurrentPage", rbCurrentPage.Checked);
			if (page.Report.Pages.Count == 1 && page.Report.Pages[0].Components.Count == 0)
			{
				((Control)checkBoxRebuildReport).Enabled = false;
				checkBoxRebuildReport.Checked = false;
			}
			else
			{
				checkBoxRebuildReport.Checked = StiSettings.GetBool(((Control)this).Name, "RebuildReport", checkBoxRebuildReport.Checked);
				((Control)checkBoxRebuildReport).Enabled = !page.Report.IsDocument;
			}
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
		glButtons = new StiGroupLine();
		btOk = new Button();
		btCancel = new Button();
		gbApplyTo = new GroupBox();
		rbAll = new RadioButton();
		rbCurrentPage = new RadioButton();
		gbMargins = new GroupBox();
		ebBottom = new TextBox();
		lbBottom = new Label();
		ebTop = new TextBox();
		lbTop = new Label();
		ebRight = new TextBox();
		lbRight = new Label();
		ebLeft = new TextBox();
		lbLeft = new Label();
		gbPaper = new GroupBox();
		lbSize = new Label();
		cbSize = new ComboBox();
		tbHeight = new TextBox();
		lbHeight = new Label();
		tbWidth = new TextBox();
		lbWidth = new Label();
		gbOrientation = new GroupBox();
		rbLandscape = new RadioButton();
		rbPortrait = new RadioButton();
		cbScaleContent = new CheckBox();
		checkBoxRebuildReport = new CheckBox();
		((Control)glButtons).SuspendLayout();
		((Control)gbApplyTo).SuspendLayout();
		((Control)gbMargins).SuspendLayout();
		((Control)gbPaper).SuspendLayout();
		((Control)gbOrientation).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 417);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(304, 46);
		((Control)glButtons).TabIndex = 6;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(139, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += btOk_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(219, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += btCancel_Click;
		((Control)gbApplyTo).Controls.Add((Control)(object)rbAll);
		((Control)gbApplyTo).Controls.Add((Control)(object)rbCurrentPage);
		((Control)gbApplyTo).Location = new Point(12, 279);
		((Control)gbApplyTo).Name = "gbApplyTo";
		((Control)gbApplyTo).Size = new Size(282, 72);
		((Control)gbApplyTo).TabIndex = 3;
		gbApplyTo.TabStop = false;
		((Control)gbApplyTo).Text = "Apply to";
		rbAll.Checked = true;
		((Control)rbAll).ForeColor = SystemColors.ControlText;
		((Control)rbAll).Location = new Point(12, 16);
		((Control)rbAll).Name = "rbAll";
		((Control)rbAll).Size = new Size(260, 24);
		((Control)rbAll).TabIndex = 0;
		rbAll.TabStop = true;
		((Control)rbAll).Text = "All";
		((Control)rbCurrentPage).ForeColor = SystemColors.ControlText;
		((Control)rbCurrentPage).Location = new Point(12, 40);
		((Control)rbCurrentPage).Name = "rbCurrentPage";
		((Control)rbCurrentPage).Size = new Size(260, 24);
		((Control)rbCurrentPage).TabIndex = 1;
		((Control)rbCurrentPage).Text = "Current Page";
		((Control)gbMargins).Controls.Add((Control)(object)ebBottom);
		((Control)gbMargins).Controls.Add((Control)(object)lbBottom);
		((Control)gbMargins).Controls.Add((Control)(object)ebTop);
		((Control)gbMargins).Controls.Add((Control)(object)lbTop);
		((Control)gbMargins).Controls.Add((Control)(object)ebRight);
		((Control)gbMargins).Controls.Add((Control)(object)lbRight);
		((Control)gbMargins).Controls.Add((Control)(object)ebLeft);
		((Control)gbMargins).Controls.Add((Control)(object)lbLeft);
		((Control)gbMargins).Location = new Point(12, 196);
		((Control)gbMargins).Name = "gbMargins";
		((Control)gbMargins).Size = new Size(282, 77);
		((Control)gbMargins).TabIndex = 2;
		gbMargins.TabStop = false;
		((Control)gbMargins).Text = "Margins";
		((Control)ebBottom).ForeColor = SystemColors.ControlText;
		((Control)ebBottom).Location = new Point(226, 48);
		((Control)ebBottom).Name = "ebBottom";
		((Control)ebBottom).Size = new Size(48, 20);
		((Control)ebBottom).TabIndex = 3;
		ebBottom.TextAlign = (HorizontalAlignment)1;
		((Control)lbBottom).ForeColor = Color.DimGray;
		((Control)lbBottom).Location = new Point(146, 48);
		((Control)lbBottom).Name = "lbBottom";
		((Control)lbBottom).Size = new Size(70, 16);
		((Control)lbBottom).TabIndex = 7;
		((Control)lbBottom).Text = "Bottom";
		lbBottom.TextAlign = (ContentAlignment)16;
		((Control)ebTop).ForeColor = SystemColors.ControlText;
		((Control)ebTop).Location = new Point(226, 19);
		((Control)ebTop).Name = "ebTop";
		((Control)ebTop).Size = new Size(48, 20);
		((Control)ebTop).TabIndex = 2;
		ebTop.TextAlign = (HorizontalAlignment)1;
		((Control)lbTop).ForeColor = Color.DimGray;
		((Control)lbTop).Location = new Point(146, 19);
		((Control)lbTop).Name = "lbTop";
		((Control)lbTop).Size = new Size(70, 16);
		((Control)lbTop).TabIndex = 6;
		((Control)lbTop).Text = "Top";
		lbTop.TextAlign = (ContentAlignment)16;
		((Control)ebRight).ForeColor = SystemColors.ControlText;
		((Control)ebRight).Location = new Point(87, 48);
		((Control)ebRight).Name = "ebRight";
		((Control)ebRight).Size = new Size(48, 20);
		((Control)ebRight).TabIndex = 1;
		ebRight.TextAlign = (HorizontalAlignment)1;
		((Control)lbRight).ForeColor = Color.DimGray;
		((Control)lbRight).Location = new Point(12, 48);
		((Control)lbRight).Name = "lbRight";
		((Control)lbRight).Size = new Size(68, 16);
		((Control)lbRight).TabIndex = 5;
		((Control)lbRight).Text = "Right";
		lbRight.TextAlign = (ContentAlignment)16;
		((Control)ebLeft).ForeColor = SystemColors.ControlText;
		((Control)ebLeft).Location = new Point(87, 19);
		((Control)ebLeft).Name = "ebLeft";
		((Control)ebLeft).Size = new Size(48, 20);
		((Control)ebLeft).TabIndex = 0;
		ebLeft.TextAlign = (HorizontalAlignment)1;
		((Control)lbLeft).ForeColor = Color.DimGray;
		((Control)lbLeft).Location = new Point(12, 19);
		((Control)lbLeft).Name = "lbLeft";
		((Control)lbLeft).Size = new Size(68, 16);
		((Control)lbLeft).TabIndex = 4;
		((Control)lbLeft).Text = "Left";
		lbLeft.TextAlign = (ContentAlignment)16;
		((Control)gbPaper).Controls.Add((Control)(object)lbSize);
		((Control)gbPaper).Controls.Add((Control)(object)cbSize);
		((Control)gbPaper).Controls.Add((Control)(object)tbHeight);
		((Control)gbPaper).Controls.Add((Control)(object)lbHeight);
		((Control)gbPaper).Controls.Add((Control)(object)tbWidth);
		((Control)gbPaper).Controls.Add((Control)(object)lbWidth);
		((Control)gbPaper).Location = new Point(12, 7);
		((Control)gbPaper).Name = "gbPaper";
		((Control)gbPaper).Size = new Size(282, 105);
		((Control)gbPaper).TabIndex = 0;
		gbPaper.TabStop = false;
		((Control)gbPaper).Text = "Paper";
		((Control)lbSize).ForeColor = Color.DimGray;
		((Control)lbSize).Location = new Point(12, 19);
		((Control)lbSize).Name = "lbSize";
		((Control)lbSize).Size = new Size(70, 17);
		((Control)lbSize).TabIndex = 3;
		((Control)lbSize).Text = "Size";
		lbSize.TextAlign = (ContentAlignment)16;
		((Control)cbSize).Anchor = (AnchorStyles)13;
		((Control)cbSize).BackColor = SystemColors.Window;
		cbSize.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbSize).ForeColor = SystemColors.ControlText;
		((Control)cbSize).Location = new Point(87, 19);
		cbSize.MaxDropDownItems = 20;
		((Control)cbSize).Name = "cbSize";
		((Control)cbSize).Size = new Size(186, 21);
		((Control)cbSize).TabIndex = 0;
		cbSize.SelectedIndexChanged += cbSize_SelectedIndexChanged;
		((Control)tbHeight).ForeColor = SystemColors.ControlText;
		((Control)tbHeight).Location = new Point(87, 77);
		((Control)tbHeight).Name = "tbHeight";
		((Control)tbHeight).Size = new Size(82, 20);
		((Control)tbHeight).TabIndex = 2;
		tbHeight.TextAlign = (HorizontalAlignment)1;
		((Control)lbHeight).ForeColor = Color.DimGray;
		((Control)lbHeight).Location = new Point(12, 77);
		((Control)lbHeight).Name = "lbHeight";
		((Control)lbHeight).Size = new Size(64, 16);
		((Control)lbHeight).TabIndex = 5;
		((Control)lbHeight).Text = "Height";
		lbHeight.TextAlign = (ContentAlignment)16;
		((Control)tbWidth).ForeColor = SystemColors.ControlText;
		((Control)tbWidth).Location = new Point(87, 48);
		((Control)tbWidth).Name = "tbWidth";
		((Control)tbWidth).Size = new Size(82, 20);
		((Control)tbWidth).TabIndex = 1;
		tbWidth.TextAlign = (HorizontalAlignment)1;
		((Control)lbWidth).ForeColor = Color.DimGray;
		((Control)lbWidth).Location = new Point(12, 48);
		((Control)lbWidth).Name = "lbWidth";
		((Control)lbWidth).Size = new Size(70, 16);
		((Control)lbWidth).TabIndex = 4;
		((Control)lbWidth).Text = "Width";
		lbWidth.TextAlign = (ContentAlignment)16;
		((Control)gbOrientation).Controls.Add((Control)(object)rbLandscape);
		((Control)gbOrientation).Controls.Add((Control)(object)rbPortrait);
		((Control)gbOrientation).Location = new Point(12, 118);
		((Control)gbOrientation).Name = "gbOrientation";
		((Control)gbOrientation).Size = new Size(282, 72);
		((Control)gbOrientation).TabIndex = 1;
		gbOrientation.TabStop = false;
		((Control)gbOrientation).Text = "Orientation";
		((Control)rbLandscape).ForeColor = SystemColors.ControlText;
		((Control)rbLandscape).Location = new Point(12, 40);
		((Control)rbLandscape).Name = "rbLandscape";
		((Control)rbLandscape).Size = new Size(260, 24);
		((Control)rbLandscape).TabIndex = 1;
		((Control)rbLandscape).Text = "Landscape";
		((Control)rbPortrait).ForeColor = SystemColors.ControlText;
		((Control)rbPortrait).Location = new Point(12, 16);
		((Control)rbPortrait).Name = "rbPortrait";
		((Control)rbPortrait).Size = new Size(260, 24);
		((Control)rbPortrait).TabIndex = 0;
		((Control)rbPortrait).Text = "Portrait";
		rbPortrait.CheckedChanged += rbPortrait_CheckedChanged;
		cbScaleContent.Checked = true;
		cbScaleContent.CheckState = (CheckState)1;
		((Control)cbScaleContent).Location = new Point(12, 359);
		((Control)cbScaleContent).Name = "cbScaleContent";
		((Control)cbScaleContent).Size = new Size(282, 24);
		((Control)cbScaleContent).TabIndex = 4;
		((Control)cbScaleContent).Text = "Scale Content";
		((Control)checkBoxRebuildReport).Location = new Point(12, 385);
		((Control)checkBoxRebuildReport).Name = "checkBoxRebuildReport";
		((Control)checkBoxRebuildReport).Size = new Size(284, 24);
		((Control)checkBoxRebuildReport).TabIndex = 5;
		((Control)checkBoxRebuildReport).Text = "Rebuild Report";
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(304, 463);
		((Control)this).Controls.Add((Control)(object)checkBoxRebuildReport);
		((Control)this).Controls.Add((Control)(object)cbScaleContent);
		((Control)this).Controls.Add((Control)(object)gbApplyTo);
		((Control)this).Controls.Add((Control)(object)gbMargins);
		((Control)this).Controls.Add((Control)(object)gbPaper);
		((Control)this).Controls.Add((Control)(object)gbOrientation);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiPageSizeForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "StiPageSizeForm";
		((Form)this).Closed += StiPageSizeForm_Closed;
		((Control)glButtons).ResumeLayout(false);
		((Control)gbApplyTo).ResumeLayout(false);
		((Control)gbMargins).ResumeLayout(false);
		((Control)gbMargins).PerformLayout();
		((Control)gbPaper).ResumeLayout(false);
		((Control)gbPaper).PerformLayout();
		((Control)gbOrientation).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private void cbSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (textLock)
		{
			return;
		}
		textLock = true;
		PaperSize val = printerSetting.PaperSizes[((ListControl)cbSize).SelectedIndex];
		((Control)cbSize).Text = val.PaperName;
		double value = val.Width;
		double value2 = val.Height;
		if (page.Unit is StiCentimetersUnit || page.Unit is StiMillimetersUnit)
		{
			value = PrinterUnitConvert.Convert(val.Width, (PrinterUnit)1, (PrinterUnit)3);
			value2 = PrinterUnitConvert.Convert(val.Height, (PrinterUnit)1, (PrinterUnit)3);
			if (page.Unit is StiCentimetersUnit)
			{
				value /= 10.0;
				value2 /= 10.0;
			}
		}
		else
		{
			value = Math.Round(page.ConvertFromHInches(page.Unit, value), 2);
			value2 = Math.Round(page.ConvertFromHInches(page.Unit, value2), 2);
		}
		if (rbLandscape.Checked)
		{
			((Control)tbHeight).Text = value.ToString();
			((Control)tbWidth).Text = value2.ToString();
		}
		else
		{
			((Control)tbWidth).Text = value.ToString();
			((Control)tbHeight).Text = value2.ToString();
		}
		textLock = false;
	}

	private void rbPortrait_CheckedChanged(object sender, EventArgs e)
	{
		string text = ((Control)ebLeft).Text;
		string text2 = ((Control)ebRight).Text;
		string text3 = ((Control)ebTop).Text;
		string text4 = ((Control)ebBottom).Text;
		string text5 = ((Control)tbHeight).Text;
		((Control)tbHeight).Text = ((Control)tbWidth).Text;
		((Control)tbWidth).Text = text5;
		if (rbLandscape.Checked)
		{
			((Control)ebLeft).Text = text3;
			((Control)ebRight).Text = text4;
			((Control)ebTop).Text = text2;
			((Control)ebBottom).Text = text;
		}
		else
		{
			((Control)ebLeft).Text = text4;
			((Control)ebRight).Text = text3;
			((Control)ebTop).Text = text;
			((Control)ebBottom).Text = text2;
		}
	}

	private void btCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void StiPageSizeForm_Closed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "ScaleContent", cbScaleContent.Checked);
		StiSettings.Set(((Control)this).Name, "All", rbAll.Checked);
		StiSettings.Set(((Control)this).Name, "CurrentPage", rbCurrentPage.Checked);
		StiSettings.Set(((Control)this).Name, "RebuildReport", checkBoxRebuildReport.Checked);
		StiSettings.Save();
	}

	private void Scan()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		int num = (int)Math.Round(page.Unit.ConvertToHInches(page.PageWidth), 0);
		int num2 = (int)Math.Round(page.Unit.ConvertToHInches(page.PageHeight), 0);
		textLock = true;
		string text = string.Empty;
		foreach (PaperSize paperSize in printerSetting.PaperSizes)
		{
			PaperSize val = paperSize;
			cbSize.Items.Add((object)val.PaperName);
			if (((Math.Abs(val.Width - num) < 10 && Math.Abs(val.Height - num2) < 10) || (Math.Abs(val.Height - num) < 10 && Math.Abs(val.Width - num2) < 10)) && text.Length == 0)
			{
				text = val.PaperName;
			}
		}
		((Control)cbSize).Text = text;
		if (((Control)cbSize).Text.Length == 0)
		{
			((ListControl)cbSize).SelectedIndex = cbSize.Items.Count - 1;
		}
		textLock = false;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "PageSizeForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)gbPaper).Text = StiLocalization.Get("PropertyMain", "Paper");
		((Control)lbSize).Text = Loc.GetCleaned("PropertyMain", "Size");
		((Control)lbWidth).Text = Loc.GetCleaned("FormPageSetup", "Width");
		((Control)lbHeight).Text = Loc.GetCleaned("FormPageSetup", "Height");
		((Control)gbOrientation).Text = StiLocalization.Get("PropertyMain", "Orientation");
		((Control)rbPortrait).Text = StiLocalization.Get("FormPageSetup", "PageOrientationPortrait");
		((Control)rbLandscape).Text = StiLocalization.Get("FormPageSetup", "PageOrientationLandscape");
		((Control)gbMargins).Text = StiLocalization.Get("PropertyMain", "Margins");
		((Control)lbLeft).Text = Loc.GetCleaned("FormPageSetup", "Left");
		((Control)lbRight).Text = Loc.GetCleaned("FormPageSetup", "Right");
		((Control)lbTop).Text = Loc.GetCleaned("FormPageSetup", "Top");
		((Control)lbBottom).Text = Loc.GetCleaned("FormPageSetup", "Bottom");
		((Control)gbApplyTo).Text = StiLocalization.Get("FormPageSetup", "ApplyTo");
		((Control)rbAll).Text = StiLocalization.Get("Report", "RangeAll");
		((Control)rbCurrentPage).Text = StiLocalization.Get("Report", "RangeCurrentPage");
		((Control)cbScaleContent).Text = StiLocalization.Get("FormPageSetup", "ScaleContent");
		((Control)checkBoxRebuildReport).Text = StiLocalization.Get("FormPageSetup", "RebuildReport");
	}
}
