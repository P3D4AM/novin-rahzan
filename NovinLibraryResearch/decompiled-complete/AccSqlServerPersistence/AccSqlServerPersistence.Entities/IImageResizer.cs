using System.Threading.Tasks;
using EntityCache.Business;

namespace AccSqlServerPersistence.Entities;

public interface IImageResizer
{
	Task<bool> ReSizeAsync(FileInfoBusiness file, int maxSize);
}
