using System.Collections;

namespace Microsoft.IdentityModel.Json.Utilities;

internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
{
	object UnderlyingDictionary { get; }
}
