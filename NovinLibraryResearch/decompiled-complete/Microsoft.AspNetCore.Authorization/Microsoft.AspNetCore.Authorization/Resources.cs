using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Microsoft.AspNetCore.Authorization;

internal static class Resources
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.AspNetCore.Authorization.Resources", typeof(Resources).GetTypeInfo().Assembly);

	internal static string Exception_AuthorizationPolicyEmpty => GetString("Exception_AuthorizationPolicyEmpty");

	internal static string Exception_AuthorizationPolicyNotFound => GetString("Exception_AuthorizationPolicyNotFound");

	internal static string Exception_RoleRequirementEmpty => GetString("Exception_RoleRequirementEmpty");

	internal static string FormatException_AuthorizationPolicyEmpty()
	{
		return GetString("Exception_AuthorizationPolicyEmpty");
	}

	internal static string FormatException_AuthorizationPolicyNotFound(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("Exception_AuthorizationPolicyNotFound"), p0);
	}

	internal static string FormatException_RoleRequirementEmpty()
	{
		return GetString("Exception_RoleRequirementEmpty");
	}

	private static string GetString(string name, params string[] formatterNames)
	{
		string text = _resourceManager.GetString(name);
		if (formatterNames != null)
		{
			for (int i = 0; i < formatterNames.Length; i++)
			{
				text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
			}
		}
		return text;
	}
}
