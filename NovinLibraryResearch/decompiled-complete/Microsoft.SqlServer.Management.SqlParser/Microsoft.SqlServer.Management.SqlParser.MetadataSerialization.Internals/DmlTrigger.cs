using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class DmlTrigger : IDmlTrigger, ITrigger, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly string bodyText;

	private readonly bool delete;

	private readonly ActivationOrder deleteActivationOrder;

	private readonly int executionContext;

	private readonly bool insert;

	private readonly ActivationOrder insertActivationOrder;

	private readonly bool insteadOf;

	private readonly bool isEnabled;

	private readonly bool isEncrypted;

	private readonly bool isQuotedIdentifierOn;

	private readonly bool isSqlClr;

	private readonly string name;

	private readonly bool notForReplication;

	private readonly int parent;

	private readonly bool update;

	private readonly ActivationOrder updateActivationOrder;

	ITableViewBase IDmlTrigger.Parent => model.GetObjectById<ITableViewBase>(parent);

	bool IDmlTrigger.NotForReplication => notForReplication;

	bool IDmlTrigger.InsteadOf => insteadOf;

	bool IDmlTrigger.Insert => insert;

	ActivationOrder IDmlTrigger.InsertActivationOrder => insertActivationOrder;

	bool IDmlTrigger.Update => update;

	ActivationOrder IDmlTrigger.UpdateActivationOrder => updateActivationOrder;

	bool IDmlTrigger.Delete => delete;

	ActivationOrder IDmlTrigger.DeleteActivationOrder => deleteActivationOrder;

	bool IDmlTrigger.IsQuotedIdentifierOn => isQuotedIdentifierOn;

	string ITrigger.BodyText => bodyText;

	bool ITrigger.IsEncrypted => isEncrypted;

	bool ITrigger.IsEnabled => isEnabled;

	bool ITrigger.IsSqlClr => isSqlClr;

	IExecutionContext ITrigger.ExecutionContext => model.GetObjectById<IExecutionContext>(executionContext);

	string IMetadataObject.Name => name;

	public DmlTrigger(DeserializationModel model, string bodyText, bool delete, ActivationOrder deleteActivationOrder, int executionContext, bool insert, ActivationOrder insertActivationOrder, bool insteadOf, bool isEnabled, bool isEncrypted, bool isQuotedIdentifierOn, bool isSqlClr, string name, bool notForReplication, int parent, bool update, ActivationOrder updateActivationOrder)
	{
		this.model = model;
		this.bodyText = bodyText;
		this.delete = delete;
		this.deleteActivationOrder = deleteActivationOrder;
		this.executionContext = executionContext;
		this.insert = insert;
		this.insertActivationOrder = insertActivationOrder;
		this.insteadOf = insteadOf;
		this.isEnabled = isEnabled;
		this.isEncrypted = isEncrypted;
		this.isQuotedIdentifierOn = isQuotedIdentifierOn;
		this.isSqlClr = isSqlClr;
		this.name = name;
		this.notForReplication = notForReplication;
		this.parent = parent;
		this.update = update;
		this.updateActivationOrder = updateActivationOrder;
	}

	public DmlTrigger(DeserializationModel model, object[] values)
		: this(model, (string)values[0], (bool)values[1], (ActivationOrder)values[2], (int)values[3], (bool)values[4], (ActivationOrder)values[5], (bool)values[6], (bool)values[7], (bool)values[8], (bool)values[9], (bool)values[10], (string)values[11], (bool)values[12], (int)values[13], (bool)values[14], (ActivationOrder)values[15])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
