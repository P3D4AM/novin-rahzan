using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class StiColorScaleConditionZoneControl : StiZoneControl
{
	internal bool blockUpdateSample;

	private IContainer components;

	public StiColorBoxXF colorBoxMinimum;

	public StiTreeViewBox cbColumn;

	private Label labelColumn;

	private Panel panelSample;

	private Label labelSample;

	private Label labelScaleType;

	public StiComboBox comboBoxScaleType;

	public StiColorBoxXF colorBoxMaximum;

	public StiComboBox comboBoxMinimum;

	private Label labelMinimum;

	public StiComboBox comboBoxMaximum;

	private Label labelMaximum;

	private Label labelType;

	public StiTextBox textBoxMinimum;

	public StiTextBox textBoxMaximum;

	private Label labelMid;

	public StiColorBoxXF colorBoxMid;

	public StiTextBox textBoxMid;

	public StiComboBox comboBoxMid;

	private Label labelColor;

	private Label labelValue;

	public StiColorScaleConditionZoneControl()
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormConditions", "title");
		((Control)labelColumn).Text = Loc.GetCleaned("PropertyMain", "Column");
		((Control)labelSample).Text = Loc.GetCleaned("FormFormatEditor", "Sample");
		((Control)labelType).Text = Loc.GetCleaned("PropertyMain", "Type");
		((Control)labelValue).Text = Loc.GetCleaned("PropertyMain", "Value");
		((Control)labelColor).Text = Loc.GetCleaned("PropertyMain", "Color");
		((Control)labelScaleType).Text = Loc.GetCleaned("PropertyMain", "ColorScaleType");
		((Control)labelMinimum).Text = Loc.GetCleaned("PropertyMain", "Minimum");
		((Control)labelMid).Text = Loc.GetCleaned("PropertyMain", "Mid");
		((Control)labelMaximum).Text = Loc.GetCleaned("PropertyMain", "Maximum");
	}

	internal void UpdateSample()
	{
		((Control)panelSample).Invalidate();
	}

	private void DoScaling()
	{
		((Control)comboBoxMaximum).Width = ((Control)textBoxMaximum).Width;
		((Control)comboBoxMinimum).Width = ((Control)textBoxMinimum).Width;
		((Control)comboBoxScaleType).Width = ((Control)cbColumn).Width;
	}

	private void PanelSample_Paint(object sender, PaintEventArgs e)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)panelSample).Width - 1, ((Control)panelSample).Height - 1);
		Rectangle rectangle2 = rectangle;
		rectangle2.Inflate(-StiScale.I2, -StiScale.I2);
		rectangle2.Width++;
		rectangle2.Height++;
		if (((ListControl)comboBoxScaleType).SelectedIndex == 0)
		{
			LinearGradientBrush val = new LinearGradientBrush(rectangle2, colorBoxMinimum.SelectedColor, colorBoxMaximum.SelectedColor, 0f);
			try
			{
				graphics.FillRectangle((Brush)(object)val, rectangle2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			Color[] colors = new Color[3] { colorBoxMinimum.SelectedColor, colorBoxMid.SelectedColor, colorBoxMaximum.SelectedColor };
			float[] positions = new float[3] { 0f, 0.5f, 1f };
			ColorBlend interpolationColors = new ColorBlend
			{
				Colors = colors,
				Positions = positions
			};
			LinearGradientBrush val2 = new LinearGradientBrush(rectangle2, colorBoxMinimum.SelectedColor, colorBoxMaximum.SelectedColor, 0f);
			try
			{
				val2.InterpolationColors = interpolationColors;
				graphics.FillRectangle((Brush)(object)val2, rectangle2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		graphics.DrawRectangle(Pens.Gray, rectangle);
	}

	private void ColorBoxColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ColorBoxNegativeColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ColorBoxMid_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ComboBoxMinimal_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)textBoxMinimum).Enabled = ((ListControl)comboBoxMinimum).SelectedIndex != 0;
	}

	private void ComboBoxMaximal_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)textBoxMaximum).Enabled = ((ListControl)comboBoxMaximum).SelectedIndex != 0;
	}

	private void ComboBoxMid_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)textBoxMid).Enabled = ((ListControl)comboBoxMid).SelectedIndex != 0;
	}

	private void ComboBoxScaleType_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)labelMid).Visible = ((ListControl)comboBoxScaleType).SelectedIndex == 1;
		((Control)comboBoxMid).Visible = ((ListControl)comboBoxScaleType).SelectedIndex == 1;
		((Control)textBoxMid).Visible = ((ListControl)comboBoxScaleType).SelectedIndex == 1;
		((Control)colorBoxMid).Visible = ((ListControl)comboBoxScaleType).SelectedIndex == 1;
		UpdateSample();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Expected O, but got Unknown
		colorBoxMinimum = new StiColorBoxXF();
		cbColumn = new StiTreeViewBox();
		labelColumn = new Label();
		panelSample = new Panel();
		labelSample = new Label();
		labelScaleType = new Label();
		comboBoxScaleType = new StiComboBox();
		colorBoxMaximum = new StiColorBoxXF();
		comboBoxMinimum = new StiComboBox();
		labelMinimum = new Label();
		comboBoxMaximum = new StiComboBox();
		labelMaximum = new Label();
		labelType = new Label();
		textBoxMinimum = new StiTextBox();
		textBoxMaximum = new StiTextBox();
		labelValue = new Label();
		labelMid = new Label();
		colorBoxMid = new StiColorBoxXF();
		textBoxMid = new StiTextBox();
		comboBoxMid = new StiComboBox();
		labelColor = new Label();
		((Control)this).SuspendLayout();
		((Control)colorBoxMinimum).BackColor = SystemColors.Window;
		((Control)colorBoxMinimum).Location = new Point(178, 163);
		((Control)colorBoxMinimum).Name = "colorBoxMinimum";
		colorBoxMinimum.SelectedColor = Color.Transparent;
		((Control)colorBoxMinimum).Size = new Size(131, 21);
		((Control)colorBoxMinimum).TabIndex = 8;
		colorBoxMinimum.SelectedColorChanged += ColorBoxColor_SelectedIndexChanged;
		((Control)cbColumn).BackColor = SystemColors.Window;
		cbColumn.DropDownWidth = 400;
		cbColumn.Flat = false;
		((Control)cbColumn).ForeColor = SystemColors.ControlText;
		((Control)cbColumn).Location = new Point(178, 12);
		cbColumn.MaxDropDownHeight = 300;
		((Control)cbColumn).Name = "cbColumn";
		((Control)cbColumn).Size = new Size(232, 21);
		((Control)cbColumn).TabIndex = 0;
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
		((Control)panelSample).BackColor = Color.White;
		((Control)panelSample).Location = new Point(178, 196);
		((Control)panelSample).Name = "panelSample";
		((Control)panelSample).Padding = new Padding(1);
		((Control)panelSample).Size = new Size(415, 25);
		((Control)panelSample).TabIndex = 11;
		((Control)panelSample).Paint += new PaintEventHandler(PanelSample_Paint);
		((Control)labelSample).BackColor = Color.Transparent;
		((Control)labelSample).ForeColor = Color.DimGray;
		((Control)labelSample).Location = new Point(12, 196);
		((Control)labelSample).Name = "labelSample";
		((Control)labelSample).Size = new Size(160, 21);
		((Control)labelSample).TabIndex = 20;
		((Control)labelSample).Text = "Sample";
		labelSample.TextAlign = (ContentAlignment)16;
		labelSample.UseCompatibleTextRendering = true;
		((Control)labelSample).Click += LabelColumn_Click;
		((Control)labelScaleType).BackColor = Color.Transparent;
		((Control)labelScaleType).ForeColor = Color.DimGray;
		((Control)labelScaleType).Location = new Point(12, 45);
		((Control)labelScaleType).Name = "labelScaleType";
		((Control)labelScaleType).Size = new Size(160, 21);
		((Control)labelScaleType).TabIndex = 22;
		((Control)labelScaleType).Text = "Scale Style";
		labelScaleType.TextAlign = (ContentAlignment)16;
		labelScaleType.UseCompatibleTextRendering = true;
		((Control)labelScaleType).Click += LabelColumn_Click;
		comboBoxScaleType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxScaleType).FormattingEnabled = true;
		((ComboBox)comboBoxScaleType).Items.AddRange(new object[2] { "No Borders", "Solid Borders" });
		((Control)comboBoxScaleType).Location = new Point(178, 45);
		((Control)comboBoxScaleType).Name = "comboBoxScaleType";
		((Control)comboBoxScaleType).Size = new Size(232, 21);
		((Control)comboBoxScaleType).TabIndex = 1;
		((ComboBox)comboBoxScaleType).SelectedIndexChanged += ComboBoxScaleType_SelectedIndexChanged;
		((Control)colorBoxMaximum).BackColor = SystemColors.Window;
		((Control)colorBoxMaximum).Location = new Point(463, 163);
		((Control)colorBoxMaximum).Name = "colorBoxMaximum";
		colorBoxMaximum.SelectedColor = Color.Transparent;
		((Control)colorBoxMaximum).Size = new Size(130, 21);
		((Control)colorBoxMaximum).TabIndex = 10;
		colorBoxMaximum.SelectedColorChanged += ColorBoxNegativeColor_SelectedIndexChanged;
		comboBoxMinimum.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxMinimum).FormattingEnabled = true;
		((ComboBox)comboBoxMinimum).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxMinimum).Location = new Point(178, 98);
		((Control)comboBoxMinimum).Name = "comboBoxMinimum";
		((Control)comboBoxMinimum).Size = new Size(131, 21);
		((Control)comboBoxMinimum).TabIndex = 2;
		((ComboBox)comboBoxMinimum).SelectedIndexChanged += ComboBoxMinimal_SelectedIndexChanged;
		((Control)labelMinimum).AutoSize = true;
		((Control)labelMinimum).BackColor = Color.Transparent;
		((Control)labelMinimum).ForeColor = Color.DimGray;
		((Control)labelMinimum).Location = new Point(177, 82);
		((Control)labelMinimum).Name = "labelMinimum";
		((Control)labelMinimum).Size = new Size(42, 13);
		((Control)labelMinimum).TabIndex = 29;
		((Control)labelMinimum).Text = "Minimal";
		((Control)labelMinimum).Click += LabelColumn_Click;
		comboBoxMaximum.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxMaximum).FormattingEnabled = true;
		((ComboBox)comboBoxMaximum).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxMaximum).Location = new Point(463, 98);
		((Control)comboBoxMaximum).Name = "comboBoxMaximum";
		((Control)comboBoxMaximum).Size = new Size(130, 21);
		((Control)comboBoxMaximum).TabIndex = 4;
		((ComboBox)comboBoxMaximum).SelectedIndexChanged += ComboBoxMaximal_SelectedIndexChanged;
		((Control)labelMaximum).AutoSize = true;
		((Control)labelMaximum).BackColor = Color.Transparent;
		((Control)labelMaximum).ForeColor = Color.DimGray;
		((Control)labelMaximum).Location = new Point(463, 82);
		((Control)labelMaximum).Name = "labelMaximum";
		((Control)labelMaximum).Size = new Size(45, 13);
		((Control)labelMaximum).TabIndex = 31;
		((Control)labelMaximum).Text = "Maximal";
		((Control)labelMaximum).Click += LabelColumn_Click;
		((Control)labelType).BackColor = Color.Transparent;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 98);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(160, 21);
		((Control)labelType).TabIndex = 32;
		((Control)labelType).Text = "Type";
		labelType.TextAlign = (ContentAlignment)16;
		labelType.UseCompatibleTextRendering = true;
		((Control)labelType).Click += LabelColumn_Click;
		((Control)textBoxMinimum).BackColor = Color.White;
		((Control)textBoxMinimum).Location = new Point(178, 131);
		((Control)textBoxMinimum).Name = "textBoxMinimum";
		((Control)textBoxMinimum).Size = new Size(131, 21);
		((Control)textBoxMinimum).TabIndex = 5;
		textBoxMinimum.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxMaximum).BackColor = Color.White;
		((Control)textBoxMaximum).Location = new Point(463, 131);
		((Control)textBoxMaximum).Name = "textBoxMaximum";
		((Control)textBoxMaximum).Size = new Size(130, 21);
		((Control)textBoxMaximum).TabIndex = 7;
		textBoxMaximum.TextAlign = (HorizontalAlignment)1;
		((Control)labelValue).BackColor = Color.Transparent;
		((Control)labelValue).ForeColor = Color.DimGray;
		((Control)labelValue).Location = new Point(12, 131);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(160, 20);
		((Control)labelValue).TabIndex = 34;
		((Control)labelValue).Text = "Value";
		labelValue.TextAlign = (ContentAlignment)16;
		labelValue.UseCompatibleTextRendering = true;
		((Control)labelValue).Click += LabelColumn_Click;
		((Control)labelMid).AutoSize = true;
		((Control)labelMid).BackColor = Color.Transparent;
		((Control)labelMid).ForeColor = Color.DimGray;
		((Control)labelMid).Location = new Point(321, 82);
		((Control)labelMid).Name = "labelMid";
		((Control)labelMid).Size = new Size(24, 13);
		((Control)labelMid).TabIndex = 38;
		((Control)labelMid).Text = "Mid";
		((Control)labelMid).Click += LabelColumn_Click;
		((Control)colorBoxMid).BackColor = SystemColors.Window;
		((Control)colorBoxMid).Location = new Point(321, 163);
		((Control)colorBoxMid).Name = "colorBoxMid";
		colorBoxMid.SelectedColor = Color.Transparent;
		((Control)colorBoxMid).Size = new Size(130, 21);
		((Control)colorBoxMid).TabIndex = 9;
		colorBoxMid.SelectedColorChanged += ColorBoxMid_SelectedIndexChanged;
		((Control)textBoxMid).BackColor = Color.White;
		((Control)textBoxMid).Location = new Point(321, 131);
		((Control)textBoxMid).Name = "textBoxMid";
		((Control)textBoxMid).Size = new Size(130, 21);
		((Control)textBoxMid).TabIndex = 6;
		textBoxMid.TextAlign = (HorizontalAlignment)1;
		comboBoxMid.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxMid).FormattingEnabled = true;
		((ComboBox)comboBoxMid).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxMid).Location = new Point(321, 98);
		((Control)comboBoxMid).Name = "comboBoxMid";
		((Control)comboBoxMid).Size = new Size(130, 21);
		((Control)comboBoxMid).TabIndex = 3;
		((ComboBox)comboBoxMid).SelectedIndexChanged += ComboBoxMid_SelectedIndexChanged;
		((Control)labelColor).BackColor = Color.Transparent;
		((Control)labelColor).ForeColor = Color.DimGray;
		((Control)labelColor).Location = new Point(12, 163);
		((Control)labelColor).Name = "labelColor";
		((Control)labelColor).Size = new Size(160, 20);
		((Control)labelColor).TabIndex = 39;
		((Control)labelColor).Text = "Color";
		labelColor.TextAlign = (ContentAlignment)16;
		labelColor.UseCompatibleTextRendering = true;
		((Control)labelColor).Click += LabelColumn_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)labelColor);
		((Control)this).Controls.Add((Control)(object)labelMid);
		((Control)this).Controls.Add((Control)(object)colorBoxMid);
		((Control)this).Controls.Add((Control)(object)textBoxMid);
		((Control)this).Controls.Add((Control)(object)comboBoxMid);
		((Control)this).Controls.Add((Control)(object)cbColumn);
		((Control)this).Controls.Add((Control)(object)labelSample);
		((Control)this).Controls.Add((Control)(object)panelSample);
		((Control)this).Controls.Add((Control)(object)comboBoxMinimum);
		((Control)this).Controls.Add((Control)(object)colorBoxMinimum);
		((Control)this).Controls.Add((Control)(object)labelMaximum);
		((Control)this).Controls.Add((Control)(object)labelColumn);
		((Control)this).Controls.Add((Control)(object)labelScaleType);
		((Control)this).Controls.Add((Control)(object)labelType);
		((Control)this).Controls.Add((Control)(object)colorBoxMaximum);
		((Control)this).Controls.Add((Control)(object)textBoxMinimum);
		((Control)this).Controls.Add((Control)(object)labelMinimum);
		((Control)this).Controls.Add((Control)(object)comboBoxScaleType);
		((Control)this).Controls.Add((Control)(object)textBoxMaximum);
		((Control)this).Controls.Add((Control)(object)labelValue);
		((Control)this).Controls.Add((Control)(object)comboBoxMaximum);
		((Control)this).Name = "StiColorScaleConditionZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(608, 236);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
