using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Protocols;

public interface IDocumentRetriever
{
	Task<string> GetDocumentAsync(string address, CancellationToken cancel);
}
