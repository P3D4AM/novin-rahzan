using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ProbeResult : IProbeResult
{
	private ProbeResultView data;

	private IDictionary<IEvaluationContext, ProbeCall> Calls { get; }

	private IDataMorph Morph { get; }

	private IEnumerable<IProbeResult> InnerResults { get; }

	public string Name { get; }

	public bool Failed
	{
		get
		{
			if (!Calls.Any() || !Calls.All((KeyValuePair<IEvaluationContext, ProbeCall> p) => p.Value.Failed))
			{
				return InnerResults.Any((IProbeResult r) => r.Failed);
			}
			return true;
		}
	}

	public IEnumerable<Exception> Exceptions => (from p in Calls
		where p.Value.Failed
		select p.Value.Exception).Concat(InnerResults.SelectMany((IProbeResult r) => r.Exceptions));

	public IDataView Data
	{
		get
		{
			if (data == null)
			{
				data = new ProbeResultView(Name, Calls.Where((KeyValuePair<IEvaluationContext, ProbeCall> p) => !p.Value.Failed).ToDictionary((KeyValuePair<IEvaluationContext, ProbeCall> p) => p.Key, (KeyValuePair<IEvaluationContext, ProbeCall> p) => ApplyMorph(p.Value.Result)));
			}
			return data;
		}
	}

	public ProbeResult(string name, IDictionary<IEvaluationContext, ProbeCall> calls, IDataMorph morph, IEnumerable<IProbeResult> innerResults)
	{
		Name = name;
		Calls = calls;
		Morph = morph;
		InnerResults = innerResults;
	}

	public bool Includes(IProbeResult result)
	{
		return InnerResults.Any((IProbeResult r) => r == result || r.Includes(result));
	}

	private IDataView ApplyMorph(IDataView source)
	{
		return Morph?.ApplyTo(source) ?? source;
	}
}
