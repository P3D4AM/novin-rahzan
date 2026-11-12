using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalCommandBuilder : IRelationalCommandBuilder
{
	private readonly List<IRelationalParameter> _parameters = new List<IRelationalParameter>();

	private readonly IndentedStringBuilder _commandTextBuilder = new IndentedStringBuilder();

	public virtual RelationalCommandBuilderDependencies Dependencies { get; }

	public virtual IRelationalTypeMappingSource TypeMappingSource => Dependencies.TypeMappingSource;

	public virtual IReadOnlyList<IRelationalParameter> Parameters => _parameters;

	public virtual int CommandTextLength => _commandTextBuilder.Length;

	public RelationalCommandBuilder([JetBrains.Annotations.NotNull] RelationalCommandBuilderDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual IRelationalCommand Build()
	{
		return new RelationalCommand(Dependencies, _commandTextBuilder.ToString(), Parameters);
	}

	public override string ToString()
	{
		return _commandTextBuilder.ToString();
	}

	public virtual IRelationalCommandBuilder AddParameter(IRelationalParameter parameter)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameter, "parameter");
		_parameters.Add(parameter);
		return this;
	}

	public virtual IRelationalCommandBuilder Append(object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		_commandTextBuilder.Append(value);
		return this;
	}

	public virtual IRelationalCommandBuilder AppendLine()
	{
		_commandTextBuilder.AppendLine();
		return this;
	}

	public virtual IRelationalCommandBuilder IncrementIndent()
	{
		_commandTextBuilder.IncrementIndent();
		return this;
	}

	public virtual IRelationalCommandBuilder DecrementIndent()
	{
		_commandTextBuilder.DecrementIndent();
		return this;
	}
}
