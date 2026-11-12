using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class CheckByTagSelector : CheckSelector
{
	public HashSet<string> Tags { get; }

	public CheckByTagSelector(string[] tags)
	{
		Tags = new HashSet<string>(tags);
	}

	public override bool IsMatch(Check check)
	{
		if (!Tags.Contains(check.Id))
		{
			if (check.Tags != null)
			{
				return check.Tags.Overlaps(Tags);
			}
			return false;
		}
		return true;
	}

	public override string ToString()
	{
		return "{" + string.Join(CultureInfo.InvariantCulture.TextInfo.ListSeparator + " ", Tags) + "}";
	}
}
