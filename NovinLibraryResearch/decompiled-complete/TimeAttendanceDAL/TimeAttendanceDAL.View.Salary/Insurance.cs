using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Enums;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.View.Salary;

[Serializable]
[DoNotPruneType]
public class Insurance : IHasContext
{
	public string ContextGuid { get; set; }

	public int FishId { get; set; }

	public long WorkShopCode { get; set; }

	public short Year { get; set; }

	public short Month { get; set; }

	public string ListNo { get; set; }

	public string InsuranceNo { get; set; }

	public string Name { get; set; }

	public string Family { get; set; }

	public string FullName => Name + " " + Family;

	public string FatherName { get; set; }

	public string ShSh { get; set; }

	public string PlaceIssue { get; set; }

	public string IssueDate { get; set; }

	public string BirthDate { get; set; }

	public string Sexuality { get; set; }

	public string Nationality { get; set; }

	public string DescJob { get; set; }

	public string StartWorkDate { get; set; }

	public string EndWorkDate { get; set; }

	public short WorkingDaysNo { get; set; }

	public decimal SalaryOnDay { get; set; }

	public decimal SalaryOnMonth { get; set; }

	public decimal AdvantagesMonthly { get; set; }

	public decimal TotalWagesMonthlyBenefits { get; set; }

	public decimal TotalPayrollMonthlyBenefits { get; set; }

	public decimal PremiumInsured { get; set; }

	public decimal EmployerInsurance { get; set; }

	public decimal UnemploymentInsurance { get; set; }

	public decimal PayTaminGov { get; set; }

	public short PorsantazhRate { get; set; }

	public string WorkingCode { get; set; }

	public string CodeMelie { get; set; }

	public short CompanyId { get; set; }

	public int GharardadId { get; set; }

	public string GetMonthYear()
	{
		return Utilities.GetMonthName((int)Month) + Utilities.GetTranslate(" [ماه سال] ") + Year;
	}

