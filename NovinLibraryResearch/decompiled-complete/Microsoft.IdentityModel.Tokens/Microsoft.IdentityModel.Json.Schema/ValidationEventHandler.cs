using System;

namespace Microsoft.IdentityModel.Json.Schema;

[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
internal delegate void ValidationEventHandler(object sender, ValidationEventArgs e);
