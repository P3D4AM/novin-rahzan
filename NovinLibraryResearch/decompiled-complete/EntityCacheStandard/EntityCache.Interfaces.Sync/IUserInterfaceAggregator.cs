using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Core;
using EntityCache.Entities;
using EntityCache.Interfaces.Notification;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace EntityCache.Interfaces.Sync;

public interface IUserInterfaceAggregator
{
	MasterSendData2ServerAggregator MasterSendData2Server { get; set; }

	SendData2ServerAggregator SendData2Server { get; set; }

	IDbMigrator DbMigrator { get; set; }

	NotificationAggregator Notification { get; set; }

	TransferDefault TransferDefault { get; set; }

	string ExeVersion { get; set; }

	IImageResizer ImageResizer { get; set; }

	string AppPathFolder { get; set; }

	Task AfterCacheInitialized();

	Task<long> GetLkSerialAsync(string contextGuid);

	AggregateSqlRepository GetPersistenceAggregator(string connectionString, WebContextHolder holder, List<IDatabaseManager> dbContexts);
}
