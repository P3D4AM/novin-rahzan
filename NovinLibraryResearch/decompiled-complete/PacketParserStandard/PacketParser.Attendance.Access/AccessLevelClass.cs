using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;

namespace PacketParser.Attendance.Access;

public class AccessLevelClass
{
	private bool _setAccessLevelsIsOk;

	private static AccessLevelClass _instance;

	public ReturnedSaveFuncInfo SaveFuncInfo { get; set; }

	public static ICheckActivation LkActivation { get; set; }

	[PersianNameAttribute.PersianName("سال های کاری")]
	public WorkYear WorkYear { get; set; } = new WorkYear();


	[PersianNameAttribute.PersianName("بخش کارمندان")]
	public Personnel Personnel { get; set; } = new Personnel();


	[PersianNameAttribute.PersianName("کدینگ حسابها")]
	public CodeingHesab CodeingHesab { get; set; } = new CodeingHesab();


	[PersianNameAttribute.PersianName("قرارداد کاری")]
	public Gharardad Gharardad { get; set; } = new Gharardad();


	[PersianNameAttribute.PersianName("پایه سنواتی ")]
	public BaseYearsSalary BaseYearsSalary { get; set; } = new BaseYearsSalary();


	[PersianNameAttribute.PersianName("فیش حقوقی")]
	public BillPrice BillPrice { get; set; } = new BillPrice();


	[PersianNameAttribute.PersianName("اسناد حسابداری")]
	public Accounting Accounting { get; set; } = new Accounting();


	[PersianNameAttribute.PersianName("کاربران")]
	public Users Users { get; set; } = new Users();


	[PersianNameAttribute.PersianName("تردد کارکنان")]
	public Taradod Taradod { get; set; } = new Taradod();


	[PersianNameAttribute.PersianName("صدور انواع مرخصی")]
	public Vacation Vacation { get; set; } = new Vacation();


	[PersianNameAttribute.PersianName("شرکت")]
	public Company Company { get; set; } = new Company();


	[PersianNameAttribute.PersianName("شیفت ها")]
	public Shift Shift { get; set; } = new Shift();


	[PersianNameAttribute.PersianName("دستگاه ها ")]
	public Device Device { get; set; } = new Device();


	[PersianNameAttribute.PersianName("گزارشات")]
	public Reports Reports { get; set; } = new Reports();


	[PersianNameAttribute.PersianName("کسورات توافقی")]
	public AgreedDeduction AgreedDeduction { get; set; } = new AgreedDeduction();


	[PersianNameAttribute.PersianName("جدول مالیاتی")]
	public TaxTable TaxTable { get; set; } = new TaxTable();


	[PersianNameAttribute.PersianName("لیست بیمه")]
	public Insurance Insurance { get; set; } = new Insurance();


	[PersianNameAttribute.PersianName("تنظیمات نرم\u00adافزار")]
	public SoftwareSetting SoftwareSetting { get; set; } = new SoftwareSetting();


	public Level_Hozor LevelHozor { get; set; }

	public static AccessLevelClass Instance => _instance ?? (_instance = new AccessLevelClass(instance_: true));

