using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Business;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters.Attendance;
using PacketParser.Settings.Attendance;
using PayamakUi.Forms.FRMOprate;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendancePrint.UI;
using TimeAttendanceUI.Forms.Gharardads;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Forms.Person.Group;
using TimeAttendanceUI.Forms.Service;
using TimeAttendanceUI.Forms.Taradod;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.UC.Base;

public class UcPersonel : UserControl, IHasContext
{
	public delegate void ExitUserControlEventHandler();

	private Font _font;

	private CancellationTokenSource tokenTranslate = new CancellationTokenSource();

	private CancellationTokenSource TokenFrm = new CancellationTokenSource();

	private CancellationTokenSource TokenLoadData = new CancellationTokenSource();

	private CancellationTokenSource TokenSendToServer = new CancellationTokenSource();

	public string SelectedCodeM = "";

	public string SelectedCodeT = "";

	public List<int> CustomersList;

	private List<PersonGroup> _lstPersonGroup;

	private List<Person> _personList;

	private IContainer components;

	internal PanelEx PanelEx1;

	internal ButtonX BtnDelete;

	internal ButtonX BtnEdit;

	internal ButtonX BtnAdd;

	internal DataGridViewX DGrid;

	internal GroupPanel GroupPanel1;

	internal LabelX LabelX1;

	internal TextBoxX TxtSearch;

	internal DataGridViewX DGridGroup;

	private ButtonX BtnPrint;

	private LabelX LblRecordCount;

	private LabelX labelX2;

	internal ButtonX BtnSelect;

	private PictureBox PIC;

	private ContextMenuStrip CMAGroup;

	private ToolStripMenuItem افزودنگروهجدیدToolStripMenuItem;

	private ToolStripMenuItem ویرایشگروهToolStripMenuItem;

	private ToolStripMenuItem حذفگروهToolStripMenuItem;

	private BindingSource persongroupBindingSource1;

	private BindingSource personBindingSource1;

	private ToolStripMenuItem ToolStripMIAdd;

	private ToolStripMenuItem ToolStripMIEdit;

	private ToolStripMenuItem ToolStripMIDelete;

	private ToolStripMenuItem ToolStripMIPrint;

	private ToolStripMenuItem ToolStripMIDisplayGharardad;

	private ToolStripMenuItem ToolStripMIDisplayMorakhasi;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem انتخابToolStripMenuItem;

	private ToolStripMenuItem ToolStripMISelectAll;

	private ToolStripMenuItem ToolStripMISelectNone;

	private ToolStripMenuItem ToolStripMISelectReverse;

	private ToolStripMenuItem ToolStripMISendSms;

	private DataGridViewTextBoxColumn DGGroupName;

	private DataGridViewTextBoxColumn DgvGroupId;

	private ButtonX btnSendToHost;

	private CheckBox ChkActivePersons;

	private ToolStripMenuItem ToolStripMenuItem_enablePerson;

	internal ButtonX btnLog;

	private DataGridViewTextBoxColumn DGID;

	private DataGridViewImageColumn dgServerStatus;

	private DataGridViewCheckBoxColumn DgChecked;

	private DataGridViewTextBoxColumn DGPicPath;

	private DataGridViewTextBoxColumn DGCodeMoein;

	private DataGridViewTextBoxColumn DGCodeTafsil;

	private DataGridViewTextBoxColumn DGPersonName;

	private DataGridViewTextBoxColumn adressDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn mob1DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn tel1DataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn DGHardwareCode;

	private DataGridViewCheckBoxColumn St;

	internal ButtonX btnImport;

	public string ContextGuid { get; set; }

	public Person SelectedPerson { get; private set; }

	private List<Person> checkedPersons => (from Person person in personBindingSource1.List
		where ((PersonBase)person).Checked
		select person).ToList();

