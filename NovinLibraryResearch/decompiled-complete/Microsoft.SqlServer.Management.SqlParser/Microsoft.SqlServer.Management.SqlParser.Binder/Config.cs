namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal static class Config
{
	public static class Names
	{
		public static class Databases
		{
			public const string Master = "master";

			public const string Model = "model";

			public const string Msdb = "msdb";

			public const string TempDb = "tempdb";
		}

		public const string DboSchema = "dbo";

		public const string DmlDeletedTable = "deleted";

		public const string DmlInsertedTable = "inserted";

		public const string InformationSchema = "INFORMATION_SCHEMA";

		public const string SysSchema = "sys";
	}

	public static class Lengths
	{
		public const int MaxIdentifierLength = 128;
	}

	public static class Constraints
	{
		public static class Prefixes
		{
			public const string Check = "CK";

			public const string Default = "DF";

			public const string ForeignKey = "FK";

			public const string PrimaryKey = "PK";

			public const string Unique = "UQ";
		}
	}

	public static class Intervals
	{
		public const int RefreshDatabaseList = 60000;
	}

	public static class Y2kCatalogViews
	{
		public static readonly string[] Server = new string[21]
		{
			"sysaltfiles", "syscacheobjects", "syscharsets", "sysconfigures", "syscurconfigs", "syscursorcolumns", "syscursorrefs", "syscursors", "syscursortables", "sysdatabases",
			"sysdevices", "syslanguages", "syslockinfo", "syslogins", "sysmessages", "sysoledbusers", "sysopentapes", "sysperfinfo", "sysprocesses", "sysremotelogins",
			"sysservers"
		};

		public static readonly string[] Database = new string[18]
		{
			"syscolumns", "syscomments", "sysconstraints", "sysdepends", "sysfilegroups", "sysfiles", "sysforeignkeys", "sysfulltextcatalogs", "sysindexes", "sysindexkeys",
			"sysmembers", "sysobjects", "syspermissions", "sysprotects", "sysreferences", "syssegments", "systypes", "sysusers"
		};
	}
}
