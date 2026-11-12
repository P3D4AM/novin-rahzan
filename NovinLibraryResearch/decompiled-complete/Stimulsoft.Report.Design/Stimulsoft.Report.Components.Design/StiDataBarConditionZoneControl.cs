using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class StiDataBarConditionZoneControl : StiZoneControl
{
	private StiText component;

	internal bool blockUpdateSample;

	private IContainer components;

	public StiColorBoxXF colorBoxPositiveColor;

	public StiColorBoxXF colorBoxPositiveBorderColor;

	public StiTreeViewBox cbColumn;

	private Label labelColumn;

	public StiComboBox comboBoxBrush;

	private Label labelBorders;

	public StiComboBox comboBoxBorders;

	private Panel panelSample;

	private Label labelSample;

	private Label labelDirection;

	public StiComboBox comboBoxDirection;

	private Label labelPositive;

	private Label labelNegative;

	public StiColorBoxXF colorBoxNegativeColor;

	public StiColorBoxXF colorBoxNegativeBorderColor;

	private StiGroupLine stiGroupLine1;

	public StiComboBox comboBoxMinimum;

	private Label labelMinimum;

	public StiComboBox comboBoxMaximum;

	private Label labelMaximum;

	private Label labelType;

	public StiTextBox textBoxMinimum;

	public StiTextBox textBoxMaximum;

	private Label labelValue;

	private Label labelBrush;

	public StiDataBarConditionZoneControl(StiText component)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.component = component;
			Localize();
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormConditions", "title");
		((Control)labelColumn).Text = Loc.Get("PropertyMain", "Column");
		((Control)labelPositive).Text = Loc.Get("PropertyMain", "Positive");
		((Control)labelNegative).Text = Loc.Get("PropertyMain", "Negative");
		((Control)labelBrush).Text = Loc.Get("PropertyMain", "BrushType");
		((Control)labelBorders).Text = Loc.Get("PropertyMain", "Borders");
		((Control)labelSample).Text = Loc.Get("FormFormatEditor", "Sample");
		((Control)labelDirection).Text = Loc.Get("PropertyMain", "Direction");
		((Control)labelType).Text = Loc.Get("PropertyMain", "Type");
		((Control)labelValue).Text = Loc.Get("PropertyMain", "Value");
		((Control)labelMinimum).Text = Loc.Get("PropertyMain", "Minimum");
		((Control)labelMaximum).Text = Loc.Get("PropertyMain", "Maximum");
	}

	internal void UpdateSample()
	{
		((Control)panelSample).Invalidate();
	}

	private void DoScaling()
	{
		((Control)comboBoxMaximum).Width = ((Control)textBoxMaximum).Width;
		((Control)comboBoxMinimum).Width = ((Control)textBoxMinimum).Width;
		StiComboBox stiComboBox = comboBoxBrush;
		int width2 = (((Control)comboBoxBorders).Width = ((Control)textBoxMinimum).Width);
		((Control)stiComboBox).Width = width2;
	}

	private void ComboBoxBorders_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)colorBoxPositiveBorderColor).Enabled = ((ListControl)comboBoxBorders).SelectedIndex == 1;
		((Control)colorBoxNegativeBorderColor).Enabled = ((ListControl)comboBoxBorders).SelectedIndex == 1;
		UpdateSample();
	}

	private void PanelSample_Paint(object sender, PaintEventArgs e)
	{
		Rectangle rectangle = new Rectangle(0, 0, ((Control)panelSample).Width - 1, ((Control)panelSample).Height - 1);
		e.Graphics.FillRectangle(Brushes.White, rectangle);
		e.Graphics.DrawRectangle(Pens.LightGray, rectangle);
		StiDataBarIndicator indicator = component.Indicator as StiDataBarIndicator;
		try
		{
			StiBrushType brushType = ((((ListControl)comboBoxBrush).SelectedIndex != 0) ? StiBrushType.Gradient : StiBrushType.Solid);
			bool flag = ((ListControl)comboBoxBorders).SelectedIndex != 0;
			StiDataBarDirection direction = StiDataBarDirection.Default;
			if (((ListControl)comboBoxDirection).SelectedIndex == 1)
			{
				direction = StiDataBarDirection.LeftToRight;
			}
			if (((ListControl)comboBoxDirection).SelectedIndex == 2)
			{
				direction = StiDataBarDirection.RighToLeft;
			}
			StiDataBarIndicator indicator2 = new StiDataBarIndicator
			{
				PositiveColor = colorBoxPositiveColor.SelectedColor,
				NegativeColor = colorBoxNegativeColor.SelectedColor,
				BrushType = brushType,
				ShowBorder = flag,
				PositiveBorderColor = colorBoxPositiveBorderColor.SelectedColor,
				NegativeBorderColor = colorBoxNegativeBorderColor.SelectedColor,
				Minimum = 0f,
				Value = 75f,
				Maximum = 100f,
				Direction = direction
			};
			component.Indicator = indicator2;
			StiDataBarGdiIndicatorTypePainter stiDataBarGdiIndicatorTypePainter = new StiDataBarGdiIndicatorTypePainter();
			if (flag)
			{
				stiDataBarGdiIndicatorTypePainter.Paint(e.Graphics, component, new RectangleD(0.0, 0.0, ((Control)panelSample).Width - 1, ((Control)panelSample).Height - 1));
			}
			else
			{
				stiDataBarGdiIndicatorTypePainter.Paint(e.Graphics, component, new RectangleD(0.0, 0.0, ((Control)panelSample).Width, ((Control)panelSample).Height));
			}
		}
		finally
		{
			component.Indicator = indicator;
		}
	}

	private void ColorBoxColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ColorBoxBorderColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ComboBoxBrush_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ComboBoxDirection_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ColorBoxNegativeColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ColorBoxNegativeBorderColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ComboBoxMinimal_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)textBoxMinimum).Enabled = ((ListControl)comboBoxMinimum).SelectedIndex == 1 || ((ListControl)comboBoxMinimum).SelectedIndex == 2;
	}

	private void ComboBoxMaximal_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)textBoxMaximum).Enabled = ((ListControl)comboBoxMaximum).SelectedIndex == 1 || ((ListControl)comboBoxMaximum).SelectedIndex == 2;
	}

	private void LabelColumn_Click(object sender, EventArgs e)
	{
		((Control)this).Focus();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
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
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Expected O, but got Unknown
		comboBoxBrush = new StiComboBox();
		cbColumn = new StiTreeViewBox();
		labelSample = new Label();
		stiGroupLine1 = new StiGroupLine();
		panelSample = new Panel();
		comboBoxMinimum = new StiComboBox();
		colorBoxPositiveColor = new StiColorBoxXF();
		colorBoxNegativeBorderColor = new StiColorBoxXF();
		labelMaximum = new Label();
		labelPositive = new Label();
		labelColumn = new Label();
		labelNegative = new Label();
		labelDirection = new Label();
		labelBrush = new Label();
		labelType = new Label();
		colorBoxNegativeColor = new StiColorBoxXF();
		textBoxMinimum = new StiTextBox();
		comboBoxBorders = new StiComboBox();
		labelMinimum = new Label();
		colorBoxPositiveBorderColor = new StiColorBoxXF();
		comboBoxDirection = new StiComboBox();
		labelBorders = new Label();
		textBoxMaximum = new StiTextBox();
		labelValue = new Label();
		comboBoxMaximum = new StiComboBox();
		((Control)this).SuspendLayout();
		comboBoxBrush.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxBrush).FormattingEnabled = true;
		((ComboBox)comboBoxBrush).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxBrush).Location = new Point(178, 192);
		((Control)comboBoxBrush).Name = "comboBoxBrush";
		((Control)comboBoxBrush).Size = new Size(147, 21);
		((Control)comboBoxBrush).TabIndex = 6;
		((ComboBox)comboBoxBrush).SelectedIndexChanged += ComboBoxBrush_SelectedIndexChanged;
		((Control)cbColumn).BackColor = SystemColors.Window;
		cbColumn.DropDownWidth = 400;
		cbColumn.Flat = false;
		((Control)cbColumn).ForeColor = SystemColors.ControlText;
		((Control)cbColumn).Location = new Point(178, 12);
		cbColumn.MaxDropDownHeight = 300;
		((Control)cbColumn).Name = "cbColumn";
		((Control)cbColumn).Size = new Size(232, 21);
		((Control)cbColumn).TabIndex = 0;
		((Control)labelSample).BackColor = Color.Transparent;
		((Control)labelSample).ForeColor = Color.DimGray;
		((Control)labelSample).Location = new Point(12, 258);
		((Control)labelSample).Name = "labelSample";
		((Control)labelSample).Size = new Size(160, 21);
		((Control)labelSample).TabIndex = 20;
		((Control)labelSample).Text = "Sample";
		labelSample.TextAlign = (ContentAlignment)16;
		labelSample.UseCompatibleTextRendering = true;
		((Control)labelSample).Click += LabelColumn_Click;
		((Control)stiGroupLine1).Anchor = (AnchorStyles)13;
		((Control)stiGroupLine1).BackColor = Color.Transparent;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(10, 161);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(602, 10);
		((Control)stiGroupLine1).TabIndex = 27;
		((Control)panelSample).BackColor = Color.White;
		((Control)panelSample).Location = new Point(178, 258);
		((Control)panelSample).Name = "panelSample";
		((Control)panelSample).Padding = new Padding(1);
		((Control)panelSample).Size = new Size(147, 21);
		((Control)panelSample).TabIndex = 12;
		((Control)panelSample).Paint += new PaintEventHandler(PanelSample_Paint);
		comboBoxMinimum.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxMinimum).FormattingEnabled = true;
		((ComboBox)comboBoxMinimum).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxMinimum).Location = new Point(178, 61);
		((Control)comboBoxMinimum).Name = "comboBoxMinimum";
		((Control)comboBoxMinimum).Size = new Size(147, 21);
		((Control)comboBoxMinimum).TabIndex = 1;
		((ComboBox)comboBoxMinimum).SelectedIndexChanged += ComboBoxMinimal_SelectedIndexChanged;
		((Control)colorBoxPositiveColor).BackColor = SystemColors.Window;
		((Control)colorBoxPositiveColor).Location = new Point(337, 192);
		((Control)colorBoxPositiveColor).Name = "colorBoxPositiveColor";
		colorBoxPositiveColor.SelectedColor = Color.Transparent;
		((Control)colorBoxPositiveColor).Size = new Size(131, 21);
		((Control)colorBoxPositiveColor).TabIndex = 7;
		colorBoxPositiveColor.SelectedColorChanged += ColorBoxColor_SelectedIndexChanged;
		((Control)colorBoxNegativeBorderColor).BackColor = SystemColors.Window;
		((Control)colorBoxNegativeBorderColor).Location = new Point(480, 225);
		((Control)colorBoxNegativeBorderColor).Name = "colorBoxNegativeBorderColor";
		colorBoxNegativeBorderColor.SelectedColor = Color.Transparent;
		((Control)colorBoxNegativeBorderColor).Size = new Size(131, 21);
		((Control)colorBoxNegativeBorderColor).TabIndex = 11;
		colorBoxNegativeBorderColor.SelectedColorChanged += ColorBoxNegativeBorderColor_SelectedIndexChanged;
		((Control)labelMaximum).AutoSize = true;
		((Control)labelMaximum).BackColor = Color.Transparent;
		((Control)labelMaximum).ForeColor = Color.DimGray;
		((Control)labelMaximum).Location = new Point(337, 45);
		((Control)labelMaximum).Name = "labelMaximum";
		((Control)labelMaximum).Size = new Size(45, 13);
		((Control)labelMaximum).TabIndex = 31;
		((Control)labelMaximum).Text = "Maximal";
		((Control)labelMaximum).Click += LabelColumn_Click;
		((Control)labelPositive).AutoSize = true;
		((Control)labelPositive).BackColor = Color.Transparent;
		((Control)labelPositive).ForeColor = Color.DimGray;
		((Control)labelPositive).Location = new Point(334, 176);
		((Control)labelPositive).Name = "labelPositive";
		((Control)labelPositive).Size = new Size(44, 13);
		((Control)labelPositive).TabIndex = 8;
		((Control)labelPositive).Text = "Positive";
		labelPositive.TextAlign = (ContentAlignment)16;
		((Control)labelPositive).Click += LabelColumn_Click;
		((Control)labelColumn).BackColor = Color.Transparent;
		((Control)labelColumn).ForeColor = Color.DimGray;
		((Control)labelColumn).Location = new Point(12, 12);
		((Control)labelColumn).Name = "labelColumn";
		((Control)labelColumn).Size = new Size(160, 21);
		((Control)labelColumn).TabIndex = 8;
		((Control)labelColumn).Text = "Column";
		labelColumn.TextAlign = (ContentAlignment)16;
		labelColumn.UseCompatibleTextRendering = true;
		((Control)labelColumn).Click += LabelColumn_Click;
		((Control)labelNegative).AutoSize = true;
		((Control)labelNegative).BackColor = Color.Transparent;
		((Control)labelNegative).ForeColor = Color.DimGray;
		((Control)labelNegative).Location = new Point(477, 176);
		((Control)labelNegative).Name = "labelNegative";
		((Control)labelNegative).Size = new Size(50, 13);
		((Control)labelNegative).TabIndex = 24;
		((Control)labelNegative).Text = "Negative";
		labelNegative.TextAlign = (ContentAlignment)16;
		((Control)labelNegative).Click += LabelColumn_Click;
		((Control)labelDirection).BackColor = Color.Transparent;
		((Control)labelDirection).ForeColor = Color.DimGray;
		((Control)labelDirection).Location = new Point(12, 126);
		((Control)labelDirection).Name = "labelDirection";
		((Control)labelDirection).Size = new Size(160, 21);
		((Control)labelDirection).TabIndex = 22;
		((Control)labelDirection).Text = "Direction";
		labelDirection.TextAlign = (ContentAlignment)16;
		labelDirection.UseCompatibleTextRendering = true;
		((Control)labelDirection).Click += LabelColumn_Click;
		((Control)labelBrush).BackColor = Color.Transparent;
		((Control)labelBrush).ForeColor = Color.DimGray;
		((Control)labelBrush).Location = new Point(12, 194);
		((Control)labelBrush).Name = "labelBrush";
		((Control)labelBrush).Size = new Size(160, 21);
		((Control)labelBrush).TabIndex = 15;
		((Control)labelBrush).Text = "Brush";
		labelBrush.TextAlign = (ContentAlignment)16;
		labelBrush.UseCompatibleTextRendering = true;
		((Control)labelBrush).Click += LabelColumn_Click;
		((Control)labelType).BackColor = Color.Transparent;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 61);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(160, 21);
		((Control)labelType).TabIndex = 32;
		((Control)labelType).Text = "Type";
		labelType.TextAlign = (ContentAlignment)16;
		labelType.UseCompatibleTextRendering = true;
		((Control)labelType).Click += LabelColumn_Click;
		((Control)colorBoxNegativeColor).BackColor = SystemColors.Window;
		((Control)colorBoxNegativeColor).Location = new Point(480, 192);
		((Control)colorBoxNegativeColor).Name = "colorBoxNegativeColor";
		colorBoxNegativeColor.SelectedColor = Color.Transparent;
		((Control)colorBoxNegativeColor).Size = new Size(131, 21);
		((Control)colorBoxNegativeColor).TabIndex = 8;
		colorBoxNegativeColor.SelectedColorChanged += ColorBoxNegativeColor_SelectedIndexChanged;
		((Control)textBoxMinimum).BackColor = Color.White;
		((Control)textBoxMinimum).Location = new Point(178, 94);
		((Control)textBoxMinimum).Name = "textBoxMinimum";
		((Control)textBoxMinimum).Size = new Size(147, 21);
		((Control)textBoxMinimum).TabIndex = 3;
		textBoxMinimum.TextAlign = (HorizontalAlignment)1;
		comboBoxBorders.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxBorders).FormattingEnabled = true;
		((ComboBox)comboBoxBorders).Items.AddRange(new object[2] { "No Borders", "Solid Borders" });
		((Control)comboBoxBorders).Location = new Point(178, 225);
		((Control)comboBoxBorders).Name = "comboBoxBorders";
		((Control)comboBoxBorders).Size = new Size(147, 21);
		((Control)comboBoxBorders).TabIndex = 9;
		((ComboBox)comboBoxBorders).SelectedIndexChanged += ComboBoxBorders_SelectedIndexChanged;
		((Control)labelMinimum).AutoSize = true;
		((Control)labelMinimum).BackColor = Color.Transparent;
		((Control)labelMinimum).ForeColor = Color.DimGray;
		((Control)labelMinimum).Location = new Point(177, 45);
		((Control)labelMinimum).Name = "labelMinimum";
		((Control)labelMinimum).Size = new Size(42, 13);
		((Control)labelMinimum).TabIndex = 29;
		((Control)labelMinimum).Text = "Minimal";
		((Control)labelMinimum).Click += LabelColumn_Click;
		((Control)colorBoxPositiveBorderColor).BackColor = SystemColors.Window;
		((Control)colorBoxPositiveBorderColor).Location = new Point(337, 225);
		((Control)colorBoxPositiveBorderColor).Name = "colorBoxPositiveBorderColor";
		colorBoxPositiveBorderColor.SelectedColor = Color.Transparent;
		((Control)colorBoxPositiveBorderColor).Size = new Size(131, 21);
		((Control)colorBoxPositiveBorderColor).TabIndex = 10;
		colorBoxPositiveBorderColor.SelectedColorChanged += ColorBoxBorderColor_SelectedIndexChanged;
		comboBoxDirection.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDirection).FormattingEnabled = true;
		((ComboBox)comboBoxDirection).Items.AddRange(new object[2] { "No Borders", "Solid Borders" });
		((Control)comboBoxDirection).Location = new Point(178, 126);
		((Control)comboBoxDirection).Name = "comboBoxDirection";
		((Control)comboBoxDirection).Size = new Size(105, 21);
		((Control)comboBoxDirection).TabIndex = 5;
		((ComboBox)comboBoxDirection).SelectedIndexChanged += ComboBoxDirection_SelectedIndexChanged;
		((Control)labelBorders).BackColor = Color.Transparent;
		((Control)labelBorders).ForeColor = Color.DimGray;
		((Control)labelBorders).Location = new Point(12, 225);
		((Control)labelBorders).Name = "labelBorders";
		((Control)labelBorders).Size = new Size(160, 21);
		((Control)labelBorders).TabIndex = 17;
		((Control)labelBorders).Text = "Borders";
		labelBorders.TextAlign = (ContentAlignment)16;
		labelBorders.UseCompatibleTextRendering = true;
		((Control)labelBorders).Click += LabelColumn_Click;
		((Control)textBoxMaximum).BackColor = Color.White;
		((Control)textBoxMaximum).Location = new Point(337, 94);
		((Control)textBoxMaximum).Name = "textBoxMaximum";
		((Control)textBoxMaximum).Size = new Size(147, 21);
		((Control)textBoxMaximum).TabIndex = 4;
		textBoxMaximum.TextAlign = (HorizontalAlignment)1;
		((Control)labelValue).BackColor = Color.Transparent;
		((Control)labelValue).ForeColor = Color.DimGray;
		((Control)labelValue).Location = new Point(12, 94);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(160, 20);
		((Control)labelValue).TabIndex = 34;
		((Control)labelValue).Text = "Value";
		labelValue.TextAlign = (ContentAlignment)16;
		labelValue.UseCompatibleTextRendering = true;
		((Control)labelValue).Click += LabelColumn_Click;
		comboBoxMaximum.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxMaximum).FormattingEnabled = true;
		((ComboBox)comboBoxMaximum).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxMaximum).Location = new Point(337, 61);
		((Control)comboBoxMaximum).Name = "comboBoxMaximum";
		((Control)comboBoxMaximum).Size = new Size(147, 21);
		((Control)comboBoxMaximum).TabIndex = 2;
		((ComboBox)comboBoxMaximum).SelectedIndexChanged += ComboBoxMaximal_SelectedIndexChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)comboBoxBrush);
		((Control)this).Controls.Add((Control)(object)cbColumn);
		((Control)this).Controls.Add((Control)(object)labelSample);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)panelSample);
		((Control)this).Controls.Add((Control)(object)comboBoxMinimum);
		((Control)this).Controls.Add((Control)(object)colorBoxPositiveColor);
		((Control)this).Controls.Add((Control)(object)colorBoxNegativeBorderColor);
		((Control)this).Controls.Add((Control)(object)labelMaximum);
		((Control)this).Controls.Add((Control)(object)labelPositive);
		((Control)this).Controls.Add((Control)(object)labelColumn);
		((Control)this).Controls.Add((Control)(object)labelNegative);
		((Control)this).Controls.Add((Control)(object)labelDirection);
		((Control)this).Controls.Add((Control)(object)labelBrush);
		((Control)this).Controls.Add((Control)(object)labelType);
		((Control)this).Controls.Add((Control)(object)colorBoxNegativeColor);
		((Control)this).Controls.Add((Control)(object)textBoxMinimum);
		((Control)this).Controls.Add((Control)(object)comboBoxBorders);
		((Control)this).Controls.Add((Control)(object)labelMinimum);
		((Control)this).Controls.Add((Control)(object)colorBoxPositiveBorderColor);
		((Control)this).Controls.Add((Control)(object)comboBoxDirection);
		((Control)this).Controls.Add((Control)(object)labelBorders);
		((Control)this).Controls.Add((Control)(object)textBoxMaximum);
		((Control)this).Controls.Add((Control)(object)labelValue);
		((Control)this).Controls.Add((Control)(object)comboBoxMaximum);
		((Control)this).Name = "StiDataBarConditionZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(622, 294);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
