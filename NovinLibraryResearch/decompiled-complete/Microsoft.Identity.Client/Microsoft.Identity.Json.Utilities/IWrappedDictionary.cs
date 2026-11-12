using System.Collections;

namespace Microsoft.Identity.Json.Utilities;

internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
{
	object UnderlyingDictionary { get; }
}
