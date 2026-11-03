using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebFileInfo : Serializable<WebFileInfo>, IFileInfo, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, ISyncFromClient2Server<WebFileInfo>, IDeliverableEntity, IEditableDelivery
{
	public enum FileInfoActions
	{
		[PersianNameAttribute.PersianName("نامشخص")]
		Unknown,
		[PersianNameAttribute.PersianName("درخواست نوشتن")]
		WriteRequest,
		[PersianNameAttribute.PersianName("پاسخ درخواست نوشتن")]
		WriteResponse,
		[PersianNameAttribute.PersianName("درخواست کامیت  ")]
		CommitRequest,
		[PersianNameAttribute.PersianName("پاسخ درخواست کامیت")]
		CommitResponse,
		[PersianNameAttribute.PersianName("درخواست خواندن")]
		ReadRequest,
		[PersianNameAttribute.PersianName("سینک")]
		ReadResponse,
		[PersianNameAttribute.PersianName("درخواست گرفتن فایل")]
		GetFileRequest,
		[PersianNameAttribute.PersianName("ریست")]
		Reset,
		[PersianNameAttribute.PersianName("پاک کردن از سرور")]
		DeleteRequest,
		[PersianNameAttribute.PersianName("درخواست ذخیره سازی")]
		SaveRequest,
		[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
		SaveResponse
	}

	private const int ChunckSizeKB = 100;

	private static int counter;

	private static int ServerRequests;

	public string FileName { get; set; }

	public string SubDirectory { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid EntityGuid { get; set; }

	public FileManagement_Parts Part { get; set; } = FileManagement_Parts.Other;


	public long FileLen { get; set; }

	public long Crc { get; set; }

	public string FileVersion { get; set; } = "0";


	[JsonIgnore]
	public string LocalFullPath { get; set; }

	public string FullPath { get; set; }

	public DateTime Modified { get; set; }

	public byte[] Content { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public string Url { get; set; }

	public short DepId { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public byte[] FileContent { get; set; }

	public ENCompressedMode CompressMode { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public short Ordinal { get; set; }

	public event Func<object, ConnectionEventArgs<WebFileInfo>, Task> OnWriteRequest;

	public event Func<object, FileWriteResponseEventArgs, Task> OnWriteResponse;

	public event Func<object, FileCommitRequestEventArgs, Task> OnCommitRequest;

	public event Func<object, FileCommitResponseEventArgs, Task> OnCommitResponse;

	public event Func<object, FileReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebFileInfo>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebFileInfo>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<WebFileInfo>, Task> OnDeleteRequest;

	public event Func<object, ConnectionEventArgs<WebFileInfo>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	private bool CheckValidation()
	{
		if (FileName.Length > 100)
		{
			throw new ValidationException("FileName");
		}
		if (SubDirectory.Length > 50)
		{
			throw new ValidationException("SubDirectory");
		}
		if (FileVersion.Length > 50)
		{
			throw new ValidationException("FileVersion");
		}
		if (LocalFullPath.Length > 300)
		{
			throw new ValidationException("LocalFullPath");
		}
		if (FullPath.Length > 300)
		{
			throw new ValidationException("FullPath");
		}
		if (Url.Length > 300)
		{
			throw new ValidationException("Url");
		}
		if (HostRefrenced.Length > 50)
		{
			throw new ValidationException("HostRefrenced");
		}
		if (DeliveryMessage.Length > 500)
		{
			throw new ValidationException("DeliveryMessage");
		}
		if (DeliveryShortMessage.Length > 100)
		{
			throw new ValidationException("DeliveryShortMessage");
		}
		return true;
	}

	public static List<Packet> GenerateWriteRequest(IEnumerable<WebFileInfo> info, string moreInfo)
	{
		using (new JaegerMonitor("GenerateWriteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				return info?.GeneratePackets(PacketType.File, 1, null, isSync: false, null, 0, deliveryRequst: true, "GenerateWriteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateWriteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 96);
				throw;
			}
		}
	}

	public static Packet GenerateWriteRequest(WebFileInfo info, string moreInfo)
	{
		using (new JaegerMonitor("GenerateWriteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				List<string> list = new List<string> { ((short)1).ToString() };
				return new Packet(PacketType.File, 1, Json.ToBytesJson(new List<WebFileInfo> { info }), list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateWriteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 123);
				throw;
			}
		}
	}

	public Guid Random()
	{
		return Guid.NewGuid();
	}

	public static Packet GenerateWriteResponse(IEnumerable<string> fileNames, IEnumerable<bool> successes, IEnumerable<string> messages, string moreInfo)
	{
		using (new JaegerMonitor("GenerateWriteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				if (fileNames == null)
				{
					throw new ArgumentNullException("fileNames");
				}
				if (successes == null)
				{
					throw new ArgumentNullException("successes");
				}
				if (messages == null)
				{
					throw new ArgumentNullException("messages");
				}
				if (fileNames != null && !fileNames.Any())
				{
					throw new ArgumentException("List is empty", "fileNames");
				}
				if (successes != null && !successes.Any())
				{
					throw new ArgumentException("List is empty", "successes");
				}
				if (messages != null && !messages.Any())
				{
					throw new ArgumentException("List is empty", "messages");
				}
				if (messages.Count() != successes.Count() || successes.Count() != fileNames.Count())
				{
					throw new ArgumentException("List counts are not equal");
				}
				List<string> list = new List<string>
				{
					((short)2).ToString(),
					new FileWriteResponseEventArgs
					{
						Messages = messages?.ToList(),
						PartNames = fileNames?.ToList(),
						Successes = successes?.ToList(),
						PacketReferenceId = Guid.Empty
					}.ToString()
				};
				return new Packet(PacketType.File, 2, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateWriteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 176);
				throw;
			}
		}
	}

	public static Packet GenerateWriteResponse(string fileName, bool success, string message, string moreInfo)
	{
		using (new JaegerMonitor("GenerateWriteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				if (fileName == null)
				{
					throw new ArgumentNullException("fileName");
				}
				if (fileName == "")
				{
					throw new ArgumentException("Argument is empty string", "fileName");
				}
				List<string> list = new List<string>
				{
					((short)2).ToString(),
					new FileWriteResponseEventArgs
					{
						Messages = new List<string> { message },
						PartNames = new List<string> { fileName },
						Successes = new List<bool> { success }
					}.ToString()
				};
				return new Packet(PacketType.File, 2, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateWriteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 212);
				throw;
			}
		}
	}

	public static Packet GenerateCommitRequest(IEnumerable<string> orderList, string fileName, FileManagement_Parts filePart, Guid packetReferenceId, long crc, Guid entityGuid, Guid? fileGuid = null)
	{
		using (new JaegerMonitor("GenerateCommitRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				if (orderList == null || !orderList.Any())
				{
					return null;
				}
				List<string> list = new List<string>
				{
					((short)3).ToString(),
					new FileCommitRequestEventArgs
					{
						Crc = crc,
						FilePart = filePart,
						PartNames = orderList.ToList(),
						FileName = fileName,
						PacketReferenceId = packetReferenceId,
						FileGuid = fileGuid.GetValueOrDefault(packetReferenceId),
						EntityGuid = entityGuid
					}.ToString()
				};
				return new Packet(PacketType.File, 3, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateCommitRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 243);
				return null;
			}
		}
	}

	public static Packet GenerateCommitResponse(long crc, bool success, string message, string fileName, string url, Guid packetReferenceId, Guid fileGuid)
	{
		using (new JaegerMonitor("GenerateCommitResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				if (fileName == null)
				{
					throw new ArgumentNullException("String is null", "fileName");
				}
				if (fileName == "")
				{
					throw new ArgumentException("String is empty", "fileName");
				}
				List<string> list = new List<string>
				{
					((short)4).ToString(),
					new FileCommitResponseEventArgs
					{
						Crc = crc,
						FileName = fileName,
						Message = message,
						Success = success,
						Url = url,
						FileGuid = fileGuid
					}.ToString()
				};
				return new Packet(PacketType.File, 4, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateCommitResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 275);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				FileReadEventArgs fileReadEventArgs = new FileReadEventArgs
				{
					Modified = modified
				};
				list.Add(fileReadEventArgs.ToString());
				return new Packet(PacketType.File, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 298);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebFileInfo> items, int partition = 0, bool isSync = false)
	{
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			int num = items?.Sum((WebFileInfo p) => (p?.FileContent?.Length).GetValueOrDefault()) ?? 0;
			if (num > 10485760)
			{
				throw new ArgumentException($"sending {items?.Count() ?? (-1)} files with len: {num} bytes");
			}
			items?.Select((WebFileInfo p) => p.CheckValidation());
			try
			{
				object obj;
				if (items == null)
				{
					obj = null;
				}
				else
				{
					obj = items.GeneratePackets(PacketType.File, 6, null, isSync, null, partition, deliveryRequst: false, "GenerateReadResponse");
				}
				List<Packet> list = (List<Packet>)obj;
				int num2 = list?.Sum((Packet p) => (p?.ToBytes()?.Length).GetValueOrDefault()) ?? 0;
				if (num2 > 20971520)
				{
					throw new ArgumentException($"itemsCount:{items?.Count() ?? (-1)}\n" + $"sending {items?.Count() ?? (-1)}\n" + $"files with len: {num2:N} bytes\n" + $"packestCount:{list.Count}");
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 324);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebFileInfo> items)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.File, 9, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 339);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebFileInfo> infos)
	{
		return infos.GeneratePackets(PacketType.File, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebFileInfo> fileInfo, string moreInfo, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				if (fileInfo == null || !fileInfo.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (fileInfo == null)
				{
					result = null;
				}
				else
				{
					result = fileInfo.GeneratePackets(PacketType.File, 10, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 384);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses, string moreInfo)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.File, 11, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 405);
				throw;
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
				if (this.OnWriteRequest != null)
				{
					this.OnWriteRequest = null;
				}
				if (this.OnCommitRequest != null)
				{
					this.OnCommitRequest = null;
				}
				if (this.OnReadResponse != null)
				{
					this.OnReadResponse = null;
				}
				if (this.OnCommitResponse != null)
				{
					this.OnCommitResponse = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
				if (this.OnDeleteRequest != null)
				{
					this.OnDeleteRequest = null;
				}
				if (this.OnReadRequest != null)
				{
					this.OnReadRequest = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 437);
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			FileInfoActions fileInfoActions = (FileInfoActions)action;
			return fileInfoActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 449);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((FileInfoActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 462);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs"))
		{
			_ = 9;
			try
			{
				switch ((FileInfoActions)pack.PacketActions)
				{
				case FileInfoActions.WriteRequest:
				{
					List<WebFileInfo> data = Json.FromJson<List<WebFileInfo>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 477);
					await Utilities.RaiseEvent(this.OnWriteRequest, this, new ConnectionEventArgs<WebFileInfo>(pack, connectionUniqueId, connection), connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
					checked
					{
						ServerRequests += data?.Count ?? 0;
						break;
					}
				}
				case FileInfoActions.WriteResponse:
				{
					FileWriteResponseEventArgs fileWriteResponseEventArgs = pack.Args[1].FromJson<FileWriteResponseEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 487);
					fileWriteResponseEventArgs.Connection = connection;
					fileWriteResponseEventArgs.ConnectionUniqueId = connectionUniqueId;
					fileWriteResponseEventArgs.PacketReferenceId = pack.PacketReferenceId;
					fileWriteResponseEventArgs.Part = pack.Part;
					fileWriteResponseEventArgs.TotalParts = pack.TotalParts;
					await Utilities.RaiseEvent(this.OnWriteResponse, this, fileWriteResponseEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
					break;
				}
				case FileInfoActions.CommitRequest:
				{
					FileCommitRequestEventArgs fileCommitRequestEventArgs = pack.Args[1].FromJson<FileCommitRequestEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 497);
					fileCommitRequestEventArgs.Connection = connection;
					fileCommitRequestEventArgs.ConnectionUniqueId = connectionUniqueId;
					fileCommitRequestEventArgs.PacketReferenceId = pack.PacketReferenceId;
					fileCommitRequestEventArgs.Part = pack.Part;
					fileCommitRequestEventArgs.TotalParts = pack.TotalParts;
					await Utilities.RaiseEvent(this.OnCommitRequest, this, fileCommitRequestEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
					break;
				}
				case FileInfoActions.CommitResponse:
				{
					FileCommitResponseEventArgs fileCommitResponseEventArgs = pack.Args[1].FromJson<FileCommitResponseEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 506);
					fileCommitResponseEventArgs.Connection = connection;
					fileCommitResponseEventArgs.ConnectionUniqueId = connectionUniqueId;
					fileCommitResponseEventArgs.PacketReferenceId = pack.PacketReferenceId;
					fileCommitResponseEventArgs.Part = pack.Part;
					fileCommitResponseEventArgs.TotalParts = pack.TotalParts;
					await Utilities.RaiseEvent(this.OnCommitResponse, this, fileCommitResponseEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
					break;
				}
				case FileInfoActions.ReadRequest:
				{
					FileReadEventArgs fileReadEventArgs = pack.Args[1].FromJson<FileReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 515);
					fileReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					fileReadEventArgs.Connection = connection;
					fileReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
					fileReadEventArgs.Part = pack.Part;
					fileReadEventArgs.TotalParts = pack.TotalParts;
					await Utilities.RaiseEvent(this.OnReadRequest, this, fileReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
					break;
				}
				case FileInfoActions.ReadResponse:
				{
					ConnectionEventArgs<WebFileInfo> args4 = new ConnectionEventArgs<WebFileInfo>(pack, connectionUniqueId, connection);
					try
					{
						if (!pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnSync, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
						}
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 532);
					}
					break;
				}
				case FileInfoActions.DeleteRequest:
				{
					ConnectionEventArgs<WebFileInfo> args3 = new ConnectionEventArgs<WebFileInfo>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeleteRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 541);
					}
					break;
				}
				case FileInfoActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebFileInfo> args2 = new ConnectionEventArgs<WebFileInfo>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
					}
					break;
				case FileInfoActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs");
					}
					break;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileInfo.cs", 575);
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

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}
}
