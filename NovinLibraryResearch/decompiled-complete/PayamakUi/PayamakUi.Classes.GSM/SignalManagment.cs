using System;
using PacketParser.Entities;
using PayamakSettings.FlrSettings;

namespace PayamakUi.Classes.GSM;

public class SignalManagment
{
	public delegate void SignalEventHandler(short Signal);

	private static TimeSpan _LastUpdate = DateTime.Now.TimeOfDay;

	public static short SignalStrength
	{
		get
		{
			try
			{
				if (Math.Abs(_LastUpdate.Subtract(DateTime.Now.TimeOfDay).Seconds) > 200)
				{
					Registry.Write("SignalStrength", (object)"0");
					_LastUpdate = DateTime.Now.TimeOfDay;
				}
				return short.Parse(Registry.Read("SignalStrength"));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SignalStrength", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\SignalManagments.cs", 38);
				return 0;
			}
		}
		set
		{
			try
			{
				_LastUpdate = DateTime.Now.TimeOfDay;
				Registry.Write("SignalStrength", (object)value);
				SignalManagment.Signal?.Invoke(value);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SignalStrength", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\SignalManagments.cs", 52);
			}
		}
	}

	public static event SignalEventHandler Signal;
}
