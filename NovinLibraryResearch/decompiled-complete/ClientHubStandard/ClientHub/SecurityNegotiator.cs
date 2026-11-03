using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;

namespace ClientHub;

internal class SecurityNegotiator
{
	private TaskCompletionSource<bool> symmetricSource;

	private SymmetricAlgorithm symmetric;

	private byte[] symmetricKey;

	private byte[] symmetricIv;

	private ISecuredConnection client;

	public event EventHandler<KeyEventArgs> KeyNegotiated;

	public SecurityNegotiator(ISecuredConnection client)
	{
		try
		{
			this.client = client;
			client.PublicKeyReceived += Connection_PublicKeyReceived;
			client.Secured += Client_Secured;
			client.Reset += Client_Reset;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\SecurityNegotiator.cs", 31);
		}
	}

	private Task Client_Reset(object arg1, EventArgs arg2)
	{
		try
		{
			Cancel();
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Client_Reset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\SecurityNegotiator.cs", 43);
			return Task.CompletedTask;
		}
	}

	private async Task Client_Secured(object arg1, EventArgs arg2)
	{
		symmetricSource.TrySetResult(result: true);
		this.KeyNegotiated?.Invoke(this, new KeyEventArgs
		{
			SymmetricIv = symmetricIv,
			SymmetricKey = symmetricKey
		});
	}

	private async Task Connection_PublicKeyReceived(object sender, PublicKeyEventArgs e)
	{
		using (new JaegerMonitor("Connection_PublicKeyReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\SecurityNegotiator.cs"))
		{
			_ = 1;
			try
			{
				symmetric = Aes.Create();
				symmetric.KeySize = 256;
				symmetric.GenerateIV();
				symmetric.GenerateKey();
				symmetricKey = symmetric.Key;
				symmetricIv = symmetric.IV;
				RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
				rSACryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(e.Data));
				ConnectionKey generatedKey = new ConnectionKey
				{
					SymmetricKey = rSACryptoServiceProvider.Encrypt(symmetric.Key, fOAEP: true),
					SymmetricIv = rSACryptoServiceProvider.Encrypt(symmetric.IV, fOAEP: true)
				};
				if (await client.Connection.GetStateAsync() == EnConnectionState.Connected)
				{
					await client.Send(EnServerMessageInstruction.symmetricKey, Json.ToStringJson(generatedKey), 2);
				}
				else
				{
					client.Connection.Stop("if (!base.State == ConnectionState.Connected)");
				}
			}
			catch (Exception ex)
			{
				client.Connection.Stop("PublicKeyReceived//" + ex.Message);
				WebErrorLog.Instance.StartLog(ex, "", "Connection_PublicKeyReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\SecurityNegotiator.cs", 84);
			}
		}
	}

	public async Task SecureChannel()
	{
		try
		{
			await symmetricSource.Task;
		}
		catch
		{
		}
	}

	private void Cancel()
	{
		symmetricSource?.TrySetException(new Exception());
		symmetricSource = new TaskCompletionSource<bool>();
	}
}
