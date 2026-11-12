using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class ResolvedSynonym<T> : SchemaOwnedObject, IResolvedSynonym, ISchemaOwnedObject, IDatabaseObject, IMetadataObject where T : ISchemaOwnedObject
{
	private readonly ISynonym synonym;

	private readonly T targetObject;

	public bool IsResolvedSynonym => true;

	public ISynonym Synonym => synonym;

	public T TargetObject => targetObject;

	ISchemaOwnedObject IResolvedSynonym.TargetObject => targetObject;

	protected ResolvedSynonym(ISynonym synonym, T targetObject)
		: base(synonym.Schema, synonym.Name)
	{
		this.synonym = synonym;
		this.targetObject = targetObject;
	}
}
