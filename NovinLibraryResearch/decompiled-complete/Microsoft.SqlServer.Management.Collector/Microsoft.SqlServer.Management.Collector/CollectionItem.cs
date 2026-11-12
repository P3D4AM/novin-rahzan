using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Collector;

public sealed class CollectionItem : SfcInstance, ISfcAlterable, IAlterable, ISfcRenamable, IRenamable, IMarkForDrop
{
	private class TypeMetadata : SfcTypeMetadata
	{
		private static readonly TypeMetadata instance;

		public static TypeMetadata Instance
		{
			[DebuggerStepThrough]
			get
			{
				return instance;
			}
		}

		static TypeMetadata()
		{
			instance = new TypeMetadata();
		}

		private TypeMetadata()
		{
		}

		public override bool IsCrudActionHandledByParent(SfcDependencyAction depAction)
		{
			switch (depAction)
			{
			case SfcDependencyAction.Create:
			case SfcDependencyAction.Drop:
			case SfcDependencyAction.Alter:
				return true;
			case SfcDependencyAction.Rename:
			case SfcDependencyAction.Move:
				return false;
			default:
				tc.TraceError("Action '{0}' not supported", depAction.ToString());
				throw new NotSupportedException(ExceptionTemplatesSR.UnsupportedAction);
			}
		}
	}

	public class Key : SfcKey
	{
		public string name;

		public string Name
		{
			get
			{
				return name;
			}
			protected set
			{
				name = value;
			}
		}

		public Key(Key other)
		{
			name = other.name;
		}

		public Key(string name)
		{
			this.name = name;
		}

		internal Key(Dictionary<string, object> filedDict)
		{
			name = (string)filedDict["Name"];
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public new bool Equals(object obj1, object obj2)
		{
			return obj1 == obj2;
		}

		public override bool Equals(SfcKey key)
		{
			return this == key;
		}

		public static bool operator ==(object obj, Key rhs)
		{
			bool flag = obj == null;
			bool flag2 = (object)rhs == null;
			if (flag && flag2)
			{
				return true;
			}
			if (!flag && !flag2 && (object)rhs != null)
			{
				return (Key)obj == rhs;
			}
			return false;
		}

		public static bool operator ==(Key lhs, object obj)
		{
			bool flag = (object)lhs == null;
			bool flag2 = obj == null;
			if (flag && flag2)
			{
				return true;
			}
			if (!flag && !flag2 && (object)lhs != null)
			{
				return (Key)obj == lhs;
			}
			return false;
		}

		public static bool operator ==(Key lhs, Key rhs)
		{
			if ((object)lhs == (object)rhs)
			{
				return true;
			}
			if ((object)lhs == null || (object)rhs == null)
			{
				return false;
			}
			return lhs.IsEqual(rhs);
		}

		public static bool operator !=(object obj, Key rhs)
		{
			return !(obj == rhs);
		}

		public static bool operator !=(Key lhs, object obj)
		{
			return !(lhs == obj);
		}

		public static bool operator !=(Key lhs, Key rhs)
		{
			return !(lhs == rhs);
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		public override string ToString()
		{
			return name;
		}

		private bool IsEqual(Key key)
		{
			return string.CompareOrdinal(name, key.name) == 0;
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@Name='{1}']", "CollectionItem", SfcSecureString.EscapeSquote(name));
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
			return new CollectionItem();
		}
	}

	private static TraceContext tc = TraceContext.GetTraceContext("DC_OM", "Microsoft.SqlServer.Management.Collector.CollectionItem");

	private object syncLock;

	internal const string typeName = "CollectionItem";

	[SfcProperty(SfcPropertyFlags.Data)]
	public int Id
	{
		get
		{
			object value = base.Properties["Id"].Value;
			if (value == null)
			{
				return 0;
			}
			return (int)value;
		}
		private set
		{
			tc.TraceVerbose("Setting Id to '{0}'", value);
			base.Properties["Id"].Value = value;
		}
	}

