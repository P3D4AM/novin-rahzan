using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebNotification : Serializable<WebNotification>, INotification, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver
{
	private const int ChunckSizeKB = 100;

	public Guid Id { get; set; }

	public short? UserId { get; set; }

	public int? AppId { get; set; }

	public string Title { get; set; }

	public string Message { get; set; }

	public short Action { get; set; }

	public string Param1 { get; set; }

	public string Param2 { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public short DepId { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid Guid { get; set; }

	public event Func<object, NotificationReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebNotification>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebNotification>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebNotification>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				NotificationReadEventArgs notificationReadEventArgs = new NotificationReadEventArgs
				{
					Modified = modified
				};
				list.Add(notificationReadEventArgs.ToString());
				return new Packet(PacketType.Notification, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs", 73);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebNotification> items, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.Notification, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs", 88);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebNotification> notifications)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs"))
		{
			try
			{
				return notifications?.GeneratePackets(PacketType.Notification, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs", 102);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.Notification, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs", 116);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			NotificationActions notificationActions = (NotificationActions)action;
			return notificationActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs", 131);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((NotificationActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs", 144);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs"))
		{
			_ = 4;
			try
			{
				switch ((NotificationActions)pack.PacketActions)
				{
				case NotificationActions.SaveRequest:
				{
					ConnectionEventArgs<WebNotification> args3 = new ConnectionEventArgs<WebNotification>(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs");
					break;
				}
				case NotificationActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs");
					break;
				}
				case NotificationActions.ReadRequest:
				{
					NotificationReadEventArgs notificationReadEventArgs = pack.Args[1].FromJson<NotificationReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs", 165);
					notificationReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					notificationReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, notificationReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs");
					break;
				}
				case NotificationActions.ReadResponse:
				{
					ConnectionEventArgs<WebNotification> args = new ConnectionEventArgs<WebNotification>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs");
					}
					break;
				}
				case (NotificationActions)3:
				case (NotificationActions)4:
					break;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNotification.cs", 182);
			}
		}
	}
}
