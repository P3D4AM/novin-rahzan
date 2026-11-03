using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Stimulsoft.Report.CodeDom;

public class StiCSharpCodeGenerator : StiCodeGenerator
{
	private const GeneratorSupport LanguageSupport = 1048575;

	private static Hashtable keywordsHashtable;

	private static readonly string[] keywords;

	private bool forLoopHack;

	protected override string NullToken => "null";

	public static bool MaximizeStringLength { get; set; }

	protected string QuoteSnippetStringCStyle(string value)
	{
		int num = value.Length / 100;
		if (num < 1024)
		{
			num = 1024;
		}
		if (num > 2796198)
		{
			num = 2796198;
		}
		if (MaximizeStringLength)
		{
			double num2 = (double)value.Length / 2796198.0;
			num2 = ((!(num2 <= 1.0)) ? Math.Ceiling(num2) : 1.0);
			num = (int)(2796198.0 / num2);
		}
		StringBuilder stringBuilder = new StringBuilder(value.Length + 5);
		stringBuilder.Append("\"");
		for (int i = 0; i < value.Length; i++)
		{
			switch (value[i])
			{
			case '\u2028':
			case '\u2029':
				stringBuilder.Append("\\u");
				stringBuilder.Append(((int)value[i]).ToString("X4", CultureInfo.InvariantCulture));
				break;
			case '\r':
				stringBuilder.Append("\\r");
				break;
			case '\t':
				stringBuilder.Append("\\t");
				break;
			case '"':
				stringBuilder.Append("\\\"");
				break;
			case '\'':
				stringBuilder.Append("\\'");
				break;
			case '\\':
				stringBuilder.Append("\\\\");
				break;
			case '\0':
				stringBuilder.Append("\\0");
				break;
			case '\n':
				stringBuilder.Append("\\n");
				break;
			default:
				stringBuilder.Append(value[i]);
				break;
			}
			if (i > 0 && i % num == 0)
			{
				if (char.IsHighSurrogate(value[i]) && i < value.Length - 1 && char.IsLowSurrogate(value[i + 1]))
				{
					stringBuilder.Append(value[++i]);
				}
				stringBuilder.Append("\" +\r\n\"");
			}
		}
		stringBuilder.Append("\"");
		return stringBuilder.ToString();
	}

	public override string QuoteSnippetString(string value)
	{
		if (value == null)
		{
			return "";
		}
		return QuoteSnippetStringCStyle(value);
	}

	protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
	{
		base.Output.Write("new ");
		CodeExpressionCollection initializers = e.Initializers;
		if (((CollectionBase)(object)initializers).Count > 0)
		{
			OutputType(e.CreateType);
			if (e.CreateType.ArrayRank == 0)
			{
				base.Output.Write("[]");
			}
			base.Output.WriteLine(" {");
			base.Indent++;
			OutputExpressionList(initializers, newlineBetweenItems: true);
			base.Indent--;
			base.Output.Write("}");
		}
		else
		{
			base.Output.Write(GetBaseTypeOutput(e.CreateType.BaseType));
			base.Output.Write("[");
			if (e.SizeExpression != null)
			{
				GenerateExpression(e.SizeExpression);
			}
			else
			{
				base.Output.Write(e.Size);
			}
			base.Output.Write("]");
		}
	}

	protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
	{
		base.Output.Write("base");
	}

	protected override void GenerateCastExpression(CodeCastExpression e)
	{
		base.Output.Write("((");
		OutputType(e.TargetType);
		base.Output.Write(")(");
		GenerateExpression(e.Expression);
		base.Output.Write("))");
	}

	protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
	{
		base.Output.Write("new ");
		OutputType(e.DelegateType);
		base.Output.Write("(");
		GenerateExpression(e.TargetObject);
		base.Output.Write(".");
		OutputIdentifier(e.MethodName);
		base.Output.Write(")");
	}

	protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
	{
		if (e.TargetObject != null)
		{
			GenerateExpression(e.TargetObject);
			base.Output.Write(".");
		}
		OutputIdentifier(e.FieldName);
	}

	protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
	{
		OutputIdentifier(e.ParameterName);
	}

	protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
	{
		OutputIdentifier(e.VariableName);
	}

