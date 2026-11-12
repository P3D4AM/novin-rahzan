namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class CreateUserStatementFactory
{
	private sealed class CreateUserFromAsymKeyStatementFactory : CreateUserStatementFactory
	{
		private readonly SqlIdentifier asymKeyName;

		internal CreateUserFromAsymKeyStatementFactory(SqlIdentifier asymKeyName)
		{
			this.asymKeyName = asymKeyName;
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema)
		{
			return new SqlCreateUserFromAsymKeyStatement(userName, asymKeyName, defaultSchema);
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password)
		{
			return Create(userName, defaultSchema);
		}
	}

	private sealed class CreateUserFromCertificateStatementFactory : CreateUserStatementFactory
	{
		private readonly SqlIdentifier certificateName;

		internal CreateUserFromCertificateStatementFactory(SqlIdentifier certificateName)
		{
			this.certificateName = certificateName;
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema)
		{
			return new SqlCreateUserFromCertificateStatement(userName, certificateName, defaultSchema);
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password)
		{
			return Create(userName, defaultSchema);
		}
	}

	private sealed class CreateUserWithImplicitAuthenticationStatementFactory : CreateUserStatementFactory
	{
		internal CreateUserWithImplicitAuthenticationStatementFactory()
		{
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema)
		{
			return Create(userName, defaultSchema, null, null);
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password)
		{
			return new SqlCreateUserWithImplicitAuthenticationStatement(userName, defaultSchema, defaultLanguage, password);
		}
	}

	private sealed class CreateUserFromLoginStatementFactory : CreateUserStatementFactory
	{
		private readonly SqlIdentifier loginName;

		internal CreateUserFromLoginStatementFactory(SqlIdentifier loginName)
		{
			this.loginName = loginName;
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema)
		{
			return new SqlCreateUserFromLoginStatement(userName, loginName, defaultSchema);
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password)
		{
			return Create(userName, defaultSchema);
		}
	}

	private sealed class CreateUserFromExternalProviderStatementFactory : CreateUserStatementFactory
	{
		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema)
		{
			return new SqlCreateUserFromExternalProviderStatement(userName, defaultSchema);
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password)
		{
			return Create(userName, defaultSchema);
		}
	}

	private sealed class CreateUserWithoutLoginStatementFactory : CreateUserStatementFactory
	{
		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema)
		{
			return new SqlCreateUserWithoutLoginStatement(userName, defaultSchema);
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password)
		{
			return Create(userName, defaultSchema);
		}
	}

	private sealed class CreateUserStatementErrorFactory : CreateUserStatementFactory
	{
		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema)
		{
			return new SqlCreateUserStatementError(userName, defaultSchema);
		}

		public override SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password)
		{
			return Create(userName, defaultSchema);
		}
	}

	public abstract SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema);

	public abstract SqlCreateUserStatement Create(SqlIdentifier userName, SqlIdentifier defaultSchema, SqlIdentifier defaultLanguage, string password);

	public static CreateUserStatementFactory CreateCreateUserFromAsymKeyStatementFactory(SqlIdentifier asymKeyName)
	{
		return new CreateUserFromAsymKeyStatementFactory(asymKeyName);
	}

	public static CreateUserStatementFactory CreateCreateUserFromCertificateStatementFactory(SqlIdentifier certificateName)
	{
		return new CreateUserFromCertificateStatementFactory(certificateName);
	}

	public static CreateUserStatementFactory CreateCreateUserWithImplicitAuthenticationStatementFactory()
	{
		return new CreateUserWithImplicitAuthenticationStatementFactory();
	}

	public static CreateUserStatementFactory CreateCreateUserFromLoginStatementFactory(SqlIdentifier loginName)
	{
		return new CreateUserFromLoginStatementFactory(loginName);
	}

	public static CreateUserStatementFactory CreateCreateUserWithoutLoginStatementFactory()
	{
		return new CreateUserWithoutLoginStatementFactory();
	}

	public static CreateUserStatementFactory CreateCreateUserFromExternalProviderStatementFactory()
	{
		return new CreateUserFromExternalProviderStatementFactory();
	}

	public static CreateUserStatementFactory CreateCreateUserStatementErrorFactory()
	{
		return new CreateUserStatementErrorFactory();
	}
}
