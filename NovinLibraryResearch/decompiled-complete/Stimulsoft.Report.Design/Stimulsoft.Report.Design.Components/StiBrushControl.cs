using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Components;

[ToolboxItem(false)]
public class StiBrushControl : UserControl
{
	private bool locked;

	private bool isInit;

	private bool createNewBrush = true;

	private StiBrush brush;

	private IContainer components;

	private Panel panelSelectBrush;

	private StiOperationCheckBox btGlass;

	private StiOperationCheckBox btHatch;

	private StiOperationCheckBox btGlare;

	private StiOperationCheckBox btSolid;

	private StiOperationCheckBox btGradient;

	private StiOperationCheckBox btEmpty;

	private Panel panelSolid;

	private StiLabelUX labelSolidColor;

	private Panel panelHatch;

	private StiLabelUX labelHatchForeColor;

	private StiLabelUX labelHatchStyle;

	private StiLabelUX labelHatchBackColor;

	private StiComboBox cbHatchStyle;

	private Panel panelGradient;

	private StiLabelUX labelGradientEndColor;

	private StiLabelUX labelGradientStartColor;

	private StiLabelUX labelGradientAngle;

	private StiNumericUpDown numericUpDownGradientAngle;

	private Panel panelGlare;

	private StiNumericUpDown numericUpDownGlareAngle;

	private StiLabelUX labelGlareEndColor;

	private StiLabelUX labelGlareStartColor;

	private StiLabelUX labelGlareAngle;

	private StiLabelUX labelGlareScale;

	private StiTextBox tbGlareScale;

	private StiLabelUX labelGlareFocus;

	private StiTextBox tbGlareFocus;

	private Panel panelGlass;

	private StiLabelUX labelBlend;

	private StiLabelUX labelGlassColor;

	private CheckBox checkBoxGlassDrawHatch;

	private StiTextBox tbGlassBlend;

	private ToolTip toolTip;

	private ColorPickerButton colorPickerSolid;

	private ColorPickerButton colorPickerHatchBackColor;

	private ColorPickerButton colorPickerHatchForeColor;

	private ColorPickerButton colorPickerGradientEndColor;

	private ColorPickerButton colorPickerGradientStartColor;

	private ColorPickerButton colorPickerGlareEndColor;

	private ColorPickerButton colorPickerGlareStartColor;

	private ColorPickerButton colorPickerGlassColor;

	public int GeneralWidth => ((Control)colorPickerSolid).Width;

