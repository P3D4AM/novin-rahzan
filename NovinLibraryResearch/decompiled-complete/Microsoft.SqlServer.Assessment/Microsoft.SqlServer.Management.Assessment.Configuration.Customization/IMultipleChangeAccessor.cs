using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

public interface IMultipleChangeAccessor<in TTarget, out TValue>
{
	IEnumerable<TValue> GetValues(TTarget target);
}
