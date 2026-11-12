using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Babel;

public class Region
{
	private Location startLocation;

	private Location endLocation;

	public Location StartLocation => startLocation;

	public Location EndLocation => endLocation;

	public Region(Location startLocation, Location endLocation)
	{
		this.startLocation = startLocation;
		this.endLocation = endLocation;
	}
}
