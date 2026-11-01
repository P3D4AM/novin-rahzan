using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[assembly: AssemblyMetadata("Serviceable", "True")]
[assembly: AssemblyMetadata("PreferInbox", "True")]
[assembly: AssemblyDefaultAlias("System.Threading.Tasks.Dataflow")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("IsTrimmable", "True")]
[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: AssemblyDescription("TPL Dataflow promotes actor/agent-oriented designs through primitives for in-process message passing, dataflow, and pipelining. TDF builds upon the APIs and scheduling infrastructure provided by the Task Parallel Library (TPL), and integrates with the language support for asynchrony provided by C#, Visual Basic, and F#.\r\n\r\nCommonly Used Types:\r\nSystem.Threading.Tasks.Dataflow.ActionBlock<TInput>\r\nSystem.Threading.Tasks.Dataflow.BatchBlock<T>\r\nSystem.Threading.Tasks.Dataflow.BatchedJoinBlock<T1, T2>\r\nSystem.Threading.Tasks.Dataflow.BatchedJoinBlock<T1, T2, T3>\r\nSystem.Threading.Tasks.Dataflow.BroadcastBlock<T>\r\nSystem.Threading.Tasks.Dataflow.BufferBlock<T>\r\nSystem.Threading.Tasks.Dataflow.DataflowBlock\r\nSystem.Threading.Tasks.Dataflow.JoinBlock<T1, T2>\r\nSystem.Threading.Tasks.Dataflow.JoinBlock<T1, T2, T3>\r\nSystem.Threading.Tasks.Dataflow.TransformBlock<TInput, TOutput>\r\nSystem.Threading.Tasks.Dataflow.TransformManyBlock<TInput, TOutput>\r\nSystem.Threading.Tasks.Dataflow.WriteOnceBlock<T>")]
[assembly: AssemblyFileVersion("8.0.724.31311")]
[assembly: AssemblyInformationalVersion("8.0.7+2aade6beb02ea367fd97c4070a4198802fe61c03")]
[assembly: AssemblyProduct("Microsoft® .NET")]
[assembly: AssemblyTitle("System.Threading.Tasks.Dataflow")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: AssemblyVersion("8.0.0.1")]
[module: System.Runtime.CompilerServices.RefSafetyRules(11)]
[module: System.Runtime.CompilerServices.NullablePublicOnly(false)]
