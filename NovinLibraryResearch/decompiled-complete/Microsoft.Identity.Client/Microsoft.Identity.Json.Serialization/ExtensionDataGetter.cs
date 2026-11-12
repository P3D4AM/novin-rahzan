using System.Collections.Generic;

namespace Microsoft.Identity.Json.Serialization;

internal delegate IEnumerable<KeyValuePair<object, object>> ExtensionDataGetter(object o);
