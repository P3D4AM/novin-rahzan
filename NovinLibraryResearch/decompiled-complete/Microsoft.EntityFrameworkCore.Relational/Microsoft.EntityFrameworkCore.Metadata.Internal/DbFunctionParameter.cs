using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class DbFunctionParameter : IMutableDbFunctionParameter, IDbFunctionParameter, IConventionDbFunctionParameter
{
	private readonly IMutableDbFunction _function;

	private readonly string _name;

	private readonly Type _clrType;

	private string _storeType;

	private RelationalTypeMapping _typeMapping;

	private ConfigurationSource? _storeTypeConfigurationSource;

	private ConfigurationSource? _typeMappingConfigurationSource;

	public virtual IConventionDbFunctionParameterBuilder Builder => new DbFunctionParameterBuilder(this);

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

	IConventionDbFunction IConventionDbFunctionParameter.Function => (IConventionDbFunction)_function;

	IDbFunction IDbFunctionParameter.Function => _function;

	IMutableDbFunction IMutableDbFunctionParameter.Function => _function;

	string IDbFunctionParameter.Name => _name;

	Type IDbFunctionParameter.ClrType => _clrType;

	public DbFunctionParameter([JetBrains.Annotations.NotNull] IMutableDbFunction function, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type clrType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(function, "function");
		_name = name;
		_function = function;
		_clrType = clrType;
	}

	public virtual void SetStoreType([JetBrains.Annotations.CanBeNull] string storeType, ConfigurationSource configurationSource)
	{
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

	public virtual void SetTypeMapping(RelationalTypeMapping typeMapping, ConfigurationSource configurationSource)
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

	void IConventionDbFunctionParameter.SetStoreType(string storeType, bool fromDataAnnotation)
	{
		SetStoreType(storeType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionDbFunctionParameter.SetTypeMapping(RelationalTypeMapping typeMapping, bool fromDataAnnotation)
	{
		SetTypeMapping(typeMapping, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
