using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalModelBuilderExtensions
{
	public static SequenceBuilder HasSequence([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		return new SequenceBuilder(GetOrAddSequence(modelBuilder, name, schema));
	}

	public static ModelBuilder HasSequence([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Action<SequenceBuilder> builderAction)
	{
		return modelBuilder.HasSequence(name, null, builderAction);
	}

	public static ModelBuilder HasSequence([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] Action<SequenceBuilder> builderAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderAction, "builderAction");
		builderAction(modelBuilder.HasSequence(name, schema));
		return modelBuilder;
	}

	public static SequenceBuilder HasSequence([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		IMutableSequence orAddSequence = GetOrAddSequence(modelBuilder, name, schema);
		orAddSequence.ClrType = clrType;
		return new SequenceBuilder(orAddSequence);
	}

	public static ModelBuilder HasSequence([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Action<SequenceBuilder> builderAction)
	{
		return modelBuilder.HasSequence(clrType, name, null, builderAction);
	}

	public static ModelBuilder HasSequence([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] Action<SequenceBuilder> builderAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderAction, "builderAction");
		builderAction(modelBuilder.HasSequence(clrType, name, schema));
		return modelBuilder;
	}

	public static SequenceBuilder HasSequence<T>([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		IMutableSequence orAddSequence = GetOrAddSequence(modelBuilder, name, schema);
		orAddSequence.ClrType = typeof(T);
		return new SequenceBuilder(orAddSequence);
	}

	public static ModelBuilder HasSequence<T>([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Action<SequenceBuilder> builderAction)
	{
		return modelBuilder.HasSequence<T>(name, null, builderAction);
	}

	public static ModelBuilder HasSequence<T>([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] Action<SequenceBuilder> builderAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderAction, "builderAction");
		builderAction(modelBuilder.HasSequence<T>(name, schema));
		return modelBuilder;
	}

	private static IMutableSequence GetOrAddSequence(ModelBuilder modelBuilder, string name, string schema)
	{
		IMutableSequence mutableSequence = modelBuilder.Model.FindSequence(name, schema);
		if (mutableSequence != null)
		{
			((Sequence)mutableSequence).UpdateConfigurationSource(ConfigurationSource.Explicit);
			return mutableSequence;
		}
		return modelBuilder.Model.AddSequence(name, schema);
	}

	private static IConventionSequence GetOrAddSequence(IConventionModelBuilder modelBuilder, string name, string schema, bool fromDataAnnotation)
	{
		IConventionSequence conventionSequence = modelBuilder.Metadata.FindSequence(name, schema);
		if (conventionSequence != null)
		{
			((Sequence)conventionSequence).UpdateConfigurationSource(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
			return conventionSequence;
		}
		return modelBuilder.Metadata.AddSequence(name, schema);
	}

	public static IConventionSequenceBuilder HasSequence([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		return new SequenceBuilder((IMutableSequence)GetOrAddSequence(modelBuilder, name, schema, fromDataAnnotation));
	}

	public static DbFunctionBuilder HasDbFunction([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] MethodInfo methodInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo");
		IMutableDbFunction mutableDbFunction = modelBuilder.Model.FindDbFunction(methodInfo);
		if (mutableDbFunction == null)
		{
			mutableDbFunction = modelBuilder.Model.AddDbFunction(methodInfo);
		}
		else
		{
			((DbFunction)mutableDbFunction).UpdateConfigurationSource(ConfigurationSource.Explicit);
		}
		return new DbFunctionBuilder(mutableDbFunction);
	}

	public static DbFunctionBuilder HasDbFunction<TResult>([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] Expression<Func<TResult>> expression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(expression, "expression");
		MethodInfo methodInfo = (expression.Body as MethodCallExpression)?.Method;
		if (methodInfo == null)
		{
			throw new ArgumentException(RelationalStrings.DbFunctionExpressionIsNotMethodCall(expression));
		}
		return modelBuilder.HasDbFunction(methodInfo);
	}

	public static ModelBuilder HasDbFunction([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] MethodInfo methodInfo, [JetBrains.Annotations.NotNull] Action<DbFunctionBuilder> builderAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builderAction, "builderAction");
		builderAction(modelBuilder.HasDbFunction(methodInfo));
		return modelBuilder;
	}

	public static IConventionDbFunctionBuilder HasDbFunction([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] MethodInfo methodInfo, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo");
		IConventionDbFunction conventionDbFunction = modelBuilder.Metadata.FindDbFunction(methodInfo);
		if (conventionDbFunction == null)
		{
			conventionDbFunction = modelBuilder.Metadata.AddDbFunction(methodInfo);
		}
		else
		{
			((DbFunction)conventionDbFunction).UpdateConfigurationSource(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
		}
		return new DbFunctionBuilder((IMutableDbFunction)conventionDbFunction);
	}

	public static ModelBuilder HasDefaultSchema([JetBrains.Annotations.NotNull] this ModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string schema)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		modelBuilder.Model.SetDefaultSchema(schema);
		return modelBuilder;
	}

	public static IConventionModelBuilder HasDefaultSchema([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
	{
		if (modelBuilder.CanSetDefaultSchema(schema, fromDataAnnotation))
		{
			modelBuilder.Metadata.SetDefaultSchema(schema);
			return modelBuilder;
		}
		return null;
	}

	public static bool CanSetDefaultSchema([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, [JetBrains.Annotations.CanBeNull] string schema, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		return modelBuilder.CanSetAnnotation("Relational:DefaultSchema", schema, fromDataAnnotation);
	}

	public static IConventionModelBuilder HasMaxIdentifierLength([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, int? length, bool fromDataAnnotation = false)
	{
		if (modelBuilder.CanSetMaxIdentifierLength(length, fromDataAnnotation))
		{
			modelBuilder.Metadata.SetMaxIdentifierLength(length);
			return modelBuilder;
		}
		return null;
	}

	public static bool CanSetMaxIdentifierLength([JetBrains.Annotations.NotNull] this IConventionModelBuilder modelBuilder, int? length, bool fromDataAnnotation = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		return modelBuilder.CanSetAnnotation("Relational:MaxIdentifierLength", length, fromDataAnnotation);
	}
}
