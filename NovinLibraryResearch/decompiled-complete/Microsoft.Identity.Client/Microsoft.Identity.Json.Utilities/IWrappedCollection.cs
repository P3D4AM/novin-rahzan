using System.Collections;

namespace Microsoft.Identity.Json.Utilities;

internal interface IWrappedCollection : IList, ICollection, IEnumerable
{
	object UnderlyingCollection { get; }
}
