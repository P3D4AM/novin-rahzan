using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Json.Serialization;

internal interface IAttributeProvider
{
	IList<Attribute> GetAttributes(bool inherit);

	IList<Attribute> GetAttributes(Type attributeType, bool inherit);
}
