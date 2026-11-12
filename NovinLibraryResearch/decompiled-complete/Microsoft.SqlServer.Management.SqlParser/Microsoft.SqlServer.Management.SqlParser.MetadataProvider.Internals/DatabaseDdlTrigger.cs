using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class DatabaseDdlTrigger : DatabaseOwnedObject, IMutableDatabaseDdlTrigger, IDatabaseDdlTrigger, ITrigger, IMetadataObject, IDatabaseOwnedObject, IDatabaseObject, IMutableTrigger, IMutableMetadataObject, IMutableDatabaseOwnedObject, IMutableDatabaseObject
{
	private readonly TriggerEventTypeSet databaseDdlEvents;

	private bool isEncrypted;

	private bool isEnabled;

	private string bodyText;

	private IExecutionContext executionContext;

	private bool isQuotedIdentifierOn;

	private bool isSqlClr;

	public IMutableTriggerEventTypeSet DatabaseDdlEvents => databaseDdlEvents;

	public bool IsQuotedIdentifierOn
	{
		get
		{
			return isQuotedIdentifierOn;
		}
		set
		{
			isQuotedIdentifierOn = true;
		}
	}

	ITriggerEventTypeSet IDatabaseDdlTrigger.DatabaseDdlEvents => databaseDdlEvents;

	public string BodyText
	{
		get
		{
			return bodyText;
		}
		set
		{
			bodyText = value;
		}
	}

	public bool IsEncrypted
	{
		get
		{
			return isEncrypted;
		}
		set
		{
			isEncrypted = value;
		}
	}

	public bool IsEnabled
	{
		get
		{
			return isEnabled;
		}
		set
		{
			isEnabled = value;
		}
	}

	public bool IsSqlClr
	{
		get
		{
			return isSqlClr;
		}
		set
		{
			isSqlClr = value;
		}
	}

	public IExecutionContext ExecutionContext
	{
		get
		{
			return executionContext;
		}
		set
		{
			executionContext = value;
		}
	}

	public DatabaseDdlTrigger(IDatabase database, string name)
		: base(database, name)
	{
		databaseDdlEvents = new TriggerEventTypeSet();
		isQuotedIdentifierOn = true;
	}

	public override T Accept<T>(IDatabaseOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
