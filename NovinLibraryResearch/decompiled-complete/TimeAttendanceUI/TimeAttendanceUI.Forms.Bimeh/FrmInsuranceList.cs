using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser.Assistance.MultiLingual;
using PacketParser.Attendance.Access;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendance.ValidatorAttribute.Insurance;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.View.Salary;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Forms;

namespace TimeAttendanceUI.Forms.Bimeh;

public class FrmInsuranceList : BaseForm
{
	private readonly List<Insurance> _insuranceList = new List<Insurance>();

	private readonly List<short> _selecteUsers;

	private readonly DateTime _dateM1;

	private readonly DateTime _dateM2;

	private bool _isDisposed;

	private CancellationTokenSource _loadDataToken;

	private Font _font;

	private List<int> personIDList;

	private IContainer components;

	private PanelEx panelEx1;

	internal DataGridViewX DGrid;

	private ButtonX BtnPrint;

	private BindingSource insuranceBindingSource;

	private ButtonX BtnDiskette;

	private LabelX labelX2;

	private DataGridViewTextBoxColumn DGFishId;

	private DataGridViewTextBoxColumn workingCodeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn insuranceNoDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn salaryOnMonthDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DgAdvantagesMonthly;

	private DataGridViewTextBoxColumn totalWagesMonthlyBenefitsDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn UnemploymentInsurance;

	private DataGridViewTextBoxColumn EmployerInsurance;

	private DataGridViewTextBoxColumn PremiumInsured;

