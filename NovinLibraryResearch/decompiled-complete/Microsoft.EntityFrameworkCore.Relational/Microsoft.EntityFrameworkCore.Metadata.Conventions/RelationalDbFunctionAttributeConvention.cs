using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RelationalDbFunctionAttributeConvention : IModelInitializedConvention, IConvention, IModelAnnotationChangedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public RelationalDbFunctionAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessModelInitialized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		Type type = Dependencies.ContextType;
		while (type != null && type != typeof(DbContext))
		{
			foreach (MethodInfo item in from mi in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
				where mi.IsDefined(typeof(DbFunctionAttribute))
				select mi)
			{
				modelBuilder.HasDbFunction(item);
			}
			type = type.BaseType;
		}
	}

	public virtual void ProcessModelAnnotationChanged(IConventionModelBuilder modelBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		if (name.StartsWith("Relational:DbFunction", StringComparison.Ordinal) && annotation != null && annotation.Value != null && oldAnnotation == null)
		{
			ProcessDbFunctionAdded(new DbFunctionBuilder((IMutableDbFunction)annotation.Value), context);
		}
	}

	protected virtual void ProcessDbFunctionAdded([JetBrains.Annotations.NotNull] IConventionDbFunctionBuilder dbFunctionBuilder, [JetBrains.Annotations.NotNull] IConventionContext context)
	{
		MethodInfo methodInfo = dbFunctionBuilder.Metadata.MethodInfo;
		DbFunctionAttribute dbFunctionAttribute = methodInfo.GetCustomAttributes<DbFunctionAttribute>().SingleOrDefault();
		dbFunctionBuilder.HasName(dbFunctionAttribute?.Name ?? methodInfo.Name);
		dbFunctionBuilder.HasSchema(dbFunctionAttribute?.Schema);
	}
}
