using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon Weapon; // recupere le script de la classe PlaYer weapon
    [SerializeField]
    private LayerMask mask; //repertorier par famille objet touché 

    [SerializeField]
    private Camera cam; // cam utilisé par le raycast (on a donner a cette cam les setting de la cam du joueur donc c la cam du joueur)
    void Start()
    {
        if (cam == null) // verification au start de si la cam est initialisé
        {
            Debug.LogError("Pas de cam renseigner sur le systeme de tir");
            this.enabled = false;
        }
    }

    [Client]
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot() // void du syteme de tire (le raycast)
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Weapon.range, mask)) //recupere des info sur ce qui est touché par le raycast est accepter par le mask le tout filtrer par le mask
        {
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, Weapon.damage); // envoie le nom du perso touché et les degats de son arme fonction CmdPlayerShot qui repertorie les degats infliger entre joueurs
            }
        }
    }

    [Command] // lu par le serv
    private void CmdPlayerShot(string playerId, float damage) // script quand perso touché
    {
        Debug.Log(playerId + " a été touché.");

        Player player =  GameManager.GetPlayer(playerId); // recupère l'emplacement du playerId Touché dans le dico des joueurs
        player.RpcTakeDamage(damage); //inflige des degats player 
    }
}
