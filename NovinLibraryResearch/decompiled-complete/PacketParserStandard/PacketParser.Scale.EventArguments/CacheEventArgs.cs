using PacketParser.Scale.Provider;

namespace PacketParser.Scale.EventArguments;

public class CacheEventArgs
{
	public MicroScaleInfo Info { get; set; }

	public IScaleConnection Connection { get; set; }

	public IScaleCommandProvider CommandProvider { get; set; }

	public CacheEventArgs(MicroScaleInfo info, IScaleConnection connection, IScaleCommandProvider provider)
	{
		Info = info;
		Connection = connection;
		CommandProvider = provider;
	}
}
