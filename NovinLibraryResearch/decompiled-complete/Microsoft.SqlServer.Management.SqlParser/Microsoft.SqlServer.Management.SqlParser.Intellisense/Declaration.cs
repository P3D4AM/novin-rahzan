using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Intellisense;

public class Declaration : IComparable<Declaration>
{
	private class DeclarationTypeVisitor : IMetadataObjectVisitor<DeclarationType>, IDatabaseObjectVisitor<DeclarationType>, IServerOwnedObjectVisitor<DeclarationType>, IDatabaseOwnedObjectVisitor<DeclarationType>, ISchemaOwnedObjectVisitor<DeclarationType>
	{
		public static readonly DeclarationTypeVisitor Instance = new DeclarationTypeVisitor();

		private DeclarationTypeVisitor()
		{
		}

		public DeclarationType Visit(IApplicationRole applicationRole)
		{
			return DeclarationType.ApplicationRole;
		}

		public DeclarationType Visit(IAsymmetricKey asymmetricKey)
		{
			return DeclarationType.AsymmetricKey;
		}

		public DeclarationType Visit(IBuiltInFunction builtInFunction)
		{
			return DeclarationType.BuiltInFunction;
		}

		public DeclarationType Visit(ICertificate certificate)
		{
			return DeclarationType.Certificate;
		}

		public DeclarationType Visit(ICheckConstraint checkConstraint)
		{
			return DeclarationType.CheckConstraint;
		}

		public DeclarationType Visit(IColumn column)
		{
			return DeclarationType.Column;
		}

		public DeclarationType Visit(ICollation collation)
		{
			return DeclarationType.Collation;
		}

		public DeclarationType Visit(ICredential credential)
		{
			return DeclarationType.Credential;
		}

		public DeclarationType Visit(ICursorDataType cursorDataType)
		{
			return DeclarationType.CursorDataType;
		}

		public DeclarationType Visit(ICursorParameter cursorParameter)
		{
			return DeclarationType.CursorParameter;
		}

		public DeclarationType Visit(ICursorVariable cursorVariable)
		{
			return DeclarationType.CursorVariable;
		}

		public DeclarationType Visit(IDatabase database)
		{
			return DeclarationType.Database;
		}

		public DeclarationType Visit(IDatabaseDdlTrigger databaseDdlTrigger)
		{
			return DeclarationType.DatabaseDdlTrigger;
		}

		public DeclarationType Visit(IDatabasePermission databasePermission)
		{
			return DeclarationType.DatabasePermission;
		}

		public DeclarationType Visit(IDatabaseRole databaseRole)
		{
			return DeclarationType.DatabaseRole;
		}

		public DeclarationType Visit(IDatePart datePart)
		{
			return DeclarationType.DatePart;
		}

		public DeclarationType Visit(IDefaultConstraint datePart)
		{
			return DeclarationType.DefaultConstraint;
		}

		public DeclarationType Visit(IDmlTrigger dmlTrigger)
		{
			return DeclarationType.DmlTrigger;
		}

		public DeclarationType Visit(IExecutionContext executionContext)
		{
			return DeclarationType.ExecutionContext;
		}

		public DeclarationType Visit(IExtendedStoredProcedure extendedStoredProcedure)
		{
			return DeclarationType.ExtendedStoredProcedure;
		}

		public DeclarationType Visit(IFileGroup fileGroup)
		{
			return DeclarationType.FileGroup;
		}

		public DeclarationType Visit(IForeignKeyColumn foreignKeyColumn)
		{
			return DeclarationType.ForeignKeyColumn;
		}

		public DeclarationType Visit(IForeignKeyConstraint foreignKeyConstraint)
		{
			return DeclarationType.ForeignKeyConstraint;
		}

		public DeclarationType Visit(IIndexedColumn indexedColumn)
		{
			return DeclarationType.IndexedColumn;
		}

		public DeclarationType Visit(ILogin login)
		{
			return DeclarationType.Login;
		}

		public DeclarationType Visit(IPartitionScheme partitionScheme)
		{
			return DeclarationType.PartitionScheme;
		}

		public DeclarationType Visit(IPrimaryKeyConstraint primaryKeyConstraint)
		{
			return DeclarationType.PrimaryKeyConstraint;
		}

		public DeclarationType Visit(IRelationalIndex relationalIndex)
		{
			return DeclarationType.RelationalIndex;
		}

		public DeclarationType Visit(IStatistics statistics)
		{
			return DeclarationType.Statistics;
		}

		public DeclarationType Visit(IScalarExpression scalarExpression)
		{
			return DeclarationType.ScalarExpression;
		}

		public DeclarationType Visit(IScalarDataType scalarDataType)
		{
			return DeclarationType.UserDefinedDataType;
		}

		public DeclarationType Visit(IClrDataType scalarDataType)
		{
			return DeclarationType.UserDefinedDataType;
		}

