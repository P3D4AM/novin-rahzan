using System;
using System.Data;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Dictionary;

public class StiBusinessObjectAdapterService : StiDataTableAdapterService
{
	public override string ServiceName => StiLocalization.Get("Adapters", "AdapterBusinessObjects");

	public override bool IsObjectAdapter => true;

	public override string GetDataCategoryName(StiData data)
	{
		if (data.ViewData is DataTable { DataSet: not null })
		{
			return base.GetDataCategoryName(data);
		}
		return ((StiService)this).ServiceName;
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiBusinessObjectSource);
	}

	public override Type[] GetDataTypes()
	{
		return new Type[1] { typeof(object) };
	}
}
