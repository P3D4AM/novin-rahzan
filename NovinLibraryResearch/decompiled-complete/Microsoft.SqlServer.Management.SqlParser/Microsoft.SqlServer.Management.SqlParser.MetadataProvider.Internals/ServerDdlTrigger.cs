using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ServerDdlTrigger : ServerOwnedObject, IMutableServerDdlTrigger, IServerDdlTrigger, ITrigger, IMetadataObject, IServerOwnedObject, IDatabaseObject, IMutableTrigger, IMutableMetadataObject, IMutableServerOwnedObject, IMutableDatabaseObject
{
	private readonly TriggerEventTypeSet serverDdlEvents;

	private bool isEncrypted;

	private bool isEnabled;

	private string bodyText;

	private IExecutionContext executionContext;

	private bool isQuotedIdentifierOn;

	private bool isSqlClr;

	public IMutableTriggerEventTypeSet ServerDdlEvents => serverDdlEvents;

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

	ITriggerEventTypeSet IServerDdlTrigger.ServerDdlEvents => serverDdlEvents;

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

	public ServerDdlTrigger(IServer server, string name)
		: base(server, name)
	{
		serverDdlEvents = new TriggerEventTypeSet();
		isQuotedIdentifierOn = true;
	}

	public override T Accept<T>(IServerOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
