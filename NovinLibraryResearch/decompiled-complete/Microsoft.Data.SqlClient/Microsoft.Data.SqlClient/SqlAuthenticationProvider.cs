using System.Threading.Tasks;

namespace Microsoft.Data.SqlClient;

public abstract class SqlAuthenticationProvider
{
	public static SqlAuthenticationProvider GetProvider(SqlAuthenticationMethod authenticationMethod)
	{
		return SqlAuthenticationProviderManager.Instance.GetProvider(authenticationMethod);
	}

	public static bool SetProvider(SqlAuthenticationMethod authenticationMethod, SqlAuthenticationProvider provider)
	{
		return SqlAuthenticationProviderManager.Instance.SetProvider(authenticationMethod, provider);
	}

	public virtual void BeforeLoad(SqlAuthenticationMethod authenticationMethod)
	{
	}

	public virtual void BeforeUnload(SqlAuthenticationMethod authenticationMethod)
	{
	}

	public abstract bool IsSupported(SqlAuthenticationMethod authenticationMethod);

	public abstract Task<SqlAuthenticationToken> AcquireTokenAsync(SqlAuthenticationParameters parameters);
}
