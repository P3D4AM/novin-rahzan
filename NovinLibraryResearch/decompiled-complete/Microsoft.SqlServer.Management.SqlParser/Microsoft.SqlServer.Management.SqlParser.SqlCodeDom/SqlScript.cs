using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Diagnostics;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlScript : SqlCodeObject
{
	private class CommentRegionScanner
	{
		private enum CommentRegionStates
		{
			Base,
			SingleLinePotentialRegion,
			MultiLinePotentialRegion,
			CompleteRegion
		}

		private SqlScript script;

		private TokenManager tokenManager;

		private List<Region> regions;

		private CommentRegionStates regionState;

		private Location? regionStartLocation;

		private Location? regionEndLocation;

		private int tokenIndex;

		internal CommentRegionScanner(SqlScript script, List<Region> regions)
		{
			this.script = script;
			tokenManager = this.script.TokenManager;
			this.regions = regions;
		}

		internal void PopulateCommentRegions()
		{
			regionState = CommentRegionStates.Base;
			int count = tokenManager.Count;
			for (tokenIndex = 0; tokenIndex < count; tokenIndex++)
			{
				switch (regionState)
				{
				case CommentRegionStates.Base:
					RegionFromBase();
					break;
				case CommentRegionStates.SingleLinePotentialRegion:
					RegionFromSingleLinePotentialRegion();
					break;
				case CommentRegionStates.MultiLinePotentialRegion:
					RegionFromMultiLinePotentialRegion();
					break;
				case CommentRegionStates.CompleteRegion:
					RegionFromCompleteRegion();
					break;
				}
			}
			if (regionState == CommentRegionStates.CompleteRegion)
			{
				script.AddRegion(regions, regionStartLocation.Value, regionEndLocation.Value);
			}
		}

		private void RegionFromBase()
		{
			int id = tokenManager.GetId(tokenIndex);
			if (tokenManager.IsTokenComment(id))
			{
				Location location = tokenManager.GetLocation(tokenIndex);
				Location location2 = tokenManager.GetLocation(tokenIndex + 1);
				if (tokenManager.IsTokenTrueMultiLineComment(id, location, location2))
				{
					regionStartLocation = location;
					regionEndLocation = location2;
					regionState = CommentRegionStates.CompleteRegion;
				}
				else
				{
					regionStartLocation = location;
					regionState = CommentRegionStates.SingleLinePotentialRegion;
				}
			}
		}

		private void RegionFromSingleLinePotentialRegion()
		{
			int id = tokenManager.GetId(tokenIndex);
			if (tokenManager.IsTokenComment(id))
			{
				Location location = tokenManager.GetLocation(tokenIndex);
				Location location2 = tokenManager.GetLocation(tokenIndex + 1);
				if (tokenManager.IsTokenTrueMultiLineComment(id, location, location2))
				{
					regionEndLocation = location2;
					regionState = CommentRegionStates.CompleteRegion;
				}
			}
			else if (tokenManager.IsTokenWhitespace(id))
			{
				if (tokenManager.GetText(tokenIndex).Contains("\n"))
				{
					regionState = CommentRegionStates.MultiLinePotentialRegion;
				}
			}
			else
			{
				regionStartLocation = null;
				regionEndLocation = null;
				regionState = CommentRegionStates.Base;
			}
		}

		private void RegionFromMultiLinePotentialRegion()
		{
			int id = tokenManager.GetId(tokenIndex);
			if (tokenManager.IsTokenComment(id))
			{
				regionEndLocation = tokenManager.GetLocation(tokenIndex + 1);
				regionState = CommentRegionStates.CompleteRegion;
			}
			else if (!tokenManager.IsTokenWhitespace(id))
			{
				regionStartLocation = null;
				regionEndLocation = null;
				regionState = CommentRegionStates.Base;
			}
		}

		private void RegionFromCompleteRegion()
		{
			int id = tokenManager.GetId(tokenIndex);
			if (tokenManager.IsTokenComment(id))
			{
				regionEndLocation = tokenManager.GetLocation(tokenIndex + 1);
			}
			else if (!tokenManager.IsTokenWhitespace(id))
			{
				script.AddRegion(regions, regionStartLocation.Value, regionEndLocation.Value);
				regionStartLocation = null;
				regionEndLocation = null;
				regionState = CommentRegionStates.Base;
			}
		}
	}

	private ErrorCollection bindErrors;

	private LexicalScope lexicalScope;

	private BatchBindingContext bindingContext;

	private GeneralErrorHandler errorHandler;

	private ErrorCollection parseErrors;

	private readonly SqlBatchCollection batches;

	private readonly bool isIncrementalMode;

	private ErrorCollection errorCollection;

	internal override bool IsVisitable => true;

	public sealed override SqlStatement Statement => null;

	internal override IBindingContext BindingContext => bindingContext;

	internal ParseContext ParseContext => base.Parent as ParseContext;

	public SqlBatchCollection Batches => batches;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlBatch batch in batches)
			{
				yield return batch;
			}
		}
	}

	public IEnumerable<Error> Errors => errorCollection.ErrorList;

	internal bool IsIncrementalMode => isIncrementalMode;

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override LexicalScope GetLexicalScope()
	{
		return lexicalScope;
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		Bind(bindingContext.Batch, null);
	}

	internal void Bind(BatchBindingContext bindingContext, BindWorkList workList)
	{
		BindScript(bindingContext.Batch, workList);
	}

	internal void BeforeBind()
	{
		errorHandler = new GeneralErrorHandler();
		ParseContext.BeginBind(errorHandler);
	}

	internal void AfterBind()
	{
		errorHandler.CommitErrors();
		ErrorCollection errorCollection = errorHandler.GetErrorCollection(TokenManager);
		errorHandler = null;
		UpdateBindErrors(errorCollection);
		ParseContext.EndBind();
	}

	private void BindScript(BatchBindingContext bindingContext, BindWorkList workList)
	{
		using MethodContext methodContext = TraceHelper.TraceContext.GetMethodContext("SqlScript.BindScript");
		lexicalScope = LexicalScope.CreateScirptScope();
		this.bindingContext = bindingContext;
		using (methodContext.GetActivityContext("BindScript"))
		{
			try
			{
				DatabaseEx database = null;
				foreach (SqlBatch batch in batches)
				{
					bindingContext = bindingContext.GetSiblingBatchBindingContext(database);
					batch.Bind(bindingContext, workList);
					base.BindingState |= batch.BindingState;
					database = batch.GetFinalDatabaseContext()?.Database;
				}
			}
			catch (Exception ex)
			{
				methodContext.TraceCatch(ex);
				throw new SqlParserInternalBinderError(ex);
			}
			finally
			{
			}
		}
	}

	private void UpdateBindErrors(ErrorCollection bindErrors)
	{
		this.bindErrors = bindErrors;
		CommitErrors();
	}

	internal override void Unbind()
	{
		using (TraceHelper.TraceContext.GetMethodContext("SqlScript.Unbind"))
		{
			errorHandler = null;
			bindingContext = null;
			base.Unbind();
			lexicalScope = null;
			ParseContext.ResetBind();
			UpdateBindErrors(null);
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		if (!base.IsBound)
		{
			return null;
		}
		return base.GetCompletionList(caretLocation, completionContext);
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		if (!base.IsBound)
		{
			return null;
		}
		return base.GetQuickInfoText(cursorLocation, displayInfoProvider);
	}

	internal override MethodHelpText GetMyMethodHelpText(LexLocation caretLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		if (!base.IsBound)
		{
			return null;
		}
		return base.GetMyMethodHelpText(caretLocation, displayInfoProvider);
	}

	internal override MethodNameAndParamLocations GetMyMethodNameAndParams(LexLocation caretLocation)
	{
		if (!base.IsBound)
		{
			return null;
		}
		return base.GetMyMethodNameAndParams(caretLocation);
	}

	internal override void PopulateRegions(List<Region> regions)
	{
		foreach (SqlCodeObject child in Children)
		{
			child.PopulateRegions(regions);
		}
		new CommentRegionScanner(this, regions).PopulateCommentRegions();
	}

	internal SqlScript(List<SqlBatch> batches, bool isIncrementalMode)
	{
		this.batches = new SqlBatchCollection(batches);
		errorCollection = parseErrors;
		this.isIncrementalMode = isIncrementalMode;
	}

	public IList<SqlIdentifier> RetrieveAllIdentifiers()
	{
		List<SqlIdentifier> list = new List<SqlIdentifier>(256);
		PopulateIdentifiers(list);
		return list;
	}

	internal override void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("SqlScript");
		if (base.Position != null)
		{
			writer.WriteAttributeString("Location", string.Format(CultureInfo.CurrentCulture, "({0}, {1})", base.StartLocation, base.EndLocation));
		}
		writer.WriteComment(base.Sql);
		if (Errors != null)
		{
			writer.WriteStartElement("Errors");
			foreach (Error error in Errors)
			{
				error.WriteXml(writer);
			}
			writer.WriteEndElement();
		}
		foreach (SqlCodeObject child in Children)
		{
			child.WriteXml(writer);
		}
		writer.WriteEndElement();
	}

	internal void UpdateParseErrors(ErrorCollection parseErrors)
	{
		this.parseErrors = parseErrors;
		CommitErrors();
	}

	private void CommitErrors()
	{
		errorCollection = ((bindErrors != null) ? ErrorCollection.Create(parseErrors.ErrorList, bindErrors.ErrorList) : parseErrors);
	}
}
