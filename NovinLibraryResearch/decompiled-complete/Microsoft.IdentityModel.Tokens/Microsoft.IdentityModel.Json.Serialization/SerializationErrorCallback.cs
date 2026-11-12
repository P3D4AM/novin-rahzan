using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Json.Serialization;

internal delegate void SerializationErrorCallback(object o, StreamingContext context, ErrorContext errorContext);
