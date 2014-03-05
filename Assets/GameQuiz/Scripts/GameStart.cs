using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
    GameObject gameManager;
	void Start () {
        gameManager = GameObject.Find("GameManager");
	}

    void OnClick()
    {
        gameManager.SendMessage("StartGame", SendMessageOptions.DontRequireReceiver);
        Destroy(this);
    }
	
	void Update () {
	
	}
}
