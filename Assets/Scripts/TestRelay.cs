using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour
{

    [SerializeField] private Text userCode;
    [SerializeField] private NetworkPrefabsList networkPrefabsList;

    private NetworkPrefab gameInputPrefab;
    [SerializeField] private GameInput gameInput;


    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        CreateRelay();

        gameInputPrefab = networkPrefabsList.PrefabList[1];
        GameObject gameInputGameObject = Instantiate(gameInputPrefab.Prefab);
        gameInputGameObject.GetComponent<NetworkObject>().Spawn(true);
        gameInput = gameInputGameObject.GetComponent<GameInput>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            JoinRelay(userCode.text);
        }
    }
    private async void CreateRelay ()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        }catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
        
    }
    private async void JoinRelay (string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    public GameInput GetGameInput()
    {
        return gameInput;
    }
}
