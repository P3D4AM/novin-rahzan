namespace Stimulsoft.Report.Web;

internal class StiVersionHelper
{
	public static string AssemblyVersion
	{
		get
		{
			string[] array = "2022.1.1.0".Split(new char[1] { '.' });
			string text = array[0] + "." + array[1] + "." + array[2];
			if (array[3] != "0")
			{
				text = text + "." + array[3];
			}
			return text;
		}
	}

	public static string ProductVersion => AssemblyVersion + " from 7 December 2021";

	public static string ReportVersion => "Stimulsoft Reports " + ProductVersion;
}
