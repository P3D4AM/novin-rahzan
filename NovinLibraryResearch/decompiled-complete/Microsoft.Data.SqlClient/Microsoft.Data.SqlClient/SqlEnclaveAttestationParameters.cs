using System;
using System.Security.Cryptography;

namespace Microsoft.Data.SqlClient;

internal class SqlEnclaveAttestationParameters
{
	private readonly byte[] _input;

	internal int Protocol { get; private set; }

	internal ECDiffieHellman ClientDiffieHellmanKey { get; private set; }

	internal SqlEnclaveAttestationParameters(int protocol, byte[] input, ECDiffieHellman clientDiffieHellmanKey)
	{
		if (input == null)
		{
			throw SQL.NullArgumentInConstructorInternal("input", "SqlEnclaveAttestationParameters");
		}
		if (clientDiffieHellmanKey == null)
		{
			throw SQL.NullArgumentInConstructorInternal("clientDiffieHellmanKey", "SqlEnclaveAttestationParameters");
		}
		_input = input;
		Protocol = protocol;
		ClientDiffieHellmanKey = clientDiffieHellmanKey;
	}

	internal byte[] GetInput()
	{
		if (_input == null)
		{
			return null;
		}
		byte[] array = new byte[_input.Length];
		Buffer.BlockCopy(_input, 0, array, 0, _input.Length);
		return array;
	}
}
