using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting.RasGiri.Calculator;

public class RasGiriStandardCalculator : IRasGiriBusinesCalculator, IHasContext
{
	protected List<RasGiriDetBusiness> _lst;

	public string ContextGuid { get; private set; }

	public RasGiriStandardCalculator(string contextGuid, List<RasGiriDetBusiness> lst)
	{
		try
		{
			ContextGuid = contextGuid;
			_lst = lst ?? new List<RasGiriDetBusiness>();
			foreach (RasGiriDetBusiness item in _lst)
			{
				item.EnherafForCalculation = item.Enheraf;
				item.EnherafForCalculationRas = item.EnherafAsRas;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\Calculator\\RasGiriStandardCalculator.cs", 28);
		}
	}

	public int CalCulateRasDaysCalculator()
	{
		try
		{
			decimal num = default(decimal);
			decimal num2 = default(decimal);
			List<RasGiriDetBusiness> lst = _lst;
			if (lst != null && lst.Any())
			{
				foreach (RasGiriDetBusiness item in _lst)
				{
					num += item.Price * item.Enheraf;
					num2 += item.Price;
				}
			}
			if (num2 == 0m)
			{
				return 0;
			}
			return (int)(num / num2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalCulateRasDaysCalculator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\Calculator\\RasGiriStandardCalculator.cs", 51);
			return 0;
		}
	}

	public virtual decimal CalcTotalSoodCalculator(decimal Percent)
	{
		try
		{
			decimal num = default(decimal);
			decimal num2 = default(decimal);
			int num3 = 0;
			List<RasGiriDetBusiness> lst = _lst;
			if (lst != null && lst.Any())
			{
				foreach (RasGiriDetBusiness item in _lst)
				{
					num += item.Price * item.Enheraf;
					num2 += item.Price;
				}
			}
			num3 = ((!(num2 == 0m)) ? ((int)(num / num2)) : 0);
			return (decimal)num3 * (num2 * Percent) / 3000m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcTotalSoodCalculator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\Calculator\\RasGiriStandardCalculator.cs", 79);
			return 0m;
		}
	}

	public virtual decimal CalcTotalSood_AghsatCalculator(decimal Totalcurrency, decimal Percent, decimal Price, int repeat)
	{
		try
		{
			decimal num = default(decimal);
			decimal result = default(decimal);
			Percent /= 100m;
			while (repeat > 0)
			{
				num = Totalcurrency * Percent;
				result += num;
				Totalcurrency -= Price;
				repeat = checked(repeat - 1);
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcTotalSood_AghsatCalculator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\Calculator\\RasGiriStandardCalculator.cs", 100);
			return 0m;
		}
	}
}
