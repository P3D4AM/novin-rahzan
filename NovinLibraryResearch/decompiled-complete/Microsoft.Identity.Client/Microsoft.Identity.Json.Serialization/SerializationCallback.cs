using System.Runtime.Serialization;

namespace Microsoft.Identity.Json.Serialization;

internal delegate void SerializationCallback(object o, StreamingContext context);
