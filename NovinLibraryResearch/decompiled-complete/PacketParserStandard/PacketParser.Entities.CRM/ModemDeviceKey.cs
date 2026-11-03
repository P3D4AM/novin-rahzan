namespace PacketParser.Entities.CRM;

internal class ModemDeviceKey
{
	public short DeviceNumber { get; set; }

	public short LineNumber { get; set; }

	public string SerialNumber { get; set; }

	public ModemDeviceKey(short DeviceNumber, short LineNumber, string SerialNumber)
	{
		this.DeviceNumber = DeviceNumber;
		this.LineNumber = LineNumber;
		this.SerialNumber = SerialNumber;
	}

	public string toString()
	{
		return $"{DeviceNumber}/{LineNumber}/{SerialNumber}";
	}
}
