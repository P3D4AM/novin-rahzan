using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business;

public class ProductAttributeRelationBusiness : Serializable<ProductAttributeRelationBusiness>, IProductAttributeRelation, IHasContext
{
	public Guid ProductGuid { get; set; }

	public Guid AttributeValueGuid { get; set; }

	public Guid AttributeGuid { get; set; }

	public string ContextGuid { get; set; }

	public ProductAttributeRelationBusiness(string contexGuid)
	{
		ContextGuid = contexGuid;
	}

	public static Task<IEnumerable<ProductAttributeRelationModel>> GetAllAsync(string contextGuid, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductAttributeRelationRepository.GetAllModelAsync(productGuid, token, tranName);
	}

	public static Task<IEnumerable<ProductAttributeRelationBusiness>> GetAllBtAttributeValueAsync(string contextGuid, Guid attributeValueGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductAttributeRelationRepository.GetAllBtAttributeValueAsync(attributeValueGuid, token, tranName);
	}
}
