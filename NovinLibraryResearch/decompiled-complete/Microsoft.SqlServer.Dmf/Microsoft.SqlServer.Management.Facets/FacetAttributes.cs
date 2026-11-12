using System;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Dmf;

namespace Microsoft.SqlServer.Management.Facets;

internal sealed class FacetAttributes
{
	private static TraceContext traceContext = TraceContext.GetTraceContext("DMF", "FacetAttributes");

	private AutomatedPolicyEvaluationMode evaluationMode;

	private FacetType facetType;

	private bool isReadOnly;

	private Type rootType;

	internal AutomatedPolicyEvaluationMode EvaluationMode
	{
		get
		{
			return evaluationMode;
		}
		set
		{
			traceContext.TraceVerbose("Setting EvaluationMode to: {0}", value);
			evaluationMode = value;
		}
	}

	internal FacetType FacetType
	{
		get
		{
			return facetType;
		}
		set
		{
			traceContext.TraceVerbose("Setting FacetType to: {0}", value);
			facetType = value;
		}
	}

	internal bool IsReadOnly
	{
		get
		{
			return isReadOnly;
		}
		set
		{
			traceContext.TraceVerbose("Setting IsReadOnly to: {0}", value);
			isReadOnly = value;
		}
	}

	internal Type RootType
	{
		get
		{
			return rootType;
		}
		set
		{
			traceContext.TraceVerbose("Setting RootType to: {0}", value);
			rootType = value;
		}
	}

	internal FacetAttributes()
	{
		evaluationMode = AutomatedPolicyEvaluationMode.None;
		facetType = FacetType.Logical;
		isReadOnly = false;
		rootType = null;
	}
}
