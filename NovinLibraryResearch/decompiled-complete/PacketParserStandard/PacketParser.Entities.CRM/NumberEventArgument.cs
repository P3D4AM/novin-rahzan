using System;

namespace PacketParser.Entities.CRM;

public class NumberEventArgument : Serializable<NumberEventArgument>
{
	public Guid Guid { get; set; }

	public short Prefix { get; set; }

	public bool Mobile { get; set; }

	public string CallerIdNumber { get; set; } = "";


	public string Number { get; set; } = "";


	public DateTime DateM { get; set; }

	public short DeviceNumber { get; set; }

	public string DeviceSerial { get; set; }

	public short LineNumber { get; set; }

	public bool IsTcpEvent { get; set; }

	public string Temp { get; set; }

	public NumberEventArgument()
	{
	}

	public NumberEventArgument(string callerIdNumber, Guid eventGuid, short deviceNumber, short lineNumber, bool isTcpEvent, string deviceSerialNumber)
	{
		try
		{
			CallerIdNumber = callerIdNumber.RemoveNoNumbers();
			DateM = DateTime.Now;
			Guid = eventGuid;
			DeviceSerial = deviceSerialNumber;
			DeviceNumber = deviceNumber;
			LineNumber = lineNumber;
			WebTafsilNumbers webTafsilNumbers = new WebTafsilNumbers(CallerIdNumber);
			if (webTafsilNumbers.IsValid)
			{
				Prefix = webTafsilNumbers.Prefix;
				Mobile = webTafsilNumbers.Mobile;
				Number = webTafsilNumbers.CalcedNumber.ToString();
			}
			IsTcpEvent = isTcpEvent;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "PortData:" + callerIdNumber, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\NumberEventArgument.cs", 43);
		}
	}
}
