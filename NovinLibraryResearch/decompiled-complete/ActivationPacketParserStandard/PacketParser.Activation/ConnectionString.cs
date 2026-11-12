namespace PacketParser.Activation;

public static class ConnectionString
{
	public static string LocalConnection { get; set; } = "Data Source=185.231.113.133;Initial Catalog=npsms_Activation;User Id=novin;Password=asdfsdfsdfgfdgety34FDGDFGDF#@$%#$$;Pooling=true;Connection Timeout=60000;TrustServerCertificate=True;MultipleActiveResultSets=True";


	public static string ConnectionStringActivation => "Data Source=185.231.113.133;Initial Catalog=npsms_Activation;User Id=novin;Password=asdfsdfsdfgfdgety34FDGDFGDF#@$%#$$;Pooling=true;Connection Timeout=30000;TrustServerCertificate=True;MultipleActiveResultSets=True";
}
