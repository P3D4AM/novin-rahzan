using System;
using System.Globalization;
using PacketParser.Entities;

namespace PacketParser.Print;

public static class ParameterReplacement
{
	private static string AddPrintModifier(string mod, string value, ClsRegex cls)
	{
		using (new JaegerMonitor("AddPrintModifier", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs"))
		{
			try
			{
				if (mod == "")
				{
					return value;
				}
				decimal.TryParse(value, out var result);
				bool flag = result < 0m;
				result = Math.Abs(result);
				if (cls.PrintModifier == "ABS")
				{
					if (flag)
					{
						value = result.NpToString("N0", 0);
					}
				}
				else if (cls.PrintModifier == "DECSHORT")
				{
					if (!flag && result != 0m)
					{
						value += " بس".GetTranslate();
					}
					else if (flag)
					{
						value = result.NpToString("N0", 0);
						value += " بد".GetTranslate();
					}
					else
					{
						value = "بی حساب".GetTranslate();
					}
				}
				else if (cls.PrintModifier == "DEC")
				{
					if (!flag && result != 0m)
					{
						value += " بستانکار";
					}
					else if (flag)
					{
						value = result.NpToString("N0", 0);
						value += " بدهکار".GetTranslate();
					}
					else
					{
						value = "بی حساب".GetTranslate();
					}
				}
				return value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddPrintModifier", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs", 52);
			}
			return null;
		}
	}

	public static string SetParameter_StringText(string parameterName, string parameterValue, string data)
	{
		checked
		{
			using (new JaegerMonitor("SetParameter_StringText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs"))
			{
				try
				{
					if (string.IsNullOrEmpty(data))
					{
						return "";
					}
					string text = parameterValue;
					data = data.ToUpper();
					if (string.IsNullOrEmpty(parameterValue))
					{
						parameterValue = "";
					}
					ClsRegex clsRegex = new ClsRegex(data, "<" + parameterName.ToUpper() + ",[0-9]+,[RCL][,]*(?<printMod>[a-zA-Z]*)[\\s]*/>");
					if (clsRegex.SuccessfullMatch.Trim() != "")
					{
						string value = parameterValue;
						value = AddPrintModifier(clsRegex.PrintModifier, value, clsRegex);
						value = value.Replace(Text.DecimalPoint, ",");
						if (clsRegex.Len > 0 && value.Length > clsRegex.Len)
						{
							value = value.Replace(" ", "");
							if (value.Length > clsRegex.Len)
							{
								value = value.Replace(",", "");
							}
							if (value.Length > clsRegex.Len && clsRegex.PrintModifier == "DEC")
							{
								value = value.Replace("بدهکار", "بد".GetTranslate());
								value = value.Replace("بستانکار", "بس".GetTranslate());
							}
							if (value.Length > clsRegex.Len && (clsRegex.PrintModifier == "DEC" || clsRegex.PrintModifier == "DECSHORT"))
							{
								value = value.Substring(0, value.Length - 2);
							}
							if (value.Length > clsRegex.Len)
							{
								value = value.Substring(0, clsRegex.Len);
							}
							if (value.Length < clsRegex.Len)
							{
								int num = clsRegex.Len - value.Length;
								for (int i = 0; i < num; i++)
								{
									value += " ";
								}
							}
						}
						else
						{
							int num2 = value.Length;
							if (parameterName == "PRDName" && text.Contains("لا"))
							{
								num2--;
							}
							for (int j = 0; j < clsRegex.Len - num2; j++)
							{
								value += " ";
							}
						}
						data = data.Replace(clsRegex.SuccessfullMatch, value);
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "parameterName:" + parameterName + "//parameterValue:" + parameterValue + "//data:" + data, "SetParameter_StringText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs", 104);
				}
				return data;
			}
		}
	}

	public static string SetParameter_DecimalText(string parameterName, decimal parameterValue, string data)
	{
		using (new JaegerMonitor("SetParameter_DecimalText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs"))
		{
			try
			{
				string decimalSeprator = Text.decimalSeprator;
				string decimalPoint = Text.DecimalPoint;
				string text = parameterValue.NpToString("N0", 0);
				if (decimalSeprator != "")
				{
					text = text.Replace(decimalSeprator, ",");
				}
				if (decimalPoint != "")
				{
					text = text.Replace(decimalPoint, "/");
				}
				data = SetParameter_StringText(parameterName, text, data);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "SetParameter_Decimal/ParameterName:" + parameterName + "/ParameterValue:" + parameterValue + "/Template:" + data, "SetParameter_DecimalText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs", 121);
			}
			return data;
		}
	}

	public static string SetParameter_DecimalImage(string parameterName, decimal parameterValue, string data, bool addPrintModifier = false)
	{
		using (new JaegerMonitor("SetParameter_DecimalImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs"))
		{
			try
			{
				string text;
				if (addPrintModifier)
				{
					text = Math.Abs(parameterValue).ToString("N0", CultureInfo.CreateSpecificCulture("fa-IR").NumberFormat);
					if (parameterValue > 0m)
					{
						text += " بس";
					}
					else if (parameterValue < 0m)
					{
						text += " بد";
					}
				}
				else
				{
					text = parameterValue.ToString("N0", CultureInfo.CreateSpecificCulture("fa-IR").NumberFormat);
				}
				data = SetParameter_StringImage(parameterName, text, data);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "SetParameter_Decimal2/ParameterName:" + parameterName + "/ParameterValue:" + parameterValue + "/Template:" + data, "SetParameter_DecimalImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs", 143);
			}
			return data;
		}
	}

	public static string SetParameter_StringImage(string parameterName, string parameterValue, string data)
	{
		using (new JaegerMonitor("SetParameter_StringImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs"))
		{
			try
			{
				data = data.Replace("\\r\\n", "<br/>");
				data = data.Replace("\\n", "<br/>");
				data = data.Replace("\\r", "<br/>");
				if (string.IsNullOrEmpty(parameterValue))
				{
					parameterValue = "";
				}
				ClsRegex clsRegex = new ClsRegex(data, "<" + parameterName + "[\\s]*/>");
				if (clsRegex.SuccessfullMatch.Trim() != "")
				{
					string newValue = parameterValue;
					data = data.Replace(clsRegex.SuccessfullMatch, newValue);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetParameter_StringImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Print\\ParameterReplacement.cs", 168);
			}
			return data;
		}
	}
}
