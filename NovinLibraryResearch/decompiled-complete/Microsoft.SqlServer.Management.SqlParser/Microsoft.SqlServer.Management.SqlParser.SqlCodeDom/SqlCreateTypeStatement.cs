using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCreateTypeStatement : SqlDdlStatement
{
	private UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType> boundDataType;

	private readonly SqlObjectIdentifier name;

	internal sealed override string StatementPhrase => "CREATE TYPE";

	internal sealed override bool IsCreateStatement => true;

	internal sealed override bool IsAlterStatement => false;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Type;

	public IUserDefinedType BoundDataType => boundDataType.AsUserDefinedType;

	public override IMetadataObject BoundObject => BoundDataType;

	public SqlObjectIdentifier Name => name;

	internal abstract void BindType(BatchBindingContext bindingContext, UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType> userDefinedType);

	internal abstract UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType> CreateType(IUserDefinedTypeFactory factory, ISchema schema);

	internal sealed override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		string value = Name.SchemaName.Value;
		string value2 = Name.ObjectName.Value;
		LexicalScope lexicalScope = GetLexicalScope();
		SchemaEx cDASchema = bindingContext.GetCDASchema(value, lexicalScope);
		if (cDASchema.IsEmptySchema)
		{
			if (value != null)
			{
				Name.SchemaName.ReportBindError(SR.SchemaNotExist(value), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
			base.BindingState |= BindingState.BindFailed;
			return;
		}
		IUserDefinedTypeFactory userDefinedType = bindingContext.MetadataFactory.UserDefinedType;
		SchemaEx asSingleSchema = cDASchema.AsSingleSchema;
		boundDataType = CreateType(userDefinedType, asSingleSchema.ProxyObject);
		BindType(bindingContext, boundDataType);
		if (BatchBindingContext.FindUserDefinedTypeInSchema(cDASchema, value2, lexicalScope) != null)
		{
			Name.ReportBindError(SR.UserDefinedTypeExist(value2));
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			ExecuteCreate(asSingleSchema, boundDataType, this, bindMode);
			base.BindingState |= BindingState.BindSucceeded;
		}
	}

	private static void ExecuteCreate(SchemaEx schema, UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType> userDefinedType, IBindFinalizer finalizer, StatementBindMode bindMode)
	{
		if (userDefinedType.IsUserDefinedDataType)
		{
			schema.UserDefinedDataTypes.Create(userDefinedType.UserDefinedDataType, finalizer, bindMode);
		}
		else if (userDefinedType.IsUserDefinedTableType)
		{
			schema.UserDefinedTableTypes.Create(userDefinedType.UserDefinedTableType, finalizer, bindMode);
		}
		else
		{
			schema.UserDefinedClrTypes.Create(userDefinedType.UserDefinedClrType, finalizer, bindMode);
		}
	}

	internal override void Unbind()
	{
		boundDataType = default(UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType>);
		base.Unbind();
	}

	internal SqlCreateTypeStatement(SqlObjectIdentifier name)
	{
		this.name = name;
	}

	internal static SqlCreateTypeStatement Create(SqlObjectIdentifier name, CreateTypeStatementInfo createTypeStatementInfo)
	{
		return createTypeStatementInfo.GetSqlCreateTypeStatement(name);
	}
}
