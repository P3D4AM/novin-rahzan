using System.Reflection;
using System.Resources;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public static class AbstractionsStrings
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Properties.AbstractionsStrings", typeof(AbstractionsStrings).GetTypeInfo().Assembly);

	public static string ArgumentIsEmpty([CanBeNull] object argumentName)
	{
		return string.Format(GetString("ArgumentIsEmpty", "argumentName"), argumentName);
	}

	public static string CollectionArgumentIsEmpty([CanBeNull] object argumentName)
	{
		return string.Format(GetString("CollectionArgumentIsEmpty", "argumentName"), argumentName);
	}

	private static string GetString(string name, params string[] formatterNames)
	{
		string text = _resourceManager.GetString(name);
		for (int i = 0; i < formatterNames.Length; i++)
		{
			text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
		}
		return text;
	}
}
