using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class KeyExtensions
{
	public static Func<bool, IIdentityMap> GetIdentityMapFactory([JetBrains.Annotations.NotNull] this IKey key)
	{
		return key.AsKey("GetIdentityMapFactory").IdentityMapFactory;
	}

	public static IPrincipalKeyValueFactory<TKey> GetPrincipalKeyValueFactory<TKey>([JetBrains.Annotations.NotNull] this IKey key)
	{
		return key.AsKey("GetPrincipalKeyValueFactory").GetPrincipalKeyValueFactory<TKey>();
	}

	public static int IndexOf([JetBrains.Annotations.NotNull] this IKey key, [JetBrains.Annotations.NotNull] IProperty property)
	{
		int i;
		for (i = 0; i < key.Properties.Count && key.Properties[i] != property; i++)
		{
		}
		if (i != key.Properties.Count)
		{
			return i;
		}
		return -1;
	}

	public static string ToDebugString([JetBrains.Annotations.NotNull] this IKey key, bool singleLine = true, [JetBrains.Annotations.NotNull] string indent = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(indent);
		if (singleLine)
		{
			stringBuilder.Append("Key: ");
		}
		stringBuilder.Append(string.Join(", ", key.Properties.Select((IProperty p) => (!singleLine) ? p.Name : (p.DeclaringEntityType.DisplayName() + "." + p.Name))));
		if (key.IsPrimaryKey())
		{
			stringBuilder.Append(" PK");
		}
		if (!singleLine)
		{
			stringBuilder.Append(key.AnnotationsToDebugString(indent + "  "));
		}
		return stringBuilder.ToString();
	}

	public static Key AsKey([JetBrains.Annotations.NotNull] this IKey key, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<IKey, Key>(key, methodName);
	}
}
