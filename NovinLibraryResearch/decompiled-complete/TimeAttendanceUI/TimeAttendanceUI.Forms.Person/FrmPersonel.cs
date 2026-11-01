using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BehComponents;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using EntityCache.Business.Attendance;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using Serilog;
using TimeAttendance.ValidatorAttribute.FLRPerson;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Departments;
using TimeAttendanceUI.Forms.HardwareCode;
using TimeAttendanceUI.Forms.Person.Eisar;
using TimeAttendanceUI.Forms.Person.Group;
using TimeAttendanceUI.Forms.Person.Title;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Person;

public class FrmPersonel : BaseForm
{
	private List<HardwareCodeCUSView> _lstHardwareCodeCus;

	private string FileToBeDelete;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TabControl tabControl1;

	private TabControlPanel tabControlPanel1;

	private TabItem tabItem1;

	private GroupPanel groupPanel7;

	internal LabelX labelX7;

	internal ButtonX BTNMoreT;

	internal ButtonX BTNMoreG;

	internal ComboBoxEx CMBTitle;

	internal ComboBoxEx CMBGroup;

	internal LabelX LabelX12;

	internal LabelX LabelX11;

	internal DataGridViewX DGridHardwareCodes;

	private TextBoxX TxtAdd;

	private TextBoxX TxtTel;

	private LabelX labelX6;

	private LabelX labelX4;

	private TextBoxX TxtMob;

	private LabelX labelX3;

	private TextBoxX TxtFamily;

	private TextBoxX TxtName;

	private LabelX labelX1;

	private LabelX labelX2;

	private LabelX labelX5;

	private TabItem tabItem2;

	private TabControlPanel tabControlPanel2;

	private GroupPanel groupPanel3;

	internal LabelX labelX8;

	internal ButtonX buttonX1;

	private TextBoxX textBoxX1;

	private LabelX labelX18;

	private LabelX labelX17;

	private LabelX labelX16;

	private LabelX labelX15;

	private LabelX labelX14;

	private TextBoxX TxtNumberShenasname;

	private TextBoxX TxtCodeMeli;

	private TextBoxX TxtNameFather;

	private ComboBoxEx CmbSexual;

	private ComboItem CmbFemale;

	private ComboItem CmbMale;

	private ComboBoxEx CmbMariage;

	private ComboItem CmbNaMalom;

	private ComboItem CmbSingel;

	private ComboItem CmbMarried1;

	internal LabelX labelX10;

	internal LabelX labelX9;

	private LabelX labelX13;

	private TextBoxX TxtBimeNum;

	private NumericUpDown numerichild;

	private TabControlPanel tabControlPanel3;

	private TabItem tabItem3;

	private PanelEx panelEx2;

	private TextBoxX TxtNameF;

	private TextBoxX TxtCodeM;

	private ButtonX BtnDelDevCode;

	private ButtonX BtnSelectDeviceCode;

	private OpenFileDialog FDFace;

	private CheckBoxX ChbBirth;

	private BindingSource shiftsBindingSource;

	private BindingSource personTitleBindingSource;

	private BindingSource personGroupBindingSource;

	private TextBoxX TxtMSodor;

	private LabelX labelX28;

	private LabelX labelX26;

	private LabelX labelX24;

	private PictureBox PicPerson;

	private ButtonX BtnDelPic;

	private ButtonX BtnAddPic;

	internal LabelX labelX29;

	private LabelX labelX22;

	private TextBoxX TxtSHSH;

	private LabelX labelX20;

	private TextBoxX TxtZip;

	private LabelX labelX31;

	private TextBoxX TxtSerialSH;

	private LabelX labelX30;

	private BindingSource bankBindingSource;

	private BindingSource bankHesabTypeBindingSource;

	private BindingSource perPartBindingSource;

	private BindingSource perKhedmatPlaceBindingSource;

	private BindingSource perTypeEstekhdamBindingSource;

	private BindingSource perEisarBindingSource;

	private BindingSource perKhedmatBindingSource;

	private ComboBoxEx CMBJensiat;

	private BindingSource companyBindingSource;

	private BindingSource perWorkTitleBindingSource;

	private TextBoxX txtMadrak;

	private LabelX labelX42;

	private BindingSource maliYearBindingSource;

	private CheckBoxX ChbTark;

	private CheckBoxX ChbSWork;

	internal ButtonX BtnDelEisar;

	internal ButtonX BtnMoreEisar;

	private ComboBoxEx CmbEisar;

	private LabelX labelX35;

	private TextBoxX txtNoBimeh;

	private LabelX labelX19;

	private ComboItem CmbUnknown;

	private ComboItem CmbMale1;

	private ComboItem CmbFamle;

	private TextBoxX TXTDes;

	private DateBoxFa FarsiDateBirthDay;

	private DateBoxFa DateTark;

	private DateBoxFa farsiDateSWork;

	private TextBoxX numCodePersonel;

	private TextBoxX NumCodeTafsil;

	internal LabelX labelX21;

	internal LabelX labelX23;

	private TextBoxX TxtPassword;

	private LabelX labelX25;

	private TextBoxX TxtUserName;

	private LabelX labelX27;

	private LabelX lblUrlQueryString;

	internal ButtonX btnSetPassword;

	internal ButtonX btnMoreDep;

	internal ComboBoxEx cmbDepartman;

	internal LabelX labelX32;

	private BindingSource DepartmanbindingSource;

	private ButtonX BtnCreateAndSelectNewCode;

	private BindingSource hardwareCodeCUSViewBindingSource;

	private DataGridViewTextBoxColumn DG_DeviceID;

	private DataGridViewTextBoxColumn DG_DeviceName;

	private DataGridViewTextBoxColumn DG_PersonNO;

	private DataGridViewTextBoxColumn DG_PersonId;

	private DataGridViewTextBoxColumn DG_FullName;

	private TextBoxX TxtLatinName;

	private LabelX labelX33;

	private LabelX labelX34;

	public TimeAttendanceDAL.Dal.FLRPerson.Person ClsPerson { get; private set; }

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

