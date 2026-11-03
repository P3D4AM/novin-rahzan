using PacketParser.BusinessLayer.Models.Attendance;

namespace PacketParser.Entities.MetaData.Attendance;

public class AttDepartmentMetaData
{
	public AttDepartmentAdminSms AdminSms { get; set; }

	public AttDepartmentAdminSetting AdminSetting { get; set; }
}
