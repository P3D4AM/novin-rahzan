using System.Threading.Tasks;

namespace Azure.Core.Pipeline;

public abstract class HttpPipelineTransport
{
	public abstract void Process(HttpMessage message);

	public abstract ValueTask ProcessAsync(HttpMessage message);

	public abstract Request CreateRequest();
}
