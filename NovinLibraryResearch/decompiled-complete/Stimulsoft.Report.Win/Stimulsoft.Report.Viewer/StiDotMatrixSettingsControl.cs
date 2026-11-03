using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
public class StiDotMatrixSettingsControl : UserControl
{
	public CheckBox checkBoxKillSpaceGraphLines;

	public CheckBox checkBoxCutLongLines;

	public CheckBox cbDrawBorder;

	public CheckBox cbKillSpaceLines;

	public CheckBox cbPutFeedPageCode;

	public CheckBox rbSimple;

	public CheckBox rbUnicodeSingle;

	public CheckBox rbUnicodeDouble;

	private Label lbY;

	public ComboBoxEx cbZoomY;

	private Label lbX;

	public ComboBoxEx cbZoomX;

	public ComboBoxEx cbEncoding;

	public CheckBox checkBoxAutoRefresh;

	public Button buttonRefresh;

	public ExpandablePanel groupBoxSettings;

	private Panel panel1;

	private Panel panel2;

	public ExpandablePanel groupBoxBorderType;

	private Panel panel3;

	public ExpandablePanel groupBoxZoom;

	private Panel panel4;

	public ExpandablePanel groupBoxEncoding;

	public ExpandablePanel groupBoxRefresh;

	private Container components;

	private Panel panel5;

	public ExpandablePanel groupBoxEscapeCodes;

	public ComboBoxEx cbEscapeCodesCollectionList;

	public CheckBox cbUseEscapeCodes;

	internal StiDotMatrixViewerControl view;

