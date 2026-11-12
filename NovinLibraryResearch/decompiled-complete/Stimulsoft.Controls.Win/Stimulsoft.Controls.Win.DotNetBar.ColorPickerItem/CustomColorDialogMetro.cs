using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.ColorPickers;

namespace Stimulsoft.Controls.Win.DotNetBar.ColorPickerItem;

[ToolboxItem(false)]
public class CustomColorDialogMetro : Form
{
	internal Button buttonOK;

	internal Button buttonCancel;

	internal Label labelNewColor;

	internal Label labelCurrentColor;

	private Color m_CurrentColor = Color.Black;

	private Color m_NewColor = Color.Empty;

	private ColorCombControl colorCombControl1;

	internal Label labelStandardColors;

	internal Label labelCustomColors;

	internal Label labelGreen;

	internal Label labelBlue;

	internal CustomColorBlender customColorBlender3;

	internal Label labelRed;

	private StiNumericUpDown numericBlue;

	private StiNumericUpDown numericGreen;

	private StiNumericUpDown numericRed;

	private ColorContrastControl colorContrastControl1;

	private ColorSelectionPreviewControl colorSelectionPreview;

	private TabControl tabControl1;

	internal TabPage tabPageStandard;

	internal TabPage tabPageCustom;

	private Panel panel1;

	internal Label labelAlpha;

	private StiNumericUpDown numericAlpha;

	internal Label labelHex;

	private StiTextBox textBoxHex;

	internal StiColorPipetControl colorPipet;

	private IContainer components;

	private bool m_SettingColor;

	public Color CurrentColor
	{
		get
		{
			return m_CurrentColor;
		}
		set
		{
			SetColorForBlenderContrastControl(value);
			colorSelectionPreview.CurrentColor = value;
		}
	}

	public Color NewColor => m_NewColor;

