namespace Microsoft.SqlServer.Management.Assessment;

public interface IDataMorph
{
	bool BenefitsFromCache { get; }

	IDataView ApplyTo(IDataView source, bool useCache = true);
}
