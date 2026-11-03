using System;
using System.Collections.Generic;
using EntityCache.Business.Global;
using PacketParser.Entities;

namespace SharedAccEntityFramworkPersistence.Ertegha;

public class DefaultWages
{
	private static List<WageBusiness> list = new List<WageBusiness>();

	private static WageBusiness SetDef(string contextGuid, string name, EnWageCalculationMethod calculationMethod, decimal minTransaction, decimal maxTransaction, decimal price, EnWageType type, EnKhazaneMaster master = EnKhazaneMaster.Pardakht)
	{
		try
		{
			return new WageBusiness(contextGuid)
			{
				Name = name,
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				Status = true,
				CalculationMethod = calculationMethod,
				MaxTransaction = maxTransaction,
				MinTransaction = minTransaction,
				PriceOrPercent = price,
				Type = type,
				MasterType = master
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDef", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultWages.cs", 33);
			return null;
		}
	}

	public static List<WageBusiness> ListDefaultData(string contextGuid)
	{
		try
		{
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 0m, 10000000m, 7200m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 10000000m, 20000000m, 10000m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 20000000m, 30000000m, 12800m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 30000000m, 40000000m, 15600m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 40000000m, 50000000m, 18400m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 50000000m, 60000000m, 21200m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 60000000m, 70000000m, 24000m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 70000000m, 80000000m, 26800m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 80000000m, 90000000m, 29600m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 90000000m, 100000000m, 32400m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 100000000m, 110000000m, 35200m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 110000000m, 120000000m, 38000m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 120000000m, 130000000m, 40800m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 130000000m, 140000000m, 43600m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 140000000m, 150000000m, 46400m, EnWageType.Card));
			list.Add(SetDef(contextGuid, "حواله درون بانکی", EnWageCalculationMethod.Price, 0m, 1000000000m, 30m, EnWageType.InternalHavale));
			list.Add(SetDef(contextGuid, "انتقال وجه پایا (شخصی)", EnWageCalculationMethod.Percent, 200m, 2500m, 0.01m, EnWageType.Paya_Personal));
			list.Add(SetDef(contextGuid, "انتقال وجه پایا (گروهی)", EnWageCalculationMethod.Price, 0m, 0m, 1000m, EnWageType.Paya_Group));
			list.Add(SetDef(contextGuid, "انتقال وجه ساتنا", EnWageCalculationMethod.Percent, 0m, 250000m, 0.02m, EnWageType.Satna));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Price, 0m, 6000000m, 1200m, EnWageType.Card, EnKhazaneMaster.Reception));
			list.Add(SetDef(contextGuid, "کارت به کارت", EnWageCalculationMethod.Percent, 6000000m, 0m, 0.0002m, EnWageType.Card, EnKhazaneMaster.Reception));
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListDefaultData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultWages.cs", 67);
			return null;
		}
	}
}
