using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;

[assembly: InternalsVisibleTo("System.Collections.Immutable.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004b86c4cb78549b34bab61a3b1800e23bfeb5b3ec390074041536a7e3cbd97f5f04cf0f857155a8928eaa29ebfd11cfbbad3ba70efea7bda3226c6a8d370a4cd303f714486b6ebc225985a638471e6ef571cc92a4613c00b8fa65d61ccee0cbe5f36330c9a01f4183559f1bef24cc2917c6d913e3a541333a1d05d9bed22b38cb")]
[assembly: AssemblyMetadata("Serviceable", "True")]
[assembly: AssemblyMetadata("PreferInbox", "True")]
[assembly: AssemblyDefaultAlias("System.Collections.Immutable")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("IsTrimmable", "True")]
[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.System32 | DllImportSearchPath.AssemblyDirectory)]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: AssemblyDescription("This package provides collections that are thread safe and guaranteed to never change their contents, also known as immutable collections. Like strings, any methods that perform modifications will not change the existing instance but instead return a new instance. For efficiency reasons, the implementation uses a sharing mechanism to ensure that newly created instances share as much data as possible with the previous instance while ensuring that operations have a predictable time complexity.\r\n\r\nThe System.Collections.Immutable library is built-in as part of the shared framework in .NET Runtime. The package can be installed when you need to use it in other target frameworks.")]
[assembly: AssemblyFileVersion("8.0.23.53103")]
[assembly: AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: AssemblyProduct("Microsoft® .NET")]
[assembly: AssemblyTitle("System.Collections.Immutable")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: AssemblyVersion("8.0.0.0")]
[module: System.Runtime.CompilerServices.RefSafetyRules(11)]
[module: System.Runtime.CompilerServices.NullablePublicOnly(true)]
