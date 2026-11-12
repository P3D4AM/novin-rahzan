using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

public interface ICharacteristicCollection : IDictionary<string, HashSet<string>>, ICollection<KeyValuePair<string, HashSet<string>>>, IEnumerable<KeyValuePair<string, HashSet<string>>>, IEnumerable
{
	void Add(string key, string value);
}
