using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;

public delegate void MessageReceiver(string message);

public class WebSocketClient : MonoBehaviour
{
    WebSocket websocket;

    public static WebSocketClient instance;
    private List<MessageReceiver> receivers;

    public int SubscribeToReceiver(MessageReceiver receiver)
    {
        this.receivers.Add(receiver);
        return this.receivers.Count - 1;
    }

    public void UnsubscribeReceiver(int index)
    {
        this.receivers.RemoveAt(index);
    }

    void Awake()
    {
        instance = this;
        this.receivers = new List<MessageReceiver>();
    }

    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket("ws://7fe0597d5603.ngrok.io");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += async (bytes) =>
        {
            //Debug.Log("OnMessage!");
            //Debug.Log(bytes);

            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            foreach (MessageReceiver receiver in this.receivers)
            {
                receiver(message);
            }
            Debug.Log("OnMessage! " + message);
        };

        // Keep sending messages at every 0.3s
        //InvokeRepeating("Hotdog penge", 0.0f, 0.3f);
        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    public async void SendWebSocketMessage(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(message);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}