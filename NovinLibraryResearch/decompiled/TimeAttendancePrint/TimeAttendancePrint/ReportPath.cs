using System.IO;
using System.Windows.Forms;

namespace TimeAttendancePrint;

public class ReportPath
{
	public static string ReportPath_Renamed => Application.StartupPath;

	public static string Create()
	{
		CreatePath(ReportPath_Renamed);
		return ReportPath_Renamed;
	}

	public static string CreatePath(string Path)
	{
		if (!Directory.Exists(Path))
		{
			Directory.CreateDirectory(Path);
		}
		return Path;
	}
}
