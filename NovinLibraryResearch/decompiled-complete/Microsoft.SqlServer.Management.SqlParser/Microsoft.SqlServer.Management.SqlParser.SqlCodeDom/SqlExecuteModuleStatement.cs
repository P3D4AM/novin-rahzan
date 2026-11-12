using System;
using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlExecuteModuleStatement : SqlExecuteStatement
{
	private ICallableModule boundModule;

	private SqlScalarVariableRefExpression returnValue;

	private readonly SqlObjectReference module;

	private readonly int? number;

	private readonly SqlExecuteArgumentCollection arguments;

	private readonly SqlModuleOption moduleOption;

	private readonly bool argumentsAreValid;

	internal override bool IsVisitable => true;

	public ICallableModule BoundModule => boundModule;

	public override IMetadataObject BoundObject => boundModule;

	public int? Number => number;

	public SqlScalarVariableRefExpression ReturnValue
	{
		get
		{
			return returnValue;
		}
		internal set
		{
			returnValue = value;
		}
	}

	public SqlObjectReference Module => module;

	public SqlExecuteArgumentCollection Arguments => arguments;

	public SqlModuleOption ModuleOption => moduleOption;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (returnValue != null)
			{
				yield return returnValue;
			}
			yield return module;
			if (arguments != null)
			{
				foreach (SqlExecuteArgument argument in arguments)
				{
					yield return argument;
				}
			}
			if (moduleOption != null)
			{
				yield return moduleOption;
			}
		}
	}

	public override void Accept(ISqlStatementVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlStatementVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void BindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		if (returnValue != null)
		{
			returnValue.Bind(bindingContext);
			base.BindingState = returnValue.BindingState;
		}
		SqlObjectIdentifier objectIdentifier = module.ObjectIdentifier;
		if (objectIdentifier != null)
		{
			LexicalScope lexicalScope = GetLexicalScope();
			boundModule = bindingContext.FindModule(objectIdentifier.DatabaseName.Value, objectIdentifier.SchemaName.Value, objectIdentifier.ObjectName.Value, lexicalScope);
			if (boundModule == null)
			{
				if (bindingContext.FindTableValuedFunction(objectIdentifier.DatabaseName.Value, objectIdentifier.SchemaName.Value, objectIdentifier.ObjectName.Value, lexicalScope) != null)
				{
					module.ReportBindError(SR.IsTableValuedFunction(objectIdentifier.ObjectName.Value), ErrorFlags.UnresolvedDatabaseObjectReference);
				}
				else if (objectIdentifier.DatabaseName.Value != null && bindingContext.FindDatabase(objectIdentifier.DatabaseName.Value) == null)
				{
					objectIdentifier.DatabaseName.ReportBindError(SR.CouldNotLocateDatabase(objectIdentifier.DatabaseName.Value), ErrorFlags.UnresolvedDatabaseObjectReference);
				}
				else
				{
					module.ReportBindError(SR.CannotFindStoredProcedure(objectIdentifier.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
				}
				base.BindingState |= BindingState.BindFailed;
			}
			else
			{
				base.BindingState |= BindingState.BindSucceeded;
			}
		}
		else
		{
			module.Variable.Bind(bindingContext);
			base.BindingState |= module.Variable.BindingState;
		}
		if (boundModule == null || BindUtils.Module.IsExtendedStoredProcedure(boundModule) || !argumentsAreValid)
		{
			return;
		}
		int num = ((arguments != null) ? arguments.Count : 0);
		int count = boundModule.Parameters.Count;
		if (num > count)
		{
			if (count == 0)
			{
				module.ReportBindError(SR.MissingParameters(objectIdentifier.ObjectName.Value));
			}
			else
			{
				module.ReportBindError(SR.TooManyArguments(objectIdentifier.ObjectName.Value));
			}
			base.BindingState |= BindingState.BindFailed;
			return;
		}
		IParameter[] array = Collection<IParameter>.CopyToArray(boundModule.Parameters);
		bool flag = true;
		for (int i = 0; i < num; i++)
		{
			SqlExecuteArgument sqlExecuteArgument = arguments[i];
			IParameter parameter = null;
			if (sqlExecuteArgument.Parameter != null)
			{
				parameter = boundModule.Parameters[sqlExecuteArgument.Parameter.VariableName];
				if (parameter == null)
				{
					sqlExecuteArgument.ReportBindError(SR.InvalidParameter(sqlExecuteArgument.Parameter.VariableName, objectIdentifier.ObjectName.Value));
					flag = false;
				}
				else
				{
					bool flag2 = true;
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] == parameter)
						{
							array[j] = null;
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						sqlExecuteArgument.ReportBindError(SR.ParameterSuppliedMultipleTimes(parameter.Name));
						flag = false;
					}
				}
			}
			else
			{
				parameter = array[i];
				array[i] = null;
			}
			if (parameter != null)
			{
				sqlExecuteArgument.Bind(bindingContext, parameter);
				if (sqlExecuteArgument.BindingState == BindingState.BindSucceeded && sqlExecuteArgument.IsOutput && !parameter.IsOutput)
				{
					sqlExecuteArgument.ReportBindError(SR.OutputParameterMismatch(parameter.Name));
					flag = false;
				}
			}
		}
		if (flag && !boundModule.IsEncrypted)
		{
			foreach (IParameter parameter2 in array)
			{
				if (parameter2 != null && string.IsNullOrEmpty(parameter2.DefaultValue))
				{
					module.ReportBindError(SR.MissingParameter(objectIdentifier.ObjectName.Value, parameter2.Name));
					break;
				}
			}
		}
		if (!BindUtils.Module.IsExtendedStoredProcedure(boundModule) || arguments == null)
		{
			return;
		}
		foreach (SqlExecuteArgument argument in arguments)
		{
			argument.Bind(bindingContext);
			base.BindingState |= argument.BindingState;
		}
	}

	internal override void Unbind()
	{
		boundModule = null;
		base.Unbind();
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		if (module.Position.startTokenNumber != base.Position.startTokenNumber)
		{
			SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
			if (sqlCodeObject != null)
			{
				bool flag = false;
				if (sqlCodeObject == returnValue)
				{
					result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.ScalarVariables);
				}
				else if (sqlCodeObject == module)
				{
					if (caretLocation.startTokenNumber == caretLocation.endTokenNumber && caretLocation.endTokenNumber == base.Position.endTokenNumber)
					{
						SqlObjectIdentifier objectIdentifier = module.ObjectIdentifier;
						if (objectIdentifier != null && objectIdentifier.Count > 0 && !objectIdentifier[objectIdentifier.Count - 1].IsMissing)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						result = module.GetCompletionList(caretLocation, CompletionContext.DatabaseQualifiedStoredProcedures | CompletionContext.ScalarVariables | CompletionContext.ScalarValuedFunctions);
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					result = RetrieveCompletionListItems(CompletionContext.ScalarVariables | CompletionContext.GlobalVariables);
				}
			}
		}
		return result;
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref cursorLocation);
		if (sqlCodeObject == module)
		{
			if (boundModule != null)
			{
				result = module.ObjectIdentifier.GetQuickInfoText(cursorLocation, displayInfoProvider, boundModule);
			}
			else if (module.Variable != null)
			{
				result = module.Variable.GetQuickInfoText(cursorLocation, displayInfoProvider);
			}
		}
		else if (sqlCodeObject != null)
		{
			result = sqlCodeObject.GetQuickInfoText(cursorLocation, displayInfoProvider);
		}
		return result;
	}

	internal override MethodHelpText GetMyMethodHelpText(LexLocation caretLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		MethodHelpText result = null;
		if (boundModule != null)
		{
			bool shouldShowParentheses = false;
			bool isVarArgs = false;
			switch (boundModule.ModuleType)
			{
			case CallableModuleType.StoredProcedure:
				result = BuildMethodHelpText(boundModule, boundModule.Parameters, SR.StoredProceduresAlwaysReturnInt, null, shouldShowParentheses, isVarArgs, displayInfoProvider);
				break;
			case CallableModuleType.ExtendedStoredProcedure:
				result = BuildMethodHelpText(boundModule, boundModule.Parameters, SR.ExtendedStoredProceduresNotSupported, null, shouldShowParentheses, isVarArgs, displayInfoProvider);
				break;
			case CallableModuleType.ScalarFunction:
			{
				string methodType = BuildParameterHelpMethodTypeString(BoundModule.ReturnType);
				result = BuildMethodHelpText(boundModule, boundModule.Parameters, null, methodType, shouldShowParentheses, isVarArgs, displayInfoProvider);
				break;
			}
			}
		}
		return result;
	}

	internal override MethodNameAndParamLocations GetMyMethodNameAndParams(LexLocation caretLocation)
	{
		MethodNameAndParamLocations result = null;
		if (BoundModule != null)
		{
			TokenManager tokenManager = TokenManager;
			MethodNameAndParamLocations methodNameAndParamLocations = new MethodNameAndParamLocations(BoundObject.Name);
			methodNameAndParamLocations.NameStartLocation = base.StartLocation;
			methodNameAndParamLocations.NameEndLocation = base.EndLocation;
			methodNameAndParamLocations.ParamStartLocation = Module.ObjectIdentifier.EndLocation;
			methodNameAndParamLocations.ParamSeperatorLocations = new List<Location>();
			if (Arguments != null)
			{
				SqlExecuteArgument sqlExecuteArgument = Arguments[0];
				methodNameAndParamLocations.ParamStartLocation = tokenManager.GetLocation(sqlExecuteArgument.Position.startTokenNumber - 1);
				for (int i = 1; i < Arguments.Count; i++)
				{
					SqlExecuteArgument sqlExecuteArgument2 = Arguments[i];
					int num = tokenManager.FindTokenInRange(44, sqlExecuteArgument.Position.endTokenNumber, sqlExecuteArgument2.Position.startTokenNumber);
					if (num != -1)
					{
						methodNameAndParamLocations.ParamSeperatorLocations.Add(tokenManager.GetLocation(num));
					}
					sqlExecuteArgument = sqlExecuteArgument2;
				}
				methodNameAndParamLocations.ParamEndLocation = tokenManager.GetLocation(sqlExecuteArgument.Position.endTokenNumber + 1);
			}
			Location location = tokenManager.GetLocation(caretLocation.startTokenNumber);
			if (methodNameAndParamLocations.IsLocationInArgumentList(location))
			{
				result = methodNameAndParamLocations;
			}
		}
		return result;
	}

	internal SqlExecuteModuleStatement(SqlObjectReference module, SqlLiteralExpression number, List<SqlExecuteArgument> arguments, SqlModuleOption moduleOption)
	{
		this.module = module;
		this.number = null;
		if (number != null)
		{
			this.number = number.AsInt32();
			if (!this.number.HasValue)
			{
				number.ReportSyntaxError();
			}
			else if (this.number.Value == 0 || this.number.Value > 32767)
			{
				number.ReportParseError(SR.InvalidProcedureNumberRange(this.number.Value));
				this.number = null;
			}
		}
		argumentsAreValid = true;
		if (arguments != null)
		{
			this.arguments = new SqlExecuteArgumentCollection(arguments);
			bool flag = false;
			for (int i = 0; i < this.arguments.Count; i++)
			{
				SqlExecuteArgument sqlExecuteArgument = this.arguments[i];
				bool flag2 = sqlExecuteArgument.Parameter != null;
				if (flag && !flag2)
				{
					sqlExecuteArgument.ReportParseError(SR.InconsistentParameterFormat(i + 1));
					argumentsAreValid = false;
					break;
				}
				flag = flag2;
			}
		}
		if (moduleOption != null)
		{
			this.moduleOption = moduleOption;
			SqlModuleOptionMask typeMask = moduleOption.TypeMask;
			if ((typeMask & SqlModuleOptionMask.Invalid) == SqlModuleOptionMask.Invalid)
			{
				moduleOption.ReportParseError(SR.OptionNotRecognized(moduleOption.DisplayName));
			}
			else if ((typeMask | SqlModuleOptionMask.Recompile) != SqlModuleOptionMask.Recompile)
			{
				moduleOption.ReportParseError(SR.InvalidExecuteOption);
			}
		}
	}
}
