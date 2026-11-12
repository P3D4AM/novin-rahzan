using System;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class NavigationExtensions
{
	[Obsolete]
	public static string ToDebugString([JetBrains.Annotations.NotNull] this INavigation navigation, bool singleLine, bool includeIndexes, [JetBrains.Annotations.NotNull] string indent)
	{
		return navigation.ToDebugString(singleLine, includeIndexes, indent, detailed: true);
	}

	public static string ToDebugString([JetBrains.Annotations.NotNull] this INavigation navigation, bool singleLine = true, bool includeIndexes = false, [JetBrains.Annotations.NotNull] string indent = "", bool detailed = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(indent);
		if (singleLine)
		{
			stringBuilder.Append("Navigation: " + navigation.DeclaringEntityType.DisplayName() + ".");
		}
		stringBuilder.Append(navigation.Name);
		if (!detailed)
		{
			return stringBuilder.ToString();
		}
		string fieldName = navigation.GetFieldName();
		if (fieldName == null)
		{
			stringBuilder.Append(" (no field, ");
		}
		else if (!fieldName.EndsWith(">k__BackingField", StringComparison.Ordinal))
		{
			stringBuilder.Append(" (" + fieldName + ", ");
		}
		else
		{
			stringBuilder.Append(" (");
		}
		stringBuilder.Append(navigation.ClrType?.ShortDisplayName()).Append(")");
		if (navigation.IsCollection())
		{
			stringBuilder.Append(" Collection");
		}
		stringBuilder.Append(navigation.IsDependentToPrincipal() ? " ToPrincipal " : " ToDependent ");
		stringBuilder.Append(navigation.GetTargetType().DisplayName());
		if (navigation.FindInverse() != null)
		{
			stringBuilder.Append(" Inverse: ").Append(navigation.FindInverse().Name);
		}
		if (navigation.GetPropertyAccessMode() != PropertyAccessMode.PreferField)
		{
			stringBuilder.Append(" PropertyAccessMode.").Append(navigation.GetPropertyAccessMode());
		}
		if (includeIndexes)
		{
			PropertyIndexes propertyIndexes = navigation.GetPropertyIndexes();
			stringBuilder.Append(" ").Append(propertyIndexes.Index);
			stringBuilder.Append(" ").Append(propertyIndexes.OriginalValueIndex);
			stringBuilder.Append(" ").Append(propertyIndexes.RelationshipIndex);
			stringBuilder.Append(" ").Append(propertyIndexes.ShadowIndex);
			stringBuilder.Append(" ").Append(propertyIndexes.StoreGenerationIndex);
		}
		if (!singleLine)
		{
			stringBuilder.Append(navigation.AnnotationsToDebugString(indent + "  "));
		}
		return stringBuilder.ToString();
	}

	public static MemberIdentity CreateMemberIdentity([JetBrains.Annotations.CanBeNull] this INavigation navigation)
	{
		if (!(navigation?.GetIdentifyingMemberInfo() == null))
		{
			return MemberIdentity.Create(navigation.GetIdentifyingMemberInfo());
		}
		return MemberIdentity.Create(navigation?.Name);
	}

	public static Navigation AsNavigation([JetBrains.Annotations.NotNull] this INavigation navigation, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<INavigation, Navigation>(navigation, methodName);
	}
}
