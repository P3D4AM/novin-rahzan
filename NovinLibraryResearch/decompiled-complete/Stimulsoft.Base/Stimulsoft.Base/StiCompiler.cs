using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace Stimulsoft.Base;

public sealed class StiCompiler
{
	public enum LanguageType
	{
		CSharp,
		VB
	}

	public static string AssemblyVersion { get; set; }

	public static CompilerResults Compile(string textToCompile, string outputAssembly, LanguageType languageType, StiOutputType outputType, string[] referencedAssemblies)
	{
		return Compile(textToCompile, outputAssembly, languageType, outputType, referencedAssemblies, null);
	}

	public static CompilerResults Compile(string textToCompile, string outputAssembly, LanguageType languageType, StiOutputType outputType, string[] referencedAssemblies, List<string> resources)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		CompilerParameters val = new CompilerParameters();
		if (resources != null)
		{
			foreach (string resource in resources)
			{
				val.EmbeddedResources.Add(resource);
			}
		}
		val.ReferencedAssemblies.AddRange(referencedAssemblies);
		val.OutputAssembly = outputAssembly;
		val.GenerateInMemory = outputAssembly.Length == 0;
		switch (outputType)
		{
		case StiOutputType.ClassLibrary:
			val.CompilerOptions += " /target:library";
			val.GenerateExecutable = false;
			break;
		case StiOutputType.ConsoleApplication:
			val.CompilerOptions += " /target:exe";
			val.GenerateExecutable = true;
			break;
		case StiOutputType.WindowsApplication:
			val.CompilerOptions += " /target:winexe";
			val.GenerateExecutable = true;
			break;
		}
		CodeDomProvider val2 = (CodeDomProvider)((languageType != 0) ? ((object)new VBCodeProvider()) : ((object)new CSharpCodeProvider()));
		if (AssemblyVersion != null)
		{
			int startIndex = textToCompile.IndexOf("namespace", StringComparison.Ordinal);
			textToCompile = textToCompile.Insert(startIndex, "[assembly: System.Reflection.AssemblyVersion(\"" + AssemblyVersion + "\")]\n");
		}
		CompilerResults result = val2.CompileAssemblyFromSource(val, new string[1] { textToCompile });
		((Component)(object)val2).Dispose();
		return result;
	}
}
