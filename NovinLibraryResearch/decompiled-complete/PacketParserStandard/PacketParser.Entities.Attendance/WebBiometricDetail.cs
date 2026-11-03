using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace PacketParser.Entities.Attendance;

public class WebBiometricDetail : IBiometricDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public EnrolType EnrolType { get; set; }

	public int Ordinal { get; set; }

	public byte[]? Data { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }
}
