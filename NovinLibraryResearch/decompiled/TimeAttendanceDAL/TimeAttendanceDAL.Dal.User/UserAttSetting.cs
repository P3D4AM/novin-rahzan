using System;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.User;

[Serializable]
[DoNotPruneType]
public class UserAttSetting
{
	public int LockTime { get; set; } = 120;


	public string ManualAddTaradodCharacter { get; set; } = "#";


	public string ManualEditTaradodCharacter { get; set; } = "*";


	public bool ShowFrmEnter { get; set; } = true;


	public bool viewGridLastTaradods { get; set; } = true;


	public int CountLastTaradods { get; set; } = 20;


	public bool viewGridReportDevices { get; set; }

	public int CountReportDevices { get; set; } = 20;


	public bool ViewCalendar { get; set; } = true;


	public bool EnableCloudSignalR { get; set; } = true;

}