	private DataGridViewTextBoxColumn PayTaminGov;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
		}
	}

	public short FishId
	{
		get
		{
			try
			{
				if (((DataGridView)DGrid).Rows.Count == 0 || ((DataGridView)DGrid).CurrentRow == null)
				{
					return 0;
				}
				return short.Parse(((DataGridView)DGrid)[((DataGridViewBand)DGFishId).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FishId", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs", 108);
			}
			return 0;
		}
	}

	public Insurance SelectedInsurance
	{
		get
		{
			try
			{
				return _insuranceList.FirstOrDefault((Insurance q) => q.FishId == FishId) ?? new Insurance(((CacheBaseForm)this).ContextGuid);
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SelectedInsurance", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs", 125);
			}
			return new Insurance(((CacheBaseForm)this).ContextGuid);
		}
	}

	public FrmInsuranceList(string contextGuid, List<int> personIDs, DateTime date1, DateTime date2, Font font)
		: base(contextGuid, (BaseFormConfig)null)
	{
		_font = font;
		InitializeComponent();
		try
		{
			((Control)DGrid).Focus();
			((Control)DGrid).Font = font;
			_dateM1 = date1;
			_dateM2 = date2;
			AccessLevelClass accessLevelMerg = UserAttendance.SelectedUser.AccessLevelMerg;
			((Control)BtnDiskette).Enabled = accessLevelMerg.Insurance.PreparedDiskett;
			((Control)BtnPrint).Enabled = accessLevelMerg.Insurance.InsurancePrint;
			personIDList = personIDs;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs", 57);
		}
	}

	private async Task LoadDataAsync(List<int> personIDs, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<Fish> list = await Fish.GetAllForInsuranceAsync(((CacheBaseForm)this).ContextGuid, personIDs, _dateM1, _dateM2, _selecteUsers, token);
			if (list != null)
			{
				foreach (Fish fish in list)
				{
					if ((await Insurance.GetAsync(((CacheBaseForm)this).ContextGuid, fish, token)).PayTaminGov > 0m)
					{
						List<Insurance> insuranceList = _insuranceList;
						insuranceList.Add(await Insurance.GetAsync(((CacheBaseForm)this).ContextGuid, fish, token));
					}
				}
			}
			insuranceBindingSource.DataSource = _insuranceList;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs", 77);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			if (!UserAttendance.SelectedUser.AccessLevelMerg.Taradod.TaradodReportAmarPersonnel)
			{
				((Form)this).Close();
			}
			else
			{
				((BaseForm)this).OnLoad(e);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs", 93);
		}
	}

	private async void InsuranceList_KeyDown(object sender, KeyEventArgs e)
	{
		_ = 2;
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 27)
			{
				if ((int)keyCode != 13)
				{
					if ((int)keyCode == 27)
					{
						e.Handled = true;
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else
				{
					e.Handled = true;
					Fish pFish = await Fish.GetAsync(((CacheBaseForm)this).ContextGuid, FishId, loadClsTaradod: false);
					FrmFishHoghogh frmFishHoghogh = new FrmFishHoghogh(((CacheBaseForm)this).ContextGuid, pFish, _font, isReadOnly: true);
					await ((CacheBaseForm)frmFishHoghogh).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frmFishHoghogh).Dispose();
				}
			}
			else if ((int)keyCode != 80)
			{
				if ((int)keyCode == 123)
				{
					e.Handled = true;
					FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
					await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frm).Dispose();
				}
			}
			else if (e.Control)
			{
				((PopupItemControl)BtnPrint).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InsuranceList_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs", 165);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)BtnPrint).Enabled || !((Control)BtnPrint).Visible || !IsValidObjects())
			{
				return;
			}
			List<Insurance> insuranceList = _insuranceList;
			if (insuranceList != null && insuranceList.Count <= 0)
			{
				return;
			}
			((Control)DGrid).Focus();
			if (_insuranceList == null)
			{
				return;
			}
			List<object> list = new List<object>();
			foreach (Insurance insurance in _insuranceList)
			{
				string monthYear = insurance.GetMonthYear();
				list.Add(new
				{
					MonthYear = monthYear,
					InsuranceNo = insurance.InsuranceNo,
					FullName = insurance.FullName,
					FatherName = insurance.FatherName,
					CodeMelie = insurance.CodeMelie,
					ShSh = insurance.ShSh,
					PlaceIssue = insurance.PlaceIssue,
					BirthDate = insurance.BirthDate,
					Sexuality = insurance.Sexuality,
					StartWorkDate = insurance.StartWorkDate,
					EndWorkDate = insurance.EndWorkDate,
					WorkingDaysNo = insurance.WorkingDaysNo,
					SalaryOnDay = insurance.SalaryOnDay,
					SalaryOnMonth = insurance.SalaryOnMonth,
					AdvantagesMonthly = insurance.AdvantagesMonthly,
					TotalWagesMonthlyBenefits = insurance.TotalWagesMonthlyBenefits,
					UnemploymentInsurance = insurance.UnemploymentInsurance,
					EmployerInsurance = insurance.EmployerInsurance,
					PremiumInsured = insurance.PremiumInsured,
					PayTaminGov = insurance.PayTaminGov
				});
			}
			FrmPrint frm = new FrmPrint(((CacheBaseForm)this).ContextGuid, (EnumPrintShow)14, list, null, 0, "");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
			_ = "[تهیه گزارش لیست بیمه مربوط به]  " + _insuranceList[0].GetMonthYear() + " [تعداد کارکنان در لیست]  : " + _insuranceList.Count;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs", 213);
		}
	}

	private async void BtnDiskette_Click(object sender, EventArgs e)
	{
		int line = 0;
		try
		{
			if (!((Control)BtnDiskette).Enabled || !((Control)BtnDiskette).Visible || !IsValidObjects())
			{
				return;
			}
			List<Insurance> insuranceList = _insuranceList;
			if (insuranceList != null && insuranceList.Count <= 0)
			{
				return;
			}
			line = 1;
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			try
			{
				line = 2;
				DialogResult val = ((CommonDialog)fbd).ShowDialog((IWin32Window)(object)this);
				line = 3;
				if ((int)val != 1 || string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					return;
				}
				line = 4;
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				ReturnedSaveFuncInfoWithValue<string> listMakeDisketteDskkar = await Insurance.MakeDisketteDskkarAsync(((CacheBaseForm)this).ContextGuid, _insuranceList, token);
				if (((ReturnedSaveFuncInfo)listMakeDisketteDskkar).HasError)
				{
					FrmShowErrorMessage frmError = new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, (ReturnedSaveFuncInfo)(object)listMakeDisketteDskkar, " [خطای ایجاد لیست بیمه کارگاه]", (MessageBoxButtons)0, false, (LanguageTypes)0, "BtnDiskette_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs");
					await ((CacheBaseForm)frmError).ShowAsync((IWin32Window)(object)this);
					((Form)frmError).Close();
					return;
				}
				ReturnedSaveFuncInfoWithValue<List<string>> val2 = await Insurance.MakeDisketteDskworAsync(((CacheBaseForm)this).ContextGuid, _insuranceList);
				if (((ReturnedSaveFuncInfo)val2).HasError)
				{
					FrmShowErrorMessage frmError = new FrmShowErrorMessage(((CacheBaseForm)this).ContextGuid, (ReturnedSaveFuncInfo)(object)val2, "[خطای ایجاد لیست بیمه کارکنان]", (MessageBoxButtons)0, false, (LanguageTypes)0, "BtnDiskette_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs");
					await ((CacheBaseForm)frmError).ShowAsync((IWin32Window)(object)this);
					((Form)frmError).Close();
					return;
				}
				line = 7;
				File.WriteAllText(fbd.SelectedPath + "\\DSKKAR00-" + _insuranceList[0].Year + "-" + _insuranceList[0].Month.ToString("00") + ".txt", listMakeDisketteDskkar.value, Encoding.Default);
				File.WriteAllLines(fbd.SelectedPath + "\\DSKWOR00-" + _insuranceList[0].Year + "-" + _insuranceList[0].Month.ToString("00") + ".txt", val2.value, Encoding.Default);
				line = 777;
				await Utility.CreateUnicodeFileDosFileAsync(((CacheBaseForm)this).ContextGuid, fbd.SelectedPath + "\\DSKKAR00-" + _insuranceList[0].Year + "-" + _insuranceList[0].Month.ToString("00") + ".txt", fbd.SelectedPath + "\\Farsi-DSKKAR00-" + _insuranceList[0].Year + "-" + _insuranceList[0].Month.ToString("00") + ".txt");
				await Utility.CreateUnicodeFileDosFileAsync(((CacheBaseForm)this).ContextGuid, fbd.SelectedPath + "\\DSKWOR00-" + _insuranceList[0].Year + "-" + _insuranceList[0].Month.ToString("00") + ".txt", fbd.SelectedPath + "\\Farsi-DSKWOR00-" + _insuranceList[0].Year + "-" + _insuranceList[0].Month.ToString("00") + ".txt");
				line = 8;
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[تهیه دیسکت بیمه تامین اجتماعی با موفقیت انجام شد]", "", 3);
				_ = "[تهیه فایل خروجی لیست بیمه مربوط به]  " + _insuranceList[0].GetMonthYear() + " [تعداد کارکنان در لیست]  : " + _insuranceList.Count + "\n" + listMakeDisketteDskkar.value;
			}
			finally
			{
				((IDisposable)fbd)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, $" the line no is {line} and _insuranceList?.Count: {_insuranceList?.Count}", "BtnDiskette_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Bimeh\\FrmInsuranceList.cs", 269);
		}
	}

	private bool IsValidObjects()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		DialogResult val = InsuranceCheckValidation.CheckValidation(((CacheBaseForm)this).ContextGuid, _insuranceList, isShowMessage: true);
		if ((int)val != 5 && (int)val != 1)
		{
			return (int)val == 6;
		}
		return true;
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
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void FrmInsuranceList_Load(object sender, EventArgs e)
	{
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await LoadDataAsync(personIDList, token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
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
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		DataGridViewCellStyle val6 = new DataGridViewCellStyle();
		DataGridViewCellStyle val7 = new DataGridViewCellStyle();
		DataGridViewCellStyle val8 = new DataGridViewCellStyle();
		DataGridViewCellStyle val9 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		labelX2 = new LabelX();
		DGrid = new DataGridViewX();
		DGFishId = new DataGridViewTextBoxColumn();
		workingCodeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		fullNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		insuranceNoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		salaryOnMonthDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DgAdvantagesMonthly = new DataGridViewTextBoxColumn();
		totalWagesMonthlyBenefitsDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		UnemploymentInsurance = new DataGridViewTextBoxColumn();
		EmployerInsurance = new DataGridViewTextBoxColumn();
		PremiumInsured = new DataGridViewTextBoxColumn();
		PayTaminGov = new DataGridViewTextBoxColumn();
		insuranceBindingSource = new BindingSource(components);
		BtnDiskette = new ButtonX();
		BtnPrint = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)insuranceBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX2);
		((Control)panelEx1).Controls.Add((Control)(object)DGrid);
		((Control)panelEx1).Controls.Add((Control)(object)BtnDiskette);
		((Control)panelEx1).Controls.Add((Control)(object)BtnPrint);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(871, 474);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)labelX2).ForeColor = Color.DarkRed;
		((Control)labelX2).Location = new Point(301, 429);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(559, 33);
		((Control)labelX2).TabIndex = 23;
		((Control)labelX2).Text = "[کارکنانی که حق بیمه آنها صفر شده است احتمالا\u064c در قرارداد آنها بیمه بعنوان کسورات در نظر گرفته نشده است.]";
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[11]
		{
			(DataGridViewColumn)DGFishId,
			(DataGridViewColumn)workingCodeDataGridViewTextBoxColumn,
			(DataGridViewColumn)fullNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)insuranceNoDataGridViewTextBoxColumn,
			(DataGridViewColumn)salaryOnMonthDataGridViewTextBoxColumn,
			(DataGridViewColumn)DgAdvantagesMonthly,
			(DataGridViewColumn)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn,
			(DataGridViewColumn)UnemploymentInsurance,
			(DataGridViewColumn)EmployerInsurance,
			(DataGridViewColumn)PremiumInsured,
			(DataGridViewColumn)PayTaminGov
		});
		((DataGridView)DGrid).DataSource = insuranceBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(10, 12);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(850, 411);
		((Control)DGrid).TabIndex = 17;
		((DataGridViewColumn)DGFishId).DataPropertyName = "FishId";
		((DataGridViewColumn)DGFishId).HeaderText = "FishId";
		((DataGridViewColumn)DGFishId).Name = "DGFishId";
		((DataGridViewBand)DGFishId).ReadOnly = true;
		((DataGridViewBand)DGFishId).Visible = false;
		((DataGridViewColumn)workingCodeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)workingCodeDataGridViewTextBoxColumn).DataPropertyName = "WorkingCode";
		((DataGridViewColumn)workingCodeDataGridViewTextBoxColumn).HeaderText = "[کد]";
		((DataGridViewColumn)workingCodeDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)workingCodeDataGridViewTextBoxColumn).Name = "workingCodeDataGridViewTextBoxColumn";
		((DataGridViewBand)workingCodeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)workingCodeDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)fullNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)fullNameDataGridViewTextBoxColumn).DataPropertyName = "FullName";
		((DataGridViewColumn)fullNameDataGridViewTextBoxColumn).HeaderText = "[نام و نام خانوادگی]";
		((DataGridViewColumn)fullNameDataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)fullNameDataGridViewTextBoxColumn).Name = "fullNameDataGridViewTextBoxColumn";
		((DataGridViewBand)fullNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)insuranceNoDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)insuranceNoDataGridViewTextBoxColumn).DataPropertyName = "InsuranceNo";
		((DataGridViewColumn)insuranceNoDataGridViewTextBoxColumn).HeaderText = "[شماره بیمه]";
		((DataGridViewColumn)insuranceNoDataGridViewTextBoxColumn).MinimumWidth = 90;
		((DataGridViewColumn)insuranceNoDataGridViewTextBoxColumn).Name = "insuranceNoDataGridViewTextBoxColumn";
		((DataGridViewBand)insuranceNoDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)insuranceNoDataGridViewTextBoxColumn).Width = 90;
		((DataGridViewColumn)salaryOnMonthDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)salaryOnMonthDataGridViewTextBoxColumn).DataPropertyName = "SalaryOnMonth";
		val3.Format = "N0";
		val3.NullValue = null;
		((DataGridViewBand)salaryOnMonthDataGridViewTextBoxColumn).DefaultCellStyle = val3;
		((DataGridViewColumn)salaryOnMonthDataGridViewTextBoxColumn).HeaderText = "[دستمزد ماهانه]";
		((DataGridViewColumn)salaryOnMonthDataGridViewTextBoxColumn).MinimumWidth = 150;
		((DataGridViewColumn)salaryOnMonthDataGridViewTextBoxColumn).Name = "salaryOnMonthDataGridViewTextBoxColumn";
		((DataGridViewBand)salaryOnMonthDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)salaryOnMonthDataGridViewTextBoxColumn).Width = 150;
		((DataGridViewColumn)DgAdvantagesMonthly).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DgAdvantagesMonthly).DataPropertyName = "AdvantagesMonthly";
		val4.Format = "N0";
		((DataGridViewBand)DgAdvantagesMonthly).DefaultCellStyle = val4;
		((DataGridViewColumn)DgAdvantagesMonthly).HeaderText = "[مزایای ماهانه مشمول]";
		((DataGridViewColumn)DgAdvantagesMonthly).MinimumWidth = 150;
		((DataGridViewColumn)DgAdvantagesMonthly).Name = "DgAdvantagesMonthly";
		((DataGridViewBand)DgAdvantagesMonthly).ReadOnly = true;
		((DataGridViewColumn)DgAdvantagesMonthly).Width = 150;
		((DataGridViewColumn)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn).DataPropertyName = "TotalWagesMonthlyBenefits";
		val5.Format = "N0";
		val5.NullValue = null;
		((DataGridViewBand)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn).DefaultCellStyle = val5;
		((DataGridViewColumn)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn).HeaderText = "[حقوق مشمول بیمه]";
		((DataGridViewColumn)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn).MinimumWidth = 150;
		((DataGridViewColumn)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn).Name = "totalWagesMonthlyBenefitsDataGridViewTextBoxColumn";
		((DataGridViewBand)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)totalWagesMonthlyBenefitsDataGridViewTextBoxColumn).Width = 150;
		((DataGridViewColumn)UnemploymentInsurance).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)UnemploymentInsurance).DataPropertyName = "UnemploymentInsurance";
		val6.Format = "N2";
		val6.NullValue = null;
		((DataGridViewBand)UnemploymentInsurance).DefaultCellStyle = val6;
		((DataGridViewColumn)UnemploymentInsurance).HeaderText = "[حق ییمه بیکاری]";
		((DataGridViewColumn)UnemploymentInsurance).MinimumWidth = 120;
		((DataGridViewColumn)UnemploymentInsurance).Name = "UnemploymentInsurance";
		((DataGridViewBand)UnemploymentInsurance).ReadOnly = true;
		((DataGridViewColumn)UnemploymentInsurance).Width = 120;
		((DataGridViewColumn)EmployerInsurance).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)EmployerInsurance).DataPropertyName = "EmployerInsurance";
		val7.Format = "N2";
		val7.NullValue = null;
		((DataGridViewBand)EmployerInsurance).DefaultCellStyle = val7;
		((DataGridViewColumn)EmployerInsurance).HeaderText = "[حق بیمه سهم کارفرما]";
		((DataGridViewColumn)EmployerInsurance).MinimumWidth = 120;
		((DataGridViewColumn)EmployerInsurance).Name = "EmployerInsurance";
		((DataGridViewBand)EmployerInsurance).ReadOnly = true;
		((DataGridViewColumn)EmployerInsurance).Width = 124;
		((DataGridViewColumn)PremiumInsured).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)PremiumInsured).DataPropertyName = "PremiumInsured";
		val8.Format = "N2";
		val8.NullValue = null;
		((DataGridViewBand)PremiumInsured).DefaultCellStyle = val8;
		((DataGridViewColumn)PremiumInsured).HeaderText = "[حق بیمه سهم کارگر]";
		((DataGridViewColumn)PremiumInsured).MinimumWidth = 120;
		((DataGridViewColumn)PremiumInsured).Name = "PremiumInsured";
		((DataGridViewBand)PremiumInsured).ReadOnly = true;
		((DataGridViewColumn)PremiumInsured).Width = 120;
		((DataGridViewColumn)PayTaminGov).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)PayTaminGov).DataPropertyName = "PayTaminGov";
		val9.Format = "N2";
		val9.NullValue = null;
		((DataGridViewBand)PayTaminGov).DefaultCellStyle = val9;
		((DataGridViewColumn)PayTaminGov).HeaderText = "[پرداختی به تامین اجتماعی]";
		((DataGridViewColumn)PayTaminGov).MinimumWidth = 200;
		((DataGridViewColumn)PayTaminGov).Name = "PayTaminGov";
		((DataGridViewBand)PayTaminGov).ReadOnly = true;
		((DataGridViewColumn)PayTaminGov).Width = 200;
		insuranceBindingSource.DataSource = typeof(Insurance);
		((Control)BtnDiskette).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDiskette).Anchor = (AnchorStyles)6;
		BtnDiskette.ColorTable = (eButtonColor)6;
		BtnDiskette.Image = (Image)(object)Resources.Diskette1;
		BtnDiskette.ImagePosition = (eImagePosition)1;
		BtnDiskette.ImageTextSpacing = 2;
		((Control)BtnDiskette).Location = new Point(154, 429);
		((Control)BtnDiskette).Name = "BtnDiskette";
		((Control)BtnDiskette).RightToLeft = (RightToLeft)1;
		((Control)BtnDiskette).Size = new Size(141, 33);
		((PopupItemControl)BtnDiskette).Style = (eDotNetBarStyle)9;
		((Control)BtnDiskette).TabIndex = 18;
		((Control)BtnDiskette).Text = "  [فایل خروجی بیمه]";
		BtnDiskette.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnDiskette).Click += BtnDiskette_Click;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		((Control)BtnPrint).Anchor = (AnchorStyles)6;
		BtnPrint.ColorTable = (eButtonColor)6;
		BtnPrint.Image = (Image)(object)Resources.printer_green;
		BtnPrint.ImagePosition = (eImagePosition)1;
		BtnPrint.ImageTextSpacing = 2;
		((Control)BtnPrint).Location = new Point(10, 429);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).RightToLeft = (RightToLeft)1;
		((Control)BtnPrint).Size = new Size(138, 33);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 19;
		((Control)BtnPrint).Text = "  [چاپ] (Ctrl+P)";
		BtnPrint.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnPrint).Click += BtnPrint_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(871, 474);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).MinimumSize = new Size(880, 397);
		((Control)this).Name = "FrmInsuranceList";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[گزارش لیست بیمه]";
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += FrmInsuranceList_Load;
		((Control)this).KeyDown += new KeyEventHandler(InsuranceList_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)insuranceBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
