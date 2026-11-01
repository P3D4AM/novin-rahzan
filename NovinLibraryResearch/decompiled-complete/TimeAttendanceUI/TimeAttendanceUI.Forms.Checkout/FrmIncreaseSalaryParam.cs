using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.FLRCheckout;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmIncreaseSalaryParam : FormExtention, ICheckout
{
	private readonly TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private ICheckout _nextPage;

	private Font _font;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	private TextBoxFa TbxGroceryCoupon;

	private TextBoxFa TbxChildrenBenefit;

	private TextBoxFa TbxHousingBenefit;

	private TextBoxFa TbxIncreaseSalary;

	private NumericUpDown TbxIncreaseSalaryPercentage;

	private LabelX labelX12;

	private LabelX labelX7;

	private LabelX labelX10;

	private LabelX labelX6;

	private LabelX labelX9;

	private LabelX labelX5;

	private LabelX labelX8;

	private LabelX labelX2;

	private LabelX labelX1;

	internal LabelX labelX4;

	internal LabelX labelX3;

	internal LabelX labelX11;

	private LabelX labelX13;

	public FrmIncreaseSalaryParam(string contextGuid, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout, Font font)
		: base(contextGuid, true, true)
	{
		_font = font;
		InitializeComponent();
		_checkout = checkout;
	}

	protected override void OnLoad(EventArgs e)
	{
		((FormExtention)this).OnLoad(e);
		((Control)base.statusStrip1).Visible = false;
		FillFrmFields();
		((Control)TbxIncreaseSalary).Focus();
	}

	public void FillCheckout()
	{
		try
		{
			if (((Control)TbxHousingBenefit).Text.Trim() != "")
			{
				_checkout.HousingBenefit = Convert.ToDecimal(((Control)TbxHousingBenefit).Text);
			}
			if (((Control)TbxChildrenBenefit).Text.Trim() != "")
			{
				_checkout.ChildrenBenefit = Convert.ToDecimal(((Control)TbxChildrenBenefit).Text);
			}
			if (((Control)TbxGroceryCoupon).Text.Trim() != "")
			{
				_checkout.GroceryCoupon = Convert.ToDecimal(((Control)TbxGroceryCoupon).Text);
			}
			if (((Control)TbxIncreaseSalaryPercentage).Text.Trim() != "")
			{
				_checkout.IncreaseSalaryPercentage = Convert.ToInt32(((Control)TbxIncreaseSalaryPercentage).Text);
			}
			if (((Control)TbxIncreaseSalary).Text.Trim() != "")
			{
				_checkout.IncreaseSalary = Convert.ToDecimal(((Control)TbxIncreaseSalary).Text);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillCheckout", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmIncreaseSalaryParam.cs", 49);
		}
	}

	public void FillFrmFields()
	{
		EnableEvents(key: false);
		((Control)TbxIncreaseSalary).Text = _checkout.IncreaseSalary.ToString("N0");
		((Control)TbxIncreaseSalaryPercentage).Text = _checkout.IncreaseSalaryPercentage.ToString("N0");
		((Control)TbxHousingBenefit).Text = _checkout.HousingBenefit.ToString("N0");
		((Control)TbxChildrenBenefit).Text = _checkout.ChildrenBenefit.ToString("N0");
		((Control)TbxGroceryCoupon).Text = _checkout.GroceryCoupon.ToString("N0");
		EnableEvents();
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		if (fillCheckout)
		{
			FillCheckout();
		}
		return _nextPage ?? (_nextPage = new FrmContractTransfer(((CacheBaseForm)this).ContextGuid, _font, _checkout));
	}

	private void EnableEvents(bool key = true)
	{
		try
		{
			if (key)
			{
				((Control)TbxIncreaseSalary).TextChanged += Tbx_TextChanged;
				((Control)TbxHousingBenefit).TextChanged += Tbx_TextChanged;
				((Control)TbxChildrenBenefit).TextChanged += Tbx_TextChanged;
				((Control)TbxGroceryCoupon).TextChanged += Tbx_TextChanged;
				TbxIncreaseSalaryPercentage.ValueChanged += Tbx_TextChanged;
			}
			else
			{
				((Control)TbxIncreaseSalary).TextChanged -= Tbx_TextChanged;
				((Control)TbxHousingBenefit).TextChanged -= Tbx_TextChanged;
				((Control)TbxChildrenBenefit).TextChanged -= Tbx_TextChanged;
				((Control)TbxGroceryCoupon).TextChanged -= Tbx_TextChanged;
				TbxIncreaseSalaryPercentage.ValueChanged -= Tbx_TextChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnableEvents", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmIncreaseSalaryParam.cs", 94);
		}
	}

	private void Tbx_TextChanged(object sender, EventArgs e)
	{
		FillCheckout();
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
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Expected O, but got Unknown
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Expected O, but got Unknown
		//IL_0c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c22: Expected O, but got Unknown
		//IL_13b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13bd: Expected O, but got Unknown
		//IL_14aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b4: Expected O, but got Unknown
		//IL_15a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ab: Expected O, but got Unknown
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		TbxGroceryCoupon = new TextBoxFa();
		TbxChildrenBenefit = new TextBoxFa();
		TbxHousingBenefit = new TextBoxFa();
		TbxIncreaseSalary = new TextBoxFa();
		TbxIncreaseSalaryPercentage = new NumericUpDown();
		labelX12 = new LabelX();
		labelX7 = new LabelX();
		labelX10 = new LabelX();
		labelX6 = new LabelX();
		labelX9 = new LabelX();
		labelX5 = new LabelX();
		labelX8 = new LabelX();
		labelX13 = new LabelX();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		labelX4 = new LabelX();
		labelX3 = new LabelX();
		labelX11 = new LabelX();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((ISupportInitialize)TbxIncreaseSalaryPercentage).BeginInit();
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
		((Control)groupPanel4).Controls.Add((Control)(object)TbxGroceryCoupon);
		((Control)groupPanel4).Controls.Add((Control)(object)TbxChildrenBenefit);
		((Control)groupPanel4).Controls.Add((Control)(object)TbxHousingBenefit);
		((Control)groupPanel4).Controls.Add((Control)(object)TbxIncreaseSalary);
		((Control)groupPanel4).Controls.Add((Control)(object)TbxIncreaseSalaryPercentage);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX12);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX7);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX10);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX6);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX9);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX5);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX8);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX13);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX11);
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
		((Control)groupPanel4).Text = "تنظیمات تمدید قرارداد";
		TbxGroceryCoupon.AllowEmpty = true;
		TbxGroceryCoupon.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxGroceryCoupon.BackColorTextBoxWhenLostFocus = Color.White;
		TbxGroceryCoupon.BackcolorTooltip = Color.Red;
		TbxGroceryCoupon.CurrencyFormat = "";
		((Control)TbxGroceryCoupon).Font = new Font("B Titr", 10f, (FontStyle)1);
		((Control)TbxGroceryCoupon).ForeColor = Color.Green;
		TbxGroceryCoupon.ForeColorTooltip = Color.Black;
		TbxGroceryCoupon.GetColoresFromSettingDLL = true;
		TbxGroceryCoupon.HandleKeydown = true;
		((Control)TbxGroceryCoupon).Location = new Point(314, 314);
		((TextBoxBase)TbxGroceryCoupon).MaxLength = 9;
		((Control)TbxGroceryCoupon).Name = "TbxGroceryCoupon";
		TbxGroceryCoupon.SendShiftTabAfterDown = true;
		TbxGroceryCoupon.SendShiftTabAfterEnter = true;
		TbxGroceryCoupon.SendTabAfterEnter = true;
		TbxGroceryCoupon.SendTabAfterUP = true;
		TbxGroceryCoupon.ShowHorofi = (ENShowHorofi)3;
		TbxGroceryCoupon.ShowToolTipText = true;
		((Control)TbxGroceryCoupon).Size = new Size(119, 31);
		((Control)TbxGroceryCoupon).TabIndex = 4;
		((Control)TbxGroceryCoupon).Text = "4000000";
		TbxGroceryCoupon.TootTipDelay = 1000;
		((Control)TbxGroceryCoupon).Text = "4000000";
		((Control)TbxGroceryCoupon).TextChanged += Tbx_TextChanged;
		TbxChildrenBenefit.AllowEmpty = true;
		TbxChildrenBenefit.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxChildrenBenefit.BackColorTextBoxWhenLostFocus = Color.White;
		TbxChildrenBenefit.BackcolorTooltip = Color.Red;
		TbxChildrenBenefit.CurrencyFormat = "";
		((Control)TbxChildrenBenefit).Font = new Font("B Titr", 10f, (FontStyle)1);
		((Control)TbxChildrenBenefit).ForeColor = Color.Green;
		TbxChildrenBenefit.ForeColorTooltip = Color.Black;
		TbxChildrenBenefit.GetColoresFromSettingDLL = true;
		TbxChildrenBenefit.HandleKeydown = true;
		((Control)TbxChildrenBenefit).Location = new Point(314, 270);
		((TextBoxBase)TbxChildrenBenefit).MaxLength = 9;
		((Control)TbxChildrenBenefit).Name = "TbxChildrenBenefit";
		TbxChildrenBenefit.SendShiftTabAfterDown = true;
		TbxChildrenBenefit.SendShiftTabAfterEnter = true;
		TbxChildrenBenefit.SendTabAfterEnter = true;
		TbxChildrenBenefit.SendTabAfterUP = true;
		TbxChildrenBenefit.ShowHorofi = (ENShowHorofi)3;
		TbxChildrenBenefit.ShowToolTipText = true;
		((Control)TbxChildrenBenefit).Size = new Size(119, 31);
		((Control)TbxChildrenBenefit).TabIndex = 3;
		((Control)TbxChildrenBenefit).Text = "1835427";
		TbxChildrenBenefit.TootTipDelay = 1000;
		((Control)TbxChildrenBenefit).Text = "1835427";
		((Control)TbxChildrenBenefit).TextChanged += Tbx_TextChanged;
		TbxHousingBenefit.AllowEmpty = true;
		TbxHousingBenefit.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxHousingBenefit.BackColorTextBoxWhenLostFocus = Color.White;
		TbxHousingBenefit.BackcolorTooltip = Color.Red;
		TbxHousingBenefit.CurrencyFormat = "";
		((Control)TbxHousingBenefit).Font = new Font("B Titr", 10f, (FontStyle)1);
		((Control)TbxHousingBenefit).ForeColor = Color.Green;
		TbxHousingBenefit.ForeColorTooltip = Color.Black;
		TbxHousingBenefit.GetColoresFromSettingDLL = true;
		TbxHousingBenefit.HandleKeydown = true;
		((Control)TbxHousingBenefit).Location = new Point(314, 226);
		((TextBoxBase)TbxHousingBenefit).MaxLength = 9;
		((Control)TbxHousingBenefit).Name = "TbxHousingBenefit";
		TbxHousingBenefit.SendShiftTabAfterDown = true;
		TbxHousingBenefit.SendShiftTabAfterEnter = true;
		TbxHousingBenefit.SendTabAfterEnter = true;
		TbxHousingBenefit.SendTabAfterUP = true;
		TbxHousingBenefit.ShowHorofi = (ENShowHorofi)3;
		TbxHousingBenefit.ShowToolTipText = true;
		((Control)TbxHousingBenefit).Size = new Size(119, 31);
		((Control)TbxHousingBenefit).TabIndex = 2;
		((Control)TbxHousingBenefit).Text = "2000000";
		TbxHousingBenefit.TootTipDelay = 1000;
		((Control)TbxHousingBenefit).Text = "2000000";
		((Control)TbxHousingBenefit).TextChanged += Tbx_TextChanged;
		TbxIncreaseSalary.AllowEmpty = true;
		TbxIncreaseSalary.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxIncreaseSalary.BackColorTextBoxWhenLostFocus = Color.White;
		TbxIncreaseSalary.BackcolorTooltip = Color.Red;
		TbxIncreaseSalary.CurrencyFormat = "";
		((Control)TbxIncreaseSalary).Font = new Font("B Titr", 10f, (FontStyle)1);
		((Control)TbxIncreaseSalary).ForeColor = Color.Green;
		TbxIncreaseSalary.ForeColorTooltip = Color.Black;
		TbxIncreaseSalary.GetColoresFromSettingDLL = true;
		TbxIncreaseSalary.HandleKeydown = true;
		((Control)TbxIncreaseSalary).Location = new Point(82, 185);
		((TextBoxBase)TbxIncreaseSalary).MaxLength = 10;
		((TextBoxBase)TbxIncreaseSalary).Multiline = true;
		((Control)TbxIncreaseSalary).Name = "TbxIncreaseSalary";
		TbxIncreaseSalary.SendShiftTabAfterDown = true;
		TbxIncreaseSalary.SendShiftTabAfterEnter = true;
		TbxIncreaseSalary.SendTabAfterEnter = true;
		TbxIncreaseSalary.SendTabAfterUP = true;
		TbxIncreaseSalary.ShowHorofi = (ENShowHorofi)3;
		TbxIncreaseSalary.ShowToolTipText = true;
		((Control)TbxIncreaseSalary).Size = new Size(119, 31);
		((Control)TbxIncreaseSalary).TabIndex = 0;
		TbxIncreaseSalary.TootTipDelay = 1000;
		((Control)TbxIncreaseSalary).Text = "";
		((Control)TbxIncreaseSalary).TextChanged += Tbx_TextChanged;
		((Control)TbxIncreaseSalaryPercentage).Anchor = (AnchorStyles)13;
		((Control)TbxIncreaseSalaryPercentage).Font = new Font("B Titr", 10f, (FontStyle)1);
		((Control)TbxIncreaseSalaryPercentage).Location = new Point(314, 185);
		((Control)TbxIncreaseSalaryPercentage).Name = "TbxIncreaseSalaryPercentage";
		((Control)TbxIncreaseSalaryPercentage).Size = new Size(119, 31);
		((Control)TbxIncreaseSalaryPercentage).TabIndex = 1;
		((UpDownBase)TbxIncreaseSalaryPercentage).TextAlign = (HorizontalAlignment)2;
		TbxIncreaseSalaryPercentage.Value = new decimal(new int[4] { 15, 0, 0, 0 });
		TbxIncreaseSalaryPercentage.ValueChanged += Tbx_TextChanged;
		((Control)labelX12).BackColor = Color.Transparent;
		((BaseItemControl)labelX12).BackgroundStyle.Class = "";
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).ForeColor = Color.Navy;
		((Control)labelX12).Location = new Point(288, 309);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(26, 37);
		((Control)labelX12).TabIndex = 31;
		((Control)labelX12).Text = "ریال";
		labelX12.TextAlignment = (StringAlignment)2;
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).ForeColor = Color.Navy;
		((Control)labelX7).Location = new Point(438, 315);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(126, 37);
		((Control)labelX7).TabIndex = 28;
		((Control)labelX7).Text = "بن (خوار و بار):";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.Class = "";
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).ForeColor = Color.Navy;
		((Control)labelX10).Location = new Point(288, 265);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(26, 37);
		((Control)labelX10).TabIndex = 31;
		((Control)labelX10).Text = "ریال";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)labelX6).BackColor = Color.Transparent;
		((BaseItemControl)labelX6).BackgroundStyle.Class = "";
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).ForeColor = Color.Navy;
		((Control)labelX6).Location = new Point(438, 269);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(126, 37);
		((Control)labelX6).TabIndex = 29;
		((Control)labelX6).Text = "حق اولاد:";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.Class = "";
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).ForeColor = Color.Navy;
		((Control)labelX9).Location = new Point(288, 221);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(26, 37);
		((Control)labelX9).TabIndex = 31;
		((Control)labelX9).Text = "ریال";
		labelX9.TextAlignment = (StringAlignment)2;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).ForeColor = Color.Navy;
		((Control)labelX5).Location = new Point(438, 223);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(126, 37);
		((Control)labelX5).TabIndex = 30;
		((Control)labelX5).Text = "حق مسکن:";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.Class = "";
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).ForeColor = Color.Navy;
		((Control)labelX8).Location = new Point(56, 180);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(26, 37);
		((Control)labelX8).TabIndex = 31;
		((Control)labelX8).Text = "ریال";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)labelX13).BackColor = Color.Transparent;
		((BaseItemControl)labelX13).BackgroundStyle.Class = "";
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).ForeColor = Color.Navy;
		((Control)labelX13).Location = new Point(207, 180);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(69, 37);
		((Control)labelX13).TabIndex = 31;
		((Control)labelX13).Text = "+   مبلغ ثابت:";
		labelX13.TextAlignment = (StringAlignment)2;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).ForeColor = Color.Navy;
		((Control)labelX2).Location = new Point(288, 180);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(33, 37);
		((Control)labelX2).TabIndex = 31;
		((Control)labelX2).Text = "درصد   ";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).ForeColor = Color.Navy;
		((Control)labelX1).Location = new Point(438, 177);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(126, 37);
		((Control)labelX1).TabIndex = 32;
		((Control)labelX1).Text = "افزایش پایه حقوق:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).ForeColor = Color.Black;
		((Control)labelX4).Location = new Point(2, 99);
		((Control)labelX4).Name = "labelX4";
		labelX4.PaddingBottom = 5;
		labelX4.PaddingLeft = 5;
		labelX4.PaddingRight = 5;
		labelX4.PaddingTop = 5;
		((Control)labelX4).Size = new Size(558, 31);
		((Control)labelX4).TabIndex = 25;
		((Control)labelX4).Text = "و مزایا گفته می شود . ";
		labelX4.WordWrap = true;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).ForeColor = Color.Black;
		((Control)labelX3).Location = new Point(7, 61);
		((Control)labelX3).Name = "labelX3";
		labelX3.PaddingBottom = 5;
		labelX3.PaddingLeft = 5;
		labelX3.PaddingRight = 5;
		labelX3.PaddingTop = 5;
		((Control)labelX3).Size = new Size(558, 31);
		((Control)labelX3).TabIndex = 26;
		((Control)labelX3).Text = "مسکن ، خواربار ، ایاب و ذهاب ، مزایای غیر نقدی ، پاداش افزایش تولید ، سود سالانه و نظایر اینها، حقوق";
		labelX3.WordWrap = true;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX11).BackgroundStyle.Class = "";
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX11).ForeColor = Color.Black;
		((Control)labelX11).Location = new Point(7, 25);
		((Control)labelX11).Name = "labelX11";
		labelX11.PaddingBottom = 5;
		labelX11.PaddingLeft = 5;
		labelX11.PaddingRight = 5;
		labelX11.PaddingTop = 5;
		((Control)labelX11).Size = new Size(558, 31);
		((Control)labelX11).TabIndex = 27;
		((Control)labelX11).Text = "کلیه پرداخت های قانونی به کارگر به اعتبار قرارداد کار اعم از مزد یا حقوق ،کمک عائله مندی ، هزینه های  ";
		labelX11.WordWrap = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmIncreaseSalaryParam";
		((Control)this).Text = "FrmIncreaseSalaryParam";
		((Control)this).Controls.SetChildIndex((Control)(object)PanelEx1, 0);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupPanel4).PerformLayout();
		((ISupportInitialize)TbxIncreaseSalaryPercentage).EndInit();
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
