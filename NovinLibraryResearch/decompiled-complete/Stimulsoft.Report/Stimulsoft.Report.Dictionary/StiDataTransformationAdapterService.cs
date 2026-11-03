using System;
using System.Data;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Dictionary;

public class StiDataTransformationAdapterService : StiDataStoreAdapterService
{
	public override string ServiceName => string.Format(Loc.Get("Adapters", "AdapterConnection"), Loc.Get("PropertyMain", "DataTransformation"));

	public override bool IsObjectAdapter => true;

	public override bool Edit(StiDictionary dictionary, StiDataSource dataSource)
	{
		throw new NotImplementedException();
	}

	public override bool New(StiDictionary dictionary, StiDataSource dataSource)
	{
		throw new NotImplementedException();
	}

	public override Type[] GetDataTypes()
	{
		return null;
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		return new StiDataColumnsCollection();
	}

	public override StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource)
	{
		return new StiDataParametersCollection();
	}

	public override string GetDataCategoryName(StiData data)
	{
		return ((StiService)this).ServiceName;
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiDataTransformation);
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		StiDataLeader.Disconnect(dataSource);
	}
}
