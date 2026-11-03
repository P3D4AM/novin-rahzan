using System;
using AccSqlServerPersistence.AssistanceClasses;
using PacketParser.Entities;

namespace BackUpDLL.Assistance;

public class BackUpConfig
{
	public string StartUpPath { get; set; }

	public string ConnectionString { get; set; }

	public ENSource Source { get; set; }

	public string Path { get; set; } = "";


	public Guid? Guid { get; set; }

	public BackUpMethod CompressMethod { get; set; } = BackUpMethod.GZip;


	public string RefrenceGuid { get; set; }

	public Guid? UserAccGuid { get; set; }

	public SqlVersion Version { get; private set; }

	public string VersionString
	{
		get
		{
			if (Version == null)
			{
				return null;
			}
			return "Version_" + Version.FullVersion.Replace(".", "_");
		}
	}

	public BackUpConfig(string version)
	{
		if (!string.IsNullOrEmpty(version))
		{
			Version = new SqlVersion(version);
		}
	}
}
