using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility.Settings;

namespace TimeAttendanceDAL.Dal.FLRGharardad;

[Serializable]
[DoNotPruneType]
public class FormulaList : IHasContext
{
	private readonly TaradodFull _taradodFull;

	private readonly GharardadNew _gharardad;

	public List<FormulaClass> FormulaClasses = new List<FormulaClass>();

	public string ContextGuid { get; set; }

	public TaradodFull TaradodFull { get; private set; }

	public GharardadNew Gharardad { get; private set; }

	public static async Task<FormulaList> GetAsync(string contextGuid, TaradodFull taradodFull, GharardadNew gharardad, CancellationToken token)
	{
		try
		{
			FormulaList ret = new FormulaList
			{
				ContextGuid = contextGuid,
				TaradodFull = taradodFull,
				Gharardad = gharardad
			};
			await ret.InitListFormulaAsync(token);
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task InitListFormulaAsync(CancellationToken token)
	{
		try
		{
			FormulaClasses.Add(new FormulaClass("CountAll", 0, "کل روزها"));
			FormulaClasses.Add(new FormulaClass("SumKarkerd", 0, "جمع کارکرد (دقیقه)"));
			FormulaClasses.Add(new FormulaClass("SumKasrkar", 0, "جمع کسرکار"));
			FormulaClasses.Add(new FormulaClass("SumEzafekar", 0, "جمع اضافه کار"));
			FormulaClasses.Add(new FormulaClass("CountMorakhasiNotSalary", 0, "روزهای مرخصی بدون حقوق"));
			FormulaClasses.Add(new FormulaClass("CountMorakhasiWithSalary", 0, "روزهای مرخصی با حقوق"));
			FormulaClasses.Add(new FormulaClass("CountMorakhasi", 0, "روزهای مرخصی"));
			FormulaClasses.Add(new FormulaClass("CountGheybat", 0, "روزهای غیبت"));
			FormulaClasses.Add(new FormulaClass("CountTatil", 0, "روزهای تعطیل"));
			FormulaClasses.Add(new FormulaClass("CountNaghes", 0, "کارکرد ناقص"));
			FormulaClasses.Add(new FormulaClass("Countshift", 0, "روزهای شیفت دار"));
			FormulaClasses.Add(new FormulaClass("Sumshift", 0, "جمع شیفت"));
			FormulaClasses.Add(new FormulaClass("CountWorkdayHoghogh", 0, "روزهای کارکرد"));
			FormulaClasses.Add(new FormulaClass("CountWorkdayBime", 0, "CountWorkdayBime"));
			FormulaClasses.Add(new FormulaClass("CountWorkdayMaliyat", 0, "CountWorkdayMaliyat"));
			FormulaClasses.Add(new FormulaClass("SumWorkVacation", 0, "جمع مرخصی"));
			FormulaClasses.Add(new FormulaClass("SumWorkMission", 0, "جمع ماموریت"));
			FormulaClasses.Add(new FormulaClass("SumWorkNightWork", 0, "جمع شب کاری"));
			FormulaClasses.Add(new FormulaClass("SumWorkShiftWork", 0, "جمع نوبت کاری"));
			FormulaClasses.Add(new FormulaClass("SumWorkHalioDayWork", 0, "جمع جمعه کاری"));
			FormulaClasses.Add(new FormulaClass("SumWorkTatilWork", 0, "جمع تعطیل کاری"));
			FormulaClasses.Add(new FormulaClass("CountChild", 0, "تعداد اولاد"));
			FormulaClasses.Add(new FormulaClass("Salary", 0, "میزان حقوق"));
			FormulaClasses.Add(new FormulaClass("PriceOverTime", 0, "نرخ ساعت اضافه کاری"));
			FormulaClasses.Add(new FormulaClass("PriceNight", 0, "نرخ شب کاری"));
			FormulaClasses.Add(new FormulaClass("PriceShift", 0, "نرخ نوبت کاری"));
			FormulaClasses.Add(new FormulaClass("PriceHoliday", 0, "نرخ جمعه کاری"));
			FormulaClasses.Add(new FormulaClass("PriceTatil", 0, "نرخ تعطیل کاری"));
			FormulaClasses.Add(new FormulaClass("PriceVacation", 0, "نرخ مرخصی ساعتی"));
			FormulaClasses.Add(new FormulaClass("PriceAbsent", 0, "نرخ غیبت"));
			FormulaClasses.Add(new FormulaClass("PriceMission", 0, "نرخ ماموریت"));
			FormulaClasses.Add(new FormulaClass("PriceLowTime", 0, "نرخ کسرکار"));
			FormulaClasses.Add(new FormulaClass("DailyWage", 0, "دستمزد روزانه قانون کار"));
			List<FishHesab> obj = await FishHesab.SelectAllAsync(ContextGuid, getAllField: true, token);
			int num = 32;
			foreach (FishHesab item2 in obj)
			{
				FormulaClass formulaClass = new FormulaClass(item2.Name, 0, item2.Name);
				formulaClass.Value = item2.Account;
				FormulaClasses.Add(formulaClass);
				num++;
			}
			if (Gharardad == null)
			{
				return;
			}
			foreach (GharardadDetNew det in Gharardad.ListGharardadDet)
			{
				if (!FormulaClasses.Any((FormulaClass q) => q.Formula == det.Formula && q.Name == det.Name))
				{
					FormulaClass item = new FormulaClass(det.Name, 1, det.Name);
					FormulaClasses.Add(item);
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task<decimal> GetValueAsync(FormulaClass formulaClass, CancellationToken token)
	{
		_ = 1;
		try
		{
			if (formulaClass.Value == -1m)
			{
				bool sw = false;
				if (TaradodFull != null)
				{
					PropertyInfo[] properties = TaradodFull.GetType().GetProperties();
					foreach (PropertyInfo propertyInfo in properties)
					{
						if (propertyInfo.Name == formulaClass.Formula)
						{
							sw = true;
							formulaClass.Value = Utilities.ParseToDecimal(propertyInfo.GetValue(TaradodFull, null).ToString(), 0m, 0);
							break;
						}
					}
				}
				if (!sw)
				{
					PropertyInfo[] properties = Gharardad.GetType().GetProperties();
					foreach (PropertyInfo propertyInfo2 in properties)
					{
						if (propertyInfo2.Name == formulaClass.Formula)
						{
							sw = true;
							formulaClass.Value = Utilities.ParseToDecimal(propertyInfo2.GetValue(Gharardad, null).ToString(), 0m, 0);
							break;
						}
					}
				}
				if (!sw && formulaClass.Formula == "DailyWage")
				{
					sw = true;
					FormulaClass formulaClass2 = formulaClass;
					formulaClass2.Value = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token)).LaborLaw.DailyWage;
				}
				if (!sw)
				{
					FormulaClass formulaClass2 = formulaClass;
					formulaClass2.Value = await CalculateFormulaAsync(formulaClass, token);
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		return formulaClass.Value;
	}

	public async Task<decimal> CalculateFormulaAsync(FormulaClass formulaClass, CancellationToken token)
	{
		_ = 2;
		try
		{
			ExpressionClass xcls = new ExpressionClass(formulaClass.Formula);
			decimal xA = await SuffixExpressionAsync(xcls.InwardExpFirst, token);
			if (xcls.InwardExpSecond != null)
			{
				decimal xB = await SuffixExpressionAsync(xcls.InwardExpSecond, token);
				if (xcls.InwardExpThird != null)
				{
					decimal num = await SuffixExpressionAsync(xcls.InwardExpThird, token);
					xA = ((xA < 0m) ? xB : num);
				}
			}
			return xA;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private async Task<decimal> SuffixExpressionAsync(List<string> pList, CancellationToken token)
	{
		_ = 1;
		try
		{
			decimal result = -1m;
			if (pList.Count <= 0)
			{
				return default(decimal);
			}
			Stack<string> expressionStack = new Stack<string>();
			foreach (string p in pList)
			{
				if (!"+-*/()".Contains(p))
				{
					expressionStack.Push(p);
				}
				else if ("+-*/".Contains(p))
				{
					string op = expressionStack.Pop();
					string op2 = expressionStack.Pop();
					expressionStack.Push((await CalcOperandAsync(op2, op, p, token)).ToString());
				}
			}
			string name = expressionStack.Pop();
			decimal num = await GetValueFromListAsync(name, token);
			if (num >= 0m)
			{
				result = num;
			}
			return result;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private async Task<decimal> CalcOperandAsync(string op1, string op2, string operand, CancellationToken token)
	{
		_ = 1;
		try
		{
			decimal result = -1m;
			decimal xA = await GetValueFromListAsync(op1, token);
			decimal num = await GetValueFromListAsync(op2, token);
			switch (operand)
			{
			case "+":
				result = xA + num;
				break;
			case "-":
				result = xA - num;
				break;
			case "*":
				result = xA * num;
				break;
			case "/":
				if (num > 0m)
				{
					result = xA / num;
				}
				break;
			}
			return result;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private async Task<decimal> GetValueFromListAsync(string name, CancellationToken token)
	{
		try
		{
			decimal result = -1m;
			if (name.StartsWith("["))
			{
				string xstr = name.Replace("[", "");
				xstr = xstr.Replace("]", "");
				FormulaClass formulaClass = FormulaClasses.FirstOrDefault((FormulaClass q) => q.FormulaName.Equals(xstr));
				if (formulaClass != null)
				{
					result = await GetValueAsync(formulaClass, token);
				}
			}
			else
			{
				result = Utilities.ParseToDecimal(name, 0m, 0);
			}
			return result;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}
}
