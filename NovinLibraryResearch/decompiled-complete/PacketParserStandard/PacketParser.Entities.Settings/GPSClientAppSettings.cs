namespace PacketParser.Entities.Settings;

public class GPSClientAppSettings
{
	public string HostUrl { get; set; }

	public string GpsAdminUrl => HostUrl + "api/v1/admingps";
}
