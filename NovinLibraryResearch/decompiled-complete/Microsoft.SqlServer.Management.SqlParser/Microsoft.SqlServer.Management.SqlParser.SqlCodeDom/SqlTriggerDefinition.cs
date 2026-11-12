using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlTriggerDefinition : SqlCodeObject
{
	internal const string AppendOptionDisplayName = "APPEND";

	internal const string NotForReplicationOptionDisplayName = "NOT FOR REPLICATION";

	private readonly SqlObjectIdentifier name;

	private readonly SqlCollection<SqlModuleOption> options;

	private readonly SqlTriggerActivationType activationType;

	private bool isOrAlterStatement;

	public SqlObjectIdentifier Name => name;

	public SqlCollection<SqlModuleOption> Options => options;

	public SqlTriggerActivationType ActivationType => activationType;

	public bool IsOrAlterStatement
	{
		get
		{
			return isOrAlterStatement;
		}
		set
		{
			isOrAlterStatement = value;
		}
	}

	internal abstract TriggerType TriggerType { get; }

	internal abstract IDmlTriggerDefinitionInfo DmlDefinitionInfo { get; }

	internal abstract IDdlTriggerDefinitionInfo DdlDefinitionInfo { get; }

	internal sealed override void Bind(IBindingContext bindingContext)
	{
	}

	internal void Bind(BatchBindingContext bindingContext, TriggerUnion<IMutableDmlTrigger, IMutableDatabaseDdlTrigger, IMutableServerDdlTrigger, IMutableTrigger> triggerUnion, ITableViewBase target, bool executionSuccedded)
	{
		IMutableTrigger asTrigger = triggerUnion.AsTrigger;
		asTrigger.IsEnabled = true;
		asTrigger.IsSqlClr = false;
		bool flag = true;
		if (options != null)
		{
			SqlModuleOptionMask sqlModuleOptionMask = SqlModuleOptionMask.None;
			foreach (SqlModuleOption option in options)
			{
				option.Bind(bindingContext);
				base.BindingState |= option.BindingState;
				SqlModuleOptionMask typeMask = option.TypeMask;
				if ((typeMask & SqlModuleOptionMask.Invalid) != SqlModuleOptionMask.Invalid && (sqlModuleOptionMask & typeMask) != typeMask)
				{
					switch (option.Type)
					{
					case SqlModuleOptionType.ExecuteAs:
						asTrigger.ExecutionContext = option.ExecuteAsClause.BoundExecutionContext;
						flag = false;
						break;
					case SqlModuleOptionType.Encryption:
						asTrigger.IsEncrypted = true;
						break;
					}
					sqlModuleOptionMask |= typeMask;
				}
			}
		}
		if (flag)
		{
			asTrigger.ExecutionContext = bindingContext.MetadataFactory.ExecutionContext.CreateExecuteAsCaller();
		}
		if (triggerUnion.IsDmlTrigger)
		{
			BindDmlTrigger(bindingContext, triggerUnion.DmlTrigger, target, executionSuccedded);
		}
		else if (triggerUnion.IsDatabaseDdlTrigger)
		{
			BindDatabaseDdlTrigger(bindingContext, triggerUnion.DatabaseDdlTrigger);
		}
		else if (triggerUnion.IsServerDdlTrigger)
		{
			BindServerDdlTrigger(bindingContext, triggerUnion.ServerDdlTrigger);
		}
		base.BindingState |= BindingState.BindSucceeded;
	}

	private void BindDmlTrigger(BatchBindingContext bindingContext, IMutableDmlTrigger trigger, ITableViewBase target, bool executionSuccedded)
	{
		IDmlTriggerDefinitionInfo dmlDefinitionInfo = DmlDefinitionInfo;
		trigger.NotForReplication = dmlDefinitionInfo.IsNotForReplication;
		if (target.TabularType == TabularType.View)
		{
			IView obj = target as IView;
			if (!trigger.InsteadOf)
			{
				Name.ReportBindError(SR.RequiredInsteadOfTriggerOnView(Name.ToString(), dmlDefinitionInfo.TargetName.ToString()));
			}
			if (obj.HasCheckOption)
			{
				Name.ReportBindError(SR.CannotCreateTriggerOnViewWithCheckOption(Name.ToString(), dmlDefinitionInfo.TargetName.ToString()));
			}
		}
		else if (executionSuccedded && trigger.InsteadOf)
		{
			if (trigger.Update)
			{
				foreach (IConstraint constraint in target.Constraints)
				{
					if (constraint is IForeignKeyConstraint { UpdateAction: ForeignKeyAction.Cascade })
					{
						Name.ReportBindError(SR.CannotCreateInsteadOfTriggerOnTableWithCascade(Name.ToString(), dmlDefinitionInfo.TargetName.ToString(), "UPDATE"));
						break;
					}
				}
			}
			if (trigger.Delete)
			{
				foreach (IConstraint constraint2 in target.Constraints)
				{
					if (constraint2 is IForeignKeyConstraint { DeleteAction: ForeignKeyAction.Cascade })
					{
						Name.ReportBindError(SR.CannotCreateInsteadOfTriggerOnTableWithCascade(Name.ToString(), dmlDefinitionInfo.TargetName.ToString(), "DELETE"));
						break;
					}
				}
			}
		}
		if (!executionSuccedded || !trigger.InsteadOf)
		{
			return;
		}
		if (trigger.Delete)
		{
			foreach (IDmlTrigger trigger2 in target.Triggers)
			{
				if (trigger2 != trigger && trigger2.InsteadOf && trigger2.Delete)
				{
					Name.ReportBindError(SR.DuplicateInsteadOfTrigger(Name.ToString(), dmlDefinitionInfo.TargetName.ToString(), "DELETE"));
					break;
				}
			}
		}
		if (trigger.Insert)
		{
			foreach (IDmlTrigger trigger3 in target.Triggers)
			{
				if (trigger3 != trigger && trigger3.InsteadOf && trigger3.Insert)
				{
					Name.ReportBindError(SR.DuplicateInsteadOfTrigger(Name.ToString(), dmlDefinitionInfo.TargetName.ToString(), "INSERT"));
					break;
				}
			}
		}
		if (!trigger.Update)
		{
			return;
		}
		foreach (IDmlTrigger trigger4 in target.Triggers)
		{
			if (trigger4 != trigger && trigger4.InsteadOf && trigger4.Update)
			{
				Name.ReportBindError(SR.DuplicateInsteadOfTrigger(Name.ToString(), dmlDefinitionInfo.TargetName.ToString(), "UPDATE"));
				break;
			}
		}
	}

	private void BindDatabaseDdlTrigger(BatchBindingContext bindingContext, IMutableDatabaseDdlTrigger trigger)
	{
		IDdlTriggerDefinitionInfo ddlDefinitionInfo = DdlDefinitionInfo;
		if (ddlDefinitionInfo.Events != null)
		{
			foreach (SqlTriggerEvent @event in ddlDefinitionInfo.Events)
			{
				trigger.DatabaseDdlEvents.Add(@event.Name.ToUpperInvariant());
			}
			return;
		}
		base.BindingState |= BindingState.BindFailed;
	}

	private void BindServerDdlTrigger(BatchBindingContext bindingContext, IMutableServerDdlTrigger trigger)
	{
		IDdlTriggerDefinitionInfo ddlDefinitionInfo = DdlDefinitionInfo;
		if (ddlDefinitionInfo.Events != null)
		{
			foreach (SqlTriggerEvent @event in ddlDefinitionInfo.Events)
			{
				trigger.ServerDdlEvents.Add(@event.Name.ToUpperInvariant());
			}
			return;
		}
		base.BindingState |= BindingState.BindFailed;
	}

	internal SqlTriggerDefinition(SqlObjectIdentifier name, SqlCollection<SqlModuleOption> options, SqlTriggerActivationType activationType)
	{
		this.name = name;
		this.options = options;
		this.activationType = activationType;
	}

	internal static SqlTriggerDefinition Create(SqlObjectIdentifier name, TriggerTargetData targetData, List<SqlModuleOption> options, TriggerActivationData activationData, bool isWithAppend, bool isNotForReplication)
	{
		SqlModuleOptionMask sqlModuleOptionMask = SqlModuleOptionMask.None;
		SqlCollection<SqlModuleOption> sqlCollection = null;
		if (options != null)
		{
			sqlCollection = new SqlCollection<SqlModuleOption>(options);
			foreach (SqlModuleOption option in options)
			{
				SqlModuleOptionMask typeMask = option.TypeMask;
				if ((typeMask & SqlModuleOptionMask.Invalid) == SqlModuleOptionMask.Invalid)
				{
					option.ReportParseError(SR.OptionNotRecognized(option.DisplayName));
				}
				else if ((typeMask | SqlModuleOptionMask.CreateTrigger) != SqlModuleOptionMask.CreateTrigger)
				{
					option.ReportParseError(SR.InvalidOptionInCreateTrigger);
				}
				else if ((sqlModuleOptionMask & typeMask) == typeMask)
				{
					option.ReportParseError(SR.OptionSpecifiedMultipleTimes(option.DisplayName));
				}
				else
				{
					sqlModuleOptionMask |= typeMask;
				}
			}
		}
		if (targetData.TriggerType == TriggerType.DmlTrigger && activationData.TriggerType == TriggerType.DmlTrigger)
		{
			return new SqlDmlTriggerDefinition(name, targetData.TargetName, sqlCollection, activationData.ActivationType, activationData.Actions, isWithAppend, isNotForReplication);
		}
		if (targetData.TriggerType == TriggerType.DdlTrigger && activationData.TriggerType == TriggerType.DdlTrigger)
		{
			if (isWithAppend)
			{
				name.ReportParseError(SR.OptionNotRecognized("APPEND"));
			}
			if (isNotForReplication)
			{
				name.ReportParseError(SR.OptionNotRecognized("NOT FOR REPLICATION"));
			}
			return new SqlDdlTriggerDefinition(name, targetData.TargetType, sqlCollection, activationData.ActivationType, activationData.Events);
		}
		name.ReportParseError(SR.InvalidTriggerEventTypes);
		return new SqlTriggerDefinitionError(targetData.TriggerType, name, targetData.TargetName, targetData.TargetType, sqlCollection, activationData.ActivationType, activationData.Actions, activationData.Events, isWithAppend, isNotForReplication);
	}

	internal static SqlTriggerDefinition Create(SqlTriggerDefinition sqlTriggerDefinition, bool isOrAlterStatement)
	{
		sqlTriggerDefinition.IsOrAlterStatement = isOrAlterStatement;
		return sqlTriggerDefinition;
	}
}
