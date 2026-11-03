using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebIntroductionMethod : Serializable<WebIntroductionMethod>, IIntroductionMethod, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, IDisposable, INpDisposable
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, GeneralReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebIntroductionMethod>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebIntroductionMethod>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebIntroductionMethod>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				GeneralReadEventArgs generalReadEventArgs = new GeneralReadEventArgs
				{
					Modified = modified
				};
				list.Add(generalReadEventArgs.ToString());
				return new Packet(PacketType.IntroductionMethod, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs", 46);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebIntroductionMethod> items, int partitionId, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs"))
		{
			try
			{
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.IntroductionMethod, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs", 61);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebIntroductionMethod> titles, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs"))
		{
			try
			{
				object result;
				if (titles == null)
				{
					result = null;
				}
				else
				{
					result = titles.GeneratePackets(PacketType.IntroductionMethod, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs", 76);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.IntroductionMethod, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs", 90);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebIntroductionMethod> titles)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.IntroductionMethod, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs", 103);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		IntroductionMethodActions introductionMethodActions = (IntroductionMethodActions)action;
		return introductionMethodActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((IntroductionMethodActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs"))
		{
			_ = 4;
			try
			{
				switch ((IntroductionMethodActions)pack.PacketActions)
				{
				case IntroductionMethodActions.SaveRequest:
				{
					ConnectionEventArgs<WebIntroductionMethod> args3 = new ConnectionEventArgs<WebIntroductionMethod>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs");
					break;
				}
				case IntroductionMethodActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs");
					break;
				}
				case IntroductionMethodActions.ReadRequest:
				{
					GeneralReadEventArgs generalReadEventArgs = pack.Args[1].FromJson<GeneralReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs", 126);
					generalReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					generalReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, generalReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs");
					break;
				}
				case IntroductionMethodActions.ReadResponse:
				{
					ConnectionEventArgs<WebIntroductionMethod> args = new ConnectionEventArgs<WebIntroductionMethod>(pack, connectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs");
					}
					break;
				}
				case (IntroductionMethodActions)3:
				case (IntroductionMethodActions)4:
				case IntroductionMethodActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs", 147);
			}
		}
	}

	public override void Dispose()
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIntroductionMethod.cs", 165);
		}
	}
}
