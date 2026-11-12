using System.Transactions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface ITransactionEnlistmentManager
{
	Transaction EnlistedTransaction { get; }

	void EnlistTransaction([JetBrains.Annotations.CanBeNull] Transaction transaction);
}
