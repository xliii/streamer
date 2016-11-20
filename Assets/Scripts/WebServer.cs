using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class WebServer : MonoBehaviour
{
	private readonly HttpListener _listener = new HttpListener();

	private string baseUrl = "http://127.0.0.1:3000";
	private const string STREAM_LABS = "/streamlabs";	
	private const string TWITCH_API_BOT = "/twitch-api-bot";
	private const string TWITCH_API_STREAMER = "/twitch-api-streamer";

	private const string twitchApiBotURL = "https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=9r82dk02lbj7cdk5ln8fkxzqnzvt4ic&redirect_uri=http://127.0.0.1:3000/twitch-api-bot&scope=chat_login";
	private const string twitchApiStreamerURL = "https://api.twitch.tv/kraken/oauth2/authorize?response_type=token&client_id=dbkw8my5en45onuij6p05is8g76zlxj&redirect_uri=http://127.0.0.1:3000/twitch-api-streamer&scope=user_read%20channel_editor%20channel_subscriptions%20channel_check_subscription%20chat_login";
	private const string streamLabsURL = "http://streamlabs.com/api/v1.0/authorize?client_id=iFQ71QQzZIfl4s9TTx2pFW27YBOSOARZgve6g7Sc&redirect_uri=http://127.0.0.1:3000/streamlabs&response_type=code&scope=donations.read+donations.create";

	//TODO: Infinite redirect issue
	private string redirect = "<html><head><title></title></head><body><script>window.location=\"http://127.0.0.1:3000/{0}?\"+document.location.hash.substring(1)</script></body></html>";

	private static Queue<KeyValuePair<string, string>> events = new Queue<KeyValuePair<string, string>>();

	public void Start()
	{
		if (!HttpListener.IsSupported)
		{
			throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
		}

		// URI prefixes are required, for example 
		// "http://localhost:8080/index/".
		_listener.Prefixes.Add(Url(STREAM_LABS));
		_listener.Prefixes.Add(Url(TWITCH_API_BOT));
		_listener.Prefixes.Add(Url(TWITCH_API_STREAMER));
		_listener.Start();
		Run();
	}

	public void Update()
	{
		while (events.Count > 0)
		{
			var e = events.Dequeue();
			Messenger.Broadcast(e.Key, e.Value);
		}
	}

	private void Enqueue(string key, string value)
	{
		events.Enqueue(new KeyValuePair<string, string>(key, value));
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
			case STREAM_LABS:
			{
				string code = request.QueryString.Get("code");
				if (string.IsNullOrEmpty(code))
				{
					return "Error: no \"code\" param found";
				}

				Enqueue(Events.STREAM_LABS_CODE, code);
				return "StreamLabs callback processed!";
			}
			case TWITCH_API_BOT:
			{
				string token = request.QueryString.Get("access_token");
				if (string.IsNullOrEmpty(token))
				{
					return RedirectUrl(TWITCH_API_BOT);
				}
				
				Enqueue(Events.TWITCH_API_BOT_TOKEN, token);
				return "Successfully authenticated with bot!";
			}
			case TWITCH_API_STREAMER:
			{
				string token = request.QueryString.Get("access_token");
				if (string.IsNullOrEmpty(token))
				{
					return RedirectUrl(TWITCH_API_STREAMER);
				}

				Enqueue(Events.TWITCH_API_STREAMER_TOKEN, token);
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
						}
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
				if (e.Message == "Listener was closed.") return;

				Debug.LogError("Error: " + e.Message);
			}
		});
	}

	void OnApplicationQuit()
	{
		Stop();
	}
}
