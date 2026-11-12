using Azure.Core.Pipeline;

namespace Azure.Core;

internal class AzureKeyCredentialPolicy : HttpPipelineSynchronousPolicy
{
	private readonly string _name;

	private readonly AzureKeyCredential _credential;

	public AzureKeyCredentialPolicy(AzureKeyCredential credential, string name)
	{
		Argument.AssertNotNull(credential, "credential");
		Argument.AssertNotNullOrEmpty(name, "name");
		_credential = credential;
		_name = name;
	}

	public override void OnSendingRequest(HttpMessage message)
	{
		base.OnSendingRequest(message);
		message.Request.Headers.SetValue(_name, _credential.Key);
	}
}
