using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.FLRCheckout;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceUI.Forms.Gharardads;

namespace TimeAttendanceUI.Forms.Checkout;

public class FrmContractTransfer : FormExtention, ICheckout
{
	private Font _font;

	private readonly TimeAttendanceDAL.Dal.FLRCheckout.Checkout _checkout;

	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private IContainer components;

	public PanelEx PanelEx1;

	private GroupPanel groupPanel4;

	internal LabelX labelX11;

	internal DateBoxFa TXTDateStartGharardad;

	private LabelX labelX1;

	private NumericUpDown txtContractNo;

	internal DataGridViewX DGrid;

	private LabelX labelX2;

	private BindingSource TamdidContractSource;

	private DataGridViewCheckBoxColumn DGCheck;

	private DataGridViewButtonColumn DGBtnView;

	private DataGridViewTextBoxColumn DGContractId;

	private DataGridViewTextBoxColumn DGGharardadNewNo;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewTextBoxColumn DGFullName;

	private DataGridViewTextBoxColumn DGSalary;

	private DataGridViewTextBoxColumn DGSalaryNew;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((FormExtention)this).IsDisposed;
			}
			return true;
		}
	}

	public FrmContractTransfer(string contextGuid, Font font, TimeAttendanceDAL.Dal.FLRCheckout.Checkout checkout)
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

	protected override async void OnLoad(EventArgs e)
	{
		try
		{
			_003C_003En__0(e);
			((Control)base.statusStrip1).Visible = false;
			((Control)TXTDateStartGharardad).Focus();
			((Control)TXTDateStartGharardad).Text = _checkout.ContractDateStartSh;
			NumericUpDown val = txtContractNo;
			((Control)val).Text = (await _checkout.GetFirstNewContractNoAsync()).ToString();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public void FillCheckout()
	{
		_checkout.ContractDateStartSh = ((Control)TXTDateStartGharardad).Text.Trim();
	}

	private async Task LoadGridAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<CheckoutDet> list = await _checkout.GetCheckoutDetListAsync();
			if (list != null && list.Count > 0)
			{
				new List<int> { 1, 3 };
			}
			else
			{
				TamdidContractSource.DataSource = null;
				_checkout.TamdidContractIds.Clear();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmContractTransfer.cs", 99);
		}
	}

	public ICheckout NextPage(bool fillCheckout)
	{
		if (fillCheckout)
		{
			FillCheckout();
		}
		return null;
	}

	private async void DGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		_ = 2;
		try
		{
			if (e.RowIndex >= 0)
			{
				List<CheckoutDet> list = await _checkout.GetCheckoutDetListAsync();
				if (e.ColumnIndex == ((DataGridViewBand)DGCheck).Index && ((DataGridView)DGrid).CurrentCell is DataGridViewCheckBoxCell)
				{
					list[e.RowIndex].IsChecked = !list[e.RowIndex].IsChecked;
				}
				else if (e.ColumnIndex == ((DataGridViewBand)DGBtnView).Index)
				{
					_loadDataToken?.Cancel();
					_loadDataToken = new CancellationTokenSource();
					CancellationToken token = _loadDataToken.Token;
					await LoadFrmcontractNewAsync(int.Parse(((DataGridView)DGrid).Rows[e.RowIndex].Cells[((DataGridViewBand)DGContractId).Index].Value.ToString()), token);
				}
				await LoadGridAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellContentClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmContractTransfer.cs", 129);
		}
	}

	private async void DGrid_VisibleChanged(object sender, EventArgs e)
	{
		await LoadGridAsync();
	}

	private async void DGrid_KeyUp(object sender, KeyEventArgs e)
	{
		_ = 1;
		try
		{
			e.Handled = true;
			if ((int)e.KeyCode == 32)
			{
				List<CheckoutDet> list = await _checkout.GetCheckoutDetListAsync();
				if (((DataGridView)DGrid).CurrentRow != null)
				{
					list[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].IsChecked = !list[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].IsChecked;
				}
				await LoadGridAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyUp", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmContractTransfer.cs", 153);
		}
	}

	private async Task LoadFrmcontractNewAsync(int gharardadId = 0, CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			if (((DataGridView)DGrid).CurrentRow == null)
			{
				return;
			}
			List<CheckoutDet> checkoutDet = await _checkout.GetCheckoutDetListAsync();
			FrmGharardad frmGharardad;
			if ((await _checkout.GetCheckoutDetListAsync())[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Contract != null)
			{
				frmGharardad = new FrmGharardad(((CacheBaseForm)this).ContextGuid, _font, checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Contract, readOnly: false);
			}
			else
			{
				if (gharardadId <= 0)
				{
					return;
				}
				int gharardadNo = Convert.ToInt32(((Control)txtContractNo).Text);
				if (gharardadNo <= 0)
				{
					return;
				}
				GharardadNew gharardadNew = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, gharardadId, token);
				if (gharardadNew == null)
				{
					WebErrorLog.Instance.StartLog($"امکان ایجاد قرارداد جدید وجود ندارد \r\n method=LoadFrmcontractNew(gharardadId={gharardadId})", "", "LoadFrmcontractNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmContractTransfer.cs", 176);
					throw new Exception("امکان ایجاد قرارداد جدید وجود ندارد");
				}
				gharardadNew.DateStartM = LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(((Control)TXTDateStartGharardad).Text);
				gharardadNew.PriceLeave = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].VacationPriceNew;
				gharardadNew.DateM = DateTime.Now;
				gharardadNew.PriceLowTime = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].VacationPriceNew;
				gharardadNew.LeaveSaved = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].SumVacationLeft;
				gharardadNew.PriceNobatKari1 = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].PriceShiftNew1;
				gharardadNew.PriceNobatKari2 = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].PriceShiftNew2;
				gharardadNew.PriceNobatKari3 = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].PriceShiftNew3;
				gharardadNew.DateFinishM = LanguageAndCalendar.Instance.CaltureCalender.CalendarToMiladi(((Control)TXTDateStartGharardad).Text).AddDays(365.0);
				gharardadNew.PriceAbsent = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].PriceAbsentNew;
				gharardadNew.PriceMission = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].PriceMissionNew;
				gharardadNew.PriceOverTime = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].PriceOverTimeNew;
				gharardadNew.PriceHoliday = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].PriceHoliday;
				gharardadNew.No = gharardadNo.ToString();
				gharardadNew.PriceNight = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].PriceNightNew;
				gharardadNew.ID = 0;
				gharardadNew.IsTemp = true;
				checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Contract = gharardadNew;
				decimal num = checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].SalaryNewMonthly;
				if (checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Contract.SalaryType == 3)
				{
					num = num / 30m / 7.33m;
				}
				else if (checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Contract.SalaryType == 2)
				{
					num /= 30m;
				}
				checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Contract.SetSalaryPrice(num);
				frmGharardad = new FrmGharardad(((CacheBaseForm)this).ContextGuid, _font, checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Contract, readOnly: false);
			}
			if ((int)(await ((CacheBaseForm)frmGharardad).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].CheckStatus = CheckoutDet.GharardadStatus.CheckIsOk;
				checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].IsChecked = true;
			}
			if (checkoutDet[((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Contract.No == ((Control)txtContractNo).Text)
			{
				NumericUpDown val = txtContractNo;
				((Control)val).Text = (await _checkout.GetFirstNewContractNoAsync(token)).ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadFrmcontractNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Checkout\\FrmContractTransfer.cs", 222);
		}
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6a: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		groupPanel4 = new GroupPanel();
		DGrid = new DataGridViewX();
		TamdidContractSource = new BindingSource(components);
		txtContractNo = new NumericUpDown();
		TXTDateStartGharardad = new DateBoxFa();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		labelX11 = new LabelX();
		DGCheck = new DataGridViewCheckBoxColumn();
		DGBtnView = new DataGridViewButtonColumn();
		DGContractId = new DataGridViewTextBoxColumn();
		DGGharardadNewNo = new DataGridViewTextBoxColumn();
		DGID = new DataGridViewTextBoxColumn();
		DGFullName = new DataGridViewTextBoxColumn();
		DGSalary = new DataGridViewTextBoxColumn();
		DGSalaryNew = new DataGridViewTextBoxColumn();
		((Control)PanelEx1).SuspendLayout();
		((Control)groupPanel4).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)TamdidContractSource).BeginInit();
		((ISupportInitialize)txtContractNo).BeginInit();
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
		((Control)groupPanel4).Controls.Add((Control)(object)DGrid);
		((Control)groupPanel4).Controls.Add((Control)(object)txtContractNo);
		((Control)groupPanel4).Controls.Add((Control)(object)TXTDateStartGharardad);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel4).Controls.Add((Control)(object)labelX1);
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
		((Control)groupPanel4).Text = "[انتخاب قرادادهای قابل انتقال و خاتمه یافته]";
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)8;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[8]
		{
			(DataGridViewColumn)DGCheck,
			(DataGridViewColumn)DGBtnView,
			(DataGridViewColumn)DGContractId,
			(DataGridViewColumn)DGGharardadNewNo,
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)DGFullName,
			(DataGridViewColumn)DGSalary,
			(DataGridViewColumn)DGSalaryNew
		});
		((DataGridView)DGrid).DataSource = TamdidContractSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("B Homa", 9.75f);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((Control)DGrid).Dock = (DockStyle)2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(0, 85);
		((Control)DGrid).Margin = new Padding(2);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(576, 361);
		((Control)DGrid).TabIndex = 24;
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((Control)DGrid).VisibleChanged += DGrid_VisibleChanged;
		((Control)DGrid).KeyUp += new KeyEventHandler(DGrid_KeyUp);
		((Control)txtContractNo).Anchor = (AnchorStyles)13;
		((Control)txtContractNo).Font = new Font("B Homa", 8f);
		((Control)txtContractNo).Location = new Point(9, 36);
		txtContractNo.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		txtContractNo.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)txtContractNo).Name = "txtContractNo";
		((Control)txtContractNo).Size = new Size(91, 27);
		((Control)txtContractNo).TabIndex = 23;
		((UpDownBase)txtContractNo).TextAlign = (HorizontalAlignment)2;
		txtContractNo.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		TXTDateStartGharardad.BackcolorTooltip = Color.Red;
		TXTDateStartGharardad.FarsyDateStyle = (ENFarsyDateStyle)1;
		((Control)TXTDateStartGharardad).Font = new Font("Tahoma", 11.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTDateStartGharardad).Location = new Point(300, 37);
		TXTDateStartGharardad.MiladiDate = new DateTime(2020, 3, 20, 1, 1, 0, 0);
		((Control)TXTDateStartGharardad).Name = "TXTDateStartGharardad";
		((Control)TXTDateStartGharardad).Size = new Size(121, 26);
		((Control)TXTDateStartGharardad).TabIndex = 22;
		((Control)TXTDateStartGharardad).Text = "1399/01/01";
		((TextBox)TXTDateStartGharardad).TextAlign = (HorizontalAlignment)2;
		TXTDateStartGharardad.TootTipDelay = 500;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).ForeColor = Color.Navy;
		((Control)labelX2).Location = new Point(106, 31);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(126, 37);
		((Control)labelX2).TabIndex = 21;
		((Control)labelX2).Text = "[شماره اولین قرارداد جدید :]";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).ForeColor = Color.Navy;
		((Control)labelX1).Location = new Point(427, 31);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(179, 37);
		((Control)labelX1).TabIndex = 21;
		((Control)labelX1).Text = "[تاریخ شروع قراردادهای جدید :]";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((BaseItemControl)labelX11).BackgroundStyle.Class = "";
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX11).ForeColor = Color.Black;
		((Control)labelX11).Location = new Point(9, 5);
		((Control)labelX11).Name = "labelX11";
		labelX11.PaddingBottom = 5;
		labelX11.PaddingLeft = 5;
		labelX11.PaddingRight = 5;
		labelX11.PaddingTop = 5;
		((Control)labelX11).Size = new Size(556, 31);
		((Control)labelX11).TabIndex = 20;
		((Control)labelX11).Text = "[در این مرحله قراردادهایی که قصد دارید تمدید شوند را انتخاب نمایید.]";
		labelX11.WordWrap = true;
		((DataGridViewColumn)DGCheck).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCheck).DataPropertyName = "IsChecked";
		DGCheck.FalseValue = "false";
		((DataGridViewColumn)DGCheck).HeaderText = "";
		DGCheck.IndeterminateValue = "true";
		((DataGridViewColumn)DGCheck).MinimumWidth = 30;
		((DataGridViewColumn)DGCheck).Name = "DGCheck";
		((DataGridViewBand)DGCheck).ReadOnly = true;
		((DataGridViewBand)DGCheck).Resizable = (DataGridViewTriState)2;
		((DataGridViewColumn)DGCheck).ToolTipText = "[در صورتیکه تیک داشته باشد با ثبت ویزارد، قرارداد جدید ایجاد خواهد شد]";
		DGCheck.TrueValue = "true";
		((DataGridViewColumn)DGCheck).Width = 30;
		((DataGridViewColumn)DGBtnView).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGBtnView).DataPropertyName = "BtnView";
		((DataGridViewColumn)DGBtnView).HeaderText = "[وضعیت]";
		((DataGridViewColumn)DGBtnView).Name = "DGBtnView";
		((DataGridViewBand)DGBtnView).ReadOnly = true;
		((DataGridViewBand)DGBtnView).Resizable = (DataGridViewTriState)1;
		((DataGridViewColumn)DGBtnView).SortMode = (DataGridViewColumnSortMode)1;
		((DataGridViewColumn)DGContractId).AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
		((DataGridViewColumn)DGContractId).DataPropertyName = "ContractId";
		((DataGridViewColumn)DGContractId).HeaderText = "[قرارداد قبلی]";
		((DataGridViewColumn)DGContractId).Name = "DGContractId";
		((DataGridViewBand)DGContractId).ReadOnly = true;
		((DataGridViewColumn)DGContractId).Width = 86;
		((DataGridViewColumn)DGGharardadNewNo).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGGharardadNewNo).DataPropertyName = "GharardadNewNo";
		((DataGridViewColumn)DGGharardadNewNo).HeaderText = "[قرارداد جدید]";
		((DataGridViewColumn)DGGharardadNewNo).Name = "DGGharardadNewNo";
		((DataGridViewBand)DGGharardadNewNo).ReadOnly = true;
		((DataGridViewColumn)DGGharardadNewNo).Width = 88;
		((DataGridViewColumn)DGID).AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شماره پرسنلی]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)DGID).Width = 97;
		((DataGridViewColumn)DGFullName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
		((DataGridViewColumn)DGFullName).DataPropertyName = "FullName";
		((DataGridViewColumn)DGFullName).HeaderText = "[نام و نام خانوادگی]";
		((DataGridViewColumn)DGFullName).Name = "DGFullName";
		((DataGridViewBand)DGFullName).ReadOnly = true;
		((DataGridViewColumn)DGFullName).Width = 117;
		((DataGridViewColumn)DGSalary).AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
		((DataGridViewColumn)DGSalary).DataPropertyName = "Salary";
		((DataGridViewColumn)DGSalary).HeaderText = "[حقوق قبلی]";
		((DataGridViewColumn)DGSalary).Name = "DGSalary";
		((DataGridViewBand)DGSalary).ReadOnly = true;
		((DataGridViewColumn)DGSalary).Width = 84;
		((DataGridViewColumn)DGSalaryNew).AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
		((DataGridViewColumn)DGSalaryNew).DataPropertyName = "SalaryNew";
		((DataGridViewColumn)DGSalaryNew).HeaderText = "[حقوق جدید]";
		((DataGridViewColumn)DGSalaryNew).Name = "DGSalaryNew";
		((DataGridViewBand)DGSalaryNew).ReadOnly = true;
		((DataGridViewColumn)DGSalaryNew).Width = 86;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(582, 500);
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "FrmContractTransfer";
		((Control)this).Text = "FrmContractTransfer";
		((Control)this).Controls.SetChildIndex((Control)(object)PanelEx1, 0);
		((Control)PanelEx1).ResumeLayout(false);
		((Control)groupPanel4).ResumeLayout(false);
		((Control)groupPanel4).PerformLayout();
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)TamdidContractSource).EndInit();
		((ISupportInitialize)txtContractNo).EndInit();
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

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(EventArgs e)
	{
		((FormExtention)this).OnLoad(e);
	}
}
