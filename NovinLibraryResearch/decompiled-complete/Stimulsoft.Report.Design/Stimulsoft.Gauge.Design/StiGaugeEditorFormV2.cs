using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;

namespace Stimulsoft.Gauge.Design;

public class StiGaugeEditorFormV2 : StiInplaceToolForm
{
	private StiGauge gauge;

	private StiGauge cloneGauge;

	private bool dialogResult;

	private bool lockInit;

	private IContainer components;

	private Panel panelMode;

	private ToolTip toolTip;

	private Label labelCalculationMode;

	private StiNumericUpDown numericMaximum;

	private StiNumericUpDown numericMinimum;

	private Label labelMaximum;

	private Label labelMinimum;

	private Label label1;

	private StiComboBox comboBoxCalculationMode;

	private Label labelValue;

	private Panel panelType;

	private Label labelType;

	private StiOperationRadioButton buttonHalfCircular;

	private StiOperationRadioButton buttonFullCircular;

	private StiOperationRadioButton buttonLinear;

	private StiOperationRadioButton buttonHorizontalLinear;

	private StiOperationRadioButton buttonBullet;

	private Panel panelValue;

	private StiGroupLine panelFooter;

	private Button btOk;

	private Button btCancel;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	public override bool AllowResize => false;

	internal StiDesignerControl DesignerControl { get; private set; }

	private string IndicatorColumn
	{
		get
		{
			StiIndicatorBase stiIndicatorBase = gauge.Scales[0].Items.ToArray().FirstOrDefault((StiGaugeElement x) => x is StiIndicatorBase) as StiIndicatorBase;
			return stiIndicatorBase.Value.Value;
		}
		set
		{
			StiIndicatorBase stiIndicatorBase = gauge.Scales[0].Items.ToArray().FirstOrDefault((StiGaugeElement x) => x is StiIndicatorBase) as StiIndicatorBase;
			stiIndicatorBase.Value.Value = value;
		}
	}

	public StiGaugeEditorFormV2(StiGauge gauge)
	{
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		((Control)this).DoubleBuffered = true;
		lockInit = true;
		InitializeComponent();
		this.gauge = gauge;
		cloneGauge = (StiGauge)gauge.Clone();
		DesignerControl = gauge.Report.Designer.DesignerControl as StiDesignerControl;
		if (!((Control)this).MinimumSize.IsEmpty)
		{
			((Form)this).Size = ((Control)this).MinimumSize;
		}
		Localize();
		if (!StiGaugeV2InitHelper.IsGaugeV2(this.gauge))
		{
			StiGaugeV2InitHelper.Init(this.gauge, this.gauge.Type, addRange: true, skipText: false);
		}
		numericMinimum.Value = this.gauge.Minimum;
		numericMaximum.Value = this.gauge.Maximum;
		((ListControl)comboBoxCalculationMode).SelectedIndex = ((this.gauge.CalculationMode != StiGaugeCalculationMode.Auto) ? 1 : 0);
		if (this.gauge.Type == StiGaugeType.FullCircular)
		{
			((RadioButton)buttonFullCircular).Checked = true;
		}
		else if (this.gauge.Type == StiGaugeType.HalfCircular)
		{
			((RadioButton)buttonHalfCircular).Checked = true;
		}
		else if (this.gauge.Type == StiGaugeType.Bullet)
		{
			((RadioButton)buttonBullet).Checked = true;
		}
		else if (this.gauge.Type == StiGaugeType.HorizontalLinear)
		{
			((RadioButton)buttonHorizontalLinear).Checked = true;
		}
		else
		{
			((RadioButton)buttonLinear).Checked = true;
		}
		lockInit = false;
		string indicatorColumn = IndicatorColumn;
		if (!string.IsNullOrEmpty(indicatorColumn))
		{
			((Control)textBoxExpression).Text = indicatorColumn;
		}
		((Control)textBoxExpression).LostFocus += TextBoxExpression_LostFocus;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		CalculateWindowHeight();
		((Control)textBoxExpression).Focus();
		DoScaling();
	}

