namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class CreateTypeStatementInfo
{
	private enum CreateTypeStatementInfoType
	{
		UserDefinedDataType,
		UserDefinedType,
		UserDefinedTableType
	}

	private readonly SqlDataTypeSpecification dataType;

	private readonly SqlUserDefinedDataTypeNullState nullState;

	private readonly SqlClrClassSpecifier clrClassSpecifier;

	private readonly SqlTableDefinition tableDefinition;

	private readonly CreateTypeStatementInfoType InfoType;

	public SqlDataTypeSpecification DataType => dataType;

	public SqlUserDefinedDataTypeNullState NullState => nullState;

	public SqlClrClassSpecifier ClrClassSpecifier => clrClassSpecifier;

	public SqlTableDefinition TableDefinition => tableDefinition;

	public CreateTypeStatementInfo(SqlDataTypeSpecification dataType, SqlUserDefinedDataTypeNullState nullState)
	{
		this.dataType = dataType;
		this.nullState = nullState;
		InfoType = CreateTypeStatementInfoType.UserDefinedDataType;
	}

	public CreateTypeStatementInfo(SqlClrClassSpecifier clrClassSpecifier)
	{
		this.clrClassSpecifier = clrClassSpecifier;
		InfoType = CreateTypeStatementInfoType.UserDefinedType;
	}

	public CreateTypeStatementInfo(SqlTableDefinition tableDefinition)
	{
		this.tableDefinition = tableDefinition;
		InfoType = CreateTypeStatementInfoType.UserDefinedTableType;
	}

	public SqlCreateTypeStatement GetSqlCreateTypeStatement(SqlObjectIdentifier name)
	{
		return InfoType switch
		{
			CreateTypeStatementInfoType.UserDefinedDataType => new SqlCreateUserDefinedDataTypeStatement(name, dataType, nullState), 
			CreateTypeStatementInfoType.UserDefinedType => new SqlCreateUserDefinedTypeStatement(name, clrClassSpecifier), 
			CreateTypeStatementInfoType.UserDefinedTableType => new SqlCreateUserDefinedTableTypeStatement(name, tableDefinition), 
			_ => null, 
		};
	}

	private bool IsOfType(CreateTypeStatementInfoType infoType)
	{
		if (InfoType != infoType)
		{
			return false;
		}
		switch (infoType)
		{
		case CreateTypeStatementInfoType.UserDefinedDataType:
			if (dataType == null || clrClassSpecifier != null || tableDefinition != null)
			{
				return false;
			}
			break;
		case CreateTypeStatementInfoType.UserDefinedTableType:
			if (dataType != null || clrClassSpecifier != null || tableDefinition == null)
			{
				return false;
			}
			break;
		case CreateTypeStatementInfoType.UserDefinedType:
			if (dataType != null || clrClassSpecifier == null || tableDefinition != null)
			{
				return false;
			}
			break;
		default:
			return false;
		}
		return true;
	}
}
