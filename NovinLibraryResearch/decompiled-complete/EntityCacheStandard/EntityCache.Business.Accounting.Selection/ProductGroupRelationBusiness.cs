using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Selection;

namespace EntityCache.Business.Accounting.Selection;

public class ProductGroupRelationBusiness : Serializable<ProductGroupRelationBusiness>, IProductGroupRelation, IHasContext
{
	public string ContextGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid GroupGuid { get; set; }

	public short Ordinal { get; set; }

	public ProductGroupRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<ProductGroupRelationBusiness>> GetAllAsync(string contextGuid, Guid? productGuid, Guid? groupGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.PrdGroupRelationRepository.GetAllAsync(productGuid, groupGuid, token, tranName);
	}

	public static Task<IEnumerable<ProductGroupRelationBusiness>> GetAllAsync(string contextGuid, List<Guid> groupGuidList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.PrdGroupRelationRepository.GetAllAsync(groupGuidList, token, tranName);
	}

	public static Task<ConcurrentDictionary<Guid, List<Guid>>> GetDictionaryAsync(string contextGuid, List<Guid> productGuidList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.PrdGroupRelationRepository.GetDictionaryAsync(productGuidList, token, tranName);
	}
}
