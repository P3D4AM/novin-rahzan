using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ScalarValuedFunction : UserDefinedFunction, IMutableScalarValuedFunction, IScalarValuedFunction, IUserDefinedFunction, IFunction, IMetadataObject, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject, ICallableModule, IScalarFunction, IScalar, IMutableUserDefinedFunction, IMutableFunction, IMutableFunctionModuleBase, IMutableMetadataObject, IMutableUserDefinedFunctionModuleBase, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableCallableModule, IMutableScalarFunction, IMutableScalar
{
	private bool m_returnsNullOnNullInput;

	private IScalarDataType m_returnType;

	public CallableModuleType ModuleType => CallableModuleType.ScalarFunction;

	public IScalarDataType ReturnType
	{
		get
		{
			return m_returnType;
		}
		set
		{
			m_returnType = value;
		}
	}

	public ScalarType ScalarType => ScalarType.ScalarFunction;

	public IScalarDataType DataType
	{
		get
		{
			return m_returnType;
		}
		set
		{
			m_returnType = value;
		}
	}

	public bool Nullable
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	public bool IsAggregateFunction => false;

	public bool ReturnsNullOnNullInput
	{
		get
		{
			return m_returnsNullOnNullInput;
		}
		set
		{
			m_returnsNullOnNullInput = value;
		}
	}

	public ScalarValuedFunction(ISchema schema, string name)
		: base(schema, name)
	{
	}

	public override T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
