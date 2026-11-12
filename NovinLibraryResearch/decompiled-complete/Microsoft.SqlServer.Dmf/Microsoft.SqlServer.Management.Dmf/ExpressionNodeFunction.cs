using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ExpressionNodeFunction : ExpressionNodeChildren
{
	public enum Function
	{
		ExecuteSql,
		ExecuteWql,
		GetDate,
		DateAdd,
		DatePart,
		Sum,
		Avg,
		Count,
		Len,
		IsNull,
		Array,
		Add,
		Subtract,
		Multiply,
		Divide,
		BitwiseAnd,
		BitwiseOr,
		Power,
		Mod,
		Round,
		Enum,
		DateTime,
		String,
		True,
		False,
		Guid,
		Upper,
		Lower,
		Concatenate,
		Escape
	}

	private static TraceContext traceContext;

	private const string cXmlTagFuncType = "FunctionType";

	private const string cXmlTagReturnType = "ReturnType";

	private Function mFunctionType;

	private TypeClass mReturnType;

	private List<object> mArgs = new List<object>();

	private static readonly Dictionary<Function, TypeClass[]> functionDefs;

	private FacetEvaluationContext mcontext;

	public Function FunctionType => mFunctionType;

	public TypeClass ReturnType => mReturnType;

	public static Dictionary<Function, TypeClass[]> FunctionsDefinitions => functionDefs;

	static ExpressionNodeFunction()
	{
		traceContext = TraceContext.GetTraceContext("DMF", "ExpressionNodeFunction");
		functionDefs = new Dictionary<Function, TypeClass[]>(25);
		functionDefs.Add(Function.ExecuteSql, new TypeClass[3]
		{
			TypeClass.Variant,
			TypeClass.String,
			TypeClass.String
		});
		functionDefs.Add(Function.ExecuteWql, new TypeClass[4]
		{
			TypeClass.Variant,
			TypeClass.String,
			TypeClass.String,
			TypeClass.String
		});
		functionDefs.Add(Function.GetDate, new TypeClass[1] { TypeClass.DateTime });
		functionDefs.Add(Function.DateAdd, new TypeClass[4]
		{
			TypeClass.DateTime,
			TypeClass.String,
			TypeClass.Numeric,
			TypeClass.DateTime
		});
		functionDefs.Add(Function.DatePart, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.String,
			TypeClass.DateTime
		});
		functionDefs.Add(Function.Len, new TypeClass[2]
		{
			TypeClass.Numeric,
			TypeClass.String
		});
		functionDefs.Add(Function.Add, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.Subtract, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.Multiply, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.Divide, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.BitwiseAnd, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.BitwiseOr, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.Power, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.Mod, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.Round, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.Numeric,
			TypeClass.Numeric
		});
		functionDefs.Add(Function.Array, new TypeClass[2]
		{
			TypeClass.Array,
			TypeClass.VarArgs
		});
		functionDefs.Add(Function.Sum, new TypeClass[2]
		{
			TypeClass.Numeric,
			TypeClass.VarArgs
		});
		functionDefs.Add(Function.Avg, new TypeClass[2]
		{
			TypeClass.Numeric,
			TypeClass.VarArgs
		});
		functionDefs.Add(Function.Count, new TypeClass[2]
		{
			TypeClass.Numeric,
			TypeClass.VarArgs
		});
		functionDefs.Add(Function.IsNull, new TypeClass[3]
		{
			TypeClass.Variant,
			TypeClass.Variant,
			TypeClass.Variant
		});
		functionDefs.Add(Function.Enum, new TypeClass[3]
		{
			TypeClass.Numeric,
			TypeClass.String,
			TypeClass.String
		});
		functionDefs.Add(Function.False, new TypeClass[1] { TypeClass.Bool });
		functionDefs.Add(Function.True, new TypeClass[1] { TypeClass.Bool });
		functionDefs.Add(Function.DateTime, new TypeClass[2]
		{
			TypeClass.DateTime,
			TypeClass.String
		});
		functionDefs.Add(Function.String, new TypeClass[2]
		{
			TypeClass.String,
			TypeClass.Variant
		});
		functionDefs.Add(Function.Guid, new TypeClass[2]
		{
			TypeClass.Guid,
			TypeClass.String
		});
		functionDefs.Add(Function.Upper, new TypeClass[2]
		{
			TypeClass.String,
			TypeClass.String
		});
		functionDefs.Add(Function.Lower, new TypeClass[2]
		{
			TypeClass.String,
			TypeClass.String
		});
		functionDefs.Add(Function.Concatenate, new TypeClass[3]
		{
			TypeClass.String,
			TypeClass.String,
			TypeClass.String
		});
		functionDefs.Add(Function.Escape, new TypeClass[4]
		{
			TypeClass.String,
			TypeClass.String,
			TypeClass.String,
			TypeClass.String
		});
	}

	protected override void SetProperties()
	{
		bool flag = FunctionType == Function.String && (base.ChildrenList[0].Type == ExpressionNodeType.Constant || base.ChildrenList[0].Type == ExpressionNodeType.Attribute);
		SetFilterNodeCompatible(FunctionType == Function.True || FunctionType == Function.False || FunctionType == Function.Enum || flag);
		if (mFunctionType == Function.ExecuteSql || mFunctionType == Function.ExecuteWql)
		{
			SetHasScript(value: true);
			return;
		}
		foreach (ExpressionNode enumerableChildren in base.EnumerableChildrenList)
		{
			if (enumerableChildren.HasScript)
			{
				SetHasScript(enumerableChildren.HasScript);
				break;
			}
		}
	}

	internal ExpressionNodeFunction()
	{
		SetNodeType(ExpressionNodeType.Function);
		mReturnType = TypeClass.Unsupported;
		base.ChildrenList = new List<ExpressionNode>();
	}

	public ExpressionNodeFunction(Function functionType, params ExpressionNode[] args)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("ExpressionNodeFunction", TraceEventType.Information);
		methodTraceContext.TraceParameters(functionType, args);
		if (args == null)
		{
			throw methodTraceContext.TraceThrow(new ArgumentNullException("args"));
		}
		int num = functionDefs[mFunctionType].Length - 1;
		SetNodeType(ExpressionNodeType.Function);
		mFunctionType = functionType;
		mReturnType = functionDefs[mFunctionType][0];
		base.ChildrenList = new List<ExpressionNode>(args.Length);
		if (mReturnType == TypeClass.Variant)
		{
			if (args.Length == 0)
			{
				throw methodTraceContext.TraceThrow(new FunctionWrongArgumentsNumberException(mFunctionType.ToString(), args.Length, num));
			}
			if (functionDefs[mFunctionType][1] == TypeClass.String)
			{
				if (args[0].Type != ExpressionNodeType.Constant || args[0].TypeClass != TypeClass.String)
				{
					throw methodTraceContext.TraceThrow(new FunctionWrongArgumentTypeException(mFunctionType.ToString(), args[0].TypeClass.ToString(), TypeClass.String.ToString()));
				}
				string value = (string)((ExpressionNodeConstant)args[0]).Value;
				mReturnType = (TypeClass)Enum.Parse(typeof(TypeClass), value, ignoreCase: true);
			}
		}
		if (num > 0 && args.Length != 0)
		{
			bool flag = functionDefs[mFunctionType][1] == TypeClass.VarArgs;
			if (!flag && args.Length != functionDefs[mFunctionType].Length - 1)
			{
				throw methodTraceContext.TraceThrow(new FunctionWrongArgumentsNumberException(mFunctionType.ToString(), args.Length, functionDefs[mFunctionType].Length - 1));
			}
			TypeClass typeClass = TypeClass.Unsupported;
			for (int i = 0; i < args.Length; i++)
			{
				ExpressionNode expressionNode = args[i];
				if (expressionNode == null)
				{
					throw methodTraceContext.TraceThrow(new ArgumentNullException());
				}
				if (flag || functionDefs[mFunctionType][i + 1] == TypeClass.Variant)
				{
					if (typeClass == TypeClass.Unsupported)
					{
						if (expressionNode.TypeClass != 0)
						{
							typeClass = expressionNode.TypeClass;
							if (mReturnType == TypeClass.Variant)
							{
								mReturnType = typeClass;
							}
						}
					}
					else if (expressionNode.TypeClass != 0 && expressionNode.TypeClass != typeClass)
					{
						throw methodTraceContext.TraceThrow(new FunctionWrongArgumentTypeException(mFunctionType.ToString(), expressionNode.TypeClass.ToString(), typeClass.ToString()));
					}
				}
				else if (expressionNode.TypeClass != 0 && expressionNode.TypeClass != functionDefs[mFunctionType][i + 1])
				{
					throw methodTraceContext.TraceThrow(new FunctionWrongArgumentTypeException(mFunctionType.ToString(), expressionNode.TypeClass.ToString(), functionDefs[mFunctionType][i + 1].ToString()));
				}
				Add(expressionNode);
			}
		}
		else if (num == 0 && args.Length != 0)
		{
			throw methodTraceContext.TraceThrow(new FunctionWrongArgumentsNumberException(mFunctionType.ToString(), args.Length, num));
		}
		SetTypeClass(mReturnType);
		SetProperties();
	}

	public override string ToString()
	{
		return FunctionType.ToString() + base.ToString();
	}

	[STraceConfiguration(SkipAutoTrace = true)]
	public override string ToStringForDisplay()
	{
		foreach (ExpressionNode item in EnumChildren())
		{
			if (item.Type != ExpressionNodeType.Constant)
			{
				return null;
			}
		}
		return FunctionType switch
		{
			Function.True => "True", 
			Function.False => "False", 
			Function.Enum => ((ExpressionNodeConstant)base.ChildrenList[1]).Value.ToString(), 
			Function.DateTime => ((DateTime)Evaluate(null, checkSqlScriptAsProxy: false)).ToString("o", CultureInfo.CurrentCulture), 
			Function.Guid => ((ExpressionNodeConstant)base.ChildrenList[0]).Value.ToString(), 
			_ => null, 
		};
	}

	protected override bool EqualProperties(object obj)
	{
		ExpressionNodeFunction expressionNodeFunction = (ExpressionNodeFunction)obj;
		if (FunctionType == expressionNodeFunction.FunctionType && base.TypeClass == expressionNodeFunction.TypeClass)
		{
			return base.EqualProperties((object)expressionNodeFunction);
		}
		return false;
	}

	internal override void ResetResult()
	{
		base.LastEvaluationResult = null;
		base.ResetResult();
	}

	private void GetParameters(FacetEvaluationContext context, Function funcType, bool checkSqlScriptAsProxy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("GetParameters");
		methodTraceContext.TraceParameters(context, funcType, checkSqlScriptAsProxy);
		if (mArgs.Count > 0)
		{
			mArgs.Clear();
		}
		if (functionDefs[funcType].Length == 1 || functionDefs[funcType][1] != TypeClass.VarArgs)
		{
			if (base.Count != functionDefs[funcType].Length - 1)
			{
				throw methodTraceContext.TraceThrow(new FunctionWrongArgumentsNumberException(funcType.ToString(), base.Count, functionDefs[funcType].Length - 1));
			}
			for (int i = 1; i < functionDefs[funcType].Length; i++)
			{
				mArgs.Add(base.ChildrenList[i - 1].DoEvaluate(context, checkSqlScriptAsProxy));
				if (mArgs[i - 1] != null && functionDefs[funcType][i] != TypeClass.Variant && EvaluationFactory.ClassifyType(mArgs[i - 1]) != functionDefs[funcType][i])
				{
					throw methodTraceContext.TraceThrow(new FunctionWrongArgumentTypeException(funcType.ToString(), EvaluationFactory.ClassifyType(mArgs[i - 1].GetType()).ToString(), functionDefs[funcType][i].ToString()));
				}
			}
			return;
		}
		TypeClass typeClass = TypeClass.Unsupported;
		for (int j = 0; j < base.ChildrenList.Count; j++)
		{
			object obj = base.ChildrenList[j].DoEvaluate(context, checkSqlScriptAsProxy);
			mArgs.Add(obj);
			if (obj != null)
			{
				if (typeClass == TypeClass.Unsupported)
				{
					typeClass = EvaluationFactory.ClassifyType(obj);
				}
				if (typeClass != 0 && typeClass != EvaluationFactory.ClassifyType(obj))
				{
					throw methodTraceContext.TraceThrow(new FunctionWrongArgumentTypeException(funcType.ToString(), EvaluationFactory.ClassifyType(obj).ToString(), typeClass.ToString()));
				}
			}
		}
	}

	internal override object DoEvaluate(FacetEvaluationContext context, bool checkSqlScriptAsProxy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("DoEvaluate");
		methodTraceContext.TraceParameters(context, checkSqlScriptAsProxy);
		mcontext = context;
		object obj = null;
		GetParameters(mcontext, mFunctionType, checkSqlScriptAsProxy);
		obj = (base.LastEvaluationResult = mFunctionType switch
		{
			Function.ExecuteSql => EvaluateExecuteSqlScalar(checkSqlScriptAsProxy), 
			Function.ExecuteWql => EvaluateExecuteWqlScalar(), 
			Function.GetDate => EvaluateGetDate(), 
			Function.DateAdd => EvaluateDateAdd(), 
			Function.DatePart => EvaluateDatePart(), 
			Function.Len => EvaluateLen(), 
			Function.Add => EvaluateAdd(), 
			Function.Subtract => EvaluateSubtract(), 
			Function.Multiply => EvaluateMultiply(), 
			Function.Divide => EvaluateDivide(), 
			Function.BitwiseAnd => EvaluateBitwiseAnd(), 
			Function.BitwiseOr => EvaluateBitwiseOr(), 
			Function.Power => EvaluatePower(), 
			Function.Mod => EvaluateModulus(), 
			Function.Round => EvaluateRound(), 
			Function.Array => EvaluateArray(), 
			Function.Sum => EvaluateSum(), 
			Function.Avg => EvaluateAvg(), 
			Function.Count => EvaluateCount(), 
			Function.IsNull => EvaluateIsNull(), 
			Function.Enum => EvaluateEnum(), 
			Function.False => EvaluateFalse(), 
			Function.True => EvaluateTrue(), 
			Function.DateTime => EvaluateDateTime(), 
			Function.String => EvaluateString(), 
			Function.Guid => EvaluateGuid(), 
			Function.Lower => EvaluateLower(), 
			Function.Upper => EvaluateUpper(), 
			Function.Concatenate => EvaluateConcatenate(), 
			Function.Escape => EvaluateEscape(), 
			_ => throw methodTraceContext.TraceThrow(new UnsupportedTypeException(typeof(Function).Name, mFunctionType.ToString())), 
		});
		methodTraceContext.TraceParameterOut("returnVal", obj);
		return obj;
	}

	private object EvaluateExecuteSqlScalar(bool checkSqlScriptAsProxy)
	{
		using MethodTraceContext methodTraceContext = traceContext.GetMethodContext("EvaluateExecuteSqlScalar");
		methodTraceContext.TraceParameters(checkSqlScriptAsProxy);
		if (!(mcontext.PhysicalTarget is SqlSmoObject sqlSmoObject))
		{
			throw methodTraceContext.TraceThrow(new FunctionNotASmoObjectException(mcontext.Target.GetType().Name));
		}
		Microsoft.SqlServer.Management.Smo.Server serverObj = DmfSqlSmoObjectProxy.GetServerObj(sqlSmoObject);
		if (serverObj == null)
		{
			throw methodTraceContext.TraceThrow(new FunctionNoServerException());
		}
		SqlConnection sqlConnectionObject = serverObj.ConnectionContext.SqlConnectionObject;
		bool flag = false;
		if (sqlConnectionObject.State != ConnectionState.Open)
		{
			sqlConnectionObject.Open();
			flag = true;
		}
		object obj = null;
		try
		{
			string text = Evaluator.ConvertToString(mArgs[1]);
			text = text.Replace("''", "'");
			text = SfcTsqlProcFormatter.MakeSqlString(text);
			string text2 = null;
			if (sqlSmoObject is ScriptSchemaObjectBase scriptSchemaObjectBase)
			{
				text2 = ", N'@@ObjectName sysname, @@SchemaName sysname', @@ObjectName = " + SfcTsqlProcFormatter.MakeSqlString(scriptSchemaObjectBase.Name) + ", @@SchemaName = " + SfcTsqlProcFormatter.MakeSqlString(scriptSchemaObjectBase.Schema);
			}
			string text3;
			if (int.Parse(sqlConnectionObject.ServerVersion.Split(new char[1] { '.' }, 2)[0]) <= 8)
			{
				if (text.Length > 4000)
				{
					traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.CantExecuteLongSqlScriptOn2000));
				}
				text3 = "DECLARE @@UserScript nvarchar(4000); ";
			}
			else
			{
				text3 = "DECLARE @@UserScript nvarchar(max); ";
			}
			string text4 = text3 + "SET @@UserScript = " + text + "; EXEC sp_executesql @@UserScript" + text2 + "; ";
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.CommandTimeout = 300;
			sqlCommand.Connection = sqlConnectionObject;
			if (checkSqlScriptAsProxy)
			{
				string commandText = "DECLARE @cookie varbinary(100);\r\n                                  EXECUTE AS LOGIN = N'##MS_PolicyTsqlExecutionLogin##' WITH COOKIE INTO @cookie;\r\n                                  BEGIN TRY " + text4 + "   REVERT WITH COOKIE = @cookie; \r\n                                  END TRY\r\n                                  BEGIN CATCH \r\n                                     REVERT WITH COOKIE = @cookie; \r\n                                     DECLARE @ErrorMessage   NVARCHAR(4000);\r\n                                     DECLARE @ErrorSeverity  INT;\r\n                                     DECLARE @ErrorState     INT;\r\n                                     DECLARE @ErrorNumber    INT;\r\n                                     DECLARE @ErrorLine      INT;\r\n                                     DECLARE @ErrorProcedure NVARCHAR(200);\r\n                                     SELECT @ErrorLine = ERROR_LINE(),\r\n                                            @ErrorSeverity = ERROR_SEVERITY(),\r\n                                            @ErrorState = ERROR_STATE(),\r\n                                            @ErrorNumber = ERROR_NUMBER(),\r\n                                            @ErrorMessage = ERROR_MESSAGE(),\r\n                                            @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');\r\n                                     RAISERROR (14684, @ErrorSeverity, -1 , @ErrorNumber, @ErrorSeverity, @ErrorState, @ErrorProcedure, @ErrorLine, @ErrorMessage);\r\n                                 END CATCH";
				sqlCommand.CommandText = commandText;
			}
			else
			{
				sqlCommand.CommandText = text4;
			}
			if (!string.IsNullOrEmpty(sqlSmoObject.GetDBName()))
			{
				sqlCommand.CommandText = $"use {SfcTsqlProcFormatter.MakeSqlBracket(sqlSmoObject.GetDBName())}; " + sqlCommand.CommandText;
			}
			obj = sqlCommand.ExecuteScalar();
			if (obj == null)
			{
				obj = DBNull.Value;
			}
			mReturnType = (TypeClass)Enum.Parse(typeof(TypeClass), Evaluator.ConvertToString(mArgs[0]), ignoreCase: true);
			if (!(obj is DBNull) && mReturnType != 0)
			{
				obj = Evaluator.ConvertToAny(obj, mReturnType);
			}
		}
		finally
		{
			if (flag)
			{
				sqlConnectionObject.Close();
			}
		}
		methodTraceContext.TraceParameterOut("returnVal", obj);
		return obj;
	}

	private object EvaluateExecuteWqlScalar()
	{
		string empty = string.Empty;
		int index = 0;
		int index2 = 1;
		int index3 = 2;
		string assemblyString = "System.Management, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
		object obj = null;
		string text = Evaluator.ConvertToString(mArgs[index2]);
		string text2 = Evaluator.ConvertToString(mArgs[index3]);
		text2 = text2.Replace("''", "'");
		if (!text.StartsWith("\\\\", StringComparison.Ordinal))
		{
			if (mcontext.Target is IDmfObjectInfo)
			{
				empty = (mcontext.Target as IDmfObjectInfo).RootPath;
			}
			else if (mcontext.Target is SqlSmoObject)
			{
				Microsoft.SqlServer.Management.Smo.Server serverObj = DmfSqlSmoObjectProxy.GetServerObj(mcontext.Target as SqlSmoObject);
				if (serverObj == null)
				{
					throw new FunctionNoServerException();
				}
				empty = serverObj.Information.NetName;
			}
			else
			{
				if (!(mcontext.Target is IDmfAdapter) || mcontext.PhysicalTarget == null || !(mcontext.PhysicalTarget is SqlSmoObject))
				{
					throw new FunctionNotASmoObjectException(mcontext.Target.GetType().Name);
				}
				Microsoft.SqlServer.Management.Smo.Server serverObj2 = DmfSqlSmoObjectProxy.GetServerObj(mcontext.PhysicalTarget as SqlSmoObject);
				if (serverObj2 == null)
				{
					throw new FunctionNoServerException();
				}
				empty = serverObj2.Information.NetName;
			}
			if (!Environment.MachineName.Equals(empty, StringComparison.OrdinalIgnoreCase))
			{
				text = "\\\\" + empty + (text.StartsWith("\\", StringComparison.Ordinal) ? string.Empty : "\\") + text;
			}
		}
		try
		{
			Assembly assembly = Assembly.Load(assemblyString);
			traceContext.DebugAssert(assembly != null, "Unable to find 'System.Management' assembly");
			Type type = assembly.GetType("System.Management.ManagementObjectSearcher", throwOnError: true, ignoreCase: true);
			traceContext.DebugAssert(type != null, "Unable to find 'System.Management.ManagementObjectSearcher' type");
			Type[] types = new Type[0];
			MethodInfo method = type.GetMethod("Get", types);
			traceContext.DebugAssert(method != null, "Unable to find 'ManagementObjectSearcher.Get' method");
			object obj2 = Activator.CreateInstance(type, text, text2);
			traceContext.DebugAssert(obj2 != null, "Unable to create 'System.Management.ManagementObjectSearcher' object");
			IEnumerable enumerable = (IEnumerable)method.Invoke(obj2, null);
			traceContext.DebugAssert(enumerable != null, "Unable to invoke 'ManagementObjectSearcher.Get' method");
			IEnumerator enumerator = enumerable.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					ICollection obj3 = (ICollection)current.GetType().GetProperty("Properties").GetValue(current, null);
					if (obj3.Count > 1)
					{
						throw traceContext.TraceThrow(new FunctionTooManyColumnsException());
					}
					{
						IEnumerator enumerator2 = obj3.GetEnumerator();
						try
						{
							if (enumerator2.MoveNext())
							{
								object current2 = enumerator2.Current;
								PropertyInfo property = current2.GetType().GetProperty("Value");
								traceContext.DebugAssert(property != null, "Unable to find 'ManagementObject' Value property");
								obj = property.GetValue(current2, null);
							}
						}
						finally
						{
							IDisposable disposable2 = enumerator2 as IDisposable;
							if (disposable2 != null)
							{
								disposable2.Dispose();
							}
						}
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			mReturnType = (TypeClass)Enum.Parse(typeof(TypeClass), Evaluator.ConvertToString(mArgs[index]), ignoreCase: true);
			if (obj != null && mReturnType != 0)
			{
				obj = Evaluator.ConvertToAny(obj, mReturnType);
			}
		}
		catch (Exception ex)
		{
			traceContext.TraceCatch(ex);
			if (ex is FunctionTooManyColumnsException)
			{
				throw;
			}
			throw traceContext.TraceThrow(new DmfException(ExceptionTemplatesSR.WmiException(ex.Message), ex));
		}
		return obj;
	}

	private object EvaluateGetDate()
	{
		return DateTime.Now;
	}

	private object EvaluateDateAdd()
	{
		int index = 0;
		int index2 = 1;
		int index3 = 2;
		object obj = null;
		string text = Evaluator.ConvertToString(mArgs[index]);
		int num = Convert.ToInt32(Evaluator.ConvertToLong(mArgs[index2]));
		DateTime dateTime = Evaluator.ConvertToDateTime(mArgs[index3]);
		switch (text.ToUpperInvariant())
		{
		case "YEAR":
		case "YY":
		case "YYYY":
			return dateTime.AddYears(num);
		case "MONTH":
		case "MM":
		case "M":
			return dateTime.AddMonths(num);
		case "DAYOFYEAR":
		case "DY":
		case "Y":
		case "DAY":
		case "DD":
		case "D":
		case "WEEKDAY":
		case "DW":
			return dateTime.AddDays(num);
		case "HOUR":
		case "HH":
			return dateTime.AddHours(num);
		case "MINUTE":
		case "MI":
		case "N":
			return dateTime.AddMinutes(num);
		case "SECOND":
		case "SS":
		case "S":
			return dateTime.AddSeconds(num);
		case "MILLISECOND":
		case "MS":
			return dateTime.AddMilliseconds(num);
		default:
			throw traceContext.TraceThrow(new FunctionBadDatePartException());
		}
	}

	private object EvaluateDatePart()
	{
		int index = 0;
		int index2 = 1;
		object obj = null;
		string text = Evaluator.ConvertToString(mArgs[index]);
		DateTime dateTime = Evaluator.ConvertToDateTime(mArgs[index2]);
		switch (text.ToUpperInvariant())
		{
		case "YEAR":
		case "YY":
		case "YYYY":
			return dateTime.Year;
		case "MONTH":
		case "MM":
		case "M":
			return dateTime.Month;
		case "DAYOFYEAR":
		case "DY":
		case "Y":
			return dateTime.DayOfYear;
		case "DAY":
		case "DD":
		case "D":
			return dateTime.Day;
		case "WEEKDAY":
		case "DW":
			return (int)(dateTime.DayOfWeek + 1);
		case "HOUR":
		case "HH":
			return dateTime.Hour;
		case "MINUTE":
		case "MI":
		case "N":
			return dateTime.Minute;
		case "SECOND":
		case "SS":
		case "S":
			return dateTime.Second;
		case "MILLISECOND":
		case "MS":
			return dateTime.Millisecond;
		default:
			throw traceContext.TraceThrow(new FunctionBadDatePartException());
		}
	}

	private object EvaluateLen()
	{
		int index = 0;
		return Evaluator.ConvertToString(mArgs[index]).Length;
	}

	private object EvaluateUpper()
	{
		int index = 0;
		_ = string.Empty;
		string text = Evaluator.ConvertToString(mArgs[index]);
		SqlSmoObject sqlSmoObject = mcontext.PhysicalTarget as SqlSmoObject;
		CultureInfo culture = CultureInfo.InvariantCulture;
		if (sqlSmoObject != null)
		{
			culture = sqlSmoObject.StringComparer.CultureInfo;
		}
		return text.ToUpper(culture);
	}

	private object EvaluateLower()
	{
		int index = 0;
		_ = string.Empty;
		string text = Evaluator.ConvertToString(mArgs[index]);
		SqlSmoObject sqlSmoObject = mcontext.PhysicalTarget as SqlSmoObject;
		CultureInfo culture = CultureInfo.InvariantCulture;
		if (sqlSmoObject != null)
		{
			culture = sqlSmoObject.StringComparer.CultureInfo;
		}
		return text.ToLower(culture);
	}

	private object EvaluateAdd()
	{
		int index = 0;
		int index2 = 1;
		double num = Evaluator.ConvertToDouble(mArgs[index]);
		double num2 = Evaluator.ConvertToDouble(mArgs[index2]);
		return num + num2;
	}

	private object EvaluateSubtract()
	{
		int index = 0;
		int index2 = 1;
		double num = Evaluator.ConvertToDouble(mArgs[index]);
		double num2 = Evaluator.ConvertToDouble(mArgs[index2]);
		return num - num2;
	}

	private object EvaluateMultiply()
	{
		int index = 0;
		int index2 = 1;
		double num = Evaluator.ConvertToDouble(mArgs[index]);
		double num2 = Evaluator.ConvertToDouble(mArgs[index2]);
		return num * num2;
	}

	private object EvaluateDivide()
	{
		int index = 0;
		int index2 = 1;
		double num = Evaluator.ConvertToDouble(mArgs[index]);
		double num2 = Evaluator.ConvertToDouble(mArgs[index2]);
		return num / num2;
	}

	private object EvaluatePower()
	{
		int index = 0;
		int index2 = 1;
		double x = Evaluator.ConvertToDouble(mArgs[index]);
		double y = Evaluator.ConvertToDouble(mArgs[index2]);
		return Math.Pow(x, y);
	}

	private object EvaluateModulus()
	{
		int index = 0;
		int index2 = 1;
		double num = Evaluator.ConvertToDouble(mArgs[index]);
		double num2 = Evaluator.ConvertToDouble(mArgs[index2]);
		return num % num2;
	}

	private object EvaluateRound()
	{
		int index = 0;
		int index2 = 1;
		double value = Evaluator.ConvertToDouble(mArgs[index]);
		int digits = Convert.ToInt32(Evaluator.ConvertToDouble(mArgs[index2]));
		return Math.Round(value, digits);
	}

	private object EvaluateBitwiseAnd()
	{
		int index = 0;
		int index2 = 1;
		long num = Evaluator.ConvertToLong(mArgs[index]);
		long num2 = Evaluator.ConvertToLong(mArgs[index2]);
		return num & num2;
	}

	private object EvaluateBitwiseOr()
	{
		int index = 0;
		int index2 = 1;
		long num = Evaluator.ConvertToLong(mArgs[index]);
		long num2 = Evaluator.ConvertToLong(mArgs[index2]);
		return num | num2;
	}

	private object[] EvaluateArray()
	{
		object[] array = new object[mArgs.Count];
		for (int i = 0; i < mArgs.Count; i++)
		{
			array[i] = mArgs[i];
		}
		return array;
	}

	private double EvaluateSum()
	{
		double num = 0.0;
		foreach (object mArg in mArgs)
		{
			num += Evaluator.ConvertToDouble(mArg);
		}
		return num;
	}

	private double EvaluateAvg()
	{
		double num = 0.0;
		foreach (object mArg in mArgs)
		{
			num += Evaluator.ConvertToDouble(mArg);
		}
		return num / (double)mArgs.Count;
	}

	private double EvaluateCount()
	{
		return mArgs.Count;
	}

	private object EvaluateIsNull()
	{
		int index = 0;
		int index2 = 1;
		if (mArgs[index] == null || mArgs[index] is DBNull)
		{
			return mArgs[index2];
		}
		return mArgs[index];
	}

	private object EvaluateEnum()
	{
		int index = 0;
		int index2 = 1;
		object obj = ExpressionNode.ResolveEnum(Evaluator.ConvertToString(mArgs[index]), Evaluator.ConvertToString(mArgs[index2]));
		if (obj == null)
		{
			throw traceContext.TraceThrow(new UnsupportedObjectTypeException(Evaluator.ConvertToString(mArgs[index]), GetType().ToString()));
		}
		return obj;
	}

	private object EvaluateTrue()
	{
		return true;
	}

	private object EvaluateFalse()
	{
		return false;
	}

	private object EvaluateDateTime()
	{
		int index = 0;
		return Convert.ToDateTime(Evaluator.ConvertToString(mArgs[index]), CultureInfo.InvariantCulture);
	}

	private object EvaluateString()
	{
		int index = 0;
		return Evaluator.ConvertToString(mArgs[index]);
	}

	private object EvaluateConcatenate()
	{
		int index = 0;
		int index2 = 1;
		string text = Evaluator.ConvertToString(mArgs[index]);
		string text2 = Evaluator.ConvertToString(mArgs[index2]);
		return text + text2;
	}

	private object EvaluateEscape()
	{
		int index = 0;
		int index2 = 1;
		int index3 = 2;
		string text = Evaluator.ConvertToString(mArgs[index]);
		string text2 = Evaluator.ConvertToString(mArgs[index2]);
		string text3 = Evaluator.ConvertToString(mArgs[index3]);
		if (string.IsNullOrEmpty(text2))
		{
			throw new ArgumentException(ExceptionTemplatesSR.ArgumentNullOrEmpty("StringToEscape"));
		}
		return text.Replace(text2, text3 + text2);
	}

	private object EvaluateGuid()
	{
		int index = 0;
		return new Guid(Evaluator.ConvertToString(mArgs[index]));
	}

	protected override void SerializeProperties(XmlWriter xw, bool includeResult)
	{
		xw.WriteElementString("FunctionType", mFunctionType.ToString());
		xw.WriteElementString("ReturnType", mReturnType.ToString());
		if (includeResult)
		{
			SerializeResult(xw);
		}
		base.SerializeProperties(xw, includeResult);
	}

	protected override void DeserializeProperties(XmlReader xr, bool includeResult)
	{
		Function function = ExpressionNode.MatchType<Function>(ExpressionNode.ReadNodeWithCheck(xr, "FunctionType")[0]);
		mFunctionType = function;
		List<string> list = ExpressionNode.ReadNodeWithCheck(xr, "ReturnType");
		mReturnType = (TypeClass)Enum.Parse(typeof(TypeClass), list[0]);
		if (includeResult)
		{
			DeserializeResult(xr);
		}
		base.DeserializeProperties(xr, includeResult);
		SetProperties();
	}

	protected override FilterNode DoConvertToFilterNode()
	{
		FilterNodeFunction.Type funcType = FilterNodeFunction.Type.UserDefined;
		string name = FunctionType.ToString();
		switch (FunctionType)
		{
		case Function.True:
			name = "true";
			funcType = FilterNodeFunction.Type.True;
			break;
		case Function.False:
			name = "false";
			funcType = FilterNodeFunction.Type.False;
			break;
		case Function.String:
			name = "string";
			funcType = FilterNodeFunction.Type.String;
			break;
		case Function.Enum:
		{
			int index = 0;
			int index2 = 1;
			return new FilterNodeConstant(Convert.ToInt32(ExpressionNode.ResolveEnum(((ExpressionNodeConstant)base.ChildrenList[index]).Value.ToString(), ((ExpressionNodeConstant)base.ChildrenList[index2]).Value.ToString())), FilterNodeConstant.ObjectType.Number);
		}
		}
		int count = base.ChildrenList.Count;
		FilterNode[] array = new FilterNode[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = base.ChildrenList[i].ConvertToFilterNode();
		}
		return new FilterNodeFunction(funcType, name, array);
	}

	public override ExpressionNode DeepClone()
	{
		ExpressionNodeFunction expressionNodeFunction = new ExpressionNodeFunction();
		expressionNodeFunction.mFunctionType = FunctionType;
		expressionNodeFunction.mReturnType = ReturnType;
		expressionNodeFunction.LastEvaluationResult = base.LastEvaluationResult;
		expressionNodeFunction.Tag = base.Tag;
		expressionNodeFunction.SetTypeClass(base.TypeClass);
		foreach (ExpressionNode item in EnumChildren())
		{
			expressionNodeFunction.Add(item.DeepClone());
		}
		return expressionNodeFunction;
	}
}
