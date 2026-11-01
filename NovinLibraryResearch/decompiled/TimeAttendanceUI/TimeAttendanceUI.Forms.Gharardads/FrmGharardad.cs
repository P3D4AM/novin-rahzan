using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;
using FormCustom;
using FormCustom.Cache;
using FormCustom.CurrencyTextBox;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.FarsiMessageBox;
using TimeAttendanceUI.Forms.Bimeh;
using TimeAttendanceUI.Forms.Person;
using TimeAttendanceUI.Forms.Person.Bank;
using TimeAttendanceUI.Forms.Person.Khedmat;
using TimeAttendanceUI.Forms.Person.KhedmatPlace;
using TimeAttendanceUI.Forms.Person.Part;
using TimeAttendanceUI.Forms.Person.TypeBankHesab;
using TimeAttendanceUI.Forms.Person.TypeEstekhdam;
using TimeAttendanceUI.Forms.Person.Work;
using TimeAttendanceUI.Properties;
using TimeAttendanceUI.Salary.Forms.Codeing;
using TimeAttendanceUI.Salary.Shift;
using TimeAttendanceUI.Validators;

namespace TimeAttendanceUI.Forms.Gharardads;

public class FrmGharardad : FormExtention
{
	private Font _font;

	private GharardadNew _clsGharardad;

	private TimeAttendanceDAL.Dal.FLRPerson.Person _clsPerson;

	private decimal _sumkosor;

	private decimal _sumMazaya;

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private List<Company> _listData;

	private SortableBindingList<Company> _listForDisplay;

	private CancellationTokenSource _loadDataToken;

	private CancellationTokenSource _searchToken;

	private CancellationTokenSource _changeToken;

	private bool _isDisposed;

	private IContainer components;

	private PanelEx panelEx1;

	private ButtonX BtnCancel;

	private ButtonX BtnAccept;

	private TabControl tabControl1;

	private TabControlPanel tabControlPanel1;

	private TabItem tabItem1;

	private TabControlPanel tabControlPanel2;

	private LabelX LblPriceAbsent;

	private LabelX LblPriceNobatkari1;

	private LabelX LblPriceNight;

	private LabelX LblPriceHoliday;

	private LabelX LblPriceLeave;

	private LabelX LblPriceOverTime;

	private LabelX LblPriceLowTime;

	private TabItem tabItem2;

	private TabControlPanel tabControlPanel3;

	internal ButtonX BtnDelPlace;

	internal ButtonX BtnDelRecruitmentType;

	internal ButtonX BtnDelRecruitmentST;

	internal ButtonX BtnDelPart;

	internal ButtonX BtnDelBank;

	internal ButtonX BtnMorePlace;

	private ComboBoxEx CmbPlace;

	private LabelX labelX39;

	internal ButtonX BtnMoreRecruitmentType;

	private ComboBoxEx CmbRecruitmentType;

	private LabelX labelX38;

	private LabelX labelX36;

	internal ButtonX BtnMoreRecruitmentST;

	private ComboBoxEx CmbRecruitmentST;

	internal ButtonX BtnMorePart;

	private ComboBoxEx CmbPart;

	private LabelX labelX35;

	private LabelX labelX32;

	internal ButtonX BtnMorebank;

	private ComboBoxEx CmbBank;

	private LabelX labelX21;

	private TabItem tabItem3;

	private LabelX labelX31;

	private LabelX labelX40;

	private NumericUpDown NumberCountChild;

	private LabelX labelX6;

	private LabelX labelX20;

	private ComboBoxEx CmbSalaryType;

	private LabelX labelX4;

	private LabelX labelX19;

	internal NumericUpDown numberCod;

	private ButtonX BtnmoreCompany;

	private LabelX lblname;

	private ButtonX BtnMoreP;

	private TextBoxX TxtcodePerson;

	internal ButtonX BTNMore;

	internal DateBoxFa TxtEndDate;

	internal DateBoxFa TXTStartDate;

	internal LabelX labelX8;

	internal LabelX labelX10;

	private ComboBoxEx CmbShift;

	private ComboBoxEx CmbCompany;

	internal DateBoxFa TXTDateRegister;

	private LabelX labelX7;

	private TextBoxX TxtDesc;

	private TextBoxX TxtPeyvast;

	private LabelX labelX1;

	private LabelX labelX2;

	private LabelX labelX3;

	private LabelX labelX5;

	private LabelX labelX11;

	private LabelX labelX12;

	internal ButtonX BtnDelBankType;

	internal ButtonX BtnMorebankType;

	private ComboBoxEx CmbMariage;

	private ComboItem CmbMarried;

	private ComboItem CmbSingle;

	private ComboBoxEx CmbBankType;

	private TabControlPanel tabControlPanel4;

	private LabelX LblMandeh;

	private LabelX labelX13;

	private LabelX LblSumkosor;

	private ButtonX BTNADDMazaya;

	private LabelX labelX14;

	private ButtonX BTNDelete;

	private LabelX LblSumMazaya;

	private ButtonX BTNAddKosorat;

	private LabelX labelX15;

	private ButtonX BTNEdit;

	private TabItem tabItem4;

	private TextBoxFa TxtPriceLowTime;

	private TextBoxFa TxtPriceLeave;

	private TextBoxFa TxtPriceNobatkari1;

	private TextBoxFa TxtPriceOverTime;

	private TextBoxFa TxtPriceAbsent;

	private TextBoxFa TxtPriceHoliday;

	private TextBoxFa TxtPriceNight;

	private TextBoxFa TxtSalary;

	private NumericUpDown TxtLeavHoures;

	private NumericUpDown TxtLeavMinutes;

	private DataGridView DGrid;

	private DataGridViewTextBoxColumn DGGharardadID;

	private DataGridViewTextBoxColumn DGHesabID;

	private DataGridViewTextBoxColumn DGName;

	private DataGridViewTextBoxColumn DGCredit;

	private DataGridViewTextBoxColumn DGDebit;

	private LabelX labelX17;

	private NumericUpDown TxtLeaveSaved;

	private CheckBoxX ChbMaskan;

	private CheckBoxX chBxKLhoarBar;

	private CheckBoxX chBxChild;

	private LabelX labelX16;

	private ComboBoxEx cmbGroupSanavat;

	private LabelX labelX18;

	internal ButtonX BtnAddGroupSanavat;

	private LabelX LblSumHoghMazaya;

	private LabelX labelX29;

	internal ButtonX BtnDelGroupSanavat;

	private CheckBoxX ChbHaghTaahol;

	private LabelX LblSumMazayaKosorat;

	private LabelX LblSumHogh;

	private LabelX labelX42;

	private LabelX labelX37;

	internal ButtonX btnCalOverLowTime;

	private ButtonX btnKosoratFormula;

	private LabelX LblDescFormula;

	private GroupPanel groupPanelTaradod;

	private TextBoxX txtUnemployment;

	private TextBoxX txtInsured;

	private TextBoxX txtEmployer;

	private LabelX labelX45;

	private LabelX labelX43;

	private LabelX labelX41;

	private RadioButton rbNotInsurance;

	private RadioButton rbPayloadCalculations;

	private RadioButton rbLeastLawLabor;

	private CheckBoxX chbxTaxOption;

	private TextBoxFa TxtSanavat;

	private RadioButton rbNotCheckoutVacation;

	private RadioButton rbCheckoutVacation;

	private Button btnUpdateParametersSalary;

	private TextBoxX TxtLeaveDays;

	private ComboBoxEx CmbGharardadStatus;

	private LabelX LblPriceTatil;

	private TextBoxFa TxtPriceTatil;

	private LabelX LblPriceLowTime3;

	private LabelX LblPriceOverTime3;

	private TextBoxFa TxtPriceLowTime3;

	private TextBoxFa TxtPriceOverTime3;

	private LabelX LblPriceLowTime2;

	private LabelX LblPriceOverTime2;

	private TextBoxFa TxtPriceLowTime2;

	private TextBoxFa TxtPriceOverTime2;

	private LabelX LblPriceMission;

	private TextBoxFa TxtPriceMission;

	private LabelX LblPriceJarime;

	private TextBoxFa TxtPriceJarime;

	private LabelX labelX23;

	private LabelX LblVacation;

	private TabControl TabControlExt;

	private LabelX LblPriceNobatkari3;

	private TextBoxFa TxtPriceNobatkari3;

	private LabelX LblPriceNobatkari2;

	private TextBoxFa TxtPriceNobatkari2;

	private TextBoxX TxtBankNo;

	private LabelX labelX27;

	private LabelX labelX33;

	private TextBoxX TxtShabaNo;

	private TextBoxX TxtJob;

	private LabelX labelX34;

	internal ButtonX BtnMoreJob;

	internal ButtonX BtnDelJob;

	private LabelX labelX22;

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

	public Font FontFrm { get; private set; }

	private void SetTrDictionary()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[افزودن قرارداد جدید]", out var value))
			{
				trDictionary.Add("[افزودن قرارداد جدید]", Utilities.GetTranslate("[افزودن قرارداد جدید]"));
			}
			if (!trDictionary.TryGetValue("[ویرایش قرارداد]", out value))
			{
				trDictionary.Add("[ویرایش قرارداد]", Utilities.GetTranslate("[ویرایش قرارداد]"));
			}
			if (!trDictionary.TryGetValue("[نامشخص]", out value))
			{
				trDictionary.Add("[نامشخص]", Utilities.GetTranslate("[نامشخص]"));
			}
			if (!trDictionary.TryGetValue("[بدلیل وجود فرمول در مزایا و کسورات مبالغ آنها در هنگام صدور فیش حقوقی تعیین میشود بنابراین در اینجا مبلغ آن 1- در نظر گرفته شده و در جمع مزایا و کسورات محاسبه نشده است]", out value))
			{
				trDictionary.Add("[بدلیل وجود فرمول در مزایا و کسورات مبالغ آنها در هنگام صدور فیش حقوقی تعیین میشود بنابراین در اینجا مبلغ آن 1- در نظر گرفته شده و در جمع مزایا و کسورات محاسبه نشده است]", Utilities.GetTranslate("[بدلیل وجود فرمول در مزایا و کسورات مبالغ آنها در هنگام صدور فیش حقوقی تعیین میشود بنابراین در اینجا مبلغ آن 1- در نظر گرفته شده و در جمع مزایا و کسورات محاسبه نشده است]"));
			}
			if (!trDictionary.TryGetValue("[شما با استفاده از دکمه مزایا و کسورات فرمولی میتوانید هر نوع مزایا و یا کسورات را بصورت فرمولی تعریف کنید ]", out value))
			{
				trDictionary.Add("[شما با استفاده از دکمه مزایا و کسورات فرمولی میتوانید هر نوع مزایا و یا کسورات را بصورت فرمولی تعریف کنید ]", Utilities.GetTranslate("[شما با استفاده از دکمه مزایا و کسورات فرمولی میتوانید هر نوع مزایا و یا کسورات را بصورت فرمولی تعریف کنید ]"));
			}
			if (!trDictionary.TryGetValue("[بدون شیفت]", out value))
			{
				trDictionary.Add("[بدون شیفت]", Utilities.GetTranslate("[بدون شیفت]"));
			}
			if (!trDictionary.TryGetValue("[آیا اطلاعات قبلی شیفت برنامه ریزی شده این قرارداد حذف شود؟]", out value))
			{
				trDictionary.Add("[آیا اطلاعات قبلی شیفت برنامه ریزی شده این قرارداد حذف شود؟]", Utilities.GetTranslate("[آیا اطلاعات قبلی شیفت برنامه ریزی شده این قرارداد حذف شود؟]"));
			}
			if (!trDictionary.TryGetValue("[توجه]", out value))
			{
				trDictionary.Add("[توجه]", Utilities.GetTranslate("[توجه]"));
			}
			if (!trDictionary.TryGetValue("[حق مرخصی در]", out value))
			{
				trDictionary.Add("[حق مرخصی در]", Utilities.GetTranslate("[حق مرخصی در]"));
			}
			if (!trDictionary.TryGetValue("[ ماه (روز)]", out value))
			{
				trDictionary.Add("[ ماه (روز)]", Utilities.GetTranslate("[ ماه (روز)]"));
			}
			if (!trDictionary.TryGetValue("[حق مرخصی در هر ماه(روز)]", out value))
			{
				trDictionary.Add("[حق مرخصی در هر ماه(روز)]", Utilities.GetTranslate("[حق مرخصی در هر ماه(روز)]"));
			}
			if (!trDictionary.TryGetValue("[آیا مبالغ حقوق روزانه، اضافه کار، جمعه کاری، تعطیل کاری و غیره طبق قانون کار تامین اجتماعی جایگزین شود؟]", out value))
			{
				trDictionary.Add("[آیا مبالغ حقوق روزانه، اضافه کار، جمعه کاری، تعطیل کاری و غیره طبق قانون کار تامین اجتماعی جایگزین شود؟]", Utilities.GetTranslate("[آیا مبالغ حقوق روزانه، اضافه کار، جمعه کاری، تعطیل کاری و غیره طبق قانون کار تامین اجتماعی جایگزین شود؟]"));
			}
			if (!trDictionary.TryGetValue("[متاهل]", out value))
			{
				trDictionary.Add("[متاهل]", Utilities.GetTranslate("[متاهل]"));
			}
			if (!trDictionary.TryGetValue("[مجرد]", out value))
			{
				trDictionary.Add("[مجرد]", Utilities.GetTranslate("[مجرد]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionary", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 72);
		}
	}

