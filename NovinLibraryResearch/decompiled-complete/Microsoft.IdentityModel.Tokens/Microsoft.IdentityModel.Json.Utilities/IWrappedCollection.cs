using System.Collections;

namespace Microsoft.IdentityModel.Json.Utilities;

internal interface IWrappedCollection : IList, ICollection, IEnumerable
{
	object UnderlyingCollection { get; }
}
