using System;
using PacketParser;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Taradod;

[Serializable]
[DoNotPruneType]
public class EditGrouply
{
	public long CodeTafsil { get; set; }

	public string DateSh { get; set; } = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateTime.Now);


	public string Time { get; set; } = "00:00";


	public string FullName { get; set; } = "";


	public bool Checked { get; set; } = true;

}
