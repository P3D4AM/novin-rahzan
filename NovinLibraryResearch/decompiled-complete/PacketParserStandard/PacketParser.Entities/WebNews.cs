using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebNews : Serializable<WebNews>, INews, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	private const int ChunckSizeKB = 100;

	private string _MetaDescription = "";

	public int Id { get; set; }

	public string Title { get; set; } = "";


	public string QuickText { get; set; } = "";


	public string FullContent { get; set; } = "";


	public string PicUrl { get; set; } = "";


	public string KeyWords { get; set; } = "";


	public DateTime DateM { get; set; } = DateTime.MaxValue;


	public string DateSh => DateM.MiladiToCalture();

	public NewsType Type { get; set; }

	public string Category { get; set; } = "";


	public string MetaDescription
	{
		get
		{
			return _MetaDescription;
		}
		set
		{
			_MetaDescription = value.FixString();
		}
	}

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid Guid { get; set; } = Guid.NewGuid();


	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool Checked { get; set; }

	public ServerStatusForClient ServerStatus { get; set; } = ServerStatusForClient.Delivered;


	public event Func<object, NewsReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebNews>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebNews>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebNews>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				NewsReadEventArgs newsReadEventArgs = new NewsReadEventArgs
				{
					Modified = modified
				};
				list.Add(newsReadEventArgs.ToString());
				return new Packet(PacketType.News, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 88);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebNews> items, bool isSync)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.News, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 103);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebNews> news)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs"))
		{
			try
			{
				return news?.GeneratePackets(PacketType.News, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 117);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.News, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 132);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebNews> news)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs"))
		{
			try
			{
				return news.GeneratePackets(PacketType.News, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 147);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			NewsActions newsActions = (NewsActions)action;
			return newsActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 166);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((NewsActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 179);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs"))
		{
			_ = 4;
			try
			{
				switch ((NewsActions)pack.PacketActions)
				{
				case NewsActions.SaveRequest:
				{
					ConnectionEventArgs<WebNews> args3 = new ConnectionEventArgs<WebNews>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs");
					break;
				}
				case NewsActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs");
					break;
				}
				case NewsActions.ReadRequest:
				{
					NewsReadEventArgs newsReadEventArgs = pack.Args[1].FromJson<NewsReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 202);
					newsReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					newsReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, newsReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs");
					break;
				}
				case NewsActions.ReadResponse:
				{
					ConnectionEventArgs<WebNews> args = new ConnectionEventArgs<WebNews>(pack, connectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs");
					}
					break;
				}
				case (NewsActions)3:
				case (NewsActions)4:
				case NewsActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 226);
			}
		}
	}

	public new void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnReadRequest != null)
				{
					this.OnReadRequest = null;
				}
				if (this.OnReadResponse != null)
				{
					this.OnReadResponse = null;
				}
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
				if (this.OnSaveResponse != null)
				{
					this.OnSaveResponse = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNews.cs", 244);
		}
	}
}
