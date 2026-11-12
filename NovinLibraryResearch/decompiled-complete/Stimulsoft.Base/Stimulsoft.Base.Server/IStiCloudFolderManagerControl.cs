using System;
using System.Threading.Tasks;

namespace Stimulsoft.Base.Server;

public interface IStiCloudFolderManagerControl
{
	string SelectedReportItemKey { get; }

	event EventHandler SelectedReportMouseDoubleClick;

	Task<IStiReport> GetSelectedReport();

	void Refresh();

	void Refresh(IntPtr intPtr, object dispatcher);
}
