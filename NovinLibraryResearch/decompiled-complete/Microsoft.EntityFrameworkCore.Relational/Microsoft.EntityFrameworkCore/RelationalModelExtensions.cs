using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalModelExtensions
{
	public static string GetDefaultSchema([JetBrains.Annotations.NotNull] this IModel model)
	{
		return (string)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model")["Relational:DefaultSchema"];
	}

	public static void SetDefaultSchema([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.CanBeNull] string value)
	{
		model.SetOrRemoveAnnotation("Relational:DefaultSchema", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}

	public static void SetDefaultSchema([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.CanBeNull] string value, bool fromDataAnnotation = false)
	{
		model.SetOrRemoveAnnotation("Relational:DefaultSchema", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"), fromDataAnnotation);
	}

	public static ConfigurationSource? GetDefaultSchemaConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("Relational:DefaultSchema")?.GetConfigurationSource();
	}

	public static int GetMaxIdentifierLength([JetBrains.Annotations.NotNull] this IModel model)
	{
		return ((int?)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model")["Relational:MaxIdentifierLength"]) ?? 32767;
	}

	public static void SetMaxIdentifierLength([JetBrains.Annotations.NotNull] this IMutableModel model, int? length)
	{
		model.SetOrRemoveAnnotation("Relational:MaxIdentifierLength", length);
	}

	public static void SetMaxIdentifierLength([JetBrains.Annotations.NotNull] this IConventionModel model, int? length, bool fromDataAnnotation = false)
	{
		model.SetOrRemoveAnnotation("Relational:MaxIdentifierLength", length, fromDataAnnotation);
	}

	public static ConfigurationSource? GetMaxIdentifierLengthConfigurationSource([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return model.FindAnnotation("Relational:MaxIdentifierLength")?.GetConfigurationSource();
	}

	public static ISequence FindSequence([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		return Sequence.FindSequence(model, name, schema);
	}

	public static IMutableSequence FindSequence([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return (IMutableSequence)((IModel)model).FindSequence(name, schema);
	}

	public static IConventionSequence FindSequence([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return (IConventionSequence)((IModel)model).FindSequence(name, schema);
	}

	public static IMutableSequence AddSequence([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return new Sequence(model, name, schema, ConfigurationSource.Explicit);
	}

	public static IConventionSequence AddSequence([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null, bool fromDataAnnotation = false)
	{
		return new Sequence((IMutableModel)model, name, schema, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static IMutableSequence RemoveSequence([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return (IMutableSequence)Sequence.RemoveSequence(model, name, schema);
	}

	public static IConventionSequence RemoveSequence([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		return (IConventionSequence)((IMutableModel)model).RemoveSequence(name, schema);
	}

	public static IReadOnlyList<ISequence> GetSequences([JetBrains.Annotations.NotNull] this IModel model)
	{
		return Sequence.GetSequences(model, "Relational:Sequence:").ToList();
	}

	public static IReadOnlyList<IMutableSequence> GetSequences([JetBrains.Annotations.NotNull] this IMutableModel model)
	{
		return (IReadOnlyList<IMutableSequence>)((IModel)model).GetSequences();
	}

	public static IReadOnlyList<IConventionSequence> GetSequences([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return (IReadOnlyList<IConventionSequence>)((IModel)model).GetSequences();
	}

	public static IDbFunction FindDbFunction([JetBrains.Annotations.NotNull] this IModel model, [JetBrains.Annotations.NotNull] MethodInfo method)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(method, "method");
		return DbFunction.FindDbFunction(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(method, "method"));
	}

	public static IMutableDbFunction FindDbFunction([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] MethodInfo method)
	{
		return (IMutableDbFunction)((IModel)model).FindDbFunction(method);
	}

	public static IConventionDbFunction FindDbFunction([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] MethodInfo method)
	{
		return (IConventionDbFunction)((IModel)model).FindDbFunction(method);
	}

	public static DbFunction AddDbFunction([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] MethodInfo methodInfo)
	{
		return new DbFunction(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo"), model, ConfigurationSource.Explicit);
	}

	public static IConventionDbFunction AddDbFunction([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] MethodInfo methodInfo, bool fromDataAnnotation = false)
	{
		return new DbFunction(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo"), (IMutableModel)model, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public static IMutableDbFunction RemoveDbFunction([JetBrains.Annotations.NotNull] this IMutableModel model, [JetBrains.Annotations.NotNull] MethodInfo method)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(method, "method");
		return DbFunction.RemoveDbFunction(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(method, "method"));
	}

	public static IConventionDbFunction RemoveDbFunction([JetBrains.Annotations.NotNull] this IConventionModel model, [JetBrains.Annotations.NotNull] MethodInfo method)
	{
		return (IConventionDbFunction)((IMutableModel)model).RemoveDbFunction(method);
	}

	public static IEnumerable<IDbFunction> GetDbFunctions([JetBrains.Annotations.NotNull] this IModel model)
	{
		return DbFunction.GetDbFunctions(model.AsModel("GetDbFunctions"));
	}

	public static IEnumerable<IMutableDbFunction> GetDbFunctions([JetBrains.Annotations.NotNull] this IMutableModel model)
	{
		return DbFunction.GetDbFunctions((Model)model);
	}

	public static IEnumerable<IConventionDbFunction> GetDbFunctions([JetBrains.Annotations.NotNull] this IConventionModel model)
	{
		return DbFunction.GetDbFunctions((Model)model);
	}
}
