using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Configuration;
using Microsoft.SqlServer.Management.Assessment.Configuration.Json;
using Microsoft.SqlServer.Management.Assessment.Logics;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Microsoft.SqlServer.Management.Assessment.Tools;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment;

public class Engine
{
	private static readonly Check UnsupportedTargetCheck = new Check
	{
		AssessmentMessage = Resources.UnsupportedTargetCheckMessage,
		Condition = null,
		Description = Resources.UnsupportedTargetCheckDescription,
		DisplayName = Resources.UnsupportedTargetCheckDisplayName,
		Enabled = true,
		HelpLink = string.Empty,
		Id = "__NoChecks",
		Level = SeverityLevel.Information,
		OriginVersion = new Version(1, 0),
		OriginName = "Microsoft Ruleset",
		Logics = TwoPhaseLogics.Instance
	};

	private const string VmSqlQueryText = "SELECT virtual_machine_type FROM sys.dm_os_sys_info";

	private const string VmAssetWmiQueryText = "Select * FROM Win32_SystemEnclosure WHERE SMBIOSAssetTag = '7783-7084-3265-9085-8269-3286-77'";

	private const string VmIdWmiQueryText = "SELECT * FROM Win32_ComputerSystemProduct";

	private const string AzureResourceIdPropertyName = "id";

	private const string AzureVmResourceIdPropertyName = "vmResId";

	private const string SqlOnVmKustoQueryText = "Resources\r\n| where type =~ 'Microsoft.SqlVirtualMachine/sqlVirtualMachines'\r\n| extend vmResId = toupper(tostring(properties.virtualMachineResourceId))\r\n| join (Resources\r\n    | where type =~ 'Microsoft.Compute/virtualMachines'\r\n    | where properties.vmId =~ '{0}'\r\n    | extend vmResId = toupper(tostring(id))) on vmResId\r\n| project name, id, vmResId\r\n| top 1 by name";

	private const string UuidPropertyName = "Output.UUID";

	private const string AzureVmIdName = "azure_vm_id";

	private const string AzureVmResIdName = "azure_vm_res_id";

	private const string AzureSqlOnVmResIdName = "azure_sql_on_vm_res_id";

	internal Inventory Inventory { get; } = new Inventory();


	public EngineConfig Configuration { get; } = new EngineConfig();


	private Scheduler Scheduler { get; } = new Scheduler();


	private EvaluationContext<object> SystemEvaluationContext { get; }

	internal EvaluationContext<object> GlobalEvaluationContext { get; }

	public Engine()
	{
		InitializeInventory();
		SystemEvaluationContext = new EvaluationContext<object>("system");
		GlobalEvaluationContext = new EvaluationContext<object>("global", SystemEvaluationContext);
		LoadJsonConfig();
		InitializeCharacteristicProviders();
	}

	private void InitializeInventory()
	{
		SetTool(SqlTool.Create);
		SetTool(XpCmdShellTool.Create);
		SetTool(CmdPowerShellTool.Create);
		SetTool(PsWmiTool.Create);
		SetTool(XpRegXTool.Create);
		SetTool(UnavailableAzureTool.Create);
	}

	private void InitializeCharacteristicProviders()
	{
		Configuration.CharacteristicProviderSource.Add(new PermissionsCharacteristicProvider());
		Configuration.CharacteristicProviderSource.Add(new RoleCharacteristicProvider());
		Configuration.CharacteristicProviderSource.Add(new FeatureCharacteristicProvider());
		Configuration.CharacteristicProviderSource.Add(new ChannelPresenceProvider());
		Configuration.CharacteristicProviderSource.Add(new SysConfigurationsProvider());
		Configuration.CharacteristicProviderSource.Add(new ServicesProvider());
	}

	public Task<List<IAssessmentResult>> GetAssessmentResultsList(ISqlObjectLocator target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return GetAssessmentResultsListCore(target);
	}