	public ReturnedSaveFuncInfoWithValue<HozoorPackage> GetTypeSoftware()
	{
		ReturnedSaveFuncInfoWithValue<HozoorPackage> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<HozoorPackage>();
		try
		{
			ReturnedSaveFuncInfoWithValue<HozoorPackage> hozorType = LkActivation.GetHozorType();
			returnedSaveFuncInfoWithValue.AddReturnedValue(hozorType, ReturnedState.None, "GetTypeSoftware", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\AccessLevelClass.cs", 92);
			returnedSaveFuncInfoWithValue.value = hozorType.value;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetTypeSoftware", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\AccessLevelClass.cs", 97);
			WebErrorLog.Instance.StartLog(ex, "", "GetTypeSoftware", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\AccessLevelClass.cs", 98);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public AccessLevelClass(bool instance_)
	{
		_setAccessLevelsIsOk = false;
		SetAccessLevels(instance_);
		while (!_setAccessLevelsIsOk)
		{
			Thread.Sleep(200);
		}
	}

	public AccessLevelClass()
	{
		ApllyAndTwoClass(Instance);
	}

	private async Task SetAccessLevels(bool instance_)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (instance_)
			{
				ReturnedSaveFuncInfoWithValue<Level_Hozor> returnedSaveFuncInfoWithValue = LkActivation.CheckActivation_Hozor();
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SetAccessLevels", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\AccessLevelClass.cs", 126);
				LevelHozor = returnedSaveFuncInfoWithValue.value;
			}
			if (LevelHozor == Level_Hozor.Level1)
			{
				Personnel.PersonnelGroup = false;
				Users.UsersReportLogs = false;
				Taradod.TaradodReciveData = false;
				Taradod.TaradodMain = false;
				Taradod.TaradodDelete = false;
				Gharardad.GharardadEstekhdami = false;
				Gharardad.GharardadCURDMazayaKosorat = false;
			}
			if (LevelHozor <= Level_Hozor.Level2)
			{
				Vacation.VacationMain = false;
				Taradod.TaradodReportTaradodPersonnelOnADay = false;
				Taradod.TaradodReportAmarPersonnel = false;
				Taradod.RptTaradodAllPersonnel = false;
				BillPrice.BillPriceMain = false;
				SoftwareSetting.SoftwareSettingPayamak = false;
				Shift.ShiftPackage = false;
			}
			if (LevelHozor <= Level_Hozor.Level3)
			{
				Vacation.VacationManagement = false;
				Taradod.TaradodUpdate = false;
				Accounting.AccountingMain = false;
				Taradod.TaradodEditManagement = false;
				Shift.ShiftAdvance = false;
				Shift.ShiftShenavar = false;
				Users.UsersAccessLevel = false;
			}
			if (LevelHozor <= Level_Hozor.Level4)
			{
				Taradod.TaradodWorkHoliday = false;
				Taradod.TaradodWorkNight = false;
				Taradod.TaradodWorkShift = false;
				Taradod.TaradodMissionDaily = false;
				Reports.ReportsGozareshSazPoya = false;
				Taradod.TaradodFunctionKeys = false;
				Shift.ShiftCircle = false;
				Shift.ShiftProgrammable = false;
				Gharardad.GharardadHaghs = false;
				AgreedDeduction.AgreedDeductionMain = false;
				AgreedDeduction.AgreedDeductionInsert = false;
				AgreedDeduction.AgreedDeductionUpdate = false;
				AgreedDeduction.AgreedDeductionDelete = false;
				TaxTable.TaxtableMain = false;
				TaxTable.TaxtableDelete = false;
				TaxTable.TaxtableInsert = false;
				TaxTable.TaxtableUpdate = false;
				Insurance.InsuranceMain = false;
				Insurance.PreparedDiskett = false;
				Insurance.InsurancePrint = false;
				BillPrice.changeEditStateOfFish = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAccessLevels", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\AccessLevelClass.cs", 197);
			await ret.AddErrorAsync(EnSystem.None, ex.Message, "", "SetAccessLevels", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\AccessLevelClass.cs", 198);
		}
		SaveFuncInfo = ret;
		_setAccessLevelsIsOk = true;
	}

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static AccessLevelClass FromString(string data)
	{
		return data.FromJson<AccessLevelClass>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\AccessLevelClass.cs", 215);
	}

	public void ApllyAndTwoClass(AccessLevelClass pSrcAccessLevel)
	{
		PropertyInfo[] properties = GetType().GetProperties();
		foreach (PropertyInfo obj in properties)
		{
			object value = obj.GetValue(this, null);
			object value2 = obj.GetValue(pSrcAccessLevel, null);
			value?.GetType().GetProperties();
			if (value == null)
			{
				continue;
			}
			PropertyInfo[] properties2 = value.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties2)
			{
				if (propertyInfo.PropertyType.Name.ToLower() == "boolean")
				{
					bool flag = (bool)propertyInfo.GetValue(value, null);
					bool flag2 = (bool)propertyInfo.GetValue(value2, null);
					propertyInfo.SetValue(value, flag && flag2);
				}
			}
		}
		LevelHozor = pSrcAccessLevel.LevelHozor;
		LevelHozor = LevelHozor;
	}

	public string GetDescription(string xparam)
	{
		return GetDescriptionOfObj(xparam.Split(new char[1] { '.' }).ToList(), Instance);
	}

	private string GetDescriptionOfObj(List<string> pList, object obj)
	{
		PropertyInfo[] properties = obj.GetType().GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (!(propertyInfo.Name == pList[0]))
			{
				continue;
			}
			if (pList.Count > 1)
			{
				return GetDescriptionOfObj(pList.GetRange(1, checked(pList.Count - 1)), propertyInfo.GetValue(obj));
			}
			object[] customAttributes = propertyInfo.GetCustomAttributes(inherit: true);
			PersianNameAttribute.PersianName persianName = new PersianNameAttribute.PersianName(" ");
			object[] array = customAttributes;
			foreach (object obj2 in array)
			{
				if (obj2 is PersianNameAttribute.PersianName)
				{
					persianName = obj2 as PersianNameAttribute.PersianName;
					break;
				}
			}
			return persianName.Text;
		}
		return "";
	}
}
