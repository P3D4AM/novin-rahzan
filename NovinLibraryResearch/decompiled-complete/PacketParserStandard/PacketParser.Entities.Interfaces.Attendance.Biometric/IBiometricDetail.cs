namespace PacketParser.Entities.Interfaces.Attendance.Biometric;

public interface IBiometricDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	EnrolType EnrolType { get; set; }

	int Ordinal { get; set; }
}
