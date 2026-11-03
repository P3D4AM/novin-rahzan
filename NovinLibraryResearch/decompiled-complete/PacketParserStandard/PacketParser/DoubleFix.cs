using System;
using PacketParser.Entities;

namespace PacketParser;

public class DoubleFix
{
	public static double FixDouble(double value)
	{
		try
		{
			if (double.IsInfinity(value) || double.IsInfinity(value) || double.IsNaN(value) || double.IsNaN(value) || double.IsPositiveInfinity(value) || double.IsPositiveInfinity(value) || double.IsNegativeInfinity(value) || double.IsNegativeInfinity(value))
			{
				return 0.0;
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDouble", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DoubleFix.cs", 25);
			return 0.0;
		}
	}
}
