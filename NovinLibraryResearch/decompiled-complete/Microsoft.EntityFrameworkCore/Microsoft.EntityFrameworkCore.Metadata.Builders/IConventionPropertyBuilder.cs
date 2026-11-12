using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionPropertyBuilder : IConventionAnnotatableBuilder
{
	new IConventionProperty Metadata { get; }

	IConventionPropertyBuilder IsRequired(bool? required, bool fromDataAnnotation = false);

	bool CanSetIsRequired(bool? required, bool fromDataAnnotation = false);

	IConventionPropertyBuilder ValueGenerated(ValueGenerated? valueGenerated, bool fromDataAnnotation = false);

	bool CanSetValueGenerated(ValueGenerated? valueGenerated, bool fromDataAnnotation = false);

	IConventionPropertyBuilder IsConcurrencyToken(bool? concurrencyToken, bool fromDataAnnotation = false);

	bool CanSetIsConcurrencyToken(bool? concurrencyToken, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasField([JetBrains.Annotations.CanBeNull] string fieldName, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool fromDataAnnotation = false);

	bool CanSetField([JetBrains.Annotations.CanBeNull] string fieldName, bool fromDataAnnotation = false);

	bool CanSetField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool fromDataAnnotation = false);

	IConventionPropertyBuilder UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false);

	bool CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasMaxLength(int? maxLength, bool fromDataAnnotation = false);

	bool CanSetMaxLength(int? maxLength, bool fromDataAnnotation = false);

	IConventionPropertyBuilder IsUnicode(bool? unicode, bool fromDataAnnotation = false);

	bool CanSetIsUnicode(bool? unicode, bool fromDataAnnotation = false);

	IConventionPropertyBuilder BeforeSave(PropertySaveBehavior? behavior, bool fromDataAnnotation = false);

	bool CanSetBeforeSave(PropertySaveBehavior? behavior, bool fromDataAnnotation = false);

	IConventionPropertyBuilder AfterSave(PropertySaveBehavior? behavior, bool fromDataAnnotation = false);

	bool CanSetAfterSave(PropertySaveBehavior? behavior, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasValueGenerator([JetBrains.Annotations.CanBeNull] Type valueGeneratorType, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasValueGenerator([JetBrains.Annotations.CanBeNull] Func<IProperty, IEntityType, ValueGenerator> factory, bool fromDataAnnotation = false);

	bool CanSetValueGenerator([JetBrains.Annotations.CanBeNull] Func<IProperty, IEntityType, ValueGenerator> factory, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasConversion([JetBrains.Annotations.CanBeNull] ValueConverter converter, bool fromDataAnnotation = false);

	bool CanSetConversion([JetBrains.Annotations.CanBeNull] ValueConverter converter, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasConversion([JetBrains.Annotations.CanBeNull] Type providerClrType, bool fromDataAnnotation = false);

	bool CanSetConversion([JetBrains.Annotations.CanBeNull] Type providerClrType, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false);

	bool CanSetValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasKeyValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false);

	bool CanSetKeyValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false);

	IConventionPropertyBuilder HasStructuralValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false);

	bool CanSetStructuralValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, bool fromDataAnnotation = false);
}
