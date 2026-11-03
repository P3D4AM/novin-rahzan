namespace PacketParser.Entities.Settings;

public class VersionModel
{
	public string TableName { get; set; }

	public string Version { get; set; }

	public long LongVersion => Version.VersionStringToLong();
}
