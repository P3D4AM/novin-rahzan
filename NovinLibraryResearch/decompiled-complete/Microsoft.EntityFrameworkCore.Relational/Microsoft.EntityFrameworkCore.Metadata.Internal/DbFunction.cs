using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class DbFunction : IMutableDbFunction, IDbFunction, IConventionDbFunction
{
	private readonly IMutableModel _model;

	private readonly string _annotationName;

	private readonly List<DbFunctionParameter> _parameters;

	private string _schema;

	private string _name;

	private string _storeType;

	private RelationalTypeMapping _typeMapping;

	private Func<IReadOnlyCollection<SqlExpression>, SqlExpression> _translation;

	private ConfigurationSource? _schemaConfigurationSource;

	private ConfigurationSource? _nameConfigurationSource;

	private ConfigurationSource? _storeTypeConfigurationSource;

	private ConfigurationSource? _typeMappingConfigurationSource;

	private ConfigurationSource? _translationConfigurationSource;

	public virtual IConventionDbFunctionBuilder Builder => new DbFunctionBuilder(this);

	public virtual string Schema
	{
		get
		{
			return _schema ?? _model.GetDefaultSchema();
		}
		set
		{
			SetSchema(value, ConfigurationSource.Explicit);
		}
	}

	public virtual string Name
	{
		get
		{
			return _name ?? MethodInfo.Name;
		}
		set
		{
			SetName(value, ConfigurationSource.Explicit);
		}
	}

	public virtual MethodInfo MethodInfo { get; }

	public virtual string StoreType
	{
		get
		{
			return _storeType;
		}
		set
		{
			SetStoreType(value, ConfigurationSource.Explicit);
		}
	}

	public virtual RelationalTypeMapping TypeMapping
	{
		get
		{
			return _typeMapping;
		}
		set
		{
			SetTypeMapping(value, ConfigurationSource.Explicit);
		}
	}

	public virtual Func<IReadOnlyCollection<SqlExpression>, SqlExpression> Translation
	{
		get
		{
			return _translation;
		}
		set
		{
			SetTranslation(value, ConfigurationSource.Explicit);
		}
	}

	IModel IDbFunction.Model => _model;

	IMutableModel IMutableDbFunction.Model => _model;

	IConventionModel IConventionDbFunction.Model => (IConventionModel)_model;

	public virtual IReadOnlyList<IDbFunctionParameter> Parameters => _parameters;

	IReadOnlyList<IConventionDbFunctionParameter> IConventionDbFunction.Parameters => _parameters;

	public DbFunction([JetBrains.Annotations.NotNull] MethodInfo methodInfo, [JetBrains.Annotations.NotNull] IMutableModel model, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		if (methodInfo.IsGenericMethod)
		{
			throw new ArgumentException(RelationalStrings.DbFunctionGenericMethodNotSupported(methodInfo.DisplayName()));
		}
		if (!methodInfo.IsStatic && !typeof(DbContext).IsAssignableFrom(methodInfo.DeclaringType))
		{
			throw new ArgumentException(RelationalStrings.DbFunctionInvalidInstanceType(methodInfo.DisplayName(), methodInfo.DeclaringType.ShortDisplayName()));
		}
		if (methodInfo.ReturnType == null || methodInfo.ReturnType == typeof(void))
		{
			throw new ArgumentException(RelationalStrings.DbFunctionInvalidReturnType(methodInfo.DisplayName(), methodInfo.ReturnType.ShortDisplayName()));
		}
		MethodInfo = methodInfo;
		_model = model;
		_parameters = methodInfo.GetParameters().Select((ParameterInfo pi, int i) => new DbFunctionParameter(this, pi.Name, pi.ParameterType)).ToList();
		_annotationName = BuildAnnotationName(methodInfo);
		if (configurationSource == ConfigurationSource.Explicit)
		{
			_model.AddAnnotation(_annotationName, this);
		}
		else
		{
			((IConventionModel)_model).AddAnnotation(_annotationName, this, configurationSource == ConfigurationSource.DataAnnotation);
		}
	}

	public static IEnumerable<DbFunction> GetDbFunctions([JetBrains.Annotations.NotNull] Model model)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		return (from a in model.GetAnnotations()
			where a.Name.StartsWith("Relational:DbFunction", StringComparison.Ordinal)
			select a.Value).Cast<DbFunction>();
	}

	private static string BuildAnnotationName(MethodBase methodBase)
	{
		return "Relational:DbFunction" + methodBase.DeclaringType.FullName + methodBase.Name + "(" + string.Join(",", from p in methodBase.GetParameters()
			select p.ParameterType.FullName) + ")";
	}

	public virtual ConfigurationSource GetConfigurationSource()
	{
		return ((IConventionModel)_model).FindAnnotation(_annotationName).GetConfigurationSource();
	}

	public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		((Model)_model).FindAnnotation(_annotationName).UpdateConfigurationSource(configurationSource);
	}

	public virtual void SetSchema([JetBrains.Annotations.CanBeNull] string schema, ConfigurationSource configurationSource)
	{
		_schema = schema;
		UpdateSchemaConfigurationSource(configurationSource);
	}

	private void UpdateSchemaConfigurationSource(ConfigurationSource configurationSource)
	{
		_schemaConfigurationSource = configurationSource.Max(_schemaConfigurationSource);
	}

	public virtual ConfigurationSource? GetSchemaConfigurationSource()
	{
		return _schemaConfigurationSource;
	}

	public virtual void SetName([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		_name = name;
		UpdateNameConfigurationSource(configurationSource);
	}

	private void UpdateNameConfigurationSource(ConfigurationSource configurationSource)
	{
		_nameConfigurationSource = configurationSource.Max(_nameConfigurationSource);
	}

	public virtual ConfigurationSource? GetNameConfigurationSource()
	{
		return _nameConfigurationSource;
	}

	public virtual void SetStoreType([JetBrains.Annotations.NotNull] string storeType, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(storeType, "storeType");
		_storeType = storeType;
		UpdateStoreTypeConfigurationSource(configurationSource);
	}

	private void UpdateStoreTypeConfigurationSource(ConfigurationSource configurationSource)
	{
		_storeTypeConfigurationSource = configurationSource.Max(_storeTypeConfigurationSource);
	}

	public virtual ConfigurationSource? GetStoreTypeConfigurationSource()
	{
		return _storeTypeConfigurationSource;
	}

	public virtual void SetTypeMapping([JetBrains.Annotations.CanBeNull] RelationalTypeMapping typeMapping, ConfigurationSource configurationSource)
	{
		_typeMapping = typeMapping;
		UpdateTypeMappingConfigurationSource(configurationSource);
	}

	private void UpdateTypeMappingConfigurationSource(ConfigurationSource configurationSource)
	{
		_typeMappingConfigurationSource = configurationSource.Max(_typeMappingConfigurationSource);
	}

	public virtual ConfigurationSource? GetTypeMappingConfigurationSource()
	{
		return _typeMappingConfigurationSource;
	}

	public virtual void SetTranslation([JetBrains.Annotations.CanBeNull] Func<IReadOnlyCollection<SqlExpression>, SqlExpression> translation, ConfigurationSource configurationSource)
	{
		_translation = translation;
		UpdateTranslationConfigurationSource(configurationSource);
	}

	private void UpdateTranslationConfigurationSource(ConfigurationSource configurationSource)
	{
		_translationConfigurationSource = configurationSource.Max(_translationConfigurationSource);
	}

	public virtual ConfigurationSource? GetTranslationConfigurationSource()
	{
		return _translationConfigurationSource;
	}

	public static DbFunction FindDbFunction([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] MethodInfo methodInfo)
	{
		return model[BuildAnnotationName(methodInfo)] as DbFunction;
	}

	public static DbFunction RemoveDbFunction([JetBrains.Annotations.NotNull] IMutableModel model, [JetBrains.Annotations.NotNull] MethodInfo methodInfo)
	{
		return model.RemoveAnnotation(BuildAnnotationName(methodInfo))?.Value as DbFunction;
	}

	void IConventionDbFunction.SetName(string name, bool fromDataAnnotation)
	{
		SetName(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionDbFunction.SetSchema(string schema, bool fromDataAnnotation)
	{
		SetSchema(schema, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionDbFunction.SetStoreType(string storeType, bool fromDataAnnotation)
	{
		SetStoreType(storeType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionDbFunction.SetTypeMapping(RelationalTypeMapping returnTypeMapping, bool fromDataAnnotation)
	{
		SetTypeMapping(returnTypeMapping, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionDbFunction.SetTranslation(Func<IReadOnlyCollection<SqlExpression>, SqlExpression> translation, bool fromDataAnnotation)
	{
		SetTranslation(translation, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