	public override void Localize()
	{
		((Control)this).Text = Loc.Get("Components", "StiGauge");
		((Control)labelValue).Text = Loc.Get("PropertyMain", "Value");
		((Control)labelType).Text = Loc.Get("PropertyMain", "Type");
		((Control)labelCalculationMode).Text = Loc.Get("PropertyMain", "Mode");
		((Control)labelMinimum).Text = (Loc.IsCyrillic ? Loc.GetEnum("StiFilterItemValueLow") : Loc.GetMain("Minimum"));
		((Control)labelMaximum).Text = (Loc.IsCyrillic ? Loc.GetEnum("StiFilterItemValueHigh") : Loc.GetMain("Maximum"));
		toolTip.SetToolTip((Control)(object)buttonFullCircular, Loc.Get("PropertyEnum", "StiGaugeTypeFullCircular"));
		toolTip.SetToolTip((Control)(object)buttonHalfCircular, Loc.Get("PropertyEnum", "StiGaugeTypeHalfCircular"));
		toolTip.SetToolTip((Control)(object)buttonLinear, Loc.Get("PropertyEnum", "StiGaugeTypeLinear"));
		toolTip.SetToolTip((Control)(object)buttonHorizontalLinear, Loc.GetEnum("StiGaugeTypeHorizontalLinear"));
		toolTip.SetToolTip((Control)(object)buttonBullet, Loc.GetEnum("StiGaugeTypeBullet"));
		((ComboBox)comboBoxCalculationMode).Items.Add((object)Loc.Get("PropertyEnum", "StiGaugeCalculationModeAuto"));
		((ComboBox)comboBoxCalculationMode).Items.Add((object)Loc.Get("PropertyEnum", "StiGaugeCalculationModeCustom"));
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", string.Empty);
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
	}

