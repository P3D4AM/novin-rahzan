using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class PropertyComparerFactory
{
	private readonly ChangeResultBuilder changeResultBuilder;

	private readonly UnnamedObjectsIdentityAssumptions equalityAssumptions;

	private readonly DifferencerIdGenerator metadataIdGenerator;

	private readonly DifferencerIdResolver sourceIdResolver;

	private readonly DifferencerIdResolver targetIdResolver;

	private readonly HashCodeGenerator hashCodeGenerator;

	private readonly PrimitiveTypeComparer primitiveTypeComparer;

	private readonly CollectionChildTypeComparer collectionChildTypeComparer;

	private readonly CollectionRegularTypeComparer collectionRegularTypeComparer;

	private readonly MetadataChildTypeComparer metadataChildTypeComparer;

	private readonly MetadataRegularTypeComparer metadataRegularTypeComparer;

	public DifferencerIdGenerator MetadataIdGenerator => metadataIdGenerator;

	public UnnamedObjectsIdentityAssumptions EqualityAssumptions => equalityAssumptions;

	public HashCodeGenerator HashCodeGenerator => hashCodeGenerator;

	public DifferencerIdResolver SourceIdResolver => sourceIdResolver;

	public DifferencerIdResolver TargetIdResolver => targetIdResolver;

	public PropertyComparerFactory(DifferencerIdGenerator metadataIdGenerator, IDifferencerFilter differenceFilter, DifferencerIdResolver sourceIdResolver, DifferencerIdResolver targetIdResolver)
	{
		changeResultBuilder = new ChangeResultBuilder();
		equalityAssumptions = new UnnamedObjectsIdentityAssumptions();
		this.metadataIdGenerator = metadataIdGenerator;
		hashCodeGenerator = new HashCodeGenerator(this.metadataIdGenerator, differenceFilter);
		this.sourceIdResolver = sourceIdResolver;
		this.targetIdResolver = targetIdResolver;
		primitiveTypeComparer = new PrimitiveTypeComparer(this, changeResultBuilder, differenceFilter);
		collectionChildTypeComparer = new CollectionChildTypeComparer(this, changeResultBuilder, differenceFilter);
		collectionRegularTypeComparer = new CollectionRegularTypeComparer(this, changeResultBuilder, differenceFilter);
		metadataChildTypeComparer = new MetadataChildTypeComparer(this, changeResultBuilder, differenceFilter);
		metadataRegularTypeComparer = new MetadataRegularTypeComparer(this, changeResultBuilder, differenceFilter);
	}

	public ChangeResult Process(IMetadataObject source, IMetadataObject target)
	{
		metadataChildTypeComparer.ProcessNodes(source, target, addToChangeResult: true);
		return changeResultBuilder.GetChangeResult();
	}

	public PropertyComparer Create(MetadataPropertyInfo propertyInfo)
	{
		TypeInfo returnType = propertyInfo.ReturnType;
		if (returnType.IsPrimitive)
		{
			return primitiveTypeComparer;
		}
		if (returnType.IsCollection)
		{
			switch (propertyInfo.PropertyType)
			{
			case PropertyType.Child:
				return collectionChildTypeComparer;
			case PropertyType.Regular:
				if (PropertyComparer.IsOrderedCollection(propertyInfo))
				{
					return collectionRegularTypeComparer;
				}
				return collectionChildTypeComparer;
			default:
				return null;
			}
		}
		if (returnType.IsMetadata)
		{
			return propertyInfo.PropertyType switch
			{
				PropertyType.Child => metadataChildTypeComparer, 
				PropertyType.Regular => metadataRegularTypeComparer, 
				_ => null, 
			};
		}
		return null;
	}
}
