using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Management.SqlParser.Diagnostics;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

public static class BinderProvider
{
	private sealed class Binder : IBinder
	{
		private readonly IMetadataProvider m_metadataProvider;

		private MetadataProviderEventArgs m_eventArgs;

		private event MetadataProviderEventHandler m_beforeBind;

		private event MetadataProviderEventHandler m_afterBind;

		public Binder(IMetadataProvider metadataProvider)
		{
			m_metadataProvider = metadataProvider;
			MetadataProviderEventHandler beforeBindHandler = metadataProvider.BeforeBindHandler;
			if (beforeBindHandler != null)
			{
				m_beforeBind += beforeBindHandler;
			}
			MetadataProviderEventHandler afterBindHandler = metadataProvider.AfterBindHandler;
			if (afterBindHandler != null)
			{
				m_afterBind += afterBindHandler;
			}
			m_eventArgs = new MetadataProviderEventArgs();
		}

		public IServer Bind(IEnumerable<ParseResult> parseResults, string databaseName, BindMode bindMode)
		{
			using (TraceHelper.TraceContext.GetActivityContext("Bind"))
			{
				return BindAll(parseResults, databaseName, bindMode).ProxyObject;
			}
		}

		private ServerEx BindAll(IEnumerable<ParseResult> parseResults, string databaseName, BindMode bindMode)
		{
			using (TraceHelper.TraceContext.GetMethodContext("BindAll"))
			{
				if (parseResults == null)
				{
					throw new ArgumentNullException("scripts");
				}
				if (databaseName == null)
				{
					throw new ArgumentNullException("databaseName");
				}
				try
				{
					OnBeforeBind(m_eventArgs);
					ServerEx serverEx = CreateServerEx(m_metadataProvider, bindMode);
					BatchBindingContext bindingContext = new BatchBindingContext(serverEx[databaseName]);
					BindWorkList bindWorkList = ((bindMode == BindMode.Build) ? new BindWorkList() : null);
					foreach (ParseResult parseResult in parseResults)
					{
						SqlScript script = (parseResult ?? throw new ArgumentException(SR.ParseResultsShouldNotContainNullElement, "parseResults")).Script;
						if (script.IsBound)
						{
							script.Unbind();
						}
						script.BeforeBind();
						script.Bind(bindingContext, bindWorkList);
					}
					bindWorkList?.BindAll(bindingContext);
					bindWorkList?.RunPostBindBuildModeChecks();
					foreach (ParseResult parseResult2 in parseResults)
					{
						(parseResult2 ?? throw new ArgumentException(SR.ParseResultsShouldNotContainNullElement, "parseResults")).Script.AfterBind();
					}
					return serverEx;
				}
				finally
				{
					OnAfterBind(m_eventArgs);
				}
			}
		}

		private void OnBeforeBind(MetadataProviderEventArgs e)
		{
			this.m_beforeBind?.Invoke(this, e);
		}

		private void OnAfterBind(MetadataProviderEventArgs e)
		{
			this.m_afterBind?.Invoke(this, e);
		}

		private static ServerEx CreateServerEx(IMetadataProvider metadataProvider, BindMode bindMode)
		{
			return bindMode switch
			{
				BindMode.Batch => new ServerEx(metadataProvider.Server, metadataProvider.SystemDataTypeLookup, metadataProvider.BuiltInFunctionLookup, metadataProvider.CollationLookup, metadataProvider.MetadataFactory), 
				BindMode.Build => new ServerEx(metadataProvider.MetadataFactory.Server.Create(metadataProvider.Server), metadataProvider.SystemDataTypeLookup, metadataProvider.BuiltInFunctionLookup, metadataProvider.CollationLookup, metadataProvider.MetadataFactory), 
				_ => null, 
			};
		}

		[Conditional("DEBUG")]
		private static void ValidateBoundObjects(IEnumerable<ParseResult> parseResults)
		{
			new BindChecks.MetadataObjectValidator();
			foreach (ParseResult parseResult in parseResults)
			{
				_ = parseResult;
			}
		}
	}

	public static IBinder CreateBinder(IMetadataProvider metadataProvider)
	{
		if (metadataProvider == null)
		{
			throw new ArgumentNullException("metadataProvider");
		}
		if (metadataProvider.Server == null)
		{
			throw new ArgumentException("Metadata provider 'Server' property cannot be null!", "metadataProvider");
		}
		if (metadataProvider.SystemDataTypeLookup == null)
		{
			throw new ArgumentException("Metadata provider 'SystemDataTypeLookup' property cannot be null!", "metadataProvider");
		}
		if (metadataProvider.BuiltInFunctionLookup == null)
		{
			throw new ArgumentException("Metadata provider 'BuiltInFunctionLookup' property cannot be null!", "metadataProvider");
		}
		if (metadataProvider.CollationLookup == null)
		{
			throw new ArgumentException("Metadata provider 'CollationLookup' property cannot be null!", "metadataProvider");
		}
		if (metadataProvider.MetadataFactory == null)
		{
			throw new ArgumentException("Metadata provider 'MetadataFactory' property cannot be null!", "metadataProvider");
		}
		return new Binder(metadataProvider);
	}
}
