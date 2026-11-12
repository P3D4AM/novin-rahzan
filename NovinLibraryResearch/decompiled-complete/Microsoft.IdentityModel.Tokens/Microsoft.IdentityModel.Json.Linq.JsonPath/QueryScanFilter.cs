using System.Collections.Generic;

namespace Microsoft.IdentityModel.Json.Linq.JsonPath;

internal class QueryScanFilter : PathFilter
{
	public QueryExpression Expression { get; set; }

	public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
	{
		foreach (JToken item in current)
		{
			if (item is JContainer jContainer)
			{
				foreach (JToken item2 in jContainer.DescendantsAndSelf())
				{
					if (Expression.IsMatch(root, item2))
					{
						yield return item2;
					}
				}
			}
			else if (Expression.IsMatch(root, item))
			{
				yield return item;
			}
		}
	}
}
