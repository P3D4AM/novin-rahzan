using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace PacketParser.Entities;

public class ConnectionData : Serializable<ConnectionData>, IDisposable
{
	private bool _isDisposed;

	protected CancellationTokenSource _connectionToken;

	protected CancellationTokenSource _syncToken;

	private ENSource source;

	private string _guid;

	[JsonIgnore]
	[XmlIgnore]
	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public ENSource Source
	{
		get
		{
			return source;
		}
		set
		{
			if (value == ENSource.WordPressDebug)
			{
				throw new Exception("connection source fill with wordpress debug!!!!!!");
			}
			source = value;
		}
	}

	public int UserId { get; set; }

	public Guid UserAccGuid { get; set; }

	public Guid UserDepGuid { get; set; }

	public short DepId { get; set; }

	public Guid CustomerTafsilGuid { get; set; }

	public DateTime ConnectionStartTime { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string ConnectionStartDateSh => ConnectionStartTime.MiladiToCalture();

	[JsonIgnore]
	public string ConnectionStartTimeSh => ConnectionStartTime.ToShortTimeString();

	public long TotalSendBytes { get; set; }

	public short TotalSendPacketCount { get; set; }

	public long TotalRecivedBytes { get; set; }

	public string TotalSendForDisplay
	{
		get
		{
			return FormatBytes(TotalSendBytes);
		}
		set
		{
		}
	}

	public string TotalRecivedForDisplay
	{
		get
		{
			return FormatBytes(TotalRecivedBytes);
		}
		set
		{
		}
	}

	public long TotalTransferBytes => checked(TotalRecivedBytes + TotalSendBytes);

	[XmlIgnore]
	[JsonIgnore]
	public CancellationToken cancellationToken => _connectionToken.Token;

	public long HashId { get; set; }

	public string Guid
	{
		get
		{
			return _guid;
		}
		set
		{
			if (string.IsNullOrEmpty(_guid) || _guid == System.Guid.Empty.ToString() || value == _guid)
			{
				_guid = value;
				return;
			}
			throw new InvalidOperationException("connection UniqueId cannot change from(" + _guid + ") to (" + value + ")");
		}
	}

	public long LkSerial { get; set; }

	public string Ip { get; set; }

	public long AndroidIme { get; set; }

	public string Version { get; set; }

	public Guid DbGuid { get; set; }

	public short ClientVersion { get; set; }

	public ENCompressedMode CompressedMode { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public WebContextHolder Context { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string ContextGuid => Context.Guid;

	public event Func<object, System.EventArgs, Task> OnDisposed;

	public ConnectionData(CancellationToken cancellationToken, WebContextHolder context)
	{
		_connectionToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { cancellationToken });
		Context = context;
	}

	public ConnectionData()
	{
		_connectionToken = new CancellationTokenSource();
	}

	public virtual void Dispose(string reason)
	{
		Dispose(disposing: true);
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionData.cs"))
		{
			try
			{
				if (!_isDisposed)
				{
					_isDisposed = true;
					if (this.OnDisposed != null)
					{
						Utilities.RaiseEvent(this.OnDisposed, this, new System.EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionData.cs");
						this.OnDisposed = null;
					}
					Source = ENSource.None;
					UserId = 0;
					UserAccGuid = System.Guid.Empty;
					UserDepGuid = System.Guid.Empty;
					DepId = 0;
					CustomerTafsilGuid = System.Guid.Empty;
					HashId = 0L;
					LkSerial = 0L;
					Ip = "";
					AndroidIme = 0L;
					Version = "";
					base.Dispose(disposing);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionData.cs", 72);
			}
		}
	}

	private string FormatBytes(long sizeInBytes)
	{
		try
		{
			if (sizeInBytes < 1024)
			{
				return sizeInBytes + "Bytes";
			}
			if (sizeInBytes < 1048576)
			{
				return Math.Round((double)sizeInBytes / 1024.0, 2).ToString().Replace(".", "/")
					.Replace("/00", "") + "Kb";
			}
			return Math.Round((double)sizeInBytes / 1048576.0, 2).ToString().Replace(".", "/")
				.Replace("/00", "") + "Mb";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"InputData:{sizeInBytes}", "FormatBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionData.cs", 164);
			return "";
		}
	}
}
