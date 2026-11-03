using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace EntityCache.Business.Attendance;

public class BiometricDetailBusiness : Serializable<BiometricDetailBusiness>, IBiometricDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public DateTime Modified { get; set; }

	public EnrolType EnrolType { get; set; }

	public int Ordinal { get; set; }

	public byte[]? Data { get; set; }

	[Obsolete("", true)]
	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public BiometricDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
