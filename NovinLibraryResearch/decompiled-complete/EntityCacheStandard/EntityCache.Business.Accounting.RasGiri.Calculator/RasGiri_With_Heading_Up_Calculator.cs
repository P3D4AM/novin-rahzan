using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.RasGiri.Calculator;

public class RasGiri_With_Heading_Up_Calculator : RasGiriStandardCalculator
{
	public RasGiri_With_Heading_Up_Calculator(string contextGuid, List<RasGiriDetBusiness> lst)
		: base(contextGuid, lst)
	{
		decimal enherafForCalculation = lst.Max((RasGiriDetBusiness q) => q.Enheraf);
		foreach (RasGiriDetBusiness item in lst)
		{
			item.EnherafForCalculation = enherafForCalculation;
		}
	}

	public override decimal CalcTotalSood_AghsatCalculator(decimal Totalcurrency, decimal Percent, decimal Price, int repeat)
	{
		try
		{
			return Totalcurrency * (Percent / 100m) * (decimal)repeat;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcTotalSood_AghsatCalculator", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\Calculator\\RasGiri_With_Heading_Up_Calculator.cs", 26);
			return 0m;
		}
	}
}
