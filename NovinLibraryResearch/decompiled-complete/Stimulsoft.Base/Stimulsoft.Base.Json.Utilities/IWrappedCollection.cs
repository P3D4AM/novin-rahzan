using System.Collections;

namespace Stimulsoft.Base.Json.Utilities;

internal interface IWrappedCollection : IList, ICollection, IEnumerable
{
	object UnderlyingCollection { get; }
}
