using System;
using PacketParser.Entities;

namespace PacketParser;

public class CurrencyText
{
	public static string FixString(string input)
	{
		return TextManager.Instance.FixString(input);
	}

	public static string RemoveNoNumbers(string Input, string Replcment = "")
	{
		return TextManager.Instance.RemoveNoNumbers(Input, Replcment);
	}

	public static string RemoveNoNumbers_Dot(string Input, string Replcment = "")
	{
		try
		{
			Input = FixString(Input).Trim();
			if (string.IsNullOrEmpty(Input))
			{
				return Input;
			}
			Input = Input.Replace("\r\n", Replcment);
			char[] array = Input.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				if (!"0123456789.".Contains(c.ToString()) && c.ToString() != "" && c.ToString() != Replcment)
				{
					Input = Input.Replace(c.ToString(), Replcment);
				}
			}
			if (Replcment != "")
			{
				while (Input.Contains(Replcment + Replcment))
				{
					Input = Input.Replace(Replcment + Replcment, Replcment);
				}
			}
			return Input;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveNoNumbers_Dot", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\CurrencyText.cs", 34);
			return "";
		}
	}
}
