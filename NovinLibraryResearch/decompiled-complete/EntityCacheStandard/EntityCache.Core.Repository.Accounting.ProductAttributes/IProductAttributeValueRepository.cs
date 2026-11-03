using System;
using EntityCache.Business.Accounting.ProductAttrebutes;

namespace EntityCache.Core.Repository.Accounting.ProductAttributes;

public interface IProductAttributeValueRepository : IPersistenceMasterRepository<ProductAttributeValueBusiness>, IDisposable
{
}
