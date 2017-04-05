using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HDNetworkManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }


    


    private const string typeName = "UniqueGameName";
    private const string gameName = "RoomName";

    private void StartServer()
    {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(10, 10, 75, 25), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(10, 50, 75, 25), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(200, 25 + (25 * i), 150, 20), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }
    


    private HostData[] hostList;

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }



    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);

        print("Server has joined to room.");

        LoadingSceneManager.LoadLevel("01_Intro_2p_network");
    }

    void OnConnectedToServer()
    {
        Debug.Log("Client has connected to server.");

        LoadingSceneManager.LoadLevel("01_Intro_2p_network");
    }



}
