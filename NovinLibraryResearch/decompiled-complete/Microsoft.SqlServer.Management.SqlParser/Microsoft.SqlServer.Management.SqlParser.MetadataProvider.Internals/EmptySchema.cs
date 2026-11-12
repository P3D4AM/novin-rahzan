using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class EmptySchema : ISchema, IDatabaseOwnedObject, IDatabaseObject, IMetadataObject
{
	private readonly IDatabase m_database;

	private readonly string m_name;

	private readonly bool m_isSystemObject;

	public IDatabase Database => m_database;

	public IDatabasePrincipal Owner => null;

	public IMetadataCollection<ITable> Tables => Collection<ITable>.Empty;

	public IMetadataCollection<IView> Views => Collection<IView>.Empty;

	public IMetadataCollection<IUserDefinedAggregate> UserDefinedAggregates => Collection<IUserDefinedAggregate>.Empty;

	public IMetadataCollection<ITableValuedFunction> TableValuedFunctions => Collection<ITableValuedFunction>.Empty;

	public IMetadataCollection<IScalarValuedFunction> ScalarValuedFunctions => Collection<IScalarValuedFunction>.Empty;

	public IMetadataCollection<IStoredProcedure> StoredProcedures => Collection<IStoredProcedure>.Empty;

	public IMetadataCollection<ISynonym> Synonyms => Collection<ISynonym>.Empty;

	public IMetadataCollection<IExtendedStoredProcedure> ExtendedStoredProcedures => Collection<IExtendedStoredProcedure>.Empty;

	public IMetadataCollection<IUserDefinedDataType> UserDefinedDataTypes => Collection<IUserDefinedDataType>.Empty;

	public IMetadataCollection<IUserDefinedTableType> UserDefinedTableTypes => Collection<IUserDefinedTableType>.Empty;

	public IMetadataCollection<IUserDefinedClrType> UserDefinedClrTypes => Collection<IUserDefinedClrType>.Empty;

	public IDatabaseObject Parent => m_database;

	public bool IsSystemObject => m_isSystemObject;

	public string Name => m_name;

	public EmptySchema(IDatabase database, string name, bool isSystemObject)
	{
		m_database = database;
		m_name = name;
		m_isSystemObject = isSystemObject;
	}

	public T Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public T Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		return Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}

	public T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		return Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}
}
