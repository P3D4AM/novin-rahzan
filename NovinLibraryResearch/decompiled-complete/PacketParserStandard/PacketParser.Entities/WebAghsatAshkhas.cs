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
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebAghsatAshkhas : Serializable<WebAghsatAshkhas>, IAghsatAshkhas, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	private static string apiUrl = HttpHelpers.ApiUrl + "Department/Customer/Aghsat/";

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public int Id2 { get; set; }

	[Obsolete("")]
	public Guid TafsilGuid { get; set; }

	[Obsolete("")]
	public Guid MoeinGuid { get; set; }

	[Obsolete("", true)]
	public HesabType HType { get; set; }

	public string Description { get; set; }

	public DateTime DateSarResid { get; set; }

	public DateTime Date { get; set; }

	[Obsolete("")]
	public Guid UserGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public decimal Price { get; set; }

	public DateTime? ReceptionDateM { get; set; }

	public decimal ReceptionSum { get; set; }

	public string ReceptionDescription { get; set; }

	[JsonIgnore]
	public string ReceptionDateSh
	{
		get
		{
			if (ReceptionDateM.HasValue)
			{
				return ReceptionDateM.Value.MiladiToCalture();
			}
			return "";
		}
	}

	[Obsolete("", true)]
	public bool Checked { get; set; }

	public int ReceptionId2 { get; set; }

	public string UserName { get; set; }

	public string FullCode { get; set; }

	public string Code { get; set; }

	public string FullName { get; set; }

	public string ReceptionSanadNo { get; set; }

	public string DateSh { get; set; }

	public string DateSarResidSh { get; set; }

	public int TypePardakht { get; set; }

	public string Mounth { get; set; }

	[Obsolete("", true)]
	public int Count { get; set; }

	public Guid GuidGroup { get; set; }

	[Obsolete("", true)]
	public decimal PriceMande { get; set; }

	[Obsolete("", true)]
	public decimal PriceSum { get; set; }

	public DateTime ResetTime { get; set; }

	public AghsatMMetaData MetaData { get; set; }

	public EnRasGiriType RasGiriType { get; set; }

	public decimal VajhEltezam { get; set; }

	public DateTime? VajhEltezamModified { get; set; }

	public DateTime? DateUtc { get; set; }

	public int? SanadNo { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public Guid? CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, AghsatAshkhasReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebAghsatAshkhas>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebAghsatAshkhas>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebAghsatAshkhas>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeletedRequest;

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> aghsats, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs"))
		{
			try
			{
				if (aghsats == null || aghsats.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				object result;
				if (aghsats == null)
				{
					result = null;
				}
				else
				{
					result = aghsats.GeneratePackets(PacketType.AghsatAshkhas, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 92);
				throw;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebAghsatAshkhas> aghsats, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs"))
		{
			try
			{
				if (aghsats == null || aghsats.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				object result;
				if (aghsats == null)
				{
					result = null;
				}
				else
				{
					result = aghsats.GeneratePackets(PacketType.AghsatAshkhas, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 106);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(int requestedUId, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				AghsatAshkhasReadEventArgs aghsatAshkhasReadEventArgs = new AghsatAshkhasReadEventArgs
				{
					Modified = modified
				};
				list.Add(aghsatAshkhasReadEventArgs.ToString());
				return new Packet(PacketType.AghsatAshkhas, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 122);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebAghsatAshkhas> items, int partitionId, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs"))
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
					result = items.GeneratePackets(PacketType.AghsatAshkhas, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 136);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebAghsatAshkhas> aghsats, int partitionId, string debuggerInfo)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs"))
		{
			try
			{
				if (aghsats == null || !aghsats.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (aghsats == null)
				{
					result = null;
				}
				else
				{
					result = aghsats.GeneratePackets(PacketType.AghsatAshkhas, 1, null, isSync: false, null, partitionId, deliveryRequst: false, debuggerInfo);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 150);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.AghsatAshkhas, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 164);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebAghsatAshkhas> aghsats)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs"))
		{
			try
			{
				if (aghsats == null || aghsats.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return aghsats.GeneratePackets(PacketType.AghsatAshkhas, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 178);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		AghsatAshkhasActions aghsatAshkhasActions = (AghsatAshkhasActions)action;
		return aghsatAshkhasActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((AghsatAshkhasActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs"))
		{
			_ = 5;
			try
			{
				switch ((AghsatAshkhasActions)pack.PacketActions)
				{
				case AghsatAshkhasActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						pack.ExteraDataForDebug += "/ Received Packet !!!!";
						ConnectionEventArgs<WebAghsatAshkhas> args5 = new ConnectionEventArgs<WebAghsatAshkhas>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args5, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs");
					}
					break;
				case AghsatAshkhasActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs");
					}
					break;
				case AghsatAshkhasActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						AghsatAshkhasReadEventArgs args2 = new AghsatAshkhasReadEventArgs
						{
							ConnectionUniqueId = connectionUniqueId,
							Connection = connection
						};
						await Utilities.RaiseEvent(this.OnReadRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs");
					}
					break;
				case AghsatAshkhasActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebAghsatAshkhas> args3 = new ConnectionEventArgs<WebAghsatAshkhas>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs");
						}
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 222);
						break;
					}
				}
				case AghsatAshkhasActions.DeleteRequest:
				{
					if (this.OnDeletedRequest == null)
					{
						break;
					}
					ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeletedRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 231);
						break;
					}
				}
				case (AghsatAshkhasActions)3:
				case (AghsatAshkhasActions)4:
				case AghsatAshkhasActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 237);
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAghsatAshkhas>>> GetAllFromServer(Guid userGuid, string depUser, string password, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebAghsatAshkhas>> result = new ReturnedSaveFuncInfoWithValue<List<WebAghsatAshkhas>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await DepClient.GetAsync(depUser, password, token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 245);
			if (result.HasError)
			{
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value == null || returnedSaveFuncInfoWithValue.value.Data == null)
			{
				result.AddError("دپارتمان مورد نظر معتبر نمی باشد", "GetAllFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 249);
				return result;
			}
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = await new RestTokenManagement(ENSource.Accounting, depUser, password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAllFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 253);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> obj = new Dictionary<string, string> { 
			{
				"guid",
				userGuid.ToString()
			} };
			obj.Add("Authorization", (returnedSaveFuncInfoWithValue2.value ?? "") ?? "");
			Dictionary<string, string> headers = obj;
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAghsatAshkhas>>> returnedSaveFuncInfoWithValue3 = await (apiUrl + "GetAllAghsatAsync").GetFromApi<List<WebAghsatAshkhas>>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "GetAllFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 261);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue3.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 267);
			result.AddReturnedValue(ex, "", "GetAllFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 268);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebAghsatAshkhas>> GetGhsetFromServer(Guid guid, string depUser, string password, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebAghsatAshkhas> result = new ReturnedSaveFuncInfoWithValue<WebAghsatAshkhas>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await DepClient.GetAsync(depUser, password, token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetGhsetFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 278);
			if (result.HasError)
			{
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value == null || returnedSaveFuncInfoWithValue.value.Data == null)
			{
				result.AddError("دپارتمان مورد نظر معتبر نمی باشد", "GetGhsetFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 282);
				return result;
			}
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = await new RestTokenManagement(ENSource.Accounting, depUser, password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetGhsetFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 286);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> obj = new Dictionary<string, string> { 
			{
				"guid",
				guid.ToString()
			} };
			obj.Add("Authorization", (returnedSaveFuncInfoWithValue2.value ?? "") ?? "");
			Dictionary<string, string> headers = obj;
			ReturnedSaveFuncInfoWithValue<WebResponse<WebAghsatAshkhas>> returnedSaveFuncInfoWithValue3 = await (apiUrl + "GetGhestAsync").GetFromApi<WebAghsatAshkhas>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "GetGhsetFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 294);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue3.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGhsetFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 300);
			result.AddReturnedValue(ex, "", "GetGhsetFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 301);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAghsatAshkhas>>>> SendData2SaveAsync(string depUser, string password, List<WebAghsatAshkhas> lst, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAghsatAshkhas>>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAghsatAshkhas>>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await DepClient.GetAsync(depUser, password, token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SendData2SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 311);
			if (result.HasError)
			{
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value == null || returnedSaveFuncInfoWithValue.value.Data == null)
			{
				result.AddError("دپارتمان مورد نظر معتبر نمی باشد", "SendData2SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 315);
				return result;
			}
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = await new RestTokenManagement(ENSource.Accounting, depUser, password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "SendData2SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 319);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue2.value ?? "") ?? ""
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAghsatAshkhas>>> returnedSaveFuncInfoWithValue3 = await HttpHelpers.PostToApi<List<WebAghsatAshkhas>, List<WebAghsatAshkhas>>(lst, apiUrl + "UpdatghestAsync", headers, strongDeserialize: false, 15000, 3);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "SendData2SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 323);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue3.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendData2SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 329);
			result.AddReturnedValue(ex, "", "SendData2SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 330);
		}
		return result;
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
				if (this.OnDeletedRequest != null)
				{
					this.OnDeletedRequest = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebAghsatAshkhas.cs", 348);
		}
	}
}
