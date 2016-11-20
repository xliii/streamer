using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

public class NetworkListener : MonoBehaviour
{
	private static TcpListener listener;
	private static Socket socket;
	private static Stream stream;

	private static bool LOG;

	public int port;
	public bool log;

	private static Queue<string> events = new Queue<string>();

	// Use this for initialization
	void Start ()
	{
		LOG = log;
		listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
		listener.Start();
		if (LOG)
		{
			Debug.Log("Server launched: listening to port " + port);
		}
		new Thread(Service).Start();
	}

	void Update()
	{
		while (events.Count > 0)
		{
			string e = events.Dequeue();
			Debug.Log("Broadcasting: " + e);
			Messenger.Broadcast(e);
		}
	}

	public static void Service()
	{
		while (true)
		{
			socket = listener.AcceptSocket();
			if (LOG)
			{
				Debug.Log("Client connected");
			}
			
			try
			{
				stream = new NetworkStream(socket);
				StreamReader sr = new StreamReader(stream);
				while (true)
				{
					string input = sr.ReadLine();
					if (string.IsNullOrEmpty(input)) break;

					events.Enqueue(input);
				}
				stream.Close();
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);
			}
			if (LOG)
			{
				Debug.Log("Client disconnected");
			}
			socket.Close();
		}
	}

	void OnApplicationQuit()
	{
		if (LOG)
		{
			Debug.Log("Cleanup");
		}
		
		if (stream != null)
		{
			stream.Close();
		}
		socket.Close();
	}

}
