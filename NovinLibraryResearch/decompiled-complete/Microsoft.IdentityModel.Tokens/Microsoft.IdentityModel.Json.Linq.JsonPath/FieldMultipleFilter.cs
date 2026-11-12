using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.IdentityModel.Json.Utilities;

namespace Microsoft.IdentityModel.Json.Linq.JsonPath;

internal class FieldMultipleFilter : PathFilter
{
	public List<string> Names { get; set; }

	public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
	{
		foreach (JToken item in current)
		{
			if (item is JObject o)
			{
				foreach (string name in Names)
				{
					JToken jToken = o[name];
					if (jToken != null)
					{
						yield return jToken;
					}
					if (errorWhenNoMatch)
					{
						throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, name));
					}
				}
			}
			else if (errorWhenNoMatch)
			{
				throw new JsonException("Properties {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", Names.Select((string n) => "'" + n + "'")), item.GetType().Name));
			}
		}
	}
}