	protected override void GenerateIndexerExpression(CodeIndexerExpression e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		GenerateExpression(e.TargetObject);
		base.Output.Write("[");
		bool flag = true;
		foreach (CodeExpression item in (CollectionBase)(object)e.Indices)
		{
			CodeExpression e2 = item;
			if (flag)
			{
				flag = false;
			}
			else
			{
				base.Output.Write(", ");
			}
			GenerateExpression(e2);
		}
		base.Output.Write("]");
	}

	protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		GenerateExpression(e.TargetObject);
		base.Output.Write("[");
		bool flag = true;
		foreach (CodeExpression item in (CollectionBase)(object)e.Indices)
		{
			CodeExpression e2 = item;
			if (flag)
			{
				flag = false;
			}
			else
			{
				base.Output.Write(", ");
			}
			GenerateExpression(e2);
		}
		base.Output.Write("]");
	}

	protected override void GenerateSnippetExpression(CodeSnippetExpression e)
	{
		base.Output.Write(e.Value);
	}

	protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
	{
		GenerateMethodReferenceExpression(e.Method);
		base.Output.Write("(");
		OutputExpressionList(e.Parameters);
		base.Output.Write(")");
	}

	protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
	{
		if (e.TargetObject != null)
		{
			if (e.TargetObject is CodeBinaryOperatorExpression)
			{
				base.Output.Write("(");
				GenerateExpression(e.TargetObject);
				base.Output.Write(")");
			}
			else
			{
				GenerateExpression(e.TargetObject);
			}
			base.Output.Write(".");
		}
		OutputIdentifier(e.MethodName);
	}

	protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
	{
		if (e.TargetObject != null)
		{
			GenerateExpression(e.TargetObject);
			base.Output.Write(".");
		}
		OutputIdentifier(e.EventName);
	}

	protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
	{
		if (e.TargetObject != null)
		{
			GenerateExpression(e.TargetObject);
		}
		base.Output.Write("(");
		OutputExpressionList(e.Parameters);
		base.Output.Write(")");
	}

	protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
	{
		base.Output.Write("new ");
		OutputType(e.CreateType);
		base.Output.Write("(");
		OutputExpressionList(e.Parameters);
		base.Output.Write(")");
	}

	protected override void GeneratePrimitiveExpression(CodePrimitiveExpression e)
	{
		if (e.Value is char)
		{
			GeneratePrimitiveChar((char)e.Value);
		}
		else
		{
			base.GeneratePrimitiveExpression(e);
		}
	}

	protected override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
	{
		base.Output.Write("value");
	}

	protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
	{
		base.Output.Write("this");
	}

	protected override void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (((CollectionBase)(object)e.CustomAttributes).Count > 0)
		{
			GenerateAttributes(e.CustomAttributes, null, inLine: true);
		}
		OutputDirection(e.Direction);
		OutputTypeNamePair(e.Type, e.Name);
	}

	protected override void GenerateExpressionStatement(CodeExpressionStatement e)
	{
		GenerateExpression(e.Expression);
		if (!forLoopHack)
		{
			base.Output.WriteLine(";");
		}
	}

	protected override void GenerateIterationStatement(CodeIterationStatement e)
	{
		forLoopHack = true;
		base.Output.Write("for (");
		GenerateStatement(e.InitStatement);
		base.Output.Write("; ");
		GenerateExpression(e.TestExpression);
		base.Output.Write("; ");
		GenerateStatement(e.IncrementStatement);
		base.Output.Write(")");
		OutputStartingBrace();
		forLoopHack = false;
		base.Indent++;
		GenerateStatements(e.Statements);
		base.Indent--;
		base.Output.WriteLine("}");
	}

	protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
	{
		base.Output.Write("throw");
		if (e.ToThrow != null)
		{
			base.Output.Write(" ");
			GenerateExpression(e.ToThrow);
		}
		base.Output.WriteLine(";");
	}

	protected override void GenerateComment(CodeComment e)
	{
		string value = ConvertToCommentEscapeCodes(e.Text);
		if (e.DocComment)
		{
			base.Output.Write("/// ");
		}
		else
		{
			base.Output.Write("// ");
		}
		base.Output.WriteLine(value);
	}

	protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
	{
		base.Output.Write("return");
		if (e.Expression != null)
		{
			base.Output.Write(" ");
			GenerateExpression(e.Expression);
		}
		base.Output.WriteLine(";");
	}

	protected override void GenerateConditionStatement(CodeConditionStatement e)
	{
		base.Output.Write("if (");
		GenerateExpression(e.Condition);
		base.Output.Write(")");
		OutputStartingBrace();
		base.Indent++;
		GenerateStatements(e.TrueStatements);
		base.Indent--;
		CodeStatementCollection falseStatements = e.FalseStatements;
		if (((CollectionBase)(object)falseStatements).Count > 0)
		{
			base.Output.Write("}");
			if (base.Options.ElseOnClosing)
			{
				base.Output.Write(" ");
			}
			else
			{
				base.Output.WriteLine("");
			}
			base.Output.Write("else");
			OutputStartingBrace();
			base.Indent++;
			GenerateStatements(e.FalseStatements);
			base.Indent--;
		}
		base.Output.WriteLine("}");
	}

	protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		base.Output.Write("try");
		OutputStartingBrace();
		base.Indent++;
		GenerateStatements(e.TryStatements);
		base.Indent--;
		CodeCatchClauseCollection catchClauses = e.CatchClauses;
		if (((CollectionBase)(object)catchClauses).Count > 0)
		{
			IEnumerator enumerator = ((CollectionBase)(object)catchClauses).GetEnumerator();
			while (enumerator.MoveNext())
			{
				base.Output.Write("}");
				if (base.Options.ElseOnClosing)
				{
					base.Output.Write(" ");
				}
				else
				{
					base.Output.WriteLine("");
				}
				CodeCatchClause val = (CodeCatchClause)enumerator.Current;
				base.Output.Write("catch (");
				OutputType(val.CatchExceptionType);
				base.Output.Write(" ");
				OutputIdentifier(val.LocalName);
				base.Output.Write(")");
				OutputStartingBrace();
				base.Indent++;
				GenerateStatements(val.Statements);
				base.Indent--;
			}
		}
		CodeStatementCollection finallyStatements = e.FinallyStatements;
		if (((CollectionBase)(object)finallyStatements).Count > 0)
		{
			base.Output.Write("}");
			if (base.Options.ElseOnClosing)
			{
				base.Output.Write(" ");
			}
			else
			{
				base.Output.WriteLine("");
			}
			base.Output.Write("finally");
			OutputStartingBrace();
			base.Indent++;
			GenerateStatements(finallyStatements);
			base.Indent--;
		}
		base.Output.WriteLine("}");
	}

	protected override void GenerateAssignStatement(CodeAssignStatement e)
	{
		GenerateExpression(e.Left);
		base.Output.Write(" = ");
		GenerateExpression(e.Right);
		if (!forLoopHack)
		{
			base.Output.WriteLine(";");
		}
	}

	protected override void GenerateAttachEventStatement(CodeAttachEventStatement e)
	{
		GenerateEventReferenceExpression(e.Event);
		base.Output.Write(" += ");
		GenerateExpression(e.Listener);
		base.Output.WriteLine(";");
	}

	protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
	{
		GenerateEventReferenceExpression(e.Event);
		base.Output.Write(" -= ");
		GenerateExpression(e.Listener);
		base.Output.WriteLine(";");
	}

	protected override void GenerateSnippetStatement(CodeSnippetStatement e)
	{
		base.Output.WriteLine(e.Value);
	}

	protected override void GenerateGotoStatement(CodeGotoStatement e)
	{
		base.Output.Write("goto ");
		base.Output.Write(e.Label);
		base.Output.WriteLine(";");
	}

	protected override void GenerateLabeledStatement(CodeLabeledStatement e)
	{
		base.Indent--;
		base.Output.Write(e.Label);
		base.Output.WriteLine(":");
		base.Indent++;
		if (e.Statement != null)
		{
			GenerateStatement(e.Statement);
		}
	}

	protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
	{
		OutputTypeNamePair(e.Type, e.Name);
		if (e.InitExpression != null)
		{
			base.Output.Write(" = ");
			GenerateExpression(e.InitExpression);
		}
		if (!forLoopHack)
		{
			base.Output.WriteLine(";");
		}
	}

	protected override void GenerateLinePragmaStart(CodeLinePragma e)
	{
		base.Output.WriteLine("");
		base.Output.Write("#line ");
		base.Output.Write(e.LineNumber);
		base.Output.Write(" \"");
		base.Output.Write(e.FileName);
		base.Output.Write("\"");
		base.Output.WriteLine("");
	}

	protected override void GenerateLinePragmaEnd(CodeLinePragma e)
	{
		base.Output.WriteLine();
		base.Output.WriteLine("#line default");
	}

	protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (!base.IsCurrentDelegate && !base.IsCurrentEnum)
		{
			if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
			{
				GenerateAttributes(((CodeTypeMember)e).CustomAttributes);
			}
			if (e.PrivateImplementationType == null)
			{
				OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
			}
			base.Output.Write("event ");
			string text = ((CodeTypeMember)e).Name;
			if (e.PrivateImplementationType != null)
			{
				text = e.PrivateImplementationType.BaseType + "." + text;
			}
			OutputTypeNamePair(e.Type, text);
			base.Output.WriteLine(";");
		}
	}

	protected override void GenerateField(CodeMemberField e)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsCurrentDelegate || base.IsCurrentInterface)
		{
			return;
		}
		if (base.IsCurrentEnum)
		{
			if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
			{
				GenerateAttributes(((CodeTypeMember)e).CustomAttributes);
			}
			OutputIdentifier(((CodeTypeMember)e).Name);
			if (e.InitExpression != null)
			{
				base.Output.Write(" = ");
				GenerateExpression(e.InitExpression);
			}
			base.Output.WriteLine(",");
			return;
		}
		if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
		{
			GenerateAttributes(((CodeTypeMember)e).CustomAttributes);
		}
		OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
		OutputVTableModifier(((CodeTypeMember)e).Attributes);
		OutputFieldScopeModifier(((CodeTypeMember)e).Attributes);
		OutputTypeNamePair(e.Type, ((CodeTypeMember)e).Name);
		if (e.InitExpression != null)
		{
			base.Output.Write(" = ");
			GenerateExpression(e.InitExpression);
		}
		base.Output.WriteLine(";");
	}

	protected override void GenerateSnippetMember(CodeSnippetTypeMember e)
	{
		base.Output.Write(e.Text);
	}

	protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
	{
		base.Output.Write("public static void Main()");
		OutputStartingBrace();
		base.Indent++;
		GenerateStatements(((CodeMemberMethod)e).Statements);
		base.Indent--;
		base.Output.WriteLine("}");
	}

	protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Invalid comparison between Unknown and I4
		if (!base.IsCurrentClass && !base.IsCurrentStruct && !base.IsCurrentInterface)
		{
			return;
		}
		if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
		{
			GenerateAttributes(((CodeTypeMember)e).CustomAttributes);
		}
		if (((CollectionBase)(object)e.ReturnTypeCustomAttributes).Count > 0)
		{
			GenerateAttributes(e.ReturnTypeCustomAttributes, "return: ");
		}
		if (!base.IsCurrentInterface)
		{
			if (e.PrivateImplementationType == null)
			{
				OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
				OutputVTableModifier(((CodeTypeMember)e).Attributes);
				OutputMemberScopeModifier(((CodeTypeMember)e).Attributes);
			}
		}
		else
		{
			OutputVTableModifier(((CodeTypeMember)e).Attributes);
		}
		OutputType(e.ReturnType);
		base.Output.Write(" ");
		if (e.PrivateImplementationType != null)
		{
			base.Output.Write(e.PrivateImplementationType.BaseType);
			base.Output.Write(".");
		}
		OutputIdentifier(((CodeTypeMember)e).Name);
		base.Output.Write("(");
		OutputParameters(e.Parameters);
		base.Output.Write(")");
		if (!base.IsCurrentInterface && (((CodeTypeMember)e).Attributes & 0xF) != 1)
		{
			OutputStartingBrace();
			base.Indent++;
			GenerateStatements(e.Statements);
			base.Indent--;
			base.Output.WriteLine("}");
		}
		else
		{
			base.Output.WriteLine(";");
		}
	}

	protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Invalid comparison between Unknown and I4
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Invalid comparison between Unknown and I4
		if (!base.IsCurrentClass && !base.IsCurrentStruct && !base.IsCurrentInterface)
		{
			return;
		}
		if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
		{
			GenerateAttributes(((CodeTypeMember)e).CustomAttributes);
		}
		if (!base.IsCurrentInterface)
		{
			if (e.PrivateImplementationType == null)
			{
				OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
				OutputVTableModifier(((CodeTypeMember)e).Attributes);
				OutputMemberScopeModifier(((CodeTypeMember)e).Attributes);
			}
		}
		else
		{
			OutputVTableModifier(((CodeTypeMember)e).Attributes);
		}
		OutputType(e.Type);
		base.Output.Write(" ");
		if (e.PrivateImplementationType != null && !base.IsCurrentInterface)
		{
			base.Output.Write(e.PrivateImplementationType.BaseType);
			base.Output.Write(".");
		}
		if (((CollectionBase)(object)e.Parameters).Count > 0 && string.Compare(((CodeTypeMember)e).Name, "Item", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
		{
			base.Output.Write("this[");
			OutputParameters(e.Parameters);
			base.Output.Write("]");
		}
		else
		{
			OutputIdentifier(((CodeTypeMember)e).Name);
		}
		OutputStartingBrace();
		base.Indent++;
		if (e.HasGet)
		{
			if (base.IsCurrentInterface || (((CodeTypeMember)e).Attributes & 0xF) == 1)
			{
				base.Output.WriteLine("get;");
			}
			else
			{
				base.Output.Write("get");
				OutputStartingBrace();
				base.Indent++;
				GenerateStatements(e.GetStatements);
				base.Indent--;
				base.Output.WriteLine("}");
			}
		}
		if (e.HasSet)
		{
			if (base.IsCurrentInterface || (((CodeTypeMember)e).Attributes & 0xF) == 1)
			{
				base.Output.WriteLine("set;");
			}
			else
			{
				base.Output.Write("set");
				OutputStartingBrace();
				base.Indent++;
				GenerateStatements(e.SetStatements);
				base.Indent--;
				base.Output.WriteLine("}");
			}
		}
		base.Indent--;
		base.Output.WriteLine("}");
	}

	protected override void GenerateSingleFloatValue(float s)
	{
		base.Output.Write(s.ToString(CultureInfo.InvariantCulture));
		base.Output.Write('F');
	}

	protected override void GenerateDecimalValue(decimal d)
	{
		base.Output.Write(d.ToString(CultureInfo.InvariantCulture));
		base.Output.Write('m');
	}

	protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
	{
		if (e.TargetObject != null)
		{
			GenerateExpression(e.TargetObject);
			base.Output.Write(".");
		}
		OutputIdentifier(e.PropertyName);
	}

	protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsCurrentClass || base.IsCurrentStruct)
		{
			if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
			{
				GenerateAttributes(((CodeTypeMember)e).CustomAttributes);
			}
			OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
			OutputIdentifier(base.CurrentTypeName);
			base.Output.Write("(");
			OutputParameters(((CodeMemberMethod)e).Parameters);
			base.Output.Write(")");
			CodeExpressionCollection baseConstructorArgs = e.BaseConstructorArgs;
			CodeExpressionCollection chainedConstructorArgs = e.ChainedConstructorArgs;
			if (((CollectionBase)(object)baseConstructorArgs).Count > 0)
			{
				base.Output.WriteLine(" : ");
				base.Indent++;
				base.Indent++;
				base.Output.Write("base(");
				OutputExpressionList(baseConstructorArgs);
				base.Output.Write(")");
				base.Indent--;
				base.Indent--;
			}
			if (((CollectionBase)(object)chainedConstructorArgs).Count > 0)
			{
				base.Output.WriteLine(" : ");
				base.Indent++;
				base.Indent++;
				base.Output.Write("this(");
				OutputExpressionList(chainedConstructorArgs);
				base.Output.Write(")");
				base.Indent--;
				base.Indent--;
			}
			OutputStartingBrace();
			base.Indent++;
			GenerateStatements(((CodeMemberMethod)e).Statements);
			base.Indent--;
			base.Output.WriteLine("}");
		}
	}

	protected override void GenerateTypeConstructor(CodeTypeConstructor e)
	{
		if (base.IsCurrentClass || base.IsCurrentStruct)
		{
			base.Output.Write("static ");
			base.Output.Write(base.CurrentTypeName);
			base.Output.Write("()");
			OutputStartingBrace();
			base.Indent++;
			GenerateStatements(((CodeMemberMethod)e).Statements);
			base.Indent--;
			base.Output.WriteLine("}");
		}
	}

	protected override void GenerateTypeStart(CodeTypeDeclaration e)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
		{
			GenerateAttributes(((CodeTypeMember)e).CustomAttributes);
		}
		if (base.IsCurrentDelegate)
		{
			TypeAttributes typeAttributes = e.TypeAttributes & TypeAttributes.VisibilityMask;
			if (typeAttributes != 0 && typeAttributes == TypeAttributes.Public)
			{
				base.Output.Write("public ");
			}
			CodeTypeDelegate val = (CodeTypeDelegate)e;
			base.Output.Write("delegate ");
			OutputType(val.ReturnType);
			base.Output.Write(" ");
			OutputIdentifier(((CodeTypeMember)e).Name);
			base.Output.Write("(");
			OutputParameters(val.Parameters);
			base.Output.WriteLine(");");
			return;
		}
		OutputTypeAttributes(e);
		OutputIdentifier(((CodeTypeMember)e).Name);
		bool flag = true;
		foreach (CodeTypeReference item in (CollectionBase)(object)e.BaseTypes)
		{
			CodeTypeReference typeRef = item;
			if (flag)
			{
				base.Output.Write(" : ");
				flag = false;
			}
			else
			{
				base.Output.Write(", ");
			}
			OutputType(typeRef);
		}
		OutputStartingBrace();
		base.Indent++;
	}

	protected override void GenerateTypeEnd(CodeTypeDeclaration e)
	{
		if (!base.IsCurrentDelegate)
		{
			base.Indent--;
			base.Output.WriteLine("}");
		}
	}

	protected override void GenerateNamespaceStart(CodeNamespace e)
	{
		if (!string.IsNullOrEmpty(e.Name))
		{
			base.Output.Write("namespace ");
			OutputIdentifier(e.Name);
			OutputStartingBrace();
			base.Indent++;
		}
	}

	protected override void GenerateCompileUnitStart(CodeCompileUnit e)
	{
		if (((CollectionBase)(object)e.AssemblyCustomAttributes).Count > 0)
		{
			GenerateAttributes(e.AssemblyCustomAttributes, "assembly: ");
			base.Output.WriteLine("");
		}
	}

	protected override void GenerateNamespaceEnd(CodeNamespace e)
	{
		if (!string.IsNullOrEmpty(e.Name))
		{
			base.Indent--;
			base.Output.WriteLine("}");
		}
	}

	protected override void GenerateNamespaceImport(CodeNamespaceImport e)
	{
		base.Output.Write("using " + e.Namespace);
		base.Output.WriteLine(";");
	}

	protected override void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
	{
		base.Output.Write("[");
	}

	protected override void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
	{
		base.Output.Write("]");
	}

	private void GenerateAttributes(CodeAttributeDeclarationCollection attributes)
	{
		GenerateAttributes(attributes, null, inLine: false);
	}

	private void GenerateAttributes(CodeAttributeDeclarationCollection attributes, string prefix)
	{
		GenerateAttributes(attributes, prefix, inLine: false);
	}

	private void GenerateAttributes(CodeAttributeDeclarationCollection attributes, string prefix, bool inLine)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		if (((CollectionBase)(object)attributes).Count == 0)
		{
			return;
		}
		IEnumerator enumerator = ((CollectionBase)(object)attributes).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GenerateAttributeDeclarationsStart(attributes);
			if (prefix != null)
			{
				base.Output.Write(prefix);
			}
			CodeAttributeDeclaration val = (CodeAttributeDeclaration)enumerator.Current;
			base.Output.Write(GetBaseTypeOutput(val.Name));
			base.Output.Write("(");
			bool flag = true;
			foreach (CodeAttributeArgument item in (CollectionBase)(object)val.Arguments)
			{
				CodeAttributeArgument arg = item;
				if (flag)
				{
					flag = false;
				}
				else
				{
					base.Output.Write(", ");
				}
				OutputAttributeArgument(arg);
			}
			base.Output.Write(")");
			GenerateAttributeDeclarationsEnd(attributes);
			if (inLine)
			{
				base.Output.Write(" ");
			}
			else
			{
				base.Output.WriteLine();
			}
		}
	}

	public static bool IsKeywordExist(string value)
	{
		return keywordsHashtable[value] != null;
	}

	public override bool IsKeyword(string value)
	{
		return IsKeywordExist(value);
	}

	public override string GetClassKeyword()
	{
		return "class";
	}

	protected override bool Supports(GeneratorSupport support)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return (GeneratorSupport)(support & 0xFFFFF) == support;
	}

	protected override bool IsValidIdentifier(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		if (value[0] != '@')
		{
			if (IsKeyword(value))
			{
				return false;
			}
		}
		else
		{
			value = value.Substring(1);
		}
		return CodeGenerator.IsValidLanguageIndependentIdentifier(value);
	}

	protected override string CreateValidIdentifier(string name)
	{
		if (name == "date")
		{
			return name;
		}
		if (IsKeyword(name))
		{
			return "_" + name;
		}
		return name;
	}

	protected override string CreateEscapedIdentifier(string name)
	{
		if (IsKeyword(name))
		{
			return "@" + name;
		}
		return name;
	}

	public override string GetBaseTypeOutput(string baseType)
	{
		string text = baseType;
		if (text.Length == 0)
		{
			return "void";
		}
		return text switch
		{
			"System.Byte" => "byte", 
			"System.Int16" => "short", 
			"System.Int32" => "int", 
			"System.Int64" => "long", 
			"System.String" => "string", 
			"System.Object" => "object", 
			"System.Boolean" => "bool", 
			"System.Void" => "void", 
			"System.Char" => "char", 
			"System.Double" => "double", 
			"System.Single" => "float", 
			"System.Decimal" => "decimal", 
			_ => text.Replace('+', '.'), 
		};
	}

	protected override string GetTypeOutput(CodeTypeReference typeRef)
	{
		string text;
		if (typeRef.ArrayElementType != null)
		{
			text = GetTypeOutput(typeRef.ArrayElementType);
		}
		else
		{
			text = GetBaseTypeOutput(typeRef.BaseType);
			if (((CollectionBase)(object)typeRef.TypeArguments).Count > 0)
			{
				if (text.Length > 2 && text[text.Length - 2] == '`' && char.IsDigit(text, text.Length - 1))
				{
					text = text.Substring(0, text.Length - 2);
				}
				text += "<";
				for (int i = 0; i < ((CollectionBase)(object)typeRef.TypeArguments).Count; i++)
				{
					if (i > 0)
					{
						text += ",";
					}
					text += GetTypeOutput(typeRef.TypeArguments[i]);
				}
				text += ">";
			}
		}
		if (typeRef.ArrayRank > 0)
		{
			char[] array = new char[typeRef.ArrayRank + 1];
			array[0] = '[';
			array[typeRef.ArrayRank] = ']';
			for (int j = 1; j < typeRef.ArrayRank; j++)
			{
				array[j] = ',';
			}
			text += new string(array);
		}
		return text;
	}

	private static string ConvertToCommentEscapeCodes(string value)
	{
		StringBuilder stringBuilder = new StringBuilder(value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			switch (value[i])
			{
			case '\r':
				if (i < value.Length - 1 && value[i + 1] == '\n')
				{
					stringBuilder.Append("\r\n//");
					i++;
				}
				else
				{
					stringBuilder.Append("\r//");
				}
				break;
			case '\n':
				stringBuilder.Append("\n//");
				break;
			case '\u2028':
				stringBuilder.Append("\u2028//");
				break;
			case '\u2029':
				stringBuilder.Append("\u2029//");
				break;
			default:
				stringBuilder.Append(value[i]);
				break;
			case '\0':
				break;
			}
		}
		return stringBuilder.ToString();
	}

	private void OutputTypeAttributes(CodeTypeDeclaration e)
	{
		TypeAttributes typeAttributes = e.TypeAttributes;
		switch (typeAttributes & TypeAttributes.VisibilityMask)
		{
		case TypeAttributes.Public:
		case TypeAttributes.NestedPublic:
			base.Output.Write("public ");
			break;
		case TypeAttributes.NestedPrivate:
			base.Output.Write("private ");
			break;
		}
		if (e.IsStruct)
		{
			base.Output.Write("struct ");
			return;
		}
		if (e.IsEnum)
		{
			base.Output.Write("enum ");
			return;
		}
		switch (typeAttributes & TypeAttributes.ClassSemanticsMask)
		{
		case TypeAttributes.NotPublic:
			if ((typeAttributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
			{
				base.Output.Write("sealed ");
			}
			if ((typeAttributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
			{
				base.Output.Write("abstract ");
			}
			base.Output.Write("class ");
			break;
		case TypeAttributes.ClassSemanticsMask:
			base.Output.Write("interface ");
			break;
		}
	}

	private void OutputVTableModifier(MemberAttributes attributes)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		MemberAttributes val = (MemberAttributes)(attributes & 0xF0);
		if ((int)val == 16)
		{
			base.Output.Write("new ");
		}
	}

	protected override void OutputMemberScopeModifier(MemberAttributes attributes)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected I4, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		MemberAttributes val = (MemberAttributes)(attributes & 0xF);
		switch (val - 1)
		{
		case 0:
			base.Output.Write("abstract ");
			return;
		case 1:
			base.Output.Write("");
			return;
		case 2:
			base.Output.Write("static ");
			return;
		case 3:
			base.Output.Write("override ");
			return;
		}
		MemberAttributes val2 = (MemberAttributes)(attributes & 0xF000);
		if ((int)val2 == 12288 || (int)val2 == 24576)
		{
			base.Output.Write("virtual ");
		}
	}

	protected override void OutputFieldScopeModifier(MemberAttributes attributes)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected I4, but got Unknown
		MemberAttributes val = (MemberAttributes)(attributes & 0xF);
		switch (val - 2)
		{
		case 1:
			base.Output.Write("static ");
			break;
		case 3:
			base.Output.Write("const ");
			break;
		case 0:
		case 2:
			break;
		}
	}

	protected override void OutputIdentifier(string ident)
	{
		base.Output.Write(CreateEscapedIdentifier(ident));
	}

	protected override void OutputType(CodeTypeReference typeRef)
	{
		base.Output.Write(GetTypeOutput(typeRef));
	}

	protected virtual void OutputStartingBrace()
	{
		if (base.Options.BracingStyle == "C")
		{
			base.Output.WriteLine("");
			base.Output.WriteLine("{");
		}
		else
		{
			base.Output.WriteLine(" {");
		}
	}

	private void GeneratePrimitiveChar(char c)
	{
		base.Output.Write('\'');
		switch (c)
		{
		case '\r':
			base.Output.Write("\\r");
			break;
		case '\t':
			base.Output.Write("\\t");
			break;
		case '"':
			base.Output.Write("\\\"");
			break;
		case '\'':
			base.Output.Write("\\'");
			break;
		case '\\':
			base.Output.Write("\\\\");
			break;
		case '\0':
			base.Output.Write("\\0");
			break;
		case '\n':
			base.Output.Write("\\n");
			break;
		default:
			base.Output.Write(c);
			break;
		}
		base.Output.Write('\'');
	}

	public override string GetRegionStartWord()
	{
		return "#region";
	}

	public override string GetRegionEndWord()
	{
		return "#endregion";
	}

	public override string GetRegionStart(string s)
	{
		return GetRegionStartWord() + " " + s;
	}

	public override string GetRegionEnd(string s)
	{
		return GetRegionEndWord() + " " + s;
	}

	protected override void GenerateRegionStart(StiCodeRegionStart e)
	{
		base.Output.Write(GetRegionStart(e.Text));
		base.Output.WriteLine("");
	}

	protected override void GenerateRegionEnd(StiCodeRegionEnd e)
	{
		base.Output.Write(GetRegionEnd(e.Text));
		base.Output.WriteLine("");
	}

	static StiCSharpCodeGenerator()
	{
		keywordsHashtable = new Hashtable();
		keywords = new string[77]
		{
			"as", "do", "if", "in", "is", "for", "int", "new", "out", "ref",
			"try", "base", "bool", "byte", "case", "char", "else", "enum", "goto", "lock",
			"long", "null", "this", "true", "uint", "void", "break", "catch", "class", "const",
			"event", "false", "fixed", "float", "sbyte", "short", "throw", "ulong", "using", "while",
			"double", "extern", "object", "params", "public", "return", "sealed", "sizeof", "static", "string",
			"struct", "switch", "typeof", "unsafe", "ushort", "checked", "decimal", "default", "exfloat", "finally",
			"foreach", "private", "virtual", "abstract", "continue", "delegate", "exdouble", "explicit", "implicit", "internal",
			"operator", "override", "readonly", "interface", "namespace", "protected", "unchecked"
		};
		MaximizeStringLength = true;
		string[] array = keywords;
		foreach (string text in array)
		{
			keywordsHashtable.Add(text, text);
		}
	}
}
