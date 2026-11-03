namespace PacketParser.Filters;

public class OrderRemoveFilter
{
	public bool IsDeleteReception { get; set; }

	public bool IsDeletePardakht { get; set; }

	public bool IsDeleteAghsat { get; set; }

	public bool IsDeleteTolid { get; set; }

	public OrderRemoveFilter(bool config = false)
	{
		IsDeleteReception = config;
		IsDeletePardakht = config;
		IsDeleteAghsat = config;
		IsDeleteTolid = config;
	}
}
