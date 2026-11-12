using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design;

public class StiStylesCreateForm : Form
{
	private StiStyleDesigner styleDesigner;

	private StiReport report;

	private IContainer components;

	private StiGroupLine glButtons;

	private StiStyleViewReportControl viewReportControl;

	private Button btCancel;

	private Button btOk;

	private ColorPickerButton buttonColor;

	private Label labelColor;

	private ErrorProvider errorProvider1;

	private StiComboBox comboBoxNestedLevel;

	private Label labelNestedLevel;

	private StiTextBox textBoxCollectionName;

	private Label labelCollectionName;

	private StiGroupLine stiGroupLine1;

	private CheckBox checkBoxHeader;

	private CheckBox checkBoxData;

	private CheckBox checkBoxFooter;

	private Label labelFactor;

	private StiComboBox comboBoxNestedFactor;

	private CheckBox checkBoxGroupHeader;

	private CheckBox checkBoxReportTitle;

	private CheckBox checkBoxGroupFooter;

	private CheckBox checkBoxReportSummary;

	private CheckBox checkBoxPageHeader;

	private CheckBox checkBoxPageFooter;

	private Panel panel1;

	private Panel panelViewReport;

	private StiGroupLine stiGroupLine2;

	private CheckBox checkBoxBorders;

	internal CheckBox checkBoxRemoveExistingStyles;

	public List<StiBaseStyle> Styles { get; private set; }

