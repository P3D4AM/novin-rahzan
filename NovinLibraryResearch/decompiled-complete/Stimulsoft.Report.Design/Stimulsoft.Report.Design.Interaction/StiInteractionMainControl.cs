using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Interaction;

[ToolboxItem(false)]
public class StiInteractionMainControl : UserControl
{
	private StiInteraction interaction;

	private StiBandInteraction bandInteraction;

	private StiCrossHeaderInteraction crossHeaderInteraction;

	private StiComponent comp;

	private IContainer components;

	private StiComboBox comboBoxHyperlinkType;

	private StiOffice2013GroupBox groupBoxHyperlink;

	private StiButtonEdit textBoxHyperlink;

	private Label labelHyperlink;

	private Label labelToolTip;

	private Label labelTag;

	private StiButtonEdit textBoxToolTip;

	private StiButtonEdit textBoxTag;

	private Label labelBookmark;

	private StiButtonEdit textBoxBookmark;

	private StiOffice2013GroupBox groupBoxBand;

	private CheckBox checkBoxSelectionEnabled;

	private CheckBox checkBoxCollapsingEnabled;

	private CheckBox checkBoxCollapseGroupFooter;

	private StiButtonEdit textBoxCollapsingExpression;

	private StiOffice2013GroupBox groupBoxCrossHeader;

	private CheckBox checkBoxCrossHeaderCollapsingEnabled;

	private StiGroupLine stiGroupLine1;

	private Label labelCollapsingExpression;

	private Label labelSample;

	private Label labelSample2;

	private Label labelSample4;

	private Label labelSample3;

	private Label labelSample5;

	private Panel panel1;

	public StiButtonEdit BookmarkTextBox
	{
		get
		{
			return textBoxBookmark;
		}
		set
		{
			textBoxBookmark = value;
		}
	}

	public StiButtonEdit ToolTipTextBox
	{
		get
		{
			return textBoxToolTip;
		}
		set
		{
			textBoxToolTip = value;
		}
	}

	public StiButtonEdit TagTextBox
	{
		get
		{
			return textBoxTag;
		}
		set
		{
			textBoxTag = value;
		}
	}

	public StiButtonEdit HyperlinkTextBox
	{
		get
		{
			return textBoxHyperlink;
		}
		set
		{
			textBoxHyperlink = value;
		}
	}

	public StiButtonEdit CollapsingExpressionTextBox
	{
		get
		{
			return textBoxCollapsingExpression;
		}
		set
		{
			textBoxCollapsingExpression = value;
		}
	}

	public CheckBox CollapsingEnabledCheckBox
	{
		get
		{
			return checkBoxCollapsingEnabled;
		}
		set
		{
			checkBoxCollapsingEnabled = value;
		}
	}

	public CheckBox CrossHeaderCollapsingEnabledCheckBox
	{
		get
		{
			return checkBoxCrossHeaderCollapsingEnabled;
		}
		set
		{
			checkBoxCrossHeaderCollapsingEnabled = value;
		}
	}

	public CheckBox SelectionEnabledCheckBox
	{
		get
		{
			return checkBoxSelectionEnabled;
		}
		set
		{
			checkBoxSelectionEnabled = value;
		}
	}

