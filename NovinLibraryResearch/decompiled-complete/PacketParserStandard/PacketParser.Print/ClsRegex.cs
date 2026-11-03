using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PacketParser.Entities;

namespace PacketParser.Print;

public class ClsRegex
{
	private readonly string _Data = "";

	private readonly string _EbarateBaGhaede = "";

	private Match _MatchObject;

	public short Len
	{
		get
		{
			try
			{
				List<char> list = new List<char> { ',' };
				List<string> list2 = SuccessfullMatch.Split(list.ToArray()).ToList();
				if (list2.Count >= 2)
				{
					short.TryParse(list2[1], out var result);
					return result;
				}
				return 0;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Len", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ClsRegex.cs", 81);
				return 0;
			}
		}
	}

	public string Align
	{
		get
		{
			try
			{
				List<char> list = new List<char> { ',' };
				List<string> list2 = SuccessfullMatch.Split(list.ToArray()).ToList();
				if (list2.Count >= 3)
				{
					return list2[2].Substring(0, 1).ToUpper();
				}
				return "R";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Align", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ClsRegex.cs", 107);
				return "R";
			}
		}
	}

	public string PrintModifier
	{
		get
		{
			try
			{
				if (_MatchObject.Groups.Count > 0 && _MatchObject.Groups["printMod"].Length > 0)
				{
					return _MatchObject.Groups["printMod"].Value;
				}
				return "";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PrintModifier", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ClsRegex.cs", 127);
				return "R";
			}
		}
	}

	public string SuccessfullMatch { get; } = "";


	public ClsRegex(string Data, string EbarateBaGhaede)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ClsRegex.cs"))
		{
			try
			{
				int num = 0;
				try
				{
					_Data = Data;
					num = 1;
					_EbarateBaGhaede = EbarateBaGhaede;
					num = 2;
					num = 3;
					Regex regex = new Regex(_EbarateBaGhaede, RegexOptions.IgnoreCase);
					num = 4;
					MatchCollection matchCollection = regex.Matches(_Data);
					num = 5;
					num = 6;
					IEnumerator enumerator = matchCollection.GetEnumerator();
					try
					{
						if (enumerator.MoveNext())
						{
							Match match = (Match)enumerator.Current;
							num = 7;
							Match match2 = match;
							num = 8;
							SuccessfullMatch = match2.Value;
							num = 9;
							_MatchObject = match;
							num = 10;
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, num.ToString(), ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ClsRegex.cs", 48);
				}
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ClsRegex.cs", 54);
			}
		}
	}
}
