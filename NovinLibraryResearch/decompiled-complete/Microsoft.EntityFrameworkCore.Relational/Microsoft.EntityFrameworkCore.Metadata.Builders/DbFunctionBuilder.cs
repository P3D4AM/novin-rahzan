using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DbFunctionBuilder : IConventionDbFunctionBuilder
{
	private readonly DbFunction _function;

	public virtual IMutableDbFunction Metadata => _function;

	IConventionDbFunction IConventionDbFunctionBuilder.Metadata => _function;

	[EntityFrameworkInternal]
	public DbFunctionBuilder([JetBrains.Annotations.NotNull] IMutableDbFunction function)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(function, "function");
		_function = (DbFunction)function;
	}

	public virtual DbFunctionBuilder HasName([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		_function.Name = name;
		return this;
	}

	IConventionDbFunctionBuilder IConventionDbFunctionBuilder.HasName(string name, bool fromDataAnnotation)
	{
		if (((IConventionDbFunctionBuilder)this).CanSetName(name, fromDataAnnotation))
		{
			((IConventionDbFunction)_function).SetName(name, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionDbFunctionBuilder.CanSetName(string name, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _function.GetNameConfigurationSource()))
		{
			return _function.Name == name;
		}
		return true;
	}

	public virtual DbFunctionBuilder HasSchema([JetBrains.Annotations.CanBeNull] string schema)
	{
		_function.Schema = schema;
		return this;
	}

	IConventionDbFunctionBuilder IConventionDbFunctionBuilder.HasSchema(string schema, bool fromDataAnnotation)
	{
		if (((IConventionDbFunctionBuilder)this).CanSetSchema(schema, fromDataAnnotation))
		{
			((IConventionDbFunction)_function).SetSchema(schema, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionDbFunctionBuilder.CanSetSchema(string schema, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _function.GetSchemaConfigurationSource()))
		{
			return _function.Schema == schema;
		}
		return true;
	}

	public virtual DbFunctionBuilder HasStoreType([JetBrains.Annotations.CanBeNull] string storeType)
	{
		_function.StoreType = storeType;
		return this;
	}

	IConventionDbFunctionBuilder IConventionDbFunctionBuilder.HasStoreType(string storeType, bool fromDataAnnotation)
	{
		if (((IConventionDbFunctionBuilder)this).CanSetStoreType(storeType, fromDataAnnotation))
		{
			((IConventionDbFunction)_function).SetStoreType(storeType, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionDbFunctionBuilder.CanSetStoreType(string storeType, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _function.GetStoreTypeConfigurationSource()))
		{
			return _function.StoreType == storeType;
		}
		return true;
	}

	IConventionDbFunctionBuilder IConventionDbFunctionBuilder.HasTypeMapping(RelationalTypeMapping returnTypeMapping, bool fromDataAnnotation)
	{
		if (((IConventionDbFunctionBuilder)this).CanSetTypeMapping(returnTypeMapping, fromDataAnnotation))
		{
			((IConventionDbFunction)_function).SetTypeMapping(returnTypeMapping, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionDbFunctionBuilder.CanSetTypeMapping(RelationalTypeMapping returnTypeMapping, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _function.GetTypeMappingConfigurationSource()))
		{
			return _function.TypeMapping == returnTypeMapping;
		}
		return true;
	}

	public virtual DbFunctionBuilder HasTranslation([JetBrains.Annotations.NotNull] Func<IReadOnlyCollection<SqlExpression>, SqlExpression> translation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(translation, "translation");
		_function.Translation = translation;
		return this;
	}

	IConventionDbFunctionBuilder IConventionDbFunctionBuilder.HasTranslation(Func<IReadOnlyCollection<SqlExpression>, SqlExpression> translation, bool fromDataAnnotation)
	{
		if (((IConventionDbFunctionBuilder)this).CanSetTranslation(translation, fromDataAnnotation))
		{
			((IConventionDbFunction)_function).SetTranslation(translation, fromDataAnnotation);
			return this;
		}
		return null;
	}

	public virtual DbFunctionParameterBuilder HasParameter([JetBrains.Annotations.NotNull] string name)
	{
		return new DbFunctionParameterBuilder((DbFunctionParameter)FindParameter(name));
	}

	private IDbFunctionParameter FindParameter(string name)
	{
		return Metadata.Parameters.SingleOrDefault((IDbFunctionParameter funcParam) => string.Compare(funcParam.Name, name, StringComparison.OrdinalIgnoreCase) == 0) ?? throw new ArgumentException(RelationalStrings.DbFunctionInvalidParameterName(name, Metadata.MethodInfo.DisplayName()));
	}

	bool IConventionDbFunctionBuilder.CanSetTranslation(Func<IReadOnlyCollection<SqlExpression>, SqlExpression> translation, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _function.GetTranslationConfigurationSource()))
		{
			return _function.Translation == translation;
		}
		return true;
	}

	private bool Overrides(bool fromDataAnnotation, ConfigurationSource? configurationSource)
	{
		return (fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention).Overrides(configurationSource);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
