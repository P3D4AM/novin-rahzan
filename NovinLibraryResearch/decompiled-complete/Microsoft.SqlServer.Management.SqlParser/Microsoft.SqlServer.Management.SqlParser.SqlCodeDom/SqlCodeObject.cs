using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCodeObject : IVisitableSqlCodeObject
{
	private class SqlCodeObjectLexicalComparer : IComparer<SqlCodeObject>
	{
		public int Compare(SqlCodeObject x, SqlCodeObject y)
		{
			if (x == y)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			LexLocation position = x.Position;
			_ = y.Position;
			int num = position.startTokenNumber - y.Position.startTokenNumber;
			if (num != 0)
			{
				return num;
			}
			return position.endTokenNumber - y.Position.endTokenNumber;
		}
	}

	private static class ExcludePropertyNameList
	{
		private static readonly List<string> ExcludeList;

		static ExcludePropertyNameList()
		{
			ExcludeList = new List<string>(32);
			PropertyInfo[] properties = typeof(SqlCodeObject).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				ExcludeList.Add(propertyInfo.Name);
			}
			ExcludeList.Add("Item");
			ExcludeList.Add("IsReadOnly");
			ExcludeList.Add("IsMultiPartName");
			ExcludeList.Add("Count");
		}

		public static bool Contains(string propertyName)
		{
			return ExcludeList.Contains(propertyName);
		}
	}

	private static class ConsistencyCheckVisitors
	{
		public static readonly ISqlCodeObjectVisitor DummyVisitor;

		public static readonly ISqlCodeObjectVisitor<bool> DummyVisitorT;

		public static readonly ISqlCodeObjectContextVisitor<bool> DummyVisitorC;

		public static readonly ISqlCodeObjectContextVisitor<bool, bool> DummyVisitorTC;

		static ConsistencyCheckVisitors()
		{
			DummyVisitor = null;
			DummyVisitorT = null;
			DummyVisitorC = null;
			DummyVisitorTC = null;
		}
	}

	private BindingState m_bindingState;

	private LexLocation relativePosition;

	private SqlCodeObject parent;

	internal static readonly IComparer<SqlCodeObject> LexicalComparer = new SqlCodeObjectLexicalComparer();

	internal abstract bool IsVisitable { get; }

	public virtual SqlStatement Statement => parent.Statement;

	internal virtual bool CanBeBoundIndependently => false;

	internal virtual bool IsStatementLevelObject => false;

	internal virtual SqlStatement AsStatement => null;

	public virtual IMetadataObject BoundObject => null;

	internal virtual IBindingContext BindingContext
	{
		get
		{
			if (parent == null)
			{
				return null;
			}
			return parent.BindingContext;
		}
	}

	internal BindingState BindingState
	{
		get
		{
			return m_bindingState;
		}
		set
		{
			m_bindingState = value;
		}
	}

	internal bool IsBound => (m_bindingState & BindingState.Bound) != 0;

	internal bool HasBindingErrors => (m_bindingState & BindingState.BindFailed) == BindingState.BindFailed;

	internal virtual bool HasAnyBindSkipped
	{
		get
		{
			if ((BindingState & BindingState.BindSkipped) == BindingState.BindSkipped)
			{
				return true;
			}
			return parent.HasAnyBindSkipped;
		}
	}

	public virtual IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield break;
		}
	}

	public Location StartLocation
	{
		get
		{
			if (Position == null)
			{
				return new Location(-1, -1);
			}
			return ParseContext.TokenManager.GetLocation(Position.startTokenNumber);
		}
	}

	public Location EndLocation
	{
		get
		{
			if (Position == null)
			{
				return new Location(-1, -1);
			}
			return ParseContext.TokenManager.GetLocation(Position.endTokenNumber);
		}
	}

	public string Sql
	{
		get
		{
			LexLocation position = Position;
			if (position == null)
			{
				return null;
			}
			return ParseContext.TokenManager.GetText(position.startTokenNumber, position.endTokenNumber);
		}
	}

	public int Length
	{
		get
		{
			if (Sql != null)
			{
				return Sql.Length;
			}
			return 0;
		}
	}

	public string Xml
	{
		get
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				Indent = true
			});
			WriteXml(xmlWriter);
			xmlWriter.Flush();
			string result = stringWriter.ToString();
			stringWriter.Dispose();
			return result;
		}
	}

	public IEnumerable<Token> Tokens
	{
		get
		{
			LexLocation position = Position;
			if (position != null)
			{
				TokenManager tokenManager = ParseContext.TokenManager;
				int i = position.startTokenNumber;
				while (i < position.endTokenNumber)
				{
					yield return tokenManager.GetToken(i);
					int num = i + 1;
					i = num;
				}
			}
		}
	}

	public bool IsSqlCodeSnippet => this is INullCodeObject;

	[XmlIgnore]
	public virtual TokenManager TokenManager => parent.TokenManager;

	private ParseContext ParseContext
	{
		get
		{
			SqlCodeObject sqlCodeObject = this;
			while (sqlCodeObject.parent != null)
			{
				sqlCodeObject = sqlCodeObject.parent;
			}
			return sqlCodeObject as ParseContext;
		}
	}

	[XmlIgnore]
	public LexLocation Position
	{
		get
		{
			int absolutePositionOffset = AbsolutePositionOffset;
			if (absolutePositionOffset == 0)
			{
				return relativePosition;
			}
			return new LexLocation(relativePosition.startTokenNumber + absolutePositionOffset, relativePosition.endTokenNumber + absolutePositionOffset);
		}
		set
		{
			relativePosition = value;
		}
	}

	internal virtual int LocalPositionOffset => 0;

	internal int AbsolutePositionOffset
	{
		get
		{
			int num = 0;
			for (SqlCodeObject sqlCodeObject = this; sqlCodeObject != null; sqlCodeObject = sqlCodeObject.parent)
			{
				num += sqlCodeObject.LocalPositionOffset;
			}
			return num;
		}
	}

	public SqlCodeObject Parent => parent;

	public abstract void Accept(ISqlCodeObjectVisitor visitor);

	public abstract T Accept<T>(ISqlCodeObjectVisitor<T> visitor);

	public abstract void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context);

	public abstract T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context);

	internal virtual LexicalScope GetLexicalScope()
	{
		return parent.GetChildLexicalScope(this);
	}

	internal virtual LexicalScope GetChildLexicalScope(SqlCodeObject child)
	{
		return GetLexicalScope();
	}

	internal LexicalScope CreateLexicalScope(bool isModule)
	{
		LexicalScope lexicalScope = parent.GetLexicalScope();
		if (!isModule)
		{
			return lexicalScope.CreateChildScope();
		}
		return lexicalScope.CreateModuleScope();
	}

	internal virtual void RegisterDatabaseContextChange(DatabaseContext databaseContext)
	{
	}

	internal virtual DatabaseContext GetDatabaseContext(LexicalScope lexScope)
	{
		return null;
	}

	internal virtual SqlTableExpression FindSiblingTableExpressionBoundTo(SqlTableExpression tableExpression, ITabular boundTable)
	{
		return null;
	}

	internal T GetBoundObject<T>(SqlCodeObject codeObject) where T : class, IMetadataObject
	{
		IBindingContext bindingContext = BindingContext;
		if (bindingContext != null)
		{
			BatchBindingContext batch = bindingContext.Batch;
			return codeObject.GetBoundObject<T>(batch);
		}
		return null;
	}

	internal T GetBoundObject<T>(BatchBindingContext bindingContext) where T : class, IMetadataObject
	{
		return bindingContext.GetBoundObject<T>(this);
	}

	internal virtual void Bind(IBindingContext bindingContext)
	{
		bool flag = false;
		foreach (SqlCodeObject child in Children)
		{
			child.Bind(bindingContext);
			BindingState |= child.BindingState;
			flag = true;
		}
		if (!flag)
		{
			BindingState = BindingState.BindSucceeded;
		}
	}

	internal virtual void Unbind()
	{
		foreach (SqlCodeObject child in Children)
		{
			child.Unbind();
		}
		BindingState = BindingState.Unbound;
	}

	[Conditional("DEBUG")]
	internal virtual void ValidateBoundObjects(BindChecks.MetadataObjectValidator validator)
	{
		_ = BoundObject;
		foreach (SqlCodeObject child in Children)
		{
			_ = child;
		}
	}

	internal virtual IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null && MayChildHaveCompletionList(sqlCodeObject))
		{
			return sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		return null;
	}

	internal SqlCodeObject FindChildContainingLocation(ref LexLocation lexLocation)
	{
		SqlCodeObject result = null;
		foreach (SqlCodeObject child in Children)
		{
			LexLocation position = child.Position;
			if (position.Contains(lexLocation))
			{
				result = child;
				break;
			}
			if (position.endTokenNumber <= lexLocation.endTokenNumber)
			{
				continue;
			}
			if (lexLocation.startTokenNumber == lexLocation.endTokenNumber)
			{
				LexLocation altCaretLexLocation = TokenManager.GetAltCaretLexLocation(lexLocation);
				if (altCaretLexLocation != null && position.Contains(altCaretLexLocation))
				{
					result = child;
					lexLocation = altCaretLexLocation;
				}
			}
			break;
		}
		return result;
	}

	internal virtual bool MayChildHaveCompletionList(SqlCodeObject child)
	{
		return true;
	}

	internal IMetadataCollection<IMetadataObject> RetrieveCompletionListItems(CompletionContext completionContext)
	{
		return RetrieveCompletionListItems(null, completionContext);
	}

	internal IMetadataCollection<IMetadataObject> RetrieveCompletionListItems(LexLocation caretLocation, IEnumerable<SqlIdentifier> identifiers, CompletionContext completionContext)
	{
		List<string> list = new List<string>(4);
		foreach (SqlIdentifier identifier in identifiers)
		{
			if (identifier.Position.endTokenNumber > caretLocation.startTokenNumber || identifier.IsMissing)
			{
				break;
			}
			list.Add(identifier.Value);
		}
		return RetrieveCompletionListItems(list, completionContext);
	}

	private IMetadataCollection<IMetadataObject> RetrieveCompletionListItems(List<string> ids, CompletionContext completionContext)
	{
		return BindingContext.FindAll(ids, completionContext, GetLexicalScope());
	}

	internal bool IsCaretBetweenEmptyParenthesis(LexLocation caretLocation)
	{
		bool flag = false;
		if (caretLocation.startTokenNumber == caretLocation.endTokenNumber)
		{
			TokenManager tokenManager = TokenManager;
			int startTokenNumber = caretLocation.startTokenNumber;
			if (tokenManager.GetId(startTokenNumber - 1) == 40)
			{
				flag = startTokenNumber == Position.endTokenNumber || tokenManager.GetId(startTokenNumber) == 41;
				if (!flag)
				{
					LexLocation altCaretLexLocation = tokenManager.GetAltCaretLexLocation(caretLocation);
					if (altCaretLexLocation != null)
					{
						flag = tokenManager.GetId(altCaretLexLocation.startTokenNumber) == 41;
					}
				}
			}
		}
		return flag;
	}

	internal virtual string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		return FindChildContainingLocation(ref cursorLocation)?.GetQuickInfoText(cursorLocation, displayInfoProvider);
	}

	internal MethodHelpText GetMethodHelpTextRecursive(LexLocation caretLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		MethodHelpText methodHelpText = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null)
		{
			methodHelpText = sqlCodeObject.GetMethodHelpTextRecursive(caretLocation, displayInfoProvider);
		}
		if (methodHelpText == null)
		{
			return GetMyMethodHelpText(caretLocation, displayInfoProvider);
		}
		return methodHelpText;
	}

	internal virtual MethodHelpText GetMyMethodHelpText(LexLocation caretLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		return null;
	}

	internal MethodHelpText BuildMethodHelpText(IMetadataObject boundObject, IMetadataCollection<IParameter> parameters, string methodDescription, string methodType, bool shouldShowParentheses, bool isVarArgs, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string text = displayInfoProvider.GetDatabaseQualifiedName(boundObject);
		if (text == null)
		{
			text = displayInfoProvider.GetDisplayName(boundObject);
		}
		MethodHelpText methodHelpText = new MethodHelpText(text, methodDescription, methodType, shouldShowParentheses, isVarArgs);
		foreach (IParameter parameter in parameters)
		{
			methodHelpText.Parameters.Add(new Parameter(parameter.Name, null, displayInfoProvider.ObjectToString(parameter)));
		}
		return methodHelpText;
	}

	internal string BuildParameterHelpMethodTypeString(IDataType dataType)
	{
		return "RETURNS " + dataType.ToString();
	}

	internal MethodNameAndParamLocations GetMethodNameAndParamsRecursive(LexLocation caretLocation)
	{
		MethodNameAndParamLocations methodNameAndParamLocations = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null)
		{
			methodNameAndParamLocations = sqlCodeObject.GetMethodNameAndParamsRecursive(caretLocation);
		}
		if (methodNameAndParamLocations == null)
		{
			return GetMyMethodNameAndParams(caretLocation);
		}
		return methodNameAndParamLocations;
	}

	internal virtual MethodNameAndParamLocations GetMyMethodNameAndParams(LexLocation caretLocation)
	{
		return null;
	}

	internal List<SqlCodeObject> GetSqlCodeObjectList(SqlScalarExpressionCollection scalarExpressionCollection)
	{
		List<SqlCodeObject> list = null;
		if (scalarExpressionCollection != null)
		{
			list = new List<SqlCodeObject>();
			foreach (SqlScalarExpression item in scalarExpressionCollection)
			{
				list.Add(item);
			}
		}
		return list;
	}

	internal void BuildMethodNameAndParams(MethodNameAndParamLocations methodNameAndParams, int nameEndPosition, List<SqlCodeObject> arguments)
	{
		TokenManager tokenManager = TokenManager;
		if (arguments == null)
		{
			LexLocation position = Position;
			int num = tokenManager.FindTokenInRange(40, nameEndPosition, position.endTokenNumber);
			if (num != -1)
			{
				methodNameAndParams.ParamStartLocation = tokenManager.GetLocation(num);
				int num2 = tokenManager.FindTokenInRange(41, num, position.endTokenNumber);
				if (num2 != -1)
				{
					methodNameAndParams.ParamEndLocation = tokenManager.GetLocation(num2);
				}
			}
			return;
		}
		SqlCodeObject sqlCodeObject = arguments[0];
		int num3 = tokenManager.FindTokenInRange(40, nameEndPosition, sqlCodeObject.Position.startTokenNumber);
		if (num3 == -1)
		{
			num3 = nameEndPosition;
		}
		methodNameAndParams.ParamStartLocation = tokenManager.GetLocation(num3);
		for (int i = 1; i < arguments.Count; i++)
		{
			SqlCodeObject sqlCodeObject2 = arguments[i];
			int num4 = tokenManager.FindTokenInRange(44, sqlCodeObject.Position.endTokenNumber, sqlCodeObject2.Position.startTokenNumber);
			if (num4 != -1)
			{
				methodNameAndParams.ParamSeperatorLocations.Add(tokenManager.GetLocation(num4));
			}
			sqlCodeObject = sqlCodeObject2;
		}
		int num5 = tokenManager.FindTokenInRange(41, sqlCodeObject.Position.endTokenNumber, Position.endTokenNumber);
		if (num5 != -1)
		{
			methodNameAndParams.ParamEndLocation = tokenManager.GetLocation(num5);
		}
	}

	internal virtual void PopulateRegions(List<Region> regions)
	{
	}

	internal void AddRegion(List<Region> regions, Location startLocation, Location endLocation)
	{
		Location startLocation2 = new Location(startLocation.LineNumber, TokenManager.GetLastCaretColumnNumber(startLocation.LineNumber), startLocation.Offset);
		Location endLocation2 = new Location(endLocation.LineNumber, Math.Min(endLocation.ColumnNumber, TokenManager.GetLastCaretColumnNumber(endLocation.LineNumber)), endLocation.Offset);
		regions.Add(new Region(startLocation2, endLocation2));
	}

	internal SqlCodeObject()
	{
		parent = ShiftReduceParser.CurrentParseContext;
	}

	internal void SetParentParseContext(ParseContext parseContext)
	{
		parent = parseContext;
	}

	internal void ClearParent()
	{
		parent = null;
	}

	internal virtual void SetParentPointers()
	{
		foreach (SqlCodeObject child in Children)
		{
			child.parent = this;
			child.SetParentPointers();
		}
	}

	internal void PopulateIdentifiers(List<SqlIdentifier> identifiers)
	{
		foreach (SqlCodeObject child in Children)
		{
			if (child is SqlIdentifier item)
			{
				identifiers.Add(item);
			}
			else if (child is INullCodeObject)
			{
				child.PopulateIdentifierFromTokens(identifiers);
			}
			else
			{
				child.PopulateIdentifiers(identifiers);
			}
		}
	}

	internal string GetSql(LexLocation location)
	{
		return TokenManager.GetText(location);
	}

	private void PopulateIdentifierFromTokens(List<SqlIdentifier> identifiers)
	{
		TokenManager tokenManager = TokenManager;
		int absolutePositionOffset = AbsolutePositionOffset;
		int num = relativePosition.startTokenNumber + absolutePositionOffset;
		int num2 = relativePosition.endTokenNumber + absolutePositionOffset;
		for (int i = num; i < num2; i++)
		{
			if (tokenManager.GetId(i) == 173)
			{
				SqlIdentifier sqlIdentifier = new SqlIdentifier(tokenManager.GetText(i));
				sqlIdentifier.parent = this;
				sqlIdentifier.Position = new LexLocation(i - absolutePositionOffset, i + 1 - absolutePositionOffset);
				identifiers.Add(sqlIdentifier);
			}
		}
	}

	public static string GetSqlSource<T>(ISqlCodeCollection<T> collection) where T : SqlCodeObject
	{
		int count = collection.Count;
		if (count > 0)
		{
			T first = collection[0];
			T last = collection[count - 1];
			return GetSqlSource(first, last);
		}
		return null;
	}

	public static string GetSqlSource(params SqlCodeObject[] codeObjects)
	{
		int num = codeObjects.Length;
		if (num > 0)
		{
			SqlCodeObject first = codeObjects[0];
			SqlCodeObject last = codeObjects[num - 1];
			return GetSqlSource(first, last);
		}
		return null;
	}

	public static string GetSqlSource(SqlCodeObject first, SqlCodeObject last)
	{
		int startTokenNumber = first.Position.startTokenNumber;
		int endTokenNumber = last.Position.endTokenNumber;
		return first.TokenManager.GetText(startTokenNumber, endTokenNumber);
	}

	internal int GetTokenNumber(int line, int col)
	{
		TokenManager tokenManager = ParseContext.TokenManager;
		int num = tokenManager.FindToken(line, col);
		if (num == tokenManager.Count)
		{
			num--;
		}
		while (num > 0 && !TokenManager.IsSignificantToken(tokenManager.GetId(num)))
		{
			num--;
		}
		return num;
	}

	internal virtual void WriteXml(XmlWriter writer)
	{
		Type type = GetType();
		while (type.GetIsNestedPrivate())
		{
			type = type.GetBaseType();
		}
		writer.WriteStartElement(type.Name);
		if (Position != null)
		{
			writer.WriteAttributeString("Location", string.Format(CultureInfo.CurrentCulture, "({0},{1})", StartLocation, EndLocation));
		}
		PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (!ExcludePropertyNameList.Contains(propertyInfo.Name))
			{
				object value = propertyInfo.GetValue(this, null);
				if (value != null && value != SqlIdentifier.Null && !(value is SqlScalarExpression) && !(value is IMetadataObject) && !(value.ToString() == value.GetType().ToString()))
				{
					writer.WriteAttributeString(propertyInfo.Name, value.ToString());
				}
			}
		}
		if (BoundObject != null)
		{
			writer.WriteAttributeString("BoundObject", ExtensionMethods.GetDescription(BoundObject));
		}
		writer.WriteComment(Sql);
		foreach (SqlCodeObject child in Children)
		{
			child.WriteXml(writer);
		}
		writer.WriteEndElement();
	}

	internal void ReportSyntaxError()
	{
		ReportError(ErrorType.SyntaxError, ErrorFlags.None, SR.IncorrectSyntaxNear(Sql), null);
	}

	internal void ReportSyntaxErrorOnFirstToken()
	{
		LexLocation firstTokenLocation = Position.GetFirstTokenLocation();
		ReportSyntaxError(firstTokenLocation);
	}

	internal void ReportSyntaxErrorNearKeyword(string keyword)
	{
		ReportError(ErrorType.SyntaxError, ErrorFlags.None, SR.IncorrectSyntaxNearKeyword(keyword), null);
	}

	internal void ReportSyntaxError(LexLocation lexLocation)
	{
		ReportSyntaxError(ParseContext.TokenManager.GetText(lexLocation), lexLocation);
	}

	internal void ReportSyntaxError(string tokenString, LexLocation lexLocation)
	{
		string message = SR.IncorrectSyntaxNear(tokenString);
		ParseContext.ErrorHandler.AddError(message, ErrorType.SyntaxError, ErrorFlags.None, lexLocation);
	}

	internal void ReportParseError(string message)
	{
		ReportError(ErrorType.ParseError, ErrorFlags.None, message, null);
	}

	internal void ReportBindError(string message)
	{
		ReportError(ErrorType.BindError, ErrorFlags.None, message, null);
	}

	internal void ReportBindError(string message, ErrorFlags errorFlags)
	{
		ReportError(ErrorType.BindError, errorFlags, message, null);
	}

	internal static void ReportParseError<T>(SqlCollection<T> codeObjects, string message) where T : SqlCodeObject
	{
		ReportError(codeObjects, ErrorType.ParseError, message, ErrorFlags.None);
	}

	internal static void ReportBindError<T>(SqlCollection<T> codeObjects, string message) where T : SqlCodeObject
	{
		ReportError(codeObjects, ErrorType.BindError, message, ErrorFlags.None);
	}

	private static void ReportError<T>(SqlCollection<T> codeObjects, ErrorType type, string message, ErrorFlags errorFlags) where T : SqlCodeObject
	{
		int count = codeObjects.Count;
		switch (count)
		{
		case 1:
			codeObjects[0].ReportError(type, errorFlags, message, null);
			break;
		default:
			codeObjects[0].ReportError(type, errorFlags, message, codeObjects[count - 1]);
			break;
		case 0:
			break;
		}
	}

	private void ReportError(ErrorType type, ErrorFlags errorFlags, string message, SqlCodeObject lastCodeObject)
	{
		ParseContext parseContext = ParseContext;
		if (lastCodeObject == null)
		{
			parseContext.ErrorHandler.AddError(message, type, errorFlags, this);
		}
		else
		{
			parseContext.ErrorHandler.AddError(message, type, errorFlags, this, lastCodeObject);
		}
	}

	[Conditional("DEBUG")]
	internal void RunConsistencyChecks()
	{
	}

	[Conditional("DEBUG")]
	internal virtual void RunConsistencyChecks(string path, int depth)
	{
		path = path + "/" + GetType().Name;
		List<SqlCodeObject> list = new List<SqlCodeObject>();
		list.AddRange(Children);
		Dictionary<SqlCodeObject, PropertyInfo> dictionary = new Dictionary<SqlCodeObject, PropertyInfo>(list.Count);
		PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (ExcludePropertyNameList.Contains(propertyInfo.Name))
			{
				continue;
			}
			object value = propertyInfo.GetValue(this, null);
			if (value == null)
			{
				continue;
			}
			if (value.GetType().GetInterface("ISqlCodeCollection`1") != null)
			{
				foreach (SqlCodeObject item in value as IEnumerable)
				{
					if (item != null)
					{
						dictionary.Add(item, propertyInfo);
					}
				}
			}
			else if (propertyInfo.GetValue(this, null) is SqlCodeObject sqlCodeObject2 && sqlCodeObject2 != SqlIdentifier.Null && (!(propertyInfo.Name == "ColumnName") || !(this is SqlColumnRefExpression)) && (!(propertyInfo.Name == "ColumnOrPropertyName") || !(this is SqlScalarRefExpression)))
			{
				dictionary.Add(sqlCodeObject2, propertyInfo);
			}
		}
		SqlCodeObject sqlCodeObject3 = null;
		for (int j = 0; j < list.Count; j++)
		{
			SqlCodeObject sqlCodeObject4 = list[j];
			_ = sqlCodeObject4.Position;
			Position.Contains(sqlCodeObject4.Position);
			if (sqlCodeObject3 != null)
			{
				sqlCodeObject4.Position.Before(sqlCodeObject3.Position);
			}
			if (sqlCodeObject3 != null)
			{
				sqlCodeObject4.Position.Overlaps(sqlCodeObject3.Position);
			}
			_ = sqlCodeObject4.parent;
			if (sqlCodeObject4 is SqlBatch)
			{
				_ = this is SqlScript;
			}
			if (sqlCodeObject4.IsStatementLevelObject && !IsStatementLevelObject && !(this is SqlBatch))
			{
				_ = this is SqlErrorCodeObject;
			}
			sqlCodeObject3 = sqlCodeObject4;
			dictionary.Remove(sqlCodeObject4);
			_ = 1000;
		}
		if (dictionary.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			foreach (SqlCodeObject key in dictionary.Keys)
			{
				PropertyInfo propertyInfo2 = dictionary[key];
				stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "    {0}: {1}", propertyInfo2.Name, key.GetType().Name));
			}
		}
		Accept(ConsistencyCheckVisitors.DummyVisitor);
		Accept(ConsistencyCheckVisitors.DummyVisitorT);
		Accept(ConsistencyCheckVisitors.DummyVisitorC, context: false);
		Accept(ConsistencyCheckVisitors.DummyVisitorTC, context: false);
	}
}
