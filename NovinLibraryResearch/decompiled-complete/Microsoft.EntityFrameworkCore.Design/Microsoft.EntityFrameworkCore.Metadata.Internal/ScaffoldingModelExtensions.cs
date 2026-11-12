using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ScaffoldingModelExtensions
{
	public static IDictionary<string, string> GetEntityTypeErrors(this IModel model)
	{
		IDictionary<string, string> dictionary = (IDictionary<string, string>)model["Scaffolding:EntityTypeErrors"];
		if (dictionary == null)
		{
			dictionary = new Dictionary<string, string>();
			(model as IMutableModel)?.SetEntityTypeErrors(dictionary);
		}
		return dictionary;
	}

	public static void SetEntityTypeErrors(this IMutableModel model, IDictionary<string, string> value)
	{
		model.SetAnnotation("Scaffolding:EntityTypeErrors", Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value"));
	}

	public static string GetDatabaseName(this IModel model)
	{
		return (string)model["Scaffolding:DatabaseName"];
	}

	public static void SetDatabaseName(this IMutableModel model, string value)
	{
		model.SetAnnotation("Scaffolding:DatabaseName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}
}
