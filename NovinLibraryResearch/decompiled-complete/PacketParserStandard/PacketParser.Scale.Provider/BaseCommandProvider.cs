using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Scale.Entities;
using PacketParser.Scale.EventArguments;
using Serilog;

namespace PacketParser.Scale.Provider;

public abstract class BaseCommandProvider : IScaleCommandProvider, INpDisposable, IDisposable
{
	protected string buffer;

	private bool _isDisposed;

	protected ILogger logger;

	protected readonly MicroScaleInfo info;

	protected SemaphoreSlim sem;

	public IScalePacketParser PacketParser { get; private set; }

	protected IScaleEventReceiver receiver { get; private set; }

	protected IScaleConnection connection { get; private set; }

	public abstract bool ScaleJustWeight { get; }

	public bool IsDisposed => _isDisposed;

	public BaseCommandProvider(EnReadWeightType readWeightType, IScaleConnection connection, IScaleEventReceiver receiver, MicroScaleInfo info, ILogger logger)
	{
		this.logger = logger;
		this.receiver = receiver;
		this.info = info;
		this.connection = connection;
		PacketParser = GenerateScalePacketParser();
		if (PacketParser != null)
		{
			PacketParser.NewFactor += PacketParser_NewFactor;
		}
		sem = new SemaphoreSlim(1, 1);
		Task.Run((Func<Task?>)ResetBufferAsync);
		if (ScaleJustWeight && readWeightType == EnReadWeightType.ByRequest)
		{
			Task.Run(() => ReadWeightLoopAsync());
		}
	}

	private async Task ReadWeightLoopAsync(CancellationToken inputToken = default(CancellationToken))
	{
		while (!IsDisposed && !inputToken.IsCancellationRequested)
		{
			try
			{
				Log.Verbose("sending Read Weight Request....");
				CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(inputToken, new CancellationTokenSource(1000).Token);
				ReturnedSaveFuncInfoWithValue<WeightEventArgs> returnedSaveFuncInfoWithValue = await ReadWeightByRequestAsync(info.ScaleNumber, cancellationTokenSource.Token);
				if (returnedSaveFuncInfoWithValue.HasError || returnedSaveFuncInfoWithValue.value == null)
				{
					Log.Verbose("read weight has error!! " + returnedSaveFuncInfoWithValue.ErrorMessage);
					continue;
				}
				Log.Verbose($"sending Read Weight result: GrossWeight:{returnedSaveFuncInfoWithValue.value.GrossWeight}  NetWeight:{returnedSaveFuncInfoWithValue.value.NetWeight}");
			}
			catch (OperationCanceledException)
			{
				Log.Verbose("readWeight operation has been cancelled!");
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ReadWeightLoopAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 57);
			}
			finally
			{
				await Task.Delay(10);
			}
		}
	}

	private async Task ResetBufferAsync()
	{
		while (!IsDisposed)
		{
			try
			{
				if (!string.IsNullOrEmpty(buffer))
				{
					buffer = "";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResetBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 68);
			}
			finally
			{
				await Task.Delay(60000);
			}
		}
	}

	protected virtual Task PacketParser_NewFactor(object sender, FactorEventArgs e)
	{
		return Task.CompletedTask;
	}

	protected abstract IScalePacketParser GenerateScalePacketParser();

	public Task ClearMemory(int scaleNumber, uint index, CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ClearMemory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 87);
		}
		return Task.CompletedTask;
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (PacketParser != null)
				{
					PacketParser.NewFactor -= PacketParser_NewFactor;
				}
				PacketParser = null;
				logger = null;
				receiver = null;
				connection?.Dispose();
				connection = null;
				sem?.Dispose();
				sem = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 108);
		}
	}

	public Task Ping(int scaleNumber, CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Ping", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 117);
		}
		return Task.CompletedTask;
	}

	public abstract Task<FactorModel> ReadLastUnfetchedReciept(int scaleNumber, CancellationToken token = default(CancellationToken));

	public Task<ProductModel> ReadPluMemory(int scaleNumber, int memoryIndex, CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReadPluMemory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 139);
		}
		return Task.FromResult<ProductModel>(null);
	}

	public Task<List<ProductModel>> ReadPluMemoryBatch(int scaleNumber, int memoryIndex, int length, CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReadPluMemoryBatch", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 149);
		}
		return Task.FromResult<List<ProductModel>>(null);
	}

	public Task<FactorModel> ReadReceipt(int scaleNumber, int receiptIndex, CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReadReceipt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 159);
		}
		return Task.FromResult<FactorModel>(null);
	}

	public Task<List<FactorModel>> ReadReceiptBatch(int scaleNumber, int startIndex, int length, CancellationToken token = default(CancellationToken))
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReadReceiptBatch", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 169);
		}
		return Task.FromResult<List<FactorModel>>(null);
	}

	public abstract Task<ReturnedSaveFuncInfo> WritePluMemoryAsync(int scaleNumber, int memoryIndex, PluWritableMemory content, CancellationToken token = default(CancellationToken));

	public virtual async Task<ReturnedSaveFuncInfo> WritePluMemoryBatchAsync(int scaleNumber, IEnumerable<uint> memoryIndexes, IEnumerable<PluWritableMemory> contents, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (!(contents?.Any() ?? false))
				{
					return ret;
				}
				List<uint> mems = memoryIndexes.ToList();
				List<PluWritableMemory> cntns = contents.ToList();
				if (memoryIndexes.Count() != contents.Count())
				{
					throw new ArgumentException($"memoryIndexes.Count({memoryIndexes.Count()}) != contents.Count({contents.Count()})");
				}
				for (int i = 0; i < contents.Count(); i++)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await WritePluMemoryAsync(scaleNumber, (int)mems[i], cntns[i], token), ReturnedState.None, "WritePluMemoryBatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 187);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WritePluMemoryBatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 191);
				ret.AddReturnedValue(ex, "", "WritePluMemoryBatchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Provider\\BaseCommandProvider.cs", 192);
			}
			return ret;
		}
	}

	public abstract Task<ReturnedSaveFuncInfo> ChangeReadWeightStatusAsync(int scaleNumber, EnReadWeightType type, CancellationToken token = default(CancellationToken));

	public abstract Task<ReturnedSaveFuncInfoWithValue<WeightEventArgs>> ReadWeightByRequestAsync(int scaleNumber, CancellationToken token = default(CancellationToken));

	public abstract Task<ReturnedSaveFuncInfo> TareAsync(MicroScaleInfo scaleInfo, CancellationToken token);
}
