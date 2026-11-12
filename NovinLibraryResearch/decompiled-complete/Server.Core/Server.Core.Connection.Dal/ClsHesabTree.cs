using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsHesabTree : IEntity<WebHesabTree>
{
	private class NestedHesabTree
	{
		internal static readonly ClsHesabTree HesabTreeInstance;

		static NestedHesabTree()
		{
			HesabTreeInstance = new ClsHesabTree();
		}
	}

	private static ConcurrentDictionary<short, DateTime> dic_isModifiedDepartmentData = new ConcurrentDictionary<short, DateTime>();

	public static ClsHesabTree HesabTreeInstance => NestedHesabTree.HesabTreeInstance;

	private ClsHesabTree()
	{
	}

	public async Task<List<WebHesabTree>> ApplyAccessAsync(IEnumerable<WebHesabTree> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs"))
		{
			try
			{
				if (!connection.IsAccountingFamily)
				{
					return null;
				}
				if (connection.Source != ENSource.Accounting)
				{
					lst?.Count();
					lst = lst?.Where((WebHesabTree p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				if (lst != null)
				{
					IEnumerable<WebHesabTree> enumerable = lst;
					if (enumerable == null || enumerable.Count() > 0)
					{
						WebUserAcc userAccInfo = await connection.GetUserAccInfoAsync();
						if (userAccInfo == null)
						{
							return lst?.ToList();
						}
						WebUserAcc webUserAcc = userAccInfo;
						if (webUserAcc != null && !webUserAcc.AllBanksAccess)
						{
							lst = lst.Where(delegate(WebHesabTree e)
							{
								if (e == null || e.HType == HesabType.Bank)
								{
									if (e.HType == HesabType.Bank)
									{
										Guid guid2 = e.Guid;
										Guid? bankGuid = userAccInfo.BankGuid1;
										if (!(guid2 == bankGuid))
										{
											guid2 = e.Guid;
											bankGuid = userAccInfo.BankGuid2;
											if (!(guid2 == bankGuid))
											{
												guid2 = e.Guid;
												bankGuid = userAccInfo.BankGuid3;
												if (!(guid2 == bankGuid))
												{
													guid2 = e.Guid;
													bankGuid = userAccInfo.BankGuid4;
													return guid2 == bankGuid;
												}
											}
										}
										return true;
									}
									return false;
								}
								return true;
							})?.ToList();
						}
						if (lst == null)
						{
							return null;
						}
						WebUserAcc webUserAcc2 = userAccInfo;
						if (webUserAcc2 != null && !webUserAcc2.AllSandoghsAccess)
						{
							lst = lst.Where(delegate(WebHesabTree e)
							{
								if (e == null || e.HType == HesabType.Sandogh)
								{
									if (e.HType == HesabType.Sandogh)
									{
										Guid guid = e.Guid;
										Guid? sandoghGuid = userAccInfo.SandoghGuid;
										return guid == sandoghGuid;
									}
									return false;
								}
								return true;
							})?.ToList();
						}
						WebUserAcc webUserAcc3 = userAccInfo;
						if (webUserAcc3 != null && webUserAcc3.UserAccess?.Customers?.Customer_DastrasiBeAshkhaseKhas == false)
						{
							lst = lst.Where((WebHesabTree e) => e != null && !e.Special).ToList();
						}
						WebUserAcc webUserAcc4 = userAccInfo;
						if (webUserAcc4 != null && webUserAcc4.UserAccess?.TafsilGroupList?.Count > 0)
						{
							lst = lst?.Where((WebHesabTree e) => userAccInfo.UserAccess.TafsilGroupList.Any((short e2) => e2.Equals(e?.TafsilLevel))).ToList();
						}
						return lst?.ToList();
					}
				}
				return null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 88);
				return null;
			}
		}
	}

	internal async Task HesabTreeInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebHesabTree> e)
	{
		using (new JaegerMonitor("HesabTreeInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 100;
				WebDepartment obj = await Server.Core.Assistance.Initializer.Dal.Department.LoadDataAsync(e.Connection.DepId);
				line = 101;
				if (obj == null)
				{
					throw new NullReferenceException("dep is null|!");
				}
				if (obj.SyncsKolMoeinTafsil)
				{
					throw new Exception($"Department({e.Connection.DepId}) نباید ساختار درختی حسابها را ارسال کند.");
				}
				line = 102;
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.HesabTree, 1, isReceivedPacket: true);
				line = 1;
				if (e.Connection.cancellationToken.IsCancellationRequested || e.Connection.Source == ENSource.WebsiteWordPress)
				{
					return;
				}
				line = 2;
				if (e == null || e?.Connection == null || e?.ConnectionData == null)
				{
					return;
				}
				ConnectionEventArgs<WebHesabTree> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData?.Count <= 0)
				{
					return;
				}
				line = 3;
				List<SaveResponse> responses = new List<SaveResponse>();
				line = 4;
				ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				line = 5;
				if (connection2 == null || e.Connection == null || e.Connection.DepId <= 0)
				{
					return;
				}
				line = 6;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.HesabTree.SaveAsync(e.Connection.DepId, e.ConnectionData, connection2.cancellationToken, connection2.HashId, connection2.Source), ReturnedState.None, "HesabTreeInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 137);
				line = 7;
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				line = 8;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, !ret.HasError))
				{
					line = 9;
					responses.Add(new SaveResponse
					{
						SaveResultStatus = true,
						PartitionId = e.PartitionId
					});
					line = 10;
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					line = 11;
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebHesabTree.GenerateSaveResponse(responses, e.PacketReferenceId), Priority.High);
					line = 12;
				}
				line = 13;
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				line = 14;
				if (e.Connection.Source != ENSource.Accounting)
				{
					List<WebHesabTree> connectionData = e.ConnectionData;
					if (connectionData != null && connectionData.Count > 0)
					{
						line = 15;
						if (e.Connection.cancellationToken.IsCancellationRequested)
						{
							return;
						}
						line = 16;
						ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebHesabTree.GenerateSaveRequest(e.ConnectionData, 0), Priority.Medium);
						line = 17;
					}
				}
				line = 18;
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				line = 19;
				List<WebHesabTree> connectionData2 = e.ConnectionData;
				if (connectionData2 == null || connectionData2.Count <= 0)
				{
					return;
				}
				line = 20;
				if (e.Connection.cancellationToken.IsCancellationRequested || e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				List<ClsConnectionData> list = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.ToList();
				line = 25;
				list = list?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && ((p != null && p.Source == ENSource.Android_HesabYar) || (p != null && p.Source == ENSource.IOS_HesabYar) || (p != null && p.Source == ENSource.Android_SefareshGir) || (p != null && p.Source == ENSource.Baje) || (p != null && p.Source == ENSource.AccountingPortal) || (p != null && p.Source == ENSource.WebsiteWordPress) || (p != null && p.Source == ENSource.WebSite))).ToList();
				line = 300;
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				line = 301;
				if (list != null)
				{
					foreach (ClsConnectionData connection in list)
					{
						if (connection != null && !connection.IsDisposed)
						{
							line = 35;
							if (e.Connection.cancellationToken.IsCancellationRequested)
							{
								return;
							}
							List<WebHesabTree> list2 = await ApplyAccessAsync(e.ConnectionData, connection);
							line = 40;
							if (list2 != null && list2.Any())
							{
								connection?.SendPacketAsync(WebHesabTree.GenerateReadResponse(list2, isSync: false, 0), Priority.Medium);
							}
						}
					}
				}
				line = 45;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//Count:{(e?.ConnectionData?.Count).GetValueOrDefault(-1)}", "HesabTreeInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 206);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb, short maxParalel = 10)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		int line = 0;
		try
		{
			line = 5000;
			if (connection?.IsDisposed ?? true)
			{
				return ret.AddError("connection is disposed!", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 217);
			}
			line = 5001;
			WebDepartment obj = (await connection.GetDepartmentAsync()) ?? throw new NullReferenceException($"Department of connection is null!! depid:{connection?.DepId ?? (-1)}");
			line = 5002;
			if (obj?.SyncsKolMoeinTafsil ?? true)
			{
				line = 1002;
				ret.AddInformation("این دپارتمان درخت حساب را سینک نمیکند", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 226);
				line = 1003;
				return ret;
			}
			line = 1004;
			if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 232);
			}
			line = 1005;
			ClsConnectionData clsConnectionData = connection;
			if (clsConnectionData != null && clsConnectionData.Source == ENSource.WebsiteWordPress)
			{
				return ret;
			}
			line = 1006;
			SyncInformation syncInformation = await connection.GetSyncInfoAsync();
			if (syncInformation == null)
			{
				ret.AddReturnedValue(ReturnedState.Error, "connection?.SyncInfo == null", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 239);
				return ret;
			}
			line = 2;
			if (syncInformation.HesabTreeReset)
			{
				line = 5;
				IEnumerable<WebHesabTree> enumerable = await Server.Core.Assistance.Initializer.Dal.HesabTree.GetAllAsync(connection.DepId, new DateTime(1900, 1, 1), connection.cancellationToken, removeGardesh: false);
				line = 7;
				if (enumerable != null && enumerable.Count() > 0)
				{
					if (connection.Source == ENSource.WebSite)
					{
						if (enumerable != null && enumerable.Count() > 0)
						{
							enumerable = enumerable.Where((WebHesabTree p) => p.HType == HesabType.Bank)?.ToList();
						}
						if (enumerable != null && enumerable.Count() > 0)
						{
							foreach (WebHesabTree item in enumerable)
							{
								item.GardeshTafsili = null;
							}
						}
					}
					else
					{
						enumerable = await ApplyAccessAsync(enumerable, connection);
					}
				}
				line = 9;
				List<SyncResetItem> ResetItems = new List<SyncResetItem>();
				line = 10;
				if (enumerable != null && enumerable.Count() > 0)
				{
					foreach (WebHesabTree item2 in enumerable)
					{
						if (item2 != null)
						{
							ResetItems.Add(new SyncResetItem
							{
								Guid = item2.Guid,
								Id = item2.Id
							});
						}
					}
				}
				line = 15;
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.HesabTree, 300000, connection.cancellationToken);
				line = 16;
				Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.HesabTree, ResetItems), Priority.High));
				line = 17;
				if (await waiter.WaitForSyncDonePacket())
				{
					await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.HesabTree, connection.cancellationToken);
				}
				line = 20;
			}
			line = 21;
			List<short> totalPartitions = new List<short>();
			List<short> list = await Server.Core.Assistance.Initializer.Dal.HesabTree.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
			if (list != null && list.Any())
			{
				totalPartitions.AddRange(list);
				while (list != null && list.Any())
				{
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 313);
						return ret;
					}
					await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 319);
						return ret;
					}
					line = 22;
					if (connection == null || connection.IsDisposed)
					{
						return ret;
					}
					list = await Server.Core.Assistance.Initializer.Dal.HesabTree.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
					line = 23;
					list = list?.Where((short x) => !totalPartitions.Contains(x))?.ToList();
					line = 24;
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 331);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 334);
			ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 335);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			sb.AppendLine($"HesabtreeeeeeeeeeeeSyncResult:{!ret.HasError}");
		}
		return ret;
	}

	private async Task<bool> SyncPartitionsAsync(IEnumerable<short> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		_ = 1;
		try
		{
			new CancellationTokenSource();
			if (partitions != null)
			{
				if (partitions == null || partitions.Count() > 0)
				{
					List<Task> tasks = new List<Task>();
					foreach (short partition in partitions)
					{
						tasks.Add(syncPartitionAsync(partition, connection));
						if (tasks.Count >= maxParalel)
						{
							List<Task> list = tasks;
							list.Remove(await Task.WhenAny(tasks));
						}
					}
					await Task.WhenAll(tasks);
					return true;
				}
			}
			return true;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 361);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> syncPartitionAsync(short partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (connection.Source == ENSource.WebsiteWordPress)
			{
				return ret;
			}
			line = 25;
			DateTime syncing = DateTime.Now;
			line = 26;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 376);
			}
			IEnumerable<WebHesabTree> lst = await Server.Core.Assistance.Initializer.Dal.HesabTree.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken, removeGardesh: false);
			using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.HesabTree, 300000, connection.cancellationToken);
			line = 30;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 387);
			}
			lst = await ApplyAccessAsync(lst, connection);
			line = 35;
			if (lst?.Any() ?? false)
			{
				line = 40;
				Task.Run(() => connection.SendPacketAsync(WebHesabTree.GenerateReadResponse(lst, isSync: true, partition), Priority.Low));
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					ret.AddReturnedValue(ReturnedState.Error, "waiter time out", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 401);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 404);
				}
				line = 45;
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.HesabTree, syncing, connection), ReturnedState.None, "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 407);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 410);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 413);
			ret.AddReturnedValue(ex2, "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsHesabTree.cs", 414);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
		}
		return ret;
	}
}
