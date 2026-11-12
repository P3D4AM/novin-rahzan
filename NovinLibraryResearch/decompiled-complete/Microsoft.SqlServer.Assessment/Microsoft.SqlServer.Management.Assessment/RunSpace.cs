using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Configuration;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment;

public class RunSpace
{
	public ISqlObjectLocator Target { get; }

	internal DbConnection Connection { get; }

	internal IInventory Inventory { get; }

	internal CharacteristicCollection KnownCharacteristics { get; } = new CharacteristicCollection();


	internal List<CharacteristicFailure> FailedCharacteristics { get; } = new List<CharacteristicFailure>();


	internal EngineConfig Configuration { get; }

	internal EvaluationContext<object> EvaluationContext { get; }

	internal RunSpace(ISqlObjectLocator target, DbConnection connection, IInventory inventory, EngineConfig configuration, EvaluationContext<object> evaluationContext)
	{
		Target = target;
		Connection = connection;
		Inventory = inventory;
		Configuration = configuration;
		EvaluationContext = evaluationContext;
	}

	internal T GetTool<T>() where T : class, ITool
	{
		return Inventory.GetTool<T>(this);
	}

	internal ITool GetTool(Type channelType)
	{
		return Inventory.GetTool(this, channelType);
	}

	internal bool Meets(ICharacteristicCollection requirements)
	{
		return KnownCharacteristics.IsSupersetOf(requirements);
	}

	internal bool ProvidesAll(IEnumerable<Type> toolTypes)
	{
		string[] specification = toolTypes.Select(ToolKey.From).ToArray();
		CharacteristicCollection requirements = new CharacteristicCollection("channel", specification);
		return Meets(requirements);
	}

	internal CharacteristicCollection GetMissingDependencies(ICharacteristicCollection requirements, IEnumerable<Type> toolTypes)
	{
		string[] specification = toolTypes.Select(ToolKey.From).ToArray();
		CharacteristicCollection characteristicCollection = new CharacteristicCollection("channel", specification);
		characteristicCollection.UnionWith(requirements);
		characteristicCollection.ExceptWith(KnownCharacteristics);
		return characteristicCollection;
	}

	internal IProbe GetProbeImplementation(string name)
	{
		return Configuration.GetProbeImplementation(name, Target);
	}
}
