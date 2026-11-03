using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebContactUs : Serializable<WebContactUs>, IContactUs, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, ISyncFromClient2Server<WebContactUs>, IDeliverableEntity, IEditableDelivery, INpDisposable, IDisposable
{
	private const int ChunckSizeKB = 100;

	public string Name { get; set; }

	public string Family { get; set; }

	public string Fax { get; set; }

	public string Tel { get; set; }

	public string Mob { get; set; }

	public string Address { get; set; }

	public string Email { get; set; }

	public string Message { get; set; }

	public string Subject { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public string Ip { get; set; } = "";


	[JsonIgnore]
	[XmlIgnore]
	public string DateSh => DateM.MiladiToCalture();

	public string CodeMeli { get; set; }

	public string Zip { get; set; }

	public DateTime? DateBirth { get; set; }

	public string Ostan { get; set; }

	public string Shahrestan { get; set; }

	public string Shoghl { get; set; }

	public string Senf { get; set; }

	public string Tel2 { get; set; }

	public string Mob2 { get; set; }

	public string WebSite { get; set; }

	public string NahveAshnaee { get; set; }

	public string Tahsilat { get; set; }

	public string Description { get; set; }

	public TypeContact Type { get; set; } = TypeContact.CallUS;


	[DefaultValue(true)]
	public bool Jensiat { get; set; } = true;


	public string SabegheKari { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, ContactUsReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebContactUs>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebContactUs>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebContactUs>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				ContactUsReadEventArgs contactUsReadEventArgs = new ContactUsReadEventArgs
				{
					Modified = modified
				};
				list.Add(contactUsReadEventArgs.ToString());
				return new Packet(PacketType.ContactUs, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 84);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebContactUs> contacts, bool isSync = false)
	{
		if (contacts == null || contacts.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs"))
		{
			try
			{
				if (contacts == null || !contacts.Any())
				{
					return null;
				}
				return contacts?.GeneratePackets(PacketType.ContactUs, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 100);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebContactUs> contacts)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs"))
		{
			try
			{
				if (contacts == null || !contacts.Any())
				{
					throw new NullReferenceException();
				}
				return contacts?.GeneratePackets(PacketType.ContactUs, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 117);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.ContactUs, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 133);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebContactUs> contacts)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs"))
		{
			try
			{
				if (contacts == null || contacts.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return contacts?.GeneratePackets(PacketType.ContactUs, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 150);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			ContactUsActions contactUsActions = (ContactUsActions)action;
			return contactUsActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 166);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((ContactUsActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 179);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs"))
		{
			_ = 4;
			try
			{
				switch ((ContactUsActions)pack.PacketActions)
				{
				case ContactUsActions.SaveRequest:
				{
					ConnectionEventArgs<WebContactUs> args3 = new ConnectionEventArgs<WebContactUs>(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs");
					break;
				}
				case ContactUsActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs");
					break;
				}
				case ContactUsActions.ReadRequest:
				{
					ContactUsReadEventArgs contactUsReadEventArgs = pack.Args[1].FromJson<ContactUsReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 202);
					contactUsReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					contactUsReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, contactUsReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs");
					break;
				}
				case ContactUsActions.ReadResponse:
				{
					ConnectionEventArgs<WebContactUs> args = new ConnectionEventArgs<WebContactUs>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs");
					}
					break;
				}
				case (ContactUsActions)3:
				case (ContactUsActions)4:
				case ContactUsActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 227);
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebContactUs.cs", 264);
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}
}
