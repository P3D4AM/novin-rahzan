using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CodeDom;

public class StiCodeDomEventHelper
{
	public static string GetEventMethod(StiEvent ev, StiReport report)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		StiCodeGenerator stiCodeGenerator = report.Language.GetProvider().CreateGenerator() as StiCodeGenerator;
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder);
		CodeGeneratorOptions o = new CodeGeneratorOptions
		{
			BlankLinesBetweenMembers = true,
			BracingStyle = "C"
		};
		CodeMemberMethod val = new CodeMemberMethod
		{
			Attributes = (MemberAttributes)24578,
			Name = ev.Script
		};
		CodeParameterDeclarationExpressionCollection val2 = new CodeParameterDeclarationExpressionCollection();
		StiParameterInfo[] parameters = ev.GetParameters();
		StiParameterInfo[] array = parameters;
		foreach (StiParameterInfo stiParameterInfo in array)
		{
			CodeParameterDeclarationExpression val3 = new CodeParameterDeclarationExpression(stiParameterInfo.Type, stiParameterInfo.Name);
			val2.Add(val3);
		}
		val.Parameters.AddRange(val2);
		CodeTypeDeclaration val4 = new CodeTypeDeclaration("Sample")
		{
			IsClass = true
		};
		((CodeTypeMember)val).Comments.Add((CodeCommentStatement)(object)new StiCodeRegionStart("Sample"));
		((CodeTypeMember)val).Comments.Add((CodeCommentStatement)(object)new StiCodeRegionEnd("Sample"));
		val4.Members.Add((CodeTypeMember)(object)val);
		val.Statements.Add((CodeStatement)new CodeCommentStatement(""));
		stiCodeGenerator.GenerateCodeFromType(val4, stringWriter, o);
		string text = stringBuilder.ToString();
		string regionStart = stiCodeGenerator.GetRegionStart("Sample");
		string regionEnd = stiCodeGenerator.GetRegionEnd("Sample");
		int num = text.IndexOf(regionStart, StringComparison.InvariantCulture);
		text = text.Remove(0, num + regionStart.Length);
		int num2 = text.IndexOf(regionEnd, StringComparison.InvariantCulture);
		text = text.Remove(num2, text.Length - num2);
		stringWriter.Flush();
		stringWriter.Close();
		return text;
	}
}
