using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Facets;

public sealed class FacetEvaluationContext
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "FacetEvaluationContext");

	private Type iface;

	private object target;

	private FacetType facetType;

	private object physicalTarget;

	public Type Interface => iface;

	public object Target => target;

	internal FacetType FacetType => facetType;

	internal object PhysicalTarget => physicalTarget;

	internal FacetEvaluationContext(Type iface, object target, FacetType facetType)
	{
		traceContext.TraceMethodEnter("FacetEvaluationContext");
		traceContext.TraceParameters(iface, target, facetType);
		this.iface = iface;
		this.target = target;
		this.facetType = facetType;
		physicalTarget = target;
		traceContext.TraceMethodExit("FacetEvaluationContext");
	}

	internal FacetEvaluationContext(Type iface, object target, FacetType facetType, object physicalTarget)
	{
		traceContext.TraceMethodEnter("FacetEvaluationContext");
		traceContext.TraceParameters(iface, target, facetType, physicalTarget);
		this.iface = iface;
		this.target = target;
		this.facetType = facetType;
		this.physicalTarget = physicalTarget;
		traceContext.TraceMethodExit("FacetEvaluationContext");
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static FacetEvaluationContext GetFacetEvaluationContext(string facetName, object target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		Type type = null;
		FacetEvaluationContext facetEvaluationContext = null;
		if (facetName != null)
		{
			type = FacetRepository.GetFacetType(facetName);
		}
		if (type != null)
		{
			return FacetRepository.GetAdapterObject(target, type);
		}
		if ((facetName == null || target.GetType().Name == facetName) && target is ISfcPropertyProvider)
		{
			return new FacetEvaluationContext(target.GetType(), target, FacetType.Physical);
		}
		throw traceContext.TraceThrow(new MissingObjectException(ExceptionTemplatesSR.ManagementFacet, facetName));
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public static FacetEvaluationContext GetFacetEvaluationContext(Type facetType, object target)
	{
		FacetEvaluationContext facetEvaluationContext = null;
		if (FacetRepository.IsRegisteredFacet(facetType))
		{
			return FacetRepository.GetAdapterObject(target, facetType);
		}
		if (target.GetType() == facetType && target is ISfcPropertyProvider)
		{
			return new FacetEvaluationContext(target.GetType(), target, FacetType.Physical);
		}
		throw traceContext.TraceThrow(new MissingObjectException(ExceptionTemplatesSR.ManagementFacet, facetType.Name));
	}

	public void Alter()
	{
		if (Target is IAlterable)
		{
			((IAlterable)Target).Alter();
		}
	}

	public void Refresh()
	{
		if (Target is IRefreshable)
		{
			((IRefreshable)Target).Refresh();
		}
	}

	public object GetPropertyValue(string name)
	{
		traceContext.TraceMethodEnter("GetPropertyValue");
		traceContext.TraceParameters(name);
		object value = null;
		if (FacetType == FacetType.Physical)
		{
			if (Target is ISfcPropertyProvider && !((ISfcPropertyProvider)Target).GetPropertySet().TryGetPropertyValue(name, out value))
			{
				if (name == "Name" || name == "Schema")
				{
					string name2 = "get_" + name;
					MethodInfo method = Target.GetType().GetMethod(name2);
					if (null != method)
					{
						return method.Invoke(Target, null);
					}
				}
				throw traceContext.TraceThrow(new MissingPropertyException(name));
			}
		}
		else if (null != Interface)
		{
			string text = "get_" + name;
			InterfaceMapping interfaceMap = Target.GetType().GetInterfaceMap(Interface);
			bool flag = true;
			int num = interfaceMap.InterfaceMethods.Length;
			for (int i = 0; i < num; i++)
			{
				if (!(interfaceMap.InterfaceMethods[i].Name == text))
				{
					continue;
				}
				MethodInfo methodInfo = interfaceMap.TargetMethods[i];
				try
				{
					value = methodInfo.Invoke(Target, null);
				}
				catch (TargetInvocationException ex)
				{
					traceContext.TraceCatch(ex);
					if (ex.InnerException is RestartPendingException)
					{
						throw traceContext.TraceThrow(ex.InnerException);
					}
					throw traceContext.TraceThrow(new NonRetrievablePropertyException(name, ex.InnerException));
				}
				flag = false;
				break;
			}
			if (flag)
			{
				throw traceContext.TraceThrow(new MissingPropertyException(name));
			}
		}
		traceContext.TraceParameterOut("return", value);
		traceContext.TraceMethodExit("GetPropertyValue");
		return value;
	}

	public void SetPropertyValue(string name, object value)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("SetPropertyValue", TraceEventType.Information);
		methodTraceContext.TraceParameters(name, value);
		if (!FacetRepository.IsPropertyConfigurable(Interface, name))
		{
			throw methodTraceContext.TraceThrow(new NonConfigurableReadOnlyPropertyException(name));
		}
		if (FacetType == FacetType.Physical)
		{
			if (Target is ISfcPropertyProvider)
			{
				ISfcProperty property;
				bool test = ((ISfcPropertyProvider)Target).GetPropertySet().TryGetProperty(name, out property);
				traceContext.DebugAssert(test, "Cannot find verified property");
				traceContext.DebugAssert(property.Writable, "Property supposed to be writable");
				property.Value = value;
			}
		}
		else
		{
			if (!(null != Interface))
			{
				return;
			}
			string text = "set_" + name;
			InterfaceMapping interfaceMap = Target.GetType().GetInterfaceMap(Interface);
			bool flag = true;
			int num = interfaceMap.InterfaceMethods.Length;
			for (int i = 0; i < num; i++)
			{
				if (interfaceMap.InterfaceMethods[i].Name == text)
				{
					MethodInfo methodInfo = interfaceMap.TargetMethods[i];
					try
					{
						object[] parameters = new object[1] { value };
						methodInfo.Invoke(Target, parameters);
					}
					catch (ArgumentException)
					{
						methodTraceContext.TraceError("Caught a general Exception of type System.ArgumentException");
						object obj = Convert.ChangeType(value, methodInfo.GetParameters()[0].ParameterType, CultureInfo.InvariantCulture);
						object[] parameters2 = new object[1] { obj };
						methodInfo.Invoke(Target, parameters2);
					}
					flag = false;
					break;
				}
			}
			if (flag)
			{
				traceContext.DebugAssert(test: false, "Cannot find verified property");
			}
		}
	}
}
