using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Forms.Taradod;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmCheckoutDetails : BaseForm
{
	private readonly int _gharardadId;

	private CheckoutDet _checkoutDet;

	private TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private Font _font;

	private IContainer components;

	internal LabelX LabelX1;

	private BindingSource CheckoutPersonBindingSource;

	private PanelEx panelEx1;

	private TabControl tabControl1;

	private TabControlPanel tabControlPanel1;

	private LabelX labelX12;

	internal DateBoxFa TbxCheckoutDate;

	internal LabelX labelX17;

	private TabItem tabItem1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private LabelX labelX24;

	private LabelX labelX23;

	private LabelX labelX19;

	internal LabelX labelX4;

	internal DateBoxFa TbxEndDate;

	internal LabelX labelX3;

	internal DateBoxFa TbxStartDate;

	internal LabelX labelX2;

	private LabelX labelX5;

	private LabelX labelX26;

	private TextBoxFa TbxSumCheckoutPrice;

	private LabelX labelX25;

	private TextBoxFa TbxSumRewardPrice;

	private TextBoxFa TbxSumSanavatPrice;

	private TextBoxFa TbxSumVacationPrice;

	internal ButtonX BtnDetailMorakhasi;

	internal ButtonX BtnDetailSaati;

	private LabelX LblVacationLeft;

	private LabelX labelX7;

	private LabelX TbxSumVacationDaily;

	private LabelX TbxVacationLeft;

	private LabelX TbxSumVacationHourly;

	private LabelX labelX9;

	private TextBoxFa TbxVacationPrice;

	private ButtonX BtnAccept2;

	private TextBox TbxNationalCode;

	private TextBox TbxName;

	private TextBox TbxDurationDate;

	public FrmCheckoutDetails(string contextGuid, Font font, int gharardadId, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout)
		: base(contextGuid, (BaseFormConfig)null)
	{
		try
		{
			_font = font;
			InitializeComponent();
			_gharardadId = gharardadId;
			_checkout = checkout;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutDetails.cs", 39);
		}
	}

	protected override async void OnLoad(EventArgs e)
	{
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadMain)
		{
			((Form)this).Close();
			return;
		}
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Vacation.VacationMain)
		{
			((Form)this).Close();
			return;
		}
		if (_checkout != null)
		{
			_checkoutDet = (await _checkout.GetCheckoutDetListAsync()).First((CheckoutDet p) => p.ContractId == _gharardadId);
		}
		if (_checkoutDet.Contract.ID <= 0)
		{
			Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, "[شخص مورد نظر در این تاریخ قراردادی ندارد و نمیتوان مانده مرخصی را محاسبه نمود]", "", 3);
			((Form)this).Close();
		}
		_003C_003En__0(e);
		await FillFormAsync();
	}

	private async Task FillFormAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			GharardadNew contract = _checkoutDet.Contract;
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson = contract.ClsPerson;
			string _TbxName = ((PersonBase)clsPerson).FullName;
			string _TbxNationalCode = ((PersonBase)clsPerson).CodeMeli;
			string _TbxCheckoutDate = _checkoutDet.CheckoutDateSh;
			string _TbxStartDate = Calendar.MiladiToCalture(contract.DateStartM);
			string _TbxEndDate = Calendar.MiladiToCalture(contract.DateFinishM);
			string _TbxDurationDate = _checkoutDet.Duration.ToString("N0");
			string _TbxVacationPrice = _checkoutDet.VacationPrice.ToString("N0");
			string _TbxSumRewardPrice = _checkoutDet.SumRewardPrice.ToString("N0");
			string _TbxSumSanavatPrice = _checkoutDet.SumSanavatPrice.ToString("N0");
			string _TbxSumVacationPrice = _checkoutDet.SumVacationPrice.ToString("N0");
			string _TbxSumVacationDaily = _checkoutDet.SumVacationDaily.ToString();
			string _TbxVacationLeft = _checkoutDet.SumVacationLeft.ToString("0.00");
			string _TbxSumVacationHourly = (_checkoutDet.SumVacationHourly % 60).ToString().PadLeft(2, '0') + " : " + (_checkoutDet.SumVacationHourly / 60).ToString().PadLeft(2, '0');
			string _TbxSumCheckoutPrice = _checkoutDet.SumAll.ToString("N0");
			Color _TbxVacationLeftColor = ((_checkoutDet.SumVacationLeft > 0.0) ? Color.Green : Color.Red);
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((BaseForm)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					EnableEvents(key: false);
					if (!((BaseForm)this).IsDisposed && !token.IsCancellationRequested)
					{
						((Control)TbxName).Text = _TbxName;
						((Control)TbxNationalCode).Text = _TbxNationalCode;
						((Control)TbxCheckoutDate).Text = _TbxCheckoutDate;
						((Control)TbxStartDate).Text = _TbxStartDate;
						((Control)TbxEndDate).Text = _TbxEndDate;
						((Control)TbxDurationDate).Text = _TbxDurationDate;
						((Control)TbxVacationPrice).Text = _TbxVacationPrice;
						((Control)TbxSumRewardPrice).Text = _TbxSumRewardPrice;
						((Control)TbxSumSanavatPrice).Text = _TbxSumSanavatPrice;
						((Control)TbxSumVacationPrice).Text = _TbxSumVacationPrice;
						((Control)TbxSumVacationDaily).Text = _TbxSumVacationDaily;
						((Control)TbxVacationLeft).Text = _TbxVacationLeft;
						((Control)TbxSumVacationHourly).Text = _TbxSumVacationHourly;
						((Control)TbxSumCheckoutPrice).Text = _TbxSumCheckoutPrice;
						((Control)TbxVacationLeft).ForeColor = _TbxVacationLeftColor;
						EnableEvents();
					}
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutDetails.cs", 119);
		}
	}

	private async void BtnDetailMorakhasi_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			FrmMorakhasiDailyMain frm = new FrmMorakhasiDailyMain(((CacheBaseForm)this).ContextGuid, _font, new List<int> { _checkoutDet.Contract.PersonId }, _checkoutDet.Contract.DateStartM, _checkoutDet.Contract.DateFinishM, "", isCalcSaati: false);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			await FillFormAsync();
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDetailMorakhasi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutDetails.cs", 131);
		}
	}

	private async void BtnDetailSaati_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			FrmMor_SaatiList frm = new FrmMor_SaatiList(((CacheBaseForm)this).ContextGuid, _checkoutDet.Morakhasi.Item5);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			await FillFormAsync();
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDetailSaati_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutDetails.cs", 143);
		}
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		SaveFrmFieldInCls();
		((Form)this).DialogResult = (DialogResult)6;
		((Form)this).Close();
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void BtnAccept2_Click(object sender, EventArgs e)
	{
		SaveFrmFieldInCls();
		((Form)this).DialogResult = (DialogResult)7;
		((Form)this).Close();
	}

	private void TbxVacationPrice_TextChanged(object sender, EventArgs e)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		try
		{
			EnableEvents(key: false);
			if (string.IsNullOrEmpty(((Control)TbxVacationPrice).Text))
			{
				TextBoxFa tbxVacationPrice = TbxVacationPrice;
				string text2 = (((Control)TbxSumVacationPrice).Text = "0");
				((Control)tbxVacationPrice).Text = text2;
			}
			else
			{
				TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout = _checkout;
				if (checkout != null && (int)checkout.VacationType == 2)
				{
					((Control)TbxSumVacationPrice).Text = (Convert.ToDouble(((Control)TbxVacationPrice).Text) * _checkoutDet.SumVacationLeft).ToString("N");
				}
				else
				{
					((Control)TbxSumVacationPrice).Text = "0";
				}
			}
			EnableEvents();
			ReCalcFrmFieds();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TbxVacationPrice_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutDetails.cs", 182);
		}
	}

	private void EnableEvents(bool key = true)
	{
		try
		{
			if (key)
			{
				((Control)TbxVacationPrice).TextChanged += TbxVacationPrice_TextChanged;
				((Control)TbxSumRewardPrice).TextChanged += TbxSumRewardPrice_TextChanged;
				((Control)TbxSumSanavatPrice).TextChanged += TbxSumSanavatPrice_TextChanged;
				((Control)TbxSumVacationPrice).TextChanged += TbxSumVacationPrice_TextChanged;
			}
			else
			{
				((Control)TbxVacationPrice).TextChanged -= TbxVacationPrice_TextChanged;
				((Control)TbxSumRewardPrice).TextChanged -= TbxSumRewardPrice_TextChanged;
				((Control)TbxSumSanavatPrice).TextChanged -= TbxSumSanavatPrice_TextChanged;
				((Control)TbxSumVacationPrice).TextChanged -= TbxSumVacationPrice_TextChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "EnableEvents", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutDetails.cs", 206);
		}
	}

	private void ReCalcFrmFieds()
	{
		try
		{
			if (TbxSumCheckoutPrice != null)
			{
				((Control)TbxSumCheckoutPrice).Text = (Convert.ToDouble(((Control)TbxSumVacationPrice).Text) + Convert.ToDouble(((Control)TbxSumSanavatPrice).Text) + Convert.ToDouble(((Control)TbxSumRewardPrice).Text)).ToString("N");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReCalcFrmFieds", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutDetails.cs", 221);
		}
	}

	private void SaveFrmFieldInCls()
	{
		try
		{
			if (_checkoutDet != null)
			{
				_checkoutDet.CheckoutDateSh = ((Control)TbxCheckoutDate).Text;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveFrmFieldInCls", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmCheckoutDetails.cs", 233);
		}
	}

	private void TbxSumRewardPrice_TextChanged(object sender, EventArgs e)
	{
		ReCalcFrmFieds();
	}

	private void TbxSumSanavatPrice_TextChanged(object sender, EventArgs e)
	{
		ReCalcFrmFieds();
	}

	private void TbxSumVacationPrice_TextChanged(object sender, EventArgs e)
	{
		ReCalcFrmFieds();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Expected O, but got Unknown
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Expected O, but got Unknown
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Expected O, but got Unknown
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Expected O, but got Unknown
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da0: Expected O, but got Unknown
		//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e44: Expected O, but got Unknown
		//IL_0ee2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eec: Expected O, but got Unknown
		//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d3: Expected O, but got Unknown
		//IL_1163: Unknown result type (might be due to invalid IL or missing references)
		//IL_116d: Expected O, but got Unknown
		//IL_1261: Unknown result type (might be due to invalid IL or missing references)
		//IL_126b: Expected O, but got Unknown
		//IL_1307: Unknown result type (might be due to invalid IL or missing references)
		//IL_1311: Expected O, but got Unknown
		//IL_1404: Unknown result type (might be due to invalid IL or missing references)
		//IL_140e: Expected O, but got Unknown
		//IL_1622: Unknown result type (might be due to invalid IL or missing references)
		//IL_162c: Expected O, but got Unknown
		//IL_1833: Unknown result type (might be due to invalid IL or missing references)
		//IL_183d: Expected O, but got Unknown
		//IL_1998: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a2: Expected O, but got Unknown
		//IL_1afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b07: Expected O, but got Unknown
		//IL_1ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee2: Expected O, but got Unknown
		//IL_1fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd0: Expected O, but got Unknown
		//IL_20a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b1: Expected O, but got Unknown
		//IL_21a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b2: Expected O, but got Unknown
		//IL_22b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_22bd: Expected O, but got Unknown
		components = new Container();
		CheckoutPersonBindingSource = new BindingSource(components);
		LabelX1 = new LabelX();
		panelEx1 = new PanelEx();
		tabControl1 = new TabControl();
		tabControlPanel1 = new TabControlPanel();
		TbxDurationDate = new TextBox();
		TbxName = new TextBox();
		TbxNationalCode = new TextBox();
		TbxVacationPrice = new TextBoxFa();
		BtnDetailSaati = new ButtonX();
		LblVacationLeft = new LabelX();
		labelX7 = new LabelX();
		TbxSumVacationDaily = new LabelX();
		TbxVacationLeft = new LabelX();
		TbxSumVacationHourly = new LabelX();
		labelX9 = new LabelX();
		BtnDetailMorakhasi = new ButtonX();
		labelX4 = new LabelX();
		TbxEndDate = new DateBoxFa();
		labelX3 = new LabelX();
		TbxStartDate = new DateBoxFa();
		labelX2 = new LabelX();
		labelX5 = new LabelX();
		labelX26 = new LabelX();
		TbxSumCheckoutPrice = new TextBoxFa();
		labelX25 = new LabelX();
		TbxSumRewardPrice = new TextBoxFa();
		TbxSumSanavatPrice = new TextBoxFa();
		TbxSumVacationPrice = new TextBoxFa();
		labelX24 = new LabelX();
		labelX23 = new LabelX();
		labelX19 = new LabelX();
		labelX12 = new LabelX();
		TbxCheckoutDate = new DateBoxFa();
		labelX17 = new LabelX();
		tabItem1 = new TabItem(components);
		BtnCancel = new ButtonX();
		BtnAccept2 = new ButtonX();
		BtnAccept = new ButtonX();
		((ISupportInitialize)CheckoutPersonBindingSource).BeginInit();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)tabControl1).BeginInit();
		((Control)tabControl1).SuspendLayout();
		((Control)tabControlPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		CheckoutPersonBindingSource.DataSource = typeof(GharardadNew);
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.Class = "";
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(663, 4);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(58, 23);
		((Control)LabelX1).TabIndex = 0;
		((Control)LabelX1).Text = "[جستجو]  :";
		LabelX1.TextAlignment = (StringAlignment)2;
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)tabControl1);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept2);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(915, 561);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 1;
		((Control)tabControl1).Anchor = (AnchorStyles)15;
		((Control)tabControl1).BackColor = Color.Transparent;
		tabControl1.CanReorderTabs = true;
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel1);
		((Control)tabControl1).Location = new Point(12, 12);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedTabFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		tabControl1.SelectedTabIndex = 0;
		((Control)tabControl1).Size = new Size(891, 506);
		((Control)tabControl1).TabIndex = 0;
		tabControl1.TabLayoutType = (eTabLayoutType)1;
		tabControl1.Tabs.Add(tabItem1);
		((Control)tabControl1).TabStop = false;
		((Control)tabControl1).Text = "tabControl1";
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxDurationDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxName);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxNationalCode);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxVacationPrice);
		((Control)tabControlPanel1).Controls.Add((Control)(object)BtnDetailSaati);
		((Control)tabControlPanel1).Controls.Add((Control)(object)LblVacationLeft);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX7);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxSumVacationDaily);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxVacationLeft);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxSumVacationHourly);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX9);
		((Control)tabControlPanel1).Controls.Add((Control)(object)BtnDetailMorakhasi);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX4);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxEndDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX3);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxStartDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX2);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX5);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX26);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxSumCheckoutPrice);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX25);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxSumRewardPrice);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxSumSanavatPrice);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxSumVacationPrice);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX24);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX23);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX19);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX12);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TbxCheckoutDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX17);
		((Control)tabControlPanel1).Dock = (DockStyle)5;
		((Control)tabControlPanel1).Location = new Point(0, 26);
		((Control)tabControlPanel1).Name = "tabControlPanel1";
		((Control)tabControlPanel1).Padding = new Padding(1);
		((Control)tabControlPanel1).Size = new Size(891, 480);
		((PanelEx)tabControlPanel1).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel1).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel1).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel1).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel1).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel1).Style.GradientAngle = 90;
		((Control)tabControlPanel1).TabIndex = 0;
		tabControlPanel1.TabItem = tabItem1;
		((Control)TbxDurationDate).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxDurationDate).Location = new Point(14, 33);
		((TextBoxBase)TbxDurationDate).MaxLength = 3652;
		((Control)TbxDurationDate).Name = "TbxDurationDate";
		((TextBoxBase)TbxDurationDate).ReadOnly = true;
		((Control)TbxDurationDate).Size = new Size(119, 22);
		((Control)TbxDurationDate).TabIndex = 16;
		((Control)TbxName).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxName).Location = new Point(588, 5);
		((Control)TbxName).Name = "TbxName";
		((TextBoxBase)TbxName).ReadOnly = true;
		((Control)TbxName).Size = new Size(138, 22);
		((Control)TbxName).TabIndex = 0;
		TbxName.TextAlign = (HorizontalAlignment)1;
		((Control)TbxNationalCode).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxNationalCode).Location = new Point(589, 32);
		((Control)TbxNationalCode).Name = "TbxNationalCode";
		((TextBoxBase)TbxNationalCode).ReadOnly = true;
		((Control)TbxNationalCode).Size = new Size(137, 22);
		((Control)TbxNationalCode).TabIndex = 1;
		TbxVacationPrice.AllowEmpty = true;
		TbxVacationPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxVacationPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TbxVacationPrice.BackcolorTooltip = Color.Red;
		TbxVacationPrice.CurrencyFormat = "";
		((Control)TbxVacationPrice).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxVacationPrice).ForeColor = Color.Green;
		TbxVacationPrice.ForeColorTooltip = Color.Black;
		TbxVacationPrice.GetColoresFromSettingDLL = true;
		TbxVacationPrice.HandleKeydown = true;
		((Control)TbxVacationPrice).Location = new Point(323, 153);
		((TextBoxBase)TbxVacationPrice).MaxLength = 7;
		((Control)TbxVacationPrice).Name = "TbxVacationPrice";
		TbxVacationPrice.SendShiftTabAfterDown = true;
		TbxVacationPrice.SendShiftTabAfterEnter = true;
		TbxVacationPrice.SendTabAfterEnter = true;
		TbxVacationPrice.SendTabAfterUP = true;
		TbxVacationPrice.ShowHorofi = (ENShowHorofi)3;
		TbxVacationPrice.ShowToolTipText = true;
		((Control)TbxVacationPrice).Size = new Size(120, 22);
		((Control)TbxVacationPrice).TabIndex = 11;
		TbxVacationPrice.TootTipDelay = 1000;
		((Control)TbxVacationPrice).Text = "";
		((Control)BtnDetailSaati).AccessibleRole = (AccessibleRole)43;
		BtnDetailSaati.ColorTable = (eButtonColor)6;
		BtnDetailSaati.Image = (Image)(object)Resources.search2;
		((Control)BtnDetailSaati).Location = new Point(557, 114);
		((Control)BtnDetailSaati).Name = "BtnDetailSaati";
		((Control)BtnDetailSaati).Size = new Size(24, 24);
		((PopupItemControl)BtnDetailSaati).Style = (eDotNetBarStyle)9;
		((Control)BtnDetailSaati).TabIndex = 9;
		((Control)BtnDetailSaati).Click += BtnDetailSaati_Click;
		((Control)LblVacationLeft).BackColor = Color.Transparent;
		((BaseItemControl)LblVacationLeft).BackgroundStyle.Class = "";
		((BaseItemControl)LblVacationLeft).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblVacationLeft).ForeColor = SystemColors.ControlText;
		((Control)LblVacationLeft).Location = new Point(670, 153);
		((Control)LblVacationLeft).Name = "LblVacationLeft";
		((Control)LblVacationLeft).Size = new Size(143, 21);
		((Control)LblVacationLeft).TabIndex = 0;
		((Control)LblVacationLeft).Text = "[مانده مرخصی استحقاقی] : ";
		LblVacationLeft.TextAlignment = (StringAlignment)2;
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(670, 115);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(194, 27);
		((Control)labelX7).TabIndex = 0;
		((Control)labelX7).Text = "[جمع مرخصی ساعتی استفاده شده] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)TbxSumVacationDaily).BackColor = Color.White;
		((BaseItemControl)TbxSumVacationDaily).BackgroundStyle.Class = "";
		((BaseItemControl)TbxSumVacationDaily).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TbxSumVacationDaily).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxSumVacationDaily).Location = new Point(589, 79);
		((Control)TbxSumVacationDaily).Name = "TbxSumVacationDaily";
		((Control)TbxSumVacationDaily).Size = new Size(57, 21);
		((Control)TbxSumVacationDaily).TabIndex = 6;
		TbxSumVacationDaily.TextAlignment = (StringAlignment)1;
		((Control)TbxVacationLeft).BackColor = Color.White;
		((BaseItemControl)TbxVacationLeft).BackgroundStyle.Class = "";
		((BaseItemControl)TbxVacationLeft).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TbxVacationLeft).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxVacationLeft).Location = new Point(589, 152);
		((Control)TbxVacationLeft).Name = "TbxVacationLeft";
		((Control)TbxVacationLeft).Size = new Size(57, 21);
		((Control)TbxVacationLeft).TabIndex = 10;
		TbxVacationLeft.TextAlignment = (StringAlignment)1;
		((Control)TbxSumVacationHourly).BackColor = Color.White;
		((BaseItemControl)TbxSumVacationHourly).BackgroundStyle.Class = "";
		((BaseItemControl)TbxSumVacationHourly).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)TbxSumVacationHourly).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxSumVacationHourly).Location = new Point(589, 117);
		((Control)TbxSumVacationHourly).Name = "TbxSumVacationHourly";
		((Control)TbxSumVacationHourly).Size = new Size(57, 21);
		((Control)TbxSumVacationHourly).TabIndex = 8;
		TbxSumVacationHourly.TextAlignment = (StringAlignment)1;
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.Class = "";
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Location = new Point(672, 77);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(211, 27);
		((Control)labelX9).TabIndex = 0;
		((Control)labelX9).Text = "[جمع مرخصی استحقاقی استفاده شده] :";
		labelX9.TextAlignment = (StringAlignment)2;
		((Control)BtnDetailMorakhasi).AccessibleRole = (AccessibleRole)43;
		BtnDetailMorakhasi.ColorTable = (eButtonColor)6;
		BtnDetailMorakhasi.Image = (Image)(object)Resources.search2;
		((Control)BtnDetailMorakhasi).Location = new Point(557, 79);
		((Control)BtnDetailMorakhasi).Name = "BtnDetailMorakhasi";
		((Control)BtnDetailMorakhasi).Size = new Size(24, 23);
		((PopupItemControl)BtnDetailMorakhasi).Style = (eDotNetBarStyle)9;
		((Control)BtnDetailMorakhasi).TabIndex = 7;
		((Control)BtnDetailMorakhasi).Click += BtnDetailMorakhasi_Click;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX4).Location = new Point(139, 32);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(125, 26);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "[مدت قرارداد (روز)]:";
		labelX4.TextAlignment = (StringAlignment)2;
		TbxEndDate.BackcolorTooltip = Color.Red;
		TbxEndDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		((Control)TbxEndDate).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxEndDate).Location = new Point(323, 33);
		TbxEndDate.MiladiDate = new DateTime(2013, 3, 2, 1, 1, 1, 0);
		((Control)TbxEndDate).Name = "TbxEndDate";
		((TextBoxBase)TbxEndDate).ReadOnly = true;
		((Control)TbxEndDate).Size = new Size(120, 22);
		((Control)TbxEndDate).TabIndex = 4;
		((Control)TbxEndDate).Text = "1391/12/12";
		((TextBox)TbxEndDate).TextAlign = (HorizontalAlignment)2;
		TbxEndDate.TootTipDelay = 500;
		((Control)labelX3).Anchor = (AnchorStyles)9;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.Class = "";
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX3).Location = new Point(449, 33);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(125, 26);
		((Control)labelX3).TabIndex = 0;
		((Control)labelX3).Text = "[تاریخ پایان قرارداد]:";
		labelX3.TextAlignment = (StringAlignment)2;
		TbxStartDate.BackcolorTooltip = Color.Red;
		((Control)TbxStartDate).Enabled = false;
		TbxStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		((Control)TbxStartDate).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxStartDate).Location = new Point(323, 5);
		TbxStartDate.MiladiDate = new DateTime(2013, 3, 2, 1, 1, 1, 0);
		((Control)TbxStartDate).Name = "TbxStartDate";
		((TextBoxBase)TbxStartDate).ReadOnly = true;
		((Control)TbxStartDate).Size = new Size(121, 22);
		((Control)TbxStartDate).TabIndex = 3;
		((Control)TbxStartDate).Text = "1391/12/12";
		((TextBox)TbxStartDate).TextAlign = (HorizontalAlignment)2;
		TbxStartDate.TootTipDelay = 500;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX2).Location = new Point(450, 5);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(125, 26);
		((Control)labelX2).TabIndex = 0;
		((Control)labelX2).Text = "[تاریخ شروع قرارداد]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(139, 441);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(145, 21);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[مبلغ کل تسویه حساب]:";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)labelX26).Anchor = (AnchorStyles)9;
		((Control)labelX26).BackColor = Color.Transparent;
		((BaseItemControl)labelX26).BackgroundStyle.Class = "";
		((BaseItemControl)labelX26).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX26).Location = new Point(139, 80);
		((Control)labelX26).Name = "labelX26";
		((Control)labelX26).Size = new Size(145, 21);
		((Control)labelX26).TabIndex = 0;
		((Control)labelX26).Text = "[مبلغ عیدی]: ";
		labelX26.TextAlignment = (StringAlignment)2;
		TbxSumCheckoutPrice.AllowEmpty = true;
		TbxSumCheckoutPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxSumCheckoutPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TbxSumCheckoutPrice.BackcolorTooltip = Color.Red;
		TbxSumCheckoutPrice.CurrencyFormat = "";
		((Control)TbxSumCheckoutPrice).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxSumCheckoutPrice).ForeColor = Color.Green;
		TbxSumCheckoutPrice.ForeColorTooltip = Color.Black;
		TbxSumCheckoutPrice.GetColoresFromSettingDLL = true;
		TbxSumCheckoutPrice.HandleKeydown = true;
		((Control)TbxSumCheckoutPrice).Location = new Point(14, 439);
		((TextBoxBase)TbxSumCheckoutPrice).MaxLength = 12;
		((Control)TbxSumCheckoutPrice).Name = "TbxSumCheckoutPrice";
		((TextBoxBase)TbxSumCheckoutPrice).ReadOnly = true;
		TbxSumCheckoutPrice.SendShiftTabAfterDown = true;
		TbxSumCheckoutPrice.SendShiftTabAfterEnter = true;
		TbxSumCheckoutPrice.SendTabAfterEnter = true;
		TbxSumCheckoutPrice.SendTabAfterUP = true;
		TbxSumCheckoutPrice.ShowHorofi = (ENShowHorofi)3;
		TbxSumCheckoutPrice.ShowToolTipText = true;
		((Control)TbxSumCheckoutPrice).Size = new Size(119, 22);
		((Control)TbxSumCheckoutPrice).TabIndex = 15;
		TbxSumCheckoutPrice.TootTipDelay = 1000;
		((Control)TbxSumCheckoutPrice).Text = "";
		((Control)labelX25).Anchor = (AnchorStyles)9;
		((Control)labelX25).BackColor = Color.Transparent;
		((BaseItemControl)labelX25).BackgroundStyle.Class = "";
		((BaseItemControl)labelX25).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX25).Location = new Point(139, 118);
		((Control)labelX25).Name = "labelX25";
		((Control)labelX25).Size = new Size(145, 21);
		((Control)labelX25).TabIndex = 0;
		((Control)labelX25).Text = "[مبلغ سنوات]: ";
		labelX25.TextAlignment = (StringAlignment)2;
		TbxSumRewardPrice.AllowEmpty = true;
		TbxSumRewardPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxSumRewardPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TbxSumRewardPrice.BackcolorTooltip = Color.Red;
		TbxSumRewardPrice.CurrencyFormat = "";
		((Control)TbxSumRewardPrice).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxSumRewardPrice).ForeColor = Color.Green;
		TbxSumRewardPrice.ForeColorTooltip = Color.Black;
		TbxSumRewardPrice.GetColoresFromSettingDLL = true;
		TbxSumRewardPrice.HandleKeydown = true;
		((Control)TbxSumRewardPrice).Location = new Point(14, 81);
		((TextBoxBase)TbxSumRewardPrice).MaxLength = 11;
		((Control)TbxSumRewardPrice).Name = "TbxSumRewardPrice";
		((TextBoxBase)TbxSumRewardPrice).ReadOnly = true;
		TbxSumRewardPrice.SendShiftTabAfterDown = true;
		TbxSumRewardPrice.SendShiftTabAfterEnter = true;
		TbxSumRewardPrice.SendTabAfterEnter = true;
		TbxSumRewardPrice.SendTabAfterUP = true;
		TbxSumRewardPrice.ShowHorofi = (ENShowHorofi)3;
		TbxSumRewardPrice.ShowToolTipText = true;
		((Control)TbxSumRewardPrice).Size = new Size(119, 22);
		((Control)TbxSumRewardPrice).TabIndex = 12;
		TbxSumRewardPrice.TootTipDelay = 1000;
		((Control)TbxSumRewardPrice).Text = "";
		TbxSumSanavatPrice.AllowEmpty = true;
		TbxSumSanavatPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxSumSanavatPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TbxSumSanavatPrice.BackcolorTooltip = Color.Red;
		TbxSumSanavatPrice.CurrencyFormat = "";
		((Control)TbxSumSanavatPrice).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxSumSanavatPrice).ForeColor = Color.Green;
		TbxSumSanavatPrice.ForeColorTooltip = Color.Black;
		TbxSumSanavatPrice.GetColoresFromSettingDLL = true;
		TbxSumSanavatPrice.HandleKeydown = true;
		((Control)TbxSumSanavatPrice).Location = new Point(14, 118);
		((TextBoxBase)TbxSumSanavatPrice).MaxLength = 11;
		((Control)TbxSumSanavatPrice).Name = "TbxSumSanavatPrice";
		((TextBoxBase)TbxSumSanavatPrice).ReadOnly = true;
		TbxSumSanavatPrice.SendShiftTabAfterDown = true;
		TbxSumSanavatPrice.SendShiftTabAfterEnter = true;
		TbxSumSanavatPrice.SendTabAfterEnter = true;
		TbxSumSanavatPrice.SendTabAfterUP = true;
		TbxSumSanavatPrice.ShowHorofi = (ENShowHorofi)3;
		TbxSumSanavatPrice.ShowToolTipText = true;
		((Control)TbxSumSanavatPrice).Size = new Size(119, 22);
		((Control)TbxSumSanavatPrice).TabIndex = 13;
		TbxSumSanavatPrice.TootTipDelay = 1000;
		((Control)TbxSumSanavatPrice).Text = "";
		TbxSumVacationPrice.AllowEmpty = true;
		TbxSumVacationPrice.BackColorTextBoxWhenFocus = Color.Yellow;
		TbxSumVacationPrice.BackColorTextBoxWhenLostFocus = Color.White;
		TbxSumVacationPrice.BackcolorTooltip = Color.Red;
		TbxSumVacationPrice.CurrencyFormat = "";
		((Control)TbxSumVacationPrice).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxSumVacationPrice).ForeColor = Color.Green;
		TbxSumVacationPrice.ForeColorTooltip = Color.Black;
		TbxSumVacationPrice.GetColoresFromSettingDLL = true;
		TbxSumVacationPrice.HandleKeydown = true;
		((Control)TbxSumVacationPrice).Location = new Point(14, 153);
		((TextBoxBase)TbxSumVacationPrice).MaxLength = 10;
		((Control)TbxSumVacationPrice).Name = "TbxSumVacationPrice";
		((TextBoxBase)TbxSumVacationPrice).ReadOnly = true;
		TbxSumVacationPrice.SendShiftTabAfterDown = true;
		TbxSumVacationPrice.SendShiftTabAfterEnter = true;
		TbxSumVacationPrice.SendTabAfterEnter = true;
		TbxSumVacationPrice.SendTabAfterUP = true;
		TbxSumVacationPrice.ShowHorofi = (ENShowHorofi)3;
		TbxSumVacationPrice.ShowToolTipText = true;
		((Control)TbxSumVacationPrice).Size = new Size(119, 22);
		((Control)TbxSumVacationPrice).TabIndex = 14;
		TbxSumVacationPrice.TootTipDelay = 1000;
		((Control)TbxSumVacationPrice).Text = "";
		((Control)labelX24).Anchor = (AnchorStyles)9;
		((Control)labelX24).BackColor = Color.Transparent;
		((BaseItemControl)labelX24).BackgroundStyle.Class = "";
		((BaseItemControl)labelX24).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX24).Location = new Point(139, 153);
		((Control)labelX24).Name = "labelX24";
		((Control)labelX24).Size = new Size(145, 21);
		((Control)labelX24).TabIndex = 0;
		((Control)labelX24).Text = "[مبلغ کل مانده مرخصی]: ";
		labelX24.TextAlignment = (StringAlignment)2;
		((Control)labelX23).Anchor = (AnchorStyles)9;
		((Control)labelX23).BackColor = Color.Transparent;
		((BaseItemControl)labelX23).BackgroundStyle.Class = "";
		((BaseItemControl)labelX23).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX23).Location = new Point(449, 153);
		((Control)labelX23).Name = "labelX23";
		((Control)labelX23).Size = new Size(145, 21);
		((Control)labelX23).TabIndex = 0;
		((Control)labelX23).Text = "[مبلغ هر ساعت مرخصی] : ";
		labelX23.TextAlignment = (StringAlignment)2;
		((Control)labelX19).Anchor = (AnchorStyles)9;
		((Control)labelX19).BackColor = Color.Transparent;
		((BaseItemControl)labelX19).BackgroundStyle.Class = "";
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Location = new Point(732, 35);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).Size = new Size(59, 21);
		((Control)labelX19).TabIndex = 0;
		((Control)labelX19).Text = "[کد ملی] : ";
		labelX19.TextAlignment = (StringAlignment)2;
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((Control)labelX12).BackColor = Color.Transparent;
		((BaseItemControl)labelX12).BackgroundStyle.Class = "";
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Location = new Point(732, 4);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(125, 21);
		((Control)labelX12).TabIndex = 0;
		((Control)labelX12).Text = "[نام و نام خانوادگی] : ";
		labelX12.TextAlignment = (StringAlignment)2;
		TbxCheckoutDate.BackcolorTooltip = Color.Red;
		TbxCheckoutDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		((Control)TbxCheckoutDate).Font = new Font("Tahoma", 9f, (FontStyle)1);
		((Control)TbxCheckoutDate).Location = new Point(14, 5);
		TbxCheckoutDate.MiladiDate = new DateTime(2013, 3, 2, 1, 1, 1, 0);
		((Control)TbxCheckoutDate).Name = "TbxCheckoutDate";
		((Control)TbxCheckoutDate).Size = new Size(119, 22);
		((Control)TbxCheckoutDate).TabIndex = 2;
		((Control)TbxCheckoutDate).Text = "1391/12/12";
		((TextBox)TbxCheckoutDate).TextAlign = (HorizontalAlignment)2;
		TbxCheckoutDate.TootTipDelay = 500;
		((Control)labelX17).Anchor = (AnchorStyles)9;
		((Control)labelX17).BackColor = Color.Transparent;
		((BaseItemControl)labelX17).BackgroundStyle.Class = "";
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX17).Location = new Point(139, 5);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(125, 26);
		((Control)labelX17).TabIndex = 0;
		((Control)labelX17).Text = "[تاریخ تسویه قرارداد] : ";
		labelX17.TextAlignment = (StringAlignment)2;
		tabItem1.AttachedControl = (Control)(object)tabControlPanel1;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[اطلاعات تسویه حساب]";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnCancel).ForeColor = Color.Black;
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 524);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(100, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 2;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept2).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept2).Anchor = (AnchorStyles)6;
		BtnAccept2.ColorTable = (eButtonColor)6;
		((Control)BtnAccept2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept2).ForeColor = Color.Black;
		BtnAccept2.Image = (Image)(object)Resources.accept1;
		BtnAccept2.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept2).Location = new Point(271, 524);
		((Control)BtnAccept2).Name = "BtnAccept2";
		((Control)BtnAccept2).RightToLeft = (RightToLeft)1;
		BtnAccept2.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept2).Size = new Size(114, 34);
		((PopupItemControl)BtnAccept2).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept2).TabIndex = 1;
		((Control)BtnAccept2).Text = "[ثبت و تمام]";
		((Control)BtnAccept2).Click += BtnAccept2_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(118, 524);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(147, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 0;
		((Control)BtnAccept).Text = "[ثبت و ادامه]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(915, 561);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).MaximumSize = new Size(931, 600);
		((Control)this).MinimumSize = new Size(931, 600);
		((Control)this).Name = "FrmCheckoutDetails";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فرم تسویه حساب]";
		((ISupportInitialize)CheckoutPersonBindingSource).EndInit();
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)tabControl1).EndInit();
		tabControl1.ResumeLayout(false);
		((Control)tabControlPanel1).ResumeLayout(false);
		((Control)tabControlPanel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(EventArgs e)
	{
		((BaseForm)this).OnLoad(e);
	}
}