	public StiStylesCreateForm(StiStyleDesigner styleDesigner, StiReport report)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.styleDesigner = styleDesigner;
			this.report = report;
			Localize();
			((Form)this).ShowIcon = false;
			((ListControl)comboBoxNestedFactor).SelectedIndex = 1;
			((ListControl)comboBoxNestedLevel).SelectedIndex = 3;
			((Control)textBoxCollectionName).Text = GetNewCollectionName();
			StiSettings.Load();
			checkBoxRemoveExistingStyles.Checked = StiSettings.GetBool(((Control)this).Name, "RemoveExistingStyles", checkBoxRemoveExistingStyles.Checked);
			checkBoxBorders.Checked = StiSettings.GetBool(((Control)this).Name, "ShowBorders", checkBoxBorders.Checked);
			((ListControl)comboBoxNestedLevel).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "NestedLevel", ((ListControl)comboBoxNestedLevel).SelectedIndex);
			((ListControl)comboBoxNestedFactor).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "NestedFactor", ((ListControl)comboBoxNestedFactor).SelectedIndex);
			buttonColor.SelectedColor = StiSettings.GetColor(((Control)this).Name, "SelectedColor", buttonColor.SelectedColor);
			checkBoxReportTitle.Checked = StiSettings.GetBool(((Control)this).Name, "ReportTitle", checkBoxReportTitle.Checked);
			checkBoxReportSummary.Checked = StiSettings.GetBool(((Control)this).Name, "ReportSummary", checkBoxReportSummary.Checked);
			checkBoxPageHeader.Checked = StiSettings.GetBool(((Control)this).Name, "PageHeader", checkBoxPageHeader.Checked);
			checkBoxPageFooter.Checked = StiSettings.GetBool(((Control)this).Name, "PageFooter", checkBoxPageFooter.Checked);
			checkBoxGroupHeader.Checked = StiSettings.GetBool(((Control)this).Name, "GroupHeader", checkBoxGroupHeader.Checked);
			checkBoxGroupFooter.Checked = StiSettings.GetBool(((Control)this).Name, "GroupFooter", checkBoxGroupFooter.Checked);
			checkBoxHeader.Checked = StiSettings.GetBool(((Control)this).Name, "Header", checkBoxHeader.Checked);
			checkBoxData.Checked = StiSettings.GetBool(((Control)this).Name, "Data", checkBoxData.Checked);
			checkBoxFooter.Checked = StiSettings.GetBool(((Control)this).Name, "Footer", checkBoxFooter.Checked);
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelColor).Text = Loc.GetCleaned("Report", "LabelColor");
		((Control)labelNestedLevel).Text = Loc.GetCleaned("Report", "LabelNestedLevel");
		((Control)labelCollectionName).Text = Loc.GetCleaned("Report", "LabelCollectionName");
		((Control)labelFactor).Text = Loc.GetCleaned("Report", "LabelFactorLevel");
		((Control)this).Text = StiLocalization.Get("FormStyleDesigner", "CreateStyleCollection");
		((Control)checkBoxRemoveExistingStyles).Text = StiLocalization.Get("FormStyleDesigner", "RemoveExistingStyles");
		((Control)checkBoxBorders).Text = StiLocalization.Get("PropertyMain", "Borders");
		((Control)checkBoxReportTitle).Text = StiLocalization.Get("Components", "StiReportTitleBand");
		((Control)checkBoxReportSummary).Text = StiLocalization.Get("Components", "StiReportSummaryBand");
		((Control)checkBoxPageHeader).Text = StiLocalization.Get("Components", "StiPageHeaderBand");
		((Control)checkBoxPageFooter).Text = StiLocalization.Get("Components", "StiPageFooterBand");
		((Control)checkBoxGroupHeader).Text = StiLocalization.Get("Components", "StiGroupHeaderBand");
		((Control)checkBoxGroupFooter).Text = StiLocalization.Get("Components", "StiGroupFooterBand");
		((Control)checkBoxHeader).Text = StiLocalization.Get("Components", "StiHeaderBand");
		((Control)checkBoxData).Text = StiLocalization.Get("Components", "StiDataBand");
		((Control)checkBoxFooter).Text = StiLocalization.Get("Components", "StiFooterBand");
		((ComboBox)comboBoxNestedFactor).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiNestedFactorHigh"));
		((ComboBox)comboBoxNestedFactor).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiNestedFactorNormal"));
		((ComboBox)comboBoxNestedFactor).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiNestedFactorLow"));
	}

	private void DoScaling()
	{
		StiComboBox stiComboBox = comboBoxNestedLevel;
		int width2 = (((Control)comboBoxNestedFactor).Width = ((Control)buttonColor).Width);
		((Control)stiComboBox).Width = width2;
	}

	private string GetNewCollectionName()
	{
		string text = StiLocalization.Get("Report", "Collection");
		int num = 1;
		string text2;
		while (true)
		{
			text2 = text + num;
			if (!styleDesigner.HashStyles.ContainsKey(text2))
			{
				break;
			}
			num++;
		}
		return text2;
	}

	private void RefreshViewReportControl()
	{
		viewReportControl.BaseColor = buttonColor.SelectedColor;
		viewReportControl.MaxNestedLevel = ((ListControl)comboBoxNestedLevel).SelectedIndex + 1;
		if (((Control)comboBoxNestedFactor).Text == StiLocalization.Get("PropertyEnum", "StiNestedFactorHigh"))
		{
			viewReportControl.NestedFactor = StiNestedFactor.High;
		}
		if (((Control)comboBoxNestedFactor).Text == StiLocalization.Get("PropertyEnum", "StiNestedFactorNormal"))
		{
			viewReportControl.NestedFactor = StiNestedFactor.Normal;
		}
		if (((Control)comboBoxNestedFactor).Text == StiLocalization.Get("PropertyEnum", "StiNestedFactorLow"))
		{
			viewReportControl.NestedFactor = StiNestedFactor.Low;
		}
		viewReportControl.Borders = checkBoxBorders.Checked;
		viewReportControl.GroupHeader = checkBoxGroupHeader.Checked;
		viewReportControl.GroupFooter = checkBoxGroupFooter.Checked;
		viewReportControl.Header = checkBoxHeader.Checked;
		viewReportControl.Data = checkBoxData.Checked;
		viewReportControl.Footer = checkBoxFooter.Checked;
		viewReportControl.ReportTitle = checkBoxReportTitle.Checked;
		viewReportControl.ReportSummary = checkBoxReportSummary.Checked;
		viewReportControl.PageHeader = checkBoxPageHeader.Checked;
		viewReportControl.PageFooter = checkBoxPageFooter.Checked;
		((Control)viewReportControl).Invalidate();
	}

	private void DrawBorderShadow(Graphics g)
	{
		Rectangle rectangle = new Rectangle(((Control)viewReportControl).Location.X, ((Control)viewReportControl).Location.Y, ((Control)viewReportControl).Width, ((Control)viewReportControl).Height);
		ShadowPaintInfo shadowPaintInfo = new ShadowPaintInfo();
		shadowPaintInfo.Graphics = g;
		shadowPaintInfo.Rectangle = rectangle;
		shadowPaintInfo.Size = 4;
		ShadowPainter.Paint(shadowPaintInfo);
	}

	private void CheckBoxBorders_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void ButtonColor_SelectedColorChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void ComboBoxNestedLevel_SelectedIndexChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void ComboBoxNestedFactor_SelectedIndexChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxGroupHeader_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxGroupFooter_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxHeader_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxData_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxFooter_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxReportTitle_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxReportSummary_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxPageHeader_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void CheckBoxPageFooter_CheckedChanged(object sender, EventArgs e)
	{
		RefreshViewReportControl();
	}

	private void PanelViewReport_Paint(object sender, PaintEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		DrawBorderShadow(graphics);
		Pen val = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(0, 0);
			Point point2 = new Point(0, ((Control)this).ClientRectangle.Bottom);
			graphics.DrawLine(val, point, point2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (checkBoxRemoveExistingStyles.Checked)
		{
			report.Styles.Clear();
		}
		Color selectedColor = buttonColor.SelectedColor;
		StiStylesCreator stiStylesCreator = new StiStylesCreator(styleDesigner.HashAllStyles)
		{
			ShowBorders = checkBoxBorders.Checked,
			MaxNestedLevel = ((ListControl)comboBoxNestedLevel).SelectedIndex + 1
		};
		if (((Control)comboBoxNestedFactor).Text == StiLocalization.Get("PropertyEnum", "StiNestedFactorHigh"))
		{
			stiStylesCreator.NestedFactor = StiNestedFactor.High;
		}
		if (((Control)comboBoxNestedFactor).Text == StiLocalization.Get("PropertyEnum", "StiNestedFactorNormal"))
		{
			stiStylesCreator.NestedFactor = StiNestedFactor.Normal;
		}
		if (((Control)comboBoxNestedFactor).Text == StiLocalization.Get("PropertyEnum", "StiNestedFactorLow"))
		{
			stiStylesCreator.NestedFactor = StiNestedFactor.Low;
		}
		stiStylesCreator.ShowReportTitles = checkBoxReportTitle.Checked;
		stiStylesCreator.ShowReportSummaries = checkBoxReportSummary.Checked;
		stiStylesCreator.ShowPageHeaders = checkBoxPageHeader.Checked;
		stiStylesCreator.ShowPageFooters = checkBoxPageFooter.Checked;
		stiStylesCreator.ShowGroupHeaders = checkBoxGroupHeader.Checked;
		stiStylesCreator.ShowGroupFooters = checkBoxGroupFooter.Checked;
		stiStylesCreator.ShowHeaders = checkBoxHeader.Checked;
		stiStylesCreator.ShowDatas = checkBoxData.Checked;
		stiStylesCreator.ShowFooters = checkBoxFooter.Checked;
		Styles = stiStylesCreator.CreateStyles(((Control)textBoxCollectionName).Text, selectedColor);
		((Form)this).Close();
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "RemoveExistingStyles", checkBoxRemoveExistingStyles.Checked);
		StiSettings.Set(((Control)this).Name, "ShowBorders", checkBoxBorders.Checked);
		StiSettings.Set(((Control)this).Name, "NestedLevel", ((ListControl)comboBoxNestedLevel).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "NestedFactor", ((ListControl)comboBoxNestedFactor).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "SelectedColor", buttonColor.SelectedColor);
		StiSettings.Set(((Control)this).Name, "ReportTitle", checkBoxReportTitle.Checked);
		StiSettings.Set(((Control)this).Name, "ReportSummary", checkBoxReportSummary.Checked);
		StiSettings.Set(((Control)this).Name, "PageHeader", checkBoxPageHeader.Checked);
		StiSettings.Set(((Control)this).Name, "PageFooter", checkBoxPageFooter.Checked);
		StiSettings.Set(((Control)this).Name, "GroupHeader", checkBoxGroupHeader.Checked);
		StiSettings.Set(((Control)this).Name, "GroupFooter", checkBoxGroupFooter.Checked);
		StiSettings.Set(((Control)this).Name, "Header", checkBoxHeader.Checked);
		StiSettings.Set(((Control)this).Name, "Data", checkBoxData.Checked);
		StiSettings.Set(((Control)this).Name, "Footer", checkBoxFooter.Checked);
		StiSettings.Save();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_appearance_style_designer_creating_collection_of_styles.htm");
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1038: Unknown result type (might be due to invalid IL or missing references)
		//IL_1042: Expected O, but got Unknown
		//IL_10dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_145b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1465: Expected O, but got Unknown
		components = new Container();
		buttonColor = new ColorPickerButton();
		labelColor = new Label();
		errorProvider1 = new ErrorProvider(components);
		comboBoxNestedLevel = new StiComboBox();
		labelNestedLevel = new Label();
		labelCollectionName = new Label();
		textBoxCollectionName = new StiTextBox();
		labelFactor = new Label();
		comboBoxNestedFactor = new StiComboBox();
		panel1 = new Panel();
		stiGroupLine2 = new StiGroupLine();
		checkBoxBorders = new CheckBox();
		checkBoxRemoveExistingStyles = new CheckBox();
		stiGroupLine1 = new StiGroupLine();
		checkBoxGroupHeader = new CheckBox();
		checkBoxReportTitle = new CheckBox();
		checkBoxGroupFooter = new CheckBox();
		checkBoxReportSummary = new CheckBox();
		checkBoxPageHeader = new CheckBox();
		checkBoxPageFooter = new CheckBox();
		checkBoxHeader = new CheckBox();
		checkBoxData = new CheckBox();
		checkBoxFooter = new CheckBox();
		panelViewReport = new Panel();
		viewReportControl = new StiStyleViewReportControl();
		btOk = new Button();
		btCancel = new Button();
		glButtons = new StiGroupLine();
		((ISupportInitialize)errorProvider1).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)stiGroupLine2).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)panelViewReport).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonColor).AccessibleRole = (AccessibleRole)43;
		buttonColor.AutoExpandOnClick = true;
		buttonColor.ColorTable = eButtonColor.OrangeWithBackground;
		buttonColor.Is20Button = false;
		((Control)buttonColor).Location = new Point(177, 45);
		buttonColor.LockClosePopup = false;
		((Control)buttonColor).Name = "buttonColor";
		((Control)buttonColor).Size = new Size(122, 23);
		((Control)buttonColor).TabIndex = 1;
		((Control)buttonColor).Text = "Color";
		buttonColor.ThemeAware = true;
		buttonColor.SelectedColorChanged += ButtonColor_SelectedColorChanged;
		((Control)labelColor).ForeColor = Color.DimGray;
		((Control)labelColor).Location = new Point(12, 45);
		((Control)labelColor).Name = "labelColor";
		((Control)labelColor).Size = new Size(159, 23);
		((Control)labelColor).TabIndex = 6;
		((Control)labelColor).Text = "Color";
		labelColor.TextAlign = (ContentAlignment)16;
		errorProvider1.ContainerControl = (ContainerControl)(object)this;
		comboBoxNestedLevel.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxNestedLevel).FormattingEnabled = true;
		((ComboBox)comboBoxNestedLevel).Items.AddRange(new object[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
		((Control)comboBoxNestedLevel).Location = new Point(177, 78);
		((Control)comboBoxNestedLevel).Name = "comboBoxNestedLevel";
		((Control)comboBoxNestedLevel).Size = new Size(121, 21);
		((Control)comboBoxNestedLevel).TabIndex = 2;
		((ComboBox)comboBoxNestedLevel).SelectedIndexChanged += ComboBoxNestedLevel_SelectedIndexChanged;
		((Control)labelNestedLevel).ForeColor = Color.DimGray;
		((Control)labelNestedLevel).Location = new Point(12, 81);
		((Control)labelNestedLevel).Name = "labelNestedLevel";
		((Control)labelNestedLevel).Size = new Size(159, 17);
		((Control)labelNestedLevel).TabIndex = 8;
		((Control)labelNestedLevel).Text = "Nested Level";
		labelNestedLevel.TextAlign = (ContentAlignment)16;
		((Control)labelCollectionName).ForeColor = Color.DimGray;
		((Control)labelCollectionName).Location = new Point(12, 12);
		((Control)labelCollectionName).Name = "labelCollectionName";
		((Control)labelCollectionName).Size = new Size(158, 16);
		((Control)labelCollectionName).TabIndex = 6;
		((Control)labelCollectionName).Text = "Collection Name";
		labelCollectionName.TextAlign = (ContentAlignment)16;
		((Control)textBoxCollectionName).BackColor = SystemColors.Window;
		((Control)textBoxCollectionName).Location = new Point(177, 12);
		((Control)textBoxCollectionName).Name = "textBoxCollectionName";
		((Control)textBoxCollectionName).Size = new Size(156, 21);
		((Control)textBoxCollectionName).TabIndex = 0;
		((Control)labelFactor).ForeColor = Color.DimGray;
		((Control)labelFactor).Location = new Point(12, 111);
		((Control)labelFactor).Name = "labelFactor";
		((Control)labelFactor).Size = new Size(159, 21);
		((Control)labelFactor).TabIndex = 10;
		((Control)labelFactor).Text = "Nested Factor";
		labelFactor.TextAlign = (ContentAlignment)16;
		comboBoxNestedFactor.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxNestedFactor).FormattingEnabled = true;
		((Control)comboBoxNestedFactor).Location = new Point(177, 111);
		((Control)comboBoxNestedFactor).Name = "comboBoxNestedFactor";
		((Control)comboBoxNestedFactor).Size = new Size(121, 21);
		((Control)comboBoxNestedFactor).TabIndex = 3;
		((ComboBox)comboBoxNestedFactor).SelectedIndexChanged += ComboBoxNestedFactor_SelectedIndexChanged;
		((Control)panel1).Controls.Add((Control)(object)stiGroupLine2);
		((Control)panel1).Controls.Add((Control)(object)labelCollectionName);
		((Control)panel1).Controls.Add((Control)(object)buttonColor);
		((Control)panel1).Controls.Add((Control)(object)stiGroupLine1);
		((Control)panel1).Controls.Add((Control)(object)labelFactor);
		((Control)panel1).Controls.Add((Control)(object)labelColor);
		((Control)panel1).Controls.Add((Control)(object)comboBoxNestedFactor);
		((Control)panel1).Controls.Add((Control)(object)comboBoxNestedLevel);
		((Control)panel1).Controls.Add((Control)(object)labelNestedLevel);
		((Control)panel1).Controls.Add((Control)(object)textBoxCollectionName);
		((Control)panel1).Dock = (DockStyle)3;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).MinimumSize = new Size(459, 300);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(459, 339);
		((Control)panel1).TabIndex = 0;
		((Control)stiGroupLine2).Anchor = (AnchorStyles)13;
		((Control)stiGroupLine2).BackColor = Color.White;
		((Control)stiGroupLine2).Controls.Add((Control)(object)checkBoxBorders);
		((Control)stiGroupLine2).Controls.Add((Control)(object)checkBoxRemoveExistingStyles);
		stiGroupLine2.LinePosition = StiLinePosition.Top;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(0, 147);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(459, 51);
		((Control)stiGroupLine2).TabIndex = 4;
		((Control)checkBoxBorders).AutoSize = true;
		checkBoxBorders.Checked = true;
		checkBoxBorders.CheckState = (CheckState)1;
		((Control)checkBoxBorders).Location = new Point(110, 12);
		((Control)checkBoxBorders).Name = "checkBoxBorders";
		((Control)checkBoxBorders).Size = new Size(62, 17);
		((Control)checkBoxBorders).TabIndex = 0;
		((Control)checkBoxBorders).Text = "Borders";
		((ButtonBase)checkBoxBorders).UseVisualStyleBackColor = true;
		checkBoxBorders.CheckedChanged += CheckBoxBorders_CheckedChanged;
		((Control)checkBoxRemoveExistingStyles).AutoSize = true;
		checkBoxRemoveExistingStyles.Checked = true;
		checkBoxRemoveExistingStyles.CheckState = (CheckState)1;
		((Control)checkBoxRemoveExistingStyles).Location = new Point(110, 35);
		((Control)checkBoxRemoveExistingStyles).Name = "checkBoxRemoveExistingStyles";
		((Control)checkBoxRemoveExistingStyles).Size = new Size(136, 17);
		((Control)checkBoxRemoveExistingStyles).TabIndex = 1;
		((Control)checkBoxRemoveExistingStyles).Text = "Remove Existing Styles";
		((ButtonBase)checkBoxRemoveExistingStyles).UseVisualStyleBackColor = true;
		((Control)stiGroupLine1).Anchor = (AnchorStyles)13;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxGroupHeader);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxReportTitle);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxGroupFooter);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxReportSummary);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxPageHeader);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxPageFooter);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxHeader);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxData);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxFooter);
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 208);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(459, 124);
		((Control)stiGroupLine1).TabIndex = 5;
		((Control)checkBoxGroupHeader).AutoSize = true;
		checkBoxGroupHeader.Checked = true;
		checkBoxGroupHeader.CheckState = (CheckState)1;
		((Control)checkBoxGroupHeader).Location = new Point(110, 12);
		((Control)checkBoxGroupHeader).Name = "checkBoxGroupHeader";
		((Control)checkBoxGroupHeader).Size = new Size(93, 17);
		((Control)checkBoxGroupHeader).TabIndex = 0;
		((Control)checkBoxGroupHeader).Text = "Group Header";
		((ButtonBase)checkBoxGroupHeader).UseVisualStyleBackColor = true;
		checkBoxGroupHeader.CheckedChanged += CheckBoxGroupHeader_CheckedChanged;
		((Control)checkBoxReportTitle).AutoSize = true;
		checkBoxReportTitle.Checked = true;
		checkBoxReportTitle.CheckState = (CheckState)1;
		((Control)checkBoxReportTitle).Location = new Point(275, 12);
		((Control)checkBoxReportTitle).Name = "checkBoxReportTitle";
		((Control)checkBoxReportTitle).Size = new Size(81, 17);
		((Control)checkBoxReportTitle).TabIndex = 5;
		((Control)checkBoxReportTitle).Text = "Report Title";
		((ButtonBase)checkBoxReportTitle).UseVisualStyleBackColor = true;
		checkBoxReportTitle.CheckedChanged += CheckBoxReportTitle_CheckedChanged;
		((Control)checkBoxGroupFooter).AutoSize = true;
		checkBoxGroupFooter.Checked = true;
		checkBoxGroupFooter.CheckState = (CheckState)1;
		((Control)checkBoxGroupFooter).Location = new Point(110, 35);
		((Control)checkBoxGroupFooter).Name = "checkBoxGroupFooter";
		((Control)checkBoxGroupFooter).Size = new Size(88, 17);
		((Control)checkBoxGroupFooter).TabIndex = 1;
		((Control)checkBoxGroupFooter).Text = "Group Footer";
		((ButtonBase)checkBoxGroupFooter).UseVisualStyleBackColor = true;
		checkBoxGroupFooter.CheckedChanged += CheckBoxGroupFooter_CheckedChanged;
		((Control)checkBoxReportSummary).AutoSize = true;
		checkBoxReportSummary.Checked = true;
		checkBoxReportSummary.CheckState = (CheckState)1;
		((Control)checkBoxReportSummary).Location = new Point(275, 35);
		((Control)checkBoxReportSummary).Name = "checkBoxReportSummary";
		((Control)checkBoxReportSummary).Size = new Size(104, 17);
		((Control)checkBoxReportSummary).TabIndex = 6;
		((Control)checkBoxReportSummary).Text = "Report Summary";
		((ButtonBase)checkBoxReportSummary).UseVisualStyleBackColor = true;
		checkBoxReportSummary.CheckedChanged += CheckBoxReportSummary_CheckedChanged;
		((Control)checkBoxPageHeader).AutoSize = true;
		checkBoxPageHeader.Checked = true;
		checkBoxPageHeader.CheckState = (CheckState)1;
		((Control)checkBoxPageHeader).Location = new Point(275, 58);
		((Control)checkBoxPageHeader).Name = "checkBoxPageHeader";
		((Control)checkBoxPageHeader).Size = new Size(89, 17);
		((Control)checkBoxPageHeader).TabIndex = 7;
		((Control)checkBoxPageHeader).Text = "Page Header";
		((ButtonBase)checkBoxPageHeader).UseVisualStyleBackColor = true;
		checkBoxPageHeader.CheckedChanged += CheckBoxPageHeader_CheckedChanged;
		((Control)checkBoxPageFooter).AutoSize = true;
		checkBoxPageFooter.Checked = true;
		checkBoxPageFooter.CheckState = (CheckState)1;
		((Control)checkBoxPageFooter).Location = new Point(275, 81);
		((Control)checkBoxPageFooter).Name = "checkBoxPageFooter";
		((Control)checkBoxPageFooter).Size = new Size(84, 17);
		((Control)checkBoxPageFooter).TabIndex = 8;
		((Control)checkBoxPageFooter).Text = "Page Footer";
		((ButtonBase)checkBoxPageFooter).UseVisualStyleBackColor = true;
		checkBoxPageFooter.CheckedChanged += CheckBoxPageFooter_CheckedChanged;
		((Control)checkBoxHeader).AutoSize = true;
		checkBoxHeader.Checked = true;
		checkBoxHeader.CheckState = (CheckState)1;
		((Control)checkBoxHeader).Location = new Point(110, 58);
		((Control)checkBoxHeader).Name = "checkBoxHeader";
		((Control)checkBoxHeader).Size = new Size(61, 17);
		((Control)checkBoxHeader).TabIndex = 2;
		((Control)checkBoxHeader).Text = "Header";
		((ButtonBase)checkBoxHeader).UseVisualStyleBackColor = true;
		checkBoxHeader.CheckedChanged += CheckBoxHeader_CheckedChanged;
		((Control)checkBoxData).AutoSize = true;
		checkBoxData.Checked = true;
		checkBoxData.CheckState = (CheckState)1;
		((Control)checkBoxData).Location = new Point(110, 81);
		((Control)checkBoxData).Name = "checkBoxData";
		((Control)checkBoxData).Size = new Size(49, 17);
		((Control)checkBoxData).TabIndex = 3;
		((Control)checkBoxData).Text = "Data";
		((ButtonBase)checkBoxData).UseVisualStyleBackColor = true;
		checkBoxData.CheckedChanged += CheckBoxData_CheckedChanged;
		((Control)checkBoxFooter).AutoSize = true;
		checkBoxFooter.Checked = true;
		checkBoxFooter.CheckState = (CheckState)1;
		((Control)checkBoxFooter).Location = new Point(110, 104);
		((Control)checkBoxFooter).Name = "checkBoxFooter";
		((Control)checkBoxFooter).Size = new Size(56, 17);
		((Control)checkBoxFooter).TabIndex = 4;
		((Control)checkBoxFooter).Text = "Footer";
		((ButtonBase)checkBoxFooter).UseVisualStyleBackColor = true;
		checkBoxFooter.CheckedChanged += CheckBoxFooter_CheckedChanged;
		((Control)panelViewReport).Controls.Add((Control)(object)viewReportControl);
		((Control)panelViewReport).Dock = (DockStyle)5;
		((Control)panelViewReport).Location = new Point(459, 0);
		((Control)panelViewReport).Name = "panelViewReport";
		((Control)panelViewReport).Padding = new Padding(10, 6, 4, 4);
		((Control)panelViewReport).Size = new Size(232, 339);
		((Control)panelViewReport).TabIndex = 2;
		((Control)panelViewReport).Paint += new PaintEventHandler(PanelViewReport_Paint);
		((Control)viewReportControl).BackColor = SystemColors.ButtonHighlight;
		viewReportControl.BaseColor = Color.White;
		viewReportControl.Borders = true;
		((UserControl)viewReportControl).BorderStyle = (BorderStyle)1;
		viewReportControl.Data = true;
		((Control)viewReportControl).Dock = (DockStyle)5;
		viewReportControl.Footer = true;
		viewReportControl.GroupFooter = true;
		viewReportControl.GroupHeader = true;
		viewReportControl.Header = true;
		((Control)viewReportControl).Location = new Point(10, 6);
		((Control)viewReportControl).Margin = new Padding(6);
		((Control)viewReportControl).MaximumSize = new Size(217, 307);
		viewReportControl.MaxNestedLevel = 2;
		((Control)viewReportControl).MinimumSize = new Size(217, 2);
		((Control)viewReportControl).Name = "viewReportControl";
		viewReportControl.NestedFactor = StiNestedFactor.Low;
		((Control)viewReportControl).Padding = new Padding(10);
		viewReportControl.PageFooter = true;
		viewReportControl.PageHeader = true;
		viewReportControl.ReportSummary = true;
		viewReportControl.ReportTitle = true;
		((Control)viewReportControl).Size = new Size(217, 307);
		((Control)viewReportControl).TabIndex = 0;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(522, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 25);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(604, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 25);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 339);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(691, 48);
		((Control)glButtons).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(691, 387);
		((Control)this).Controls.Add((Control)(object)panelViewReport);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).DoubleBuffered = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(692, 353);
		((Control)this).Name = "StiStylesCreateForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Create New Styles";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((ISupportInitialize)errorProvider1).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)stiGroupLine2).ResumeLayout(false);
		((Control)stiGroupLine2).PerformLayout();
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)stiGroupLine1).PerformLayout();
		((Control)panelViewReport).ResumeLayout(false);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
