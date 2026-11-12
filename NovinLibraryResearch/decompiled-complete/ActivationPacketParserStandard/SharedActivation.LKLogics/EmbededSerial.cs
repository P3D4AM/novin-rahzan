using System;
using PacketParser;
using PacketParser.Entities;

namespace SharedActivation.LKLogics;

public class EmbededSerial
{
	private long _lkSerial;

	private SoftWareSerial _SoftWareSerial;

	public bool IsValid = true;

	public bool IsDemo = false;

	public string Version => SoftWareSerial?.Version;

	public ENSource Source
	{
		get
		{
			try
			{
				return (SoftWareSerial?.InstancePackage ?? Package.None) switch
				{
					Package.None => ENSource.None, 
					Package.HoghoghVaDastmozd => ENSource.TimeAttendance, 
					Package.TamdideAccounting => ENSource.Accounting, 
					Package.Accounting => ENSource.Accounting, 
					Package.TamdideScale => ENSource.ScaleDal, 
					Package.TamdideHozor => ENSource.TimeAttendance, 
					Package.baje => ENSource.Accounting, 
					_ => ENSource.Accounting, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Source", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\EmbededSerial.cs", 43);
				return ENSource.None;
			}
		}
	}

	public long LkSerial
	{
		get
		{
			return _lkSerial;
		}
		set
		{
			_lkSerial = value;
		}
	}

	public SoftWareSerial SoftWareSerial
	{
		get
		{
			return _SoftWareSerial;
		}
		set
		{
			_SoftWareSerial = value;
		}
	}

	public EmbededSerial(string EmbededSerial)
	{
		ParseActivationCode(EmbededSerial);
	}

	private void ParseActivationCode(string embededSerial)
	{
		int num = 0;
		try
		{
			if (string.IsNullOrEmpty(embededSerial))
			{
				IsValid = false;
				return;
			}
			string serialNumber = "";
			num = 5;
			if (embededSerial.Length == 12)
			{
				LkSerial = embededSerial.ParseToLong(0L);
				serialNumber = "1234567890";
			}
			else if (embededSerial.Length > 10)
			{
				num = 10;
				if ((embededSerial.Substring(0, 10) ?? "") == "1234567890")
				{
					LkSerial = embededSerial.Substring(0, 10).ParseToLong(0L);
					serialNumber = embededSerial.Remove(0, 10);
				}
				else if (embededSerial.Length <= 17)
				{
					num = 15;
					LkSerial = 1234567890L;
					serialNumber = embededSerial;
				}
				else if (embededSerial.Length > 12)
				{
					num = 20;
					LkSerial = embededSerial.Substring(0, 12).ParseToLong(0L);
					serialNumber = embededSerial.Remove(0, 12);
				}
				else
				{
					IsValid = false;
				}
				num = 25;
			}
			SoftWareSerial = new SoftWareSerial(serialNumber);
			num = 26;
			IsValid &= SoftWareSerial.Isvalid;
			num = 27;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}//Input:{embededSerial}", "ParseActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\EmbededSerial.cs", 126);
			IsValid = false;
		}
	}
}
