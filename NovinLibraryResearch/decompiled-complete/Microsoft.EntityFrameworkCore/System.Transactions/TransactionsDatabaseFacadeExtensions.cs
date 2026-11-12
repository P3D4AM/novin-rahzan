using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace System.Transactions;

public static class TransactionsDatabaseFacadeExtensions
{
	public static void EnlistTransaction([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade, [JetBrains.Annotations.CanBeNull] Transaction transaction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade");
		if (((IDatabaseFacadeDependenciesAccessor)databaseFacade).Dependencies.TransactionManager is ITransactionEnlistmentManager transactionEnlistmentManager)
		{
			transactionEnlistmentManager.EnlistTransaction(transaction);
			return;
		}
		throw new NotSupportedException(CoreStrings.TransactionsNotSupported);
	}

	public static Transaction GetEnlistedTransaction([JetBrains.Annotations.NotNull] this DatabaseFacade databaseFacade)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseFacade, "databaseFacade");
		if (((IDatabaseFacadeDependenciesAccessor)databaseFacade).Dependencies.TransactionManager is ITransactionEnlistmentManager transactionEnlistmentManager)
		{
			return transactionEnlistmentManager.EnlistedTransaction;
		}
		throw new NotSupportedException(CoreStrings.TransactionsNotSupported);
	}
}
