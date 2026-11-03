using System;
using System.ComponentModel;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiAzureBlobStorageSourceConverter))]
public class StiAzureBlobStorageSource : StiNoSqlSource
{
	public override StiComponentId ComponentId => StiComponentId.StiAzureBlobStorageSource;

	protected override Type GetDataAdapterType()
	{
		return typeof(StiAzureBlobStorageAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiAzureBlobStorageSource();
	}

	public StiAzureBlobStorageSource()
		: this(string.Empty, string.Empty, string.Empty)
	{
	}

	public StiAzureBlobStorageSource(string nameInSource, string name)
		: base(nameInSource, name)
	{
	}

	public StiAzureBlobStorageSource(string nameInSource, string name, string alias)
		: base(nameInSource, name, alias)
	{
	}

	public StiAzureBlobStorageSource(string nameInSource, string name, string alias, string sqlCommand)
		: base(nameInSource, name, alias, sqlCommand)
	{
	}

	public StiAzureBlobStorageSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart)
	{
	}

	public StiAzureBlobStorageSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow)
	{
	}

	public StiAzureBlobStorageSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout)
	{
	}

	public StiAzureBlobStorageSource(string nameInSource, string name, string alias, string sqlCommand, bool connectOnStart, bool reconnectOnEachRow, int commandTimeout, string key)
		: base(nameInSource, name, alias, sqlCommand, connectOnStart, reconnectOnEachRow, commandTimeout, key)
	{
	}
}
