namespace FormCustom.ServiceManagement;

public class ServiceManagerConfig
{
	private string _setUpIp;

	private string _connectionIp;

	public ServiceBaseManager Manager { get; set; }

	public string Ip
	{
		get
		{
			if (!string.IsNullOrEmpty(_connectionIp))
			{
				return _connectionIp;
			}
			_connectionIp = "127.0.0.1";
			return _connectionIp;
		}
		set
		{
			_connectionIp = value;
		}
	}

	public string SetUpIp
	{
		get
		{
			if (!string.IsNullOrEmpty(_setUpIp))
			{
				return _setUpIp;
			}
			_setUpIp = "0.0.0.0";
			return _setUpIp;
		}
		set
		{
			_setUpIp = value;
		}
	}

	public int Port { get; set; }

	public string ConnectionString { get; set; }

	public bool HandleConnectionString { get; set; } = true;


	public bool HandleConnectionStringEncrypt { get; set; }

	public bool HandleConnectionStringTrustServerCertificate { get; set; }

	public bool HandleConnectionStringPersistSecurityInfo { get; set; }

	public bool HandleConnectionStringMultiSubnetFailover { get; set; }
}