	private List<Person> PersonSelected
	{
		get
		{
			try
			{
				List<Person> list = (from Person person in personBindingSource1.List
					where ((PersonBase)person).Checked
					select person).ToList();
				return (list.Count != 0) ? list : ((personBindingSource1.Current == null) ? null : new List<Person>(1) { (Person)personBindingSource1.Current });
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	public short GroupId
	{
		get
		{
			try
			{
				return (short)((((DataGridView)DGridGroup).RowCount > 0 && ((DataGridView)DGridGroup).CurrentRow != null && DgvGroupId != null) ? short.Parse(((DataGridView)DGridGroup)[((DataGridViewBand)DgvGroupId).Index, ((DataGridViewBand)((DataGridView)DGridGroup).CurrentRow).Index].Value.ToString()) : 0);
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GroupId", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 300);
				return 0;
			}
		}
	}

	private string CodeM
	{
		get
		{
			try
			{
				return (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null) ? "" : ((DataGridView)DGrid)[((DataGridViewBand)DGCodeMoein).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CodeM", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 316);
				return "";
			}
		}
	}

	private string CodeT
	{
		get
		{
			try
			{
				return (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null) ? "" : ((DataGridView)DGrid)[((DataGridViewBand)DGCodeTafsil).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CodeT", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 332);
				return "";
			}
		}
	}

	public event ExitUserControlEventHandler Exit_UserControl;

	private async Task<List<Person>> GetSelectedPersonsAsync()
	{
		try
		{
			List<Person> list = (from Person person in personBindingSource1.List
				where ((PersonBase)person).Checked
				select person).ToList();
			if (list != null && list.Any())
			{
				return list;
			}
			List<Person> tempList = new List<Person>();
			if (((DataGridView)DGridGroup).CurrentRow != null && ((DataGridViewBand)((DataGridView)DGridGroup).CurrentRow).Index > 0)
			{
				string searchStr = ((DataGridView)DGridGroup).CurrentCell.Value.ToString().Trim();
				foreach (Person person2 in _personList)
				{
					string text = await person2.GetGroupNameAsync();
					if (!string.IsNullOrEmpty(text) && text.Contains(searchStr))
					{
						tempList.Add(person2);
					}
				}
				return tempList;
			}
			return _personList;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public UcPersonel(string contextGuid, Font font)
	{
		try
		{
			InitializeComponent();
			ContextGuid = contextGuid;
			_font = font;
			Person.OnServerStatusChanged += Person_OnServerStatusChanged;
			tokenTranslate?.Cancel();
			tokenTranslate = new CancellationTokenSource();
			CancellationToken token = tokenTranslate.Token;
			translateControlsAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 124);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			Person.OnServerStatusChanged -= Person_OnServerStatusChanged;
			components?.Dispose();
			((ContainerControl)this).Dispose(disposing);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task Person_OnServerStatusChanged()
	{
		try
		{
			Form obj = ((Control)this).FindForm();
			if (obj == null || ((Control)obj).IsDisposed)
			{
				return;
			}
			TokenSendToServer?.Cancel();
			TokenSendToServer = new CancellationTokenSource();
			CancellationToken token = TokenSendToServer.Token;
			await Task.Delay(2000);
			if (token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				Form obj2 = ((Control)this).FindForm();
				if (obj2 == null || ((Control)obj2).IsDisposed)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Func<Task>)async delegate
			{
				Form obj3 = ((Control)this).FindForm();
				if (obj3 == null || ((Control)obj3).IsDisposed)
				{
					return;
				}
				try
				{
					await LoadDataAsync(token);
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token);
			Stopwatch sw = new Stopwatch();
			sw.Start();
			_personList = await Person.GetAllAsync(ContextGuid, new List<int>(), ChkActivePersons.Checked, token);
			if (ChkActivePersons.Checked)
			{
				_personList = _personList?.Where((Person p) => ((PersonBase)p).St).ToList();
			}
			_ = sw.ElapsedMilliseconds;
			sw.Stop();
			DeviceServiceSettings deviceServiceSetting = await LocalSettingBusiness.GetDeviceServiceSettingsAsync(ContextGuid, token, true);
			do
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !((Control)this).IsDisposed)
					{
						((Control)DGrid).Font = setting.FontProgram;
						personBindingSource1.DataSource = _personList;
						UserAttSetting userAttSetting = UserAttendance.SelectedUser?.UserSetting;
						if (userAttSetting != null)
						{
							DataGridViewImageColumn obj = dgServerStatus;
							bool visible = (((Control)btnSendToHost).Enabled = userAttSetting.EnableCloudSignalR);
							((DataGridViewBand)obj).Visible = visible;
						}
						((Control)BtnAdd).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Personnel.PersonnelInsert;
						((Control)BtnDelete).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Personnel.PersonnelDelete;
						((Control)BtnEdit).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Personnel.PersonnelUpdate;
						((Control)BtnPrint).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Personnel.PersonnelPrint;
						((Control)btnLog).Enabled = deviceServiceSetting.HandleService;
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
			await LoadDataGroupAsync(default(CancellationToken));
			await SearchAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 211);
		}
	}

	private async Task CalculateHardwareCodeAsync(int rowIndex, int personId, List<HardwareCodeCus> hardwareCodes, CancellationToken token)
	{
		try
		{
			string result = await Task.Run(() => GetHardwareCodes(personId, hardwareCodes), token);
			if (token.IsCancellationRequested)
			{
				return;
			}
			if (((Control)DGrid).InvokeRequired)
			{
				((Control)DGrid).Invoke((Delegate)(Action)delegate
				{
					((DataGridView)DGrid).Rows[rowIndex].Cells[((DataGridViewBand)DGHardwareCode).Index].Value = result;
				});
			}
			else
			{
				((DataGridView)DGrid).Rows[rowIndex].Cells[((DataGridViewBand)DGHardwareCode).Index].Value = result;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
	}

	private string GetHardwareCodes(int personId, List<HardwareCodeCus> hardwareCodes)
	{
		try
		{
			return string.Join(",", (from p in hardwareCodes
				where p.PersonId == personId
				select p.HardwareCode).ToList());
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return "";
	}

	private async Task LoadDataGroupAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			((Control)DGridGroup).Focus();
			DataGridViewX dGridGroup = DGridGroup;
			((Control)dGridGroup).Font = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token)).FontProgram;
			_lstPersonGroup = await PersonGroup.SelectAllAsync(ContextGuid, containsAllGroups: true, token);
			do
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					persongroupBindingSource1.DataSource = _lstPersonGroup;
					for (int i = 0; i < _lstPersonGroup.Count - 1; i++)
					{
						_lstPersonGroup[i].Name = " " + _lstPersonGroup[i].Name;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 280);
		}
	}

	private async Task translateControlsAsync(CancellationToken token)
	{
		try
		{
			string toolStripMiAdd = Utilities.GetTranslate("[اضافه]");
			string toolStripMiEdit = Utilities.GetTranslate("[ویرایش]");
			string toolStripMiDelete = Utilities.GetTranslate("[شخص غیر فعال شود]");
			string toolStripMiPrint = Utilities.GetTranslate("[چاپ]");
			string toolStripMiDisplayGharardad = Utilities.GetTranslate("[مشاهده قرارداد]");
			string toolStripMiDisplayMorakhasi = Utilities.GetTranslate("[مشاهده مرخصی]");
			string toolStripMenuItem = Utilities.GetTranslate("[انتخاب]");
			string toolStripMiSelectAll = Utilities.GetTranslate("[انتخاب همه]");
			string toolStripMiSelectNone = Utilities.GetTranslate("[پاک کردن همه]");
			string toolStripMiSelectReverse = Utilities.GetTranslate("[معکوس کردن انتخاب ها]");
			string toolStripMiSendSms = Utilities.GetTranslate("[ارسال پیامک]");
			string addToolStripMenuItem = Utilities.GetTranslate("[افزودن گروه جدید]");
			string editToolStripMenuItem = Utilities.GetTranslate("[ویرایش گروه]");
			string deleteToolStripMenuItem = Utilities.GetTranslate("[حذف گروه]");
			while (!((Control)this).IsHandleCreated && !((Control)this).IsDisposed && !token.IsCancellationRequested)
			{
				await Task.Delay(100);
			}
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !((Control)this).IsDisposed)
					{
						((ToolStripItem)ToolStripMIAdd).Text = toolStripMiAdd;
						((ToolStripItem)ToolStripMIEdit).Text = toolStripMiEdit;
						((ToolStripItem)ToolStripMIDelete).Text = toolStripMiDelete;
						((ToolStripItem)ToolStripMIPrint).Text = toolStripMiPrint;
						((ToolStripItem)ToolStripMIDisplayGharardad).Text = toolStripMiDisplayGharardad;
						((ToolStripItem)ToolStripMIDisplayMorakhasi).Text = toolStripMiDisplayMorakhasi;
						((ToolStripItem)انتخابToolStripMenuItem).Text = toolStripMenuItem;
						((ToolStripItem)ToolStripMISelectAll).Text = toolStripMiSelectAll;
						((ToolStripItem)ToolStripMISelectNone).Text = toolStripMiSelectNone;
						((ToolStripItem)ToolStripMISelectReverse).Text = toolStripMiSelectReverse;
						((ToolStripItem)ToolStripMISendSms).Text = toolStripMiSendSms;
						((ToolStripItem)افزودنگروهجدیدToolStripMenuItem).Text = addToolStripMenuItem;
						((ToolStripItem)ویرایشگروهToolStripMenuItem).Text = editToolStripMenuItem;
						((ToolStripItem)حذفگروهToolStripMenuItem).Text = deleteToolStripMenuItem;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "translateControlsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 385);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "translateControlsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 393);
		}
	}

	private async void BtnAdd_Click(object sender, EventArgs e)
	{
		if (!UserAttendance.SelectedUser.AccessLevelMerg.Personnel.CanInsert(UserAttendance.SelectedUser.AccessLevelMerg, personBindingSource1.Count + 1))
		{
			Utility.MessageBoxShowError("[با توجه به سطح دسترسی نرم افزار، شما اجازه ندارید کارمند ایجاد نمائید]", ContextGuid);
			return;
		}
		try
		{
			TokenLoadData?.Cancel();
			TokenLoadData = new CancellationTokenSource();
			CancellationToken token = TokenLoadData.Token;
			((Control)DGrid).Focus();
			Person person = await Person.FactoryAsync(ContextGuid);
			await ((CacheBaseForm)new FrmPersonel(ContextGuid, person, _font, isReadOnly: false)).ShowAsync((IWin32Window)(object)this);
			await LoadDataAsync(token);
			LogOnPersonel.CalcPersonIdAsync(ContextGuid, new CancellationTokenSource(30000).Token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BtnAdd_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 421);
		}
	}

	public void Insert()
	{
		((PopupItemControl)BtnAdd).PerformClick();
	}

	public void UpDate()
	{
		((PopupItemControl)BtnEdit).PerformClick();
	}

	public void Delete()
	{
		((PopupItemControl)BtnDelete).PerformClick();
	}

	private async void BtnDelete_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (checkedPersons == null || checkedPersons.Count <= 0)
			{
				FMessageBox.Show("هیچ شخصی انتخاب نشده است");
				return;
			}
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			if ((int)FMessageBox.Show(Utilities.GetTranslate("[مایل به غیر فعال کردن اشخاص انتخاب شده هستید؟]") + $"\r\n تعداد {checkedPersons.Count} شخص", Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Exclamation, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				foreach (Person person in checkedPersons)
				{
					await HardwareCodeCus.DeleteAsync(ContextGuid, ((PersonBase)person).Id, token);
					((PersonBase)person).St = false;
					if (await person.SaveAsync(token) && !string.IsNullOrEmpty(((PersonBase)person).PicPath) && File.Exists(((PersonBase)person).PicPath))
					{
						File.Delete(((PersonBase)person).PicPath);
					}
				}
				TokenLoadData?.Cancel();
				TokenLoadData = new CancellationTokenSource();
				CancellationToken token2 = TokenLoadData.Token;
				await LoadDataAsync(token2);
			}
			LogOnPersonel.CalcPersonIdAsync(ContextGuid, new CancellationTokenSource(30000).Token);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BtnDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 471);
		}
	}

	private async void BtnEdit_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)DGrid).Focus();
			if (SelectedPerson != null)
			{
				TokenLoadData?.Cancel();
				TokenLoadData = new CancellationTokenSource();
				CancellationToken token = TokenLoadData.Token;
				FrmPersonel frm = new FrmPersonel(ContextGuid, SelectedPerson, _font, isReadOnly: false);
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					await LoadDataAsync(token);
				}
				((Component)(object)frm).Dispose();
				LogOnPersonel.CalcPersonIdAsync(ContextGuid, new CancellationTokenSource(30000).Token);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "BtnEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 490);
		}
	}

	public bool SelectPerform()
	{
		try
		{
			((PopupItemControl)BtnSelect).PerformClick();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SelectPerform", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 503);
		}
		return false;
	}

	public void Print()
	{
		try
		{
			((PopupItemControl)BtnPrint).PerformClick();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Print", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 515);
		}
	}

