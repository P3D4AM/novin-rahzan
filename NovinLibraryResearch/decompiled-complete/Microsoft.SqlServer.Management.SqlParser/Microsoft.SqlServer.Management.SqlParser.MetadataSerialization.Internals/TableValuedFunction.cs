using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class TableValuedFunction : ITableValuedFunction, IDatabaseTable, ITabular, IMetadataObject, IUserDefinedFunction, IFunction, IFunctionModuleBase, IUserDefinedFunctionModuleBase, ISchemaOwnedObject, IDatabaseObject
{
	private readonly DeserializationModel model;

	private readonly string bodyText;

	private readonly int columns;

	private readonly int constraints;

	private readonly int executionContext;

	private readonly int indexes;

	private readonly bool isEncrypted;

	private readonly bool isInline;

	private readonly bool isQuotedIdentifierOn;

	private readonly bool isSchemaBound;

	private readonly bool isSqlClr;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int parameters;

	private readonly int schema;

	private readonly int statistics;

	private readonly string tableVariableName;

	bool ITableValuedFunction.IsInline => isInline;

	string ITableValuedFunction.TableVariableName => tableVariableName;

	CollationInfo IDatabaseTable.CollationInfo => ((ISchemaOwnedObject)this).Schema.Database.CollationInfo;

	IMetadataCollection<IConstraint> IDatabaseTable.Constraints => model.GetCollectionById<IConstraint>(constraints);

	IMetadataCollection<IIndex> IDatabaseTable.Indexes => model.GetCollectionById<IIndex>(indexes);

	IMetadataCollection<IStatistics> IDatabaseTable.Statistics => model.GetCollectionById<IStatistics>(statistics);

	TabularType ITabular.TabularType => TabularType.TableValuedFunction;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => model.GetOrderedCollectionById<IColumn>(columns);

	ITabular ITabular.Unaliased => this;

	string IMetadataObject.Name => name;

	string IUserDefinedFunction.BodyText => bodyText;

	bool IUserDefinedFunction.IsSchemaBound => isSchemaBound;

	bool IUserDefinedFunction.IsSqlClr => isSqlClr;

	bool IUserDefinedFunction.IsQuotedIdentifierOn => isQuotedIdentifierOn;

	IMetadataOrderedCollection<IParameter> IFunctionModuleBase.Parameters => model.GetOrderedCollectionById<IParameter>(parameters);

	bool IUserDefinedFunctionModuleBase.IsEncrypted => isEncrypted;

	IExecutionContext IUserDefinedFunctionModuleBase.ExecutionContext => model.GetObjectById<IExecutionContext>(executionContext);

	ISchema ISchemaOwnedObject.Schema => model.GetObjectById<ISchema>(schema);

	IDatabaseObject IDatabaseObject.Parent => ((ISchemaOwnedObject)this).Schema;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	public TableValuedFunction(DeserializationModel model, string bodyText, int columns, int constraints, int executionContext, int indexes, bool isEncrypted, bool isInline, bool isQuotedIdentifierOn, bool isSchemaBound, bool isSqlClr, bool isSystemObject, string name, int parameters, int schema, int statistics, string tableVariableName)
	{
		this.model = model;
		this.bodyText = bodyText;
		this.columns = columns;
		this.constraints = constraints;
		this.executionContext = executionContext;
		this.indexes = indexes;
		this.isEncrypted = isEncrypted;
		this.isInline = isInline;
		this.isQuotedIdentifierOn = isQuotedIdentifierOn;
		this.isSchemaBound = isSchemaBound;
		this.isSqlClr = isSqlClr;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.parameters = parameters;
		this.schema = schema;
		this.statistics = statistics;
		this.tableVariableName = tableVariableName;
	}

	public TableValuedFunction(DeserializationModel model, object[] values)
		: this(model, (string)values[0], (int)values[1], (int)values[2], (int)values[3], (int)values[4], (bool)values[5], (bool)values[6], (bool)values[7], (bool)values[8], (bool)values[9], (bool)values[10], (string)values[11], (int)values[12], (int)values[13], (int)values[14], (string)values[15])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}

	T ISchemaOwnedObject.Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	T IDatabaseObject.Accept<T>(IDatabaseObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((ISchemaOwnedObject)this).Accept((ISchemaOwnedObjectVisitor<T>)visitor);
	}
}
