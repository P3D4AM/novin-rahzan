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
public class WebSend2Friend : Serializable<WebSend2Friend>, ISend2Friend, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver
{
	private class NestedWebSend2Friend
	{
		internal static readonly WebSend2Friend instance;

		static NestedWebSend2Friend()
		{
			instance = new WebSend2Friend();
		}
	}

	private const int ChunckSizeKB = 100;

	public string Name { get; set; }

	public string Family { get; set; }

	public short Gender { get; set; }

	public string Message { get; set; }

	public int Type { get; set; }

	public int CustomerId { get; set; }

	public string Mob { get; set; }

	public string Email { get; set; }

	public string Ip { get; set; }

	public DateTime DateM { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string DateSh => DateM.MiladiToCalture();

	public long SmsId { get; set; }

	public short SmsSt { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid Guid { get; set; } = Guid.NewGuid();


	public static WebSend2Friend Send2FriendInstance => NestedWebSend2Friend.instance;

	public Guid CustomerGuid { get; set; }

	public event Func<object, Send2FriendReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebSend2Friend>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebSend2Friend>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebSend2Friend>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs"))
		{
			try
			{
				return new Packet(PacketType.Send2Friend, 5, null, null, 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, string.Empty, "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs", 71);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebSend2Friend> items, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.Send2Friend, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, string.Empty, "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs", 93);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebSend2Friend> friends)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs"))
		{
			try
			{
				return friends?.GeneratePackets(PacketType.Send2Friend, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, string.Empty, "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs", 111);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.Send2Friend, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, string.Empty, "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs", 129);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebSend2Friend> sends)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs"))
		{
			try
			{
				return sends?.GeneratePackets(PacketType.Send2Friend, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs", 143);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			Send2FriendActions send2FriendActions = (Send2FriendActions)action;
			return send2FriendActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs", 158);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((Send2FriendActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs", 171);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs"))
		{
			_ = 4;
			try
			{
				switch ((Send2FriendActions)pack.PacketActions)
				{
				case Send2FriendActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebSend2Friend> args3 = new ConnectionEventArgs<WebSend2Friend>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs");
					}
					break;
				case Send2FriendActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs");
					}
					break;
				case Send2FriendActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						Send2FriendReadEventArgs args2 = new Send2FriendReadEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnReadRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs");
					}
					break;
				case Send2FriendActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebSend2Friend> args = new ConnectionEventArgs<WebSend2Friend>(pack, connectionUniqueId, connection);
					if (pack.IsSynced)
					{
						if (this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs");
						}
					}
					else if (this.OnReadResponse != null)
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs");
					}
					break;
				}
				case (Send2FriendActions)3:
				case (Send2FriendActions)4:
				case Send2FriendActions.Reset:
					break;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSend2Friend.cs", 220);
			}
		}
	}
}
