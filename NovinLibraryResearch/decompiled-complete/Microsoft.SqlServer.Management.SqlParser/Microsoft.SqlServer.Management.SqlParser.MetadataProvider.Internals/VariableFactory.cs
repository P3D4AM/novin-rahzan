using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class VariableFactory : ObjectFactoryBase<VariableFactory>, IVariableFactory
{
	public IScalarVariable CreateScalarVariable(string name, IScalarDataType dataType)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		return new ScalarVariable(name, dataType);
	}

	public ITableVariable CreateTableVariable(string name, ITableDataType dataType)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		return new TableVariable(name, dataType);
	}

	public ICursorVariable CreateCursorVariable(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new CursorVariable(name);
	}
}
