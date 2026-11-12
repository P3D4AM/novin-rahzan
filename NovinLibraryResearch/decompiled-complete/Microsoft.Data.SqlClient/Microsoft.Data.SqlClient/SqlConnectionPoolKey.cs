using System;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal class SqlConnectionPoolKey : DbConnectionPoolKey, ICloneable
{
	private SqlCredential _credential;

	private int _hashValue;

	private readonly string _accessToken;

	private ServerCertificateValidationCallback _serverCertificateValidationCallback;

	private ClientCertificateRetrievalCallback _clientCertificateRetrievalCallback;

	private SqlClientOriginalNetworkAddressInfo _originalNetworkAddressInfo;

	internal override string ConnectionString
	{
		get
		{
			return base.ConnectionString;
		}
		set
		{
			base.ConnectionString = value;
			CalculateHashCode();
		}
	}

	internal SqlCredential Credential => _credential;

	internal string AccessToken => _accessToken;

	internal ServerCertificateValidationCallback ServerCertificateValidationCallback => _serverCertificateValidationCallback;

	internal ClientCertificateRetrievalCallback ClientCertificateRetrievalCallback => _clientCertificateRetrievalCallback;

	internal SqlClientOriginalNetworkAddressInfo OriginalNetworkAddressInfo => _originalNetworkAddressInfo;

	internal SqlConnectionPoolKey(string connectionString, SqlCredential credential, string accessToken, ServerCertificateValidationCallback serverCertificateValidationCallback, ClientCertificateRetrievalCallback clientCertificateRetrievalCallback, SqlClientOriginalNetworkAddressInfo originalNetworkAddressInfo)
		: base(connectionString)
	{
		_credential = credential;
		_accessToken = accessToken;
		_serverCertificateValidationCallback = serverCertificateValidationCallback;
		_clientCertificateRetrievalCallback = clientCertificateRetrievalCallback;
		_originalNetworkAddressInfo = originalNetworkAddressInfo;
		CalculateHashCode();
	}

	private SqlConnectionPoolKey(SqlConnectionPoolKey key)
		: base(key)
	{
		_credential = key.Credential;
		_accessToken = key.AccessToken;
		_serverCertificateValidationCallback = key._serverCertificateValidationCallback;
		_clientCertificateRetrievalCallback = key._clientCertificateRetrievalCallback;
		CalculateHashCode();
	}

	object ICloneable.Clone()
	{
		return new SqlConnectionPoolKey(this);
	}

	public override bool Equals(object obj)
	{
		if (obj is SqlConnectionPoolKey sqlConnectionPoolKey && _credential == sqlConnectionPoolKey._credential && ConnectionString == sqlConnectionPoolKey.ConnectionString && string.CompareOrdinal(_accessToken, sqlConnectionPoolKey._accessToken) == 0 && _serverCertificateValidationCallback == sqlConnectionPoolKey._serverCertificateValidationCallback && _clientCertificateRetrievalCallback == sqlConnectionPoolKey._clientCertificateRetrievalCallback)
		{
			return _originalNetworkAddressInfo == sqlConnectionPoolKey._originalNetworkAddressInfo;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _hashValue;
	}

	private void CalculateHashCode()
	{
		_hashValue = base.GetHashCode();
		if (_credential != null)
		{
			_hashValue = _hashValue * 17 + _credential.GetHashCode();
		}
		else if (_accessToken != null)
		{
			_hashValue = _hashValue * 17 + _accessToken.GetHashCode();
		}
		if (_originalNetworkAddressInfo != null)
		{
			_hashValue = _hashValue * 17 + _originalNetworkAddressInfo.GetHashCode();
		}
	}
}
