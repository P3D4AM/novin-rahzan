using System.Collections.Generic;

namespace TimeAttendanceDAL.Dal.Taradod;

public class SaveLogonPersonelResult
{
	public List<int> SavedLogIds { get; set; } = new List<int>();


	public List<int> deviceIds { get; set; } = new List<int>();


	public int CountSavedLogs { get; set; }
}
