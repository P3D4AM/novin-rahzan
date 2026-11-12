using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Microsoft.EntityFrameworkCore.Migrations;

public abstract class Migration
{
	public const string InitialDatabase = "0";

	private IModel _targetModel;

	private List<MigrationOperation> _upOperations;

	private List<MigrationOperation> _downOperations;

	public virtual IModel TargetModel
	{
		get
		{
			return _targetModel ?? (_targetModel = Create());
			IModel Create()
			{
				Model model = new Model();
				ModelBuilder modelBuilder = new ModelBuilder(model);
				BuildTargetModel(modelBuilder);
				return model.FinalizeModel();
			}
		}
	}

	public virtual IReadOnlyList<MigrationOperation> UpOperations => _upOperations ?? (_upOperations = BuildOperations(Up));

	public virtual IReadOnlyList<MigrationOperation> DownOperations => _downOperations ?? (_downOperations = BuildOperations(Down));

	public virtual string ActiveProvider
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	protected virtual void BuildTargetModel([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder)
	{
	}

	protected abstract void Up([JetBrains.Annotations.NotNull] MigrationBuilder migrationBuilder);

	protected virtual void Down([JetBrains.Annotations.NotNull] MigrationBuilder migrationBuilder)
	{
		throw new NotImplementedException();
	}

	private List<MigrationOperation> BuildOperations(Action<MigrationBuilder> buildAction)
	{
		MigrationBuilder migrationBuilder = new MigrationBuilder(ActiveProvider);
		buildAction(migrationBuilder);
		return migrationBuilder.Operations;
	}
}
