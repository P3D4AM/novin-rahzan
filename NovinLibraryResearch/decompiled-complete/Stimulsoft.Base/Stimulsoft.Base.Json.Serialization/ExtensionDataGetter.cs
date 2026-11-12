using System.Collections.Generic;

namespace Stimulsoft.Base.Json.Serialization;

public delegate IEnumerable<KeyValuePair<object, object>> ExtensionDataGetter(object o);
