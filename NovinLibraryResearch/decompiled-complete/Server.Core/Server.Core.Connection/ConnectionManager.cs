using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.TaxService;
using Serilog;
using Server.Core.Assistance;

namespace Server.Core.Connection;

public class ConnectionManager : IConnectionManager
{
	private ConcurrentDictionary<string, ClsConnectionData> Guid2ConnectionData = new ConcurrentDictionary<string, ClsConnectionData>();

	private ConcurrentDictionary<short, ConnectionDataAggregator> DepId2ConnectionInfo = new ConcurrentDictionary<short, ConnectionDataAggregator>();

	public static ConnectionManager Instance { get; } = new ConnectionManager();


	ConnectionData IConnectionManager.GetConnection(string connectionUniqueId)
	{
		return GetConnection(connectionUniqueId);
	}

	public ClsConnectionData GetConnection(string connectionUniqueId)
	{
		int num = 0;
		try
		{
			if (string.IsNullOrEmpty(connectionUniqueId))
			{
				return null;
			}
			if (Guid2ConnectionData == null)
			{
				Guid2ConnectionData = new ConcurrentDictionary<string, ClsConnectionData>();
			}
			num = 1;
			Guid2ConnectionData.TryGetValue(connectionUniqueId, out var value);
			num = 2;
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"GetConnection(string conneGuid)//Line:{num}", "GetConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 40);
			return null;
		}
	}

