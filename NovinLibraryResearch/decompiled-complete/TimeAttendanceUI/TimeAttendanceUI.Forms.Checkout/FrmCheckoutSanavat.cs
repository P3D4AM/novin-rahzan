using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmCheckoutSanavat : FormExtention, ICheckout
{
	private Font _font;

	private readonly TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private ICheckout _nextPage;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX labelX1;

	private GroupBox groupBox1;

	private TextBoxFa txtSanavatMax;

	private TextBoxFa txtSanavatMin;

	private TextBoxFa txtFixPrice;

	private RadioButton rbSanavat2;

	private RadioButton rbSanavat1;

	internal LabelX labelX8;

	internal LabelX labelX7;

	internal LabelX labelX9;

	internal LabelX labelX10;

	internal LabelX labelX5;

	internal LabelX labelX6;

	internal LabelX labelX2;

	private CheckBoxX chkSanavat;

	internal LabelX labelX4;

	internal LabelX labelX3;

	internal LabelX labelX11;

	internal LabelX labelX13;

	internal LabelX labelX12;

	private NumericUpDown txtDateCount;

	public FrmCheckoutSanavat(string contextGuid, Font font, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout)
		: base(contextGuid, true, true)
	{
		try
		{
			_font = font;
			InitializeComponent();
			_checkout = checkout;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		((FormExtention)this).OnLoad(e);
		((Control)base.statusStrip1).Visible = false;
		((Control)chkSanavat).Focus();
	}

	private void ChkSanavat_CheckedChanged(object sender, EventArgs e)
	{
		RadioButton obj = rbSanavat1;
		bool enabled = (((Control)rbSanavat2).Enabled = chkSanavat.Checked);
		((Control)obj).Enabled = enabled;
		if (chkSanavat.Checked)
		{
			((Control)txtFixPrice).Enabled = true;
			return;
		}
		NumericUpDown obj2 = txtDateCount;
		TextBoxFa obj3 = txtSanavatMin;
		TextBoxFa obj4 = txtSanavatMax;
		bool flag2 = (((Control)txtFixPrice).Enabled = false);
		bool flag4 = (((Control)obj4).Enabled = flag2);
		enabled = (((Control)obj3).Enabled = flag4);
		((Control)obj2).Enabled = enabled;
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		if (fillCheckout)
		{
			FillCheckout();
		}
		return _nextPage ?? (_nextPage = new FrmCheckOutContractSelect(((CacheBaseForm)this).ContextGuid, _font, _checkout));
	}

	public void FillCheckout()
	{
		_checkout.HasSanavat = chkSanavat.Checked;
		_checkout.SanavatType = ((!rbSanavat1.Checked) ? TimeAttendanceDAL.Dal.FLRCheckout.Checkout.SanavatTypeEnum.CurrentContract : TimeAttendanceDAL.Dal.FLRCheckout.Checkout.SanavatTypeEnum.Fixed);
		_checkout.FixSanavat = ((((Control)txtFixPrice).Text.Trim() == "") ? 0m : decimal.Parse(((Control)txtFixPrice).Text.Trim()));
		_checkout.DaysOfSanavat = (short)((!(((Control)txtDateCount).Text.Trim() == "")) ? short.Parse(((Control)txtDateCount).Text.Trim()) : 0);
		_checkout.MinSanavat = ((((Control)txtSanavatMin).Text.Trim() == "") ? 0m : decimal.Parse(((Control)txtSanavatMin).Text.Trim()));
		_checkout.MaxSanavat = ((((Control)txtSanavatMax).Text.Trim() == "") ? 0m : decimal.Parse(((Control)txtSanavatMax).Text.Trim()));
	}

	private void RbSanavat1_CheckedChanged(object sender, EventArgs e)
	{
		((Control)txtFixPrice).Enabled = rbSanavat1.Checked;
	}

	private void RbSanavat2_CheckedChanged(object sender, EventArgs e)
	{
		NumericUpDown obj = txtDateCount;
		TextBoxFa obj2 = txtSanavatMin;
		bool flag = (((Control)txtSanavatMax).Enabled = rbSanavat2.Checked);
		bool enabled = (((Control)obj2).Enabled = flag);
		((Control)obj).Enabled = enabled;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((FormExtention)this).Dispose(disposing);
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
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Expected O, but got Unknown
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Expected O, but got Unknown
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab7: Expected O, but got Unknown
		//IL_0bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf8: Expected O, but got Unknown
		//IL_0cc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cce: Expected O, but got Unknown
		//IL_0db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dba: Expected O, but got Unknown
		//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb1: Expected O, but got Unknown
		//IL_0f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa8: Expected O, but got Unknown
		//IL_1092: Unknown result type (might be due to invalid IL or missing references)
		//IL_109c: Expected O, but got Unknown
		//IL_1185: Unknown result type (might be due to invalid IL or missing references)
		//IL_118f: Expected O, but got Unknown
		//IL_1279: Unknown result type (might be due to invalid IL or missing references)
		//IL_1283: Expected O, but got Unknown
		//IL_1370: Unknown result type (might be due to invalid IL or missing references)
		//IL_137a: Expected O, but got Unknown
		//IL_1467: Unknown result type (might be due to invalid IL or missing references)
		//IL_1471: Expected O, but got Unknown
		//IL_154c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1556: Expected O, but got Unknown
		//IL_1647: Unknown result type (might be due to invalid IL or missing references)
		//IL_1651: Expected O, but got Unknown
		//IL_1742: Unknown result type (might be due to invalid IL or missing references)
		//IL_174c: Expected O, but got Unknown
		//IL_183a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1844: Expected O, but got Unknown
		//IL_1932: Unknown result type (might be due to invalid IL or missing references)
		//IL_193c: Expected O, but got Unknown
		//IL_1a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a34: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		groupBox1 = new GroupBox();
		txtDateCount = new NumericUpDown();
		txtSanavatMax = new TextBoxFa();
		txtSanavatMin = new TextBoxFa();
		txtFixPrice = new TextBoxFa();
		rbSanavat2 = new RadioButton();
		rbSanavat1 = new RadioButton();
		labelX8 = new LabelX();
		labelX7 = new LabelX();
		labelX9 = new LabelX();
		labelX10 = new LabelX();
		labelX5 = new LabelX();
		labelX6 = new LabelX();
		labelX2 = new LabelX();
		chkSanavat = new CheckBoxX();
		labelX13 = new LabelX();
		labelX12 = new LabelX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX11 = new LabelX();
		labelX1 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((Control)groupBox1).SuspendLayout();
		((ISupportInitialize)txtDateCount).BeginInit();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)groupPanel4);
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Font = new Font("B Homa", 9.75f);
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Margin = new Padding(3, 4, 3, 4);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).RightToLeft = (RightToLeft)1;
		((Control)PanelEx1).Size = new Size(582, 478);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 0;
		((PanelControl)groupPanel4).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel4).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel4).Controls.Add((Control)(object)groupBox1);
		((Control)groupPanel4).Controls.Add((Control)(object)chkSanavat);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX13);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX12);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX11);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel4).Dock = (DockStyle)5;
		((Control)groupPanel4).Location = new Point(0, 0);
		((Control)groupPanel4).Name = "groupPanel4";
		((Control)groupPanel4).Size = new Size(582, 478);
		((PanelControl)groupPanel4).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel4).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel4).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel4).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanel4).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel4).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanel4).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderRightWidth = 1;
		((PanelControl)groupPanel4).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel4).Style.BorderTopWidth = 1;
		((PanelControl)groupPanel4).Style.Class = "";
		((PanelControl)groupPanel4).Style.CornerDiameter = 4;
		((PanelControl)groupPanel4).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel4).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel4).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel4).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel4).StyleMouseDown.Class = "";
		((PanelControl)groupPanel4).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel4).StyleMouseOver.Class = "";
		((PanelControl)groupPanel4).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel4).TabIndex = 5;
		((Control)groupPanel4).Text = "[محاسبه سنوات]";
		((Control)groupBox1).Controls.Add((Control)(object)txtDateCount);
		((Control)groupBox1).Controls.Add((Control)(object)txtSanavatMax);
		((Control)groupBox1).Controls.Add((Control)(object)txtSanavatMin);
		((Control)groupBox1).Controls.Add((Control)(object)txtFixPrice);
		((Control)groupBox1).Controls.Add((Control)(object)rbSanavat2);
		((Control)groupBox1).Controls.Add((Control)(object)rbSanavat1);
		((Control)groupBox1).Controls.Add((Control)(object)labelX8);
		((Control)groupBox1).Controls.Add((Control)(object)labelX7);
		((Control)groupBox1).Controls.Add((Control)(object)labelX9);
		((Control)groupBox1).Controls.Add((Control)(object)labelX10);
		((Control)groupBox1).Controls.Add((Control)(object)labelX5);
		((Control)groupBox1).Controls.Add((Control)(object)labelX6);
		((Control)groupBox1).Controls.Add((Control)(object)labelX2);
		((Control)groupBox1).Location = new Point(3, 265);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(554, 116);
		((Control)groupBox1).TabIndex = 22;
		groupBox1.TabStop = false;
		((Control)txtDateCount).Enabled = false;
		((Control)txtDateCount).Font = new Font("B Titr", 8f);
		((Control)txtDateCount).Location = new Point(342, 75);
		txtDateCount.Maximum = new decimal(new int[4] { 366, 0, 0, 0 });
		((Control)txtDateCount).Name = "txtDateCount";
		((Control)txtDateCount).Size = new Size(56, 26);
		((Control)txtDateCount).TabIndex = 3;
		((UpDownBase)txtDateCount).TextAlign = (HorizontalAlignment)2;
		txtDateCount.Value = new decimal(new int[4] { 60, 0, 0, 0 });
		txtSanavatMax.AllowEmpty = true;
		txtSanavatMax.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSanavatMax.BackColorTextBoxWhenLostFocus = Color.White;
		txtSanavatMax.BackcolorTooltip = Color.Red;
		txtSanavatMax.CurrencyFormat = "";
		((Control)txtSanavatMax).Enabled = false;
		((Control)txtSanavatMax).Font = new Font("B Titr", 8f);
		((Control)txtSanavatMax).ForeColor = Color.Green;
		txtSanavatMax.ForeColorTooltip = Color.Black;
		txtSanavatMax.GetColoresFromSettingDLL = true;
		txtSanavatMax.HandleKeydown = true;
		((Control)txtSanavatMax).Location = new Point(31, 74);
		((TextBoxBase)txtSanavatMax).MaxLength = 19;
		((Control)txtSanavatMax).Name = "txtSanavatMax";
		txtSanavatMax.SendShiftTabAfterDown = true;
		txtSanavatMax.SendShiftTabAfterEnter = true;
		txtSanavatMax.SendTabAfterEnter = true;
		txtSanavatMax.SendTabAfterUP = true;
		txtSanavatMax.ShowHorofi = (ENShowHorofi)3;
		txtSanavatMax.ShowToolTipText = true;
		((Control)txtSanavatMax).Size = new Size(62, 26);
		((Control)txtSanavatMax).TabIndex = 5;
		((TextBox)txtSanavatMax).TextAlign = (HorizontalAlignment)2;
		txtSanavatMax.TootTipDelay = 1000;
		((Control)txtSanavatMax).Text = "";
		txtSanavatMin.AllowEmpty = true;
		txtSanavatMin.BackColorTextBoxWhenFocus = Color.Yellow;
		txtSanavatMin.BackColorTextBoxWhenLostFocus = Color.White;
		txtSanavatMin.BackcolorTooltip = Color.Red;
		txtSanavatMin.CurrencyFormat = "";
		((Control)txtSanavatMin).Enabled = false;
		((Control)txtSanavatMin).Font = new Font("B Titr", 8f);
		((Control)txtSanavatMin).ForeColor = Color.Green;
		txtSanavatMin.ForeColorTooltip = Color.Black;
		txtSanavatMin.GetColoresFromSettingDLL = true;
		txtSanavatMin.HandleKeydown = true;
		((Control)txtSanavatMin).Location = new Point(201, 74);
		((TextBoxBase)txtSanavatMin).MaxLength = 19;
		((Control)txtSanavatMin).Name = "txtSanavatMin";
		txtSanavatMin.SendShiftTabAfterDown = true;
		txtSanavatMin.SendShiftTabAfterEnter = true;
		txtSanavatMin.SendTabAfterEnter = true;
		txtSanavatMin.SendTabAfterUP = true;
		txtSanavatMin.ShowHorofi = (ENShowHorofi)3;
		txtSanavatMin.ShowToolTipText = true;
		((Control)txtSanavatMin).Size = new Size(62, 26);
		((Control)txtSanavatMin).TabIndex = 4;
		((TextBox)txtSanavatMin).TextAlign = (HorizontalAlignment)2;
		txtSanavatMin.TootTipDelay = 1000;
		((Control)txtSanavatMin).Text = "";
		txtFixPrice.AllowEmpty = true;
		txtFixPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		txtFixPrice.BackColorTextBoxWhenLostFocus = Color.White;
		txtFixPrice.BackcolorTooltip = Color.Red;
		txtFixPrice.CurrencyFormat = "";
		((Control)txtFixPrice).Enabled = false;
		((Control)txtFixPrice).Font = new Font("B Titr", 10f, (FontStyle)1);
		((Control)txtFixPrice).ForeColor = Color.Green;
		txtFixPrice.ForeColorTooltip = Color.Black;
		txtFixPrice.GetColoresFromSettingDLL = true;
		txtFixPrice.HandleKeydown = true;
		((Control)txtFixPrice).Location = new Point(129, 24);
		((TextBoxBase)txtFixPrice).MaxLength = 19;
		((Control)txtFixPrice).Name = "txtFixPrice";
		txtFixPrice.SendShiftTabAfterDown = true;
		txtFixPrice.SendShiftTabAfterEnter = true;
		txtFixPrice.SendTabAfterEnter = true;
		txtFixPrice.SendTabAfterUP = true;
		txtFixPrice.ShowHorofi = (ENShowHorofi)3;
		txtFixPrice.ShowToolTipText = true;
		((Control)txtFixPrice).Size = new Size(119, 31);
		((Control)txtFixPrice).TabIndex = 1;
		((Control)txtFixPrice).Text = "15168820";
		txtFixPrice.TootTipDelay = 1000;
		((Control)txtFixPrice).Text = "15168820";
		((Control)rbSanavat2).BackColor = Color.Transparent;
		((Control)rbSanavat2).Enabled = false;
		((Control)rbSanavat2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbSanavat2).ForeColor = Color.Navy;
		((Control)rbSanavat2).Location = new Point(222, 52);
		((Control)rbSanavat2).Name = "rbSanavat2";
		((Control)rbSanavat2).Size = new Size(296, 22);
		((Control)rbSanavat2).TabIndex = 2;
		((Control)rbSanavat2).Text = "[محاسبه سنوات بر اساس قرارداد جاری]";
		((ButtonBase)rbSanavat2).UseVisualStyleBackColor = false;
		rbSanavat2.CheckedChanged += RbSanavat2_CheckedChanged;
		((Control)rbSanavat1).BackColor = Color.Transparent;
		rbSanavat1.Checked = true;
		((Control)rbSanavat1).Enabled = false;
		((Control)rbSanavat1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbSanavat1).ForeColor = Color.Navy;
		((Control)rbSanavat1).Location = new Point(222, 0);
		((Control)rbSanavat1).Name = "rbSanavat1";
		((Control)rbSanavat1).Size = new Size(296, 22);
		((Control)rbSanavat1).TabIndex = 0;
		rbSanavat1.TabStop = true;
		((Control)rbSanavat1).Text = "[محاسبه سنوات بر اساس حداقل قانون کار]";
		((ButtonBase)rbSanavat1).UseVisualStyleBackColor = false;
		rbSanavat1.CheckedChanged += RbSanavat1_CheckedChanged;
		((Control)labelX8).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX8).BackgroundStyle.Class = "";
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).ForeColor = Color.Navy;
		((Control)labelX8).Location = new Point(256, 69);
		((Control)labelX8).Name = "labelX8";
		labelX8.PaddingBottom = 5;
		labelX8.PaddingLeft = 5;
		labelX8.PaddingRight = 5;
		labelX8.PaddingTop = 5;
		((Control)labelX8).Size = new Size(45, 38);
		((Control)labelX8).TabIndex = 8;
		((Control)labelX8).Text = "[حداقل]";
		labelX8.WordWrap = true;
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).ForeColor = Color.Navy;
		((Control)labelX7).Location = new Point(311, 69);
		((Control)labelX7).Name = "labelX7";
		labelX7.PaddingBottom = 5;
		labelX7.PaddingLeft = 5;
		labelX7.PaddingRight = 5;
		labelX7.PaddingTop = 5;
		((Control)labelX7).Size = new Size(33, 38);
		((Control)labelX7).TabIndex = 8;
		((Control)labelX7).Text = "[روز]";
		labelX7.WordWrap = true;
		((Control)labelX9).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX9).BackgroundStyle.Class = "";
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX9).ForeColor = Color.Navy;
		((Control)labelX9).Location = new Point(89, 69);
		((Control)labelX9).Name = "labelX9";
		labelX9.PaddingBottom = 5;
		labelX9.PaddingLeft = 5;
		labelX9.PaddingRight = 5;
		labelX9.PaddingTop = 5;
		((Control)labelX9).Size = new Size(112, 38);
		((Control)labelX9).TabIndex = 8;
		((Control)labelX9).Text = "[ریال تا سقف حداکثر]";
		labelX9.WordWrap = true;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX10).BackgroundStyle.Class = "";
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).ForeColor = Color.Navy;
		((Control)labelX10).Location = new Point(0, 69);
		((Control)labelX10).Name = "labelX10";
		labelX10.PaddingBottom = 5;
		labelX10.PaddingLeft = 5;
		labelX10.PaddingRight = 5;
		labelX10.PaddingTop = 5;
		((Control)labelX10).Size = new Size(33, 38);
		((Control)labelX10).TabIndex = 8;
		((Control)labelX10).Text = "Rial";
		labelX10.WordWrap = true;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX5).ForeColor = Color.Navy;
		((Control)labelX5).Location = new Point(98, 18);
		((Control)labelX5).Name = "labelX5";
		labelX5.PaddingBottom = 5;
		labelX5.PaddingLeft = 5;
		labelX5.PaddingRight = 5;
		labelX5.PaddingTop = 5;
		((Control)labelX5).Size = new Size(33, 38);
		((Control)labelX5).TabIndex = 8;
		((Control)labelX5).Text = "Rial";
		labelX5.WordWrap = true;
		((Control)labelX6).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX6).ForeColor = Color.Navy;
		((Control)labelX6).Location = new Point(385, 69);
		((Control)labelX6).Name = "labelX6";
		labelX6.PaddingBottom = 5;
		labelX6.PaddingLeft = 5;
		labelX6.PaddingRight = 5;
		labelX6.PaddingTop = 5;
		((Control)labelX6).Size = new Size(109, 38);
		((Control)labelX6).TabIndex = 8;
		((Control)labelX6).Text = "[محاسبه بر اساس]";
		labelX6.WordWrap = true;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).ForeColor = Color.Navy;
		((Control)labelX2).Location = new Point(238, 18);
		((Control)labelX2).Name = "labelX2";
		labelX2.PaddingBottom = 5;
		labelX2.PaddingLeft = 5;
		labelX2.PaddingRight = 5;
		labelX2.PaddingTop = 5;
		((Control)labelX2).Size = new Size(258, 38);
		((Control)labelX2).TabIndex = 8;
		((Control)labelX2).Text = "[مبلغ ثابت سنوات به ازای 12 ماه کارکرد]:";
		labelX2.WordWrap = true;
		((Control)chkSanavat).BackColor = Color.Transparent;
		((BaseItemControl)chkSanavat).BackgroundStyle.Class = "";
		((BaseItemControl)chkSanavat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chkSanavat).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)chkSanavat).Location = new Point(374, 225);
		((Control)chkSanavat).Name = "chkSanavat";
		((Control)chkSanavat).Size = new Size(169, 34);
		((BaseItemControl)chkSanavat).Style = (eDotNetBarStyle)9;
		((Control)chkSanavat).TabIndex = 0;
		((Control)chkSanavat).Text = "[سنوات محاسبه شود]";
		chkSanavat.TextColor = Color.Navy;
		chkSanavat.CheckedChanged += ChkSanavat_CheckedChanged;
		((Control)labelX13).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX13).BackgroundStyle.Class = "";
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).Font = new Font("Tahoma", 7.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX13).ForeColor = Color.Black;
		((Control)labelX13).Location = new Point(9, 162);
		((Control)labelX13).Name = "labelX13";
		labelX13.PaddingBottom = 5;
		labelX13.PaddingLeft = 5;
		labelX13.PaddingRight = 5;
		labelX13.PaddingTop = 5;
		((Control)labelX13).Size = new Size(557, 38);
		((Control)labelX13).TabIndex = 17;
		((Control)labelX13).Text = "[در روش دوم ، کارفرما حق سنوات را در پایان هر سال با کارگر تسویه و پرداخت می نماید .]";
		labelX13.WordWrap = true;
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX12).BackgroundStyle.Class = "";
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Font = new Font("Tahoma", 7.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX12).ForeColor = Color.Black;
		((Control)labelX12).Location = new Point(9, 136);
		((Control)labelX12).Name = "labelX12";
		labelX12.PaddingBottom = 5;
		labelX12.PaddingLeft = 5;
		labelX12.PaddingRight = 5;
		labelX12.PaddingTop = 5;
		((Control)labelX12).Size = new Size(557, 38);
		((Control)labelX12).TabIndex = 17;
		((Control)labelX12).Text = "[در روش اول و قانونی ، حق سنوات کارگر ذخیره شده و هنگام فسخ یا اتمام همکاری به کارگر پرداخت می گردد .]";
		labelX12.WordWrap = true;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).ForeColor = Color.Black;
		((Control)labelX4).Location = new Point(9, 99);
		((Control)labelX4).Name = "labelX4";
		labelX4.PaddingBottom = 5;
		labelX4.PaddingLeft = 5;
		labelX4.PaddingRight = 5;
		labelX4.PaddingTop = 5;
		((Control)labelX4).Size = new Size(557, 38);
		((Control)labelX4).TabIndex = 17;
		((Control)labelX4).Text = "[دقت داشته باشید ، سنوات می تواند به دو روش به اشخاص پرداخت گردد .]";
		labelX4.WordWrap = true;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).ForeColor = Color.Black;
		((Control)labelX3).Location = new Point(10, 66);
		((Control)labelX3).Name = "labelX3";
		labelX3.PaddingBottom = 5;
		labelX3.PaddingLeft = 5;
		labelX3.PaddingRight = 5;
		labelX3.PaddingTop = 5;
		((Control)labelX3).Size = new Size(557, 34);
		((Control)labelX3).TabIndex = 18;
		((Control)labelX3).Text = "[قانونی آن معادل یک ماه ( 30 روز ) حداقل دستمزد قانونی سال جاری است در پایان سال می باشد .]";
		labelX3.WordWrap = true;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX11).BackgroundStyle.Class = "";
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX11).ForeColor = Color.Black;
		((Control)labelX11).Location = new Point(10, 34);
		((Control)labelX11).Name = "labelX11";
		labelX11.PaddingBottom = 5;
		labelX11.PaddingLeft = 5;
		labelX11.PaddingRight = 5;
		labelX11.PaddingTop = 5;
		((Control)labelX11).Size = new Size(556, 31);
		((Control)labelX11).TabIndex = 19;
		((Control)labelX11).Text = "[سنوات ، پشوانه مالی است که اداره قانون کار برای پایان کار کارگران منظور نموده است و نحوه محاسبه  ]";
		labelX11.WordWrap = true;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).ForeColor = Color.Black;
		((Control)labelX1).Location = new Point(23, 62);
		((Control)labelX1).Name = "labelX1";
		labelX1.PaddingBottom = 5;
		labelX1.PaddingLeft = 5;
		labelX1.PaddingRight = 5;
		labelX1.PaddingTop = 5;
		((Control)labelX1).Size = new Size(557, 34);
		((Control)labelX1).TabIndex = 7;
		((Control)labelX1).Text = "6";
		labelX1.TextAlignment = (StringAlignment)1;
		labelX1.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCheckoutSanavat";
		((Control)this).Text = "FrmCheckoutSanavat";
		((Control)this).Controls.SetChildIndex((Control)(object)PanelEx1, 0);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((ISupportInitialize)txtDateCount).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	bool ICheckout.get_TopLevel()
	{
		return ((Form)this).TopLevel;
	}

	void ICheckout.set_TopLevel(bool value)
	{
		((Form)this).TopLevel = value;
	}

	void ICheckout.Show(IWin32Window owner)
	{
		((Form)this).Show(owner);
	}
}
