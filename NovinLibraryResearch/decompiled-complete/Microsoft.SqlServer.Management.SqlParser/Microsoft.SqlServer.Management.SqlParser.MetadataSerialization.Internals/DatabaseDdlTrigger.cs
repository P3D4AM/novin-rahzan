using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class DatabaseDdlTrigger : IDatabaseDdlTrigger, ITrigger, IMetadataObject, IDatabaseOwnedObject, IDatabaseObject
{
	private readonly DeserializationModel model;

	private readonly string bodyText;

	private readonly int database;

	private readonly ITriggerEventTypeSet databaseDdlEvents;

	private readonly int executionContext;

	private readonly bool isEnabled;

	private readonly bool isEncrypted;

	private readonly bool isQuotedIdentifierOn;

	private readonly bool isSqlClr;

	private readonly bool isSystemObject;

	private readonly string name;

	ITriggerEventTypeSet IDatabaseDdlTrigger.DatabaseDdlEvents => databaseDdlEvents;

	bool IDatabaseDdlTrigger.IsQuotedIdentifierOn => isQuotedIdentifierOn;

	string ITrigger.BodyText => bodyText;

	bool ITrigger.IsEncrypted => isEncrypted;

	bool ITrigger.IsEnabled => isEnabled;

	bool ITrigger.IsSqlClr => isSqlClr;

	IExecutionContext ITrigger.ExecutionContext => model.GetObjectById<IExecutionContext>(executionContext);

	string IMetadataObject.Name => name;

	IDatabase IDatabaseOwnedObject.Database => model.GetObjectById<IDatabase>(database);

	IDatabaseObject IDatabaseObject.Parent => ((IDatabaseOwnedObject)this).Database;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	public DatabaseDdlTrigger(DeserializationModel model, string bodyText, int database, ITriggerEventTypeSet databaseDdlEvents, int executionContext, bool isEnabled, bool isEncrypted, bool isQuotedIdentifierOn, bool isSqlClr, bool isSystemObject, string name)
	{
		this.model = model;
		this.bodyText = bodyText;
		this.database = database;
		this.databaseDdlEvents = databaseDdlEvents;
		this.executionContext = executionContext;
		this.isEnabled = isEnabled;
		this.isEncrypted = isEncrypted;
		this.isQuotedIdentifierOn = isQuotedIdentifierOn;
		this.isSqlClr = isSqlClr;
		this.isSystemObject = isSystemObject;
		this.name = name;
	}

	public DatabaseDdlTrigger(DeserializationModel model, object[] values)
		: this(model, (string)values[0], (int)values[1], (ITriggerEventTypeSet)values[2], (int)values[3], (bool)values[4], (bool)values[5], (bool)values[6], (bool)values[7], (bool)values[8], (string)values[9])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IDatabaseOwnedObject)this).Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}

	T IDatabaseOwnedObject.Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
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
		return ((IDatabaseOwnedObject)this).Accept((IDatabaseOwnedObjectVisitor<T>)visitor);
	}
}
