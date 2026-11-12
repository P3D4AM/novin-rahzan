using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class CoreAnnotationNames
{
	public const string MaxLength = "MaxLength";

	public const string Unicode = "Unicode";

	public const string ProductVersion = "ProductVersion";

	public const string ValueGeneratorFactory = "ValueGeneratorFactory";

	public const string PropertyAccessMode = "PropertyAccessMode";

	public const string NavigationAccessMode = "NavigationAccessMode";

	public const string ChangeTrackingStrategy = "ChangeTrackingStrategy";

	public const string OwnedTypes = "OwnedTypes";

	public const string DiscriminatorProperty = "DiscriminatorProperty";

	public const string DiscriminatorValue = "DiscriminatorValue";

	public const string ConstructorBinding = "ConstructorBinding";

	public const string TypeMapping = "TypeMapping";

	public const string ValueConverter = "ValueConverter";

	public const string ValueComparer = "ValueComparer";

	public const string KeyValueComparer = "KeyValueComparer";

	public const string StructuralValueComparer = "StructuralValueComparer";

	public const string AfterSaveBehavior = "AfterSaveBehavior";

	public const string BeforeSaveBehavior = "BeforeSaveBehavior";

	public const string QueryFilter = "QueryFilter";

	public const string DefiningQuery = "DefiningQuery";

	public const string EagerLoaded = "EagerLoaded";

	public const string ProviderClrType = "ProviderClrType";

	public const string InverseNavigations = "InversePropertyAttributeConvention:InverseNavigations";

	public const string NavigationCandidates = "RelationshipDiscoveryConvention:NavigationCandidates";

	public const string AmbiguousNavigations = "RelationshipDiscoveryConvention:AmbiguousNavigations";

	public const string DuplicateServiceProperties = "ServicePropertyDiscoveryConvention:DuplicateServiceProperties";

	public const string AmbiguousField = "BackingFieldConvention:AmbiguousField";

	public static readonly ISet<string> AllNames = new HashSet<string>
	{
		"MaxLength", "Unicode", "ProductVersion", "ValueGeneratorFactory", "PropertyAccessMode", "NavigationAccessMode", "ChangeTrackingStrategy", "OwnedTypes", "DiscriminatorProperty", "DiscriminatorValue",
		"ConstructorBinding", "TypeMapping", "ValueConverter", "ValueComparer", "KeyValueComparer", "StructuralValueComparer", "AfterSaveBehavior", "BeforeSaveBehavior", "QueryFilter", "DefiningQuery",
		"EagerLoaded", "ProviderClrType", "InversePropertyAttributeConvention:InverseNavigations", "RelationshipDiscoveryConvention:NavigationCandidates", "RelationshipDiscoveryConvention:AmbiguousNavigations", "ServicePropertyDiscoveryConvention:DuplicateServiceProperties", "BackingFieldConvention:AmbiguousField"
	};
}