	public StiInteractionMainControl(StiComponent comp, StiInteraction interaction)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.comp = comp;
			this.interaction = interaction;
			bandInteraction = this.interaction as StiBandInteraction;
			crossHeaderInteraction = this.interaction as StiCrossHeaderInteraction;
			Localize();
			DoScaling();
			ReadInteraction();
			((Control)textBoxBookmark).Focus();
			Label[] array = (Label[])(object)new Label[4] { labelSample2, labelSample3, labelSample4, labelSample5 };
			foreach (Label val in array)
			{
				((Control)val).MouseDown += new MouseEventHandler(LabelSample_MouseDown);
			}
		}
	}

	public void Localize()
	{
		((Control)groupBoxBand).Text = StiLocalization.Get("Report", "Bands");
		((Control)groupBoxCrossHeader).Text = StiLocalization.Get("Components", "StiCrossHeaderBand");
		((Control)labelBookmark).Text = Loc.GetCleaned("PropertyMain", "Bookmark");
		((Control)labelHyperlink).Text = Loc.GetCleaned("PropertyMain", "Hyperlink");
		((Control)labelToolTip).Text = Loc.GetCleaned("PropertyMain", "ToolTip");
		((Control)labelTag).Text = Loc.GetCleaned("PropertyMain", "Tag");
		((Control)labelCollapsingExpression).Text = Loc.GetMain("Expression");
		((Control)checkBoxCollapsingEnabled).Text = StiLocalization.Get("PropertyMain", "CollapsingEnabled");
		((Control)checkBoxCollapseGroupFooter).Text = StiLocalization.Get("PropertyMain", "CollapseGroupFooter");
		((ComboBox)comboBoxHyperlinkType).BeginUpdate();
		int num = ((ListControl)comboBoxHyperlinkType).SelectedIndex;
		if (num == -1)
		{
			num = 2;
		}
		((ComboBox)comboBoxHyperlinkType).Items.Clear();
		((ComboBox)comboBoxHyperlinkType).Items.Add((object)StiLocalization.Get("FormInteraction", "HyperlinkUsingInteractionBookmark"));
		((ComboBox)comboBoxHyperlinkType).Items.Add((object)StiLocalization.Get("FormInteraction", "HyperlinkUsingInteractionTag"));
		((ComboBox)comboBoxHyperlinkType).Items.Add((object)StiLocalization.Get("FormInteraction", "HyperlinkExternalDocuments"));
		((ListControl)comboBoxHyperlinkType).SelectedIndex = num;
		((ComboBox)comboBoxHyperlinkType).EndUpdate();
		((Control)checkBoxCrossHeaderCollapsingEnabled).Text = StiLocalization.Get("PropertyMain", "CollapsingEnabled");
		((Control)checkBoxSelectionEnabled).Text = StiLocalization.Get("PropertyMain", "SelectionEnabled");
		((Control)labelSample).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
		((Control)labelSample).Width = TextRenderer.MeasureText(((Control)labelSample).Text, ((Control)labelSample).Font).Width;
		((Control)labelSample2).Left = ((Control)labelSample).Right + StiScaleUI.I4;
	}

	private void ReadInteraction()
	{
		((Control)textBoxBookmark).Text = interaction.Bookmark.Value;
		((Control)textBoxToolTip).Text = interaction.ToolTip.Value;
		((Control)textBoxTag).Text = interaction.Tag.Value;
		((Control)textBoxHyperlink).Text = interaction.Bookmark.Value;
		ReadHyperlink();
		ReadBandInteraction();
		ReadCrossHeaderInteraction();
		RefreshCollapsingExpression();
		((Control)groupBoxBand).Visible = bandInteraction != null;
		((Control)groupBoxCrossHeader).Visible = crossHeaderInteraction != null;
	}

	private void ReadCrossHeaderInteraction()
	{
		if (crossHeaderInteraction != null)
		{
			checkBoxCrossHeaderCollapsingEnabled.Checked = crossHeaderInteraction.CollapsingEnabled;
		}
	}

	private void ReadBandInteraction()
	{
		if (bandInteraction != null)
		{
			checkBoxSelectionEnabled.Checked = bandInteraction.SelectionEnabled;
			checkBoxCollapsingEnabled.Checked = bandInteraction.CollapsingEnabled;
			checkBoxCollapseGroupFooter.Checked = bandInteraction.CollapseGroupFooter;
			((Control)textBoxCollapsingExpression).Text = bandInteraction.Collapsed.Value;
		}
	}

	private void ReadHyperlink()
	{
		string value = interaction.Hyperlink.Value;
		if (value.StartsWith("##"))
		{
			((ListControl)comboBoxHyperlinkType).SelectedIndex = 1;
			((Control)textBoxHyperlink).Text = value.Remove(0, 2);
		}
		else if (value.StartsWith("#"))
		{
			((ListControl)comboBoxHyperlinkType).SelectedIndex = 0;
			((Control)textBoxHyperlink).Text = value.Remove(0, 1);
		}
		else
		{
			((ListControl)comboBoxHyperlinkType).SelectedIndex = 2;
			((Control)textBoxHyperlink).Text = value;
		}
	}

	private void DoScaling()
	{
		textBoxBookmark.Image = StiReportDesignImages.Actions.EditSmall();
		textBoxToolTip.Image = StiReportDesignImages.Actions.EditSmall();
		textBoxHyperlink.Image = StiReportDesignImages.Actions.EditSmall();
		textBoxTag.Image = StiReportDesignImages.Actions.EditSmall();
		textBoxCollapsingExpression.Image = StiReportDesignImages.Actions.EditSmall();
	}

	private void RunExpressionEditor(StiButtonEdit textBox)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		string text = ((Control)textBox).Text;
		DialogResult val = StiTextEditor.EditText(comp.Report, ref text, StiCodeEditorMode.EditorExpression | StiCodeEditorMode.IgnoreAskOnClosing, Loc.Get("FormTitles", "ExpressionEditorForm"));
		if ((int)val == 1)
		{
			((Control)textBox).Text = text;
		}
		((Control)textBox).Focus();
		textBox.Select(0, 0);
	}

	public void WriteInteraction()
	{
		interaction.Bookmark.Value = ((Control)textBoxBookmark).Text;
		interaction.ToolTip.Value = ((Control)textBoxToolTip).Text;
		interaction.Tag.Value = ((Control)textBoxTag).Text;
		WriteHyperlink();
		WriteBandInteraction();
		WriteCrossHeaderInteraction();
	}

	private void WriteCrossHeaderInteraction()
	{
		if (crossHeaderInteraction != null)
		{
			crossHeaderInteraction.CollapsingEnabled = checkBoxCrossHeaderCollapsingEnabled.Checked;
		}
	}

	private void WriteBandInteraction()
	{
		if (bandInteraction != null)
		{
			bandInteraction.SelectionEnabled = checkBoxSelectionEnabled.Checked;
			bandInteraction.CollapsingEnabled = checkBoxCollapsingEnabled.Checked;
			bandInteraction.CollapseGroupFooter = checkBoxCollapseGroupFooter.Checked;
			bandInteraction.Collapsed.Value = ((Control)textBoxCollapsingExpression).Text;
		}
	}

	private void WriteHyperlink()
	{
		switch (((ListControl)comboBoxHyperlinkType).SelectedIndex)
		{
		case 0:
			interaction.Hyperlink.Value = "#" + ((Control)textBoxHyperlink).Text;
			break;
		case 1:
			interaction.Hyperlink.Value = "##" + ((Control)textBoxHyperlink).Text;
			break;
		case 2:
			interaction.Hyperlink.Value = ((Control)textBoxHyperlink).Text;
			break;
		default:
			interaction.Hyperlink.Value = ((Control)textBoxHyperlink).Text;
			break;
		}
	}

	private void RefreshCollapsingExpression()
	{
		StiButtonEdit stiButtonEdit = textBoxCollapsingExpression;
		CheckBox obj = checkBoxCollapseGroupFooter;
		Label obj2 = labelSample2;
		Label obj3 = labelSample3;
		Label obj4 = labelSample4;
		bool flag = (((Control)labelSample5).Enabled = checkBoxCollapsingEnabled.Checked);
		bool flag3 = (((Control)obj4).Enabled = flag);
		bool flag5 = (((Control)obj3).Enabled = flag3);
		bool flag7 = (((Control)obj2).Enabled = flag5);
		bool enabled = (((Control)obj).Enabled = flag7);
		((Control)stiButtonEdit).Enabled = enabled;
	}

	private void ButtonEditHyperlink_Click(object sender, EventArgs e)
	{
		RunExpressionEditor(textBoxHyperlink);
	}

	private void ButtonEditToolTip_Click(object sender, EventArgs e)
	{
		RunExpressionEditor(textBoxToolTip);
	}

	private void ButtonEditTag_Click(object sender, EventArgs e)
	{
		RunExpressionEditor(textBoxTag);
	}

	private void ButtonEditBookmark_Click(object sender, EventArgs e)
	{
		RunExpressionEditor(textBoxBookmark);
	}

	private void TextBoxCollapsingExpression_ButtonClick(object sender, EventArgs e)
	{
		RunExpressionEditor(textBoxCollapsingExpression);
	}

	private void CheckBoxCollapsingEnabled_CheckedChanged(object sender, EventArgs e)
	{
		RefreshCollapsingExpression();
	}

	private void LabelSample_MouseDown(object sender, MouseEventArgs e)
	{
		string text = ((Control)((sender is Label) ? sender : null)).Text.Trim();
		if (text.EndsWith(","))
		{
			text = text.Substring(0, text.Length - 1);
		}
		((Control)textBoxCollapsingExpression).Text = text;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
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
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
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
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Expected O, but got Unknown
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec7: Expected O, but got Unknown
		//IL_0ef7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8b: Expected O, but got Unknown
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1042: Unknown result type (might be due to invalid IL or missing references)
		//IL_104c: Expected O, but got Unknown
		//IL_107c: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ec: Unknown result type (might be due to invalid IL or missing references)
		comboBoxHyperlinkType = new StiComboBox();
		groupBoxHyperlink = new StiOffice2013GroupBox();
		textBoxHyperlink = new StiButtonEdit();
		textBoxTag = new StiButtonEdit();
		textBoxToolTip = new StiButtonEdit();
		textBoxBookmark = new StiButtonEdit();
		labelBookmark = new Label();
		labelToolTip = new Label();
		labelTag = new Label();
		labelHyperlink = new Label();
		groupBoxBand = new StiOffice2013GroupBox();
		stiGroupLine1 = new StiGroupLine();
		labelSample = new Label();
		labelSample2 = new Label();
		labelCollapsingExpression = new Label();
		checkBoxCollapsingEnabled = new CheckBox();
		checkBoxCollapseGroupFooter = new CheckBox();
		textBoxCollapsingExpression = new StiButtonEdit();
		checkBoxSelectionEnabled = new CheckBox();
		groupBoxCrossHeader = new StiOffice2013GroupBox();
		checkBoxCrossHeaderCollapsingEnabled = new CheckBox();
		labelSample5 = new Label();
		labelSample3 = new Label();
		labelSample4 = new Label();
		panel1 = new Panel();
		((Control)groupBoxHyperlink).SuspendLayout();
		((Control)groupBoxBand).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)groupBoxCrossHeader).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		comboBoxHyperlinkType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxHyperlinkType).FormattingEnabled = true;
		((Control)comboBoxHyperlinkType).Location = new Point(306, 277);
		((Control)comboBoxHyperlinkType).Margin = new Padding(6);
		((Control)comboBoxHyperlinkType).Name = "comboBoxHyperlinkType";
		((Control)comboBoxHyperlinkType).Size = new Size(472, 42);
		((Control)comboBoxHyperlinkType).TabIndex = 4;
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)textBoxHyperlink);
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)textBoxTag);
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)textBoxToolTip);
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)textBoxBookmark);
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)labelBookmark);
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)labelToolTip);
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)labelTag);
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)comboBoxHyperlinkType);
		((Control)groupBoxHyperlink).Controls.Add((Control)(object)labelHyperlink);
		((Control)groupBoxHyperlink).Dock = (DockStyle)1;
		groupBoxHyperlink.HeaderHeight = 0;
		((Control)groupBoxHyperlink).Location = new Point(0, 0);
		((Control)groupBoxHyperlink).Margin = new Padding(6);
		((Control)groupBoxHyperlink).Name = "groupBoxHyperlink";
		((Control)groupBoxHyperlink).Size = new Size(1000, 342);
		((Control)groupBoxHyperlink).TabIndex = 0;
		((Control)groupBoxHyperlink).TabStop = false;
		((Control)textBoxHyperlink).AllowDrop = true;
		((Control)textBoxHyperlink).Anchor = (AnchorStyles)13;
		((Control)textBoxHyperlink).BackColor = SystemColors.Window;
		((Control)textBoxHyperlink).Location = new Point(306, 213);
		((Control)textBoxHyperlink).Margin = new Padding(4);
		((Control)textBoxHyperlink).Name = "textBoxHyperlink";
		((Control)textBoxHyperlink).Size = new Size(672, 42);
		((Control)textBoxHyperlink).TabIndex = 3;
		textBoxHyperlink.ButtonClick += ButtonEditHyperlink_Click;
		((Control)textBoxTag).AllowDrop = true;
		((Control)textBoxTag).Anchor = (AnchorStyles)13;
		((Control)textBoxTag).BackColor = SystemColors.Window;
		((Control)textBoxTag).Location = new Point(306, 150);
		((Control)textBoxTag).Margin = new Padding(4);
		((Control)textBoxTag).Name = "textBoxTag";
		((Control)textBoxTag).Size = new Size(672, 42);
		((Control)textBoxTag).TabIndex = 2;
		textBoxTag.ButtonClick += ButtonEditTag_Click;
		((Control)textBoxToolTip).AllowDrop = true;
		((Control)textBoxToolTip).Anchor = (AnchorStyles)13;
		((Control)textBoxToolTip).BackColor = SystemColors.Window;
		((Control)textBoxToolTip).Location = new Point(306, 87);
		((Control)textBoxToolTip).Margin = new Padding(4);
		((Control)textBoxToolTip).Name = "textBoxToolTip";
		((Control)textBoxToolTip).Size = new Size(672, 42);
		((Control)textBoxToolTip).TabIndex = 1;
		textBoxToolTip.ButtonClick += ButtonEditToolTip_Click;
		((Control)textBoxBookmark).AllowDrop = true;
		((Control)textBoxBookmark).Anchor = (AnchorStyles)13;
		((Control)textBoxBookmark).BackColor = SystemColors.Window;
		((Control)textBoxBookmark).Location = new Point(306, 23);
		((Control)textBoxBookmark).Margin = new Padding(4);
		((Control)textBoxBookmark).Name = "textBoxBookmark";
		((Control)textBoxBookmark).Size = new Size(672, 42);
		((Control)textBoxBookmark).TabIndex = 0;
		textBoxBookmark.ButtonClick += ButtonEditBookmark_Click;
		((Control)labelBookmark).ForeColor = Color.DimGray;
		((Control)labelBookmark).Location = new Point(24, 23);
		((Control)labelBookmark).Margin = new Padding(6, 0, 6, 0);
		((Control)labelBookmark).Name = "labelBookmark";
		((Control)labelBookmark).Size = new Size(272, 40);
		((Control)labelBookmark).TabIndex = 5;
		((Control)labelBookmark).Text = "Bookmark";
		labelBookmark.TextAlign = (ContentAlignment)16;
		((Control)labelToolTip).ForeColor = Color.DimGray;
		((Control)labelToolTip).Location = new Point(24, 87);
		((Control)labelToolTip).Margin = new Padding(6, 0, 6, 0);
		((Control)labelToolTip).Name = "labelToolTip";
		((Control)labelToolTip).Size = new Size(272, 40);
		((Control)labelToolTip).TabIndex = 6;
		((Control)labelToolTip).Text = "ToolTip";
		labelToolTip.TextAlign = (ContentAlignment)16;
		((Control)labelTag).ForeColor = Color.DimGray;
		((Control)labelTag).Location = new Point(24, 150);
		((Control)labelTag).Margin = new Padding(6, 0, 6, 0);
		((Control)labelTag).Name = "labelTag";
		((Control)labelTag).Size = new Size(272, 40);
		((Control)labelTag).TabIndex = 7;
		((Control)labelTag).Text = "Tag";
		labelTag.TextAlign = (ContentAlignment)16;
		((Control)labelHyperlink).ForeColor = Color.DimGray;
		((Control)labelHyperlink).Location = new Point(24, 213);
		((Control)labelHyperlink).Margin = new Padding(6, 0, 6, 0);
		((Control)labelHyperlink).Name = "labelHyperlink";
		((Control)labelHyperlink).Size = new Size(272, 40);
		((Control)labelHyperlink).TabIndex = 8;
		((Control)labelHyperlink).Text = "Hyperlink";
		labelHyperlink.TextAlign = (ContentAlignment)16;
		((Control)groupBoxBand).Controls.Add((Control)(object)stiGroupLine1);
		((Control)groupBoxBand).Controls.Add((Control)(object)checkBoxSelectionEnabled);
		((Control)groupBoxBand).Dock = (DockStyle)1;
		groupBoxBand.HeaderHeight = 24;
		((Control)groupBoxBand).Location = new Point(0, 342);
		((Control)groupBoxBand).Margin = new Padding(6);
		((Control)groupBoxBand).Name = "groupBoxBand";
		((Control)groupBoxBand).Size = new Size(1000, 350);
		((Control)groupBoxBand).TabIndex = 1;
		((Control)groupBoxBand).TabStop = false;
		((Control)groupBoxBand).Text = "Band";
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)panel1);
		((Control)stiGroupLine1).Controls.Add((Control)(object)labelCollapsingExpression);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxCollapsingEnabled);
		((Control)stiGroupLine1).Controls.Add((Control)(object)checkBoxCollapseGroupFooter);
		((Control)stiGroupLine1).Controls.Add((Control)(object)textBoxCollapsingExpression);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 127);
		((Control)stiGroupLine1).Margin = new Padding(6);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(1000, 223);
		((Control)stiGroupLine1).TabIndex = 1;
		((Control)labelSample).Dock = (DockStyle)3;
		((Control)labelSample).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelSample).ForeColor = Color.DimGray;
		((Control)labelSample).Location = new Point(0, 0);
		((Control)labelSample).Margin = new Padding(6, 0, 6, 0);
		((Control)labelSample).Name = "labelSample";
		((Control)labelSample).Size = new Size(90, 25);
		((Control)labelSample).TabIndex = 19;
		((Control)labelSample).Text = "Sample:";
		labelSample.TextAlign = (ContentAlignment)16;
		labelSample2.AutoEllipsis = true;
		((Control)labelSample2).Dock = (DockStyle)3;
		((Control)labelSample2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelSample2).ForeColor = Color.DimGray;
		((Control)labelSample2).Location = new Point(328, 0);
		((Control)labelSample2).Margin = new Padding(6, 0, 6, 0);
		((Control)labelSample2).Name = "labelSample2";
		((Control)labelSample2).Size = new Size(95, 25);
		((Control)labelSample2).TabIndex = 18;
		((Control)labelSample2).Text = "{Variable},";
		labelSample2.TextAlign = (ContentAlignment)16;
		((Control)labelCollapsingExpression).ForeColor = Color.DimGray;
		((Control)labelCollapsingExpression).Location = new Point(24, 137);
		((Control)labelCollapsingExpression).Margin = new Padding(6, 0, 6, 0);
		((Control)labelCollapsingExpression).Name = "labelCollapsingExpression";
		((Control)labelCollapsingExpression).Size = new Size(272, 40);
		((Control)labelCollapsingExpression).TabIndex = 3;
		((Control)labelCollapsingExpression).Text = "Expression";
		labelCollapsingExpression.TextAlign = (ContentAlignment)16;
		((Control)checkBoxCollapsingEnabled).Location = new Point(306, 15);
		((Control)checkBoxCollapsingEnabled).Margin = new Padding(6);
		((Control)checkBoxCollapsingEnabled).Name = "checkBoxCollapsingEnabled";
		((Control)checkBoxCollapsingEnabled).Size = new Size(672, 46);
		((Control)checkBoxCollapsingEnabled).TabIndex = 0;
		((Control)checkBoxCollapsingEnabled).Text = "Collapsing Enabled";
		((ButtonBase)checkBoxCollapsingEnabled).UseVisualStyleBackColor = true;
		checkBoxCollapsingEnabled.CheckedChanged += CheckBoxCollapsingEnabled_CheckedChanged;
		((Control)checkBoxCollapseGroupFooter).Location = new Point(306, 75);
		((Control)checkBoxCollapseGroupFooter).Margin = new Padding(6);
		((Control)checkBoxCollapseGroupFooter).Name = "checkBoxCollapseGroupFooter";
		((Control)checkBoxCollapseGroupFooter).Size = new Size(672, 46);
		((Control)checkBoxCollapseGroupFooter).TabIndex = 1;
		((Control)checkBoxCollapseGroupFooter).Text = "Collapse Group Footer";
		((ButtonBase)checkBoxCollapseGroupFooter).UseVisualStyleBackColor = true;
		((Control)textBoxCollapsingExpression).AllowDrop = true;
		((Control)textBoxCollapsingExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxCollapsingExpression).BackColor = SystemColors.Window;
		((Control)textBoxCollapsingExpression).Location = new Point(306, 137);
		((Control)textBoxCollapsingExpression).Margin = new Padding(4);
		((Control)textBoxCollapsingExpression).Name = "textBoxCollapsingExpression";
		((Control)textBoxCollapsingExpression).Size = new Size(672, 42);
		((Control)textBoxCollapsingExpression).TabIndex = 2;
		textBoxCollapsingExpression.ButtonClick += TextBoxCollapsingExpression_ButtonClick;
		((Control)checkBoxSelectionEnabled).Location = new Point(306, 67);
		((Control)checkBoxSelectionEnabled).Margin = new Padding(6);
		((Control)checkBoxSelectionEnabled).Name = "checkBoxSelectionEnabled";
		((Control)checkBoxSelectionEnabled).Size = new Size(672, 46);
		((Control)checkBoxSelectionEnabled).TabIndex = 0;
		((Control)checkBoxSelectionEnabled).Text = "Selection Enabled";
		((ButtonBase)checkBoxSelectionEnabled).UseVisualStyleBackColor = true;
		((Control)groupBoxCrossHeader).Controls.Add((Control)(object)checkBoxCrossHeaderCollapsingEnabled);
		((Control)groupBoxCrossHeader).Dock = (DockStyle)1;
		groupBoxCrossHeader.HeaderHeight = 24;
		((Control)groupBoxCrossHeader).Location = new Point(0, 692);
		((Control)groupBoxCrossHeader).Margin = new Padding(6);
		((Control)groupBoxCrossHeader).Name = "groupBoxCrossHeader";
		((Control)groupBoxCrossHeader).Size = new Size(1000, 127);
		((Control)groupBoxCrossHeader).TabIndex = 2;
		((Control)groupBoxCrossHeader).TabStop = false;
		((Control)groupBoxCrossHeader).Text = "Cross-Header";
		((Control)checkBoxCrossHeaderCollapsingEnabled).Location = new Point(306, 67);
		((Control)checkBoxCrossHeaderCollapsingEnabled).Margin = new Padding(6);
		((Control)checkBoxCrossHeaderCollapsingEnabled).Name = "checkBoxCrossHeaderCollapsingEnabled";
		((Control)checkBoxCrossHeaderCollapsingEnabled).Size = new Size(672, 46);
		((Control)checkBoxCrossHeaderCollapsingEnabled).TabIndex = 0;
		((Control)checkBoxCrossHeaderCollapsingEnabled).Text = "Collapsing Enabled";
		((ButtonBase)checkBoxCrossHeaderCollapsingEnabled).UseVisualStyleBackColor = true;
		labelSample5.AutoEllipsis = true;
		((Control)labelSample5).Dock = (DockStyle)3;
		((Control)labelSample5).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelSample5).ForeColor = Color.DimGray;
		((Control)labelSample5).Location = new Point(423, 0);
		((Control)labelSample5).Margin = new Padding(6, 0, 6, 0);
		((Control)labelSample5).Name = "labelSample5";
		((Control)labelSample5).Size = new Size(139, 25);
		((Control)labelSample5).TabIndex = 20;
		((Control)labelSample5).Text = "{GroupLine!=1}";
		labelSample5.TextAlign = (ContentAlignment)16;
		labelSample3.AutoEllipsis = true;
		((Control)labelSample3).Dock = (DockStyle)3;
		((Control)labelSample3).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelSample3).ForeColor = Color.DimGray;
		((Control)labelSample3).Location = new Point(90, 0);
		((Control)labelSample3).Margin = new Padding(6, 0, 6, 0);
		((Control)labelSample3).Name = "labelSample3";
		((Control)labelSample3).Size = new Size(173, 25);
		((Control)labelSample3).TabIndex = 21;
		((Control)labelSample3).Text = "{DataSource.Field},";
		labelSample3.TextAlign = (ContentAlignment)16;
		labelSample4.AutoEllipsis = true;
		((Control)labelSample4).Dock = (DockStyle)3;
		((Control)labelSample4).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelSample4).ForeColor = Color.DimGray;
		((Control)labelSample4).Location = new Point(263, 0);
		((Control)labelSample4).Margin = new Padding(6, 0, 6, 0);
		((Control)labelSample4).Name = "labelSample4";
		((Control)labelSample4).Size = new Size(65, 25);
		((Control)labelSample4).TabIndex = 22;
		((Control)labelSample4).Text = "{True},";
		labelSample4.TextAlign = (ContentAlignment)16;
		((Control)panel1).Controls.Add((Control)(object)labelSample5);
		((Control)panel1).Controls.Add((Control)(object)labelSample2);
		((Control)panel1).Controls.Add((Control)(object)labelSample4);
		((Control)panel1).Controls.Add((Control)(object)labelSample3);
		((Control)panel1).Controls.Add((Control)(object)labelSample);
		((Control)panel1).Location = new Point(306, 181);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(672, 25);
		((Control)panel1).TabIndex = 23;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)groupBoxCrossHeader);
		((Control)this).Controls.Add((Control)(object)groupBoxBand);
		((Control)this).Controls.Add((Control)(object)groupBoxHyperlink);
		((Control)this).Margin = new Padding(6);
		((Control)this).Name = "StiInteractionMainControl";
		((Control)this).Size = new Size(1000, 837);
		((Control)groupBoxHyperlink).ResumeLayout(false);
		((Control)groupBoxBand).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)groupBoxCrossHeader).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