	private List<short> LstIdDevice
	{
		get
		{
			List<short> list = new List<short>();
			try
			{
				for (int i = 0; i < ((DataGridView)DGridHardwareCodes).RowCount; i++)
				{
					list.Add(short.Parse(((DataGridView)DGridHardwareCodes)[((DataGridViewBand)DG_DeviceID).Index, i].Value.ToString()));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LstIdDevice", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 422);
			}
			return list;
		}
	}

	public Font FontFrm { get; private set; }

	public AttDepartmentBusiness CurrentDepartment { get; private set; }

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

	private void SetTrDictionary()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[افزودن شخص جدید]", out var value))
			{
				trDictionary.Add("[افزودن شخص جدید]", Utilities.GetTranslate("[افزودن شخص جدید]"));
			}
			if (!trDictionary.TryGetValue("[ویرایش یک شخص]", out value))
			{
				trDictionary.Add("[ویرایش یک شخص]", Utilities.GetTranslate("[ویرایش یک شخص]"));
			}
			if (!trDictionary.TryGetValue("[مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) پرسنل]", out value))
			{
				trDictionary.Add("[مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) پرسنل]", Utilities.GetTranslate("[مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) پرسنل]"));
			}
			if (!trDictionary.TryGetValue("[مرد]", out value))
			{
				trDictionary.Add("[مرد]", Utilities.GetTranslate("[مرد]"));
			}
			if (!trDictionary.TryGetValue("[زن]", out value))
			{
				trDictionary.Add("[زن]", Utilities.GetTranslate("[زن]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionary", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 76);
		}
	}

	public FrmPersonel(string contextGuid, TimeAttendanceDAL.Dal.FLRPerson.Person person, Font font, bool isReadOnly)
		: base(contextGuid, (BaseFormConfig)null)
	{
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ClsPerson = person;
			InitializeComponent();
			if (isReadOnly)
			{
				((Control)this).Text = "مشاهده لاگ عملکرد کاربران ( قفط خواندنی ) پرسنل";
				if (trDictionary.TryGetValue("[مشاهده لاگ عملکرد کاربران ( فقط خواندنی ) پرسنل]", out var value))
				{
					((Control)this).Text = value;
				}
				((Control)BtnAccept).Enabled = false;
			}
			else if (((PersonBase)person).Id > 0)
			{
				((Control)this).Text = "ویرایش یک شخص";
				if (trDictionary.TryGetValue("[ویرایش یک شخص]", out var value2))
				{
					((Control)this).Text = value2;
				}
			}
			else
			{
				((Control)this).Text = "تعریف یک شخص";
				if (trDictionary.TryGetValue("[افزودن شخص جدید]", out var value3))
				{
					((Control)this).Text = value3;
				}
			}
			FontFrm = font;
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 114);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		_ = 4;
		try
		{
			SetTrDictionary();
			await LoadDataGroupAsync(token);
			await LoaddataTitleAsync(token);
			await LoadDataEisarAsync(token);
			await LoadDataDepartmentAsync(token);
			LoadDataJensiat();
			await FillFormAsync();
			((Control)numCodePersonel).Focus();
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private async Task FillFormAsync()
	{
		_ = 1;
		try
		{
			((Control)numCodePersonel).Text = ((PersonBase)ClsPerson).CodeMoein.ToString();
			((Control)NumCodeTafsil).Text = ((PersonBase)ClsPerson).CodeTafsil.ToString();
			((Control)farsiDateSWork).Text = Calendar.MiladiToCalture(DateTime.Now);
			ChbSWork.Checked = true;
			((Control)DateTark).Text = "1300/01/01";
			((Control)FarsiDateBirthDay).Text = "1300/01/01";
			if (((PersonBase)ClsPerson).Id <= 0)
			{
				return;
			}
			((FileDialog)FDFace).FileName = "";
			((ListControl)CMBGroup).SelectedValue = ((PersonBase)ClsPerson).GroupId;
			((ListControl)CMBTitle).SelectedValue = ((PersonBase)ClsPerson).TitleId;
			((Control)TxtName).Text = ((PersonBase)ClsPerson).Name;
			((Control)TxtLatinName).Text = ((PersonBase)ClsPerson).LatinFullName;
			((Control)TxtFamily).Text = ((PersonBase)ClsPerson).Family;
			((Control)TxtMob).Text = ((PersonBase)ClsPerson).Mob1;
			((Control)TxtTel).Text = ((PersonBase)ClsPerson).Tel1;
			((Control)TxtAdd).Text = ((PersonBase)ClsPerson).Address;
			((Control)TXTDes).Text = ((PersonBase)ClsPerson).Description;
			((Control)TxtCodeM).Text = ((PersonBase)ClsPerson).CodeMeli;
			((Control)TxtZip).Text = ((PersonBase)ClsPerson).ZipCode;
			((Control)TxtSHSH).Text = ((PersonBase)ClsPerson).Shsh;
			((Control)TxtMSodor).Text = ((PersonBase)ClsPerson).SodorSh;
			((Control)TxtNameF).Text = ((PersonBase)ClsPerson).FatherName;
			((Control)TxtSerialSH).Text = ((PersonBase)ClsPerson).SerialSh;
			((ListControl)CMBJensiat).SelectedIndex = ((PersonBase)ClsPerson).Jensiat;
			((Control)txtNoBimeh).Text = ((PersonBase)ClsPerson).NoBimeh;
			((Control)txtMadrak).Text = ((PersonBase)ClsPerson).MadrakTahsili;
			ChbBirth.Checked = ((PersonBase)ClsPerson).DateMBirthday != ConstantValues.MinDateM;
			((Control)FarsiDateBirthDay).Text = Calendar.MiladiToCalture(((PersonBase)ClsPerson).DateMBirthday);
			((Control)farsiDateSWork).Text = Calendar.MiladiToCalture(((PersonBase)ClsPerson).DateMStartWork);
			ChbSWork.Checked = ((PersonBase)ClsPerson).DateMStartWork != ConstantValues.MinDateM;
			((Control)DateTark).Text = Calendar.MiladiToCalture(((PersonBase)ClsPerson).DateMFinishW);
			ChbTark.Checked = ((PersonBase)ClsPerson).DateMFinishW != ConstantValues.MinDateM;
			((Control)FarsiDateBirthDay).Enabled = ChbBirth.Checked;
			((Control)farsiDateSWork).Enabled = ChbSWork.Checked;
			((Control)DateTark).Enabled = ChbTark.Checked;
			((ListControl)CmbEisar).SelectedValue = ((PersonBase)ClsPerson).EisarId;
			if (((PersonBase)ClsPerson).PicPath != null)
			{
				try
				{
					LoadPicture(((PersonBase)ClsPerson).PicPath);
				}
				catch (Exception ex)
				{
					Log.Error(ex, "");
					PicPerson.Image = (Image)(object)Resources.people;
				}
			}
			if (string.IsNullOrEmpty(ClsPerson.UserName))
			{
				ClsPerson.UserName = ((PersonBase)ClsPerson).CodeMeli;
			}
			((Control)TxtUserName).Text = ClsPerson.UserName;
			if (ClsPerson.HashPassword != null && ClsPerson.HashPassword.Length > 1)
			{
				((Control)TxtPassword).Text = Convert.ToBase64String(ClsPerson.HashPassword);
			}
			LabelX val = lblUrlQueryString;
			((Control)val).Text = (await ClsPerson.GenerateUrlQueryStringAsync(((CacheBaseForm)this).ContextGuid)) ?? "";
			await FillGridHardwareCodesAsync(default(CancellationToken));
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson = ClsPerson;
			if ((((clsPerson != null) ? ((PersonBase)clsPerson).GuidDepartment : null) ?? Guid.Empty) != Guid.Empty)
			{
				((ListControl)cmbDepartman).SelectedValue = ((PersonBase)ClsPerson).GuidDepartment.Value;
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 201);
		}
	}

	private async Task FillGridHardwareCodesAsync(CancellationToken token)
	{
		try
		{
			ClsPerson.ResetHardWareCodes();
			_lstHardwareCodeCus = await ClsPerson.GetHardWareCodesAsync();
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
						((DataGridView)DGridHardwareCodes).DataSource = _lstHardwareCodeCus;
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
			Log.Error(ex, "");
		}
	}

	private void GetData()
	{
		try
		{
			((PersonBase)ClsPerson).CodeMoein = (long.TryParse(((Control)numCodePersonel).Text.Trim(), out var result) ? result : 0);
			((PersonBase)ClsPerson).CodeTafsil = (long.TryParse(((Control)NumCodeTafsil).Text.Trim(), out var result2) ? result2 : 0);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson = ClsPerson;
			ComboBoxEx cMBGroup = CMBGroup;
			((PersonBase)clsPerson).GroupId = short.Parse(((cMBGroup == null) ? null : ((ListControl)cMBGroup).SelectedValue?.ToString()) ?? "0");
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson2 = ClsPerson;
			ComboBoxEx cMBTitle = CMBTitle;
			((PersonBase)clsPerson2).TitleId = short.Parse(((cMBTitle == null) ? null : ((ListControl)cMBTitle).SelectedValue?.ToString()) ?? "0");
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson3 = ClsPerson;
			TextBoxX txtName = TxtName;
			((PersonBase)clsPerson3).Name = Text.FixString((txtName == null) ? null : ((Control)txtName).Text?.Trim());
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson4 = ClsPerson;
			TextBoxX txtLatinName = TxtLatinName;
			((PersonBase)clsPerson4).LatinFullName = Text.FixString((txtLatinName == null) ? null : ((Control)txtLatinName).Text?.Trim());
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson5 = ClsPerson;
			TextBoxX txtFamily = TxtFamily;
			((PersonBase)clsPerson5).Family = ((txtFamily == null) ? null : ((Control)txtFamily).Text?.Trim());
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson6 = ClsPerson;
			ComboBoxEx cMBJensiat = CMBJensiat;
			((PersonBase)clsPerson6).Jensiat = short.Parse(((cMBJensiat != null) ? ((ListControl)cMBJensiat).SelectedIndex.ToString() : null) ?? "0");
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson7 = ClsPerson;
			TextBoxX txtMob = TxtMob;
			((PersonBase)clsPerson7).Mob1 = ((txtMob != null) ? ((Control)txtMob).Text.Trim() : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson8 = ClsPerson;
			TextBoxX txtTel = TxtTel;
			((PersonBase)clsPerson8).Tel1 = ((txtTel != null) ? ((Control)txtTel).Text.Trim() : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson9 = ClsPerson;
			TextBoxX txtAdd = TxtAdd;
			((PersonBase)clsPerson9).Address = Text.FixString((txtAdd != null) ? ((Control)txtAdd).Text : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson10 = ClsPerson;
			TextBoxX tXTDes = TXTDes;
			((PersonBase)clsPerson10).Description = Text.FixString((tXTDes != null) ? ((Control)tXTDes).Text : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson11 = ClsPerson;
			TextBoxX txtCodeM = TxtCodeM;
			((PersonBase)clsPerson11).CodeMeli = Text.FixString((txtCodeM != null) ? ((Control)txtCodeM).Text.Trim() : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson12 = ClsPerson;
			TextBoxX txtZip = TxtZip;
			((PersonBase)clsPerson12).ZipCode = ((txtZip != null) ? ((Control)txtZip).Text : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson13 = ClsPerson;
			TextBoxX txtSHSH = TxtSHSH;
			((PersonBase)clsPerson13).Shsh = ((txtSHSH != null) ? ((Control)txtSHSH).Text : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson14 = ClsPerson;
			TextBoxX txtMSodor = TxtMSodor;
			((PersonBase)clsPerson14).SodorSh = ((txtMSodor != null) ? ((Control)txtMSodor).Text : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson15 = ClsPerson;
			TextBoxX txtNameF = TxtNameF;
			((PersonBase)clsPerson15).FatherName = Text.FixString((txtNameF == null) ? null : ((Control)txtNameF).Text?.Trim());
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson16 = ClsPerson;
			TextBoxX txtSerialSH = TxtSerialSH;
			((PersonBase)clsPerson16).SerialSh = ((txtSerialSH != null) ? ((Control)txtSerialSH).Text : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson17 = ClsPerson;
			TextBoxX obj = txtNoBimeh;
			((PersonBase)clsPerson17).NoBimeh = Text.FixString((obj != null) ? ((Control)obj).Text : null);
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson18 = ClsPerson;
			TextBoxX obj2 = txtMadrak;
			((PersonBase)clsPerson18).MadrakTahsili = Text.FixString((obj2 != null) ? ((Control)obj2).Text.Trim() : null);
			((PersonBase)ClsPerson).GuidDepartment = CurrentDepartment.Guid;
			((PersonBase)ClsPerson).DateMBirthday = Utilities.CalendarToMiladi(((Control)FarsiDateBirthDay).Text);
			((PersonBase)ClsPerson).DateMStartWork = Utilities.CalendarToMiladi(((Control)farsiDateSWork).Text);
			((PersonBase)ClsPerson).DateMFinishW = Utilities.CalendarToMiladi(((Control)DateTark).Text);
			((PersonBase)ClsPerson).HType = 1;
			if (((ListControl)CmbEisar).SelectedValue != null)
			{
				ComboBoxEx cmbEisar = CmbEisar;
				if (Convert.ToInt16((cmbEisar == null) ? null : ((ListControl)cmbEisar).SelectedValue?.ToString()) > 0)
				{
					TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson19 = ClsPerson;
					ComboBoxEx cmbEisar2 = CmbEisar;
					((PersonBase)clsPerson19).EisarId = Convert.ToInt16((cmbEisar2 == null) ? null : ((ListControl)cmbEisar2).SelectedValue?.ToString());
				}
			}
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson20 = ClsPerson;
			TextBoxX txtUserName = TxtUserName;
			clsPerson20.UserName = ((txtUserName != null) ? ((Control)txtUserName).Text.Trim() : null);
			((PersonBase)ClsPerson).GuidDepartment = ((((ListControl)cmbDepartman).SelectedValue != null && (Guid)((ListControl)cmbDepartman).SelectedValue != Guid.Empty) ? new Guid?((Guid)((ListControl)cmbDepartman).SelectedValue) : null);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 269);
		}
	}

	private async Task LoadDataGroupAsync(CancellationToken token)
	{
		try
		{
			ComboBoxEx cMBGroup = CMBGroup;
			((ComboBox)cMBGroup).DataSource = await PersonGroup.SelectAllAsync(((CacheBaseForm)this).ContextGuid, containsAllGroups: true, token);
			((ListControl)CMBGroup).ValueMember = "Id";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 280);
		}
	}

	private async Task LoadDataDepartmentAsync(CancellationToken token)
	{
		try
		{
			List<AttDepartmentBusiness> list = (await AttDepartmentBusiness.GetAllAsync(((CacheBaseForm)this).ContextGuid, (bool?)true, token, "")).ToList();
			list.Add(new AttDepartmentBusiness(((CacheBaseForm)this).ContextGuid)
			{
				Name = "[نامشخص]",
				Guid = Guid.Empty
			});
			DepartmanbindingSource.DataSource = ((list != null) ? Utilities.ToSortableBindingList<AttDepartmentBusiness>((IEnumerable<AttDepartmentBusiness>)list.OrderBy((AttDepartmentBusiness q) => q.Name), token) : null);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 294);
		}
	}

	private async Task LoaddataTitleAsync(CancellationToken token)
	{
		try
		{
			ComboBoxEx cMBTitle = CMBTitle;
			((ComboBox)cMBTitle).DataSource = await PersonTitle.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoaddataTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 305);
		}
	}

	private async Task LoadDataEisarAsync(CancellationToken token)
	{
		try
		{
			ComboBoxEx cmbEisar = CmbEisar;
			((ComboBox)cmbEisar).DataSource = await Per_Eisar.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "-----", token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataEisarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 316);
		}
	}

	private void LoadDataJensiat()
	{
		try
		{
			if (trDictionary.TryGetValue("[مرد]", out var value))
			{
				CmbMale1.Text = value;
			}
			if (trDictionary.TryGetValue("[زن]", out var value2))
			{
				CmbFamle.Text = value2;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataJensiat", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 330);
		}
	}

	private async void BtnAccept_Click(object sender, EventArgs e)
	{
		if (!((Control)BtnAccept).Enabled || !((Control)BtnAccept).Visible)
		{
			return;
		}
		GetData();
		_loadDataToken?.Cancel();
		_loadDataToken = new CancellationTokenSource();
		CancellationToken token = _loadDataToken.Token;
		DialogResult val = await PersonCheckValidation.CheckValidationAsync(((CacheBaseForm)this).ContextGuid, ClsPerson, isShowMessage: true, token);
		if ((int)val != 2)
		{
			if ((int)val == 6)
			{
				SavePicture();
				ClsPerson.sentToServerStatus = (ServerStatusForClient)0;
				if (await ClsPerson.SaveAsync(token))
				{
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)1, "[اطلاعات با موفقیت ثبت شد]", "", 3);
				}
				else
				{
					((Control)BtnAccept).Enabled = true;
					((Control)numCodePersonel).Focus();
				}
			}
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 376);
		}
	}

	private async void FRMPersonel_KeyDown(object sender, KeyEventArgs e)
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
		else if ((int)keyCode != 45)
		{
			if ((int)keyCode != 116)
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
		else
		{
			e.Handled = true;
			((PopupItemControl)BtnSelectDeviceCode).PerformClick();
		}
	}

	private void TxtNameFather_Enter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((TextBoxBase)(TextBox)sender).SelectAll();
			((Control)(TextBox)sender).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtNameFather_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 440);
		}
	}

	private void textBoxX1_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(TextBox)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "textBoxX1_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 452);
		}
	}

	private async void BtnSelectDeviceCode_Click(object sender, EventArgs e)
	{
		_ = 6;
		try
		{
			if (ClsPerson == null || ((PersonBase)ClsPerson).Id <= 0)
			{
				MessageBoxFarsi.Show("ابتدا شخص را تعریف و ذخیره کنید . سپس جهت اختصاص کد سخت افزاری اقدام کنید");
				return;
			}
			FrmHardwareCodeList frm = new FrmHardwareCodeList(((CacheBaseForm)this).ContextGuid, ClsPerson, LstIdDevice);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				HardwareCodeCus HardwareCode = new HardwareCodeCus(((CacheBaseForm)this).ContextGuid)
				{
					DeviceId = frm.SelectedDeviceId,
					PersonId = ((PersonBase)ClsPerson).Id,
					HardwareCode = frm.SelectedHardwareCode
				};
				HardwareCodeCus hardwareCodeCus = await HardwareCodeCus.GetAsync_ByPersonNoDeviceId(((CacheBaseForm)this).ContextGuid, int.Parse(frm.SelectedHardwareCode), frm.SelectedDeviceId);
				if (hardwareCodeCus == null)
				{
					await HardwareCode.SaveAsync();
				}
				else
				{
					string personName = "";
					TimeAttendanceDAL.Dal.FLRPerson.Person person = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, hardwareCodeCus.PersonId);
					if (person != null)
					{
						personName = ((PersonBase)person).FullName;
					}
					if (6 == (int)FMessageBox.Show("کد سخت افزاری " + frm.SelectedHardwareCode + " قبلا به شخصی به نام " + personName + " اختصاص داده شده است.\r\n آیا ادامه می دهید؟", "", FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Question) && await HardwareCodeCus.DeleteAsync(((CacheBaseForm)this).ContextGuid, frm.SelectedDeviceId, frm.SelectedHardwareCode, default(CancellationToken)))
					{
						await HardwareCode.SaveAsync();
					}
				}
			}
			await FillGridHardwareCodesAsync(default(CancellationToken));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnDelDevCode_Click(object sender, EventArgs e)
	{
		((Control)DGridHardwareCodes).Focus();
		if (((DataGridView)DGridHardwareCodes).Rows.Count <= 0 || !((Control)BtnDelDevCode).Visible || !((Control)BtnDelDevCode).Enabled)
		{
			return;
		}
		try
		{
			if ((int)FMessageBox.Show(Utilities.GetTranslate("[برای حذف کردن مطمئن هستید؟]"), Utilities.GetTranslate("[هشدار]"), FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Error, FMessageBox.FMessageBoxDefaultButton.Button2) != 7 && ((DataGridView)DGridHardwareCodes).CurrentRow != null)
			{
				short deviceId = short.Parse(((DataGridView)DGridHardwareCodes).CurrentRow.Cells[((DataGridViewBand)DG_DeviceID).Index].Value.ToString());
				string hardwareCode = ((DataGridView)DGridHardwareCodes).CurrentRow.Cells[((DataGridViewBand)DG_PersonNO).Index].Value.ToString();
				await HardwareCodeCus.DeleteAsync(((CacheBaseForm)this).ContextGuid, deviceId, hardwareCode, default(CancellationToken));
				FillGridHardwareCodesAsync(default(CancellationToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelDevCode_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 517);
		}
	}

	private bool LoadPicture(string fileName)
	{
		bool result = false;
		try
		{
			if (File.Exists(fileName))
			{
				PicPerson.SizeMode = (PictureBoxSizeMode)1;
				PicPerson.Image = Image.FromFile(fileName);
				result = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadPicture", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 535);
		}
		return result;
	}

	private void LoadFromFilePic()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		try
		{
			FDFace.Multiselect = false;
			((FileDialog)FDFace).Filter = "*.JPG;*.GIF;*.PNG|*.JPG;*.GIF;*.PNG";
			if ((int)((CommonDialog)FDFace).ShowDialog((IWin32Window)(object)this) == 1)
			{
				if (new FileInfo(((FileDialog)FDFace).FileName).Length > 1000000)
				{
					Utility.MessageBoxShowError("[اندازه فایل بیش از حد است.]", ((CacheBaseForm)this).ContextGuid);
				}
				else
				{
					LoadPicture(((FileDialog)FDFace).FileName);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadFromFilePic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 555);
		}
	}

	private async void BTNMoreG_Click_1(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CMBGroup).Focus();
			FRMPersonGroupMain frm = new FRMPersonGroupMain(((CacheBaseForm)this).ContextGuid, FontFrm);
			short id = -1;
			if (((ListControl)CMBGroup).SelectedValue != null && short.TryParse(((ListControl)CMBGroup).SelectedValue.ToString(), out var result))
			{
				id = result;
			}
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1 && frm.ID > 0)
			{
				id = frm.ID;
			}
			_loadDataToken?.Cancel();
			_loadDataToken?.Dispose();
			_loadDataToken = new CancellationTokenSource();
			await LoadDataGroupAsync(_loadDataToken.Token);
			((ListControl)CMBGroup).SelectedValue = id;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNMoreG_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 580);
		}
	}

	private async void BTNMoreT_Click_1(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CMBTitle).Focus();
			FRMPersonTitleMain frm = new FRMPersonTitleMain(((CacheBaseForm)this).ContextGuid, btnSelect: true, FontFrm);
			if (((ListControl)CMBTitle).SelectedValue != null)
			{
				short id = short.Parse(((ListControl)CMBTitle).SelectedValue.ToString());
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					id = frm.SelectedRow.Id;
				}
				_loadDataToken?.Cancel();
				_loadDataToken = new CancellationTokenSource();
				CancellationToken token = _loadDataToken.Token;
				await LoaddataTitleAsync(token);
				((ListControl)CMBTitle).SelectedValue = id;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNMoreT_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 603);
		}
	}

	private void BtnAddPic_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtFamily).Focus();
			LoadFromFilePic();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAddPic_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 616);
		}
	}

	private void BtnDelPic_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtFamily).Focus();
			PicPerson.Image = (Image)(object)Resources.people;
			((PersonBase)ClsPerson).PicPath = "";
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelPic_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 629);
		}
	}

	private void FRMPersonel_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)numCodePersonel).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMPersonel_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 645);
		}
	}

	private async void BtnMoreEisar_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CmbEisar).Focus();
			short id = 0;
			FRMEisarMain frm = new FRMEisarMain(((CacheBaseForm)this).ContextGuid, btSelect: true);
			if (((ListControl)CmbEisar).SelectedValue != null)
			{
				id = short.Parse(((ListControl)CmbEisar).SelectedValue.ToString());
			}
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				id = frm.ID;
			}
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			await LoadDataEisarAsync(token);
			((ListControl)CmbEisar).SelectedValue = id;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreEisar_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 669);
		}
	}

	private void BtnDelEisar_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbEisar).Focus();
			((ListControl)CmbEisar).SelectedValue = 0;
			((PersonBase)ClsPerson).EisarId = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelEisar_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 682);
		}
	}

	private void TxtCodePersonel_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		try
		{
			e.SuppressKeyPress = ((int)e.KeyCode < 96 || (int)e.KeyCode > 105) && ((int)e.KeyCode < 48 || (int)e.KeyCode > 57) && (int)e.KeyCode != 13 && (int)e.KeyCode != 38 && (int)e.KeyCode != 40 && (int)e.KeyCode != 39 && (int)e.KeyCode != 37 && (int)e.KeyCode != 46 && (int)e.KeyCode != 8;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtCodePersonel_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 696);
		}
	}

	private void ChbBirth_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)FarsiDateBirthDay).Enabled = ChbBirth.Checked;
			if (((Control)FarsiDateBirthDay).Enabled)
			{
				((Control)FarsiDateBirthDay).Text = Calendar.MiladiToCalture(ConstantValues.MinDateM);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbBirth_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 710);
		}
	}

	private void ChbSWork_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)farsiDateSWork).Enabled = ChbSWork.Checked;
			if (!((Control)farsiDateSWork).Enabled)
			{
				((Control)farsiDateSWork).Text = Calendar.MiladiToCalture(ConstantValues.MinDateM);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbSWork_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 724);
		}
	}

	private void ChbTark_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)DateTark).Enabled = ChbTark.Checked;
			if (!((Control)DateTark).Enabled)
			{
				((Control)DateTark).Text = Calendar.MiladiToCalture(ConstantValues.MinDateM);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbTark_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 738);
		}
	}

	private void PicPerson_Click(object sender, EventArgs e)
	{
		LoadFromFilePic();
	}

	private void SavePicture()
	{
		try
		{
			if (((FileDialog)FDFace).FileName == "openFileDialog1")
			{
				return;
			}
			string text = Application.StartupPath + "\\pictures\\";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (!string.IsNullOrEmpty(((FileDialog)FDFace).FileName))
			{
				string text2 = Guid.NewGuid().ToString() + Path.GetExtension(((FileDialog)FDFace).FileName);
				text += text2;
				File.Copy(((FileDialog)FDFace).FileName, text, overwrite: true);
				if (((PersonBase)ClsPerson).PicPath != null)
				{
					FileToBeDelete = ((PersonBase)ClsPerson).PicPath;
				}
				((PersonBase)ClsPerson).PicPath = text;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SavePicture", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 766);
		}
	}

	private async void TxtUserName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (ClsPerson != null && ((Control)TxtUserName).Focused)
			{
				ClsPerson.UserName = ((Control)TxtUserName).Text;
				LabelX val = lblUrlQueryString;
				((Control)val).Text = (await ClsPerson.GenerateUrlQueryStringAsync(((CacheBaseForm)this).ContextGuid)) ?? "";
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void btnSetPassword_Click(object sender, EventArgs e)
	{
		try
		{
			frmHashedPassword val = new frmHashedPassword(((CacheBaseForm)this).ContextGuid, ((PersonBase)ClsPerson).Guid);
			if ((int)((Form)val).ShowDialog((IWin32Window)(object)this) == 1 && val.HashPassword != null && val.HashPassword.Length > 1)
			{
				ClsPerson.HashPassword = val.HashPassword;
				((Control)TxtPassword).Text = Convert.ToBase64String(ClsPerson.HashPassword);
				LabelX val2 = lblUrlQueryString;
				((Control)val2).Text = (await ClsPerson.GenerateUrlQueryStringAsync(((CacheBaseForm)this).ContextGuid)) ?? "";
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void DepartmanbindingSource_CurrentChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		CurrentDepartment = (AttDepartmentBusiness)DepartmanbindingSource.Current;
	}

	private void btnMoreDep_Click(object sender, EventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		try
		{
			frmDepartmentList frmDepartmentList = new frmDepartmentList(((CacheBaseForm)this).ContextGuid, selectMode: true);
			if ((int)((Form)frmDepartmentList).ShowDialog((IWin32Window)(object)this) == 1)
			{
				((PersonBase)ClsPerson).GuidDepartment = frmDepartmentList.DepartmentGuid;
				((ListControl)cmbDepartman).SelectedValue = ((PersonBase)ClsPerson).GuidDepartment;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnCreateAndSelectNewCode_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (ClsPerson == null || ((PersonBase)ClsPerson).Id <= 0)
			{
				MessageBoxFarsi.Show("ابتدا شخص را تعریف و ذخیره کنید . سپس جهت اختصاص کد سخت افزاری اقدام کنید");
				return;
			}
			await ((CacheBaseForm)new FrmDefinitionNewCode(((CacheBaseForm)this).ContextGuid, ClsPerson)).ShowAsync((IWin32Window)(object)this);
			await FillGridHardwareCodesAsync(default(CancellationToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCreateAndSelectNewCode_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Person\\FRMPersonel.cs", 820);
		}
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
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
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
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Expected O, but got Unknown
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Expected O, but got Unknown
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Expected O, but got Unknown
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Expected O, but got Unknown
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected O, but got Unknown
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Expected O, but got Unknown
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Expected O, but got Unknown
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Expected O, but got Unknown
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Expected O, but got Unknown
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Expected O, but got Unknown
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Expected O, but got Unknown
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Expected O, but got Unknown
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Expected O, but got Unknown
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Expected O, but got Unknown
		//IL_09b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c2: Expected O, but got Unknown
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Expected O, but got Unknown
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d4: Expected O, but got Unknown
		//IL_14cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d6: Expected O, but got Unknown
		//IL_187e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1888: Expected O, but got Unknown
		//IL_193c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1946: Expected O, but got Unknown
		//IL_1aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af5: Expected O, but got Unknown
		//IL_1c30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3a: Expected O, but got Unknown
		//IL_1cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d02: Expected O, but got Unknown
		//IL_1fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff1: Expected O, but got Unknown
		//IL_23d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_23e3: Expected O, but got Unknown
		//IL_247a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2484: Expected O, but got Unknown
		//IL_25ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d4: Expected O, but got Unknown
		//IL_294f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2959: Expected O, but got Unknown
		//IL_2b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b16: Expected O, but got Unknown
		//IL_2ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3396: Unknown result type (might be due to invalid IL or missing references)
		//IL_33a0: Expected O, but got Unknown
		//IL_351d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3527: Expected O, but got Unknown
		//IL_3553: Unknown result type (might be due to invalid IL or missing references)
		//IL_355d: Expected O, but got Unknown
		//IL_36d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e2: Expected O, but got Unknown
		//IL_370e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3718: Expected O, but got Unknown
		//IL_3890: Unknown result type (might be due to invalid IL or missing references)
		//IL_389a: Expected O, but got Unknown
		//IL_38c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_38d0: Expected O, but got Unknown
		//IL_3fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4009: Expected O, but got Unknown
		//IL_4a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b52: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b5c: Expected O, but got Unknown
		//IL_4d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d30: Expected O, but got Unknown
		//IL_55bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_55c9: Expected O, but got Unknown
		//IL_5661: Unknown result type (might be due to invalid IL or missing references)
		//IL_566b: Expected O, but got Unknown
		//IL_593f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5949: Expected O, but got Unknown
		//IL_5995: Unknown result type (might be due to invalid IL or missing references)
		//IL_599f: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		panelEx1 = new PanelEx();
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		tabControl1 = new TabControl();
		tabControlPanel1 = new TabControlPanel();
		groupPanel7 = new GroupPanel();
		BtnCreateAndSelectNewCode = new ButtonX();
		btnMoreDep = new ButtonX();
		cmbDepartman = new ComboBoxEx();
		DepartmanbindingSource = new BindingSource(components);
		labelX32 = new LabelX();
		lblUrlQueryString = new LabelX();
		TxtPassword = new TextBoxX();
		labelX25 = new LabelX();
		TxtUserName = new TextBoxX();
		labelX27 = new LabelX();
		labelX23 = new LabelX();
		labelX21 = new LabelX();
		numCodePersonel = new TextBoxX();
		NumCodeTafsil = new TextBoxX();
		labelX29 = new LabelX();
		PicPerson = new PictureBox();
		BtnDelPic = new ButtonX();
		BtnAddPic = new ButtonX();
		BtnDelDevCode = new ButtonX();
		BtnSelectDeviceCode = new ButtonX();
		TXTDes = new TextBoxX();
		labelX7 = new LabelX();
		btnSetPassword = new ButtonX();
		BTNMoreT = new ButtonX();
		BTNMoreG = new ButtonX();
		CMBTitle = new ComboBoxEx();
		personTitleBindingSource = new BindingSource(components);
		CMBGroup = new ComboBoxEx();
		personGroupBindingSource = new BindingSource(components);
		LabelX12 = new LabelX();
		LabelX11 = new LabelX();
		DGridHardwareCodes = new DataGridViewX();
		DG_DeviceID = new DataGridViewTextBoxColumn();
		DG_DeviceName = new DataGridViewTextBoxColumn();
		DG_PersonNO = new DataGridViewTextBoxColumn();
		DG_PersonId = new DataGridViewTextBoxColumn();
		DG_FullName = new DataGridViewTextBoxColumn();
		hardwareCodeCUSViewBindingSource = new BindingSource(components);
		TxtAdd = new TextBoxX();
		TxtTel = new TextBoxX();
		labelX6 = new LabelX();
		labelX4 = new LabelX();
		TxtMob = new TextBoxX();
		labelX3 = new LabelX();
		TxtFamily = new TextBoxX();
		TxtLatinName = new TextBoxX();
		labelX33 = new LabelX();
		TxtName = new TextBoxX();
		labelX1 = new LabelX();
		labelX2 = new LabelX();
		labelX5 = new LabelX();
		tabItem1 = new TabItem(components);
		tabControlPanel3 = new TabControlPanel();
		panelEx2 = new PanelEx();
		farsiDateSWork = new DateBoxFa();
		DateTark = new DateBoxFa();
		FarsiDateBirthDay = new DateBoxFa();
		BtnDelEisar = new ButtonX();
		BtnMoreEisar = new ButtonX();
		CmbEisar = new ComboBoxEx();
		labelX35 = new LabelX();
		ChbTark = new CheckBoxX();
		ChbSWork = new CheckBoxX();
		txtNoBimeh = new TextBoxX();
		labelX19 = new LabelX();
		txtMadrak = new TextBoxX();
		labelX42 = new LabelX();
		CMBJensiat = new ComboBoxEx();
		CmbUnknown = new ComboItem();
		CmbMale1 = new ComboItem();
		CmbFamle = new ComboItem();
		labelX31 = new LabelX();
		TxtSerialSH = new TextBoxX();
		labelX30 = new LabelX();
		labelX22 = new LabelX();
		TxtSHSH = new TextBoxX();
		labelX20 = new LabelX();
		TxtZip = new TextBoxX();
		TxtMSodor = new TextBoxX();
		labelX28 = new LabelX();
		labelX26 = new LabelX();
		labelX24 = new LabelX();
		ChbBirth = new CheckBoxX();
		TxtNameF = new TextBoxX();
		TxtCodeM = new TextBoxX();
		tabItem3 = new TabItem(components);
		companyBindingSource = new BindingSource(components);
		bankHesabTypeBindingSource = new BindingSource(components);
		perKhedmatPlaceBindingSource = new BindingSource(components);
		perTypeEstekhdamBindingSource = new BindingSource(components);
		perEisarBindingSource = new BindingSource(components);
		perKhedmatBindingSource = new BindingSource(components);
		perPartBindingSource = new BindingSource(components);
		bankBindingSource = new BindingSource(components);
		maliYearBindingSource = new BindingSource(components);
		shiftsBindingSource = new BindingSource(components);
		perWorkTitleBindingSource = new BindingSource(components);
		tabItem2 = new TabItem(components);
		tabControlPanel2 = new TabControlPanel();
		groupPanel3 = new GroupPanel();
		labelX8 = new LabelX();
		buttonX1 = new ButtonX();
		textBoxX1 = new TextBoxX();
		labelX18 = new LabelX();
		labelX17 = new LabelX();
		labelX16 = new LabelX();
		labelX15 = new LabelX();
		labelX14 = new LabelX();
		TxtNumberShenasname = new TextBoxX();
		TxtCodeMeli = new TextBoxX();
		TxtNameFather = new TextBoxX();
		CmbSexual = new ComboBoxEx();
		CmbFemale = new ComboItem();
		CmbMale = new ComboItem();
		CmbMariage = new ComboBoxEx();
		CmbNaMalom = new ComboItem();
		CmbSingel = new ComboItem();
		CmbMarried1 = new ComboItem();
		labelX10 = new LabelX();
		labelX9 = new LabelX();
		labelX13 = new LabelX();
		TxtBimeNum = new TextBoxX();
		numerichild = new NumericUpDown();
		FDFace = new OpenFileDialog();
		labelX34 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)tabControl1).BeginInit();
		((Control)tabControl1).SuspendLayout();
		((Control)tabControlPanel1).SuspendLayout();
		((Control)groupPanel7).SuspendLayout();
		((ISupportInitialize)DepartmanbindingSource).BeginInit();
		((ISupportInitialize)PicPerson).BeginInit();
		((ISupportInitialize)personTitleBindingSource).BeginInit();
		((ISupportInitialize)personGroupBindingSource).BeginInit();
		((ISupportInitialize)DGridHardwareCodes).BeginInit();
		((ISupportInitialize)hardwareCodeCUSViewBindingSource).BeginInit();
		((Control)tabControlPanel3).SuspendLayout();
		((Control)panelEx2).SuspendLayout();
		((ISupportInitialize)companyBindingSource).BeginInit();
		((ISupportInitialize)bankHesabTypeBindingSource).BeginInit();
		((ISupportInitialize)perKhedmatPlaceBindingSource).BeginInit();
		((ISupportInitialize)perTypeEstekhdamBindingSource).BeginInit();
		((ISupportInitialize)perEisarBindingSource).BeginInit();
		((ISupportInitialize)perKhedmatBindingSource).BeginInit();
		((ISupportInitialize)perPartBindingSource).BeginInit();
		((ISupportInitialize)bankBindingSource).BeginInit();
		((ISupportInitialize)maliYearBindingSource).BeginInit();
		((ISupportInitialize)shiftsBindingSource).BeginInit();
		((ISupportInitialize)perWorkTitleBindingSource).BeginInit();
		((Control)tabControlPanel2).SuspendLayout();
		((ISupportInitialize)numerichild).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)tabControl1);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(721, 537);
		panelEx1.Style.Alignment = (StringAlignment)1;
		panelEx1.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx1.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx1.Style.Border = (eBorderType)1;
		panelEx1.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx1.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx1.Style.GradientAngle = 90;
		((Control)panelEx1).TabIndex = 0;
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(14, 499);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(117, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 1;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو]   (Esc)";
		((Control)BtnCancel).Click += BtnCancel_Click;
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(138, 499);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		((Control)BtnAccept).Size = new Size(111, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 0;
		((Control)BtnAccept).Text = "[ثبت]  (F5)";
		((Control)BtnAccept).Click += BtnAccept_Click;
		((Control)tabControl1).Anchor = (AnchorStyles)15;
		((Control)tabControl1).BackColor = Color.Transparent;
		tabControl1.CanReorderTabs = true;
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel1);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel3);
		((Control)tabControl1).Location = new Point(3, 3);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedTabFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		tabControl1.SelectedTabIndex = 0;
		((Control)tabControl1).Size = new Size(717, 479);
		((Control)tabControl1).TabIndex = 0;
		tabControl1.TabLayoutType = (eTabLayoutType)1;
		tabControl1.Tabs.Add(tabItem1);
		tabControl1.Tabs.Add(tabItem3);
		((Control)tabControl1).Text = "tabControl1";
		((Control)tabControlPanel1).Controls.Add((Control)(object)groupPanel7);
		((PanelEx)tabControlPanel1).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel1).Dock = (DockStyle)5;
		((Control)tabControlPanel1).Location = new Point(0, 26);
		((Control)tabControlPanel1).Name = "tabControlPanel1";
		((Control)tabControlPanel1).Padding = new Padding(1);
		((Control)tabControlPanel1).Size = new Size(717, 453);
		((PanelEx)tabControlPanel1).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel1).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel1).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel1).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel1).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel1).Style.GradientAngle = 90;
		((Control)tabControlPanel1).TabIndex = 0;
		tabControlPanel1.TabItem = tabItem1;
		((Control)groupPanel7).BackColor = Color.Transparent;
		((PanelControl)groupPanel7).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel7).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanel7).Controls.Add((Control)(object)BtnCreateAndSelectNewCode);
		((Control)groupPanel7).Controls.Add((Control)(object)btnMoreDep);
		((Control)groupPanel7).Controls.Add((Control)(object)cmbDepartman);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX32);
		((Control)groupPanel7).Controls.Add((Control)(object)lblUrlQueryString);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtPassword);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX25);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtUserName);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX27);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX23);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX21);
		((Control)groupPanel7).Controls.Add((Control)(object)numCodePersonel);
		((Control)groupPanel7).Controls.Add((Control)(object)NumCodeTafsil);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX29);
		((Control)groupPanel7).Controls.Add((Control)(object)PicPerson);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnDelPic);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnAddPic);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnDelDevCode);
		((Control)groupPanel7).Controls.Add((Control)(object)BtnSelectDeviceCode);
		((Control)groupPanel7).Controls.Add((Control)(object)TXTDes);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX7);
		((Control)groupPanel7).Controls.Add((Control)(object)btnSetPassword);
		((Control)groupPanel7).Controls.Add((Control)(object)BTNMoreT);
		((Control)groupPanel7).Controls.Add((Control)(object)BTNMoreG);
		((Control)groupPanel7).Controls.Add((Control)(object)CMBTitle);
		((Control)groupPanel7).Controls.Add((Control)(object)CMBGroup);
		((Control)groupPanel7).Controls.Add((Control)(object)LabelX12);
		((Control)groupPanel7).Controls.Add((Control)(object)LabelX11);
		((Control)groupPanel7).Controls.Add((Control)(object)DGridHardwareCodes);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtAdd);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtTel);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX6);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX4);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtMob);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX34);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX3);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtFamily);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtLatinName);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX33);
		((Control)groupPanel7).Controls.Add((Control)(object)TxtName);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX1);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX2);
		((Control)groupPanel7).Controls.Add((Control)(object)labelX5);
		groupPanel7.DisabledBackColor = Color.Empty;
		((Control)groupPanel7).Dock = (DockStyle)5;
		((Control)groupPanel7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel7).Location = new Point(1, 1);
		((Control)groupPanel7).Name = "groupPanel7";
		((Control)groupPanel7).Size = new Size(715, 451);
		((PanelControl)groupPanel7).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel7).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel7).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel7).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderBottomWidth = 3;
		((PanelControl)groupPanel7).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel7).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderLeftWidth = 3;
		((PanelControl)groupPanel7).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderRightWidth = 3;
		((PanelControl)groupPanel7).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel7).Style.BorderTopWidth = 3;
		((PanelControl)groupPanel7).Style.CornerDiameter = 4;
		((PanelControl)groupPanel7).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel7).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel7).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel7).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel7).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel7).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel7).TabIndex = 0;
		((Control)BtnCreateAndSelectNewCode).AccessibleRole = (AccessibleRole)43;
		BtnCreateAndSelectNewCode.ColorTable = (eButtonColor)6;
		BtnCreateAndSelectNewCode.Image = (Image)(object)Resources.edit_add1;
		((Control)BtnCreateAndSelectNewCode).Location = new Point(15, 327);
		((Control)BtnCreateAndSelectNewCode).Name = "BtnCreateAndSelectNewCode";
		((Control)BtnCreateAndSelectNewCode).Size = new Size(102, 28);
		((PopupItemControl)BtnCreateAndSelectNewCode).Style = (eDotNetBarStyle)9;
		((Control)BtnCreateAndSelectNewCode).TabIndex = 43;
		((Control)BtnCreateAndSelectNewCode).TabStop = false;
		((Control)BtnCreateAndSelectNewCode).Text = "          تعریف";
		BtnCreateAndSelectNewCode.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnCreateAndSelectNewCode).Click += BtnCreateAndSelectNewCode_Click;
		((Control)btnMoreDep).AccessibleRole = (AccessibleRole)43;
		btnMoreDep.ColorTable = (eButtonColor)6;
		((Control)btnMoreDep).Location = new Point(433, 75);
		((Control)btnMoreDep).Name = "btnMoreDep";
		((Control)btnMoreDep).Size = new Size(27, 21);
		((PopupItemControl)btnMoreDep).Style = (eDotNetBarStyle)9;
		((Control)btnMoreDep).TabIndex = 42;
		((Control)btnMoreDep).TabStop = false;
		((Control)btnMoreDep).Text = "...";
		((Control)btnMoreDep).Click += btnMoreDep_Click;
		((ComboBox)cmbDepartman).DataSource = DepartmanbindingSource;
		((ListControl)cmbDepartman).DisplayMember = "Name";
		((ComboBox)cmbDepartman).DrawMode = (DrawMode)1;
		((ComboBox)cmbDepartman).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cmbDepartman).FormattingEnabled = true;
		((ComboBox)cmbDepartman).ItemHeight = 16;
		((Control)cmbDepartman).Location = new Point(466, 75);
		((Control)cmbDepartman).Name = "cmbDepartman";
		((Control)cmbDepartman).Size = new Size(120, 22);
		cmbDepartman.Style = (eDotNetBarStyle)9;
		((Control)cmbDepartman).TabIndex = 4;
		((ListControl)cmbDepartman).ValueMember = "Guid";
		DepartmanbindingSource.DataSource = typeof(AttDepartmentBusiness);
		DepartmanbindingSource.CurrentChanged += DepartmanbindingSource_CurrentChanged;
		((BaseItemControl)labelX32).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX32).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX32).Location = new Point(594, 75);
		((Control)labelX32).Name = "labelX32";
		((Control)labelX32).RightToLeft = (RightToLeft)1;
		((Control)labelX32).Size = new Size(98, 21);
		((Control)labelX32).TabIndex = 40;
		((Control)labelX32).Text = "دپارتمان :";
		labelX32.TextAlignment = (StringAlignment)2;
		((Control)lblUrlQueryString).Anchor = (AnchorStyles)9;
		((Control)lblUrlQueryString).BackColor = Color.Transparent;
		((BaseItemControl)lblUrlQueryString).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblUrlQueryString).Location = new Point(14, 179);
		((Control)lblUrlQueryString).Name = "lblUrlQueryString";
		((Control)lblUrlQueryString).RightToLeft = (RightToLeft)1;
		((Control)lblUrlQueryString).Size = new Size(573, 21);
		((Control)lblUrlQueryString).TabIndex = 39;
		((Control)lblUrlQueryString).Text = "---";
		lblUrlQueryString.TextAlignment = (StringAlignment)2;
		TxtPassword.Border.Class = "TextBoxBorder";
		TxtPassword.Border.CornerType = (eCornerType)1;
		((Control)TxtPassword).Location = new Point(159, 157);
		((TextBoxBase)TxtPassword).MaxLength = 11;
		((Control)TxtPassword).Name = "TxtPassword";
		((TextBox)TxtPassword).PasswordChar = '*';
		((TextBoxBase)TxtPassword).ReadOnly = true;
		((Control)TxtPassword).Size = new Size(119, 21);
		((Control)TxtPassword).TabIndex = 9;
		((TextBox)TxtPassword).UseSystemPasswordChar = true;
		((Control)labelX25).BackColor = Color.Transparent;
		((BaseItemControl)labelX25).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX25).Location = new Point(348, 179);
		((Control)labelX25).Name = "labelX25";
		((Control)labelX25).RightToLeft = (RightToLeft)1;
		((Control)labelX25).Size = new Size(125, 21);
		((Control)labelX25).TabIndex = 37;
		((Control)labelX25).Text = "رمز :";
		labelX25.TextAlignment = (StringAlignment)2;
		TxtUserName.Border.Class = "TextBoxBorder";
		TxtUserName.Border.CornerType = (eCornerType)1;
		((Control)TxtUserName).Location = new Point(431, 157);
		((TextBoxBase)TxtUserName).MaxLength = 11;
		((Control)TxtUserName).Name = "TxtUserName";
		((Control)TxtUserName).Size = new Size(154, 21);
		((Control)TxtUserName).TabIndex = 8;
		((Control)TxtUserName).TextChanged += TxtUserName_TextChanged;
		((Control)labelX27).BackColor = Color.Transparent;
		((BaseItemControl)labelX27).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX27).Location = new Point(594, 157);
		((Control)labelX27).Name = "labelX27";
		((Control)labelX27).RightToLeft = (RightToLeft)1;
		((Control)labelX27).Size = new Size(98, 21);
		((Control)labelX27).TabIndex = 35;
		((Control)labelX27).Text = "نام کاربری پرتال :";
		labelX27.TextAlignment = (StringAlignment)2;
		((Control)labelX23).Anchor = (AnchorStyles)10;
		((BaseItemControl)labelX23).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX23).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX23).Location = new Point(604, 359);
		((Control)labelX23).Name = "labelX23";
		((Control)labelX23).Size = new Size(98, 49);
		((Control)labelX23).TabIndex = 34;
		((Control)labelX23).Text = "[کدهای  شخص    در دستگاه     حضور و غیاب]";
		labelX23.TextAlignment = (StringAlignment)1;
		labelX23.TextLineAlignment = (StringAlignment)0;
		labelX23.WordWrap = true;
		((Control)labelX21).Anchor = (AnchorStyles)14;
		((BaseItemControl)labelX21).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX21).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX21).ForeColor = Color.Maroon;
		((Control)labelX21).Location = new Point(83, 301);
		((Control)labelX21).Name = "labelX21";
		((Control)labelX21).RightToLeft = (RightToLeft)1;
		((Control)labelX21).Size = new Size(503, 23);
		((Control)labelX21).TabIndex = 33;
		((Control)labelX21).Text = "بعد از اضافه یا حذف کردن کدهای شخص، دکمه ثبت را بزنید تا ذخیره شوند ";
		labelX21.WordWrap = true;
		numCodePersonel.Border.Class = "TextBoxBorder";
		numCodePersonel.Border.CornerType = (eCornerType)1;
		((Control)numCodePersonel).Location = new Point(433, 21);
		((TextBoxBase)numCodePersonel).MaxLength = 50;
		((Control)numCodePersonel).Name = "numCodePersonel";
		((Control)numCodePersonel).Size = new Size(153, 21);
		((Control)numCodePersonel).TabIndex = 0;
		NumCodeTafsil.Border.Class = "TextBoxBorder";
		NumCodeTafsil.Border.CornerType = (eCornerType)1;
		((Control)NumCodeTafsil).Location = new Point(131, 21);
		((TextBoxBase)NumCodeTafsil).MaxLength = 50;
		((Control)NumCodeTafsil).Name = "NumCodeTafsil";
		((Control)NumCodeTafsil).Size = new Size(147, 21);
		((Control)NumCodeTafsil).TabIndex = 1;
		((BaseItemControl)labelX29).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX29).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX29).Location = new Point(285, 21);
		((Control)labelX29).Name = "labelX29";
		((Control)labelX29).RightToLeft = (RightToLeft)1;
		((Control)labelX29).Size = new Size(142, 21);
		((Control)labelX29).TabIndex = 2;
		((Control)labelX29).Text = " کد تفصیل(پرسنلی)*:";
		labelX29.TextAlignment = (StringAlignment)2;
		PicPerson.BorderStyle = (BorderStyle)1;
		PicPerson.Image = (Image)(object)Resources.people;
		((Control)PicPerson).Location = new Point(14, 27);
		((Control)PicPerson).Name = "PicPerson";
		((Control)PicPerson).Size = new Size(93, 91);
		PicPerson.SizeMode = (PictureBoxSizeMode)1;
		PicPerson.TabIndex = 28;
		PicPerson.TabStop = false;
		((Control)PicPerson).Click += PicPerson_Click;
		((Control)BtnDelPic).AccessibleRole = (AccessibleRole)43;
		((Control)BtnDelPic).Anchor = (AnchorStyles)9;
		BtnDelPic.ColorTable = (eButtonColor)6;
		((Control)BtnDelPic).Font = new Font("Tahoma", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnDelPic.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelPic).Location = new Point(29, 118);
		((Control)BtnDelPic).Name = "BtnDelPic";
		((Control)BtnDelPic).Size = new Size(29, 27);
		((PopupItemControl)BtnDelPic).Style = (eDotNetBarStyle)9;
		((Control)BtnDelPic).TabIndex = 16;
		((Control)BtnDelPic).TabStop = false;
		((Control)BtnDelPic).Click += BtnDelPic_Click;
		((Control)BtnAddPic).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAddPic).Anchor = (AnchorStyles)9;
		BtnAddPic.ColorTable = (eButtonColor)6;
		((Control)BtnAddPic).Font = new Font("Tahoma", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAddPic.Image = (Image)(object)Resources.edit_add1;
		((Control)BtnAddPic).Location = new Point(64, 118);
		((Control)BtnAddPic).Name = "BtnAddPic";
		((Control)BtnAddPic).Size = new Size(29, 27);
		((PopupItemControl)BtnAddPic).Style = (eDotNetBarStyle)9;
		((Control)BtnAddPic).TabIndex = 15;
		((Control)BtnAddPic).TabStop = false;
		((Control)BtnAddPic).Click += BtnAddPic_Click;
		((Control)BtnDelDevCode).AccessibleRole = (AccessibleRole)43;
		BtnDelDevCode.ColorTable = (eButtonColor)6;
		BtnDelDevCode.Image = (Image)(object)Resources.stop;
		BtnDelDevCode.ImageFixedSize = new Size(28, 28);
		((Control)BtnDelDevCode).Location = new Point(15, 395);
		((Control)BtnDelDevCode).Name = "BtnDelDevCode";
		((Control)BtnDelDevCode).Size = new Size(102, 28);
		((PopupItemControl)BtnDelDevCode).Style = (eDotNetBarStyle)9;
		((Control)BtnDelDevCode).TabIndex = 14;
		((Control)BtnDelDevCode).TabStop = false;
		((Control)BtnDelDevCode).Text = "      حذف کد";
		BtnDelDevCode.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnDelDevCode).Click += BtnDelDevCode_Click;
		((Control)BtnSelectDeviceCode).AccessibleRole = (AccessibleRole)43;
		BtnSelectDeviceCode.ColorTable = (eButtonColor)6;
		BtnSelectDeviceCode.Image = (Image)(object)Resources.note1s;
		BtnSelectDeviceCode.ImageFixedSize = new Size(25, 25);
		((Control)BtnSelectDeviceCode).Location = new Point(15, 361);
		((Control)BtnSelectDeviceCode).Name = "BtnSelectDeviceCode";
		((Control)BtnSelectDeviceCode).Size = new Size(102, 28);
		((PopupItemControl)BtnSelectDeviceCode).Style = (eDotNetBarStyle)9;
		((Control)BtnSelectDeviceCode).TabIndex = 13;
		((Control)BtnSelectDeviceCode).TabStop = false;
		((Control)BtnSelectDeviceCode).Text = "         انتخاب";
		BtnSelectDeviceCode.TextAlignment = (eButtonTextAlignment)0;
		((Control)BtnSelectDeviceCode).Click += BtnSelectDeviceCode_Click;
		TXTDes.Border.Class = "TextBoxBorder";
		TXTDes.Border.CornerType = (eCornerType)1;
		((Control)TXTDes).Location = new Point(14, 232);
		((TextBoxBase)TXTDes).MaxLength = 100;
		((TextBoxBase)TXTDes).Multiline = true;
		((Control)TXTDes).Name = "TXTDes";
		((TextBox)TXTDes).ScrollBars = (ScrollBars)2;
		((Control)TXTDes).Size = new Size(573, 63);
		((Control)TXTDes).TabIndex = 12;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX7).Location = new Point(594, 238);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).RightToLeft = (RightToLeft)1;
		((Control)labelX7).Size = new Size(98, 22);
		((Control)labelX7).TabIndex = 20;
		((Control)labelX7).Text = "[شرح] :";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)btnSetPassword).AccessibleRole = (AccessibleRole)43;
		btnSetPassword.ColorTable = (eButtonColor)6;
		((Control)btnSetPassword).Location = new Point(126, 157);
		((Control)btnSetPassword).Name = "btnSetPassword";
		((Control)btnSetPassword).Size = new Size(27, 21);
		((PopupItemControl)btnSetPassword).Style = (eDotNetBarStyle)9;
		((Control)btnSetPassword).TabIndex = 9;
		((Control)btnSetPassword).TabStop = false;
		((Control)btnSetPassword).Text = "...";
		((Control)btnSetPassword).Click += btnSetPassword_Click;
		((Control)BTNMoreT).AccessibleRole = (AccessibleRole)43;
		BTNMoreT.ColorTable = (eButtonColor)6;
		((Control)BTNMoreT).Location = new Point(131, 47);
		((Control)BTNMoreT).Name = "BTNMoreT";
		((Control)BTNMoreT).Size = new Size(27, 21);
		((PopupItemControl)BTNMoreT).Style = (eDotNetBarStyle)9;
		((Control)BTNMoreT).TabIndex = 3;
		((Control)BTNMoreT).TabStop = false;
		((Control)BTNMoreT).Text = "...";
		((Control)BTNMoreT).Click += BTNMoreT_Click_1;
		((Control)BTNMoreG).AccessibleRole = (AccessibleRole)43;
		BTNMoreG.ColorTable = (eButtonColor)6;
		((Control)BTNMoreG).Location = new Point(433, 47);
		((Control)BTNMoreG).Name = "BTNMoreG";
		((Control)BTNMoreG).Size = new Size(27, 21);
		((PopupItemControl)BTNMoreG).Style = (eDotNetBarStyle)9;
		((Control)BTNMoreG).TabIndex = 6;
		((Control)BTNMoreG).TabStop = false;
		((Control)BTNMoreG).Text = "...";
		((Control)BTNMoreG).Click += BTNMoreG_Click_1;
		((ComboBox)CMBTitle).DataSource = personTitleBindingSource;
		((ListControl)CMBTitle).DisplayMember = "Name";
		((ComboBox)CMBTitle).DrawMode = (DrawMode)1;
		((ComboBox)CMBTitle).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMBTitle).FormattingEnabled = true;
		((ComboBox)CMBTitle).ItemHeight = 16;
		((Control)CMBTitle).Location = new Point(164, 47);
		((Control)CMBTitle).Name = "CMBTitle";
		((Control)CMBTitle).Size = new Size(114, 22);
		CMBTitle.Style = (eDotNetBarStyle)9;
		((Control)CMBTitle).TabIndex = 3;
		((ListControl)CMBTitle).ValueMember = "Id";
		((ComboBox)CMBGroup).DataSource = personGroupBindingSource;
		((ListControl)CMBGroup).DisplayMember = "Name";
		((ComboBox)CMBGroup).DrawMode = (DrawMode)1;
		((ComboBox)CMBGroup).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CMBGroup).FormattingEnabled = true;
		((ComboBox)CMBGroup).ItemHeight = 16;
		((Control)CMBGroup).Location = new Point(466, 47);
		((Control)CMBGroup).Name = "CMBGroup";
		((Control)CMBGroup).Size = new Size(120, 22);
		CMBGroup.Style = (eDotNetBarStyle)9;
		((Control)CMBGroup).TabIndex = 2;
		((ListControl)CMBGroup).ValueMember = "id";
		((BaseItemControl)LabelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX12).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX12).Location = new Point(594, 47);
		((Control)LabelX12).Name = "LabelX12";
		((Control)LabelX12).RightToLeft = (RightToLeft)1;
		((Control)LabelX12).Size = new Size(98, 21);
		((Control)LabelX12).TabIndex = 4;
		((Control)LabelX12).Text = "[گروه] :";
		LabelX12.TextAlignment = (StringAlignment)2;
		((BaseItemControl)LabelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LabelX11).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LabelX11).Location = new Point(285, 47);
		((Control)LabelX11).Name = "LabelX11";
		((Control)LabelX11).RightToLeft = (RightToLeft)1;
		((Control)LabelX11).Size = new Size(125, 21);
		((Control)LabelX11).TabIndex = 7;
		((Control)LabelX11).Text = "[عنوان] :";
		LabelX11.TextAlignment = (StringAlignment)2;
		((DataGridView)DGridHardwareCodes).AllowUserToAddRows = false;
		((DataGridView)DGridHardwareCodes).AllowUserToDeleteRows = false;
		((DataGridView)DGridHardwareCodes).AllowUserToResizeRows = false;
		val.BackColor = Color.FromArgb(255, 255, 192);
		((DataGridView)DGridHardwareCodes).AlternatingRowsDefaultCellStyle = val;
		((DataGridView)DGridHardwareCodes).AutoGenerateColumns = false;
		((DataGridView)DGridHardwareCodes).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGridHardwareCodes).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)DG_DeviceID,
			(DataGridViewColumn)DG_DeviceName,
			(DataGridViewColumn)DG_PersonNO,
			(DataGridViewColumn)DG_PersonId,
			(DataGridViewColumn)DG_FullName
		});
		((DataGridView)DGridHardwareCodes).DataSource = hardwareCodeCUSViewBindingSource;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.ControlText;
		val2.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGridHardwareCodes).DefaultCellStyle = val2;
		((DataGridView)DGridHardwareCodes).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGridHardwareCodes).Location = new Point(123, 328);
		((Control)DGridHardwareCodes).Name = "DGridHardwareCodes";
		((DataGridView)DGridHardwareCodes).ReadOnly = true;
		((Control)DGridHardwareCodes).RightToLeft = (RightToLeft)1;
		((DataGridView)DGridHardwareCodes).RowHeadersVisible = false;
		((DataGridView)DGridHardwareCodes).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGridHardwareCodes).Size = new Size(463, 96);
		((Control)DGridHardwareCodes).TabIndex = 27;
		((DataGridViewColumn)DG_DeviceID).DataPropertyName = "DeviceId";
		((DataGridViewColumn)DG_DeviceID).HeaderText = "DeviceID";
		((DataGridViewColumn)DG_DeviceID).Name = "DG_DeviceID";
		((DataGridViewBand)DG_DeviceID).ReadOnly = true;
		((DataGridViewBand)DG_DeviceID).Visible = false;
		((DataGridViewColumn)DG_DeviceName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DG_DeviceName).DataPropertyName = "DeviceName";
		((DataGridViewColumn)DG_DeviceName).HeaderText = "[نام دستگاه]";
		((DataGridViewColumn)DG_DeviceName).MinimumWidth = 100;
		((DataGridViewColumn)DG_DeviceName).Name = "DG_DeviceName";
		((DataGridViewBand)DG_DeviceName).ReadOnly = true;
		((DataGridViewColumn)DG_PersonNO).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DG_PersonNO).DataPropertyName = "HardwareCode";
		((DataGridViewColumn)DG_PersonNO).HeaderText = "کد سخت افزاری";
		((DataGridViewColumn)DG_PersonNO).MinimumWidth = 30;
		((DataGridViewColumn)DG_PersonNO).Name = "DG_PersonNO";
		((DataGridViewBand)DG_PersonNO).ReadOnly = true;
		((DataGridViewColumn)DG_PersonNO).Width = 107;
		((DataGridViewColumn)DG_PersonId).DataPropertyName = "PersonId";
		((DataGridViewColumn)DG_PersonId).HeaderText = "PersonId";
		((DataGridViewColumn)DG_PersonId).Name = "DG_PersonId";
		((DataGridViewBand)DG_PersonId).ReadOnly = true;
		((DataGridViewBand)DG_PersonId).Visible = false;
		((DataGridViewColumn)DG_FullName).DataPropertyName = "FullName";
		((DataGridViewColumn)DG_FullName).HeaderText = "FullName";
		((DataGridViewColumn)DG_FullName).Name = "DG_FullName";
		((DataGridViewBand)DG_FullName).ReadOnly = true;
		((DataGridViewBand)DG_FullName).Visible = false;
		hardwareCodeCUSViewBindingSource.DataSource = typeof(HardwareCodeCUSView);
		TxtAdd.Border.Class = "TextBoxBorder";
		TxtAdd.Border.CornerType = (eCornerType)1;
		((Control)TxtAdd).Location = new Point(14, 208);
		((TextBoxBase)TxtAdd).MaxLength = 100;
		((Control)TxtAdd).Name = "TxtAdd";
		((Control)TxtAdd).Size = new Size(573, 21);
		((Control)TxtAdd).TabIndex = 11;
		TxtTel.Border.Class = "TextBoxBorder";
		TxtTel.Border.CornerType = (eCornerType)1;
		((Control)TxtTel).Location = new Point(131, 75);
		((TextBoxBase)TxtTel).MaxLength = 11;
		((Control)TxtTel).Name = "TxtTel";
		((Control)TxtTel).Size = new Size(147, 21);
		((Control)TxtTel).TabIndex = 5;
		((Control)TxtTel).KeyDown += new KeyEventHandler(TxtCodePersonel_KeyDown);
		((Control)labelX6).BackColor = Color.Transparent;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(593, 210);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).RightToLeft = (RightToLeft)1;
		((Control)labelX6).Size = new Size(99, 21);
		((Control)labelX6).TabIndex = 18;
		((Control)labelX6).Text = "[آدرس] :";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(285, 75);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).RightToLeft = (RightToLeft)1;
		((Control)labelX4).Size = new Size(125, 21);
		((Control)labelX4).TabIndex = 16;
		((Control)labelX4).Text = "[تلفن] :";
		labelX4.TextAlignment = (StringAlignment)2;
		TxtMob.Border.Class = "TextBoxBorder";
		TxtMob.Border.CornerType = (eCornerType)1;
		((Control)TxtMob).Location = new Point(131, 131);
		((TextBoxBase)TxtMob).MaxLength = 11;
		((Control)TxtMob).Name = "TxtMob";
		((Control)TxtMob).Size = new Size(147, 21);
		((Control)TxtMob).TabIndex = 10;
		((Control)TxtMob).KeyDown += new KeyEventHandler(TxtCodePersonel_KeyDown);
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(286, 131);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).RightToLeft = (RightToLeft)1;
		((Control)labelX3).Size = new Size(98, 21);
		((Control)labelX3).TabIndex = 14;
		((Control)labelX3).Text = "[موبایل] :";
		labelX3.TextAlignment = (StringAlignment)2;
		TxtFamily.Border.Class = "TextBoxBorder";
		TxtFamily.Border.CornerType = (eCornerType)1;
		((Control)TxtFamily).Location = new Point(131, 103);
		((TextBoxBase)TxtFamily).MaxLength = 50;
		((Control)TxtFamily).Name = "TxtFamily";
		((Control)TxtFamily).Size = new Size(147, 21);
		((Control)TxtFamily).TabIndex = 7;
		TxtLatinName.Border.Class = "TextBoxBorder";
		TxtLatinName.Border.CornerType = (eCornerType)1;
		((Control)TxtLatinName).Location = new Point(431, 131);
		((TextBoxBase)TxtLatinName).MaxLength = 50;
		((Control)TxtLatinName).Name = "TxtLatinName";
		((Control)TxtLatinName).Size = new Size(154, 21);
		((Control)TxtLatinName).TabIndex = 6;
		((Control)labelX33).BackColor = Color.Transparent;
		((BaseItemControl)labelX33).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX33).Location = new Point(592, 131);
		((Control)labelX33).Name = "labelX33";
		((Control)labelX33).RightToLeft = (RightToLeft)1;
		((Control)labelX33).Size = new Size(98, 21);
		((Control)labelX33).TabIndex = 10;
		((Control)labelX33).Text = "نام روی دستگاه :";
		labelX33.TextAlignment = (StringAlignment)2;
		TxtName.Border.Class = "TextBoxBorder";
		TxtName.Border.CornerType = (eCornerType)1;
		((Control)TxtName).Location = new Point(432, 103);
		((TextBoxBase)TxtName).MaxLength = 50;
		((Control)TxtName).Name = "TxtName";
		((Control)TxtName).Size = new Size(154, 21);
		((Control)TxtName).TabIndex = 6;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(593, 103);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).RightToLeft = (RightToLeft)1;
		((Control)labelX1).Size = new Size(98, 21);
		((Control)labelX1).TabIndex = 10;
		((Control)labelX1).Text = "[نام] :";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(285, 103);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).RightToLeft = (RightToLeft)1;
		((Control)labelX2).Size = new Size(125, 21);
		((Control)labelX2).TabIndex = 12;
		((Control)labelX2).Text = "نام خانوادگی*:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(594, 21);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).RightToLeft = (RightToLeft)1;
		((Control)labelX5).Size = new Size(98, 21);
		((Control)labelX5).TabIndex = 0;
		((Control)labelX5).Text = "[کد معین] * :";
		labelX5.TextAlignment = (StringAlignment)2;
		tabItem1.AttachedControl = (Control)(object)tabControlPanel1;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[اطلاعات عمومی]";
		((Control)tabControlPanel3).Anchor = (AnchorStyles)0;
		((Control)tabControlPanel3).Controls.Add((Control)(object)panelEx2);
		((PanelEx)tabControlPanel3).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel3).Location = new Point(0, 38);
		((Control)tabControlPanel3).Name = "tabControlPanel3";
		((Control)tabControlPanel3).Padding = new Padding(1);
		((Control)tabControlPanel3).Size = new Size(717, 429);
		((PanelEx)tabControlPanel3).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel3).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel3).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel3).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel3).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel3).Style.GradientAngle = 90;
		((Control)tabControlPanel3).TabIndex = 0;
		tabControlPanel3.TabItem = tabItem3;
		panelEx2.CanvasColor = SystemColors.Control;
		panelEx2.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx2).Controls.Add((Control)(object)farsiDateSWork);
		((Control)panelEx2).Controls.Add((Control)(object)DateTark);
		((Control)panelEx2).Controls.Add((Control)(object)FarsiDateBirthDay);
		((Control)panelEx2).Controls.Add((Control)(object)BtnDelEisar);
		((Control)panelEx2).Controls.Add((Control)(object)BtnMoreEisar);
		((Control)panelEx2).Controls.Add((Control)(object)CmbEisar);
		((Control)panelEx2).Controls.Add((Control)(object)labelX35);
		((Control)panelEx2).Controls.Add((Control)(object)ChbTark);
		((Control)panelEx2).Controls.Add((Control)(object)ChbSWork);
		((Control)panelEx2).Controls.Add((Control)(object)txtNoBimeh);
		((Control)panelEx2).Controls.Add((Control)(object)labelX19);
		((Control)panelEx2).Controls.Add((Control)(object)txtMadrak);
		((Control)panelEx2).Controls.Add((Control)(object)labelX42);
		((Control)panelEx2).Controls.Add((Control)(object)CMBJensiat);
		((Control)panelEx2).Controls.Add((Control)(object)labelX31);
		((Control)panelEx2).Controls.Add((Control)(object)TxtSerialSH);
		((Control)panelEx2).Controls.Add((Control)(object)labelX30);
		((Control)panelEx2).Controls.Add((Control)(object)labelX22);
		((Control)panelEx2).Controls.Add((Control)(object)TxtSHSH);
		((Control)panelEx2).Controls.Add((Control)(object)labelX20);
		((Control)panelEx2).Controls.Add((Control)(object)TxtZip);
		((Control)panelEx2).Controls.Add((Control)(object)TxtMSodor);
		((Control)panelEx2).Controls.Add((Control)(object)labelX28);
		((Control)panelEx2).Controls.Add((Control)(object)labelX26);
		((Control)panelEx2).Controls.Add((Control)(object)labelX24);
		((Control)panelEx2).Controls.Add((Control)(object)ChbBirth);
		((Control)panelEx2).Controls.Add((Control)(object)TxtNameF);
		((Control)panelEx2).Controls.Add((Control)(object)TxtCodeM);
		panelEx2.DisabledBackColor = Color.Empty;
		((Control)panelEx2).Dock = (DockStyle)5;
		((Control)panelEx2).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)panelEx2).Location = new Point(1, 1);
		((Control)panelEx2).Name = "panelEx2";
		((Control)panelEx2).Size = new Size(715, 427);
		panelEx2.Style.Alignment = (StringAlignment)1;
		panelEx2.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		panelEx2.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		panelEx2.Style.Border = (eBorderType)1;
		panelEx2.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		panelEx2.Style.BorderWidth = 3;
		panelEx2.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		panelEx2.Style.GradientAngle = 90;
		((Control)panelEx2).TabIndex = 0;
		farsiDateSWork.AllowNull = false;
		((Control)farsiDateSWork).Anchor = (AnchorStyles)9;
		farsiDateSWork.BackcolorTooltip = Color.Red;
		((TextBoxX)farsiDateSWork).Border.Class = "TextBoxBorder";
		((TextBoxX)farsiDateSWork).Border.CornerType = (eCornerType)1;
		farsiDateSWork.EnterToTab = true;
		farsiDateSWork.EscToClose = true;
		farsiDateSWork.FarsyDateStyle = (ENFarsyDateStyle)1;
		farsiDateSWork.FocusBackColor = Color.Yellow;
		farsiDateSWork.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		farsiDateSWork.FocusForeColor = Color.Blue;
		farsiDateSWork.FocusTextSelect = true;
		((Control)farsiDateSWork).Font = new Font("Times New Roman", 12f);
		((Control)farsiDateSWork).Location = new Point(409, 171);
		farsiDateSWork.MessageEmptyShowDialog = false;
		farsiDateSWork.MessegeEmpty = "";
		farsiDateSWork.MessegeEmptyInFormRight = true;
		farsiDateSWork.MessegeEmptyShowInForm = false;
		farsiDateSWork.MiladiDate = new DateTime(2025, 9, 3, 21, 50, 2, 417);
		((Control)farsiDateSWork).Name = "farsiDateSWork";
		((Control)farsiDateSWork).RightToLeft = (RightToLeft)0;
		farsiDateSWork.ShowNameOfDayOnToolTip = false;
		farsiDateSWork.ShowToolTipText = false;
		((Control)farsiDateSWork).Size = new Size(155, 26);
		((Control)farsiDateSWork).TabIndex = 12;
		((Control)farsiDateSWork).Text = "03/09/2025";
		((TextBox)farsiDateSWork).TextAlign = (HorizontalAlignment)2;
		farsiDateSWork.TootTipDelay = 500;
		DateTark.AllowNull = false;
		DateTark.BackcolorTooltip = Color.Red;
		((TextBoxX)DateTark).Border.Class = "TextBoxBorder";
		((TextBoxX)DateTark).Border.CornerType = (eCornerType)1;
		DateTark.EnterToTab = true;
		DateTark.EscToClose = true;
		DateTark.FarsyDateStyle = (ENFarsyDateStyle)1;
		DateTark.FocusBackColor = Color.Yellow;
		DateTark.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		DateTark.FocusForeColor = Color.Blue;
		DateTark.FocusTextSelect = true;
		((Control)DateTark).Font = new Font("Times New Roman", 12f);
		((Control)DateTark).Location = new Point(40, 171);
		DateTark.MessageEmptyShowDialog = false;
		DateTark.MessegeEmpty = "";
		DateTark.MessegeEmptyInFormRight = true;
		DateTark.MessegeEmptyShowInForm = false;
		DateTark.MiladiDate = new DateTime(2025, 9, 3, 21, 50, 3, 419);
		((Control)DateTark).Name = "DateTark";
		((Control)DateTark).RightToLeft = (RightToLeft)0;
		DateTark.ShowNameOfDayOnToolTip = false;
		DateTark.ShowToolTipText = false;
		((Control)DateTark).Size = new Size(155, 26);
		((Control)DateTark).TabIndex = 14;
		((Control)DateTark).Text = "03/09/2025";
		((TextBox)DateTark).TextAlign = (HorizontalAlignment)2;
		DateTark.TootTipDelay = 500;
		FarsiDateBirthDay.AllowNull = false;
		FarsiDateBirthDay.BackcolorTooltip = Color.Red;
		((TextBoxX)FarsiDateBirthDay).Border.Class = "TextBoxBorder";
		((TextBoxX)FarsiDateBirthDay).Border.CornerType = (eCornerType)1;
		FarsiDateBirthDay.EnterToTab = true;
		FarsiDateBirthDay.EscToClose = true;
		FarsiDateBirthDay.FarsyDateStyle = (ENFarsyDateStyle)1;
		FarsiDateBirthDay.FocusBackColor = Color.Yellow;
		FarsiDateBirthDay.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		FarsiDateBirthDay.FocusForeColor = Color.Blue;
		FarsiDateBirthDay.FocusTextSelect = true;
		((Control)FarsiDateBirthDay).Font = new Font("Times New Roman", 12f);
		((Control)FarsiDateBirthDay).Location = new Point(40, 139);
		FarsiDateBirthDay.MessageEmptyShowDialog = false;
		FarsiDateBirthDay.MessegeEmpty = "";
		FarsiDateBirthDay.MessegeEmptyInFormRight = true;
		FarsiDateBirthDay.MessegeEmptyShowInForm = false;
		FarsiDateBirthDay.MiladiDate = new DateTime(2025, 9, 3, 21, 50, 3, 421);
		((Control)FarsiDateBirthDay).Name = "FarsiDateBirthDay";
		((Control)FarsiDateBirthDay).RightToLeft = (RightToLeft)0;
		FarsiDateBirthDay.ShowNameOfDayOnToolTip = false;
		FarsiDateBirthDay.ShowToolTipText = false;
		((Control)FarsiDateBirthDay).Size = new Size(155, 26);
		((Control)FarsiDateBirthDay).TabIndex = 10;
		((Control)FarsiDateBirthDay).Text = "03/09/2025";
		((TextBox)FarsiDateBirthDay).TextAlign = (HorizontalAlignment)2;
		FarsiDateBirthDay.TootTipDelay = 500;
		((Control)BtnDelEisar).AccessibleRole = (AccessibleRole)43;
		BtnDelEisar.ColorTable = (eButtonColor)3;
		BtnDelEisar.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelEisar).Location = new Point(40, 201);
		((Control)BtnDelEisar).Name = "BtnDelEisar";
		((Control)BtnDelEisar).Size = new Size(27, 21);
		((PopupItemControl)BtnDelEisar).Style = (eDotNetBarStyle)9;
		((Control)BtnDelEisar).TabIndex = 27;
		((Control)BtnDelEisar).TabStop = false;
		((Control)BtnDelEisar).Click += BtnDelEisar_Click;
		((Control)BtnMoreEisar).AccessibleRole = (AccessibleRole)43;
		BtnMoreEisar.ColorTable = (eButtonColor)6;
		((Control)BtnMoreEisar).Location = new Point(75, 201);
		((Control)BtnMoreEisar).Name = "BtnMoreEisar";
		((Control)BtnMoreEisar).Size = new Size(27, 21);
		((PopupItemControl)BtnMoreEisar).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreEisar).TabIndex = 26;
		((Control)BtnMoreEisar).TabStop = false;
		((Control)BtnMoreEisar).Text = "...";
		((Control)BtnMoreEisar).Click += BtnMoreEisar_Click;
		((Control)CmbEisar).Anchor = (AnchorStyles)13;
		((ListControl)CmbEisar).DisplayMember = "Name";
		((ComboBox)CmbEisar).DrawMode = (DrawMode)1;
		((ComboBox)CmbEisar).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbEisar).FormattingEnabled = true;
		((ComboBox)CmbEisar).ItemHeight = 16;
		((Control)CmbEisar).Location = new Point(113, 201);
		((Control)CmbEisar).Name = "CmbEisar";
		((Control)CmbEisar).Size = new Size(451, 22);
		CmbEisar.Style = (eDotNetBarStyle)9;
		((Control)CmbEisar).TabIndex = 15;
		((ListControl)CmbEisar).ValueMember = "Id";
		((Control)labelX35).Anchor = (AnchorStyles)9;
		((Control)labelX35).BackColor = Color.Transparent;
		((BaseItemControl)labelX35).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX35).Location = new Point(571, 201);
		((Control)labelX35).Name = "labelX35";
		((Control)labelX35).RightToLeft = (RightToLeft)1;
		((Control)labelX35).Size = new Size(125, 21);
		((Control)labelX35).TabIndex = 24;
		((Control)labelX35).Text = "[ایثارگری] :";
		labelX35.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChbTark).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbTark).Location = new Point(196, 177);
		((Control)ChbTark).Name = "ChbTark";
		((Control)ChbTark).Size = new Size(92, 16);
		((BaseItemControl)ChbTark).Style = (eDotNetBarStyle)9;
		((Control)ChbTark).TabIndex = 13;
		((Control)ChbTark).Text = ":تاریخ ترک کار";
		ChbTark.CheckedChanged += ChbTark_CheckedChanged;
		((Control)ChbSWork).Anchor = (AnchorStyles)9;
		((BaseItemControl)ChbSWork).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbSWork).Location = new Point(567, 173);
		((Control)ChbSWork).Name = "ChbSWork";
		((Control)ChbSWork).Size = new Size(116, 21);
		((BaseItemControl)ChbSWork).Style = (eDotNetBarStyle)9;
		((Control)ChbSWork).TabIndex = 11;
		((Control)ChbSWork).Text = ":تاریخ شروع به کار";
		ChbSWork.CheckedChanged += ChbSWork_CheckedChanged;
		((Control)txtNoBimeh).Anchor = (AnchorStyles)9;
		txtNoBimeh.Border.Class = "TextBoxBorder";
		txtNoBimeh.Border.CornerType = (eCornerType)1;
		((Control)txtNoBimeh).Location = new Point(410, 143);
		((TextBoxBase)txtNoBimeh).MaxLength = 10;
		((Control)txtNoBimeh).Name = "txtNoBimeh";
		((Control)txtNoBimeh).Size = new Size(154, 21);
		((Control)txtNoBimeh).TabIndex = 8;
		((Control)labelX19).Anchor = (AnchorStyles)9;
		((Control)labelX19).BackColor = Color.Transparent;
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Location = new Point(571, 142);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).RightToLeft = (RightToLeft)1;
		((Control)labelX19).Size = new Size(134, 21);
		((Control)labelX19).TabIndex = 16;
		((Control)labelX19).Text = "[شماره بیمه] :";
		labelX19.TextAlignment = (StringAlignment)2;
		txtMadrak.Border.Class = "TextBoxBorder";
		txtMadrak.Border.CornerType = (eCornerType)1;
		((Control)txtMadrak).Location = new Point(40, 112);
		((Control)txtMadrak).Name = "txtMadrak";
		((Control)txtMadrak).Size = new Size(154, 21);
		((Control)txtMadrak).TabIndex = 7;
		((Control)labelX42).BackColor = Color.Transparent;
		((BaseItemControl)labelX42).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX42).Location = new Point(201, 112);
		((Control)labelX42).Name = "labelX42";
		((Control)labelX42).RightToLeft = (RightToLeft)1;
		((Control)labelX42).Size = new Size(141, 21);
		((Control)labelX42).TabIndex = 18;
		((Control)labelX42).Text = "[مدرک تحصیلی] :";
		labelX42.TextAlignment = (StringAlignment)2;
		((Control)CMBJensiat).Anchor = (AnchorStyles)9;
		((ListControl)CMBJensiat).DisplayMember = "Text";
		((ComboBox)CMBJensiat).DrawMode = (DrawMode)1;
		((ComboBox)CMBJensiat).DropDownStyle = (ComboBoxStyle)2;
		((Control)CMBJensiat).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((ListControl)CMBJensiat).FormattingEnabled = true;
		((ComboBox)CMBJensiat).ItemHeight = 15;
		CMBJensiat.Items.AddRange(new object[3] { CmbUnknown, CmbMale1, CmbFamle });
		((Control)CMBJensiat).Location = new Point(410, 115);
		((Control)CMBJensiat).Name = "CMBJensiat";
		((Control)CMBJensiat).Size = new Size(153, 21);
		CMBJensiat.Style = (eDotNetBarStyle)9;
		((Control)CMBJensiat).TabIndex = 6;
		CmbUnknown.Text = "------";
		CmbUnknown.TextAlignment = (StringAlignment)2;
		CmbUnknown.TextLineAlignment = (StringAlignment)2;
		CmbMale1.FontName = "Tahoma";
		CmbMale1.FontStyle = (FontStyle)1;
		CmbMale1.Text = "[مرد]";
		CmbMale1.TextAlignment = (StringAlignment)2;
		CmbMale1.TextLineAlignment = (StringAlignment)2;
		CmbFamle.FontName = "Tahoma";
		CmbFamle.FontStyle = (FontStyle)1;
		CmbFamle.Text = "[زن]";
		CmbFamle.TextAlignment = (StringAlignment)2;
		CmbFamle.TextLineAlignment = (StringAlignment)2;
		((Control)labelX31).Anchor = (AnchorStyles)9;
		((Control)labelX31).BackColor = Color.Transparent;
		((BaseItemControl)labelX31).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX31).Location = new Point(571, 114);
		((Control)labelX31).Name = "labelX31";
		((Control)labelX31).RightToLeft = (RightToLeft)1;
		((Control)labelX31).Size = new Size(125, 21);
		((Control)labelX31).TabIndex = 12;
		((Control)labelX31).Text = "[جنسیت] :";
		labelX31.TextAlignment = (StringAlignment)2;
		TxtSerialSH.Border.Class = "TextBoxBorder";
		TxtSerialSH.Border.CornerType = (eCornerType)1;
		((Control)TxtSerialSH).Location = new Point(40, 85);
		((TextBoxBase)TxtSerialSH).MaxLength = 10;
		((Control)TxtSerialSH).Name = "TxtSerialSH";
		((Control)TxtSerialSH).Size = new Size(154, 21);
		((Control)TxtSerialSH).TabIndex = 5;
		((Control)labelX30).BackColor = Color.Transparent;
		((BaseItemControl)labelX30).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX30).Location = new Point(201, 85);
		((Control)labelX30).Name = "labelX30";
		((Control)labelX30).RightToLeft = (RightToLeft)1;
		((Control)labelX30).Size = new Size(141, 21);
		((Control)labelX30).TabIndex = 10;
		((Control)labelX30).Text = "[سریال شناسنامه] :";
		labelX30.TextAlignment = (StringAlignment)2;
		((Control)labelX22).Anchor = (AnchorStyles)9;
		((Control)labelX22).BackColor = Color.Transparent;
		((BaseItemControl)labelX22).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX22).Location = new Point(571, 58);
		((Control)labelX22).Name = "labelX22";
		((Control)labelX22).RightToLeft = (RightToLeft)1;
		((Control)labelX22).Size = new Size(125, 21);
		((Control)labelX22).TabIndex = 4;
		((Control)labelX22).Text = "[شماره شناسنامه]:";
		labelX22.TextAlignment = (StringAlignment)2;
		((Control)TxtSHSH).Anchor = (AnchorStyles)9;
		TxtSHSH.Border.Class = "TextBoxBorder";
		TxtSHSH.Border.CornerType = (eCornerType)1;
		((Control)TxtSHSH).Location = new Point(410, 59);
		((TextBoxBase)TxtSHSH).MaxLength = 10;
		((Control)TxtSHSH).Name = "TxtSHSH";
		((Control)TxtSHSH).Size = new Size(154, 21);
		((Control)TxtSHSH).TabIndex = 2;
		((Control)labelX20).BackColor = Color.Transparent;
		((BaseItemControl)labelX20).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX20).Location = new Point(201, 31);
		((Control)labelX20).Name = "labelX20";
		((Control)labelX20).RightToLeft = (RightToLeft)1;
		((Control)labelX20).Size = new Size(141, 21);
		((Control)labelX20).TabIndex = 1;
		((Control)labelX20).Text = "[کد پستی] :";
		labelX20.TextAlignment = (StringAlignment)2;
		TxtZip.Border.Class = "TextBoxBorder";
		TxtZip.Border.CornerType = (eCornerType)1;
		((Control)TxtZip).Location = new Point(40, 31);
		((TextBoxBase)TxtZip).MaxLength = 10;
		((Control)TxtZip).Name = "TxtZip";
		((Control)TxtZip).Size = new Size(154, 21);
		((Control)TxtZip).TabIndex = 1;
		TxtMSodor.Border.Class = "TextBoxBorder";
		TxtMSodor.Border.CornerType = (eCornerType)1;
		((Control)TxtMSodor).Location = new Point(40, 58);
		((Control)TxtMSodor).Name = "TxtMSodor";
		((Control)TxtMSodor).Size = new Size(154, 21);
		((Control)TxtMSodor).TabIndex = 3;
		((Control)labelX28).BackColor = Color.Transparent;
		((BaseItemControl)labelX28).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX28).Location = new Point(201, 58);
		((Control)labelX28).Name = "labelX28";
		((Control)labelX28).RightToLeft = (RightToLeft)1;
		((Control)labelX28).Size = new Size(141, 21);
		((Control)labelX28).TabIndex = 6;
		((Control)labelX28).Text = "[محل صدور]  :";
		labelX28.TextAlignment = (StringAlignment)2;
		((Control)labelX26).Anchor = (AnchorStyles)9;
		((Control)labelX26).BackColor = Color.Transparent;
		((BaseItemControl)labelX26).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX26).Location = new Point(571, 86);
		((Control)labelX26).Name = "labelX26";
		((Control)labelX26).RightToLeft = (RightToLeft)1;
		((Control)labelX26).Size = new Size(125, 21);
		((Control)labelX26).TabIndex = 8;
		((Control)labelX26).Text = "[نام پدر] :";
		labelX26.TextAlignment = (StringAlignment)2;
		((Control)labelX24).Anchor = (AnchorStyles)9;
		((Control)labelX24).BackColor = Color.Transparent;
		((BaseItemControl)labelX24).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX24).Location = new Point(571, 30);
		((Control)labelX24).Name = "labelX24";
		((Control)labelX24).RightToLeft = (RightToLeft)1;
		((Control)labelX24).Size = new Size(125, 21);
		((Control)labelX24).TabIndex = 0;
		((Control)labelX24).Text = "[کد ملی] :";
		labelX24.TextAlignment = (StringAlignment)2;
		((BaseItemControl)ChbBirth).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbBirth).Location = new Point(195, 145);
		((Control)ChbBirth).Name = "ChbBirth";
		((Control)ChbBirth).Size = new Size(75, 16);
		((BaseItemControl)ChbBirth).Style = (eDotNetBarStyle)9;
		((Control)ChbBirth).TabIndex = 9;
		((Control)ChbBirth).Text = ":تاریخ تولد";
		ChbBirth.CheckedChanged += ChbBirth_CheckedChanged;
		((Control)TxtNameF).Anchor = (AnchorStyles)9;
		TxtNameF.Border.Class = "TextBoxBorder";
		TxtNameF.Border.CornerType = (eCornerType)1;
		((Control)TxtNameF).Location = new Point(410, 87);
		((Control)TxtNameF).Name = "TxtNameF";
		((Control)TxtNameF).Size = new Size(154, 21);
		((Control)TxtNameF).TabIndex = 4;
		((Control)TxtCodeM).Anchor = (AnchorStyles)9;
		TxtCodeM.Border.Class = "TextBoxBorder";
		TxtCodeM.Border.CornerType = (eCornerType)1;
		((Control)TxtCodeM).Location = new Point(410, 31);
		((TextBoxBase)TxtCodeM).MaxLength = 10;
		((Control)TxtCodeM).Name = "TxtCodeM";
		((Control)TxtCodeM).Size = new Size(154, 21);
		((Control)TxtCodeM).TabIndex = 0;
		tabItem3.AttachedControl = (Control)(object)tabControlPanel3;
		tabItem3.Name = "tabItem3";
		tabItem3.Text = "[اطلاعات تکمیلی]";
		tabItem2.AttachedControl = (Control)(object)tabControlPanel2;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "[اطلاعات تکمیلی]";
		((Control)tabControlPanel2).Controls.Add((Control)(object)groupPanel3);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX8);
		((Control)tabControlPanel2).Controls.Add((Control)(object)buttonX1);
		((Control)tabControlPanel2).Controls.Add((Control)(object)textBoxX1);
		((PanelEx)tabControlPanel2).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel2).Dock = (DockStyle)5;
		((Control)tabControlPanel2).Location = new Point(0, 26);
		((Control)tabControlPanel2).Name = "tabControlPanel2";
		((Control)tabControlPanel2).Padding = new Padding(1);
		((Control)tabControlPanel2).Size = new Size(575, 375);
		((PanelEx)tabControlPanel2).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel2).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel2).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel2).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel2).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel2).Style.GradientAngle = 90;
		((Control)tabControlPanel2).TabIndex = 1;
		tabControlPanel2.TabItem = tabItem2;
		((Control)groupPanel3).Anchor = (AnchorStyles)13;
		((Control)groupPanel3).BackColor = Color.Transparent;
		((PanelControl)groupPanel3).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanel3).ColorSchemeStyle = (eDotNetBarStyle)4;
		groupPanel3.DisabledBackColor = Color.Empty;
		((Control)groupPanel3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)groupPanel3).Location = new Point(12, 122);
		((Control)groupPanel3).Name = "groupPanel3";
		((Control)groupPanel3).Size = new Size(554, 235);
		((PanelControl)groupPanel3).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanel3).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanel3).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanel3).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderBottomWidth = 4;
		((PanelControl)groupPanel3).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanel3).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderLeftWidth = 4;
		((PanelControl)groupPanel3).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderRightWidth = 4;
		((PanelControl)groupPanel3).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanel3).Style.BorderTopWidth = 4;
		((PanelControl)groupPanel3).Style.CornerDiameter = 4;
		((PanelControl)groupPanel3).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanel3).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanel3).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanel3).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanel3).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanel3).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanel3).TabIndex = 3;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(420, 95);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(40, 23);
		((Control)labelX8).TabIndex = 0;
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)buttonX1).AccessibleRole = (AccessibleRole)43;
		buttonX1.ColorTable = (eButtonColor)3;
		((Control)buttonX1).Location = new Point(14, 14);
		((Control)buttonX1).Name = "buttonX1";
		((Control)buttonX1).Size = new Size(23, 21);
		((PopupItemControl)buttonX1).Style = (eDotNetBarStyle)9;
		((Control)buttonX1).TabIndex = 1;
		((Control)buttonX1).TabStop = false;
		textBoxX1.Border.Class = "TextBoxBorder";
		textBoxX1.Border.CornerType = (eCornerType)1;
		((Control)textBoxX1).Location = new Point(36, 95);
		((Control)textBoxX1).Name = "textBoxX1";
		((Control)textBoxX1).Size = new Size(378, 20);
		((Control)textBoxX1).TabIndex = 1;
		((Control)textBoxX1).Enter += TxtNameFather_Enter;
		((Control)textBoxX1).Leave += textBoxX1_Leave;
		((Control)labelX18).BackColor = Color.Transparent;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Location = new Point(154, 34);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(120, 23);
		((Control)labelX18).TabIndex = 10;
		labelX18.TextAlignment = (StringAlignment)2;
		((Control)labelX17).Anchor = (AnchorStyles)9;
		((Control)labelX17).BackColor = Color.Transparent;
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).Location = new Point(435, 35);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(53, 23);
		((Control)labelX17).TabIndex = 2;
		labelX17.TextAlignment = (StringAlignment)2;
		((Control)labelX16).Anchor = (AnchorStyles)9;
		((Control)labelX16).BackColor = Color.Transparent;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).Location = new Point(435, 91);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(61, 23);
		((Control)labelX16).TabIndex = 6;
		labelX16.TextAlignment = (StringAlignment)2;
		((Control)labelX15).Anchor = (AnchorStyles)9;
		((Control)labelX15).BackColor = Color.Transparent;
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).Location = new Point(435, 62);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).Size = new Size(48, 23);
		((Control)labelX15).TabIndex = 4;
		labelX15.TextAlignment = (StringAlignment)2;
		((Control)labelX14).Anchor = (AnchorStyles)9;
		((Control)labelX14).BackColor = Color.Transparent;
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).Location = new Point(154, 94);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(87, 23);
		((Control)labelX14).TabIndex = 14;
		labelX14.TextAlignment = (StringAlignment)2;
		TxtNumberShenasname.Border.Class = "TextBoxBorder";
		TxtNumberShenasname.Border.CornerType = (eCornerType)1;
		((Control)TxtNumberShenasname).Location = new Point(200, 30);
		((Control)TxtNumberShenasname).Name = "TxtNumberShenasname";
		((Control)TxtNumberShenasname).Size = new Size(135, 21);
		((Control)TxtNumberShenasname).TabIndex = 11;
		((Control)TxtNumberShenasname).Visible = false;
		((Control)TxtNumberShenasname).Enter += TxtNameFather_Enter;
		((Control)TxtNumberShenasname).Leave += textBoxX1_Leave;
		((Control)TxtCodeMeli).Anchor = (AnchorStyles)9;
		TxtCodeMeli.Border.Class = "TextBoxBorder";
		TxtCodeMeli.Border.CornerType = (eCornerType)1;
		((Control)TxtCodeMeli).Location = new Point(294, 35);
		((Control)TxtCodeMeli).Name = "TxtCodeMeli";
		((Control)TxtCodeMeli).Size = new Size(135, 21);
		((Control)TxtCodeMeli).TabIndex = 3;
		((Control)TxtCodeMeli).Enter += TxtNameFather_Enter;
		((Control)TxtCodeMeli).Leave += textBoxX1_Leave;
		((Control)TxtNameFather).Anchor = (AnchorStyles)9;
		TxtNameFather.Border.Class = "TextBoxBorder";
		TxtNameFather.Border.CornerType = (eCornerType)1;
		((Control)TxtNameFather).Location = new Point(294, 62);
		((Control)TxtNameFather).Name = "TxtNameFather";
		((Control)TxtNameFather).Size = new Size(135, 21);
		((Control)TxtNameFather).TabIndex = 5;
		((Control)TxtNameFather).Enter += TxtNameFather_Enter;
		((Control)TxtNameFather).Leave += textBoxX1_Leave;
		((Control)CmbSexual).Anchor = (AnchorStyles)9;
		CmbSexual.DisableInternalDrawing = true;
		((ListControl)CmbSexual).DisplayMember = "Text";
		((ComboBox)CmbSexual).DrawMode = (DrawMode)1;
		((ComboBox)CmbSexual).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbSexual).FormattingEnabled = true;
		((ComboBox)CmbSexual).ItemHeight = 15;
		((Control)CmbSexual).Location = new Point(294, 89);
		((Control)CmbSexual).Name = "CmbSexual";
		((Control)CmbSexual).Size = new Size(135, 21);
		CmbSexual.Style = (eDotNetBarStyle)9;
		((Control)CmbSexual).TabIndex = 7;
		CmbFemale.FontSize = 20f;
		CmbFemale.FontStyle = (FontStyle)1;
		CmbFemale.Text = "[Zan]";
		CmbFemale.TextAlignment = (StringAlignment)2;
		CmbMale.FontSize = 20f;
		CmbMale.FontStyle = (FontStyle)1;
		CmbMale.Text = "[مرد]";
		CmbMale.TextAlignment = (StringAlignment)2;
		CmbMariage.DisableInternalDrawing = true;
		((ListControl)CmbMariage).DisplayMember = "Text";
		((ComboBox)CmbMariage).DrawMode = (DrawMode)1;
		((ComboBox)CmbMariage).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbMariage).FormattingEnabled = true;
		((ComboBox)CmbMariage).ItemHeight = 15;
		((Control)CmbMariage).Location = new Point(13, 94);
		((Control)CmbMariage).Name = "CmbMariage";
		((Control)CmbMariage).Size = new Size(135, 21);
		CmbMariage.Style = (eDotNetBarStyle)9;
		((Control)CmbMariage).TabIndex = 15;
		CmbNaMalom.FontSize = 15f;
		CmbNaMalom.FontStyle = (FontStyle)1;
		CmbNaMalom.Text = "-----";
		CmbNaMalom.TextAlignment = (StringAlignment)2;
		CmbSingel.FontSize = 15f;
		CmbSingel.FontStyle = (FontStyle)1;
		CmbSingel.Text = "[مجرد]";
		CmbSingel.TextAlignment = (StringAlignment)2;
		CmbMarried1.FontSize = 15f;
		CmbMarried1.FontStyle = (FontStyle)1;
		CmbMarried1.Text = "[متاهل]";
		CmbMarried1.TextAlignment = (StringAlignment)2;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(435, 120);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(88, 23);
		((Control)labelX10).TabIndex = 8;
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)labelX9).Anchor = (AnchorStyles)9;
		((Control)labelX9).BackColor = Color.Transparent;
		((BaseItemControl)labelX9).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX9).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX9).Location = new Point(435, 6);
		((Control)labelX9).Name = "labelX9";
		((Control)labelX9).Size = new Size(95, 23);
		((Control)labelX9).TabIndex = 0;
		labelX9.TextAlignment = (StringAlignment)2;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).Location = new Point(154, 64);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(70, 23);
		((Control)labelX13).TabIndex = 12;
		labelX13.TextAlignment = (StringAlignment)2;
		((Control)TxtBimeNum).Anchor = (AnchorStyles)9;
		TxtBimeNum.Border.Class = "TextBoxBorder";
		TxtBimeNum.Border.CornerType = (eCornerType)1;
		((Control)TxtBimeNum).Location = new Point(294, 8);
		((Control)TxtBimeNum).Name = "TxtBimeNum";
		((Control)TxtBimeNum).Size = new Size(135, 21);
		((Control)TxtBimeNum).TabIndex = 1;
		((Control)TxtBimeNum).Visible = false;
		((Control)TxtBimeNum).Enter += TxtNameFather_Enter;
		((Control)TxtBimeNum).Leave += textBoxX1_Leave;
		((Control)numerichild).Anchor = (AnchorStyles)9;
		((Control)numerichild).Location = new Point(294, 119);
		numerichild.Maximum = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)numerichild).Name = "numerichild";
		((Control)numerichild).Size = new Size(135, 20);
		((Control)numerichild).TabIndex = 9;
		((FileDialog)FDFace).FileName = "openFileDialog1";
		((Control)labelX34).BackColor = Color.Transparent;
		((BaseItemControl)labelX34).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX34).Location = new Point(286, 158);
		((Control)labelX34).Name = "labelX34";
		((Control)labelX34).RightToLeft = (RightToLeft)1;
		((Control)labelX34).Size = new Size(98, 21);
		((Control)labelX34).TabIndex = 14;
		((Control)labelX34).Text = "رمز پرتال :";
		labelX34.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(721, 537);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmPersonel";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[کارکنان]*";
		((Form)this).Load += FRMPersonel_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMPersonel_KeyDown);
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)tabControl1).EndInit();
		tabControl1.ResumeLayout(false);
		((Control)tabControlPanel1).ResumeLayout(false);
		((Control)groupPanel7).ResumeLayout(false);
		((ISupportInitialize)DepartmanbindingSource).EndInit();
		((ISupportInitialize)PicPerson).EndInit();
		((ISupportInitialize)personTitleBindingSource).EndInit();
		((ISupportInitialize)personGroupBindingSource).EndInit();
		((ISupportInitialize)DGridHardwareCodes).EndInit();
		((ISupportInitialize)hardwareCodeCUSViewBindingSource).EndInit();
		((Control)tabControlPanel3).ResumeLayout(false);
		((Control)panelEx2).ResumeLayout(false);
		((ISupportInitialize)companyBindingSource).EndInit();
		((ISupportInitialize)bankHesabTypeBindingSource).EndInit();
		((ISupportInitialize)perKhedmatPlaceBindingSource).EndInit();
		((ISupportInitialize)perTypeEstekhdamBindingSource).EndInit();
		((ISupportInitialize)perEisarBindingSource).EndInit();
		((ISupportInitialize)perKhedmatBindingSource).EndInit();
		((ISupportInitialize)perPartBindingSource).EndInit();
		((ISupportInitialize)bankBindingSource).EndInit();
		((ISupportInitialize)maliYearBindingSource).EndInit();
		((ISupportInitialize)shiftsBindingSource).EndInit();
		((ISupportInitialize)perWorkTitleBindingSource).EndInit();
		((Control)tabControlPanel2).ResumeLayout(false);
		((ISupportInitialize)numerichild).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