	private void DoScaling()
	{
		((ButtonBase)buttonExpressionMenu).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonFullCircular).Image = StiReportDesignImages.Gauge.RadialBar();
		((ButtonBase)buttonHalfCircular).Image = StiReportDesignImages.Gauge.HalfDonuts();
		((ButtonBase)buttonLinear).Image = StiReportDesignImages.Gauge.VerticalLinear();
		((ButtonBase)buttonHorizontalLinear).Image = StiReportDesignImages.Gauge.HorizontalLinear();
		((ButtonBase)buttonBullet).Image = StiReportDesignImages.Gauge.Bullet();
	}

	private void UpdateFormPosition()
	{
		if (DesignerControl != null && gauge != null)
		{
			Size size = ((Form)this).Size;
			Rectangle rectangle = Stimulsoft.Report.Design.StiConvert.RectangleToScreen(DesignerControl, gauge.GetDisplayRectangle());
			rectangle = ((Control)DesignerControl.PageView.GetCurrentPageView().View).RectangleToScreen(rectangle);
			Point location = ((rectangle.Right + StiInplaceEditor.ButtonMargin + StiInplaceEditor.ButtonSize > Screen.PrimaryScreen.Bounds.Right) ? new Point(rectangle.Left - size.Width - StiScale.I2, rectangle.Top) : new Point(rectangle.Right + StiScale.I2, rectangle.Top));
			if (location.Y + size.Height > Screen.PrimaryScreen.Bounds.Bottom)
			{
				location.Y = Screen.PrimaryScreen.Bounds.Bottom - StiScale.I20 - size.Height;
			}
			((Form)this).Location = location;
		}
	}

	protected override void CalculateWindowHeight()
	{
		int height = ((Control)panelFooter).Height + ((Control)panelType).Height + ((Control)panelMode).Height + ((Control)panelValue).Height;
		((Form)this).ClientSize = new Size(StiScale.XXI(302.0), height);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		UpdateFormPosition();
	}

	private void This_Load(object sender, EventArgs e)
	{
		((Control)panelBoxExpression).Focus();
		((Control)textBoxExpression).Focus();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_Closed(object sender, FormClosedEventArgs e)
	{
		if (dialogResult)
		{
			return;
		}
		StiContainer parent = gauge.Parent;
		if (parent != null)
		{
			int num = parent.Components.IndexOf(gauge);
			if (num != -1)
			{
				parent.Components.RemoveAt(num);
				parent.Components.Insert(num, cloneGauge);
				cloneGauge.Mode = StiScaleMode.V2;
			}
			((Control)DesignerControl).Refresh();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		dialogResult = true;
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
			if (IndicatorColumn != ((Control)textBoxExpression).Text)
			{
				IndicatorColumn = ((Control)textBoxExpression).Text;
				((Control)DesignerControl).Refresh();
			}
		}
	}

	private void TextBoxExpression_LostFocus(object sender, EventArgs e)
	{
		if (IndicatorColumn != ((Control)textBoxExpression).Text)
		{
			IndicatorColumn = ((Control)textBoxExpression).Text;
			((Control)DesignerControl).Refresh();
		}
	}

	private void NumericMinimum_ValueChanged(object sender, EventArgs e)
	{
		gauge.Minimum = numericMinimum.Value;
		((Control)DesignerControl).Refresh();
	}

	private void NumericMaximum_ValueChanged(object sender, EventArgs e)
	{
		gauge.Maximum = numericMaximum.Value;
		((Control)DesignerControl).Refresh();
	}

	private void ComboBoxCalculationMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		StiComboBox stiComboBox = comboBoxCalculationMode;
		bool flag = (string)((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) == Loc.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
		gauge.CalculationMode = (flag ? StiGaugeCalculationMode.Auto : StiGaugeCalculationMode.Custom);
		Panel obj = panelMode;
		int height;
		if (flag)
		{
			int num = ((Control)comboBoxCalculationMode).Bottom + StiScale.I12;
			Padding padding = ((Control)this).Padding;
			height = num + ((Padding)(ref padding)).Bottom;
		}
		else
		{
			height = ((Control)numericMaximum).Bottom + StiScale.I12;
		}
		((Control)obj).Height = height;
		((Control)numericMaximum).Visible = !flag;
		((Control)numericMinimum).Visible = !flag;
		((Control)labelMaximum).Visible = !flag;
		((Control)labelMinimum).Visible = !flag;
		StiScaleBase stiScaleBase = gauge.Scales[0];
		if (flag)
		{
			stiScaleBase.Minimum = 0f;
			switch (gauge.Type)
			{
			case StiGaugeType.FullCircular:
				stiScaleBase.Maximum = 180f;
				break;
			case StiGaugeType.HalfCircular:
				stiScaleBase.Maximum = 150f;
				break;
			case StiGaugeType.Linear:
			case StiGaugeType.HorizontalLinear:
				stiScaleBase.Maximum = 100f;
				break;
			}
		}
		else
		{
			StiGaugeV2InitHelper.GetScaleMinMax(gauge.Type, out var min, out var max);
			stiScaleBase.Minimum = min;
			stiScaleBase.Maximum = max;
		}
		CalculateWindowHeight();
		((Control)DesignerControl).Refresh();
	}

	private void ButtonGaugeType_CheckedChanged(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		if (lockInit)
		{
			return;
		}
		RadioButton val = (RadioButton)sender;
		if (val.Checked)
		{
			StiGaugeType type = StiGaugeType.FullCircular;
			if (((RadioButton)buttonHalfCircular).Checked)
			{
				type = StiGaugeType.HalfCircular;
			}
			else if (((RadioButton)buttonLinear).Checked)
			{
				type = StiGaugeType.Linear;
			}
			else if (((RadioButton)buttonHorizontalLinear).Checked)
			{
				type = StiGaugeType.HorizontalLinear;
			}
			else if (((RadioButton)buttonBullet).Checked)
			{
				type = StiGaugeType.Bullet;
			}
			gauge.Type = type;
			StiGaugeV2InitHelper.Init(gauge, type, addRange: true, skipText: false);
			IndicatorColumn = ((Control)textBoxExpression).Text;
			StiComboBox stiComboBox = comboBoxCalculationMode;
			bool flag = (string)((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) == Loc.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
			gauge.CalculationMode = (flag ? StiGaugeCalculationMode.Auto : StiGaugeCalculationMode.Custom);
			if (flag)
			{
				StiGaugeV2InitHelper.GetScaleMinMax(gauge.Type, out var min, out var max);
				gauge.Minimum = (decimal)min;
				gauge.Maximum = (decimal)max;
			}
			else
			{
				gauge.Minimum = numericMinimum.Value;
				gauge.Maximum = numericMaximum.Value;
			}
			((Control)DesignerControl).Refresh();
		}
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxExpression).Text;
		DialogResult val = StiTextEditor.EditText(gauge.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1 && ((Control)textBoxExpression).Text != text)
		{
			((Control)textBoxExpression).Text = text;
			IndicatorColumn = text;
			((Control)textBoxExpression).Focus();
			((TextBoxBase)textBoxExpression).Select(0, 0);
		}
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Expected O, but got Unknown
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Expected O, but got Unknown
		//IL_09b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Expected O, but got Unknown
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b12: Expected O, but got Unknown
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Expected O, but got Unknown
		//IL_0bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Expected O, but got Unknown
		//IL_0e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1127: Unknown result type (might be due to invalid IL or missing references)
		//IL_1184: Unknown result type (might be due to invalid IL or missing references)
		//IL_118e: Expected O, but got Unknown
		//IL_11a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b2: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiGaugeEditorFormV2));
		panelMode = new Panel();
		label1 = new Label();
		numericMaximum = new StiNumericUpDown();
		numericMinimum = new StiNumericUpDown();
		labelMaximum = new Label();
		labelMinimum = new Label();
		labelCalculationMode = new Label();
		comboBoxCalculationMode = new StiComboBox();
		toolTip = new ToolTip(components);
		labelValue = new Label();
		panelType = new Panel();
		labelType = new Label();
		buttonLinear = new StiOperationRadioButton();
		buttonHorizontalLinear = new StiOperationRadioButton();
		buttonBullet = new StiOperationRadioButton();
		buttonHalfCircular = new StiOperationRadioButton();
		buttonFullCircular = new StiOperationRadioButton();
		panelValue = new Panel();
		panelBoxExpression = new StiPanelBox();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		panelFooter = new StiGroupLine();
		btOk = new Button();
		btCancel = new Button();
		((Control)panelMode).SuspendLayout();
		((ISupportInitialize)numericMaximum).BeginInit();
		((ISupportInitialize)numericMinimum).BeginInit();
		((Control)panelType).SuspendLayout();
		((Control)panelValue).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)panelFooter).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelMode).BackColor = Color.White;
		((Control)panelMode).Controls.Add((Control)(object)label1);
		((Control)panelMode).Controls.Add((Control)(object)numericMaximum);
		((Control)panelMode).Controls.Add((Control)(object)numericMinimum);
		((Control)panelMode).Controls.Add((Control)(object)labelMaximum);
		((Control)panelMode).Controls.Add((Control)(object)labelMinimum);
		((Control)panelMode).Controls.Add((Control)(object)labelCalculationMode);
		((Control)panelMode).Controls.Add((Control)(object)comboBoxCalculationMode);
		((Control)panelMode).Dock = (DockStyle)1;
		((Control)panelMode).Location = new Point(0, 99);
		((Control)panelMode).Margin = new Padding(2);
		((Control)panelMode).Name = "panelMode";
		((Control)panelMode).Size = new Size(289, 83);
		((Control)panelMode).TabIndex = 2;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)label1).Location = new Point(183, 59);
		((Control)label1).Margin = new Padding(2, 0, 2, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(13, 20);
		((Control)label1).TabIndex = 6;
		((Control)label1).Text = "-";
		label1.TextAlign = (ContentAlignment)32;
		((Control)numericMaximum).Location = new Point(200, 60);
		((Control)numericMaximum).Margin = new Padding(2);
		numericMaximum.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numericMaximum.Minimum = new decimal(new int[4] { 10000000, 0, 0, -2147483648 });
		((Control)numericMaximum).Name = "numericMaximum";
		((Control)numericMaximum).Size = new Size(75, 20);
		((Control)numericMaximum).TabIndex = 5;
		numericMaximum.ValueChanged += NumericMaximum_ValueChanged;
		((Control)numericMinimum).Location = new Point(100, 60);
		((Control)numericMinimum).Margin = new Padding(2);
		numericMinimum.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numericMinimum.Minimum = new decimal(new int[4] { 10000000, 0, 0, -2147483648 });
		((Control)numericMinimum).Name = "numericMinimum";
		((Control)numericMinimum).Size = new Size(79, 20);
		((Control)numericMinimum).TabIndex = 4;
		numericMinimum.ValueChanged += NumericMinimum_ValueChanged;
		((Control)labelMaximum).ForeColor = Color.DimGray;
		((Control)labelMaximum).Location = new Point(199, 36);
		((Control)labelMaximum).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMaximum).Name = "labelMaximum";
		((Control)labelMaximum).Size = new Size(79, 17);
		((Control)labelMaximum).TabIndex = 3;
		((Control)labelMaximum).Text = "Maximum";
		labelMaximum.TextAlign = (ContentAlignment)16;
		((Control)labelMinimum).ForeColor = Color.DimGray;
		((Control)labelMinimum).Location = new Point(99, 36);
		((Control)labelMinimum).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMinimum).Name = "labelMinimum";
		((Control)labelMinimum).Size = new Size(79, 17);
		((Control)labelMinimum).TabIndex = 2;
		((Control)labelMinimum).Text = "Minimum";
		labelMinimum.TextAlign = (ContentAlignment)16;
		((Control)labelCalculationMode).ForeColor = Color.DimGray;
		((Control)labelCalculationMode).Location = new Point(10, 9);
		((Control)labelCalculationMode).Margin = new Padding(2, 0, 2, 0);
		((Control)labelCalculationMode).Name = "labelCalculationMode";
		((Control)labelCalculationMode).Size = new Size(85, 17);
		((Control)labelCalculationMode).TabIndex = 1;
		((Control)labelCalculationMode).Text = "Mode";
		labelCalculationMode.TextAlign = (ContentAlignment)16;
		((Control)comboBoxCalculationMode).Anchor = (AnchorStyles)13;
		comboBoxCalculationMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxCalculationMode).FormattingEnabled = true;
		((Control)comboBoxCalculationMode).Location = new Point(100, 7);
		((Control)comboBoxCalculationMode).Margin = new Padding(2);
		((Control)comboBoxCalculationMode).Name = "comboBoxCalculationMode";
		((Control)comboBoxCalculationMode).Size = new Size(143, 21);
		((Control)comboBoxCalculationMode).TabIndex = 1;
		((ComboBox)comboBoxCalculationMode).SelectedIndexChanged += ComboBoxCalculationMode_SelectedIndexChanged;
		((Control)labelValue).ForeColor = Color.DimGray;
		((Control)labelValue).Location = new Point(11, 11);
		((Control)labelValue).Margin = new Padding(2, 0, 2, 0);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(248, 24);
		((Control)labelValue).TabIndex = 0;
		((Control)labelValue).Text = "Value";
		((Control)panelType).BackColor = Color.White;
		((Control)panelType).Controls.Add((Control)(object)labelType);
		((Control)panelType).Controls.Add((Control)(object)buttonBullet);
		((Control)panelType).Controls.Add((Control)(object)buttonHorizontalLinear);
		((Control)panelType).Controls.Add((Control)(object)buttonLinear);
		((Control)panelType).Controls.Add((Control)(object)buttonHalfCircular);
		((Control)panelType).Controls.Add((Control)(object)buttonFullCircular);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(0, 63);
		((Control)panelType).Margin = new Padding(2);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(289, 36);
		((Control)panelType).TabIndex = 1;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(10, 6);
		((Control)labelType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(82, 25);
		((Control)labelType).TabIndex = 1;
		((Control)labelType).Text = "Type";
		labelType.TextAlign = (ContentAlignment)16;
		((RadioButton)buttonLinear).Appearance = (Appearance)1;
		((ButtonBase)buttonLinear).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonLinear).Image = (Image)componentResourceManager.GetObject("buttonLinear.Image");
		((Control)buttonLinear).Location = new Point(159, 6);
		((Control)buttonLinear).Margin = new Padding(2);
		((Control)buttonLinear).Name = "buttonLinear";
		((Control)buttonLinear).Size = new Size(26, 26);
		((Control)buttonLinear).TabIndex = 0;
		((RadioButton)buttonLinear).TabStop = true;
		((ButtonBase)buttonLinear).UseVisualStyleBackColor = true;
		((RadioButton)buttonLinear).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((RadioButton)buttonHorizontalLinear).Appearance = (Appearance)1;
		((ButtonBase)buttonHorizontalLinear).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonHorizontalLinear).Image = (Image)componentResourceManager.GetObject("buttonHorizontalLinear.Image");
		((Control)buttonHorizontalLinear).Location = new Point(190, 6);
		((Control)buttonHorizontalLinear).Margin = new Padding(2);
		((Control)buttonHorizontalLinear).Name = "buttonHorizontalLinear";
		((Control)buttonHorizontalLinear).Size = new Size(26, 26);
		((Control)buttonHorizontalLinear).TabIndex = 0;
		((RadioButton)buttonHorizontalLinear).TabStop = true;
		((ButtonBase)buttonHorizontalLinear).UseVisualStyleBackColor = true;
		((RadioButton)buttonHorizontalLinear).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((RadioButton)buttonBullet).Appearance = (Appearance)1;
		((ButtonBase)buttonBullet).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonBullet).Image = (Image)componentResourceManager.GetObject("buttonBullet.Image");
		((Control)buttonBullet).Location = new Point(221, 6);
		((Control)buttonBullet).Margin = new Padding(2);
		((Control)buttonBullet).Name = "buttonBullet";
		((Control)buttonBullet).Size = new Size(26, 26);
		((Control)buttonBullet).TabIndex = 0;
		((RadioButton)buttonBullet).TabStop = true;
		((ButtonBase)buttonBullet).UseVisualStyleBackColor = true;
		((RadioButton)buttonBullet).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((RadioButton)buttonHalfCircular).Appearance = (Appearance)1;
		((ButtonBase)buttonHalfCircular).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonHalfCircular).Image = (Image)componentResourceManager.GetObject("buttonHalfCircular.Image");
		((Control)buttonHalfCircular).Location = new Point(129, 6);
		((Control)buttonHalfCircular).Margin = new Padding(2);
		((Control)buttonHalfCircular).Name = "buttonHalfCircular";
		((Control)buttonHalfCircular).Size = new Size(26, 26);
		((Control)buttonHalfCircular).TabIndex = 1;
		((ButtonBase)buttonHalfCircular).UseVisualStyleBackColor = true;
		((RadioButton)buttonHalfCircular).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((RadioButton)buttonFullCircular).Appearance = (Appearance)1;
		((ButtonBase)buttonFullCircular).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonFullCircular).Image = (Image)componentResourceManager.GetObject("buttonFullCircular.Image");
		((Control)buttonFullCircular).Location = new Point(99, 6);
		((Control)buttonFullCircular).Margin = new Padding(2);
		((Control)buttonFullCircular).Name = "buttonFullCircular";
		((Control)buttonFullCircular).Size = new Size(26, 26);
		((Control)buttonFullCircular).TabIndex = 0;
		((RadioButton)buttonFullCircular).TabStop = true;
		((ButtonBase)buttonFullCircular).UseVisualStyleBackColor = true;
		((RadioButton)buttonFullCircular).CheckedChanged += ButtonGaugeType_CheckedChanged;
		((Control)panelValue).BackColor = Color.White;
		((Control)panelValue).Controls.Add((Control)(object)panelBoxExpression);
		((Control)panelValue).Controls.Add((Control)(object)labelValue);
		((Control)panelValue).Dock = (DockStyle)1;
		((Control)panelValue).Location = new Point(0, 0);
		((Control)panelValue).Margin = new Padding(2);
		((Control)panelValue).Name = "panelValue";
		((Control)panelValue).Size = new Size(289, 63);
		((Control)panelValue).TabIndex = 0;
		((Control)panelBoxExpression).Anchor = (AnchorStyles)13;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Location = new Point(12, 33);
		((Control)panelBoxExpression).Margin = new Padding(2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(265, 25);
		((Control)panelBoxExpression).TabIndex = 1;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Margin = new Padding(2);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(235, 13);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionMenu).Image = (Image)componentResourceManager.GetObject("buttonExpressionMenu.Image");
		((Control)buttonExpressionMenu).Location = new Point(241, 1);
		((Control)buttonExpressionMenu).Margin = new Padding(2);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(23, 23);
		((Control)buttonExpressionMenu).TabIndex = 1;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)btOk);
		((Control)panelFooter).Controls.Add((Control)(object)btCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 285);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(289, 46);
		((Control)panelFooter).TabIndex = 3;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(119, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(200, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(289, 331);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Control)this).Controls.Add((Control)(object)panelMode);
		((Control)this).Controls.Add((Control)(object)panelType);
		((Control)this).Controls.Add((Control)(object)panelValue);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(2);
		((Control)this).MinimumSize = new Size(339, 56);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiGaugeEditorFormV2";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Control)this).Text = "Gauge";
		((Form)this).FormClosed += new FormClosedEventHandler(This_Closed);
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panelMode).ResumeLayout(false);
		((ISupportInitialize)numericMaximum).EndInit();
		((ISupportInitialize)numericMinimum).EndInit();
		((Control)panelType).ResumeLayout(false);
		((Control)panelValue).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)panelFooter).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
