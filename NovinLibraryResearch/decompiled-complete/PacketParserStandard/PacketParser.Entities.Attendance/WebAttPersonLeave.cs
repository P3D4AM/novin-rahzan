using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using PacketParser.EventArgs;

namespace PacketParser.Entities.Attendance;

public class WebAttPersonLeave : Serializable<WebAttPersonLeave>, IAttPersonLeave, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable, IDeliverableEntity, IReceiver
{
	public enum AttPersonLeaveActions
	{
		[PersianNameAttribute.PersianName("نامشخص")]
		Unknown = 0,
		[PersianNameAttribute.PersianName("درخواست ذخیره")]
		SaveRequest = 1,
		[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
		SaveResponse = 2,
		[PersianNameAttribute.PersianName("درخواست خواندن")]
		ReadRequest = 5,
		[PersianNameAttribute.PersianName("سینک")]
		ReadResponse = 6,
		[PersianNameAttribute.PersianName("ریست")]
		Reset = 7,
		[PersianNameAttribute.PersianName("درخواست حذف")]
		DeleteRequest = 8
	}

	private static string apiUrl = HttpHelpers.ApiUrl + "api/Attendance/PersonLeave/";

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	public short DepId { get; set; }

	public Guid Guid { get; set; }

	public Guid PersonGuid { get; set; }

	public string PersonName { get; set; }

	public DateTime RequestDate { get; set; }

