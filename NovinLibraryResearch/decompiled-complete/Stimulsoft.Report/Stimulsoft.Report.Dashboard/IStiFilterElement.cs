using Stimulsoft.Base;

namespace Stimulsoft.Report.Dashboard;

public interface IStiFilterElement : IStiAppCell, IStiConvertibleElement
{
	string GetParentKey();

	void SetParentKey(string key);

	void ApplyDefaultFilters();
}
