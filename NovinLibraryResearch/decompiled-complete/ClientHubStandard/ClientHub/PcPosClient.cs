using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace ClientHub;

public class PcPosClient : RetriableHubConnection
{
	private CancellationTokenSource _connectinToken;

	public PcPosClient(string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
	}

	public override Task InitializeConnectionAsync()
	{
		throw new NotImplementedException();
	}
}
