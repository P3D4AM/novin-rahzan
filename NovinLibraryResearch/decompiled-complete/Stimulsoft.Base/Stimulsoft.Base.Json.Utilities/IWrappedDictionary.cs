using System.Collections;

namespace Stimulsoft.Base.Json.Utilities;

internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
{
	object UnderlyingDictionary { get; }
}
