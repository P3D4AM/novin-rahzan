using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Stimulsoft.Report.CodeDom;

public class StiVBCodeGenerator : StiCodeGenerator
{
	private static Hashtable keywordsHashtable;

	private static readonly string[] keywords;

	private const GeneratorSupport LanguageSupport = 2097151;

	private const int MaxLineLength = 80;

	protected override string NullToken => "Nothing";

	static StiVBCodeGenerator()
	{
		keywordsHashtable = new Hashtable();
		keywords = new string[153]
		{
			"as", "do", "if", "in", "is", "me", "on", "or", "to", "and",
			"chr", "dim", "end", "for", "get", "let", "lib", "mod", "new", "not",
			"off", "rem", "set", "sub", "try", "xor", "ansi", "auto", "byte", "call",
			"case", "cchr", "cdbl", "cdec", "char", "chrw", "cint", "clng", "cobj", "csng",
			"cstr", "date", "each", "else", "enum", "exit", "goto", "like", "long", "loop",
			"next", "null", "step", "stop", "text", "then", "true", "when", "with", "alias",
			"andif", "byref", "byval", "catch", "cbool", "cbyte", "cchar", "cdate", "class", "const",
			"ctype", "endif", "erase", "error", "event", "false", "gosub", "redim", "short", "throw",
			"until", "while", "binary", "cshort", "double", "elseif", "friend", "module", "mythis", "object",
			"option", "orelse", "public", "region", "resume", "return", "select", "shared", "single", "static",
			"strict", "string", "typeof", "andalso", "boolean", "compare", "convert", "decimal", "declare", "default",
			"finally", "gettype", "handles", "imports", "integer", "myclass", "nothing", "private", "shadows", "unicode",
			"variant", "assembly", "delegate", "explicit", "function", "inherits", "optional", "preserve", "property", "readonly",
			"synclock", "addressof", "interface", "namespace", "overloads", "overrides", "protected", "structure", "writeonly", "addhandler",
			"implements", "paramarray", "raiseevent", "withevents", "endprologue", "mustinherit", "overridable", "mustoverride", "beginepilogue", "removehandler",
			"externalsource", "notinheritable", "notoverridable"
		};
		string[] array = keywords;
		foreach (string text in array)
		{
			keywordsHashtable.Add(text, text);
		}
	}

	protected bool AllowLateBound(CodeCompileUnit e)
	{
		object obj = ((CodeObject)e).UserData["AllowLateBound"];
		if (obj != null && (bool)obj)
		{
			return (bool)obj;
		}
		return true;
	}

	private static void AppendEscapedChar(StringBuilder b, char value)
	{
		b.Append("&Microsoft.VisualBasic.ChrW(");
		int num = value;
		b.Append(num.ToString(CultureInfo.InvariantCulture));
		b.Append(")");
	}

	protected override void ContinueOnNewLine(string st)
	{
		base.Output.Write(st);
		base.Output.WriteLine(" _");
	}

	private static string ConvertToCommentEscapeCodes(string value)
	{
		StringBuilder stringBuilder = new StringBuilder(value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			char c = value[i];
			switch (c)
			{
			case '\r':
				if (i < value.Length - 1 && value[i + 1] == '\n')
				{
					stringBuilder.Append(" '");
					i++;
				}
				else
				{
					stringBuilder.Append(" '");
				}
				continue;
			case '\n':
				stringBuilder.Append(" '");
				continue;
			}
			int num = c - 8232;
			if (num == 0 || num == 1)
			{
				stringBuilder.Append(value[i]);
				stringBuilder.Append(39);
			}
			else
			{
				stringBuilder.Append(value[i]);
			}
		}
		return stringBuilder.ToString();
	}

	protected override string CreateEscapedIdentifier(string name)
	{
		if (IsKeyword(name))
		{
			return "[" + name + "]";
		}
		return name;
	}

	protected override string CreateValidIdentifier(string name)
	{
		if (IsKeyword(name))
		{
			return "_" + name;
		}
		return name;
	}

	private void EnsureInDoubleQuotes(ref bool fInDoubleQuotes, StringBuilder b)
	{
		if (!fInDoubleQuotes)
		{
			b.Append("&\"");
			fInDoubleQuotes = true;
		}
	}

	private void EnsureNotInDoubleQuotes(ref bool fInDoubleQuotes, StringBuilder b)
	{
		if (fInDoubleQuotes)
		{
			b.Append('"');
			fInDoubleQuotes = false;
		}
	}

