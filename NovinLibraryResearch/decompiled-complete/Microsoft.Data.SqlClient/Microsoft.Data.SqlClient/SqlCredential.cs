using System.Security;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

public sealed class SqlCredential
{
	private string _userId;

	private SecureString _password;

	public string UserId => _userId;

	public SecureString Password => _password;

	public SqlCredential(string userId, SecureString password)
	{
		if (userId == null)
		{
			throw ADP.ArgumentNull("userId");
		}
		if (userId.Length > 128)
		{
			throw ADP.InvalidArgumentLength("userId", 128);
		}
		if (password == null)
		{
			throw ADP.ArgumentNull("password");
		}
		if (password.Length > 128)
		{
			throw ADP.InvalidArgumentLength("password", 128);
		}
		if (!password.IsReadOnly())
		{
			throw ADP.MustBeReadOnly("password");
		}
		_userId = userId;
		_password = password;
	}
}
