using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ResolvedSynonymFactory : ObjectFactoryBase<ResolvedSynonymFactory>, IResolvedSynonymFactory
{
	public IResolvedTableSynonym CreateResolvedTableSynonym(ISynonym synonym, ITable table)
	{
		if (synonym == null)
		{
			throw new ArgumentNullException("synonym");
		}
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		return new ResolvedTableSynonym(synonym, table);
	}

	public IResolvedViewSynonym CreateResolvedViewSynonym(ISynonym synonym, IView view)
	{
		if (synonym == null)
		{
			throw new ArgumentNullException("synonym");
		}
		if (view == null)
		{
			throw new ArgumentNullException("view");
		}
		return new ResolvedViewSynonym(synonym, view);
	}

	public IResolvedExtendedStoredProcedureSynonym CreateResolvedExtendedStoredProcedureSynonym(ISynonym synonym, IExtendedStoredProcedure extendedStoredProcedure)
	{
		if (synonym == null)
		{
			throw new ArgumentNullException("synonym");
		}
		if (extendedStoredProcedure == null)
		{
			throw new ArgumentNullException("extendedStoredProcedure");
		}
		return new ResolvedExtendedStoredProcedureSynonym(synonym, extendedStoredProcedure);
	}

	public IResolvedScalarValuedFunctionSynonym CreateResolvedScalarValuedFunctionSynonym(ISynonym synonym, IScalarValuedFunction scalarValuedFunction)
	{
		if (synonym == null)
		{
			throw new ArgumentNullException("synonym");
		}
		if (scalarValuedFunction == null)
		{
			throw new ArgumentNullException("scalarValuedFunction");
		}
		return new ResolvedScalarValuedFunctionSynonym(synonym, scalarValuedFunction);
	}

	public IResolvedStoredProcedureSynonym CreateResolvedStoredProcedureSynonym(ISynonym synonym, IStoredProcedure storedProcedure)
	{
		if (synonym == null)
		{
			throw new ArgumentNullException("synonym");
		}
		if (storedProcedure == null)
		{
			throw new ArgumentNullException("storedProcedure");
		}
		return new ResolvedStoredProcedureSynonym(synonym, storedProcedure);
	}

	public IResolvedTableValuedFunctionSynonym CreateResolvedTableValuedFunctionSynonym(ISynonym synonym, ITableValuedFunction tableValuedFunction)
	{
		if (synonym == null)
		{
			throw new ArgumentNullException("synonym");
		}
		if (tableValuedFunction == null)
		{
			throw new ArgumentNullException("tableValuedFunction");
		}
		return new ResolvedTableValuedFunctionSynonym(synonym, tableValuedFunction);
	}

	public IResolvedUserDefinedAggregateSynonym CreateResolvedUserDefinedAggregateSynonym(ISynonym synonym, IUserDefinedAggregate userDefinedAggregate)
	{
		if (synonym == null)
		{
			throw new ArgumentNullException("synonym");
		}
		if (userDefinedAggregate == null)
		{
			throw new ArgumentNullException("userDefinedAggregate");
		}
		return new ResolvedUserDefinedAggregateSynonym(synonym, userDefinedAggregate);
	}
}
