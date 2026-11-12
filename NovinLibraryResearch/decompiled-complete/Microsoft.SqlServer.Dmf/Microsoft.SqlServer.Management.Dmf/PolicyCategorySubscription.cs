using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class PolicyCategorySubscription : SfcInstance, ISfcCreatable, ICreatable, ISfcDroppable, IDroppable, ISfcAlterable, IAlterable
{
	public sealed class Key : SfcKey
	{
		private int id;

		public int ID
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

		public Key()
		{
		}

		public Key(Key other)
		{
			ID = other.ID;
		}

		public Key(int id)
		{
			ID = id;
		}

		internal Key(Dictionary<string, object> filedDict)
		{
			ID = Convert.ToInt32(filedDict["ID"]);
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
			return ID.GetHashCode();
		}

		private bool IsEqual(Key key)
		{
			return ID == key.ID;
		}

		public override string GetUrnFragment()
		{
			return string.Format("{0}[@ID='{1}']", "PolicyCategorySubscription", ID);
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
			return new PolicyCategorySubscription();
		}
	}

	private static TraceContext traceContext;

	private static readonly SfcTsqlProcFormatter scriptCreateAction;

	private static readonly SfcTsqlProcFormatter scriptAlterAction;

	private static readonly SfcTsqlProcFormatter scriptDropAction;

	internal const string typeName = "PolicyCategorySubscription";

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

	[SfcProperty(Data = true, Required = false)]
	[SfcKey(0)]
	public int ID => (int)base.Properties["ID"].Value;

	[SfcProperty(SfcPropertyFlags.Required)]
	public string TargetType
	{
		get
		{
			return (string)base.Properties["TargetType"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting TargetType to: {0}", value);
			base.Properties["TargetType"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	public string Target
	{
		get
		{
			return (string)base.Properties["Target"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting Target to: {0}", value);
			base.Properties["Target"].Value = value;
		}
	}

	[SfcProperty(SfcPropertyFlags.Required)]
	[SfcReference(typeof(Condition), "PolicyStore/PolicyCategory[@Name='{0}']", new string[] { "PolicyCategory" })]
	public string PolicyCategory
	{
		get
		{
			return (string)base.Properties["PolicyCategory"].Value;
		}
		set
		{
			traceContext.TraceVerbose("Setting PolicyCategory to: {0}", value);
			base.Properties["PolicyCategory"].Value = value;
		}
	}

	static PolicyCategorySubscription()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "PolicyCategorySubscription");
		scriptCreateAction = new SfcTsqlProcFormatter();
		scriptAlterAction = new SfcTsqlProcFormatter();
		scriptDropAction = new SfcTsqlProcFormatter();
		scriptCreateAction.Procedure = "msdb.dbo.sp_syspolicy_add_policy_category_subscription";
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_type", "TargetType", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_object", "Target", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category", "PolicyCategory", required: true, output: false));
		scriptCreateAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category_subscription_id", "ID", required: false, output: true));
		scriptAlterAction.Procedure = "msdb.dbo.sp_syspolicy_update_policy_category_subscription";
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category_subscription_id", "ID", required: true, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_type", "TargetType", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("target_object", "Target", required: false, output: false));
		scriptAlterAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category", "PolicyCategory", required: false, output: false));
		scriptDropAction.Procedure = "msdb.dbo.sp_syspolicy_delete_policy_category_subscription";
		scriptDropAction.Arguments.Add(new SfcTsqlProcFormatter.SprocArg("policy_category_subscription_id", "ID", required: true, output: false));
	}

	public PolicyCategorySubscription()
	{
	}

	public PolicyCategorySubscription(PolicyStore parent)
	{
		traceContext.TraceMethodEnter("PolicyCategorySubscription");
		Parent = parent;
		traceContext.TraceMethodExit("PolicyCategorySubscription");
	}

	public PolicyCategorySubscription(PolicyStore parent, SqlSmoObject obj)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("PolicyCategorySubscription", TraceEventType.Information);
		if (obj == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("obj"));
		}
		if (obj is Database)
		{
			Parent = parent;
			Target = ((Database)obj).Name;
			TargetType = typeof(Database).Name.ToUpperInvariant();
			return;
		}
		throw methodTraceContext.TraceThrow(new UnsupportedObjectTypeException(obj.GetType().Name, GetType().Name));
	}

	internal PolicyCategorySubscription(PolicyStore parent, SfcQueryExpression target, string policyCategory)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("PolicyCategorySubscription");
		methodTraceContext.TraceParameters(parent, target, policyCategory);
		XPathExpression xPathExpression = new Urn(target.ToString()).XPathExpression;
		if (xPathExpression.Length <= 1 || xPathExpression[1].Name != "Database")
		{
			throw methodTraceContext.TraceThrow(new UnsupportedObjectTypeException(xPathExpression[1].Name, GetType().Name));
		}
		if (!(xPathExpression[1].Filter is FilterNodeOperator filterNodeOperator) || !(filterNodeOperator.Left is FilterNodeAttribute) || !(filterNodeOperator.Right is FilterNodeConstant) || !(((FilterNodeAttribute)filterNodeOperator.Left).Name == "Name") || string.IsNullOrEmpty(((FilterNodeConstant)filterNodeOperator.Right).ValueAsString))
		{
			throw new BadExpressionTreeException(ExceptionTemplatesSR.UnsupportedTargetFilter(target.ToString()));
		}
		Parent = parent;
		Target = ((FilterNodeConstant)filterNodeOperator.Right).ValueAsString;
		PolicyCategory = policyCategory;
		TargetType = typeof(Database).Name.ToUpperInvariant();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static SfcObjectFactory GetObjectFactory()
	{
		return ObjectFactory.Instance;
	}

	protected override SfcKey CreateIdentityKey()
	{
		try
		{
			return new Key(ID);
		}
		catch (NullReferenceException)
		{
			traceContext.TraceError("Caught a general Exception of type NullReferenceException");
			return null;
		}
	}

	protected override ISfcCollection GetChildCollection(string elementType)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetChildCollection");
		methodTraceContext.TraceParameters(elementType);
		throw methodTraceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.NoSuchCollection(elementType)));
	}

	private void SetID(int id)
	{
		traceContext.TraceMethodEnter("SetID");
		traceContext.TraceParameters(id);
		base.Properties["ID"].Value = id;
		traceContext.TraceMethodExit("SetID");
	}

	public void Create()
	{
		traceContext.TraceMethodEnter("Create");
		Validate(ValidationMethod.Create);
		base.Properties["ID"].Value = -1;
		CreateImpl();
		traceContext.TraceMethodExit("Create");
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
			ResetKey();
		}
	}

	public void Alter()
	{
		traceContext.TraceMethodEnter("Alter");
		Validate(ValidationMethod.Alter);
		AlterImpl();
		traceContext.TraceMethodExit("Alter");
	}

	ISfcScript ISfcAlterable.ScriptAlter()
	{
		return new SfcTSqlScript(scriptAlterAction.GenerateScript(this));
	}

	public void Drop()
	{
		DropImpl();
	}

	ISfcScript ISfcDroppable.ScriptDrop()
	{
		return new SfcTSqlScript(scriptDropAction.GenerateScript(this));
	}

	public void Validate(string mode)
	{
		ValidateProperties(mode);
	}

	public void ValidateProperties(string mode)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ValidateProperties", TraceEventType.Information);
		methodTraceContext.TraceParameters(mode);
		if (Parent == null)
		{
			throw methodTraceContext.TraceThrow(new SfcMissingParentException());
		}
		if (string.IsNullOrEmpty(TargetType))
		{
			throw methodTraceContext.TraceThrow(new SfcPropertyNotSetException("TargetType"));
		}
		if (TargetType != "DATABASE")
		{
			throw methodTraceContext.TraceThrow(new UnsupportedObjectTypeException(TargetType, ExceptionTemplatesSR.CategorySubscription));
		}
		if (string.IsNullOrEmpty(PolicyCategory))
		{
			throw methodTraceContext.TraceThrow(new SfcPropertyNotSetException("PolicyCategory"));
		}
		if (string.IsNullOrEmpty(Target))
		{
			throw methodTraceContext.TraceThrow(new SfcPropertyNotSetException("Target"));
		}
		if (!new Microsoft.SqlServer.Management.Smo.Server(Parent.SqlStoreConnection.ServerConnection).Databases.Contains(Target))
		{
			throw methodTraceContext.TraceThrow(new MissingObjectException(typeof(Database).Name, Target));
		}
	}
}
