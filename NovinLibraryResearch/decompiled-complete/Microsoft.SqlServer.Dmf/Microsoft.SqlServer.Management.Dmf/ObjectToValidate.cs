using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

internal sealed class ObjectToValidate
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "ObjectToValidate");

	private object obj;

	private Type type;

	private string validationMethod;

	public static readonly string Drop = "Drop";

	public object Object => obj;

	public Type Type => type;

	public string ValidationMethod => validationMethod;

	public ObjectToValidate(object obj, Type type, string validationMethod)
	{
		traceContext.TraceMethodEnter("ObjectToValidate");
		traceContext.TraceParameters(obj, type, validationMethod);
		this.obj = obj;
		this.type = type;
		this.validationMethod = validationMethod;
		traceContext.TraceMethodExit("ObjectToValidate");
	}

	internal static void ValidateObjects(List<ObjectToValidate> objectsToValidate)
	{
		traceContext.TraceMethodEnter("ValidateObjects");
		traceContext.TraceParameters(objectsToValidate);
		Policy policy = null;
		bool flag = false;
		foreach (ObjectToValidate item in objectsToValidate)
		{
			if (item.Type == typeof(Policy))
			{
				((Policy)item.Object).Validate(item.ValidationMethod);
				policy = (Policy)item.Object;
			}
			else if (item.Type == typeof(ObjectSet))
			{
				if (item.ValidationMethod != Drop)
				{
					((ObjectSet)item.Object).Validate(item.ValidationMethod);
				}
			}
			else if (item.Type == typeof(Condition))
			{
				if (item.ValidationMethod == Microsoft.SqlServer.Management.Sdk.Sfc.ValidationMethod.Alter)
				{
					flag = true;
				}
				else
				{
					((Condition)item.Object).Validate(item.ValidationMethod);
				}
			}
			else if (!(item.Type == typeof(PolicyCategory)))
			{
				traceContext.DebugAssert(test: true, "Policy deserialization - unknown object type");
			}
		}
		if (flag)
		{
			traceContext.DebugAssert(policy != null);
			foreach (ObjectToValidate item2 in objectsToValidate)
			{
				if (item2.Type == typeof(Condition) && item2.ValidationMethod == Microsoft.SqlServer.Management.Sdk.Sfc.ValidationMethod.Alter)
				{
					((Condition)item2.Object).ValidateDeserialized(policy);
				}
			}
		}
		traceContext.TraceMethodExit("ValidateObjects");
	}

	internal static void CommitObjects(PolicyStore ps, List<ObjectToValidate> objectsToValidate)
	{
		traceContext.TraceMethodEnter("CommitObjects");
		traceContext.TraceParameters(ps, objectsToValidate);
		ObjectToValidate objectToValidate = null;
		ObjectSet objectSet = null;
		ObjectSet objectSet2 = null;
		foreach (ObjectToValidate item in objectsToValidate)
		{
			if (item.Type == typeof(Policy))
			{
				objectToValidate = item;
			}
			else if (item.Type == typeof(ObjectSet))
			{
				if (item.ValidationMethod == Microsoft.SqlServer.Management.Sdk.Sfc.ValidationMethod.Create)
				{
					objectSet2 = (ObjectSet)item.Object;
				}
				else if (item.ValidationMethod == Drop)
				{
					objectSet = (ObjectSet)item.Object;
				}
			}
			else if (item.Type == typeof(Condition))
			{
				if (item.ValidationMethod == Microsoft.SqlServer.Management.Sdk.Sfc.ValidationMethod.Create)
				{
					((Condition)item.Object).Create();
				}
				else if (item.ValidationMethod == Microsoft.SqlServer.Management.Sdk.Sfc.ValidationMethod.Alter)
				{
					((Condition)item.Object).AlterNoValidation();
				}
			}
			else if (item.Type == typeof(PolicyCategory))
			{
				((PolicyCategory)item.Object).Create();
			}
		}
		if (objectToValidate.ValidationMethod == Microsoft.SqlServer.Management.Sdk.Sfc.ValidationMethod.Create)
		{
			traceContext.DebugAssert(objectSet == null, "only drop ObjectSet for policy overwrite");
			objectSet2?.Create();
			((Policy)objectToValidate.Object).Create();
		}
		else if (objectToValidate.ValidationMethod == Microsoft.SqlServer.Management.Sdk.Sfc.ValidationMethod.Alter)
		{
			if (objectSet != null)
			{
				bool enabled = ((Policy)objectToValidate.Object).Enabled;
				((Policy)objectToValidate.Object).Enabled = false;
				AutomatedPolicyEvaluationMode automatedPolicyEvaluationMode = ((Policy)objectToValidate.Object).AutomatedPolicyEvaluationMode;
				((Policy)objectToValidate.Object).AutomatedPolicyEvaluationMode = AutomatedPolicyEvaluationMode.None;
				((Policy)objectToValidate.Object).ObjectSet = string.Empty;
				((Policy)objectToValidate.Object).Alter();
				ps.ObjectSets.Refresh();
				traceContext.DebugAssert(!ps.ObjectSets.Contains(objectSet), "ObjectSet should have been dropped by a trigger");
				((Policy)objectToValidate.Object).ObjectSet = objectSet2.Name;
				((Policy)objectToValidate.Object).AutomatedPolicyEvaluationMode = automatedPolicyEvaluationMode;
				((Policy)objectToValidate.Object).Enabled = enabled;
			}
			objectSet2?.Create();
			((Policy)objectToValidate.Object).Alter();
		}
		traceContext.TraceMethodExit("CommitObjects");
	}

	internal static void RestoreCollections(PolicyStore ps, List<ObjectToValidate> objectsToValidate)
	{
		traceContext.TraceMethodEnter("RestoreCollections");
		traceContext.TraceParameters(ps, objectsToValidate);
		if (((ISfcHasConnection)ps).ConnectionContext.Mode == SfcConnectionContextMode.Offline)
		{
			foreach (ObjectToValidate item in objectsToValidate)
			{
				if (item.Type == typeof(Policy))
				{
					((Policy)item.Object).Drop();
					ps.Policies.Remove((Policy)item.Object);
				}
				else if (item.Type == typeof(Condition))
				{
					((Condition)item.Object).Drop();
					ps.Conditions.Remove((Condition)item.Object);
				}
				else if (item.Type == typeof(ObjectSet))
				{
					((ObjectSet)item.Object).Drop();
					ps.ObjectSets.Remove((ObjectSet)item.Object);
				}
				else if (item.Type == typeof(PolicyCategory))
				{
					((PolicyCategory)item.Object).Drop();
					ps.PolicyCategories.Remove((PolicyCategory)item.Object);
				}
			}
		}
		else
		{
			ps.Policies.Refresh();
			ps.ObjectSets.Refresh();
			ps.Conditions.Refresh();
			ps.PolicyCategories.Refresh();
			foreach (ObjectToValidate item2 in objectsToValidate)
			{
				if (item2.ValidationMethod == Microsoft.SqlServer.Management.Sdk.Sfc.ValidationMethod.Alter && (item2.Type == typeof(Policy) || item2.Type == typeof(Condition)))
				{
					((SfcInstance)item2.Object).Refresh();
				}
			}
		}
		traceContext.TraceMethodExit("RestoreCollections");
	}
}