	public StiBrush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			brush = value;
			isInit = false;
			SetBrush();
			CheckState();
			isInit = true;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).ForeColor;
			}
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).BackColor;
			}
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

	public event EventHandler BrushChanged;

	public StiBrushControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			locked = true;
			isInit = true;
			InitDefaultColor();
			CheckState();
			FillHatchStyle();
			locked = false;
			DoScaling();
		}
	}

	private void OnBrushChanged()
	{
		if (isInit)
		{
			this.BrushChanged?.Invoke(this, EventArgs.Empty);
			RefreshBrushes();
		}
	}

	private void btEmpty_Click(object sender, EventArgs e)
	{
		SelectedBrush((CheckBox)((sender is CheckBox) ? sender : null));
	}

	private void btSolid_Click(object sender, EventArgs e)
	{
		SelectedBrush((CheckBox)((sender is CheckBox) ? sender : null));
	}

	private void btHatch_Click(object sender, EventArgs e)
	{
		SelectedBrush((CheckBox)((sender is CheckBox) ? sender : null));
	}

	private void btGradient_Click(object sender, EventArgs e)
	{
		SelectedBrush((CheckBox)((sender is CheckBox) ? sender : null));
	}

	private void btGlare_Click(object sender, EventArgs e)
	{
		SelectedBrush((CheckBox)((sender is CheckBox) ? sender : null));
	}

	private void btGlass_Click(object sender, EventArgs e)
	{
		SelectedBrush((CheckBox)((sender is CheckBox) ? sender : null));
	}

	private void SetSolidBrush()
	{
		brush = GetSolidBrush();
		OnBrushChanged();
	}

	private StiSolidBrush GetSolidBrush()
	{
		return new StiSolidBrush(colorPickerSolid.SelectedColor);
	}

	private void ColorPickerSolid_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			SetSolidBrush();
		}
	}

	private void SetHatchBrush()
	{
		brush = GetHatchBrush();
		OnBrushChanged();
	}

	private StiHatchBrush GetHatchBrush()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		HatchStyle style = (HatchStyle)((((ComboBox)cbHatchStyle).SelectedItem == null || !(((ComboBoxItem)((ComboBox)cbHatchStyle).SelectedItem).Tag is HatchStyle)) ? 32 : ((int)(HatchStyle)((ComboBoxItem)((ComboBox)cbHatchStyle).SelectedItem).Tag));
		return new StiHatchBrush(style, colorPickerHatchForeColor.SelectedColor, colorPickerHatchBackColor.SelectedColor);
	}

	private void ColorPickerHatchBackColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			SetHatchBrush();
		}
	}

	private void ComboBoxHatchStyle_SelectedValueChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			SetHatchBrush();
		}
	}

	private void SetGradientBrush()
	{
		brush = GetGradientBrush();
		OnBrushChanged();
	}

	private StiGradientBrush GetGradientBrush()
	{
		if (!double.TryParse(((Control)numericUpDownGradientAngle).Text, out var result))
		{
			((Control)numericUpDownGradientAngle).Text = "0";
		}
		return new StiGradientBrush(colorPickerGradientStartColor.SelectedColor, colorPickerGradientEndColor.SelectedColor, result);
	}

	private void LinearGradientChange(object sender, EventArgs e)
	{
		if (!locked)
		{
			SetGradientBrush();
		}
	}

	private void SetGlareBrush()
	{
		brush = GetGlareBrush();
		OnBrushChanged();
	}

	private StiGlareBrush GetGlareBrush()
	{
		if (!double.TryParse(((Control)numericUpDownGlareAngle).Text, out var result))
		{
			((Control)numericUpDownGlareAngle).Text = "0";
		}
		if (!float.TryParse(((Control)tbGlareFocus).Text, out var result2) || result2 < 0f || result2 > 1f)
		{
			((Control)tbGlareFocus).Text = 0.5f.ToString();
			result2 = 0.5f;
		}
		if (!float.TryParse(((Control)tbGlareScale).Text, out var result3) || result3 < 0f || result3 > 1f)
		{
			((Control)tbGlareScale).Text = "1";
			result3 = 1f;
		}
		return new StiGlareBrush(colorPickerGlareStartColor.SelectedColor, colorPickerGlareEndColor.SelectedColor, result, result2, result3);
	}

	private void GlareColorChange(object sender, EventArgs e)
	{
		if (!locked)
		{
			SetGlareBrush();
		}
	}

	private void SetGlassBrush()
	{
		brush = GetGlassBrush();
		OnBrushChanged();
	}

	private StiGlassBrush GetGlassBrush()
	{
		if (!float.TryParse(((Control)tbGlassBlend).Text, out var result) || result < 0f || result > 1f)
		{
			((Control)tbGlassBlend).Text = 0.2f.ToString();
			result = 0.2f;
		}
		return new StiGlassBrush(colorPickerGlassColor.SelectedColor, checkBoxGlassDrawHatch.Checked, result);
	}

	private void GlassChange(object sender, EventArgs e)
	{
		if (!locked)
		{
			SetGlassBrush();
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		RefreshBrushes();
	}

	private void Localize()
	{
		((Control)labelSolidColor).Text = Loc.GetCleaned("PropertyMain", "Color");
		((Control)labelGlassColor).Text = ((Control)labelSolidColor).Text;
		((Control)labelGradientAngle).Text = Loc.GetCleaned("PropertyMain", "Angle");
		((Control)labelGlareAngle).Text = ((Control)labelGradientAngle).Text;
		((Control)labelGradientStartColor).Text = Loc.GetCleaned("PropertyMain", "StartColor");
		((Control)labelGlareStartColor).Text = ((Control)labelGradientStartColor).Text;
		((Control)labelGradientEndColor).Text = Loc.GetCleaned("PropertyMain", "EndColor");
		((Control)labelGlareEndColor).Text = ((Control)labelGradientEndColor).Text;
		((Control)labelHatchForeColor).Text = Loc.GetCleaned("PropertyMain", "ForeColor");
		((Control)labelHatchBackColor).Text = Loc.GetCleaned("PropertyMain", "BackColor");
		((Control)labelHatchStyle).Text = Loc.GetCleaned("PropertyMain", "Style");
		((Control)labelGlareFocus).Text = Loc.GetCleaned("PropertyMain", "Focus");
		((Control)labelGlareScale).Text = Loc.GetCleaned("PropertyMain", "Scale");
		((Control)labelBlend).Text = Loc.GetCleaned("PropertyMain", "Blend");
		((Control)checkBoxGlassDrawHatch).Text = StiLocalization.Get("PropertyMain", "DrawHatch");
		toolTip.SetToolTip((Control)(object)btEmpty, StiLocalization.Get("Report", "StiEmptyBrush"));
		toolTip.SetToolTip((Control)(object)btSolid, StiLocalization.Get("Report", "StiSolidBrush"));
		toolTip.SetToolTip((Control)(object)btHatch, StiLocalization.Get("Report", "StiHatchBrush"));
		toolTip.SetToolTip((Control)(object)btGradient, StiLocalization.Get("Report", "StiGradientBrush"));
		toolTip.SetToolTip((Control)(object)btGlare, StiLocalization.Get("Report", "StiGlareBrush"));
		toolTip.SetToolTip((Control)(object)btGlass, StiLocalization.Get("Report", "StiGlassBrush"));
	}

	private void DoScaling()
	{
		((Control)cbHatchStyle).Width = ((Control)colorPickerSolid).Width;
		((Control)numericUpDownGradientAngle).Width = ((Control)colorPickerSolid).Width;
		((Control)numericUpDownGlareAngle).Width = ((Control)colorPickerSolid).Width;
		((Control)tbGlareFocus).Width = ((Control)colorPickerSolid).Width;
		((Control)tbGlareScale).Width = ((Control)colorPickerSolid).Width;
		((Control)tbGlassBlend).Width = ((Control)colorPickerSolid).Width;
	}

	private void InitDefaultColor()
	{
		colorPickerSolid.SelectedColor = Color.Silver;
		colorPickerGlassColor.SelectedColor = Color.Silver;
		colorPickerHatchForeColor.SelectedColor = Color.White;
		colorPickerHatchBackColor.SelectedColor = Color.Silver;
		colorPickerGradientStartColor.SelectedColor = Color.White;
		colorPickerGradientEndColor.SelectedColor = Color.Silver;
		colorPickerGlareStartColor.SelectedColor = Color.White;
		colorPickerGlareEndColor.SelectedColor = Color.Silver;
	}

	private void FillHatchStyle()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		((ComboBox)cbHatchStyle).Items.Clear();
		foreach (HatchStyle item in Enum.GetValues(typeof(HatchStyle)).Cast<HatchStyle>().Distinct())
		{
			HatchStyle current = item;
			ComboBoxItem comboBoxItem = new ComboBoxItem
			{
				Text = StiLocalization.Get("PropertyHatchStyle", ((object)(HatchStyle)(ref current)).ToString()),
				Tag = current
			};
			((ComboBox)cbHatchStyle).Items.Add((object)comboBoxItem);
			if ((int)current == 3)
			{
				((ComboBox)cbHatchStyle).SelectedItem = comboBoxItem;
			}
		}
	}

	private void SelectedBrush(CheckBox button)
	{
		locked = true;
		foreach (object item in (ArrangedElementCollection)((Control)panelSelectBrush).Controls)
		{
			CheckBox val = (CheckBox)((item is CheckBox) ? item : null);
			if (val != null && !((object)val).Equals((object?)button))
			{
				val.Checked = false;
			}
		}
		CheckState();
	}

	private void CheckState()
	{
		int num = ((Control)panelSelectBrush).Height;
		((Control)panelSolid).Visible = ((CheckBox)btSolid).Checked;
		((Control)panelHatch).Visible = ((CheckBox)btHatch).Checked;
		((Control)panelGradient).Visible = ((CheckBox)btGradient).Checked;
		((Control)panelGlare).Visible = ((CheckBox)btGlare).Checked;
		((Control)panelGlass).Visible = ((CheckBox)btGlass).Checked;
		locked = true;
		if (((CheckBox)btEmpty).Checked)
		{
			if (createNewBrush)
			{
				brush = new StiEmptyBrush();
			}
		}
		else if (((CheckBox)btSolid).Checked)
		{
			num += ((Control)panelSolid).Height;
			if (createNewBrush)
			{
				SetSolidBrush();
				locked = false;
			}
		}
		else if (((CheckBox)btHatch).Checked)
		{
			num += ((Control)panelHatch).Height;
			if (createNewBrush)
			{
				SetHatchBrush();
				locked = false;
			}
		}
		else if (((CheckBox)btGradient).Checked)
		{
			num += ((Control)panelGradient).Height;
			if (createNewBrush)
			{
				SetGradientBrush();
				locked = false;
			}
		}
		else if (((CheckBox)btGlare).Checked)
		{
			num += ((Control)panelGlare).Height;
			if (createNewBrush)
			{
				SetGlareBrush();
				locked = false;
			}
		}
		else if (((CheckBox)btGlass).Checked)
		{
			num += ((Control)panelGlass).Height;
			if (createNewBrush)
			{
				SetGlassBrush();
				locked = false;
			}
		}
		((Control)this).Height = num + StiScale.I2;
		OnBrushChanged();
		locked = false;
	}

	private void SetBrush()
	{
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		createNewBrush = false;
		((CheckBox)btEmpty).Checked = false;
		((CheckBox)btSolid).Checked = false;
		((CheckBox)btGradient).Checked = false;
		((CheckBox)btHatch).Checked = false;
		((CheckBox)btGlare).Checked = false;
		((CheckBox)btGlass).Checked = false;
		if (brush is StiEmptyBrush)
		{
			((CheckBox)btEmpty).Checked = true;
			createNewBrush = true;
		}
		else if (brush is StiSolidBrush stiSolidBrush)
		{
			((CheckBox)btSolid).Checked = true;
			locked = true;
			colorPickerSolid.SelectedColor = stiSolidBrush.Color;
			locked = false;
			createNewBrush = true;
		}
		else if (brush is StiGradientBrush stiGradientBrush)
		{
			((CheckBox)btGradient).Checked = true;
			locked = true;
			colorPickerGradientStartColor.SelectedColor = stiGradientBrush.StartColor;
			colorPickerGradientEndColor.SelectedColor = stiGradientBrush.EndColor;
			((Control)numericUpDownGradientAngle).Text = stiGradientBrush.Angle.ToString();
			locked = false;
			createNewBrush = true;
		}
		else if (brush is StiHatchBrush stiHatchBrush)
		{
			((CheckBox)btHatch).Checked = true;
			locked = true;
			colorPickerHatchForeColor.SelectedColor = stiHatchBrush.ForeColor;
			colorPickerHatchBackColor.SelectedColor = stiHatchBrush.BackColor;
			foreach (ComboBoxItem item in ((ComboBox)cbHatchStyle).Items)
			{
				if ((HatchStyle)item.Tag == stiHatchBrush.Style)
				{
					((ComboBox)cbHatchStyle).SelectedItem = item;
					break;
				}
			}
			locked = false;
			createNewBrush = true;
		}
		else if (brush is StiGlareBrush stiGlareBrush)
		{
			((CheckBox)btGlare).Checked = true;
			locked = true;
			((Control)numericUpDownGlareAngle).Text = stiGlareBrush.Angle.ToString();
			colorPickerGlareStartColor.SelectedColor = stiGlareBrush.StartColor;
			colorPickerGlareEndColor.SelectedColor = stiGlareBrush.EndColor;
			((Control)tbGlareFocus).Text = stiGlareBrush.Focus.ToString();
			((Control)tbGlareScale).Text = stiGlareBrush.Scale.ToString();
			locked = false;
			createNewBrush = true;
		}
		else
		{
			if (brush is StiGlassBrush stiGlassBrush)
			{
				((CheckBox)btGlass).Checked = true;
				locked = true;
				colorPickerGlassColor.SelectedColor = stiGlassBrush.Color;
				((Control)tbGlassBlend).Text = stiGlassBrush.Blend.ToString();
				checkBoxGlassDrawHatch.Checked = stiGlassBrush.DrawHatch;
				locked = false;
			}
			createNewBrush = true;
		}
	}

	private void RefreshBrushes()
	{
		Size size = ((Control)btEmpty).Size;
		size.Width -= StiScale.I8;
		size.Height -= StiScale.I8;
		((ButtonBase)btEmpty).Image = new StiEmptyBrush().GetImage(size.Width, size.Height);
		((ButtonBase)btSolid).Image = GetSolidBrush().GetImage(size.Width, size.Height);
		((ButtonBase)btHatch).Image = GetHatchBrush().GetImage(size.Width, size.Height);
		((ButtonBase)btGradient).Image = GetGradientBrush().GetImage(size.Width, size.Height);
		((ButtonBase)btGlare).Image = GetGlareBrush().GetImage(size.Width, size.Height);
		((ButtonBase)btGlass).Image = GetGlassBrush().GetImage(size.Width, size.Height);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		panelSelectBrush = new Panel();
		btGlass = new StiOperationCheckBox();
		btHatch = new StiOperationCheckBox();
		btGlare = new StiOperationCheckBox();
		btSolid = new StiOperationCheckBox();
		btGradient = new StiOperationCheckBox();
		btEmpty = new StiOperationCheckBox();
		panelSolid = new Panel();
		colorPickerSolid = new ColorPickerButton();
		labelSolidColor = new StiLabelUX();
		panelHatch = new Panel();
		colorPickerHatchBackColor = new ColorPickerButton();
		colorPickerHatchForeColor = new ColorPickerButton();
		cbHatchStyle = new StiComboBox();
		labelHatchStyle = new StiLabelUX();
		labelHatchBackColor = new StiLabelUX();
		labelHatchForeColor = new StiLabelUX();
		panelGradient = new Panel();
		colorPickerGradientEndColor = new ColorPickerButton();
		colorPickerGradientStartColor = new ColorPickerButton();
		numericUpDownGradientAngle = new StiNumericUpDown();
		labelGradientEndColor = new StiLabelUX();
		labelGradientStartColor = new StiLabelUX();
		labelGradientAngle = new StiLabelUX();
		panelGlare = new Panel();
		colorPickerGlareEndColor = new ColorPickerButton();
		colorPickerGlareStartColor = new ColorPickerButton();
		labelGlareScale = new StiLabelUX();
		tbGlareScale = new StiTextBox();
		labelGlareFocus = new StiLabelUX();
		tbGlareFocus = new StiTextBox();
		numericUpDownGlareAngle = new StiNumericUpDown();
		labelGlareEndColor = new StiLabelUX();
		labelGlareStartColor = new StiLabelUX();
		labelGlareAngle = new StiLabelUX();
		panelGlass = new Panel();
		colorPickerGlassColor = new ColorPickerButton();
		checkBoxGlassDrawHatch = new CheckBox();
		tbGlassBlend = new StiTextBox();
		labelBlend = new StiLabelUX();
		labelGlassColor = new StiLabelUX();
		toolTip = new ToolTip(components);
		((Control)panelSelectBrush).SuspendLayout();
		((Control)panelSolid).SuspendLayout();
		((Control)panelHatch).SuspendLayout();
		((Control)panelGradient).SuspendLayout();
		((ISupportInitialize)numericUpDownGradientAngle).BeginInit();
		((Control)panelGlare).SuspendLayout();
		((ISupportInitialize)numericUpDownGlareAngle).BeginInit();
		((Control)panelGlass).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelSelectBrush).Controls.Add((Control)(object)btGlass);
		((Control)panelSelectBrush).Controls.Add((Control)(object)btHatch);
		((Control)panelSelectBrush).Controls.Add((Control)(object)btGlare);
		((Control)panelSelectBrush).Controls.Add((Control)(object)btSolid);
		((Control)panelSelectBrush).Controls.Add((Control)(object)btGradient);
		((Control)panelSelectBrush).Controls.Add((Control)(object)btEmpty);
		((Control)panelSelectBrush).Dock = (DockStyle)1;
		((Control)panelSelectBrush).Location = new Point(0, 0);
		((Control)panelSelectBrush).Margin = new Padding(0);
		((Control)panelSelectBrush).Name = "panelSelectBrush";
		((Control)panelSelectBrush).Size = new Size(264, 26);
		((Control)panelSelectBrush).TabIndex = 0;
		panelSelectBrush.TabStop = true;
		((Control)btGlass).Location = new Point(191, 3);
		((Control)btGlass).Name = "btGlass";
		((Control)btGlass).Size = new Size(35, 23);
		((Control)btGlass).TabIndex = 5;
		((ButtonBase)btGlass).UseVisualStyleBackColor = false;
		((Control)btGlass).Click += btGlass_Click;
		((Control)btHatch).Location = new Point(76, 3);
		((Control)btHatch).Name = "btHatch";
		((Control)btHatch).Size = new Size(34, 23);
		((Control)btHatch).TabIndex = 2;
		((ButtonBase)btHatch).UseVisualStyleBackColor = false;
		((Control)btHatch).Click += btHatch_Click;
		((Control)btGlare).Location = new Point(153, 3);
		((Control)btGlare).Name = "btGlare";
		((Control)btGlare).Size = new Size(35, 23);
		((Control)btGlare).TabIndex = 4;
		((ButtonBase)btGlare).UseVisualStyleBackColor = false;
		((Control)btGlare).Click += btGlare_Click;
		((Control)btSolid).Location = new Point(38, 3);
		((Control)btSolid).Name = "btSolid";
		((Control)btSolid).Size = new Size(34, 23);
		((Control)btSolid).TabIndex = 1;
		((ButtonBase)btSolid).UseVisualStyleBackColor = false;
		((Control)btSolid).Click += btSolid_Click;
		((Control)btGradient).Location = new Point(114, 3);
		((Control)btGradient).Name = "btGradient";
		((Control)btGradient).Size = new Size(35, 23);
		((Control)btGradient).TabIndex = 3;
		((ButtonBase)btGradient).UseVisualStyleBackColor = false;
		((Control)btGradient).Click += btGradient_Click;
		((Control)btEmpty).Location = new Point(0, 3);
		((Control)btEmpty).Name = "btEmpty";
		((Control)btEmpty).Size = new Size(34, 23);
		((Control)btEmpty).TabIndex = 0;
		((ButtonBase)btEmpty).UseVisualStyleBackColor = false;
		((Control)btEmpty).Click += btEmpty_Click;
		((Control)panelSolid).Controls.Add((Control)(object)colorPickerSolid);
		((Control)panelSolid).Controls.Add((Control)(object)labelSolidColor);
		((Control)panelSolid).Dock = (DockStyle)1;
		((Control)panelSolid).Location = new Point(0, 26);
		((Control)panelSolid).Name = "panelSolid";
		((Control)panelSolid).Size = new Size(264, 33);
		((Control)panelSolid).TabIndex = 3;
		panelSolid.TabStop = true;
		((Control)colorPickerSolid).AccessibleRole = (AccessibleRole)43;
		colorPickerSolid.AutoExpandOnClick = true;
		colorPickerSolid.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerSolid.EnableMarkup = false;
		colorPickerSolid.ImagePosition = eImagePosition.Right;
		((Control)colorPickerSolid).Location = new Point(0, 6);
		colorPickerSolid.LockClosePopup = false;
		((Control)colorPickerSolid).Name = "colorPickerSolid";
		((Control)colorPickerSolid).Size = new Size(125, 24);
		((Control)colorPickerSolid).TabIndex = 6;
		((Control)colorPickerSolid).TabStop = false;
		colorPickerSolid.SelectedColorChanged += ColorPickerSolid_SelectedColorChanged;
		((Control)labelSolidColor).ForeColor = Color.DimGray;
		((Control)labelSolidColor).Location = new Point(136, 6);
		((Control)labelSolidColor).Name = "labelSolidColor";
		((Control)labelSolidColor).Size = new Size(125, 24);
		((Control)labelSolidColor).TabIndex = 0;
		((Control)labelSolidColor).Text = "Color";
		((Label)labelSolidColor).TextAlign = (ContentAlignment)16;
		((Label)labelSolidColor).UseCompatibleTextRendering = true;
		((Control)panelHatch).Controls.Add((Control)(object)colorPickerHatchBackColor);
		((Control)panelHatch).Controls.Add((Control)(object)colorPickerHatchForeColor);
		((Control)panelHatch).Controls.Add((Control)(object)cbHatchStyle);
		((Control)panelHatch).Controls.Add((Control)(object)labelHatchStyle);
		((Control)panelHatch).Controls.Add((Control)(object)labelHatchBackColor);
		((Control)panelHatch).Controls.Add((Control)(object)labelHatchForeColor);
		((Control)panelHatch).Dock = (DockStyle)1;
		((Control)panelHatch).Location = new Point(0, 59);
		((Control)panelHatch).Name = "panelHatch";
		((Control)panelHatch).Size = new Size(264, 94);
		((Control)panelHatch).TabIndex = 1;
		panelHatch.TabStop = true;
		((Control)colorPickerHatchBackColor).AccessibleRole = (AccessibleRole)43;
		colorPickerHatchBackColor.AutoExpandOnClick = true;
		colorPickerHatchBackColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerHatchBackColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerHatchBackColor).Location = new Point(0, 36);
		colorPickerHatchBackColor.LockClosePopup = false;
		((Control)colorPickerHatchBackColor).Name = "colorPickerHatchBackColor";
		((Control)colorPickerHatchBackColor).Size = new Size(125, 24);
		((Control)colorPickerHatchBackColor).TabIndex = 13;
		((Control)colorPickerHatchBackColor).TabStop = false;
		colorPickerHatchBackColor.SelectedColorChanged += ColorPickerHatchBackColor_SelectedColorChanged;
		((Control)colorPickerHatchForeColor).AccessibleRole = (AccessibleRole)43;
		colorPickerHatchForeColor.AutoExpandOnClick = true;
		colorPickerHatchForeColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerHatchForeColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerHatchForeColor).Location = new Point(0, 6);
		colorPickerHatchForeColor.LockClosePopup = false;
		((Control)colorPickerHatchForeColor).Name = "colorPickerHatchForeColor";
		((Control)colorPickerHatchForeColor).Size = new Size(125, 24);
		((Control)colorPickerHatchForeColor).TabIndex = 7;
		((Control)colorPickerHatchForeColor).TabStop = false;
		colorPickerHatchForeColor.SelectedColorChanged += ColorPickerHatchBackColor_SelectedColorChanged;
		cbHatchStyle.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbHatchStyle).Location = new Point(0, 66);
		((ComboBox)cbHatchStyle).MaxDropDownItems = 20;
		((Control)cbHatchStyle).Name = "cbHatchStyle";
		((Control)cbHatchStyle).Size = new Size(127, 21);
		((Control)cbHatchStyle).TabIndex = 2;
		((ListControl)cbHatchStyle).SelectedValueChanged += ComboBoxHatchStyle_SelectedValueChanged;
		((Control)labelHatchStyle).ForeColor = Color.DimGray;
		((Control)labelHatchStyle).Location = new Point(136, 66);
		((Control)labelHatchStyle).Name = "labelHatchStyle";
		((Control)labelHatchStyle).Size = new Size(125, 21);
		((Control)labelHatchStyle).TabIndex = 12;
		((Control)labelHatchStyle).Text = "Style";
		((Label)labelHatchStyle).TextAlign = (ContentAlignment)16;
		((Label)labelHatchStyle).UseCompatibleTextRendering = true;
		((Control)labelHatchBackColor).ForeColor = Color.DimGray;
		((Control)labelHatchBackColor).Location = new Point(136, 36);
		((Control)labelHatchBackColor).Name = "labelHatchBackColor";
		((Control)labelHatchBackColor).Size = new Size(125, 24);
		((Control)labelHatchBackColor).TabIndex = 10;
		((Control)labelHatchBackColor).Text = "Back Color";
		((Label)labelHatchBackColor).TextAlign = (ContentAlignment)16;
		((Label)labelHatchBackColor).UseCompatibleTextRendering = true;
		((Control)labelHatchForeColor).ForeColor = Color.DimGray;
		((Control)labelHatchForeColor).Location = new Point(136, 6);
		((Control)labelHatchForeColor).Name = "labelHatchForeColor";
		((Control)labelHatchForeColor).Size = new Size(125, 24);
		((Control)labelHatchForeColor).TabIndex = 8;
		((Control)labelHatchForeColor).Text = "Fore Color";
		((Label)labelHatchForeColor).TextAlign = (ContentAlignment)16;
		((Label)labelHatchForeColor).UseCompatibleTextRendering = true;
		((Control)panelGradient).Controls.Add((Control)(object)colorPickerGradientEndColor);
		((Control)panelGradient).Controls.Add((Control)(object)colorPickerGradientStartColor);
		((Control)panelGradient).Controls.Add((Control)(object)numericUpDownGradientAngle);
		((Control)panelGradient).Controls.Add((Control)(object)labelGradientEndColor);
		((Control)panelGradient).Controls.Add((Control)(object)labelGradientStartColor);
		((Control)panelGradient).Controls.Add((Control)(object)labelGradientAngle);
		((Control)panelGradient).Dock = (DockStyle)1;
		((Control)panelGradient).Location = new Point(0, 153);
		((Control)panelGradient).Name = "panelGradient";
		((Control)panelGradient).Size = new Size(264, 91);
		((Control)panelGradient).TabIndex = 2;
		panelGradient.TabStop = true;
		((Control)colorPickerGradientEndColor).AccessibleRole = (AccessibleRole)43;
		colorPickerGradientEndColor.AutoExpandOnClick = true;
		colorPickerGradientEndColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerGradientEndColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerGradientEndColor).Location = new Point(0, 36);
		colorPickerGradientEndColor.LockClosePopup = false;
		((Control)colorPickerGradientEndColor).Name = "colorPickerGradientEndColor";
		((Control)colorPickerGradientEndColor).Size = new Size(125, 24);
		((Control)colorPickerGradientEndColor).TabIndex = 19;
		((Control)colorPickerGradientEndColor).TabStop = false;
		colorPickerGradientEndColor.SelectedColorChanged += LinearGradientChange;
		((Control)colorPickerGradientStartColor).AccessibleRole = (AccessibleRole)43;
		colorPickerGradientStartColor.AutoExpandOnClick = true;
		colorPickerGradientStartColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerGradientStartColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerGradientStartColor).Location = new Point(0, 6);
		colorPickerGradientStartColor.LockClosePopup = false;
		((Control)colorPickerGradientStartColor).Name = "colorPickerGradientStartColor";
		((Control)colorPickerGradientStartColor).Size = new Size(125, 24);
		((Control)colorPickerGradientStartColor).TabIndex = 15;
		((Control)colorPickerGradientStartColor).TabStop = false;
		colorPickerGradientStartColor.SelectedColorChanged += LinearGradientChange;
		numericUpDownGradientAngle.DecimalPlaces = 0;
		((Control)numericUpDownGradientAngle).Location = new Point(0, 66);
		((Control)numericUpDownGradientAngle).Name = "numericUpDownGradientAngle";
		((Control)numericUpDownGradientAngle).Size = new Size(125, 21);
		((Control)numericUpDownGradientAngle).TabIndex = 2;
		numericUpDownGradientAngle.ValueChanged += LinearGradientChange;
		((Control)labelGradientEndColor).ForeColor = Color.DimGray;
		((Control)labelGradientEndColor).Location = new Point(136, 36);
		((Control)labelGradientEndColor).Name = "labelGradientEndColor";
		((Control)labelGradientEndColor).Size = new Size(125, 24);
		((Control)labelGradientEndColor).TabIndex = 18;
		((Control)labelGradientEndColor).Text = "End Color";
		((Label)labelGradientEndColor).TextAlign = (ContentAlignment)16;
		((Label)labelGradientEndColor).UseCompatibleTextRendering = true;
		((Control)labelGradientStartColor).ForeColor = Color.DimGray;
		((Control)labelGradientStartColor).Location = new Point(136, 6);
		((Control)labelGradientStartColor).Name = "labelGradientStartColor";
		((Control)labelGradientStartColor).Size = new Size(125, 24);
		((Control)labelGradientStartColor).TabIndex = 16;
		((Control)labelGradientStartColor).Text = "Start Color";
		((Label)labelGradientStartColor).TextAlign = (ContentAlignment)16;
		((Label)labelGradientStartColor).UseCompatibleTextRendering = true;
		((Control)labelGradientAngle).ForeColor = Color.DimGray;
		((Control)labelGradientAngle).Location = new Point(136, 66);
		((Control)labelGradientAngle).Name = "labelGradientAngle";
		((Control)labelGradientAngle).Size = new Size(125, 20);
		((Control)labelGradientAngle).TabIndex = 14;
		((Control)labelGradientAngle).Text = "Angle";
		((Label)labelGradientAngle).TextAlign = (ContentAlignment)16;
		((Label)labelGradientAngle).UseCompatibleTextRendering = true;
		((Control)panelGlare).Controls.Add((Control)(object)colorPickerGlareEndColor);
		((Control)panelGlare).Controls.Add((Control)(object)colorPickerGlareStartColor);
		((Control)panelGlare).Controls.Add((Control)(object)labelGlareScale);
		((Control)panelGlare).Controls.Add((Control)(object)tbGlareScale);
		((Control)panelGlare).Controls.Add((Control)(object)labelGlareFocus);
		((Control)panelGlare).Controls.Add((Control)(object)tbGlareFocus);
		((Control)panelGlare).Controls.Add((Control)(object)numericUpDownGlareAngle);
		((Control)panelGlare).Controls.Add((Control)(object)labelGlareEndColor);
		((Control)panelGlare).Controls.Add((Control)(object)labelGlareStartColor);
		((Control)panelGlare).Controls.Add((Control)(object)labelGlareAngle);
		((Control)panelGlare).Dock = (DockStyle)1;
		((Control)panelGlare).Location = new Point(0, 244);
		((Control)panelGlare).Name = "panelGlare";
		((Control)panelGlare).Size = new Size(264, 144);
		((Control)panelGlare).TabIndex = 3;
		panelGlare.TabStop = true;
		((Control)colorPickerGlareEndColor).AccessibleRole = (AccessibleRole)43;
		colorPickerGlareEndColor.AutoExpandOnClick = true;
		colorPickerGlareEndColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerGlareEndColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerGlareEndColor).Location = new Point(0, 36);
		colorPickerGlareEndColor.LockClosePopup = false;
		((Control)colorPickerGlareEndColor).Name = "colorPickerGlareEndColor";
		((Control)colorPickerGlareEndColor).Size = new Size(125, 24);
		((Control)colorPickerGlareEndColor).TabIndex = 29;
		((Control)colorPickerGlareEndColor).TabStop = false;
		colorPickerGlareEndColor.SelectedColorChanged += GlareColorChange;
		((Control)colorPickerGlareStartColor).AccessibleRole = (AccessibleRole)43;
		colorPickerGlareStartColor.AutoExpandOnClick = true;
		colorPickerGlareStartColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerGlareStartColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerGlareStartColor).Location = new Point(0, 6);
		colorPickerGlareStartColor.LockClosePopup = false;
		((Control)colorPickerGlareStartColor).Name = "colorPickerGlareStartColor";
		((Control)colorPickerGlareStartColor).Size = new Size(125, 24);
		((Control)colorPickerGlareStartColor).TabIndex = 14;
		((Control)colorPickerGlareStartColor).TabStop = false;
		colorPickerGlareStartColor.SelectedColorChanged += GlareColorChange;
		((Control)labelGlareScale).ForeColor = Color.DimGray;
		((Control)labelGlareScale).Location = new Point(136, 118);
		((Control)labelGlareScale).Name = "labelGlareScale";
		((Control)labelGlareScale).Size = new Size(125, 20);
		((Control)labelGlareScale).TabIndex = 28;
		((Control)labelGlareScale).Text = "Scale";
		((Label)labelGlareScale).TextAlign = (ContentAlignment)16;
		((Label)labelGlareScale).UseCompatibleTextRendering = true;
		((Control)tbGlareScale).Location = new Point(0, 118);
		((Control)tbGlareScale).Name = "tbGlareScale";
		((Control)tbGlareScale).Size = new Size(125, 21);
		((Control)tbGlareScale).TabIndex = 4;
		tbGlareScale.TextAlign = (HorizontalAlignment)1;
		((Control)tbGlareScale).TextChanged += GlareColorChange;
		((Control)labelGlareFocus).ForeColor = Color.DimGray;
		((Control)labelGlareFocus).Location = new Point(136, 92);
		((Control)labelGlareFocus).Name = "labelGlareFocus";
		((Control)labelGlareFocus).Size = new Size(125, 20);
		((Control)labelGlareFocus).TabIndex = 26;
		((Control)labelGlareFocus).Text = "Focus";
		((Label)labelGlareFocus).TextAlign = (ContentAlignment)16;
		((Label)labelGlareFocus).UseCompatibleTextRendering = true;
		((Control)tbGlareFocus).Location = new Point(0, 92);
		((Control)tbGlareFocus).Name = "tbGlareFocus";
		((Control)tbGlareFocus).Size = new Size(125, 21);
		((Control)tbGlareFocus).TabIndex = 3;
		tbGlareFocus.TextAlign = (HorizontalAlignment)1;
		((Control)tbGlareFocus).TextChanged += GlareColorChange;
		numericUpDownGlareAngle.DecimalPlaces = 0;
		((Control)numericUpDownGlareAngle).Location = new Point(0, 66);
		((Control)numericUpDownGlareAngle).Name = "numericUpDownGlareAngle";
		((Control)numericUpDownGlareAngle).Size = new Size(125, 21);
		((Control)numericUpDownGlareAngle).TabIndex = 2;
		numericUpDownGlareAngle.ValueChanged += GlareColorChange;
		((Control)labelGlareEndColor).ForeColor = Color.DimGray;
		((Control)labelGlareEndColor).Location = new Point(136, 36);
		((Control)labelGlareEndColor).Name = "labelGlareEndColor";
		((Control)labelGlareEndColor).Size = new Size(125, 24);
		((Control)labelGlareEndColor).TabIndex = 24;
		((Control)labelGlareEndColor).Text = "End Color";
		((Label)labelGlareEndColor).TextAlign = (ContentAlignment)16;
		((Label)labelGlareEndColor).UseCompatibleTextRendering = true;
		((Control)labelGlareStartColor).ForeColor = Color.DimGray;
		((Control)labelGlareStartColor).Location = new Point(136, 6);
		((Control)labelGlareStartColor).Name = "labelGlareStartColor";
		((Control)labelGlareStartColor).Size = new Size(125, 24);
		((Control)labelGlareStartColor).TabIndex = 22;
		((Control)labelGlareStartColor).Text = "Start Color";
		((Label)labelGlareStartColor).TextAlign = (ContentAlignment)16;
		((Label)labelGlareStartColor).UseCompatibleTextRendering = true;
		((Control)labelGlareAngle).ForeColor = Color.DimGray;
		((Control)labelGlareAngle).Location = new Point(136, 66);
		((Control)labelGlareAngle).Name = "labelGlareAngle";
		((Control)labelGlareAngle).Size = new Size(125, 19);
		((Control)labelGlareAngle).TabIndex = 20;
		((Control)labelGlareAngle).Text = "Angle";
		((Label)labelGlareAngle).TextAlign = (ContentAlignment)16;
		((Label)labelGlareAngle).UseCompatibleTextRendering = true;
		((Control)panelGlass).Controls.Add((Control)(object)colorPickerGlassColor);
		((Control)panelGlass).Controls.Add((Control)(object)checkBoxGlassDrawHatch);
		((Control)panelGlass).Controls.Add((Control)(object)tbGlassBlend);
		((Control)panelGlass).Controls.Add((Control)(object)labelBlend);
		((Control)panelGlass).Controls.Add((Control)(object)labelGlassColor);
		((Control)panelGlass).Dock = (DockStyle)1;
		((Control)panelGlass).Location = new Point(0, 388);
		((Control)panelGlass).Name = "panelGlass";
		((Control)panelGlass).Size = new Size(264, 88);
		((Control)panelGlass).TabIndex = 4;
		panelGlass.TabStop = true;
		((Control)colorPickerGlassColor).AccessibleRole = (AccessibleRole)43;
		colorPickerGlassColor.AutoExpandOnClick = true;
		colorPickerGlassColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerGlassColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerGlassColor).Location = new Point(0, 6);
		colorPickerGlassColor.LockClosePopup = false;
		((Control)colorPickerGlassColor).Name = "colorPickerGlassColor";
		((Control)colorPickerGlassColor).Size = new Size(125, 24);
		((Control)colorPickerGlassColor).TabIndex = 30;
		((Control)colorPickerGlassColor).TabStop = false;
		colorPickerGlassColor.SelectedColorChanged += GlassChange;
		((Control)checkBoxGlassDrawHatch).Location = new Point(0, 59);
		((Control)checkBoxGlassDrawHatch).Name = "checkBoxGlassDrawHatch";
		((Control)checkBoxGlassDrawHatch).Size = new Size(261, 24);
		((Control)checkBoxGlassDrawHatch).TabIndex = 2;
		((ButtonBase)checkBoxGlassDrawHatch).UseVisualStyleBackColor = true;
		checkBoxGlassDrawHatch.CheckedChanged += GlassChange;
		((Control)tbGlassBlend).Location = new Point(0, 36);
		((Control)tbGlassBlend).Name = "tbGlassBlend";
		((Control)tbGlassBlend).Size = new Size(125, 21);
		((Control)tbGlassBlend).TabIndex = 1;
		tbGlassBlend.TextAlign = (HorizontalAlignment)1;
		((Control)tbGlassBlend).TextChanged += GlassChange;
		((Control)labelBlend).ForeColor = Color.DimGray;
		((Control)labelBlend).Location = new Point(136, 35);
		((Control)labelBlend).Name = "labelBlend";
		((Control)labelBlend).Size = new Size(125, 21);
		((Control)labelBlend).TabIndex = 32;
		((Control)labelBlend).Text = "Blend";
		((Label)labelBlend).TextAlign = (ContentAlignment)16;
		((Label)labelBlend).UseCompatibleTextRendering = true;
		((Control)labelGlassColor).ForeColor = Color.DimGray;
		((Control)labelGlassColor).Location = new Point(136, 6);
		((Control)labelGlassColor).Name = "labelGlassColor";
		((Control)labelGlassColor).Size = new Size(125, 21);
		((Control)labelGlassColor).TabIndex = 30;
		((Control)labelGlassColor).Text = "Color";
		((Label)labelGlassColor).TextAlign = (ContentAlignment)16;
		((Label)labelGlassColor).UseCompatibleTextRendering = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelGlass);
		((Control)this).Controls.Add((Control)(object)panelGlare);
		((Control)this).Controls.Add((Control)(object)panelGradient);
		((Control)this).Controls.Add((Control)(object)panelHatch);
		((Control)this).Controls.Add((Control)(object)panelSolid);
		((Control)this).Controls.Add((Control)(object)panelSelectBrush);
		((Control)this).Name = "StiBrushControl";
		((Control)this).Size = new Size(264, 491);
		((UserControl)this).Load += This_Load;
		((Control)panelSelectBrush).ResumeLayout(false);
		((Control)panelSolid).ResumeLayout(false);
		((Control)panelHatch).ResumeLayout(false);
		((Control)panelGradient).ResumeLayout(false);
		((ISupportInitialize)numericUpDownGradientAngle).EndInit();
		((Control)panelGlare).ResumeLayout(false);
		((ISupportInitialize)numericUpDownGlareAngle).EndInit();
		((Control)panelGlass).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
