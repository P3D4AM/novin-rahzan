using System;
using System.Threading.Tasks;

namespace Stimulsoft.Base.Server;

public interface IStiCloudOpenControl
{
	string ReportItemName { get; set; }

	string ReportItemKey { get; set; }

	event EventHandler SelectedItem;

	Task RefreshAsync();
}
