using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalWooMapping
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebWooMapping wooMapping, string callerName);

	Task ResetByPartAsync(short depId, WooEntityType wooEntityType, short TryCount = 10);

	Task ResetAllPartAsync(short depId, short TryCount = 10);

	Task<WebWooMapping> GetAsync(WooEntityType entityType, Guid entityGuid, int depId);

	Task<WebWooMapping> GetAsync(WooEntityType entityType, int entityId, int depId);

	Task<int> GetLastIdAsync(short depId, WooEntityType entityType);

	Task<ReturnedSaveFuncInfo> DeleteAsync(WooEntityType entityType, short depId, Guid entityGuid);

	Task<ReturnedSaveFuncInfo> DeleteAsync(WooEntityType entityType, short depId, int entityId);

	Task<bool> UpdateCrcAsync(WebWooMapping webWooMapping);
}
