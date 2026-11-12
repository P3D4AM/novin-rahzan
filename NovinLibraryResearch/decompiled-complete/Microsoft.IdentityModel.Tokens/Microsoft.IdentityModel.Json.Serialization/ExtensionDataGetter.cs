using System.Collections.Generic;

namespace Microsoft.IdentityModel.Json.Serialization;

internal delegate IEnumerable<KeyValuePair<object, object>> ExtensionDataGetter(object o);
