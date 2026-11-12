using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Configuration;
using Microsoft.SqlServer.Management.Assessment.Expressions;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment.Logics;

internal class TwoPhaseLogics : ILogicsProvider2, ILogicsProvider
{
	internal Inventory Inventory { get; } = new Inventory();


	internal IResultGrouper ResultGrouper { get; set; } = new DefaultResultGrouper();


	internal static TwoPhaseLogics Instance { get; } = new TwoPhaseLogics();


	public TwoPhaseLogics()
	{
		InitializeInventory();
	}

	public async Task<List<IAssessmentResult>> GetAssessmentResults(IAssessmentRequest request, DbConnection connection, EngineConfig configuration)
	{
		RunSpace runSpace = new RunSpace(request.Target, connection, Inventory, configuration, request.EvaluationContext);
		return await GetAssessmentResults(request, runSpace).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<List<IAssessmentResult>> GetAssessmentResults(IAssessmentRequest request, RunSpace runSpace)
	{
		List<IAssessmentResult> results = new List<IAssessmentResult>();
		if (!(await Touch(runSpace).ConfigureAwait(continueOnCapturedContext: false)))
		{
			FatalError item = new FatalError(runSpace.Target, Resources.NotEnoughPermissionsToRunAssessmentMessage);
			results.Add(item);
			return results;
		}
		List<CheckContext> contexts = GetContexts(request);
		CharacteristicCollection checkRequirements = new CharacteristicCollection();
		foreach (CheckContext item2 in contexts)
		{
			checkRequirements.UnionWith(item2.Requirements);
		}
		await runSpace.Configuration.CharacteristicProviderSource.Update(runSpace, checkRequirements).ConfigureAwait(continueOnCapturedContext: false);
		results.AddRange(FilterContexts(runSpace, contexts));
		await CollectCharacteristics(runSpace, contexts.SelectMany((CheckContext c) => c.Check.Probes), checkRequirements).ConfigureAwait(continueOnCapturedContext: false);
		AtomicSet<ProbeCall> calls = new AtomicSet<ProbeCall>(ProbeCallComparer.Instance);
		foreach (CheckContext item3 in contexts)
		{
			await GetData(runSpace, item3, item3.Check.Probes, calls).ConfigureAwait(continueOnCapturedContext: false);
		}
		results.AddRange(GetAssessmentResults(request.Target, contexts));
		ResultGrouper.Consolidate(runSpace.Configuration.CharacteristicProviderSource, results);
		return results;
	}

	private static List<IAssessmentResult> GetAssessmentResults(ISqlObjectLocator target, IEnumerable<CheckContext> contexts)
	{
		List<IAssessmentResult> list = new List<IAssessmentResult>();
		foreach (CheckContext context in contexts)
		{
			if (GetProbeErrors(target, context, list))
			{
				continue;
			}
			foreach (PermutationContext permutation in GetPermutations(context))
			{
				IAssessmentResult recommendations = context.Check.GetRecommendations(target, permutation);
				if (recommendations != null)
				{
					list.Add(recommendations);
				}
			}
		}
		return list;
	}

	private static bool GetProbeErrors(ISqlObjectLocator target, CheckContext checkContext, List<IAssessmentResult> result)
	{
		bool result2 = false;
		foreach (IProbeResult probeResult in checkContext.ProbeResults)
		{
			if (probeResult.Failed)
			{
				result2 = true;
			}
			foreach (Exception exception in probeResult.Exceptions)
			{
				result.Add(ProbeError.Create(exception, checkContext.Check, probeResult.Name, target.Type, target.Urn));
			}
		}
		return result2;
	}

	private static IEnumerable<PermutationContext> GetPermutations(ProbeContext context)
	{
		IEnumerable<int> source = context.ProbeResults.Select((IProbeResult c) => c.Data.Count);
		IEnumerable<int[]> permutations = AlgebraicUtil.GetPermutations(source.ToArray());
		return permutations.Select((int[] i) => new PermutationContext(context, i));
	}

	private static async Task<bool> Touch(RunSpace runSpace)
	{
		ISqlTool tool = runSpace.GetTool<ISqlTool>();
		if (tool?.Connection != null)
		{
			using (DbCommand command = tool.CreateCommand())
			{
				switch (runSpace.Target.Type)
				{
				case SqlObjectType.Server:
					command.CommandText = "SELECT count(*) FROM fn_my_permissions(NULL, 'SERVER') WHERE permission_name = 'VIEW SERVER STATE'";
					break;
				case SqlObjectType.Database:
					command.Connection.ChangeDatabase(runSpace.Target.Name);
					command.CommandText = "SELECT count(*) FROM fn_my_permissions(NULL, 'DATABASE') WHERE permission_name = 'VIEW DATABASE STATE'";
					break;
				default:
					return true;
				}
				return (int)(await command.ExecuteScalarAsync().ConfigureAwait(continueOnCapturedContext: false)) != 0;
			}
		}
		return true;
	}

	private static List<CheckContext> GetContexts(IAssessmentRequest request)
	{
		return (from check in request.Checks
			where check.Enabled
			select BuildCheckContext(request.EvaluationContext, check)).ToList();
	}

	private static CheckContext BuildCheckContext(EvaluationContext<object> context, Check check)
	{
		CheckContext checkContext = new CheckContext(context, check);
		CollectRequirements(checkContext);
		return checkContext;
	}

	private static void CollectRequirements(CheckContext context)
	{
		context.Requirements = new CharacteristicCollection();
		CollectRequirements(context.Check, context.Requirements);
	}

	private static void CollectRequirements(Check check, CharacteristicCollection query)
	{
		CollectRequirements(check.Condition, query);
		foreach (Expression value in check.Parameters.Values)
		{
			CollectRequirements(value, query);
		}
		foreach (ProbeReference probe in check.Probes)
		{
			if (probe.Parameters == null)
			{
				continue;
			}
			foreach (Expression value2 in probe.Parameters.Values)
			{
				CollectRequirements(value2, query);
			}
		}
	}

	private static void CollectRequirements(Expression expression, CharacteristicCollection query)
	{
		if (expression == null)
		{
			return;
		}
		foreach (string name in expression.GetNames())
		{
			if (Names.SplitIf("attr", name, out var section, out var value, out var _))
			{
				query.Add(section, value);
			}
		}
	}

	private static IEnumerable<IAssessmentResult> FilterContexts(RunSpace runSpace, List<CheckContext> contexts)
	{
		List<int> skippedContexts = new List<int>();
		for (int i = 0; i < contexts.Count; i++)
		{
			if (runSpace.Meets(contexts[i].Requirements))
			{
				continue;
			}
			skippedContexts.Add(i);
			foreach (CharacteristicFailure failedCharacteristic in runSpace.FailedCharacteristics)
			{
				CharacteristicCollection characteristicCollection = new CharacteristicCollection(failedCharacteristic.Subject);
				characteristicCollection.IntersectWith(contexts[i].Requirements);
				if (characteristicCollection.Count > 0)
				{
					ProbeRequirementsException exception = new ProbeRequirementsException(Resources.ErrorProbeRequirements)
					{
						MissingDependencies = failedCharacteristic.Reason
					};
					yield return ProbeError.Create(exception, contexts[i].Check, string.Empty, runSpace.Target.Type, runSpace.Target.Urn);
				}
			}
		}
		skippedContexts.Reverse();
		foreach (int item in skippedContexts)
		{
			contexts.RemoveAt(item);
		}
	}

	private static ProbeCall GetProbeCall(RunSpace runSpace, ProbeReference probeRef, IEvaluationContext context)
	{
		IProbe probeImplementation = runSpace.GetProbeImplementation(probeRef.ProbeName);
		ProbeCall probeCall = new ProbeCall(probeRef.ProbeName, probeImplementation);
		if (probeRef.Parameters != null)
		{
			foreach (KeyValuePair<string, Expression> parameter in probeRef.Parameters)
			{
				probeCall.Parameters.Add(parameter.Key, parameter.Value.Evaluate(context));
			}
		}
		return probeCall;
	}

	private static ProbeContext GetProbeContext(CheckContext checkContext, ProbeReference probeRef, Dictionary<string, IProbeResult> results, HashSet<string> refs)
	{
		string text = probeRef.ProbeNameAlias;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = probeRef.ProbeName;
		}
		ProbeContext probeContext = new ProbeContext(text, checkContext);
		if (probeRef.Parameters == null)
		{
			return probeContext;
		}
		List<string> list = new List<string>();
		List<IProbeResult> list2 = new List<IProbeResult>();
		foreach (KeyValuePair<string, Expression> parameter in probeRef.Parameters)
		{
			foreach (string name in parameter.Value.GetNames())
			{
				if (name.Split(out var ns, out var _) && !list.Contains(ns))
				{
					list.Add(ns);
					if (results.TryGetValue(ns, out var value))
					{
						list2.Add(value);
					}
				}
			}
		}
		foreach (IProbeResult res in list2)
		{
			if (!list2.Any((IProbeResult r) => r != res && r.Includes(res)))
			{
				probeContext.Add(res);
			}
		}
		refs.UnionWith(list);
		return probeContext;
	}

