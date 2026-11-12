using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Data.SqlClient;

internal delegate bool ServerCertificateValidationCallback(X509Certificate2 certificate);