	public string Reason { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public string DateFromSH => DateFrom.MiladiToCalture();

	public string DateToSH => DateTo.MiladiToCalture();

	public EnPersonLeaveType Type { get; set; }

	public EnPersonLeaveStatus Status { get; set; }

	public EnPersonLeaveStatus LeaveStatus
	{
		get
		{
			return Status;
		}
		set
		{
			Status = value;
		}
	}

	public EnPersonLeaveKind Kind { get; set; }

	public string ManagerMessage { get; set; }

	public DateTime? ManagerDate { get; set; }

	[DefaultValue(EnPersonLeaveQuiddity.Leave)]
	public EnPersonLeaveQuiddity Quiddity { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public DateTime Modified { get; set; }

	bool IHasStatus.Status
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public event Func<object, HesabKolReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebAttPersonLeave>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebAttPersonLeave>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebAttPersonLeave>, Task> OnSync;

	public static async Task<ReturnedSaveFuncInfo> PostToApiAsync(WebAttPersonLeave personInfo, string depUser_UserName, string depUser_Password, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			LoginInfo loginInfo = new LoginInfo
			{
				UserName = depUser_UserName,
				Password = depUser_Password,
				Source = ENSource.TimeAttendance
			};
			return await HttpHelpers.PostToApiAsync<WebAttPersonLeave, WebAttPersonLeave>(personInfo, apiUrl + "SaveBytes", loginInfo, null, 15000, 5, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 124);
			res.AddReturnedValue(ex, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 125);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteAsync(Guid guid, string depUser_UserName, string depUser_Password, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			LoginInfo loginInfo = new LoginInfo
			{
				UserName = depUser_UserName,
				Password = depUser_Password,
				Source = ENSource.TimeAttendance
			};
			return await HttpHelpers.PostToApiAsync<bool, Guid>(guid, apiUrl + "DeleteQuiddity", loginInfo, null, 15000, 5, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 145);
			res.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 146);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>>> GetAllAsync(Guid? personGuid, Guid? adminGuid, string depUserName, string depUser_UserName, string depUser_Password, EnPersonLeaveStatus? status = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>> result = new ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(ENSource.TimeAttendance, depUser_UserName, depUser_Password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 156);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{
					"Authorization",
					(returnedSaveFuncInfoWithValue.value ?? "") ?? ""
				},
				{
					"adminGuid",
					$"{adminGuid}"
				}
			};
			if ((personGuid ?? Guid.Empty) != Guid.Empty)
			{
				dictionary.Add("PersonGuid", personGuid?.ToString() ?? "");
			}
			if (status.HasValue)
			{
				dictionary.Add("Status", ((short)status.Value).ToString());
			}
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAttPersonLeave>>> returnedSaveFuncInfoWithValue2 = await (apiUrl + "GetAll").GetFromApi<List<WebAttPersonLeave>>(token, 3, dictionary);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 166);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 172);
			result.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 173);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>>> GetAllQuiddityListAsync(Guid? personGuid, string depUser_UserName, string depUser_Password, EnPersonLeaveQuiddity? status = null, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>> result = new ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(ENSource.TimeAttendance, depUser_UserName, depUser_Password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllQuiddityListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 183);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue.value ?? "") ?? ""
			} };
			if ((personGuid ?? Guid.Empty) != Guid.Empty)
			{
				dictionary.Add("PersonGuid", personGuid?.ToString() ?? "");
			}
			if (status.HasValue)
			{
				dictionary.Add("Status", ((short)status.Value).ToString());
			}
			if (fromDate.HasValue)
			{
				dictionary.Add("fromDate", fromDate.ToString());
			}
			if (toDate.HasValue)
			{
				dictionary.Add("toDate", toDate.ToString());
			}
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAttPersonLeave>>> returnedSaveFuncInfoWithValue2 = await (apiUrl + "GetAllQuiddity").GetFromApi<List<WebAttPersonLeave>>(token, 3, dictionary);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAllQuiddityListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 194);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllQuiddityListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 200);
			result.AddReturnedValue(ex, "", "GetAllQuiddityListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 201);
		}
		return result;
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebAttPersonLeave> list, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs"))
		{
			try
			{
				if (list == null || !list.Any())
				{
					throw new NullReferenceException();
				}
				string text = list.Select((WebAttPersonLeave p) => $"{p.Guid}/{p.PersonGuid}").GenerateDebugDescription(5);
				object result;
				if (list == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = list.GeneratePackets(PacketType.AttPersonLeave, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 216);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses, Guid? packetRefrenceId)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs"))
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
					result = responses.GeneratePackets(PacketType.AttPersonLeave, 2, null, isSync: false, packetRefrenceId, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 231);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebAttPersonLeave> items, bool isSync, int partitionId = 0)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs"))
		{
			try
			{
				if (items == null || items.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.AttPersonLeave, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 246);
				return null;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 263);
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs"))
		{
			_ = 4;
			try
			{
				switch ((AttPersonLeaveActions)pack.PacketActions)
				{
				case AttPersonLeaveActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebAttPersonLeave> args2 = new ConnectionEventArgs<WebAttPersonLeave>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs");
					}
					break;
				case AttPersonLeaveActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args3 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs");
					}
					break;
				case AttPersonLeaveActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						HesabKolReadEventArgs hesabKolReadEventArgs = pack.Args[1].FromJson<HesabKolReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 285);
						hesabKolReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						hesabKolReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, hesabKolReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs");
					}
					break;
				case AttPersonLeaveActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebAttPersonLeave> args = new ConnectionEventArgs<WebAttPersonLeave>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						if (this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs");
						}
						else if (this.OnReadResponse != null)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs");
						}
					}
					break;
				}
				case (AttPersonLeaveActions)3:
				case (AttPersonLeaveActions)4:
					break;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 301);
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		return ((AttPersonLeaveActions)action).GetDisplay();
	}

	public string GetPacketActionEnumName(short action)
	{
		AttPersonLeaveActions attPersonLeaveActions = (AttPersonLeaveActions)action;
		return attPersonLeaveActions.ToString();
	}

	public static async Task<ReturnedSaveFuncInfo> CheckOverlappAsync(WebAttPersonLeave item, string depUserName, string depUser_UserName, string depUser_Password, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (item.Quiddity != 0)
			{
				return res;
			}
			if (item.Type == EnPersonLeaveType.Daily)
			{
				item.DateTo = new DateTime(item.DateTo.Year, item.DateTo.Month, item.DateTo.Day, 23, 59, 59);
			}
			ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>> returnedSaveFuncInfoWithValue = await GetAllAsync(item.PersonGuid, null, depUserName, depUser_UserName, depUser_Password);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckOverlappAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 315);
			if (res.HasError)
			{
				return res;
			}
			List<WebAttPersonLeave> list = returnedSaveFuncInfoWithValue?.value?.Where((WebAttPersonLeave q) => q.Status == EnPersonLeaveStatus.Pending || q.Status == EnPersonLeaveStatus.Accepted || q.Status == EnPersonLeaveStatus.AcceptedByAdmin || q.Status == EnPersonLeaveStatus.PendingForAdsmin)?.ToList();
			if (list != null && list.Any())
			{
				List<WebAttPersonLeave> list2 = new List<WebAttPersonLeave>();
				foreach (WebAttPersonLeave item2 in list)
				{
					if ((item.DateFrom >= item2.DateFrom && item.DateFrom <= item2.DateTo) || (item.DateTo >= item2.DateFrom && item.DateTo <= item2.DateTo) || (item.DateFrom <= item2.DateFrom && item.DateTo >= item2.DateTo))
					{
						list2.Add(item2);
					}
				}
				if (list2 != null && list2.Any())
				{
					await res.AddErrorAsync("مرخصی جاری با مرخصی دیگر در همان بازه همپوشانی دارد", "", "CheckOverlappAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 329);
				}
			}
			if (res.HasError)
			{
				return res;
			}
			ReturnedSaveFuncInfoWithValue<List<WebAttTempVacations>> returnedSaveFuncInfoWithValue2 = await WebAttTempVacations.GetAllAsync(item.PersonGuid, depUser_UserName, depUser_Password, token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "CheckOverlappAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 334);
			if (res.HasError)
			{
				return res;
			}
			List<WebAttTempVacations> list3 = returnedSaveFuncInfoWithValue2?.value;
			if (list3 != null && list3.Any())
			{
				List<WebAttTempVacations> list4 = new List<WebAttTempVacations>();
				foreach (WebAttTempVacations item3 in list3)
				{
					if ((item.DateFrom >= item3.DateFrom && item.DateFrom <= item3.DateTo) || (item.DateTo >= item3.DateFrom && item.DateTo <= item3.DateTo) || (item.DateFrom <= item3.DateFrom && item.DateTo >= item3.DateTo))
					{
						list4.Add(item3);
					}
				}
				if (list4 != null && list4.Any())
				{
					await res.AddErrorAsync("مرخصی جاری با مرخصی دیگر در همان بازه همپوشانی دارد", "", "CheckOverlappAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 345);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckOverlappAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 350);
			res.AddReturnedValue(ex, "", "CheckOverlappAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 351);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>>> GetAllOverlappsAsync(DateTime dateFrom, DateTime dateTo, Guid? adminGuid, string depUser_UserName, string depUser_Password, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>> result = new ReturnedSaveFuncInfoWithValue<List<WebAttPersonLeave>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(ENSource.TimeAttendance, depUser_UserName, depUser_Password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllOverlappsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 361);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"Authorization",
					(returnedSaveFuncInfoWithValue.value ?? "") ?? ""
				},
				{
					"adminGuid",
					$"{adminGuid}"
				},
				{
					"dateFrom",
					$"{dateFrom}"
				},
				{
					"dateTo",
					$"{dateTo}"
				}
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAttPersonLeave>>> returnedSaveFuncInfoWithValue2 = await (apiUrl + "GetAllOverlapps").GetFromApi<List<WebAttPersonLeave>>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAllOverlappsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 372);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue2.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllOverlappsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 378);
			result.AddReturnedValue(ex, "", "GetAllOverlappsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttPersonLeave.cs", 379);
		}
		return result;
	}
}
