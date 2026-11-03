namespace PacketParser.BusinessLayer.Models.Attendance;

public class AttDepartmentAdminSms
{
	public decimal SendTaradodSmsMaxMinutes;

	public decimal SendAbsentSmsMinMinutesAfterShift { get; set; }

	public decimal SendAbsentSmsMaxMinutesAfterShift { get; set; }

	public decimal SendIncompleteSmsFromClock { get; set; }

	public decimal SendIncompleteSmsToClock { get; set; }

	public decimal SendSmsFromClockOfDay { get; set; }

	public decimal SendSmsToClockOfDay { get; set; }

	public bool EnablePayamak { get; set; }

	public string PayamakManageVorod { get; set; }

	public string PayamakManageKhoroj { get; set; }

	public string SmsAbsentManager { get; set; }

	public string SmsIncompleteManager { get; set; }

	public string PayamakManageAdminMobs { get; set; }

	public bool SendVorodManage { get; set; }

	public bool SendKhorojManage { get; set; }

	public bool IsSendSmsAbsentManager { get; set; }

	public bool IsSendSmsIncompleteManager { get; set; }

	public int IdSamane { get; set; }
}
