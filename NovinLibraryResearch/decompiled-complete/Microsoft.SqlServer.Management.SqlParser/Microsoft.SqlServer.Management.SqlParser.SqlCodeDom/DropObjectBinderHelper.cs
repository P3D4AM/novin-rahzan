using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal static class DropObjectBinderHelper
{
	private delegate SqlIdentifier GetObjectNameDelegate<N>(N dropObject);

	private abstract class DropObjectBinderHelperBase<T, N> : IDropObjectBinderHelper<T, N> where T : class, IMetadataObject where N : SqlCodeObject
	{
		private readonly IDropObjectStatement<T, N> dropStatement;

		private readonly GetObjectNameDelegate<N> getObjectName;

		public ISqlCodeCollection<N> DropObjects => dropStatement.DropObjects;

		public DropObjectBinderHelperBase(IDropObjectStatement<T, N> dropStatement, GetObjectNameDelegate<N> getObjectName)
		{
			this.dropStatement = dropStatement;
			this.getObjectName = getObjectName;
		}

		public SqlIdentifier GetObjectName(N dropObject)
		{
			return getObjectName(dropObject);
		}

		public T FindDropMetadataObject(BatchBindingContext bindingContext, N name, LexicalScope lexicalScope)
		{
			return dropStatement.FindMetadataObject(bindingContext, name, lexicalScope);
		}

		public abstract IMetadataObject FindAnyMetadataObject(BatchBindingContext bindingContext, N name, LexicalScope lexicalScope, out DdlTargetObjectType actualType);

		public void ExecuteDrop(BatchBindingContext bindingContext, T dropObject, LexicalScope lexicalScope)
		{
			dropStatement.ExecuteDrop(bindingContext, dropObject, lexicalScope);
		}
	}

	private sealed class DropStandardBinderHelper<T, N> : DropObjectBinderHelperBase<T, N> where T : class, IMetadataObject where N : SqlCodeObject
	{
		public DropStandardBinderHelper(IDropObjectStatement<T, N> dropStatement, GetObjectNameDelegate<N> getObjectName)
			: base(dropStatement, getObjectName)
		{
		}

		public override IMetadataObject FindAnyMetadataObject(BatchBindingContext bindingContext, N name, LexicalScope lexicalScope, out DdlTargetObjectType actualType)
		{
			actualType = DdlTargetObjectType.None;
			return null;
		}
	}

	private sealed class DropSchemaOwnedObjectBinderHelper<T> : DropObjectBinderHelperBase<T, SqlObjectIdentifier> where T : class, ISchemaOwnedObject
	{
		public DropSchemaOwnedObjectBinderHelper(IDropObjectStatement<T, SqlObjectIdentifier> dropStatement)
			: base(dropStatement, SqlObjectIdentifierObjectName)
		{
		}

		public override IMetadataObject FindAnyMetadataObject(BatchBindingContext bindingContext, SqlObjectIdentifier name, LexicalScope lexicalScope, out DdlTargetObjectType actualType)
		{
			ISchemaOwnedObject schemaOwnedObject = bindingContext.FindSchemaOwnedObject(name.DatabaseName.Value, name.SchemaName.Value, name.ObjectName.Value, lexicalScope);
			actualType = ((schemaOwnedObject != null) ? RetrieveDdlTargetObjectType(schemaOwnedObject) : DdlTargetObjectType.None);
			return schemaOwnedObject;
		}
	}

	private sealed class DdlTargetObjectTypeVisitor : ISchemaOwnedObjectVisitor<DdlTargetObjectType>
	{
		public static readonly DdlTargetObjectTypeVisitor Instance = new DdlTargetObjectTypeVisitor();

		private DdlTargetObjectTypeVisitor()
		{
		}

		public DdlTargetObjectType Visit(IExtendedStoredProcedure extendedStoredProcedure)
		{
			return DdlTargetObjectType.Procedure;
		}

		public DdlTargetObjectType Visit(IScalarValuedFunction scalarValuedFunction)
		{
			return DdlTargetObjectType.Function;
		}

		public DdlTargetObjectType Visit(IStoredProcedure storedProcedure)
		{
			return DdlTargetObjectType.Procedure;
		}

		public DdlTargetObjectType Visit(ISynonym synonym)
		{
			return DdlTargetObjectType.Synonym;
		}

		public DdlTargetObjectType Visit(ITable table)
		{
			return DdlTargetObjectType.Table;
		}

		public DdlTargetObjectType Visit(ITableValuedFunction tableValuedFunction)
		{
			return DdlTargetObjectType.Function;
		}

		public DdlTargetObjectType Visit(IUserDefinedAggregate userDefinedAggregate)
		{
			return DdlTargetObjectType.None;
		}

		public DdlTargetObjectType Visit(IUserDefinedClrType userDefinedClrType)
		{
			return DdlTargetObjectType.None;
		}

		public DdlTargetObjectType Visit(IUserDefinedDataType userDefinedDataType)
		{
			return DdlTargetObjectType.None;
		}

		public DdlTargetObjectType Visit(IUserDefinedTableType userDefinedTableType)
		{
			return DdlTargetObjectType.None;
		}

		public DdlTargetObjectType Visit(IView view)
		{
			return DdlTargetObjectType.View;
		}
	}

	private static readonly GetObjectNameDelegate<SqlIdentifier> SqlIdentifierObjectName = GetObjectName;

	private static readonly GetObjectNameDelegate<SqlObjectIdentifier> SqlObjectIdentifierObjectName = GetObjectName;

	public static IDropObjectBinderHelper<T, SqlObjectIdentifier> GetSchemaOwnedObjectHelper<T>(IDropObjectStatement<T, SqlObjectIdentifier> dropStatement) where T : class, ISchemaOwnedObject
	{
		return new DropSchemaOwnedObjectBinderHelper<T>(dropStatement);
	}

	public static IDropObjectBinderHelper<T, SqlObjectIdentifier> GetStandardHelper<T>(IDropObjectStatement<T, SqlObjectIdentifier> dropStatement) where T : class, IMetadataObject
	{
		return new DropStandardBinderHelper<T, SqlObjectIdentifier>(dropStatement, SqlObjectIdentifierObjectName);
	}

	public static IDropObjectBinderHelper<T, SqlIdentifier> GetStandardHelper<T>(IDropObjectStatement<T, SqlIdentifier> dropStatement) where T : class, IMetadataObject
	{
		return new DropStandardBinderHelper<T, SqlIdentifier>(dropStatement, SqlIdentifierObjectName);
	}

	private static DdlTargetObjectType RetrieveDdlTargetObjectType(ISchemaOwnedObject schemaOwnedObject)
	{
		return schemaOwnedObject.Accept(DdlTargetObjectTypeVisitor.Instance);
	}

	private static SqlIdentifier GetObjectName(SqlIdentifier dropObject)
	{
		return dropObject;
	}

	private static SqlIdentifier GetObjectName(SqlObjectIdentifier dropObject)
	{
		return dropObject.ObjectName;
	}
}
