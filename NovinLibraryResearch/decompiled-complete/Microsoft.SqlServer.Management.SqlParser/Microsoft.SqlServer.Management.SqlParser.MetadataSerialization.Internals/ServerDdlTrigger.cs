using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ServerDdlTrigger : IServerDdlTrigger, ITrigger, IMetadataObject, IServerOwnedObject, IDatabaseObject
{
	private readonly DeserializationModel model;

	private readonly string bodyText;

	private readonly int executionContext;

	private readonly bool isEnabled;

	private readonly bool isEncrypted;

	private readonly bool isQuotedIdentifierOn;

	private readonly bool isSqlClr;

	private readonly bool isSystemObject;

	private readonly string name;

	private readonly int server;

	private readonly ITriggerEventTypeSet serverDdlEvents;

	ITriggerEventTypeSet IServerDdlTrigger.ServerDdlEvents => serverDdlEvents;

	bool IServerDdlTrigger.IsQuotedIdentifierOn => isQuotedIdentifierOn;

	string ITrigger.BodyText => bodyText;

	bool ITrigger.IsEncrypted => isEncrypted;

	bool ITrigger.IsEnabled => isEnabled;

	bool ITrigger.IsSqlClr => isSqlClr;

	IExecutionContext ITrigger.ExecutionContext => model.GetObjectById<IExecutionContext>(executionContext);

	string IMetadataObject.Name => name;

	IServer IServerOwnedObject.Server => model.GetObjectById<IServer>(server);

	IDatabaseObject IDatabaseObject.Parent => ((IServerOwnedObject)this).Server;

	bool IDatabaseObject.IsSystemObject => isSystemObject;

	public ServerDdlTrigger(DeserializationModel model, string bodyText, int executionContext, bool isEnabled, bool isEncrypted, bool isQuotedIdentifierOn, bool isSqlClr, bool isSystemObject, string name, int server, ITriggerEventTypeSet serverDdlEvents)
	{
		this.model = model;
		this.bodyText = bodyText;
		this.executionContext = executionContext;
		this.isEnabled = isEnabled;
		this.isEncrypted = isEncrypted;
		this.isQuotedIdentifierOn = isQuotedIdentifierOn;
		this.isSqlClr = isSqlClr;
		this.isSystemObject = isSystemObject;
		this.name = name;
		this.server = server;
		this.serverDdlEvents = serverDdlEvents;
	}

	public ServerDdlTrigger(DeserializationModel model, object[] values)
		: this(model, (string)values[0], (int)values[1], (bool)values[2], (bool)values[3], (bool)values[4], (bool)values[5], (bool)values[6], (string)values[7], (int)values[8], (ITriggerEventTypeSet)values[9])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return ((IServerOwnedObject)this).Accept((IServerOwnedObjectVisitor<T>)visitor);
	}

	T IServerOwnedObject.Accept<T>(IServerOwnedObjectVisitor<T> visitor)
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
		return ((IServerOwnedObject)this).Accept((IServerOwnedObjectVisitor<T>)visitor);
	}
}
