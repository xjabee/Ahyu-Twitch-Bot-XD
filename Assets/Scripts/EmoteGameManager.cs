using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteGameManager : MonoBehaviour
{
    public List<GameObject> emotes = new List<GameObject>();
    public GameObject[] EmotePos;
    public GameObject EmoteBoard;
    public WebSocketClient webSocketClient;

    GameObject[] currentEmotes = new GameObject[3];
    void Start()
    {
        SubscribeToSocketMessage();
        SummonEmotes();
        //StartCoroutine(SummonEmotes());

    }

    public void SubscribeToSocketMessage()
    {
        if(WebSocketClient.instance != null)
        {
            WebSocketClient.instance.SubscribeToReceiver(message => {
                Debug.Log(message);
            });
        }
    }

    public void SummonEmotes()
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
        //yield return new WaitForSeconds(30f);

        //EmoteBoard.SetActive(false);
    }

    void RandomizeEmote()
    {
        int randomNumber = Random.Range(0, emotes.Count);

    }

    public void hehexd()
    {
        EmoteBoard.SetActive(false);
        //StartCoroutine(SummonEmotes());
    }

}
