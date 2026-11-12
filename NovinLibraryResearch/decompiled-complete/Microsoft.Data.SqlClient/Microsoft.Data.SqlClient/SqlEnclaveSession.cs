namespace Microsoft.Data.SqlClient;

internal class SqlEnclaveSession
{
	private static readonly string _sessionKeyName = "SessionKey";

	private static readonly string _className = "EnclaveSession";

	private readonly byte[] _sessionKey;

	internal long SessionId { get; }

	internal byte[] GetSessionKey()
	{
		return Clone(_sessionKey);
	}

	private byte[] Clone(byte[] arrayToClone)
	{
		byte[] array = new byte[arrayToClone.Length];
		for (int i = 0; i < arrayToClone.Length; i++)
		{
			array[i] = arrayToClone[i];
		}
		return array;
	}

	internal SqlEnclaveSession(byte[] sessionKey, long sessionId)
	{
		if (sessionKey == null)
		{
			throw SQL.NullArgumentInConstructorInternal(_sessionKeyName, _className);
		}
		if (sessionKey.Length == 0)
		{
			throw SQL.EmptyArgumentInConstructorInternal(_sessionKeyName, _className);
		}
		_sessionKey = sessionKey;
		SessionId = sessionId;
	}
}