	[SfcProperty]
	[SfcKey(0)]
	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return base.Properties["Name"].Value as string;
		}
		set
		{
			tc.TraceVerbose("Setting Name to '{0}'", value);
			if (SfcObjectState.Pending == State)
			{
				CheckNewName(value);
				base.Properties["Name"].Value = value;
				return;
			}
			throw new CollectorException(ExceptionTemplatesSR.CannotSetObjectNameExistingObject);
		}
	}

	[SfcProperty]
	public string TypeName
	{
		[DebuggerStepThrough]
		get
		{
			return base.Properties["TypeName"].Value as string;
		}
		set
		{
			Helpers.CheckPropertyValueLength(value, 128, "TypeName");
			base.Properties["TypeName"].Value = value;
		}
	}

	[SfcProperty]
	public TimeSpan CollectionFrequency
	{
		get
		{
			object value = base.Properties["CollectionFrequency"].Value;
			if (value == null)
			{
				return new TimeSpan(0L);
			}
			return TimeSpan.FromSeconds((int)value);
		}
		set
		{
			TimeSpan timeSpan = value;
			double totalSeconds = timeSpan.TotalSeconds;
			tc.TraceVerbose("Setting CollectionFrequency to {0} sec", totalSeconds);
			if (totalSeconds > 2147483647.0)
			{
				throw new CollectorException(ExceptionTemplatesSR.CollectionFrequencyTooLarge(int.MaxValue));
			}
			SfcProperty sfcProperty = base.Properties["CollectionFrequency"];
			timeSpan = value;
			sfcProperty.Value = (int)timeSpan.TotalSeconds;
		}
	}

	[SfcProperty]
	public string Parameters
	{
		[DebuggerStepThrough]
		get
		{
			return base.Properties["Parameters"].Value as string;
		}
		set
		{
			base.Properties["Parameters"].Value = value;
		}
	}

	private object SyncLock
	{
		get
		{
			if (syncLock == null)
			{
				syncLock = Parent.SyncLock;
			}
			return syncLock;
		}
	}

	[SfcIgnore]
	internal new SfcObjectState State
	{
		[DebuggerStepThrough]
		get
		{
			return base.State;
		}
	}

	[SfcIgnore]
	public Key IdentityKey => (Key)base.AbstractIdentityKey;

	[SfcObject(SfcObjectRelationship.ParentObject, SfcObjectCardinality.One)]
	public new CollectionSet Parent
	{
		get
		{
			return (CollectionSet)base.Parent;
		}
		set
		{
			base.Parent = value;
		}
	}

	public CollectionItem(CollectionSet set, string name)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("CollectionItem");
		if (set == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.ParentCollectionSetCannotBeNull);
		}
		methodTraceContext.TraceParameterIn("name", name);
		methodTraceContext.TraceParameterIn("set", (set != null) ? set.Name : "(null)");
		Name = name;
		Parent = set;
		Id = 0;
		CollectionFrequency = new TimeSpan(-1L);
		syncLock = Parent.SyncLock;
	}

	public void Alter()
	{
		using (tc.GetMethodContext("Alter"))
		{
			lock (SyncLock)
			{
				AlterImpl();
			}
		}
	}

	ISfcScript ISfcAlterable.ScriptAlter()
	{
		using (tc.GetMethodContext("ScriptAlter"))
		{
			return new SfcTSqlScript(GetAlterScript());
		}
	}

	public void Rename(string newName)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("Rename");
		methodTraceContext.TraceParameterIn("newName", newName);
		CheckNewName(newName);
		lock (SyncLock)
		{
			RenameImpl(new Key(newName));
		}
	}

	void ISfcRenamable.Rename(SfcKey newKey)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("Rename");
		Key key = (Key)newKey;
		if (null == key)
		{
			throw new CollectorException(ExceptionTemplatesSR.NameNotNull);
		}
		methodTraceContext.TraceParameterIn("newKey", key);
		CheckNewName(key.Name);
		lock (SyncLock)
		{
			RenameImpl(newKey);
		}
	}

	ISfcScript ISfcRenamable.ScriptRename(SfcKey newKey)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("ScriptRename");
		Key key = newKey as Key;
		methodTraceContext.TraceParameterIn("newKey", (key != null) ? key.ToString() : "(null)");
		return new SfcTSqlScript(GetRenameScript(key));
	}

	public void MarkForDrop(bool dropOnAlter)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("MarkForDrop");
		methodTraceContext.TraceParameterIn("dropOnAlter", dropOnAlter);
		MarkForDropImpl(dropOnAlter);
	}

	public override void Discover(ISfcDependencyDiscoveryObjectSink sink)
	{
		SfcDependencyAction action = sink.Action;
		if ((uint)(action - 2) <= 4u)
		{
			return;
		}
		tc.TraceError("Action '{0}' not supported", sink.Action.ToString());
		throw new NotSupportedException(ExceptionTemplatesSR.UnsupportedAction);
	}

	internal static SfcTypeMetadata GetTypeMetadata()
	{
		return TypeMetadata.Instance;
	}

	internal string GetCreateScript(string setParamName)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("GetCreateScript");
		methodTraceContext.TraceParameterIn("setParamName", setParamName);
		if (Name == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionItemHasNoName);
		}
		if (TypeName == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionItemNoType(Name));
		}
		tc.TraceVerbose("Add script arguments");
		List<TsqlProcFormatter.ScriptTypeArg> list = new List<TsqlProcFormatter.ScriptTypeArg>();
		TsqlProcFormatter.ScriptTypeArg item = default(TsqlProcFormatter.ScriptTypeArg);
		item.argName = "collector_type_uid";
		item.type = typeof(Guid);
		item.script = "Select @{0} = collector_type_uid From [msdb].[dbo].[syscollector_collector_types] Where name = " + TsqlProcFormatter.MakeSqlString(TypeName);
		list.Add(item);
		tc.TraceVerbose("Create the script");
		TsqlProcFormatter obj = new TsqlProcFormatter
		{
			Procedure = "[msdb].[dbo].[sp_syscollector_create_collection_item]",
			Arguments = 
			{
				new TsqlProcFormatter.SprocArg("name", "Name", required: true, output: false),
				new TsqlProcFormatter.SprocArg("parameters", "Parameters", required: false, output: false),
				new TsqlProcFormatter.SprocArg("collection_item_id", "Id", required: true, output: true)
			}
		};
		tc.TraceVerbose("Runtime arguments");
		List<TsqlProcFormatter.RuntimeArg> list2 = new List<TsqlProcFormatter.RuntimeArg>();
		TsqlProcFormatter.RuntimeArg item2 = new TsqlProcFormatter.RuntimeArg(typeof(int), (int)CollectionFrequency.TotalSeconds);
		obj.Arguments.Add(new TsqlProcFormatter.SprocArg("frequency", required: true));
		list2.Add(item2);
		TsqlProcFormatter.RuntimeArg item3 = ((setParamName == null) ? new TsqlProcFormatter.RuntimeArg(typeof(int), Parent.Id) : new TsqlProcFormatter.RuntimeArg(typeof(object), "@" + setParamName));
		obj.Arguments.Add(new TsqlProcFormatter.SprocArg("collection_set_id", required: true));
		list2.Add(item3);
		return obj.GenerateScript(this, list2, list);
	}

	internal string GetAlterScript()
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("GetAlterScript");
		StringBuilder stringBuilder = new StringBuilder();
		List<TsqlProcFormatter.RuntimeArg> list = new List<TsqlProcFormatter.RuntimeArg>();
		TsqlProcFormatter tsqlProcFormatter = new TsqlProcFormatter();
		tsqlProcFormatter.Procedure = "[msdb].[dbo].[sp_syscollector_update_collection_item]";
		tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("collection_item_id", "Id", required: true, output: false));
		if (base.Properties["Parameters"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("parameters", "Parameters", required: false, output: false));
		}
		if (base.Properties["CollectionFrequency"].Dirty)
		{
			tsqlProcFormatter.Arguments.Add(new TsqlProcFormatter.SprocArg("frequency", required: false));
			list.Add(new TsqlProcFormatter.RuntimeArg(typeof(int), (int)CollectionFrequency.TotalSeconds));
		}
		stringBuilder.AppendLine(tsqlProcFormatter.GenerateScript(this, list));
		if (tsqlProcFormatter.Arguments.Count > 1)
		{
			return stringBuilder.ToString();
		}
		methodTraceContext.TraceVerbose("No dirty items -- no script is needed");
		return string.Empty;
	}

	internal string GetDropScript()
	{
		using (tc.GetMethodContext("GetDropScript"))
		{
			return new TsqlProcFormatter
			{
				Procedure = "[msdb].[dbo].[sp_syscollector_delete_collection_item]",
				Arguments = 
				{
					new TsqlProcFormatter.SprocArg("collection_item_id", "Id", required: true, output: false)
				}
			}.GenerateScript(this);
		}
	}

	private string GetRenameScript(Key newKey)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("GetRenameScript");
		if (null == newKey)
		{
			throw new CollectorException(ExceptionTemplatesSR.NameNotNull);
		}
		methodTraceContext.TraceParameterIn("newKey", newKey);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(new TsqlProcFormatter
		{
			Procedure = "[msdb].[dbo].[sp_syscollector_update_collection_item]",
			Arguments = 
			{
				new TsqlProcFormatter.SprocArg("name", "Name", required: true, output: false),
				new TsqlProcFormatter.SprocArg("new_name", required: true)
			}
		}.GenerateScript(this, new List<TsqlProcFormatter.RuntimeArg>
		{
			new TsqlProcFormatter.RuntimeArg(typeof(string), newKey.Name)
		}));
		return stringBuilder.ToString();
	}

	internal void SetId(int id)
	{
		tc.TraceVerbose("Setting collection item Id to: {0}", id);
		Id = id;
	}

	private void CheckNewName(string newName)
	{
		if (newName == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.NameNotNull);
		}
		Helpers.CheckPropertyValueLength(newName, 128, "Name");
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

	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	private CollectionItem()
	{
		Id = 0;
		CollectionFrequency = new TimeSpan(-1L);
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		throw new CollectorException(ExceptionTemplatesSR.NoSuchCollection(elementType));
	}
}
