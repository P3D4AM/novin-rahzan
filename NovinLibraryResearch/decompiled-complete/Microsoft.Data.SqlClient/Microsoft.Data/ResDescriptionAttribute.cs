using System;
using System.ComponentModel;

namespace Microsoft.Data;

[AttributeUsage(AttributeTargets.All)]
internal sealed class ResDescriptionAttribute : DescriptionAttribute
{
	private bool replaced;

	public override string Description
	{
		get
		{
			if (!replaced)
			{
				replaced = true;
				base.DescriptionValue = StringsHelper.GetString(base.Description);
			}
			return base.Description;
		}
	}

	public ResDescriptionAttribute(string description)
		: base(description)
	{
	}
}
