using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary;

public abstract class StiDataStoreAdapterService : StiDataAdapterService
{
	public override void SetDataSourceNames(StiData data, StiDataSource dataSource)
	{
		((StiDataStoreSource)dataSource).NameInSource = data.Name;
	}

	public override StiDataSource Create(StiDictionary dictionary, bool addToDictionary)
	{
		StiDataSource stiDataSource = StiActivator.CreateObject(GetDataSourceType()) as StiDataSource;
		if (stiDataSource != null && addToDictionary)
		{
			string name = (stiDataSource.Alias = StiNameCreation.CreateName(dictionary.Report, StiLocalization.Get("PropertyMain", "DataSource")));
			stiDataSource.Name = name;
			dictionary.DataSources.Add(stiDataSource);
		}
		return stiDataSource;
	}

	public override bool Edit(StiDictionary dictionary, StiDataSource dataSource)
	{
		return StiDataEditorsHelper.Get().DataStoreAdapterEdit(this, dictionary, dataSource);
	}

	public override bool New(StiDictionary dictionary, StiDataSource dataSource)
	{
		return StiDataEditorsHelper.Get().DataStoreAdapterNew(this, dictionary, dataSource);
	}
}
