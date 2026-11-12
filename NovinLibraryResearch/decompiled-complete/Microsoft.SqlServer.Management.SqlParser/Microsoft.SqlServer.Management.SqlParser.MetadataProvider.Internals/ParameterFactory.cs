using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class ParameterFactory : ObjectFactoryBase<ParameterFactory>, IParameterFactory
{
	public IScalarParameter CreateScalarParameter(string name, IScalarDataType dataType)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		return new ScalarParameter(name, dataType);
	}

	public IScalarParameter CreateScalarParameter(string name, IScalarDataType dataType, bool isOutput, string defaultValue)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		return new ScalarParameter(name, dataType, isOutput, defaultValue);
	}

	public ITableParameter CreateTableParameter(string name, ITableDataType dataType)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		return new TableParameter(name, dataType);
	}

	public ICursorParameter CreateCursorParameter(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new CursorParameter(name);
	}
}
