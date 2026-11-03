namespace PacketParser.Connection;

public class ClientConnectionSettings
{
	public const string RequestPrefix = "&ConnectionSetting*";

	public const string Response = "&ConnectionSetting:OK*";

	public short clientVersion { get; set; }

	public string ToString(bool AddPrefix)
	{
		if (AddPrefix)
		{
			return "&ConnectionSetting*" + Json.ToStringJson(this);
		}
		return Json.ToStringJson(this);
	}

	public override string ToString()
	{
		return ToString(AddPrefix: true);
	}

	public static ClientConnectionSettings FromString(string data)
	{
		return data.Replace("&ConnectionSetting*", "").FromJson<ClientConnectionSettings>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Connection\\ClientConnectionSettings.cs", 20);
	}
}
