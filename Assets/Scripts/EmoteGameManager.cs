using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteGameManager : MonoBehaviour
{
    public List<GameObject> emotes = new List<GameObject>();
    public GameObject[] EmotePos;
    public GameObject EmoteBoard;
    public WebSocketClient webSocketClient;
    public float emoteTimer = 10f;

    GameObject[] currentEmotes = new GameObject[3];
    void Start()
    {
        SubscribeToSocketMessage();
        StartCoroutine(SummonEmotes());

    }

    public void SubscribeToSocketMessage()
    {
        if (WebSocketClient.instance != null)
        {
            WebSocketClient.instance.SubscribeToReceiver(message =>
            {
                string emoteSeq = "";
                foreach (GameObject pog in currentEmotes)
                {
                    emoteSeq += pog.tag + " ";
                }
                if (message == emoteSeq.Trim())
                {
                    WebSocketClient.instance.SendWebSocketMessage("Hotdog");
                }
                Debug.Log(message);
            });
        }
    }

    IEnumerator SummonEmotes()
    {
        EmoteBoard.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            int randomNumber = Random.Range(0, emotes.Count);
            Destroy(currentEmotes[i]);
            var go = Instantiate(emotes[randomNumber], transform.position, Quaternion.identity) as GameObject;
            go.transform.parent = EmoteBoard.transform;
            go.transform.position = EmotePos[i].transform.position;
            currentEmotes[i] = go;
        }
        yield return new WaitForSeconds(emoteTimer);
        EmoteBoard.SetActive(false);
        EmoteBoard.transform.position = new Vector3(Random.Range(-4.60f, 4.76f), Random.Range(-3f, 3f), 0);
        yield return new WaitForSeconds(emoteTimer);
        StartCoroutine(SummonEmotes());
    }


    public void hehexd()
    {
        EmoteBoard.SetActive(false);
        //StartCoroutine(SummonEmotes());
    }

}
