using System;

namespace SharpCompress.Crypto;

public interface IBlockCipher
{
	string AlgorithmName { get; }

	bool IsPartialBlockOkay { get; }

	void Init(bool forEncryption, ICipherParameters parameters);

	int GetBlockSize();

	int ProcessBlock(ReadOnlySpan<byte> inBuf, Span<byte> outBuf);

	void Reset();
}
