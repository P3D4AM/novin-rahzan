using System.Net;
using System.Net.Http;

namespace Azure.Core.Pipeline;

internal static class ServicePointHelpers
{
	private const int RuntimeDefaultConnectionLimit = 2;

	private const int IncreasedConnectionLimit = 50;

	public static void SetLimits(ServicePoint requestServicePoint)
	{
		if (requestServicePoint.ConnectionLimit == 2)
		{
			requestServicePoint.ConnectionLimit = 50;
		}
	}

	public static void SetLimits(HttpClientHandler requestServicePoint)
	{
		if (requestServicePoint.MaxConnectionsPerServer == 2)
		{
			requestServicePoint.MaxConnectionsPerServer = 50;
		}
	}
}
