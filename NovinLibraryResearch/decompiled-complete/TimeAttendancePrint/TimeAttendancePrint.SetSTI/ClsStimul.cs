using System;
using System.IO;
using System.Windows.Forms;

namespace TimeAttendancePrint.SetSTI;

public class ClsStimul
{
	public static void CreateFile(byte[] resource, string fileName)
	{
		try
		{
			string path = Path.Combine(Application.StartupPath, fileName);
			if (!File.Exists(path))
			{
				File.WriteAllBytes(path, resource);
				return;
			}
			try
			{
				File.Delete(path);
				File.WriteAllBytes(path, resource);
			}
			catch (Exception)
			{
			}
		}
		catch (Exception)
		{
		}
	}
}
