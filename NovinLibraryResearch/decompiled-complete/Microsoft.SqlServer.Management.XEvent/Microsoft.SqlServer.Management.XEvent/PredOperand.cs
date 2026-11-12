using System;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class PredOperand : Predicate
{
	internal string name;

	private readonly Guid typePackageID;

	private readonly string typeName;

	internal Package package;

	internal object operandObject;

	public string TypeName => typeName;

	public Guid TypePackageId => typePackageID;

	public object OperandObject => operandObject;

	public PredOperand(DataEventColumnInfo eventColumn)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("PredOperand:Event column");
		methodTraceContext.TraceParameterIn("event column", eventColumn);
		if (eventColumn == null)
		{
			methodTraceContext.TraceError("event column can not be null.");
			throw new ArgumentNullException("eventColumn");
		}
		name = eventColumn.Name;
		typePackageID = eventColumn.TypePackageID;
		typeName = eventColumn.TypeName;
		package = null;
		operandObject = eventColumn;
	}

	public PredOperand(PredSourceInfo sourceInfo)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("PredOperand:Pred Source");
		methodTraceContext.TraceParameterIn("pred source", sourceInfo);
		if (sourceInfo == null)
		{
			methodTraceContext.TraceError("pred source can not be null.");
			throw new ArgumentNullException("sourceInfo");
		}
		name = sourceInfo.Name;
		typePackageID = sourceInfo.TypePackageID;
		typeName = sourceInfo.TypeName;
		package = sourceInfo.Parent;
		operandObject = sourceInfo;
	}

	public override string ToString()
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("ToString");
		if (package == null)
		{
			return SfcTsqlProcFormatter.MakeSqlBracket(name);
		}
		if (operandObject is PredSourceInfo)
		{
			if (package.Parent.ObjectInfoSet.GetAll<PredSourceInfo>(package.Name, name).Count > 1)
			{
				return string.Format(CultureInfo.InvariantCulture, "[{0}].{1}.{2}", package.ModuleID, SfcTsqlProcFormatter.MakeSqlBracket(package.Name), SfcTsqlProcFormatter.MakeSqlBracket(name));
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", SfcTsqlProcFormatter.MakeSqlBracket(package.Name), SfcTsqlProcFormatter.MakeSqlBracket(name));
		}
		methodTraceContext.Assert(false, "unexpected operand object.");
		return null;
	}
}
