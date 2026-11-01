using System.IO;
using System.Windows.Forms;

namespace TimeAttendanceDAL.Classes;

public class ClsMaliyatPath
{
	public static string MaliyatPath => Application.StartupPath + "\\Maliyat";

	public static string CreatePath(string path)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		return path;
	}

	public static string Create()
	{
		CreatePath(MaliyatPath);
		return MaliyatPath;
	}
}
