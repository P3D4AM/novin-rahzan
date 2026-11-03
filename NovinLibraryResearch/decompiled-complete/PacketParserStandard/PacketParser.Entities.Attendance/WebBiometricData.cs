using System;
using System.Collections.Generic;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace PacketParser.Entities.Attendance;

public class WebBiometricData : IBiometric, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	public Guid Guid { get; set; }

	public EnDeviceGroup DeviceGroup { get; set; }

	public Guid? PersonGuid { get; set; }

	public short FingerPrintCount { get; set; }

	public bool HasPalm { get; set; }

	public bool HasFace { get; set; }

	public bool HasCard { get; set; }

	public bool HasPassword { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public List<WebBiometricDetail> DetailList { get; set; }
}
