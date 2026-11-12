using System.Runtime.Serialization;

namespace Microsoft.Identity.Json.Serialization;

internal delegate void SerializationErrorCallback(object o, StreamingContext context, ErrorContext errorContext);
