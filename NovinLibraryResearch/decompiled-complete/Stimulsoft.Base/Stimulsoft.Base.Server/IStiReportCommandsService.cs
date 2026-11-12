using System.Threading.Tasks;

namespace Stimulsoft.Base.Server;

public interface IStiReportCommandsService
{
	Task<object> FindPublicAsync(string text);

	Task<byte[]> DownloadAsync(string itemKey);

	Task<byte[]> GetThumbnailAsync(string itemKey, string stateKey);
}
