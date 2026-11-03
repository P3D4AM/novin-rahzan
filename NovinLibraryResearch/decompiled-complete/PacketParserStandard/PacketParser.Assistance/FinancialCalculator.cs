using System;
using PacketParser.Entities;

namespace PacketParser.Assistance;

public static class FinancialCalculator
{
	public static decimal CalculateProfit(decimal price, decimal percent, bool upToDown, int digitAfterPoint)
	{
		try
		{
			if (!upToDown)
			{
				return Math.Round(price * (percent / 100m), digitAfterPoint);
			}
			if (percent >= 100m)
			{
				return 0m;
			}
			if (1m - percent / 100m <= 0m)
			{
				return 0m;
			}
			return Math.Round(price * (1m / (1m - percent / 100m) - 1m), digitAfterPoint);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateProfit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FinancialCalculator.cs", 23);
			return 0m;
		}
	}

	public static decimal CalculateProfitPercent(decimal sell, decimal kardex, bool upToDown, int digitForRound)
	{
		try
		{
			if (upToDown)
			{
				if (sell <= 0m)
				{
					return 0m;
				}
				return Math.Round(100m * (sell - kardex) / sell, digitForRound);
			}
			if (kardex <= 0m)
			{
				return 0m;
			}
			return Math.Round(100m * (sell - kardex) / kardex, digitForRound);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateProfitPercent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\FinancialCalculator.cs", 42);
			return 0m;
		}
	}
}
