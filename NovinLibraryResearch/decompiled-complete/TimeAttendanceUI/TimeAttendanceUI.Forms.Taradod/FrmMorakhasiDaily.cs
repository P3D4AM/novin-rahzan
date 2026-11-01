using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BehComponents;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using Serilog;
using TimeAttendance.ValidatorAttribute.Vacation;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Taradod;

public class FrmMorakhasiDaily : BaseForm
{
	private Morakhasidaily _clsMorakhasidaily;

	private List<TypeMorakhasi> TypeMorakhasis;

	private readonly List<int> _personIDs = new List<int>();

	private int _personId;

	private readonly int _morakhasiId;

	private string Frm_Text = "[ویرایش مرخصی روزانه]";

	private List<TimeAttendanceDAL.Dal.FLRPerson.Person> PersonList;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _SaveToken;

	private DateTime _dateMorakhasi;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private TaskCompletionSource<bool> TaskConmpleteBind = new TaskCompletionSource<bool>();

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private PanelEx paneldays;

	private LabelX labelX2;

	private LabelX labelX5;

	internal DateBoxFa TbxDateFrom;

	private TextBoxX descdays;

	private ComboBoxEx CmbPersonel;

	private LabelX labelX4;

	private LabelX labelX8;

	private ComboBoxEx CmbType;

	private CheckBox ChkMorkhasiTatilat;

	private BindingSource personBindingSource;

	internal DateBoxFa TbxDateTo;

