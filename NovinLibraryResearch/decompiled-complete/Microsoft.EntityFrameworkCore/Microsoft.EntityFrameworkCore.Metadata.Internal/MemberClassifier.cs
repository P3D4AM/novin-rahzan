using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class MemberClassifier : IMemberClassifier
{
	private readonly ITypeMappingSource _typeMappingSource;

	private readonly IParameterBindingFactories _parameterBindingFactories;

	public MemberClassifier([JetBrains.Annotations.NotNull] ITypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] IParameterBindingFactories parameterBindingFactories)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterBindingFactories, "parameterBindingFactories");
		_typeMappingSource = typeMappingSource;
		_parameterBindingFactories = parameterBindingFactories;
	}

	public virtual Type FindCandidateNavigationPropertyType(MemberInfo memberInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberInfo, "memberInfo");
		Type memberType = memberInfo.GetMemberType();
		Type type = memberType.TryGetSequenceType();
		if (!(memberInfo is PropertyInfo propertyInfo) || !propertyInfo.IsCandidateProperty(type == null))
		{
			return null;
		}
		memberType = type ?? memberType;
		memberType = memberType.UnwrapNullableType();
		if (!memberType.GetTypeInfo().IsInterface && !memberType.GetTypeInfo().IsValueType && !(memberType == typeof(object)) && _parameterBindingFactories.FindFactory(memberType, memberInfo.GetSimpleMemberName()) == null && _typeMappingSource.FindMapping(memberType) == null && !memberType.GetTypeInfo().IsArray)
		{
			return memberType;
		}
		return null;
	}
}
