using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

public class HTTPClient {

	private string login;
	private string skirmish;
	private string training;
	private string username;

	public struct MatchInfo {
		public int port;
		public string hash;
	}

	public HTTPClient (string baseURL) {
		login = baseURL + "/login";
		skirmish = baseURL + "/skirmish";
		training = baseURL + "/training";
	}

	public bool Login (string username) {
		this.username = username;
		string payload = "{\"user_name\": \"" + username + "\"}";
		byte[] data = Encoding.ASCII.GetBytes(payload);
		SendSync (login, data);
		return true;
	}

	public bool Skirmish () {
		string payload = "{\"user_name\": \"" + username + "\"}";
		byte[] data = Encoding.ASCII.GetBytes(payload);
		string result = SendSync (skirmish, data);
		MatchInfo match = JsonUtility.FromJson<MatchInfo>(result);
		Endpoints.ServerPort = match.port;
		Endpoints.Hash = match.hash;
		return true;
	}

	public bool Training () {
		string payload = "{\"user_name\": \"" + username + "\"}";
		byte[] data = Encoding.ASCII.GetBytes(payload);
		string result = SendSync (training, data);
		MatchInfo match = JsonUtility.FromJson<MatchInfo>(result);
		Endpoints.ServerPort = match.port;
		Endpoints.Hash = match.hash;
		return true;
	}

	private string SendSync (string endpoint, byte[] data) {
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (endpoint);
		request.Method = "POST";
		request.ContentType = "application/json";
		request.ContentLength = data.Length;
		using (var stream = request.GetRequestStream()) {
			stream.Write(data, 0, data.Length);
		}
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
		return responseString;
	}
}
