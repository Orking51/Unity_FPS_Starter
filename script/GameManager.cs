using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player"; // prefix ajouté au netId pour cree le playerId

    private  static Dictionary<string, Player> players = new Dictionary<string, Player> (); // dictionnaire repertoriant les id des joueurs

    public MatchSettings matchSettings;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            return;
        }

        Debug.LogError("Plus d'une instance de GameManager dans la scène, impossible en temps normale car GameManager.cs est un singleton");
    }
    public static void RegisterPLayer(string netID, Player player) // ajoute les joueurs dans un dico avec un id diff pour tous
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.transform.name = playerId; // permet qque le joueur dans l inspecteur est le meme nom que dans le dico

    }
    public static void UnregisterPlayer(string playerId) // deco d'un joueur
    {
        players.Remove(playerId); // l'enleve du dico
    }

    public static Player GetPlayer(string playerId) // retourne l'emblacement dans le dico d'un playerId
    {
        return players[playerId];
    }
}
