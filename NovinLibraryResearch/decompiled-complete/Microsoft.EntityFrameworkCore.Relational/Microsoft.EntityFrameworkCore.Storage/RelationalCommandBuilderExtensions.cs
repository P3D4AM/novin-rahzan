using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public static class RelationalCommandBuilderExtensions
{
	private sealed class Indenter : IDisposable
	{
		private readonly IRelationalCommandBuilder _builder;

		public Indenter(IRelationalCommandBuilder builder)
		{
			_builder = builder;
			_builder.IncrementIndent();
		}

		public void Dispose()
		{
			_builder.DecrementIndent();
		}
	}

	public static IRelationalCommandBuilder AppendLine([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		commandBuilder.Append(value).AppendLine();
		return commandBuilder;
	}

	public static IRelationalCommandBuilder AppendLines([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder, [JetBrains.Annotations.NotNull] object value, bool skipFinalNewline = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		using (StringReader stringReader = new StringReader(value.ToString()))
		{
			bool flag = true;
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					commandBuilder.AppendLine();
				}
				if (text.Length != 0)
				{
					commandBuilder.Append(text);
				}
			}
		}
		if (!skipFinalNewline)
		{
			commandBuilder.AppendLine();
		}
		return commandBuilder;
	}

	public static IDisposable Indent([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder)
	{
		return new Indenter(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder"));
	}

	public static IRelationalCommandBuilder AddParameter([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder, [JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		return commandBuilder.AddParameter(new DynamicRelationalParameter(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName"), Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"), commandBuilder.TypeMappingSource));
	}

	public static IRelationalCommandBuilder AddParameter([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder, [JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] RelationalTypeMapping typeMapping, bool nullable)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMapping, "typeMapping");
		return commandBuilder.AddParameter(new TypeMappedRelationalParameter(invariantName, name, typeMapping, nullable));
	}

	public static IRelationalCommandBuilder AddParameter([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder, [JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		return commandBuilder.AddParameter(new TypeMappedRelationalParameter(invariantName, name, property.GetRelationalTypeMapping(), property.IsNullable));
	}

	public static IRelationalCommandBuilder AddCompositeParameter([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder, [JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] IReadOnlyList<IRelationalParameter> subParameters)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(subParameters, "subParameters");
		if (subParameters.Count > 0)
		{
			commandBuilder.AddParameter(new CompositeRelationalParameter(invariantName, subParameters));
		}
		return commandBuilder;
	}

	public static IRelationalCommandBuilder AddRawParameter([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder, [JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] DbParameter dbParameter)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dbParameter, "dbParameter");
		return commandBuilder.AddParameter(new RawRelationalParameter(invariantName, dbParameter));
	}

	public static IRelationalCommandBuilder AddPropertyParameter([JetBrains.Annotations.NotNull] this IRelationalCommandBuilder commandBuilder, [JetBrains.Annotations.NotNull] string invariantName, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(commandBuilder, "commandBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(invariantName, "invariantName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		return commandBuilder.AddParameter(new TypeMappedPropertyRelationalParameter(invariantName, name, property.GetRelationalTypeMapping(), property));
	}
}
