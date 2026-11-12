using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyCategory : SfcInstance, ISfcCreatable, ICreatable, ISfcDroppable, IDroppable, ISfcAlterable, IAlterable, ISfcRenamable, IRenamable
{
	public sealed class Key : SfcKey
	{
		private string keyName;

		public string Name => keyName;

		public Key()
		{
		}

		public Key(Key other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			keyName = other.Name;
		}

		public Key(string name)
		{
			keyName = name;
		}

		internal Key(Dictionary<string, object> filedDict)
		{
			keyName = (string)filedDict["Name"];
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public new static bool Equals(object obj1, object obj2)
		{
			return obj1 as Key == obj2 as Key;
		}

		public override bool Equals(SfcKey key)
		{
			return this == key;
		}

		public static bool operator ==(object obj, Key rightOperand)
		{
			if (obj == null || obj is Key)
			{
				return (Key)obj == rightOperand;
			}
			return false;
		}

		public static bool operator ==(Key leftOperand, object obj)
		{
			if (obj == null || obj is Key)
			{
				return leftOperand == (Key)obj;
			}
			return false;
		}

		public static bool operator ==(Key leftOperand, Key rightOperand)
		{
			if ((object)leftOperand == (object)rightOperand)
			{
				return true;
			}
			if ((object)leftOperand == null || (object)rightOperand == null)
			{
				return false;
			}
			return leftOperand.IsEqual(rightOperand);
		}

		public static bool operator !=(object obj, Key rightOperand)
		{
			return !(obj == rightOperand);
		}

		public static bool operator !=(Key leftOperand, object obj)
		{
			return !(leftOperand == obj);
		}

		public static bool operator !=(Key leftOperand, Key rightOperand)
		{
			return !(leftOperand == rightOperand);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		private bool IsEqual(Key key)
		{
			return string.Compare(Name, key.Name, StringComparison.Ordinal) == 0;
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@Name='{1}']", "PolicyCategory", SfcSecureString.EscapeSquote(Name));
		}
	}

	private sealed class ObjectFactory : SfcObjectFactory
	{
		private static readonly ObjectFactory instance;

		public static ObjectFactory Instance => instance;

		static ObjectFactory()
		{
			instance = new ObjectFactory();
		}

		private ObjectFactory()
		{
		}

		protected override SfcInstance CreateImpl()
		{
			return new PolicyCategory();
		}
	}

	private static TraceContext traceContext;

	private static readonly SfcTsqlProcFormatter scriptDropAction;

	private static readonly SfcTsqlProcFormatter scriptCreateAction;

	private static readonly SfcTsqlProcFormatter scriptRenameAction;

	private static readonly SfcTsqlProcFormatter scriptAlterAction;

	internal const string typeName = "PolicyCategory";

	public static string DefaultCategory => ExceptionTemplatesSR.DefaultCategory;

	[SfcProperty(ReadOnlyAfterCreation = true)]
	[SfcKey(0)]
	public string Name => (string)base.Properties["Name"].Value;

	[SfcProperty(Data = true, Required = false)]
	public int ID
	{
		get
		{
			object value = base.Properties["ID"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
	}

	[SfcProperty(Required = false, Data = true)]
	public bool MandateDatabaseSubscriptions
	{
		get
		{
			SfcProperty sfcProperty = base.Properties["MandateDatabaseSubscriptions"];
			if (sfcProperty.Value == null && base.State == SfcObjectState.Pending)
			{
				sfcProperty.Value = false;
			}
			return (bool)sfcProperty.Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting MandateDatabaseSubscriptions to: {0}", value);
			base.Properties["MandateDatabaseSubscriptions"].Value = value;
		}
	}

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new PolicyStore Parent
	{
		get
		{
			return (PolicyStore)base.Parent;
		}
		set
		{
			traceContext.TraceVerbose("Setting Parent to: {0}", value);
			base.Parent = value;
		}
	}

	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	static PolicyCategory()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "PolicyCategory");
		scriptDropAction = new SfcTsqlProcFormatter();
		scriptCreateAction = new SfcTsqlProcFormatter();
		scriptRenameAction = new SfcTsqlProcFormatter();
		scriptAlterAction = new SfcTsqlProcFormatter();
		scriptCreateAction.Procedure = "msdb.dbo.sp_syspolicy_add_policy_category";
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category_id", "ID", required: false, output: true));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("mandate_database_subscriptions", "MandateDatabaseSubscriptions", required: false, output: false));
		scriptDropAction.Procedure = "msdb.dbo.sp_syspolicy_delete_policy_category";
		scriptDropAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category_id", "ID", required: true, output: false));
		scriptRenameAction.Procedure = "msdb.dbo.sp_syspolicy_rename_policy_category";
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptRenameAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("new_name", required: true));
		scriptAlterAction.Procedure = "msdb.dbo.sp_syspolicy_update_policy_category";
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("name", "Name", required: true, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("mandate_database_subscriptions", "MandateDatabaseSubscriptions", required: false, output: false));
	}

	public PolicyCategory()
	{
	}

	public PolicyCategory(PolicyStore parent, string name)
	{
		traceContext.TraceMethodEnter("PolicyCategory");
		SetName(name);
		Parent = parent;
		MandateDatabaseSubscriptions = true;
		traceContext.TraceMethodExit("PolicyCategory");
	}

	private void SetName(string name)
	{
		traceContext.TraceMethodEnter("SetName");
		traceContext.TraceParameters(name);
		base.Properties["Name"].Value = name;
		traceContext.TraceMethodExit("SetName");
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	protected override SfcKey CreateIdentityKey()
	{
		Key result = null;
		if (Name != null)
		{
			result = new Key(Name);
		}
		return result;
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	protected override ISfcCollection GetChildCollection(string elementType)
	{
		throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	public void Create()
	{
		CreateImpl();
	}

	ISfcScript ISfcCreatable.ScriptCreate()
	{
		return new SfcTSqlScript(scriptCreateAction.GenerateScript(this));
	}

	protected override void PostCreate(object executionResult)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("PostCreate");
		methodTraceContext.TraceParameters(executionResult);
		if (GetDomain().ConnectionContext.Mode != 0)
		{
			base.Properties["ID"].Value = executionResult;
		}
	}

	public void Drop()
	{
		DropImpl();
	}

	ISfcScript ISfcDroppable.ScriptDrop()
	{
		return new SfcTSqlScript(scriptDropAction.GenerateScript(this));
	}

	public void Rename(string name)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("Rename", TraceEventType.Information);
		methodTraceContext.TraceParameters(name);
		if (string.IsNullOrEmpty(name))
		{
			throw methodTraceContext.TraceThrow(new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("Name")));
		}
		RenameImpl(new Key(name));
	}

	void ISfcRenamable.Rename(SfcKey key)
	{
		RenameImpl(key);
	}

	ISfcScript ISfcRenamable.ScriptRename(SfcKey key)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ISfcRenamable.ScriptRename");
		methodTraceContext.TraceParameters(key);
		List<SfcTsqlProcFormatter.RuntimeArg> list = new List<SfcTsqlProcFormatter.RuntimeArg>();
		Key key2 = key as Key;
		list.Add(new SfcTsqlProcFormatter.RuntimeArg(key2.Name.GetType(), key2.Name));
		string text = scriptRenameAction.GenerateScript(this, list);
		methodTraceContext.TraceVerbose("Script generated: " + text);
		return new SfcTSqlScript(text);
	}

	public void Alter()
	{
		traceContext.TraceMethodEnter("Alter");
		Validate();
		AlterImpl();
		traceContext.TraceMethodExit("Alter");
	}

	ISfcScript ISfcAlterable.ScriptAlter()
	{
		return new SfcTSqlScript(scriptAlterAction.GenerateScript(this));
	}
}
