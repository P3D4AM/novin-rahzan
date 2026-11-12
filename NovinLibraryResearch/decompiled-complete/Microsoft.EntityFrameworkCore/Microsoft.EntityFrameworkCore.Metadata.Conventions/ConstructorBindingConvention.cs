using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ConstructorBindingConvention : IModelFinalizedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ConstructorBindingConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			Type clrType = entityType.ClrType;
			if ((object)clrType == null || clrType.IsAbstract || !entityType.Builder.CanSetAnnotation("ConstructorBinding", null))
			{
				continue;
			}
			int num = 0;
			int num2 = int.MaxValue;
			List<InstantiationBinding> list = new List<InstantiationBinding>();
			List<IEnumerable<ParameterInfo>> list2 = new List<IEnumerable<ParameterInfo>>();
			foreach (ConstructorInfo item in entityType.ClrType.GetTypeInfo().DeclaredConstructors.Where((ConstructorInfo c) => !c.IsStatic))
			{
				if (Dependencies.ConstructorBindingFactory.TryBindConstructor(entityType, item, out var binding, out var unboundParameters))
				{
					int num3 = binding.ParameterBindings.OfType<ServiceParameterBinding>().Count();
					int num4 = binding.ParameterBindings.Count - num3;
					if (num3 == num && num4 == num2)
					{
						list.Add(binding);
					}
					else if (num3 > num)
					{
						list.Clear();
						list.Add(binding);
						num = num3;
						num2 = num4;
					}
					else if (num4 < num2)
					{
						list.Clear();
						list.Add(binding);
						num = num3;
						num2 = num4;
					}
				}
				else
				{
					list2.Add(unboundParameters);
				}
			}
			if (list.Count == 0)
			{
				IEnumerable<string> values = from f in list2.SelectMany((IEnumerable<ParameterInfo> f) => f)
					group f by f.Member as ConstructorInfo into x
					select CoreStrings.ConstructorBindingFailed(string.Join("', '", x.Select((ParameterInfo f) => f.Name)), entityType.DisplayName() + "(" + string.Join(", ", from y in x.Key.GetParameters()
						select y.ParameterType.ShortDisplayName() + " " + y.Name) + ")");
				throw new InvalidOperationException(CoreStrings.ConstructorNotFound(entityType.DisplayName(), string.Join("; ", values)));
			}
			if (list.Count > 1)
			{
				throw new InvalidOperationException(CoreStrings.ConstructorConflict(FormatConstructorString(entityType, list[0]), FormatConstructorString(entityType, list[1])));
			}
			entityType.Builder.HasAnnotation("ConstructorBinding", list[0]);
		}
	}

	private static string FormatConstructorString(IEntityType entityType, InstantiationBinding binding)
	{
		return entityType.ClrType.ShortDisplayName() + "(" + string.Join(", ", binding.ParameterBindings.Select((ParameterBinding b) => b.ParameterType.ShortDisplayName())) + ")";
	}
}
