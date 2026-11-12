using System;
using System.ComponentModel;

namespace Microsoft.Data;

[AttributeUsage(AttributeTargets.All)]
internal sealed class ResCategoryAttribute : CategoryAttribute
{
	public ResCategoryAttribute(string category)
		: base(category)
	{
	}

	protected override string GetLocalizedString(string value)
	{
		return StringsHelper.GetString(value);
	}
}
