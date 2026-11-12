using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DbFunctionParameterBuilder : IConventionDbFunctionParameterBuilder
{
	private readonly DbFunctionParameter _parameter;

	public virtual IMutableDbFunctionParameter Metadata => _parameter;

	IConventionDbFunctionParameter IConventionDbFunctionParameterBuilder.Metadata => _parameter;

	[EntityFrameworkInternal]
	public DbFunctionParameterBuilder([JetBrains.Annotations.NotNull] IMutableDbFunctionParameter parameter)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameter, "parameter");
		_parameter = (DbFunctionParameter)parameter;
	}

	public virtual DbFunctionParameterBuilder HasStoreType([JetBrains.Annotations.CanBeNull] string storeType)
	{
		_parameter.StoreType = storeType;
		return this;
	}

	IConventionDbFunctionParameterBuilder IConventionDbFunctionParameterBuilder.HasStoreType(string storeType, bool fromDataAnnotation)
	{
		if (((IConventionDbFunctionParameterBuilder)this).CanSetStoreType(storeType, fromDataAnnotation))
		{
			((IConventionDbFunctionParameter)_parameter).SetStoreType(storeType, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionDbFunctionParameterBuilder.CanSetStoreType(string storeType, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _parameter.GetStoreTypeConfigurationSource()))
		{
			return _parameter.StoreType == storeType;
		}
		return true;
	}

	IConventionDbFunctionParameterBuilder IConventionDbFunctionParameterBuilder.HasTypeMapping(RelationalTypeMapping typeMapping, bool fromDataAnnotation)
	{
		if (((IConventionDbFunctionParameterBuilder)this).CanSetTypeMapping(typeMapping, fromDataAnnotation))
		{
			((IConventionDbFunctionParameter)_parameter).SetTypeMapping(typeMapping, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionDbFunctionParameterBuilder.CanSetTypeMapping(RelationalTypeMapping typeMapping, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _parameter.GetTypeMappingConfigurationSource()))
		{
			return _parameter.TypeMapping == typeMapping;
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
