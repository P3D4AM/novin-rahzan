using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using PacketParser.TransferLayer.Waiters.Accounting.Product;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsProduct : IEntity<WebProduct>
{
	private class NestedProduct
	{
		internal static readonly ClsProduct ProductInstance;

		static NestedProduct()
		{
			ProductInstance = new ClsProduct();
		}
	}

	public static ClsProduct ProductInstance => NestedProduct.ProductInstance;

	public async Task<List<WebProduct>> ApplyAccessAsync(IEnumerable<WebProduct> lst, IClsConnectionData connection)
	{
		int line = 0;
		try
		{
			WebUser user = await connection.GetUserAsync();
			line = 1;
			WebUserAcc userAccInfo = await connection.GetUserAccInfoAsync();
			line = 2;
			StringBuilder strB = new StringBuilder(100000);
			line = 3;
			strB.AppendLine("#ProductApplyAccess");
			line = 4;
			strB.AppendLine($"#DepId_{connection?.DepId ?? 0}");
			strB.AppendLine($"#UserId_{user?.Id ?? 0}");
			strB.AppendLine("#UserName_" + user?.Name);
			strB.AppendLine($"CountBefore:{lst?.Count() ?? 0}");
			line = 10;
			if (!connection.IsAccountingFamily)
			{
				return null;
			}
			line = 11;
			WebDepartment webDepartment = await connection.GetDepartmentAsync();
			line = 120;
			if (webDepartment == null)
			{
				throw new NullReferenceException("department!!!");
			}
			line = 121;
			if (connection == null)
			{
				throw new NullReferenceException("connection!!");
			}
			line = 122;
			IEnumerable<WebProduct> enumerable = await ApplyAccessAsync(strB, webDepartment.IsAccountingVerion6, lst, userAccInfo, connection.DepId, connection.cancellationToken);
			line = 13;
			if ((enumerable?.Count() ?? 0) != (lst?.Count() ?? 0))
			{
				line = 14;
				strB.AppendLine($"تعداد کالاهای ارسال شده:{enumerable?.Count() ?? 0}//کالاهای فعال:{enumerable?.Count((WebProduct p) => p.Status) ?? 0}");
				line = 15;
				WebTelegramMessage.GetWebServiceMonitoring_bot().Sent2web(strB.ToString(), null, "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 56);
			}
			line = 16;
			return enumerable?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "#ApplyAccess_1_\r\n" + $"#Line_{line}", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 63);
			throw;
		}
	}

	public async Task<IEnumerable<WebProduct>> ApplyAccessAsync(StringBuilder sb, bool IsAccountingVerion6, IEnumerable<WebProduct> lst, IUserAcc user, short depId, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs"))
		{
			int line = 1;
			try
			{
				if (!(lst?.Any() ?? false))
				{
					return lst;
				}
				lst.ToList();
				line = 2;
				if (!(user?.UserAccess?.Product?.Product_DastrasiBeKalahayeKhas).GetValueOrDefault())
				{
					line = 3;
					List<WebPrdGroup> source = await Server.Core.Assistance.Initializer.Dal.ProductGroup.GetAllAsync(depId, DateTime.Now.AddYears(-100), token, 10);
					line = 4;
					int num = lst?.Count() ?? 0;
					line = 5;
					if (IsAccountingVerion6)
					{
						line = 6;
						List<Guid> groupGuids = source.Where((WebPrdGroup p) => !p.Special)?.Select((WebPrdGroup p) => p.Guid)?.ToList() ?? new List<Guid>();
						line = 7;
						if (groupGuids?.Any() ?? false)
						{
							lst = lst?.Where((WebProduct p) => !p.Special && groupGuids.Contains(p.GuidGroup));
						}
					}
					else
					{
						line = 8;
						List<short> groupIds = source.Where((WebPrdGroup p) => !p.Name.Contains("#"))?.Select((WebPrdGroup p) => p.Id)?.ToList() ?? new List<short>();
						line = 9;
						if (groupIds?.Any() ?? false)
						{
							lst = lst?.Where((WebProduct p) => !p.Name.Contains("#") && groupIds.Contains(p.IdGroup));
						}
					}
					line = 10;
					lst = lst?.Where((WebProduct p) => !p.Special);
					lst = lst?.ToList();
					sb.AppendLine($"#اعمال_دسترسی_به_کالاهای_خاص =>{num}=>{lst?.Count() ?? 0}");
				}
				line = 11;
				if (IsAccountingVerion6)
				{
					line = 12;
					if ((user?.UserAccess?.PrdGroupListGuid?.Any()).GetValueOrDefault())
					{
						line = 13;
						int num2 = lst?.Count() ?? 0;
						line = 14;
						lst = lst?.Where((WebProduct e) => user.UserAccess.PrdGroupListGuid.Any((Guid e2) => e2.Equals(e.GuidGroup)));
						line = 15;
						sb.AppendLine($"#اعمال_دسترسی_به_گروه_های_کالای_انتخابی_V6 =>{num2}=>{lst?.Count() ?? 0}");
					}
					line = 16;
				}
				else
				{
					line = 17;
					if ((user?.UserAccess?.PrdGroupList?.Any()).GetValueOrDefault())
					{
						line = 18;
						int num3 = lst?.Count() ?? 0;
						line = 19;
						lst = lst?.Where((WebProduct e) => user.UserAccess.PrdGroupList.Any((short e2) => e2.Equals(e.IdGroup)));
						line = 20;
						sb.AppendLine($"#اعمال_دسترسی_به_گروه_های_کالای_انتخابی_V5 =>{num3}=>{lst?.Count() ?? 0}");
					}
				}
				line = 21;
				if (IsAccountingVerion6)
				{
					line = 22;
					int num4 = lst?.Count() ?? 0;
					line = 23;
					lst = lst?.Where((WebProduct p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
					line = 24;
					sb.AppendLine($"#اعمال_دسترسی_به_کالاهای_تحویل_شده_V5 =>{num4}=>{lst?.Count() ?? 0}");
				}
				return lst;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "#ApplyAccess_2\r\n" + $"#Line_{line}", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 163);
				return null;
			}
		}
	}

	internal async Task ProductInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebProduct> e)
	{
		using (new JaegerMonitor("ProductInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs"))
		{
			StringBuilder sb = new StringBuilder(100000);
			int line = 0;
			try
			{
				line = 1;
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Product, 1, isReceivedPacket: true);
				line = 2;
				if (e == null || !(e.ConnectionData?.Any() ?? false) || e.Connection == null || e.Connection.DepId <= 0)
				{
					return;
				}
				line = 50;
				sb.AppendLine($"#DepId{e.Connection.DepId}");
				sb.AppendLine(string.Join(",", e.ConnectionData) ?? "");
				CancellationToken valueOrDefault = (ConnectionManager.Instance?.GetConnection(e.ConnectionUniqueId)?.cancellationToken).GetValueOrDefault();
				line = 51;
				List<SaveResponse> ret = await Server.Core.Assistance.Initializer.Dal.Product.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, e.Connection.Source, valueOrDefault);
				line = 10;
				if (ret != null)
				{
					sb.AppendLine(string.Join("\r\n", ret.Select((SaveResponse s) => s.Shortmessage)));
				}
				else
				{
					sb.AppendLine("Ret is null");
				}
				line = 15;
				foreach (SaveResponse item in ret)
				{
					item.PartitionId = e.PartitionId;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					Task.Run(() => ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebProduct.GenerateSaveResponse(ret), Priority.High));
				}
				line = 20;
				IEnumerable<WebProduct> enumerable = e.ConnectionData.Where((WebProduct prd) => prd.Guid == Guid.Empty);
				if (enumerable != null && enumerable.Count() > 0)
				{
					throw new Exception($"کالای دریافتی حاوی Guid خالی میباشد\\Source:{(e?.Connection?.Source).GetValueOrDefault()}\\depId:{(e?.Connection?.DepId).GetValueOrDefault(-1)}");
				}
				line = 25;
				ConnectionEventArgs<WebProduct> connectionEventArgs = e;
				if (connectionEventArgs == null || connectionEventArgs.Connection.Source != ENSource.Accounting)
				{
					line = 26;
					ClsConnectionData connection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (!(connection?.IsDisposed ?? true))
					{
						return;
					}
					line = 300;
					if (e.Connection.Source == ENSource.WebsiteWordPress)
					{
						throw new Exception("invalid data for send to wordpress!!");
					}
					line = 301;
					if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
					{
						throw new NullReferenceException("e.ConnectionData");
					}
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					line = 302;
					Task.Run(() => connection?.SendPacketAsync(WebProduct.GenerateSaveRequest(e.ConnectionData, 0), Priority.High));
				}
				line = 35;
				if (e.Connection.Source == ENSource.Accounting)
				{
					await SendToOtherClientsExceptAccounting(e.Connection.DepId, e.ConnectionData);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}\r\n" + $"DepId:{e.Connection.DepId}\r\n {Json.ToStringJson(e.ConnectionData)}\r\n{DateTime.Now}", "ProductInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 235);
			}
			finally
			{
				if (e.Connection.Source == ENSource.Android_HesabYar)
				{
					WebTelegramMessage.GetNp_Ticket_bot().Sent2web(sb?.ToString() ?? "sb is null!!", null, "ProductInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 241);
				}
			}
		}
	}

	private async Task SendToOtherClientsExceptAccounting(short depId, List<WebProduct> data)
	{
		int line = 0;
		try
		{
			IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(depId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.Source != ENSource.Accounting && p.IsAccountingFamily);
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (ClsConnectionData connection in enumerable)
			{
				line = 40;
				if (connection == null || connection.IsDisposed)
				{
					line = 41;
					IEnumerable<WebProduct> enumerable2 = data;
					if (connection.ClientVersion < 10 || connection.Source == ENSource.WebsiteWordPress)
					{
						enumerable2 = await ApplyAccessAsync(enumerable2, connection);
					}
					line = 42;
					if (enumerable2 != null && enumerable2.Any())
					{
						await connection.SendPacketAsync(WebProduct.GenerateReadResponse(enumerable2, 0), Priority.High);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SendToOtherClientsExceptAccounting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 273);
		}
	}

	internal async Task ProductInstance_OnReadRequest(object sender, ConnectionEventArgs<ProductReadEventArgs> e)
	{
		using (new JaegerMonitor("ProductInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs"))
		{
			_ = 2;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Product, 5, isReceivedPacket: true);
				if (e == null || e?.ConnectionData == null || e.ConnectionData.Count <= 0 || e.Connection == null)
				{
					return;
				}
				_ = e.Connection.cancellationToken;
				if (!(e?.Connection?.DepId <= 0))
				{
					IEnumerable<WebProduct> enumerable = await Server.Core.Assistance.Initializer.Dal.Product.GetAllAsync(e.ConnectionData.FirstOrDefault(), e.Connection.DepId, null);
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.Connection.Guid);
					if (connection.ClientVersion < 10 || connection.Source == ENSource.WebsiteWordPress)
					{
						enumerable = await ProductInstance.ApplyAccessAsync(enumerable, connection);
					}
					await connection.SendPacketAsync(WebProduct.GenerateReadResponse(enumerable, 0), Priority.High);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ProductInstance_OnReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 293);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10, int waiterDelayTimeSeconds = 60000)
	{
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 314);
			}
			SyncInformation syncInformation = await connection.GetSyncInfoAsync();
			if (syncInformation == null)
			{
				throw new NullReferenceException("connection?.syncInfo is null!!");
			}
			if (connection.ClientVersion < 10 && syncInformation != null && syncInformation.ProductReset && connection.Source != ENSource.WebsiteWordPress)
			{
				line = 5;
				IEnumerable<WebProduct> enumerable = await Server.Core.Assistance.Initializer.Dal.Product.GetAllAsync(connection.DepId, connection.cancellationToken);
				line = 6;
				if (connection.ClientVersion < 10 || connection.Source == ENSource.WebsiteWordPress)
				{
					enumerable = await ApplyAccessAsync(enumerable, connection);
				}
				line = 7;
				List<SyncResetItem> ResetItems = new List<SyncResetItem>();
				line = 8;
				if (enumerable != null && enumerable.Any())
				{
					foreach (WebProduct item in enumerable)
					{
						ResetItems.Add(new SyncResetItem
						{
							Guid = item.Guid,
							Id = item.Id
						});
					}
				}
				line = 15;
				using (new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.Product, 300000, connection.cancellationToken))
				{
					line = 16;
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.Product, ResetItems), Priority.High));
					line = 17;
					await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.Product, connection.cancellationToken);
					line = 20;
				}
			}
			line = 21;
			List<int> totalPartitions = new List<int>();
			line = 22;
			IEnumerable<int> enumerable2 = await Server.Core.Assistance.Initializer.Dal.Product.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
			if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 354);
			}
			line = 23;
			if (enumerable2 != null && enumerable2.Any())
			{
				line = 24;
				totalPartitions.AddRange(enumerable2);
				line = 25;
				while (enumerable2 != null && enumerable2.Any())
				{
					if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 367);
						return ret;
					}
					line = 26;
					if (connection.Source == ENSource.WebsiteWordPress)
					{
						maxParalel = 1;
					}
					await SyncPartitionsAsync(enumerable2, connection, ConnectionUniqueId, maxParalel);
					line = 27;
					enumerable2 = await Server.Core.Assistance.Initializer.Dal.Product.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
					if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 376);
					}
					line = 28;
					enumerable2 = enumerable2?.Where((int x) => totalPartitions != null && !totalPartitions.Contains(x));
					line = 29;
				}
				line = 30;
			}
			line = 31;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 388);
			ret.AddReturnedValue(ex2, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 389);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
		}
		return ret;
	}

	internal async Task ProductInstance_OnReadResponse(object sender, ConnectionEventArgs<WebProduct> e)
	{
		new StringBuilder(100000);
		try
		{
			ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Product, 6, isReceivedPacket: true);
			if (e.Connection.Source != ENSource.Accounting)
			{
				throw new Exception($"Source({e.Connection.Source}) not supported!");
			}
			IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p.Source != ENSource.Accounting);
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (ClsConnectionData connection in enumerable)
			{
				Task.Run(() => connection.SendPacketAsync(WebProduct.GenerateReadResponse(e.ConnectionData, 0), Priority.High));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ProductInstance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 407);
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionsAsync(IEnumerable<int> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			new CancellationTokenSource();
			if (partitions == null)
			{
				goto IL_0051;
			}
			if (partitions != null && partitions.Count() <= 0)
			{
				goto IL_0051;
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (int partition in partitions)
			{
				tasks.Add(SyncPartitionAsync(partition, connection));
				if (tasks.Count >= maxParalel)
				{
					Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 431);
					tasks.Remove(t);
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 436);
			goto end_IL_0029;
			IL_0051:
			return ret.AddError("partitions IS Null Or Empty", "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 420);
			end_IL_0029:;
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 438);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 441);
			ret.AddReturnedValue(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 442);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(int partition, ClsConnectionData connection)
	{
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (partition < 0)
		{
			throw new ArgumentException($"value({partition}) is not valid for value partitionId");
		}
		try
		{
			line = 1;
			if (connection?.IsDisposed ?? true)
			{
				return ret.AddError("connection is disposed", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 460);
			}
			SyncInformation syncInfo = await connection.GetSyncInfoAsync();
			line = 2;
			DateTime syncing = DateTime.Now;
			line = 3;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 466);
			}
			line = 4;
			IEnumerable<WebProduct> lst = await Server.Core.Assistance.Initializer.Dal.Product.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
			line = 5;
			line = 100;
			IEnumerable<WebProduct> deletedLst = lst?.ToList();
			line = 101;
			if (connection.ClientVersion < 10 || connection.Source == ENSource.WebsiteWordPress)
			{
				lst = await ApplyAccessAsync(lst, connection);
			}
			line = 102;
			if ((lst?.Any() ?? false) && (deletedLst?.Any() ?? false))
			{
				deletedLst = deletedLst?.Except(lst)?.ToList();
			}
			line = 103;
			if ((connection.ClientVersion < 10 || connection.Source == ENSource.WebsiteWordPress) && (deletedLst?.Any() ?? false))
			{
				line = 20;
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 493);
					return ret;
				}
				deletedLst = deletedLst.Where((WebProduct p) => p.ResetTime > syncInfo.Product && (p.ServerStatus == ServerStatusForClient.Delivered || p.ServerStatus == ServerStatusForClient.DirectDelivery)).ToList();
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 495);
					return ret;
				}
				if (deletedLst.Any())
				{
					line = 25;
					GetDeleteResponsePacket getDeleteResponsePacket = new GetDeleteResponsePacket(connection.HashId, partition, (connection.Source == ENSource.WebsiteWordPress) ? 1800000 : 300000, connection.cancellationToken, EntityPart.Product, connection.ContextGuid);
					if (connection.Source == ENSource.WebsiteWordPress)
					{
						Task.Run(() => connection.SendPacketAsync(WebProduct.GenerateDeleteRequest(deletedLst, partition), Priority.Low));
					}
					else
					{
						Task.Run(() => connection.SendPacketAsync(WebProduct.GenerateDeleteRequest(deletedLst.Select((WebProduct p) => p.Guid), partition), Priority.Low));
					}
					line = 30;
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 507);
					}
					bool flag = connection.Source != ENSource.WebsiteWordPress;
					if (flag)
					{
						flag = !(await getDeleteResponsePacket.WaitForDeleteDonePacket());
					}
					if (flag && Server.Core.Assistance.Initializer.Dal.ResetTimeFlag)
					{
						return ret.AddError("DeleteWaiter TimeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 511);
					}
				}
			}
			line = 40;
			line = 45;
			if (lst?.Any() ?? false)
			{
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 525);
				}
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.Product, (connection.Source == ENSource.WebsiteWordPress) ? 1800000 : 300000, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebProduct.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
				line = 50;
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					line = 55;
					ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut or error\r\nsyncResult:" + waiter.Message, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 534);
					return ret;
				}
				line = 60;
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Product, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 540);
			}
			else
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Product, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 543);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, string.Format("Line:{0}//ConnectionIsDisposed:{1}//connection:{2}", line, connection?.IsDisposed.ToString() ?? "null!!", Json.ToStringJson(connection)), "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 548);
			ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 549);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string ConnectionUniqueId, CancellationToken token, short maxParalelTasks = 5)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool hasData = false;
			try
			{
				_ = 5;
				ReturnedSaveFuncInfo result;
				try
				{
					if ((connection?.cancellationToken.IsCancellationRequested ?? true) || (connection?.IsDisposed ?? true))
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 562);
						goto end_IL_007c;
					}
					List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Product, connection.cancellationToken);
					hasData = list?.Any() ?? false;
					if ((connection?.cancellationToken.IsCancellationRequested ?? true) || (connection?.IsDisposed ?? true))
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 565);
						goto end_IL_007c;
					}
					if (list == null || !list.Any())
					{
						result = ret.AddInformation("کالایی برای ارسال به حسابداری موجود نیست.", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 567);
						goto end_IL_007c;
					}
					List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					foreach (Guid guid in list)
					{
						ret.AddInformation($"در حال ارسال کالا به حسابداری ({guid})", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 572);
						if ((connection?.cancellationToken.IsCancellationRequested ?? true) || (connection?.IsDisposed ?? true))
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 573);
						}
						else
						{
							WebProduct prd = (WebProduct)(await Server.Core.Assistance.Initializer.Dal.Product.GetAsync(connection.DepId, guid));
							if (!(connection?.cancellationToken.IsCancellationRequested ?? true) && !(connection?.IsDisposed ?? true))
							{
								if (prd == null)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.DeleteAsync(connection.DepId, EntityPart.Product, guid), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 580);
									WebErrorLog.Instance.StartLog("این خطا مدیریت شده و کالای مورد نظر از جدول دلیوری ها حذف  گردید!" + $"\r\nprd is null//Guid:{guid}//depId:{connection.DepId} not exists. so we have delete record from Web_EntitiesDeliveryStatus table", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 581);
								}
								tasks.Add(Send2AccountingAsync(prd, connection));
								if (tasks.Count >= maxParalelTasks)
								{
									Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
									tasks.Remove(task);
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 590);
								}
								continue;
							}
							result = ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 575);
						}
						goto end_IL_007c;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 593);
					goto end_IL_0059;
					end_IL_007c:;
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 595);
					goto end_IL_0059;
				}
				catch (Exception ex)
				{
					ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 598);
					WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 599);
					goto end_IL_0059;
				}
				return result;
				end_IL_0059:;
			}
			finally
			{
				if (hasData && !token.IsCancellationRequested)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SendNotDeliveredAsync(connection, ConnectionUniqueId, token, maxParalelTasks), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 604);
				}
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> Send2AccountingAsync(WebProduct prd, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		if (prd == null)
		{
			throw new ArgumentException("prd is null!!");
		}
		try
		{
			if ((connection?.cancellationToken.IsCancellationRequested ?? true) || (connection?.IsDisposed ?? true))
			{
				return ret.AddErrorTokenIsCancelled("", "", "Send2AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 616);
			}
			line = 10;
			using ProductWaiter waiter = new ProductWaiter(connection.Context, prd.Guid, connection.cancellationToken, 30000);
			line = 2;
			if (connection.Source == ENSource.WebsiteWordPress)
			{
				throw new Exception("invalid data for send to wordpress!!");
			}
			Task.Run(() => connection.SendPacketAsync(WebProduct.GenerateSaveRequest(new List<WebProduct> { prd }, 0, deliveryRequest: false), Priority.Medium));
			line = 3;
			ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse($"DepId:{connection?.DepId ?? (-1)}\r\nProductGuid:{prd?.Guid}\r\nPrdName:{prd?.Name}\r\nprdQt:{prd?.Amount}");
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 624);
			if (!ret.HasError && returnedSaveFuncInfoWithValue.value.SaveResultStatus)
			{
				prd.Modified = DateTime.Now;
				prd.ServerStatus = ServerStatusForClient.Delivered;
				await Server.Core.Assistance.Initializer.Dal.Product.SaveAsync(connection.DepId, connection.HashId, new List<WebProduct> { prd }, ENSource.Accounting, default(CancellationToken));
			}
			else if (!ret.HasError && !returnedSaveFuncInfoWithValue.value.SaveResultStatus)
			{
				WebErrorLog.Instance.StartLog("خطا در تحویل کالا به حسابداری \r\n Error:" + returnedSaveFuncInfoWithValue.value.Message + " \r\n " + Json.ToStringJson(prd), "", "Send2AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 631);
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "Send2AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 633);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}//PrdIsNull:{prd == null}//ConnectionIsNull:{connection == null}//ConnectionIsDisposed:{connection?.IsDisposed ?? true}//Connection.Context is Null:{connection?.Context == null}", "Send2AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 636);
			ret.AddReturnedValue(ex, "", "Send2AccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 637);
		}
		finally
		{
			SendToOtherClientsExceptAccounting(connection.DepId, new List<WebProduct> { prd });
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SendProductAsync(WebProduct prd, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using ProductWaiter waiter = new ProductWaiter(connection.Context, prd.Guid, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebProduct.GenerateSaveRequest(new List<WebProduct> { prd }, 0), Priority.Medium));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("ClsProduct.sendProductAsync"), ReturnedState.None, "SendProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 655);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 657);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 660);
				ret.AddReturnedValue(ex, "", "SendProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 661);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SendProductIfNotDeliveredToAccAsync(ClsConnectionData accountingConnection, short maxParalelTasks, List<Guid> guids)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (accountingConnection.ClientVersion < 10)
			{
				return ret;
			}
			line = 1;
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			line = 2;
			if (!(guids?.Any() ?? false))
			{
				return ret;
			}
			line = 3;
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (Guid guid in guids)
			{
				if (!(guid != Guid.Empty))
				{
					continue;
				}
				line = 5;
				WebProduct prd = (WebProduct)(await Server.Core.Assistance.Initializer.Dal.Product.GetAsync(accountingConnection.DepId, guid));
				line = 6;
				WebPrdGroup webPrdGroup = await Server.Core.Assistance.Initializer.Dal.ProductGroup.LoadDataAsync(accountingConnection.DepId, prd.GuidGroup, accountingConnection.cancellationToken, 10);
				line = 7;
				if (webPrdGroup.ServerStatus != ServerStatusForClient.Delivered && webPrdGroup.ServerStatus != ServerStatusForClient.DirectDelivery)
				{
					line = 10;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsProductGroup.ProductGroupInstance.SendGroupAsync(webPrdGroup, accountingConnection), ReturnedState.None, "SendProductIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 691);
				}
				line = 11;
				WebPrdVahed webPrdVahed = await Server.Core.Assistance.Initializer.Dal.ProductVahed.GetAsync(accountingConnection.DepId, prd.GuidVahed);
				line = 12;
				if (webPrdVahed.ServerStatus != ServerStatusForClient.Delivered && webPrdVahed.ServerStatus != ServerStatusForClient.DirectDelivery)
				{
					line = 13;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsProductVahed.ProductVahedInstance.SendVahedAsync(webPrdVahed, accountingConnection), ReturnedState.None, "SendProductIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 701);
				}
				line = 15;
				ret.throwExceptionIfError();
				line = 16;
				if (prd.ServerStatus != ServerStatusForClient.Delivered || prd.ServerStatus != ServerStatusForClient.DirectDelivery)
				{
					line = 20;
					tasks.Add(ProductInstance.SendProductAsync(prd, accountingConnection));
					if (tasks.Count >= maxParalelTasks)
					{
						line = 25;
						Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SendProductIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 715);
						line = 30;
						tasks.Remove(t);
					}
				}
			}
			line = 40;
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendProductIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 722);
		}
		catch (Exception ex)
		{
			if (!(accountingConnection?.IsDisposed ?? true))
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//depId:{accountingConnection?.DepId ?? (-1)}//clientVersion:{accountingConnection?.ClientVersion ?? (-1)}", "SendProductIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 727);
			}
			ret.AddReturnedValue(ex, "", "SendProductIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProduct.cs", 728);
		}
		return ret;
	}
}
