using UnityEngine;
using Mirror;
using Unity.Collections;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead // ca s apelle une proprieté ou preperty en anglais
    {
        get { return _isDead; } //permet d obtenir des info sur _isDead depuis n importe ou
        protected set { _isDead = value; } //permet de d appliquer value a _isDead depuis n importe ou
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]   // permet de syncro avec toutes les instances / tous les clients / tous les joueurs
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath; //aray de type Behaviour / si x element dans disableOnDeath alors x vcaleur dans wasEnableOnStart
    private bool[] wasEnableOnStart; //array de type bool

    public void Setup() // prepare le spawn
    {
        wasEnableOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++) //stock tous les status des elements de disableOnDeath et les stocks dans wasEnableOnStart 
        {
            wasEnableOnStart[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    public void SetDefaults() //spawn
    {
        isDead = false;
        currentHealth = maxHealth; //vie actuel = vie max au demarage

        for (int i = 0; i < disableOnDeath.Length; i++) // permet de reset les elements de disableOnDeath au spawn comme si il venait de rejoindre le serve
        {
            disableOnDeath[i].enabled = wasEnableOnStart[i];
        }

        Collider col = GetComponent<Collider>(); // le system si dessus marcha avec 1 seul collider sur la prefabe si plusieur collideur faire pareil que avec un Behaviour[]  mais en appellant ca un Collider[]
        if (col != null) //permet de reactiver le collider parce que c un ptit pd et qu il veut pas aller avec les autre dans la array Behaviour
                         // et != null pour verifier si la prefabe possede un collider desactiver ou non
        {
            col.enabled = true;
        }
    }

    private IEnumerator Respawn() // coroutine du respawn
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);// accede depuis le singletone a MatchSettings.cs et recupere la val RespawnTimer
        SetDefaults(); // appelle setdefaults() 
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition(); //recupere les spawnpoints
        transform.position = spawnPoint.position; // replace le joueur sur le spawnPoint 
        transform.rotation = spawnPoint.rotation; // et les fait regarder dans la direction prevu par le spawnPoint
    }

    private void Update() //temporaire pour s infliger des degat a soit meme sans build
    {
        if (!isLocalPlayer) { return; }
        if (Input.GetKeyDown(KeyCode.K)) {RpcTakeDamage(100);}
    }

    [ClientRpc] // le serv dit a toutes les instance [Client] de lire le script
    public void RpcTakeDamage(float amount) 
    {
        if (isDead) // sert a r d infliger des degat a un mort a par a utilisé des perfs
        {
            return;
        }

        currentHealth -= amount; // enleve les pv (currentHealth) au joueur en fonction des degats (amount)
        Debug.Log(transform.name + " a maintenant : " + currentHealth + " points de vies.");

        if (currentHealth <= 0) //si pv iferieur ou = 0 alors appelle void Die() 
        {
            Die();  
        }
    }

    private void Die() // void de setting de mort du joueur 
    {
        isDead = true;

        for (int i = 0;i < disableOnDeath.Length; i++) // desactive les elements de disableOnDeath
        {
            disableOnDeath[i].enabled = false;
        }

        Collider col = GetComponent<Collider>(); // le system si dessus marcha avec 1 seul collider sur la prefabe si plusieur collideur faire pareil que avec un Behaviour[]  mais en appellant ca un Collider[]
        if (col != null) //permet de desactiver le collider parce que c un ptit pd et qu il veut pas aller avec les autre dans la array Behaviour
                         // et != null pour verifier si la prefabe possede un collider desactiver ou non
        {
            col.enabled = false;
        }

        Debug.Log(transform.name + " a été éliminé");

        StartCoroutine(Respawn()); // appelle la coroutine (ou IEnumerator) Respawn
    }
}
