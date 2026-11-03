using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using PacketParser.Entities;

namespace PacketParser.Assistance.MultiLingual;

internal class Language : ILanguage
{
	private string item;

	public string KeyLang { get; set; }

	public string ValueLang { get; set; }

	public Language(string item)
	{
		try
		{
			string[] array = item.Split(new char[1] { ',' });
			if (array.Length == 2)
			{
				KeyLang = array[0];
				ValueLang = array[1];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\Language.cs", 25);
		}
	}

	public static List<string> ChecKParamters(string Key)
	{
		List<string> list = new List<string>();
		try
		{
			foreach (Match item in new Regex("\\[(?<word>[^\\]]+)\\]", RegexOptions.IgnoreCase | RegexOptions.Compiled).Matches(Key))
			{
				list.Add(item.Groups["word"].Value);
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ChecKParamters", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\MultiLingual\\Language.cs", 45);
		}
		return list;
	}
}