		public DeclarationType Visit(IXmlDataType scalarDataType)
		{
			return DeclarationType.SystemDataType;
		}

		public DeclarationType Visit(IVoidDataType scalarDataType)
		{
			return DeclarationType.SystemDataType;
		}

		public DeclarationType Visit(ISystemClrDataType scalarDataType)
		{
			return DeclarationType.SystemDataType;
		}

		public DeclarationType Visit(IScalarParameter scalarParameter)
		{
			return DeclarationType.ScalarParameter;
		}

		public DeclarationType Visit(IScalarValuedFunction scalarValuedFunction)
		{
			return DeclarationType.ScalarValuedFunction;
		}

		public DeclarationType Visit(IScalarVariable scalarVariable)
		{
			return DeclarationType.ScalarVariable;
		}

		public DeclarationType Visit(ISchema schema)
		{
			return DeclarationType.Schema;
		}

		public DeclarationType Visit(IServer server)
		{
			return DeclarationType.Server;
		}

		public DeclarationType Visit(IServerDdlTrigger serverDdlTrigger)
		{
			return DeclarationType.ServerDdlTrigger;
		}

		public DeclarationType Visit(ISpatialIndex spatialIndex)
		{
			return DeclarationType.SpatialIndex;
		}

		public DeclarationType Visit(IStoredProcedure storedProcedure)
		{
			return DeclarationType.StoredProcedure;
		}

		public DeclarationType Visit(ISynonym synonym)
		{
			return DeclarationType.Synonym;
		}

		public DeclarationType Visit(ISystemDataType systemDataType)
		{
			return DeclarationType.SystemDataType;
		}

		public DeclarationType Visit(ITable table)
		{
			return DeclarationType.Table;
		}

		public DeclarationType Visit(ITableDataType tableDataType)
		{
			return DeclarationType.TableDataType;
		}

		public DeclarationType Visit(ITableParameter tableParameter)
		{
			return DeclarationType.TableParameter;
		}

		public DeclarationType Visit(ITableValuedFunction tableValuedFunction)
		{
			return DeclarationType.TableValuedFunction;
		}

		public DeclarationType Visit(ITableVariable tableVariable)
		{
			return DeclarationType.TableVariable;
		}

		public DeclarationType Visit(IUdtDataMember udtDataMember)
		{
			return DeclarationType.UdtDataMember;
		}

		public DeclarationType Visit(IUdtMethod udtMethod)
		{
			return DeclarationType.UdtMethod;
		}

		public DeclarationType Visit(IUniqueConstraint uniqueConstraint)
		{
			return DeclarationType.UniqueConstraint;
		}

		public DeclarationType Visit(IUser user)
		{
			return DeclarationType.User;
		}

		public DeclarationType Visit(IUserDefinedAggregate userDefinedAggregate)
		{
			return DeclarationType.UserDefinedAggregate;
		}

		public DeclarationType Visit(IUserDefinedClrType userDefinedClrType)
		{
			return DeclarationType.UserDefinedClrType;
		}

		public DeclarationType Visit(IUserDefinedDataType userDefinedDataType)
		{
			return DeclarationType.UserDefinedDataType;
		}

		public DeclarationType Visit(IUserDefinedTableType userDefinedTableType)
		{
			return DeclarationType.UserDefinedTableType;
		}

		public DeclarationType Visit(IView view)
		{
			return DeclarationType.View;
		}

		public DeclarationType Visit(IVirtualTable virtualTable)
		{
			return DeclarationType.VirtualTable;
		}

		public DeclarationType Visit(IXmlDataTypeMethod xmlDataTypeMethod)
		{
			return DeclarationType.XmlDataTypeMethod;
		}

		public DeclarationType Visit(IXmlDataTypeTableMethod xmlDataTypeTableMethod)
		{
			return DeclarationType.XmlDataTypeMethod;
		}

		public DeclarationType Visit(IXmlIndex xmlIndex)
		{
			return DeclarationType.XmlIndex;
		}
	}

	private readonly IMetadataObject metadataObject;

	private readonly IMetadataDisplayInfoProvider displayInfoProvider;

	private readonly string title;

	public string Title => title;

	public string Description => displayInfoProvider.GetDescription(metadataObject);

	public string DatabaseQualifiedName => displayInfoProvider.GetDatabaseQualifiedName(metadataObject);

	public DeclarationType Type => metadataObject.Accept(DeclarationTypeVisitor.Instance);

	internal Declaration(IMetadataObject metadataObject, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		this.metadataObject = metadataObject;
		this.displayInfoProvider = displayInfoProvider;
		title = displayInfoProvider.GetDisplayName(metadataObject);
	}

	public override string ToString()
	{
		return Description;
	}

	public int CompareTo(Declaration other)
	{
		if (other == null)
		{
			return 1;
		}
		int num = string.Compare(Title, other.Title, StringComparison.Ordinal);
		if (num == 0)
		{
			num = Type - other.Type;
		}
		return num;
	}
}