	public StiDotMatrixSettingsControl()
	{
		InitializeComponent();
		Localize();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	public void Localize()
	{
		try
		{
			groupBoxEscapeCodes.TitleText = StiLocalization.Get("Export", "EscapeCodes");
			groupBoxEncoding.TitleText = StiLocalization.Get("Export", "Encoding").Replace(":", "");
			groupBoxBorderType.TitleText = StiLocalization.Get("Export", "TxtBorderType");
			groupBoxSettings.TitleText = StiLocalization.Get("Export", "Settings");
			groupBoxZoom.TitleText = StiLocalization.Get("FormViewer", "Zoom");
			groupBoxRefresh.TitleText = StiLocalization.Get("PropertyMain", "Refresh");
			((Control)checkBoxAutoRefresh).Text = StiLocalization.Get("PropertyMain", "AutoRefresh");
			((Control)buttonRefresh).Text = StiLocalization.Get("PropertyMain", "Refresh");
			((Control)rbSimple).Text = StiLocalization.Get("Export", "TxtBorderTypeSimple");
			((Control)rbUnicodeSingle).Text = StiLocalization.Get("Export", "TxtBorderTypeSingle");
			((Control)rbUnicodeDouble).Text = StiLocalization.Get("Export", "TxtBorderTypeDouble");
			((Control)cbKillSpaceLines).Text = StiLocalization.Get("Export", "TxtKillSpaceLines");
			((Control)checkBoxKillSpaceGraphLines).Text = StiLocalization.Get("Export", "TxtKillSpaceGraphLines");
			((Control)checkBoxCutLongLines).Text = StiLocalization.Get("Export", "TxtCutLongLines");
			((Control)cbPutFeedPageCode).Text = StiLocalization.Get("Export", "TxtPutFeedPageCode");
			((Control)cbDrawBorder).Text = StiLocalization.Get("Export", "TxtDrawBorder");
			((Control)cbUseEscapeCodes).Text = StiLocalization.Get("Export", "UseEscapeCodes");
			((Control)lbX).Text = StiLocalization.Get("Export", "X");
			((Control)lbY).Text = StiLocalization.Get("Export", "Y");
		}
		catch
		{
		}
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
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Expected O, but got Unknown
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Expected O, but got Unknown
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a79: Expected O, but got Unknown
		//IL_0cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce3: Expected O, but got Unknown
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1165: Expected O, but got Unknown
		//IL_157a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1584: Expected O, but got Unknown
		//IL_1a12: Unknown result type (might be due to invalid IL or missing references)
		panel1 = new Panel();
		panel2 = new Panel();
		panel3 = new Panel();
		panel4 = new Panel();
		panel5 = new Panel();
		groupBoxRefresh = new ExpandablePanel();
		checkBoxAutoRefresh = new CheckBox();
		buttonRefresh = new Button();
		groupBoxEscapeCodes = new ExpandablePanel();
		cbUseEscapeCodes = new CheckBox();
		cbEscapeCodesCollectionList = new ComboBoxEx();
		groupBoxEncoding = new ExpandablePanel();
		cbEncoding = new ComboBoxEx();
		groupBoxZoom = new ExpandablePanel();
		lbY = new Label();
		cbZoomX = new ComboBoxEx();
		cbZoomY = new ComboBoxEx();
		lbX = new Label();
		groupBoxBorderType = new ExpandablePanel();
		rbSimple = new CheckBox();
		rbUnicodeSingle = new CheckBox();
		rbUnicodeDouble = new CheckBox();
		groupBoxSettings = new ExpandablePanel();
		checkBoxKillSpaceGraphLines = new CheckBox();
		cbKillSpaceLines = new CheckBox();
		checkBoxCutLongLines = new CheckBox();
		cbPutFeedPageCode = new CheckBox();
		cbDrawBorder = new CheckBox();
		((Control)groupBoxRefresh).SuspendLayout();
		((Control)groupBoxEscapeCodes).SuspendLayout();
		((Control)groupBoxEncoding).SuspendLayout();
		((Control)groupBoxZoom).SuspendLayout();
		((Control)groupBoxBorderType).SuspendLayout();
		((Control)groupBoxSettings).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(4, 160);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(176, 8);
		((Control)panel1).TabIndex = 7;
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(4, 273);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(176, 8);
		((Control)panel2).TabIndex = 9;
		((Control)panel3).Dock = (DockStyle)1;
		((Control)panel3).Location = new Point(4, 368);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(176, 8);
		((Control)panel3).TabIndex = 11;
		((Control)panel4).Dock = (DockStyle)1;
		((Control)panel4).Location = new Point(4, 538);
		((Control)panel4).Name = "panel4";
		((Control)panel4).Size = new Size(176, 8);
		((Control)panel4).TabIndex = 13;
		((Control)panel5).Dock = (DockStyle)1;
		((Control)panel5).Location = new Point(4, 441);
		((Control)panel5).Name = "panel5";
		((Control)panel5).Size = new Size(176, 8);
		((Control)panel5).TabIndex = 14;
		((PanelEx)groupBoxRefresh).CanvasColor = SystemColors.Control;
		((PanelEx)groupBoxRefresh).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)groupBoxRefresh).Controls.Add((Control)(object)checkBoxAutoRefresh);
		((Control)groupBoxRefresh).Controls.Add((Control)(object)buttonRefresh);
		((Control)groupBoxRefresh).Dock = (DockStyle)1;
		groupBoxRefresh.ExpandOnTitleClick = true;
		((Control)groupBoxRefresh).Location = new Point(4, 546);
		((Control)groupBoxRefresh).Name = "groupBoxRefresh";
		((Control)groupBoxRefresh).Size = new Size(176, 97);
		((PanelEx)groupBoxRefresh).Style.Alignment = (StringAlignment)1;
		((PanelEx)groupBoxRefresh).Style.Border = (eBorderType)1;
		((PanelEx)groupBoxRefresh).Style.ForeColor.ColorSchemePart = (eColorSchemePart)40;
		((PanelEx)groupBoxRefresh).Style.GradientAngle = 90;
		((Control)groupBoxRefresh).TabIndex = 5;
		((PanelEx)groupBoxRefresh).ThemeAware = true;
		groupBoxRefresh.TitleStyle.Alignment = (StringAlignment)1;
		groupBoxRefresh.TitleStyle.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		groupBoxRefresh.TitleStyle.Border = (eBorderType)7;
		groupBoxRefresh.TitleStyle.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		groupBoxRefresh.TitleStyle.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		groupBoxRefresh.TitleStyle.GradientAngle = 90;
		groupBoxRefresh.TitleText = "Refresh";
		groupBoxRefresh.ExpandedChanged += new ExpandChangeEventHandler(groupBoxSettings_ExpandedChanged);
		((Control)checkBoxAutoRefresh).Anchor = (AnchorStyles)13;
		((Control)checkBoxAutoRefresh).BackColor = Color.Transparent;
		checkBoxAutoRefresh.Checked = true;
		checkBoxAutoRefresh.CheckState = (CheckState)1;
		((Control)checkBoxAutoRefresh).ForeColor = SystemColors.ControlText;
		((Control)checkBoxAutoRefresh).Location = new Point(8, 32);
		((Control)checkBoxAutoRefresh).Name = "checkBoxAutoRefresh";
		((Control)checkBoxAutoRefresh).Size = new Size(160, 24);
		((Control)checkBoxAutoRefresh).TabIndex = 0;
		((Control)checkBoxAutoRefresh).Text = "Auto Refresh";
		((ButtonBase)checkBoxAutoRefresh).UseVisualStyleBackColor = false;
		checkBoxAutoRefresh.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((Control)buttonRefresh).AccessibleRole = (AccessibleRole)43;
		((Control)buttonRefresh).Anchor = (AnchorStyles)13;
		((Control)buttonRefresh).Location = new Point(24, 64);
		((Control)buttonRefresh).Name = "buttonRefresh";
		((Control)buttonRefresh).Size = new Size(128, 23);
		((Control)buttonRefresh).TabIndex = 1;
		((Control)buttonRefresh).Text = "&Refresh";
		((PanelEx)groupBoxEscapeCodes).CanvasColor = SystemColors.Control;
		((PanelEx)groupBoxEscapeCodes).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)groupBoxEscapeCodes).Controls.Add((Control)(object)cbUseEscapeCodes);
		((Control)groupBoxEscapeCodes).Controls.Add((Control)(object)cbEscapeCodesCollectionList);
		((Control)groupBoxEscapeCodes).Dock = (DockStyle)1;
		groupBoxEscapeCodes.ExpandOnTitleClick = true;
		((Control)groupBoxEscapeCodes).Location = new Point(4, 449);
		((Control)groupBoxEscapeCodes).Name = "groupBoxEscapeCodes";
		((Control)groupBoxEscapeCodes).Size = new Size(176, 89);
		((PanelEx)groupBoxEscapeCodes).Style.Alignment = (StringAlignment)1;
		((PanelEx)groupBoxEscapeCodes).Style.Border = (eBorderType)1;
		((PanelEx)groupBoxEscapeCodes).Style.ForeColor.ColorSchemePart = (eColorSchemePart)40;
		((PanelEx)groupBoxEscapeCodes).Style.GradientAngle = 90;
		((Control)groupBoxEscapeCodes).TabIndex = 4;
		((PanelEx)groupBoxEscapeCodes).ThemeAware = true;
		groupBoxEscapeCodes.TitleStyle.Alignment = (StringAlignment)1;
		groupBoxEscapeCodes.TitleStyle.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		groupBoxEscapeCodes.TitleStyle.Border = (eBorderType)7;
		groupBoxEscapeCodes.TitleStyle.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		groupBoxEscapeCodes.TitleStyle.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		groupBoxEscapeCodes.TitleStyle.GradientAngle = 90;
		groupBoxEscapeCodes.TitleText = "Escape codes";
		groupBoxEscapeCodes.ExpandedChanged += new ExpandChangeEventHandler(groupBoxSettings_ExpandedChanged);
		((Control)cbUseEscapeCodes).Anchor = (AnchorStyles)13;
		((Control)cbUseEscapeCodes).BackColor = Color.Transparent;
		cbUseEscapeCodes.Checked = true;
		cbUseEscapeCodes.CheckState = (CheckState)1;
		((Control)cbUseEscapeCodes).ForeColor = SystemColors.ControlText;
		((Control)cbUseEscapeCodes).Location = new Point(8, 32);
		((Control)cbUseEscapeCodes).Name = "cbUseEscapeCodes";
		((Control)cbUseEscapeCodes).Size = new Size(160, 24);
		((Control)cbUseEscapeCodes).TabIndex = 0;
		((Control)cbUseEscapeCodes).Text = "Use escape codes";
		((ButtonBase)cbUseEscapeCodes).UseVisualStyleBackColor = false;
		cbUseEscapeCodes.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((Control)cbEscapeCodesCollectionList).Anchor = (AnchorStyles)13;
		((ComboBox)cbEscapeCodesCollectionList).DrawMode = (DrawMode)1;
		((ComboBox)cbEscapeCodesCollectionList).DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbEscapeCodesCollectionList).FlatStyle = (FlatStyle)0;
		((Control)cbEscapeCodesCollectionList).ForeColor = SystemColors.ControlText;
		((Control)cbEscapeCodesCollectionList).Location = new Point(8, 57);
		((Control)cbEscapeCodesCollectionList).Name = "cbEscapeCodesCollectionList";
		((Control)cbEscapeCodesCollectionList).Size = new Size(160, 21);
		((Control)cbEscapeCodesCollectionList).TabIndex = 1;
		((ComboBox)cbEscapeCodesCollectionList).SelectedIndexChanged += cbZoomX_SelectedIndexChanged;
		((PanelEx)groupBoxEncoding).CanvasColor = SystemColors.Control;
		((PanelEx)groupBoxEncoding).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)groupBoxEncoding).Controls.Add((Control)(object)cbEncoding);
		((Control)groupBoxEncoding).Dock = (DockStyle)1;
		groupBoxEncoding.ExpandOnTitleClick = true;
		((Control)groupBoxEncoding).Location = new Point(4, 376);
		((Control)groupBoxEncoding).Name = "groupBoxEncoding";
		((Control)groupBoxEncoding).Size = new Size(176, 65);
		((PanelEx)groupBoxEncoding).Style.Alignment = (StringAlignment)1;
		((PanelEx)groupBoxEncoding).Style.Border = (eBorderType)1;
		((PanelEx)groupBoxEncoding).Style.ForeColor.ColorSchemePart = (eColorSchemePart)40;
		((PanelEx)groupBoxEncoding).Style.GradientAngle = 90;
		((Control)groupBoxEncoding).TabIndex = 3;
		((PanelEx)groupBoxEncoding).ThemeAware = true;
		groupBoxEncoding.TitleStyle.Alignment = (StringAlignment)1;
		groupBoxEncoding.TitleStyle.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		groupBoxEncoding.TitleStyle.Border = (eBorderType)7;
		groupBoxEncoding.TitleStyle.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		groupBoxEncoding.TitleStyle.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		groupBoxEncoding.TitleStyle.GradientAngle = 90;
		groupBoxEncoding.TitleText = "Encoding";
		groupBoxEncoding.ExpandedChanged += new ExpandChangeEventHandler(groupBoxSettings_ExpandedChanged);
		((Control)cbEncoding).Anchor = (AnchorStyles)13;
		((ComboBox)cbEncoding).DrawMode = (DrawMode)1;
		((ComboBox)cbEncoding).DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbEncoding).FlatStyle = (FlatStyle)0;
		((Control)cbEncoding).ForeColor = SystemColors.ControlText;
		((Control)cbEncoding).Location = new Point(8, 33);
		((Control)cbEncoding).Name = "cbEncoding";
		((Control)cbEncoding).Size = new Size(160, 21);
		((Control)cbEncoding).TabIndex = 0;
		((ComboBox)cbEncoding).SelectedIndexChanged += cbZoomX_SelectedIndexChanged;
		((PanelEx)groupBoxZoom).CanvasColor = SystemColors.Control;
		((PanelEx)groupBoxZoom).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)groupBoxZoom).Controls.Add((Control)(object)lbY);
		((Control)groupBoxZoom).Controls.Add((Control)(object)cbZoomX);
		((Control)groupBoxZoom).Controls.Add((Control)(object)cbZoomY);
		((Control)groupBoxZoom).Controls.Add((Control)(object)lbX);
		((Control)groupBoxZoom).Dock = (DockStyle)1;
		groupBoxZoom.ExpandOnTitleClick = true;
		((Control)groupBoxZoom).Location = new Point(4, 281);
		((Control)groupBoxZoom).Name = "groupBoxZoom";
		((Control)groupBoxZoom).Size = new Size(176, 87);
		((PanelEx)groupBoxZoom).Style.Alignment = (StringAlignment)1;
		((PanelEx)groupBoxZoom).Style.Border = (eBorderType)1;
		((PanelEx)groupBoxZoom).Style.ForeColor.ColorSchemePart = (eColorSchemePart)40;
		((PanelEx)groupBoxZoom).Style.GradientAngle = 90;
		((Control)groupBoxZoom).TabIndex = 2;
		((PanelEx)groupBoxZoom).ThemeAware = true;
		groupBoxZoom.TitleStyle.Alignment = (StringAlignment)1;
		groupBoxZoom.TitleStyle.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		groupBoxZoom.TitleStyle.Border = (eBorderType)7;
		groupBoxZoom.TitleStyle.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		groupBoxZoom.TitleStyle.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		groupBoxZoom.TitleStyle.GradientAngle = 90;
		groupBoxZoom.TitleText = "Zoom";
		groupBoxZoom.ExpandedChanged += new ExpandChangeEventHandler(groupBoxSettings_ExpandedChanged);
		((Control)lbY).BackColor = Color.Transparent;
		((Control)lbY).ForeColor = SystemColors.ControlText;
		((Control)lbY).Location = new Point(5, 55);
		((Control)lbY).Name = "lbY";
		((Control)lbY).Size = new Size(32, 20);
		((Control)lbY).TabIndex = 3;
		((Control)lbY).Text = "Y";
		lbY.TextAlign = (ContentAlignment)64;
		((Control)cbZoomX).Anchor = (AnchorStyles)13;
		((ComboBox)cbZoomX).DrawMode = (DrawMode)1;
		((ComboBox)cbZoomX).FlatStyle = (FlatStyle)0;
		cbZoomX.Items.AddRange(new object[7] { "25%", "50%", "75%", "100%", "125%", "150%", "200%" });
		((Control)cbZoomX).Location = new Point(45, 31);
		((Control)cbZoomX).Name = "cbZoomX";
		((Control)cbZoomX).Size = new Size(64, 21);
		((Control)cbZoomX).TabIndex = 0;
		((Control)cbZoomX).Text = "125%";
		((ComboBox)cbZoomX).SelectedIndexChanged += cbZoomX_SelectedIndexChanged;
		((Control)cbZoomY).Anchor = (AnchorStyles)13;
		((ComboBox)cbZoomY).DrawMode = (DrawMode)1;
		((ComboBox)cbZoomY).FlatStyle = (FlatStyle)0;
		((Control)cbZoomY).ForeColor = SystemColors.ControlText;
		cbZoomY.Items.AddRange(new object[7] { "25%", "50%", "75%", "100%", "125%", "150%", "200%" });
		((Control)cbZoomY).Location = new Point(45, 55);
		((Control)cbZoomY).Name = "cbZoomY";
		((Control)cbZoomY).Size = new Size(64, 21);
		((Control)cbZoomY).TabIndex = 1;
		((Control)cbZoomY).Text = "125%";
		((ComboBox)cbZoomY).SelectedIndexChanged += cbZoomX_SelectedIndexChanged;
		((Control)lbX).BackColor = Color.Transparent;
		((Control)lbX).ForeColor = SystemColors.ControlText;
		((Control)lbX).Location = new Point(5, 31);
		((Control)lbX).Name = "lbX";
		((Control)lbX).Size = new Size(32, 20);
		((Control)lbX).TabIndex = 2;
		((Control)lbX).Text = "X";
		lbX.TextAlign = (ContentAlignment)64;
		((PanelEx)groupBoxBorderType).CanvasColor = SystemColors.Control;
		((PanelEx)groupBoxBorderType).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)groupBoxBorderType).Controls.Add((Control)(object)rbSimple);
		((Control)groupBoxBorderType).Controls.Add((Control)(object)rbUnicodeSingle);
		((Control)groupBoxBorderType).Controls.Add((Control)(object)rbUnicodeDouble);
		((Control)groupBoxBorderType).Dock = (DockStyle)1;
		groupBoxBorderType.ExpandOnTitleClick = true;
		((Control)groupBoxBorderType).Location = new Point(4, 168);
		((Control)groupBoxBorderType).Name = "groupBoxBorderType";
		((Control)groupBoxBorderType).Size = new Size(176, 105);
		((PanelEx)groupBoxBorderType).Style.Alignment = (StringAlignment)1;
		((PanelEx)groupBoxBorderType).Style.Border = (eBorderType)1;
		((PanelEx)groupBoxBorderType).Style.ForeColor.ColorSchemePart = (eColorSchemePart)40;
		((PanelEx)groupBoxBorderType).Style.GradientAngle = 90;
		((Control)groupBoxBorderType).TabIndex = 1;
		((PanelEx)groupBoxBorderType).ThemeAware = true;
		groupBoxBorderType.TitleStyle.Alignment = (StringAlignment)1;
		groupBoxBorderType.TitleStyle.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		groupBoxBorderType.TitleStyle.Border = (eBorderType)7;
		groupBoxBorderType.TitleStyle.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		groupBoxBorderType.TitleStyle.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		groupBoxBorderType.TitleStyle.GradientAngle = 90;
		groupBoxBorderType.TitleText = "Border type";
		groupBoxBorderType.ExpandedChanged += new ExpandChangeEventHandler(groupBoxSettings_ExpandedChanged);
		((Control)rbSimple).Anchor = (AnchorStyles)13;
		((Control)rbSimple).BackColor = Color.Transparent;
		((Control)rbSimple).ForeColor = SystemColors.ControlText;
		((Control)rbSimple).Location = new Point(8, 28);
		((Control)rbSimple).Name = "rbSimple";
		((Control)rbSimple).Size = new Size(160, 24);
		((Control)rbSimple).TabIndex = 0;
		((Control)rbSimple).Text = "Simple";
		((ButtonBase)rbSimple).UseVisualStyleBackColor = false;
		rbSimple.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((Control)rbUnicodeSingle).Anchor = (AnchorStyles)13;
		((Control)rbUnicodeSingle).BackColor = Color.Transparent;
		rbUnicodeSingle.Checked = true;
		rbUnicodeSingle.CheckState = (CheckState)1;
		((Control)rbUnicodeSingle).ForeColor = SystemColors.ControlText;
		((Control)rbUnicodeSingle).Location = new Point(8, 52);
		((Control)rbUnicodeSingle).Name = "rbUnicodeSingle";
		((Control)rbUnicodeSingle).Size = new Size(160, 24);
		((Control)rbUnicodeSingle).TabIndex = 1;
		((Control)rbUnicodeSingle).Text = "Single";
		((ButtonBase)rbUnicodeSingle).UseVisualStyleBackColor = false;
		rbUnicodeSingle.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((Control)rbUnicodeDouble).Anchor = (AnchorStyles)13;
		((Control)rbUnicodeDouble).BackColor = Color.Transparent;
		((Control)rbUnicodeDouble).ForeColor = SystemColors.ControlText;
		((Control)rbUnicodeDouble).Location = new Point(8, 76);
		((Control)rbUnicodeDouble).Name = "rbUnicodeDouble";
		((Control)rbUnicodeDouble).Size = new Size(160, 24);
		((Control)rbUnicodeDouble).TabIndex = 2;
		((Control)rbUnicodeDouble).Text = "Double";
		((ButtonBase)rbUnicodeDouble).UseVisualStyleBackColor = false;
		rbUnicodeDouble.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((PanelEx)groupBoxSettings).CanvasColor = SystemColors.Control;
		((PanelEx)groupBoxSettings).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)groupBoxSettings).Controls.Add((Control)(object)checkBoxKillSpaceGraphLines);
		((Control)groupBoxSettings).Controls.Add((Control)(object)cbKillSpaceLines);
		((Control)groupBoxSettings).Controls.Add((Control)(object)checkBoxCutLongLines);
		((Control)groupBoxSettings).Controls.Add((Control)(object)cbPutFeedPageCode);
		((Control)groupBoxSettings).Controls.Add((Control)(object)cbDrawBorder);
		((Control)groupBoxSettings).Dock = (DockStyle)1;
		groupBoxSettings.ExpandOnTitleClick = true;
		((Control)groupBoxSettings).Location = new Point(4, 4);
		((Control)groupBoxSettings).Name = "groupBoxSettings";
		((Control)groupBoxSettings).Size = new Size(176, 156);
		((PanelEx)groupBoxSettings).Style.Alignment = (StringAlignment)1;
		((PanelEx)groupBoxSettings).Style.BackColor1.Color = Color.White;
		((PanelEx)groupBoxSettings).Style.BackColor2.Color = Color.White;
		((PanelEx)groupBoxSettings).Style.Border = (eBorderType)1;
		((PanelEx)groupBoxSettings).Style.ForeColor.ColorSchemePart = (eColorSchemePart)40;
		((PanelEx)groupBoxSettings).Style.GradientAngle = 90;
		((Control)groupBoxSettings).TabIndex = 0;
		((PanelEx)groupBoxSettings).ThemeAware = true;
		groupBoxSettings.TitleStyle.Alignment = (StringAlignment)1;
		groupBoxSettings.TitleStyle.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		groupBoxSettings.TitleStyle.Border = (eBorderType)7;
		groupBoxSettings.TitleStyle.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		groupBoxSettings.TitleStyle.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		groupBoxSettings.TitleStyle.GradientAngle = 90;
		groupBoxSettings.TitleText = "Settings";
		groupBoxSettings.ExpandedChanged += new ExpandChangeEventHandler(groupBoxSettings_ExpandedChanged);
		((Control)checkBoxKillSpaceGraphLines).Anchor = (AnchorStyles)13;
		((Control)checkBoxKillSpaceGraphLines).BackColor = Color.Transparent;
		((Control)checkBoxKillSpaceGraphLines).ForeColor = SystemColors.ControlText;
		((Control)checkBoxKillSpaceGraphLines).Location = new Point(8, 56);
		((Control)checkBoxKillSpaceGraphLines).Name = "checkBoxKillSpaceGraphLines";
		((Control)checkBoxKillSpaceGraphLines).Size = new Size(160, 24);
		((Control)checkBoxKillSpaceGraphLines).TabIndex = 1;
		((Control)checkBoxKillSpaceGraphLines).Text = "Kill space graph lines";
		((ButtonBase)checkBoxKillSpaceGraphLines).UseVisualStyleBackColor = false;
		checkBoxKillSpaceGraphLines.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((Control)cbKillSpaceLines).Anchor = (AnchorStyles)13;
		((Control)cbKillSpaceLines).BackColor = Color.Transparent;
		cbKillSpaceLines.Checked = true;
		cbKillSpaceLines.CheckState = (CheckState)1;
		((Control)cbKillSpaceLines).ForeColor = SystemColors.ControlText;
		((Control)cbKillSpaceLines).Location = new Point(8, 32);
		((Control)cbKillSpaceLines).Name = "cbKillSpaceLines";
		((Control)cbKillSpaceLines).Size = new Size(160, 24);
		((Control)cbKillSpaceLines).TabIndex = 0;
		((Control)cbKillSpaceLines).Text = "Kill space lines";
		((ButtonBase)cbKillSpaceLines).UseVisualStyleBackColor = false;
		cbKillSpaceLines.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((Control)checkBoxCutLongLines).Anchor = (AnchorStyles)13;
		((Control)checkBoxCutLongLines).BackColor = Color.Transparent;
		((Control)checkBoxCutLongLines).ForeColor = SystemColors.ControlText;
		((Control)checkBoxCutLongLines).Location = new Point(8, 128);
		((Control)checkBoxCutLongLines).Name = "checkBoxCutLongLines";
		((Control)checkBoxCutLongLines).Size = new Size(160, 24);
		((Control)checkBoxCutLongLines).TabIndex = 4;
		((Control)checkBoxCutLongLines).Text = "Cut Long Lines";
		((ButtonBase)checkBoxCutLongLines).UseVisualStyleBackColor = false;
		checkBoxCutLongLines.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((Control)cbPutFeedPageCode).Anchor = (AnchorStyles)13;
		((Control)cbPutFeedPageCode).BackColor = Color.Transparent;
		cbPutFeedPageCode.Checked = true;
		cbPutFeedPageCode.CheckState = (CheckState)1;
		((Control)cbPutFeedPageCode).ForeColor = SystemColors.ControlText;
		((Control)cbPutFeedPageCode).Location = new Point(8, 80);
		((Control)cbPutFeedPageCode).Name = "cbPutFeedPageCode";
		((Control)cbPutFeedPageCode).Size = new Size(160, 24);
		((Control)cbPutFeedPageCode).TabIndex = 2;
		((Control)cbPutFeedPageCode).Text = "Put feed page code";
		((ButtonBase)cbPutFeedPageCode).UseVisualStyleBackColor = false;
		cbPutFeedPageCode.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((Control)cbDrawBorder).Anchor = (AnchorStyles)13;
		((Control)cbDrawBorder).BackColor = Color.Transparent;
		cbDrawBorder.Checked = true;
		cbDrawBorder.CheckState = (CheckState)1;
		((Control)cbDrawBorder).ForeColor = SystemColors.ControlText;
		((Control)cbDrawBorder).Location = new Point(8, 104);
		((Control)cbDrawBorder).Name = "cbDrawBorder";
		((Control)cbDrawBorder).Size = new Size(160, 24);
		((Control)cbDrawBorder).TabIndex = 3;
		((Control)cbDrawBorder).Text = "Draw border";
		((ButtonBase)cbDrawBorder).UseVisualStyleBackColor = false;
		cbDrawBorder.CheckedChanged += checkBoxAutoRefresh_CheckedChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Control)this).Controls.Add((Control)(object)groupBoxRefresh);
		((Control)this).Controls.Add((Control)(object)panel4);
		((Control)this).Controls.Add((Control)(object)groupBoxEscapeCodes);
		((Control)this).Controls.Add((Control)(object)panel5);
		((Control)this).Controls.Add((Control)(object)groupBoxEncoding);
		((Control)this).Controls.Add((Control)(object)panel3);
		((Control)this).Controls.Add((Control)(object)groupBoxZoom);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)groupBoxBorderType);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)groupBoxSettings);
		((Control)this).Name = "StiDotMatrixSettingsControl";
		((Control)this).Padding = new Padding(4);
		((Control)this).Size = new Size(184, 672);
		((Control)groupBoxRefresh).ResumeLayout(false);
		((Control)groupBoxEscapeCodes).ResumeLayout(false);
		((Control)groupBoxEncoding).ResumeLayout(false);
		((Control)groupBoxZoom).ResumeLayout(false);
		((Control)groupBoxBorderType).ResumeLayout(false);
		((Control)groupBoxSettings).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private void groupBoxSettings_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
	{
		if (view != null)
		{
			view.SaveConfig();
		}
	}

	private void checkBoxAutoRefresh_CheckedChanged(object sender, EventArgs e)
	{
		if (view != null)
		{
			view.SaveConfig();
		}
	}

	private void cbZoomX_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (view != null)
		{
			view.SaveConfig();
		}
	}
}