	public Insurance(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<Insurance> GetAsync(string contextGuid, Fish fish, CancellationToken token)
	{
		_ = 3;
		try
		{
			Insurance ret = new Insurance(contextGuid);
			Person clsPerson = fish.ClsPerson;
			ret.ContextGuid = contextGuid;
			ret.FishId = fish.Id;
			ret.CompanyId = fish.ClsGharardad.CompanyId;
			ret.GharardadId = fish.ClsGharardad.ID;
			ret.WorkShopCode = fish.MoazafiMin;
			ret.Year = (short)LanguageAndCalendar.Instance.CaltureCalender.GetYearOfDate(Calendar.MiladiToCalture(fish.DateStartM));
			ret.Month = (short)LanguageAndCalendar.Instance.CaltureCalender.GetMonthOfDate(Calendar.MiladiToCalture(fish.DateStartM));
			ret.ListNo = "01";
			ret.InsuranceNo = ((PersonBase)clsPerson).NoBimeh;
			ret.Name = ((PersonBase)clsPerson).Name;
			ret.Family = ((PersonBase)clsPerson).Family;
			ret.FatherName = ((PersonBase)clsPerson).FatherName;
			ret.ShSh = ((PersonBase)clsPerson).Shsh;
			ret.PlaceIssue = ((PersonBase)clsPerson).SodorSh;
			ret.BirthDate = Calendar.MiladiToCalture(((PersonBase)clsPerson).DateMBirthday);
			ret.Sexuality = ((((PersonBase)clsPerson).Jensiat == 2) ? "زن" : "مرد");
			ret.Nationality = "ایرانی";
			ret.StartWorkDate = Calendar.MiladiToCalture(((PersonBase)clsPerson).DateMStartWork);
			ret.EndWorkDate = Calendar.MiladiToCalture(((PersonBase)clsPerson).DateMFinishW);
			ret.WorkingDaysNo = fish.CountKarKerd;
			ret.SalaryOnDay = fish.KarkerdPrice;
			switch ((EnumsHoghogh.EnumkarkardSt)fish.St)
			{
			case EnumsHoghogh.EnumkarkardSt.Monthly:
				ret.SalaryOnMonth = fish.KarkerdPrice;
				break;
			case EnumsHoghogh.EnumkarkardSt.Daily:
				ret.SalaryOnMonth = ret.SalaryOnDay * (decimal)ret.WorkingDaysNo;
				break;
			case EnumsHoghogh.EnumkarkardSt.Hourly:
				ret.SalaryOnMonth = (decimal)((double)fish.MoazafiMin / 60.0 * (double)fish.KarkerdPrice);
				break;
			}
			decimal num = await fish.GetIncludedInsuranceAsync(token);
			decimal num2 = (num - ret.SalaryOnMonth).RoundPrice();
			ret.AdvantagesMonthly = ((num2 > 0m) ? num2 : 0m);
			ret.TotalWagesMonthlyBenefits = num;
			ret.TotalPayrollMonthlyBenefits = num;
			if (fish.ListFishDet.Any((FishDetJson q) => q.HesabId == 5))
			{
				TimeAttendanceSettings timeAttendanceSettings = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
				ret.PremiumInsured = ret.TotalWagesMonthlyBenefits * (decimal)timeAttendanceSettings.LaborLaw.CoefficientInsuranceInsured;
				ret.EmployerInsurance = ret.TotalWagesMonthlyBenefits * (decimal)timeAttendanceSettings.LaborLaw.CoefficientInsuranceEmployer;
				ret.UnemploymentInsurance = ret.TotalWagesMonthlyBenefits * (decimal)timeAttendanceSettings.LaborLaw.CoefficientInsuranceUnemployment;
			}
			ret.PayTaminGov = ret.PremiumInsured + ret.EmployerInsurance + ret.UnemploymentInsurance;
			ret.PorsantazhRate = 0;
			Insurance insurance = ret;
			insurance.WorkingCode = await fish.GetJobCodeAsync(token);
			ret.CodeMelie = ((PersonBase)clsPerson).CodeMeli;
			insurance = ret;
			insurance.DescJob = await fish.GetJobTitleAsync(token);
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<string>> MakeDisketteDskkarAsync(string contextGuid, List<Insurance> pInsurances, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>
		{
			value = ""
		};
		string xstr = "";
		try
		{
			if (pInsurances == null || pInsurances.Count <= 0)
			{
				ret.AddError((EnSystem)2, "[لیست بیمه خالی است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 148);
			}
			Insurance firstObj = pInsurances[0];
			Company company = await Company.GetAsync(contextGuid, firstObj?.CompanyId ?? 1);
			short year = firstObj.Year;
			short month = firstObj.Month;
			if (year < 1399 && year > LanguageAndCalendar.Instance.CaltureCalender.GetYearOfDate(Calendar.MiladiToCalture(DateTime.Now)))
			{
				ret.AddError((EnSystem)2, $" [سال عملکرد] ({year})  [نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 156);
			}
			if (month <= 0 || month > 12)
			{
				ret.AddError((EnSystem)2, $" [ماه عملکرد] ({month})  [نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 160);
			}
			string code = company.Code;
			if (code.Length <= 0 || code.Length > 10)
			{
				ret.AddError((EnSystem)2, " [کد کارگاه نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 165);
			}
			xstr = code;
			code = Utilities.UntilMaxLen(company.NameWorkShop.WintoDosString(), 100);
			if (code.Length <= 0)
			{
				ret.AddError((EnSystem)2, "[نام کارگاه خالی است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 171);
			}
			xstr = xstr + "," + code;
			code = Utilities.UntilMaxLen(company.NameKarfarma.WintoDosString(), 100);
			if (code.Length <= 0)
			{
				ret.AddError((EnSystem)2, "[نام کارفرما خالی است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 177);
			}
			xstr = xstr + " ," + code;
			code = Utilities.UntilMaxLen(company.AddressWorkShop.WintoDosString(), 100);
			if (code.Length <= 0)
			{
				ret.AddError((EnSystem)2, "[آدرس کارگاه خالی است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 182);
			}
			xstr = xstr + "," + code;
			xstr += ", 0 ";
			xstr = xstr + "," + year.ToString().Substring(2);
			xstr = xstr + "," + month.ToString("00");
			xstr += " ,01 ";
			xstr = xstr + ",  " + Utilities.UntilMaxLen(Utilities.GetTranslate("[شرح لیست بیمه]").WintoDosString(), 100);
			if (pInsurances.Count <= 0 || pInsurances.Count > 99999)
			{
				ret.AddError((EnSystem)2, "[تعداد کارکنان لیست صفر است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 195);
			}
			xstr = xstr + ", " + pInsurances.Count;
			int num = pInsurances.Sum((Insurance q) => q.WorkingDaysNo);
			if (num <= 0 || num > 999999)
			{
				ret.AddError((EnSystem)2, "[مجموع روزهای کارکرد صفر است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 201);
			}
			xstr = xstr + " , " + num;
			decimal num2 = pInsurances.Sum((Insurance q) => q.SalaryOnDay.RoundPrice());
			if (num2 <= 500000m || num2 > 999999999999m)
			{
				ret.AddError((EnSystem)2, "[مجموع دستمزد روزانه نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 207);
			}
			xstr = xstr + " , " + num2;
			num2 = pInsurances.Sum((Insurance q) => q.SalaryOnMonth.RoundPrice());
			if (num2 <= 500000m || num2 > 999999999999m)
			{
				ret.AddError((EnSystem)2, "[مجموع دستمزد ماهانه نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 213);
			}
			xstr = xstr + " , " + num2;
			num2 = pInsurances.Sum((Insurance q) => q.AdvantagesMonthly.RoundPrice());
			if (num2 < 0m || num2 > 999999999999m)
			{
				ret.AddError((EnSystem)2, "[مجموع مزایای ماهانه نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 219);
			}
			xstr = xstr + " , " + num2;
			num2 = pInsurances.Sum((Insurance q) => q.TotalWagesMonthlyBenefits.RoundPrice());
			if (num2 <= 500000m || num2 > 999999999999m)
			{
				ret.AddError((EnSystem)2, "[مجموع دستمزد و مزایای ماهانه مشمول نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 225);
			}
			xstr = xstr + " , " + num2;
			decimal num3 = pInsurances.Sum((Insurance q) => q.TotalPayrollMonthlyBenefits.RoundPrice());
			if (num3 <= 500000m || num3 > 999999999999m)
			{
				ret.AddError((EnSystem)2, "[مجموع کل دستمزد و مزایای ماهانه مشمول و غیر مشمول نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 231);
			}
			xstr = xstr + " , " + num3;
			num2 = num3 * 7m / 100m;
			if (num2 <= 1000m || num2 > 999999999999m)
			{
				ret.AddError((EnSystem)2, "[مجموع حق بیمه بیمه شده ها نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 238);
			}
			xstr = xstr + " , " + num2;
			num2 = num3 * 20m / 100m;
			if (num2 <= 1000m || num2 > 999999999999m)
			{
				ret.AddError((EnSystem)2, "[مجموع حق بیمه سهم کارفرما نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 245);
			}
			xstr = xstr + " , " + num2;
			num2 = num3 * 3m / 100m;
			if (num2 < 0m || num2 > 999999999999m)
			{
				ret.AddError((EnSystem)2, "[مجموع حق بیمه بیکاری نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 252);
			}
			xstr = xstr + " , " + num2;
			LaborLaw laborLaw = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token)).LaborLaw;
			float num4 = laborLaw.CoefficientInsuranceEmployer * 100f + laborLaw.CoefficientInsuranceUnemployment * 100f;
			if (num4 <= 0f)
			{
				ret.AddError((EnSystem)2, "[نرخ بیمه نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 260);
			}
			xstr = xstr + " , " + num4;
			xstr += " , 0";
			xstr += " , 0";
			code = company.NoPeyman;
			if (code.Length == 0 || code.Length > 3)
			{
				ret.AddError((EnSystem)2, "[ردیف پیمان نادرست است]", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 276);
			}
			xstr = xstr + "," + code;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MakeDisketteDskkarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 281);
		}
		ret.value = xstr;
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<string>>> MakeDisketteDskworAsync(string contextGuid, List<Insurance> pInsurances)
	{
		ReturnedSaveFuncInfoWithValue<List<string>> returnedSaveFunc = new ReturnedSaveFuncInfoWithValue<List<string>>
		{
			value = new List<string>()
		};
		List<string> lineList = new List<string>();
		try
		{
			if (pInsurances == null || pInsurances.Count <= 0)
			{
				return null;
			}
			Insurance firstObj = pInsurances[0];
			Company company = await Company.GetAsync(contextGuid, firstObj?.CompanyId ?? 1);
			short year = firstObj.Year;
			short month = firstObj.Month;
			if (year < 1399 && year > LanguageAndCalendar.Instance.CaltureCalender.GetYearOfDate(Calendar.MiladiToCalture(DateTime.Now)))
			{
				returnedSaveFunc.AddError((EnSystem)2, $" [سال عملکرد] ({year})  [نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 305);
			}
			if ((month <= 0 || month > 12) ? true : false)
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowError($" [ماه عملکرد] ({month})  [نادرست است]", contextGuid);
				return null;
			}
			string code = company.Code;
			int length = code.Length;
			if ((length <= 0 || length > 10) ? true : false)
			{
				returnedSaveFunc.AddError((EnSystem)2, " [کد کارگاه نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 315);
			}
			foreach (Insurance pInsurance in pInsurances)
			{
				string text = Utilities.GetTranslate("[در لیست بیمه] ") + pInsurance.FullName + "\r\n";
				string code2 = company.Code;
				code2 = code2 + "," + year.ToString().Substring(2);
				code2 = code2 + "," + month.ToString("00");
				code2 += ",01";
				code = pInsurance.InsuranceNo;
				length = code.Length;
				if ((length <= 0 || length > 10) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد شماره بیمه نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 329);
				}
				code2 = code2 + "," + code;
				code = Utilities.UntilMaxLen(pInsurance.Name.WintoDosString(), 100);
				if (code.Length <= 0)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد نام خالی است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 335);
				}
				code2 = code2 + " ," + code;
				code = Utilities.UntilMaxLen(pInsurance.Family.WintoDosString(), 100);
				if (code.Length <= 0)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد نام خانوادگی خالی است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 341);
				}
				code2 = code2 + "," + code;
				code = Utilities.UntilMaxLen(pInsurance.FatherName.WintoDosString(), 100);
				if (code.Length <= 0)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد نام پدر خالی است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 347);
				}
				code2 = code2 + "," + code;
				code = Utilities.UntilMaxLen(pInsurance.ShSh, 15);
				if (code.Length <= 0)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد شماره شناسنامه خالی است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 353);
				}
				code2 = code2 + "," + code;
				code = Utilities.UntilMaxLen(pInsurance.PlaceIssue.WintoDosString(), 100);
				if (code.Length <= 0)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد محل صدور خالی است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 359);
				}
				code2 = code2 + " ," + code;
				code2 += ", 0 ";
				code = pInsurance.Sexuality.WintoDosString();
				if (code.Length <= 0 || code.Length > 3)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد جنسیت نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 366);
				}
				code2 = code2 + "," + code;
				code = Utilities.UntilMaxLen(pInsurance.Nationality.WintoDosString(), 10);
				if (code.Length <= 0)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد ملیت مشخص نیست]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 372);
				}
				code2 = code2 + " ," + code;
				code = Utilities.UntilMaxLen(pInsurance.DescJob.WintoDosString(), 100);
				if (code.Length <= 0)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد شرح شغل خالی است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 378);
				}
				code2 = code2 + " , " + code;
				code = pInsurance.StartWorkDate.Replace("/", "");
				if (code.Length != 8)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد تاریخ شروع به کار نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 388);
				}
				code2 = code2 + " ," + code;
				if (pInsurance.EndWorkDate == Calendar.MiladiToCalture(ConstantValues.MinDateM))
				{
					code = "";
				}
				else
				{
					code = pInsurance.EndWorkDate.Replace("/", "");
					if (code.Length != 8)
					{
						returnedSaveFunc.AddError((EnSystem)2, text + "[فیلد تاریخ اتمام کار نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 403);
					}
				}
				code2 = code2 + " , " + code;
				code = pInsurance.WorkingDaysNo.ToString();
				length = code.Length;
				if ((length <= 0 || length > 2) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + " [روزهای کارکرد نامشخص است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 410);
				}
				code2 = code2 + ", " + code;
				code = pInsurance.SalaryOnDay.RoundPrice().ToString();
				length = code.Length;
				if ((length < 5 || length > 12) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[دستمزد روزانه نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 416);
				}
				code2 = code2 + "  , " + code;
				code = pInsurance.SalaryOnMonth.RoundPrice().ToString();
				length = code.Length;
				if ((length < 5 || length > 12) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[دستمزد ماهانه نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 422);
				}
				code2 = code2 + "  , " + code;
				code = pInsurance.AdvantagesMonthly.RoundPrice().ToString();
				length = code.Length;
				if ((length < 5 || length > 12) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[مزایای ماهانه نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 428);
				}
				code2 = code2 + "  , " + code;
				code = pInsurance.TotalWagesMonthlyBenefits.RoundPrice().ToString();
				length = code.Length;
				if ((length < 5 || length > 12) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[جمع دستمزد و مزایای ماهانه مشمول نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 434);
				}
				code2 = code2 + "  , " + code;
				code = pInsurance.TotalPayrollMonthlyBenefits.RoundPrice().ToString();
				length = code.Length;
				if ((length < 5 || length > 12) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[جمع کل دستمزد و مزایای ماهانه مشمول و غیر مشمول نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 440);
				}
				code2 = code2 + "  , " + code;
				code = pInsurance.PremiumInsured.RoundPrice().ToString();
				length = code.Length;
				if ((length < 5 || length > 8) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[حق بیمه سهم بیمه شده نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 446);
				}
				code2 = code2 + "  , " + code;
				code = pInsurance.PorsantazhRate.ToString();
				length = code.Length;
				if ((length <= 0 || length > 2) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[نرخ پورسانتاژ نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 452);
				}
				code2 = code2 + "  , " + code;
				code = pInsurance.WorkingCode;
				length = code.Length;
				if ((length <= 0 || length > 6) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[کد شغل نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 458);
				}
				code2 = code2 + " ," + code;
				code = Utilities.GetShortDateSh(pInsurance.BirthDate).Substring(0, 2) + "/" + Utilities.GetShortDateSh(pInsurance.BirthDate).Substring(2, 2) + "/" + Utilities.GetShortDateSh(pInsurance.BirthDate).Substring(4, 2);
				length = code.Length;
				if ((length < 6 || length > 8) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[تاریخ تولد نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 466);
				}
				code2 = code2 + "," + code;
				code = pInsurance.CodeMelie;
				length = code.Length;
				if ((length < 9 || length > 10) ? true : false)
				{
					returnedSaveFunc.AddError((EnSystem)2, text + "[کد ملی نادرست است]", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 472);
				}
				code2 = code2 + " ," + code;
				lineList.Add(code2);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MakeDisketteDskworAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\View\\Salary\\Insurance.cs", 480);
		}
		returnedSaveFunc.value = lineList;
		return returnedSaveFunc;
	}
}
