using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public class CommandBatchPreparer : ICommandBatchPreparer
{
	private readonly IModificationCommandBatchFactory _modificationCommandBatchFactory;

	private readonly IParameterNameGeneratorFactory _parameterNameGeneratorFactory;

	private readonly IComparer<ModificationCommand> _modificationCommandComparer;

	private readonly IKeyValueIndexFactorySource _keyValueIndexFactorySource;

	private readonly int _minBatchSize;

	private readonly bool _sensitiveLoggingEnabled;

	private IReadOnlyDictionary<(string Schema, string Name), SharedTableEntryMapFactory<ModificationCommand>> _sharedTableEntryMapFactories;

	private CommandBatchPreparerDependencies Dependencies { get; }

	public CommandBatchPreparer([JetBrains.Annotations.NotNull] CommandBatchPreparerDependencies dependencies)
	{
		_modificationCommandBatchFactory = dependencies.ModificationCommandBatchFactory;
		_parameterNameGeneratorFactory = dependencies.ParameterNameGeneratorFactory;
		_modificationCommandComparer = dependencies.ModificationCommandComparer;
		_keyValueIndexFactorySource = dependencies.KeyValueIndexFactorySource;
		_minBatchSize = dependencies.Options.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault()?.MinBatchSize ?? 4;
		Dependencies = dependencies;
		if (dependencies.LoggingOptions.IsSensitiveDataLoggingEnabled)
		{
			_sensitiveLoggingEnabled = true;
		}
	}

	public virtual IEnumerable<ModificationCommandBatch> BatchCommands(IList<IUpdateEntry> entries, IUpdateAdapter updateAdapter)
	{
		ParameterNameGenerator parameterNameGenerator = _parameterNameGeneratorFactory.Create();
		IEnumerable<ModificationCommand> commands = CreateModificationCommands(entries, updateAdapter, parameterNameGenerator.GenerateNext);
		IReadOnlyList<List<ModificationCommand>> readOnlyList = TopologicalSort(commands);
		foreach (List<ModificationCommand> item in readOnlyList)
		{
			item.Sort(_modificationCommandComparer);
			ModificationCommandBatch modificationCommandBatch = _modificationCommandBatchFactory.Create();
			foreach (ModificationCommand modificationCommand in item)
			{
				if (modificationCommandBatch.AddCommand(modificationCommand))
				{
					continue;
				}
				if (modificationCommandBatch.ModificationCommands.Count == 1 || modificationCommandBatch.ModificationCommands.Count >= _minBatchSize)
				{
					if (modificationCommandBatch.ModificationCommands.Count > 1)
					{
						Dependencies.UpdateLogger.BatchReadyForExecution(modificationCommandBatch.ModificationCommands.SelectMany((ModificationCommand c) => c.Entries), modificationCommandBatch.ModificationCommands.Count);
					}
					yield return modificationCommandBatch;
				}
				else
				{
					Dependencies.UpdateLogger.BatchSmallerThanMinBatchSize(modificationCommandBatch.ModificationCommands.SelectMany((ModificationCommand c) => c.Entries), modificationCommandBatch.ModificationCommands.Count, _minBatchSize);
					foreach (ModificationCommand modificationCommand2 in modificationCommandBatch.ModificationCommands)
					{
						yield return StartNewBatch(parameterNameGenerator, modificationCommand2);
					}
				}
				modificationCommandBatch = StartNewBatch(parameterNameGenerator, modificationCommand);
			}
			if (modificationCommandBatch.ModificationCommands.Count == 1 || modificationCommandBatch.ModificationCommands.Count >= _minBatchSize)
			{
				if (modificationCommandBatch.ModificationCommands.Count > 1)
				{
					Dependencies.UpdateLogger.BatchReadyForExecution(modificationCommandBatch.ModificationCommands.SelectMany((ModificationCommand c) => c.Entries), modificationCommandBatch.ModificationCommands.Count);
				}
				yield return modificationCommandBatch;
				continue;
			}
			Dependencies.UpdateLogger.BatchSmallerThanMinBatchSize(modificationCommandBatch.ModificationCommands.SelectMany((ModificationCommand c) => c.Entries), modificationCommandBatch.ModificationCommands.Count, _minBatchSize);
			foreach (ModificationCommand modificationCommand3 in modificationCommandBatch.ModificationCommands)
			{
				yield return StartNewBatch(parameterNameGenerator, modificationCommand3);
			}
		}
	}

	private ModificationCommandBatch StartNewBatch(ParameterNameGenerator parameterNameGenerator, ModificationCommand modificationCommand)
	{
		parameterNameGenerator.Reset();
		ModificationCommandBatch modificationCommandBatch = _modificationCommandBatchFactory.Create();
		modificationCommandBatch.AddCommand(modificationCommand);
		return modificationCommandBatch;
	}

	protected virtual IEnumerable<ModificationCommand> CreateModificationCommands([JetBrains.Annotations.NotNull] IList<IUpdateEntry> entries, [JetBrains.Annotations.NotNull] IUpdateAdapter updateAdapter, [JetBrains.Annotations.NotNull] Func<string> generateParameterName)
	{
		List<ModificationCommand> list = new List<ModificationCommand>();
		if (_sharedTableEntryMapFactories == null)
		{
			_sharedTableEntryMapFactories = SharedTableEntryMap<ModificationCommand>.CreateSharedTableEntryMapFactories(updateAdapter.Model, updateAdapter);
		}
		Dictionary<(string, string), SharedTableEntryMap<ModificationCommand>> dictionary = null;
		foreach (IUpdateEntry entry in entries)
		{
			if (entry.SharedIdentityEntry != null && entry.EntityState == EntityState.Deleted)
			{
				continue;
			}
			IEntityType entityType = entry.EntityType;
			string tableName = entityType.GetTableName();
			string schema = entityType.GetSchema();
			(string, string) key = (schema, tableName);
			bool mainEntry = true;
			ModificationCommand modificationCommand;
			if (_sharedTableEntryMapFactories.TryGetValue(key, out var value))
			{
				if (dictionary == null)
				{
					dictionary = new Dictionary<(string, string), SharedTableEntryMap<ModificationCommand>>();
				}
				if (!dictionary.TryGetValue(key, out var value2))
				{
					value2 = value((string t, string s, IComparer<IUpdateEntry> c) => new ModificationCommand(t, s, generateParameterName, _sensitiveLoggingEnabled, c));
					dictionary.Add((schema, tableName), value2);
				}
				modificationCommand = value2.GetOrAddValue(entry);
				mainEntry = value2.GetPrincipals(entry.EntityType.GetRootType()).Count == 0;
			}
			else
			{
				modificationCommand = new ModificationCommand(tableName, schema, generateParameterName, _sensitiveLoggingEnabled, null);
			}
			modificationCommand.AddEntry(entry, mainEntry);
			list.Add(modificationCommand);
		}
		if (dictionary != null)
		{
			AddUnchangedSharingEntries(dictionary, entries);
		}
		return list.Where((ModificationCommand c) => c.EntityState != EntityState.Modified || c.ColumnModifications.Any((ColumnModification m) => m.IsWrite));
	}

	private void AddUnchangedSharingEntries(Dictionary<(string Schema, string Name), SharedTableEntryMap<ModificationCommand>> sharedTablesCommandsMap, IList<IUpdateEntry> entries)
	{
		foreach (SharedTableEntryMap<ModificationCommand> value in sharedTablesCommandsMap.Values)
		{
			foreach (ModificationCommand value2 in value.Values)
			{
				if (value2.EntityState != EntityState.Modified)
				{
					continue;
				}
				foreach (IUpdateEntry allEntry in value.GetAllEntries(value2.Entries[0]))
				{
					if (allEntry.EntityState == EntityState.Unchanged)
					{
						allEntry.EntityState = EntityState.Modified;
						bool mainEntry = value.GetPrincipals(allEntry.EntityType.GetRootType()).Count == 0;
						value2.AddEntry(allEntry, mainEntry);
						entries.Add(allEntry);
					}
				}
			}
		}
	}

	protected virtual IReadOnlyList<List<ModificationCommand>> TopologicalSort([JetBrains.Annotations.NotNull] IEnumerable<ModificationCommand> commands)
	{
		Multigraph<ModificationCommand, IAnnotatable> multigraph = new Multigraph<ModificationCommand, IAnnotatable>();
		multigraph.AddVertices(commands);
		Dictionary<IKeyValueIndex, List<ModificationCommand>> predecessorsMap = CreateKeyValuePredecessorMap(multigraph);
		AddForeignKeyEdges(multigraph, predecessorsMap);
		AddUniqueValueEdges(multigraph);
		return multigraph.BatchingTopologicalSort(FormatCycle);
	}

	private string FormatCycle(IReadOnlyList<Tuple<ModificationCommand, ModificationCommand, IEnumerable<IAnnotatable>>> data)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < data.Count; i++)
		{
			Tuple<ModificationCommand, ModificationCommand, IEnumerable<IAnnotatable>> tuple = data[i];
			Format(tuple.Item1, stringBuilder);
			IAnnotatable annotatable = tuple.Item3.First();
			if (!(annotatable is IForeignKey foreignKey))
			{
				if (annotatable is IIndex index)
				{
					Format(index, tuple.Item1, tuple.Item2, stringBuilder);
				}
			}
			else
			{
				Format(foreignKey, tuple.Item1, tuple.Item2, stringBuilder);
			}
			if (i == data.Count - 1)
			{
				Format(tuple.Item2, stringBuilder);
			}
		}
		return stringBuilder.ToString();
	}

	private void Format(ModificationCommand command, StringBuilder builder)
	{
		IUpdateEntry updateEntry = command.Entries.First();
		IEntityType entityType = updateEntry.EntityType;
		builder.Append(entityType.DisplayName());
		if (_sensitiveLoggingEnabled)
		{
			builder.Append(" { ");
			IReadOnlyList<IProperty> properties = entityType.FindPrimaryKey().Properties;
			for (int i = 0; i < properties.Count; i++)
			{
				IProperty property = properties[i];
				builder.Append("'");
				builder.Append(property.Name);
				builder.Append("': ");
				builder.Append(updateEntry.GetCurrentValue(property));
				if (i != properties.Count - 1)
				{
					builder.Append(", ");
				}
			}
			builder.Append(" } ");
		}
		else
		{
			builder.Append(" ");
		}
		builder.Append("[");
		builder.Append(updateEntry.EntityState);
		builder.Append("]");
	}

	private void Format(IForeignKey foreignKey, ModificationCommand source, ModificationCommand target, StringBuilder builder)
	{
		bool flag = !source.Entries.Any((IUpdateEntry e) => foreignKey.DeclaringEntityType.IsAssignableFrom(e.EntityType));
		if (flag)
		{
			builder.Append(" <-");
		}
		builder.Append(" ");
		if (foreignKey.DependentToPrincipal != null || foreignKey.PrincipalToDependent != null)
		{
			if (!flag && foreignKey.DependentToPrincipal != null)
			{
				builder.Append(foreignKey.DependentToPrincipal.Name);
				builder.Append(" ");
			}
			if (foreignKey.PrincipalToDependent != null)
			{
				builder.Append(foreignKey.PrincipalToDependent.Name);
				builder.Append(" ");
			}
			if (flag && foreignKey.DependentToPrincipal != null)
			{
				builder.Append(foreignKey.DependentToPrincipal.Name);
				builder.Append(" ");
			}
		}
		else
		{
			builder.Append("ForeignKey ");
		}
		IUpdateEntry updateEntry = (flag ? target : source).Entries.First((IUpdateEntry e) => foreignKey.DeclaringEntityType.IsAssignableFrom(e.EntityType));
		builder.Append("{ ");
		for (int i = 0; i < foreignKey.Properties.Count; i++)
		{
			IProperty property = foreignKey.Properties[i];
			builder.Append("'");
			builder.Append(property.Name);
			builder.Append("'");
			if (_sensitiveLoggingEnabled)
			{
				builder.Append(": ");
				builder.Append(updateEntry.GetCurrentValue(property));
			}
			if (i != foreignKey.Properties.Count - 1)
			{
				builder.Append(", ");
			}
		}
		builder.Append(" } ");
		if (!flag)
		{
			builder.Append("<- ");
		}
	}

	private void Format(IIndex index, ModificationCommand source, ModificationCommand target, StringBuilder builder)
	{
		bool flag = source.EntityState != EntityState.Deleted;
		if (flag)
		{
			builder.Append(" <-");
		}
		builder.Append(" Index ");
		IUpdateEntry updateEntry = (flag ? target : source).Entries.First((IUpdateEntry e) => index.DeclaringEntityType.IsAssignableFrom(e.EntityType));
		builder.Append("{ ");
		for (int i = 0; i < index.Properties.Count; i++)
		{
			IProperty property = index.Properties[i];
			builder.Append("'");
			builder.Append(property.Name);
			builder.Append("'");
			if (_sensitiveLoggingEnabled)
			{
				builder.Append(": ");
				builder.Append(updateEntry.GetCurrentValue(property));
			}
			if (i != index.Properties.Count - 1)
			{
				builder.Append(", ");
			}
		}
		builder.Append(" } ");
		if (!flag)
		{
			builder.Append("<- ");
		}
	}

	private Dictionary<IKeyValueIndex, List<ModificationCommand>> CreateKeyValuePredecessorMap(Graph<ModificationCommand> commandGraph)
	{
		Dictionary<IKeyValueIndex, List<ModificationCommand>> dictionary = new Dictionary<IKeyValueIndex, List<ModificationCommand>>();
		foreach (ModificationCommand vertex in commandGraph.Vertices)
		{
			IReadOnlyList<ColumnModification> columnModifications = vertex.ColumnModifications;
			if (vertex.EntityState == EntityState.Modified || vertex.EntityState == EntityState.Added)
			{
				for (int i = 0; i < vertex.Entries.Count; i++)
				{
					IUpdateEntry updateEntry = vertex.Entries[i];
					foreach (IForeignKey foreignKey in updateEntry.EntityType.GetReferencingForeignKeys())
					{
						IKeyValueIndexFactory keyValueIndexFactory = _keyValueIndexFactorySource.GetKeyValueIndexFactory(foreignKey.PrincipalKey);
						IEnumerable<ColumnModification> source = columnModifications.Where((ColumnModification cm) => foreignKey.PrincipalKey.Properties.Contains(cm.Property) && (cm.IsWrite || cm.IsRead));
						if (vertex.EntityState != EntityState.Added && !source.Any())
						{
							continue;
						}
						IKeyValueIndex keyValueIndex = keyValueIndexFactory.CreatePrincipalKeyValue((InternalEntityEntry)updateEntry, foreignKey);
						if (keyValueIndex != null)
						{
							if (!dictionary.TryGetValue(keyValueIndex, out var value))
							{
								value = new List<ModificationCommand>();
								dictionary.Add(keyValueIndex, value);
							}
							value.Add(vertex);
						}
					}
				}
			}
			if (vertex.EntityState != EntityState.Modified && vertex.EntityState != EntityState.Deleted)
			{
				continue;
			}
			foreach (IUpdateEntry entry in vertex.Entries)
			{
				foreach (IForeignKey foreignKey2 in entry.EntityType.GetForeignKeys())
				{
					IKeyValueIndexFactory keyValueIndexFactory2 = _keyValueIndexFactorySource.GetKeyValueIndexFactory(foreignKey2.PrincipalKey);
					IForeignKey currentForeignKey = foreignKey2;
					IEnumerable<ColumnModification> source2 = columnModifications.Where((ColumnModification cm) => currentForeignKey.Properties.Contains(cm.Property) && (cm.IsWrite || cm.IsRead));
					if (vertex.EntityState != EntityState.Deleted && !source2.Any())
					{
						continue;
					}
					IKeyValueIndex keyValueIndex2 = keyValueIndexFactory2.CreateDependentKeyValueFromOriginalValues((InternalEntityEntry)entry, foreignKey2);
					if (keyValueIndex2 != null)
					{
						if (!dictionary.TryGetValue(keyValueIndex2, out var value2))
						{
							value2 = new List<ModificationCommand>();
							dictionary.Add(keyValueIndex2, value2);
						}
						value2.Add(vertex);
					}
				}
			}
		}
		return dictionary;
	}

	private void AddForeignKeyEdges(Multigraph<ModificationCommand, IAnnotatable> commandGraph, Dictionary<IKeyValueIndex, List<ModificationCommand>> predecessorsMap)
	{
		foreach (ModificationCommand vertex in commandGraph.Vertices)
		{
			switch (vertex.EntityState)
			{
			case EntityState.Modified:
			case EntityState.Added:
			{
				for (int j = 0; j < vertex.Entries.Count; j++)
				{
					IUpdateEntry updateEntry2 = vertex.Entries[j];
					foreach (IForeignKey foreignKey in updateEntry2.EntityType.GetForeignKeys())
					{
						IKeyValueIndex keyValueIndex2 = _keyValueIndexFactorySource.GetKeyValueIndexFactory(foreignKey.PrincipalKey).CreateDependentKeyValue((InternalEntityEntry)updateEntry2, foreignKey);
						if (keyValueIndex2 != null)
						{
							AddMatchingPredecessorEdge(predecessorsMap, keyValueIndex2, commandGraph, vertex, foreignKey);
						}
					}
				}
				break;
			}
			case EntityState.Deleted:
			{
				for (int i = 0; i < vertex.Entries.Count; i++)
				{
					IUpdateEntry updateEntry = vertex.Entries[i];
					foreach (IForeignKey referencingForeignKey in updateEntry.EntityType.GetReferencingForeignKeys())
					{
						IKeyValueIndex keyValueIndex = _keyValueIndexFactorySource.GetKeyValueIndexFactory(referencingForeignKey.PrincipalKey).CreatePrincipalKeyValueFromOriginalValues((InternalEntityEntry)updateEntry, referencingForeignKey);
						if (keyValueIndex != null)
						{
							AddMatchingPredecessorEdge(predecessorsMap, keyValueIndex, commandGraph, vertex, referencingForeignKey);
						}
					}
				}
				break;
			}
			}
		}
	}

	private static void AddMatchingPredecessorEdge(Dictionary<IKeyValueIndex, List<ModificationCommand>> predecessorsMap, IKeyValueIndex dependentKeyValue, Multigraph<ModificationCommand, IAnnotatable> commandGraph, ModificationCommand command, IForeignKey foreignKey)
	{
		if (!predecessorsMap.TryGetValue(dependentKeyValue, out var value))
		{
			return;
		}
		foreach (ModificationCommand item in value)
		{
			if (item != command)
			{
				commandGraph.AddEdge(item, command, foreignKey);
			}
		}
	}

	private void AddUniqueValueEdges(Multigraph<ModificationCommand, IAnnotatable> commandGraph)
	{
		Dictionary<IIndex, Dictionary<object[], ModificationCommand>> dictionary = null;
		foreach (ModificationCommand vertex in commandGraph.Vertices)
		{
			if (vertex.EntityState != EntityState.Modified && vertex.EntityState != EntityState.Deleted)
			{
				continue;
			}
			for (int j = 0; j < vertex.Entries.Count; j++)
			{
				IUpdateEntry updateEntry = vertex.Entries[j];
				foreach (IIndex item in from i in updateEntry.EntityType.GetIndexes()
					where i.IsUnique
					select i)
				{
					if (vertex.EntityState != EntityState.Deleted)
					{
						bool flag = false;
						for (int k = 0; k < vertex.ColumnModifications.Count; k++)
						{
							ColumnModification columnModification = vertex.ColumnModifications[k];
							if (item.Properties.Contains(columnModification.Property) && (columnModification.IsWrite || columnModification.IsRead))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							continue;
						}
					}
					INullableValueFactory<object[]> nullableValueFactory = item.GetNullableValueFactory<object[]>();
					if (nullableValueFactory.TryCreateFromOriginalValues((InternalEntityEntry)updateEntry, out var key))
					{
						if (dictionary == null)
						{
							dictionary = new Dictionary<IIndex, Dictionary<object[], ModificationCommand>>();
						}
						if (!dictionary.TryGetValue(item, out var value))
						{
							value = new Dictionary<object[], ModificationCommand>(nullableValueFactory.EqualityComparer);
							dictionary.Add(item, value);
						}
						if (!value.ContainsKey(key))
						{
							value.Add(key, vertex);
						}
					}
				}
			}
		}
		if (dictionary == null)
		{
			return;
		}
		foreach (ModificationCommand vertex2 in commandGraph.Vertices)
		{
			if (vertex2.EntityState != EntityState.Modified && vertex2.EntityState != EntityState.Added)
			{
				continue;
			}
			foreach (IUpdateEntry entry in vertex2.Entries)
			{
				foreach (IIndex index in from i in entry.EntityType.GetIndexes()
					where i.IsUnique
					select i)
				{
					IEnumerable<ColumnModification> source = vertex2.ColumnModifications.Where((ColumnModification cm) => index.Properties.Contains(cm.Property) && cm.IsWrite);
					if ((vertex2.EntityState == EntityState.Added || source.Any()) && index.GetNullableValueFactory<object[]>().TryCreateFromCurrentValues((InternalEntityEntry)entry, out var key2) && dictionary.TryGetValue(index, out var value2) && value2.TryGetValue(key2, out var value3) && value3 != vertex2)
					{
						commandGraph.AddEdge(value3, vertex2, index);
					}
				}
			}
		}
	}
}
