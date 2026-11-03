using System;
using System.Data;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Dictionary;

public class StiDataTableSetNameService : StiService
{
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryDictionary");

	public sealed override Type ServiceType => typeof(StiDataTableSetNameService);

	public void SetName(StiDataTableSource dataTableSource, StiReport report, DataSet dataSet, string name)
	{
		dataTableSource.NameInSource = dataSet.DataSetName + "." + name;
		dataTableSource.Name = name;
		dataTableSource.Alias = name;
	}
}
