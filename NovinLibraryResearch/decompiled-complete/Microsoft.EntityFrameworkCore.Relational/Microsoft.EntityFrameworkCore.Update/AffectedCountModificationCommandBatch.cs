using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Update;

public abstract class AffectedCountModificationCommandBatch : ReaderModificationCommandBatch
{
	protected AffectedCountModificationCommandBatch([JetBrains.Annotations.NotNull] ModificationCommandBatchFactoryDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void Consume(RelationalDataReader reader)
	{
		int num = 0;
		try
		{
			int num2 = 0;
			while (true)
			{
				if (num < CommandResultSet.Count && CommandResultSet[num] == ResultSetMapping.NoResultSet)
				{
					num++;
					continue;
				}
				if (num < CommandResultSet.Count)
				{
					num = (ModificationCommands[num].RequiresResultPropagation ? ConsumeResultSetWithPropagation(num, reader) : ConsumeResultSetWithoutPropagation(num, reader));
					num2++;
				}
				if (num >= CommandResultSet.Count || !reader.DbDataReader.NextResult())
				{
					break;
				}
			}
		}
		catch (Exception ex) when (!(ex is DbUpdateException))
		{
			throw new DbUpdateException(RelationalStrings.UpdateStoreException, ex, ModificationCommands[num].Entries);
		}
	}

	protected override async Task ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken = default(CancellationToken))
	{
		int commandIndex = 0;
		try
		{
			int actualResultSetCount = 0;
			while (true)
			{
				if (commandIndex < CommandResultSet.Count && CommandResultSet[commandIndex] == ResultSetMapping.NoResultSet)
				{
					commandIndex++;
					continue;
				}
				if (commandIndex < CommandResultSet.Count)
				{
					int num = ((!ModificationCommands[commandIndex].RequiresResultPropagation) ? (await ConsumeResultSetWithoutPropagationAsync(commandIndex, reader, cancellationToken)) : (await ConsumeResultSetWithPropagationAsync(commandIndex, reader, cancellationToken)));
					commandIndex = num;
					actualResultSetCount++;
				}
				bool flag = commandIndex < CommandResultSet.Count;
				if (flag)
				{
					flag = await reader.DbDataReader.NextResultAsync(cancellationToken);
				}
				if (!flag)
				{
					break;
				}
			}
		}
		catch (Exception ex) when (!(ex is DbUpdateException))
		{
			throw new DbUpdateException(RelationalStrings.UpdateStoreException, ex, ModificationCommands[commandIndex].Entries);
		}
	}

	protected virtual int ConsumeResultSetWithPropagation(int commandIndex, [JetBrains.Annotations.NotNull] RelationalDataReader reader)
	{
		int num = 0;
		do
		{
			ModificationCommand modificationCommand = ModificationCommands[commandIndex];
			if (!reader.Read())
			{
				int num2 = num + 1;
				while (++commandIndex < CommandResultSet.Count && CommandResultSet[commandIndex - 1] == ResultSetMapping.NotLastInResultSet)
				{
					num2++;
				}
				ThrowAggregateUpdateConcurrencyException(commandIndex, num2, num);
			}
			IRelationalValueBufferFactory relationalValueBufferFactory = CreateValueBufferFactory(modificationCommand.ColumnModifications);
			modificationCommand.PropagateResults(relationalValueBufferFactory.Create(reader.DbDataReader));
			num++;
		}
		while (++commandIndex < CommandResultSet.Count && CommandResultSet[commandIndex - 1] == ResultSetMapping.NotLastInResultSet);
		return commandIndex;
	}

	protected virtual async Task<int> ConsumeResultSetWithPropagationAsync(int commandIndex, [JetBrains.Annotations.NotNull] RelationalDataReader reader, CancellationToken cancellationToken)
	{
		int rowsAffected = 0;
		int num2;
		do
		{
			ModificationCommand tableModification = ModificationCommands[commandIndex];
			if (!(await reader.ReadAsync(cancellationToken)))
			{
				int num = rowsAffected + 1;
				while (true)
				{
					num2 = commandIndex + 1;
					commandIndex = num2;
					if (num2 >= CommandResultSet.Count || CommandResultSet[commandIndex - 1] != ResultSetMapping.NotLastInResultSet)
					{
						break;
					}
					num++;
				}
				ThrowAggregateUpdateConcurrencyException(commandIndex, num, rowsAffected);
			}
			IRelationalValueBufferFactory relationalValueBufferFactory = CreateValueBufferFactory(tableModification.ColumnModifications);
			tableModification.PropagateResults(relationalValueBufferFactory.Create(reader.DbDataReader));
			rowsAffected++;
			num2 = commandIndex + 1;
			commandIndex = num2;
		}
		while (num2 < CommandResultSet.Count && CommandResultSet[commandIndex - 1] == ResultSetMapping.NotLastInResultSet);
		return commandIndex;
	}

	protected virtual int ConsumeResultSetWithoutPropagation(int commandIndex, [JetBrains.Annotations.NotNull] RelationalDataReader reader)
	{
		int num = 1;
		while (++commandIndex < CommandResultSet.Count && CommandResultSet[commandIndex - 1] == ResultSetMapping.NotLastInResultSet)
		{
			num++;
		}
		if (reader.Read())
		{
			int @int = reader.DbDataReader.GetInt32(0);
			if (@int != num)
			{
				ThrowAggregateUpdateConcurrencyException(commandIndex, num, @int);
			}
		}
		else
		{
			ThrowAggregateUpdateConcurrencyException(commandIndex, 1, 0);
		}
		return commandIndex;
	}

	protected virtual async Task<int> ConsumeResultSetWithoutPropagationAsync(int commandIndex, [JetBrains.Annotations.NotNull] RelationalDataReader reader, CancellationToken cancellationToken)
	{
		int expectedRowsAffected = 1;
		while (true)
		{
			int num = commandIndex + 1;
			commandIndex = num;
			if (num >= CommandResultSet.Count || CommandResultSet[commandIndex - 1] != ResultSetMapping.NotLastInResultSet)
			{
				break;
			}
			expectedRowsAffected++;
		}
		if (await reader.ReadAsync(cancellationToken))
		{
			int @int = reader.DbDataReader.GetInt32(0);
			if (@int != expectedRowsAffected)
			{
				ThrowAggregateUpdateConcurrencyException(commandIndex, expectedRowsAffected, @int);
			}
		}
		else
		{
			ThrowAggregateUpdateConcurrencyException(commandIndex, 1, 0);
		}
		return commandIndex;
	}

	private IReadOnlyList<IUpdateEntry> AggregateEntries(int endIndex, int commandCount)
	{
		List<IUpdateEntry> list = new List<IUpdateEntry>();
		for (int i = endIndex - commandCount; i < endIndex; i++)
		{
			list.AddRange(ModificationCommands[i].Entries);
		}
		return list;
	}

	protected virtual void ThrowAggregateUpdateConcurrencyException(int commandIndex, int expectedRowsAffected, int rowsAffected)
	{
		throw new DbUpdateConcurrencyException(RelationalStrings.UpdateConcurrencyException(expectedRowsAffected, rowsAffected), AggregateEntries(commandIndex, expectedRowsAffected));
	}
}
