using System;
using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Diagnostics;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Intellisense;

public abstract class Resolver
{
	public class BlockInformation
	{
		private readonly Location start;

		private readonly Location end;

		private readonly string description;

		public Location Start => start;

		public Location End => end;

		public string Description => description;

		internal BlockInformation(SqlCodeObject codeObject)
		{
			start = codeObject.StartLocation;
			end = codeObject.EndLocation;
			description = codeObject.GetType().Name;
		}

		internal BlockInformation(Location startLocation, Location endLocation, string description)
		{
			start = startLocation;
			end = endLocation;
			this.description = description;
		}
	}

	private static readonly IList<Declaration> EmptyDeclarationList = new List<Declaration>().AsReadOnly();

	public static IList<Declaration> FindCompletions(ParseResult parseResult, int line, int col, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		using (TraceHelper.TraceContext.GetMethodContext("FindCompletions"))
		{
			if (parseResult == null)
			{
				throw new ArgumentNullException("parseResult");
			}
			if (displayInfoProvider == null)
			{
				throw new ArgumentNullException("displayInfoProvider");
			}
			SqlScript script = parseResult.Script;
			IMetadataCollection<IMetadataObject> metadataCollection = null;
			LexLocation caretLexLocation = script.TokenManager.GetCaretLexLocation(line, col);
			if (caretLexLocation != null)
			{
				metadataCollection = script.GetCompletionList(caretLexLocation, CompletionContext.None);
			}
			if (metadataCollection != null)
			{
				List<Declaration> list = new List<Declaration>(metadataCollection.Count);
				foreach (IMetadataObject item2 in metadataCollection)
				{
					Declaration item = new Declaration(item2, displayInfoProvider);
					int num = list.BinarySearch(item);
					if (num < 0)
					{
						list.Insert(~num, item);
					}
				}
				return list;
			}
			return EmptyDeclarationList;
		}
	}

	public static CodeObjectQuickInfo GetQuickInfo(ParseResult parseResult, int line, int col, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		using (TraceHelper.TraceContext.GetMethodContext("GetQuickInfo"))
		{
			if (parseResult == null)
			{
				throw new ArgumentNullException("parseResult");
			}
			if (displayInfoProvider == null)
			{
				throw new ArgumentNullException("displayInfoProvider");
			}
			SqlScript script = parseResult.Script;
			CodeObjectQuickInfo result = null;
			TokenManager tokenManager = script.TokenManager;
			LexLocation cursorLocation = tokenManager.GetCursorLocation(line, col);
			if (cursorLocation != null)
			{
				string quickInfoText = script.GetQuickInfoText(cursorLocation, displayInfoProvider);
				if (quickInfoText != null)
				{
					result = new CodeObjectQuickInfo(quickInfoText, tokenManager.GetLocation(cursorLocation.startTokenNumber), tokenManager.GetLocation(cursorLocation.endTokenNumber));
				}
			}
			return result;
		}
	}

	public static BlockInformation GetBlockInformation(ParseResult parseResult, int line, int col)
	{
		if (parseResult == null)
		{
			throw new ArgumentNullException("parseResult");
		}
		SqlScript script = parseResult.Script;
		int tokenNumber = script.GetTokenNumber(line, col);
		LexLocation value = new LexLocation(tokenNumber, tokenNumber + 1);
		SqlCodeObject sqlCodeObject = ((script.Batches.Count != 1) ? ((SqlCodeObject)script) : ((SqlCodeObject)script.Batches[0]));
		foreach (SqlCodeObject child in sqlCodeObject.Children)
		{
			if (child.Position.Contains(value))
			{
				return new BlockInformation(child);
			}
		}
		return new BlockInformation(sqlCodeObject);
	}

	public static IEnumerable<Region> FindRegionObjects(ParseResult parseResult)
	{
		if (parseResult == null)
		{
			throw new ArgumentNullException("parseResult");
		}
		List<Region> list = new List<Region>();
		parseResult.Script.PopulateRegions(list);
		return list;
	}

	public static List<MethodHelpText> FindMethods(ParseResult parseResult, int line, int col, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		if (parseResult == null)
		{
			throw new ArgumentNullException("parseResult");
		}
		SqlScript script = parseResult.Script;
		List<MethodHelpText> list = new List<MethodHelpText>();
		LexLocation caretLexLocation = script.TokenManager.GetCaretLexLocation(line, col);
		if (caretLexLocation != null)
		{
			MethodHelpText methodHelpTextRecursive = script.GetMethodHelpTextRecursive(caretLexLocation, displayInfoProvider);
			if (methodHelpTextRecursive != null)
			{
				list.Add(methodHelpTextRecursive);
			}
		}
		return list;
	}

