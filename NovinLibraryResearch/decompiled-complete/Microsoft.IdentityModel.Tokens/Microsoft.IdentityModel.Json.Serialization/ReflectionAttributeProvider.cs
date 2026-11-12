using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Json.Utilities;

namespace Microsoft.IdentityModel.Json.Serialization;

internal class ReflectionAttributeProvider : IAttributeProvider
{
	private readonly object _attributeProvider;

	public ReflectionAttributeProvider(object attributeProvider)
	{
		ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
		_attributeProvider = attributeProvider;
	}

	public IList<Attribute> GetAttributes(bool inherit)
	{
		return ReflectionUtils.GetAttributes(_attributeProvider, null, inherit);
	}

	public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
	{
		return ReflectionUtils.GetAttributes(_attributeProvider, attributeType, inherit);
	}
}
