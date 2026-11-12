using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public abstract class HistoryRepository : IHistoryRepository
{
	public const string DefaultTableName = "__EFMigrationsHistory";

	private IModel _model;

	private string _migrationIdColumnName;

	private string _productVersionColumnName;

	protected virtual HistoryRepositoryDependencies Dependencies { get; }

	protected virtual ISqlGenerationHelper SqlGenerationHelper => Dependencies.SqlGenerationHelper;

	protected virtual string TableName { get; }

	protected virtual string TableSchema { get; }

	protected virtual string MigrationIdColumnName => _migrationIdColumnName ?? (_migrationIdColumnName = EnsureModel().FindEntityType(typeof(HistoryRow)).FindProperty("MigrationId").GetColumnName());

	protected virtual string ProductVersionColumnName => _productVersionColumnName ?? (_productVersionColumnName = EnsureModel().FindEntityType(typeof(HistoryRow)).FindProperty("ProductVersion").GetColumnName());

	protected abstract string ExistsSql { get; }

	protected virtual string GetAppliedMigrationsSql => new StringBuilder().Append("SELECT ").Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName)).Append(", ")
		.AppendLine(SqlGenerationHelper.DelimitIdentifier(ProductVersionColumnName))
		.Append("FROM ")
		.AppendLine(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
		.Append("ORDER BY ")
		.Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
		.Append(SqlGenerationHelper.StatementTerminator)
		.ToString();

	protected HistoryRepository([JetBrains.Annotations.NotNull] HistoryRepositoryDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
		RelationalOptionsExtension relationalOptionsExtension = RelationalOptionsExtension.Extract(dependencies.Options);
		TableName = relationalOptionsExtension?.MigrationsHistoryTableName ?? "__EFMigrationsHistory";
		TableSchema = relationalOptionsExtension?.MigrationsHistoryTableSchema;
	}

	private IModel EnsureModel()
	{
		if (_model == null)
		{
			ConventionSet conventionSet = Dependencies.ConventionSetBuilder.CreateConventionSet();
			ConventionSet.Remove(conventionSet.ModelInitializedConventions, typeof(DbSetFindingConvention));
			ModelBuilder modelBuilder = new ModelBuilder(conventionSet);
			modelBuilder.Entity(delegate(EntityTypeBuilder<HistoryRow> x)
			{
				ConfigureTable(x);
				x.ToTable(TableName, TableSchema);
			});
			_model = modelBuilder.FinalizeModel();
		}
		return _model;
	}

	public virtual bool Exists()
	{
		if (Dependencies.DatabaseCreator.Exists())
		{
			return InterpretExistsResult(Dependencies.RawSqlCommandBuilder.Build(ExistsSql).ExecuteScalar(new RelationalCommandParameterObject(Dependencies.Connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger)));
		}
		return false;
	}

	public virtual async Task<bool> ExistsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		bool flag = await Dependencies.DatabaseCreator.ExistsAsync(cancellationToken);
		if (flag)
		{
			flag = InterpretExistsResult(await Dependencies.RawSqlCommandBuilder.Build(ExistsSql).ExecuteScalarAsync(new RelationalCommandParameterObject(Dependencies.Connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger), cancellationToken));
		}
		return flag;
	}

	protected abstract bool InterpretExistsResult([JetBrains.Annotations.NotNull] object value);

	public abstract string GetCreateIfNotExistsScript();

	public virtual string GetCreateScript()
	{
		IModel model = EnsureModel();
		IReadOnlyList<MigrationOperation> differences = Dependencies.ModelDiffer.GetDifferences(null, model);
		return string.Concat(from c in Dependencies.MigrationsSqlGenerator.Generate(differences, model)
			select c.CommandText);
	}

	protected virtual void ConfigureTable([JetBrains.Annotations.NotNull] EntityTypeBuilder<HistoryRow> history)
	{
		history.ToTable("__EFMigrationsHistory");
		history.HasKey((HistoryRow h) => h.MigrationId);
		history.Property((HistoryRow h) => h.MigrationId).HasMaxLength(150);
		history.Property((HistoryRow h) => h.ProductVersion).HasMaxLength(32).IsRequired();
	}

	public virtual IReadOnlyList<HistoryRow> GetAppliedMigrations()
	{
		List<HistoryRow> list = new List<HistoryRow>();
		if (Exists())
		{
			using RelationalDataReader relationalDataReader = Dependencies.RawSqlCommandBuilder.Build(GetAppliedMigrationsSql).ExecuteReader(new RelationalCommandParameterObject(Dependencies.Connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger));
			while (relationalDataReader.Read())
			{
				list.Add(new HistoryRow(relationalDataReader.DbDataReader.GetString(0), relationalDataReader.DbDataReader.GetString(1)));
			}
		}
		return list;
	}

	public virtual async Task<IReadOnlyList<HistoryRow>> GetAppliedMigrationsAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		List<HistoryRow> rows = new List<HistoryRow>();
		if (await ExistsAsync(cancellationToken))
		{
			await using RelationalDataReader reader = await Dependencies.RawSqlCommandBuilder.Build(GetAppliedMigrationsSql).ExecuteReaderAsync(new RelationalCommandParameterObject(Dependencies.Connection, null, null, Dependencies.CurrentContext.Context, Dependencies.CommandLogger), cancellationToken);
			while (await reader.ReadAsync(cancellationToken))
			{
				rows.Add(new HistoryRow(reader.DbDataReader.GetString(0), reader.DbDataReader.GetString(1)));
			}
		}
		return rows;
	}

	public virtual string GetInsertScript(HistoryRow row)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(row, "row");
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		return new StringBuilder().Append("INSERT INTO ").Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema)).Append(" (")
			.Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
			.Append(", ")
			.Append(SqlGenerationHelper.DelimitIdentifier(ProductVersionColumnName))
			.AppendLine(")")
			.Append("VALUES (")
			.Append(mapping.GenerateSqlLiteral(row.MigrationId))
			.Append(", ")
			.Append(mapping.GenerateSqlLiteral(row.ProductVersion))
			.Append(")")
			.AppendLine(SqlGenerationHelper.StatementTerminator)
			.ToString();
	}

	public virtual string GetDeleteScript(string migrationId)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationId, "migrationId");
		RelationalTypeMapping mapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));
		return new StringBuilder().Append("DELETE FROM ").AppendLine(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema)).Append("WHERE ")
			.Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
			.Append(" = ")
			.Append(mapping.GenerateSqlLiteral(migrationId))
			.AppendLine(SqlGenerationHelper.StatementTerminator)
			.ToString();
	}

	public abstract string GetBeginIfNotExistsScript(string migrationId);

	public abstract string GetBeginIfExistsScript(string migrationId);

	public abstract string GetEndIfScript();
}
