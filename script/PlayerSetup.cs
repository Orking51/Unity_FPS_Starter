using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    Camera sceneCamera;

    private void Start()
    {
        // si ce n est n est pas joueur alors...
       if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
       else 
        { 
            sceneCamera = Camera.main;
            if(sceneCamera != null )
            {
                //desactive la camera de base quand on lance 
                sceneCamera.gameObject.SetActive( false );
            }
        }

        GetComponent<Player>().Setup(); // appelle Setup de GetComponent

    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPLayer(netId, player);  
    }

    private void DisableComponents()
    {
        //on desactive les parametres qui ne sont pas celui de notre joueur avec une boucle pour evité d interagir avec les autres persos depuis 1 seul setup 
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void OnDisable() //quand un joueur quitte la scene
    {
        if(sceneCamera != null )
        {
            sceneCamera.gameObject.SetActive( true ); // reactive la cam de la scene
        }

        GameManager.UnregisterPlayer(transform.name);
    }


}
