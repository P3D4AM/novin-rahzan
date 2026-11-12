using System;

namespace Microsoft.Data.SqlClient;

internal class EnclaveDiffieHellmanInfo
{
	public int Size { get; private set; }

	public byte[] PublicKey { get; private set; }

	public byte[] PublicKeySignature { get; private set; }

	public EnclaveDiffieHellmanInfo(byte[] payload)
	{
		Size = payload.Length;
		int num = BitConverter.ToInt32(payload, 0);
		int num2 = BitConverter.ToInt32(payload, 4);
		PublicKey = new byte[num];
		PublicKeySignature = new byte[num2];
		Buffer.BlockCopy(payload, 8, PublicKey, 0, num);
		Buffer.BlockCopy(payload, 8 + num, PublicKeySignature, 0, num2);
	}
}
