using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class IndexExtensions
{
	public static INullableValueFactory<TKey> GetNullableValueFactory<TKey>([JetBrains.Annotations.NotNull] this IIndex index)
	{
		return index.AsIndex("GetNullableValueFactory").GetNullableValueFactory<TKey>();
	}

	public static Index AsIndex([JetBrains.Annotations.NotNull] this IIndex index, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<IIndex, Index>(index, methodName);
	}

	public static string ToDebugString([JetBrains.Annotations.NotNull] this IIndex index, bool singleLine = true, [JetBrains.Annotations.NotNull] string indent = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(indent);
		if (singleLine)
		{
			stringBuilder.Append("Index: ");
		}
		stringBuilder.Append(string.Join(", ", index.Properties.Select((IProperty p) => (!singleLine) ? p.Name : (p.DeclaringEntityType.DisplayName() + "." + p.Name))));
		if (index.IsUnique)
		{
			stringBuilder.Append(" Unique");
		}
		if (!singleLine)
		{
			stringBuilder.Append(index.AnnotationsToDebugString(indent + "  "));
		}
		return stringBuilder.ToString();
	}
}
