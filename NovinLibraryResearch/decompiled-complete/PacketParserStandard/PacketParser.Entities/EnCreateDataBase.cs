namespace PacketParser.Entities;

public enum EnCreateDataBase
{
	Unknown,
	serverConnectionStringError,
	databaseNameEmpty,
	DatabaseExists,
	Success,
	SqlServerIsOld
}
