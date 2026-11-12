using System;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Cryptography;

public interface IKeyEncryptionKey
{
	string KeyId { get; }

	byte[] WrapKey(string algorithm, ReadOnlyMemory<byte> key, CancellationToken cancellationToken = default(CancellationToken));

	Task<byte[]> WrapKeyAsync(string algorithm, ReadOnlyMemory<byte> key, CancellationToken cancellationToken = default(CancellationToken));

	byte[] UnwrapKey(string algorithm, ReadOnlyMemory<byte> encryptedKey, CancellationToken cancellationToken = default(CancellationToken));

	Task<byte[]> UnwrapKeyAsync(string algorithm, ReadOnlyMemory<byte> encryptedKey, CancellationToken cancellationToken = default(CancellationToken));
}