	protected override void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e)
	{
		OutputIdentifier(e.ParameterName);
	}

	protected override void GenerateArrayCreateExpression(CodeArrayCreateExpression e)
	{
		base.Output.Write("New ");
		OutputType(e.CreateType);
		CodeExpressionCollection initializers = e.Initializers;
		if (((CollectionBase)(object)initializers).Count > 0)
		{
			base.Output.Write("() {");
			base.Indent++;
			OutputExpressionList(initializers);
			base.Indent--;
			base.Output.Write("}");
			return;
		}
		base.Output.Write("(");
		if (e.SizeExpression != null)
		{
			base.Output.Write("(");
			GenerateExpression(e.SizeExpression);
			base.Output.Write(") - 1");
		}
		else
		{
			base.Output.Write(e.Size - 1);
		}
		base.Output.Write(") {}");
	}

	protected override void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		GenerateExpression(e.TargetObject);
		base.Output.Write("(");
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
		base.Output.Write(")");
	}

	protected override void GenerateAssignStatement(CodeAssignStatement e)
	{
		GenerateExpression(e.Left);
		base.Output.Write(" = ");
		GenerateExpression(e.Right);
		base.Output.WriteLine(string.Empty);
	}

	protected override void GenerateAttachEventStatement(CodeAttachEventStatement e)
	{
		base.Output.Write("AddHandler ");
		GenerateFormalEventReferenceExpression(e.Event);
		base.Output.Write(", ");
		GenerateExpression(e.Listener);
		base.Output.WriteLine(string.Empty);
	}

	protected override void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes)
	{
		base.Output.Write(">");
	}

	protected override void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes)
	{
		base.Output.Write("<");
	}

	protected override void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e)
	{
		base.Output.Write("MyBase");
	}

	protected override void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Operator != 6)
		{
			base.GenerateBinaryOperatorExpression(e);
		}
		else if (e.Right is CodePrimitiveExpression && ((CodePrimitiveExpression)e.Right).Value == null)
		{
			GenerateNotIsNullExpression(e.Left);
		}
		else if (e.Left is CodePrimitiveExpression && ((CodePrimitiveExpression)e.Left).Value == null)
		{
			GenerateNotIsNullExpression(e.Right);
		}
		else
		{
			base.GenerateBinaryOperatorExpression(e);
		}
	}

	protected override void GenerateCastExpression(CodeCastExpression e)
	{
		base.Output.Write("CType(");
		GenerateExpression(e.Expression);
		base.Output.Write(",");
		OutputType(e.TargetType);
		OutputArrayPostfix(e.TargetType);
		base.Output.Write(")");
	}

	protected override void GenerateComment(CodeComment e)
	{
		base.Output.Write("'");
		base.Output.WriteLine(ConvertToCommentEscapeCodes(e.Text));
	}

	protected override void GenerateCompileUnit(CodeCompileUnit e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		GenerateCompileUnitStart(e);
		SortedList sortedList = new SortedList(new CaseInsensitiveComparer(CultureInfo.InvariantCulture));
		foreach (CodeNamespace item in (CollectionBase)(object)e.Namespaces)
		{
			CodeNamespace val = item;
			((CodeObject)val).UserData["GenerateImports"] = 0;
			foreach (CodeNamespaceImport import in val.Imports)
			{
				CodeNamespaceImport val2 = import;
				if (!sortedList.Contains(val2.Namespace))
				{
					sortedList.Add(val2.Namespace, val2.Namespace);
				}
			}
		}
		foreach (string key in sortedList.Keys)
		{
			base.Output.Write("Imports ");
			OutputIdentifier(key);
			base.Output.WriteLine(string.Empty);
		}
		if (((CollectionBase)(object)e.AssemblyCustomAttributes).Count > 0)
		{
			OutputAttributes(e.AssemblyCustomAttributes, inLine: false, "Assembly: ", closingLine: true);
		}
		GenerateNamespaces(e);
		GenerateCompileUnitEnd(e);
	}

	protected override void GenerateCompileUnitStart(CodeCompileUnit e)
	{
		base.Output.WriteLine("'------------------------------------------------------------------------------");
		base.Output.WriteLine("' <autogenerated>");
		base.Output.WriteLine("' This code was generated by a tool.");
		base.Output.WriteLine("' Runtime Version: " + Environment.Version.ToString());
		base.Output.WriteLine("'");
		base.Output.WriteLine("' Changes to this file may cause incorrect behavior and will be lost if ");
		base.Output.WriteLine("' the code is regenerated.");
		base.Output.WriteLine("' </autogenerated>");
		base.Output.WriteLine("'------------------------------------------------------------------------------");
		base.Output.WriteLine(string.Empty);
		if (AllowLateBound(e))
		{
			base.Output.WriteLine("Option Strict Off");
		}
		else
		{
			base.Output.WriteLine("Option Strict On");
		}
		if (!RequireVariableDeclaration(e))
		{
			base.Output.WriteLine("Option Explicit Off");
		}
		else
		{
			base.Output.WriteLine("Option Explicit On");
		}
		base.Output.WriteLine();
	}

	protected override void GenerateConditionStatement(CodeConditionStatement e)
	{
		base.Output.Write("If ");
		GenerateExpression(e.Condition);
		base.Output.WriteLine(" Then");
		base.Indent++;
		GenerateStatements(e.TrueStatements);
		base.Indent--;
		CodeStatementCollection falseStatements = e.FalseStatements;
		if (((CollectionBase)(object)falseStatements).Count > 0)
		{
			base.Output.Write("Else");
			base.Output.WriteLine(string.Empty);
			base.Indent++;
			GenerateStatements(e.FalseStatements);
			base.Indent--;
		}
		base.Output.WriteLine("End If");
	}

	protected override void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsCurrentClass || base.IsCurrentStruct)
		{
			if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
			{
				OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
			}
			OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
			base.Output.Write("Sub New(");
			OutputParameters(((CodeMemberMethod)e).Parameters);
			base.Output.WriteLine(")");
			base.Indent++;
			CodeExpressionCollection baseConstructorArgs = e.BaseConstructorArgs;
			CodeExpressionCollection chainedConstructorArgs = e.ChainedConstructorArgs;
			if (((CollectionBase)(object)chainedConstructorArgs).Count > 0)
			{
				base.Output.Write("Me.New(");
				OutputExpressionList(chainedConstructorArgs);
				base.Output.Write(")");
				base.Output.WriteLine(string.Empty);
			}
			else if (((CollectionBase)(object)baseConstructorArgs).Count > 0)
			{
				base.Output.Write("MyBase.New(");
				OutputExpressionList(baseConstructorArgs);
				base.Output.Write(")");
				base.Output.WriteLine(string.Empty);
			}
			else
			{
				base.Output.WriteLine("MyBase.New");
			}
			GenerateStatements(((CodeMemberMethod)e).Statements);
			base.Indent--;
			base.Output.WriteLine("End Sub");
		}
	}

	protected override void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e)
	{
		base.Output.Write("AddressOf ");
		GenerateExpression(e.TargetObject);
		base.Output.Write(".");
		OutputIdentifier(e.MethodName);
	}

	protected override void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		base.Output.Write("RaiseEvent ");
		if (e.TargetObject != null)
		{
			if (e.TargetObject is CodeEventReferenceExpression)
			{
				GenerateFormalEventReferenceExpression((CodeEventReferenceExpression)e.TargetObject);
			}
			else
			{
				GenerateExpression(e.TargetObject);
			}
		}
		CodeExpressionCollection parameters = e.Parameters;
		if (((CollectionBase)(object)parameters).Count > 0)
		{
			base.Output.Write("(");
			OutputExpressionList(e.Parameters);
			base.Output.Write(")");
		}
	}

	protected override void GenerateDirectionExpression(CodeDirectionExpression e)
	{
		GenerateExpression(e.Expression);
	}

	protected override void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c)
	{
		base.Output.WriteLine("Public Shared Sub Main()");
		base.Indent++;
		GenerateStatements(((CodeMemberMethod)e).Statements);
		base.Indent--;
		base.Output.WriteLine("End Sub");
	}

	protected override void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (!base.IsCurrentDelegate && !base.IsCurrentEnum)
		{
			if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
			{
				OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
			}
			OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
			base.Output.Write("Event ");
			OutputTypeNamePair(e.Type, ((CodeTypeMember)e).Name);
			base.Output.WriteLine(string.Empty);
		}
	}

	protected override void GenerateEventReferenceExpression(CodeEventReferenceExpression e)
	{
		if (e.TargetObject != null)
		{
			bool flag = e.TargetObject is CodeThisReferenceExpression;
			GenerateExpression(e.TargetObject);
			base.Output.Write(".");
			if (flag)
			{
				base.Output.Write(e.EventName + "Event");
			}
			else
			{
				base.Output.Write(e.EventName);
			}
		}
		else
		{
			OutputIdentifier(e.EventName + "Event");
		}
	}

	protected override void GenerateExpressionStatement(CodeExpressionStatement e)
	{
		GenerateExpression(e.Expression);
		base.Output.WriteLine(string.Empty);
	}

	protected override void GenerateField(CodeMemberField e)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		if (base.IsCurrentDelegate || base.IsCurrentInterface)
		{
			return;
		}
		if (base.IsCurrentEnum)
		{
			if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
			{
				OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
			}
			OutputIdentifier(((CodeTypeMember)e).Name);
			if (e.InitExpression != null)
			{
				base.Output.Write(" = ");
				GenerateExpression(e.InitExpression);
			}
			base.Output.WriteLine(string.Empty);
			return;
		}
		if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
		{
			OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
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
		base.Output.WriteLine(string.Empty);
	}

	protected override void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e)
	{
		if (e.TargetObject != null)
		{
			GenerateExpression(e.TargetObject);
			base.Output.Write(".");
			base.Output.Write(e.FieldName);
		}
		else
		{
			OutputIdentifier(e.FieldName);
		}
	}

	protected override void GenerateGotoStatement(CodeGotoStatement e)
	{
		base.Output.Write("goto ");
		base.Output.WriteLine(e.Label);
	}

	protected override void GenerateIndexerExpression(CodeIndexerExpression e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		GenerateExpression(e.TargetObject);
		base.Output.Write("(");
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
		base.Output.Write(")");
	}

	protected override void GenerateIterationStatement(CodeIterationStatement e)
	{
		GenerateStatement(e.InitStatement);
		base.Output.Write("Do While ");
		GenerateExpression(e.TestExpression);
		base.Output.WriteLine(string.Empty);
		base.Indent++;
		GenerateStatements(e.Statements);
		GenerateStatement(e.IncrementStatement);
		base.Indent--;
		base.Output.WriteLine("Loop");
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

	protected override void GenerateLinePragmaEnd(CodeLinePragma e)
	{
		base.Output.WriteLine(string.Empty);
		base.Output.WriteLine("#End ExternalSource");
	}

	protected override void GenerateLinePragmaStart(CodeLinePragma e)
	{
		base.Output.WriteLine(string.Empty);
		base.Output.Write("#ExternalSource(\"");
		base.Output.Write(e.FileName);
		base.Output.Write("\",");
		base.Output.Write(e.LineNumber);
		base.Output.WriteLine(")");
	}

	protected override void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c)
	{
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Invalid comparison between Unknown and I4
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		if (!base.IsCurrentClass && !base.IsCurrentStruct && !base.IsCurrentInterface)
		{
			return;
		}
		if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
		{
			OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
		}
		string name = ((CodeTypeMember)e).Name;
		if (e.PrivateImplementationType != null)
		{
			string baseType = e.PrivateImplementationType.BaseType;
			baseType = baseType.Replace('.', '_');
			((CodeTypeMember)e).Name = baseType + "_" + ((CodeTypeMember)e).Name;
		}
		if (!base.IsCurrentInterface)
		{
			if (e.PrivateImplementationType == null)
			{
				OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
				if (c != null && MethodIsOverloaded(e, c))
				{
					base.Output.Write("Overloads ");
				}
			}
			OutputVTableModifier(((CodeTypeMember)e).Attributes);
			OutputMemberScopeModifier(((CodeTypeMember)e).Attributes);
		}
		else
		{
			OutputVTableModifier(((CodeTypeMember)e).Attributes);
		}
		bool flag = false;
		if (e.ReturnType.BaseType.Length == 0 || string.Compare(e.ReturnType.BaseType, typeof(void).FullName, ignoreCase: true, CultureInfo.InvariantCulture) == 0)
		{
			flag = true;
		}
		if (flag)
		{
			base.Output.Write("Sub ");
		}
		else
		{
			base.Output.Write("Function ");
		}
		OutputIdentifier(((CodeTypeMember)e).Name);
		base.Output.Write("(");
		OutputParameters(e.Parameters);
		base.Output.Write(")");
		if (!flag)
		{
			base.Output.Write(" As ");
			if (((CollectionBase)(object)e.ReturnTypeCustomAttributes).Count > 0)
			{
				OutputAttributes(e.ReturnTypeCustomAttributes, inLine: true);
			}
			OutputType(e.ReturnType);
			OutputArrayPostfix(e.ReturnType);
		}
		if (((CollectionBase)(object)e.ImplementationTypes).Count > 0)
		{
			base.Output.Write(" Implements ");
			bool flag2 = true;
			foreach (CodeTypeReference item in (CollectionBase)(object)e.ImplementationTypes)
			{
				CodeTypeReference typeRef = item;
				if (flag2)
				{
					flag2 = false;
				}
				else
				{
					base.Output.Write(" , ");
				}
				OutputType(typeRef);
				base.Output.Write(".");
				OutputIdentifier(name);
			}
		}
		else if (e.PrivateImplementationType != null)
		{
			base.Output.Write(" Implements ");
			OutputType(e.PrivateImplementationType);
			base.Output.Write(".");
			OutputIdentifier(name);
		}
		base.Output.WriteLine(string.Empty);
		if (!base.IsCurrentInterface && (((CodeTypeMember)e).Attributes & 0xF) != 1)
		{
			base.Indent++;
			GenerateStatements(e.Statements);
			base.Indent--;
			if (flag)
			{
				base.Output.WriteLine("End Sub");
			}
			else
			{
				base.Output.WriteLine("End Function");
			}
		}
		((CodeTypeMember)e).Name = name;
	}

	protected override void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e)
	{
		GenerateMethodReferenceExpression(e.Method);
		CodeExpressionCollection parameters = e.Parameters;
		if (((CollectionBase)(object)parameters).Count > 0)
		{
			base.Output.Write("(");
			OutputExpressionList(e.Parameters);
			base.Output.Write(")");
		}
	}

	protected override void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e)
	{
		if (e.TargetObject != null)
		{
			GenerateExpression(e.TargetObject);
			base.Output.Write(".");
			base.Output.Write(e.MethodName);
		}
		else
		{
			OutputIdentifier(e.MethodName);
		}
	}

	protected override void GenerateMethodReturnStatement(CodeMethodReturnStatement e)
	{
		if (e.Expression != null)
		{
			base.Output.Write("Return ");
			GenerateExpression(e.Expression);
			base.Output.WriteLine(string.Empty);
		}
		else
		{
			base.Output.WriteLine("Return");
		}
	}

	protected override void GenerateNamespace(CodeNamespace e)
	{
		if (GetUserData((CodeObject)(object)e, "GenerateImports", defaultValue: true))
		{
			GenerateNamespaceImports(e);
		}
		base.Output.WriteLine();
		GenerateNamespaceStart(e);
		GenerateTypes(e);
		GenerateNamespaceEnd(e);
	}

	protected override void GenerateNamespaceEnd(CodeNamespace e)
	{
		if (!string.IsNullOrEmpty(e.Name))
		{
			base.Indent--;
			base.Output.WriteLine("End Namespace");
		}
	}

	protected override void GenerateNamespaceImport(CodeNamespaceImport e)
	{
		base.Output.Write("Imports ");
		OutputIdentifier(e.Namespace);
		base.Output.WriteLine(string.Empty);
	}

	protected override void GenerateNamespaceStart(CodeNamespace e)
	{
		if (!string.IsNullOrEmpty(e.Name))
		{
			base.Output.Write("Namespace ");
			OutputIdentifier(e.Name);
			base.Output.WriteLine();
			base.Indent++;
		}
	}

	protected override void GenerateObjectCreateExpression(CodeObjectCreateExpression e)
	{
		base.Output.Write("New ");
		OutputType(e.CreateType);
		CodeExpressionCollection parameters = e.Parameters;
		if (((CollectionBase)(object)parameters).Count > 0)
		{
			base.Output.Write("(");
			OutputExpressionList(parameters);
			base.Output.Write(")");
		}
	}

	protected override void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (((CollectionBase)(object)e.CustomAttributes).Count > 0)
		{
			OutputAttributes(e.CustomAttributes, inLine: true);
		}
		OutputDirection(e.Direction);
		OutputTypeNamePair(e.Type, e.Name);
	}

	protected override void GeneratePrimitiveExpression(CodePrimitiveExpression e)
	{
		if (e.Value is char)
		{
			int num = ((IConvertible)e.Value).ToInt32(null);
			base.Output.Write("Microsoft.VisualBasic.ChrW(" + num + ")");
		}
		else
		{
			base.GeneratePrimitiveExpression(e);
		}
	}

	protected override void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Invalid comparison between Unknown and I4
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Invalid comparison between Unknown and I4
		if (!base.IsCurrentClass && !base.IsCurrentStruct && !base.IsCurrentInterface)
		{
			return;
		}
		if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
		{
			OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
		}
		string name = ((CodeTypeMember)e).Name;
		if (e.PrivateImplementationType != null)
		{
			string baseType = e.PrivateImplementationType.BaseType;
			baseType = baseType.Replace('.', '_');
			((CodeTypeMember)e).Name = baseType + "_" + ((CodeTypeMember)e).Name;
		}
		if (!base.IsCurrentInterface)
		{
			if (e.PrivateImplementationType == null)
			{
				OutputMemberAccessModifier(((CodeTypeMember)e).Attributes);
				if (PropertyIsOverloaded(e, c))
				{
					base.Output.Write("Overloads ");
				}
			}
			OutputVTableModifier(((CodeTypeMember)e).Attributes);
			OutputMemberScopeModifier(((CodeTypeMember)e).Attributes);
		}
		else
		{
			OutputVTableModifier(((CodeTypeMember)e).Attributes);
		}
		if (((CollectionBase)(object)e.Parameters).Count > 0 && string.Compare(((CodeTypeMember)e).Name, "Item", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
		{
			base.Output.Write("Default ");
		}
		if (e.HasGet)
		{
			if (!e.HasSet)
			{
				base.Output.Write("ReadOnly ");
			}
		}
		else if (e.HasSet)
		{
			base.Output.Write("WriteOnly ");
		}
		base.Output.Write("Property ");
		OutputIdentifier(((CodeTypeMember)e).Name);
		if (((CollectionBase)(object)e.Parameters).Count > 0)
		{
			base.Output.Write("(");
			OutputParameters(e.Parameters);
			base.Output.Write(")");
		}
		base.Output.Write(" As ");
		OutputType(e.Type);
		OutputArrayPostfix(e.Type);
		if (((CollectionBase)(object)e.ImplementationTypes).Count > 0)
		{
			base.Output.Write(" Implements ");
			bool flag = true;
			foreach (CodeTypeReference item in (CollectionBase)(object)e.ImplementationTypes)
			{
				CodeTypeReference typeRef = item;
				if (flag)
				{
					flag = false;
				}
				else
				{
					base.Output.Write(" , ");
				}
				OutputType(typeRef);
				base.Output.Write(".");
				OutputIdentifier(name);
			}
		}
		else if (e.PrivateImplementationType != null)
		{
			base.Output.Write(" Implements ");
			OutputType(e.PrivateImplementationType);
			base.Output.Write(".");
			OutputIdentifier(name);
		}
		base.Output.WriteLine("");
		if (!c.IsInterface)
		{
			base.Indent++;
			if (e.HasGet)
			{
				base.Output.WriteLine("Get");
				if (!base.IsCurrentInterface && (((CodeTypeMember)e).Attributes & 0xF) != 1)
				{
					base.Indent++;
					GenerateStatements(e.GetStatements);
					((CodeTypeMember)e).Name = name;
					base.Indent--;
					base.Output.WriteLine("End Get");
				}
			}
			if (e.HasSet)
			{
				base.Output.WriteLine("Set");
				if (!base.IsCurrentInterface && (((CodeTypeMember)e).Attributes & 0xF) != 1)
				{
					base.Indent++;
					GenerateStatements(e.SetStatements);
					base.Indent--;
					base.Output.WriteLine("End Set");
				}
			}
			base.Indent--;
			base.Output.WriteLine("End Property");
		}
		((CodeTypeMember)e).Name = name;
	}

	protected override void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e)
	{
		if (e.TargetObject != null)
		{
			GenerateExpression(e.TargetObject);
			base.Output.Write(".");
			base.Output.Write(e.PropertyName);
		}
		else
		{
			OutputIdentifier(e.PropertyName);
		}
	}

	protected override void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e)
	{
		base.Output.Write("value");
	}

	protected override void GenerateRemoveEventStatement(CodeRemoveEventStatement e)
	{
		base.Output.Write("RemoveHandler ");
		GenerateFormalEventReferenceExpression(e.Event);
		base.Output.Write(", ");
		GenerateExpression(e.Listener);
		base.Output.WriteLine("");
	}

	protected override void GenerateSingleFloatValue(float s)
	{
		base.Output.Write(s.ToString(CultureInfo.InvariantCulture));
		base.Output.Write('!');
	}

	protected override void GenerateSnippetExpression(CodeSnippetExpression e)
	{
		base.Output.Write(e.Value);
	}

	protected override void GenerateSnippetMember(CodeSnippetTypeMember e)
	{
		base.Output.Write(e.Text);
	}

	protected override void GenerateSnippetStatement(CodeSnippetStatement e)
	{
		base.Output.WriteLine(e.Value);
	}

	protected override void GenerateThisReferenceExpression(CodeThisReferenceExpression e)
	{
		base.Output.Write("Me");
	}

	protected override void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e)
	{
		base.Output.Write("Throw");
		if (e.ToThrow != null)
		{
			base.Output.Write(" ");
			GenerateExpression(e.ToThrow);
		}
		base.Output.WriteLine("");
	}

	protected override void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		base.Output.WriteLine("Try ");
		base.Indent++;
		GenerateStatements(e.TryStatements);
		base.Indent--;
		CodeCatchClauseCollection catchClauses = e.CatchClauses;
		if (((CollectionBase)(object)catchClauses).Count > 0)
		{
			IEnumerator enumerator = ((CollectionBase)(object)catchClauses).GetEnumerator();
			while (enumerator.MoveNext())
			{
				CodeCatchClause val = (CodeCatchClause)enumerator.Current;
				base.Output.Write("Catch ");
				OutputTypeNamePair(val.CatchExceptionType, val.LocalName);
				base.Output.WriteLine("");
				base.Indent++;
				GenerateStatements(val.Statements);
				base.Indent--;
			}
		}
		CodeStatementCollection finallyStatements = e.FinallyStatements;
		if (((CollectionBase)(object)finallyStatements).Count > 0)
		{
			base.Output.WriteLine("Finally");
			base.Indent++;
			GenerateStatements(finallyStatements);
			base.Indent--;
		}
		base.Output.WriteLine("End Try");
	}

	protected override void GenerateTypeConstructor(CodeTypeConstructor e)
	{
		if (base.IsCurrentClass || base.IsCurrentStruct)
		{
			base.Output.WriteLine("Shared Sub New()");
			base.Indent++;
			GenerateStatements(((CodeMemberMethod)e).Statements);
			base.Indent--;
			base.Output.WriteLine("End Sub");
		}
	}

	protected override void GenerateTypeEnd(CodeTypeDeclaration e)
	{
		if (!base.IsCurrentDelegate)
		{
			base.Indent--;
			string value = (e.IsEnum ? "End Enum" : (e.IsInterface ? "End Interface" : ((!e.IsStruct) ? "End Class" : "End Structure")));
			base.Output.WriteLine(value);
		}
	}

	protected override void GenerateTypeOfExpression(CodeTypeOfExpression e)
	{
		base.Output.Write("GetType(");
		base.Output.Write(e.Type.BaseType);
		OutputArrayPostfix(e.Type);
		base.Output.Write(")");
	}

	protected override void GenerateTypeStart(CodeTypeDeclaration e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		if (base.IsCurrentDelegate)
		{
			if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
			{
				OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
			}
			TypeAttributes typeAttributes = e.TypeAttributes & TypeAttributes.NestedFamANDAssem;
			if (typeAttributes != 0 && typeAttributes == TypeAttributes.Public)
			{
				base.Output.Write("Public ");
			}
			CodeTypeDelegate val = (CodeTypeDelegate)e;
			if (val.ReturnType.BaseType.Length > 0 && string.Compare(val.ReturnType.BaseType, "System.Void", ignoreCase: true, CultureInfo.InvariantCulture) != 0)
			{
				base.Output.Write("Delegate Function ");
			}
			else
			{
				base.Output.Write("Delegate Sub ");
			}
			OutputIdentifier(((CodeTypeMember)e).Name);
			base.Output.Write("(");
			OutputParameters(val.Parameters);
			base.Output.Write(")");
			if (val.ReturnType.BaseType.Length > 0 && string.Compare(val.ReturnType.BaseType, "System.Void", ignoreCase: true, CultureInfo.InvariantCulture) != 0)
			{
				base.Output.Write(" As ");
				OutputType(val.ReturnType);
				OutputArrayPostfix(val.ReturnType);
			}
			base.Output.WriteLine("");
			return;
		}
		if (e.IsEnum)
		{
			if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
			{
				OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
			}
			OutputTypeAttributes(e);
			OutputIdentifier(((CodeTypeMember)e).Name);
			if (((CollectionBase)(object)e.BaseTypes).Count > 0)
			{
				base.Output.Write(" As ");
				OutputType(e.BaseTypes[0]);
			}
			base.Output.WriteLine("");
			base.Indent++;
			return;
		}
		if (((CollectionBase)(object)((CodeTypeMember)e).CustomAttributes).Count > 0)
		{
			OutputAttributes(((CodeTypeMember)e).CustomAttributes, inLine: false);
		}
		OutputTypeAttributes(e);
		OutputIdentifier(((CodeTypeMember)e).Name);
		bool flag = false;
		bool flag2 = false;
		if (e.IsStruct)
		{
			flag = true;
		}
		if (e.IsInterface)
		{
			flag2 = true;
		}
		base.Indent++;
		foreach (CodeTypeReference item in (CollectionBase)(object)e.BaseTypes)
		{
			CodeTypeReference typeRef = item;
			if (!flag)
			{
				base.Output.WriteLine("");
				base.Output.Write("Inherits ");
				flag = true;
			}
			else if (!flag2)
			{
				base.Output.WriteLine("");
				base.Output.Write("Implements ");
				flag2 = true;
			}
			else
			{
				base.Output.Write(", ");
			}
			OutputType(typeRef);
		}
		base.Output.WriteLine("");
	}

	protected override void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e)
	{
		base.Output.Write("Dim ");
		OutputTypeNamePair(e.Type, e.Name);
		if (e.InitExpression != null)
		{
			base.Output.Write(" = ");
			GenerateExpression(e.InitExpression);
		}
		base.Output.WriteLine("");
	}

	protected override void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e)
	{
		OutputIdentifier(e.VariableName);
	}

	private void GenerateFormalEventReferenceExpression(CodeEventReferenceExpression e)
	{
		if (e.TargetObject != null && !(e.TargetObject is CodeThisReferenceExpression))
		{
			GenerateExpression(e.TargetObject);
			base.Output.Write(".");
		}
		OutputIdentifier(e.EventName);
	}

	private string GetArrayPostfix(CodeTypeReference typeRef)
	{
		string text = "";
		if (typeRef.ArrayElementType != null)
		{
			text = GetArrayPostfix(typeRef.ArrayElementType);
		}
		if (typeRef.ArrayRank > 0)
		{
			char[] array = new char[typeRef.ArrayRank + 1];
			array[0] = '(';
			array[typeRef.ArrayRank] = ')';
			for (int i = 1; i < typeRef.ArrayRank; i++)
			{
				array[i] = ',';
			}
			text += new string(array);
		}
		return text;
	}

	private void GenerateNotIsNullExpression(CodeExpression e)
	{
		base.Output.Write("(Not (");
		GenerateExpression(e);
		base.Output.Write(") Is ");
		base.Output.Write(NullToken);
		base.Output.Write(")");
	}

	public override string GetBaseTypeOutput(string thisType)
	{
		if (thisType.Length == 0)
		{
			return "Void";
		}
		switch (thisType)
		{
		case "System.Byte":
			return "Byte";
		case "System.Int16":
			return "Short";
		case "System.Int32":
			return "Integer";
		case "System.Int64":
			return "Long";
		case "System.string":
			return "String";
		case "System.DateTime":
			return "Date";
		case "System.Decimal":
			return "Decimal";
		case "System.Single":
			return "Single";
		case "System.Double":
			return "Double";
		case "System.Boolean":
			return "Boolean";
		case "System.Char":
			return "Char";
		case "System.Object":
			return "Object";
		default:
			thisType = thisType.Replace('+', '.');
			thisType = CreateEscapedIdentifier(thisType);
			return thisType;
		}
	}

	protected override string GetTypeOutput(CodeTypeReference typeRef)
	{
		string text = GetBaseTypeOutput(typeRef.BaseType);
		if (typeRef.ArrayRank > 0)
		{
			text += GetArrayPostfix(typeRef);
		}
		return text;
	}

	private bool GetUserData(CodeObject e, string property, bool defaultValue)
	{
		object obj = e.UserData[property];
		if (obj != null && (bool)obj)
		{
			return (bool)obj;
		}
		return defaultValue;
	}

	public static bool IsKeywordExist(string value)
	{
		return keywordsHashtable[value] != null;
	}

	public override bool IsKeyword(string value)
	{
		return IsKeywordExist(value);
	}

	protected override bool IsValidIdentifier(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		if (value[0] != '[' || value[value.Length - 1] != ']')
		{
			if (IsKeyword(value))
			{
				return false;
			}
		}
		else
		{
			value = value.Substring(1, value.Length - 2);
		}
		return CodeGenerator.IsValidLanguageIndependentIdentifier(value);
	}

	private bool MethodIsOverloaded(CodeMemberMethod e, CodeTypeDeclaration c)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if ((((CodeTypeMember)e).Attributes & 0x100) != 0)
		{
			return true;
		}
		IEnumerator enumerator = ((CollectionBase)(object)c.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeMemberMethod)
			{
				CodeMemberMethod val = (CodeMemberMethod)enumerator.Current;
				if (!(enumerator.Current is CodeTypeConstructor) && !(enumerator.Current is CodeConstructor) && val != e && ((CodeTypeMember)val).Name.Equals(((CodeTypeMember)e).Name) && val.PrivateImplementationType == null)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void OutputArrayPostfix(CodeTypeReference typeRef)
	{
		if (typeRef.ArrayRank > 0)
		{
			base.Output.Write(GetArrayPostfix(typeRef));
		}
	}

	private void OutputAttributes(CodeAttributeDeclarationCollection attributes, bool inLine)
	{
		OutputAttributes(attributes, inLine, null, closingLine: false);
	}

	private void OutputAttributes(CodeAttributeDeclarationCollection attributes, bool inLine, string prefix, bool closingLine)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		if (((CollectionBase)(object)attributes).Count == 0)
		{
			return;
		}
		IEnumerator enumerator = ((CollectionBase)(object)attributes).GetEnumerator();
		bool flag = true;
		GenerateAttributeDeclarationsStart(attributes);
		while (enumerator.MoveNext())
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				base.Output.Write(", ");
				if (!inLine)
				{
					ContinueOnNewLine("");
					base.Output.Write(" ");
				}
			}
			if (!string.IsNullOrEmpty(prefix))
			{
				base.Output.Write(prefix);
			}
			CodeAttributeDeclaration val = (CodeAttributeDeclaration)enumerator.Current;
			base.Output.Write(GetBaseTypeOutput(val.Name));
			base.Output.Write("(");
			bool flag2 = true;
			foreach (CodeAttributeArgument item in (CollectionBase)(object)val.Arguments)
			{
				CodeAttributeArgument arg = item;
				if (flag2)
				{
					flag2 = false;
				}
				else
				{
					base.Output.Write(", ");
				}
				OutputAttributeArgument(arg);
			}
			base.Output.Write(")");
		}
		GenerateAttributeDeclarationsEnd(attributes);
		base.Output.Write(" ");
		if (!inLine)
		{
			if (closingLine)
			{
				base.Output.WriteLine();
			}
			else
			{
				ContinueOnNewLine("");
			}
		}
	}

	private void OutputTypeAttributes(CodeTypeDeclaration e)
	{
		TypeAttributes typeAttributes = e.TypeAttributes;
		switch (typeAttributes & TypeAttributes.VisibilityMask)
		{
		case TypeAttributes.Public:
		case TypeAttributes.NestedPublic:
			base.Output.Write("Public ");
			break;
		case TypeAttributes.NestedPrivate:
			base.Output.Write("Private ");
			break;
		}
		if (e.IsStruct)
		{
			base.Output.Write("Structure ");
			return;
		}
		if (e.IsEnum)
		{
			base.Output.Write("Enum ");
			return;
		}
		switch (typeAttributes & TypeAttributes.ClassSemanticsMask)
		{
		case TypeAttributes.NotPublic:
			if ((typeAttributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
			{
				base.Output.Write("NotInheritable ");
			}
			if ((typeAttributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
			{
				base.Output.Write("MustInherit ");
			}
			base.Output.Write("Class ");
			break;
		case TypeAttributes.ClassSemanticsMask:
			base.Output.Write("Interface ");
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
			base.Output.Write("Shadows ");
		}
	}

	protected override void OutputAttributeArgument(CodeAttributeArgument arg)
	{
		if (!string.IsNullOrEmpty(arg.Name))
		{
			OutputIdentifier(arg.Name);
			base.Output.Write(":=");
		}
		GenerateCodeFromExpression(arg.Value, ((IndentedTextWriter)base.Output).InnerWriter, base.Options);
	}

	protected override void OutputDirection(FieldDirection dir)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected I4, but got Unknown
		switch ((int)dir)
		{
		case 0:
			base.Output.Write("ByVal ");
			break;
		case 1:
		case 2:
			base.Output.Write("ByRef ");
			break;
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
		case 0:
			base.Output.Write("");
			break;
		case 1:
			base.Output.Write("Shared ");
			break;
		case 3:
			base.Output.Write("Const ");
			break;
		default:
			base.Output.Write("");
			break;
		}
	}

	protected override void OutputIdentifier(string ident)
	{
		base.Output.Write(CreateEscapedIdentifier(ident));
	}

	protected override void OutputMemberAccessModifier(MemberAttributes attributes)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		MemberAttributes val = (MemberAttributes)(attributes & 0xF000);
		if ((int)val <= 12288)
		{
			if ((int)val == 4096)
			{
				base.Output.Write("Friend ");
			}
			else if ((int)val == 8192)
			{
				base.Output.Write("Friend ");
			}
			else if ((int)val == 12288)
			{
				base.Output.Write("Protected ");
			}
		}
		else if ((int)val == 16384)
		{
			base.Output.Write("Protected ");
		}
		else if ((int)val == 20480)
		{
			base.Output.Write("Private ");
		}
		else if ((int)val == 24576)
		{
			base.Output.Write("Public ");
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
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		MemberAttributes val = (MemberAttributes)(attributes & 0xF);
		switch (val - 1)
		{
		case 0:
			base.Output.Write("MustOverride ");
			return;
		case 1:
			base.Output.Write("");
			return;
		case 2:
			base.Output.Write("Shared ");
			return;
		case 3:
			base.Output.Write("Overrides ");
			return;
		}
		if ((int)val == 20480)
		{
			base.Output.Write("Private ");
			return;
		}
		val = (MemberAttributes)(attributes & 0xF000);
		if ((int)val == 12288 || (int)val == 24576)
		{
			base.Output.Write("Overridable ");
		}
	}

	protected override void OutputOperator(CodeBinaryOperatorType op)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected I4, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		switch (op - 4)
		{
		case 0:
			base.Output.Write("Mod");
			break;
		case 1:
			OutputOperator(op);
			break;
		case 2:
			base.Output.Write("<>");
			break;
		case 3:
			base.Output.Write("Is");
			break;
		case 4:
			base.Output.Write("=");
			break;
		case 5:
			base.Output.Write("Or");
			break;
		case 6:
			base.Output.Write("And");
			break;
		case 7:
			base.Output.Write("OrElse");
			break;
		case 8:
			base.Output.Write("AndAlso");
			break;
		default:
			base.OutputOperator(op);
			break;
		}
	}

	protected override void OutputType(CodeTypeReference typeRef)
	{
		base.Output.Write(GetBaseTypeOutput(typeRef.BaseType));
	}

	protected override void OutputTypeNamePair(CodeTypeReference typeRef, string name)
	{
		OutputIdentifier(name);
		OutputArrayPostfix(typeRef);
		base.Output.Write(" As ");
		OutputType(typeRef);
	}

	private bool PropertyIsOverloaded(CodeMemberProperty e, CodeTypeDeclaration c)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if ((((CodeTypeMember)e).Attributes & 0x100) != 0)
		{
			return true;
		}
		IEnumerator enumerator = ((CollectionBase)(object)c.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeMemberProperty)
			{
				CodeMemberProperty val = (CodeMemberProperty)enumerator.Current;
				if (val != e && ((CodeTypeMember)val).Name.Equals(((CodeTypeMember)e).Name) && val.PrivateImplementationType == null)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected bool RequireVariableDeclaration(CodeCompileUnit e)
	{
		object obj = ((CodeObject)e).UserData["RequireVariableDeclaration"];
		if (obj != null && (bool)obj)
		{
			return (bool)obj;
		}
		return true;
	}

	protected override bool Supports(GeneratorSupport support)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return (GeneratorSupport)(support & 0x1FFFFF) == support;
	}

	public override string QuoteSnippetString(string value)
	{
		StringBuilder stringBuilder = new StringBuilder(value.Length + 5);
		bool fInDoubleQuotes = true;
		stringBuilder.Append("\"");
		int num = 0;
		while (num < value.Length)
		{
			char c = value[num];
			char c2 = c;
			if (c2 <= '"')
			{
				switch (c2)
				{
				case '\t':
					break;
				case '\n':
					goto IL_0084;
				case '\r':
					goto IL_009e;
				case '"':
					goto IL_0121;
				case '\0':
					goto IL_014a;
				default:
					goto IL_0161;
				}
				EnsureNotInDoubleQuotes(ref fInDoubleQuotes, stringBuilder);
				stringBuilder.Append("&Microsoft.VisualBasic.ChrW(9)");
				goto IL_0178;
			}
			if ((uint)c2 <= 8221u)
			{
				if (c2 == '“' || c2 == '”')
				{
					goto IL_0121;
				}
			}
			else if (c2 == '\u2028' || c2 == '\u2029')
			{
				EnsureNotInDoubleQuotes(ref fInDoubleQuotes, stringBuilder);
				AppendEscapedChar(stringBuilder, c);
				goto IL_0178;
			}
			if (c2 == '＂')
			{
				goto IL_0121;
			}
			goto IL_0161;
			IL_0178:
			if (num > 0 && num % 80 == 0)
			{
				if (fInDoubleQuotes)
				{
					stringBuilder.Append("\"");
				}
				fInDoubleQuotes = true;
				stringBuilder.Append("& _ \r\n\"");
			}
			num++;
			continue;
			IL_0084:
			EnsureNotInDoubleQuotes(ref fInDoubleQuotes, stringBuilder);
			stringBuilder.Append("&Microsoft.VisualBasic.ChrW(10)");
			goto IL_0178;
			IL_014a:
			EnsureNotInDoubleQuotes(ref fInDoubleQuotes, stringBuilder);
			stringBuilder.Append("&Microsoft.VisualBasic.ChrW(0)");
			goto IL_0178;
			IL_009e:
			EnsureNotInDoubleQuotes(ref fInDoubleQuotes, stringBuilder);
			if (num < value.Length - 1 && value[num + 1] == '\n')
			{
				stringBuilder.Append("&Microsoft.VisualBasic.ChrW(13)&Microsoft.VisualBasic.ChrW(10)");
				num++;
			}
			else
			{
				stringBuilder.Append("&Microsoft.VisualBasic.ChrW(13)");
			}
			goto IL_0178;
			IL_0161:
			EnsureInDoubleQuotes(ref fInDoubleQuotes, stringBuilder);
			stringBuilder.Append(value[num]);
			goto IL_0178;
			IL_0121:
			EnsureInDoubleQuotes(ref fInDoubleQuotes, stringBuilder);
			stringBuilder.Append(c);
			stringBuilder.Append(c);
			goto IL_0178;
		}
		if (fInDoubleQuotes)
		{
			stringBuilder.Append("\"");
		}
		return stringBuilder.ToString();
	}

	public override string GetRegionStartWord()
	{
		return "#Region";
	}

	public override string GetRegionEndWord()
	{
		return "#End Region";
	}

	public override string GetRegionStart(string s)
	{
		return GetRegionStartWord() + " \"" + s + "\"";
	}

	public override string GetRegionEnd(string s)
	{
		return GetRegionEndWord() + " '" + s;
	}

	protected override void GenerateRegionStart(StiCodeRegionStart e)
	{
		base.Output.Write(GetRegionStart(e.Text));
		base.Output.WriteLine(string.Empty);
	}

	protected override void GenerateRegionEnd(StiCodeRegionEnd e)
	{
		base.Output.Write(GetRegionEnd(e.Text));
		base.Output.WriteLine(string.Empty);
	}

	public override string GetClassKeyword()
	{
		return "Class";
	}
}