	private static async Task GetData(RunSpace runSpace, CheckContext checkContext, IEnumerable<ProbeReference> references, AtomicSet<ProbeCall> callsCache)
	{
		Dictionary<string, IProbeResult> results = new Dictionary<string, IProbeResult>();
		HashSet<string> refs = new HashSet<string>();
		foreach (ProbeReference probeRef in references)
		{
			ProbeContext probeContext = GetProbeContext(checkContext, probeRef, results, refs);
			IProbeResult probeResult;
			try
			{
				Dictionary<IEvaluationContext, ProbeCall> probeCalls = new Dictionary<IEvaluationContext, ProbeCall>();
				foreach (PermutationContext c in GetPermutations(probeContext))
				{
					ProbeCall probeCall = GetProbeCall(runSpace, probeRef, c);
					probeCalls.Add(c, await InvokeProbeCall(runSpace, probeCall, callsCache).ConfigureAwait(continueOnCapturedContext: false));
				}
				probeResult = new ProbeResult(probeContext.Name, probeCalls, probeRef.Morph, probeContext.ProbeResults);
			}
			catch (Exception ex) when (ex is ProbeNotFoundException || ex is SqlAssessmentEvaluationException)
			{
				probeResult = new FailedProbeResult(probeContext.Name, ex);
			}
			results.Add(probeResult.Name, probeResult);
		}
		checkContext.Add(from p in results
			where !refs.Contains(p.Key)
			select p.Value);
	}

