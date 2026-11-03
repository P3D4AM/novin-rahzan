using System;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class SqlVersion
{
	public string FullVersion { get; private set; }

	public string Name => GetVersionName();

	public int SqlServerVersion => GetSqlVersion();

	public int Minor { get; private set; }

	public int Major { get; private set; }

	public int Revision { get; private set; }

	public short MajorRevision { get; private set; }

	public short MinorRevision { get; private set; }

	public SqlVersion(string _version)
	{
		try
		{
			FullVersion = _version;
			Version version = new Version(FullVersion);
			Minor = version.Minor;
			Major = version.Major;
			Revision = version.Revision;
			MajorRevision = version.MajorRevision;
			MinorRevision = version.MinorRevision;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlVersion.cs", 30);
		}
	}

	private string GetVersionName()
	{
		try
		{
			switch (Major)
			{
			case 8:
				return "SQL Server 2000";
			case 9:
				return "SQL Server 2005";
			case 10:
				if (Minor < 5)
				{
					return "SQL Server 2008";
				}
				return "SQL Server 2008 R2";
			case 11:
				return "SQL Server 2012";
			case 12:
				return "SQL Server 2014";
			case 13:
				return "SQL Server 2016";
			case 14:
				if (Minor == 0 && MajorRevision < 9)
				{
					return "SQL Server 2017";
				}
				return "SQL Server 2019 RC";
			default:
				return "SQL Server 2019 RC";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetVersionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlVersion.cs", 55);
		}
		return "";
	}

	private int GetSqlVersion()
	{
		try
		{
			switch (Major)
			{
			case 8:
				return 2000;
			case 9:
				return 2005;
			case 10:
				return 2008;
			case 11:
				return 2012;
			case 12:
				return 2014;
			case 13:
				return 2016;
			case 14:
				if (Minor == 0 && MajorRevision < 9)
				{
					return 2017;
				}
				return 2019;
			default:
				return 2019;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSqlVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlVersion.cs", 77);
		}
		return 0;
	}
}
