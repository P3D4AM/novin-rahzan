using System;
using PacketParser.BusinessLayer.Models.Attendance;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Settings;

[Serializable]
[DoNotPruneType]
public class Payamak : AttDepartmentAdminSms
{
	public string PayamakValedVorod { get; set; }

	public string PayamakValedKhoroj { get; set; }

	public string SmsAbsentPerson { get; set; }

	public string SmsIncompletePerson { get; set; }

	public bool SendVorodValed { get; set; }

	public bool SendKhorojValed { get; set; }

	public bool IsSendSmsAbsentPerson { get; set; }

	public bool IsSendSmsVacationManager { get; set; }

	public bool IsSendSmsIncompletePerson { get; set; }

	public int SignalRPort { get; set; } = 5001;


	public string SignalRIp { get; set; } = "0.0.0.0";


	public string SqlConnectionString { get; set; }

	public string TitlePersonNameNew { get; set; } = "[نام]";


	public string TitlePersonFamilyNew { get; set; } = "[فامیل]";


	public string TitleMoeinCode { get; set; } = "[کد معین]";


	public string TitleTafsilCode { get; set; } = "[کد تفصیل]";


	public string TitleDate { get; set; } = "[تاریخ]";


	public string TitleTime { get; set; } = "[زمان]";


	public string TitleTypeTaradod { get; set; } = "[نوع تردد]";


	public string TitleOverTime { get; set; } = "[اضافه کار]";


	public string TitleLowTime { get; set; } = "[کسرکار]";


	public string ConnectionStringPayamak { get; set; } = "";

}