	private static async Task<ProbeCall> InvokeProbeCall(RunSpace runSpace, ProbeCall call, AtomicSet<ProbeCall> callsCache)
	{
		ProbeCall probeCall = callsCache.Find(call);
		if (probeCall == null)
		{
			await call.Invoke(runSpace).ConfigureAwait(continueOnCapturedContext: false);
			callsCache.Add(call);
			probeCall = call;
		}
		return probeCall;
	}

	private static async Task CollectCharacteristics(RunSpace runSpace, IEnumerable<ProbeReference> references, CharacteristicCollection advanceRequirements)
	{
		HashSet<IProbe> hashSet = new HashSet<IProbe>();
		foreach (ProbeReference reference in references)
		{
			try
			{
				IProbe probeImplementation = runSpace.GetProbeImplementation(reference.ProbeName);
				hashSet.Add(probeImplementation);
			}
			catch (Exception ex)
			{
				if (ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
			}
		}
		await CollectCharacteristics(runSpace, hashSet, advanceRequirements).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal static Task CollectCharacteristics(RunSpace runSpace, IEnumerable<IProbe> probes, CharacteristicCollection advanceRequirements)
	{
		CharacteristicCollection characteristicCollection = new CharacteristicCollection();
		HashSet<Type> hashSet = new HashSet<Type>();
		foreach (IProbe probe in probes)
		{
			characteristicCollection.UnionWith(probe.Subject);
			characteristicCollection.UnionWith(probe.Requirements);
			hashSet.UnionWith(probe.ToolTypes);
		}
		foreach (Type item in hashSet)
		{
			characteristicCollection.UnionWith(new CharacteristicCollection("channel", ToolKey.From(item)));
		}
		characteristicCollection.ExceptWith(advanceRequirements);
		return runSpace.Configuration.CharacteristicProviderSource.Update(runSpace, characteristicCollection);
	}

	private void InitializeInventory()
	{
		Inventory.Add(typeof(ISqlTool), SqlTool.Create);
		Inventory.Add(typeof(ICmdShellTool), XpCmdShellTool.Create);
		Inventory.Add(typeof(IPowerShellTool), CmdPowerShellTool.Create);
		Inventory.Add(typeof(IWmiTool), PsWmiTool.Create);
		Inventory.Add(typeof(IRegistryTool), XpRegXTool.Create);
	}
}