	private async Task LoadDataAsync(CancellationToken token)
	{
		try
		{
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			List<Company> company = await Company.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem, token);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Func<Task>)async delegate
			{
				_ = 11;
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						base.objsCanEmpty.Add((Control)(object)DGrid);
						base.objsCanEmpty.Add((Control)(object)TxtPriceOverTime);
						base.objsCanEmpty.Add((Control)(object)TxtPriceOverTime2);
						base.objsCanEmpty.Add((Control)(object)TxtPriceOverTime3);
						base.objsCanEmpty.Add((Control)(object)TxtPriceNight);
						base.objsCanEmpty.Add((Control)(object)TxtPriceNobatkari1);
						base.objsCanEmpty.Add((Control)(object)TxtPriceNobatkari2);
						base.objsCanEmpty.Add((Control)(object)TxtPriceNobatkari3);
						base.objsCanEmpty.Add((Control)(object)TxtPriceHoliday);
						base.objsCanEmpty.Add((Control)(object)TxtPriceTatil);
						base.objsCanEmpty.Add((Control)(object)TxtPriceJarime);
						base.objsCanEmpty.Add((Control)(object)TxtPriceLeave);
						base.objsCanEmpty.Add((Control)(object)TxtPriceAbsent);
						base.objsCanEmpty.Add((Control)(object)TxtPriceMission);
						base.objsCanEmpty.Add((Control)(object)TxtPriceLowTime);
						base.objsCanEmpty.Add((Control)(object)TxtPriceLowTime2);
						base.objsCanEmpty.Add((Control)(object)TxtPriceLowTime3);
						base.objsCanEmpty.Add((Control)(object)TxtPeyvast);
						base.objsCanEmpty.Add((Control)(object)TxtDesc);
						((ComboBox)CmbCompany).DataSource = company;
						await LoadDataGharardadStatusAsync(token);
						await LoadDataShiftAsync(token);
						await LoadDataBankAsync(token);
						await LoadDataRecruitmentStAsync(token);
						await LoadDataRecruitmentTypeAsync(token);
						await LoadDataPlaceAsync(token);
						await LoadDataPartAsync(token);
						await LoadDataBankTypeAsync(token);
						ComboBoxEx cmbSalaryType = CmbSalaryType;
						((ComboBox)cmbSalaryType).DataSource = await Per_SalaryType.SelectAllAsync(((CacheBaseForm)this).ContextGuid, "", token);
						if (((ListControl)CmbSalaryType).SelectedValue == null)
						{
							((ListControl)CmbSalaryType).SelectedIndex = 1;
						}
						await LoadDataGroupSanavatAsync(token);
						((Control)tabControlPanel3).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadEstekhdami;
						((Control)tabControlPanel4).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadCURDMazayaKosorat;
						CheckBoxX chbMaskan = ChbMaskan;
						CheckBoxX obj = chBxChild;
						CheckBoxX obj2 = chBxKLhoarBar;
						ComboBoxEx obj3 = cmbGroupSanavat;
						ButtonX btnDelGroupSanavat = BtnDelGroupSanavat;
						CheckBoxX chbHaghTaahol = ChbHaghTaahol;
						ButtonX btnAddGroupSanavat = BtnAddGroupSanavat;
						bool flag = (((Control)TxtSanavat).Enabled = UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadHaghs);
						bool flag3 = (((Control)btnAddGroupSanavat).Enabled = flag);
						bool flag5 = (((Control)chbHaghTaahol).Enabled = flag3);
						bool flag7 = (((Control)btnDelGroupSanavat).Enabled = flag5);
						bool flag9 = (((Control)obj3).Enabled = flag7);
						bool flag11 = (((Control)obj2).Enabled = flag9);
						bool enabled = (((Control)obj).Enabled = flag11);
						((Control)chbMaskan).Enabled = enabled;
						string text = "متاهل";
						if (trDictionary.TryGetValue("[متاهل]", out var value2))
						{
							text = value2;
						}
						string text2 = "مجرد";
						if (trDictionary.TryGetValue("[مجرد]", out var value3))
						{
							text2 = value3;
						}
						CmbMarried.Text = text;
						CmbSingle.Text = text2;
						await Task.Run((Func<Task?>)SetTooltipAsync);
						await FillFormAsync(token);
					}
				}
				catch (Exception ex3)
				{
					Log.Error(ex3, "");
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	public FrmGharardad(string contextGuid, Font font, GharardadNew gharardad)
		: base(contextGuid, true, true)
	{
		try
		{
			_font = font;
			((Control)this).Font = _font;
			_clsGharardad = gharardad;
			InitializeComponent();
			if (_clsGharardad.ID > 0)
			{
				((Control)this).Text = "ویرایش قرارداد ";
				if (trDictionary.TryGetValue("[ویرایش قرارداد]", out var value))
				{
					((Control)this).Text = value;
				}
			}
			else
			{
				((Control)this).Text = "افزودن قرارداد جدید";
				if (trDictionary.TryGetValue("[افزودن قرارداد جدید]", out var value2))
				{
					((Control)this).Text = value2;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 186);
		}
	}

	public FrmGharardad(string contextGuid, Font font, GharardadNew pGharardad, bool readOnly = true)
		: base(contextGuid, true, true)
	{
		try
		{
			_font = font;
			InitializeComponent();
			_clsGharardad = pGharardad;
			((BaseForm)this).NoCheckFormContentsChanged = true;
			((Control)BtnAccept).Enabled = !readOnly;
			string text = "مشاهده قرارداد(غیر قابل ویرایش)";
			string text2 = "ایجاد قرارداد جدید";
			if (trDictionary.TryGetValue("[مشاهده قرارداد- غیر قابل ویرایش]", out var value))
			{
				text = value;
			}
			if (trDictionary.TryGetValue("[ایجاد قرارداد جدید]", out var value2))
			{
				text2 = value2;
			}
			((Form)this).Text = (readOnly ? text : text2);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 213);
		}
	}

	private void FrmGharardad_Load(object sender, EventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_loadDataToken?.Cancel();
			_loadDataToken = new CancellationTokenSource();
			CancellationToken token = _loadDataToken.Token;
			new Waiter("بارگذاری داده", (Form)(object)this, LoadDataAsync(token), false, (EnWaiterType)0, (Control)null, "FrmGharardad_Load", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void SetEvents(bool enableEvents = true)
	{
		try
		{
			if (enableEvents)
			{
				((Control)TXTStartDate).TextChanged += TXTStartAndEndDate_TextChanged;
				((Control)TxtEndDate).TextChanged += TXTStartAndEndDate_TextChanged;
				((ComboBox)CmbSalaryType).SelectedIndexChanged += CmbSalaryType_SelectedIndexChanged;
				ChbMaskan.CheckedChanged += ChBxChild_CheckedChanged;
				chBxKLhoarBar.CheckedChanged += ChBxChild_CheckedChanged;
				chBxChild.CheckedChanged += ChBxChild_CheckedChanged;
				((ComboBox)CmbMariage).SelectedIndexChanged += CmbMariage_SelectedIndexChanged;
				((Control)TxtSalary).TextChanged += TxtSalary_TextChanged;
				NumberCountChild.ValueChanged += NumberCountChild_ValueChanged;
				((ComboBox)cmbGroupSanavat).SelectedIndexChanged += TxtSanavat_TextChanged;
				((Control)TxtSanavat).TextChanged += TxtSanavat_TextChanged;
				chbxTaxOption.CheckedChanged += ChBxChild_CheckedChanged;
				rbNotInsurance.CheckedChanged += ChBxChild_CheckedChanged;
				rbLeastLawLabor.CheckedChanged += ChBxChild_CheckedChanged;
				rbPayloadCalculations.CheckedChanged += ChBxChild_CheckedChanged;
			}
			else
			{
				((Control)TXTStartDate).TextChanged -= TXTStartAndEndDate_TextChanged;
				((Control)TxtEndDate).TextChanged -= TXTStartAndEndDate_TextChanged;
				((ComboBox)CmbSalaryType).SelectedIndexChanged -= CmbSalaryType_SelectedIndexChanged;
				ChbMaskan.CheckedChanged -= ChBxChild_CheckedChanged;
				chBxKLhoarBar.CheckedChanged -= ChBxChild_CheckedChanged;
				chBxChild.CheckedChanged -= ChBxChild_CheckedChanged;
				((ComboBox)CmbMariage).SelectedIndexChanged -= CmbMariage_SelectedIndexChanged;
				((Control)TxtSalary).TextChanged -= TxtSalary_TextChanged;
				NumberCountChild.ValueChanged -= NumberCountChild_ValueChanged;
				((ComboBox)cmbGroupSanavat).SelectedIndexChanged -= TxtSanavat_TextChanged;
				((Control)TxtSanavat).TextChanged -= TxtSanavat_TextChanged;
				chbxTaxOption.CheckedChanged -= ChBxChild_CheckedChanged;
				rbNotInsurance.CheckedChanged -= ChBxChild_CheckedChanged;
				rbLeastLawLabor.CheckedChanged -= ChBxChild_CheckedChanged;
				rbPayloadCalculations.CheckedChanged -= ChBxChild_CheckedChanged;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetEvents", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 271);
		}
	}

	private async Task FillFormAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			SetEvents(enableEvents: false);
			_clsPerson = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonIdAsync(((CacheBaseForm)this).ContextGuid, _clsGharardad.PersonId, token);
			((Control)numberCod).Text = _clsGharardad.No;
			((Control)TXTDateRegister).Text = Calendar.MiladiToCalture(_clsGharardad.DateM);
			((Control)TXTStartDate).TextChanged -= TXTStartAndEndDate_TextChanged;
			((Control)TxtEndDate).TextChanged -= TXTStartAndEndDate_TextChanged;
			((Control)TXTStartDate).Text = Calendar.MiladiToCalture(_clsGharardad.DateStartM);
			((Control)TxtEndDate).Text = Calendar.MiladiToCalture(_clsGharardad.DateFinishM);
			if (_clsPerson == null)
			{
				((PopupItemControl)BtnMoreP).PerformClick();
			}
			else
			{
				((Control)TxtcodePerson).Text = ((PersonBase)_clsPerson).CodeTafsil.ToString();
				LoadPersonData();
			}
			((ListControl)CmbCompany).SelectedValue = _clsGharardad.CompanyId;
			((ListControl)CmbGharardadStatus).SelectedValue = _clsGharardad.GharardadStatus;
			((ListControl)CmbShift).SelectedValue = (int)_clsGharardad.ShiftId;
			((Control)TxtPeyvast).Text = _clsGharardad.Peyvast;
			((Control)TxtDesc).Text = _clsGharardad.Description;
			if (_clsGharardad.SalaryType > 0)
			{
				((ListControl)CmbSalaryType).SelectedValue = _clsGharardad.SalaryType;
			}
			else
			{
				((ListControl)CmbSalaryType).SelectedValue = 1;
			}
			((Control)TxtSalary).Text = _clsGharardad.SalaryPrice.ToString("N0");
			((Control)TxtSanavat).Text = _clsGharardad.Sanavat.ToString("N0");
			if (_clsGharardad.GroupSanavat > 0)
			{
				((ListControl)cmbGroupSanavat).SelectedValue = _clsGharardad.GroupSanavat;
			}
			((ComboBox)CmbMariage).SelectedItem = ((!_clsGharardad.Married) ? CmbSingle : CmbMarried);
			ChbHaghTaahol.Checked = _clsGharardad.IsMotahel;
			chBxChild.Checked = _clsGharardad.GetHesabId(2) != null;
			ChbMaskan.Checked = _clsGharardad.GetHesabId(1) != null;
			chBxKLhoarBar.Checked = _clsGharardad.GetHesabId(3) != null;
			NumberCountChild.Value = _clsGharardad.CountChild;
			((Control)TxtPriceNobatkari1).Text = _clsGharardad.PriceNobatKari1.ToString("N0");
			((Control)TxtPriceNobatkari2).Text = _clsGharardad.PriceNobatKari2.ToString("N0");
			((Control)TxtPriceNobatkari3).Text = _clsGharardad.PriceNobatKari3.ToString("N0");
			((Control)TxtPriceOverTime).Text = _clsGharardad.PriceOverTime.ToString("N0");
			((Control)TxtPriceOverTime2).Text = _clsGharardad.PriceOverTime2.ToString("N0");
			((Control)TxtPriceOverTime3).Text = _clsGharardad.PriceOverTime3.ToString("N0");
			((Control)TxtPriceLeave).Text = _clsGharardad.PriceLeave.ToString("N0");
			((Control)TxtPriceNight).Text = _clsGharardad.PriceNight.ToString("N0");
			((Control)TxtPriceHoliday).Text = _clsGharardad.PriceHoliday.ToString("N0");
			((Control)TxtPriceTatil).Text = _clsGharardad.PriceTatil.ToString("N0");
			((Control)TxtPriceJarime).Text = _clsGharardad.PriceJarime.ToString("N0");
			((Control)TxtPriceLowTime).Text = _clsGharardad.PriceLowTime.ToString("N0");
			((Control)TxtPriceLowTime2).Text = _clsGharardad.PriceLowTime2.ToString("N0");
			((Control)TxtPriceLowTime3).Text = _clsGharardad.PriceLowTime3.ToString("N0");
			((Control)TxtPriceAbsent).Text = _clsGharardad.PriceAbsent.ToString("N0");
			((Control)TxtPriceMission).Text = _clsGharardad.PriceMission.ToString("N0");
			if (_clsGharardad.IsCheckoutVacation)
			{
				rbCheckoutVacation.Checked = true;
			}
			else
			{
				rbNotCheckoutVacation.Checked = true;
			}
			((Control)TxtLeaveDays).Text = _clsGharardad.LeaveDays.ToString();
			if ((decimal)_clsGharardad.LeaveSaved > TxtLeaveSaved.Maximum)
			{
				_clsGharardad.LeaveSaved = (double)TxtLeaveSaved.Maximum;
			}
			else if ((decimal)_clsGharardad.LeaveSaved < TxtLeaveSaved.Minimum)
			{
				_clsGharardad.LeaveSaved = (double)TxtLeaveSaved.Minimum;
			}
			else
			{
				TxtLeaveSaved.Value = (decimal)_clsGharardad.LeaveSaved;
			}
			int num = _clsGharardad.LeaveMinutes / 60;
			int num2 = _clsGharardad.LeaveMinutes % 60;
			TxtLeavHoures.ValueChanged -= TxtLeavHoures_ValueChanged;
			TxtLeavMinutes.ValueChanged -= TxtLeavHoures_ValueChanged;
			TxtLeavHoures.Value = num;
			TxtLeavMinutes.Value = num2;
			TxtLeavHoures.ValueChanged += TxtLeavHoures_ValueChanged;
			TxtLeavMinutes.ValueChanged += TxtLeavHoures_ValueChanged;
			if (_clsGharardad.BankId > 0)
			{
				((ListControl)CmbBank).SelectedValue = _clsGharardad.BankId;
			}
			if (_clsGharardad.BankType > 0)
			{
				((ListControl)CmbBankType).SelectedValue = _clsGharardad.BankType;
			}
			((Control)TxtBankNo).Text = _clsGharardad.BankNo;
			((Control)TxtShabaNo).Text = _clsGharardad.ShabaNo;
			if (_clsGharardad.JobId > 0)
			{
				TextBoxX txtJob = TxtJob;
				((Control)txtJob).Text = (await Per_WorkTitle.GetAsync(((CacheBaseForm)this).ContextGuid, _clsGharardad.JobId.Value, token)).Name;
			}
			if (_clsGharardad.PartId > 0)
			{
				((ListControl)CmbPart).SelectedValue = _clsGharardad.PartId;
			}
			if (_clsGharardad.RecruitmentSt > 0)
			{
				((ListControl)CmbRecruitmentST).SelectedValue = _clsGharardad.RecruitmentSt;
			}
			if (_clsGharardad.RecruitmentType > 0)
			{
				((ListControl)CmbRecruitmentType).SelectedValue = _clsGharardad.RecruitmentType;
			}
			if (_clsGharardad.PlaceId > 0)
			{
				((ListControl)CmbPlace).SelectedValue = _clsGharardad.PlaceId;
			}
			chbxTaxOption.Checked = (int)_clsGharardad.TaxOption == 1;
			((Control)txtInsured).Text = _clsGharardad.ZaribInsuredInsurance.ToString();
			((Control)txtEmployer).Text = _clsGharardad.ZaribEmployerInsurance.ToString();
			((Control)txtUnemployment).Text = _clsGharardad.ZaribUnemploymentInsurance.ToString();
			InsuranceGharadadEnumType insuranceOption = _clsGharardad.InsuranceOption;
			switch (insuranceOption - -1)
			{
			case 0:
				rbNotInsurance.Checked = true;
				break;
			case 1:
				rbNotInsurance.Checked = true;
				break;
			case 2:
				rbLeastLawLabor.Checked = true;
				break;
			case 3:
				rbPayloadCalculations.Checked = true;
				break;
			default:
				rbNotInsurance.Checked = true;
				break;
			}
			((DataGridViewBand)DGGharardadID).Visible = false;
			((DataGridViewColumn)DGCredit).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
			await SumHoghMazayaAsync(token);
			SetEvents();
			((ComboBox)CmbShift).SelectedIndexChanged += CmbShift_SelectedIndexChanged;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FillFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 411);
		}
	}

	private void LoadPersonData()
	{
		try
		{
			if (_clsPerson != null)
			{
				((Control)lblname).Text = ((PersonBase)_clsPerson).FullName;
				if (((PersonBase)_clsPerson).Jensiat == 2)
				{
					((ComboBox)CmbRecruitmentST).SelectedItem = null;
					((Control)CmbRecruitmentST).Enabled = false;
					((Control)BtnMoreRecruitmentST).Enabled = false;
					((Control)BtnDelRecruitmentST).Enabled = false;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadPersonData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 429);
		}
	}

	private async Task<ReturnedSaveFuncInfo> LoadDataShiftAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			string extItem = "بدون شیفت";
			if (trDictionary.TryGetValue("[بدون شیفت]", out var value))
			{
				extItem = value;
			}
			List<Shifts> list = await Shifts.SelectAllAsync(((CacheBaseForm)this).ContextGuid, st: true, (PackageEnumType)100, extItem, token);
			ReturnedSaveFuncInfoWithValue<HozoorPackage> typeSoftware = UserAttendance.SelectedUser.AccessLevelMerg.GetTypeSoftware();
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)typeSoftware, (ReturnedState)0, "LoadDataShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 443);
			if ((int)typeSoftware.value == 0)
			{
				list = list.Where((Shifts q) => (int)q.PackageType > 0).ToList();
			}
			((ComboBox)CmbShift).DataSource = list;
			((ListControl)CmbShift).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "LoadDataShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 452);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataShiftAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 453);
		}
		return ret;
	}

	private async Task LoadDataGharardadStatusAsync(CancellationToken token)
	{
		try
		{
			ComboBoxEx cmbGharardadStatus = CmbGharardadStatus;
			((ComboBox)cmbGharardadStatus).DataSource = await GharardadStatus.SelectAllAsync(((CacheBaseForm)this).ContextGuid, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataGharardadStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 466);
		}
	}

	private async Task LoadDataGroupSanavatAsync(CancellationToken token)
	{
		try
		{
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			ComboBoxEx val = cmbGroupSanavat;
			((ComboBox)val).DataSource = await GroupSanavat.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem, token);
			((ListControl)cmbGroupSanavat).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataGroupSanavatAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 481);
		}
	}

	private async Task LoadDataBankAsync(CancellationToken token)
	{
		try
		{
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			ComboBoxEx cmbBank = CmbBank;
			((ComboBox)cmbBank).DataSource = await Bank.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem, token);
			((ListControl)CmbBank).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataBankAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 496);
		}
	}

	private async Task LoadDataRecruitmentStAsync(CancellationToken token)
	{
		try
		{
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			ComboBoxEx cmbRecruitmentST = CmbRecruitmentST;
			((ComboBox)cmbRecruitmentST).DataSource = await Per_Khedmat.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem, token);
			((ListControl)CmbRecruitmentST).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataRecruitmentStAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 511);
		}
	}

	private async Task LoadDataRecruitmentTypeAsync(CancellationToken token)
	{
		try
		{
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			ComboBoxEx cmbRecruitmentType = CmbRecruitmentType;
			((ComboBox)cmbRecruitmentType).DataSource = await Per_Typeestekhdam.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem, token);
			((ListControl)CmbRecruitmentType).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataRecruitmentTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 526);
		}
	}

	private async Task LoadDataPlaceAsync(CancellationToken token)
	{
		try
		{
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			ComboBoxEx cmbPlace = CmbPlace;
			((ComboBox)cmbPlace).DataSource = await Per_KhedmatPlace.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem, token);
			((ListControl)CmbPlace).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataPlaceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 541);
		}
	}

	private async Task LoadDataPartAsync(CancellationToken token)
	{
		try
		{
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			ComboBoxEx cmbPart = CmbPart;
			((ComboBox)cmbPart).DataSource = await Per_Part.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem, token);
			((ListControl)CmbPart).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 556);
		}
	}

	private async Task LoadDataBankTypeAsync(CancellationToken token)
	{
		try
		{
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			ComboBoxEx cmbBankType = CmbBankType;
			((ComboBox)cmbBankType).DataSource = await BankHesabType.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem, token);
			((ListControl)CmbBankType).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataBankTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 571);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		try
		{
			((FormExtention)this).OnLoad(e);
			Task.Run((Action)SetTrDictionary);
			TabControlExt = tabControl1;
			((FormExtention)this).OK_Click += new CrmEventHandler(BtnAccept_click);
			((FormExtention)this).Insert_Click += new CrmEventHandler(BTNADDMazaya_Click);
			((FormExtention)this).Edit_Click += new CrmEventHandler(BTNEdit_Click);
			((FormExtention)this).Other_Click += new CrmEventHandler(BTNAddKosorat_Click);
			((FormExtention)this).Delete_Click += new CrmEventHandler(BTNDelete_Click);
			tabControl1.SelectedTabIndex = 0;
			((Control)TxtcodePerson).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnLoad", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 592);
		}
	}

	private async void BtnAccept_click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			GetData();
			string contextGuid = ((CacheBaseForm)this).ContextGuid;
			DialogResult val = CheckValidation.Validtor(contextGuid, await _clsGharardad.CheckValidationAsync(token), isShowMessage: true);
			if ((int)val != 2)
			{
				if (val - 5 <= 1)
				{
					if (_clsGharardad.IsTemp)
					{
						((Form)this).DialogResult = (DialogResult)1;
					}
					else if (await _clsGharardad.SaveAsync(token))
					{
						((Form)this).DialogResult = (DialogResult)1;
					}
					else
					{
						((Form)this).DialogResult = (DialogResult)2;
					}
					((FormExtention)this).ResetControlChanged();
					((Form)this).Close();
				}
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)2;
				((FormExtention)this).ResetControlChanged();
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnAccept_click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 631);
		}
	}

	private void GetData()
	{
		int num = 0;
		try
		{
			num = 1;
			_clsGharardad.No = Text.FixString(((Control)numberCod).Text);
			num = 2;
			_clsGharardad.DateM = Utilities.CalendarToMiladi(((Control)TXTDateRegister).Text);
			num = 3;
			_clsGharardad.DateFinishM = Utilities.CalendarToMiladi(((Control)TxtEndDate).Text);
			num = 4;
			_clsGharardad.DateStartM = Utilities.CalendarToMiladi(((Control)TXTStartDate).Text);
			num = 5;
			if (_clsPerson != null)
			{
				_clsGharardad.PersonId = ((PersonBase)_clsPerson).Id;
			}
			num = 6;
			if (((ListControl)CmbCompany).SelectedValue != null)
			{
				_clsGharardad.CompanyId = short.Parse(((ListControl)CmbCompany).SelectedValue.ToString());
			}
			if (((ListControl)CmbGharardadStatus).SelectedValue != null)
			{
				_clsGharardad.GharardadStatus = short.Parse(((ListControl)CmbGharardadStatus).SelectedValue.ToString());
			}
			num = 7;
			if (((ListControl)CmbShift).SelectedValue != null)
			{
				_clsGharardad.ShiftId = short.Parse(((ListControl)CmbShift).SelectedValue.ToString());
			}
			num = 8;
			_clsGharardad.Peyvast = Text.FixString(((Control)TxtPeyvast).Text);
			num = 9;
			_clsGharardad.Description = Text.FixString(((Control)TxtDesc).Text);
			num = 10;
			_clsGharardad.SalaryType = Convert.ToInt16(((ListControl)CmbSalaryType).SelectedValue);
			num = 23;
			_clsGharardad.SetSalaryPrice(Utilities.ParseToDecimal(((Control)TxtSalary).Text, 0m, 0));
			num = 24;
			_clsGharardad.Sanavat = Utilities.ParseToShort(((Control)TxtSanavat).Text, (short)0);
			num = 25;
			_clsGharardad.GroupSanavat = Convert.ToInt16(((ListControl)cmbGroupSanavat).SelectedValue);
			num = 26;
			_clsGharardad.Married = ((ComboBox)CmbMariage).SelectedItem == CmbMarried;
			num = 27;
			_clsGharardad.IsMotahel = ChbHaghTaahol.Checked;
			num = 28;
			_clsGharardad.CountChild = (short)NumberCountChild.Value;
			num = 29;
			_clsGharardad.PriceNobatKari1 = Utilities.ParseToDecimal(((Control)TxtPriceNobatkari1).Text, 0m, 0);
			_clsGharardad.PriceNobatKari2 = Utilities.ParseToDecimal(((Control)TxtPriceNobatkari2).Text, 0m, 0);
			_clsGharardad.PriceNobatKari3 = Utilities.ParseToDecimal(((Control)TxtPriceNobatkari3).Text, 0m, 0);
			num = 30;
			_clsGharardad.PriceOverTime = Utilities.ParseToDecimal(((Control)TxtPriceOverTime).Text, 0m, 0);
			_clsGharardad.PriceOverTime2 = Utilities.ParseToDecimal(((Control)TxtPriceOverTime2).Text, 0m, 0);
			_clsGharardad.PriceOverTime3 = Utilities.ParseToDecimal(((Control)TxtPriceOverTime3).Text, 0m, 0);
			num = 31;
			_clsGharardad.PriceLeave = Utilities.ParseToDecimal(((Control)TxtPriceLeave).Text, 0m, 0);
			num = 32;
			_clsGharardad.PriceNight = Utilities.ParseToDecimal(((Control)TxtPriceNight).Text, 0m, 0);
			num = 33;
			_clsGharardad.PriceLowTime = Utilities.ParseToDecimal(((Control)TxtPriceLowTime).Text, 0m, 0);
			_clsGharardad.PriceLowTime2 = Utilities.ParseToDecimal(((Control)TxtPriceLowTime2).Text, 0m, 0);
			_clsGharardad.PriceLowTime3 = Utilities.ParseToDecimal(((Control)TxtPriceLowTime3).Text, 0m, 0);
			num = 34;
			_clsGharardad.PriceHoliday = Utilities.ParseToDecimal(((Control)TxtPriceHoliday).Text, 0m, 0);
			_clsGharardad.PriceTatil = Utilities.ParseToDecimal(((Control)TxtPriceTatil).Text, 0m, 0);
			_clsGharardad.PriceJarime = Utilities.ParseToDecimal(((Control)TxtPriceJarime).Text, 0m, 0);
			num = 35;
			_clsGharardad.PriceAbsent = Utilities.ParseToDecimal(((Control)TxtPriceAbsent).Text, 0m, 0);
			_clsGharardad.PriceMission = Utilities.ParseToDecimal(((Control)TxtPriceMission).Text, 0m, 0);
			num = 36;
			_clsGharardad.LeaveDays = Utilities.ParseToDouble(((Control)TxtLeaveDays).Text, 0.0, (short)3);
			_clsGharardad.IsCheckoutVacation = rbCheckoutVacation.Checked;
			num = 37;
			_clsGharardad.LeaveSaved = Utilities.ParseToDouble(((Control)TxtLeaveSaved).Text, 0.0, (short)3);
			num = 38;
			_clsGharardad.LeaveMinutes = (short)(Utilities.ParseToShort(((Control)TxtLeavHoures).Text, (short)0) * 60 + Utilities.ParseToShort(((Control)TxtLeavMinutes).Text, (short)0));
			_clsGharardad.BankId = Convert.ToInt16(((ListControl)CmbBank).SelectedValue);
			num = 42;
			_clsGharardad.BankType = Convert.ToInt16(((ListControl)CmbBankType).SelectedValue);
			num = 43;
			_clsGharardad.BankNo = ((Control)TxtBankNo).Text;
			num = 44;
			_clsGharardad.ShabaNo = ((Control)TxtShabaNo).Text;
			num = 45;
			_clsGharardad.PartId = Convert.ToInt16(((ListControl)CmbPart).SelectedValue);
			num = 46;
			_clsGharardad.RecruitmentSt = Convert.ToInt16(((ListControl)CmbRecruitmentST).SelectedValue);
			num = 47;
			_clsGharardad.RecruitmentType = Convert.ToInt16(((ListControl)CmbRecruitmentType).SelectedValue);
			num = 48;
			_clsGharardad.PlaceId = Convert.ToInt16(((ListControl)CmbPlace).SelectedValue);
			num = 49;
			_clsGharardad.TaxOption = (TaxGharadadEnumType)(chbxTaxOption.Checked ? 1 : 0);
			num = 50;
			_clsGharardad.ZaribInsuredInsurance = Utilities.ParseTofloat(((Control)txtInsured).Text, 0f, (short)3);
			num = 51;
			_clsGharardad.ZaribEmployerInsurance = Utilities.ParseTofloat(((Control)txtEmployer).Text, 0f, (short)3);
			num = 52;
			_clsGharardad.ZaribUnemploymentInsurance = Utilities.ParseTofloat(((Control)txtUnemployment).Text, 0f, (short)3);
			num = 53;
			if (rbNotInsurance.Checked)
			{
				_clsGharardad.InsuranceOption = (InsuranceGharadadEnumType)0;
			}
			else
			{
				num = 54;
				_clsGharardad.InsuranceOption = (InsuranceGharadadEnumType)(rbLeastLawLabor.Checked ? 1 : 2);
				num = 55;
			}
			num = 56;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, $"line no : {num}", "GetData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 761);
		}
	}

	private async void CmbMariage_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (((ComboBox)CmbMariage).SelectedItem == CmbSingle)
			{
				ChbHaghTaahol.Checked = false;
				chBxChild.Checked = false;
			}
			else
			{
				((Control)NumberCountChild).Enabled = true;
				ChbHaghTaahol.Checked = true;
				((Control)chBxChild).Enabled = true;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmbMariage_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 790);
		}
	}

	private async void TxtcodePerson_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtcodePerson).Text = Text.RemoveNoNumbers(((Control)TxtcodePerson).Text, "");
			long.TryParse(((Control)TxtcodePerson).Text, out var result);
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			_clsPerson = await TimeAttendanceDAL.Dal.FLRPerson.Person.GetByPersonCodeTafsilAsync(((CacheBaseForm)this).ContextGuid, result, token);
			LoadPersonData();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtcodePerson_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 806);
		}
	}

	private async void BtnMoreP_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtcodePerson).Focus();
			FrmSelectPersonel frm = new FrmSelectPersonel(((CacheBaseForm)this).ContextGuid, isSelect: true, _font);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			_clsPerson = frm.SelectedPerson;
			TextBoxX txtcodePerson = TxtcodePerson;
			TimeAttendanceDAL.Dal.FLRPerson.Person clsPerson = _clsPerson;
			((Control)txtcodePerson).Text = ((clsPerson != null) ? ((PersonBase)clsPerson).CodeTafsil.ToString() : null);
			((Form)frm).Close();
			LoadPersonData();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreP_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 822);
		}
	}

	private async void BtnmoreCompany_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CmbCompany).Focus();
			FrmCompanyMain frm = new FrmCompanyMain(((CacheBaseForm)this).ContextGuid, _font, pIsSelect: true);
			if (((ListControl)CmbCompany).SelectedValue != null)
			{
				short.Parse(((ListControl)CmbCompany).SelectedValue.ToString());
			}
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			short ID = frm.SelectedRow.Id;
			string extItem = "نامشخص";
			if (trDictionary.TryGetValue("[نامشخص]", out var value))
			{
				extItem = value;
			}
			List<Company> dataSource = await Company.SelectAllAsync(((CacheBaseForm)this).ContextGuid, extItem);
			((ComboBox)CmbCompany).DataSource = dataSource;
			((ListControl)CmbCompany).SelectedValue = ID;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnmoreCompany_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 844);
		}
	}

	private async void BtnGroupSanavat_Click(object sender, EventArgs e)
	{
		_ = 2;
		try
		{
			((Control)CmbCompany).Focus();
			FrmGroupSanavatList frm = new FrmGroupSanavatList(((CacheBaseForm)this).ContextGuid, pIsSelect: true);
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				await LoadDataGroupSanavatAsync(token);
				((ListControl)CmbCompany).SelectedValue = frm.ID;
			}
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnGroupSanavat_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 864);
		}
	}

	private async void BTNMore_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbShift).Focus();
			FrmShiftMain frmShiftMain = new FrmShiftMain(((CacheBaseForm)this).ContextGuid, _font, Select: true);
			if (((ListControl)CmbShift).SelectedValue != null)
			{
				short.Parse(((ListControl)CmbShift).SelectedValue.ToString());
			}
			((Form)frmShiftMain).ShowDialog();
			short ID = frmShiftMain.ShiftId;
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await LoadDataShiftAsync(token);
			((ListControl)CmbShift).SelectedValue = ID;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNMore_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 884);
		}
	}

	private async void BtnMorebank_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CmbBank).Focus();
			short ID = 0;
			FRMBankMain frm = new FRMBankMain(((CacheBaseForm)this).ContextGuid, _font);
			if (((ListControl)CmbBank).SelectedValue != null)
			{
				ID = short.Parse(((ListControl)CmbBank).SelectedValue.ToString());
			}
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				ID = frm.SelectedItem.ID;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await LoadDataBankAsync(token);
			((ListControl)CmbBank).SelectedValue = ID;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMorebank_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 906);
		}
	}

	private void BtnDelBank_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbBank).Focus();
			((ListControl)CmbBank).SelectedValue = 0;
			_clsGharardad.BankId = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelBank_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 917);
		}
	}

	private async void BtnMorebankType_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CmbBankType).Focus();
			short ID = 0;
			FrmTypeHesabMain frm = new FrmTypeHesabMain(((CacheBaseForm)this).ContextGuid, pIsSelect: true, _font);
			if (((ListControl)CmbBankType).SelectedValue != null)
			{
				ID = short.Parse(((ListControl)CmbBankType).SelectedValue.ToString());
			}
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				ID = frm.SelectedRow.ID;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await LoadDataBankTypeAsync(token);
			((ListControl)CmbBankType).SelectedValue = ID;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMorebankType_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 939);
		}
	}

	private void BtnDelBankType_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbBankType).Focus();
			((ListControl)CmbBankType).SelectedValue = 0;
			_clsGharardad.BankType = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelBankType_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 950);
		}
	}

	private async void BtnMoreJob_Click(object sender, EventArgs e)
	{
		try
		{
			FrmWorkTMain frm = new FrmWorkTMain(((CacheBaseForm)this).ContextGuid, pIsSelect: true);
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				if (_clsGharardad != null)
				{
					_clsGharardad.JobId = frm.SelectedRow.ID;
				}
				if (TxtJob != null && frm.SelectedRow.Name != null)
				{
					((Control)TxtJob).Text = frm.SelectedRow.Name;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreJob_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 964);
		}
	}

	private void BtnDelJob_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtJob).Focus();
			((Control)TxtJob).Text = "";
			_clsGharardad.JobId = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelJob_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 975);
		}
	}

	private async void BtnMorePart_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CmbPart).Focus();
			short ID = 0;
			FRMPartMain frm = new FRMPartMain(((CacheBaseForm)this).ContextGuid, pIsSelect: true, _font);
			if (((ListControl)CmbPart).SelectedValue != null)
			{
				ID = short.Parse(((ListControl)CmbPart).SelectedValue.ToString());
			}
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				ID = frm.SelectedRow.ID;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await LoadDataPartAsync(token);
			((ListControl)CmbPart).SelectedValue = ID;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMorePart_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 997);
		}
	}

	private void BtnDelPart_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbPart).Focus();
			((ListControl)CmbPart).SelectedValue = 0;
			_clsGharardad.PartId = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelPart_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1008);
		}
	}

	private async void BtnMoreRecruitmentST_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CmbRecruitmentST).Focus();
			short ID = 0;
			FRMKHSTMain frm = new FRMKHSTMain(((CacheBaseForm)this).ContextGuid, pIsSelect: true, FontFrm);
			if (((ListControl)CmbRecruitmentST).SelectedValue != null)
			{
				ID = short.Parse(((ListControl)CmbRecruitmentST).SelectedValue.ToString());
			}
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				ID = frm.SelectedRow.ID;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await LoadDataRecruitmentStAsync(token);
			((ListControl)CmbRecruitmentST).SelectedValue = ID;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreRecruitmentST_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1030);
		}
	}

	private void BtnDelRecruitmentST_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbRecruitmentST).Focus();
			((ListControl)CmbRecruitmentST).SelectedValue = 0;
			_clsGharardad.RecruitmentSt = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelRecruitmentST_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1041);
		}
	}

	private async void BtnMoreRecruitmentType_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CmbRecruitmentType).Focus();
			short ID = 0;
			FRMTypeEmain frm = new FRMTypeEmain(((CacheBaseForm)this).ContextGuid, pIsSelect: true, _font);
			if (((ListControl)CmbRecruitmentType).SelectedValue != null)
			{
				ID = short.Parse(((ListControl)CmbRecruitmentType).SelectedValue.ToString());
			}
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				ID = frm.SelectedRow.ID;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await LoadDataRecruitmentTypeAsync(token);
			((ListControl)CmbRecruitmentType).SelectedValue = ID;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMoreRecruitmentType_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1063);
		}
	}

	private void BtnDelRecruitmentType_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbRecruitmentType).Focus();
			((ListControl)CmbRecruitmentType).SelectedValue = 0;
			_clsGharardad.RecruitmentType = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelRecruitmentType_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1074);
		}
	}

	private async void BtnMorePlace_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			((Control)CmbPlace).Focus();
			short ID = 0;
			FRMKhedmatPMain frm = new FRMKhedmatPMain(((CacheBaseForm)this).ContextGuid, pIsSelect: true);
			if (((ListControl)CmbPlace).SelectedValue != null)
			{
				ID = short.Parse(((ListControl)CmbPlace).SelectedValue.ToString());
			}
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				ID = frm.SelectedRow.ID;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await LoadDataPlaceAsync(token);
			((ListControl)CmbPlace).SelectedValue = ID;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMorePlace_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1096);
		}
	}

	private void BtnDelPlace_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)CmbPlace).Focus();
			((ListControl)CmbPlace).SelectedValue = 0;
			_clsGharardad.PlaceId = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelPlace_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1107);
		}
	}

	private void BtnDelGroupSanavat_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)cmbGroupSanavat).Focus();
			((ListControl)cmbGroupSanavat).SelectedValue = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnDelGroupSanavat_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1117);
		}
	}

	private async void BTNADDMazaya_Click(object sender, EventArgs e)
	{
		if (!((Control)BTNADDMazaya).Enabled || !((Control)BTNADDMazaya).Visible)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			FrmHesabList frmHesabList = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 1);
			((Form)frmHesabList).WindowState = (FormWindowState)0;
			FrmHesabList frm = frmHesabList;
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				string selectedItems = frm.SelectedItems;
				if (selectedItems == "")
				{
					return;
				}
				string[] array = selectedItems.Split(new char[1] { char.Parse(";") });
				foreach (string s in array)
				{
					if (DGrid.RowCount <= 0 || !CheckRepeat(int.Parse(s)))
					{
						await AddAsync(int.Parse(s), token);
					}
				}
			}
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNADDMazaya_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1149);
		}
	}

	private async void BTNAddKosorat_Click(object sender, EventArgs e)
	{
		if (!((Control)BTNAddKosorat).Enabled || !((Control)BTNAddKosorat).Visible)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			FrmHesabList frmHesabList = new FrmHesabList(((CacheBaseForm)this).ContextGuid, _font, 2);
			((Form)frmHesabList).WindowState = (FormWindowState)0;
			FrmHesabList frm = frmHesabList;
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
			{
				string selectedItems = frm.SelectedItems;
				if (selectedItems == "")
				{
					return;
				}
				string[] array = selectedItems.Split(new char[1] { char.Parse(";") });
				foreach (string s in array)
				{
					if (DGrid.RowCount <= 0 || !CheckRepeat(int.Parse(s)))
					{
						await AddAsync(int.Parse(s), token);
					}
				}
			}
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNAddKosorat_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1176);
		}
	}

	private async void BTNEdit_Click(object sender, EventArgs e)
	{
		if (!((Control)BTNEdit).Enabled || !((Control)BTNEdit).Visible)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			if (DGrid.CurrentRow == null)
			{
				return;
			}
			if (Utilities.ParseToInt(DGrid[((DataGridViewBand)DGHesabID).Index, ((DataGridViewBand)DGrid.CurrentRow).Index].Value.ToString(), 0) != -1)
			{
				await ((CacheBaseForm)new FrmGharardadDet(((CacheBaseForm)this).ContextGuid, _clsGharardad.ListGharardadDet[((DataGridViewBand)DGrid.CurrentRow).Index])).ShowAsync((IWin32Window)(object)this);
			}
			else
			{
				FrmFormula frm2 = new FrmFormula(((CacheBaseForm)this).ContextGuid, _clsGharardad, _clsGharardad.ListGharardadDet[((DataGridViewBand)DGrid.CurrentRow).Index]);
				await ((CacheBaseForm)frm2).ShowAsync((IWin32Window)(object)this);
				if ((int)((Form)frm2).DialogResult == 1)
				{
					_clsGharardad.ListGharardadDet[((DataGridViewBand)DGrid.CurrentRow).Index] = frm2.GharardadDet;
				}
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNEdit_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1209);
		}
	}

	private async void BTNDelete_Click(object sender, EventArgs e)
	{
		if (!((Control)BTNDelete).Enabled || !((Control)BTNDelete).Visible)
		{
			return;
		}
		try
		{
			((Control)DGrid).Focus();
			if (DGrid.RowCount > 0 && DGrid.CurrentRow != null)
			{
				int num = Utilities.ParseToInt(DGrid[((DataGridViewBand)DGHesabID).Index, ((DataGridViewBand)DGrid.CurrentRow).Index].Value.ToString(), 0);
				if (num != -1 && num <= FishHesab.LastValBaseHesabOnHkgogh)
				{
					Utility.MessageBoxShowError("[شما مجاز به حذف این مزایا نیستید. برای حذف به صفحه اطلاعات حقوقی برودید و تیک مربوط به این مزایا را بردارید]", ((CacheBaseForm)this).ContextGuid);
				}
				else
				{
					_clsGharardad.RemoveAt(((DataGridViewBand)DGrid.CurrentRow).Index);
				}
				_changeToken?.Cancel();
				_changeToken = new CancellationTokenSource();
				CancellationToken token = _changeToken.Token;
				await SumHoghMazayaAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BTNDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1233);
		}
	}

	private async Task AddAsync(int pHesabId, CancellationToken token)
	{
		try
		{
			GharardadDetNew cls = await GharardadDetNew.GetAsync(((CacheBaseForm)this).ContextGuid, pHesabId, token);
			_clsGharardad.AddToList(cls);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1245);
		}
	}

	private void SetGridDet()
	{
		try
		{
			DGrid.Rows.Clear();
			foreach (GharardadDetNew item in _clsGharardad.ListGharardadDet)
			{
				DGrid.Rows.Add();
				DGrid[((DataGridViewBand)DGHesabID).Index, DGrid.RowCount - 1].Value = item.HesabId.ToString();
				DGrid[((DataGridViewBand)DGName).Index, DGrid.RowCount - 1].Value = item.Name;
				if (item.TypeAccount == 1)
				{
					DGrid[((DataGridViewBand)DGCredit).Index, DGrid.RowCount - 1].Value = item.nValue.ToString("N0");
				}
				else
				{
					DGrid[((DataGridViewBand)DGDebit).Index, DGrid.RowCount - 1].Value = item.nValue.ToString("N0");
				}
			}
			bool flag = _clsGharardad.ListGharardadDet.Any((GharardadDetNew q) => q.HesabId == -1);
			string text = "بدلیل وجود فرمول در مزایا و کسورات مبالغ آنها در هنگام صدور فیش حقوقی تعیین میشود بنابراین در اینجا مبلغ آن 1- در نظر گرفته شده و در جمع مزایا و کسورات محاسبه نشده است";
			if (trDictionary.TryGetValue("[بدلیل وجود فرمول در مزایا و کسورات مبالغ آنها در هنگام صدور فیش حقوقی تعیین میشود بنابراین در اینجا مبلغ آن 1- در نظر گرفته شده و در جمع مزایا و کسورات محاسبه نشده است]", out var value))
			{
				text = value;
			}
			string text2 = "شما با استفاده از دکمه مزایا و کسورات فرمولی میتوانید هر نوع مزایا و یا کسورات را بصورت فرمولی تعریف کنید ";
			if (trDictionary.TryGetValue("[شما با استفاده از دکمه مزایا و کسورات فرمولی میتوانید هر نوع مزایا و یا کسورات را بصورت فرمولی تعریف کنید ]", out var value2))
			{
				text2 = value2;
			}
			((Control)LblDescFormula).Text = (flag ? text : text2);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetGridDet", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1276);
		}
	}

	private bool CheckRepeat(int hesabId)
	{
		try
		{
			for (int i = 0; i < DGrid.RowCount; i++)
			{
				if (hesabId == int.Parse(DGrid[((DataGridViewBand)DGHesabID).Index, i].Value.ToString()))
				{
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CheckRepeat", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1293);
		}
		return false;
	}

	private void DGrid_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		((PopupItemControl)BTNEdit).PerformClick();
	}

	private async void BtnMazayaFormula_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			FrmFormula frm = new FrmFormula(((CacheBaseForm)this).ContextGuid, _clsGharardad);
			await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this);
			if ((int)((Form)frm).DialogResult == 1)
			{
				_clsGharardad.ListGharardadDet.Add(frm.GharardadDet);
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnMazayaFormula_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1318);
		}
	}

	private void TabControlPanel3_Enter(object sender, EventArgs e)
	{
	}

	private async void TxtSanavat_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSanavat_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1353);
		}
	}

	private async void TxtSalary_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			if (_clsGharardad.SalaryType == 0)
			{
				_clsGharardad.SalaryType = 2;
				((ListControl)CmbSalaryType).SelectedValue = _clsGharardad.SalaryType;
			}
			if (!decimal.TryParse(TxtSalary.Value.ToString(), out var result))
			{
				((Control)TxtSalary).Text = "0";
			}
			_clsGharardad.SetSalaryPrice(result);
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtSalary_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1376);
		}
	}

	private async Task SumHoghMazayaAsync(CancellationToken token)
	{
		try
		{
			await Task.Delay(100);
			if (token.IsCancellationRequested)
			{
				return;
			}
			int num = 0;
			if (((ListControl)cmbGroupSanavat).SelectedValue != null)
			{
				num = Convert.ToInt32(((ListControl)cmbGroupSanavat).SelectedValue.ToString());
			}
			if (((Control)TxtSanavat).Text != "" && num > 0)
			{
				await _clsGharardad.AddNewGharardadDetAsync(4, (short)TXTStartDate.Year, num, Convert.ToInt32(((Control)TxtSanavat).Text), token);
			}
			else
			{
				_clsGharardad.ListGharardadDet.Remove(_clsGharardad.GetHesabId(4));
			}
			if (chBxChild.Checked && NumberCountChild.Value > 0m)
			{
				await _clsGharardad.AddNewGharardadDetAsync(2, NumberCountChild.Value, token);
			}
			else
			{
				_clsGharardad.ListGharardadDet.Remove(_clsGharardad.GetHesabId(2));
			}
			if (ChbMaskan.Checked)
			{
				await _clsGharardad.AddNewGharardadDetAsync(1, 1m, token);
			}
			else
			{
				_clsGharardad.ListGharardadDet.Remove(_clsGharardad.GetHesabId(1));
			}
			if (ChbHaghTaahol.Checked)
			{
				await _clsGharardad.AddNewGharardadDetAsync(7, 1m, token);
			}
			else
			{
				_clsGharardad.ListGharardadDet.Remove(_clsGharardad.GetHesabId(7));
			}
			if (chBxKLhoarBar.Checked)
			{
				await _clsGharardad.AddNewGharardadDetAsync(3, 1m, token);
			}
			else
			{
				_clsGharardad.ListGharardadDet.Remove(_clsGharardad.GetHesabId(3));
			}
			if (chbxTaxOption.Checked)
			{
				await _clsGharardad.AddNewGharardadDetAsync(6, 1m, token);
			}
			else
			{
				_clsGharardad.ListGharardadDet.Remove(_clsGharardad.GetHesabId(6));
			}
			if (rbLeastLawLabor.Checked || rbPayloadCalculations.Checked)
			{
				await _clsGharardad.AddNewGharardadDetAsync(5, 1m, token);
			}
			else
			{
				_clsGharardad.ListGharardadDet.Remove(_clsGharardad.GetHesabId(5));
			}
			SetGridDet();
			_sumkosor = 0m;
			_sumMazaya = 0m;
			foreach (GharardadDetNew item in _clsGharardad.ListGharardadDet)
			{
				if (!(item.nValue == -1m))
				{
					if (item.TypeAccount == 1)
					{
						_sumMazaya += item.nValue;
					}
					else
					{
						_sumkosor += item.nValue;
					}
				}
			}
			decimal mondehMazayaKosorat = _sumMazaya - _sumkosor;
			string LblSumHogh_Text = _clsGharardad.SalaryOneMonth.ToString("N0");
			string LblSumHoghMazaya_Text = (_clsGharardad.SalaryOneMonth + mondehMazayaKosorat).ToString("N0");
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)LblSumkosor).Text = _sumkosor.ToString("N0");
						((Control)LblSumMazaya).Text = _sumMazaya.ToString("N0");
						((Control)LblMandeh).Text = mondehMazayaKosorat.ToString("N0");
						((Control)LblSumHogh).Text = LblSumHogh_Text;
						((Control)LblSumMazayaKosorat).Text = mondehMazayaKosorat.ToString("N0");
						((Control)LblSumHoghMazaya).Text = LblSumHoghMazaya_Text;
					}
				}
				catch (OperationCanceledException)
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SumHoghMazayaAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1463);
		}
	}

	private async void ChbHaghTahol_CheckValueChanged(object sender, EventArgs e)
	{
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ChbHaghTahol_CheckValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1476);
		}
	}

	private async void ChBxChild_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async void BtnCalOverLowTime_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (await CalculatePriceOverTimeAsync())
			{
				_changeToken?.Cancel();
				_changeToken = new CancellationTokenSource();
				CancellationToken token = _changeToken.Token;
				await SumHoghMazayaAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnCalOverLowTime_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1508);
		}
	}

	private async Task<bool> CalculatePriceOverTimeAsync(bool showUi = true)
	{
		string errMsg = $"TxtSalary.Value={TxtSalary.Value}";
		try
		{
			errMsg = errMsg + "TxtLeaveDays.Text=" + ((Control)TxtLeaveDays).Text;
			FrmCalOverLowTime frm = new FrmCalOverLowTime(((CacheBaseForm)this).ContextGuid, _clsGharardad);
			if (showUi)
			{
				if ((int)(await ((CacheBaseForm)frm).ShowAsync((IWin32Window)(object)this)) == 1)
				{
					TextBoxFa txtPriceLowTime = TxtPriceLowTime;
					TextBoxFa txtPriceLowTime2 = TxtPriceLowTime2;
					string text2 = (((Control)TxtPriceLowTime3).Text = frm.GetPriceLow().ToString());
					string text4 = (((Control)txtPriceLowTime2).Text = text2);
					((Control)txtPriceLowTime).Text = text4;
					TextBoxFa txtPriceOverTime = TxtPriceOverTime;
					TextBoxFa txtPriceOverTime2 = TxtPriceOverTime2;
					text2 = (((Control)TxtPriceOverTime3).Text = frm.GetPriceEzafe().ToString());
					text4 = (((Control)txtPriceOverTime2).Text = text2);
					((Control)txtPriceOverTime).Text = text4;
					((Control)TxtPriceHoliday).Text = frm.GetPriceFriday().ToString();
					((Control)TxtPriceTatil).Text = frm.GetPriceTatil().ToString();
					return true;
				}
			}
			else
			{
				TextBoxFa txtPriceLowTime3 = TxtPriceLowTime;
				TextBoxFa txtPriceLowTime4 = TxtPriceLowTime2;
				string text2 = (((Control)TxtPriceLowTime3).Text = frm.GetPriceLow().ToString());
				string text4 = (((Control)txtPriceLowTime4).Text = text2);
				((Control)txtPriceLowTime3).Text = text4;
				TextBoxFa txtPriceOverTime3 = TxtPriceOverTime;
				TextBoxFa txtPriceOverTime4 = TxtPriceOverTime2;
				text2 = (((Control)TxtPriceOverTime3).Text = frm.GetPriceEzafe().ToString());
				text4 = (((Control)txtPriceOverTime4).Text = text2);
				((Control)txtPriceOverTime3).Text = text4;
				((Control)TxtPriceHoliday).Text = frm.GetPriceFriday().ToString();
				((Control)TxtPriceTatil).Text = frm.GetPriceTatil().ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "errMsg=" + errMsg, "CalculatePriceOverTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1538);
		}
		return false;
	}

	protected override async void OnKeyDown(KeyEventArgs e)
	{
		try
		{
			_003C_003En__0(e);
			Control sender = ((ContainerControl)this).ActiveControl;
			if ((int)e.KeyData == 32)
			{
				e.Handled = true;
			}
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			if ((int)e.KeyData == 32)
			{
				if (sender.Name == ((Control)TxtSalary).Name)
				{
					((Control)TxtSalary).Text = _clsGharardad.SalaryPrice.ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceOverTime).Name)
				{
					((Control)TxtPriceOverTime).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * (decimal)(timeAttendanceSettings.ZaribEzafeDay + 100) / 100m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceOverTime2).Name)
				{
					((Control)TxtPriceOverTime2).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * (decimal)(timeAttendanceSettings.ZaribEzafeDay + 100) / 100m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceOverTime3).Name)
				{
					((Control)TxtPriceOverTime3).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * (decimal)(timeAttendanceSettings.ZaribEzafeDay + 100) / 100m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceHoliday).Name)
				{
					((Control)TxtPriceHoliday).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * (decimal)(timeAttendanceSettings.ZaribFriday + 100) / 100m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceTatil).Name)
				{
					((Control)TxtPriceTatil).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * (decimal)(timeAttendanceSettings.ZaribTatil + 100) / 100m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceLowTime).Name)
				{
					((Control)TxtPriceLowTime).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * 100m / 100m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceLowTime2).Name)
				{
					((Control)TxtPriceLowTime2).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * 100m / 100m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceLowTime3).Name)
				{
					((Control)TxtPriceLowTime3).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * 100m / 100m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceNobatkari1).Name)
				{
					((Control)TxtPriceNobatkari1).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * 1.1m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceNobatkari2).Name)
				{
					((Control)TxtPriceNobatkari2).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * 1.15m).ToString("N0");
				}
				else if (sender.Name == ((Control)TxtPriceNobatkari3).Name)
				{
					((Control)TxtPriceNobatkari3).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * 1.225m).ToString("N0");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnKeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1592);
		}
	}

	private async void BtnUpdateParametersSalary_Click(object sender, EventArgs e)
	{
		try
		{
			string messege = "آیا مبالغ حقوق روزانه، اضافه کار، جمعه کاری، تعطیل کاری و غیره طبق قانون کار تامین اجتماعی جایگزین شود؟";
			if (trDictionary.TryGetValue("[آیا مبالغ حقوق روزانه، اضافه کار، جمعه کاری، تعطیل کاری و غیره طبق قانون کار تامین اجتماعی جایگزین شود؟]", out var value))
			{
				messege = value;
			}
			string title = "توجه";
			if (trDictionary.TryGetValue("[توجه]", out var value2))
			{
				title = value2;
			}
			if ((int)FMessageBox.Show(messege, title, FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Information, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				_changeToken?.Cancel();
				_changeToken = new CancellationTokenSource();
				_ = _changeToken.Token;
				TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
				_clsGharardad.SalaryType = 2;
				((ListControl)CmbSalaryType).SelectedValue = _clsGharardad.SalaryType;
				_clsGharardad.SetSalaryPrice(timeAttendanceSettings.LaborLaw.DailyWage);
				((Control)TxtSalary).Text = _clsGharardad.SalaryPrice.ToString(CultureInfo.CurrentCulture);
				TextBoxFa txtPriceOverTime = TxtPriceOverTime;
				TextBoxFa txtPriceOverTime2 = TxtPriceOverTime2;
				string text2 = (((Control)TxtPriceOverTime3).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * timeAttendanceSettings.LaborLaw.OvertimeCoefficient).ToString(CultureInfo.CurrentCulture));
				string text4 = (((Control)txtPriceOverTime2).Text = text2);
				((Control)txtPriceOverTime).Text = text4;
				((Control)TxtPriceHoliday).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * (decimal)(100 + timeAttendanceSettings.ZaribFriday) / 100m).ToString(CultureInfo.CurrentCulture);
				((Control)TxtPriceTatil).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * (decimal)(100 + timeAttendanceSettings.ZaribTatil) / 100m).ToString(CultureInfo.CurrentCulture);
				TextBoxFa txtPriceLowTime = TxtPriceLowTime;
				TextBoxFa txtPriceLowTime2 = TxtPriceLowTime2;
				text2 = (((Control)TxtPriceLowTime3).Text = (_clsGharardad.SalaryOneHourWithBaseSanavat * 1m).ToString(CultureInfo.CurrentCulture));
				text4 = (((Control)txtPriceLowTime2).Text = text2);
				((Control)txtPriceLowTime).Text = text4;
				((Control)TxtPriceNobatkari1).Text = "";
				((Control)TxtPriceNobatkari2).Text = "";
				((Control)TxtPriceNobatkari3).Text = "";
				((Control)TxtPriceMission).Text = "";
				((Control)TxtPriceAbsent).Text = "";
				((Control)TxtPriceLeave).Text = "";
				((Control)TxtPriceNight).Text = "";
				if (UserAttendance.SelectedUser.AccessLevelMerg.Gharardad.GharardadHaghs)
				{
					CheckBoxX obj = chBxKLhoarBar;
					bool @checked = (ChbMaskan.Checked = true);
					obj.Checked = @checked;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BtnUpdateParametersSalary_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1630);
		}
	}

	private void RbCheckoutVacation_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rbCheckoutVacation.Checked)
			{
				int num = ((Utilities.CalendarToMiladi(((Control)TxtEndDate).Text).Date - Utilities.CalendarToMiladi(((Control)TXTStartDate).Text).Date).Days + 2) / 30;
				if (num <= 0)
				{
					num = 1;
				}
				((Control)TxtLeaveDays).Text = (Utilities.ParseToDouble(((Control)TxtLeaveDays).Text, 0.0, (short)3) / (double)num).ToString("N2");
				string text = "حق مرخصی در هر ماه(روز)";
				if (trDictionary.TryGetValue("[حق مرخصی در هر ماه(روز)]", out var value))
				{
					text = value;
				}
				((Control)LblVacation).Text = text;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RbCheckoutVacation_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1647);
		}
	}

	private void RbNotCheckoutVacation_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rbNotCheckoutVacation.Checked)
			{
				double num = (Utilities.CalendarToMiladi(((Control)TxtEndDate).Text).Date - Utilities.CalendarToMiladi(((Control)TXTStartDate).Text).Date).Days + 1;
				if (num <= 0.0)
				{
					num = 0.0;
				}
				double value = 0.07123 * num;
				((Control)TxtLeaveDays).Text = Math.Round(value, 2).ToString("N2");
				trDictionary.TryGetValue("[حق مرخصی در]", out var _);
				trDictionary.TryGetValue("[ ماه (روز)]", out var _);
				((Control)LblVacation).Text = $"حق مرخصی در مدت {num} روز:";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "RbNotCheckoutVacation_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1672);
		}
	}

	private async void NumberCountChild_ValueChanged(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
			if (NumberCountChild.Value > (decimal)timeAttendanceSettings.MaxTedadOlaadInSalary)
			{
				NumberCountChild.Value = timeAttendanceSettings.MaxTedadOlaadInSalary;
			}
			if ((decimal)_clsGharardad.CountChild != NumberCountChild.Value)
			{
				_clsGharardad.ListGharardadDet.Remove(_clsGharardad.GetHesabId(2));
				_clsGharardad.CountChild = (short)NumberCountChild.Value;
			}
			_changeToken?.Cancel();
			_changeToken = new CancellationTokenSource();
			CancellationToken token = _changeToken.Token;
			await SumHoghMazayaAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "NumberCountChild_ValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1694);
		}
	}

	private void TXTStartAndEndDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			RbNotCheckoutVacation_CheckedChanged(new object(), new EventArgs());
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTStartAndEndDate_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1703);
		}
	}

	private async Task SetTooltipAsync()
	{
		TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(((CacheBaseForm)this).ContextGuid);
		try
		{
			((Control)(object)LblPriceOverTime).SetTooltip($"[مبلغ یک ساعت حقوق پایه] * {100 + timeAttendanceSettings.ZaribEzafeDay}[درصد]", "[اضافه کاری یک ساعت]:", "", (eTooltipColor)0);
			((Control)(object)LblPriceHoliday).SetTooltip($"[مبلغ یک روز حقوق پایه] * {100 + timeAttendanceSettings.ZaribFriday}[درصد]", "[یک ساعت جمعه کاری]:", "", (eTooltipColor)0);
			((Control)(object)LblPriceTatil).SetTooltip($"[مبلغ یک ساعت حقوق پایه] * {100 + timeAttendanceSettings.ZaribTatil}[درصد]", "[یک ساعت تعطیل کاری]:", "", (eTooltipColor)0);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTooltipAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1728);
		}
	}

	private async void CmbSalaryType_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (((ListControl)CmbSalaryType).SelectedValue != null && !(((ListControl)CmbSalaryType).SelectedValue.ToString() == "0"))
			{
				_clsGharardad.SalaryType = short.Parse(((ListControl)CmbSalaryType).SelectedValue.ToString());
				((Control)TxtSalary).Text = _clsGharardad.SalaryPrice.ToString();
				_changeToken?.Cancel();
				_changeToken = new CancellationTokenSource();
				CancellationToken token = _changeToken.Token;
				await SumHoghMazayaAsync(token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CmbSalaryType_SelectedIndexChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1744);
		}
	}

	private void TxtLeavHoures_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			_clsGharardad.LeaveMinutes = (short)(Utilities.ParseToShort(((Control)TxtLeavHoures).Text, (short)0) * 60 + Utilities.ParseToShort(((Control)TxtLeavMinutes).Text, (short)0));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TxtLeavHoures_ValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Gharardads\\FRMGharardad.cs", 1753);
		}
	}

	private async void CmbShift_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)CmbShift).SelectedValue == null)
		{
			((ListControl)CmbShift).SelectedIndex = 0;
		}
		if (int.Parse(((ListControl)CmbShift).SelectedValue.ToString()) != -1)
		{
			return;
		}
		_changeToken?.Cancel();
		_changeToken = new CancellationTokenSource();
		CancellationToken token = _changeToken.Token;
		List<ShiftProgrammableDet> obj = await ShiftProgrammableDet.SelectRowByGharardadIDAsync(((CacheBaseForm)this).ContextGuid, _clsGharardad.ID, token);
		if (obj != null && obj.Count > 0)
		{
			string messege = "آیا اطلاعات قبلی شیفت برنامه ریزی شده این قرارداد حذف شود؟";
			if (trDictionary.TryGetValue("[آیا اطلاعات قبلی شیفت برنامه ریزی شده این قرارداد حذف شود؟]", out var value))
			{
				messege = value;
			}
			string title = "توجه";
			if (trDictionary.TryGetValue("[توجه]", out var value2))
			{
				title = value2;
			}
			if ((int)FMessageBox.Show(messege, title, FMessageBox.FMessageBoxButtons.YesNo, FMessageBox.FMessageBoxIcon.Information, FMessageBox.FMessageBoxDefaultButton.Button2) == 6)
			{
				await ShiftProgrammableDet.DeleteByGharardadAsync(((CacheBaseForm)this).ContextGuid, _clsGharardad.ID, token);
			}
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
				_changeToken?.Cancel();
				_changeToken?.Dispose();
				_changeToken = null;
				_searchToken?.Cancel();
				_searchToken?.Dispose();
				_searchToken = null;
				components?.Dispose();
				((FormExtention)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void FrmGharardad_FormClosing(object sender, FormClosingEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void tabControl1_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
	{
		try
		{
			if (tabControl1.SelectedTab != null && tabControl1.SelectedTab == tabItem2 && TxtSalary.Value == 0m)
			{
				btnUpdateParametersSalary.PerformClick();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
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
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Expected O, but got Unknown
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Expected O, but got Unknown
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Expected O, but got Unknown
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Expected O, but got Unknown
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Expected O, but got Unknown
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Expected O, but got Unknown
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Expected O, but got Unknown
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Expected O, but got Unknown
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Expected O, but got Unknown
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Expected O, but got Unknown
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Expected O, but got Unknown
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Expected O, but got Unknown
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Expected O, but got Unknown
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Expected O, but got Unknown
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Expected O, but got Unknown
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Expected O, but got Unknown
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Expected O, but got Unknown
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Expected O, but got Unknown
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Expected O, but got Unknown
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Expected O, but got Unknown
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Expected O, but got Unknown
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Expected O, but got Unknown
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Expected O, but got Unknown
		//IL_0b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b87: Expected O, but got Unknown
		//IL_0e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1086: Unknown result type (might be due to invalid IL or missing references)
		//IL_112f: Unknown result type (might be due to invalid IL or missing references)
		//IL_11da: Unknown result type (might be due to invalid IL or missing references)
		//IL_1278: Unknown result type (might be due to invalid IL or missing references)
		//IL_1308: Unknown result type (might be due to invalid IL or missing references)
		//IL_1399: Unknown result type (might be due to invalid IL or missing references)
		//IL_145e: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d9: Expected O, but got Unknown
		//IL_17fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1893: Unknown result type (might be due to invalid IL or missing references)
		//IL_189d: Expected O, but got Unknown
		//IL_18be: Unknown result type (might be due to invalid IL or missing references)
		//IL_1945: Unknown result type (might be due to invalid IL or missing references)
		//IL_194f: Expected O, but got Unknown
		//IL_1970: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0d: Expected O, but got Unknown
		//IL_1a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_206d: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2263: Unknown result type (might be due to invalid IL or missing references)
		//IL_234c: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_249f: Unknown result type (might be due to invalid IL or missing references)
		//IL_252c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2613: Unknown result type (might be due to invalid IL or missing references)
		//IL_26bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2763: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_297d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_3551: Unknown result type (might be due to invalid IL or missing references)
		//IL_3572: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_388f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3899: Expected O, but got Unknown
		//IL_3921: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af4: Expected O, but got Unknown
		//IL_3b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e08: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e12: Expected O, but got Unknown
		//IL_3e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f86: Expected O, but got Unknown
		//IL_3fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_406d: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ff: Expected O, but got Unknown
		//IL_4123: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4284: Unknown result type (might be due to invalid IL or missing references)
		//IL_428e: Expected O, but got Unknown
		//IL_42b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_433d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4347: Expected O, but got Unknown
		//IL_436b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4403: Unknown result type (might be due to invalid IL or missing references)
		//IL_440d: Expected O, but got Unknown
		//IL_4431: Unknown result type (might be due to invalid IL or missing references)
		//IL_44dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_456c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4650: Unknown result type (might be due to invalid IL or missing references)
		//IL_4711: Unknown result type (might be due to invalid IL or missing references)
		//IL_47ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_485b: Unknown result type (might be due to invalid IL or missing references)
		//IL_48f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c18: Expected O, but got Unknown
		//IL_4c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d75: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e10: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eb2: Expected O, but got Unknown
		//IL_4ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_5061: Unknown result type (might be due to invalid IL or missing references)
		//IL_5184: Unknown result type (might be due to invalid IL or missing references)
		//IL_51fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5293: Unknown result type (might be due to invalid IL or missing references)
		//IL_5360: Unknown result type (might be due to invalid IL or missing references)
		//IL_53f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_549d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5538: Unknown result type (might be due to invalid IL or missing references)
		//IL_55fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_56a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5743: Unknown result type (might be due to invalid IL or missing references)
		//IL_57fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_58b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_59d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a94: Expected O, but got Unknown
		//IL_5b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c33: Expected O, but got Unknown
		//IL_5d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dd2: Expected O, but got Unknown
		//IL_5ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f71: Expected O, but got Unknown
		//IL_605c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6116: Unknown result type (might be due to invalid IL or missing references)
		//IL_6120: Expected O, but got Unknown
		//IL_620b: Unknown result type (might be due to invalid IL or missing references)
		//IL_62c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_62cf: Expected O, but got Unknown
		//IL_63aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_6464: Unknown result type (might be due to invalid IL or missing references)
		//IL_646e: Expected O, but got Unknown
		//IL_6559: Unknown result type (might be due to invalid IL or missing references)
		//IL_6613: Unknown result type (might be due to invalid IL or missing references)
		//IL_661d: Expected O, but got Unknown
		//IL_6705: Unknown result type (might be due to invalid IL or missing references)
		//IL_67bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_67c7: Expected O, but got Unknown
		//IL_68a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_695c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6966: Expected O, but got Unknown
		//IL_6a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_6afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b05: Expected O, but got Unknown
		//IL_6bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cb4: Expected O, but got Unknown
		//IL_6d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e53: Expected O, but got Unknown
		//IL_6f3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_7002: Expected O, but got Unknown
		//IL_70ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_71a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_71b1: Expected O, but got Unknown
		//IL_7279: Unknown result type (might be due to invalid IL or missing references)
		//IL_7332: Unknown result type (might be due to invalid IL or missing references)
		//IL_733c: Expected O, but got Unknown
		//IL_75ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_760d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7747: Unknown result type (might be due to invalid IL or missing references)
		//IL_780d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7896: Unknown result type (might be due to invalid IL or missing references)
		//IL_7968: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_7aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_7da0: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_803b: Unknown result type (might be due to invalid IL or missing references)
		//IL_80ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_80d4: Expected O, but got Unknown
		//IL_80f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_819f: Unknown result type (might be due to invalid IL or missing references)
		//IL_822f: Unknown result type (might be due to invalid IL or missing references)
		//IL_8239: Expected O, but got Unknown
		//IL_825a: Unknown result type (might be due to invalid IL or missing references)
		//IL_8303: Unknown result type (might be due to invalid IL or missing references)
		//IL_83d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_8482: Unknown result type (might be due to invalid IL or missing references)
		//IL_8544: Unknown result type (might be due to invalid IL or missing references)
		//IL_8611: Unknown result type (might be due to invalid IL or missing references)
		//IL_861b: Expected O, but got Unknown
		//IL_8647: Unknown result type (might be due to invalid IL or missing references)
		//IL_8651: Expected O, but got Unknown
		//IL_866f: Unknown result type (might be due to invalid IL or missing references)
		//IL_87c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_87ce: Expected O, but got Unknown
		//IL_87fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_8804: Expected O, but got Unknown
		//IL_8822: Unknown result type (might be due to invalid IL or missing references)
		//IL_8984: Unknown result type (might be due to invalid IL or missing references)
		//IL_898e: Expected O, but got Unknown
		//IL_89ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_89c4: Expected O, but got Unknown
		//IL_89e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_8c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_8dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_8e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_8e69: Expected O, but got Unknown
		//IL_8fe8: Unknown result type (might be due to invalid IL or missing references)
		//IL_8ff2: Expected O, but got Unknown
		//IL_9022: Unknown result type (might be due to invalid IL or missing references)
		//IL_90b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_90ba: Expected O, but got Unknown
		//IL_90eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_9177: Unknown result type (might be due to invalid IL or missing references)
		//IL_9181: Expected O, but got Unknown
		//IL_91b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_923f: Unknown result type (might be due to invalid IL or missing references)
		//IL_9249: Expected O, but got Unknown
		//IL_927a: Unknown result type (might be due to invalid IL or missing references)
		//IL_9305: Unknown result type (might be due to invalid IL or missing references)
		//IL_930f: Expected O, but got Unknown
		//IL_9340: Unknown result type (might be due to invalid IL or missing references)
		//IL_93cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_93d5: Expected O, but got Unknown
		//IL_9406: Unknown result type (might be due to invalid IL or missing references)
		//IL_9493: Unknown result type (might be due to invalid IL or missing references)
		//IL_949d: Expected O, but got Unknown
		//IL_94ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_9553: Unknown result type (might be due to invalid IL or missing references)
		//IL_955d: Expected O, but got Unknown
		//IL_9597: Unknown result type (might be due to invalid IL or missing references)
		//IL_9658: Unknown result type (might be due to invalid IL or missing references)
		//IL_9662: Expected O, but got Unknown
		//IL_96a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_9768: Unknown result type (might be due to invalid IL or missing references)
		//IL_9772: Expected O, but got Unknown
		//IL_97b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_97d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_9889: Unknown result type (might be due to invalid IL or missing references)
		//IL_9893: Expected O, but got Unknown
		//IL_98d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_999a: Unknown result type (might be due to invalid IL or missing references)
		//IL_99a4: Expected O, but got Unknown
		//IL_99ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_9af7: Unknown result type (might be due to invalid IL or missing references)
		//IL_9b01: Expected O, but got Unknown
		//IL_9b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_9be9: Unknown result type (might be due to invalid IL or missing references)
		//IL_9bf3: Expected O, but got Unknown
		//IL_9c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_9d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_9de1: Unknown result type (might be due to invalid IL or missing references)
		//IL_9e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_9f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_9ff2: Unknown result type (might be due to invalid IL or missing references)
		//IL_a09d: Unknown result type (might be due to invalid IL or missing references)
		//IL_a150: Unknown result type (might be due to invalid IL or missing references)
		//IL_a15a: Expected O, but got Unknown
		//IL_a166: Unknown result type (might be due to invalid IL or missing references)
		//IL_a1b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_a1c2: Expected O, but got Unknown
		components = new Container();
		panelEx1 = new PanelEx();
		tabControl1 = new TabControl();
		tabControlPanel3 = new TabControlPanel();
		TxtBankNo = new TextBoxX();
		labelX27 = new LabelX();
		labelX33 = new LabelX();
		TxtShabaNo = new TextBoxX();
		TxtJob = new TextBoxX();
		labelX34 = new LabelX();
		BtnMoreJob = new ButtonX();
		BtnDelJob = new ButtonX();
		groupPanelTaradod = new GroupPanel();
		chbxTaxOption = new CheckBoxX();
		rbPayloadCalculations = new RadioButton();
		rbLeastLawLabor = new RadioButton();
		rbNotInsurance = new RadioButton();
		txtUnemployment = new TextBoxX();
		txtInsured = new TextBoxX();
		txtEmployer = new TextBoxX();
		labelX45 = new LabelX();
		labelX43 = new LabelX();
		labelX41 = new LabelX();
		labelX21 = new LabelX();
		CmbBank = new ComboBoxEx();
		CmbBankType = new ComboBoxEx();
		BtnMorebank = new ButtonX();
		BtnMorePlace = new ButtonX();
		BtnMorebankType = new ButtonX();
		CmbPlace = new ComboBoxEx();
		labelX39 = new LabelX();
		labelX32 = new LabelX();
		BtnMoreRecruitmentType = new ButtonX();
		CmbRecruitmentType = new ComboBoxEx();
		labelX38 = new LabelX();
		labelX36 = new LabelX();
		BtnMoreRecruitmentST = new ButtonX();
		labelX35 = new LabelX();
		CmbRecruitmentST = new ComboBoxEx();
		CmbPart = new ComboBoxEx();
		BtnDelPlace = new ButtonX();
		BtnDelRecruitmentType = new ButtonX();
		BtnDelRecruitmentST = new ButtonX();
		BtnMorePart = new ButtonX();
		BtnDelPart = new ButtonX();
		BtnDelBankType = new ButtonX();
		BtnDelBank = new ButtonX();
		tabItem3 = new TabItem(components);
		tabControlPanel2 = new TabControlPanel();
		LblPriceNobatkari3 = new LabelX();
		TxtPriceNobatkari3 = new TextBoxFa();
		LblPriceNobatkari2 = new LabelX();
		TxtPriceNobatkari2 = new TextBoxFa();
		labelX23 = new LabelX();
		LblVacation = new LabelX();
		TxtLeaveDays = new TextBoxX();
		LblPriceJarime = new LabelX();
		rbNotCheckoutVacation = new RadioButton();
		LblPriceMission = new LabelX();
		rbCheckoutVacation = new RadioButton();
		LblPriceTatil = new LabelX();
		LblPriceLowTime3 = new LabelX();
		btnUpdateParametersSalary = new Button();
		LblPriceOverTime3 = new LabelX();
		LblPriceLowTime2 = new LabelX();
		LblPriceOverTime2 = new LabelX();
		LblPriceNobatkari1 = new LabelX();
		BtnAddGroupSanavat = new ButtonX();
		ChbMaskan = new CheckBoxX();
		chBxKLhoarBar = new CheckBoxX();
		ChbHaghTaahol = new CheckBoxX();
		chBxChild = new CheckBoxX();
		labelX16 = new LabelX();
		cmbGroupSanavat = new ComboBoxEx();
		labelX18 = new LabelX();
		TxtLeavHoures = new NumericUpDown();
		TxtLeavMinutes = new NumericUpDown();
		LblPriceLowTime = new LabelX();
		labelX31 = new LabelX();
		labelX17 = new LabelX();
		labelX40 = new LabelX();
		LblPriceOverTime = new LabelX();
		LblPriceLeave = new LabelX();
		CmbMariage = new ComboBoxEx();
		CmbMarried = new ComboItem();
		CmbSingle = new ComboItem();
		LblPriceHoliday = new LabelX();
		TxtLeaveSaved = new NumericUpDown();
		NumberCountChild = new NumericUpDown();
		LblPriceNight = new LabelX();
		labelX6 = new LabelX();
		labelX20 = new LabelX();
		LblPriceAbsent = new LabelX();
		CmbSalaryType = new ComboBoxEx();
		labelX19 = new LabelX();
		BtnDelGroupSanavat = new ButtonX();
		labelX4 = new LabelX();
		btnCalOverLowTime = new ButtonX();
		TxtPriceJarime = new TextBoxFa();
		TxtPriceMission = new TextBoxFa();
		TxtPriceTatil = new TextBoxFa();
		TxtPriceLowTime3 = new TextBoxFa();
		TxtPriceOverTime3 = new TextBoxFa();
		TxtPriceNobatkari1 = new TextBoxFa();
		TxtPriceLowTime2 = new TextBoxFa();
		TxtPriceOverTime2 = new TextBoxFa();
		TxtSanavat = new TextBoxFa();
		TxtPriceLowTime = new TextBoxFa();
		TxtPriceLeave = new TextBoxFa();
		TxtPriceOverTime = new TextBoxFa();
		TxtPriceAbsent = new TextBoxFa();
		TxtPriceHoliday = new TextBoxFa();
		TxtPriceNight = new TextBoxFa();
		TxtSalary = new TextBoxFa();
		tabItem2 = new TabItem(components);
		tabControlPanel1 = new TabControlPanel();
		labelX22 = new LabelX();
		CmbGharardadStatus = new ComboBoxEx();
		numberCod = new NumericUpDown();
		BtnmoreCompany = new ButtonX();
		lblname = new LabelX();
		labelX12 = new LabelX();
		BtnMoreP = new ButtonX();
		labelX11 = new LabelX();
		TxtcodePerson = new TextBoxX();
		labelX5 = new LabelX();
		BTNMore = new ButtonX();
		labelX3 = new LabelX();
		labelX2 = new LabelX();
		labelX1 = new LabelX();
		labelX8 = new LabelX();
		TxtPeyvast = new TextBoxX();
		labelX10 = new LabelX();
		TxtDesc = new TextBoxX();
		CmbShift = new ComboBoxEx();
		labelX7 = new LabelX();
		CmbCompany = new ComboBoxEx();
		TXTDateRegister = new DateBoxFa();
		TxtEndDate = new DateBoxFa();
		TXTStartDate = new DateBoxFa();
		tabItem1 = new TabItem(components);
		tabControlPanel4 = new TabControlPanel();
		DGrid = new DataGridView();
		DGGharardadID = new DataGridViewTextBoxColumn();
		DGHesabID = new DataGridViewTextBoxColumn();
		DGName = new DataGridViewTextBoxColumn();
		DGCredit = new DataGridViewTextBoxColumn();
		DGDebit = new DataGridViewTextBoxColumn();
		LblDescFormula = new LabelX();
		LblMandeh = new LabelX();
		labelX13 = new LabelX();
		LblSumkosor = new LabelX();
		labelX15 = new LabelX();
		labelX14 = new LabelX();
		LblSumMazaya = new LabelX();
		BTNEdit = new ButtonX();
		BTNADDMazaya = new ButtonX();
		btnKosoratFormula = new ButtonX();
		BTNAddKosorat = new ButtonX();
		BTNDelete = new ButtonX();
		tabItem4 = new TabItem(components);
		BtnCancel = new ButtonX();
		BtnAccept = new ButtonX();
		LblSumMazayaKosorat = new LabelX();
		LblSumHogh = new LabelX();
		LblSumHoghMazaya = new LabelX();
		labelX42 = new LabelX();
		labelX37 = new LabelX();
		labelX29 = new LabelX();
		((Control)panelEx1).SuspendLayout();
		((ISupportInitialize)tabControl1).BeginInit();
		((Control)tabControl1).SuspendLayout();
		((Control)tabControlPanel3).SuspendLayout();
		((Control)groupPanelTaradod).SuspendLayout();
		((Control)tabControlPanel2).SuspendLayout();
		((ISupportInitialize)TxtLeavHoures).BeginInit();
		((ISupportInitialize)TxtLeavMinutes).BeginInit();
		((ISupportInitialize)TxtLeaveSaved).BeginInit();
		((ISupportInitialize)NumberCountChild).BeginInit();
		((Control)tabControlPanel1).SuspendLayout();
		((ISupportInitialize)numberCod).BeginInit();
		((Control)tabControlPanel4).SuspendLayout();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		panelEx1.CanvasColor = SystemColors.Control;
		panelEx1.ColorSchemeStyle = (eDotNetBarStyle)9;
		((Control)panelEx1).Controls.Add((Control)(object)tabControl1);
		((Control)panelEx1).Controls.Add((Control)(object)BtnCancel);
		((Control)panelEx1).Controls.Add((Control)(object)BtnAccept);
		((Control)panelEx1).Controls.Add((Control)(object)LblSumMazayaKosorat);
		((Control)panelEx1).Controls.Add((Control)(object)LblSumHogh);
		((Control)panelEx1).Controls.Add((Control)(object)LblSumHoghMazaya);
		((Control)panelEx1).Controls.Add((Control)(object)labelX42);
		((Control)panelEx1).Controls.Add((Control)(object)labelX37);
		((Control)panelEx1).Controls.Add((Control)(object)labelX29);
		panelEx1.DisabledBackColor = Color.Empty;
		((Control)panelEx1).Dock = (DockStyle)5;
		((Control)panelEx1).Location = new Point(0, 0);
		((Control)panelEx1).Margin = new Padding(4, 3, 4, 3);
		((Control)panelEx1).Name = "panelEx1";
		((Control)panelEx1).Size = new Size(752, 577);
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
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel2);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel4);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel3);
		((Control)tabControl1).Controls.Add((Control)(object)tabControlPanel1);
		((Control)tabControl1).Location = new Point(12, 12);
		((Control)tabControl1).Margin = new Padding(4, 3, 4, 3);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedTabFont = new Font("Tahoma", 8.25f, (FontStyle)1);
		tabControl1.SelectedTabIndex = 0;
		((Control)tabControl1).Size = new Size(728, 450);
		((Control)tabControl1).TabIndex = 0;
		tabControl1.TabLayoutType = (eTabLayoutType)1;
		tabControl1.Tabs.Add(tabItem1);
		tabControl1.Tabs.Add(tabItem2);
		tabControl1.Tabs.Add(tabItem3);
		tabControl1.Tabs.Add(tabItem4);
		((Control)tabControl1).TabStop = false;
		((Control)tabControl1).Text = "tabControl1";
		tabControl1.SelectedTabChanged += new SelectedTabChangedEventHandler(tabControl1_SelectedTabChanged);
		((Control)tabControlPanel3).Controls.Add((Control)(object)TxtBankNo);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX27);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX33);
		((Control)tabControlPanel3).Controls.Add((Control)(object)TxtShabaNo);
		((Control)tabControlPanel3).Controls.Add((Control)(object)TxtJob);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX34);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnMoreJob);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnDelJob);
		((Control)tabControlPanel3).Controls.Add((Control)(object)groupPanelTaradod);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX21);
		((Control)tabControlPanel3).Controls.Add((Control)(object)CmbBank);
		((Control)tabControlPanel3).Controls.Add((Control)(object)CmbBankType);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnMorebank);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnMorePlace);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnMorebankType);
		((Control)tabControlPanel3).Controls.Add((Control)(object)CmbPlace);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX39);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX32);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnMoreRecruitmentType);
		((Control)tabControlPanel3).Controls.Add((Control)(object)CmbRecruitmentType);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX38);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX36);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnMoreRecruitmentST);
		((Control)tabControlPanel3).Controls.Add((Control)(object)labelX35);
		((Control)tabControlPanel3).Controls.Add((Control)(object)CmbRecruitmentST);
		((Control)tabControlPanel3).Controls.Add((Control)(object)CmbPart);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnDelPlace);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnDelRecruitmentType);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnDelRecruitmentST);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnMorePart);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnDelPart);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnDelBankType);
		((Control)tabControlPanel3).Controls.Add((Control)(object)BtnDelBank);
		((PanelEx)tabControlPanel3).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel3).Dock = (DockStyle)5;
		((Control)tabControlPanel3).Location = new Point(0, 26);
		((Control)tabControlPanel3).Margin = new Padding(4, 3, 4, 3);
		((Control)tabControlPanel3).Name = "tabControlPanel3";
		((Control)tabControlPanel3).Padding = new Padding(1);
		((Control)tabControlPanel3).Size = new Size(728, 424);
		((PanelEx)tabControlPanel3).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel3).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel3).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel3).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel3).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel3).Style.GradientAngle = 90;
		((Control)tabControlPanel3).TabIndex = 0;
		tabControlPanel3.TabItem = tabItem3;
		((Control)TxtBankNo).Anchor = (AnchorStyles)13;
		TxtBankNo.Border.Class = "TextBoxBorder";
		TxtBankNo.Border.CornerType = (eCornerType)1;
		((Control)TxtBankNo).Location = new Point(74, 209);
		((Control)TxtBankNo).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtBankNo).Name = "TxtBankNo";
		((Control)TxtBankNo).Size = new Size(533, 20);
		((Control)TxtBankNo).TabIndex = 35;
		((Control)labelX27).Anchor = (AnchorStyles)9;
		((Control)labelX27).BackColor = Color.Transparent;
		((BaseItemControl)labelX27).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX27).Location = new Point(610, 209);
		((Control)labelX27).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX27).Name = "labelX27";
		((Control)labelX27).Size = new Size(102, 21);
		((Control)labelX27).TabIndex = 34;
		((Control)labelX27).Text = "[شماره حساب]:";
		labelX27.TextAlignment = (StringAlignment)2;
		((Control)labelX33).Anchor = (AnchorStyles)9;
		((Control)labelX33).BackColor = Color.Transparent;
		((BaseItemControl)labelX33).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX33).Location = new Point(610, 238);
		((Control)labelX33).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX33).Name = "labelX33";
		((Control)labelX33).Size = new Size(102, 21);
		((Control)labelX33).TabIndex = 36;
		((Control)labelX33).Text = "[شناسه شبا]:";
		labelX33.TextAlignment = (StringAlignment)2;
		((Control)TxtShabaNo).Anchor = (AnchorStyles)13;
		TxtShabaNo.Border.Class = "TextBoxBorder";
		TxtShabaNo.Border.CornerType = (eCornerType)1;
		((Control)TxtShabaNo).Location = new Point(74, 237);
		((Control)TxtShabaNo).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtShabaNo).Name = "TxtShabaNo";
		((Control)TxtShabaNo).Size = new Size(533, 20);
		((Control)TxtShabaNo).TabIndex = 37;
		((Control)TxtJob).Anchor = (AnchorStyles)13;
		TxtJob.Border.Class = "TextBoxBorder";
		TxtJob.Border.CornerType = (eCornerType)1;
		((Control)TxtJob).Enabled = false;
		((Control)TxtJob).Location = new Point(74, 181);
		((Control)TxtJob).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtJob).Name = "TxtJob";
		((Control)TxtJob).Size = new Size(533, 20);
		((Control)TxtJob).TabIndex = 39;
		((Control)labelX34).Anchor = (AnchorStyles)9;
		((Control)labelX34).BackColor = Color.Transparent;
		((BaseItemControl)labelX34).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX34).Location = new Point(610, 180);
		((Control)labelX34).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX34).Name = "labelX34";
		((Control)labelX34).Size = new Size(102, 21);
		((Control)labelX34).TabIndex = 38;
		((Control)labelX34).Text = "[عنوان شغل]:";
		labelX34.TextAlignment = (StringAlignment)2;
		((Control)BtnMoreJob).AccessibleRole = (AccessibleRole)43;
		BtnMoreJob.ColorTable = (eButtonColor)6;
		((Control)BtnMoreJob).Location = new Point(46, 181);
		((Control)BtnMoreJob).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnMoreJob).Name = "BtnMoreJob";
		((Control)BtnMoreJob).Size = new Size(23, 21);
		((PopupItemControl)BtnMoreJob).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreJob).TabIndex = 40;
		((Control)BtnMoreJob).TabStop = false;
		((Control)BtnMoreJob).Text = "...";
		((Control)BtnMoreJob).Click += BtnMoreJob_Click;
		((Control)BtnDelJob).AccessibleRole = (AccessibleRole)43;
		BtnDelJob.ColorTable = (eButtonColor)3;
		BtnDelJob.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelJob).Location = new Point(18, 181);
		((Control)BtnDelJob).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelJob).Name = "BtnDelJob";
		((Control)BtnDelJob).Size = new Size(23, 21);
		((PopupItemControl)BtnDelJob).Style = (eDotNetBarStyle)9;
		((Control)BtnDelJob).TabIndex = 41;
		((Control)BtnDelJob).TabStop = false;
		((Control)groupPanelTaradod).Anchor = (AnchorStyles)13;
		((Control)groupPanelTaradod).BackColor = Color.Transparent;
		((PanelControl)groupPanelTaradod).CanvasColor = SystemColors.Control;
		((PanelControl)groupPanelTaradod).ColorSchemeStyle = (eDotNetBarStyle)4;
		((Control)groupPanelTaradod).Controls.Add((Control)(object)chbxTaxOption);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)rbPayloadCalculations);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)rbLeastLawLabor);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)rbNotInsurance);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)txtUnemployment);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)txtInsured);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)txtEmployer);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)labelX45);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)labelX43);
		((Control)groupPanelTaradod).Controls.Add((Control)(object)labelX41);
		groupPanelTaradod.DisabledBackColor = Color.Empty;
		((Control)groupPanelTaradod).Location = new Point(4, 273);
		((Control)groupPanelTaradod).Margin = new Padding(4, 3, 4, 3);
		((Control)groupPanelTaradod).Name = "groupPanelTaradod";
		((Control)groupPanelTaradod).Size = new Size(721, 150);
		((PanelControl)groupPanelTaradod).Style.BackColor2SchemePart = (eColorSchemePart)52;
		((PanelControl)groupPanelTaradod).Style.BackColorGradientAngle = 90;
		((PanelControl)groupPanelTaradod).Style.BackColorSchemePart = (eColorSchemePart)51;
		((PanelControl)groupPanelTaradod).Style.BorderBottom = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderBottomWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderColorSchemePart = (eColorSchemePart)53;
		((PanelControl)groupPanelTaradod).Style.BorderLeft = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderLeftWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderRight = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderRightWidth = 1;
		((PanelControl)groupPanelTaradod).Style.BorderTop = (eStyleBorderType)1;
		((PanelControl)groupPanelTaradod).Style.BorderTopWidth = 1;
		((PanelControl)groupPanelTaradod).Style.CornerDiameter = 4;
		((PanelControl)groupPanelTaradod).Style.CornerType = (eCornerType)2;
		((PanelControl)groupPanelTaradod).Style.TextAlignment = (eStyleTextAlignment)1;
		((PanelControl)groupPanelTaradod).Style.TextColorSchemePart = (eColorSchemePart)54;
		((PanelControl)groupPanelTaradod).Style.TextLineAlignment = (eStyleTextAlignment)0;
		((PanelControl)groupPanelTaradod).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)groupPanelTaradod).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)groupPanelTaradod).TabIndex = 33;
		((Control)groupPanelTaradod).Text = "[تنظیــمات بــیــمــه و مالیــات]";
		((Control)chbxTaxOption).Anchor = (AnchorStyles)9;
		((Control)chbxTaxOption).BackColor = Color.Transparent;
		((BaseItemControl)chbxTaxOption).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chbxTaxOption).Font = new Font("Tahoma", 8.2f, (FontStyle)1);
		((Control)chbxTaxOption).Location = new Point(553, 10);
		((Control)chbxTaxOption).Margin = new Padding(4, 3, 4, 3);
		((Control)chbxTaxOption).Name = "chbxTaxOption";
		((Control)chbxTaxOption).Size = new Size(153, 21);
		((BaseItemControl)chbxTaxOption).Style = (eDotNetBarStyle)9;
		((Control)chbxTaxOption).TabIndex = 32;
		((Control)chbxTaxOption).Text = "[مالیات محاسبه شود]";
		chbxTaxOption.TextColor = Color.Black;
		((Control)rbPayloadCalculations).Anchor = (AnchorStyles)9;
		((Control)rbPayloadCalculations).BackColor = Color.Transparent;
		((Control)rbPayloadCalculations).Font = new Font("Tahoma", 8.2f, (FontStyle)1);
		((Control)rbPayloadCalculations).Location = new Point(385, 103);
		((Control)rbPayloadCalculations).Margin = new Padding(4, 3, 4, 3);
		((Control)rbPayloadCalculations).Name = "rbPayloadCalculations";
		((Control)rbPayloadCalculations).Size = new Size(321, 22);
		((Control)rbPayloadCalculations).TabIndex = 8;
		((Control)rbPayloadCalculations).Text = "بر اساس آیتمهای فیش محاسبه شود";
		((ButtonBase)rbPayloadCalculations).UseVisualStyleBackColor = false;
		((Control)rbLeastLawLabor).Anchor = (AnchorStyles)9;
		((Control)rbLeastLawLabor).BackColor = Color.Transparent;
		((Control)rbLeastLawLabor).Font = new Font("Tahoma", 8.2f, (FontStyle)1);
		((Control)rbLeastLawLabor).Location = new Point(221, 75);
		((Control)rbLeastLawLabor).Margin = new Padding(4, 3, 4, 3);
		((Control)rbLeastLawLabor).Name = "rbLeastLawLabor";
		((Control)rbLeastLawLabor).Size = new Size(485, 22);
		((Control)rbLeastLawLabor).TabIndex = 7;
		((Control)rbLeastLawLabor).Text = "[براساس حداقل قانون کار محاسبه شود (حداقل حقوق پایه +حق مسکن +بن کارگری)]";
		((ButtonBase)rbLeastLawLabor).UseVisualStyleBackColor = false;
		((Control)rbNotInsurance).Anchor = (AnchorStyles)9;
		((Control)rbNotInsurance).BackColor = Color.Transparent;
		rbNotInsurance.Checked = true;
		((Control)rbNotInsurance).Font = new Font("Tahoma", 8.2f, (FontStyle)1);
		((Control)rbNotInsurance).Location = new Point(548, 46);
		((Control)rbNotInsurance).Margin = new Padding(4, 3, 4, 3);
		((Control)rbNotInsurance).Name = "rbNotInsurance";
		((Control)rbNotInsurance).Size = new Size(158, 22);
		((Control)rbNotInsurance).TabIndex = 6;
		rbNotInsurance.TabStop = true;
		((Control)rbNotInsurance).Text = "[بیمه محاسبه نشود]";
		((ButtonBase)rbNotInsurance).UseVisualStyleBackColor = false;
		((Control)txtUnemployment).Anchor = (AnchorStyles)13;
		txtUnemployment.Border.Class = "TextBoxBorder";
		txtUnemployment.Border.CornerType = (eCornerType)1;
		((Control)txtUnemployment).Location = new Point(9, 104);
		((Control)txtUnemployment).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)txtUnemployment).MaxLength = 2;
		((Control)txtUnemployment).Name = "txtUnemployment";
		((Control)txtUnemployment).Size = new Size(55, 20);
		((Control)txtUnemployment).TabIndex = 5;
		((Control)txtUnemployment).Text = "3";
		((TextBox)txtUnemployment).TextAlign = (HorizontalAlignment)2;
		((Control)txtInsured).Anchor = (AnchorStyles)9;
		txtInsured.Border.Class = "TextBoxBorder";
		txtInsured.Border.CornerType = (eCornerType)1;
		((Control)txtInsured).Location = new Point(9, 47);
		((Control)txtInsured).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)txtInsured).MaxLength = 2;
		((Control)txtInsured).Name = "txtInsured";
		((Control)txtInsured).Size = new Size(55, 20);
		((Control)txtInsured).TabIndex = 1;
		((Control)txtInsured).Text = "7";
		((TextBox)txtInsured).TextAlign = (HorizontalAlignment)2;
		((Control)txtEmployer).Anchor = (AnchorStyles)9;
		txtEmployer.Border.Class = "TextBoxBorder";
		txtEmployer.Border.CornerType = (eCornerType)1;
		((Control)txtEmployer).Location = new Point(9, 76);
		((Control)txtEmployer).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)txtEmployer).MaxLength = 2;
		((Control)txtEmployer).Name = "txtEmployer";
		((Control)txtEmployer).Size = new Size(55, 20);
		((Control)txtEmployer).TabIndex = 0;
		((Control)txtEmployer).Text = "20";
		((TextBox)txtEmployer).TextAlign = (HorizontalAlignment)2;
		((Control)labelX45).Anchor = (AnchorStyles)9;
		((Control)labelX45).BackColor = Color.Transparent;
		((BaseItemControl)labelX45).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX45).Location = new Point(67, 104);
		((Control)labelX45).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX45).Name = "labelX45";
		((Control)labelX45).Size = new Size(135, 21);
		((Control)labelX45).TabIndex = 4;
		((Control)labelX45).Text = "[نرخ بیمه بیکاری ]:";
		labelX45.TextAlignment = (StringAlignment)2;
		((Control)labelX43).Anchor = (AnchorStyles)9;
		((Control)labelX43).BackColor = Color.Transparent;
		((BaseItemControl)labelX43).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX43).Location = new Point(67, 75);
		((Control)labelX43).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX43).Name = "labelX43";
		((Control)labelX43).Size = new Size(130, 21);
		((Control)labelX43).TabIndex = 2;
		((Control)labelX43).Text = "[نرخ بیمه سهم کارفرما]:";
		labelX43.TextAlignment = (StringAlignment)2;
		((Control)labelX41).Anchor = (AnchorStyles)9;
		((Control)labelX41).BackColor = Color.Transparent;
		((BaseItemControl)labelX41).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX41).Location = new Point(67, 46);
		((Control)labelX41).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX41).Name = "labelX41";
		((Control)labelX41).Size = new Size(124, 21);
		((Control)labelX41).TabIndex = 0;
		((Control)labelX41).Text = "[نرخ بیمه سهم کارگر]:";
		labelX41.TextAlignment = (StringAlignment)2;
		((Control)labelX21).Anchor = (AnchorStyles)9;
		((Control)labelX21).BackColor = Color.Transparent;
		((BaseItemControl)labelX21).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX21).Location = new Point(610, 6);
		((Control)labelX21).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX21).Name = "labelX21";
		((Control)labelX21).Size = new Size(102, 21);
		((Control)labelX21).TabIndex = 0;
		((Control)labelX21).Text = "[بانک]:";
		labelX21.TextAlignment = (StringAlignment)2;
		((Control)CmbBank).Anchor = (AnchorStyles)13;
		((ListControl)CmbBank).DisplayMember = "fullName";
		((ComboBox)CmbBank).DrawMode = (DrawMode)1;
		((ComboBox)CmbBank).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbBank).FormattingEnabled = true;
		((ComboBox)CmbBank).ItemHeight = 15;
		((Control)CmbBank).Location = new Point(74, 7);
		((Control)CmbBank).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbBank).Name = "CmbBank";
		((Control)CmbBank).Size = new Size(534, 21);
		CmbBank.Style = (eDotNetBarStyle)9;
		((Control)CmbBank).TabIndex = 1;
		((ListControl)CmbBank).ValueMember = "Id";
		((Control)CmbBankType).Anchor = (AnchorStyles)13;
		((ListControl)CmbBankType).DisplayMember = "Name";
		((ComboBox)CmbBankType).DrawMode = (DrawMode)1;
		((ComboBox)CmbBankType).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbBankType).FormattingEnabled = true;
		((ComboBox)CmbBankType).ItemHeight = 15;
		((Control)CmbBankType).Location = new Point(74, 36);
		((Control)CmbBankType).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbBankType).Name = "CmbBankType";
		((Control)CmbBankType).Size = new Size(534, 21);
		CmbBankType.Style = (eDotNetBarStyle)9;
		((Control)CmbBankType).TabIndex = 5;
		((ListControl)CmbBankType).ValueMember = "Id";
		((Control)BtnMorebank).AccessibleRole = (AccessibleRole)43;
		BtnMorebank.ColorTable = (eButtonColor)6;
		((Control)BtnMorebank).Location = new Point(46, 7);
		((Control)BtnMorebank).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnMorebank).Name = "BtnMorebank";
		((Control)BtnMorebank).Size = new Size(23, 21);
		((PopupItemControl)BtnMorebank).Style = (eDotNetBarStyle)9;
		((Control)BtnMorebank).TabIndex = 2;
		((Control)BtnMorebank).TabStop = false;
		((Control)BtnMorebank).Text = "...";
		((Control)BtnMorebank).Click += BtnMorebank_Click;
		((Control)BtnMorePlace).AccessibleRole = (AccessibleRole)43;
		BtnMorePlace.ColorTable = (eButtonColor)6;
		((Control)BtnMorePlace).Location = new Point(46, 152);
		((Control)BtnMorePlace).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnMorePlace).Name = "BtnMorePlace";
		((Control)BtnMorePlace).Size = new Size(23, 21);
		((PopupItemControl)BtnMorePlace).Style = (eDotNetBarStyle)9;
		((Control)BtnMorePlace).TabIndex = 30;
		((Control)BtnMorePlace).TabStop = false;
		((Control)BtnMorePlace).Text = "...";
		((Control)BtnMorePlace).Click += BtnMorePlace_Click;
		((Control)BtnMorebankType).AccessibleRole = (AccessibleRole)43;
		BtnMorebankType.ColorTable = (eButtonColor)6;
		((Control)BtnMorebankType).Location = new Point(46, 36);
		((Control)BtnMorebankType).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnMorebankType).Name = "BtnMorebankType";
		((Control)BtnMorebankType).Size = new Size(23, 21);
		((PopupItemControl)BtnMorebankType).Style = (eDotNetBarStyle)9;
		((Control)BtnMorebankType).TabIndex = 6;
		((Control)BtnMorebankType).TabStop = false;
		((Control)BtnMorebankType).Text = "...";
		((Control)BtnMorebankType).Click += BtnMorebankType_Click;
		((Control)CmbPlace).Anchor = (AnchorStyles)13;
		((ListControl)CmbPlace).DisplayMember = "Name";
		((ComboBox)CmbPlace).DrawMode = (DrawMode)1;
		((ComboBox)CmbPlace).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbPlace).FormattingEnabled = true;
		((ComboBox)CmbPlace).ItemHeight = 15;
		((Control)CmbPlace).Location = new Point(74, 152);
		((Control)CmbPlace).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbPlace).Name = "CmbPlace";
		((Control)CmbPlace).Size = new Size(534, 21);
		CmbPlace.Style = (eDotNetBarStyle)9;
		((Control)CmbPlace).TabIndex = 29;
		((ListControl)CmbPlace).ValueMember = "Id";
		((Control)labelX39).Anchor = (AnchorStyles)9;
		((Control)labelX39).BackColor = Color.Transparent;
		((BaseItemControl)labelX39).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX39).Location = new Point(610, 151);
		((Control)labelX39).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX39).Name = "labelX39";
		((Control)labelX39).Size = new Size(102, 21);
		((Control)labelX39).TabIndex = 28;
		((Control)labelX39).Text = "[محل  خدمت]:";
		labelX39.TextAlignment = (StringAlignment)2;
		((Control)labelX32).Anchor = (AnchorStyles)9;
		((Control)labelX32).BackColor = Color.Transparent;
		((BaseItemControl)labelX32).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX32).Location = new Point(610, 35);
		((Control)labelX32).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX32).Name = "labelX32";
		((Control)labelX32).Size = new Size(102, 21);
		((Control)labelX32).TabIndex = 4;
		((Control)labelX32).Text = "[نوع حساب بانکی]:";
		labelX32.TextAlignment = (StringAlignment)2;
		((Control)BtnMoreRecruitmentType).AccessibleRole = (AccessibleRole)43;
		BtnMoreRecruitmentType.ColorTable = (eButtonColor)6;
		((Control)BtnMoreRecruitmentType).Location = new Point(46, 123);
		((Control)BtnMoreRecruitmentType).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnMoreRecruitmentType).Name = "BtnMoreRecruitmentType";
		((Control)BtnMoreRecruitmentType).Size = new Size(23, 21);
		((PopupItemControl)BtnMoreRecruitmentType).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreRecruitmentType).TabIndex = 26;
		((Control)BtnMoreRecruitmentType).TabStop = false;
		((Control)BtnMoreRecruitmentType).Text = "...";
		((Control)BtnMoreRecruitmentType).Click += BtnMoreRecruitmentType_Click;
		((Control)CmbRecruitmentType).Anchor = (AnchorStyles)13;
		((ListControl)CmbRecruitmentType).DisplayMember = "Name";
		((ComboBox)CmbRecruitmentType).DrawMode = (DrawMode)1;
		((ComboBox)CmbRecruitmentType).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbRecruitmentType).FormattingEnabled = true;
		((ComboBox)CmbRecruitmentType).ItemHeight = 15;
		((Control)CmbRecruitmentType).Location = new Point(74, 123);
		((Control)CmbRecruitmentType).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbRecruitmentType).Name = "CmbRecruitmentType";
		((Control)CmbRecruitmentType).Size = new Size(534, 21);
		CmbRecruitmentType.Style = (eDotNetBarStyle)9;
		((Control)CmbRecruitmentType).TabIndex = 25;
		((ListControl)CmbRecruitmentType).ValueMember = "Id";
		((Control)labelX38).Anchor = (AnchorStyles)9;
		((Control)labelX38).BackColor = Color.Transparent;
		((BaseItemControl)labelX38).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX38).Location = new Point(610, 122);
		((Control)labelX38).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX38).Name = "labelX38";
		((Control)labelX38).Size = new Size(102, 21);
		((Control)labelX38).TabIndex = 24;
		((Control)labelX38).Text = "[نوع استخدامی]:";
		labelX38.TextAlignment = (StringAlignment)2;
		((Control)labelX36).Anchor = (AnchorStyles)9;
		((Control)labelX36).BackColor = Color.Transparent;
		((BaseItemControl)labelX36).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX36).Location = new Point(610, 93);
		((Control)labelX36).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX36).Name = "labelX36";
		((Control)labelX36).Size = new Size(102, 21);
		((Control)labelX36).TabIndex = 20;
		((Control)labelX36).Text = "[وضعیت خدمتی]:";
		labelX36.TextAlignment = (StringAlignment)2;
		((Control)BtnMoreRecruitmentST).AccessibleRole = (AccessibleRole)43;
		BtnMoreRecruitmentST.ColorTable = (eButtonColor)6;
		((Control)BtnMoreRecruitmentST).Location = new Point(46, 94);
		((Control)BtnMoreRecruitmentST).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnMoreRecruitmentST).Name = "BtnMoreRecruitmentST";
		((Control)BtnMoreRecruitmentST).Size = new Size(23, 21);
		((PopupItemControl)BtnMoreRecruitmentST).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreRecruitmentST).TabIndex = 1;
		((Control)BtnMoreRecruitmentST).TabStop = false;
		((Control)BtnMoreRecruitmentST).Text = "...";
		((Control)BtnMoreRecruitmentST).Click += BtnMoreRecruitmentST_Click;
		((Control)labelX35).Anchor = (AnchorStyles)9;
		((Control)labelX35).BackColor = Color.Transparent;
		((BaseItemControl)labelX35).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX35).Location = new Point(610, 64);
		((Control)labelX35).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX35).Name = "labelX35";
		((Control)labelX35).Size = new Size(102, 21);
		((Control)labelX35).TabIndex = 16;
		((Control)labelX35).Text = "[بخش]:";
		labelX35.TextAlignment = (StringAlignment)2;
		((Control)CmbRecruitmentST).Anchor = (AnchorStyles)13;
		((ListControl)CmbRecruitmentST).DisplayMember = "Name";
		((ComboBox)CmbRecruitmentST).DrawMode = (DrawMode)1;
		((ComboBox)CmbRecruitmentST).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbRecruitmentST).FormattingEnabled = true;
		((ComboBox)CmbRecruitmentST).ItemHeight = 15;
		((Control)CmbRecruitmentST).Location = new Point(74, 94);
		((Control)CmbRecruitmentST).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbRecruitmentST).Name = "CmbRecruitmentST";
		((Control)CmbRecruitmentST).Size = new Size(534, 21);
		CmbRecruitmentST.Style = (eDotNetBarStyle)9;
		((Control)CmbRecruitmentST).TabIndex = 21;
		((ListControl)CmbRecruitmentST).ValueMember = "Id";
		((Control)CmbPart).Anchor = (AnchorStyles)13;
		((ListControl)CmbPart).DisplayMember = "Name";
		((ComboBox)CmbPart).DrawMode = (DrawMode)1;
		((ComboBox)CmbPart).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbPart).FormattingEnabled = true;
		((ComboBox)CmbPart).ItemHeight = 15;
		((Control)CmbPart).Location = new Point(74, 65);
		((Control)CmbPart).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbPart).Name = "CmbPart";
		((Control)CmbPart).Size = new Size(534, 21);
		CmbPart.Style = (eDotNetBarStyle)9;
		((Control)CmbPart).TabIndex = 0;
		((ListControl)CmbPart).ValueMember = "Id";
		((Control)BtnDelPlace).AccessibleRole = (AccessibleRole)43;
		BtnDelPlace.ColorTable = (eButtonColor)3;
		BtnDelPlace.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelPlace).Location = new Point(18, 152);
		((Control)BtnDelPlace).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelPlace).Name = "BtnDelPlace";
		((Control)BtnDelPlace).Size = new Size(23, 21);
		((PopupItemControl)BtnDelPlace).Style = (eDotNetBarStyle)9;
		((Control)BtnDelPlace).TabIndex = 31;
		((Control)BtnDelPlace).TabStop = false;
		((Control)BtnDelPlace).Click += BtnDelPlace_Click;
		((Control)BtnDelRecruitmentType).AccessibleRole = (AccessibleRole)43;
		BtnDelRecruitmentType.ColorTable = (eButtonColor)3;
		BtnDelRecruitmentType.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelRecruitmentType).Location = new Point(18, 123);
		((Control)BtnDelRecruitmentType).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelRecruitmentType).Name = "BtnDelRecruitmentType";
		((Control)BtnDelRecruitmentType).Size = new Size(23, 21);
		((PopupItemControl)BtnDelRecruitmentType).Style = (eDotNetBarStyle)9;
		((Control)BtnDelRecruitmentType).TabIndex = 27;
		((Control)BtnDelRecruitmentType).TabStop = false;
		((Control)BtnDelRecruitmentType).Click += BtnDelRecruitmentType_Click;
		((Control)BtnDelRecruitmentST).AccessibleRole = (AccessibleRole)43;
		BtnDelRecruitmentST.ColorTable = (eButtonColor)3;
		BtnDelRecruitmentST.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelRecruitmentST).Location = new Point(18, 94);
		((Control)BtnDelRecruitmentST).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelRecruitmentST).Name = "BtnDelRecruitmentST";
		((Control)BtnDelRecruitmentST).Size = new Size(23, 21);
		((PopupItemControl)BtnDelRecruitmentST).Style = (eDotNetBarStyle)9;
		((Control)BtnDelRecruitmentST).TabIndex = 23;
		((Control)BtnDelRecruitmentST).TabStop = false;
		((Control)BtnDelRecruitmentST).Click += BtnDelRecruitmentST_Click;
		((Control)BtnMorePart).AccessibleRole = (AccessibleRole)43;
		BtnMorePart.ColorTable = (eButtonColor)6;
		((Control)BtnMorePart).Location = new Point(46, 65);
		((Control)BtnMorePart).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnMorePart).Name = "BtnMorePart";
		((Control)BtnMorePart).Size = new Size(23, 21);
		((PopupItemControl)BtnMorePart).Style = (eDotNetBarStyle)9;
		((Control)BtnMorePart).TabIndex = 18;
		((Control)BtnMorePart).TabStop = false;
		((Control)BtnMorePart).Text = "...";
		((Control)BtnMorePart).Click += BtnMorePart_Click;
		((Control)BtnDelPart).AccessibleRole = (AccessibleRole)43;
		BtnDelPart.ColorTable = (eButtonColor)3;
		BtnDelPart.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelPart).Location = new Point(18, 65);
		((Control)BtnDelPart).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelPart).Name = "BtnDelPart";
		((Control)BtnDelPart).Size = new Size(23, 21);
		((PopupItemControl)BtnDelPart).Style = (eDotNetBarStyle)9;
		((Control)BtnDelPart).TabIndex = 19;
		((Control)BtnDelPart).TabStop = false;
		((Control)BtnDelPart).Click += BtnDelPart_Click;
		((Control)BtnDelBankType).AccessibleRole = (AccessibleRole)43;
		BtnDelBankType.ColorTable = (eButtonColor)3;
		BtnDelBankType.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelBankType).Location = new Point(18, 36);
		((Control)BtnDelBankType).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelBankType).Name = "BtnDelBankType";
		((Control)BtnDelBankType).Size = new Size(23, 21);
		((PopupItemControl)BtnDelBankType).Style = (eDotNetBarStyle)9;
		((Control)BtnDelBankType).TabIndex = 7;
		((Control)BtnDelBankType).TabStop = false;
		((Control)BtnDelBankType).Click += BtnDelBankType_Click;
		((Control)BtnDelBank).AccessibleRole = (AccessibleRole)43;
		BtnDelBank.ColorTable = (eButtonColor)3;
		BtnDelBank.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelBank).Location = new Point(18, 7);
		((Control)BtnDelBank).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelBank).Name = "BtnDelBank";
		((Control)BtnDelBank).Size = new Size(23, 21);
		((PopupItemControl)BtnDelBank).Style = (eDotNetBarStyle)9;
		((Control)BtnDelBank).TabIndex = 3;
		((Control)BtnDelBank).TabStop = false;
		((Control)BtnDelBank).Click += BtnDelBank_Click;
		tabItem3.AttachedControl = (Control)(object)tabControlPanel3;
		tabItem3.Name = "tabItem3";
		tabItem3.Text = "[اطلاعات استخدامی]";
		((Control)tabControlPanel2).Anchor = (AnchorStyles)15;
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceNobatkari3);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceNobatkari3);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceNobatkari2);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceNobatkari2);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX23);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblVacation);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtLeaveDays);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceJarime);
		((Control)tabControlPanel2).Controls.Add((Control)(object)rbNotCheckoutVacation);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceMission);
		((Control)tabControlPanel2).Controls.Add((Control)(object)rbCheckoutVacation);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceTatil);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceLowTime3);
		((Control)tabControlPanel2).Controls.Add((Control)(object)btnUpdateParametersSalary);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceOverTime3);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceLowTime2);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceOverTime2);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceNobatkari1);
		((Control)tabControlPanel2).Controls.Add((Control)(object)BtnAddGroupSanavat);
		((Control)tabControlPanel2).Controls.Add((Control)(object)ChbMaskan);
		((Control)tabControlPanel2).Controls.Add((Control)(object)chBxKLhoarBar);
		((Control)tabControlPanel2).Controls.Add((Control)(object)ChbHaghTaahol);
		((Control)tabControlPanel2).Controls.Add((Control)(object)chBxChild);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX16);
		((Control)tabControlPanel2).Controls.Add((Control)(object)cmbGroupSanavat);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX18);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtLeavHoures);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtLeavMinutes);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceLowTime);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX31);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX17);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX40);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceOverTime);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceLeave);
		((Control)tabControlPanel2).Controls.Add((Control)(object)CmbMariage);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceHoliday);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtLeaveSaved);
		((Control)tabControlPanel2).Controls.Add((Control)(object)NumberCountChild);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceNight);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX6);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX20);
		((Control)tabControlPanel2).Controls.Add((Control)(object)LblPriceAbsent);
		((Control)tabControlPanel2).Controls.Add((Control)(object)CmbSalaryType);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX19);
		((Control)tabControlPanel2).Controls.Add((Control)(object)BtnDelGroupSanavat);
		((Control)tabControlPanel2).Controls.Add((Control)(object)labelX4);
		((Control)tabControlPanel2).Controls.Add((Control)(object)btnCalOverLowTime);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceJarime);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceMission);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceTatil);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceLowTime3);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceOverTime3);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceNobatkari1);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceLowTime2);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceOverTime2);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtSanavat);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceLowTime);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceLeave);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceOverTime);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceAbsent);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceHoliday);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtPriceNight);
		((Control)tabControlPanel2).Controls.Add((Control)(object)TxtSalary);
		((PanelEx)tabControlPanel2).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel2).Location = new Point(1, 25);
		((Control)tabControlPanel2).Margin = new Padding(4, 3, 4, 3);
		((Control)tabControlPanel2).Name = "tabControlPanel2";
		((Control)tabControlPanel2).Padding = new Padding(1);
		((Control)tabControlPanel2).Size = new Size(727, 427);
		((PanelEx)tabControlPanel2).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel2).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel2).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel2).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel2).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel2).Style.GradientAngle = 90;
		((Control)tabControlPanel2).TabIndex = 2;
		tabControlPanel2.TabItem = tabItem2;
		((Control)tabControlPanel2).Enter += TabControlPanel3_Enter;
		((Control)LblPriceNobatkari3).Anchor = (AnchorStyles)9;
		((Control)LblPriceNobatkari3).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceNobatkari3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceNobatkari3).Location = new Point(563, 287);
		((Control)LblPriceNobatkari3).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceNobatkari3).Name = "LblPriceNobatkari3";
		((Control)LblPriceNobatkari3).Size = new Size(152, 21);
		((Control)LblPriceNobatkari3).TabIndex = 33;
		((Control)LblPriceNobatkari3).Text = "[نوبت کاری شب (ساعت)]:";
		LblPriceNobatkari3.TextAlignment = (StringAlignment)2;
		TxtPriceNobatkari3.AllowEmpty = true;
		((Control)TxtPriceNobatkari3).Anchor = (AnchorStyles)13;
		TxtPriceNobatkari3.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceNobatkari3.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceNobatkari3.BackcolorTooltip = Color.Red;
		TxtPriceNobatkari3.CurrencyFormat = "";
		((Control)TxtPriceNobatkari3).ForeColor = Color.Green;
		TxtPriceNobatkari3.ForeColorTooltip = Color.Black;
		TxtPriceNobatkari3.GetColoresFromSettingDLL = true;
		TxtPriceNobatkari3.HandleKeydown = true;
		((Control)TxtPriceNobatkari3).Location = new Point(401, 289);
		((Control)TxtPriceNobatkari3).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceNobatkari3).MaxLength = 10;
		((Control)TxtPriceNobatkari3).Name = "TxtPriceNobatkari3";
		TxtPriceNobatkari3.SendShiftTabAfterDown = true;
		TxtPriceNobatkari3.SendShiftTabAfterEnter = true;
		TxtPriceNobatkari3.SendTabAfterEnter = true;
		TxtPriceNobatkari3.SendTabAfterUP = true;
		TxtPriceNobatkari3.ShowHorofi = (ENShowHorofi)3;
		TxtPriceNobatkari3.ShowToolTipText = true;
		((Control)TxtPriceNobatkari3).Size = new Size(158, 20);
		((Control)TxtPriceNobatkari3).TabIndex = 16;
		((Control)TxtPriceNobatkari3).Text = "0";
		TxtPriceNobatkari3.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceNobatkari3.ToolTipPostFix = "";
		TxtPriceNobatkari3.TootTipDelay = 1000;
		TxtPriceNobatkari3.Value = new decimal(new int[4]);
		((Control)LblPriceNobatkari2).Anchor = (AnchorStyles)9;
		((Control)LblPriceNobatkari2).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceNobatkari2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceNobatkari2).Location = new Point(563, 262);
		((Control)LblPriceNobatkari2).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceNobatkari2).Name = "LblPriceNobatkari2";
		((Control)LblPriceNobatkari2).Size = new Size(152, 21);
		((Control)LblPriceNobatkari2).TabIndex = 31;
		((Control)LblPriceNobatkari2).Text = "[نوبت کاری عصر (ساعت)]:";
		LblPriceNobatkari2.TextAlignment = (StringAlignment)2;
		TxtPriceNobatkari2.AllowEmpty = true;
		((Control)TxtPriceNobatkari2).Anchor = (AnchorStyles)13;
		TxtPriceNobatkari2.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceNobatkari2.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceNobatkari2.BackcolorTooltip = Color.Red;
		TxtPriceNobatkari2.CurrencyFormat = "";
		((Control)TxtPriceNobatkari2).ForeColor = Color.Green;
		TxtPriceNobatkari2.ForeColorTooltip = Color.Black;
		TxtPriceNobatkari2.GetColoresFromSettingDLL = true;
		TxtPriceNobatkari2.HandleKeydown = true;
		((Control)TxtPriceNobatkari2).Location = new Point(401, 264);
		((Control)TxtPriceNobatkari2).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceNobatkari2).MaxLength = 10;
		((Control)TxtPriceNobatkari2).Name = "TxtPriceNobatkari2";
		TxtPriceNobatkari2.SendShiftTabAfterDown = true;
		TxtPriceNobatkari2.SendShiftTabAfterEnter = true;
		TxtPriceNobatkari2.SendTabAfterEnter = true;
		TxtPriceNobatkari2.SendTabAfterUP = true;
		TxtPriceNobatkari2.ShowHorofi = (ENShowHorofi)3;
		TxtPriceNobatkari2.ShowToolTipText = true;
		((Control)TxtPriceNobatkari2).Size = new Size(158, 20);
		((Control)TxtPriceNobatkari2).TabIndex = 15;
		((Control)TxtPriceNobatkari2).Text = "0";
		TxtPriceNobatkari2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceNobatkari2.ToolTipPostFix = "";
		TxtPriceNobatkari2.TootTipDelay = 1000;
		TxtPriceNobatkari2.Value = new decimal(new int[4]);
		((Control)labelX23).BackColor = Color.Transparent;
		((BaseItemControl)labelX23).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX23).ForeColor = SystemColors.MenuHighlight;
		((Control)labelX23).Location = new Point(575, 397);
		((Control)labelX23).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX23).Name = "labelX23";
		((Control)labelX23).Size = new Size(140, 21);
		((Control)labelX23).TabIndex = 0;
		((Control)labelX23).Text = "[تسویه مانده مرخصی در]:";
		labelX23.TextAlignment = (StringAlignment)2;
		((Control)LblVacation).BackColor = Color.Transparent;
		((BaseItemControl)LblVacation).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblVacation).ForeColor = SystemColors.MenuHighlight;
		((Control)LblVacation).Location = new Point(131, 397);
		((Control)LblVacation).Margin = new Padding(4, 3, 4, 3);
		((Control)LblVacation).Name = "LblVacation";
		((Control)LblVacation).Size = new Size(191, 21);
		((Control)LblVacation).TabIndex = 0;
		((Control)LblVacation).Text = "حق مرخصی در بازه قرارداد:";
		LblVacation.TextAlignment = (StringAlignment)2;
		TxtLeaveDays.Border.Class = "TextBoxBorder";
		TxtLeaveDays.Border.CornerType = (eCornerType)1;
		((Control)TxtLeaveDays).ForeColor = Color.Green;
		((Control)TxtLeaveDays).Location = new Point(14, 397);
		((Control)TxtLeaveDays).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtLeaveDays).MaxLength = 4;
		((Control)TxtLeaveDays).Name = "TxtLeaveDays";
		((Control)TxtLeaveDays).Size = new Size(110, 20);
		((Control)TxtLeaveDays).TabIndex = 29;
		((Control)TxtLeaveDays).Text = "0";
		((TextBox)TxtLeaveDays).TextAlign = (HorizontalAlignment)2;
		((Control)LblPriceJarime).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceJarime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceJarime).Location = new Point(178, 240);
		((Control)LblPriceJarime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceJarime).Name = "LblPriceJarime";
		((Control)LblPriceJarime).Size = new Size(127, 21);
		((Control)LblPriceJarime).TabIndex = 0;
		((Control)LblPriceJarime).Text = "[مبلغ جریمه]:";
		LblPriceJarime.TextAlignment = (StringAlignment)2;
		((Control)rbNotCheckoutVacation).BackColor = Color.Transparent;
		rbNotCheckoutVacation.Checked = true;
		((Control)rbNotCheckoutVacation).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)rbNotCheckoutVacation).ForeColor = SystemColors.HotTrack;
		((Control)rbNotCheckoutVacation).Location = new Point(453, 396);
		((Control)rbNotCheckoutVacation).Margin = new Padding(4, 3, 4, 3);
		((Control)rbNotCheckoutVacation).Name = "rbNotCheckoutVacation";
		((Control)rbNotCheckoutVacation).Size = new Size(114, 22);
		((Control)rbNotCheckoutVacation).TabIndex = 30;
		rbNotCheckoutVacation.TabStop = true;
		((Control)rbNotCheckoutVacation).Text = "[پایان قرارداد]";
		((ButtonBase)rbNotCheckoutVacation).UseVisualStyleBackColor = false;
		rbNotCheckoutVacation.CheckedChanged += RbNotCheckoutVacation_CheckedChanged;
		((Control)LblPriceMission).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceMission).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceMission).Location = new Point(178, 290);
		((Control)LblPriceMission).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceMission).Name = "LblPriceMission";
		((Control)LblPriceMission).Size = new Size(127, 21);
		((Control)LblPriceMission).TabIndex = 0;
		((Control)LblPriceMission).Text = "[ماموریت روزانه]:";
		LblPriceMission.TextAlignment = (StringAlignment)2;
		((Control)rbCheckoutVacation).BackColor = Color.Transparent;
		((Control)rbCheckoutVacation).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)rbCheckoutVacation).ForeColor = SystemColors.MenuHighlight;
		((Control)rbCheckoutVacation).Location = new Point(336, 396);
		((Control)rbCheckoutVacation).Margin = new Padding(4, 3, 4, 3);
		((Control)rbCheckoutVacation).Name = "rbCheckoutVacation";
		((Control)rbCheckoutVacation).Size = new Size(115, 22);
		((Control)rbCheckoutVacation).TabIndex = 31;
		((Control)rbCheckoutVacation).Text = " [پایان هر ماه]";
		((ButtonBase)rbCheckoutVacation).UseVisualStyleBackColor = false;
		rbCheckoutVacation.CheckedChanged += RbCheckoutVacation_CheckedChanged;
		((Control)LblPriceTatil).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceTatil).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceTatil).Location = new Point(178, 315);
		((Control)LblPriceTatil).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceTatil).Name = "LblPriceTatil";
		((Control)LblPriceTatil).Size = new Size(127, 21);
		((Control)LblPriceTatil).TabIndex = 0;
		((Control)LblPriceTatil).Text = "[تعطیل کاری (ساعت)]:";
		LblPriceTatil.TextAlignment = (StringAlignment)2;
		((Control)LblPriceLowTime3).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceLowTime3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceLowTime3).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)LblPriceLowTime3).Location = new Point(178, 190);
		((Control)LblPriceLowTime3).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceLowTime3).Name = "LblPriceLowTime3";
		((Control)LblPriceLowTime3).Size = new Size(172, 21);
		((Control)LblPriceLowTime3).TabIndex = 0;
		((Control)LblPriceLowTime3).Text = "[کسر کار آخر وقت (ساعت)]:";
		LblPriceLowTime3.TextAlignment = (StringAlignment)2;
		((Control)btnUpdateParametersSalary).Anchor = (AnchorStyles)13;
		((Control)btnUpdateParametersSalary).BackColor = Color.FromArgb(255, 255, 192);
		((ButtonBase)btnUpdateParametersSalary).FlatAppearance.BorderSize = 3;
		((Control)btnUpdateParametersSalary).Location = new Point(4, 5);
		((Control)btnUpdateParametersSalary).Margin = new Padding(4, 3, 4, 3);
		((Control)btnUpdateParametersSalary).Name = "btnUpdateParametersSalary";
		((Control)btnUpdateParametersSalary).Size = new Size(720, 26);
		((Control)btnUpdateParametersSalary).TabIndex = 0;
		((Control)btnUpdateParametersSalary).Text = "[در صورتیکه تمایل دارید پارامتر های حقوق بر اساس حداقل حقوق قانون کار تنظیم شوند کلیک نمائید]";
		((ButtonBase)btnUpdateParametersSalary).UseVisualStyleBackColor = false;
		((Control)btnUpdateParametersSalary).Click += BtnUpdateParametersSalary_Click;
		((Control)LblPriceOverTime3).Anchor = (AnchorStyles)9;
		((Control)LblPriceOverTime3).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceOverTime3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceOverTime3).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)LblPriceOverTime3).Location = new Point(563, 187);
		((Control)LblPriceOverTime3).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceOverTime3).Name = "LblPriceOverTime3";
		((Control)LblPriceOverTime3).Size = new Size(164, 21);
		((Control)LblPriceOverTime3).TabIndex = 0;
		((Control)LblPriceOverTime3).Text = "[اضافه کار آخر وقت(ساعت)]:";
		LblPriceOverTime3.TextAlignment = (StringAlignment)2;
		((Control)LblPriceLowTime2).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceLowTime2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceLowTime2).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)LblPriceLowTime2).Location = new Point(178, 165);
		((Control)LblPriceLowTime2).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceLowTime2).Name = "LblPriceLowTime2";
		((Control)LblPriceLowTime2).Size = new Size(172, 21);
		((Control)LblPriceLowTime2).TabIndex = 0;
		((Control)LblPriceLowTime2).Text = "[کسر کار وسط وقت (ساعت)]:";
		LblPriceLowTime2.TextAlignment = (StringAlignment)2;
		((Control)LblPriceOverTime2).Anchor = (AnchorStyles)9;
		((Control)LblPriceOverTime2).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceOverTime2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceOverTime2).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)LblPriceOverTime2).Location = new Point(563, 162);
		((Control)LblPriceOverTime2).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceOverTime2).Name = "LblPriceOverTime2";
		((Control)LblPriceOverTime2).Size = new Size(164, 21);
		((Control)LblPriceOverTime2).TabIndex = 0;
		((Control)LblPriceOverTime2).Text = "[اضافه کار وسط وقت(ساعت)]:";
		LblPriceOverTime2.TextAlignment = (StringAlignment)2;
		((Control)LblPriceNobatkari1).Anchor = (AnchorStyles)9;
		((Control)LblPriceNobatkari1).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceNobatkari1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceNobatkari1).Location = new Point(563, 237);
		((Control)LblPriceNobatkari1).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceNobatkari1).Name = "LblPriceNobatkari1";
		((Control)LblPriceNobatkari1).Size = new Size(152, 21);
		((Control)LblPriceNobatkari1).TabIndex = 0;
		((Control)LblPriceNobatkari1).Text = "[نوبت کاری صبح (ساعت)]:";
		LblPriceNobatkari1.TextAlignment = (StringAlignment)2;
		((Control)BtnAddGroupSanavat).AccessibleRole = (AccessibleRole)43;
		BtnAddGroupSanavat.ColorTable = (eButtonColor)6;
		((Control)BtnAddGroupSanavat).Location = new Point(42, 62);
		((Control)BtnAddGroupSanavat).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnAddGroupSanavat).Name = "BtnAddGroupSanavat";
		((Control)BtnAddGroupSanavat).Size = new Size(23, 21);
		((PopupItemControl)BtnAddGroupSanavat).Style = (eDotNetBarStyle)9;
		((Control)BtnAddGroupSanavat).TabIndex = 0;
		((Control)BtnAddGroupSanavat).TabStop = false;
		((Control)BtnAddGroupSanavat).Text = "...";
		((Control)BtnAddGroupSanavat).Click += BtnGroupSanavat_Click;
		((Control)ChbMaskan).BackColor = Color.Transparent;
		((BaseItemControl)ChbMaskan).BackgroundStyle.CornerType = (eCornerType)1;
		ChbMaskan.Checked = true;
		ChbMaskan.CheckState = (CheckState)1;
		ChbMaskan.CheckValue = "Y";
		((Control)ChbMaskan).Location = new Point(178, 114);
		((Control)ChbMaskan).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbMaskan).Name = "ChbMaskan";
		((Control)ChbMaskan).Size = new Size(86, 21);
		((BaseItemControl)ChbMaskan).Style = (eDotNetBarStyle)9;
		((Control)ChbMaskan).TabIndex = 6;
		((Control)ChbMaskan).Text = "حق مسکن";
		((Control)chBxKLhoarBar).BackColor = Color.Transparent;
		((BaseItemControl)chBxKLhoarBar).BackgroundStyle.CornerType = (eCornerType)1;
		chBxKLhoarBar.Checked = true;
		chBxKLhoarBar.CheckState = (CheckState)1;
		chBxKLhoarBar.CheckValue = "Y";
		((Control)chBxKLhoarBar).Location = new Point(297, 114);
		((Control)chBxKLhoarBar).Margin = new Padding(4, 3, 4, 3);
		((Control)chBxKLhoarBar).Name = "chBxKLhoarBar";
		((Control)chBxKLhoarBar).Size = new Size(96, 21);
		((BaseItemControl)chBxKLhoarBar).Style = (eDotNetBarStyle)9;
		((Control)chBxKLhoarBar).TabIndex = 9;
		((Control)chBxKLhoarBar).Text = "حق خوار و بار";
		((Control)ChbHaghTaahol).BackColor = Color.Transparent;
		((BaseItemControl)ChbHaghTaahol).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)ChbHaghTaahol).Location = new Point(401, 114);
		((Control)ChbHaghTaahol).Margin = new Padding(4, 3, 4, 3);
		((Control)ChbHaghTaahol).Name = "ChbHaghTaahol";
		((Control)ChbHaghTaahol).Size = new Size(157, 21);
		((BaseItemControl)ChbHaghTaahol).Style = (eDotNetBarStyle)9;
		((Control)ChbHaghTaahol).TabIndex = 7;
		((Control)ChbHaghTaahol).Text = "کمک هزینه عائله مندی";
		ChbHaghTaahol.CheckValueChanged += ChbHaghTahol_CheckValueChanged;
		((Control)chBxChild).BackColor = Color.Transparent;
		((BaseItemControl)chBxChild).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)chBxChild).Location = new Point(14, 114);
		((Control)chBxChild).Margin = new Padding(4, 3, 4, 3);
		((Control)chBxChild).Name = "chBxChild";
		((Control)chBxChild).Size = new Size(156, 21);
		((BaseItemControl)chBxChild).Style = (eDotNetBarStyle)9;
		((Control)chBxChild).TabIndex = 8;
		((Control)chBxChild).Text = "حق اولاد";
		((Control)labelX16).BackColor = Color.Transparent;
		((BaseItemControl)labelX16).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX16).Location = new Point(178, 62);
		((Control)labelX16).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX16).Name = "labelX16";
		((Control)labelX16).Size = new Size(83, 21);
		((Control)labelX16).TabIndex = 0;
		((Control)labelX16).Text = "[گروه سنواتی]:";
		labelX16.TextAlignment = (StringAlignment)2;
		((ListControl)cmbGroupSanavat).DisplayMember = "Name";
		((ComboBox)cmbGroupSanavat).DrawMode = (DrawMode)1;
		((ComboBox)cmbGroupSanavat).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cmbGroupSanavat).FormattingEnabled = true;
		((ComboBox)cmbGroupSanavat).ItemHeight = 15;
		((Control)cmbGroupSanavat).Location = new Point(73, 62);
		((Control)cmbGroupSanavat).Margin = new Padding(4, 3, 4, 3);
		((Control)cmbGroupSanavat).Name = "cmbGroupSanavat";
		((Control)cmbGroupSanavat).Size = new Size(99, 21);
		cmbGroupSanavat.Style = (eDotNetBarStyle)9;
		((Control)cmbGroupSanavat).TabIndex = 3;
		((ListControl)cmbGroupSanavat).ValueMember = "Id";
		((Control)labelX18).Anchor = (AnchorStyles)9;
		((Control)labelX18).BackColor = Color.Transparent;
		((BaseItemControl)labelX18).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX18).Location = new Point(563, 62);
		((Control)labelX18).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX18).Name = "labelX18";
		((Control)labelX18).Size = new Size(119, 21);
		((Control)labelX18).TabIndex = 0;
		((Control)labelX18).Text = "[سابقه خدمت(ماه)]:";
		labelX18.TextAlignment = (StringAlignment)2;
		((Control)TxtLeavHoures).Location = new Point(14, 367);
		((Control)TxtLeavHoures).Margin = new Padding(4, 3, 4, 3);
		TxtLeavHoures.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)TxtLeavHoures).Name = "TxtLeavHoures";
		((Control)TxtLeavHoures).Size = new Size(46, 20);
		((Control)TxtLeavHoures).TabIndex = 27;
		((UpDownBase)TxtLeavHoures).TextAlign = (HorizontalAlignment)2;
		TxtLeavHoures.ValueChanged += TxtLeavHoures_ValueChanged;
		((Control)TxtLeavMinutes).Location = new Point(76, 367);
		((Control)TxtLeavMinutes).Margin = new Padding(4, 3, 4, 3);
		TxtLeavMinutes.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)TxtLeavMinutes).Name = "TxtLeavMinutes";
		((Control)TxtLeavMinutes).Size = new Size(48, 20);
		((Control)TxtLeavMinutes).TabIndex = 28;
		((UpDownBase)TxtLeavMinutes).TextAlign = (HorizontalAlignment)2;
		TxtLeavMinutes.ValueChanged += TxtLeavHoures_ValueChanged;
		((Control)LblPriceLowTime).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceLowTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceLowTime).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)LblPriceLowTime).Location = new Point(178, 140);
		((Control)LblPriceLowTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceLowTime).Name = "LblPriceLowTime";
		((Control)LblPriceLowTime).Size = new Size(172, 21);
		((Control)LblPriceLowTime).TabIndex = 0;
		((Control)LblPriceLowTime).Text = "[کسر کار اول وقت (ساعت)]:";
		LblPriceLowTime.TextAlignment = (StringAlignment)2;
		((Control)labelX31).BackColor = Color.Transparent;
		((BaseItemControl)labelX31).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX31).Location = new Point(178, 340);
		((Control)labelX31).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX31).Name = "labelX31";
		((Control)labelX31).Size = new Size(346, 21);
		((Control)labelX31).TabIndex = 0;
		((Control)labelX31).Text = "[تعداد روزهای مرخصی منتقل شده از قرارداد قبل به قرارداد حاضر]:";
		labelX31.TextAlignment = (StringAlignment)2;
		((Control)labelX17).BackColor = Color.Transparent;
		((BaseItemControl)labelX17).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX17).Location = new Point(57, 367);
		((Control)labelX17).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX17).Name = "labelX17";
		((Control)labelX17).Size = new Size(20, 21);
		((Control)labelX17).TabIndex = 0;
		((Control)labelX17).Text = " : ";
		labelX17.TextAlignment = (StringAlignment)1;
		((Control)labelX40).BackColor = Color.Transparent;
		((BaseItemControl)labelX40).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX40).Location = new Point(178, 365);
		((Control)labelX40).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX40).Name = "labelX40";
		((Control)labelX40).Size = new Size(200, 21);
		((Control)labelX40).TabIndex = 0;
		((Control)labelX40).Text = "[ساعت کارکرد معادل یک روز کاری]:";
		labelX40.TextAlignment = (StringAlignment)2;
		((Control)LblPriceOverTime).Anchor = (AnchorStyles)9;
		((Control)LblPriceOverTime).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceOverTime).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceOverTime).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)LblPriceOverTime).Location = new Point(563, 137);
		((Control)LblPriceOverTime).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceOverTime).Name = "LblPriceOverTime";
		((Control)LblPriceOverTime).Size = new Size(164, 21);
		((Control)LblPriceOverTime).TabIndex = 0;
		((Control)LblPriceOverTime).Text = "[اضافه کار اول وقت(ساعت)]:";
		LblPriceOverTime.TextAlignment = (StringAlignment)2;
		((Control)LblPriceLeave).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceLeave).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceLeave).Location = new Point(178, 265);
		((Control)LblPriceLeave).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceLeave).Name = "LblPriceLeave";
		((Control)LblPriceLeave).Size = new Size(104, 21);
		((Control)LblPriceLeave).TabIndex = 0;
		((Control)LblPriceLeave).Text = "[مرخصی ساعتی]:";
		LblPriceLeave.TextAlignment = (StringAlignment)2;
		((Control)CmbMariage).Anchor = (AnchorStyles)13;
		((ListControl)CmbMariage).DisplayMember = "Text";
		((ComboBox)CmbMariage).DrawMode = (DrawMode)1;
		((ComboBox)CmbMariage).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbMariage).FormattingEnabled = true;
		((ComboBox)CmbMariage).ItemHeight = 15;
		CmbMariage.Items.AddRange(new object[2] { CmbMarried, CmbSingle });
		((Control)CmbMariage).Location = new Point(401, 90);
		((Control)CmbMariage).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbMariage).Name = "CmbMariage";
		((Control)CmbMariage).Size = new Size(158, 21);
		CmbMariage.Style = (eDotNetBarStyle)9;
		((Control)CmbMariage).TabIndex = 4;
		((ComboBox)CmbMariage).SelectedIndexChanged += CmbMariage_SelectedIndexChanged;
		CmbMarried.FontSize = 15f;
		CmbMarried.FontStyle = (FontStyle)1;
		CmbMarried.Text = "[متاهل]";
		CmbMarried.TextAlignment = (StringAlignment)2;
		CmbSingle.FontSize = 15f;
		CmbSingle.FontStyle = (FontStyle)1;
		CmbSingle.Text = "[مجرد]";
		CmbSingle.TextAlignment = (StringAlignment)2;
		((Control)LblPriceHoliday).Anchor = (AnchorStyles)9;
		((Control)LblPriceHoliday).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceHoliday).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceHoliday).Location = new Point(563, 312);
		((Control)LblPriceHoliday).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceHoliday).Name = "LblPriceHoliday";
		((Control)LblPriceHoliday).Size = new Size(127, 21);
		((Control)LblPriceHoliday).TabIndex = 0;
		((Control)LblPriceHoliday).Text = "[جمعه کاری (ساعت)]:";
		LblPriceHoliday.TextAlignment = (StringAlignment)2;
		((Control)TxtLeaveSaved).Location = new Point(14, 342);
		((Control)TxtLeaveSaved).Margin = new Padding(4, 3, 4, 3);
		TxtLeaveSaved.Minimum = new decimal(new int[4] { 30, 0, 0, -2147483648 });
		((Control)TxtLeaveSaved).Name = "TxtLeaveSaved";
		((Control)TxtLeaveSaved).Size = new Size(158, 20);
		((Control)TxtLeaveSaved).TabIndex = 26;
		((UpDownBase)TxtLeaveSaved).TextAlign = (HorizontalAlignment)2;
		((Control)NumberCountChild).Anchor = (AnchorStyles)13;
		((Control)NumberCountChild).Location = new Point(14, 88);
		((Control)NumberCountChild).Margin = new Padding(4, 3, 4, 3);
		NumberCountChild.Maximum = new decimal(new int[4] { 10, 0, 0, 0 });
		((Control)NumberCountChild).Name = "NumberCountChild";
		((Control)NumberCountChild).Size = new Size(158, 20);
		((Control)NumberCountChild).TabIndex = 5;
		((UpDownBase)NumberCountChild).TextAlign = (HorizontalAlignment)2;
		NumberCountChild.ValueChanged += NumberCountChild_ValueChanged;
		((Control)LblPriceNight).Anchor = (AnchorStyles)9;
		((Control)LblPriceNight).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceNight).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceNight).Location = new Point(563, 212);
		((Control)LblPriceNight).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceNight).Name = "LblPriceNight";
		((Control)LblPriceNight).Size = new Size(127, 21);
		((Control)LblPriceNight).TabIndex = 0;
		((Control)LblPriceNight).Text = "[شب کاری (ساعت)]:";
		LblPriceNight.TextAlignment = (StringAlignment)2;
		((Control)labelX6).BackColor = Color.Transparent;
		((BaseItemControl)labelX6).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX6).Location = new Point(178, 37);
		((Control)labelX6).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX6).Name = "labelX6";
		((Control)labelX6).Size = new Size(83, 21);
		((Control)labelX6).TabIndex = 0;
		((Control)labelX6).Text = "[میزان حقوق]:";
		labelX6.TextAlignment = (StringAlignment)2;
		((Control)labelX20).Anchor = (AnchorStyles)9;
		((Control)labelX20).BackColor = Color.Transparent;
		((BaseItemControl)labelX20).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX20).Location = new Point(178, 89);
		((Control)labelX20).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX20).Name = "labelX20";
		((Control)labelX20).Size = new Size(103, 21);
		((Control)labelX20).TabIndex = 0;
		((Control)labelX20).Text = " [فرزندان مشمول]:";
		labelX20.TextAlignment = (StringAlignment)2;
		((Control)LblPriceAbsent).BackColor = Color.Transparent;
		((BaseItemControl)LblPriceAbsent).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblPriceAbsent).Location = new Point(178, 215);
		((Control)LblPriceAbsent).Margin = new Padding(4, 3, 4, 3);
		((Control)LblPriceAbsent).Name = "LblPriceAbsent";
		((Control)LblPriceAbsent).Size = new Size(104, 21);
		((Control)LblPriceAbsent).TabIndex = 0;
		((Control)LblPriceAbsent).Text = "[غیبت (روزانه)]:";
		LblPriceAbsent.TextAlignment = (StringAlignment)2;
		((Control)CmbSalaryType).Anchor = (AnchorStyles)13;
		((ListControl)CmbSalaryType).DisplayMember = "Name";
		((ComboBox)CmbSalaryType).DrawMode = (DrawMode)1;
		((ComboBox)CmbSalaryType).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbSalaryType).FormattingEnabled = true;
		((ComboBox)CmbSalaryType).ItemHeight = 15;
		((Control)CmbSalaryType).Location = new Point(401, 37);
		((Control)CmbSalaryType).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbSalaryType).Name = "CmbSalaryType";
		((Control)CmbSalaryType).Size = new Size(158, 21);
		CmbSalaryType.Style = (eDotNetBarStyle)9;
		((Control)CmbSalaryType).TabIndex = 0;
		((ListControl)CmbSalaryType).ValueMember = "Id";
		((Control)labelX19).Anchor = (AnchorStyles)9;
		((Control)labelX19).BackColor = Color.Transparent;
		((BaseItemControl)labelX19).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX19).Location = new Point(563, 89);
		((Control)labelX19).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX19).Name = "labelX19";
		((Control)labelX19).Size = new Size(120, 21);
		((Control)labelX19).TabIndex = 0;
		((Control)labelX19).Text = "[وضعیت ازدواج]:";
		labelX19.TextAlignment = (StringAlignment)2;
		((Control)BtnDelGroupSanavat).AccessibleRole = (AccessibleRole)43;
		BtnDelGroupSanavat.ColorTable = (eButtonColor)3;
		BtnDelGroupSanavat.Image = (Image)(object)Resources.edit_delete;
		((Control)BtnDelGroupSanavat).Location = new Point(14, 62);
		((Control)BtnDelGroupSanavat).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnDelGroupSanavat).Name = "BtnDelGroupSanavat";
		((Control)BtnDelGroupSanavat).Size = new Size(23, 21);
		((PopupItemControl)BtnDelGroupSanavat).Style = (eDotNetBarStyle)9;
		((Control)BtnDelGroupSanavat).TabIndex = 0;
		((Control)BtnDelGroupSanavat).TabStop = false;
		((Control)BtnDelGroupSanavat).Click += BtnDelGroupSanavat_Click;
		((Control)labelX4).Anchor = (AnchorStyles)9;
		((Control)labelX4).BackColor = Color.Transparent;
		((BaseItemControl)labelX4).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX4).Location = new Point(563, 37);
		((Control)labelX4).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX4).Name = "labelX4";
		((Control)labelX4).Size = new Size(119, 21);
		((Control)labelX4).TabIndex = 0;
		((Control)labelX4).Text = "[نوع پرداخت حقوق]:";
		labelX4.TextAlignment = (StringAlignment)2;
		((Control)btnCalOverLowTime).AccessibleRole = (AccessibleRole)43;
		((Control)btnCalOverLowTime).BackColor = Color.Transparent;
		((Control)btnCalOverLowTime).BackgroundImageLayout = (ImageLayout)2;
		btnCalOverLowTime.ColorTable = (eButtonColor)6;
		btnCalOverLowTime.Image = (Image)(object)Resources.Calculator;
		((Control)btnCalOverLowTime).Location = new Point(14, 37);
		((Control)btnCalOverLowTime).Margin = new Padding(4, 3, 4, 3);
		((Control)btnCalOverLowTime).Name = "btnCalOverLowTime";
		((Control)btnCalOverLowTime).Size = new Size(20, 21);
		btnCalOverLowTime.SplitButton = true;
		((PopupItemControl)btnCalOverLowTime).Style = (eDotNetBarStyle)9;
		((Control)btnCalOverLowTime).TabIndex = 0;
		((Control)btnCalOverLowTime).TabStop = false;
		((Control)btnCalOverLowTime).Click += BtnCalOverLowTime_Click;
		TxtPriceJarime.AllowEmpty = true;
		TxtPriceJarime.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceJarime.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceJarime.BackcolorTooltip = Color.Red;
		TxtPriceJarime.CurrencyFormat = "";
		((Control)TxtPriceJarime).ForeColor = Color.Red;
		TxtPriceJarime.ForeColorTooltip = Color.Black;
		TxtPriceJarime.GetColoresFromSettingDLL = true;
		TxtPriceJarime.HandleKeydown = true;
		((Control)TxtPriceJarime).Location = new Point(14, 240);
		((Control)TxtPriceJarime).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceJarime).MaxLength = 10;
		((Control)TxtPriceJarime).Name = "TxtPriceJarime";
		TxtPriceJarime.SendShiftTabAfterDown = true;
		TxtPriceJarime.SendShiftTabAfterEnter = true;
		TxtPriceJarime.SendTabAfterEnter = true;
		TxtPriceJarime.SendTabAfterUP = true;
		TxtPriceJarime.ShowHorofi = (ENShowHorofi)3;
		TxtPriceJarime.ShowToolTipText = true;
		((Control)TxtPriceJarime).Size = new Size(158, 20);
		((Control)TxtPriceJarime).TabIndex = 22;
		((Control)TxtPriceJarime).Text = "0";
		TxtPriceJarime.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceJarime.ToolTipPostFix = "";
		TxtPriceJarime.TootTipDelay = 1000;
		TxtPriceJarime.Value = new decimal(new int[4]);
		TxtPriceMission.AllowEmpty = true;
		TxtPriceMission.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceMission.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceMission.BackcolorTooltip = Color.Red;
		TxtPriceMission.CurrencyFormat = "";
		((Control)TxtPriceMission).ForeColor = Color.Green;
		TxtPriceMission.ForeColorTooltip = Color.Black;
		TxtPriceMission.GetColoresFromSettingDLL = true;
		TxtPriceMission.HandleKeydown = true;
		((Control)TxtPriceMission).Location = new Point(14, 290);
		((Control)TxtPriceMission).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceMission).MaxLength = 10;
		((Control)TxtPriceMission).Name = "TxtPriceMission";
		TxtPriceMission.SendShiftTabAfterDown = true;
		TxtPriceMission.SendShiftTabAfterEnter = true;
		TxtPriceMission.SendTabAfterEnter = true;
		TxtPriceMission.SendTabAfterUP = true;
		TxtPriceMission.ShowHorofi = (ENShowHorofi)3;
		TxtPriceMission.ShowToolTipText = true;
		((Control)TxtPriceMission).Size = new Size(158, 20);
		((Control)TxtPriceMission).TabIndex = 24;
		((Control)TxtPriceMission).Text = "0";
		TxtPriceMission.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceMission.ToolTipPostFix = "";
		TxtPriceMission.TootTipDelay = 1000;
		TxtPriceMission.Value = new decimal(new int[4]);
		TxtPriceTatil.AllowEmpty = true;
		TxtPriceTatil.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceTatil.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceTatil.BackcolorTooltip = Color.Red;
		TxtPriceTatil.CurrencyFormat = "";
		((Control)TxtPriceTatil).ForeColor = Color.Green;
		TxtPriceTatil.ForeColorTooltip = Color.Black;
		TxtPriceTatil.GetColoresFromSettingDLL = true;
		TxtPriceTatil.HandleKeydown = true;
		((Control)TxtPriceTatil).Location = new Point(14, 315);
		((Control)TxtPriceTatil).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceTatil).MaxLength = 10;
		((Control)TxtPriceTatil).Name = "TxtPriceTatil";
		TxtPriceTatil.SendShiftTabAfterDown = true;
		TxtPriceTatil.SendShiftTabAfterEnter = true;
		TxtPriceTatil.SendTabAfterEnter = true;
		TxtPriceTatil.SendTabAfterUP = true;
		TxtPriceTatil.ShowHorofi = (ENShowHorofi)3;
		TxtPriceTatil.ShowToolTipText = true;
		((Control)TxtPriceTatil).Size = new Size(158, 20);
		((Control)TxtPriceTatil).TabIndex = 25;
		((Control)TxtPriceTatil).Text = "0";
		TxtPriceTatil.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceTatil.ToolTipPostFix = "";
		TxtPriceTatil.TootTipDelay = 1000;
		TxtPriceTatil.Value = new decimal(new int[4]);
		TxtPriceLowTime3.AllowEmpty = true;
		TxtPriceLowTime3.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceLowTime3.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceLowTime3.BackcolorTooltip = Color.Red;
		TxtPriceLowTime3.CurrencyFormat = "";
		((Control)TxtPriceLowTime3).ForeColor = Color.Red;
		TxtPriceLowTime3.ForeColorTooltip = Color.Black;
		TxtPriceLowTime3.GetColoresFromSettingDLL = true;
		TxtPriceLowTime3.HandleKeydown = true;
		((Control)TxtPriceLowTime3).Location = new Point(14, 190);
		((Control)TxtPriceLowTime3).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceLowTime3).MaxLength = 10;
		((Control)TxtPriceLowTime3).Name = "TxtPriceLowTime3";
		TxtPriceLowTime3.SendShiftTabAfterDown = true;
		TxtPriceLowTime3.SendShiftTabAfterEnter = true;
		TxtPriceLowTime3.SendTabAfterEnter = true;
		TxtPriceLowTime3.SendTabAfterUP = true;
		TxtPriceLowTime3.ShowHorofi = (ENShowHorofi)3;
		TxtPriceLowTime3.ShowToolTipText = true;
		((Control)TxtPriceLowTime3).Size = new Size(158, 20);
		((Control)TxtPriceLowTime3).TabIndex = 20;
		((Control)TxtPriceLowTime3).Text = "0";
		TxtPriceLowTime3.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceLowTime3.ToolTipPostFix = "";
		TxtPriceLowTime3.TootTipDelay = 1000;
		TxtPriceLowTime3.Value = new decimal(new int[4]);
		TxtPriceOverTime3.AllowEmpty = true;
		((Control)TxtPriceOverTime3).Anchor = (AnchorStyles)13;
		TxtPriceOverTime3.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceOverTime3.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceOverTime3.BackcolorTooltip = Color.Red;
		TxtPriceOverTime3.CurrencyFormat = "";
		((Control)TxtPriceOverTime3).ForeColor = Color.Green;
		TxtPriceOverTime3.ForeColorTooltip = Color.Black;
		TxtPriceOverTime3.GetColoresFromSettingDLL = true;
		TxtPriceOverTime3.HandleKeydown = true;
		((Control)TxtPriceOverTime3).Location = new Point(401, 189);
		((Control)TxtPriceOverTime3).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceOverTime3).MaxLength = 10;
		((Control)TxtPriceOverTime3).Name = "TxtPriceOverTime3";
		TxtPriceOverTime3.SendShiftTabAfterDown = true;
		TxtPriceOverTime3.SendShiftTabAfterEnter = true;
		TxtPriceOverTime3.SendTabAfterEnter = true;
		TxtPriceOverTime3.SendTabAfterUP = true;
		TxtPriceOverTime3.ShowHorofi = (ENShowHorofi)3;
		TxtPriceOverTime3.ShowToolTipText = true;
		((Control)TxtPriceOverTime3).Size = new Size(158, 20);
		((Control)TxtPriceOverTime3).TabIndex = 12;
		((Control)TxtPriceOverTime3).Text = "0";
		TxtPriceOverTime3.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceOverTime3.ToolTipPostFix = "";
		TxtPriceOverTime3.TootTipDelay = 1000;
		TxtPriceOverTime3.Value = new decimal(new int[4]);
		TxtPriceNobatkari1.AllowEmpty = true;
		((Control)TxtPriceNobatkari1).Anchor = (AnchorStyles)13;
		TxtPriceNobatkari1.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceNobatkari1.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceNobatkari1.BackcolorTooltip = Color.Red;
		TxtPriceNobatkari1.CurrencyFormat = "";
		((Control)TxtPriceNobatkari1).ForeColor = Color.Green;
		TxtPriceNobatkari1.ForeColorTooltip = Color.Black;
		TxtPriceNobatkari1.GetColoresFromSettingDLL = true;
		TxtPriceNobatkari1.HandleKeydown = true;
		((Control)TxtPriceNobatkari1).Location = new Point(401, 239);
		((Control)TxtPriceNobatkari1).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceNobatkari1).MaxLength = 10;
		((Control)TxtPriceNobatkari1).Name = "TxtPriceNobatkari1";
		TxtPriceNobatkari1.SendShiftTabAfterDown = true;
		TxtPriceNobatkari1.SendShiftTabAfterEnter = true;
		TxtPriceNobatkari1.SendTabAfterEnter = true;
		TxtPriceNobatkari1.SendTabAfterUP = true;
		TxtPriceNobatkari1.ShowHorofi = (ENShowHorofi)3;
		TxtPriceNobatkari1.ShowToolTipText = true;
		((Control)TxtPriceNobatkari1).Size = new Size(158, 20);
		((Control)TxtPriceNobatkari1).TabIndex = 14;
		((Control)TxtPriceNobatkari1).Text = "0";
		TxtPriceNobatkari1.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceNobatkari1.ToolTipPostFix = "";
		TxtPriceNobatkari1.TootTipDelay = 1000;
		TxtPriceNobatkari1.Value = new decimal(new int[4]);
		TxtPriceLowTime2.AllowEmpty = true;
		TxtPriceLowTime2.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceLowTime2.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceLowTime2.BackcolorTooltip = Color.Red;
		TxtPriceLowTime2.CurrencyFormat = "";
		((Control)TxtPriceLowTime2).ForeColor = Color.Red;
		TxtPriceLowTime2.ForeColorTooltip = Color.Black;
		TxtPriceLowTime2.GetColoresFromSettingDLL = true;
		TxtPriceLowTime2.HandleKeydown = true;
		((Control)TxtPriceLowTime2).Location = new Point(14, 165);
		((Control)TxtPriceLowTime2).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceLowTime2).MaxLength = 10;
		((Control)TxtPriceLowTime2).Name = "TxtPriceLowTime2";
		TxtPriceLowTime2.SendShiftTabAfterDown = true;
		TxtPriceLowTime2.SendShiftTabAfterEnter = true;
		TxtPriceLowTime2.SendTabAfterEnter = true;
		TxtPriceLowTime2.SendTabAfterUP = true;
		TxtPriceLowTime2.ShowHorofi = (ENShowHorofi)3;
		TxtPriceLowTime2.ShowToolTipText = true;
		((Control)TxtPriceLowTime2).Size = new Size(158, 20);
		((Control)TxtPriceLowTime2).TabIndex = 19;
		((Control)TxtPriceLowTime2).Text = "0";
		TxtPriceLowTime2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceLowTime2.ToolTipPostFix = "";
		TxtPriceLowTime2.TootTipDelay = 1000;
		TxtPriceLowTime2.Value = new decimal(new int[4]);
		TxtPriceOverTime2.AllowEmpty = true;
		((Control)TxtPriceOverTime2).Anchor = (AnchorStyles)13;
		TxtPriceOverTime2.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceOverTime2.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceOverTime2.BackcolorTooltip = Color.Red;
		TxtPriceOverTime2.CurrencyFormat = "";
		((Control)TxtPriceOverTime2).ForeColor = Color.Green;
		TxtPriceOverTime2.ForeColorTooltip = Color.Black;
		TxtPriceOverTime2.GetColoresFromSettingDLL = true;
		TxtPriceOverTime2.HandleKeydown = true;
		((Control)TxtPriceOverTime2).Location = new Point(401, 164);
		((Control)TxtPriceOverTime2).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceOverTime2).MaxLength = 10;
		((Control)TxtPriceOverTime2).Name = "TxtPriceOverTime2";
		TxtPriceOverTime2.SendShiftTabAfterDown = true;
		TxtPriceOverTime2.SendShiftTabAfterEnter = true;
		TxtPriceOverTime2.SendTabAfterEnter = true;
		TxtPriceOverTime2.SendTabAfterUP = true;
		TxtPriceOverTime2.ShowHorofi = (ENShowHorofi)3;
		TxtPriceOverTime2.ShowToolTipText = true;
		((Control)TxtPriceOverTime2).Size = new Size(158, 20);
		((Control)TxtPriceOverTime2).TabIndex = 11;
		((Control)TxtPriceOverTime2).Text = "0";
		TxtPriceOverTime2.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceOverTime2.ToolTipPostFix = "";
		TxtPriceOverTime2.TootTipDelay = 1000;
		TxtPriceOverTime2.Value = new decimal(new int[4]);
		TxtSanavat.AllowEmpty = true;
		((Control)TxtSanavat).Anchor = (AnchorStyles)13;
		TxtSanavat.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtSanavat.BackColorTextBoxWhenLostFocus = Color.White;
		TxtSanavat.BackcolorTooltip = Color.Red;
		TxtSanavat.CurrencyFormat = "";
		((Control)TxtSanavat).ForeColor = Color.Green;
		TxtSanavat.ForeColorTooltip = Color.Black;
		TxtSanavat.GetColoresFromSettingDLL = true;
		TxtSanavat.HandleKeydown = true;
		((Control)TxtSanavat).Location = new Point(401, 64);
		((Control)TxtSanavat).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtSanavat).MaxLength = 3;
		((Control)TxtSanavat).Name = "TxtSanavat";
		TxtSanavat.SendShiftTabAfterDown = true;
		TxtSanavat.SendShiftTabAfterEnter = true;
		TxtSanavat.SendTabAfterEnter = true;
		TxtSanavat.SendTabAfterUP = true;
		TxtSanavat.ShowHorofi = (ENShowHorofi)0;
		TxtSanavat.ShowToolTipText = true;
		((Control)TxtSanavat).Size = new Size(158, 20);
		((Control)TxtSanavat).TabIndex = 2;
		((Control)TxtSanavat).Text = "0";
		TxtSanavat.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtSanavat.ToolTipPostFix = " [ماه]";
		TxtSanavat.TootTipDelay = 5000;
		TxtSanavat.Value = new decimal(new int[4]);
		TxtPriceLowTime.AllowEmpty = true;
		TxtPriceLowTime.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceLowTime.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceLowTime.BackcolorTooltip = Color.Red;
		TxtPriceLowTime.CurrencyFormat = "";
		((Control)TxtPriceLowTime).ForeColor = Color.Red;
		TxtPriceLowTime.ForeColorTooltip = Color.Black;
		TxtPriceLowTime.GetColoresFromSettingDLL = true;
		TxtPriceLowTime.HandleKeydown = true;
		((Control)TxtPriceLowTime).Location = new Point(14, 140);
		((Control)TxtPriceLowTime).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceLowTime).MaxLength = 10;
		((Control)TxtPriceLowTime).Name = "TxtPriceLowTime";
		TxtPriceLowTime.SendShiftTabAfterDown = true;
		TxtPriceLowTime.SendShiftTabAfterEnter = true;
		TxtPriceLowTime.SendTabAfterEnter = true;
		TxtPriceLowTime.SendTabAfterUP = true;
		TxtPriceLowTime.ShowHorofi = (ENShowHorofi)3;
		TxtPriceLowTime.ShowToolTipText = true;
		((Control)TxtPriceLowTime).Size = new Size(158, 20);
		((Control)TxtPriceLowTime).TabIndex = 18;
		((Control)TxtPriceLowTime).Text = "0";
		TxtPriceLowTime.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceLowTime.ToolTipPostFix = "";
		TxtPriceLowTime.TootTipDelay = 1000;
		TxtPriceLowTime.Value = new decimal(new int[4]);
		TxtPriceLeave.AllowEmpty = true;
		TxtPriceLeave.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceLeave.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceLeave.BackcolorTooltip = Color.Red;
		TxtPriceLeave.CurrencyFormat = "";
		((Control)TxtPriceLeave).ForeColor = Color.Green;
		TxtPriceLeave.ForeColorTooltip = Color.Black;
		TxtPriceLeave.GetColoresFromSettingDLL = true;
		TxtPriceLeave.HandleKeydown = true;
		((Control)TxtPriceLeave).Location = new Point(14, 265);
		((Control)TxtPriceLeave).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceLeave).MaxLength = 10;
		((Control)TxtPriceLeave).Name = "TxtPriceLeave";
		TxtPriceLeave.SendShiftTabAfterDown = true;
		TxtPriceLeave.SendShiftTabAfterEnter = true;
		TxtPriceLeave.SendTabAfterEnter = true;
		TxtPriceLeave.SendTabAfterUP = true;
		TxtPriceLeave.ShowHorofi = (ENShowHorofi)3;
		TxtPriceLeave.ShowToolTipText = true;
		((Control)TxtPriceLeave).Size = new Size(158, 20);
		((Control)TxtPriceLeave).TabIndex = 23;
		((Control)TxtPriceLeave).Text = "0";
		TxtPriceLeave.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceLeave.ToolTipPostFix = "";
		TxtPriceLeave.TootTipDelay = 1000;
		TxtPriceLeave.Value = new decimal(new int[4]);
		TxtPriceOverTime.AllowEmpty = true;
		((Control)TxtPriceOverTime).Anchor = (AnchorStyles)13;
		TxtPriceOverTime.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceOverTime.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceOverTime.BackcolorTooltip = Color.Red;
		TxtPriceOverTime.CurrencyFormat = "";
		((Control)TxtPriceOverTime).ForeColor = Color.Green;
		TxtPriceOverTime.ForeColorTooltip = Color.Black;
		TxtPriceOverTime.GetColoresFromSettingDLL = true;
		TxtPriceOverTime.HandleKeydown = true;
		((Control)TxtPriceOverTime).Location = new Point(401, 139);
		((Control)TxtPriceOverTime).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceOverTime).MaxLength = 10;
		((Control)TxtPriceOverTime).Name = "TxtPriceOverTime";
		TxtPriceOverTime.SendShiftTabAfterDown = true;
		TxtPriceOverTime.SendShiftTabAfterEnter = true;
		TxtPriceOverTime.SendTabAfterEnter = true;
		TxtPriceOverTime.SendTabAfterUP = true;
		TxtPriceOverTime.ShowHorofi = (ENShowHorofi)3;
		TxtPriceOverTime.ShowToolTipText = true;
		((Control)TxtPriceOverTime).Size = new Size(158, 20);
		((Control)TxtPriceOverTime).TabIndex = 10;
		((Control)TxtPriceOverTime).Text = "0";
		TxtPriceOverTime.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceOverTime.ToolTipPostFix = "";
		TxtPriceOverTime.TootTipDelay = 1000;
		TxtPriceOverTime.Value = new decimal(new int[4]);
		TxtPriceAbsent.AllowEmpty = true;
		TxtPriceAbsent.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceAbsent.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceAbsent.BackcolorTooltip = Color.Red;
		TxtPriceAbsent.CurrencyFormat = "";
		((Control)TxtPriceAbsent).ForeColor = Color.Red;
		TxtPriceAbsent.ForeColorTooltip = Color.Black;
		TxtPriceAbsent.GetColoresFromSettingDLL = true;
		TxtPriceAbsent.HandleKeydown = true;
		((Control)TxtPriceAbsent).Location = new Point(14, 215);
		((Control)TxtPriceAbsent).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceAbsent).MaxLength = 10;
		((Control)TxtPriceAbsent).Name = "TxtPriceAbsent";
		TxtPriceAbsent.SendShiftTabAfterDown = true;
		TxtPriceAbsent.SendShiftTabAfterEnter = true;
		TxtPriceAbsent.SendTabAfterEnter = true;
		TxtPriceAbsent.SendTabAfterUP = true;
		TxtPriceAbsent.ShowHorofi = (ENShowHorofi)3;
		TxtPriceAbsent.ShowToolTipText = true;
		((Control)TxtPriceAbsent).Size = new Size(158, 20);
		((Control)TxtPriceAbsent).TabIndex = 21;
		((Control)TxtPriceAbsent).Text = "0";
		TxtPriceAbsent.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceAbsent.ToolTipPostFix = "";
		TxtPriceAbsent.TootTipDelay = 1000;
		TxtPriceAbsent.Value = new decimal(new int[4]);
		TxtPriceHoliday.AllowEmpty = true;
		((Control)TxtPriceHoliday).Anchor = (AnchorStyles)13;
		TxtPriceHoliday.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceHoliday.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceHoliday.BackcolorTooltip = Color.Red;
		TxtPriceHoliday.CurrencyFormat = "";
		((Control)TxtPriceHoliday).ForeColor = Color.Green;
		TxtPriceHoliday.ForeColorTooltip = Color.Black;
		TxtPriceHoliday.GetColoresFromSettingDLL = true;
		TxtPriceHoliday.HandleKeydown = true;
		((Control)TxtPriceHoliday).Location = new Point(401, 314);
		((Control)TxtPriceHoliday).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceHoliday).MaxLength = 10;
		((Control)TxtPriceHoliday).Name = "TxtPriceHoliday";
		TxtPriceHoliday.SendShiftTabAfterDown = true;
		TxtPriceHoliday.SendShiftTabAfterEnter = true;
		TxtPriceHoliday.SendTabAfterEnter = true;
		TxtPriceHoliday.SendTabAfterUP = true;
		TxtPriceHoliday.ShowHorofi = (ENShowHorofi)3;
		TxtPriceHoliday.ShowToolTipText = true;
		((Control)TxtPriceHoliday).Size = new Size(158, 20);
		((Control)TxtPriceHoliday).TabIndex = 17;
		((Control)TxtPriceHoliday).Text = "0";
		TxtPriceHoliday.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceHoliday.ToolTipPostFix = "";
		TxtPriceHoliday.TootTipDelay = 1000;
		TxtPriceHoliday.Value = new decimal(new int[4]);
		TxtPriceNight.AllowEmpty = true;
		((Control)TxtPriceNight).Anchor = (AnchorStyles)13;
		TxtPriceNight.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtPriceNight.BackColorTextBoxWhenLostFocus = Color.White;
		TxtPriceNight.BackcolorTooltip = Color.Red;
		TxtPriceNight.CurrencyFormat = "";
		((Control)TxtPriceNight).ForeColor = Color.Green;
		TxtPriceNight.ForeColorTooltip = Color.Black;
		TxtPriceNight.GetColoresFromSettingDLL = true;
		TxtPriceNight.HandleKeydown = true;
		((Control)TxtPriceNight).Location = new Point(401, 214);
		((Control)TxtPriceNight).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPriceNight).MaxLength = 10;
		((Control)TxtPriceNight).Name = "TxtPriceNight";
		TxtPriceNight.SendShiftTabAfterDown = true;
		TxtPriceNight.SendShiftTabAfterEnter = true;
		TxtPriceNight.SendTabAfterEnter = true;
		TxtPriceNight.SendTabAfterUP = true;
		TxtPriceNight.ShowHorofi = (ENShowHorofi)3;
		TxtPriceNight.ShowToolTipText = true;
		((Control)TxtPriceNight).Size = new Size(158, 20);
		((Control)TxtPriceNight).TabIndex = 13;
		((Control)TxtPriceNight).Text = "0";
		TxtPriceNight.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtPriceNight.ToolTipPostFix = "";
		TxtPriceNight.TootTipDelay = 1000;
		TxtPriceNight.Value = new decimal(new int[4]);
		TxtSalary.AllowEmpty = true;
		TxtSalary.BackColorTextBoxWhenFocus = Color.Yellow;
		TxtSalary.BackColorTextBoxWhenLostFocus = Color.White;
		TxtSalary.BackcolorTooltip = Color.Red;
		TxtSalary.CurrencyFormat = "";
		TxtSalary.ForeColorTooltip = Color.Black;
		TxtSalary.GetColoresFromSettingDLL = true;
		TxtSalary.HandleKeydown = true;
		((Control)TxtSalary).Location = new Point(36, 37);
		((Control)TxtSalary).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtSalary).MaxLength = 20;
		((Control)TxtSalary).Name = "TxtSalary";
		TxtSalary.SendShiftTabAfterDown = true;
		TxtSalary.SendShiftTabAfterEnter = true;
		TxtSalary.SendTabAfterEnter = true;
		TxtSalary.SendTabAfterUP = true;
		TxtSalary.ShowHorofi = (ENShowHorofi)3;
		TxtSalary.ShowToolTipText = true;
		((Control)TxtSalary).Size = new Size(136, 20);
		((Control)TxtSalary).TabIndex = 1;
		((Control)TxtSalary).Text = "0";
		TxtSalary.ToolTipFont = new Font("B Traffic", 15f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		TxtSalary.ToolTipPostFix = "";
		TxtSalary.TootTipDelay = 2000;
		TxtSalary.Value = new decimal(new int[4]);
		tabItem2.AttachedControl = (Control)(object)tabControlPanel2;
		tabItem2.Name = "tabItem2";
		tabItem2.Text = "[اطلاعات حقوقی]";
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX22);
		((Control)tabControlPanel1).Controls.Add((Control)(object)CmbGharardadStatus);
		((Control)tabControlPanel1).Controls.Add((Control)(object)numberCod);
		((Control)tabControlPanel1).Controls.Add((Control)(object)BtnmoreCompany);
		((Control)tabControlPanel1).Controls.Add((Control)(object)lblname);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX12);
		((Control)tabControlPanel1).Controls.Add((Control)(object)BtnMoreP);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX11);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TxtcodePerson);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX5);
		((Control)tabControlPanel1).Controls.Add((Control)(object)BTNMore);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX3);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX2);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX1);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX8);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TxtPeyvast);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX10);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TxtDesc);
		((Control)tabControlPanel1).Controls.Add((Control)(object)CmbShift);
		((Control)tabControlPanel1).Controls.Add((Control)(object)labelX7);
		((Control)tabControlPanel1).Controls.Add((Control)(object)CmbCompany);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TXTDateRegister);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TxtEndDate);
		((Control)tabControlPanel1).Controls.Add((Control)(object)TXTStartDate);
		((PanelEx)tabControlPanel1).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel1).Dock = (DockStyle)5;
		((Control)tabControlPanel1).Location = new Point(0, 26);
		((Control)tabControlPanel1).Margin = new Padding(4, 3, 4, 3);
		((Control)tabControlPanel1).Name = "tabControlPanel1";
		((Control)tabControlPanel1).Padding = new Padding(1);
		((Control)tabControlPanel1).Size = new Size(728, 424);
		((PanelEx)tabControlPanel1).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel1).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel1).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel1).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel1).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel1).Style.GradientAngle = 90;
		((Control)tabControlPanel1).TabIndex = 0;
		tabControlPanel1.TabItem = tabItem1;
		((Control)labelX22).Anchor = (AnchorStyles)9;
		((Control)labelX22).BackColor = Color.Transparent;
		((BaseItemControl)labelX22).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX22).Location = new Point(632, 32);
		((Control)labelX22).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX22).Name = "labelX22";
		((Control)labelX22).Size = new Size(86, 23);
		((Control)labelX22).TabIndex = 23;
		((Control)labelX22).Text = "وضعیت قرارداد:";
		labelX22.TextAlignment = (StringAlignment)2;
		((Control)CmbGharardadStatus).Anchor = (AnchorStyles)13;
		((ListControl)CmbGharardadStatus).DisplayMember = "Name";
		((ComboBox)CmbGharardadStatus).DrawMode = (DrawMode)1;
		((ComboBox)CmbGharardadStatus).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbGharardadStatus).FormattingEnabled = true;
		((ComboBox)CmbGharardadStatus).ItemHeight = 15;
		((Control)CmbGharardadStatus).Location = new Point(464, 35);
		((Control)CmbGharardadStatus).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbGharardadStatus).Name = "CmbGharardadStatus";
		((Control)CmbGharardadStatus).Size = new Size(165, 21);
		CmbGharardadStatus.Style = (eDotNetBarStyle)9;
		((Control)CmbGharardadStatus).TabIndex = 22;
		((ListControl)CmbGharardadStatus).ValueMember = "Id";
		((Control)numberCod).Anchor = (AnchorStyles)13;
		((Control)numberCod).Location = new Point(464, 9);
		((Control)numberCod).Margin = new Padding(4, 3, 4, 3);
		numberCod.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		numberCod.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)numberCod).Name = "numberCod";
		((Control)numberCod).Size = new Size(165, 20);
		((Control)numberCod).TabIndex = 1;
		((UpDownBase)numberCod).TextAlign = (HorizontalAlignment)2;
		numberCod.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		((Control)BtnmoreCompany).AccessibleRole = (AccessibleRole)43;
		BtnmoreCompany.ColorTable = (eButtonColor)6;
		((Control)BtnmoreCompany).Location = new Point(9, 91);
		((Control)BtnmoreCompany).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnmoreCompany).Name = "BtnmoreCompany";
		((Control)BtnmoreCompany).Size = new Size(30, 21);
		((PopupItemControl)BtnmoreCompany).Style = (eDotNetBarStyle)9;
		((Control)BtnmoreCompany).TabIndex = 14;
		((Control)BtnmoreCompany).TabStop = false;
		((Control)BtnmoreCompany).Text = "...";
		((Control)BtnmoreCompany).Click += BtnmoreCompany_Click;
		((Control)lblname).Anchor = (AnchorStyles)13;
		((Control)lblname).BackColor = Color.Transparent;
		((BaseItemControl)lblname).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)lblname).ForeColor = Color.DarkRed;
		((Control)lblname).Location = new Point(287, 60);
		((Control)lblname).Margin = new Padding(4, 3, 4, 3);
		((Control)lblname).Name = "lblname";
		((Control)lblname).RightToLeft = (RightToLeft)1;
		((Control)lblname).Size = new Size(170, 23);
		((Control)lblname).TabIndex = 11;
		((Control)lblname).Text = "----------";
		((Control)labelX12).Anchor = (AnchorStyles)9;
		((Control)labelX12).BackColor = Color.Transparent;
		((BaseItemControl)labelX12).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX12).Location = new Point(632, 7);
		((Control)labelX12).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX12).Name = "labelX12";
		((Control)labelX12).Size = new Size(86, 23);
		((Control)labelX12).TabIndex = 0;
		((Control)labelX12).Text = "[شماره قرارداد]:";
		labelX12.TextAlignment = (StringAlignment)2;
		((Control)BtnMoreP).AccessibleRole = (AccessibleRole)43;
		((Control)BtnMoreP).Anchor = (AnchorStyles)9;
		BtnMoreP.ColorTable = (eButtonColor)6;
		((Control)BtnMoreP).Location = new Point(464, 60);
		((Control)BtnMoreP).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnMoreP).Name = "BtnMoreP";
		((Control)BtnMoreP).Size = new Size(30, 21);
		((PopupItemControl)BtnMoreP).Style = (eDotNetBarStyle)9;
		((Control)BtnMoreP).TabIndex = 10;
		((Control)BtnMoreP).TabStop = false;
		((Control)BtnMoreP).Text = "...";
		((Control)BtnMoreP).Click += BtnMoreP_Click;
		((Control)labelX11).Anchor = (AnchorStyles)9;
		((Control)labelX11).BackColor = Color.Transparent;
		((BaseItemControl)labelX11).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX11).Location = new Point(632, 91);
		((Control)labelX11).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX11).Name = "labelX11";
		((Control)labelX11).Size = new Size(86, 23);
		((Control)labelX11).TabIndex = 12;
		((Control)labelX11).Text = "[نام شرکت]:";
		labelX11.TextAlignment = (StringAlignment)2;
		((Control)TxtcodePerson).Anchor = (AnchorStyles)9;
		TxtcodePerson.Border.Class = "TextBoxBorder";
		TxtcodePerson.Border.CornerType = (eCornerType)1;
		((Control)TxtcodePerson).Location = new Point(499, 61);
		((Control)TxtcodePerson).Margin = new Padding(4, 3, 4, 3);
		((Control)TxtcodePerson).Name = "TxtcodePerson";
		((Control)TxtcodePerson).Size = new Size(130, 20);
		((Control)TxtcodePerson).TabIndex = 9;
		((Control)TxtcodePerson).TextChanged += TxtcodePerson_TextChanged;
		((Control)labelX5).Anchor = (AnchorStyles)9;
		((Control)labelX5).BackColor = Color.Transparent;
		((BaseItemControl)labelX5).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX5).Location = new Point(632, 123);
		((Control)labelX5).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX5).Name = "labelX5";
		((Control)labelX5).Size = new Size(86, 23);
		((Control)labelX5).TabIndex = 15;
		((Control)labelX5).Text = "[شیفت]:";
		labelX5.TextAlignment = (StringAlignment)2;
		((Control)BTNMore).AccessibleRole = (AccessibleRole)43;
		BTNMore.ColorTable = (eButtonColor)6;
		((Control)BTNMore).Location = new Point(9, 124);
		((Control)BTNMore).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNMore).Name = "BTNMore";
		((Control)BTNMore).Size = new Size(30, 21);
		((PopupItemControl)BTNMore).Style = (eDotNetBarStyle)9;
		((Control)BTNMore).TabIndex = 17;
		((Control)BTNMore).TabStop = false;
		((Control)BTNMore).Text = "...";
		((Control)BTNMore).Click += BTNMore_Click;
		((Control)labelX3).BackColor = Color.Transparent;
		((BaseItemControl)labelX3).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX3).Location = new Point(178, 7);
		((Control)labelX3).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX3).Name = "labelX3";
		((Control)labelX3).Size = new Size(99, 23);
		((Control)labelX3).TabIndex = 2;
		((Control)labelX3).Text = "[ ثبت قرارداد]:";
		labelX3.TextAlignment = (StringAlignment)2;
		((Control)labelX2).Anchor = (AnchorStyles)9;
		((Control)labelX2).BackColor = Color.Transparent;
		((BaseItemControl)labelX2).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX2).Location = new Point(632, 155);
		((Control)labelX2).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX2).Name = "labelX2";
		((Control)labelX2).Size = new Size(86, 23);
		((Control)labelX2).TabIndex = 18;
		((Control)labelX2).Text = "پیوست:";
		labelX2.TextAlignment = (StringAlignment)2;
		((Control)labelX1).Anchor = (AnchorStyles)9;
		((Control)labelX1).BackColor = Color.Transparent;
		((BaseItemControl)labelX1).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX1).Location = new Point(632, 188);
		((Control)labelX1).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX1).Name = "labelX1";
		((Control)labelX1).Size = new Size(86, 23);
		((Control)labelX1).TabIndex = 20;
		((Control)labelX1).Text = "[شرح]:";
		labelX1.TextAlignment = (StringAlignment)2;
		((Control)labelX8).BackColor = Color.Transparent;
		((BaseItemControl)labelX8).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX8).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX8).Location = new Point(178, 57);
		((Control)labelX8).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX8).Name = "labelX8";
		((Control)labelX8).Size = new Size(89, 23);
		((Control)labelX8).TabIndex = 6;
		((Control)labelX8).Text = "[پایان قرارداد]:";
		labelX8.TextAlignment = (StringAlignment)2;
		((Control)TxtPeyvast).Anchor = (AnchorStyles)13;
		TxtPeyvast.Border.Class = "TextBoxBorder";
		TxtPeyvast.Border.CornerType = (eCornerType)1;
		((Control)TxtPeyvast).Location = new Point(9, 156);
		((Control)TxtPeyvast).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtPeyvast).MaxLength = 100;
		((Control)TxtPeyvast).Name = "TxtPeyvast";
		((Control)TxtPeyvast).Size = new Size(620, 20);
		((Control)TxtPeyvast).TabIndex = 19;
		((Control)labelX10).Anchor = (AnchorStyles)9;
		((Control)labelX10).BackColor = Color.Transparent;
		((BaseItemControl)labelX10).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX10).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)labelX10).Location = new Point(178, 32);
		((Control)labelX10).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX10).Name = "labelX10";
		((Control)labelX10).Size = new Size(86, 23);
		((Control)labelX10).TabIndex = 4;
		((Control)labelX10).Text = "[شروع قرارداد]:";
		labelX10.TextAlignment = (StringAlignment)2;
		((Control)TxtDesc).Anchor = (AnchorStyles)15;
		TxtDesc.Border.Class = "TextBoxBorder";
		TxtDesc.Border.CornerType = (eCornerType)1;
		((Control)TxtDesc).Location = new Point(8, 188);
		((Control)TxtDesc).Margin = new Padding(4, 3, 4, 3);
		((TextBoxBase)TxtDesc).MaxLength = 990000;
		((TextBoxBase)TxtDesc).Multiline = true;
		((Control)TxtDesc).Name = "TxtDesc";
		((TextBox)TxtDesc).ScrollBars = (ScrollBars)3;
		((Control)TxtDesc).Size = new Size(621, 232);
		((Control)TxtDesc).TabIndex = 21;
		((Control)CmbShift).Anchor = (AnchorStyles)13;
		((ListControl)CmbShift).DisplayMember = "Name";
		((ComboBox)CmbShift).DrawMode = (DrawMode)1;
		((ComboBox)CmbShift).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbShift).FormattingEnabled = true;
		((ComboBox)CmbShift).ItemHeight = 15;
		((Control)CmbShift).Location = new Point(40, 124);
		((Control)CmbShift).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbShift).Name = "CmbShift";
		((Control)CmbShift).Size = new Size(588, 21);
		CmbShift.Style = (eDotNetBarStyle)9;
		((Control)CmbShift).TabIndex = 16;
		((ListControl)CmbShift).ValueMember = "Id";
		((Control)labelX7).Anchor = (AnchorStyles)9;
		((Control)labelX7).BackColor = Color.Transparent;
		((BaseItemControl)labelX7).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX7).Location = new Point(632, 57);
		((Control)labelX7).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX7).Name = "labelX7";
		((Control)labelX7).Size = new Size(86, 23);
		((Control)labelX7).TabIndex = 8;
		((Control)labelX7).Text = "[کد شخص]:";
		labelX7.TextAlignment = (StringAlignment)2;
		((Control)CmbCompany).Anchor = (AnchorStyles)13;
		((ListControl)CmbCompany).DisplayMember = "Name";
		((ComboBox)CmbCompany).DrawMode = (DrawMode)1;
		((ComboBox)CmbCompany).DropDownStyle = (ComboBoxStyle)2;
		((ListControl)CmbCompany).FormattingEnabled = true;
		((ComboBox)CmbCompany).ItemHeight = 15;
		((Control)CmbCompany).Location = new Point(40, 92);
		((Control)CmbCompany).Margin = new Padding(4, 3, 4, 3);
		((Control)CmbCompany).Name = "CmbCompany";
		((Control)CmbCompany).Size = new Size(588, 21);
		CmbCompany.Style = (eDotNetBarStyle)9;
		((Control)CmbCompany).TabIndex = 13;
		((ListControl)CmbCompany).ValueMember = "Id";
		TXTDateRegister.AllowNull = false;
		TXTDateRegister.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTDateRegister).Border.CornerType = (eCornerType)1;
		TXTDateRegister.EnterToTab = true;
		TXTDateRegister.EscToClose = true;
		TXTDateRegister.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTDateRegister.FocusBackColor = Color.Yellow;
		TXTDateRegister.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTDateRegister.FocusForeColor = Color.Blue;
		TXTDateRegister.FocusTextSelect = true;
		((Control)TXTDateRegister).Font = new Font("Times New Roman", 12f);
		((Control)TXTDateRegister).Location = new Point(9, 9);
		((Control)TXTDateRegister).Margin = new Padding(4, 3, 4, 3);
		TXTDateRegister.MessageEmptyShowDialog = false;
		TXTDateRegister.MessegeEmpty = "";
		TXTDateRegister.MessegeEmptyInFormRight = true;
		TXTDateRegister.MessegeEmptyShowInForm = false;
		TXTDateRegister.MiladiDate = new DateTime(2025, 6, 26, 19, 54, 0, 560);
		((Control)TXTDateRegister).Name = "TXTDateRegister";
		((Control)TXTDateRegister).RightToLeft = (RightToLeft)0;
		TXTDateRegister.ShowNameOfDayOnToolTip = false;
		TXTDateRegister.ShowToolTipText = false;
		((Control)TXTDateRegister).Size = new Size(161, 20);
		((Control)TXTDateRegister).TabIndex = 3;
		((Control)TXTDateRegister).Text = "26/06/2025";
		((TextBox)TXTDateRegister).TextAlign = (HorizontalAlignment)2;
		TXTDateRegister.TootTipDelay = 500;
		TxtEndDate.AllowNull = false;
		TxtEndDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TxtEndDate).Border.CornerType = (eCornerType)1;
		TxtEndDate.EnterToTab = true;
		TxtEndDate.EscToClose = true;
		TxtEndDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TxtEndDate.FocusBackColor = Color.Yellow;
		TxtEndDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TxtEndDate.FocusForeColor = Color.Blue;
		TxtEndDate.FocusTextSelect = true;
		((Control)TxtEndDate).Font = new Font("Times New Roman", 12f);
		((Control)TxtEndDate).Location = new Point(9, 61);
		((Control)TxtEndDate).Margin = new Padding(4, 3, 4, 3);
		TxtEndDate.MessageEmptyShowDialog = false;
		TxtEndDate.MessegeEmpty = "";
		TxtEndDate.MessegeEmptyInFormRight = true;
		TxtEndDate.MessegeEmptyShowInForm = false;
		TxtEndDate.MiladiDate = new DateTime(2025, 6, 26, 19, 54, 0, 576);
		((Control)TxtEndDate).Name = "TxtEndDate";
		((Control)TxtEndDate).RightToLeft = (RightToLeft)0;
		TxtEndDate.ShowNameOfDayOnToolTip = false;
		TxtEndDate.ShowToolTipText = false;
		((Control)TxtEndDate).Size = new Size(161, 20);
		((Control)TxtEndDate).TabIndex = 7;
		((Control)TxtEndDate).Text = "26/06/2025";
		((TextBox)TxtEndDate).TextAlign = (HorizontalAlignment)2;
		TxtEndDate.TootTipDelay = 500;
		TXTStartDate.AllowNull = false;
		((Control)TXTStartDate).Anchor = (AnchorStyles)13;
		TXTStartDate.BackcolorTooltip = Color.Red;
		((TextBoxX)TXTStartDate).Border.CornerType = (eCornerType)1;
		TXTStartDate.EnterToTab = true;
		TXTStartDate.EscToClose = true;
		TXTStartDate.FarsyDateStyle = (ENFarsyDateStyle)1;
		TXTStartDate.FocusBackColor = Color.Yellow;
		TXTStartDate.FocusFont = new Font("Times New Roman", 12f, (FontStyle)1);
		TXTStartDate.FocusForeColor = Color.Blue;
		TXTStartDate.FocusTextSelect = true;
		((Control)TXTStartDate).Font = new Font("Times New Roman", 12f);
		((Control)TXTStartDate).Location = new Point(9, 35);
		((Control)TXTStartDate).Margin = new Padding(4, 3, 4, 3);
		TXTStartDate.MessageEmptyShowDialog = false;
		TXTStartDate.MessegeEmpty = "";
		TXTStartDate.MessegeEmptyInFormRight = true;
		TXTStartDate.MessegeEmptyShowInForm = false;
		TXTStartDate.MiladiDate = new DateTime(2025, 6, 26, 19, 54, 0, 578);
		((Control)TXTStartDate).Name = "TXTStartDate";
		((Control)TXTStartDate).RightToLeft = (RightToLeft)0;
		TXTStartDate.ShowNameOfDayOnToolTip = false;
		TXTStartDate.ShowToolTipText = false;
		((Control)TXTStartDate).Size = new Size(161, 20);
		((Control)TXTStartDate).TabIndex = 5;
		((Control)TXTStartDate).Text = "26/06/2025";
		((TextBox)TXTStartDate).TextAlign = (HorizontalAlignment)2;
		TXTStartDate.TootTipDelay = 500;
		tabItem1.AttachedControl = (Control)(object)tabControlPanel1;
		tabItem1.Name = "tabItem1";
		tabItem1.Text = "[اطلاعات عمومی]";
		((Control)tabControlPanel4).Anchor = (AnchorStyles)15;
		((Control)tabControlPanel4).Controls.Add((Control)(object)DGrid);
		((Control)tabControlPanel4).Controls.Add((Control)(object)LblDescFormula);
		((Control)tabControlPanel4).Controls.Add((Control)(object)LblMandeh);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX13);
		((Control)tabControlPanel4).Controls.Add((Control)(object)LblSumkosor);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX15);
		((Control)tabControlPanel4).Controls.Add((Control)(object)labelX14);
		((Control)tabControlPanel4).Controls.Add((Control)(object)LblSumMazaya);
		((Control)tabControlPanel4).Controls.Add((Control)(object)BTNEdit);
		((Control)tabControlPanel4).Controls.Add((Control)(object)BTNADDMazaya);
		((Control)tabControlPanel4).Controls.Add((Control)(object)btnKosoratFormula);
		((Control)tabControlPanel4).Controls.Add((Control)(object)BTNAddKosorat);
		((Control)tabControlPanel4).Controls.Add((Control)(object)BTNDelete);
		((PanelEx)tabControlPanel4).DisabledBackColor = Color.Empty;
		((Control)tabControlPanel4).Location = new Point(0, 26);
		((Control)tabControlPanel4).Margin = new Padding(4, 3, 4, 3);
		((Control)tabControlPanel4).Name = "tabControlPanel4";
		((Control)tabControlPanel4).Padding = new Padding(1);
		((Control)tabControlPanel4).Size = new Size(728, 424);
		((PanelEx)tabControlPanel4).Style.BackColor1.Color = Color.FromArgb(142, 179, 231);
		((PanelEx)tabControlPanel4).Style.BackColor2.Color = Color.FromArgb(223, 237, 254);
		((PanelEx)tabControlPanel4).Style.Border = (eBorderType)1;
		((PanelEx)tabControlPanel4).Style.BorderColor.Color = Color.FromArgb(59, 97, 156);
		((PanelEx)tabControlPanel4).Style.BorderSide = (eBorderSide)11;
		((PanelEx)tabControlPanel4).Style.GradientAngle = 90;
		((Control)tabControlPanel4).TabIndex = 1;
		tabControlPanel4.TabItem = tabItem4;
		DGrid.AllowUserToAddRows = false;
		DGrid.AllowUserToDeleteRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		DGrid.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		DGrid.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)DGGharardadID,
			(DataGridViewColumn)DGHesabID,
			(DataGridViewColumn)DGName,
			(DataGridViewColumn)DGCredit,
			(DataGridViewColumn)DGDebit
		});
		((Control)DGrid).Location = new Point(163, 4);
		((Control)DGrid).Margin = new Padding(4, 3, 4, 3);
		((Control)DGrid).Name = "DGrid";
		DGrid.ReadOnly = true;
		DGrid.RowHeadersVisible = false;
		DGrid.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(553, 370);
		((Control)DGrid).TabIndex = 0;
		((Control)DGrid).MouseDoubleClick += new MouseEventHandler(DGrid_MouseDoubleClick);
		((DataGridViewColumn)DGGharardadID).HeaderText = "GHarardadID";
		((DataGridViewColumn)DGGharardadID).Name = "DGGharardadID";
		((DataGridViewBand)DGGharardadID).ReadOnly = true;
		((DataGridViewBand)DGGharardadID).Visible = false;
		((DataGridViewColumn)DGHesabID).HeaderText = "HesabID";
		((DataGridViewColumn)DGHesabID).Name = "DGHesabID";
		((DataGridViewBand)DGHesabID).ReadOnly = true;
		((DataGridViewBand)DGHesabID).Visible = false;
		((DataGridViewColumn)DGName).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)DGName).HeaderText = "[نام]";
		((DataGridViewColumn)DGName).MinimumWidth = 100;
		((DataGridViewColumn)DGName).Name = "DGName";
		((DataGridViewBand)DGName).ReadOnly = true;
		((DataGridViewColumn)DGCredit).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGCredit).HeaderText = "[مزایا]";
		((DataGridViewColumn)DGCredit).MinimumWidth = 100;
		((DataGridViewColumn)DGCredit).Name = "DGCredit";
		((DataGridViewBand)DGCredit).ReadOnly = true;
		((DataGridViewColumn)DGDebit).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)DGDebit).HeaderText = "[کسورات]";
		((DataGridViewColumn)DGDebit).MinimumWidth = 40;
		((DataGridViewColumn)DGDebit).Name = "DGDebit";
		((DataGridViewBand)DGDebit).ReadOnly = true;
		((DataGridViewColumn)DGDebit).Width = 86;
		((Control)LblDescFormula).BackColor = Color.Transparent;
		((BaseItemControl)LblDescFormula).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblDescFormula).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)LblDescFormula).ForeColor = Color.Crimson;
		((Control)LblDescFormula).Location = new Point(5, 378);
		((Control)LblDescFormula).Margin = new Padding(4, 3, 4, 3);
		((Control)LblDescFormula).Name = "LblDescFormula";
		((Control)LblDescFormula).Size = new Size(712, 41);
		((Control)LblDescFormula).TabIndex = 9;
		((Control)LblDescFormula).Text = "بدلیل وجود فرمول در مزایا و کسورات مبالغ آنها در هنگام صدور فیش حقوقی تعیین می شود بنابراین در اینجا مبلغ آن 1- در نظر گرفته شده و در جمع مزایا و کسورات محاسبه نشده است";
		LblDescFormula.WordWrap = true;
		((Control)LblMandeh).BackColor = Color.Transparent;
		((BaseItemControl)LblMandeh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblMandeh).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblMandeh).ForeColor = Color.Black;
		((Control)LblMandeh).Location = new Point(9, 337);
		((Control)LblMandeh).Margin = new Padding(4, 3, 4, 3);
		((Control)LblMandeh).Name = "LblMandeh";
		((Control)LblMandeh).Size = new Size(148, 23);
		((Control)LblMandeh).TabIndex = 10;
		((Control)LblMandeh).Text = "0";
		LblMandeh.TextAlignment = (StringAlignment)1;
		((Control)labelX13).BackColor = Color.Transparent;
		((BaseItemControl)labelX13).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX13).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)labelX13).ForeColor = Color.Black;
		((Control)labelX13).Location = new Point(9, 310);
		((Control)labelX13).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX13).Name = "labelX13";
		((Control)labelX13).Size = new Size(148, 23);
		((Control)labelX13).TabIndex = 0;
		((Control)labelX13).Text = "[تفاضل مزایا و کسورات]";
		labelX13.TextAlignment = (StringAlignment)1;
		((Control)LblSumkosor).BackColor = Color.Transparent;
		((BaseItemControl)LblSumkosor).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumkosor).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblSumkosor).ForeColor = Color.Red;
		((Control)LblSumkosor).Location = new Point(9, 281);
		((Control)LblSumkosor).Margin = new Padding(4, 3, 4, 3);
		((Control)LblSumkosor).Name = "LblSumkosor";
		((Control)LblSumkosor).Size = new Size(148, 23);
		((Control)LblSumkosor).TabIndex = 8;
		((Control)LblSumkosor).Text = "0";
		LblSumkosor.TextAlignment = (StringAlignment)1;
		((Control)labelX15).BackColor = Color.Transparent;
		((BaseItemControl)labelX15).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX15).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)labelX15).ForeColor = Color.Green;
		((Control)labelX15).Location = new Point(9, 201);
		((Control)labelX15).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX15).Name = "labelX15";
		((Control)labelX15).Size = new Size(148, 23);
		((Control)labelX15).TabIndex = 5;
		((Control)labelX15).Text = "[جمع کل مزایا] ";
		labelX15.TextAlignment = (StringAlignment)1;
		((Control)labelX14).BackColor = Color.Transparent;
		((BaseItemControl)labelX14).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX14).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Control)labelX14).ForeColor = Color.Crimson;
		((Control)labelX14).Location = new Point(9, 259);
		((Control)labelX14).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX14).Name = "labelX14";
		((Control)labelX14).Size = new Size(148, 23);
		((Control)labelX14).TabIndex = 7;
		((Control)labelX14).Text = "[جمع کل کسورات] ";
		labelX14.TextAlignment = (StringAlignment)1;
		((Control)LblSumMazaya).BackColor = Color.Transparent;
		((BaseItemControl)LblSumMazaya).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumMazaya).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)LblSumMazaya).ForeColor = Color.Green;
		((Control)LblSumMazaya).Location = new Point(9, 230);
		((Control)LblSumMazaya).Margin = new Padding(4, 3, 4, 3);
		((Control)LblSumMazaya).Name = "LblSumMazaya";
		((Control)LblSumMazaya).Size = new Size(148, 23);
		((Control)LblSumMazaya).TabIndex = 6;
		((Control)LblSumMazaya).Text = "0";
		LblSumMazaya.TextAlignment = (StringAlignment)1;
		((Control)BTNEdit).AccessibleRole = (AccessibleRole)43;
		BTNEdit.ColorTable = (eButtonColor)6;
		((Control)BTNEdit).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNEdit.Image = (Image)(object)Resources.edit2;
		BTNEdit.ImagePosition = (eImagePosition)1;
		((Control)BTNEdit).Location = new Point(9, 121);
		((Control)BTNEdit).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNEdit).Name = "BTNEdit";
		((Control)BTNEdit).RightToLeft = (RightToLeft)1;
		((Control)BTNEdit).Size = new Size(148, 34);
		((PopupItemControl)BTNEdit).Style = (eDotNetBarStyle)9;
		((Control)BTNEdit).TabIndex = 3;
		((Control)BTNEdit).TabStop = false;
		((Control)BTNEdit).Text = " [ویرایش مبلغ](F7)";
		BTNEdit.TextAlignment = (eButtonTextAlignment)0;
		((Control)BTNEdit).Click += BTNEdit_Click;
		((Control)BTNADDMazaya).AccessibleRole = (AccessibleRole)43;
		BTNADDMazaya.ColorTable = (eButtonColor)6;
		((Control)BTNADDMazaya).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNADDMazaya.Image = (Image)(object)Resources.edit_add1;
		BTNADDMazaya.ImagePosition = (eImagePosition)1;
		BTNADDMazaya.ImageTextSpacing = 2;
		((Control)BTNADDMazaya).Location = new Point(9, 4);
		((Control)BTNADDMazaya).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNADDMazaya).Name = "BTNADDMazaya";
		((Control)BTNADDMazaya).RightToLeft = (RightToLeft)1;
		((Control)BTNADDMazaya).Size = new Size(148, 34);
		((PopupItemControl)BTNADDMazaya).Style = (eDotNetBarStyle)9;
		((Control)BTNADDMazaya).TabIndex = 1;
		((Control)BTNADDMazaya).TabStop = false;
		((Control)BTNADDMazaya).Text = " [افزودن مزایا] (Insert) ";
		BTNADDMazaya.TextAlignment = (eButtonTextAlignment)0;
		((Control)BTNADDMazaya).Click += BTNADDMazaya_Click;
		((Control)btnKosoratFormula).AccessibleRole = (AccessibleRole)43;
		btnKosoratFormula.ColorTable = (eButtonColor)6;
		((Control)btnKosoratFormula).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		btnKosoratFormula.Image = (Image)(object)Resources.edit_add1;
		btnKosoratFormula.ImagePosition = (eImagePosition)1;
		btnKosoratFormula.ImageTextSpacing = 1;
		((Control)btnKosoratFormula).Location = new Point(9, 82);
		((Control)btnKosoratFormula).Margin = new Padding(10);
		((Control)btnKosoratFormula).Name = "btnKosoratFormula";
		((Control)btnKosoratFormula).Padding = new Padding(10);
		((Control)btnKosoratFormula).RightToLeft = (RightToLeft)1;
		((Control)btnKosoratFormula).Size = new Size(148, 34);
		((PopupItemControl)btnKosoratFormula).Style = (eDotNetBarStyle)9;
		((Control)btnKosoratFormula).TabIndex = 2;
		((Control)btnKosoratFormula).TabStop = false;
		((Control)btnKosoratFormula).Text = " [مزایا و کسورات  فرمولی]";
		btnKosoratFormula.TextAlignment = (eButtonTextAlignment)0;
		((Control)btnKosoratFormula).Click += BtnMazayaFormula_Click;
		((Control)BTNAddKosorat).AccessibleRole = (AccessibleRole)43;
		BTNAddKosorat.ColorTable = (eButtonColor)6;
		((Control)BTNAddKosorat).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNAddKosorat.Image = (Image)(object)Resources.edit_add1;
		BTNAddKosorat.ImagePosition = (eImagePosition)1;
		BTNAddKosorat.ImageTextSpacing = 2;
		((Control)BTNAddKosorat).Location = new Point(9, 43);
		((Control)BTNAddKosorat).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNAddKosorat).Name = "BTNAddKosorat";
		((Control)BTNAddKosorat).RightToLeft = (RightToLeft)1;
		((Control)BTNAddKosorat).Size = new Size(148, 34);
		((PopupItemControl)BTNAddKosorat).Style = (eDotNetBarStyle)9;
		((Control)BTNAddKosorat).TabIndex = 2;
		((Control)BTNAddKosorat).TabStop = false;
		((Control)BTNAddKosorat).Text = " [افزودن کسورات](F6)";
		BTNAddKosorat.TextAlignment = (eButtonTextAlignment)0;
		((Control)BTNAddKosorat).Click += BTNAddKosorat_Click;
		((Control)BTNDelete).AccessibleRole = (AccessibleRole)43;
		BTNDelete.ColorTable = (eButtonColor)6;
		((Control)BTNDelete).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BTNDelete.Image = (Image)(object)Resources.edit_delete;
		BTNDelete.ImagePosition = (eImagePosition)1;
		BTNDelete.ImageTextSpacing = 2;
		((Control)BTNDelete).Location = new Point(9, 160);
		((Control)BTNDelete).Margin = new Padding(4, 3, 4, 3);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).RightToLeft = (RightToLeft)1;
		((Control)BTNDelete).Size = new Size(148, 34);
		((PopupItemControl)BTNDelete).Style = (eDotNetBarStyle)9;
		((Control)BTNDelete).TabIndex = 0;
		((Control)BTNDelete).TabStop = false;
		((Control)BTNDelete).Text = " [حذف سطر](Del)";
		BTNDelete.TextAlignment = (eButtonTextAlignment)0;
		((Control)BTNDelete).Click += BTNDelete_Click;
		tabItem4.AttachedControl = (Control)(object)tabControlPanel4;
		tabItem4.Name = "tabItem4";
		tabItem4.Text = "[مزایا و کسورات]";
		((Control)BtnCancel).AccessibleRole = (AccessibleRole)43;
		((Control)BtnCancel).Anchor = (AnchorStyles)6;
		BtnCancel.ColorTable = (eButtonColor)6;
		BtnCancel.DialogResult = (DialogResult)2;
		((Control)BtnCancel).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnCancel.Image = (Image)(object)Resources.stop;
		BtnCancel.ImagePosition = (eImagePosition)1;
		((Control)BtnCancel).Location = new Point(12, 513);
		((Control)BtnCancel).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnCancel).Name = "BtnCancel";
		((Control)BtnCancel).RightToLeft = (RightToLeft)1;
		((Control)BtnCancel).Size = new Size(134, 34);
		((PopupItemControl)BtnCancel).Style = (eDotNetBarStyle)9;
		((Control)BtnCancel).TabIndex = 33;
		((Control)BtnCancel).TabStop = false;
		((Control)BtnCancel).Text = "[لغو] (Esc)";
		((Control)BtnAccept).AccessibleRole = (AccessibleRole)43;
		((Control)BtnAccept).Anchor = (AnchorStyles)6;
		BtnAccept.ColorTable = (eButtonColor)6;
		((Control)BtnAccept).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		BtnAccept.Image = (Image)(object)Resources.accept1;
		BtnAccept.ImagePosition = (eImagePosition)1;
		((Control)BtnAccept).Location = new Point(154, 513);
		((Control)BtnAccept).Margin = new Padding(4, 3, 4, 3);
		((Control)BtnAccept).Name = "BtnAccept";
		((Control)BtnAccept).RightToLeft = (RightToLeft)1;
		BtnAccept.Shortcuts.Add((eShortcut)116);
		((Control)BtnAccept).Size = new Size(134, 34);
		((PopupItemControl)BtnAccept).Style = (eDotNetBarStyle)9;
		((Control)BtnAccept).TabIndex = 32;
		((Control)BtnAccept).Text = "[ثبت] (F5)";
		((Control)BtnAccept).Click += BtnAccept_click;
		((Control)LblSumMazayaKosorat).Anchor = (AnchorStyles)10;
		((Control)LblSumMazayaKosorat).BackColor = Color.Transparent;
		((BaseItemControl)LblSumMazayaKosorat).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumMazayaKosorat).ForeColor = Color.DarkRed;
		((Control)LblSumMazayaKosorat).Location = new Point(394, 501);
		((Control)LblSumMazayaKosorat).Margin = new Padding(4, 3, 4, 3);
		((Control)LblSumMazayaKosorat).Name = "LblSumMazayaKosorat";
		((Control)LblSumMazayaKosorat).RightToLeft = (RightToLeft)1;
		((Control)LblSumMazayaKosorat).Size = new Size(206, 20);
		((Control)LblSumMazayaKosorat).TabIndex = 0;
		((Control)LblSumMazayaKosorat).Text = "----------";
		((Control)LblSumHogh).Anchor = (AnchorStyles)10;
		((Control)LblSumHogh).BackColor = Color.Transparent;
		((BaseItemControl)LblSumHogh).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumHogh).ForeColor = Color.DarkRed;
		((Control)LblSumHogh).Location = new Point(394, 475);
		((Control)LblSumHogh).Margin = new Padding(4, 3, 4, 3);
		((Control)LblSumHogh).Name = "LblSumHogh";
		((Control)LblSumHogh).RightToLeft = (RightToLeft)1;
		((Control)LblSumHogh).Size = new Size(206, 20);
		((Control)LblSumHogh).TabIndex = 0;
		((Control)LblSumHogh).Text = "----------";
		((Control)LblSumHoghMazaya).Anchor = (AnchorStyles)10;
		((Control)LblSumHoghMazaya).BackColor = Color.Transparent;
		((BaseItemControl)LblSumHoghMazaya).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)LblSumHoghMazaya).ForeColor = Color.DarkRed;
		((Control)LblSumHoghMazaya).Location = new Point(394, 527);
		((Control)LblSumHoghMazaya).Margin = new Padding(4, 3, 4, 3);
		((Control)LblSumHoghMazaya).Name = "LblSumHoghMazaya";
		((Control)LblSumHoghMazaya).RightToLeft = (RightToLeft)1;
		((Control)LblSumHoghMazaya).Size = new Size(206, 20);
		((Control)LblSumHoghMazaya).TabIndex = 0;
		((Control)LblSumHoghMazaya).Text = "----------";
		((Control)labelX42).Anchor = (AnchorStyles)10;
		((Control)labelX42).BackColor = Color.Transparent;
		((BaseItemControl)labelX42).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX42).Location = new Point(608, 501);
		((Control)labelX42).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX42).Name = "labelX42";
		((Control)labelX42).Size = new Size(132, 20);
		((Control)labelX42).TabIndex = 0;
		((Control)labelX42).Text = "[جمع مزایا و کسورات]: ";
		labelX42.TextAlignment = (StringAlignment)2;
		((Control)labelX37).Anchor = (AnchorStyles)10;
		((Control)labelX37).BackColor = Color.Transparent;
		((BaseItemControl)labelX37).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX37).Location = new Point(608, 527);
		((Control)labelX37).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX37).Name = "labelX37";
		((Control)labelX37).Size = new Size(132, 20);
		((Control)labelX37).TabIndex = 0;
		((Control)labelX37).Text = "[جمع کل حقوق و مزایا]: ";
		labelX37.TextAlignment = (StringAlignment)2;
		((Control)labelX29).Anchor = (AnchorStyles)10;
		((Control)labelX29).BackColor = Color.Transparent;
		((BaseItemControl)labelX29).BackgroundStyle.CornerType = (eCornerType)1;
		((Control)labelX29).Location = new Point(608, 475);
		((Control)labelX29).Margin = new Padding(4, 3, 4, 3);
		((Control)labelX29).Name = "labelX29";
		((Control)labelX29).Size = new Size(132, 20);
		((Control)labelX29).TabIndex = 0;
		((Control)labelX29).Text = "[جمع حقوق]: ";
		labelX29.TextAlignment = (StringAlignment)2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)BtnCancel;
		((Form)this).ClientSize = new Size(752, 577);
		((Control)this).Controls.Add((Control)(object)panelEx1);
		((Control)this).Font = new Font("Tahoma", 8f, (FontStyle)1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Margin = new Padding(4, 3, 4, 3);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(639, 562);
		((Control)this).Name = "FrmGharardad";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[قرارداد]*";
		((Form)this).FormClosing += new FormClosingEventHandler(FrmGharardad_FormClosing);
		((Form)this).Load += FrmGharardad_Load;
		((Control)panelEx1).ResumeLayout(false);
		((ISupportInitialize)tabControl1).EndInit();
		tabControl1.ResumeLayout(false);
		((Control)tabControlPanel3).ResumeLayout(false);
		((Control)groupPanelTaradod).ResumeLayout(false);
		((Control)tabControlPanel2).ResumeLayout(false);
		((Control)tabControlPanel2).PerformLayout();
		((ISupportInitialize)TxtLeavHoures).EndInit();
		((ISupportInitialize)TxtLeavMinutes).EndInit();
		((ISupportInitialize)TxtLeaveSaved).EndInit();
		((ISupportInitialize)NumberCountChild).EndInit();
		((Control)tabControlPanel1).ResumeLayout(false);
		((ISupportInitialize)numberCod).EndInit();
		((Control)tabControlPanel4).ResumeLayout(false);
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(KeyEventArgs e)
	{
		((FormExtention)this).OnKeyDown(e);
	}
}
