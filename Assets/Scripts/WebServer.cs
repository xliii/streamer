using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class WebServer : MonoBehaviour
{
	private readonly HttpListener _listener = new HttpListener();

	private string baseUrl = "http://127.0.0.1:3000";
	private const string streamLabs = "/streamlabs";
	private const string twitchApiBot = "/twitch-api-bot";
	private const string twitchApiStreamer = "/twitch-api-streamer";

	private const string twitchApiBotURL = "https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=9r82dk02lbj7cdk5ln8fkxzqnzvt4ic&redirect_uri=http://127.0.0.1:3000/twitch-api-bot&scope=chat_login";
	private const string twitchApiStreamerURL = "";

	private string redirect = "<html><head><title></title></head><body><script>window.location=\"http://127.0.0.1:3000/{0}?\"+document.location.hash.substring(1)</script></body></html>";

	public void Start()
	{
		if (!HttpListener.IsSupported)
		{
			throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
		}

		// URI prefixes are required, for example 
		// "http://localhost:8080/index/".
		_listener.Prefixes.Add(Url(streamLabs));
		_listener.Prefixes.Add(Url(twitchApiBot));
		_listener.Prefixes.Add(Url(twitchApiStreamer));
		_listener.Start();
		Run();
	}

	private string Url(string path)
	{
		return baseUrl + path + "/";
	}

	public string Process(HttpListenerRequest request)
	{
		//Debug.Log("Processing request: " + request.Url);
		var path = request.Url.AbsolutePath;
		switch (path)
		{
			case streamLabs:
			{
				string code = request.QueryString.Get("code");
				if (string.IsNullOrEmpty(code))
				{
					return "Error: no \"code\" param found";
				}
				
				Debug.Log("Code: " + code);
				return "StreamLabs callback processed!";
			}
			case twitchApiBot:
			{
				string token = request.QueryString.Get("access_token");
				if (string.IsNullOrEmpty(token))
				{
					return RedirectUrl(twitchApiBot);
					//return "Error: no \"access_token\" param found";
				}
				Debug.Log("Bot token: " + token);
				return "Successfully authenticated with bot!";
			}
			case twitchApiStreamer:
			{
					string token = request.QueryString.Get("access_token");
					if (string.IsNullOrEmpty(token))
					{
						return RedirectUrl(twitchApiStreamer);
						//return "Error: no \"access_token\" param found";
					}
					Debug.Log("Streamer token: " + token);
					return "Successfully authenticated with streamer!";
				}
			default:
			{
				Debug.LogWarning("Don't know how to process: " + path);
				return "Don't know how to process this :(";
			}

		}
	}

	private string RedirectUrl(string path)
	{
		return string.Format(redirect, path);
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