	public ClsConnectionData GetConnectionsData(short depId, ENSource enSource)
	{
		using (new JaegerMonitor("GetConnectionsData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs"))
		{
			try
			{
				return GetConnectionsData(depId)?.FirstOrDefault((ClsConnectionData w) => w != null && w.Source == enSource);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetConnectionsData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 55);
				return null;
			}
		}
	}

	public ClsConnectionData GetConnection(short DepId, int HashId)
	{
		int num = 0;
		try
		{
			num = 1;
			string text = GetGuids(DepId, HashId)?.FirstOrDefault();
			num = 2;
			if (text == null)
			{
				return null;
			}
			num = 3;
			return GetConnection(text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"GetConnection(short DepId, int HashId)//Line:{num}", "GetConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 75);
			return null;
		}
	}

	public ClsConnectionData GetAccountingConnection(short DepId)
	{
		using (new JaegerMonitor("GetAccountingConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs"))
		{
			try
			{
				DepId2ConnectionInfo.TryGetValue(DepId, out var value);
				return value?.Connections?.Where((ClsConnectionData p) => p != null && p.Source == ENSource.Accounting)?.FirstOrDefault();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAccountingConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 92);
				return null;
			}
		}
	}

	public ClsConnectionData GetAttendanceConnection(short DepId)
	{
		using (new JaegerMonitor("GetAttendanceConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs"))
		{
			try
			{
				DepId2ConnectionInfo.TryGetValue(DepId, out var value);
				return value?.Connections?.Where((ClsConnectionData p) => p != null && p.Source == ENSource.TimeAttendance)?.FirstOrDefault();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAttendanceConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 110);
				return null;
			}
		}
	}

	public async Task UnhandleAnotherServerAllConnectionsAsync(string ServerConnectionId)
	{
		int line = 0;
		try
		{
			line = 1;
			IEnumerable<ClsConnectionData> enumerable = Guid2ConnectionData.Values?.Where((ClsConnectionData p) => p.Context.Guid == ServerConnectionId);
			line = 2;
			if (enumerable != null && enumerable.Any())
			{
				line = 3;
				List<Task> list = new List<Task>();
				line = 4;
				foreach (ClsConnectionData connection in enumerable?.ToList())
				{
					line = 5;
					list.Add(Task.Run(delegate
					{
						connection?.Dispose("UnhandleAnotherServerAllConnectionsAsync");
					}));
					line = 6;
				}
				line = 7;
				await Task.WhenAll(list);
			}
			line = 10;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"line:{line}", "UnhandleAnotherServerAllConnectionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 141);
		}
	}

	public List<ClsConnectionData> GetAllConnectionsData()
	{
		try
		{
			if (DepId2ConnectionInfo == null)
			{
				throw new Exception("DepId2ConnectionInfo is null");
			}
			ICollection<ConnectionDataAggregator> values = DepId2ConnectionInfo.Values;
			List<ClsConnectionData> list = new List<ClsConnectionData>();
			if (values == null)
			{
				throw new Exception("ConnectionDataAggregator is null");
			}
			foreach (ConnectionDataAggregator item in values)
			{
				if (item.Connections != null)
				{
					list.AddRange(item.Connections);
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllConnectionsData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 162);
			return null;
		}
	}

	public async Task UnHandleAnotherServerConnectionAsync(string ServerConnectionId, string connectionUniqueId)
	{
		ClsConnectionData connection = GetConnection(connectionUniqueId);
		if (connection != null)
		{
			connection?.Dispose("****UnHandleAnotherServerConnectionAsync");
		}
	}

	public async Task HandleAnotherServerConnectionAsync(string connectionUniqueId, ConnectionData connection, ITaxConnectionManager taxManager)
	{
		_ = 2;
		try
		{
			if (connection.Source == ENSource.WordPressDebug)
			{
				return;
			}
			ClsConnectionData connection2 = GetConnection(connection.Guid);
			if (connection2 != null)
			{
				_ = connection2.IsOwnedConnection;
				return;
			}
			int maxDelay = 100;
			while (Initializer.Dal == null)
			{
				maxDelay--;
				if (maxDelay <= 0)
				{
					WebErrorLog.Instance.StartLog("Server.Core.Assistance.Initializer.Dal is null after 100 seconds!", "", "HandleAnotherServerConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 197);
					return;
				}
				await Task.Delay(1000);
			}
			ClsConnectionData clsConnectionData = await ClsConnectionData.GetConnectionAsync(connection, await Initializer.Dal.User.LoadDataAsync(connection.UserId), taxManager);
			connection.Context = WebContextHolder.GetWebContextHolder(connection.Context.Guid);
			DepId2ConnectionInfo.TryGetValue(connection.DepId, out var value);
			if (value == null)
			{
				value = new ConnectionDataAggregator();
				DepId2ConnectionInfo.TryRemove(connection.DepId, out var _);
				DepId2ConnectionInfo.TryAdd(connection.DepId, value);
			}
			value.Add(clsConnectionData, connection.Guid);
			Guid2ConnectionData.TryAdd(connection.Guid, clsConnectionData);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleAnotherServerConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 216);
		}
	}

	public async Task HandleAnotherServerConnectionsAsync(string connectionUniqueId, List<ConnectionData> connections, ITaxConnectionManager taxManager)
	{
		int line = 0;
		try
		{
			line = 1;
			List<Task> list = new List<Task>();
			line = 2;
			WebContextHolder webContextHolder = null;
			line = 3;
			if (connections != null && connections.Any())
			{
				foreach (ConnectionData connection in connections)
				{
					line = 10;
					if (connection != null)
					{
						line = 12;
						if (connection?.Context == null)
						{
							WebErrorLog.Instance.StartLog("context is null!!", "", "HandleAnotherServerConnectionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 236);
						}
						line = 13;
						webContextHolder = webContextHolder ?? WebContextHolder.GetWebContextHolder(connection.Context.Guid);
						line = 15;
						list.Add(HandleAnotherServerConnectionAsync(connectionUniqueId, connection, taxManager));
						line = 20;
					}
				}
			}
			line = 25;
			await Task.WhenAll(list);
			line = 30;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"line:{line}", "HandleAnotherServerConnectionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 249);
		}
	}

	public List<ClsConnectionData> GetConnectionsData(short DepId)
	{
		try
		{
			if (DepId2ConnectionInfo == null)
			{
				return null;
			}
			DepId2ConnectionInfo.TryGetValue(DepId, out var value);
			if (value?.Connections == null)
			{
				return null;
			}
			return value?.Connections?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetConnectionsData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 264);
			return null;
		}
	}

	public List<string> GetGuids(short DepId)
	{
		try
		{
			DepId2ConnectionInfo.TryGetValue(DepId, out var value);
			return value?.GetGuids();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGuids", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 278);
			return null;
		}
	}

	public List<string> GetGuids(ENSource Source)
	{
		try
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<short, ConnectionDataAggregator> item in DepId2ConnectionInfo?.ToList())
			{
				list.AddRange(item.Value?.GetConnections(ENSource.Programmer));
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGuids", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 294);
			return null;
		}
	}

	public List<string> GetGuids(short DepId, ENSource Source)
	{
		try
		{
			DepId2ConnectionInfo.TryGetValue(DepId, out var value);
			return value?.GetGuids(Source);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGuids", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 309);
			return null;
		}
	}

	public List<string> GetGuids(short DepId, int hashId)
	{
		try
		{
			DepId2ConnectionInfo.TryGetValue(DepId, out var value);
			return value?.GetGuids(hashId);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGuids", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 324);
			return null;
		}
	}

	public bool Contains(string guid)
	{
		try
		{
			return Guid2ConnectionData.ContainsKey(guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Contains", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 337);
			return false;
		}
	}

	public Task UnHandleConnection(ClsConnectionData connection, string Reason, ENSource source)
	{
		int num = 0;
		try
		{
			if (connection == null)
			{
				return Task.CompletedTask;
			}
			ServerMessaging.UnHandledConnection(connection.Guid);
			if (Initializer.Dal.ServerLog != null)
			{
				Initializer.Dal.ServerLog.InsertLogAsync(connection?.Ip ?? "", 0, 1, "UnHandleConnection_" + Reason + "_" + connection, default(CancellationToken), 10);
			}
			Initializer.Dal.ClientLoginHistory?.InsertDisconnectTime(connection?.Guid ?? "", DateTime.Now, Reason);
			ConnectionDataAggregator value = null;
			num = 3;
			if (connection != null)
			{
				DepId2ConnectionInfo.TryGetValue(connection.DepId, out value);
			}
			num = 5;
			num = 6;
			if (connection != null && value != null)
			{
				value?.UnHandleConnection(connection, Reason);
			}
			num = 10;
			num = 11;
			if (connection != null)
			{
				Guid2ConnectionData.TryRemove(connection.Guid, out var _);
			}
			num = 15;
			num = 20;
			if (connection == null)
			{
				num = 21;
				connection?.Dispose("ConnectionManager_Connection is null");
				connection = null;
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"UnHandleConnection(string ConnectionUniqueId) Line:{num}", "UnHandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 383);
		}
		return Task.CompletedTask;
	}

	public async Task UnHandleConnection(short DepId, int HashId, string reason, ENSource source)
	{
		try
		{
			ClsConnectionData clsConnectionData = GetConnectionsData(DepId)?.Where((ClsConnectionData P) => P != null && P.HashId == HashId)?.FirstOrDefault();
			if (clsConnectionData != null)
			{
				await UnHandleConnection(clsConnectionData, reason, source);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "UnHandleConnection(short DepId, int HashId)", "UnHandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 397);
		}
	}

	public async Task<ClsConnectionData> HandleConnection(WebContextHolder context, string ConnectionUniqueId, short DepId, LoginExtention LoginRequest, WebUserInformation LoginResponse, string Ip, ITaxConnectionManager taxManager)
	{
		CancellationToken token = default(CancellationToken);
		int line = 1;
		try
		{
			line = 2;
			line = 3;
			if (Initializer.Dal.ServerLog != null)
			{
				Initializer.Dal.ServerLog.InsertLogAsync(Ip ?? "", 0, 0, string.Format("HandleConnection DepId:{0}//Ip:{1}//result:{2}//request:{3}", DepId, Ip ?? "", LoginResponse?.ST, LoginRequest?.ToString() ?? ""), default(CancellationToken), 10);
			}
			line = 4;
			if (LoginResponse == null)
			{
				throw new NullReferenceException("LoginResponse");
			}
			line = 5;
			byte[] hashData = await CalcConnectionHash.CalcHashDataAsync("HandleConnection", LoginResponse.UserId, LoginRequest.DbGuid, LoginResponse.HesabTreeId, LoginRequest.AndroidIme, LoginRequest.LkSerial, LoginRequest.CpuSerial, LoginRequest.CashierGuid, LoginRequest.ClientContextGuid, default(CancellationToken));
			line = 6;
			int HashId = await Initializer.Dal.Sync.GetHashIdAsync(hashData, default(CancellationToken));
			line = 7;
			await UnHandleConnection(DepId, HashId, "HandleConnection then unhandled old connections", LoginRequest?.Source ?? ENSource.None);
			line = 8;
			if (HashId <= 0)
			{
				line = 9;
				HashId = await Initializer.Dal.Sync.InsertHashAsync(hashData, default(CancellationToken));
				line = 10;
				await Initializer.Dal.Sync.InsertFisrtSyncAsync(HashId, DepId, LoginResponse.UserId, 0, LoginRequest.LkSerial, LoginRequest.AndroidIme, LoginRequest.DbGuid, default(CancellationToken));
				line = 11;
			}
			line = 12;
			DepId2ConnectionInfo.TryGetValue(DepId, out var depConnections);
			line = 13;
			if (depConnections == null)
			{
				line = 14;
				depConnections = new ConnectionDataAggregator();
				line = 15;
				DepId2ConnectionInfo.TryRemove(DepId, out var _);
				line = 16;
				DepId2ConnectionInfo.TryAdd(DepId, depConnections);
				line = 17;
			}
			line = 18;
			ReturnedSaveFuncInfoWithValue<Sync> returnedSaveFuncInfoWithValue = await Initializer.Dal.Sync.LoadDataAsync(HashId, token);
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			ClsConnectionData clsConnectionData = await ClsConnectionData.FactoryAsync(context, ConnectionUniqueId, LoginRequest, LoginResponse, HashId, Ip, await Initializer.Dal.User.LoadDataAsync(returnedSaveFuncInfoWithValue.value.UserId), taxManager);
			line = 19;
			depConnections.Add(clsConnectionData, ConnectionUniqueId);
			line = 20;
			Guid2ConnectionData.TryAdd(ConnectionUniqueId, clsConnectionData);
			line = 21;
			ServerMessaging.HandledConnection(clsConnectionData);
			line = 22;
			return clsConnectionData;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}//Request:{LoginRequest}", "HandleConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 479);
			return null;
		}
	}

	public bool IsOnline(short DepId, long hashId)
	{
		try
		{
			DepId2ConnectionInfo.TryGetValue(DepId, out var value);
			return value?.IsOnline(hashId) ?? false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsOnline", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 494);
			return false;
		}
	}

	public async Task<LoginExtention> ProcessLoginAsync(LoginRequestEventArgs e, CancellationToken token)
	{
		int line = 0;
		using (new JaegerMonitor("ProcessLoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs"))
		{
			LoginExtention extension = null;
			LogLevel minLevel = LogLevel.Warning;
			try
			{
				line = 1;
				if (e == null || e.ConnectionData == null)
				{
					return null;
				}
				if (e.ConnectionData.DbGuid == Guid.Empty)
				{
					e.ConnectionData.DbGuid = Guid.Parse("123e4567-e89b-12d3-a456-426655440000");
				}
				line = 50;
				if (string.IsNullOrEmpty(e.ConnectionData.Username?.Trim()) || string.IsNullOrEmpty(e.ConnectionData.Password?.Trim()))
				{
					return Status_NotExists(e, out extension);
				}
				line = 53;
				WebUser user = await Initializer.Dal.User.LoadDataAsync(e.ConnectionData.Username, e.ConnectionData.Password);
				if ((user?.Id ?? 0) > 0 && Initializer.Dal.Sync != null)
				{
					byte[] hashData = await CalcConnectionHash.CalcHashDataAsync("ProcessLoginAsync", user?.Id ?? 0, e.ConnectionData.DbGuid, 0, e.ConnectionData.AndroidIme, e.ConnectionData.LkSerial, e.ConnectionData.CpuSerial, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid, token);
					line = 3;
					int num = await Initializer.Dal.Sync.GetHashIdAsync(hashData, token);
					minLevel = ((await Initializer.Dal.Sync.LoadDataAsync(num, token))?.value)?.MinLogLevel ?? LogLevel.Warning;
				}
				line = 10;
				if (user == null || user.DepId <= 0 || user.Id <= 0 || !user.Status)
				{
					return Status_NotExists(e, out extension);
				}
				line = 15;
				if (user == null || !user.Status)
				{
					return Status_DeletedUser(e, out extension, user);
				}
				line = 20;
				if (string.IsNullOrEmpty(user.PC_Serial) && !string.IsNullOrEmpty(e.ConnectionData.CpuSerial))
				{
					user.PC_Serial = e.ConnectionData.CpuSerial;
					await Initializer.Dal.User.SetPcSerialAsync((short)user.Id, e.ConnectionData.CpuSerial);
				}
				if (e.ConnectionData.Source != user.Type)
				{
					if (user.Type != 0)
					{
						return Status_SourceMismatch(e, out extension, user);
					}
					user.Type = e.ConnectionData.Source;
					await Initializer.Dal.User.SaveAsync(user.DepId, user, "ProcessLoginAsync");
				}
				line = 40;
				if (user.ExpireDate < DateTime.Now)
				{
					return Status_UserExpired(e, out extension, user);
				}
				line = 450;
				if (user == null)
				{
					throw new NullReferenceException("User is null!!");
				}
				line = 451;
				WebDepartment dep = await Initializer.Dal.Department.LoadDataAsync(user.DepId);
				if (dep.IsAccountingVerion6 && e.ConnectionData.ClientVersion <= 8 && e.ConnectionData.Source != ENSource.NPBackupService)
				{
					extension = new LoginExtention(EnLoginResult.WebServiceParametersMismatch, $"dep is version6 but client is not updated!//User.DepId:{user?.DepId ?? (-1)}", null, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
					WebTelegramMessage.ContactUs_bot().Sent2web("همکار گرامی\r\nکاربر وب سرویس(" + user.Name + ")\r\n" + $"از دپارتمان ({dep.Id}-{dep.Name})\r\n" + "به علت استفاده از ورژن قدیمی لاگین نشد.\r\nحسابداری این مجموعه به ورژن 6 ارتقاء پیدا کرده و باید الزاما تمامی کلاینت های این مجموعه نیز ورژن6 شوند.", null, "ProcessLoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 564);
					return extension;
				}
				line = 452;
				if (dep == null)
				{
					extension = new LoginExtention(EnLoginResult.ServerError, $"dep is null//User.DepId:{user?.DepId ?? (-1)}", null, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
					return extension;
				}
				line = 453;
				if (dep.StTaeed == EnDepartmentStatus.NotVerified)
				{
					return Status_DepartmentNotVerified(e, out extension, dep);
				}
				line = 50;
				if (dep.DateExpire < DateTime.Now)
				{
					return Status_DepartmentExpired(e, out extension, dep);
				}
				line = 55;
				if (dep == null || !dep.Status)
				{
					return Status_DepartmentDeleted(e, out extension, dep);
				}
				line = 60;
				if (user.Type == ENSource.None && e.ConnectionData.AndroidIme != 0L && e.ConnectionData.Source == ENSource.Android_HesabYar)
				{
					user.Type = ENSource.Android_HesabYar;
					await Initializer.Dal.User.SaveAsync(user.DepId, user, "ProcessLoginAsync2");
				}
				line = 65;
				if (e.ConnectionData.CpuSerial != "05137597590" && user.Type == ENSource.Baje && !string.IsNullOrEmpty(e.ConnectionData.CpuSerial))
				{
					if (string.IsNullOrEmpty(user.PC_Serial))
					{
						user.PC_Serial = e.ConnectionData.CpuSerial;
						await Initializer.Dal.User.SetPcSerialAsync(dep.Id, e.ConnectionData.CpuSerial);
					}
					else if (user.PC_Serial != e.ConnectionData.CpuSerial && dep.Id != 832)
					{
						return Status_Duplicate(e, out extension, user);
					}
				}
				line = 70;
				if (user.Type == ENSource.None && e.ConnectionData.DbGuid != Guid.Empty && e.ConnectionData.CpuSerial != "" && e.ConnectionData.Source == ENSource.Accounting)
				{
					user.Type = ENSource.Accounting;
					await Initializer.Dal.User.SaveAsync(user.DepId, user, "ProcessLoginAsync3");
				}
				line = 75;
				if (user.Type == ENSource.None && e.ConnectionData.DbGuid != Guid.Empty && e.ConnectionData.CpuSerial != "" && e.ConnectionData.Source == ENSource.Baje)
				{
					user.Type = ENSource.Baje;
					await Initializer.Dal.User.SaveAsync(user.DepId, user, "ProcessLoginAsync4");
				}
				line = 80;
				if (e.ConnectionData.AndroidIme != 5137597590L && user.Type == ENSource.Android_HesabYar)
				{
					if (user.AndroidIme <= 0)
					{
						await Initializer.Dal.User.SetAndroidImeAsync(user.Id, e.ConnectionData.AndroidIme);
						user.AndroidIme = e.ConnectionData.AndroidIme;
					}
					else if (user.AndroidIme != e.ConnectionData.AndroidIme)
					{
						ImeSeprator imeSeprator = new ImeSeprator(e.ConnectionData.AndroidIme);
						ImeSeprator imeSeprator2 = new ImeSeprator(user.AndroidIme);
						if (!imeSeprator2.IsValidIme || !imeSeprator.IsValidIme || !(imeSeprator2.Serial == imeSeprator.Serial))
						{
							return Status_Duplicate(e, out extension, user);
						}
						await Initializer.Dal.User.SetAndroidImeAsync(user.Id, e.ConnectionData.AndroidIme);
						user.AndroidIme = e.ConnectionData.AndroidIme;
					}
				}
				line = 100;
				if (user.CashierGuid == Guid.Empty && user.Type != ENSource.NPBackupService && user.Type != ENSource.TimeAttendance && user.Type != ENSource.Accounting && user.Type != ENSource.Programmer)
				{
					return Status_AccountingUserNotAssigned(e, out extension, user);
				}
				if (user.Type == ENSource.Accounting && dep.Id != 256 && user.DBGuid == Guid.Empty)
				{
					await Initializer.Dal.User.SetDBGuidAsync(user.Id, e.ConnectionData.DbGuid);
					user.DBGuid = e.ConnectionData.DbGuid;
				}
				line = 105;
				token.ThrowIfCancellationRequested();
				WebUserInformation userInfo;
				if ((e.ConnectionData.Source == ENSource.Android_SefareshGir || e.ConnectionData.Source == ENSource.WebSite) && !string.IsNullOrEmpty(e.ConnectionData.CustomerUsername) && !string.IsNullOrEmpty(e.ConnectionData.CustomerPassword))
				{
					try
					{
						userInfo = await Initializer.Dal.UserInformation.LoadDataAsync(e.ConnectionData.Username, e.ConnectionData.Password, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.LkSerial, e.ConnectionData.CpuSerial, e.ConnectionData.AndroidIme, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid, token);
						if (userInfo == null)
						{
							return Status_CustomerNotExist(e, out extension);
						}
					}
					catch (OperationCanceledException)
					{
						return new LoginExtention
						{
							Status = EnLoginResult.LoginCanceled
						};
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "Data:" + e, "ProcessLoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 693);
						return Status_CustomerNotExist(e, out extension);
					}
				}
				else
				{
					userInfo = await Initializer.Dal.UserInformation.LoadDataAsync(e.ConnectionData.Username, e.ConnectionData.Password, e.ConnectionData.DbGuid, e.ConnectionData.LkSerial, e.ConnectionData.CpuSerial, e.ConnectionData.AndroidIme, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid, token);
				}
				line = 120;
				if (e.ConnectionData.ClientVersion <= 10 && userInfo?.GlobalSettings?.Sanads != null)
				{
					userInfo.GlobalSettings.Sanads.EndMaliYearDate = DateTime.Now.AddYears(1);
				}
				token.ThrowIfCancellationRequested();
				if (userInfo == null)
				{
					Serilog.Log.Warning("userInfo not found.");
					return Status_CustomerNotExist(e, out extension, userInfo);
				}
				line = 125;
				token.ThrowIfCancellationRequested();
				if ((e.ConnectionData.Source == ENSource.Android_SefareshGir || e.ConnectionData.Source == ENSource.Baje || e.ConnectionData.Source == ENSource.Android_HesabYar || e.ConnectionData.Source == ENSource.IOS_HesabYar || e.ConnectionData.Source == ENSource.WebSite) && (userInfo?.UserAcc?.Guid ?? Guid.Empty) == Guid.Empty)
				{
					return Status_AccountingUserNotAssigned(e, out extension, user, userInfo);
				}
				line = 130;
				token.ThrowIfCancellationRequested();
				user.LastLogin = DateTime.Now;
				await Initializer.Dal.User.SetLastLoginAsync(user.Id, DateTime.Now);
				line = 135;
				token.ThrowIfCancellationRequested();
				return Status_Success(e, out extension, userInfo);
			}
			catch (OperationCanceledException ex3)
			{
				UnHandleConnection(GetConnection(e.ConnectionUniqueId), "Instance_OnLogin", (e?.ConnectionData?.Source).GetValueOrDefault());
				return new LoginExtention(EnLoginResult.LoginCanceled, ex3.Message, null, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, $"line:{line}//Data:" + e, "ProcessLoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ConnectionManager.cs", 735);
				return new LoginExtention(EnLoginResult.ServerError, ex4.Message, null, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
			}
			finally
			{
				if (extension != null && extension.Status != EnLoginResult.Success)
				{
					extension.MinLogLevel = minLevel;
				}
			}
		}
	}

	private LoginExtention Status_Success(LoginRequestEventArgs e, out LoginExtention extension, WebUserInformation userInfo)
	{
		extension = new LoginExtention(EnLoginResult.Success, "", userInfo, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Debug("successful login. user: {@user_id}", userInfo?.UserId);
		return extension;
	}

	private LoginExtention Status_CustomerNotExist(LoginRequestEventArgs e, out LoginExtention extension, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.CustomerNotExists;
		extension = new LoginExtention(enLoginResult, "نام کاربری یا کلمه عبور وارد شده(مشتری) صحیح نمیباشد.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unsuccessful login. customer does not exist. {@login_status}, {@customer_username}", enLoginResult, e.ConnectionData.CustomerUsername);
		return extension;
	}

	private LoginExtention Status_AccountingUserNotAssigned(LoginRequestEventArgs e, out LoginExtention extension, WebUser user, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.AccountingUserNotAssigned;
		extension = new LoginExtention(enLoginResult, "هرکاربر وب سرویس باید به یکی از کاربران نرم افزار حسابداری ارتباط داده شود تا از سطوح دسترسی آن کاربر جهت تبادل داده ها استفاده شود.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unsuccessful login. accounting user is not assigned. {@login_status}, user: {@user_id}", enLoginResult, user?.Id);
		return extension;
	}

	private LoginExtention Status_Duplicate(LoginRequestEventArgs e, out LoginExtention extension, WebUser user, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.Duplicate;
		extension = new LoginExtention(enLoginResult, "استفاده اشتراکی از یک کلید برای بیش از یک دستگاه.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unsuccessful login. duplicate. {@login_status}, user: {@user_id}", enLoginResult, user?.Id);
		return extension;
	}

	private LoginExtention Status_DepartmentDeleted(LoginRequestEventArgs e, out LoginExtention extension, WebDepartment dep, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.DepartmentDeleted;
		extension = new LoginExtention(enLoginResult, "دپارتمان حذف شده است.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unsuccessful login. department is deleted. {@login_status}, dep: {@dep_id}", enLoginResult, dep?.Id);
		return extension;
	}

	private LoginExtention Status_DepartmentExpired(LoginRequestEventArgs e, out LoginExtention extension, WebDepartment dep, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.DepartmentExpired;
		extension = new LoginExtention(enLoginResult, "تاریخ انقضاء دپارتمان به پایان رسیده است.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unsuccessful login. department is expired. {@login_status}, dep: {@dep_id}", enLoginResult, dep?.Id);
		return extension;
	}

	private LoginExtention Status_DepartmentNotVerified(LoginRequestEventArgs e, out LoginExtention extension, WebDepartment dep, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.DepartmentNotVerified;
		extension = new LoginExtention(enLoginResult, "این دپارتمان در وضعیت تایید شده نمیباشد.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unseccessful login. department is not verified. {@login_status}, dep: {@dep_id}", enLoginResult, dep?.Id);
		return extension;
	}

	private LoginExtention Status_UserExpired(LoginRequestEventArgs e, out LoginExtention extension, WebUser user, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.UserExpired;
		extension = new LoginExtention(enLoginResult, "کاربر منقضی شده است.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		if (DateTime.Now > new DateTime(2026, 10, 25))
		{
			Serilog.Log.Warning("unsuccesful login. user expired. {@login_status}, userId: {@user_id}, dep: {@dep_id}", enLoginResult, user?.Id, user?.DepId);
		}
		return extension;
	}

	private LoginExtention Status_SourceMismatch(LoginRequestEventArgs e, out LoginExtention extension, WebUser user, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.SourceMismatch;
		extension = new LoginExtention(enLoginResult, "کاربر تعریف شده با نوع محصول لاگین شده همخوانی ندارند.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unsuccessful login. source mismatch. {@login_status}, user: {@user_id}", enLoginResult, user?.Id);
		return extension;
	}

	private LoginExtention Status_NotExists(LoginRequestEventArgs e, out LoginExtention extension, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.NotExists;
		extension = new LoginExtention(enLoginResult, "نام کاربری یا کلمه عبور وارد شده صحیح نمیباشد.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unsuccessful login. user does not exist. {@login_status}, username: {@username}", enLoginResult, e.ConnectionData.Username);
		return extension;
	}

	private LoginExtention Status_DeletedUser(LoginRequestEventArgs e, out LoginExtention extension, WebUser user, WebUserInformation userInformation = null)
	{
		EnLoginResult enLoginResult = EnLoginResult.UserDeleted;
		extension = new LoginExtention(enLoginResult, "کاربر حذف شده است.", userInformation, e.ConnectionData.CustomerUsername, e.ConnectionData.CustomerPassword, e.ConnectionData.CashierGuid, e.ConnectionData.ClientContextGuid);
		Serilog.Log.Warning("unsuccessful login. user is deleted. {@login_status}, user: {@user_id}", enLoginResult, user.Id);
		return extension;
	}

	public Task SendPacketAsync(ConnectionData connection, Packet packet, Priority priority, int partitionId, CancellationToken token)
	{
		return ((ClsConnectionData)connection).SendPacketAsync(packet, priority, partitionId, token);
	}
}
