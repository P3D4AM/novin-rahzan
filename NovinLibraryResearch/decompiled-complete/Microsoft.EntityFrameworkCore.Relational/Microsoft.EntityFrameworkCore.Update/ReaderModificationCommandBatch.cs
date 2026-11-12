using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Update;

public abstract class ReaderModificationCommandBatch : ModificationCommandBatch
{
	private readonly List<ModificationCommand> _modificationCommands = new List<ModificationCommand>();

	public virtual ModificationCommandBatchFactoryDependencies Dependencies { get; }

	protected virtual IUpdateSqlGenerator UpdateSqlGenerator => Dependencies.UpdateSqlGenerator;

	protected virtual StringBuilder CachedCommandText
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	protected virtual int LastCachedCommandIndex { get; set; }

	public override IReadOnlyList<ModificationCommand> ModificationCommands => _modificationCommands;

	protected virtual IList<ResultSetMapping> CommandResultSet { get; } = new List<ResultSetMapping>();


	protected ReaderModificationCommandBatch([JetBrains.Annotations.NotNull] ModificationCommandBatchFactoryDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public override bool AddCommand(ModificationCommand modificationCommand)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modificationCommand, "modificationCommand");
		if (ModificationCommands.Count == 0)
		{
			ResetCommandText();
		}
		if (!CanAddCommand(modificationCommand))
		{
			return false;
		}
		_modificationCommands.Add(modificationCommand);
		CommandResultSet.Add(ResultSetMapping.LastInResultSet);
		if (!IsCommandTextValid())
		{
			ResetCommandText();
			_modificationCommands.RemoveAt(_modificationCommands.Count - 1);
			CommandResultSet.RemoveAt(CommandResultSet.Count - 1);
			return false;
		}
		return true;
	}

	protected virtual void ResetCommandText()
	{
		CachedCommandText = new StringBuilder();
		UpdateSqlGenerator.AppendBatchHeader(CachedCommandText);
		LastCachedCommandIndex = -1;
	}

	protected abstract bool CanAddCommand([JetBrains.Annotations.NotNull] ModificationCommand modificationCommand);

	protected abstract bool IsCommandTextValid();

	protected virtual string GetCommandText()
	{
		for (int i = LastCachedCommandIndex + 1; i < ModificationCommands.Count; i++)
		{
			UpdateCachedCommandText(i);
		}
		return CachedCommandText.ToString();
	}

	protected virtual void UpdateCachedCommandText(int commandPosition)
	{
		ModificationCommand modificationCommand = ModificationCommands[commandPosition];
		switch (modificationCommand.EntityState)
		{
		case EntityState.Added:
			CommandResultSet[commandPosition] = UpdateSqlGenerator.AppendInsertOperation(CachedCommandText, modificationCommand, commandPosition);
			break;
		case EntityState.Modified:
			CommandResultSet[commandPosition] = UpdateSqlGenerator.AppendUpdateOperation(CachedCommandText, modificationCommand, commandPosition);
			break;
		case EntityState.Deleted:
			CommandResultSet[commandPosition] = UpdateSqlGenerator.AppendDeleteOperation(CachedCommandText, modificationCommand, commandPosition);
			break;
		}
		LastCachedCommandIndex = commandPosition;
	}

	protected virtual int GetParameterCount()
	{
		return ModificationCommands.Sum((ModificationCommand c) => c.ColumnModifications.Count);
	}

	protected virtual RawSqlCommand CreateStoreCommand()
	{
		IRelationalCommandBuilder relationalCommandBuilder = Dependencies.CommandBuilderFactory.Create().Append(GetCommandText());
		Dictionary<string, object> dictionary = new Dictionary<string, object>(GetParameterCount());
		for (int i = 0; i < ModificationCommands.Count; i++)
		{
			ModificationCommand modificationCommand = ModificationCommands[i];
			for (int j = 0; j < modificationCommand.ColumnModifications.Count; j++)
			{
				ColumnModification columnModification = modificationCommand.ColumnModifications[j];
				if (columnModification.UseCurrentValueParameter)
				{
					relationalCommandBuilder.AddParameter(columnModification.ParameterName, Dependencies.SqlGenerationHelper.GenerateParameterName(columnModification.ParameterName), columnModification.Property);
					dictionary.Add(columnModification.ParameterName, columnModification.Value);
				}
				if (columnModification.UseOriginalValueParameter)
				{
					relationalCommandBuilder.AddParameter(columnModification.OriginalParameterName, Dependencies.SqlGenerationHelper.GenerateParameterName(columnModification.OriginalParameterName), columnModification.Property);
					dictionary.Add(columnModification.OriginalParameterName, columnModification.OriginalValue);
				}
			}
		}
		return new RawSqlCommand(relationalCommandBuilder.Build(), dictionary);
	}

	public override void Execute(IRelationalConnection connection)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		RawSqlCommand rawSqlCommand = CreateStoreCommand();
		try
		{
			using RelationalDataReader reader = rawSqlCommand.RelationalCommand.ExecuteReader(new RelationalCommandParameterObject(connection, rawSqlCommand.ParameterValues, null, Dependencies.CurrentContext.Context, Dependencies.Logger));
			Consume(reader);
		}
		catch (DbUpdateException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new DbUpdateException(RelationalStrings.UpdateStoreException, innerException);
		}
	}

	public override async Task ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken = default(CancellationToken))
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		RawSqlCommand rawSqlCommand = CreateStoreCommand();
		try
		{
			await using RelationalDataReader dataReader = await rawSqlCommand.RelationalCommand.ExecuteReaderAsync(new RelationalCommandParameterObject(connection, rawSqlCommand.ParameterValues, null, Dependencies.CurrentContext.Context, Dependencies.Logger), cancellationToken);
			await ConsumeAsync(dataReader, cancellationToken);
		}
		catch (DbUpdateException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new DbUpdateException(RelationalStrings.UpdateStoreException, innerException);
		}
	}

	protected abstract void Consume([JetBrains.Annotations.NotNull] RelationalDataReader reader);

	protected abstract Task ConsumeAsync([JetBrains.Annotations.NotNull] RelationalDataReader reader, CancellationToken cancellationToken = default(CancellationToken));

	protected virtual IRelationalValueBufferFactory CreateValueBufferFactory([JetBrains.Annotations.NotNull] IReadOnlyList<ColumnModification> columnModifications)
	{
		return Dependencies.ValueBufferFactoryFactory.Create((from c in Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columnModifications, "columnModifications")
			where c.IsRead
			select new TypeMaterializationInfo(c.Property.ClrType, c.Property, null)).ToArray());
	}
}
