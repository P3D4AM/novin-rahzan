using System.ComponentModel;

namespace PacketParser.Settings.Accounting;

public class BackUpSettings
{
	private int _backupTimeBetween;

	private string backupPath;

	public int BackupTimeBetween
	{
		get
		{
			if (_backupTimeBetween <= 0)
			{
				return 30;
			}
			return _backupTimeBetween;
		}
		set
		{
			if (value < 3)
			{
				value = 3;
			}
			_backupTimeBetween = value;
		}
	}

	[DefaultValue(true)]
	public bool EnableAutoBackup { get; set; } = true;


	[DefaultValue(true)]
	public bool EnableAutoBackupAllPartions { get; set; } = true;


	[DefaultValue(true)]
	public bool EnableZipBackup { get; set; } = true;


	public string BackupPath
	{
		get
		{
			if (string.IsNullOrEmpty(backupPath))
			{
				backupPath = "c:\\" + ResourceManagement.TextBrandResourceManagement?.DefaultBackUPFolderName;
			}
			return backupPath;
		}
		set
		{
			backupPath = value;
		}
	}
}
