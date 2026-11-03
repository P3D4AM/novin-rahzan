using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace FormCustom.UC;

public class UcNumberPanel : UserControl
{
	private NumberPanelType _type;

	private IContainer components;

	internal GroupBox GroupBox1;

	private PanelEx panelEx1;

	private Label lblSixZero;

	private PanelEx panelEx2;

	private Label lblThreeZero;

	private PanelEx pnlHeader;

	private Label lblTowZero;

	private PanelEx panelEx3;

	private Label lblDecimalPoint;

	private PanelEx panelEx5;

	private Label lbl0;

	private PanelEx panelEx14;

	private Label lblMinus1P;

	private PanelEx panelEx15;

	private Label lblPlus1P;

	private PanelEx panelEx16;

	private Label lblMinus1;

	private PanelEx panelEx17;

	private Label lblPlus1;

	private PanelEx panelEx11;

	private Label lbl9;

	private PanelEx panelEx12;

	private Label lbl8;

	private PanelEx panelEx13;

	private Label lbl7;

	private PanelEx panelEx8;

	private Label lbl6;

	private PanelEx panelEx9;

	private Label lbl5;

	private PanelEx panelEx10;

	private Label lbl4;

	private PanelEx panelEx4;

	private Label lbl3;

	private PanelEx panelEx6;

	private Label lbl2;

	private PanelEx panelEx7;

	private Label lbl1;

	private PanelEx panelEx18;

	private Label lblBackSpace;

