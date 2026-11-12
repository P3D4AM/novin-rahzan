using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class TypeMappingSourceBase : ITypeMappingSource
{
	protected virtual TypeMappingSourceDependencies Dependencies { get; }

	protected TypeMappingSourceBase([JetBrains.Annotations.NotNull] TypeMappingSourceDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	protected virtual CoreTypeMapping FindMapping(in TypeMappingInfo mappingInfo)
	{
		foreach (ITypeMappingSourcePlugin plugin in Dependencies.Plugins)
		{
			CoreTypeMapping coreTypeMapping = plugin.FindMapping(in mappingInfo);
			if (coreTypeMapping != null)
			{
				return coreTypeMapping;
			}
		}
		return null;
	}

	protected virtual void ValidateMapping([JetBrains.Annotations.CanBeNull] CoreTypeMapping mapping, [JetBrains.Annotations.CanBeNull] IProperty property)
	{
	}

	public abstract CoreTypeMapping FindMapping(IProperty property);

	public abstract CoreTypeMapping FindMapping(Type type);

	public abstract CoreTypeMapping FindMapping(MemberInfo member);
}
