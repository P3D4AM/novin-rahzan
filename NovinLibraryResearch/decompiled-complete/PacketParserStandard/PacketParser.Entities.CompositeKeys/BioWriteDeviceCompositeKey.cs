using System;

namespace PacketParser.Entities.CompositeKeys;

public class BioWriteDeviceCompositeKey
{
	public Guid BioGuid { get; set; }

	public Guid DeviceGuid { get; set; }

	public EnDeviceMessageType MessageType { get; set; }
}
