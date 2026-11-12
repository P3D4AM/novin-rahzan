using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class DmlTrigger : MetadataObjectBase, IMutableDmlTrigger, IDmlTrigger, ITrigger, IMetadataObject, IMutableTrigger, IMutableMetadataObject
{
	[Flags]
	private enum TriggerOptions
	{
		NotForReplication = 1,
		InsteadOf = 2,
		Insert = 4,
		Update = 8,
		Delete = 0x10,
		IsEncrypted = 0x20,
		IsEnabled = 0x40,
		IsSqlClr = 0x80
	}

	private ITableViewBase parent;

	private TriggerOptions options;

	private string bodyText;

	private IExecutionContext executionContext;

	private bool isQuotedIdentifierOn;

	private ActivationOrder insertActivationOrder;

	private ActivationOrder updateActivationOrder;

	private ActivationOrder deleteActivationOrder;

	public bool NotForReplication
	{
		get
		{
			return GetOption(TriggerOptions.NotForReplication);
		}
		set
		{
			SetOption(TriggerOptions.NotForReplication, value);
		}
	}

	public bool InsteadOf
	{
		get
		{
			return GetOption(TriggerOptions.InsteadOf);
		}
		set
		{
			SetOption(TriggerOptions.InsteadOf, value);
		}
	}

	public bool Insert
	{
		get
		{
			return GetOption(TriggerOptions.Insert);
		}
		set
		{
			SetOption(TriggerOptions.Insert, value);
		}
	}

	public ActivationOrder InsertActivationOrder
	{
		get
		{
			return insertActivationOrder;
		}
		set
		{
			insertActivationOrder = value;
		}
	}

	public bool Update
	{
		get
		{
			return GetOption(TriggerOptions.Update);
		}
		set
		{
			SetOption(TriggerOptions.Update, value);
		}
	}

	public ActivationOrder UpdateActivationOrder
	{
		get
		{
			return updateActivationOrder;
		}
		set
		{
			updateActivationOrder = value;
		}
	}

	public bool Delete
	{
		get
		{
			return GetOption(TriggerOptions.Delete);
		}
		set
		{
			SetOption(TriggerOptions.Delete, value);
		}
	}

	public ActivationOrder DeleteActivationOrder
	{
		get
		{
			return deleteActivationOrder;
		}
		set
		{
			deleteActivationOrder = value;
		}
	}

	public bool IsQuotedIdentifierOn
	{
		get
		{
			return isQuotedIdentifierOn;
		}
		set
		{
			isQuotedIdentifierOn = value;
		}
	}

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
			return GetOption(TriggerOptions.IsEncrypted);
		}
		set
		{
			SetOption(TriggerOptions.IsEncrypted, value);
		}
	}

	public bool IsEnabled
	{
		get
		{
			return GetOption(TriggerOptions.IsEnabled);
		}
		set
		{
			SetOption(TriggerOptions.IsEnabled, value);
		}
	}

	public bool IsSqlClr
	{
		get
		{
			return GetOption(TriggerOptions.IsSqlClr);
		}
		set
		{
			SetOption(TriggerOptions.IsSqlClr, value);
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

	public ITableViewBase Parent => parent;

	public DmlTrigger(ITableViewBase parent, string name)
		: base(name)
	{
		this.parent = parent;
		isQuotedIdentifierOn = true;
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	private bool GetOption(TriggerOptions option)
	{
		return (options & option) == option;
	}

	private void SetOption(TriggerOptions option, bool value)
	{
		options = (value ? (options | option) : (options & ~option));
	}
}
