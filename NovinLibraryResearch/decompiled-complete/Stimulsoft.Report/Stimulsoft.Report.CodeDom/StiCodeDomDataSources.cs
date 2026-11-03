using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CodeDom;

internal class StiCodeDomDataSources
{
	internal static void Serialize(StiCodeDomSerializator serializator, StiReport report)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00a2: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Expected O, but got Unknown
		//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b85: Expected O, but got Unknown
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Expected O, but got Unknown
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Expected O, but got Unknown
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Expected O, but got Unknown
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0765: Expected O, but got Unknown
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Expected O, but got Unknown
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Expected O, but got Unknown
		//IL_060f: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Expected O, but got Unknown
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Expected O, but got Unknown
		//IL_07e2: Expected O, but got Unknown
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Expected O, but got Unknown
		//IL_0698: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Expected O, but got Unknown
		//IL_0855: Expected O, but got Unknown
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Expected O, but got Unknown
		//IL_085b: Expected O, but got Unknown
		//IL_0856: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Expected O, but got Unknown
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Expected O, but got Unknown
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Expected O, but got Unknown
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Expected O, but got Unknown
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Expected O, but got Unknown
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Expected O, but got Unknown
		//IL_0721: Expected O, but got Unknown
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Expected O, but got Unknown
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a66: Expected O, but got Unknown
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Expected O, but got Unknown
		//IL_0a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a89: Expected O, but got Unknown
		//IL_0a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Expected O, but got Unknown
		//IL_0a8e: Expected O, but got Unknown
		//IL_0a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Expected O, but got Unknown
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Expected O, but got Unknown
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Expected O, but got Unknown
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Expected O, but got Unknown
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Expected O, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d8: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Expected O, but got Unknown
		//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Expected O, but got Unknown
		//IL_09fc: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Expected O, but got Unknown
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Expected O, but got Unknown
		//IL_0a12: Expected O, but got Unknown
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a17: Expected O, but got Unknown
		if (report.Dictionary.DataSources.Count <= 0)
		{
			return;
		}
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			dataSource.Dictionary = report.Dictionary;
			string text = StiNameValidator.CorrectName(dataSource.Name, report);
			CodeTypeReference val = new CodeTypeReference(text + "DataSource");
			serializator.memberMethod.Statements.Insert(0, (CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), (CodeExpression)new CodeObjectCreateExpression(val, (CodeExpression[])(object)new CodeExpression[0])));
			CodeTypeDeclaration val2 = new CodeTypeDeclaration(text + "DataSource");
			CodeConstructor val3 = new CodeConstructor
			{
				Attributes = (MemberAttributes)24576
			};
			val3.BaseConstructorArgs.AddRange(new CodeExpressionCollection(serializator.GetArguments(dataSource.GetType(), dataSource)));
			val2.Members.Add((CodeTypeMember)(object)val3);
			((CodeTypeMember)val2).Comments.Add((CodeCommentStatement)(object)new StiCodeRegionStart("DataSource " + text));
			((CodeTypeMember)val2).Comments.Add((CodeCommentStatement)(object)new StiCodeRegionEnd("DataSource " + text));
			val2.IsClass = true;
			val2.BaseTypes.Add(dataSource.GetType());
			val2.TypeAttributes = TypeAttributes.Public;
			List<CodeExpression> list = new List<CodeExpression>();
			foreach (StiDataColumn column in dataSource.Columns)
			{
				list.Add(serializator.GetObjectCreateExpression(column.GetType(), column));
				CodeMemberProperty columnProperty = serializator.GetColumnProperty(column, buildRelations: false);
				((CodeTypeMember)columnProperty).Attributes = (MemberAttributes)24576;
				if (column.Name == "Name" || column.Name == "Position" || column.Name == "Inherited" || column.Name == "RealCount" || column.Name == "IsBof" || column.Name == "IsEof" || column.Name == "IsEmpty" || column.Name == "Alias")
				{
					((CodeTypeMember)columnProperty).Attributes = (MemberAttributes)(((CodeTypeMember)columnProperty).Attributes | 0x10);
				}
				val2.Members.Add((CodeTypeMember)(object)columnProperty);
			}
			StiSqlSource stiSqlSource = dataSource as StiSqlSource;
			List<CodeExpression> list2 = new List<CodeExpression>();
			if (stiSqlSource != null && stiSqlSource.Parameters.Count > 0)
			{
				foreach (StiDataParameter parameter in stiSqlSource.Parameters)
				{
					list2.Add(serializator.GetObjectCreateExpression(parameter.GetType(), parameter));
				}
			}
			if (list.Count > 0)
			{
				int num = 100;
				int num2 = 0;
				while (num2 < list.Count)
				{
					int num3 = list.Count - num2;
					if (num3 > num)
					{
						num3 = num;
					}
					CodeExpression[] array = list.GetRange(num2, num3).ToArray();
					num2 += num3;
					CodeExpression[] array2 = (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArrayCreateExpression(typeof(StiDataColumn), array) };
					serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), "Columns"), "AddRange", array2));
				}
			}
			if (list2.Count > 0)
			{
				CodeExpression[] array3 = list2.ToArray();
				CodeExpression[] array4 = (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeArrayCreateExpression(typeof(StiDataParameter), array3) };
				serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), "Parameters"), "AddRange", array4));
			}
			if (dataSource is StiVirtualSource stiVirtualSource)
			{
				string text2 = StiNameValidator.CorrectName(dataSource.Name, report);
				string[] array5 = stiVirtualSource.Sort.Clone() as string[];
				for (int i = 0; i < array5.Length; i++)
				{
					string text3 = array5[i].Clone() as string;
					if (text3.StartsWith("{", StringComparison.InvariantCulture) && text3.EndsWith("}", StringComparison.InvariantCulture))
					{
						string text4 = "Get" + text2 + "_Sort" + i;
						text3 = text3.Substring(1, text3.Length - 2);
						serializator.GenReturnMethodForExpresion(text4, text3, typeof(object));
						array5[i] = "{" + text4 + "}";
					}
				}
				serializator.Assign(serializator.memberMethod, text2, "Sort", array5);
				serializator.Assign(serializator.memberMethod, text2, "GroupColumns", stiVirtualSource.GroupColumns);
				serializator.Assign(serializator.memberMethod, text2, "Results", stiVirtualSource.Results);
				StiCodeDomFilters.AddFilters(serializator, text2, stiVirtualSource);
			}
			if (dataSource is StiDataTransformation stiDataTransformation)
			{
				string text5 = StiNameValidator.CorrectName(dataSource.Name, report);
				CodeExpression[] array6 = stiDataTransformation.Sorts.Select((StiDataSortRule s) => serializator.GetObjectCreateExpression(typeof(StiDataSortRule), s)).ToArray();
				if (array6.Any())
				{
					serializator.memberMethod.Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text5), "Sorts"), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiDataSortRuleHelper)), "ToList", array6)));
				}
				CodeExpression[] array7 = stiDataTransformation.Filters.Select((StiDataFilterRule s) => serializator.GetObjectCreateExpression(typeof(StiDataFilterRule), s)).ToArray();
				if (array7.Any())
				{
					serializator.memberMethod.Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text5), "Filters"), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiDataFilterRuleHelper)), "ToList", array7)));
				}
				CodeExpression[] array8 = stiDataTransformation.Actions.Select((StiDataActionRule s) => serializator.GetObjectCreateExpression(typeof(StiDataActionRule), s)).ToArray();
				if (array8.Any())
				{
					serializator.memberMethod.Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text5), "Actions"), (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiDataActionRuleHelper)), "ToList", array8)));
				}
			}
			serializator.memberMethod.Statements.Add((CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "DataSources"), "Add", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text) }));
			StiDataRelationsCollection parentRelations = dataSource.GetParentRelations();
			foreach (StiDataRelation item in parentRelations)
			{
				string parentRelationName = StiCodeDomSerializator.GetParentRelationName(report, item.Name);
				CodeMemberProperty val4 = new CodeMemberProperty
				{
					Attributes = (MemberAttributes)24576,
					HasGet = true,
					Name = StiNameValidator.CorrectName(item.Name, report),
					Type = new CodeTypeReference(parentRelationName + "Relation")
				};
				if (item.Name == "Name" || item.Name == "Alias")
				{
					((CodeTypeMember)val4).Attributes = (MemberAttributes)(((CodeTypeMember)val4).Attributes | 0x10);
				}
				CodeExpression val5 = (CodeExpression)new CodeObjectCreateExpression(new CodeTypeReference(parentRelationName + "Relation"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodeMethodInvokeExpression((CodeExpression)new CodeThisReferenceExpression(), "GetParentData", (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)item.NameInSource) }) });
				val4.GetStatements.Add((CodeStatement)new CodeMethodReturnStatement(val5));
				val2.Members.Add((CodeTypeMember)(object)val4);
			}
			if (stiSqlSource != null)
			{
				StiConnectingEvent ev = new StiConnectingEvent(stiSqlSource.SqlCommand);
				string eventName = "Get" + text + "_SqlCommand";
				serializator.GenAddEvent(eventName, text, ev);
				CodeStatementCollection val6 = new CodeStatementCollection();
				string text6 = stiSqlSource.SqlCommand;
				string sqlFilterQuery = stiSqlSource.GetSqlFilterQuery();
				if (sqlFilterQuery != null)
				{
					text6 = ((text6 == null || !text6.ToLowerInvariant().Contains("where")) ? (text6 + " WHERE " + sqlFilterQuery) : (text6 + " AND " + sqlFilterQuery));
				}
				CodeExpression val7 = (CodeExpression)((!stiSqlSource.AllowExpressions || stiSqlSource is StiMongoDbSource) ? ((object)new CodePrimitiveExpression((object)text6)) : ((object)serializator.GetTextScriptExpression(null, text6, fullConvert: true, onlyText: false, text, null, isTextExpression: false)));
				val6.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), "SqlCommand"), val7));
				foreach (StiDataParameter parameter2 in stiSqlSource.Parameters)
				{
					if (!string.IsNullOrEmpty(parameter2.Expression))
					{
						string text7 = StiCodeDomFunctions.ParseFunctions(serializator, parameter2.Value);
						val6.Add((CodeStatement)new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeIndexerExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), "Parameters"), (CodeExpression[])(object)new CodeExpression[1] { (CodeExpression)new CodePrimitiveExpression((object)parameter2.Name) }), "ParameterValue"), (CodeExpression)new CodeSnippetExpression(text7)));
					}
				}
				serializator.GenEventMethod(eventName, ev, val6);
			}
			if (stiSqlSource != null && stiSqlSource.Type == StiSqlSourceType.StoredProcedure)
			{
				CodeAssignStatement val8 = new CodeAssignStatement((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodePropertyReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), text), "Type"), (CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeTypeReferenceExpression(typeof(StiSqlSourceType)), "StoredProcedure"));
				serializator.memberMethod.Statements.Add((CodeStatement)(object)val8);
			}
			CodeMemberField val9 = new CodeMemberField(new CodeTypeReference(((CodeTypeMember)val2).Name), text)
			{
				Attributes = (MemberAttributes)24576
			};
			if (Array.IndexOf(StiCodeDomSerializator.ReservedWords, text) != -1)
			{
				((CodeTypeMember)val9).Attributes = (MemberAttributes)(((CodeTypeMember)val9).Attributes | 0x10);
			}
			serializator.Members.Add((CodeTypeMember)(object)val9);
			serializator.Members.Add((CodeTypeMember)(object)val2);
		}
		List<string> dataSourcesWithRequestFromUserVariablesInCommand = StiVariableHelper.GetDataSourcesWithRequestFromUserVariablesInCommand(report);
		if (dataSourcesWithRequestFromUserVariablesInCommand.Count > 0)
		{
			CodeExpression[] array9 = (CodeExpression[])(object)new CodeExpression[dataSourcesWithRequestFromUserVariablesInCommand.Count];
			for (int j = 0; j < dataSourcesWithRequestFromUserVariablesInCommand.Count; j++)
			{
				array9[j] = (CodeExpression)new CodePrimitiveExpression((object)dataSourcesWithRequestFromUserVariablesInCommand[j]);
			}
			CodeArrayCreateExpression val10 = new CodeArrayCreateExpression(typeof(string), array9);
			CodeFieldReferenceExpression val11 = new CodeFieldReferenceExpression((CodeExpression)new CodeFieldReferenceExpression((CodeExpression)new CodeThisReferenceExpression(), "Dictionary"), "ReconnectListForRequestFromUserVariables");
			serializator.Statements.Add((CodeStatement)new CodeAssignStatement((CodeExpression)(object)val11, (CodeExpression)(object)val10));
		}
	}
}
