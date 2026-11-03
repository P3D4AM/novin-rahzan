using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary;

public abstract class StiFileAdapterService : StiDataStoreAdapterService
{
	public override string ServiceName => string.Format(StiLocalization.Get("Adapters", "AdapterConnection"), ((StiDataConnector)CreateConnector()).Name);

	public override string GetDataCategoryName(StiData data)
	{
		return data.Name;
	}

	public abstract StiFileDataConnector CreateConnector();
}
