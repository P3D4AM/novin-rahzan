using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Stimulsoft.Report.CodeDom;

public abstract class StiCodeGenerator : ICodeGenerator
{
	private const int ParameterMultilineThreshold = 15;

	private IndentedTextWriter output;

	private CodeGeneratorOptions options;

	private CodeTypeDeclaration currentClass;

	private CodeTypeMember currentMember;

	private bool inNestedBinary;

	protected string CurrentTypeName
	{
		get
		{
			if (currentClass != null)
			{
				return ((CodeTypeMember)currentClass).Name;
			}
			return "<% unknown %>";
		}
	}

	protected CodeTypeMember CurrentMember
	{
		get
		{
			return currentMember;
		}
		set
		{
			currentMember = value;
		}
	}

	protected string CurrentMemberName
	{
		get
		{
			if (currentMember != null)
			{
				return currentMember.Name;
			}
			return "<% unknown %>";
		}
	}

	protected bool IsCurrentInterface
	{
		get
		{
			if (currentClass != null && !(currentClass is CodeTypeDelegate))
			{
				return currentClass.IsInterface;
			}
			return false;
		}
	}

	protected bool IsCurrentClass
	{
		get
		{
			if (currentClass != null && !(currentClass is CodeTypeDelegate))
			{
				return currentClass.IsClass;
			}
			return false;
		}
	}

	protected bool IsCurrentStruct
	{
		get
		{
			if (currentClass != null && !(currentClass is CodeTypeDelegate))
			{
				return currentClass.IsStruct;
			}
			return false;
		}
	}

	protected bool IsCurrentEnum
	{
		get
		{
			if (currentClass != null && !(currentClass is CodeTypeDelegate))
			{
				return currentClass.IsEnum;
			}
			return false;
		}
	}

	protected bool IsCurrentDelegate
	{
		get
		{
			if (currentClass != null && currentClass is CodeTypeDelegate)
			{
				return true;
			}
			return false;
		}
	}

	protected int Indent
	{
		get
		{
			return output.Indent;
		}
		set
		{
			output.Indent = value;
		}
	}

	protected abstract string NullToken { get; }

	protected TextWriter Output => output;

	protected CodeGeneratorOptions Options => options;

	private void GenerateType(CodeTypeDeclaration e)
	{
		currentClass = e;
		GenerateRegionStarts(((CodeTypeMember)e).Comments);
		GenerateTypeStart(e);
		GenerateTypeConstructors(e);
		GenerateConstructors(e);
		GenerateFields(e);
		GenerateSnippetMembers(e);
		GenerateProperties(e);
		GenerateEvents(e);
		GenerateMethods(e);
		GenerateNestedTypes(e);
		currentClass = e;
		GenerateTypeEnd(e);
		GenerateRegionEnds(((CodeTypeMember)e).Comments);
	}