	private LabelX labelX1;

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
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public FrmMorakhasiDaily(string contextGuid, DateTime pDate, int personID)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		try
		{
			_personId = personID;
			_dateMorakhasi = pDate;
			((Control)TbxDateTo).Text = Calendar.MiladiToCalture(pDate);
			Frm_Text = "[افزودن مرخصی روزانه]";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 77);
		}
	}

	public FrmMorakhasiDaily(string contextGuid, Morakhasidaily pMorakhasidaily, bool readOnly)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
		try
		{
			_clsMorakhasidaily = pMorakhasidaily;
			if (_clsMorakhasidaily != null)
			{
				((Control)TbxDateTo).Text = _clsMorakhasidaily.DateCalture;
			}
			if (readOnly)
			{
				Frm_Text = "[مشاهده لاگ عملکرد کاربران ( قفط خواندنی ) اطلاعات مرخصی ها ]";
				((Control)BtnAccept).Enabled = false;
			}
			else
			{
				Frm_Text = "افزودن مرخصی روزانه";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 101);
		}
	}

	public FrmMorakhasiDaily(string contextGuid, AttPersonLeaveBusiness obj)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected I4, but got Unknown
		InitializeComponent();
		if (obj != null)
		{
			_clsMorakhasidaily = new Morakhasidaily(contextGuid);
			_clsMorakhasidaily.ContextGuid = contextGuid;
			_clsMorakhasidaily.PersonId = 0;
			_clsMorakhasidaily.PersonGuid = obj.PersonGuid;
			_clsMorakhasidaily.DateFrom = obj.DateFrom;
			((Control)TbxDateTo).Text = Calendar.MiladiToCalture(obj.DateTo);
			_clsMorakhasidaily.Desc = obj.Reason;
			EnPersonLeaveKind kind = obj.Kind;
			switch (kind - 1)
			{
			case 1:
				_clsMorakhasidaily.TypeId = 1;
				break;
			case 0:
				_clsMorakhasidaily.TypeId = 2;
				break;
			case 2:
				_clsMorakhasidaily.TypeId = 3;
				break;
			case 3:
				_clsMorakhasidaily.TypeId = 4;
				break;
			case 4:
				_clsMorakhasidaily.TypeId = 5;
				break;
			default:
				_clsMorakhasidaily.TypeId = 1;
				break;
			}
			Frm_Text = "ثبت مرخصی روزانه_درخواست سیستمی";
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			await LoadTypeMorakhasisAsync(token);
			await LoadPersonListAsync(token);
			if (_clsMorakhasidaily == null)
			{
				if (_morakhasiId > 0)
				{
					_clsMorakhasidaily = await Morakhasidaily.GetAsync(((CacheBaseForm)this).ContextGuid, _morakhasiId, token);
				}
				else
				{
					_clsMorakhasidaily = new Morakhasidaily(((CacheBaseForm)this).ContextGuid);
					_clsMorakhasidaily.PersonId = _personId;
					_clsMorakhasidaily.TypeId = 2;
					_clsMorakhasidaily.DateFrom = _dateMorakhasi;
				}
			}
			if (_clsMorakhasidaily.PersonId <= 0 && _clsMorakhasidaily.PersonGuid != Guid.Empty)
			{
				_clsMorakhasidaily.PersonId = ((PersonBase)PersonList.FirstOrDefault((TimeAttendanceDAL.Dal.FLRPerson.Person p) => ((PersonBase)p).Guid == _clsMorakhasidaily.PersonGuid)).Id;
			}
			await FillForm();
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private void SetFrmForEdit()
	{
		try
		{
			((Control)CmbPersonel).Enabled = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetFrmForEdit", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 175);
		}
	}

	private async Task<TaradodFullDet> GetTaradodFullDetAsync(List<DeviceInfo> devices, DateTime pDateM, TimeAttendanceDAL.Dal.FLRPerson.Person pclsPerson, CancellationToken token = default(CancellationToken))
	{
		string contextGuid = ((CacheBaseForm)this).ContextGuid;
		return await TaradodFullDet.GetAsync_(contextGuid, devices, pDateM, pclsPerson, await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, ((PersonBase)pclsPerson).Id, pDateM, token), checkValidationVorodAndKHoroj: false);
	}

	private async Task LoadTypeMorakhasisAsync(CancellationToken token)
	{
		try
		{
			if (TypeMorakhasis == null)
			{
				TypeMorakhasis = await TypeMorakhasi.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			}
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						((ComboBox)CmbType).DataSource = TypeMorakhasis;
						((ListControl)CmbType).ValueMember = "Id";
						((ListControl)CmbType).DisplayMember = "Name";
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadTypeMorakhasisAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 208);
		}
	}

	private async Task LoadPersonListAsync(CancellationToken token)
	{
		try
		{
			if (PersonList == null)
			{
				PersonList = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetAllAsync(((CacheBaseForm)this).ContextGuid, _personIDs, personStatus: true, token);
			}
			if (token.IsCancellationRequested)
			{
				return;
			}
			do
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						personBindingSource.DataSource = Utilities.ToSortableBindingList<TimeAttendanceDAL.Dal.FLRPerson.Person>((IEnumerable<TimeAttendanceDAL.Dal.FLRPerson.Person>)PersonList, token);
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
				finally
				{
					TaskConmpleteBind.TrySetResult(result: true);
					TaskConmpleteBind = new TaskCompletionSource<bool>();
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadPersonListAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 241);
		}
	}

	private async Task FillForm()
	{
		try
		{
			if (_clsMorakhasidaily == null)
			{
				return;
			}
			if (_clsMorakhasidaily.PersonId > 0 && (PersonList?.Any() ?? false))
			{
				TimeAttendanceDAL.Dal.FLRPerson.Person person = PersonList.FirstOrDefault((TimeAttendanceDAL.Dal.FLRPerson.Person p) => ((PersonBase)p).Id == _clsMorakhasidaily.PersonId);
				if (person != null)
				{
					await TaskConmpleteBind.Task;
					((ComboBox)CmbPersonel).SelectedItem = person;
				}
			}
			((ListControl)CmbType).SelectedValue = _clsMorakhasidaily.TypeId;
			((Control)descdays).Text = _clsMorakhasidaily.Desc;
			((Control)TbxDateFrom).Text = _clsMorakhasidaily.DateCalture;
			if (string.IsNullOrEmpty(((Control)TbxDateTo).Text))
			{
				((Control)TbxDateTo).Text = ((Control)TbxDateFrom).Text;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillForm", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 271);
		}
	}

	private void FRMMorakhasiDaily_Load(object sender, EventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "FRMMorakhasiDaily_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMMorakhasiDaily_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 285);
		}
	}

	private async void FRMMorakhasiDaily_KeyDown(object sender, KeyEventArgs e)
	{
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
					((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
				}
			}
			else if ((int)keyCode != 116)
			{
				if ((int)keyCode == 123)
				{
					e.Handled = true;
					FrmCalc frm = new FrmCalc(((CacheBaseForm)this).ContextGuid);
					await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
					((Component)(object)frm).Dispose();
				}
			}
			else
			{
				e.Handled = true;
				((PopupItemControl)BtnAccept).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMMorakhasiDaily_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 317);
		}
	}

	private void BtnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 329);
		}
	}

	private async Task CheckLeaveBalanceAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if (((ListControl)CmbPersonel).SelectedValue == null)
			{
				return;
			}
			_clsMorakhasidaily.PersonId = int.Parse(((ListControl)CmbPersonel).SelectedValue.ToString());
			if (((ListControl)CmbType).SelectedValue == null)
			{
				return;
			}
			_clsMorakhasidaily.TypeId = Utilities.ParseToShort(((ListControl)CmbType).SelectedValue.ToString(), (short)0);
			if (((TypeMorakhasi)((ComboBox)CmbType).SelectedItem).Id == 2)
			{
				GharardadNew gharardad = await GharardadNew.GetAsync(((CacheBaseForm)this).ContextGuid, _clsMorakhasidaily.PersonId, TbxDateFrom.MiladiDate, token);
				if ((await Morakhasidaily.CalcLeaveBalanceAsync(((CacheBaseForm)this).ContextGuid, gharardad, token)).Item3 - (double)(Calendar.ShamsiToMiladi(((Control)TbxDateTo).Text).Date - _clsMorakhasidaily.DateFrom.Date).Days <= 0.0)
				{
					string msg = "[مانده مرخصی] " + ((PersonBase)(TimeAttendanceDAL.Dal.FLRPerson.Person)((ComboBox)CmbPersonel).SelectedItem).FullName + " [منفی شده است] ";
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)5, msg, "", 3);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckLeaveBalanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 353);
		}
	}

	private async Task<DialogResult> SaveAsync(CancellationToken token)
	{
		DialogResult xDialogResult = (DialogResult)2;
		int li = 0;
		try
		{
			int countTatil = 0;
			int countSave = 0;
			await CheckLeaveBalanceAsync(token);
			Morakhasidaily temp = new Morakhasidaily(((CacheBaseForm)this).ContextGuid)
			{
				DateFrom = _clsMorakhasidaily.DateFrom,
				Desc = _clsMorakhasidaily.Desc,
				PersonId = _clsMorakhasidaily.PersonId,
				TypeId = _clsMorakhasidaily.TypeId
			};
			List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(((CacheBaseForm)this).ContextGuid, token);
			int countDay = (Utilities.CalendarToMiladi(((Control)TbxDateTo).Text).Date - _clsMorakhasidaily.DateFrom.Date).Days + 1;
			for (int i = 0; i < countDay; i++)
			{
				StatuseTaradodEnumType val = await (await GetTaradodFullDetAsync(devices, temp.DateFrom, (TimeAttendanceDAL.Dal.FLRPerson.Person)((ComboBox)CmbPersonel).SelectedItem, token)).GetStatuseTaradodAsync(token);
				if (IsTatil(val) && !ChkMorkhasiTatilat.Checked)
				{
					temp.DateFrom = temp.DateFrom.Date.AddDays(1.0);
					countTatil++;
					continue;
				}
				xDialogResult = await VacationCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, temp, val, isShowMessage: true, token);
				switch (xDialogResult - 2)
				{
				case 0:
					return xDialogResult;
				case 2:
					return xDialogResult;
				case 3:
				case 4:
					await temp.SaveAsync(token);
					countSave++;
					break;
				}
				temp.DateFrom = temp.DateFrom.Date.AddDays(1.0);
			}
			string text = "";
			if (countTatil > 0)
			{
				text = $"[تعداد] {countTatil} [روز از روزهای مرخصی بعلت تعطیلی ثبت نشدند]";
			}
			if (countSave > 0)
			{
				text = ((!(text == "")) ? (text + $"\r\n\r\n [لذا فقط] {countSave} [روز مرخصی از] {countSave + countTatil} [روز با موفقیت ثبت شد]") : "[با موفقیت ثبت شد]");
			}
			if (text != "")
			{
				Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)3, text, "", 3);
			}
			xDialogResult = (DialogResult)1;
			return xDialogResult;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, $"{((Control)this).Text} : line no is {li}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 418);
		}
		return xDialogResult;
	}

	private bool IsTatil(StatuseTaradodEnumType statusTaradod)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		if ((int)statusTaradod != 7)
		{
			return (int)statusTaradod == 6;
		}
		return true;
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnAccept).Enabled && ((Control)BtnAccept).Visible)
			{
				ValidateAndFillClsMorakhasi();
				_SaveToken?.Cancel();
				_SaveToken = new CancellationTokenSource();
				CancellationToken token = _SaveToken.Token;
				DialogResult val = await SaveAsync(token);
				if ((int)val == 1 || (int)val == 2)
				{
					((Form)this).DialogResult = val;
					((Form)this).Close();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Taradod\\FRMMorakhasiDaily.cs", 440);
		}
	}

	private bool ValidateAndFillClsMorakhasi()
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (CmbPersonel == null || ((ComboBox)CmbPersonel).SelectedItem == null)
			{
				stringBuilder.AppendLine("شخص مورد نظر را انتخاب نمائید");
			}
			if (CmbType == null || ((ComboBox)CmbType).SelectedItem == null)
			{
				stringBuilder.AppendLine("نوع مرخصی را مشخص نمائید");
			}
			if (Calendar.ShamsiToMiladi(((Control)TbxDateTo).Text).Date < Calendar.ShamsiToMiladi(((Control)TbxDateFrom).Text).Date)
			{
				stringBuilder.AppendLine("تاریخ های مرخصی نامعتبر است");
			}
			if (stringBuilder.Length > 0)
			{
				MessageBoxFarsi.Show(stringBuilder.ToString());
				return false;
			}
			_clsMorakhasidaily.PersonId = ((PersonBase)(TimeAttendanceDAL.Dal.FLRPerson.Person)((ComboBox)CmbPersonel).SelectedItem).Id;
			_clsMorakhasidaily.TypeId = ((TypeMorakhasi)((ComboBox)CmbType).SelectedItem).Id;
			_clsMorakhasidaily.DateFrom = Calendar.ShamsiToMiladi(((Control)TbxDateFrom).Text).Date;
			_clsMorakhasidaily.Desc = ((Control)descdays).Text;
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
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
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Expected O, but got Unknown
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b52: Expected O, but got Unknown
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8f: Expected O, but got Unknown
		//IL_0bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1b: Expected O, but got Unknown
		//IL_0e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f20: Expected O, but got Unknown
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_102b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1035: Expected O, but got Unknown
		//IL_103a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1089: Unknown result type (might be due to invalid IL or missing references)
		//IL_1093: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		labelX8 = new LabelX();
		CmbType = new ComboBoxEx();
		CmbPersonel = new ComboBoxEx();
		personBindingSource = new BindingSource(components);
		labelX4 = new LabelX();
		paneldays = new PanelEx();
		TbxDateTo = new DateBoxFa();
		labelX1 = new LabelX();
		ChkMorkhasiTatilat = new CheckBox();
		descdays = new TextBoxX();
		TbxDateFrom = new DateBoxFa();
		labelX2 = new LabelX();
		labelX5 = new LabelX();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)personBindingSource).BeginInit();
		((Control)paneldays).SuspendLayout();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)labelX8);
		((Control)panelEx1).Controls.Add((Control)(object)CmbType);
		((Control)panelEx1).Controls.Add((Control)(object)CmbPersonel);
		((Control)panelEx1).Controls.Add((Control)(object)labelX4);
		((Control)panelEx1).Controls.Add((Control)(object)paneldays);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(4, 3, 4, 3);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(446, 280);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Location = new Point(326, 41);
		((Control)labelX8).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).RightToLeft = (RightToLeft)1;
		((Control)labelX8).Size = new Size(108, 23);
		((Control)labelX8).TabIndex = 9;
		((Control)labelX8).Text = "[انواع مرخصی] :";
		labelX8.TextAlignment = (StringAlignment)2;
		((ComboBox)CmbType).DrawMode = (DrawMode)1;
		((ComboBox)CmbType).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbType).FormattingEnabled = true;
		((ComboBox)CmbType).ItemHeight = 15;
		((Control)CmbType).Location = new Point(105, 43);
		((Control)CmbType).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbType).Name = "CmbType";
		((Control)CmbType).Size = new Size(213, 21);
		CmbType.Style = (eDotNetBarStyle)9;
		((Control)CmbType).TabIndex = 8;
		((ComboBox)CmbPersonel).DataSource = personBindingSource;
		((ListControl)CmbPersonel).DisplayMember = "FullName";
		((ComboBox)CmbPersonel).DrawMode = (DrawMode)1;
		((ComboBox)CmbPersonel).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbPersonel).FormattingEnabled = true;
		((ComboBox)CmbPersonel).ItemHeight = 15;
		((Control)CmbPersonel).Location = new Point(105, 14);
		((Control)CmbPersonel).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbPersonel).Name = "CmbPersonel";
		((Control)CmbPersonel).Size = new Size(213, 21);
		CmbPersonel.Style = (eDotNetBarStyle)9;
		((Control)CmbPersonel).TabIndex = 1;
		((ListControl)CmbPersonel).ValueMember = "Id";
		personBindingSource.DataSource = typeof(TimeAttendanceDAL.Dal.FLRPerson.Person);
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(326, 14);
		((Control)labelX4).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(103, 23);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "[نام شخص] :";
		labelX4.TextAlignment = (StringAlignment)2;
		paneldays.CanvasColor = SystemColors.Control;
		paneldays.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)paneldays).Controls.Add((Control)(object)TbxDateTo);
		((Control)paneldays).Controls.Add((Control)(object)labelX1);
		((Control)paneldays).Controls.Add((Control)(object)ChkMorkhasiTatilat);
		((Control)paneldays).Controls.Add((Control)(object)descdays);
		((Control)paneldays).Controls.Add((Control)(object)TbxDateFrom);
		((Control)paneldays).Controls.Add((Control)(object)labelX2);
		((Control)paneldays).Controls.Add((Control)(object)labelX5);
		paneldays.DisabledBackColor = Color.Empty;
		((Control)paneldays).Location = new Point(10, 82);
		((Control)paneldays).Margin = new Padding(4, 3, 4, 3);
		((Control)paneldays).Name = "paneldays";
		((Control)paneldays).Size = new Size(424, 150);
		paneldays.Style.Alignment = (StringAlignment)1;
		paneldays.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		paneldays.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		paneldays.Style.Border = (eBorderType)1;
		paneldays.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		paneldays.Style.BorderWidth = 2;
		paneldays.Style.CornerType = (eCornerType)2;
		paneldays.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		paneldays.Style.GradientAngle = 90;
		((Control)paneldays).TabIndex = 5;
		TbxDateTo.AllowNull = false;
		((Control)TbxDateTo).Anchor = (AnchorStyles)9;
		TbxDateTo.BackcolorTooltip = Color.Red;
		((TextBoxX)TbxDateTo).Border.CornerType = (eCornerType)1;
		TbxDateTo.EnterToTab = true;
		TbxDateTo.EscToClose = true;
		TbxDateTo.FarsyDateStyle = (ENFarsyDateStyle)1;
		TbxDateTo.FocusBackColor = Color.Yellow;
		TbxDateTo.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TbxDateTo.FocusForeColor = Color.Blue;
		TbxDateTo.FocusTextSelect = true;
		((Control)TbxDateTo).Font = new Font("Arial", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TbxDateTo).Location = new Point(26, 18);
		((Control)TbxDateTo).Margin = new Padding(4, 3, 4, 3);
		TbxDateTo.MessageEmptyShowDialog = false;
		TbxDateTo.MessegeEmpty = "";
		TbxDateTo.MessegeEmptyInFormRight = true;
		TbxDateTo.MessegeEmptyShowInForm = false;
		TbxDateTo.MiladiDate = new DateTime(2024, 9, 18, 6, 42, 18, 769);
		((Control)TbxDateTo).Name = "TbxDateTo";
		((Control)TbxDateTo).RightToLeft = (RightToLeft)0;
		TbxDateTo.ShowNameOfDayOnToolTip = false;
		TbxDateTo.ShowToolTipText = false;
		((Control)TbxDateTo).Size = new Size(103, 20);
		((Control)TbxDateTo).TabIndex = 9;
		((Control)TbxDateTo).Text = "17/09/2024";
		((TextBox)TbxDateTo).TextAlign = (HorizontalAlignment)2;
		TbxDateTo.TootTipDelay = 500;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(136, 17);
		((Control)labelX1).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(55, 23);
		((Control)labelX1).TabIndex = 8;
		((Control)labelX1).Text = "تا تاریخ:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)ChkMorkhasiTatilat).AutoSize = true;
		((Control)ChkMorkhasiTatilat).ForeColor = Color.Maroon;
		((Control)ChkMorkhasiTatilat).Location = new Point(80, 49);
		((Control)ChkMorkhasiTatilat).Margin = new Padding(4, 3, 4, 3);
		((Control)ChkMorkhasiTatilat).Name = "ChkMorkhasiTatilat";
		((Control)ChkMorkhasiTatilat).Size = new Size(250, 17);
		((Control)ChkMorkhasiTatilat).TabIndex = 7;
		((Control)ChkMorkhasiTatilat).Text = "[محاسبه روزهای تعطیل به عنوان مرخصی]";
		((ButtonBase)ChkMorkhasiTatilat).UseVisualStyleBackColor = true;
		descdays.Border.Class = "TextBoxBorder";
		descdays.Border.CornerType = (eCornerType)1;
		((Control)descdays).Location = new Point(18, 72);
		((Control)descdays).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)descdays).Multiline = true;
		((Control)descdays).Name = "descdays";
		((Control)descdays).Size = new Size(314, 64);
		((Control)descdays).TabIndex = 6;
		TbxDateFrom.AllowNull = false;
		((Control)TbxDateFrom).Anchor = (AnchorStyles)9;
		TbxDateFrom.BackcolorTooltip = Color.Red;
		((TextBoxX)TbxDateFrom).Border.CornerType = (eCornerType)1;
		TbxDateFrom.EnterToTab = true;
		TbxDateFrom.EscToClose = true;
		TbxDateFrom.FarsyDateStyle = (ENFarsyDateStyle)1;
		TbxDateFrom.FocusBackColor = Color.Yellow;
		TbxDateFrom.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TbxDateFrom.FocusForeColor = Color.Blue;
		TbxDateFrom.FocusTextSelect = true;
		((Control)TbxDateFrom).Font = new Font("Arial", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TbxDateFrom).Location = new Point(226, 18);
		((Control)TbxDateFrom).Margin = new Padding(4, 3, 4, 3);
		TbxDateFrom.MessageEmptyShowDialog = false;
		TbxDateFrom.MessegeEmpty = "";
		TbxDateFrom.MessegeEmptyInFormRight = true;
		TbxDateFrom.MessegeEmptyShowInForm = false;
		TbxDateFrom.MiladiDate = new DateTime(2024, 9, 18, 3, 55, 15, 769);
		((Control)TbxDateFrom).Name = "TbxDateFrom";
		((Control)TbxDateFrom).RightToLeft = (RightToLeft)0;
		TbxDateFrom.ShowNameOfDayOnToolTip = false;
		TbxDateFrom.ShowToolTipText = false;
		((Control)TbxDateFrom).Size = new Size(103, 20);
		((Control)TbxDateFrom).TabIndex = 1;
		((Control)TbxDateFrom).Text = "17/09/2024";
		((TextBox)TbxDateFrom).TextAlign = (HorizontalAlignment)2;
		TbxDateFrom.TootTipDelay = 500;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(335, 91);
		((Control)labelX2).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(84, 23);
		((Control)labelX2).TabIndex = 5;
		((Control)labelX2).Text = "شرح مرخصی";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(336, 17);
		((Control)labelX5).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).RightToLeft = (RightToLeft)1;
		((Control)labelX5).Size = new Size(71, 23);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "از تاریخ:";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 239);
		((Control)BtnCancel).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(127, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 7;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(146, 239);
		((Control)BtnAccept).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(288, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 6;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(446, 280);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Form)this).Margin = new Padding(4, 3, 4, 3);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmMorakhasiDaily";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[مرخصی روزانه]";
		((Form)this).Load += FRMMorakhasiDaily_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMMorakhasiDaily_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)personBindingSource).EndInit();
		((Control)paneldays).ResumeLayout(false);
		((Control)paneldays).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
