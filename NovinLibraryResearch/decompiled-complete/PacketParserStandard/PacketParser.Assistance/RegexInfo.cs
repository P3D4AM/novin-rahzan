using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PacketParser.Entities;

namespace PacketParser.Assistance;

public class RegexInfo
{
	public static List<string> Calc(string data, List<string> reg, string content)
	{
		List<string> list = new List<string>();
		int num = 0;
		try
		{
			data = data.ToUpper();
			foreach (string item in reg)
			{
				MatchCollection matchCollection = new Regex(item.ToUpper()).Matches(data);
				Match match = null;
				num = 11;
				foreach (Match item2 in matchCollection)
				{
					num = 12;
					match = item2;
					list.Add(match.Value);
					num = 7;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "Calc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\RegexInfo.cs", 38);
		}
		return list;
	}

	public static List<string> CalcNumber(string data, List<string> reg)
	{
		List<string> list = new List<string>();
		try
		{
			data = data.ToUpper();
			foreach (string item in reg)
			{
				foreach (Match item2 in new Regex(item.ToUpper()).Matches(data))
				{
					list.Add(item2.Groups["DEVICE"].Value);
					list.Add(item2.Groups["LINE"].Value);
					list.Add(item2.Groups["NUMBER"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\RegexInfo.cs", 63);
		}
		return list;
	}

	public static List<string> CalcDialFromServer(string data, List<string> regexDialedFromServer)
	{
		List<string> list = new List<string>();
		try
		{
			data = data.ToUpper();
			foreach (string item in regexDialedFromServer)
			{
				foreach (Match item2 in new Regex(item.ToUpper()).Matches(data))
				{
					list.Add(item2.Groups["DEVICE"].Value);
					list.Add(item2.Groups["LINE"].Value);
					list.Add(item2.Groups["DURATION"].Value);
					list.Add(item2.Groups["NUMBER"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcDialFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\RegexInfo.cs", 89);
		}
		return list;
	}

	public static List<string> CalcOtherEvent(string data, List<string> reg)
	{
		List<string> list = new List<string>();
		try
		{
			data = data.ToUpper();
			foreach (string item in reg)
			{
				MatchCollection matchCollection = new Regex(item.ToUpper()).Matches(data);
				Match match = null;
				foreach (Match item2 in matchCollection)
				{
					list.Add(item2.Groups["DEVICE"].Value);
					list.Add(item2.Groups["LINE"].Value);
					list.Add(item2.Groups["ACTION"].Value);
					list.Add(item2.Groups["MSG"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcOtherEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\RegexInfo.cs", 119);
		}
		return list;
	}

	public static List<string> CalcIncomingCallEvent(string data, List<string> reg)
	{
		List<string> list = new List<string>();
		try
		{
			data = data.ToUpper();
			foreach (string item in reg)
			{
				MatchCollection matchCollection = new Regex(item.ToUpper()).Matches(data);
				Match match = null;
				foreach (Match item2 in matchCollection)
				{
					list.Add(item2.Groups["DATE"].Value);
					list.Add(item2.Groups["LINE"].Value);
					list.Add(item2.Groups["NUMBER"].Value);
					list.Add(item2.Groups["DURATION"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcIncomingCallEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\RegexInfo.cs", 148);
		}
		return list;
	}

	public static List<string> CalcOutgoingCallEvent(string data, List<string> reg)
	{
		List<string> list = new List<string>();
		try
		{
			data = data.ToUpper();
			foreach (string item in reg)
			{
				MatchCollection matchCollection = new Regex(item.ToUpper()).Matches(data);
				Match match = null;
				foreach (Match item2 in matchCollection)
				{
					list.Add(item2.Groups["DATE"].Value);
					list.Add(item2.Groups["LINE"].Value);
					list.Add(item2.Groups["NUMBER"].Value);
					list.Add(item2.Groups["CONUMBER"].Value);
					list.Add(item2.Groups["DURATION"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcOutgoingCallEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\RegexInfo.cs", 179);
		}
		return list;
	}
}
