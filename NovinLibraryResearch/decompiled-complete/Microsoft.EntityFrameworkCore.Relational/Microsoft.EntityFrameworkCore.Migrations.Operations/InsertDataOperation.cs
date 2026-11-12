using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("INSERT INTO {Table}")]
public class InsertDataOperation : MigrationOperation
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

	public virtual string[] Columns
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual object[,] Values
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual IEnumerable<ModificationCommand> GenerateModificationCommands([JetBrains.Annotations.CanBeNull] IModel model)
	{
		Dictionary<string, IProperty> properties = ((model == null) ? null : TableMapping.GetTableMapping(model, Table, Schema)?.GetPropertyMap());
		for (int i = 0; i < Values.GetLength(0); i++)
		{
			ColumnModification[] array = new ColumnModification[Columns.Length];
			for (int j = 0; j < Columns.Length; j++)
			{
				array[j] = new ColumnModification(Columns[j], null, Values[i, j], properties?.Find(Columns[j]), isRead: false, isWrite: true, isKey: true, isCondition: false, sensitiveLoggingEnabled: true);
			}
			yield return new ModificationCommand(Table, Schema, array, sensitiveLoggingEnabled: true);
		}
	}
}
