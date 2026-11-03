using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Access;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting.UserSettings;

namespace PacketParser.Entities;

[Serializable]
public class WebUserAcc : Serializable<WebUserAcc>, IUserAcc, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, IItemAsyncMapper<WebUserAcc, WebUserAcc>
{
	private string hashedPass;

	private byte[] _safePassword;

	[DefaultValue(0)]
	public short AccountingVersion { get; set; }

	public string Code { get; set; }

	public int BankId1 { get; set; }

	public int BankId2 { get; set; }

	public int BankId3 { get; set; }

	public int BankId4 { get; set; }

	[Obsolete("Use Guid", false)]
	public short Id { get; set; }

	public string Name { get; set; }

	public byte[] SafePassword
	{
		get
		{
			try
			{
				byte[] safePassword = _safePassword;
				if (safePassword == null || !safePassword.Any())
				{
					_safePassword = Pass?.CalcHashSafe(Guid.ToByteArray());
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SafePassword", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 50);
			}
			return _safePassword;
		}
		set
		{
			_safePassword = value;
		}
	}

	[Obsolete("This property is obsolete. Use HashedPass instead.", false)]
	public string Pass { get; set; }

	[Obsolete("This property is obsolete. Use HashedPass instead.", false)]
	public string HashedPass
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(hashedPass) && !string.IsNullOrEmpty(Pass))
				{
					hashedPass = Pass.CalcHashBase64();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "Pass:" + (Pass ?? "null!"), "HashedPass", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 69);
			}
			return hashedPass;
		}
		set
		{
			hashedPass = value;
		}
	}

	public string Description { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime Modified { get; set; } = DateTime.Now;


	public Guid Guid { get; set; }

	public int CusId { get; set; }

	public int VisitorId { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public bool AllVisitorAccess { get; set; }

	[Obsolete("Use Cashier Setting", true)]
	public short LoginAccess { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public bool AllBanksAccess { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public bool AllSandoghsAccess { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public bool AllDepotsAccess { get; set; }

	public short DepotId { get; set; }

	public int SandoghId { get; set; }

	public AccessLevel UserAccess { get; set; }

	[XmlIgnore]
	public List<Tuple<DateTime, DateTime>> UserTime { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public Guid? SandoghGuid { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public Guid? CusGuid { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public Guid? DepotGuid { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public Guid? VisitorGuid { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public Guid? BankGuid1 { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public Guid? BankGuid2 { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public Guid? BankGuid3 { get; set; }

	[Obsolete("Use Cashier Setting", false)]
	public Guid? BankGuid4 { get; set; }

	[Obsolete("Use Cashier Setting", true)]
	public UserSettings Settings { get; set; }

	public List<WebCashier> CashierList { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public event Func<object, ConnectionEventArgs<WebUserAcc>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebUserAcc>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<WebUserAcc>, Task> OnReadResponse;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebUserAcc> userAccs)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs"))
		{
			try
			{
				return userAccs?.GeneratePackets(PacketType.UserAcc, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 121);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.UserAcc, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 135);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebUserAcc> items, int partitionId, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs"))
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
					result = items.GeneratePackets(PacketType.UserAcc, 4, null, isSync, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 150);
				return null;
			}
		}
	}

	public List<WebCashier> GetDefaultCashiers()
	{
		if (CashierList == null)
		{
			List<WebCashier> list2 = (CashierList = new List<WebCashier>());
		}
		try
		{
			if (!CashierList.Any())
			{
				CashierList.Add(new WebCashier
				{
					Guid = Guid,
					Name = Name,
					UserName = Code,
					HashedPassword = SafePassword,
					UserGuid = Guid,
					DateM = DateTime.Now,
					Modified = Modified,
					CashierTime = null,
					Description = Description,
					Status = Status
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDefaultCashiers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 176);
		}
		return CashierList;
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			UserAccActions userAccActions = (UserAccActions)action;
			return userAccActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 188);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((UserAccActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 201);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs"))
		{
			_ = 3;
			try
			{
				switch ((UserAccActions)pack.PacketActions)
				{
				case UserAccActions.SaveRequest:
				{
					if (this.OnSaveRequest == null && this.OnSync == null)
					{
						throw new Exception("there is no eventHandller for WebUserAcc.OnSaveRequest");
					}
					ConnectionEventArgs<WebUserAcc> args = new ConnectionEventArgs<WebUserAcc>(pack, connectionUniqueId, connection);
					if (this.OnSaveRequest != null)
					{
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs");
					}
					if (connection.DepId == 1472)
					{
						WebTelegramMessage.GetNp_Ticket_bot().Sent2web(Json.ToStringJson(args.ConnectionData), null, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 220);
					}
					break;
				}
				case UserAccActions.SaveResponse:
				{
					if (this.OnSaveResponse == null && this.OnSync == null)
					{
						throw new Exception("there is no eventHandller for WebUserAcc.OnSaveResponse");
					}
					SaveResponseEventArgs args3 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs");
					break;
				}
				case UserAccActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						throw new Exception("there is no eventHandller for WebProduct.ReadResponse");
					}
					ConnectionEventArgs<WebUserAcc> args2 = new ConnectionEventArgs<WebUserAcc>(pack, connectionUniqueId, connection);
					try
					{
						if (!pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnSync, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs");
						}
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 237);
					}
					break;
				}
				default:
					throw new Exception($"action({pack.PacketActions}) has not defined!");
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 244);
			}
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
				if (this.OnSaveResponse != null)
				{
					this.OnSaveResponse = null;
				}
				UserAccess = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 262);
		}
	}

	public Task<WebUserAcc> MapAsync(string contextGuid, WebUserAcc source, WebUserAcc destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new WebUserAcc();
			}
			destination.Id = source.Id;
			destination.Code = source.Code;
			destination.BankId1 = source.BankId1;
			destination.BankId2 = source.BankId2;
			destination.BankId3 = source.BankId3;
			destination.BankId4 = source.BankId4;
			destination.Name = source.Name;
			destination.Pass = source.Pass;
			destination.HashedPass = source.HashedPass;
			destination.CusId = source.CusId;
			destination.VisitorId = source.VisitorId;
			destination.DepotId = source.DepotId;
			destination.Description = source.Description;
			destination.SandoghId = source.SandoghId;
			destination.UserAccess = source.UserAccess;
			destination.UserTime = source.UserTime;
			destination.Status = source.Status;
			destination.CashierList = source.CashierList;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.IsDisposed = source.IsDisposed;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebUserAcc.cs", 298);
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebUserAcc>> MapListAsync(string contextGuid, IEnumerable<WebUserAcc> source, List<WebUserAcc> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
