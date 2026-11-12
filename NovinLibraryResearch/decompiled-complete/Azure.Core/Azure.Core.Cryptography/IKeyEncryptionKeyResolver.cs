using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Cryptography;

public interface IKeyEncryptionKeyResolver
{
	IKeyEncryptionKey Resolve(string keyId, CancellationToken cancellationToken = default(CancellationToken));

	Task<IKeyEncryptionKey> ResolveAsync(string keyId, CancellationToken cancellationToken = default(CancellationToken));
}
