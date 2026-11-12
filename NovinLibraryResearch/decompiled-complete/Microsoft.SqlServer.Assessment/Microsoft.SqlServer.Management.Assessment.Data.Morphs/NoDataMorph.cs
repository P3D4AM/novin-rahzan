namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class NoDataMorph : IDataMorph
{
	public Measurement Define { get; } = new Measurement();


	public string PresenceFlag { get; set; }

	public bool BenefitsFromCache => false;

	public IDataView ApplyTo(IDataView source, bool useCache = true)
	{
		if (source != null && source.Count > 0)
		{
			if (string.IsNullOrWhiteSpace(PresenceFlag))
			{
				return source;
			}
			ExtValueRowMorph morph = new ExtValueRowMorph(PresenceFlag, true);
			return new RowMorphingView(morph, source);
		}
		if (!string.IsNullOrWhiteSpace(PresenceFlag) && !Define.ContainsKey(PresenceFlag))
		{
			Define.Add(PresenceFlag, false);
		}
		return new ConstantDataView(Define);
	}
}