	public CustomColorDialogMetro()
	{
		InitializeComponent();
		Localization();
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
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9e: Expected O, but got Unknown
		labelStandardColors = new Label();
		labelBlue = new Label();
		labelGreen = new Label();
		labelCustomColors = new Label();
		buttonOK = new Button();
		buttonCancel = new Button();
		labelNewColor = new Label();
		labelCurrentColor = new Label();
		labelRed = new Label();
		tabControl1 = new TabControl();
		tabPageStandard = new TabPage();
		tabPageCustom = new TabPage();
		labelHex = new Label();
		labelAlpha = new Label();
		panel1 = new Panel();
		colorPipet = new StiColorPipetControl();
		colorCombControl1 = new ColorCombControl();
		textBoxHex = new StiTextBox();
		numericAlpha = new StiNumericUpDown();
		colorContrastControl1 = new ColorContrastControl();
		customColorBlender3 = new CustomColorBlender();
		numericGreen = new StiNumericUpDown();
		numericRed = new StiNumericUpDown();
		numericBlue = new StiNumericUpDown();
		colorSelectionPreview = new ColorSelectionPreviewControl();
		((Control)tabControl1).SuspendLayout();
		((Control)tabPageStandard).SuspendLayout();
		((Control)tabPageCustom).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)numericAlpha).BeginInit();
		((ISupportInitialize)numericGreen).BeginInit();
		((ISupportInitialize)numericRed).BeginInit();
		((ISupportInitialize)numericBlue).BeginInit();
		((Control)this).SuspendLayout();
		((Control)labelStandardColors).BackColor = Color.Transparent;
		((Control)labelStandardColors).ForeColor = Color.DimGray;
		((Control)labelStandardColors).Location = new Point(12, 12);
		((Control)labelStandardColors).Name = "labelStandardColors";
		((Control)labelStandardColors).Size = new Size(194, 19);
		((Control)labelStandardColors).TabIndex = 1;
		((Control)labelStandardColors).Text = " Colors";
		labelStandardColors.TextAlign = (ContentAlignment)16;
		((Control)labelBlue).BackColor = Color.Transparent;
		((Control)labelBlue).ForeColor = Color.DimGray;
		((Control)labelBlue).Location = new Point(12, 301);
		((Control)labelBlue).Name = "labelBlue";
		((Control)labelBlue).Size = new Size(90, 20);
		((Control)labelBlue).TabIndex = 7;
		((Control)labelBlue).Text = "&Blue";
		labelBlue.TextAlign = (ContentAlignment)16;
		((Control)labelGreen).BackColor = Color.Transparent;
		((Control)labelGreen).ForeColor = Color.DimGray;
		((Control)labelGreen).Location = new Point(12, 268);
		((Control)labelGreen).Name = "labelGreen";
		((Control)labelGreen).Size = new Size(90, 20);
		((Control)labelGreen).TabIndex = 5;
		((Control)labelGreen).Text = "&Green";
		labelGreen.TextAlign = (ContentAlignment)16;
		((Control)labelCustomColors).BackColor = Color.Transparent;
		((Control)labelCustomColors).ForeColor = Color.DimGray;
		((Control)labelCustomColors).Location = new Point(12, 12);
		((Control)labelCustomColors).Name = "labelCustomColors";
		((Control)labelCustomColors).Size = new Size(195, 20);
		((Control)labelCustomColors).TabIndex = 0;
		((Control)labelCustomColors).Text = " Colors";
		labelCustomColors.TextAlign = (ContentAlignment)16;
		((Control)buttonOK).AccessibleRole = (AccessibleRole)43;
		((Control)buttonOK).Anchor = (AnchorStyles)9;
		buttonOK.DialogResult = (DialogResult)1;
		((Control)buttonOK).Location = new Point(86, 12);
		((Control)buttonOK).Name = "buttonOK";
		((Control)buttonOK).Size = new Size(74, 24);
		((Control)buttonOK).TabIndex = 1;
		((Control)buttonOK).Text = "OK";
		((ButtonBase)buttonOK).UseVisualStyleBackColor = true;
		((Control)buttonCancel).AccessibleRole = (AccessibleRole)43;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(166, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(74, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)labelNewColor).Anchor = (AnchorStyles)6;
		((Control)labelNewColor).BackColor = Color.White;
		((Control)labelNewColor).ForeColor = Color.DimGray;
		((Control)labelNewColor).Location = new Point(65, 406);
		((Control)labelNewColor).Name = "labelNewColor";
		((Control)labelNewColor).Size = new Size(87, 22);
		((Control)labelNewColor).TabIndex = 4;
		((Control)labelNewColor).Text = "New";
		labelNewColor.TextAlign = (ContentAlignment)32;
		((Control)labelCurrentColor).Anchor = (AnchorStyles)6;
		((Control)labelCurrentColor).BackColor = Color.White;
		((Control)labelCurrentColor).ForeColor = Color.DimGray;
		((Control)labelCurrentColor).Location = new Point(155, 406);
		((Control)labelCurrentColor).Name = "labelCurrentColor";
		((Control)labelCurrentColor).Size = new Size(84, 22);
		((Control)labelCurrentColor).TabIndex = 5;
		((Control)labelCurrentColor).Text = "Current";
		labelCurrentColor.TextAlign = (ContentAlignment)32;
		((Control)labelRed).BackColor = Color.Transparent;
		((Control)labelRed).ForeColor = Color.DimGray;
		((Control)labelRed).Location = new Point(12, 235);
		((Control)labelRed).Name = "labelRed";
		((Control)labelRed).Size = new Size(90, 20);
		((Control)labelRed).TabIndex = 3;
		((Control)labelRed).Text = "&Red";
		labelRed.TextAlign = (ContentAlignment)16;
		((Control)tabControl1).Anchor = (AnchorStyles)7;
		((Control)tabControl1).Controls.Add((Control)(object)tabPageStandard);
		((Control)tabControl1).Controls.Add((Control)(object)tabPageCustom);
		((Control)tabControl1).Location = new Point(12, 12);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedIndex = 0;
		((Control)tabControl1).Size = new Size(227, 391);
		((Control)tabControl1).TabIndex = 8;
		((Control)tabPageStandard).Controls.Add((Control)(object)colorCombControl1);
		((Control)tabPageStandard).Controls.Add((Control)(object)labelStandardColors);
		tabPageStandard.Location = new Point(4, 22);
		((Control)tabPageStandard).Name = "tabPageStandard";
		((Control)tabPageStandard).Padding = new Padding(3);
		((Control)tabPageStandard).Size = new Size(219, 365);
		tabPageStandard.TabIndex = 0;
		((Control)tabPageStandard).Text = "Standard";
		tabPageStandard.UseVisualStyleBackColor = true;
		((Control)tabPageCustom).Controls.Add((Control)(object)labelHex);
		((Control)tabPageCustom).Controls.Add((Control)(object)labelAlpha);
		((Control)tabPageCustom).Controls.Add((Control)(object)labelCustomColors);
		((Control)tabPageCustom).Controls.Add((Control)(object)labelGreen);
		((Control)tabPageCustom).Controls.Add((Control)(object)labelBlue);
		((Control)tabPageCustom).Controls.Add((Control)(object)labelRed);
		((Control)tabPageCustom).Controls.Add((Control)(object)textBoxHex);
		((Control)tabPageCustom).Controls.Add((Control)(object)numericAlpha);
		((Control)tabPageCustom).Controls.Add((Control)(object)colorContrastControl1);
		((Control)tabPageCustom).Controls.Add((Control)(object)customColorBlender3);
		((Control)tabPageCustom).Controls.Add((Control)(object)numericGreen);
		((Control)tabPageCustom).Controls.Add((Control)(object)numericRed);
		((Control)tabPageCustom).Controls.Add((Control)(object)numericBlue);
		tabPageCustom.Location = new Point(4, 22);
		((Control)tabPageCustom).Name = "tabPageCustom";
		((Control)tabPageCustom).Padding = new Padding(3);
		((Control)tabPageCustom).Size = new Size(219, 365);
		tabPageCustom.TabIndex = 1;
		((Control)tabPageCustom).Text = "Custom";
		tabPageCustom.UseVisualStyleBackColor = true;
		((Control)labelHex).BackColor = Color.Transparent;
		((Control)labelHex).ForeColor = Color.DimGray;
		((Control)labelHex).Location = new Point(12, 334);
		((Control)labelHex).Name = "labelHex";
		((Control)labelHex).Size = new Size(90, 20);
		((Control)labelHex).TabIndex = 11;
		((Control)labelHex).Text = "&Hex";
		labelHex.TextAlign = (ContentAlignment)16;
		((Control)labelAlpha).BackColor = Color.Transparent;
		((Control)labelAlpha).ForeColor = Color.DimGray;
		((Control)labelAlpha).Location = new Point(12, 202);
		((Control)labelAlpha).Name = "labelAlpha";
		((Control)labelAlpha).Size = new Size(90, 20);
		((Control)labelAlpha).TabIndex = 9;
		((Control)labelAlpha).Text = "&Alpha";
		labelAlpha.TextAlign = (ContentAlignment)16;
		((Control)panel1).Controls.Add((Control)(object)buttonCancel);
		((Control)panel1).Controls.Add((Control)(object)buttonOK);
		((Control)panel1).Dock = (DockStyle)2;
		((Control)panel1).Location = new Point(0, 467);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(250, 46);
		((Control)panel1).TabIndex = 9;
		((Control)panel1).Paint += new PaintEventHandler(Panel1_Paint);
		((Control)colorPipet).Anchor = (AnchorStyles)6;
		((Control)colorPipet).BackColor = Color.White;
		((Control)colorPipet).ForeColor = Color.DimGray;
		((Control)colorPipet).Location = new Point(12, 415);
		((Control)colorPipet).Name = "colorPipet";
		((Control)colorPipet).Size = new Size(40, 40);
		((Control)colorPipet).TabIndex = 5;
		colorPipet.SelectedColorChanged += colorPipet_SelectedColorChanged;
		((Control)colorCombControl1).BackColor = Color.Transparent;
		((Control)colorCombControl1).Location = new Point(3, 34);
		((Control)colorCombControl1).Name = "colorCombControl1";
		((Control)colorCombControl1).Size = new Size(213, 296);
		((Control)colorCombControl1).TabIndex = 0;
		colorCombControl1.SelectedColorChanged += colorCombControl1_SelectedColorChanged;
		((Control)textBoxHex).BackColor = Color.White;
		((Control)textBoxHex).Location = new Point(106, 334);
		((Control)textBoxHex).Name = "textBoxHex";
		((Control)textBoxHex).Size = new Size(101, 21);
		((Control)textBoxHex).TabIndex = 10;
		((Control)textBoxHex).Leave += TextBoxHex_Leave;
		numericAlpha.Minimum = 0m;
		numericAlpha.Maximum = 255m;
		((Control)numericAlpha).BackColor = Color.White;
		numericAlpha.DecimalPlaces = 0;
		((Control)numericAlpha).Location = new Point(106, 202);
		((Control)numericAlpha).Name = "numericAlpha";
		((Control)numericAlpha).Size = new Size(101, 21);
		((Control)numericAlpha).TabIndex = 2;
		numericAlpha.ValueChanged += numericRGBValueChanged;
		((Control)colorContrastControl1).BackColor = Color.Transparent;
		((Control)colorContrastControl1).Location = new Point(192, 35);
		((Control)colorContrastControl1).Name = "colorContrastControl1";
		((Control)colorContrastControl1).Size = new Size(24, 152);
		((Control)colorContrastControl1).TabIndex = 1;
		colorContrastControl1.SelectedColorChanged += colorContrastControl1_SelectedColorChanged;
		((Control)customColorBlender3).Location = new Point(12, 35);
		((Control)customColorBlender3).Name = "customColorBlender3";
		((Control)customColorBlender3).Size = new Size(174, 152);
		((Control)customColorBlender3).TabIndex = 0;
		customColorBlender3.SelectedColorChanged += customColorBlender3_SelectedColorChanged;
		numericGreen.Minimum = 0m;
		numericGreen.Maximum = 255m;
		((Control)numericGreen).BackColor = Color.White;
		numericGreen.DecimalPlaces = 0;
		((Control)numericGreen).Location = new Point(106, 268);
		((Control)numericGreen).Name = "numericGreen";
		((Control)numericGreen).Size = new Size(101, 21);
		((Control)numericGreen).TabIndex = 4;
		numericGreen.ValueChanged += numericRGBValueChanged;
		numericRed.Minimum = 0m;
		numericRed.Maximum = 255m;
		((Control)numericRed).BackColor = Color.White;
		numericRed.DecimalPlaces = 0;
		((Control)numericRed).Location = new Point(106, 235);
		((Control)numericRed).Name = "numericRed";
		((Control)numericRed).Size = new Size(101, 21);
		((Control)numericRed).TabIndex = 3;
		numericRed.ValueChanged += numericRGBValueChanged;
		numericBlue.Minimum = 0m;
		numericBlue.Maximum = 255m;
		((Control)numericBlue).BackColor = Color.White;
		numericBlue.DecimalPlaces = 0;
		((Control)numericBlue).Location = new Point(106, 301);
		((Control)numericBlue).Name = "numericBlue";
		((Control)numericBlue).Size = new Size(101, 21);
		((Control)numericBlue).TabIndex = 5;
		numericBlue.ValueChanged += numericRGBValueChanged;
		((Control)colorSelectionPreview).Anchor = (AnchorStyles)6;
		colorSelectionPreview.CurrentColor = Color.Black;
		((Control)colorSelectionPreview).Location = new Point(64, 434);
		((Control)colorSelectionPreview).Name = "colorSelectionPreview";
		colorSelectionPreview.NewColor = Color.Empty;
		((Control)colorSelectionPreview).Size = new Size(176, 21);
		((Control)colorSelectionPreview).TabIndex = 7;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOK;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(250, 513);
		((Control)this).Controls.Add((Control)(object)colorPipet);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)tabControl1);
		((Control)this).Controls.Add((Control)(object)colorSelectionPreview);
		((Control)this).Controls.Add((Control)(object)labelCurrentColor);
		((Control)this).Controls.Add((Control)(object)labelNewColor);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "CustomColorDialogMetro";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "Colors";
		((Control)tabControl1).ResumeLayout(false);
		((Control)tabPageStandard).ResumeLayout(false);
		((Control)tabPageCustom).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)numericAlpha).EndInit();
		((ISupportInitialize)numericGreen).EndInit();
		((ISupportInitialize)numericRed).EndInit();
		((ISupportInitialize)numericBlue).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void colorPipet_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!m_SettingColor)
		{
			SetNewColor(colorPipet.SelectedColor);
			SetColorForBlenderContrastControl(colorPipet.SelectedColor);
		}
	}

	private void colorCombControl1_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!m_SettingColor)
		{
			SetNewColor(colorCombControl1.SelectedColor);
			SetColorForBlenderContrastControl(colorCombControl1.SelectedColor);
		}
	}

	private void customColorBlender3_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!m_SettingColor)
		{
			colorContrastControl1.SelectedColor = customColorBlender3.SelectedColor;
			SetNewColor(colorContrastControl1.SelectedColor);
		}
	}

	public void SetNewColor(Color color)
	{
		if (m_SettingColor)
		{
			return;
		}
		m_SettingColor = true;
		try
		{
			m_NewColor = color;
			colorSelectionPreview.NewColor = color;
			numericRed.Value = m_NewColor.R;
			numericGreen.Value = m_NewColor.G;
			numericBlue.Value = m_NewColor.B;
			numericAlpha.Value = m_NewColor.A;
			if (m_NewColor.A == byte.MaxValue)
			{
				Color color2 = Color.FromArgb(0, m_NewColor);
				((Control)textBoxHex).Text = "#" + color2.R.ToString("X2") + color2.G.ToString("X2") + color2.B.ToString("X2");
				return;
			}
			((Control)textBoxHex).Text = "#" + m_NewColor.A.ToString("X2") + m_NewColor.R.ToString("X2") + m_NewColor.G.ToString("X2") + m_NewColor.B.ToString("X2");
		}
		finally
		{
			m_SettingColor = false;
		}
	}

	private void SetColorForBlenderContrastControl(Color value)
	{
		m_SettingColor = true;
		double H = 0.0;
		double S = 0.0;
		double L = 0.0;
		ColorContrastControl.GetHSLFromRGB(value, ref H, ref S, ref L);
		Color colorFromHSL = ColorContrastControl.GetColorFromHSL(H, S, 0.5);
		customColorBlender3.SelectedColor = colorFromHSL;
		colorContrastControl1.SelectedColor = colorFromHSL;
		colorContrastControl1.SetLuminance(L);
		m_SettingColor = false;
	}

	private void Localization()
	{
		((Control)this).Text = StiLocalization.Get("Gui", "colorpickerdialog_caption");
		((Control)labelStandardColors).Text = StiLocalization.Get("Gui", "colorpickerdialog_caption");
		((Control)tabPageStandard).Text = StiLocalization.Get("Gui", "colorpickerdialog_tabstandard");
		((Control)tabPageCustom).Text = StiLocalization.Get("Gui", "colorpickerdialog_tabcustom");
		((Control)labelNewColor).Text = StiLocalization.Get("Gui", "colorpickerdialog_newcolorlabel");
		((Control)labelCurrentColor).Text = StiLocalization.Get("Gui", "colorpickerdialog_currentcolorlabel");
		((Control)labelCustomColors).Text = StiLocalization.Get("Gui", "colorpickerdialog_caption");
		((Control)labelAlpha).Text = StiLocalization.Get("Gui", "colorpickerdialog_alphalabel").Replace(":", "");
		((Control)labelRed).Text = StiLocalization.Get("Gui", "colorpickerdialog_redlabel").Replace(":", "");
		((Control)labelGreen).Text = StiLocalization.Get("Gui", "colorpickerdialog_greenlabel").Replace(":", "");
		((Control)labelBlue).Text = StiLocalization.Get("Gui", "colorpickerdialog_bluelabel").Replace(":", "");
	}

	private void numericRGBValueChanged(object sender, EventArgs e)
	{
		if (!m_SettingColor)
		{
			Color color = Color.FromArgb((int)numericAlpha.Value, (int)numericRed.Value, (int)numericGreen.Value, (int)numericBlue.Value);
			SetNewColor(color);
			SetColorForBlenderContrastControl(color);
		}
	}

	private void colorContrastControl1_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!m_SettingColor)
		{
			SetNewColor(colorContrastControl1.SelectedColor);
		}
	}

	private void Panel1_Paint(object sender, PaintEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Pen val = new Pen(Color.Gray);
		try
		{
			val.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(clientRectangle.X, clientRectangle.Y);
			Point point2 = new Point(clientRectangle.Right, clientRectangle.Y);
			graphics.DrawLine(val, point, point2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void TextBoxHex_Leave(object sender, EventArgs e)
	{
		try
		{
			string text = ((Control)textBoxHex).Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				text = text.Trim();
				if (!text.StartsWith("#"))
				{
					text = "#" + text;
				}
				Color newColor = (Color)new ColorConverter().ConvertFromString(text);
				SetNewColor(newColor);
			}
		}
		catch
		{
			SetNewColor(m_NewColor);
		}
	}
}
