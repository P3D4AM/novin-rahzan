using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("DELETE FROM {Table}")]
public class DeleteDataOperation : MigrationOperation
{
	public virtual string Table
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string Schema
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string[] KeyColumns
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual object[,] KeyValues
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual IEnumerable<ModificationCommand> GenerateModificationCommands([JetBrains.Annotations.CanBeNull] IModel model)
	{
		Dictionary<string, IProperty> properties = ((model == null) ? null : TableMapping.GetTableMapping(model, Table, Schema)?.GetPropertyMap());
		for (int i = 0; i < KeyValues.GetLength(0); i++)
		{
			ColumnModification[] array = new ColumnModification[KeyColumns.Length];
			for (int j = 0; j < KeyColumns.Length; j++)
			{
				array[j] = new ColumnModification(KeyColumns[j], null, KeyValues[i, j], properties?.Find(KeyColumns[j]), isRead: false, isWrite: true, isKey: true, isCondition: true, sensitiveLoggingEnabled: true);
			}
			yield return new ModificationCommand(Table, Schema, array, sensitiveLoggingEnabled: true);
		}
	}
}
