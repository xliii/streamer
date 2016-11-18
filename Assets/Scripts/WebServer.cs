using System;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class WebServer : MonoBehaviour
{
	private readonly HttpListener _listener = new HttpListener();

	private string baseUrl = "http://127.0.0.1:3000";
	private const string streamLabs = "/streamlabs";

	public void Start()
	{
		if (!HttpListener.IsSupported)
		{
			throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
		}

		// URI prefixes are required, for example 
		// "http://localhost:8080/index/".
		_listener.Prefixes.Add(Url(streamLabs));
		_listener.Start();
		Run();
	}

	private string Url(string path)
	{
		return baseUrl + path + "/";
	}

	public string Process(HttpListenerRequest request)
	{
		Debug.Log("Processing request: " + request.Url);
		var path = request.Url.AbsolutePath;
		switch (path)
		{
			case streamLabs:
			{
				Debug.Log("CODE: " + request.QueryString.Get("code"));
				return "StreamLabs callback processed!";
			}
			default:
			{
				Debug.LogWarning("Don't know how to process: " + path);
				return "Don't know how to process this :(";
			}

		}
	}

	public void Stop()
	{
		_listener.Stop();
		_listener.Close();
	}

	public void Run()
	{
		ThreadPool.QueueUserWorkItem((o) =>
		{
			Console.WriteLine("Webserver running...");
			try
			{
				while (_listener.IsListening)
				{
					ThreadPool.QueueUserWorkItem((c) =>
					{
						var ctx = c as HttpListenerContext;
						try
						{
							string rstr = Process(ctx.Request);
							byte[] buf = Encoding.UTF8.GetBytes(rstr);
							ctx.Response.ContentLength64 = buf.Length;
							ctx.Response.OutputStream.Write(buf, 0, buf.Length);
						}
						catch (Exception e)
						{
							Debug.LogError("Error: " + e.Message);
						} // suppress any exceptions
						finally
						{
							// always close the stream
							ctx.Response.OutputStream.Close();
						}
					}, _listener.GetContext());
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Error: " + e.Message);
			} // suppress any exceptions
		});
	}

	void OnApplicationQuit()
	{
		Stop();
	}
}
