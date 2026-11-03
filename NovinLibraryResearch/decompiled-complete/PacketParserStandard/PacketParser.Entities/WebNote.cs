using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Note;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebNote : Serializable<WebNote>, INote, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, IDeliverableEntity
{
	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string Title { get; set; }

	public string Text { get; set; }

	public DateTime DateM { get; set; }

	public DateTime? DateSarResidM { get; set; }

	public NotePirority Pirority { get; set; }

	public NoteStatus Status { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid GroupGuid { get; set; }

	public string GroupName { get; set; }

	public string PirorityName { get; set; }

	public string StatusName { get; set; }

	public NoteDisplayState DisplayState { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	bool IHasStatus.Status => true;

	public Guid? EntityGuid { get; set; }

	[Obsolete("Use AsType")]
	public HesabType? EntityType { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public DateTime? DateUtc { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Document? AsType { get; set; }

	public WebNoteMetaData MetaData { get; set; }

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebNote>, Task> OnSaveRequest;

	public event Func<object, CusTitleReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebNote>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebNote>, Task> OnSync;

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs"))
		{
			_ = 4;
			try
			{
				switch ((NoteActions)pack.PacketActions)
				{
				case NoteActions.SaveRequest:
				{
					ConnectionEventArgs<WebNote> args3 = new ConnectionEventArgs<WebNote>(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs");
					break;
				}
				case NoteActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs");
					break;
				}
				case NoteActions.ReadRequest:
				{
					CusTitleReadEventArgs cusTitleReadEventArgs = pack.Args[1].FromJson<CusTitleReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 69);
					cusTitleReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					cusTitleReadEventArgs.Connection = Connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, cusTitleReadEventArgs, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs");
					break;
				}
				case NoteActions.ReadResponse:
				{
					ConnectionEventArgs<WebNote> args = new ConnectionEventArgs<WebNote>(pack, ConnectionUniqueId, Connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs");
					}
					break;
				}
				case (NoteActions)3:
				case (NoteActions)4:
				case NoteActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 93);
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		NoteActions noteActions = (NoteActions)action;
		return noteActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((NoteActions)action).GetDisplay();
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 111);
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				CusTitleReadEventArgs cusTitleReadEventArgs = new CusTitleReadEventArgs
				{
					Modified = modified
				};
				list.Add(cusTitleReadEventArgs.ToString());
				return new Packet(PacketType.Note, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 125);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebNote> items, bool isSync, int partitionId = 0)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs"))
		{
			try
			{
				if (items == null || items.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return items?.GeneratePackets(PacketType.Note, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 141);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebNote> titles, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs"))
		{
			try
			{
				if (titles == null || !titles.Any())
				{
					throw new NullReferenceException();
				}
				string text = titles.Select((WebNote p) => $"{p.Guid}/{p.Title}").GenerateDebugDescription(5);
				object result;
				if (titles == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = titles.GeneratePackets(PacketType.Note, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 157);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				string text = responses.Select((SaveResponse p) => $"Guid:{p.Guid}/Success:{p.SaveResultStatus}/{p.Message}").GenerateDebugDescription(5);
				object result;
				if (responses == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = responses.GeneratePackets(PacketType.Note, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 173);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebNote> titles)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.Note, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 187);
				throw;
			}
		}
	}

	public virtual ReturnedSaveFuncInfo CheckValidation()
	{
		using (new JaegerMonitor("CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				_ = Guid;
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				if (Title == null)
				{
					returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "عدم امکان یادداشت بدون موضوع - وب سرویس ثبت یادداشت", "Title", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 204);
				}
				if (Text == null)
				{
					returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "عدم امکان یادداشت بدون متن - وب سرویس ثبت یادداشت", Text, "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 207);
				}
			}
			catch (Exception exception)
			{
				returnedSaveFuncInfo.AddReturnedValue(exception, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebNote.cs", 225);
			}
			return returnedSaveFuncInfo;
		}
	}
}
