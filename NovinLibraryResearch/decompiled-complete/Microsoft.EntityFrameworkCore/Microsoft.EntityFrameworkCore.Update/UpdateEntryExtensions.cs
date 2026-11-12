using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update;

public static class UpdateEntryExtensions
{
	public static string BuildCurrentValuesString([JetBrains.Annotations.NotNull] this IUpdateEntry entry, [JetBrains.Annotations.NotNull] IEnumerable<IPropertyBase> properties)
	{
		return "{" + string.Join(", ", properties.Select((IPropertyBase p) => p.Name + ": " + Convert.ToString(entry.GetCurrentValue(p), CultureInfo.InvariantCulture))) + "}";
	}

	public static string BuildOriginalValuesString([JetBrains.Annotations.NotNull] this IUpdateEntry entry, [JetBrains.Annotations.NotNull] IEnumerable<IPropertyBase> properties)
	{
		return "{" + string.Join(", ", properties.Select((IPropertyBase p) => p.Name + ": " + Convert.ToString(entry.GetOriginalValue(p), CultureInfo.InvariantCulture))) + "}";
	}
}