	private async void BtnPrint_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			if (!((Control)BtnPrint).Enabled || !((Control)BtnPrint).Visible)
			{
				return;
			}
			((Control)DGrid).Focus();
			List<object> lst = new List<object>();
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			foreach (Person item in await GetSelectedPersonsAsync())
			{
				string text = "";
				List<object> list = lst;
				long codeMoein = ((PersonBase)item).CodeMoein;
				long codeTafsil = ((PersonBase)item).CodeTafsil;
				string fullName = ((PersonBase)item).FullName;
				string codeMeli = ((PersonBase)item).CodeMeli;
				string address = ((PersonBase)item).Address;
				string mob = ((PersonBase)item).Mob1;
				string mob2 = ((PersonBase)item).Mob2;
				string tel = ((PersonBase)item).Tel1;
				string tel2 = ((PersonBase)item).Tel2;
				string fatherName = ((PersonBase)item).FatherName;
				string dateBirthday = Calendar.MiladiToCalture(((PersonBase)item).DateMBirthday);
				string dateStartWork = Calendar.MiladiToCalture(((PersonBase)item).DateMStartWork);
				string dateFinishWork = Calendar.MiladiToCalture(((PersonBase)item).DateMFinishW);
				int hid = ((PersonBase)item).Hid;
				string shsh = ((PersonBase)item).Shsh;
				string sodorSh = ((PersonBase)item).SodorSh;
				short jensiat = ((PersonBase)item).Jensiat;
				string hardwareCode = text;
				list.Add(new
				{
					CodeMoein = codeMoein,
					CodeTafsil = codeTafsil,
					FullName = fullName,
					CodeMeli = codeMeli,
					Adress = address,
					Mob1 = mob,
					Mob2 = mob2,
					Tel1 = tel,
					Tel2 = tel2,
					FatherName = fatherName,
					DateBirthday = dateBirthday,
					DateStartWork = dateStartWork,
					DateFinishWork = dateFinishWork,
					HID = hid,
					Shsh = shsh,
					SodorSh = sodorSh,
					Jensiat = jensiat,
					HardwareCode = hardwareCode,
					GroupName = await item.GetGroupNameAsync(token),
					MadrakTahsili = ((PersonBase)item).MadrakTahsili,
					Description = ((PersonBase)item).Description,
					MTavalod = ((PersonBase)item).MTavalod,
					NoBimeh = ((PersonBase)item).NoBimeh,
					ZipCode = ((PersonBase)item).ZipCode,
					SerialSh = ((PersonBase)item).SerialSh,
					Family = ((PersonBase)item).Family,
					Name = ((PersonBase)item).Name
				});
			}
			FrmPrint frm = new FrmPrint(ContextGuid, (EnumPrintShow)2, lst, null, 0, "");
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnPrint_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 570);
		}
	}

	public void BtnSelect_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGrid).Focus();
			if (((Control)BtnSelect).Enabled && ((Control)BtnSelect).Visible && SelectedPerson != null)
			{
				SelectedCodeM = CodeM;
				SelectedCodeT = CodeT;
				this.Exit_UserControl?.Invoke();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnSelect_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 584);
		}
	}

	private bool LoadPicture(string fileName)
	{
		bool result = false;
		try
		{
			if (File.Exists(fileName))
			{
				PIC.SizeMode = (PictureBoxSizeMode)1;
				PIC.Image = Image.FromFile(fileName);
				result = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadPicture", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 601);
		}
		return result;
	}

	private async Task SearchAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(200);
			if (token.IsCancellationRequested)
			{
				return;
			}
			string text = Text.FixString(((Control)TxtSearch).Text).Trim();
			List<Person> list = _personList;
			if (GroupId > 0)
			{
				list = list.Where((Person x) => ((PersonBase)x).GroupId == GroupId)?.ToList();
			}
			if (text != "")
			{
				string[] array = text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (string item in array)
				{
					list = list?.Where((Person p) => p != null && (Text.StringComparisons(((PersonBase)p).FullName, item, StringComparison.OrdinalIgnoreCase) || Text.StringComparisons(((PersonBase)p).CodeTafsil, item, StringComparison.OrdinalIgnoreCase)))?.ToList();
				}
			}
			if (token.IsCancellationRequested)
			{
				return;
			}
			personBindingSource1.DataSource = list;
			personBindingSource1.ResetBindings(false);
			((Control)LblRecordCount).Text = personBindingSource1.Count.ToString("N0");
			if (token.IsCancellationRequested)
			{
				return;
			}
			List<HardwareCodeCus> hardwareCodes = await HardwareCodeCus.GetAllAsync(ContextGuid, token);
			foreach (DataGridViewRow item2 in (IEnumerable)((DataGridView)DGrid).Rows)
			{
				DataGridViewRow val = item2;
				CalculateHardwareCodeAsync(((DataGridViewBand)val).Index, int.Parse(val.Cells[((DataGridViewBand)DGID).Index].Value.ToString()), hardwareCodes, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 635);
		}
	}

	private async Task Sort_DGridListsAsync(int ColumnIndex, CancellationToken token)
	{
		try
		{
			if (((DataGridViewBand)DGPersonName).Index == ColumnIndex)
			{
				((List<Person>)personBindingSource1.DataSource).Sort((Person x, Person y) => string.Compare(((PersonBase)x).FullName, ((PersonBase)y).FullName, StringComparison.Ordinal));
			}
			else if (((DataGridViewBand)DGCodeMoein).Index == ColumnIndex)
			{
				((List<Person>)personBindingSource1.DataSource).Sort((Person x, Person y) => ((PersonBase)x).CodeMoein.CompareTo(((PersonBase)y).CodeMoein));
			}
			else if (((DataGridViewBand)DGCodeTafsil).Index == ColumnIndex)
			{
				((List<Person>)personBindingSource1.DataSource).Sort((Person x, Person y) => ((PersonBase)x).CodeTafsil.CompareTo(((PersonBase)y).CodeTafsil));
			}
			else if (((DataGridViewBand)adressDataGridViewTextBoxColumn).Index == ColumnIndex)
			{
				((List<Person>)personBindingSource1.DataSource).Sort((Person x, Person y) => string.Compare(((PersonBase)x).Address, ((PersonBase)y).Address, StringComparison.Ordinal));
			}
			else if (((DataGridViewBand)mob1DataGridViewTextBoxColumn).Index == ColumnIndex)
			{
				((List<Person>)personBindingSource1.DataSource).Sort((Person x, Person y) => string.Compare(((PersonBase)x).Mob1, ((PersonBase)y).Mob1, StringComparison.Ordinal));
			}
			else if (((DataGridViewBand)tel1DataGridViewTextBoxColumn).Index == ColumnIndex)
			{
				((List<Person>)personBindingSource1.DataSource).Sort((Person x, Person y) => string.Compare(((PersonBase)x).Tel1, ((PersonBase)y).Tel1, StringComparison.Ordinal));
			}
			personBindingSource1.ResetBindings(false);
			List<HardwareCodeCus> hardwareCodes = await HardwareCodeCus.GetAllAsync(ContextGuid, token);
			foreach (DataGridViewRow item in (IEnumerable)((DataGridView)DGrid).Rows)
			{
				DataGridViewRow val = item;
				CalculateHardwareCodeAsync(((DataGridViewBand)val).Index, int.Parse(val.Cells[((DataGridViewBand)DGID).Index].Value.ToString()), hardwareCodes, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Sort_DGridListsAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 664);
		}
	}

	private void TxtSearch_Enter(object sender, EventArgs e)
	{
		try
		{
			((TextBoxBase)TxtSearch).SelectAll();
			((Control)TxtSearch).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 678);
		}
	}

	private void TxtSearch_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtSearch).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 689);
		}
	}

	private void TxtSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TokenLoadData?.Cancel();
			TokenLoadData = new CancellationTokenSource();
			CancellationToken token = TokenLoadData.Token;
			SearchAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 703);
		}
	}

	private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13 || (int)e.KeyCode == 40)
			{
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSearch_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 715);
		}
	}

	private void DGridGroup_CellEnter(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			TokenLoadData?.Cancel();
			TokenLoadData = new CancellationTokenSource();
			CancellationToken token = TokenLoadData.Token;
			SearchAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGridGroup_CellEnter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 732);
		}
	}

	private void DGridGroup_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 37)
			{
				e.Handled = true;
				((Control)DGrid).Focus();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGridGroup_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 749);
		}
	}

	private void DGridGroup_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			char c = char.Parse(Text.FixString(e.KeyChar.ToString()));
			if (!Text.IsStringOrNumber(e.KeyChar) || ((DataGridView)DGridGroup).CurrentRow == null)
			{
				return;
			}
			int num = ((DataGridViewBand)((DataGridView)DGridGroup).CurrentRow).Index;
			for (int i = 0; i <= ((DataGridView)DGridGroup).RowCount - 1; i++)
			{
				num++;
				if (num == _lstPersonGroup.Count)
				{
					num = 0;
				}
				if (!(((DataGridView)DGridGroup)[((DataGridViewBand)DGGroupName).Index, num].Value.ToString().Substring(2, 1) != c.ToString()))
				{
					((DataGridView)DGridGroup).CurrentCell = ((DataGridView)DGridGroup)[((DataGridViewBand)DGGroupName).Index, num];
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGridGroup_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 772);
		}
	}

	private void DGridGroup_Enter(object sender, EventArgs e)
	{
		try
		{
			((DataGridView)DGridGroup).BackgroundColor = Color.SteelBlue;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGridGroup_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 783);
		}
	}

	private void DGridGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			((DataGridView)DGridGroup).DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGridGroup_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 794);
		}
	}

	private async void افزودنگروهجدیدToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)DGridGroup).Focus();
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			if ((int)((Form)new FRMPersonGroup(ContextGuid)).ShowDialog((IWin32Window)(object)this) == 1)
			{
				await LoadDataGroupAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "افزودنگروهجدیدToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 814);
		}
	}

	private async void ویرایشگروهToolStripMenuItem_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		try
		{
			if (GroupId > 0)
			{
				TokenFrm?.Cancel();
				TokenFrm = new CancellationTokenSource();
				CancellationToken token = TokenFrm.Token;
				if ((int)((Form)new FRMPersonGroup(ContextGuid, GroupId)).ShowDialog() == 1)
				{
					await LoadDataGroupAsync(token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ویرایشگروهToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 832);
		}
	}

	private async void حذفگروهToolStripMenuItem_Click(object sender, EventArgs e)
	{
		((Control)DGrid).Focus();
		try
		{
			if (GroupId <= 0)
			{
				return;
			}
			if (((DataGridView)DGrid).RowCount == 1)
			{
				Utility.MessageBoxShowError("[حداقل یک گروه  باید در سیستم وجود داشته باشد]", ContextGuid);
				return;
			}
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			int num = await PersonGroup.CountGroupIDAsync(ContextGuid, GroupId, token);
			if (num > 0)
			{
				Utility.MessageBoxShowError("[به تعداد] " + num + " [شخص دراین گروه وجود دارد ، جهت حذف گروه ابتدا باید گروه این اشخاص را تغییر دهید سپس اقدام به حذف نمایید]", ContextGuid);
			}
			else if ((int)FMessageBox.Show(Utilities.GetTranslate("[برای حذف کردن مطمئن هستید؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7)
			{
				PersonGroup obj = await PersonGroup.GetAsync(ContextGuid, GroupId, token);
				obj.St = false;
				await obj.SaveAsync(token);
				await LoadDataAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "حذفگروهToolStripMenuItem_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 866);
		}
	}

	private async void ToolStripMIDisplayGharardad_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			string msg = "این شخص در روز جاری قرارداد ندارد";
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			GharardadNew gharardadNew = await GharardadNew.GetAsync(ContextGuid, ((PersonBase)SelectedPerson).Id, DateTime.Now, token);
			if (gharardadNew == null || gharardadNew.ID <= 0)
			{
				Utility.MessageBoxShowError(msg, ContextGuid);
				return;
			}
			FrmGharardad frm = new FrmGharardad(ContextGuid, _font, gharardadNew);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToolStripMIDisplayGharardad_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 889);
		}
	}

	private async void ToolStripMIDisplayMorakhasi_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			string msg = "این شخص در روز جاری قرارداد ندارد";
			TokenFrm?.Cancel();
			TokenFrm = new CancellationTokenSource();
			CancellationToken token = TokenFrm.Token;
			GharardadNew gharardadNew = await GharardadNew.GetAsync(ContextGuid, ((PersonBase)SelectedPerson).Id, DateTime.Now, token);
			if (gharardadNew == null || gharardadNew.ID <= 0)
			{
				Utility.MessageBoxShowError(msg, ContextGuid);
				return;
			}
			FrmReportMorakhasi frm = new FrmReportMorakhasi(ContextGuid, gharardadNew, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			((Component)(object)frm).Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToolStripMIDisplayMorakhasi_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 912);
		}
	}

	private void ToolStripMISelectAll_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (Person item in personBindingSource1.List)
			{
				((PersonBase)item).Checked = true;
			}
			personBindingSource1.ResetBindings(false);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToolStripMISelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 925);
		}
	}

	private void ToolStripMISelectNone_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (Person item in personBindingSource1.List)
			{
				((PersonBase)item).Checked = false;
			}
			personBindingSource1.ResetBindings(false);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToolStripMISelectNone_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 939);
		}
	}

	private void ToolStripMISelectReverse_Click(object sender, EventArgs e)
	{
		try
		{
			foreach (Person item in personBindingSource1.List)
			{
				((PersonBase)item).Checked = !((PersonBase)item).Checked;
			}
			personBindingSource1.ResetBindings(false);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToolStripMISelectReverse_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 953);
		}
	}

	private async void ToolStripMISendSms_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			List<Person> source = await GetSelectedPersonsAsync();
			List<string> list = new List<string>();
			list.AddRange(source.Select((Person x) => ((PersonBase)x).Mob1));
			list.AddRange(source.Select((Person x) => ((PersonBase)x).Mob2));
			list = list?.Distinct()?.ToList();
			await ((CacheBaseForm)new FRMSend(ContextGuid)
			{
				Mobiles = string.Join(";", list)
			}).ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ToolStripMISendSms_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 971);
		}
	}

	private void DGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).CurrentCell != null && e.ColumnIndex == ((DataGridViewBand)DgChecked).Index)
			{
				((PersonBase)(Person)personBindingSource1.Current).Checked = !((PersonBase)(Person)personBindingSource1.Current).Checked;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellContentClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 987);
		}
	}

	private void DGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		TokenLoadData?.Cancel();
		TokenLoadData = new CancellationTokenSource();
		CancellationToken token = TokenLoadData.Token;
		Sort_DGridListsAsync(e.ColumnIndex, token);
	}

	private void DGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode == 32)
				{
					personBindingSource1.ResetBindings(false);
					e.Handled = true;
				}
			}
			else if (((Control)BtnSelect).Visible)
			{
				((PopupItemControl)BtnSelect).PerformClick();
				e.Handled = true;
			}
			else
			{
				((PopupItemControl)BtnEdit).PerformClick();
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 1019);
		}
	}

	private void DGrid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (Text.IsStringOrNumber(e.KeyChar))
			{
				((Control)TxtSearch).Focus();
				((Control)TxtSearch).Text = e.KeyChar.ToString();
				((TextBoxBase)TxtSearch).SelectionStart = 9999;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_KeyPress", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 1033);
		}
	}

	private void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount > 0 && ((DataGridView)DGrid).CurrentRow != null)
			{
				if (((DataGridView)DGrid)[((DataGridViewBand)DGPicPath).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value != null)
				{
					LoadPicture(((DataGridView)DGrid)[((DataGridViewBand)DGPicPath).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
				}
				else
				{
					PIC.Image = (Image)(object)Resources.people;
				}
			}
		}
		catch (Exception ex)
		{
			PIC.Image = (Image)(object)Resources.no_camera_sign;
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 1050);
		}
	}

	private void DGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount > 0 && ((DataGridView)DGrid).CurrentRow != null)
			{
				if (((DataGridView)DGrid)[((DataGridViewBand)DGPicPath).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value != null)
				{
					LoadPicture(((DataGridView)DGrid)[((DataGridViewBand)DGPicPath).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
				}
				else
				{
					PIC.Image = (Image)(object)Resources.people;
				}
			}
		}
		catch (Exception ex)
		{
			PIC.Image = (Image)(object)Resources.no_camera_sign;
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_CellEnter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 1068);
		}
	}

	private void DGrid_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (((Control)BtnSelect).Visible)
			{
				((PopupItemControl)BtnSelect).PerformClick();
			}
			else
			{
				((PopupItemControl)BtnEdit).PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DGrid_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 1083);
		}
	}

	private void GroupPanel1_Click(object sender, EventArgs e)
	{
	}

	private async void btnSendToHost_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)btnSendToHost).Enabled = false;
			((Control)DGrid).Focus();
			if (!(PersonSelected?.Any() ?? false))
			{
				return;
			}
			TokenSendToServer?.Cancel();
			TokenSendToServer = new CancellationTokenSource();
			CancellationToken token = TokenSendToServer.Token;
			foreach (Person item in PersonSelected)
			{
				item.sentToServerStatus = (ServerStatusForClient)0;
				await item.SaveAsync(token);
				Task.Run(() => item.SendToServerAsync(ContextGuid, token));
			}
			TokenLoadData?.Cancel();
			TokenLoadData = new CancellationTokenSource();
			CancellationToken token2 = TokenLoadData.Token;
			await LoadDataAsync(token2);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
		finally
		{
			((Control)btnSendToHost).Enabled = true;
		}
	}

	private async void personBindingSource1_CurrentChanged(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			Person person = (Person)personBindingSource1.Current;
			if (person != null)
			{
				if (person.ContextGuid.Length < 10)
				{
					await Task.Delay(1000);
				}
				SelectedPerson = person;
				if (SelectedPerson.ContextGuid.Length < 10)
				{
					await Task.Delay(1000);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void ChkActivePersons_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			TokenLoadData?.Cancel();
			TokenLoadData = new CancellationTokenSource();
			CancellationToken token = TokenLoadData.Token;
			await LoadDataAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void ToolStripMenuItem_EnablePerson_Click(object sender, EventArgs e)
	{
		try
		{
			if (SelectedPerson != null && !((PersonBase)SelectedPerson).St)
			{
				((PersonBase)SelectedPerson).St = true;
				await SelectedPerson.SaveAsync();
				TokenLoadData?.Cancel();
				TokenLoadData = new CancellationTokenSource();
				CancellationToken token = TokenLoadData.Token;
				SearchAsync(token);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
	{
		try
		{
			if (SelectedPerson != null)
			{
				((ToolStripItem)ToolStripMenuItem_enablePerson).Enabled = !((PersonBase)SelectedPerson).St;
				((ToolStripItem)ToolStripMIDelete).Enabled = ((PersonBase)SelectedPerson).St;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void btnLog_Click(object sender, EventArgs e)
	{
		try
		{
			string contextGuid = ContextGuid;
			BioWriteDeviceLogFilter val = new BioWriteDeviceLogFilter();
			Person selectedPerson = SelectedPerson;
			val.PersonGuid = ((selectedPerson != null) ? new Guid?(((PersonBase)selectedPerson).Guid) : null);
			await ((CacheBaseForm)new frmBioWriteDeviceLog(contextGuid, val)).ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnLog_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\UC\\Base\\UCPersonel.cs", 1183);
		}
	}

	private async void btnImport_Click(object sender, EventArgs e)
	{
		((Form)new FrmImportpersons(ContextGuid)).ShowDialog((IWin32Window)(object)this);
		TokenLoadData?.Cancel();
		TokenLoadData = new CancellationTokenSource();
		CancellationToken token = TokenLoadData.Token;
		await LoadDataAsync(token);
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Expected O, but got Unknown
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Expected O, but got Unknown
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Expected O, but got Unknown
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Expected O, but got Unknown
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Expected O, but got Unknown
		//IL_1461: Unknown result type (might be due to invalid IL or missing references)
		//IL_146b: Expected O, but got Unknown
		//IL_15e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ec: Expected O, but got Unknown
		//IL_16cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d6: Expected O, but got Unknown
		//IL_16fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1704: Expected O, but got Unknown
		//IL_1711: Unknown result type (might be due to invalid IL or missing references)
		//IL_171b: Expected O, but got Unknown
		//IL_1963: Unknown result type (might be due to invalid IL or missing references)
		//IL_196d: Expected O, but got Unknown
		//IL_1a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a41: Expected O, but got Unknown
		//IL_1b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1e: Expected O, but got Unknown
		//IL_1fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc3: Expected O, but got Unknown
		//IL_2006: Unknown result type (might be due to invalid IL or missing references)
		//IL_2010: Expected O, but got Unknown
		//IL_20d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20df: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		DataGridViewCellStyle val5 = new DataGridViewCellStyle();
		PanelEx1 = new PanelEx();
		BtnSelect = new ButtonX();
		btnImport = new ButtonX();
		btnLog = new ButtonX();
		DGrid = new DataGridViewX();
		DGID = new DataGridViewTextBoxColumn();
		dgServerStatus = new DataGridViewImageColumn();
		DgChecked = new DataGridViewCheckBoxColumn();
		DGPicPath = new DataGridViewTextBoxColumn();
		DGCodeMoein = new DataGridViewTextBoxColumn();
		DGCodeTafsil = new DataGridViewTextBoxColumn();
		DGPersonName = new DataGridViewTextBoxColumn();
		adressDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		mob1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		tel1DataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		DGHardwareCode = new DataGridViewTextBoxColumn();
		St = new DataGridViewCheckBoxColumn();
		contextMenuStrip1 = new ContextMenuStrip(components);
		ToolStripMIAdd = new ToolStripMenuItem();
		ToolStripMIEdit = new ToolStripMenuItem();
		ToolStripMIPrint = new ToolStripMenuItem();
		ToolStripMIDisplayGharardad = new ToolStripMenuItem();
		ToolStripMIDisplayMorakhasi = new ToolStripMenuItem();
		انتخابToolStripMenuItem = new ToolStripMenuItem();
		ToolStripMISelectAll = new ToolStripMenuItem();
		ToolStripMISelectNone = new ToolStripMenuItem();
		ToolStripMISelectReverse = new ToolStripMenuItem();
		ToolStripMISendSms = new ToolStripMenuItem();
		ToolStripMIDelete = new ToolStripMenuItem();
		ToolStripMenuItem_enablePerson = new ToolStripMenuItem();
		personBindingSource1 = new BindingSource(components);
		PIC = new PictureBox();
		LblRecordCount = new LabelX();
		labelX2 = new LabelX();
		BtnPrint = new ButtonX();
		DGridGroup = new DataGridViewX();
		DGGroupName = new DataGridViewTextBoxColumn();
		DgvGroupId = new DataGridViewTextBoxColumn();
		CMAGroup = new ContextMenuStrip(components);
		افزودنگروهجدیدToolStripMenuItem = new ToolStripMenuItem();
		ویرایشگروهToolStripMenuItem = new ToolStripMenuItem();
		حذفگروهToolStripMenuItem = new ToolStripMenuItem();
		persongroupBindingSource1 = new BindingSource(components);
		BtnDelete = new ButtonX();
		BtnEdit = new ButtonX();
		BtnAdd = new ButtonX();
		GroupPanel1 = new GroupPanel();
		ChkActivePersons = new CheckBox();
		LabelX1 = new LabelX();
		TxtSearch = new TextBoxX();
		btnSendToHost = new ButtonX();
		((Control)PanelEx1).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)contextMenuStrip1).SuspendLayout();
		((ISupportInitialize)personBindingSource1).BeginInit();
		((ISupportInitialize)PIC).BeginInit();
		((ISupportInitialize)DGridGroup).BeginInit();
		((Control)CMAGroup).SuspendLayout();
		((ISupportInitialize)persongroupBindingSource1).BeginInit();
		((Control)GroupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		PanelEx1.CanvasColor = SystemColors.Control;
		PanelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)PanelEx1).Controls.Add((Control)(object)btnImport);
		((Control)PanelEx1).Controls.Add((Control)(object)btnLog);
		((Control)PanelEx1).Controls.Add((Control)(object)DGrid);
		((Control)PanelEx1).Controls.Add((Control)(object)PIC);
		((Control)PanelEx1).Controls.Add((Control)(object)LblRecordCount);
		((Control)PanelEx1).Controls.Add((Control)(object)labelX2);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnPrint);
		((Control)PanelEx1).Controls.Add((Control)(object)DGridGroup);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnDelete);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnEdit);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnAdd);
		((Control)PanelEx1).Controls.Add((Control)(object)GroupPanel1);
		((Control)PanelEx1).Controls.Add((Control)(object)btnSendToHost);
		((Control)PanelEx1).Controls.Add((Control)(object)BtnSelect);
		PanelEx1.DisabledBackColor = Color.Empty;
		((Control)PanelEx1).Dock = (DockStyle)5;
		((Control)PanelEx1).Location = new Point(0, 0);
		((Control)PanelEx1).Name = "PanelEx1";
		((Control)PanelEx1).Size = new Size(926, 574);
		PanelEx1.Style.Alignment = (StringAlignment)1;
		PanelEx1.Style.BackColor1.Color = Color.LightSteelBlue;
		PanelEx1.Style.BackColor2.Color = Color.LightSteelBlue;
		PanelEx1.Style.Border = (eBorderType)1;
		PanelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		PanelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		PanelEx1.Style.GradientAngle = 90;
		((Control)PanelEx1).TabIndex = 2;
		((Control)BtnSelect).AccessibleRole = (AccessibleRole)43;
		BtnSelect.ColorTable = (eButtonColor)6;
		((Control)BtnSelect).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)BtnSelect).Location = new Point(5, 72);
		((Control)BtnSelect).Name = "BtnSelect";
		((Control)BtnSelect).Size = new Size(131, 266);
		((Control)BtnSelect).TabIndex = 17;
		((Control)BtnSelect).Text = "[انتخاب شخص]\r\n    \r\n    (Enter)";
		((Control)BtnSelect).Visible = false;
		((Control)BtnSelect).Click += BtnSelect_Click;
		((Control)btnImport).AccessibleRole = (AccessibleRole)43;
		btnImport.ColorTable = (eButtonColor)6;
		((Control)btnImport).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)btnImport).Location = new Point(5, 306);
		((Control)btnImport).Name = "btnImport";
		((Control)btnImport).Size = new Size(131, 32);
		((Control)btnImport).TabIndex = 20;
		((Control)btnImport).Text = "دریافت از اکسل";
		((Control)btnImport).Click += btnImport_Click;
		((Control)btnLog).AccessibleRole = (AccessibleRole)43;
		btnLog.ColorTable = (eButtonColor)6;
		((Control)btnLog).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		btnLog.ImagePosition = (eImagePosition)1;
		((Control)btnLog).Location = new Point(5, 267);
		((Control)btnLog).Name = "btnLog";
		((Control)btnLog).Size = new Size(131, 33);
		((PopupItemControl)btnLog).Style = (eDotNetBarStyle)9;
		((Control)btnLog).TabIndex = 19;
		((Control)btnLog).Text = "[سابقه اطلاعات روی دستگاه]";
		((Control)btnLog).Click += btnLog_Click;
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGrid).AlternatingRowsDefaultCellStyle = val;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[12]
		{
			(DataGridViewColumn)DGID,
			(DataGridViewColumn)dgServerStatus,
			(DataGridViewColumn)DgChecked,
			(DataGridViewColumn)DGPicPath,
			(DataGridViewColumn)DGCodeMoein,
			(DataGridViewColumn)DGCodeTafsil,
			(DataGridViewColumn)DGPersonName,
			(DataGridViewColumn)adressDataGridViewTextBoxColumn,
			(DataGridViewColumn)mob1DataGridViewTextBoxColumn,
			(DataGridViewColumn)tel1DataGridViewTextBoxColumn,
			(DataGridViewColumn)DGHardwareCode,
			(DataGridViewColumn)St
		});
		((Control)DGrid).ContextMenuStrip = contextMenuStrip1;
		((DataGridView)DGrid).DataSource = personBindingSource1;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val2;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(142, 72);
		((DataGridView)DGrid).MultiSelect = false;
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(648, 472);
		((Control)DGrid).TabIndex = 8;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellContentClick += new DataGridViewCellEventHandler(DGrid_CellContentClick);
		((DataGridView)DGrid).CellEnter += new DataGridViewCellEventHandler(DGrid_CellEnter);
		((DataGridView)DGrid).ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(DGrid_ColumnHeaderMouseClick);
		((Control)DGrid).DoubleClick += DGrid_DoubleClick;
		((Control)DGrid).KeyDown += new KeyEventHandler(DGrid_KeyDown);
		((DataGridViewColumn)DGID).DataPropertyName = "Id";
		((DataGridViewColumn)DGID).HeaderText = "[شناسه]";
		((DataGridViewColumn)DGID).Name = "DGID";
		((DataGridViewBand)DGID).ReadOnly = true;
		((DataGridViewBand)DGID).Visible = false;
		((DataGridViewColumn)dgServerStatus).DataPropertyName = "ServerStatusImage";
		((DataGridViewColumn)dgServerStatus).HeaderText = "";
		((DataGridViewColumn)dgServerStatus).Name = "dgServerStatus";
		((DataGridViewBand)dgServerStatus).ReadOnly = true;
		((DataGridViewColumn)dgServerStatus).Width = 30;
		((DataGridViewColumn)DgChecked).DataPropertyName = "Checked";
		((DataGridViewColumn)DgChecked).HeaderText = "";
		((DataGridViewColumn)DgChecked).Name = "DgChecked";
		((DataGridViewBand)DgChecked).ReadOnly = true;
		DgChecked.ThreeState = true;
		((DataGridViewColumn)DgChecked).Width = 25;
		((DataGridViewColumn)DGPicPath).DataPropertyName = "PicPath";
		((DataGridViewColumn)DGPicPath).HeaderText = "PicPath";
		((DataGridViewColumn)DGPicPath).Name = "DGPicPath";
		((DataGridViewBand)DGPicPath).ReadOnly = true;
		((DataGridViewBand)DGPicPath).Visible = false;
		((DataGridViewColumn)DGCodeMoein).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCodeMoein).DataPropertyName = "CodeMoein";
		((DataGridViewColumn)DGCodeMoein).HeaderText = "[کد معین]";
		((DataGridViewColumn)DGCodeMoein).MinimumWidth = 40;
		((DataGridViewColumn)DGCodeMoein).Name = "DGCodeMoein";
		((DataGridViewBand)DGCodeMoein).ReadOnly = true;
		((DataGridViewColumn)DGCodeMoein).Width = 77;
		((DataGridViewColumn)DGCodeTafsil).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCodeTafsil).DataPropertyName = "CodeTafsil";
		((DataGridViewColumn)DGCodeTafsil).HeaderText = "[کد تفصیل]";
		((DataGridViewColumn)DGCodeTafsil).MinimumWidth = 40;
		((DataGridViewColumn)DGCodeTafsil).Name = "DGCodeTafsil";
		((DataGridViewBand)DGCodeTafsil).ReadOnly = true;
		((DataGridViewColumn)DGCodeTafsil).Width = 84;
		((DataGridViewColumn)DGPersonName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGPersonName).DataPropertyName = "FullName";
		((DataGridViewColumn)DGPersonName).HeaderText = "[نام و نام خانوادگی]";
		((DataGridViewColumn)DGPersonName).MinimumWidth = 70;
		((DataGridViewColumn)DGPersonName).Name = "DGPersonName";
		((DataGridViewBand)DGPersonName).ReadOnly = true;
		((DataGridViewColumn)DGPersonName).Width = 125;
		((DataGridViewColumn)adressDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)adressDataGridViewTextBoxColumn).DataPropertyName = "Address";
		((DataGridViewColumn)adressDataGridViewTextBoxColumn).HeaderText = "[آدرس]";
		((DataGridViewColumn)adressDataGridViewTextBoxColumn).MinimumWidth = 50;
		((DataGridViewColumn)adressDataGridViewTextBoxColumn).Name = "adressDataGridViewTextBoxColumn";
		((DataGridViewBand)adressDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)mob1DataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)mob1DataGridViewTextBoxColumn).DataPropertyName = "Mob1";
		((DataGridViewColumn)mob1DataGridViewTextBoxColumn).HeaderText = "[موبایل]";
		((DataGridViewColumn)mob1DataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)mob1DataGridViewTextBoxColumn).Name = "mob1DataGridViewTextBoxColumn";
		((DataGridViewBand)mob1DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)tel1DataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)tel1DataGridViewTextBoxColumn).DataPropertyName = "Tel1";
		((DataGridViewColumn)tel1DataGridViewTextBoxColumn).HeaderText = "[تلفن]";
		((DataGridViewColumn)tel1DataGridViewTextBoxColumn).MinimumWidth = 100;
		((DataGridViewColumn)tel1DataGridViewTextBoxColumn).Name = "tel1DataGridViewTextBoxColumn";
		((DataGridViewBand)tel1DataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)DGHardwareCode).AutoSizeMode = (DataGridViewAutoSizeColumnMode)10;
		val3.NullValue = "...";
		((DataGridViewBand)DGHardwareCode).DefaultCellStyle = val3;
		((DataGridViewColumn)DGHardwareCode).HeaderText = "کد سخت افزاری";
		((DataGridViewColumn)DGHardwareCode).MinimumWidth = 30;
		((DataGridViewColumn)DGHardwareCode).Name = "DGHardwareCode";
		((DataGridViewBand)DGHardwareCode).ReadOnly = true;
		((DataGridViewColumn)DGHardwareCode).Width = 107;
		((DataGridViewColumn)St).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)St).DataPropertyName = "St";
		((DataGridViewColumn)St).HeaderText = "فعال";
		((DataGridViewColumn)St).MinimumWidth = 20;
		((DataGridViewColumn)St).Name = "St";
		((DataGridViewBand)St).ReadOnly = true;
		((DataGridViewColumn)St).Width = 37;
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[9]
		{
			(ToolStripItem)ToolStripMIAdd,
			(ToolStripItem)ToolStripMIEdit,
			(ToolStripItem)ToolStripMIPrint,
			(ToolStripItem)ToolStripMIDisplayGharardad,
			(ToolStripItem)ToolStripMIDisplayMorakhasi,
			(ToolStripItem)انتخابToolStripMenuItem,
			(ToolStripItem)ToolStripMISendSms,
			(ToolStripItem)ToolStripMIDelete,
			(ToolStripItem)ToolStripMenuItem_enablePerson
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)contextMenuStrip1).Size = new Size(179, 202);
		((ToolStripDropDown)contextMenuStrip1).Opening += contextMenuStrip1_Opening;
		((ToolStripItem)ToolStripMIAdd).Name = "ToolStripMIAdd";
		((ToolStripItem)ToolStripMIAdd).Size = new Size(178, 22);
		((ToolStripItem)ToolStripMIAdd).Text = "اضافه";
		((ToolStripItem)ToolStripMIAdd).Click += BtnAdd_Click;
		((ToolStripItem)ToolStripMIEdit).Name = "ToolStripMIEdit";
		((ToolStripItem)ToolStripMIEdit).Size = new Size(178, 22);
		((ToolStripItem)ToolStripMIEdit).Text = "ویرایش";
		((ToolStripItem)ToolStripMIEdit).Click += BtnEdit_Click;
		((ToolStripItem)ToolStripMIPrint).Name = "ToolStripMIPrint";
		((ToolStripItem)ToolStripMIPrint).Size = new Size(178, 22);
		((ToolStripItem)ToolStripMIPrint).Text = "چاپ";
		((ToolStripItem)ToolStripMIPrint).Click += BtnPrint_Click;
		((ToolStripItem)ToolStripMIDisplayGharardad).Name = "ToolStripMIDisplayGharardad";
		((ToolStripItem)ToolStripMIDisplayGharardad).Size = new Size(178, 22);
		((ToolStripItem)ToolStripMIDisplayGharardad).Text = "مشاهده قرارداد";
		((ToolStripItem)ToolStripMIDisplayGharardad).Click += ToolStripMIDisplayGharardad_Click;
		((ToolStripItem)ToolStripMIDisplayMorakhasi).Name = "ToolStripMIDisplayMorakhasi";
		((ToolStripItem)ToolStripMIDisplayMorakhasi).Size = new Size(178, 22);
		((ToolStripItem)ToolStripMIDisplayMorakhasi).Text = "مشاهده مرخصی";
		((ToolStripItem)ToolStripMIDisplayMorakhasi).Click += ToolStripMIDisplayMorakhasi_Click;
		((ToolStripDropDownItem)انتخابToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)ToolStripMISelectAll,
			(ToolStripItem)ToolStripMISelectNone,
			(ToolStripItem)ToolStripMISelectReverse
		});
		((ToolStripItem)انتخابToolStripMenuItem).Name = "انتخابToolStripMenuItem";
		((ToolStripItem)انتخابToolStripMenuItem).Size = new Size(178, 22);
		((ToolStripItem)انتخابToolStripMenuItem).Text = "انتخاب";
		((ToolStripItem)ToolStripMISelectAll).Name = "ToolStripMISelectAll";
		((ToolStripItem)ToolStripMISelectAll).Size = new Size(176, 22);
		((ToolStripItem)ToolStripMISelectAll).Text = "انتخاب همه";
		((ToolStripItem)ToolStripMISelectAll).Click += ToolStripMISelectAll_Click;
		((ToolStripItem)ToolStripMISelectNone).Name = "ToolStripMISelectNone";
		((ToolStripItem)ToolStripMISelectNone).Size = new Size(176, 22);
		((ToolStripItem)ToolStripMISelectNone).Text = "انتخاب هیچکدام";
		((ToolStripItem)ToolStripMISelectNone).Click += ToolStripMISelectNone_Click;
		((ToolStripItem)ToolStripMISelectReverse).Name = "ToolStripMISelectReverse";
		((ToolStripItem)ToolStripMISelectReverse).Size = new Size(176, 22);
		((ToolStripItem)ToolStripMISelectReverse).Text = "معکوس کردن انتخاب";
		((ToolStripItem)ToolStripMISelectReverse).Click += ToolStripMISelectReverse_Click;
		((ToolStripItem)ToolStripMISendSms).Name = "ToolStripMISendSms";
		((ToolStripItem)ToolStripMISendSms).Size = new Size(178, 22);
		((ToolStripItem)ToolStripMISendSms).Text = "ارسال پیامک";
		((ToolStripItem)ToolStripMISendSms).Click += ToolStripMISendSms_Click;
		((ToolStripItem)ToolStripMIDelete).Name = "ToolStripMIDelete";
		((ToolStripItem)ToolStripMIDelete).Size = new Size(178, 22);
		((ToolStripItem)ToolStripMIDelete).Text = "شخص غیر فعال شود";
		((ToolStripItem)ToolStripMIDelete).Click += BtnDelete_Click;
		((ToolStripItem)ToolStripMenuItem_enablePerson).Name = "ToolStripMenuItem_enablePerson";
		((ToolStripItem)ToolStripMenuItem_enablePerson).Size = new Size(178, 22);
		((ToolStripItem)ToolStripMenuItem_enablePerson).Text = "شخص فعال شود";
		((ToolStripItem)ToolStripMenuItem_enablePerson).Click += ToolStripMenuItem_EnablePerson_Click;
		personBindingSource1.DataSource = typeof(Person);
		personBindingSource1.CurrentChanged += personBindingSource1_CurrentChanged;
		PIC.BorderStyle = (BorderStyle)1;
		PIC.Image = (Image)(object)Resources.people;
		((Control)PIC).Location = new Point(5, 344);
		((Control)PIC).Name = "PIC";
		((Control)PIC).Size = new Size(131, 131);
		PIC.SizeMode = (PictureBoxSizeMode)1;
		PIC.TabIndex = 3;
		PIC.TabStop = false;
		((Control)LblRecordCount).Anchor = (AnchorStyles)10;
		((BaseItemControl)LblRecordCount).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblRecordCount).ForeColor = Color.DarkRed;
		((Control)LblRecordCount).Location = new Point(460, 548);
		((Control)LblRecordCount).Name = "LblRecordCount";
		((Control)LblRecordCount).Size = new Size(360, 23);
		((Control)LblRecordCount).TabIndex = 16;
		((Control)LblRecordCount).Text = "labelX3";
		((Control)labelX2).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(826, 548);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(95, 23);
		((Control)labelX2).TabIndex = 15;
		((Control)labelX2).Text = "[تعداد پرسنل]:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)BtnPrint).AccessibleRole = (AccessibleRole)43;
		BtnPrint.ColorTable = (eButtonColor)6;
		((Control)BtnPrint).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnPrint.Image = (Image)(object)Resources.printer_green;
		BtnPrint.ImagePosition = (eImagePosition)1;
		((Control)BtnPrint).Location = new Point(5, 189);
		((Control)BtnPrint).Name = "BtnPrint";
		((Control)BtnPrint).Size = new Size(131, 33);
		((PopupItemControl)BtnPrint).Style = (eDotNetBarStyle)9;
		((Control)BtnPrint).TabIndex = 14;
		((Control)BtnPrint).Text = "[چاپ](Ctrl+P)";
		((Control)BtnPrint).Click += BtnPrint_Click;
		((DataGridView)DGridGroup).AllowUserToAddRows = false;
		((DataGridView)DGridGroup).AllowUserToDeleteRows = false;
		((DataGridView)DGridGroup).AllowUserToResizeColumns = false;
		val4.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGridGroup).AlternatingRowsDefaultCellStyle = val4;
		((Control)DGridGroup).Anchor = (AnchorStyles)11;
		((DataGridView)DGridGroup).AutoGenerateColumns = false;
		((DataGridView)DGridGroup).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridGroup).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[2]
		{
			(DataGridViewColumn)DGGroupName,
			(DataGridViewColumn)DgvGroupId
		});
		((Control)DGridGroup).ContextMenuStrip = CMAGroup;
		((DataGridView)DGridGroup).DataSource = persongroupBindingSource1;
		val5.Alignment = (DataGridViewContentAlignment)16;
		val5.BackColor = SystemColors.Window;
		val5.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val5.ForeColor = SystemColors.ControlText;
		val5.SelectionBackColor = SystemColors.Highlight;
		val5.SelectionForeColor = SystemColors.ControlText;
		val5.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridGroup).DefaultCellStyle = val5;
		((DataGridView)DGridGroup).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridGroup).Location = new Point(796, 72);
		((DataGridView)DGridGroup).MultiSelect = false;
		((Control)DGridGroup).Name = "DGridGroup";
		((DataGridView)DGridGroup).ReadOnly = true;
		((DataGridView)DGridGroup).RowHeadersVisible = false;
		((DataGridView)DGridGroup).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridGroup).Size = new Size(125, 472);
		((Control)DGridGroup).TabIndex = 13;
		((DataGridView)DGridGroup).CellEnter += new DataGridViewCellEventHandler(DGridGroup_CellEnter);
		((Control)DGridGroup).Enter += DGridGroup_Enter;
		((Control)DGridGroup).KeyDown += new KeyEventHandler(DGridGroup_KeyDown);
		((Control)DGridGroup).KeyPress += new KeyPressEventHandler(DGridGroup_KeyPress);
		((Control)DGridGroup).Leave += DGridGroup_Leave;
		((DataGridViewColumn)DGGroupName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGGroupName).DataPropertyName = "Name";
		((DataGridViewColumn)DGGroupName).HeaderText = "[گروه]";
		((DataGridViewColumn)DGGroupName).MinimumWidth = 60;
		((DataGridViewColumn)DGGroupName).Name = "DGGroupName";
		((DataGridViewBand)DGGroupName).ReadOnly = true;
		((DataGridViewColumn)DgvGroupId).DataPropertyName = "Id";
		((DataGridViewColumn)DgvGroupId).HeaderText = "DgvGroupId";
		((DataGridViewColumn)DgvGroupId).Name = "DgvGroupId";
		((DataGridViewBand)DgvGroupId).ReadOnly = true;
		((DataGridViewBand)DgvGroupId).Visible = false;
		((ToolStrip)CMAGroup).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)افزودنگروهجدیدToolStripMenuItem,
			(ToolStripItem)ویرایشگروهToolStripMenuItem,
			(ToolStripItem)حذفگروهToolStripMenuItem
		});
		((Control)CMAGroup).Name = "CMAGroup";
		((Control)CMAGroup).RightToLeft = (RightToLeft)1;
		((Control)CMAGroup).Size = new Size(159, 70);
		((ToolStripItem)افزودنگروهجدیدToolStripMenuItem).Name = "افزودنگروهجدیدToolStripMenuItem";
		((ToolStripItem)افزودنگروهجدیدToolStripMenuItem).Size = new Size(158, 22);
		((ToolStripItem)افزودنگروهجدیدToolStripMenuItem).Text = "افزودن گروه جدید";
		((ToolStripItem)افزودنگروهجدیدToolStripMenuItem).Click += افزودنگروهجدیدToolStripMenuItem_Click;
		((ToolStripItem)ویرایشگروهToolStripMenuItem).Name = "ویرایشگروهToolStripMenuItem";
		((ToolStripItem)ویرایشگروهToolStripMenuItem).Size = new Size(158, 22);
		((ToolStripItem)ویرایشگروهToolStripMenuItem).Text = "ویرایش گروه";
		((ToolStripItem)ویرایشگروهToolStripMenuItem).Click += ویرایشگروهToolStripMenuItem_Click;
		((ToolStripItem)حذفگروهToolStripMenuItem).Name = "حذفگروهToolStripMenuItem";
		((ToolStripItem)حذفگروهToolStripMenuItem).Size = new Size(158, 22);
		((ToolStripItem)حذفگروهToolStripMenuItem).Text = "حذف گروه";
		((ToolStripItem)حذفگروهToolStripMenuItem).Click += حذفگروهToolStripMenuItem_Click;
		persongroupBindingSource1.DataSource = typeof(PersonGroup);
		((Control)BtnDelete).AccessibleRole = (AccessibleRole)43;
		BtnDelete.ColorTable = (eButtonColor)6;
		((Control)BtnDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnDelete.Image = (Image)(object)Resources.edit_delete;
		BtnDelete.ImagePosition = (eImagePosition)1;
		((Control)BtnDelete).Location = new Point(5, 150);
		((Control)BtnDelete).Name = "BtnDelete";
		((Control)BtnDelete).Size = new Size(131, 33);
		((PopupItemControl)BtnDelete).Style = (eDotNetBarStyle)9;
		((Control)BtnDelete).TabIndex = 11;
		((Control)BtnDelete).Text = "[حذف] (Delete)    ";
		((Control)BtnDelete).Click += BtnDelete_Click;
		((Control)BtnEdit).AccessibleRole = (AccessibleRole)43;
		BtnEdit.ColorTable = (eButtonColor)6;
		((Control)BtnEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnEdit.Image = (Image)(object)Resources.edit2;
		BtnEdit.ImagePosition = (eImagePosition)1;
		((Control)BtnEdit).Location = new Point(5, 111);
		((Control)BtnEdit).Name = "BtnEdit";
		((Control)BtnEdit).RightToLeft = (RightToLeft)1;
		((Control)BtnEdit).Size = new Size(131, 33);
		((PopupItemControl)BtnEdit).Style = (eDotNetBarStyle)9;
		((Control)BtnEdit).TabIndex = 10;
		((Control)BtnEdit).Text = "    [ویرایش](F7)";
		((Control)BtnEdit).Click += BtnEdit_Click;
		((Control)BtnAdd).AccessibleRole = (AccessibleRole)43;
		BtnAdd.ColorTable = (eButtonColor)6;
		((Control)BtnAdd).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		BtnAdd.Image = (Image)(object)Resources.edit_add1;
		BtnAdd.ImagePosition = (eImagePosition)1;
		((Control)BtnAdd).Location = new Point(5, 72);
		((Control)BtnAdd).Name = "BtnAdd";
		((Control)BtnAdd).Size = new Size(131, 33);
		((PopupItemControl)BtnAdd).Style = (eDotNetBarStyle)9;
		((Control)BtnAdd).TabIndex = 9;
		((Control)BtnAdd).Text = "[اضافه](Insert)";
		((Control)BtnAdd).Click += BtnAdd_Click;
		((Control)GroupPanel1).Anchor = (AnchorStyles)13;
		((PanelControl)GroupPanel1).CanvasColor = SystemColors.Control;
		((PanelControl)GroupPanel1).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)GroupPanel1).Controls.Add((Control)(object)ChkActivePersons);
		((Control)GroupPanel1).Controls.Add((Control)(object)LabelX1);
		((Control)GroupPanel1).Controls.Add((Control)(object)TxtSearch);
		GroupPanel1.DisabledBackColor = Color.Empty;
		((Control)GroupPanel1).Location = new Point(3, 6);
		((Control)GroupPanel1).Name = "GroupPanel1";
		((Control)GroupPanel1).Size = new Size(918, 60);
		((PanelControl)GroupPanel1).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)GroupPanel1).Style.BackColorGradientAngle = 90;
		((PanelControl)GroupPanel1).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)GroupPanel1).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderBottomWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)GroupPanel1).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderLeftWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderRightWidth = 1;
		((PanelControl)GroupPanel1).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)GroupPanel1).Style.BorderTopWidth = 1;
		((PanelControl)GroupPanel1).Style.CornerDiameter = 4;
		((PanelControl)GroupPanel1).Style.CornerType = (eCornerType)2;
		((PanelControl)GroupPanel1).Style.TextAlignment = (eStyleTextAlignment)2;
		((PanelControl)GroupPanel1).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)GroupPanel1).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)GroupPanel1).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)GroupPanel1).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)GroupPanel1).TabIndex = 0;
		((Control)GroupPanel1).Text = "[جستجو]";
		GroupPanel1.TitleImage = (Image)(object)Resources.search2;
		((Control)GroupPanel1).Click += GroupPanel1_Click;
		((Control)ChkActivePersons).AutoSize = true;
		ChkActivePersons.Checked = true;
		ChkActivePersons.CheckState = (CheckState)1;
		((Control)ChkActivePersons).Location = new Point(2, 8);
		((Control)ChkActivePersons).Name = "ChkActivePersons";
		((Control)ChkActivePersons).Size = new Size(121, 17);
		((Control)ChkActivePersons).TabIndex = 9;
		((Control)ChkActivePersons).Text = "فقط اشخاص فعال";
		((ButtonBase)ChkActivePersons).UseVisualStyleBackColor = true;
		ChkActivePersons.CheckedChanged += ChkActivePersons_CheckedChanged;
		((Control)LabelX1).Anchor = (AnchorStyles)9;
		((BaseItemControl)LabelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX1).Location = new Point(826, 4);
		((Control)LabelX1).Name = "LabelX1";
		((Control)LabelX1).Size = new Size(57, 23);
		((Control)LabelX1).TabIndex = 8;
		((Control)LabelX1).Text = "[جستجو]  :";
		((Control)TxtSearch).Anchor = (AnchorStyles)13;
		TxtSearch.Border.Class = "TextBoxBorder";
		TxtSearch.Border.CornerType = (eCornerType)1;
		((Control)TxtSearch).Location = new Point(136, 5);
		((Control)TxtSearch).Name = "TxtSearch";
		((Control)TxtSearch).Size = new Size(684, 21);
		((Control)TxtSearch).TabIndex = 1;
		((Control)TxtSearch).TextChanged += TxtSearch_TextChanged;
		((Control)TxtSearch).Enter += TxtSearch_Enter;
		((Control)TxtSearch).KeyDown += new KeyEventHandler(TxtSearch_KeyDown);
		((Control)TxtSearch).Leave += TxtSearch_Leave;
		((Control)btnSendToHost).AccessibleRole = (AccessibleRole)43;
		btnSendToHost.ColorTable = (eButtonColor)6;
		((Control)btnSendToHost).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnSendToHost.ImagePosition = (eImagePosition)1;
		((Control)btnSendToHost).Location = new Point(5, 228);
		((Control)btnSendToHost).Name = "btnSendToHost";
		((Control)btnSendToHost).Size = new Size(131, 33);
		((PopupItemControl)btnSendToHost).Style = (eDotNetBarStyle)9;
		((Control)btnSendToHost).TabIndex = 18;
		((Control)btnSendToHost).Text = "ارسال فوری به هاست";
		((Control)btnSendToHost).Click += btnSendToHost_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)PanelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)this).Name = "UcPersonel";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(926, 574);
		((Control)PanelEx1).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)contextMenuStrip1).ResumeLayout(false);
		((ISupportInitialize)personBindingSource1).EndInit();
		((ISupportInitialize)PIC).EndInit();
		((ISupportInitialize)DGridGroup).EndInit();
		((Control)CMAGroup).ResumeLayout(false);
		((ISupportInitialize)persongroupBindingSource1).EndInit();
		((Control)GroupPanel1).ResumeLayout(false);
		((Control)GroupPanel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
