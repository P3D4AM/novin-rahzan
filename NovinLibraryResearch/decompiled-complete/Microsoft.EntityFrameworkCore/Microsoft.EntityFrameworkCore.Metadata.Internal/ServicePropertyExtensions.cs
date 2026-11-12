using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ServicePropertyExtensions
{
	public static ServiceParameterBinding GetParameterBinding([JetBrains.Annotations.NotNull] this IServiceProperty serviceProperty)
	{
		return serviceProperty.AsServiceProperty("GetParameterBinding").ParameterBinding;
	}

	public static string ToDebugString([JetBrains.Annotations.NotNull] this IServiceProperty serviceProperty, bool singleLine = true, [JetBrains.Annotations.NotNull] string indent = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(indent);
		if (singleLine)
		{
			stringBuilder.Append("Service property: ").Append(serviceProperty.DeclaringType.DisplayName()).Append(".");
		}
		stringBuilder.Append(serviceProperty.Name);
		if (serviceProperty.GetFieldName() == null)
		{
			stringBuilder.Append(" (no field, ");
		}
		else
		{
			stringBuilder.Append(" (").Append(serviceProperty.GetFieldName()).Append(", ");
		}
		stringBuilder.Append(serviceProperty.ClrType?.ShortDisplayName()).Append(")");
		if (!singleLine)
		{
			stringBuilder.Append(serviceProperty.AnnotationsToDebugString(indent + "  "));
		}
		return stringBuilder.ToString();
	}

	public static ServiceProperty AsServiceProperty([JetBrains.Annotations.NotNull] this IServiceProperty serviceProperty, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<IServiceProperty, ServiceProperty>(serviceProperty, methodName);
	}
}
