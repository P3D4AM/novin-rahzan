namespace PacketParser.Entities;

public enum EnSqlAuthenticationMode : short
{
	[PersianNameAttribute.PersianName("WindowsAuthentication")]
	WindowsAuthentication = 1,
	[PersianNameAttribute.PersianName("SqlServerAuthentication")]
	SqlServerAuthentication
}
