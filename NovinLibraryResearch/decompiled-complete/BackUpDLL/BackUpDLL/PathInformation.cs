using System.IO;

namespace BackUpDLL;

public class PathInformation
{
	private string _path;

	public string Path
	{
		get
		{
			try
			{
				if (!Directory.Exists(_path))
				{
					Directory.CreateDirectory(_path);
				}
				return _path;
			}
			catch
			{
				return "";
			}
		}
		set
		{
			_path = value;
		}
	}

	public DriveInfo DriveInfo { get; set; }
}
