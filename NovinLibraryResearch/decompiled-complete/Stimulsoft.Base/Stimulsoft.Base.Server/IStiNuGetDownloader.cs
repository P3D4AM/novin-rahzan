using System;
using System.Threading.Tasks;

namespace Stimulsoft.Base.Server;

public interface IStiNuGetDownloader
{
	Task RunDownloaderAsync(StiDataConnector connector, IntPtr intPtr);

	void RunDownloadSvg();
}
