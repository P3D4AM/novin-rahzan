using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;

namespace EntityCache.Entities;

public interface IImageResizer
{
	Task<ReturnedSaveFuncInfo> ReSizeAsync(FileInfoBusiness file, int maxSize, int jpegQuality);

	Task<bool> ReSizeAsync(string path, int MaxSize);

	void LoadBitmapImage(string path);
}