	public NumberPanelType PanelType
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
			if (_type == NumberPanelType.Quantity)
			{
				Label obj = lblTowZero;
				Label obj2 = lblThreeZero;
				bool flag2 = (((Control)lblSixZero).Visible = false);
				bool visible = (((Control)obj2).Visible = flag2);
				((Control)obj).Visible = visible;
				PanelEx panelEx = pnlHeader;
				PanelEx panelEx2 = this.panelEx2;
				flag2 = (((Control)panelEx1).Visible = false);
				visible = (((Control)panelEx2).Visible = flag2);
				((Control)panelEx).Visible = visible;
				Label obj3 = lblPlus1P;
				visible = (((Control)lblMinus1P).Visible = true);
				((Control)obj3).Visible = visible;
				PanelEx panelEx3 = panelEx15;
				visible = (((Control)panelEx14).Visible = true);
				((Control)panelEx3).Visible = visible;
			}
			else
			{
				Label obj4 = lblTowZero;
				Label obj5 = lblThreeZero;
				bool flag2 = (((Control)lblSixZero).Visible = true);
				bool visible = (((Control)obj5).Visible = flag2);
				((Control)obj4).Visible = visible;
				PanelEx panelEx4 = pnlHeader;
				PanelEx panelEx5 = this.panelEx2;
				flag2 = (((Control)panelEx1).Visible = true);
				visible = (((Control)panelEx5).Visible = flag2);
				((Control)panelEx4).Visible = visible;
				Label obj6 = lblPlus1P;
				visible = (((Control)lblMinus1P).Visible = false);
				((Control)obj6).Visible = visible;
				PanelEx panelEx6 = panelEx15;
				visible = (((Control)panelEx14).Visible = false);
				((Control)panelEx6).Visible = visible;
			}
		}
	}

	public event Func<string, EventArgs, Task> OnButtonClick;

	public event Func<object, EventArgs, Task> OnClear;

	public event Func<double, EventArgs, Task> OnIncrease;

	public event Func<double, EventArgs, Task> OnDecrease;

	public UcNumberPanel()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void btnBackSpace_Click(object sender, EventArgs e)
	{
		this.OnClear?.Invoke(sender, e);
	}

	private void btnMinus1P_Click(object sender, EventArgs e)
	{
		this.OnDecrease?.Invoke(0.1, e);
	}

	private void btnPlus1P_Click(object sender, EventArgs e)
	{
		this.OnIncrease?.Invoke(0.1, e);
	}

	private void btnMinus1_Click(object sender, EventArgs e)
	{
		this.OnDecrease?.Invoke(1.0, e);
	}

	private void btnPlus1_Click(object sender, EventArgs e)
	{
		this.OnIncrease?.Invoke(1.0, e);
	}

	private void btn9_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("9", e);
	}

	private void btn8_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("8", e);
	}

	private void btn7_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("7", e);
	}

	private void btn6_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("6", e);
	}

	private void btn5_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("5", e);
	}

	private void btn4_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("4", e);
	}

	private void btn3_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("3", e);
	}

	private void btn2_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("2", e);
	}

	private void btn1_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("1", e);
	}

	private void btnDecimalPoint_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("/", e);
	}

	private void btn0_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("0", e);
	}

	private void btnTowZero_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("00", e);
	}

	private void btnThreeZero_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("000", e);
	}

	private void btnSixZero_Click(object sender, EventArgs e)
	{
		this.OnButtonClick?.Invoke("000000", e);
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Expected O, but got Unknown
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Expected O, but got Unknown
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0980: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab8: Expected O, but got Unknown
		//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Expected O, but got Unknown
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f24: Expected O, but got Unknown
		//IL_0f59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Unknown result type (might be due to invalid IL or missing references)
		//IL_1152: Unknown result type (might be due to invalid IL or missing references)
		//IL_115c: Expected O, but got Unknown
		//IL_1191: Unknown result type (might be due to invalid IL or missing references)
		//IL_125c: Unknown result type (might be due to invalid IL or missing references)
		//IL_138a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1394: Expected O, but got Unknown
		//IL_13c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1490: Unknown result type (might be due to invalid IL or missing references)
		//IL_15be: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c8: Expected O, but got Unknown
		//IL_15fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1800: Expected O, but got Unknown
		//IL_1835: Unknown result type (might be due to invalid IL or missing references)
		//IL_1900: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a38: Expected O, but got Unknown
		//IL_1a6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6c: Expected O, but got Unknown
		//IL_1ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea1: Expected O, but got Unknown
		//IL_1ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_20cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d6: Expected O, but got Unknown
		//IL_210b: Unknown result type (might be due to invalid IL or missing references)
		//IL_21cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_22fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2307: Expected O, but got Unknown
		//IL_233c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2404: Unknown result type (might be due to invalid IL or missing references)
		//IL_2532: Unknown result type (might be due to invalid IL or missing references)
		//IL_253c: Expected O, but got Unknown
		//IL_2571: Unknown result type (might be due to invalid IL or missing references)
		//IL_2635: Unknown result type (might be due to invalid IL or missing references)
		//IL_2763: Unknown result type (might be due to invalid IL or missing references)
		//IL_276d: Expected O, but got Unknown
		//IL_27a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_286a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2998: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a2: Expected O, but got Unknown
		//IL_29d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd3: Expected O, but got Unknown
		//IL_2c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e08: Expected O, but got Unknown
		//IL_2e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef1: Expected O, but got Unknown
		//IL_2ef6: Unknown result type (might be due to invalid IL or missing references)
		GroupBox1 = new GroupBox();
		pnlHeader = new PanelEx();
		lblTowZero = new Label();
		panelEx1 = new PanelEx();
		lblSixZero = new Label();
		panelEx2 = new PanelEx();
		lblThreeZero = new Label();
		panelEx3 = new PanelEx();
		lblDecimalPoint = new Label();
		panelEx5 = new PanelEx();
		lbl0 = new Label();
		panelEx4 = new PanelEx();
		lbl3 = new Label();
		panelEx6 = new PanelEx();
		lbl2 = new Label();
		panelEx7 = new PanelEx();
		lbl1 = new Label();
		panelEx8 = new PanelEx();
		lbl6 = new Label();
		panelEx9 = new PanelEx();
		lbl5 = new Label();
		panelEx10 = new PanelEx();
		lbl4 = new Label();
		panelEx11 = new PanelEx();
		lbl9 = new Label();
		panelEx12 = new PanelEx();
		lbl8 = new Label();
		panelEx13 = new PanelEx();
		lbl7 = new Label();
		panelEx14 = new PanelEx();
		lblMinus1P = new Label();
		panelEx15 = new PanelEx();
		lblPlus1P = new Label();
		panelEx16 = new PanelEx();
		lblMinus1 = new Label();
		panelEx17 = new PanelEx();
		lblPlus1 = new Label();
		panelEx18 = new PanelEx();
		lblBackSpace = new Label();
		((Control)GroupBox1).SuspendLayout();
		((Control)pnlHeader).SuspendLayout();
		((Control)panelEx1).SuspendLayout();
		((Control)panelEx2).SuspendLayout();
		((Control)panelEx3).SuspendLayout();
		((Control)panelEx5).SuspendLayout();
		((Control)panelEx4).SuspendLayout();
		((Control)panelEx6).SuspendLayout();
		((Control)panelEx7).SuspendLayout();
		((Control)panelEx8).SuspendLayout();
		((Control)panelEx9).SuspendLayout();
		((Control)panelEx10).SuspendLayout();
		((Control)panelEx11).SuspendLayout();
		((Control)panelEx12).SuspendLayout();
		((Control)panelEx13).SuspendLayout();
		((Control)panelEx14).SuspendLayout();
		((Control)panelEx15).SuspendLayout();
		((Control)panelEx16).SuspendLayout();
		((Control)panelEx17).SuspendLayout();
		((Control)panelEx18).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)GroupBox1).Anchor = (AnchorStyles)15;
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx18);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx14);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx15);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx16);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx17);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx11);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx12);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx13);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx8);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx9);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx10);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx4);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx6);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx7);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx3);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx5);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx1);
		((Control)GroupBox1).Controls.Add((Control)(object)panelEx2);
		((Control)GroupBox1).Controls.Add((Control)(object)pnlHeader);
		((Control)GroupBox1).Location = new Point(4, 3);
		((Control)GroupBox1).Margin = new Padding(4, 3, 4, 3);
		((Control)GroupBox1).Name = "GroupBox1";
		((Control)GroupBox1).Padding = new Padding(4, 3, 4, 3);
		((Control)GroupBox1).Size = new Size(426, 329);
		((Control)GroupBox1).TabIndex = 0;
		GroupBox1.TabStop = false;
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)lblTowZero);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Location = new Point(5, 281);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(133, 39);
		pnlHeader.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		pnlHeader.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		pnlHeader.Style.BorderWidth = 2;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 135;
		((Control)lblTowZero).Cursor = Cursors.Hand;
		((Control)lblTowZero).Dock = (DockStyle)5;
		((Control)lblTowZero).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTowZero).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblTowZero).Location = new Point(0, 0);
		((Control)lblTowZero).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTowZero).Name = "lblTowZero";
		((Control)lblTowZero).Size = new Size(133, 39);
		((Control)lblTowZero).TabIndex = 0;
		((Control)lblTowZero).Text = "00";
		lblTowZero.TextAlign = (ContentAlignment)32;
		((Control)lblTowZero).Click += btnTowZero_Click;
		panelEx1.CanvasColor = Color.Transparent;
		((Control)panelEx1).Controls.Add((Control)(object)lblSixZero);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(286, 281);
		((Control)panelEx1).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(133, 39);
		panelEx1.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx1.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx1.Style.Border = eBorderType.SingleLine;
		panelEx1.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx1.Style.BorderWidth = 2;
		panelEx1.Style.CornerDiameter = 10;
		panelEx1.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx1).TabIndex = 135;
		((Control)lblSixZero).Cursor = Cursors.Hand;
		((Control)lblSixZero).Dock = (DockStyle)5;
		((Control)lblSixZero).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblSixZero).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblSixZero).Location = new Point(0, 0);
		((Control)lblSixZero).Margin = new Padding(4, 0, 4, 0);
		((Control)lblSixZero).Name = "lblSixZero";
		((Control)lblSixZero).Size = new Size(133, 39);
		((Control)lblSixZero).TabIndex = 0;
		((Control)lblSixZero).Text = "000.000";
		lblSixZero.TextAlign = (ContentAlignment)32;
		((Control)lblSixZero).Click += btnSixZero_Click;
		panelEx2.CanvasColor = Color.Transparent;
		((Control)panelEx2).Controls.Add((Control)(object)lblThreeZero);
		panelEx2.DisabledBackColor = Color.Empty;
		((Control)panelEx2).Location = new Point(146, 281);
		((Control)panelEx2).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(133, 39);
		panelEx2.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx2.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx2.Style.Border = eBorderType.SingleLine;
		panelEx2.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx2.Style.BorderWidth = 2;
		panelEx2.Style.CornerDiameter = 10;
		panelEx2.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx2).TabIndex = 135;
		((Control)lblThreeZero).Cursor = Cursors.Hand;
		((Control)lblThreeZero).Dock = (DockStyle)5;
		((Control)lblThreeZero).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblThreeZero).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblThreeZero).Location = new Point(0, 0);
		((Control)lblThreeZero).Margin = new Padding(4, 0, 4, 0);
		((Control)lblThreeZero).Name = "lblThreeZero";
		((Control)lblThreeZero).Size = new Size(133, 39);
		((Control)lblThreeZero).TabIndex = 0;
		((Control)lblThreeZero).Text = "000";
		lblThreeZero.TextAlign = (ContentAlignment)32;
		((Control)lblThreeZero).Click += btnThreeZero_Click;
		panelEx3.CanvasColor = Color.Transparent;
		((Control)panelEx3).Controls.Add((Control)(object)lblDecimalPoint);
		panelEx3.DisabledBackColor = Color.Empty;
		((Control)panelEx3).Location = new Point(286, 237);
		((Control)panelEx3).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx3).Name = "panelEx3";
		((Control)panelEx3).Size = new Size(133, 39);
		panelEx3.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx3.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx3.Style.Border = eBorderType.SingleLine;
		panelEx3.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx3.Style.BorderWidth = 2;
		panelEx3.Style.CornerDiameter = 10;
		panelEx3.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx3).TabIndex = 151;
		((Control)lblDecimalPoint).Cursor = Cursors.Hand;
		((Control)lblDecimalPoint).Dock = (DockStyle)5;
		((Control)lblDecimalPoint).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDecimalPoint).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblDecimalPoint).Location = new Point(0, 0);
		((Control)lblDecimalPoint).Margin = new Padding(4, 0, 4, 0);
		((Control)lblDecimalPoint).Name = "lblDecimalPoint";
		((Control)lblDecimalPoint).Size = new Size(133, 39);
		((Control)lblDecimalPoint).TabIndex = 0;
		((Control)lblDecimalPoint).Text = ".";
		lblDecimalPoint.TextAlign = (ContentAlignment)32;
		((Control)lblDecimalPoint).Click += btnDecimalPoint_Click;
		panelEx5.CanvasColor = Color.Transparent;
		((Control)panelEx5).Controls.Add((Control)(object)lbl0);
		panelEx5.DisabledBackColor = Color.Empty;
		((Control)panelEx5).Location = new Point(5, 237);
		((Control)panelEx5).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx5).Name = "panelEx5";
		((Control)panelEx5).Size = new Size(274, 39);
		panelEx5.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx5.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx5.Style.Border = eBorderType.SingleLine;
		panelEx5.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx5.Style.BorderWidth = 2;
		panelEx5.Style.CornerDiameter = 10;
		panelEx5.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx5).TabIndex = 153;
		((Control)lbl0).Cursor = Cursors.Hand;
		((Control)lbl0).Dock = (DockStyle)5;
		((Control)lbl0).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl0).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl0).Location = new Point(0, 0);
		((Control)lbl0).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl0).Name = "lbl0";
		((Control)lbl0).Size = new Size(274, 39);
		((Control)lbl0).TabIndex = 0;
		((Control)lbl0).Text = "0";
		lbl0.TextAlign = (ContentAlignment)32;
		((Control)lbl0).Click += btn0_Click;
		panelEx4.CanvasColor = Color.Transparent;
		((Control)panelEx4).Controls.Add((Control)(object)lbl3);
		panelEx4.DisabledBackColor = Color.Empty;
		((Control)panelEx4).Location = new Point(286, 193);
		((Control)panelEx4).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx4).Name = "panelEx4";
		((Control)panelEx4).Size = new Size(133, 39);
		panelEx4.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx4.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx4.Style.Border = eBorderType.SingleLine;
		panelEx4.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx4.Style.BorderWidth = 2;
		panelEx4.Style.CornerDiameter = 10;
		panelEx4.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx4).TabIndex = 163;
		((Control)lbl3).Cursor = Cursors.Hand;
		((Control)lbl3).Dock = (DockStyle)5;
		((Control)lbl3).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl3).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl3).Location = new Point(0, 0);
		((Control)lbl3).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl3).Name = "lbl3";
		((Control)lbl3).Size = new Size(133, 39);
		((Control)lbl3).TabIndex = 0;
		((Control)lbl3).Text = "3";
		lbl3.TextAlign = (ContentAlignment)32;
		((Control)lbl3).Click += btn3_Click;
		panelEx6.CanvasColor = Color.Transparent;
		((Control)panelEx6).Controls.Add((Control)(object)lbl2);
		panelEx6.DisabledBackColor = Color.Empty;
		((Control)panelEx6).Location = new Point(146, 193);
		((Control)panelEx6).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx6).Name = "panelEx6";
		((Control)panelEx6).Size = new Size(133, 39);
		panelEx6.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx6.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx6.Style.Border = eBorderType.SingleLine;
		panelEx6.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx6.Style.BorderWidth = 2;
		panelEx6.Style.CornerDiameter = 10;
		panelEx6.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx6).TabIndex = 164;
		((Control)lbl2).Cursor = Cursors.Hand;
		((Control)lbl2).Dock = (DockStyle)5;
		((Control)lbl2).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl2).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl2).Location = new Point(0, 0);
		((Control)lbl2).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl2).Name = "lbl2";
		((Control)lbl2).Size = new Size(133, 39);
		((Control)lbl2).TabIndex = 0;
		((Control)lbl2).Text = "2";
		lbl2.TextAlign = (ContentAlignment)32;
		((Control)lbl2).Click += btn2_Click;
		panelEx7.CanvasColor = Color.Transparent;
		((Control)panelEx7).Controls.Add((Control)(object)lbl1);
		panelEx7.DisabledBackColor = Color.Empty;
		((Control)panelEx7).Location = new Point(5, 193);
		((Control)panelEx7).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx7).Name = "panelEx7";
		((Control)panelEx7).Size = new Size(133, 39);
		panelEx7.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx7.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx7.Style.Border = eBorderType.SingleLine;
		panelEx7.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx7.Style.BorderWidth = 2;
		panelEx7.Style.CornerDiameter = 10;
		panelEx7.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx7).TabIndex = 165;
		((Control)lbl1).Cursor = Cursors.Hand;
		((Control)lbl1).Dock = (DockStyle)5;
		((Control)lbl1).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl1).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl1).Location = new Point(0, 0);
		((Control)lbl1).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl1).Name = "lbl1";
		((Control)lbl1).Size = new Size(133, 39);
		((Control)lbl1).TabIndex = 0;
		((Control)lbl1).Text = "1";
		lbl1.TextAlign = (ContentAlignment)32;
		((Control)lbl1).Click += btn1_Click;
		panelEx8.CanvasColor = Color.Transparent;
		((Control)panelEx8).Controls.Add((Control)(object)lbl6);
		panelEx8.DisabledBackColor = Color.Empty;
		((Control)panelEx8).Location = new Point(286, 149);
		((Control)panelEx8).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx8).Name = "panelEx8";
		((Control)panelEx8).Size = new Size(133, 39);
		panelEx8.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx8.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx8.Style.Border = eBorderType.SingleLine;
		panelEx8.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx8.Style.BorderWidth = 2;
		panelEx8.Style.CornerDiameter = 10;
		panelEx8.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx8).TabIndex = 175;
		((Control)lbl6).Cursor = Cursors.Hand;
		((Control)lbl6).Dock = (DockStyle)5;
		((Control)lbl6).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl6).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl6).Location = new Point(0, 0);
		((Control)lbl6).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl6).Name = "lbl6";
		((Control)lbl6).Size = new Size(133, 39);
		((Control)lbl6).TabIndex = 0;
		((Control)lbl6).Text = "6";
		lbl6.TextAlign = (ContentAlignment)32;
		((Control)lbl6).Click += btn6_Click;
		panelEx9.CanvasColor = Color.Transparent;
		((Control)panelEx9).Controls.Add((Control)(object)lbl5);
		panelEx9.DisabledBackColor = Color.Empty;
		((Control)panelEx9).Location = new Point(146, 149);
		((Control)panelEx9).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx9).Name = "panelEx9";
		((Control)panelEx9).Size = new Size(133, 39);
		panelEx9.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx9.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx9.Style.Border = eBorderType.SingleLine;
		panelEx9.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx9.Style.BorderWidth = 2;
		panelEx9.Style.CornerDiameter = 10;
		panelEx9.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx9).TabIndex = 176;
		((Control)lbl5).Cursor = Cursors.Hand;
		((Control)lbl5).Dock = (DockStyle)5;
		((Control)lbl5).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl5).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl5).Location = new Point(0, 0);
		((Control)lbl5).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl5).Name = "lbl5";
		((Control)lbl5).Size = new Size(133, 39);
		((Control)lbl5).TabIndex = 0;
		((Control)lbl5).Text = "5";
		lbl5.TextAlign = (ContentAlignment)32;
		((Control)lbl5).Click += btn5_Click;
		panelEx10.CanvasColor = Color.Transparent;
		((Control)panelEx10).Controls.Add((Control)(object)lbl4);
		panelEx10.DisabledBackColor = Color.Empty;
		((Control)panelEx10).Location = new Point(5, 149);
		((Control)panelEx10).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx10).Name = "panelEx10";
		((Control)panelEx10).Size = new Size(133, 39);
		panelEx10.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx10.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx10.Style.Border = eBorderType.SingleLine;
		panelEx10.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx10.Style.BorderWidth = 2;
		panelEx10.Style.CornerDiameter = 10;
		panelEx10.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx10).TabIndex = 177;
		((Control)lbl4).Cursor = Cursors.Hand;
		((Control)lbl4).Dock = (DockStyle)5;
		((Control)lbl4).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl4).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl4).Location = new Point(0, 0);
		((Control)lbl4).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl4).Name = "lbl4";
		((Control)lbl4).Size = new Size(133, 39);
		((Control)lbl4).TabIndex = 0;
		((Control)lbl4).Text = "4";
		lbl4.TextAlign = (ContentAlignment)32;
		((Control)lbl4).Click += btn4_Click;
		panelEx11.CanvasColor = Color.Transparent;
		((Control)panelEx11).Controls.Add((Control)(object)lbl9);
		panelEx11.DisabledBackColor = Color.Empty;
		((Control)panelEx11).Location = new Point(286, 105);
		((Control)panelEx11).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx11).Name = "panelEx11";
		((Control)panelEx11).Size = new Size(133, 39);
		panelEx11.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx11.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx11.Style.Border = eBorderType.SingleLine;
		panelEx11.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx11.Style.BorderWidth = 2;
		panelEx11.Style.CornerDiameter = 10;
		panelEx11.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx11).TabIndex = 187;
		((Control)lbl9).Cursor = Cursors.Hand;
		((Control)lbl9).Dock = (DockStyle)5;
		((Control)lbl9).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl9).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl9).Location = new Point(0, 0);
		((Control)lbl9).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl9).Name = "lbl9";
		((Control)lbl9).Size = new Size(133, 39);
		((Control)lbl9).TabIndex = 0;
		((Control)lbl9).Text = "9";
		lbl9.TextAlign = (ContentAlignment)32;
		((Control)lbl9).Click += btn9_Click;
		panelEx12.CanvasColor = Color.Transparent;
		((Control)panelEx12).Controls.Add((Control)(object)lbl8);
		panelEx12.DisabledBackColor = Color.Empty;
		((Control)panelEx12).Location = new Point(146, 105);
		((Control)panelEx12).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx12).Name = "panelEx12";
		((Control)panelEx12).Size = new Size(133, 39);
		panelEx12.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx12.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx12.Style.Border = eBorderType.SingleLine;
		panelEx12.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx12.Style.BorderWidth = 2;
		panelEx12.Style.CornerDiameter = 10;
		panelEx12.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx12).TabIndex = 188;
		((Control)lbl8).Cursor = Cursors.Hand;
		((Control)lbl8).Dock = (DockStyle)5;
		((Control)lbl8).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl8).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl8).Location = new Point(0, 0);
		((Control)lbl8).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl8).Name = "lbl8";
		((Control)lbl8).Size = new Size(133, 39);
		((Control)lbl8).TabIndex = 0;
		((Control)lbl8).Text = "8";
		lbl8.TextAlign = (ContentAlignment)32;
		((Control)lbl8).Click += btn8_Click;
		panelEx13.CanvasColor = Color.Transparent;
		((Control)panelEx13).Controls.Add((Control)(object)lbl7);
		panelEx13.DisabledBackColor = Color.Empty;
		((Control)panelEx13).Location = new Point(5, 105);
		((Control)panelEx13).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx13).Name = "panelEx13";
		((Control)panelEx13).Size = new Size(133, 39);
		panelEx13.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx13.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx13.Style.Border = eBorderType.SingleLine;
		panelEx13.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx13.Style.BorderWidth = 2;
		panelEx13.Style.CornerDiameter = 10;
		panelEx13.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx13).TabIndex = 189;
		((Control)lbl7).Cursor = Cursors.Hand;
		((Control)lbl7).Dock = (DockStyle)5;
		((Control)lbl7).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lbl7).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lbl7).Location = new Point(0, 0);
		((Control)lbl7).Margin = new Padding(4, 0, 4, 0);
		((Control)lbl7).Name = "lbl7";
		((Control)lbl7).Size = new Size(133, 39);
		((Control)lbl7).TabIndex = 0;
		((Control)lbl7).Text = "7";
		lbl7.TextAlign = (ContentAlignment)32;
		((Control)lbl7).Click += btn7_Click;
		panelEx14.CanvasColor = Color.Transparent;
		((Control)panelEx14).Controls.Add((Control)(object)lblMinus1P);
		panelEx14.DisabledBackColor = Color.Empty;
		((Control)panelEx14).Location = new Point(146, 15);
		((Control)panelEx14).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx14).Name = "panelEx14";
		((Control)panelEx14).Size = new Size(133, 39);
		panelEx14.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx14.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx14.Style.Border = eBorderType.SingleLine;
		panelEx14.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx14.Style.BorderWidth = 2;
		panelEx14.Style.CornerDiameter = 10;
		panelEx14.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx14).TabIndex = 204;
		((Control)lblMinus1P).Cursor = Cursors.Hand;
		((Control)lblMinus1P).Dock = (DockStyle)5;
		((Control)lblMinus1P).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblMinus1P).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblMinus1P).Location = new Point(0, 0);
		((Control)lblMinus1P).Margin = new Padding(4, 0, 4, 0);
		((Control)lblMinus1P).Name = "lblMinus1P";
		((Control)lblMinus1P).Size = new Size(133, 39);
		((Control)lblMinus1P).TabIndex = 0;
		((Control)lblMinus1P).Text = "0.1-";
		lblMinus1P.TextAlign = (ContentAlignment)32;
		((Control)lblMinus1P).Click += btnMinus1P_Click;
		panelEx15.CanvasColor = Color.Transparent;
		((Control)panelEx15).Controls.Add((Control)(object)lblPlus1P);
		panelEx15.DisabledBackColor = Color.Empty;
		((Control)panelEx15).Location = new Point(5, 15);
		((Control)panelEx15).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx15).Name = "panelEx15";
		((Control)panelEx15).Size = new Size(133, 39);
		panelEx15.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx15.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx15.Style.Border = eBorderType.SingleLine;
		panelEx15.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx15.Style.BorderWidth = 2;
		panelEx15.Style.CornerDiameter = 10;
		panelEx15.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx15).TabIndex = 205;
		((Control)lblPlus1P).Cursor = Cursors.Hand;
		((Control)lblPlus1P).Dock = (DockStyle)5;
		((Control)lblPlus1P).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblPlus1P).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblPlus1P).Location = new Point(0, 0);
		((Control)lblPlus1P).Margin = new Padding(4, 0, 4, 0);
		((Control)lblPlus1P).Name = "lblPlus1P";
		((Control)lblPlus1P).Size = new Size(133, 39);
		((Control)lblPlus1P).TabIndex = 0;
		((Control)lblPlus1P).Text = "0.1+";
		lblPlus1P.TextAlign = (ContentAlignment)32;
		((Control)lblPlus1P).Click += btnPlus1P_Click;
		panelEx16.CanvasColor = Color.Transparent;
		((Control)panelEx16).Controls.Add((Control)(object)lblMinus1);
		panelEx16.DisabledBackColor = Color.Empty;
		((Control)panelEx16).Location = new Point(146, 59);
		((Control)panelEx16).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx16).Name = "panelEx16";
		((Control)panelEx16).Size = new Size(133, 39);
		panelEx16.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx16.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx16.Style.Border = eBorderType.SingleLine;
		panelEx16.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx16.Style.BorderWidth = 2;
		panelEx16.Style.CornerDiameter = 10;
		panelEx16.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx16).TabIndex = 202;
		((Control)lblMinus1).Cursor = Cursors.Hand;
		((Control)lblMinus1).Dock = (DockStyle)5;
		((Control)lblMinus1).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblMinus1).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblMinus1).Location = new Point(0, 0);
		((Control)lblMinus1).Margin = new Padding(4, 0, 4, 0);
		((Control)lblMinus1).Name = "lblMinus1";
		((Control)lblMinus1).Size = new Size(133, 39);
		((Control)lblMinus1).TabIndex = 0;
		((Control)lblMinus1).Text = "1-";
		lblMinus1.TextAlign = (ContentAlignment)32;
		((Control)lblMinus1).Click += btnMinus1_Click;
		panelEx17.CanvasColor = Color.Transparent;
		((Control)panelEx17).Controls.Add((Control)(object)lblPlus1);
		panelEx17.DisabledBackColor = Color.Empty;
		((Control)panelEx17).Location = new Point(5, 59);
		((Control)panelEx17).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx17).Name = "panelEx17";
		((Control)panelEx17).Size = new Size(133, 39);
		panelEx17.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx17.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx17.Style.Border = eBorderType.SingleLine;
		panelEx17.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx17.Style.BorderWidth = 2;
		panelEx17.Style.CornerDiameter = 10;
		panelEx17.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx17).TabIndex = 203;
		((Control)lblPlus1).Cursor = Cursors.Hand;
		((Control)lblPlus1).Dock = (DockStyle)5;
		((Control)lblPlus1).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblPlus1).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblPlus1).Location = new Point(0, 0);
		((Control)lblPlus1).Margin = new Padding(4, 0, 4, 0);
		((Control)lblPlus1).Name = "lblPlus1";
		((Control)lblPlus1).Size = new Size(133, 39);
		((Control)lblPlus1).TabIndex = 0;
		((Control)lblPlus1).Text = "1+";
		lblPlus1.TextAlign = (ContentAlignment)32;
		((Control)lblPlus1).Click += btnPlus1_Click;
		panelEx18.CanvasColor = Color.Transparent;
		((Control)panelEx18).Controls.Add((Control)(object)lblBackSpace);
		panelEx18.DisabledBackColor = Color.Empty;
		((Control)panelEx18).Location = new Point(286, 15);
		((Control)panelEx18).Margin = new Padding(4, 5, 4, 5);
		((Control)panelEx18).Name = "panelEx18";
		((Control)panelEx18).Size = new Size(133, 85);
		panelEx18.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panelEx18.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panelEx18.Style.Border = eBorderType.SingleLine;
		panelEx18.Style.BorderColor.Color = Color.FromArgb(65, 81, 220);
		panelEx18.Style.BorderWidth = 2;
		panelEx18.Style.CornerDiameter = 10;
		panelEx18.Style.CornerType = eCornerType.Rounded;
		((Control)panelEx18).TabIndex = 209;
		((Control)lblBackSpace).Cursor = Cursors.Hand;
		((Control)lblBackSpace).Dock = (DockStyle)5;
		((Control)lblBackSpace).Font = new Font("B Yekan", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblBackSpace).ForeColor = Color.FromArgb(65, 81, 220);
		((Control)lblBackSpace).Location = new Point(0, 0);
		((Control)lblBackSpace).Margin = new Padding(4, 0, 4, 0);
		((Control)lblBackSpace).Name = "lblBackSpace";
		((Control)lblBackSpace).Size = new Size(133, 85);
		((Control)lblBackSpace).TabIndex = 0;
		((Control)lblBackSpace).Text = "پاک کن";
		lblBackSpace.TextAlign = (ContentAlignment)32;
		((Control)lblBackSpace).Click += btnBackSpace_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)GroupBox1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)this).Margin = new Padding(4, 3, 4, 3);
		((Control)this).Name = "UcNumberPanel";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(434, 335);
		((Control)GroupBox1).ResumeLayout(false);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)panelEx1).ResumeLayout(false);
		((Control)panelEx2).ResumeLayout(false);
		((Control)panelEx3).ResumeLayout(false);
		((Control)panelEx5).ResumeLayout(false);
		((Control)panelEx4).ResumeLayout(false);
		((Control)panelEx6).ResumeLayout(false);
		((Control)panelEx7).ResumeLayout(false);
		((Control)panelEx8).ResumeLayout(false);
		((Control)panelEx9).ResumeLayout(false);
		((Control)panelEx10).ResumeLayout(false);
		((Control)panelEx11).ResumeLayout(false);
		((Control)panelEx12).ResumeLayout(false);
		((Control)panelEx13).ResumeLayout(false);
		((Control)panelEx14).ResumeLayout(false);
		((Control)panelEx15).ResumeLayout(false);
		((Control)panelEx16).ResumeLayout(false);
		((Control)panelEx17).ResumeLayout(false);
		((Control)panelEx18).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
