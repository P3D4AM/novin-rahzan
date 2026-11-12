using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public class MigrationCommandListBuilder
{
	private readonly List<MigrationCommand> _commands = new List<MigrationCommand>();

	private readonly MigrationsSqlGeneratorDependencies _dependencies;

	private IRelationalCommandBuilder _commandBuilder;

	public MigrationCommandListBuilder([JetBrains.Annotations.NotNull] MigrationsSqlGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		_dependencies = dependencies;
		_commandBuilder = dependencies.CommandBuilderFactory.Create();
	}

	public virtual IReadOnlyList<MigrationCommand> GetCommandList()
	{
		return _commands;
	}

	public virtual MigrationCommandListBuilder EndCommand(bool suppressTransaction = false)
	{
		if (_commandBuilder.CommandTextLength != 0)
		{
			_commands.Add(new MigrationCommand(_commandBuilder.Build(), _dependencies.CurrentContext.Context, _dependencies.Logger, suppressTransaction));
			_commandBuilder = _dependencies.CommandBuilderFactory.Create();
		}
		return this;
	}

	public virtual MigrationCommandListBuilder Append([JetBrains.Annotations.NotNull] object o)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(o, "o");
		_commandBuilder.Append(o);
		return this;
	}

	public virtual MigrationCommandListBuilder AppendLine()
	{
		_commandBuilder.AppendLine();
		return this;
	}

	public virtual MigrationCommandListBuilder AppendLine([JetBrains.Annotations.NotNull] object o)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(o, "o");
		_commandBuilder.AppendLine(o);
		return this;
	}

	public virtual MigrationCommandListBuilder AppendLines([JetBrains.Annotations.NotNull] object o)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(o, "o");
		_commandBuilder.AppendLines(o);
		return this;
	}

	public virtual IDisposable Indent()
	{
		return _commandBuilder.Indent();
	}

	public virtual MigrationCommandListBuilder IncrementIndent()
	{
		_commandBuilder.IncrementIndent();
		return this;
	}

	public virtual MigrationCommandListBuilder DecrementIndent()
	{
		_commandBuilder.DecrementIndent();
		return this;
	}
}
