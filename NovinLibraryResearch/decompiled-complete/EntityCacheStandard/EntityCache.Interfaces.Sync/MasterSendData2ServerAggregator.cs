namespace EntityCache.Interfaces.Sync;

public class MasterSendData2ServerAggregator
{
	public IMasterSendData2Server Master { get; private set; }

	public MasterSendData2ServerAggregator(IMasterSendData2Server master)
	{
		Master = master;
	}
}