	private void GenerateTypeConstructors(CodeTypeDeclaration e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)e.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeTypeConstructor)
			{
				currentMember = (CodeTypeMember)enumerator.Current;
				if (options.BlankLinesBetweenMembers)
				{
					Output.WriteLine();
				}
				GenerateRegionStarts(currentMember.Comments);
				CodeTypeConstructor val = (CodeTypeConstructor)enumerator.Current;
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaStart(((CodeTypeMember)val).LinePragma);
				}
				GenerateTypeConstructor(val);
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaEnd(((CodeTypeMember)val).LinePragma);
				}
				GenerateRegionEnds(currentMember.Comments);
			}
		}
	}

	protected virtual void GenerateNamespaces(CodeCompileUnit e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (CodeNamespace item in (CollectionBase)(object)e.Namespaces)
		{
			CodeNamespace val = item;
			((ICodeGenerator)this).GenerateCodeFromNamespace(val, output.InnerWriter, options);
		}
	}

	protected void GenerateTypes(CodeNamespace e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (CodeTypeDeclaration item in (CollectionBase)(object)e.Types)
		{
			CodeTypeDeclaration val = item;
			if (options.BlankLinesBetweenMembers)
			{
				Output.WriteLine();
			}
			((ICodeGenerator)this).GenerateCodeFromType(val, output.InnerWriter, options);
		}
	}

	bool ICodeGenerator.Supports(GeneratorSupport support)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Supports(support);
	}

	public void GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (output != null && w != output.InnerWriter)
		{
			throw new InvalidOperationException("CodeGenOutputWriter");
		}
		if (output == null)
		{
			flag = true;
			options = (CodeGeneratorOptions)((o != null) ? ((object)o) : ((object)new CodeGeneratorOptions()));
			output = new IndentedTextWriter(w, options.IndentString);
		}
		try
		{
			GenerateType(e);
		}
		finally
		{
			if (flag)
			{
				output = null;
				options = null;
			}
		}
	}

	public void GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (output != null && w != output.InnerWriter)
		{
			throw new InvalidOperationException("CodeGenOutputWriter");
		}
		if (output == null)
		{
			flag = true;
			options = (CodeGeneratorOptions)((o != null) ? ((object)o) : ((object)new CodeGeneratorOptions()));
			output = new IndentedTextWriter(w, options.IndentString);
		}
		try
		{
			GenerateExpression(e);
		}
		finally
		{
			if (flag)
			{
				output = null;
				options = null;
			}
		}
	}

	public void GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		bool flag = false;
		if (output != null && w != output.InnerWriter)
		{
			throw new InvalidOperationException("CodeGenOutputWriter");
		}
		if (output == null)
		{
			flag = true;
			options = (CodeGeneratorOptions)((o != null) ? ((object)o) : ((object)new CodeGeneratorOptions()));
			output = new IndentedTextWriter(w, options.IndentString);
		}
		try
		{
			if (e is CodeSnippetCompileUnit)
			{
				GenerateSnippetCompileUnit((CodeSnippetCompileUnit)e);
			}
			else
			{
				GenerateCompileUnit(e);
			}
		}
		finally
		{
			if (flag)
			{
				output = null;
				options = null;
			}
		}
	}

	public void GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (output != null && w != output.InnerWriter)
		{
			throw new InvalidOperationException("CodeGenOutputWriter");
		}
		if (output == null)
		{
			flag = true;
			options = (CodeGeneratorOptions)((o != null) ? ((object)o) : ((object)new CodeGeneratorOptions()));
			output = new IndentedTextWriter(w, options.IndentString);
		}
		try
		{
			GenerateNamespace(e);
		}
		finally
		{
			if (flag)
			{
				output = null;
				options = null;
			}
		}
	}

	public void GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (output != null && w != output.InnerWriter)
		{
			throw new InvalidOperationException("CodeGenOutputWriter");
		}
		if (output == null)
		{
			flag = true;
			options = (CodeGeneratorOptions)((o != null) ? ((object)o) : ((object)new CodeGeneratorOptions()));
			output = new IndentedTextWriter(w, options.IndentString);
		}
		try
		{
			GenerateStatement(e);
		}
		finally
		{
			if (flag)
			{
				output = null;
				options = null;
			}
		}
	}

	protected abstract string GetTypeOutput(CodeTypeReference value);

	bool ICodeGenerator.IsValidIdentifier(string value)
	{
		return IsValidIdentifier(value);
	}

	void ICodeGenerator.ValidateIdentifier(string value)
	{
		ValidateIdentifier(value);
	}

	string ICodeGenerator.CreateEscapedIdentifier(string value)
	{
		return CreateEscapedIdentifier(value);
	}

	string ICodeGenerator.CreateValidIdentifier(string value)
	{
		return CreateValidIdentifier(value);
	}

	string ICodeGenerator.GetTypeOutput(CodeTypeReference type)
	{
		return GetTypeOutput(type);
	}

	private void GenerateConstructors(CodeTypeDeclaration e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)e.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeConstructor)
			{
				currentMember = (CodeTypeMember)enumerator.Current;
				if (options.BlankLinesBetweenMembers)
				{
					Output.WriteLine();
				}
				GenerateRegionStarts(currentMember.Comments);
				CodeConstructor val = (CodeConstructor)enumerator.Current;
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaStart(((CodeTypeMember)val).LinePragma);
				}
				GenerateConstructor(val, e);
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaEnd(((CodeTypeMember)val).LinePragma);
				}
				GenerateRegionEnds(currentMember.Comments);
			}
		}
	}

	private void GenerateEvents(CodeTypeDeclaration e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)e.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeMemberEvent)
			{
				currentMember = (CodeTypeMember)enumerator.Current;
				if (options.BlankLinesBetweenMembers)
				{
					Output.WriteLine();
				}
				GenerateRegionStarts(currentMember.Comments);
				CodeMemberEvent val = (CodeMemberEvent)enumerator.Current;
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaStart(((CodeTypeMember)val).LinePragma);
				}
				GenerateEvent(val, e);
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaEnd(((CodeTypeMember)val).LinePragma);
				}
				GenerateRegionEnds(currentMember.Comments);
			}
		}
	}

	protected void GenerateExpression(CodeExpression e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		if (e is CodeArrayCreateExpression)
		{
			GenerateArrayCreateExpression((CodeArrayCreateExpression)e);
		}
		else if (e is CodeBaseReferenceExpression)
		{
			GenerateBaseReferenceExpression((CodeBaseReferenceExpression)e);
		}
		else if (e is CodeBinaryOperatorExpression)
		{
			GenerateBinaryOperatorExpression((CodeBinaryOperatorExpression)e);
		}
		else if (e is CodeCastExpression)
		{
			GenerateCastExpression((CodeCastExpression)e);
		}
		else if (e is CodeDelegateCreateExpression)
		{
			GenerateDelegateCreateExpression((CodeDelegateCreateExpression)e);
		}
		else if (e is CodeFieldReferenceExpression)
		{
			GenerateFieldReferenceExpression((CodeFieldReferenceExpression)e);
		}
		else if (e is CodeArgumentReferenceExpression)
		{
			GenerateArgumentReferenceExpression((CodeArgumentReferenceExpression)e);
		}
		else if (e is CodeVariableReferenceExpression)
		{
			GenerateVariableReferenceExpression((CodeVariableReferenceExpression)e);
		}
		else if (e is CodeIndexerExpression)
		{
			GenerateIndexerExpression((CodeIndexerExpression)e);
		}
		else if (e is CodeArrayIndexerExpression)
		{
			GenerateArrayIndexerExpression((CodeArrayIndexerExpression)e);
		}
		else if (e is CodeSnippetExpression)
		{
			GenerateSnippetExpression((CodeSnippetExpression)e);
		}
		else if (e is CodeMethodInvokeExpression)
		{
			GenerateMethodInvokeExpression((CodeMethodInvokeExpression)e);
		}
		else if (e is CodeMethodReferenceExpression)
		{
			GenerateMethodReferenceExpression((CodeMethodReferenceExpression)e);
		}
		else if (e is CodeEventReferenceExpression)
		{
			GenerateEventReferenceExpression((CodeEventReferenceExpression)e);
		}
		else if (e is CodeDelegateInvokeExpression)
		{
			GenerateDelegateInvokeExpression((CodeDelegateInvokeExpression)e);
		}
		else if (e is CodeObjectCreateExpression)
		{
			GenerateObjectCreateExpression((CodeObjectCreateExpression)e);
		}
		else if (e is CodeParameterDeclarationExpression)
		{
			GenerateParameterDeclarationExpression((CodeParameterDeclarationExpression)e);
		}
		else if (e is CodeDirectionExpression)
		{
			GenerateDirectionExpression((CodeDirectionExpression)e);
		}
		else if (e is CodePrimitiveExpression)
		{
			GeneratePrimitiveExpression((CodePrimitiveExpression)e);
		}
		else if (e is CodePropertyReferenceExpression)
		{
			GeneratePropertyReferenceExpression((CodePropertyReferenceExpression)e);
		}
		else if (e is CodePropertySetValueReferenceExpression)
		{
			GeneratePropertySetValueReferenceExpression((CodePropertySetValueReferenceExpression)e);
		}
		else if (e is CodeThisReferenceExpression)
		{
			GenerateThisReferenceExpression((CodeThisReferenceExpression)e);
		}
		else if (e is CodeTypeReferenceExpression)
		{
			GenerateTypeReferenceExpression((CodeTypeReferenceExpression)e);
		}
		else if (e is CodeTypeOfExpression)
		{
			GenerateTypeOfExpression((CodeTypeOfExpression)e);
		}
		else if (e == null)
		{
			throw new ArgumentNullException("e");
		}
	}

	private void GenerateFields(CodeTypeDeclaration e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)e.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeMemberField)
			{
				currentMember = (CodeTypeMember)enumerator.Current;
				GenerateRegionStarts(currentMember.Comments);
				CodeMemberField val = (CodeMemberField)enumerator.Current;
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaStart(((CodeTypeMember)val).LinePragma);
				}
				GenerateField(val);
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaEnd(((CodeTypeMember)val).LinePragma);
				}
				GenerateRegionEnds(currentMember.Comments);
			}
		}
	}

	private void GenerateSnippetMembers(CodeTypeDeclaration e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)e.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeSnippetTypeMember)
			{
				currentMember = (CodeTypeMember)enumerator.Current;
				if (options.BlankLinesBetweenMembers)
				{
					Output.WriteLine();
				}
				GenerateRegionStarts(currentMember.Comments);
				CodeSnippetTypeMember val = (CodeSnippetTypeMember)enumerator.Current;
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaStart(((CodeTypeMember)val).LinePragma);
				}
				GenerateSnippetMember(val);
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaEnd(((CodeTypeMember)val).LinePragma);
				}
				GenerateRegionEnds(currentMember.Comments);
			}
		}
	}

	protected virtual void GenerateSnippetCompileUnit(CodeSnippetCompileUnit e)
	{
		if (e.LinePragma != null)
		{
			GenerateLinePragmaStart(e.LinePragma);
		}
		Output.WriteLine(e.Value);
		if (e.LinePragma != null)
		{
			GenerateLinePragmaEnd(e.LinePragma);
		}
	}

	protected virtual void GenerateMethods(CodeTypeDeclaration e)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)e.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeMemberMethod && !(enumerator.Current is CodeTypeConstructor) && !(enumerator.Current is CodeConstructor))
			{
				currentMember = (CodeTypeMember)enumerator.Current;
				if (options.BlankLinesBetweenMembers)
				{
					Output.WriteLine();
				}
				GenerateRegionStarts(currentMember.Comments);
				CodeMemberMethod val = (CodeMemberMethod)enumerator.Current;
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaStart(((CodeTypeMember)val).LinePragma);
				}
				if (enumerator.Current is CodeEntryPointMethod)
				{
					GenerateEntryPointMethod((CodeEntryPointMethod)enumerator.Current, e);
				}
				else
				{
					GenerateMethod(val, e);
				}
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaEnd(((CodeTypeMember)val).LinePragma);
				}
				GenerateRegionEnds(currentMember.Comments);
			}
		}
	}

	private void GenerateNestedTypes(CodeTypeDeclaration e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)e.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeTypeDeclaration)
			{
				if (options.BlankLinesBetweenMembers)
				{
					Output.WriteLine();
				}
				CodeTypeDeclaration val = (CodeTypeDeclaration)enumerator.Current;
				((ICodeGenerator)this).GenerateCodeFromType(val, output.InnerWriter, options);
			}
		}
	}

	protected virtual void GenerateCompileUnit(CodeCompileUnit e)
	{
		GenerateCompileUnitStart(e);
		GenerateNamespaces(e);
		GenerateCompileUnitEnd(e);
	}

	protected virtual void GenerateNamespace(CodeNamespace e)
	{
		GenerateNamespaceImports(e);
		Output.WriteLine(string.Empty);
		GenerateRegionStarts(e.Comments);
		GenerateNamespaceStart(e);
		GenerateTypes(e);
		GenerateNamespaceEnd(e);
		GenerateRegionEnds(e.Comments);
	}

	protected virtual void GenerateNamespaceImports(CodeNamespace e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		IEnumerator enumerator = e.Imports.GetEnumerator();
		while (enumerator.MoveNext())
		{
			CodeNamespaceImport val = (CodeNamespaceImport)enumerator.Current;
			if (val.LinePragma != null)
			{
				GenerateLinePragmaStart(val.LinePragma);
			}
			GenerateNamespaceImport(val);
			if (val.LinePragma != null)
			{
				GenerateLinePragmaEnd(val.LinePragma);
			}
		}
	}

	private void GenerateProperties(CodeTypeDeclaration e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)e.Members).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current is CodeMemberProperty)
			{
				currentMember = (CodeTypeMember)enumerator.Current;
				if (options.BlankLinesBetweenMembers)
				{
					Output.WriteLine();
				}
				GenerateRegionStarts(currentMember.Comments);
				CodeMemberProperty val = (CodeMemberProperty)enumerator.Current;
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaStart(((CodeTypeMember)val).LinePragma);
				}
				GenerateProperty(val, e);
				if (((CodeTypeMember)val).LinePragma != null)
				{
					GenerateLinePragmaEnd(((CodeTypeMember)val).LinePragma);
				}
				GenerateRegionEnds(currentMember.Comments);
			}
		}
	}

	protected virtual void GenerateStatement(CodeStatement e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		if (e.LinePragma != null)
		{
			GenerateLinePragmaStart(e.LinePragma);
		}
		if (e is CodeCommentStatement)
		{
			GenerateCommentStatement((CodeCommentStatement)e);
		}
		else if (e is CodeMethodReturnStatement)
		{
			GenerateMethodReturnStatement((CodeMethodReturnStatement)e);
		}
		else if (e is CodeConditionStatement)
		{
			GenerateConditionStatement((CodeConditionStatement)e);
		}
		else if (e is CodeTryCatchFinallyStatement)
		{
			GenerateTryCatchFinallyStatement((CodeTryCatchFinallyStatement)e);
		}
		else if (e is CodeAssignStatement)
		{
			GenerateAssignStatement((CodeAssignStatement)e);
		}
		else if (e is CodeExpressionStatement)
		{
			GenerateExpressionStatement((CodeExpressionStatement)e);
		}
		else if (e is CodeIterationStatement)
		{
			GenerateIterationStatement((CodeIterationStatement)e);
		}
		else if (e is CodeThrowExceptionStatement)
		{
			GenerateThrowExceptionStatement((CodeThrowExceptionStatement)e);
		}
		else if (e is CodeSnippetStatement)
		{
			GenerateSnippetStatement((CodeSnippetStatement)e);
		}
		else if (e is CodeVariableDeclarationStatement)
		{
			GenerateVariableDeclarationStatement((CodeVariableDeclarationStatement)e);
		}
		else if (e is CodeAttachEventStatement)
		{
			GenerateAttachEventStatement((CodeAttachEventStatement)e);
		}
		else if (e is CodeRemoveEventStatement)
		{
			GenerateRemoveEventStatement((CodeRemoveEventStatement)e);
		}
		else if (e is CodeGotoStatement)
		{
			GenerateGotoStatement((CodeGotoStatement)e);
		}
		else if (e is CodeLabeledStatement)
		{
			GenerateLabeledStatement((CodeLabeledStatement)e);
		}
		if (e.LinePragma != null)
		{
			GenerateLinePragmaEnd(e.LinePragma);
		}
	}

	protected void GenerateStatements(CodeStatementCollection stms)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		IEnumerator enumerator = ((CollectionBase)(object)stms).GetEnumerator();
		while (enumerator.MoveNext())
		{
			((ICodeGenerator)this).GenerateCodeFromStatement((CodeStatement)enumerator.Current, output.InnerWriter, options);
		}
	}

	protected virtual void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		if (((CollectionBase)(object)attributes).Count == 0)
		{
			return;
		}
		GenerateAttributeDeclarationsStart(attributes);
		bool flag = true;
		IEnumerator enumerator = ((CollectionBase)(object)attributes).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				ContinueOnNewLine(", ");
			}
			CodeAttributeDeclaration val = (CodeAttributeDeclaration)enumerator.Current;
			Output.Write(val.Name);
			Output.Write("(");
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
					Output.Write(", ");
				}
				OutputAttributeArgument(arg);
			}
			Output.Write(")");
		}
		GenerateAttributeDeclarationsEnd(attributes);
	}

	protected virtual void OutputAttributeArgument(CodeAttributeArgument arg)
	{
		if (!string.IsNullOrEmpty(arg.Name))
		{
			OutputIdentifier(arg.Name);
			Output.Write("=");
		}
		((ICodeGenerator)this).GenerateCodeFromExpression(arg.Value, output.InnerWriter, options);
	}

	protected virtual void OutputDirection(FieldDirection dir)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected I4, but got Unknown
		switch ((int)dir)
		{
		case 1:
			Output.Write("out ");
			break;
		case 2:
			Output.Write("ref ");
			break;
		case 0:
			break;
		}
	}

	protected virtual void OutputFieldScopeModifier(MemberAttributes attributes)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected I4, but got Unknown
		MemberAttributes val = (MemberAttributes)(attributes & 0xF0);
		if ((int)val == 16)
		{
			Output.Write("new ");
		}
		MemberAttributes val2 = (MemberAttributes)(attributes & 0xF);
		switch (val2 - 2)
		{
		case 1:
			Output.Write("static ");
			break;
		case 3:
			Output.Write("const ");
			break;
		case 0:
		case 2:
			break;
		}
	}

	protected virtual void OutputMemberAccessModifier(MemberAttributes attributes)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		MemberAttributes val = (MemberAttributes)(attributes & 0xF000);
		if ((int)val <= 12288)
		{
			if ((int)val != 4096)
			{
				if ((int)val != 8192)
				{
					if ((int)val == 12288)
					{
						Output.Write("protected ");
					}
				}
				else
				{
					Output.Write("/*FamANDAssem*/ internal ");
				}
			}
			else
			{
				Output.Write("internal ");
			}
		}
		else if ((int)val != 16384)
		{
			if ((int)val != 20480)
			{
				if ((int)val == 24576)
				{
					Output.Write("public ");
				}
			}
			else
			{
				Output.Write("private ");
			}
		}
		else
		{
			Output.Write("protected internal ");
		}
	}

	protected virtual void OutputMemberScopeModifier(MemberAttributes attributes)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected I4, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		MemberAttributes val = (MemberAttributes)(attributes & 0xF0);
		if ((int)val == 16)
		{
			Output.Write("new ");
		}
		MemberAttributes val2 = (MemberAttributes)(attributes & 0xF);
		switch (val2 - 1)
		{
		case 0:
			Output.Write("abstract ");
			return;
		case 1:
			Output.Write(string.Empty);
			return;
		case 2:
			Output.Write("static ");
			return;
		case 3:
			Output.Write("override ");
			return;
		}
		MemberAttributes val3 = (MemberAttributes)(attributes & 0xF000);
		if ((int)val3 == 12288 || (int)val3 == 24576)
		{
			Output.Write("virtual ");
		}
	}

	protected abstract void OutputType(CodeTypeReference typeRef);

	protected virtual void OutputTypeAttributes(TypeAttributes attributes, bool isStruct, bool isEnum)
	{
		switch (attributes & TypeAttributes.VisibilityMask)
		{
		case TypeAttributes.Public:
		case TypeAttributes.NestedPublic:
			Output.Write("public ");
			break;
		case TypeAttributes.NestedPrivate:
			Output.Write("private ");
			break;
		}
		if (isStruct)
		{
			Output.Write("struct ");
			return;
		}
		if (isEnum)
		{
			Output.Write("enum ");
			return;
		}
		switch (attributes & TypeAttributes.ClassSemanticsMask)
		{
		case TypeAttributes.NotPublic:
			if ((attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed)
			{
				Output.Write("sealed ");
			}
			if ((attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract)
			{
				Output.Write("abstract ");
			}
			Output.Write("class ");
			break;
		case TypeAttributes.ClassSemanticsMask:
			Output.Write("interface ");
			break;
		}
	}

	protected virtual void OutputTypeNamePair(CodeTypeReference typeRef, string name)
	{
		OutputType(typeRef);
		Output.Write(" ");
		OutputIdentifier(name);
	}

	protected virtual void OutputIdentifier(string ident)
	{
		Output.Write(ident);
	}

	protected virtual void OutputExpressionList(CodeExpressionCollection expressions)
	{
		OutputExpressionList(expressions, newlineBetweenItems: false);
	}

	protected virtual void OutputExpressionList(CodeExpressionCollection expressions, bool newlineBetweenItems)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		bool flag = true;
		IEnumerator enumerator = ((CollectionBase)(object)expressions).GetEnumerator();
		Indent++;
		while (enumerator.MoveNext())
		{
			if (flag)
			{
				flag = false;
			}
			else if (newlineBetweenItems)
			{
				ContinueOnNewLine(",");
			}
			else
			{
				Output.Write(", ");
			}
			((ICodeGenerator)this).GenerateCodeFromExpression((CodeExpression)enumerator.Current, output.InnerWriter, options);
		}
		Indent--;
	}

	protected virtual void OutputOperator(CodeBinaryOperatorType op)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected I4, but got Unknown
		switch ((int)op)
		{
		case 0:
			Output.Write("+");
			break;
		case 1:
			Output.Write("-");
			break;
		case 2:
			Output.Write("*");
			break;
		case 3:
			Output.Write("/");
			break;
		case 4:
			Output.Write("%");
			break;
		case 5:
			Output.Write("=");
			break;
		case 6:
			Output.Write("!=");
			break;
		case 7:
			Output.Write("==");
			break;
		case 8:
			Output.Write("==");
			break;
		case 9:
			Output.Write("|");
			break;
		case 10:
			Output.Write("&");
			break;
		case 11:
			Output.Write("||");
			break;
		case 12:
			Output.Write("&&");
			break;
		case 13:
			Output.Write("<");
			break;
		case 14:
			Output.Write("<=");
			break;
		case 15:
			Output.Write(">");
			break;
		case 16:
			Output.Write(">=");
			break;
		}
	}

	protected virtual void OutputParameters(CodeParameterDeclarationExpressionCollection parameters)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		bool flag = true;
		bool flag2 = ((CollectionBase)(object)parameters).Count > 15;
		if (flag2)
		{
			Indent += 3;
		}
		IEnumerator enumerator = ((CollectionBase)(object)parameters).GetEnumerator();
		while (enumerator.MoveNext())
		{
			CodeParameterDeclarationExpression e = (CodeParameterDeclarationExpression)enumerator.Current;
			if (flag)
			{
				flag = false;
			}
			else
			{
				Output.Write(", ");
			}
			if (flag2)
			{
				ContinueOnNewLine(string.Empty);
			}
			GenerateExpression((CodeExpression)(object)e);
		}
		if (flag2)
		{
			Indent -= 3;
		}
	}

	protected abstract void GenerateArrayCreateExpression(CodeArrayCreateExpression e);

	protected abstract void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e);

	protected abstract void GenerateCastExpression(CodeCastExpression e);

	protected abstract void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e);

	protected abstract void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e);

	protected abstract void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e);

	protected abstract void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e);

	protected abstract void GenerateIndexerExpression(CodeIndexerExpression e);

	protected abstract void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e);

	protected abstract void GenerateSnippetExpression(CodeSnippetExpression e);

	protected abstract void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e);

	protected abstract void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e);

	protected abstract void GenerateEventReferenceExpression(CodeEventReferenceExpression e);

	protected abstract void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e);

	protected abstract void GenerateObjectCreateExpression(CodeObjectCreateExpression e);

	protected abstract void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e);

	protected abstract void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e);

	protected abstract void GenerateThisReferenceExpression(CodeThisReferenceExpression e);

	protected abstract void GenerateExpressionStatement(CodeExpressionStatement e);

	protected abstract void GenerateIterationStatement(CodeIterationStatement e);

	protected abstract void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e);

	protected abstract void GenerateComment(CodeComment e);

	protected abstract void GenerateRegionStart(StiCodeRegionStart e);

	protected abstract void GenerateRegionEnd(StiCodeRegionEnd e);

	protected abstract void GenerateMethodReturnStatement(CodeMethodReturnStatement e);

	protected abstract void GenerateConditionStatement(CodeConditionStatement e);

	protected abstract void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e);

	protected abstract void GenerateAssignStatement(CodeAssignStatement e);

	protected abstract void GenerateAttachEventStatement(CodeAttachEventStatement e);

	protected abstract void GenerateRemoveEventStatement(CodeRemoveEventStatement e);

	protected abstract void GenerateGotoStatement(CodeGotoStatement e);

	protected abstract void GenerateLabeledStatement(CodeLabeledStatement e);

	protected abstract void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e);

	protected abstract void GenerateLinePragmaStart(CodeLinePragma e);

	protected abstract void GenerateLinePragmaEnd(CodeLinePragma e);

	protected abstract void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c);

	protected abstract void GenerateField(CodeMemberField e);

	protected abstract void GenerateSnippetMember(CodeSnippetTypeMember e);

	protected abstract void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c);

	protected abstract void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c);

	protected abstract void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c);

	protected abstract void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c);

	protected abstract void GenerateTypeConstructor(CodeTypeConstructor e);

	protected abstract void GenerateTypeStart(CodeTypeDeclaration e);

	protected abstract void GenerateTypeEnd(CodeTypeDeclaration e);

	protected abstract void GenerateNamespaceStart(CodeNamespace e);

	protected abstract void GenerateNamespaceEnd(CodeNamespace e);

	protected abstract void GenerateNamespaceImport(CodeNamespaceImport e);

	protected abstract void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes);

	protected abstract void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes);

	public abstract string GetBaseTypeOutput(string baseType);

	public abstract string GetRegionStartWord();

	public abstract string GetRegionEndWord();

	public abstract string GetRegionStart(string s);

	public abstract string GetRegionEnd(string s);

	protected virtual void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e)
	{
		OutputType(e.Type);
	}

	protected virtual void GenerateTypeOfExpression(CodeTypeOfExpression e)
	{
		Output.Write("typeof(");
		OutputType(e.Type);
		Output.Write(")");
	}

	protected virtual void GenerateSnippetStatement(CodeSnippetStatement e)
	{
		Output.WriteLine(e.Value);
	}

	protected virtual void GenerateCompileUnitStart(CodeCompileUnit e)
	{
	}

	protected virtual void GenerateCompileUnitEnd(CodeCompileUnit e)
	{
	}

	protected virtual void GenerateCommentStatement(CodeCommentStatement e)
	{
		GenerateComment(e.Comment);
	}

	protected virtual void GenerateRegionStarts(CodeCommentStatementCollection e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (CodeCommentStatement item in (CollectionBase)(object)e)
		{
			CodeCommentStatement val = item;
			object obj = val;
			if (obj is StiCodeRegionStart)
			{
				GenerateRegionStart(obj as StiCodeRegionStart);
			}
			else if (!(obj is StiCodeRegionEnd))
			{
				GenerateCommentStatement(val);
			}
		}
	}

	protected virtual void GenerateRegionEnds(CodeCommentStatementCollection e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (CodeCommentStatement item in (CollectionBase)(object)e)
		{
			CodeCommentStatement val = item;
			object obj = val;
			if (obj is StiCodeRegionEnd)
			{
				GenerateRegionEnd(obj as StiCodeRegionEnd);
			}
		}
	}

	protected virtual void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (((CollectionBase)(object)e.CustomAttributes).Count > 0)
		{
			OutputAttributeDeclarations(e.CustomAttributes);
			Output.Write(" ");
		}
		OutputDirection(e.Direction);
		OutputTypeNamePair(e.Type, e.Name);
	}

	protected virtual void GenerateDirectionExpression(CodeDirectionExpression e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		OutputDirection(e.Direction);
		GenerateExpression(e.Expression);
	}

	protected virtual void GeneratePrimitiveExpression(CodePrimitiveExpression e)
	{
		if (e.Value == null)
		{
			Output.Write(NullToken);
		}
		else if (e.Value is string)
		{
			Output.Write(QuoteSnippetString((string)e.Value));
		}
		else if (e.Value is char)
		{
			Output.Write("'" + e.Value.ToString() + "'");
		}
		else if (e.Value is byte)
		{
			Output.Write(((byte)e.Value).ToString());
		}
		else if (e.Value is short)
		{
			Output.Write(((short)e.Value).ToString());
		}
		else if (e.Value is int)
		{
			Output.Write(((int)e.Value).ToString());
		}
		else if (e.Value is long)
		{
			Output.Write(((long)e.Value).ToString());
		}
		else if (e.Value is float)
		{
			GenerateSingleFloatValue((float)e.Value);
		}
		else if (e.Value is double)
		{
			GenerateDoubleValue((double)e.Value);
		}
		else if (e.Value is decimal)
		{
			GenerateDecimalValue((decimal)e.Value);
		}
		else if (e.Value is bool)
		{
			if ((bool)e.Value)
			{
				Output.Write("true");
			}
			else
			{
				Output.Write("false");
			}
		}
	}

	protected virtual void GenerateSingleFloatValue(float s)
	{
		Output.Write(s.ToString(CultureInfo.InvariantCulture));
	}

	protected virtual void GenerateDoubleValue(double d)
	{
		Output.Write(d.ToString("R", CultureInfo.InvariantCulture));
	}

	protected virtual void GenerateDecimalValue(decimal d)
	{
		Output.Write(d.ToString(CultureInfo.InvariantCulture));
	}

	protected virtual void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		Output.Write("(");
		GenerateExpression(e.Left);
		Output.Write(" ");
		if (e.Left is CodeBinaryOperatorExpression || e.Right is CodeBinaryOperatorExpression)
		{
			if (!inNestedBinary)
			{
				flag = true;
				inNestedBinary = true;
				Indent += 3;
			}
			ContinueOnNewLine(string.Empty);
		}
		OutputOperator(e.Operator);
		Output.Write(" ");
		GenerateExpression(e.Right);
		Output.Write(")");
		if (flag)
		{
			Indent -= 3;
			inNestedBinary = false;
		}
	}

	protected virtual void ContinueOnNewLine(string st)
	{
		Output.WriteLine(st);
	}

	protected abstract bool Supports(GeneratorSupport support);

	protected abstract bool IsValidIdentifier(string value);

	public abstract bool IsKeyword(string value);

	public abstract string GetClassKeyword();

	protected virtual void ValidateIdentifier(string value)
	{
		IsValidIdentifier(value);
	}

	protected abstract string CreateEscapedIdentifier(string value);

	protected abstract string CreateValidIdentifier(string value);

	public abstract string QuoteSnippetString(string value);

	public static bool IsValidLanguageIndependentIdentifier(string value)
	{
		char[] array = value.ToCharArray();
		if (array.Length == 0)
		{
			return false;
		}
		if (char.GetUnicodeCategory(array[0]) == UnicodeCategory.DecimalDigitNumber)
		{
			return false;
		}
		char[] array2 = array;
		foreach (char c in array2)
		{
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			if ((uint)unicodeCategory > 6u && unicodeCategory != UnicodeCategory.DecimalDigitNumber && unicodeCategory != UnicodeCategory.ConnectorPunctuation)
			{
				return false;
			}
		}
		return true;
	}
}
