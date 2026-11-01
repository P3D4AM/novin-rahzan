using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using AccSqlServerPersistence.Persistence.Aggregator;
using EntityCache;
using EntityCache.Core;
using EntityCache.Entities;
using EntityCache.Interfaces.Notification;
using EntityCache.Interfaces.Sync;
using EntityCache.Utilities;
using FormCustom;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Properties;

namespace TimeAttendanceDAL;

public class UserInterfaceAggregator : IUserInterfaceAggregator
{
	public MasterSendData2ServerAggregator MasterSendData2Server { get; set; }

	public SendData2ServerAggregator SendData2Server { get; set; }

	public IDbMigrator DbMigrator { get; set; }

	public TransferDefault TransferDefault { get; set; }

	public string ExeVersion { get; set; }

	public NotificationAggregator Notification { get; set; }

	public IImageResizer ImageResizer { get; set; }

	public string AppPathFolder { get; set; }

	public long LkSerial => RegistryManager.GetLkSerial();

	public UserInterfaceAggregator()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		MasterSendData2Server = null;
		SendData2Server = null;
		DbMigrator = null;
		TransferDefault = new TransferDefault(false);
		SetImageResource();
	}

	public AggregateSqlRepository GetPersistenceAggregator(string connectionString, WebContextHolder holder, List<IDatabaseManager> dbContexts)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		PersistenceAggregatorBajeVersion6 val = new PersistenceAggregatorBajeVersion6(connectionString, holder);
		((BaseAggregator)val).SetAggregator(dbContexts);
		return (AggregateSqlRepository)val;
	}

	private void SetImageResource()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0095: Expected O, but got Unknown
		try
		{
			ImageConverter val = new ImageConverter();
			ImageResourceManager.ServerStatusSent = (byte[])((TypeConverter)val).ConvertTo((object?)Resources.ServerStatus_Sent, typeof(byte[]));
			ImageResourceManager.ServerStatusSentError = (byte[])((TypeConverter)val).ConvertTo((object?)Resources.ServerStatus_SendError, typeof(byte[]));
			ImageResourceManager.ServerStatusNone = (byte[])((TypeConverter)val).ConvertTo((object?)Resources.ServerStatus_None, typeof(byte[]));
			ImageResourceManager.ServerStatusDeliveryFailed = (byte[])((TypeConverter)val).ConvertTo((object?)Resources.ServerStatus_DeliveryFailed, typeof(byte[]));
			ImageResourceManager.ServerStatusDelivered = (byte[])((TypeConverter)val).ConvertTo((object?)Resources.ServerStatus_Delivered, typeof(byte[]));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public async Task AfterCacheInitialized()
	{
	}

	public async Task<long> GetLkSerialAsync(string contextGuid)
	{
		try
		{
			return RegistryManager.GetLkSerial();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