	public Task<List<IAssessmentResult>> GetAssessmentResultsList(ISqlObjectLocator target, SeverityLevel minLevel)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return GetAssessmentResultsListCore(target, null, (ICheck c) => c.Level >= minLevel);
	}

	public Task<List<IAssessmentResult>> GetAssessmentResultsList(ISqlObjectLocator target, IEnumerable<string> checkNamesOrTags)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (checkNamesOrTags == null)
		{
			throw new ArgumentNullException("checkNamesOrTags");
		}
		return GetAssessmentResultsListCore(target, checkNamesOrTags.ToArray());
	}

	public Task<List<IAssessmentResult>> GetAssessmentResultsList(ISqlObjectLocator target, IEnumerable<string> checkNamesOrTags, SeverityLevel minLevel)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (checkNamesOrTags == null)
		{
			throw new ArgumentNullException("checkNamesOrTags");
		}
		return GetAssessmentResultsListCore(target, checkNamesOrTags.ToArray(), (ICheck c) => c.Level >= minLevel);
	}

	public Task<List<IAssessmentResult>> GetAssessmentResultsList(ISqlObjectLocator target, IEnumerable<ICheck> checks)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (checks == null)
		{
			throw new ArgumentNullException("checks");
		}
		return GetAssessmentResultsListCore(target, checks.Select((ICheck c) => c.Id).ToArray());
	}

	public IEnumerable<ICheck> GetChecks(ISqlObjectLocator target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		AssessmentRequest2 request = new AssessmentRequest2(target, GlobalEvaluationContext, null, null, isDryRun: true);
		return GetChecksCore(request).Result;
	}

	public async Task<IReadOnlyList<ICheck>> GetChecksAsync(ISqlObjectLocator target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		AssessmentRequest2 request = new AssessmentRequest2(target, GlobalEvaluationContext, null, null, isDryRun: true);
		return (await GetChecksCore(request).ConfigureAwait(continueOnCapturedContext: false)).ToList();
	}

	public IEnumerable<ICheck> GetChecks(ISqlObjectLocator target, string[] checkNamesOrTags)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (checkNamesOrTags == null)
		{
			throw new ArgumentNullException("checkNamesOrTags");
		}
		AssessmentRequest2 request = new AssessmentRequest2(target, GlobalEvaluationContext, checkNamesOrTags, null, isDryRun: true);
		return GetChecksCore(request).Result;
	}

	public async Task<IReadOnlyList<ICheck>> GetChecksAsync(ISqlObjectLocator target, string[] checkNamesOrTags)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (checkNamesOrTags == null)
		{
			throw new ArgumentNullException("checkNamesOrTags");
		}
		AssessmentRequest2 request = new AssessmentRequest2(target, GlobalEvaluationContext, checkNamesOrTags, null, isDryRun: true);
		return (await GetChecksCore(request).ConfigureAwait(continueOnCapturedContext: false)).ToList();
	}

	public void SetTool<TToolInterface>(Func<RunSpace, TToolInterface> constructor) where TToolInterface : ITool
	{
		Inventory.Add(constructor);
	}

	internal Task<List<IAssessmentResult>> GetAssessmentResultsList(ISqlObjectLocator target, IEnumerable<string> checks, Func<ICheck, bool> checkSelector)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (checks == null)
		{
			throw new ArgumentNullException("checks");
		}
		return GetAssessmentResultsListCore(target, checks.ToArray(), checkSelector);
	}

	public void PushRuleFactoryJson(Stream inputStream, string alias = null)
	{
		PushRuleFactory(LoadRuleFactoryJson(inputStream), alias);
	}

	public void PushRuleFactoryJson(TextReader textReader, string alias = null)
	{
		PushRuleFactory(LoadRuleFactoryJson(textReader), alias);
	}

	public void PushRuleFactoryJson(JsonTextReader reader, string alias = null)
	{
		PushRuleFactory(LoadRuleFactoryJson(reader), alias);
	}

	public void PushRuleFactory(Ruleset factory, string alias = null)
	{
		if (factory == null)
		{
			throw new ArgumentNullException("factory");
		}
		string text = alias ?? factory.Name;
		Configuration.AddRuleset(factory, text);
		Configuration.CurrentRulesets.Add(text);
	}

	public Ruleset LoadRuleFactoryJson(Stream inputStream)
	{
		using StreamReader textReader = new StreamReader(inputStream);
		return LoadRuleFactoryJson(textReader);
	}

	public Ruleset LoadRuleFactoryJson(TextReader textReader)
	{
		using JsonTextReader reader = new JsonTextReader(textReader);
		return LoadRuleFactoryJson(reader);
	}

	public Ruleset LoadRuleFactoryJson(JsonTextReader reader)
	{
		JsonSerializer jsonSerializer = SerializationUtil.CreateJsonSerializer();
		return jsonSerializer.Deserialize<Ruleset>(reader);
	}

	protected internal virtual async Task<DbConnection> GetOpenedConnectionAsync(ISqlObjectLocator target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		DbConnection connection = ((target.Connection is ICloneable cloneable) ? (cloneable.Clone() as DbConnection) : target.Connection);
		if (connection == null)
		{
			return null;
		}
		while (connection.State == ConnectionState.Connecting)
		{
			await Task.Yield();
		}
		switch (connection.State)
		{
		case ConnectionState.Closed:
			await connection.OpenAsync().ConfigureAwait(continueOnCapturedContext: false);
			break;
		}
		return connection;
	}

	private static IEnumerable<Check> GetImplementedChecks(IEnumerable<ICheck> input, ICollection<ICheck> unimplemented)
	{
		foreach (ICheck item in input)
		{
			if (item is Check check)
			{
				yield return check;
			}
			else
			{
				unimplemented.Add(item);
			}
		}
	}

	private static StreamReader GetDefaultConfigurationStream()
	{
		Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Microsoft.SqlServer.Management.Assessment.config.json");
		return new StreamReader(manifestResourceStream);
	}

	private async Task<IEnumerable<ICheck>> GetChecksCore(AssessmentRequest2 request)
	{
		if (request.CheckSelector != null && request.CheckSelector.LongLength == 0L)
		{
			return Array.Empty<ICheck>();
		}
		await Scheduler.Run(request, ProcessAssessmentRequest).ConfigureAwait(continueOnCapturedContext: false);
		return request.Checks;
	}

	private Task<List<IAssessmentResult>> GetAssessmentResultsListCore(ISqlObjectLocator target, string[] stringSelector = null, Func<ICheck, bool> selector = null)
	{
		if (stringSelector != null && stringSelector.LongLength == 0L)
		{
			return Task.FromResult(new List<IAssessmentResult>());
		}
		AssessmentRequest2 request = new AssessmentRequest2(target, GlobalEvaluationContext, stringSelector, selector);
		return Scheduler.Run(request, ProcessAssessmentRequest);
	}

	private void LoadJsonConfig()
	{
		PushRuleFactoryJson(GetDefaultConfigurationStream(), string.Empty);
		Configuration.DefaultRuleset = Configuration.Rulesets.First().Value.First().Value;
		UnsupportedTargetCheck.OriginVersion = Configuration.DefaultRuleset.Version;
		UnsupportedTargetCheck.OriginName = Configuration.DefaultRuleset.Name;
	}

	private static DbConnection GetSqlConnectionClone(ISqlObjectLocator target)
	{
		return ((ICloneable)target.Connection).Clone() as DbConnection;
	}

	private async Task<IEnumerable<IAssessmentResult>> ProcessRequestAsync(AssessmentRequest request)
	{
		List<IAssessmentResult> results = new List<IAssessmentResult>();
		using (DbConnection connection = await GetOpenedConnectionAsync(request.Target).ConfigureAwait(continueOnCapturedContext: false))
		{
			foreach (IGrouping<ILogicsProvider, Check> item in from c in request.Checks
				group c by c.Logics)
			{
				try
				{
					List<IAssessmentResult> list = results;
					list.AddRange(await item.Key.GetAssessmentResults(request, connection, Configuration).ConfigureAwait(continueOnCapturedContext: false));
				}
				catch (Exception exception)
				{
					if (HandleException(exception, request, out var result))
					{
						results.AddRange(result);
					}
					throw;
				}
			}
		}
		return results;
	}

	private async Task<List<IAssessmentResult>> ProcessAssessmentRequest(AssessmentRequest2 request)
	{
		List<IAssessmentResult> results = new List<IAssessmentResult>();
		using (DbConnection connection = await GetOpenedConnectionAsync(request.Target).ConfigureAwait(continueOnCapturedContext: false))
		{
			RunSpace runSpace = new RunSpace(request.Target, connection, Inventory, Configuration, request.EvaluationContext);
			await MakeSpecific(runSpace).ConfigureAwait(continueOnCapturedContext: false);
			IEnumerable<ICheck> enumerable = ((request.CheckSelector == null) ? Configuration.GetChecks(request.Target).ToList() : Configuration.GetChecks(request.Target, request.CheckSelector).ToList());
			if (request.Predicate != null)
			{
				enumerable = enumerable.Where(request.Predicate).ToList();
			}
			List<ICheck> list = new List<ICheck>(0);
			Check[] array = GetImplementedChecks(enumerable, list).ToArray();
			if (array.Length == 0)
			{
				array = new Check[1] { UnsupportedTargetCheck };
			}
			if (request.IsDryRun)
			{
				request.Checks = array;
			}
			else
			{
				results.AddRange(((IEnumerable<ICheck>)list).Select((Func<ICheck, IAssessmentResult>)((ICheck i) => new CheckError(i, string.Format(CultureInfo.CurrentCulture, Resources.ErrorCheckHasNoImplementation, i.Id), request.Target.Type, request.Target.Urn))));
				foreach (IGrouping<ILogicsProvider, Check> item in from c in array
					group c by c.Logics)
				{
					request.Checks = item;
					try
					{
						ILogicsProvider key = item.Key;
						if (!(key is ILogicsProvider2 logicsProvider))
						{
							if (key != null)
							{
								List<IAssessmentResult> list2 = results;
								list2.AddRange(await key.GetAssessmentResults(request, connection, Configuration).ConfigureAwait(continueOnCapturedContext: false));
							}
						}
						else
						{
							List<IAssessmentResult> list2 = results;
							list2.AddRange(await logicsProvider.GetAssessmentResults(request, runSpace).ConfigureAwait(continueOnCapturedContext: false));
						}
					}
					catch (Exception exception)
					{
						if (HandleException(exception, request, out var result))
						{
							results.AddRange(result);
						}
						throw;
					}
				}
			}
		}
		return results;
	}

	private static bool HandleException(Exception exception, ITargetedObject subject, out IAssessmentResult[] result)
	{
		if (!(exception is DbException))
		{
			if (exception is AggregateException ex2 && ex2.InnerExceptions.Count == 1)
			{
				return HandleException(ex2.InnerException, subject, out result);
			}
			result = null;
			return false;
		}
		FatalError fatalError = new FatalError(subject.Target, string.Format(CultureInfo.CurrentCulture, Resources.ErrorAccessTarget, subject.Target.Type, subject.Target.Name), exception);
		result = new IAssessmentResult[1] { fatalError };
		return true;
	}

	private async Task MakeSpecific(RunSpace runSpace)
	{
		await SpecifyVmType(runSpace).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task SpecifyVmType(RunSpace runSpace)
	{
		if (runSpace.Target.MachineType != null)
		{
			return;
		}
		try
		{
			await Configuration.CharacteristicProviderSource.Update(runSpace, new CharacteristicCollection("channel", "SQL", "WMI", "Azure")).ConfigureAwait(continueOnCapturedContext: false);
			ISqlObjectLocator target2 = runSpace.Target;
			if (!(target2 is SqlObjectLocator target) || !string.IsNullOrWhiteSpace(target.MachineType))
			{
				return;
			}
			ISqlTool sqlTool = runSpace.GetTool<ISqlTool>();
			bool flag = sqlTool == null;
			bool flag2 = flag;
			if (!flag2)
			{
				flag2 = !(await sqlTool.IsAvailable(runSpace).ConfigureAwait(continueOnCapturedContext: false));
			}
			if (flag2)
			{
				return;
			}
			using (DbCommand command = sqlTool.CreateCommand("SELECT virtual_machine_type FROM sys.dm_os_sys_info"))
			{
				switch ((int)(await command.ExecuteScalarAsync().ConfigureAwait(continueOnCapturedContext: false)))
				{
				case 0:
					target.MachineType = "Physical";
					return;
				case 1:
					target.MachineType = "Hypervisor";
					break;
				default:
					target.MachineType = "Other";
					return;
				}
			}
			IWmiTool wmiTool = runSpace.GetTool<IWmiTool>();
			bool flag3 = wmiTool == null;
			bool flag4 = flag3;
			if (!flag4)
			{
				flag4 = !(await wmiTool.IsAvailable(runSpace).ConfigureAwait(continueOnCapturedContext: false));
			}
			if (flag4 || (await wmiTool.Execute("Select * FROM Win32_SystemEnclosure WHERE SMBIOSAssetTag = '7783-7084-3265-9085-8269-3286-77'", null, null).ConfigureAwait(continueOnCapturedContext: false)).Count == 0)
			{
				return;
			}
			IDataView dataView = await wmiTool.Execute("SELECT * FROM Win32_ComputerSystemProduct", null, null).ConfigureAwait(continueOnCapturedContext: false);
			if (dataView.Count == 0 || !dataView[0].TryGetData("Output.UUID", out var vmId) || !(vmId is string vmIdStr) || string.IsNullOrWhiteSpace(vmIdStr))
			{
				return;
			}
			IAzureTool azureTool = runSpace.GetTool<IAzureTool>();
			bool flag5 = azureTool == null;
			bool flag6 = flag5;
			if (!flag6)
			{
				flag6 = !(await azureTool.IsAvailable(runSpace).ConfigureAwait(continueOnCapturedContext: false));
			}
			if (flag6)
			{
				return;
			}
			string kustoQuery = string.Format(CultureInfo.InvariantCulture, "Resources\r\n| where type =~ 'Microsoft.SqlVirtualMachine/sqlVirtualMachines'\r\n| extend vmResId = toupper(tostring(properties.virtualMachineResourceId))\r\n| join (Resources\r\n    | where type =~ 'Microsoft.Compute/virtualMachines'\r\n    | where properties.vmId =~ '{0}'\r\n    | extend vmResId = toupper(tostring(id))) on vmResId\r\n| project name, id, vmResId\r\n| top 1 by name", vmIdStr);
			IDataView dataView2 = await azureTool.Query(kustoQuery).ConfigureAwait(continueOnCapturedContext: false);
			if (dataView2.Count != 0)
			{
				IDataRow dataRow = dataView2[0];
				if (dataRow.TryGetData("id", out var value) && dataRow.TryGetData("vmResId", out var value2))
				{
					GlobalEvaluationContext.Data.Add("azure_vm_id", vmId);
					GlobalEvaluationContext.Data.Add("azure_sql_on_vm_res_id", value);
					GlobalEvaluationContext.Data.Add("azure_vm_res_id", value2);
					target.MachineType = "AzureSQL";
				}
			}
		}
		catch (Exception ex)
		{
			if (ex is StackOverflowException || ex is OutOfMemoryException)
			{
				throw;
			}
		}
	}
}
