using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public abstract class BuiltInFunctionLookupBase : IBuiltInFunctionLookup
{
	private readonly IMetadataCollection<IBuiltInFunction> m_allBuiltIns;

	private readonly IMetadataCollection<IDatePart> m_dateParts;

	private readonly BuiltInFunctionCollection m_builtInFunctions;

	private readonly BuiltInFunctionCollection m_globalVariables;

	public IMetadataCollection<IBuiltInFunction> BuiltInFunctions => m_builtInFunctions;

	public IMetadataCollection<IBuiltInFunction> GlobalVariables => m_globalVariables;

	public IMetadataCollection<IBuiltInFunction> AllBuiltIns => m_allBuiltIns;

	public IMetadataCollection<IDatePart> DateParts => m_dateParts;

	protected BuiltInFunctionLookupBase()
		: this(BuiltInFunctionCollection.PopulatedInstance, DatePartCollection.PopulatedInstance)
	{
	}

	protected BuiltInFunctionLookupBase(IMetadataCollection<IBuiltInFunction> allBuiltIns, IMetadataCollection<IDatePart> dateParts)
	{
		if (allBuiltIns == null)
		{
			throw new ArgumentNullException("allBuiltIns");
		}
		if (dateParts == null)
		{
			throw new ArgumentNullException("dateParts");
		}
		m_allBuiltIns = allBuiltIns;
		m_dateParts = dateParts;
		m_builtInFunctions = new BuiltInFunctionCollection(allBuiltIns.Count);
		m_globalVariables = new BuiltInFunctionCollection(64);
		foreach (IBuiltInFunction allBuiltIn in allBuiltIns)
		{
			if (allBuiltIn.IsGlobalVariable)
			{
				m_globalVariables.Add(allBuiltIn);
			}
			else
			{
				m_builtInFunctions.Add(allBuiltIn);
			}
		}
	}

	public virtual IBuiltInFunction GetMultiTypeBuiltInFunction(string name, ISystemDataType dataType)
	{
		if (!(m_allBuiltIns[name] is BuiltInFunction builtInFunction))
		{
			return null;
		}
		return builtInFunction[dataType];
	}
}
