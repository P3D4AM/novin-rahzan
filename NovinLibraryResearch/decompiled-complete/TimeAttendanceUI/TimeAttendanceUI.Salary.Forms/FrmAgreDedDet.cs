using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Utility;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Salary.Forms;

public class FrmAgreDedDet : FormExtention
{
	private readonly AgreedDeduction _clsAgreedDeduction;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private CancellationTokenSource _loadDataToken;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private LabelX labelX2;

	private LabelX labelX4;

	private LabelX labelX1;

	private LabelX labelX5;

	private ButtonX BtnAccept;

	private LabelX LblFullName;

	private LabelX labelX7;

	private LabelX LblDateSh;

	private LabelX LblMonthlyInstallment;

	private LabelX LblAmountPayable;

	private LabelX LblHesabTitle;

	internal DataGridViewX DGrid;

	private BindingSource agreedDeductionDetBindingSource;

	private ButtonX BtnPrint;

	private DataGridViewCheckBoxColumn DgChecked;

	private DataGridViewTextBoxColumn DGFishId;

	private DataGridViewTextBoxColumn DGFishNo;

	private DataGridViewTextBoxColumn DGDateShDesc;

	private DataGridViewTextBoxColumn DGPrice;

	private DataGridViewTextBoxColumn DGRemnant;

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

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadDataToken?.Cancel();
				_loadDataToken?.Dispose();
				_loadDataToken = null;
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void SetTrDictionary()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[جزئیات کسورات توافقی]", out var _))
			{
				trDictionary.Add("[جزئیات کسورات توافقی]", Utilities.GetTranslate("[جزئیات کسورات توافقی]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionary", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreDedDet.cs", 51);
		}
	}

	public FrmAgreDedDet(string contextGuid, AgreedDeduction agreedDeduction)
		: base(contextGuid, true, true)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			SetTrDictionary();
			InitializeComponent();
			_clsAgreedDeduction = agreedDeduction;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, FillFormAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreDedDet.cs");
			((Control)this).Text = "جزئیات کسورات توافقی";
			if (trDictionary.TryGetValue("[جزئیات کسورات توافقی]", out var value))
			{
				((Control)this).Text = value;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task FillFormAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			LabelX lblFullName = LblFullName;
			((Control)lblFullName).Text = await (_clsAgreedDeduction?.GetFullNameAsync(token));
			lblFullName = LblHesabTitle;
			((Control)lblFullName).Text = await (_clsAgreedDeduction?.GetHesabTitleAsync(token));
			((Control)LblAmountPayable).Text = _clsAgreedDeduction?.AmountPayable.ToString("N0");
			LabelX lblDateSh = LblDateSh;
			AgreedDeduction clsAgreedDeduction = _clsAgreedDeduction;
			((Control)lblDateSh).Text = ((clsAgreedDeduction != null) ? Calendar.MiladiToCalture(clsAgreedDeduction.DateCreatedM) : null);
			((Control)LblMonthlyInstallment).Text = _clsAgreedDeduction?.MonthlyInstallment.ToString("N0");
			BindingSource obj = agreedDeductionDetBindingSource;
			AgreedDeduction clsAgreedDeduction2 = _clsAgreedDeduction;
			obj.DataSource = ((clsAgreedDeduction2 != null) ? Utilities.ToSortableBindingList<AgreedDeductionDet>((IEnumerable<AgreedDeductionDet>)clsAgreedDeduction2.AgreedDeductionDets, default(CancellationToken)) : null);
			((Control)BtnAccept).Text = "خروج";
			if (trDictionary.TryGetValue("[خروج]", out var value))
			{
				((Control)BtnAccept).Text = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreDedDet.cs", 89);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		((FormExtention)this).OnLoad(e);
		((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_Click);
	}

	private void BtnAccept_Click(object sender, EventArgs e)
	{
		try
		{
			((FormExtention)this).ResetControlChanged();
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreDedDet.cs", 108);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		List<object> kosuratList = new List<object>();
		List<object> list;
		foreach (AgreedDeductionDet item in _clsAgreedDeduction.AgreedDeductionDets)
		{
			if (item.Checked)
			{
				list = kosuratList;
				string dateCaltureDesc = await item.GetDateCaltureDescAsync();
				int fishId = item.FishId;
				list.Add(new
				{
					DateCaltureDesc = dateCaltureDesc,
					FishId = fishId,
					FishNo = await item.GetFishNoAsync(),
					MonthYear = Utilities.GetTranslate(item.MonthYear),
					Price = item.Price,
					Remnant = item.Remnant
				});
			}
		}
		if (kosuratList.Count <= 0)
		{
			Utility.MessageBoxShowWarning("[موردی برای چاپ وجود ندارد]", ((CacheBaseForm)this).ContextGuid);
			return;
		}
		List<object> list2 = new List<object>();
		list = list2;
		list.Add(await DebtHeader.GetAsync(((CacheBaseForm)this).ContextGuid, _clsAgreedDeduction));
		FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)16, kosuratList, null, _clsAgreedDeduction.Id, "", list2);
		await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
		((Component)(object)frm).Dispose();
	}

	private void DGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).CurrentCell != null && e.ColumnIndex == ((DataGridViewBand)DgChecked).Index)
			{
				((AgreedDeductionDet)agreedDeductionDetBindingSource.Current).Checked = !((AgreedDeductionDet)agreedDeductionDetBindingSource.Current).Checked;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellContentClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreDedDet.cs", 142);
		}
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 32)
			{
				agreedDeductionDetBindingSource.ResetBindings(false);
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Salary\\Forms\\FrmAgreDedDet.cs", 158);
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Expected O, but got Unknown
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Expected O, but got Unknown
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Expected O, but got Unknown
		//IL_0b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1008: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		BtnPrint = new ButtonX();
		DGrid = new DataGridViewX();
		agreedDeductionDetBindingSource = new BindingSource(components);
		LblDateSh = new LabelX();
		LblMonthlyInstallment = new LabelX();
		LblAmountPayable = new LabelX();
		LblHesabTitle = new LabelX();
		LblFullName = new LabelX();
		labelX7 = new LabelX();
		BtnAccept = new ButtonX();
		labelX5 = new LabelX();
		labelX4 = new LabelX();
		labelX1 = new LabelX();
		labelX2 = new LabelX();
		DgChecked = new DataGridViewCheckBoxColumn();
		DGFishId = new DataGridViewTextBoxColumn();
		DGFishNo = new DataGridViewTextBoxColumn();
		DGDateShDesc = new DataGridViewTextBoxColumn();
		DGPrice = new DataGridViewTextBoxColumn();
		DGRemnant = new DataGridViewTextBoxColumn();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)agreedDeductionDetBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)LblDateSh);
		((Control)panelEx1).Controls.Add((Control)(object)LblMonthlyInstallment);
		((Control)panelEx1).Controls.Add((Control)(object)LblAmountPayable);
		((Control)panelEx1).Controls.Add((Control)(object)LblHesabTitle);
		((Control)panelEx1).Controls.Add((Control)(object)LblFullName);
		((Control)panelEx1).Controls.Add((Control)(object)labelX7);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)labelX5);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)labelX1);
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(600, 469);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		((Control)BtnPrint).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnPrint).ForeColor = Color.Black;
		BtnPrint.Image = (Image)(object)Resources.printer_green;
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(129, 432);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).RightToLeft = (RightToLeft)1;
		((Control)BtnPrint).Size = new Size(111, 34);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 22;
		((Control)BtnPrint).Text = "[چاپ]";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[6]
		{
			(DataGridViewColumn)DgChecked,
			(DataGridViewColumn)DGFishId,
			(DataGridViewColumn)DGFishNo,
			(DataGridViewColumn)DGDateShDesc,
			(DataGridViewColumn)DGPrice,
			(DataGridViewColumn)DGRemnant
		});
		((DataGridView)DGrid).DataSource = agreedDeductionDetBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(12, 70);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(569, 356);
		((Control)DGrid).TabIndex = 21;
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		agreedDeductionDetBindingSource.DataSource = typeof(AgreedDeductionDet);
		((Control)LblDateSh).BackColor = Color.Transparent;
		((BaseItemControl)LblDateSh).BackgroundStyle.Class = "";
		((BaseItemControl)LblDateSh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblDateSh).ForeColor = Color.DarkRed;
		((Control)LblDateSh).Location = new Point(12, 42);
		((Control)LblDateSh).Name = "LblDateSh";
		((Control)LblDateSh).RightToLeft = (RightToLeft)1;
		((Control)LblDateSh).Size = new Size(87, 23);
		((Control)LblDateSh).TabIndex = 20;
		((Control)LblDateSh).Text = "----------";
		((Control)LblMonthlyInstallment).BackColor = Color.Transparent;
		((BaseItemControl)LblMonthlyInstallment).BackgroundStyle.Class = "";
		((BaseItemControl)LblMonthlyInstallment).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMonthlyInstallment).ForeColor = Color.DarkRed;
		((Control)LblMonthlyInstallment).Location = new Point(197, 41);
		((Control)LblMonthlyInstallment).Name = "LblMonthlyInstallment";
		((Control)LblMonthlyInstallment).RightToLeft = (RightToLeft)1;
		((Control)LblMonthlyInstallment).Size = new Size(87, 23);
		((Control)LblMonthlyInstallment).TabIndex = 19;
		((Control)LblMonthlyInstallment).Text = "----------";
		((Control)LblAmountPayable).BackColor = Color.Transparent;
		((BaseItemControl)LblAmountPayable).BackgroundStyle.Class = "";
		((BaseItemControl)LblAmountPayable).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblAmountPayable).ForeColor = Color.DarkRed;
		((Control)LblAmountPayable).Location = new Point(382, 41);
		((Control)LblAmountPayable).Name = "LblAmountPayable";
		((Control)LblAmountPayable).RightToLeft = (RightToLeft)1;
		((Control)LblAmountPayable).Size = new Size(87, 23);
		((Control)LblAmountPayable).TabIndex = 18;
		((Control)LblAmountPayable).Text = "----------";
		((Control)LblHesabTitle).BackColor = Color.Transparent;
		((BaseItemControl)LblHesabTitle).BackgroundStyle.Class = "";
		((BaseItemControl)LblHesabTitle).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblHesabTitle).ForeColor = Color.DarkRed;
		((Control)LblHesabTitle).Location = new Point(12, 12);
		((Control)LblHesabTitle).Name = "LblHesabTitle";
		((Control)LblHesabTitle).RightToLeft = (RightToLeft)1;
		((Control)LblHesabTitle).Size = new Size(180, 23);
		((Control)LblHesabTitle).TabIndex = 17;
		((Control)LblHesabTitle).Text = "----------";
		((Control)LblFullName).BackColor = Color.Transparent;
		((BaseItemControl)LblFullName).BackgroundStyle.Class = "";
		((BaseItemControl)LblFullName).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblFullName).ForeColor = Color.DarkRed;
		((Control)LblFullName).Location = new Point(271, 12);
		((Control)LblFullName).Name = "LblFullName";
		((Control)LblFullName).RightToLeft = (RightToLeft)1;
		((Control)LblFullName).Size = new Size(199, 23);
		((Control)LblFullName).TabIndex = 3;
		((Control)LblFullName).Text = "----------";
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.Class = "";
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(476, 12);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(86, 23);
		((Control)labelX7).TabIndex = 0;
		((Control)labelX7).Text = "[نام شخص] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnAccept).ForeColor = Color.Black;
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(12, 432);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 15;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((BaseItemControl)labelX5).BackgroundStyle.Class = "";
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(105, 42);
		((Control)labelX5).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(86, 23);
		((Control)labelX5).TabIndex = 11;
		((Control)labelX5).Text = "[تاریخ شروع] :";
		labelX5.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX4).BackgroundStyle.Class = "";
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(290, 41);
		((Control)labelX4).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(86, 23);
		((Control)labelX4).TabIndex = 9;
		((Control)labelX4).Text = "[قسط ماهیانه] :";
		labelX4.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX1).BackgroundStyle.Class = "";
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(475, 41);
		((Control)labelX1).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(106, 23);
		((Control)labelX1).TabIndex = 7;
		((Control)labelX1).Text = "[مبلغ قابل پرداخت] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX2).BackgroundStyle.Class = "";
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(198, 12);
		((Control)labelX2).Margin = new Padding(3, 2, 3, 2);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(67, 23);
		((Control)labelX2).TabIndex = 4;
		((Control)labelX2).Text = "[نام حساب] :";
		labelX2.TextAlignment = (StringAlignment)2;
		((DataGridViewColumn)DgChecked).DataPropertyName = "Checked";
		((DataGridViewColumn)DgChecked).HeaderText = "";
		((DataGridViewColumn)DgChecked).Name = "DgChecked";
		((DataGridViewBand)DgChecked).ReadOnly = true;
		((DataGridViewColumn)DGFishId).DataPropertyName = "FishId";
		((DataGridViewColumn)DGFishId).HeaderText = "FishId";
		((DataGridViewColumn)DGFishId).Name = "DGFishId";
		((DataGridViewBand)DGFishId).ReadOnly = true;
		((DataGridViewBand)DGFishId).Visible = false;
		((DataGridViewColumn)DGFishNo).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGFishNo).DataPropertyName = "FishNo";
		((DataGridViewColumn)DGFishNo).HeaderText = "[شماره فیش]";
		((DataGridViewColumn)DGFishNo).MinimumWidth = 50;
		((DataGridViewColumn)DGFishNo).Name = "DGFishNo";
		((DataGridViewBand)DGFishNo).ReadOnly = true;
		((DataGridViewColumn)DGDateShDesc).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGDateShDesc).DataPropertyName = "DateShDesc";
		((DataGridViewColumn)DGDateShDesc).HeaderText = "[تاریخ]";
		((DataGridViewColumn)DGDateShDesc).MinimumWidth = 70;
		((DataGridViewColumn)DGDateShDesc).Name = "DGDateShDesc";
		((DataGridViewBand)DGDateShDesc).ReadOnly = true;
		((DataGridViewColumn)DGPrice).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGPrice).DataPropertyName = "Price";
		val3.Format = "N0";
		val3.NullValue = null;
		((DataGridViewBand)DGPrice).DefaultCellStyle = val3;
		((DataGridViewColumn)DGPrice).HeaderText = "[مبلغ]";
		((DataGridViewColumn)DGPrice).MinimumWidth = 70;
		((DataGridViewColumn)DGPrice).Name = "DGPrice";
		((DataGridViewBand)DGPrice).ReadOnly = true;
		((DataGridViewColumn)DGRemnant).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGRemnant).DataPropertyName = "Remnant";
		val4.Format = "N0";
		val4.NullValue = null;
		((DataGridViewBand)DGRemnant).DefaultCellStyle = val4;
		((DataGridViewColumn)DGRemnant).HeaderText = "[مانده]";
		((DataGridViewColumn)DGRemnant).MinimumWidth = 50;
		((DataGridViewColumn)DGRemnant).Name = "DGRemnant";
		((DataGridViewBand)DGRemnant).ReadOnly = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(600, 491);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(449, 353);
		((Control)this).Name = "FrmAgreDedDet";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[افزودن مزایا و کسورات]";
		((Control)this).Controls.SetChildIndex((Control)(object)panelEx1, 0);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)agreedDeductionDetBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
