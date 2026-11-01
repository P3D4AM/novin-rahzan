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

public class FrmCheckOutReward : FormExtention, ICheckout
{
	private readonly TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private ICheckout _nextPage;

	private Font _font;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX labelX4;

	internal LabelX labelX3;

	internal LabelX labelX2;

	private CheckBoxX chkReward;

	private GroupBox groupBox1;

	private RadioButton rbReward2;

	private RadioButton rbReward1;

	internal LabelX labelX5;

	internal LabelX labelX1;

	private TextBoxFa txtPriceMax;

	private TextBoxFa txtPriceMin;

	private TextBoxFa txtFixPrice;

	internal LabelX labelX8;

	internal LabelX labelX7;

	internal LabelX labelX9;

	internal LabelX labelX10;

	internal LabelX labelX6;

	private NumericUpDown txtDateCount;

	public FrmCheckOutReward(string contextGuid, Font font, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout)
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
		((Control)chkReward).Focus();
	}

	public void FillCheckout()
	{
		try
		{
			_checkout.HasReward = chkReward.Checked;
			_checkout.RewardType = ((!rbReward1.Checked) ? TimeAttendanceDAL.Dal.FLRCheckout.Checkout.RewardTypeEnum.CurrentContract : TimeAttendanceDAL.Dal.FLRCheckout.Checkout.RewardTypeEnum.Fixed);
			_checkout.FixedReward = ((((Control)txtFixPrice).Text.Trim() == "") ? 0m : decimal.Parse(((Control)txtFixPrice).Text.Trim()));
			_checkout.DaysOfReward = (short)((!(((Control)txtDateCount).Text.Trim() == "")) ? short.Parse(((Control)txtDateCount).Text.Trim()) : 0);
			_checkout.MinReward = ((((Control)txtPriceMin).Text.Trim() == "") ? 0m : decimal.Parse(((Control)txtPriceMin).Text.Trim()));
			_checkout.MaxReward = ((((Control)txtPriceMax).Text.Trim() == "") ? 0m : decimal.Parse(((Control)txtPriceMax).Text.Trim()));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		if (fillCheckout)
		{
			FillCheckout();
		}
		return _nextPage ?? (_nextPage = new FrmCheckoutSanavat(((CacheBaseForm)this).ContextGuid, _font, _checkout));
	}

	private void ChkReward_CheckedChanged(object sender, EventArgs e)
	{
		RadioButton obj = rbReward1;
		bool enabled = (((Control)rbReward2).Enabled = chkReward.Checked);
		((Control)obj).Enabled = enabled;
		if (chkReward.Checked)
		{
			((Control)txtFixPrice).Enabled = true;
			return;
		}
		TextBoxFa obj2 = txtFixPrice;
		NumericUpDown obj3 = txtDateCount;
		TextBoxFa obj4 = txtPriceMax;
		bool flag2 = (((Control)txtPriceMin).Enabled = false);
		bool flag4 = (((Control)obj4).Enabled = flag2);
		enabled = (((Control)obj3).Enabled = flag4);
		((Control)obj2).Enabled = enabled;
	}

	private void RbReward1_CheckedChanged(object sender, EventArgs e)
	{
		((Control)txtFixPrice).Enabled = ((Control)rbReward1).Enabled;
	}

	private void RbReward2_CheckedChanged(object sender, EventArgs e)
	{
		NumericUpDown obj = txtDateCount;
		TextBoxFa obj2 = txtPriceMin;
		bool flag = (((Control)txtPriceMax).Enabled = rbReward2.Checked);
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
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Expected O, but got Unknown
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Expected O, but got Unknown
		//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Expected O, but got Unknown
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baa: Expected O, but got Unknown
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Expected O, but got Unknown
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d60: Expected O, but got Unknown
		//IL_0e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e57: Expected O, but got Unknown
		//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4e: Expected O, but got Unknown
		//IL_1038: Unknown result type (might be due to invalid IL or missing references)
		//IL_1042: Expected O, but got Unknown
		//IL_112b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1135: Expected O, but got Unknown
		//IL_121f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1229: Expected O, but got Unknown
		//IL_1316: Unknown result type (might be due to invalid IL or missing references)
		//IL_1320: Expected O, but got Unknown
		//IL_1435: Unknown result type (might be due to invalid IL or missing references)
		//IL_143f: Expected O, but got Unknown
		//IL_151a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1524: Expected O, but got Unknown
		//IL_1611: Unknown result type (might be due to invalid IL or missing references)
		//IL_161b: Expected O, but got Unknown
		//IL_1708: Unknown result type (might be due to invalid IL or missing references)
		//IL_1712: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		groupBox1 = new GroupBox();
		txtDateCount = new NumericUpDown();
		txtPriceMax = new TextBoxFa();
		txtPriceMin = new TextBoxFa();
		txtFixPrice = new TextBoxFa();
		rbReward2 = new RadioButton();
		rbReward1 = new RadioButton();
		labelX8 = new LabelX();
		labelX7 = new LabelX();
		labelX9 = new LabelX();
		labelX10 = new LabelX();
		labelX5 = new LabelX();
		labelX6 = new LabelX();
		labelX1 = new LabelX();
		chkReward = new CheckBoxX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
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
		((Control)groupPanel4).Controls.Add((Control)(object)chkReward);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX2);
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
		((Control)groupPanel4).Text = "محاسبه عیدی و پاداش";
		((Control)groupBox1).Controls.Add((Control)(object)txtDateCount);
		((Control)groupBox1).Controls.Add((Control)(object)txtPriceMax);
		((Control)groupBox1).Controls.Add((Control)(object)txtPriceMin);
		((Control)groupBox1).Controls.Add((Control)(object)txtFixPrice);
		((Control)groupBox1).Controls.Add((Control)(object)rbReward2);
		((Control)groupBox1).Controls.Add((Control)(object)rbReward1);
		((Control)groupBox1).Controls.Add((Control)(object)labelX8);
		((Control)groupBox1).Controls.Add((Control)(object)labelX7);
		((Control)groupBox1).Controls.Add((Control)(object)labelX9);
		((Control)groupBox1).Controls.Add((Control)(object)labelX10);
		((Control)groupBox1).Controls.Add((Control)(object)labelX5);
		((Control)groupBox1).Controls.Add((Control)(object)labelX6);
		((Control)groupBox1).Controls.Add((Control)(object)labelX1);
		((Control)groupBox1).Location = new Point(13, 211);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(554, 110);
		((Control)groupBox1).TabIndex = 16;
		groupBox1.TabStop = false;
		((Control)txtDateCount).Enabled = false;
		((Control)txtDateCount).Font = new Font("B Titr", 8f);
		((Control)txtDateCount).Location = new Point(339, 75);
		txtDateCount.Maximum = new decimal(new int[4] { 366, 0, 0, 0 });
		((Control)txtDateCount).Name = "txtDateCount";
		((Control)txtDateCount).Size = new Size(58, 26);
		((Control)txtDateCount).TabIndex = 3;
		((UpDownBase)txtDateCount).TextAlign = (HorizontalAlignment)2;
		txtDateCount.Value = new decimal(new int[4] { 60, 0, 0, 0 });
		txtPriceMax.AllowEmpty = true;
		txtPriceMax.BackColorTextBoxWhenFocus = Color.Yellow;
		txtPriceMax.BackColorTextBoxWhenLostFocus = Color.White;
		txtPriceMax.BackcolorTooltip = Color.Red;
		txtPriceMax.CurrencyFormat = "";
		((Control)txtPriceMax).Enabled = false;
		((Control)txtPriceMax).Font = new Font("B Titr", 8f);
		((Control)txtPriceMax).ForeColor = Color.Green;
		txtPriceMax.ForeColorTooltip = Color.Black;
		txtPriceMax.GetColoresFromSettingDLL = true;
		txtPriceMax.HandleKeydown = true;
		((Control)txtPriceMax).Location = new Point(31, 74);
		((TextBoxBase)txtPriceMax).MaxLength = 19;
		((Control)txtPriceMax).Name = "txtPriceMax";
		txtPriceMax.SendShiftTabAfterDown = true;
		txtPriceMax.SendShiftTabAfterEnter = true;
		txtPriceMax.SendTabAfterEnter = true;
		txtPriceMax.SendTabAfterUP = true;
		txtPriceMax.ShowHorofi = (ENShowHorofi)3;
		txtPriceMax.ShowToolTipText = true;
		((Control)txtPriceMax).Size = new Size(62, 26);
		((Control)txtPriceMax).TabIndex = 5;
		((Control)txtPriceMax).Text = "45480000";
		((TextBox)txtPriceMax).TextAlign = (HorizontalAlignment)2;
		txtPriceMax.TootTipDelay = 1000;
		((Control)txtPriceMax).Text = "45480000";
		txtPriceMin.AllowEmpty = true;
		txtPriceMin.BackColorTextBoxWhenFocus = Color.Yellow;
		txtPriceMin.BackColorTextBoxWhenLostFocus = Color.White;
		txtPriceMin.BackcolorTooltip = Color.Red;
		txtPriceMin.CurrencyFormat = "";
		((Control)txtPriceMin).Enabled = false;
		((Control)txtPriceMin).Font = new Font("B Titr", 8f);
		((Control)txtPriceMin).ForeColor = Color.Green;
		txtPriceMin.ForeColorTooltip = Color.Black;
		txtPriceMin.GetColoresFromSettingDLL = true;
		txtPriceMin.HandleKeydown = true;
		((Control)txtPriceMin).Location = new Point(201, 74);
		((TextBoxBase)txtPriceMin).MaxLength = 19;
		((Control)txtPriceMin).Name = "txtPriceMin";
		txtPriceMin.SendShiftTabAfterDown = true;
		txtPriceMin.SendShiftTabAfterEnter = true;
		txtPriceMin.SendTabAfterEnter = true;
		txtPriceMin.SendTabAfterUP = true;
		txtPriceMin.ShowHorofi = (ENShowHorofi)3;
		txtPriceMin.ShowToolTipText = true;
		((Control)txtPriceMin).Size = new Size(62, 26);
		((Control)txtPriceMin).TabIndex = 4;
		((Control)txtPriceMin).Text = "30320000";
		((TextBox)txtPriceMin).TextAlign = (HorizontalAlignment)2;
		txtPriceMin.TootTipDelay = 1000;
		((Control)txtPriceMin).Text = "30320000";
		txtFixPrice.AllowEmpty = true;
		txtFixPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		txtFixPrice.BackColorTextBoxWhenLostFocus = Color.White;
		txtFixPrice.BackcolorTooltip = Color.Red;
		txtFixPrice.CurrencyFormat = "";
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
		((Control)txtFixPrice).Text = "30320000";
		((TextBox)txtFixPrice).TextAlign = (HorizontalAlignment)2;
		txtFixPrice.TootTipDelay = 1000;
		((Control)txtFixPrice).Text = "30320000";
		((Control)rbReward2).BackColor = Color.Transparent;
		((Control)rbReward2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbReward2).ForeColor = Color.Navy;
		((Control)rbReward2).Location = new Point(222, 52);
		((Control)rbReward2).Name = "rbReward2";
		((Control)rbReward2).Size = new Size(296, 22);
		((Control)rbReward2).TabIndex = 2;
		((Control)rbReward2).Text = "محاسبه عیدی و پاداش بر اساس قرارداد جاری";
		((ButtonBase)rbReward2).UseVisualStyleBackColor = false;
		rbReward2.CheckedChanged += RbReward2_CheckedChanged;
		((Control)rbReward1).BackColor = Color.Transparent;
		rbReward1.Checked = true;
		((Control)rbReward1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)rbReward1).ForeColor = Color.Navy;
		((Control)rbReward1).Location = new Point(222, 0);
		((Control)rbReward1).Name = "rbReward1";
		((Control)rbReward1).Size = new Size(296, 22);
		((Control)rbReward1).TabIndex = 0;
		rbReward1.TabStop = true;
		((Control)rbReward1).Text = "محاسبه عیدی و پاداش بر اساس حداقل قانون کار";
		((ButtonBase)rbReward1).UseVisualStyleBackColor = false;
		rbReward1.CheckedChanged += RbReward1_CheckedChanged;
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
		((Control)labelX8).Text = "حداقل";
		labelX8.WordWrap = true;
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).ForeColor = Color.Navy;
		((Control)labelX7).Location = new Point(307, 69);
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
		((Control)labelX10).Text = "[ریال]";
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
		((Control)labelX5).Text = "[ریال]";
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
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Font = new Font("Tahoma", 8f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX1).ForeColor = Color.Navy;
		((Control)labelX1).Location = new Point(238, 18);
		((Control)labelX1).Name = "labelX1";
		labelX1.PaddingBottom = 5;
		labelX1.PaddingLeft = 5;
		labelX1.PaddingRight = 5;
		labelX1.PaddingTop = 5;
		((Control)labelX1).Size = new Size(258, 38);
		((Control)labelX1).TabIndex = 8;
		((Control)labelX1).Text = "[مبلغ ثابت عیدی و پاداش به ازای 12 ماه کارکرد] :";
		labelX1.WordWrap = true;
		((Control)chkReward).BackColor = Color.Transparent;
		((BaseItemControl)chkReward).BackgroundStyle.Class = "";
		((BaseItemControl)chkReward).BackgroundStyle.CornerType = (eCornerType)1;
		chkReward.Checked = true;
		chkReward.CheckState = (CheckState)1;
		chkReward.CheckValue = "Y";
		((Control)chkReward).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)chkReward).Location = new Point(388, 171);
		((Control)chkReward).Name = "chkReward";
		((Control)chkReward).Size = new Size(169, 34);
		((BaseItemControl)chkReward).Style = (eDotNetBarStyle)9;
		((Control)chkReward).TabIndex = 0;
		((Control)chkReward).Text = "[عیدی و پاداش محاسبه شود] ";
		chkReward.TextColor = Color.Navy;
		chkReward.CheckedChanged += ChkReward_CheckedChanged;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).ForeColor = Color.Black;
		((Control)labelX4).Location = new Point(12, 111);
		((Control)labelX4).Name = "labelX4";
		labelX4.PaddingBottom = 5;
		labelX4.PaddingLeft = 5;
		labelX4.PaddingRight = 5;
		labelX4.PaddingTop = 5;
		((Control)labelX4).Size = new Size(557, 38);
		((Control)labelX4).TabIndex = 8;
		((Control)labelX4).Text = "[ توجه داشته باشید عیدی و پاداش می تواند به حداکثر معادل 90 روز کاری افزایش یابد .]";
		labelX4.WordWrap = true;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).ForeColor = Color.Black;
		((Control)labelX3).Location = new Point(9, 78);
		((Control)labelX3).Name = "labelX3";
		labelX3.PaddingBottom = 5;
		labelX3.PaddingLeft = 5;
		labelX3.PaddingRight = 5;
		labelX3.PaddingTop = 5;
		((Control)labelX3).Size = new Size(557, 34);
		((Control)labelX3).TabIndex = 8;
		((Control)labelX3).Text = "[به یکسال کاری و معادل 60 روز حداقل دستمزد قانون کارسال جاری به پرسنل پرداخت نمایند . ]";
		labelX3.WordWrap = true;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).ForeColor = Color.Black;
		((Control)labelX2).Location = new Point(32, 46);
		((Control)labelX2).Name = "labelX2";
		labelX2.PaddingBottom = 5;
		labelX2.PaddingLeft = 5;
		labelX2.PaddingRight = 5;
		labelX2.PaddingTop = 5;
		((Control)labelX2).Size = new Size(537, 31);
		((Control)labelX2).TabIndex = 8;
		((Control)labelX2).Text = "[بر اساس قانون اداره کار و تامین اجتماعی ، کلیه کارفرمایان مکلف می باشند عیدی و پاداش را نسبت]";
		labelX2.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmCheckOutReward";
		((Control)this).Text = "FrmCheckOutReward";
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
