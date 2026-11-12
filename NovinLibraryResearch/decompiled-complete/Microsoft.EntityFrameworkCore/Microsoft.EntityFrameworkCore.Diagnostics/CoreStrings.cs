using System.Reflection;
using System.Resources;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public static class CoreStrings
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Properties.CoreStrings", typeof(CoreStrings).GetTypeInfo().Assembly);

	public static string NoElements => GetString("NoElements");

	public static string IQueryableProviderNotAsync => GetString("IQueryableProviderNotAsync");

	public static string ResetNotSupported => GetString("ResetNotSupported");

	public static string NoProviderConfigured => GetString("NoProviderConfigured");

	public static string NoEfServices => GetString("NoEfServices");

	public static string CanConnectNotImplemented => GetString("CanConnectNotImplemented");

	public static string PropertyMethodInvoked => GetString("PropertyMethodInvoked");

	public static string RecursiveOnModelCreating => GetString("RecursiveOnModelCreating");

	public static string RecursiveOnConfiguring => GetString("RecursiveOnConfiguring");

	public static string RelationshipCannotBeInverted => GetString("RelationshipCannotBeInverted");

	public static string ExpressionParameterizationException => GetString("ExpressionParameterizationException");

	public static string HiLoBadBlockSize => GetString("HiLoBadBlockSize");

	public static string DataBindingWithIListSource => GetString("DataBindingWithIListSource");

	public static string DataBindingToLocalWithIListSource => GetString("DataBindingToLocalWithIListSource");

	public static string InvalidMemberInitBinding => GetString("InvalidMemberInitBinding");

	public static string ConcurrentMethodInvocation => GetString("ConcurrentMethodInvocation");

	public static string ContextDisposed => GetString("ContextDisposed");

	public static string ErrorMaterializingValue => GetString("ErrorMaterializingValue");

	public static string InvalidPoolSize => GetString("InvalidPoolSize");

	public static string PoolingOptionsModified => GetString("PoolingOptionsModified");

	public static string ConventionsInfiniteLoop => GetString("ConventionsInfiniteLoop");

	public static string TransactionsNotSupported => GetString("TransactionsNotSupported");

	public static string ErrorInvalidQueryable => GetString("ErrorInvalidQueryable");

	public static string NoInterceptionResult => GetString("NoInterceptionResult");

	public static string SetOperationWithDifferentIncludesInOperands => GetString("SetOperationWithDifferentIncludesInOperands");

	public static string CircularDependency([JetBrains.Annotations.CanBeNull] object cycle)
	{
		return string.Format(GetString("CircularDependency", "cycle"), cycle);
	}

	public static string TranslationFailed([JetBrains.Annotations.CanBeNull] object expression)
	{
		return string.Format(GetString("TranslationFailed", "expression"), expression);
	}

	public static string QueryFailed([JetBrains.Annotations.CanBeNull] object expression, [JetBrains.Annotations.CanBeNull] object visitor)
	{
		return string.Format(GetString("QueryFailed", "expression", "visitor"), expression, visitor);
	}

	public static string ModelNotFinalized([JetBrains.Annotations.CanBeNull] object method)
	{
		return string.Format(GetString("ModelNotFinalized", "method"), method);
	}

	public static string InvalidEnumValue([JetBrains.Annotations.CanBeNull] object argumentName, [JetBrains.Annotations.CanBeNull] object enumType)
	{
		return string.Format(GetString("InvalidEnumValue", "argumentName", "enumType"), argumentName, enumType);
	}

	public static string LiteralGenerationNotSupported([JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("LiteralGenerationNotSupported", "type"), type);
	}

	public static string InvalidPropertiesExpression([JetBrains.Annotations.CanBeNull] object expression)
	{
		return string.Format(GetString("InvalidPropertiesExpression", "expression"), expression);
	}

	public static string InvalidPropertyExpression([JetBrains.Annotations.CanBeNull] object expression)
	{
		return string.Format(GetString("InvalidPropertyExpression", "expression"), expression);
	}

	public static string IdentityConflict([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyProperties)
	{
		return string.Format(GetString("IdentityConflict", "entityType", "keyProperties"), entityType, keyProperties);
	}

	public static string IdentityConflictSensitive([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyValue)
	{
		return string.Format(GetString("IdentityConflictSensitive", "entityType", "keyValue"), entityType, keyValue);
	}

	public static string WrongStateManager([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("WrongStateManager", "entityType"), entityType);
	}

	public static string MultipleEntries([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("MultipleEntries", "entityType"), entityType);
	}

	public static string PropertyNotFound([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("PropertyNotFound", "property", "entityType"), property, entityType);
	}

	public static string PropertyIsNavigation([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object PropertyMethod, [JetBrains.Annotations.CanBeNull] object ReferenceMethod, [JetBrains.Annotations.CanBeNull] object CollectionMethod)
	{
		return string.Format(GetString("PropertyIsNavigation", "property", "entityType", "PropertyMethod", "ReferenceMethod", "CollectionMethod"), property, entityType, PropertyMethod, ReferenceMethod, CollectionMethod);
	}

	public static string NavigationIsProperty([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object ReferenceMethod, [JetBrains.Annotations.CanBeNull] object CollectionMethod, [JetBrains.Annotations.CanBeNull] object PropertyMethod)
	{
		return string.Format(GetString("NavigationIsProperty", "property", "entityType", "ReferenceMethod", "CollectionMethod", "PropertyMethod"), property, entityType, ReferenceMethod, CollectionMethod, PropertyMethod);
	}

	public static string ReferenceIsCollection([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object ReferenceMethod, [JetBrains.Annotations.CanBeNull] object CollectionMethod)
	{
		return string.Format(GetString("ReferenceIsCollection", "property", "entityType", "ReferenceMethod", "CollectionMethod"), property, entityType, ReferenceMethod, CollectionMethod);
	}

	public static string CollectionIsReference([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object CollectionMethod, [JetBrains.Annotations.CanBeNull] object ReferenceMethod)
	{
		return string.Format(GetString("CollectionIsReference", "property", "entityType", "CollectionMethod", "ReferenceMethod"), property, entityType, CollectionMethod, ReferenceMethod);
	}

	public static string ReferenceMustBeLoaded([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ReferenceMustBeLoaded", "navigation", "entityType"), navigation, entityType);
	}

	public static string CannotLoadDetached([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("CannotLoadDetached", "navigation", "entityType"), navigation, entityType);
	}

	public static string EntityRequiresKey([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("EntityRequiresKey", "entityType"), entityType);
	}

	public static string KeyPropertiesWrongEntity([JetBrains.Annotations.CanBeNull] object key, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("KeyPropertiesWrongEntity", "key", "entityType"), key, entityType);
	}

	public static string ForeignKeyPropertiesWrongEntity([JetBrains.Annotations.CanBeNull] object foreignKey, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ForeignKeyPropertiesWrongEntity", "foreignKey", "entityType"), foreignKey, entityType);
	}

	public static string IndexPropertiesWrongEntity([JetBrains.Annotations.CanBeNull] object index, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("IndexPropertiesWrongEntity", "index", "entityType"), index, entityType);
	}

	public static string IQueryableNotAsync([JetBrains.Annotations.CanBeNull] object genericParameter)
	{
		return string.Format(GetString("IQueryableNotAsync", "genericParameter"), genericParameter);
	}

	public static string ChangeTrackingInterfaceMissing([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object changeTrackingStrategy, [JetBrains.Annotations.CanBeNull] object notificationInterface)
	{
		return string.Format(GetString("ChangeTrackingInterfaceMissing", "entityType", "changeTrackingStrategy", "notificationInterface"), entityType, changeTrackingStrategy, notificationInterface);
	}

	public static string NonNotifyingCollection([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object changeTrackingStrategy)
	{
		return string.Format(GetString("NonNotifyingCollection", "navigation", "entityType", "changeTrackingStrategy"), navigation, entityType, changeTrackingStrategy);
	}

	public static string OriginalValueNotTracked([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("OriginalValueNotTracked", "property", "entityType"), property, entityType);
	}

	public static string ValueCannotBeNull([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("ValueCannotBeNull", "property", "entityType", "propertyType"), property, entityType, propertyType);
	}

	public static string InvalidType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object valueType, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("InvalidType", "property", "entityType", "valueType", "propertyType"), property, entityType, valueType, propertyType);
	}

	public static string PropertyDoesNotBelong([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object expectedType)
	{
		return string.Format(GetString("PropertyDoesNotBelong", "property", "entityType", "expectedType"), property, entityType, expectedType);
	}

	public static string MissingBackingField([JetBrains.Annotations.CanBeNull] object field, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("MissingBackingField", "field", "property", "entityType"), field, property, entityType);
	}

	public static string BadBackingFieldType([JetBrains.Annotations.CanBeNull] object field, [JetBrains.Annotations.CanBeNull] object fieldType, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("BadBackingFieldType", "field", "fieldType", "entityType", "property", "propertyType"), field, fieldType, entityType, property, propertyType);
	}

	public static string NoBackingField([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entity, [JetBrains.Annotations.CanBeNull] object pam)
	{
		return string.Format(GetString("NoBackingField", "property", "entity", "pam"), property, entity, pam);
	}

	public static string NoBackingFieldLazyLoading([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entity)
	{
		return string.Format(GetString("NoBackingFieldLazyLoading", "property", "entity"), property, entity);
	}

	public static string NoFieldOrSetter([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entity)
	{
		return string.Format(GetString("NoFieldOrSetter", "property", "entity"), property, entity);
	}

	public static string NoFieldOrGetter([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entity)
	{
		return string.Format(GetString("NoFieldOrGetter", "property", "entity"), property, entity);
	}

	public static string NoProperty([JetBrains.Annotations.CanBeNull] object field, [JetBrains.Annotations.CanBeNull] object entity, [JetBrains.Annotations.CanBeNull] object pam)
	{
		return string.Format(GetString("NoProperty", "field", "entity", "pam"), field, entity, pam);
	}

	public static string NoSetter([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entity, [JetBrains.Annotations.CanBeNull] object pam)
	{
		return string.Format(GetString("NoSetter", "property", "entity", "pam"), property, entity, pam);
	}

	public static string NoGetter([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entity, [JetBrains.Annotations.CanBeNull] object pam)
	{
		return string.Format(GetString("NoGetter", "property", "entity", "pam"), property, entity, pam);
	}

	public static string NoClrType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NoClrType", "entityType"), entityType);
	}

	public static string MultipleProvidersConfigured([JetBrains.Annotations.CanBeNull] object storeNames)
	{
		return string.Format(GetString("MultipleProvidersConfigured", "storeNames"), storeNames);
	}

	public static string DbContextMissingConstructor([JetBrains.Annotations.CanBeNull] object contextType)
	{
		return string.Format(GetString("DbContextMissingConstructor", "contextType"), contextType);
	}

	public static string InvalidReplaceService([JetBrains.Annotations.CanBeNull] object replaceService, [JetBrains.Annotations.CanBeNull] object useInternalServiceProvider)
	{
		return string.Format(GetString("InvalidReplaceService", "replaceService", "useInternalServiceProvider"), replaceService, useInternalServiceProvider);
	}

	public static string InvalidUseService([JetBrains.Annotations.CanBeNull] object useService, [JetBrains.Annotations.CanBeNull] object useInternalServiceProvider, [JetBrains.Annotations.CanBeNull] object service)
	{
		return string.Format(GetString("InvalidUseService", "useService", "useInternalServiceProvider", "service"), useService, useInternalServiceProvider, service);
	}

	public static string SingletonOptionChanged([JetBrains.Annotations.CanBeNull] object optionCall, [JetBrains.Annotations.CanBeNull] object useInternalServiceProvider)
	{
		return string.Format(GetString("SingletonOptionChanged", "optionCall", "useInternalServiceProvider"), optionCall, useInternalServiceProvider);
	}

	public static string ServiceProviderConfigChanged([JetBrains.Annotations.CanBeNull] object key)
	{
		return string.Format(GetString("ServiceProviderConfigChanged", "key"), key);
	}

	public static string ServiceProviderConfigAdded([JetBrains.Annotations.CanBeNull] object key)
	{
		return string.Format(GetString("ServiceProviderConfigAdded", "key"), key);
	}

	public static string ServiceProviderConfigRemoved([JetBrains.Annotations.CanBeNull] object key)
	{
		return string.Format(GetString("ServiceProviderConfigRemoved", "key"), key);
	}

	public static string NotAnEFService([JetBrains.Annotations.CanBeNull] object service)
	{
		return string.Format(GetString("NotAnEFService", "service"), service);
	}

	public static string ImplementationTypeRequired([JetBrains.Annotations.CanBeNull] object service)
	{
		return string.Format(GetString("ImplementationTypeRequired", "service"), service);
	}

	public static string SingletonRequired([JetBrains.Annotations.CanBeNull] object scope, [JetBrains.Annotations.CanBeNull] object service)
	{
		return string.Format(GetString("SingletonRequired", "scope", "service"), scope, service);
	}

	public static string NoValueGenerator([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("NoValueGenerator", "property", "entityType", "propertyType"), property, entityType, propertyType);
	}

	public static string BadDependencyRegistration([JetBrains.Annotations.CanBeNull] object dependenciesType)
	{
		return string.Format(GetString("BadDependencyRegistration", "dependenciesType"), dependenciesType);
	}

	public static string BadValueGeneratorType([JetBrains.Annotations.CanBeNull] object givenType, [JetBrains.Annotations.CanBeNull] object expectedType)
	{
		return string.Format(GetString("BadValueGeneratorType", "givenType", "expectedType"), givenType, expectedType);
	}

	public static string CannotCreateValueGenerator([JetBrains.Annotations.CanBeNull] object generatorType)
	{
		return string.Format(GetString("CannotCreateValueGenerator", "generatorType"), generatorType);
	}

	public static string TempValuePersists([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object state)
	{
		return string.Format(GetString("TempValuePersists", "property", "entityType", "state"), property, entityType, state);
	}

	public static string PropertyWrongClrType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object clrType, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("PropertyWrongClrType", "property", "entityType", "clrType", "propertyType"), property, entityType, clrType, propertyType);
	}

	public static string ClrPropertyOnShadowEntity([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ClrPropertyOnShadowEntity", "property", "entityType"), property, entityType);
	}

	public static string PropertyInUseKey([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object key)
	{
		return string.Format(GetString("PropertyInUseKey", "property", "entityType", "key"), property, entityType, key);
	}

	public static string KeyInUse([JetBrains.Annotations.CanBeNull] object key, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object dependentType)
	{
		return string.Format(GetString("KeyInUse", "key", "entityType", "dependentType"), key, entityType, dependentType);
	}

	public static string DuplicateServicePropertyType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object serviceType, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object duplicateName, [JetBrains.Annotations.CanBeNull] object duplicateEntityType)
	{
		return string.Format(GetString("DuplicateServicePropertyType", "property", "serviceType", "entityType", "duplicateName", "duplicateEntityType"), property, serviceType, entityType, duplicateName, duplicateEntityType);
	}

	public static string NoClrNavigation([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NoClrNavigation", "navigation", "entityType"), navigation, entityType);
	}

	public static string NavigationSingleWrongClrType([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object clrType, [JetBrains.Annotations.CanBeNull] object targetType)
	{
		return string.Format(GetString("NavigationSingleWrongClrType", "navigation", "entityType", "clrType", "targetType"), navigation, entityType, clrType, targetType);
	}

	public static string NavigationCollectionWrongClrType([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object clrType, [JetBrains.Annotations.CanBeNull] object targetType)
	{
		return string.Format(GetString("NavigationCollectionWrongClrType", "navigation", "entityType", "clrType", "targetType"), navigation, entityType, clrType, targetType);
	}

	public static string ForeignKeyCountMismatch([JetBrains.Annotations.CanBeNull] object foreignKey, [JetBrains.Annotations.CanBeNull] object dependentType, [JetBrains.Annotations.CanBeNull] object principalKey, [JetBrains.Annotations.CanBeNull] object principalType)
	{
		return string.Format(GetString("ForeignKeyCountMismatch", "foreignKey", "dependentType", "principalKey", "principalType"), foreignKey, dependentType, principalKey, principalType);
	}

	public static string ForeignKeyTypeMismatch([JetBrains.Annotations.CanBeNull] object foreignKey, [JetBrains.Annotations.CanBeNull] object dependentType, [JetBrains.Annotations.CanBeNull] object principalKey, [JetBrains.Annotations.CanBeNull] object principalType)
	{
		return string.Format(GetString("ForeignKeyTypeMismatch", "foreignKey", "dependentType", "principalKey", "principalType"), foreignKey, dependentType, principalKey, principalType);
	}

	public static string NavigationBadType([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object foundType, [JetBrains.Annotations.CanBeNull] object targetType)
	{
		return string.Format(GetString("NavigationBadType", "navigation", "entityType", "foundType", "targetType"), navigation, entityType, foundType, targetType);
	}

	public static string NavigationArray([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object foundType)
	{
		return string.Format(GetString("NavigationArray", "navigation", "entityType", "foundType"), navigation, entityType, foundType);
	}

	public static string NavigationNoSetter([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NavigationNoSetter", "navigation", "entityType"), navigation, entityType);
	}

	public static string NavigationCannotCreateType([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object foundType)
	{
		return string.Format(GetString("NavigationCannotCreateType", "navigation", "entityType", "foundType"), navigation, entityType, foundType);
	}

	public static string KeyReadOnly([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("KeyReadOnly", "property", "entityType"), property, entityType);
	}

	public static string PropertyReadOnlyAfterSave([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("PropertyReadOnlyAfterSave", "property", "entityType"), property, entityType);
	}

	public static string PropertyReadOnlyBeforeSave([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("PropertyReadOnlyBeforeSave", "property", "entityType"), property, entityType);
	}

	public static string KeyPropertyMustBeReadOnly([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("KeyPropertyMustBeReadOnly", "property", "entityType"), property, entityType);
	}

	public static string RelationshipConceptualNull([JetBrains.Annotations.CanBeNull] object firstType, [JetBrains.Annotations.CanBeNull] object secondType)
	{
		return string.Format(GetString("RelationshipConceptualNull", "firstType", "secondType"), firstType, secondType);
	}

	public static string PropertyConceptualNull([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("PropertyConceptualNull", "property", "entityType"), property, entityType);
	}

	public static string DuplicateForeignKey([JetBrains.Annotations.CanBeNull] object foreignKey, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object duplicateEntityType, [JetBrains.Annotations.CanBeNull] object key, [JetBrains.Annotations.CanBeNull] object principalType)
	{
		return string.Format(GetString("DuplicateForeignKey", "foreignKey", "entityType", "duplicateEntityType", "key", "principalType"), foreignKey, entityType, duplicateEntityType, key, principalType);
	}

	public static string DuplicateIndex([JetBrains.Annotations.CanBeNull] object index, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object duplicateEntityType)
	{
		return string.Format(GetString("DuplicateIndex", "index", "entityType", "duplicateEntityType"), index, entityType, duplicateEntityType);
	}

	public static string DuplicateKey([JetBrains.Annotations.CanBeNull] object key, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object duplicateEntityType)
	{
		return string.Format(GetString("DuplicateKey", "key", "entityType", "duplicateEntityType"), key, entityType, duplicateEntityType);
	}

	public static string NavigationToShadowEntity([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object targetType)
	{
		return string.Format(GetString("NavigationToShadowEntity", "navigation", "entityType", "targetType"), navigation, entityType, targetType);
	}

	public static string EntityTypeNotInRelationship([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object dependentType, [JetBrains.Annotations.CanBeNull] object principalType)
	{
		return string.Format(GetString("EntityTypeNotInRelationship", "entityType", "dependentType", "principalType"), entityType, dependentType, principalType);
	}

	public static string DuplicateEntityType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("DuplicateEntityType", "entityType"), entityType);
	}

	public static string DuplicateAnnotation([JetBrains.Annotations.CanBeNull] object annotation)
	{
		return string.Format(GetString("DuplicateAnnotation", "annotation"), annotation);
	}

	public static string AnnotationNotFound([JetBrains.Annotations.CanBeNull] object annotation)
	{
		return string.Format(GetString("AnnotationNotFound", "annotation"), annotation);
	}

	public static string IncludeBadNavigation([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("IncludeBadNavigation", "property", "entityType"), property, entityType);
	}

	public static string CannotBeNullable([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("CannotBeNullable", "property", "entityType", "propertyType"), property, entityType, propertyType);
	}

	public static string KeyPropertyCannotBeNullable([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object key)
	{
		return string.Format(GetString("KeyPropertyCannotBeNullable", "property", "entityType", "key"), property, entityType, key);
	}

	public static string EntityTypeInUseByReferencingForeignKey([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object foreignKey, [JetBrains.Annotations.CanBeNull] object referencingEntityType)
	{
		return string.Format(GetString("EntityTypeInUseByReferencingForeignKey", "entityType", "foreignKey", "referencingEntityType"), entityType, foreignKey, referencingEntityType);
	}

	public static string ArgumentPropertyNull([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object argument)
	{
		return string.Format(GetString("ArgumentPropertyNull", "property", "argument"), property, argument);
	}

	public static string InvalidEntityType([JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("InvalidEntityType", "type"), type);
	}

	public static string ReferencedShadowKey([JetBrains.Annotations.CanBeNull] object referencingEntityTypeOrNavigation, [JetBrains.Annotations.CanBeNull] object referencedEntityTypeOrNavigation, [JetBrains.Annotations.CanBeNull] object foreignKeyPropertiesWithTypes, [JetBrains.Annotations.CanBeNull] object primaryKeyPropertiesWithTypes)
	{
		return string.Format(GetString("ReferencedShadowKey", "referencingEntityTypeOrNavigation", "referencedEntityTypeOrNavigation", "foreignKeyPropertiesWithTypes", "primaryKeyPropertiesWithTypes"), referencingEntityTypeOrNavigation, referencedEntityTypeOrNavigation, foreignKeyPropertiesWithTypes, primaryKeyPropertiesWithTypes);
	}

	public static string MutableKeyProperty([JetBrains.Annotations.CanBeNull] object keyProperty)
	{
		return string.Format(GetString("MutableKeyProperty", "keyProperty"), keyProperty);
	}

	public static string InvalidValueGeneratorFactoryProperty([JetBrains.Annotations.CanBeNull] object factory, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("InvalidValueGeneratorFactoryProperty", "factory", "property", "entityType"), factory, property, entityType);
	}

	public static string DerivedEntityTypeKey([JetBrains.Annotations.CanBeNull] object derivedType, [JetBrains.Annotations.CanBeNull] object rootType)
	{
		return string.Format(GetString("DerivedEntityTypeKey", "derivedType", "rootType"), derivedType, rootType);
	}

	public static string CircularInheritance([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseEntityType)
	{
		return string.Format(GetString("CircularInheritance", "entityType", "baseEntityType"), entityType, baseEntityType);
	}

	public static string DerivedEntityCannotHaveKeys([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("DerivedEntityCannotHaveKeys", "entityType"), entityType);
	}

	public static string GraphDoesNotContainVertex([JetBrains.Annotations.CanBeNull] object vertex)
	{
		return string.Format(GetString("GraphDoesNotContainVertex", "vertex"), vertex);
	}

	public static string CannotMaterializeAbstractType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("CannotMaterializeAbstractType", "entityType"), entityType);
	}

	public static string FindNotCompositeKey([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object valuesCount)
	{
		return string.Format(GetString("FindNotCompositeKey", "entityType", "valuesCount"), entityType, valuesCount);
	}

	public static string FindValueCountMismatch([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertiesCount, [JetBrains.Annotations.CanBeNull] object valuesCount)
	{
		return string.Format(GetString("FindValueCountMismatch", "entityType", "propertiesCount", "valuesCount"), entityType, propertiesCount, valuesCount);
	}

	public static string FindValueTypeMismatch([JetBrains.Annotations.CanBeNull] object index, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object valueType, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("FindValueTypeMismatch", "index", "entityType", "valueType", "propertyType"), index, entityType, valueType, propertyType);
	}

	public static string ForeignKeyReferencedEntityKeyMismatch([JetBrains.Annotations.CanBeNull] object principalKey, [JetBrains.Annotations.CanBeNull] object principalEntityType)
	{
		return string.Format(GetString("ForeignKeyReferencedEntityKeyMismatch", "principalKey", "principalEntityType"), principalKey, principalEntityType);
	}

	public static string WrongGenericPropertyType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object actualType, [JetBrains.Annotations.CanBeNull] object genericType)
	{
		return string.Format(GetString("WrongGenericPropertyType", "property", "entityType", "actualType", "genericType"), property, entityType, actualType, genericType);
	}

	public static string NonGenericOptions([JetBrains.Annotations.CanBeNull] object contextType)
	{
		return string.Format(GetString("NonGenericOptions", "contextType"), contextType);
	}

	public static string OptionsExtensionNotFound([JetBrains.Annotations.CanBeNull] object optionsExtension)
	{
		return string.Format(GetString("OptionsExtensionNotFound", "optionsExtension"), optionsExtension);
	}

	public static string DuplicatePropertiesOnBase([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseType, [JetBrains.Annotations.CanBeNull] object derivedPropertyType, [JetBrains.Annotations.CanBeNull] object derivedProperty, [JetBrains.Annotations.CanBeNull] object basePropertyType, [JetBrains.Annotations.CanBeNull] object baseProperty)
	{
		return string.Format(GetString("DuplicatePropertiesOnBase", "entityType", "baseType", "derivedPropertyType", "derivedProperty", "basePropertyType", "baseProperty"), entityType, baseType, derivedPropertyType, derivedProperty, basePropertyType, baseProperty);
	}

	public static string CannotBeNullablePK([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("CannotBeNullablePK", "property", "entityType"), property, entityType);
	}

	public static string ShadowEntity([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ShadowEntity", "entityType"), entityType);
	}

	public static string CompositePKWithDataAnnotation([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("CompositePKWithDataAnnotation", "entityType"), entityType);
	}

	public static string DuplicateNavigationsOnBase([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseType, [JetBrains.Annotations.CanBeNull] object navigations)
	{
		return string.Format(GetString("DuplicateNavigationsOnBase", "entityType", "baseType", "navigations"), entityType, baseType, navigations);
	}

	public static string EntityTypeModelMismatch([JetBrains.Annotations.CanBeNull] object firstEntityType, [JetBrains.Annotations.CanBeNull] object secondEntityType)
	{
		return string.Format(GetString("EntityTypeModelMismatch", "firstEntityType", "secondEntityType"), firstEntityType, secondEntityType);
	}

	public static string ValueGenWithConversion([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object converter)
	{
		return string.Format(GetString("ValueGenWithConversion", "entityType", "property", "converter"), entityType, property, converter);
	}

	public static string IntraHierarchicalAmbiguousTargetEntityType([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object foreignKey, [JetBrains.Annotations.CanBeNull] object principalEntityType, [JetBrains.Annotations.CanBeNull] object dependentEntityType)
	{
		return string.Format(GetString("IntraHierarchicalAmbiguousTargetEntityType", "entityType", "foreignKey", "principalEntityType", "dependentEntityType"), entityType, foreignKey, principalEntityType, dependentEntityType);
	}

	public static string NonClrBaseType([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseEntityType)
	{
		return string.Format(GetString("NonClrBaseType", "entityType", "baseEntityType"), entityType, baseEntityType);
	}

	public static string NonShadowBaseType([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseEntityType)
	{
		return string.Format(GetString("NonShadowBaseType", "entityType", "baseEntityType"), entityType, baseEntityType);
	}

	public static string NotAssignableClrBaseType([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseEntityType, [JetBrains.Annotations.CanBeNull] object clrType, [JetBrains.Annotations.CanBeNull] object baseClrType)
	{
		return string.Format(GetString("NotAssignableClrBaseType", "entityType", "baseEntityType", "clrType", "baseClrType"), entityType, baseEntityType, clrType, baseClrType);
	}

	public static string PropertyWrongEntityClrType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object clrType)
	{
		return string.Format(GetString("PropertyWrongEntityClrType", "property", "entityType", "clrType"), property, entityType, clrType);
	}

	public static string InvalidNavigationWithInverseProperty([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object referencedProperty, [JetBrains.Annotations.CanBeNull] object referencedEntityType)
	{
		return string.Format(GetString("InvalidNavigationWithInverseProperty", "property", "entityType", "referencedProperty", "referencedEntityType"), property, entityType, referencedProperty, referencedEntityType);
	}

	public static string SelfReferencingNavigationWithInverseProperty([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object referencedProperty, [JetBrains.Annotations.CanBeNull] object referencedEntityType)
	{
		return string.Format(GetString("SelfReferencingNavigationWithInverseProperty", "property", "entityType", "referencedProperty", "referencedEntityType"), property, entityType, referencedProperty, referencedEntityType);
	}

	public static string KeyAttributeOnDerivedEntity([JetBrains.Annotations.CanBeNull] object derivedType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("KeyAttributeOnDerivedEntity", "derivedType", "property"), derivedType, property);
	}

	public static string InversePropertyMismatch([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object referencedNavigation, [JetBrains.Annotations.CanBeNull] object referencedEntityType)
	{
		return string.Format(GetString("InversePropertyMismatch", "navigation", "entityType", "referencedNavigation", "referencedEntityType"), navigation, entityType, referencedNavigation, referencedEntityType);
	}

	public static string CompositeFkOnProperty([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("CompositeFkOnProperty", "navigation", "entityType"), navigation, entityType);
	}

	public static string FkAttributeOnPropertyNavigationMismatch([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("FkAttributeOnPropertyNavigationMismatch", "property", "navigation", "entityType"), property, navigation, entityType);
	}

	public static string InvalidPropertyListOnNavigation([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("InvalidPropertyListOnNavigation", "navigation", "entityType"), navigation, entityType);
	}

	public static string InvalidRelationshipUsingDataAnnotations([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object referencedNavigation, [JetBrains.Annotations.CanBeNull] object referencedEntityType)
	{
		return string.Format(GetString("InvalidRelationshipUsingDataAnnotations", "navigation", "entityType", "referencedNavigation", "referencedEntityType"), navigation, entityType, referencedNavigation, referencedEntityType);
	}

	public static string ConflictingPropertyOrNavigation([JetBrains.Annotations.CanBeNull] object member, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object conflictingEntityType)
	{
		return string.Format(GetString("ConflictingPropertyOrNavigation", "member", "entityType", "conflictingEntityType"), member, entityType, conflictingEntityType);
	}

	public static string EntityTypeNotInRelationshipStrict([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object dependentType, [JetBrains.Annotations.CanBeNull] object principalType)
	{
		return string.Format(GetString("EntityTypeNotInRelationshipStrict", "entityType", "dependentType", "principalType"), entityType, dependentType, principalType);
	}

	public static string EntityTypeInUseByDerived([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object derivedEntityType)
	{
		return string.Format(GetString("EntityTypeInUseByDerived", "entityType", "derivedEntityType"), entityType, derivedEntityType);
	}

	public static string NavigationNotAdded([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("NavigationNotAdded", "entityType", "navigation", "propertyType"), entityType, navigation, propertyType);
	}

	public static string PropertyNotAdded([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("PropertyNotAdded", "entityType", "property", "propertyType"), entityType, property, propertyType);
	}

	public static string PropertyNotMapped([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("PropertyNotMapped", "entityType", "property", "propertyType"), entityType, property, propertyType);
	}

	public static string InterfacePropertyNotAdded([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("InterfacePropertyNotAdded", "entityType", "navigation", "propertyType"), entityType, navigation, propertyType);
	}

	public static string NavigationForWrongForeignKey([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object targetFk, [JetBrains.Annotations.CanBeNull] object actualFk)
	{
		return string.Format(GetString("NavigationForWrongForeignKey", "navigation", "entityType", "targetFk", "actualFk"), navigation, entityType, targetFk, actualFk);
	}

	public static string EntityTypeNotFound([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("EntityTypeNotFound", "entityType"), entityType);
	}

	public static string CustomMetadata([JetBrains.Annotations.CanBeNull] object method, [JetBrains.Annotations.CanBeNull] object interfaceType, [JetBrains.Annotations.CanBeNull] object concreteType)
	{
		return string.Format(GetString("CustomMetadata", "method", "interfaceType", "concreteType"), method, interfaceType, concreteType);
	}

	public static string InvalidKeyValue([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyProperty)
	{
		return string.Format(GetString("InvalidKeyValue", "entityType", "keyProperty"), entityType, keyProperty);
	}

	public static string InvalidAlternateKeyValue([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyProperty)
	{
		return string.Format(GetString("InvalidAlternateKeyValue", "entityType", "keyProperty"), entityType, keyProperty);
	}

	public static string ExpressionParameterizationExceptionSensitive([JetBrains.Annotations.CanBeNull] object expression)
	{
		return string.Format(GetString("ExpressionParameterizationExceptionSensitive", "expression"), expression);
	}

	public static string MultipleNavigationsSameFk([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertyList)
	{
		return string.Format(GetString("MultipleNavigationsSameFk", "entityType", "propertyList"), entityType, propertyList);
	}

	public static string InconsistentInheritance([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseEntityType)
	{
		return string.Format(GetString("InconsistentInheritance", "entityType", "baseEntityType"), entityType, baseEntityType);
	}

	public static string DependentEntityTypeNotInRelationship([JetBrains.Annotations.CanBeNull] object dependentEntityType, [JetBrains.Annotations.CanBeNull] object principalEntityType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("DependentEntityTypeNotInRelationship", "dependentEntityType", "principalEntityType", "entityType"), dependentEntityType, principalEntityType, entityType);
	}

	public static string PrincipalEntityTypeNotInRelationship([JetBrains.Annotations.CanBeNull] object dependentEntityType, [JetBrains.Annotations.CanBeNull] object principalEntityType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("PrincipalEntityTypeNotInRelationship", "dependentEntityType", "principalEntityType", "entityType"), dependentEntityType, principalEntityType, entityType);
	}

	public static string ForeignKeyPropertyInKey([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object key, [JetBrains.Annotations.CanBeNull] object baseEntityType)
	{
		return string.Format(GetString("ForeignKeyPropertyInKey", "property", "entityType", "key", "baseEntityType"), property, entityType, key, baseEntityType);
	}

	public static string KeyPropertyInForeignKey([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("KeyPropertyInForeignKey", "property", "entityType"), property, entityType);
	}

	public static string NullableKey([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("NullableKey", "entityType", "property"), entityType, property);
	}

	public static string EntityTypesNotInRelationship([JetBrains.Annotations.CanBeNull] object invalidDependentType, [JetBrains.Annotations.CanBeNull] object invalidPrincipalType, [JetBrains.Annotations.CanBeNull] object dependentType, [JetBrains.Annotations.CanBeNull] object principalType)
	{
		return string.Format(GetString("EntityTypesNotInRelationship", "invalidDependentType", "invalidPrincipalType", "dependentType", "principalType"), invalidDependentType, invalidPrincipalType, dependentType, principalType);
	}

	public static string InvalidSetType([JetBrains.Annotations.CanBeNull] object typeName)
	{
		return string.Format(GetString("InvalidSetType", "typeName"), typeName);
	}

	public static string AmbiguousOneToOneRelationship([JetBrains.Annotations.CanBeNull] object dependentToPrincipalNavigationSpecification, [JetBrains.Annotations.CanBeNull] object principalToDependentNavigationSpecification)
	{
		return string.Format(GetString("AmbiguousOneToOneRelationship", "dependentToPrincipalNavigationSpecification", "principalToDependentNavigationSpecification"), dependentToPrincipalNavigationSpecification, principalToDependentNavigationSpecification);
	}

	public static string AmbiguousForeignKeyPropertyCandidates([JetBrains.Annotations.CanBeNull] object firstDependentToPrincipalNavigationSpecification, [JetBrains.Annotations.CanBeNull] object firstPrincipalToDependentNavigationSpecification, [JetBrains.Annotations.CanBeNull] object secondDependentToPrincipalNavigationSpecification, [JetBrains.Annotations.CanBeNull] object secondPrincipalToDependentNavigationSpecification, [JetBrains.Annotations.CanBeNull] object foreignKeyProperties)
	{
		return string.Format(GetString("AmbiguousForeignKeyPropertyCandidates", "firstDependentToPrincipalNavigationSpecification", "firstPrincipalToDependentNavigationSpecification", "secondDependentToPrincipalNavigationSpecification", "secondPrincipalToDependentNavigationSpecification", "foreignKeyProperties"), firstDependentToPrincipalNavigationSpecification, firstPrincipalToDependentNavigationSpecification, secondDependentToPrincipalNavigationSpecification, secondPrincipalToDependentNavigationSpecification, foreignKeyProperties);
	}

	public static string InvalidIncludeLambdaExpression([JetBrains.Annotations.CanBeNull] object methodName, [JetBrains.Annotations.CanBeNull] object includeLambdaExpression)
	{
		return string.Format(GetString("InvalidIncludeLambdaExpression", "methodName", "includeLambdaExpression"), methodName, includeLambdaExpression);
	}

	public static string AbstractLeafEntityType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("AbstractLeafEntityType", "entityType"), entityType);
	}

	public static string NoPropertyType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NoPropertyType", "property", "entityType"), property, entityType);
	}

	public static string TempValue([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("TempValue", "property", "entityType"), property, entityType);
	}

	public static string StoreGenValue([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("StoreGenValue", "property", "entityType"), property, entityType);
	}

	public static string NoParameterlessConstructor([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NoParameterlessConstructor", "entityType"), entityType);
	}

	public static string ConflictingRelationshipNavigation([JetBrains.Annotations.CanBeNull] object newPrincipalEntityType, [JetBrains.Annotations.CanBeNull] object newPrincipalNavigation, [JetBrains.Annotations.CanBeNull] object newDependentEntityType, [JetBrains.Annotations.CanBeNull] object newDependentNavigation, [JetBrains.Annotations.CanBeNull] object existingPrincipalEntityType, [JetBrains.Annotations.CanBeNull] object existingPrincipalNavigation, [JetBrains.Annotations.CanBeNull] object existingDependentEntityType, [JetBrains.Annotations.CanBeNull] object existingDependentNavigation)
	{
		return string.Format(GetString("ConflictingRelationshipNavigation", "newPrincipalEntityType", "newPrincipalNavigation", "newDependentEntityType", "newDependentNavigation", "existingPrincipalEntityType", "existingPrincipalNavigation", "existingDependentEntityType", "existingDependentNavigation"), newPrincipalEntityType, newPrincipalNavigation, newDependentEntityType, newDependentNavigation, existingPrincipalEntityType, existingPrincipalNavigation, existingDependentEntityType, existingDependentNavigation);
	}

	public static string WarningAsErrorTemplate([JetBrains.Annotations.CanBeNull] object eventName, [JetBrains.Annotations.CanBeNull] object message, [JetBrains.Annotations.CanBeNull] object eventId)
	{
		return string.Format(GetString("WarningAsErrorTemplate", "eventName", "message", "eventId"), eventName, message, eventId);
	}

	public static string NoProviderConfiguredFailedToResolveService([JetBrains.Annotations.CanBeNull] object service)
	{
		return string.Format(GetString("NoProviderConfiguredFailedToResolveService", "service"), service);
	}

	public static string ErrorMaterializingProperty([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("ErrorMaterializingProperty", "entityType", "property"), entityType, property);
	}

	public static string ErrorMaterializingPropertyInvalidCast([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object expectedType, [JetBrains.Annotations.CanBeNull] object actualType)
	{
		return string.Format(GetString("ErrorMaterializingPropertyInvalidCast", "entityType", "property", "expectedType", "actualType"), entityType, property, expectedType, actualType);
	}

	public static string ErrorMaterializingPropertyNullReference([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object expectedType)
	{
		return string.Format(GetString("ErrorMaterializingPropertyNullReference", "entityType", "property", "expectedType"), entityType, property, expectedType);
	}

	public static string ErrorMaterializingValueInvalidCast([JetBrains.Annotations.CanBeNull] object expectedType, [JetBrains.Annotations.CanBeNull] object actualType)
	{
		return string.Format(GetString("ErrorMaterializingValueInvalidCast", "expectedType", "actualType"), expectedType, actualType);
	}

	public static string ErrorMaterializingValueNullReference([JetBrains.Annotations.CanBeNull] object expectedType)
	{
		return string.Format(GetString("ErrorMaterializingValueNullReference", "expectedType"), expectedType);
	}

	public static string InheritedPropertyCannotBeIgnored([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseEntityType)
	{
		return string.Format(GetString("InheritedPropertyCannotBeIgnored", "property", "entityType", "baseEntityType"), property, entityType, baseEntityType);
	}

	public static string RetryLimitExceeded([JetBrains.Annotations.CanBeNull] object retryLimit, [JetBrains.Annotations.CanBeNull] object strategy)
	{
		return string.Format(GetString("RetryLimitExceeded", "retryLimit", "strategy"), retryLimit, strategy);
	}

	public static string ExecutionStrategyExistingTransaction([JetBrains.Annotations.CanBeNull] object strategy, [JetBrains.Annotations.CanBeNull] object getExecutionStrategyMethod)
	{
		return string.Format(GetString("ExecutionStrategyExistingTransaction", "strategy", "getExecutionStrategyMethod"), strategy, getExecutionStrategyMethod);
	}

	public static string PropertyCalledOnNavigation([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("PropertyCalledOnNavigation", "property", "entityType"), property, entityType);
	}

	public static string PropertyInUseForeignKey([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object foreignKey, [JetBrains.Annotations.CanBeNull] object foreignKeyType)
	{
		return string.Format(GetString("PropertyInUseForeignKey", "property", "entityType", "foreignKey", "foreignKeyType"), property, entityType, foreignKey, foreignKeyType);
	}

	public static string PropertyInUseIndex([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object index, [JetBrains.Annotations.CanBeNull] object indexType)
	{
		return string.Format(GetString("PropertyInUseIndex", "property", "entityType", "index", "indexType"), property, entityType, index, indexType);
	}

	public static string PoolingContextCtorError([JetBrains.Annotations.CanBeNull] object contextType)
	{
		return string.Format(GetString("PoolingContextCtorError", "contextType"), contextType);
	}

	public static string ForeignKeySelfReferencingDependentEntityType([JetBrains.Annotations.CanBeNull] object dependentType)
	{
		return string.Format(GetString("ForeignKeySelfReferencingDependentEntityType", "dependentType"), dependentType);
	}

	public static string EntityTypeInUseByForeignKey([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object referencedEntityType, [JetBrains.Annotations.CanBeNull] object foreignKey)
	{
		return string.Format(GetString("EntityTypeInUseByForeignKey", "entityType", "referencedEntityType", "foreignKey"), entityType, referencedEntityType, foreignKey);
	}

	public static string ClashingWeakEntityType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ClashingWeakEntityType", "entityType"), entityType);
	}

	public static string ClashingNonWeakEntityType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ClashingNonWeakEntityType", "entityType"), entityType);
	}

	public static string WeakBaseType([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseType)
	{
		return string.Format(GetString("WeakBaseType", "entityType", "baseType"), entityType, baseType);
	}

	public static string WeakDerivedType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("WeakDerivedType", "entityType"), entityType);
	}

	public static string DuplicatePropertyInList([JetBrains.Annotations.CanBeNull] object propertyList, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("DuplicatePropertyInList", "propertyList", "property"), propertyList, property);
	}

	public static string NoDefiningNavigation([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object definingEntityType)
	{
		return string.Format(GetString("NoDefiningNavigation", "navigation", "entityType", "definingEntityType"), navigation, entityType, definingEntityType);
	}

	public static string MultipleOwnerships([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("MultipleOwnerships", "entityType"), entityType);
	}

	public static string NonDefiningOwnership([JetBrains.Annotations.CanBeNull] object ownershipNavigation, [JetBrains.Annotations.CanBeNull] object definingNavigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NonDefiningOwnership", "ownershipNavigation", "definingNavigation", "entityType"), ownershipNavigation, definingNavigation, entityType);
	}

	public static string InconsistentOwnership([JetBrains.Annotations.CanBeNull] object ownedEntityType, [JetBrains.Annotations.CanBeNull] object nonOwnedEntityType)
	{
		return string.Format(GetString("InconsistentOwnership", "ownedEntityType", "nonOwnedEntityType"), ownedEntityType, nonOwnedEntityType);
	}

	public static string InverseToOwnedType([JetBrains.Annotations.CanBeNull] object principalEntityType, [JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object ownedType, [JetBrains.Annotations.CanBeNull] object ownerType)
	{
		return string.Format(GetString("InverseToOwnedType", "principalEntityType", "navigation", "ownedType", "ownerType"), principalEntityType, navigation, ownedType, ownerType);
	}

	public static string PrincipalOwnedType([JetBrains.Annotations.CanBeNull] object referencingEntityTypeOrNavigation, [JetBrains.Annotations.CanBeNull] object referencedEntityTypeOrNavigation, [JetBrains.Annotations.CanBeNull] object ownedType)
	{
		return string.Format(GetString("PrincipalOwnedType", "referencingEntityTypeOrNavigation", "referencedEntityTypeOrNavigation", "ownedType"), referencingEntityTypeOrNavigation, referencedEntityTypeOrNavigation, ownedType);
	}

	public static string AmbiguousDependentEntity([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object targetEntryCall)
	{
		return string.Format(GetString("AmbiguousDependentEntity", "entityType", "targetEntryCall"), entityType, targetEntryCall);
	}

	public static string UntrackedDependentEntity([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object referenceCall, [JetBrains.Annotations.CanBeNull] object collectionCall)
	{
		return string.Format(GetString("UntrackedDependentEntity", "entityType", "referenceCall", "collectionCall"), entityType, referenceCall, collectionCall);
	}

	public static string BadFilterExpression([JetBrains.Annotations.CanBeNull] object filter, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object clrType)
	{
		return string.Format(GetString("BadFilterExpression", "filter", "entityType", "clrType"), filter, entityType, clrType);
	}

	public static string BadFilterDerivedType([JetBrains.Annotations.CanBeNull] object filter, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("BadFilterDerivedType", "filter", "entityType"), filter, entityType);
	}

	public static string DefiningQueryWithKey([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("DefiningQueryWithKey", "entityType"), entityType);
	}

	public static string ConverterPropertyMismatch([JetBrains.Annotations.CanBeNull] object converterType, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertyName, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("ConverterPropertyMismatch", "converterType", "entityType", "propertyName", "propertyType"), converterType, entityType, propertyName, propertyType);
	}

	public static string ComparerPropertyMismatch([JetBrains.Annotations.CanBeNull] object type, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object propertyName, [JetBrains.Annotations.CanBeNull] object propertyType)
	{
		return string.Format(GetString("ComparerPropertyMismatch", "type", "entityType", "propertyName", "propertyType"), type, entityType, propertyName, propertyType);
	}

	public static string IncludeNotSpecifiedDirectlyOnEntityType([JetBrains.Annotations.CanBeNull] object include, [JetBrains.Annotations.CanBeNull] object invalidNavigation)
	{
		return string.Format(GetString("IncludeNotSpecifiedDirectlyOnEntityType", "include", "invalidNavigation"), include, invalidNavigation);
	}

	public static string IdentityConflictOwned([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyProperties)
	{
		return string.Format(GetString("IdentityConflictOwned", "entityType", "keyProperties"), entityType, keyProperties);
	}

	public static string IdentityConflictOwnedSensitive([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyValue)
	{
		return string.Format(GetString("IdentityConflictOwnedSensitive", "entityType", "keyValue"), entityType, keyValue);
	}

	public static string ConvertersCannotBeComposed([JetBrains.Annotations.CanBeNull] object typeOneIn, [JetBrains.Annotations.CanBeNull] object typeOneOut, [JetBrains.Annotations.CanBeNull] object typeTwoIn, [JetBrains.Annotations.CanBeNull] object typeTwoOut)
	{
		return string.Format(GetString("ConvertersCannotBeComposed", "typeOneIn", "typeOneOut", "typeTwoIn", "typeTwoOut"), typeOneIn, typeOneOut, typeTwoIn, typeTwoOut);
	}

	public static string ConverterBadType([JetBrains.Annotations.CanBeNull] object converter, [JetBrains.Annotations.CanBeNull] object type, [JetBrains.Annotations.CanBeNull] object allowed)
	{
		return string.Format(GetString("ConverterBadType", "converter", "type", "allowed"), converter, type, allowed);
	}

	public static string SeedDatumDuplicate([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyProperties)
	{
		return string.Format(GetString("SeedDatumDuplicate", "entityType", "keyProperties"), entityType, keyProperties);
	}

	public static string SeedDatumDuplicateSensitive([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyValue)
	{
		return string.Format(GetString("SeedDatumDuplicateSensitive", "entityType", "keyValue"), entityType, keyValue);
	}

	public static string SeedDatumIncompatibleValue([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("SeedDatumIncompatibleValue", "entityType", "property", "type"), entityType, property, type);
	}

	public static string SeedDatumIncompatibleValueSensitive([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object value, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("SeedDatumIncompatibleValueSensitive", "entityType", "value", "property", "type"), entityType, value, property, type);
	}

	public static string SeedDatumMissingValue([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("SeedDatumMissingValue", "entityType", "property"), entityType, property);
	}

	public static string SeedDatumDefaultValue([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object defaultValue)
	{
		return string.Format(GetString("SeedDatumDefaultValue", "entityType", "property", "defaultValue"), entityType, property, defaultValue);
	}

	public static string SeedDatumSignedNumericValue([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("SeedDatumSignedNumericValue", "entityType", "property"), entityType, property);
	}

	public static string SeedDatumNavigation([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object relatedEntityType, [JetBrains.Annotations.CanBeNull] object foreignKeyProperties)
	{
		return string.Format(GetString("SeedDatumNavigation", "entityType", "navigation", "relatedEntityType", "foreignKeyProperties"), entityType, navigation, relatedEntityType, foreignKeyProperties);
	}

	public static string SeedDatumNavigationSensitive([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyValue, [JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object relatedEntityType, [JetBrains.Annotations.CanBeNull] object foreignKeyProperties)
	{
		return string.Format(GetString("SeedDatumNavigationSensitive", "entityType", "keyValue", "navigation", "relatedEntityType", "foreignKeyProperties"), entityType, keyValue, navigation, relatedEntityType, foreignKeyProperties);
	}

	public static string SeedDatumDerivedType([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object derivedType)
	{
		return string.Format(GetString("SeedDatumDerivedType", "entityType", "derivedType"), entityType, derivedType);
	}

	public static string ConstructorNotFound([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object constructors)
	{
		return string.Format(GetString("ConstructorNotFound", "entityType", "constructors"), entityType, constructors);
	}

	public static string ConstructorConflict([JetBrains.Annotations.CanBeNull] object firstConstructor, [JetBrains.Annotations.CanBeNull] object secondConstructor)
	{
		return string.Format(GetString("ConstructorConflict", "firstConstructor", "secondConstructor"), firstConstructor, secondConstructor);
	}

	public static string ClashingNonOwnedEntityType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ClashingNonOwnedEntityType", "entityType"), entityType);
	}

	public static string KeylessTypeTracked([JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("KeylessTypeTracked", "type"), type);
	}

	public static string OwnedDerivedType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("OwnedDerivedType", "entityType"), entityType);
	}

	public static string InvalidSetTypeWeak([JetBrains.Annotations.CanBeNull] object typeName)
	{
		return string.Format(GetString("InvalidSetTypeWeak", "typeName"), typeName);
	}

	public static string PropertyConceptualNullSensitive([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object keyValue)
	{
		return string.Format(GetString("PropertyConceptualNullSensitive", "property", "entityType", "keyValue"), property, entityType, keyValue);
	}

	public static string RelationshipConceptualNullSensitive([JetBrains.Annotations.CanBeNull] object firstType, [JetBrains.Annotations.CanBeNull] object secondType, [JetBrains.Annotations.CanBeNull] object secondKeyValue)
	{
		return string.Format(GetString("RelationshipConceptualNullSensitive", "firstType", "secondType", "secondKeyValue"), firstType, secondType, secondKeyValue);
	}

	public static string IdentifyingRelationshipCycle([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("IdentifyingRelationshipCycle", "entityType"), entityType);
	}

	public static string AmbiguousServiceProperty([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object serviceType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("AmbiguousServiceProperty", "property", "serviceType", "entityType"), property, serviceType, entityType);
	}

	public static string DerivedTypeDefiningQuery([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseType)
	{
		return string.Format(GetString("DerivedTypeDefiningQuery", "entityType", "baseType"), entityType, baseType);
	}

	public static string OwnerlessOwnedType([JetBrains.Annotations.CanBeNull] object ownedType)
	{
		return string.Format(GetString("OwnerlessOwnedType", "ownedType"), ownedType);
	}

	public static string AmbiguousOwnedNavigation([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object otherEntityType)
	{
		return string.Format(GetString("AmbiguousOwnedNavigation", "entityType", "otherEntityType"), entityType, otherEntityType);
	}

	public static string FkAttributeOnNonUniquePrincipal([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object principalType, [JetBrains.Annotations.CanBeNull] object dependentType)
	{
		return string.Format(GetString("FkAttributeOnNonUniquePrincipal", "navigation", "principalType", "dependentType"), navigation, principalType, dependentType);
	}

	public static string NoIndexer([JetBrains.Annotations.CanBeNull] object entity)
	{
		return string.Format(GetString("NoIndexer", "entity"), entity);
	}

	public static string ConstructorBindingFailed([JetBrains.Annotations.CanBeNull] object failedBinds, [JetBrains.Annotations.CanBeNull] object parameters)
	{
		return string.Format(GetString("ConstructorBindingFailed", "failedBinds", "parameters"), failedBinds, parameters);
	}

	public static string NavigationToKeylessType([JetBrains.Annotations.CanBeNull] object navigation, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NavigationToKeylessType", "navigation", "entityType"), navigation, entityType);
	}

	public static string ConflictingBackingFields([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object field1, [JetBrains.Annotations.CanBeNull] object field2)
	{
		return string.Format(GetString("ConflictingBackingFields", "property", "entityType", "field1", "field2"), property, entityType, field1, field2);
	}

	public static string KeylessTypeExistingKey([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("KeylessTypeExistingKey", "entityType"), entityType);
	}

	public static string KeylessTypeWithKey([JetBrains.Annotations.CanBeNull] object key, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("KeylessTypeWithKey", "key", "entityType"), key, entityType);
	}

	public static string ConflictingForeignKeyAttributes([JetBrains.Annotations.CanBeNull] object propertyList, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ConflictingForeignKeyAttributes", "propertyList", "entityType"), propertyList, entityType);
	}

	public static string ClashingOwnedEntityType([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ClashingOwnedEntityType", "entityType"), entityType);
	}

	public static string InvalidSetTypeOwned([JetBrains.Annotations.CanBeNull] object typeName)
	{
		return string.Format(GetString("InvalidSetTypeOwned", "typeName"), typeName);
	}

	public static string InvalidSetKeylessOperation([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("InvalidSetKeylessOperation", "entityType"), entityType);
	}

	public static string DerivedEntityTypeHasNoKey([JetBrains.Annotations.CanBeNull] object derivedType, [JetBrains.Annotations.CanBeNull] object rootType)
	{
		return string.Format(GetString("DerivedEntityTypeHasNoKey", "derivedType", "rootType"), derivedType, rootType);
	}

	public static string DerivedEntityCannotBeKeyless([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("DerivedEntityCannotBeKeyless", "entityType"), entityType);
	}

	public static string TrackingTypeMismatch([JetBrains.Annotations.CanBeNull] object runtimeEntityType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("TrackingTypeMismatch", "runtimeEntityType", "entityType"), runtimeEntityType, entityType);
	}

	public static string FieldNameMismatch([JetBrains.Annotations.CanBeNull] object field, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object property)
	{
		return string.Format(GetString("FieldNameMismatch", "field", "entityType", "property"), field, entityType, property);
	}

	public static string DiscriminatorEntityTypeNotDerived([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object rootEntityType)
	{
		return string.Format(GetString("DiscriminatorEntityTypeNotDerived", "entityType", "rootEntityType"), entityType, rootEntityType);
	}

	public static string DiscriminatorPropertyMustBeOnRoot([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("DiscriminatorPropertyMustBeOnRoot", "entityType"), entityType);
	}

	public static string DiscriminatorPropertyNotFound([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("DiscriminatorPropertyNotFound", "property", "entityType"), property, entityType);
	}

	public static string DiscriminatorValueIncompatible([JetBrains.Annotations.CanBeNull] object value, [JetBrains.Annotations.CanBeNull] object discriminator, [JetBrains.Annotations.CanBeNull] object discriminatorType)
	{
		return string.Format(GetString("DiscriminatorValueIncompatible", "value", "discriminator", "discriminatorType"), value, discriminator, discriminatorType);
	}

	public static string DuplicateDiscriminatorValue([JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object discriminatorValue, [JetBrains.Annotations.CanBeNull] object entityType2)
	{
		return string.Format(GetString("DuplicateDiscriminatorValue", "entityType1", "discriminatorValue", "entityType2"), entityType1, discriminatorValue, entityType2);
	}

	public static string NoDiscriminatorForValue([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object rootEntityType)
	{
		return string.Format(GetString("NoDiscriminatorForValue", "entityType", "rootEntityType"), entityType, rootEntityType);
	}

	public static string NoDiscriminatorProperty([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NoDiscriminatorProperty", "entityType"), entityType);
	}

	public static string NoDiscriminatorValue([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("NoDiscriminatorValue", "entityType"), entityType);
	}

	public static string ForeignKeyWrongType([JetBrains.Annotations.CanBeNull] object foreignKey, [JetBrains.Annotations.CanBeNull] object key, [JetBrains.Annotations.CanBeNull] object principalType, [JetBrains.Annotations.CanBeNull] object otherEntityType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("ForeignKeyWrongType", "foreignKey", "key", "principalType", "otherEntityType", "entityType"), foreignKey, key, principalType, otherEntityType, entityType);
	}

	public static string IndexWrongType([JetBrains.Annotations.CanBeNull] object index, [JetBrains.Annotations.CanBeNull] object otherEntityType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("IndexWrongType", "index", "otherEntityType", "entityType"), index, otherEntityType, entityType);
	}

	public static string KeyWrongType([JetBrains.Annotations.CanBeNull] object key, [JetBrains.Annotations.CanBeNull] object otherEntityType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("KeyWrongType", "key", "otherEntityType", "entityType"), key, otherEntityType, entityType);
	}

	public static string PropertyWrongType([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object otherEntityType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("PropertyWrongType", "property", "otherEntityType", "entityType"), property, otherEntityType, entityType);
	}

	public static string NoNavigation([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object foreignKey)
	{
		return string.Format(GetString("NoNavigation", "entityType", "foreignKey"), entityType, foreignKey);
	}

	public static string PropertyWrongName([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object clrName)
	{
		return string.Format(GetString("PropertyWrongName", "property", "entityType", "clrName"), property, entityType, clrName);
	}

	public static string PropertyClashingNonIndexer([JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("PropertyClashingNonIndexer", "property", "entityType"), property, entityType);
	}

	public static string EntityEqualitySubqueryWithCompositeKeyNotSupported([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("EntityEqualitySubqueryWithCompositeKeyNotSupported", "entityType"), entityType);
	}

	public static string EntityEqualityContainsWithCompositeKeyNotSupported([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("EntityEqualityContainsWithCompositeKeyNotSupported", "entityType"), entityType);
	}

	public static string EntityEqualityOnKeylessEntityNotSupported([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("EntityEqualityOnKeylessEntityNotSupported", "entityType"), entityType);
	}

	public static string UnableToDiscriminate([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object discriminator)
	{
		return string.Format(GetString("UnableToDiscriminate", "entityType", "discriminator"), entityType, discriminator);
	}

	public static string IncludeOnEntityWithDefiningQueryNotSupported([JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("IncludeOnEntityWithDefiningQueryNotSupported", "entityType"), entityType);
	}

	public static string ClashingNonOwnedDerivedEntityType([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object derivedType)
	{
		return string.Format(GetString("ClashingNonOwnedDerivedEntityType", "entityType", "derivedType"), entityType, derivedType);
	}

	public static string ClientProjectionCapturingConstantInMethodArgument([JetBrains.Annotations.CanBeNull] object constantType, [JetBrains.Annotations.CanBeNull] object methodName)
	{
		return string.Format(GetString("ClientProjectionCapturingConstantInMethodArgument", "constantType", "methodName"), constantType, methodName);
	}

	public static string ClientProjectionCapturingConstantInMethodInstance([JetBrains.Annotations.CanBeNull] object constantType, [JetBrains.Annotations.CanBeNull] object methodName)
	{
		return string.Format(GetString("ClientProjectionCapturingConstantInMethodInstance", "constantType", "methodName"), constantType, methodName);
	}

	public static string ClientProjectionCapturingConstantInTree([JetBrains.Annotations.CanBeNull] object constantType)
	{
		return string.Format(GetString("ClientProjectionCapturingConstantInTree", "constantType"), constantType);
	}

	private static string GetString(string name, params string[] formatterNames)
	{
		string text = _resourceManager.GetString(name);
		for (int i = 0; i < formatterNames.Length; i++)
		{
			text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
		}
		return text;
	}
}