	public static MethodNameAndParamLocations GetMethodNameAndParams(ParseResult parseResult, int line, int column, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		if (parseResult == null)
		{
			throw new ArgumentNullException("parseResult");
		}
		SqlScript script = parseResult.Script;
		MethodNameAndParamLocations methodNameAndParamLocations = null;
		LexLocation caretLexLocation = script.TokenManager.GetCaretLexLocation(line, column);
		if (caretLexLocation != null)
		{
			methodNameAndParamLocations = script.GetMethodNameAndParamsRecursive(caretLexLocation);
			if (methodNameAndParamLocations != null)
			{
				Location nameEndLocation = new Location(line, column);
				if (nameEndLocation.CompareTo(methodNameAndParamLocations.NameEndLocation) > 0)
				{
					methodNameAndParamLocations.NameEndLocation = nameEndLocation;
				}
			}
		}
		return methodNameAndParamLocations;
	}

	public static PairMatch FindPairMatch(ParseResult parseResult, int line, int col)
	{
		if (parseResult == null)
		{
			throw new ArgumentNullException("parseResult");
		}
		return (parseResult.Script.Parent as ParseContext).TokenManager.FindPairMatch(line, col);
	}

	public static BlockInformation FindBreakPointInformation(ParseResult parseResult, int line, int col)
	{
		if (parseResult == null)
		{
			throw new ArgumentNullException("parseResult");
		}
		SqlScript script = parseResult.Script;
		int num = script.TokenManager.FindToken(line, col);
		Token token = script.TokenManager.GetToken(num);
		int num2 = num;
		if (!token.IsSignificant)
		{
			num2 = script.TokenManager.GetNextSignificantTokenIndex(num);
		}
		LexLocation lexLocation = null;
		SqlCodeObject sqlCodeObject2;
		if (num2 != -1)
		{
			lexLocation = new LexLocation(num2, num2 + 1);
			SqlCodeObject sqlCodeObject = ((script.Batches.Count != 1) ? ((SqlCodeObject)script) : ((SqlCodeObject)script.Batches[0]));
			sqlCodeObject2 = sqlCodeObject.FindChildContainingLocation(ref lexLocation);
			SqlCodeObject sqlCodeObject3 = sqlCodeObject2;
			while (sqlCodeObject3 != null)
			{
				bool flag = false;
				foreach (SqlCodeObject child in sqlCodeObject3.Children)
				{
					if (child.Position.Contains(lexLocation))
					{
						if (child is SqlStatement)
						{
							sqlCodeObject2 = child;
						}
						sqlCodeObject3 = child;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					sqlCodeObject3 = null;
				}
			}
			if (sqlCodeObject2 is SqlStatementError)
			{
				sqlCodeObject2 = null;
			}
			if (sqlCodeObject2 != null)
			{
				Location other = new Location(line, col);
				if ((sqlCodeObject2.StartLocation.CompareTo(other) > 0 || sqlCodeObject2.EndLocation.CompareTo(other) < 0) && (token.Id != 986 || (token.StartLocation.ColumnNumber != 1 && (token.StartLocation.LineNumber == token.EndLocation.LineNumber || line != sqlCodeObject2.StartLocation.LineNumber))))
				{
					sqlCodeObject2 = null;
				}
			}
		}
		else
		{
			sqlCodeObject2 = null;
		}
		if (sqlCodeObject2 == null)
		{
			return null;
		}
		return ExtractBPLocation(sqlCodeObject2, lexLocation);
	}

	private static BlockInformation ExtractBPLocation(SqlCodeObject codeObject, LexLocation lexLocation)
	{
		if (codeObject is SqlWhileStatement || codeObject is SqlIfElseStatement)
		{
			return new BlockInformation(codeObject.StartLocation, (codeObject as SqlConditionalStatement).Condition.EndLocation, codeObject.GetType().ToString());
		}
		if (codeObject is SqlCompoundStatement || codeObject is SqlTryCatchStatement)
		{
			Token token = codeObject.TokenManager.GetToken(codeObject.Position.startTokenNumber);
			return new BlockInformation(token.StartLocation, token.EndLocation, codeObject.GetType().ToString());
		}
		if (codeObject is SqlCreateAlterProcedureStatementBase || codeObject is SqlCreateAlterFunctionStatementBase)
		{
			return null;
		}
		return new BlockInformation(codeObject.StartLocation, codeObject.EndLocation, codeObject.GetType().ToString());
	}
}
