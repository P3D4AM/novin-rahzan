using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

public sealed class ProviderConventionSetBuilderDependencies
{
	private readonly ICurrentDbContext _currentContext;

	public ITypeMappingSource TypeMappingSource { get; }

	public IParameterBindingFactories ParameterBindingFactories { get; }

	public IMemberClassifier MemberClassifier { get; }

	public IConstructorBindingFactory ConstructorBindingFactory { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Model> Logger { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Model.Validation> ValidationLogger { get; }

	public IDbSetFinder SetFinder { get; }

	public Type ContextType => _currentContext.Context.GetType();

	public IModelValidator ModelValidator { get; }

	[EntityFrameworkInternal]
	public ProviderConventionSetBuilderDependencies([JetBrains.Annotations.NotNull] ITypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] IConstructorBindingFactory constructorBindingFactory, [JetBrains.Annotations.NotNull] IParameterBindingFactories parameterBindingFactories, [JetBrains.Annotations.NotNull] IMemberClassifier memberClassifier, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model> logger, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> validationLogger, [JetBrains.Annotations.NotNull] IDbSetFinder setFinder, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IModelValidator validator)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(constructorBindingFactory, "constructorBindingFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameterBindingFactories, "parameterBindingFactories");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberClassifier, "memberClassifier");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(validationLogger, "validationLogger");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(setFinder, "setFinder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(validator, "validator");
		TypeMappingSource = typeMappingSource;
		ParameterBindingFactories = parameterBindingFactories;
		MemberClassifier = memberClassifier;
		ConstructorBindingFactory = constructorBindingFactory;
		Logger = logger;
		ValidationLogger = validationLogger;
		SetFinder = setFinder;
		_currentContext = currentContext;
		ModelValidator = validator;
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] ITypeMappingSource typeMappingSource)
	{
		return new ProviderConventionSetBuilderDependencies(typeMappingSource, ConstructorBindingFactory, ParameterBindingFactories, MemberClassifier, Logger, ValidationLogger, SetFinder, _currentContext, ModelValidator);
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] IConstructorBindingFactory constructorBindingFactory)
	{
		return new ProviderConventionSetBuilderDependencies(TypeMappingSource, constructorBindingFactory, ParameterBindingFactories, MemberClassifier, Logger, ValidationLogger, SetFinder, _currentContext, ModelValidator);
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model> logger)
	{
		return new ProviderConventionSetBuilderDependencies(TypeMappingSource, ConstructorBindingFactory, ParameterBindingFactories, MemberClassifier, logger, ValidationLogger, SetFinder, _currentContext, ModelValidator);
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> validationLogger)
	{
		return new ProviderConventionSetBuilderDependencies(TypeMappingSource, ConstructorBindingFactory, ParameterBindingFactories, MemberClassifier, Logger, validationLogger, SetFinder, _currentContext, ModelValidator);
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] IParameterBindingFactories parameterBindingFactories)
	{
		return new ProviderConventionSetBuilderDependencies(TypeMappingSource, ConstructorBindingFactory, parameterBindingFactories, MemberClassifier, Logger, ValidationLogger, SetFinder, _currentContext, ModelValidator);
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] IMemberClassifier memberClassifier)
	{
		return new ProviderConventionSetBuilderDependencies(TypeMappingSource, ConstructorBindingFactory, ParameterBindingFactories, memberClassifier, Logger, ValidationLogger, SetFinder, _currentContext, ModelValidator);
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] IDbSetFinder setFinder)
	{
		return new ProviderConventionSetBuilderDependencies(TypeMappingSource, ConstructorBindingFactory, ParameterBindingFactories, MemberClassifier, Logger, ValidationLogger, setFinder, _currentContext, ModelValidator);
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new ProviderConventionSetBuilderDependencies(TypeMappingSource, ConstructorBindingFactory, ParameterBindingFactories, MemberClassifier, Logger, ValidationLogger, SetFinder, currentContext, ModelValidator);
	}

	public ProviderConventionSetBuilderDependencies With([JetBrains.Annotations.NotNull] IModelValidator validator)
	{
		return new ProviderConventionSetBuilderDependencies(TypeMappingSource, ConstructorBindingFactory, ParameterBindingFactories, MemberClassifier, Logger, ValidationLogger, SetFinder, _currentContext, validator);
	}
}
