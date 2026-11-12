using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Server.Core.Connection.Dal;
using SharedServer.Core.Connection.Dal;

namespace Server.Core.Connection;

public class Sync2Client
{
	public static async Task<ReturnedSaveFuncInfo> SyncProducts(ClsConnectionData connection, string ConnectionUniqueId, short maxParalelTasks, int waiterDelayTimeSeconds = 60000)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SyncProducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 300000))
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(ClsProductGroup.ProductGroupInstance.Sync2Client(connection, ConnectionUniqueId), ClsProductVahed.ProductVahedInstance.Sync2Client(connection, ConnectionUniqueId), clsTaxClass.Instance.Sync2ClientAsync(connection, ConnectionUniqueId, 5), clsPrdSerial.Instance.Sync2ClientAsync(connection, ConnectionUniqueId, 10), ClsDepot.DepotInstance.Sync2Client(connection, ConnectionUniqueId), clsProductPriceLevel.Instance.SyncToClientAsync(connection, 5)), ReturnedState.None, "SyncProducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 20);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsProduct.ProductInstance.Sync2ClientAsync(connection, ConnectionUniqueId, maxParalelTasks, waiterDelayTimeSeconds), ReturnedState.None, "SyncProducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 28);
			}
			catch (OperationCanceledException ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SyncProducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 32);
				ret.AddReturnedValue(ex, "", "SyncProducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 33);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SyncProducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 37);
				ret.AddReturnedValue(ex2, "", "SyncProducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 38);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SyncHesab(ClsConnectionData Connection, string ConnectionUniqueId, StringBuilder sb)
	{
		using (new JaegerMonitor("SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 300000))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			_ = Connection?.DepId;
			int line = 0;
			try
			{
				line = 1;
				sb.AppendLine("start #Hesab_Sync User:#" + Connection?.Username?.FixForTelegram());
				line = 2;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (Connection.Source != ENSource.WebsiteWordPress && Connection.ClientVersion >= 5)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsTafsilGroup.TafsilGroupInstance.Sync2Client(Connection, ConnectionUniqueId, sb), ReturnedState.None, "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 56);
				}
				line = 5;
				if (Connection.Source != ENSource.WebsiteWordPress && Connection.ClientVersion >= 10)
				{
					line = 6;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsKol.KolInstance.Sync2Client(Connection, ConnectionUniqueId, sb, 10), ReturnedState.None, "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 64);
					line = 10;
					short maxParalelPacketForSyncTafsil = 10;
					if (Connection.Source == ENSource.Android_HesabYar)
					{
						maxParalelPacketForSyncTafsil = 5;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsMoein.MoeinInstance.Sync2Client(Connection, ConnectionUniqueId, sb, 10), ReturnedState.None, "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 70);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Task.Run(() => ClsCusGroup.CusGroupInstance.Sync2Client(Connection, ConnectionUniqueId, sb)), Task.Run(() => ClsCusTitle.CusTitleInstance.Sync2Client(Connection, ConnectionUniqueId, sb)), Task.Run(() => ClsCustomerRole.CustomerRoleInstance.Sync2Client(Connection, ConnectionUniqueId, sb, 5)), Task.Run(() => ClsTafsil.TafsilInstance.Sync2Client(Connection, ConnectionUniqueId, sb, maxParalelPacketForSyncTafsil))), ReturnedState.None, "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 71);
					line = 15;
				}
				line = 16;
				sb.AppendLine($"Syncing..... //sync of kol/Moein/Tafsil has been finished! result:{!ret.HasError}");
				line = 17;
				if (Connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 84);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Task.Run(() => ClsHesabTree.HesabTreeInstance.Sync2Client(Connection, ConnectionUniqueId, sb, 10)), Task.Run(() => ClsCusGroup.CusGroupInstance.Sync2Client(Connection, ConnectionUniqueId, sb)), Task.Run(() => ClsCustomerRole.CustomerRoleInstance.Sync2Client(Connection, ConnectionUniqueId, sb, 5)), Task.Run(() => ClsCusTitle.CusTitleInstance.Sync2Client(Connection, ConnectionUniqueId, sb))), ReturnedState.None, "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 86);
				line = 21;
				if (Connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 92);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Task.Run(() => ClsCustomer.CusInstance.Sync2Client(Connection, ConnectionUniqueId, 5, sb)), Task.Run(() => clsAccount.Instance.Sync2Client(Connection, Connection.Guid, 5)), Task.Run(() => clsDiscountCoupon.Instance.Sync2ClientAsync(Connection, ConnectionUniqueId, 5)), Task.Run(() => ClsDesc.DescInstance.Sync2Client(Connection, ConnectionUniqueId, 10)), Task.Run(() => clsSanadPrint.Instance.Sync2ClientAsync(Connection, ConnectionUniqueId, 5))), ReturnedState.None, "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 94);
				line = 22;
				sb.AppendLine($"Syncing..... //sync of customer has been finished! result:{!ret.HasError}");
				bool flag = !ret.HasError && Connection.ClientVersion >= 12 && Connection.Source != ENSource.WebsiteWordPress;
				if (flag && Connection.Version.ParseToLong(0L) < "6.2.43".ParseToLong(0L))
				{
					flag = false;
				}
				if (flag)
				{
					line = 30;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Task.Run(() => clsAghsatMGroup.Instance.Sync2ClientAsync(Connection, ConnectionUniqueId, sb, 10)), Task.Run(() => ClsAghsatShakhsi.AghsatShakhsiInstance.Sync2ClientAsync(Connection, ConnectionUniqueId, sb, 10)), Task.Run(() => ClsViewCheckM.ViewCheckMInstance.Sync2ClientAsync(Connection, ConnectionUniqueId, sb, 10)), Task.Run(() => ClsViewCheckSh.ViewCheckShInstance.Sync2ClientAsync(Connection, ConnectionUniqueId, sb, 10))), ReturnedState.None, "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 113);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsAghsatAshkhas.AghsatAshkhasInstance.Sync2ClientAsync(Connection, ConnectionUniqueId, sb, 10), ReturnedState.None, "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 121);
				}
				line = 35;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 125);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 128);
				ret.AddReturnedValue(ex, "", "SyncHesab", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Sync2Client.cs", 129);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(Connection?.cancellationToken ?? default(CancellationToken), Connection, sb);
			}
			return ret;
		}
	}
}
