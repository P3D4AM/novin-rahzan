using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

public interface IDataView : IReadOnlyList<IDataRow>, IReadOnlyCollection<IDataRow>, IEnumerable<IDataRow>, IEnumerable
{
}
