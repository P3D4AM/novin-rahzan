using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Microsoft.EntityFrameworkCore;

public static class DbLoggerCategory
{
	public class Database : LoggerCategory<Database>
	{
		public class Connection : LoggerCategory<Connection>
		{
		}

		public class Command : LoggerCategory<Command>
		{
		}

		public class Transaction : LoggerCategory<Transaction>
		{
		}
	}

	public class Update : LoggerCategory<Update>
	{
	}

	public class Model : LoggerCategory<Model>
	{
		public class Validation : LoggerCategory<Validation>
		{
		}
	}

	public class Query : LoggerCategory<Query>
	{
	}

	public class Infrastructure : LoggerCategory<Infrastructure>
	{
	}

	public class Scaffolding : LoggerCategory<Scaffolding>
	{
	}

	public class Migrations : LoggerCategory<Migrations>
	{
	}

	public class ChangeTracking : LoggerCategory<ChangeTracking>
	{
	}

	public const string Name = "Microsoft.EntityFrameworkCore";
}
