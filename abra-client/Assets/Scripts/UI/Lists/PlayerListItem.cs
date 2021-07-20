using TalofaGames.UI.Pooling;
using UnityEngine;
using UnityEngine.UI;

namespace TalofaGames.UI.Lists
{
  public class PlayerListItem : PoolableGameObject
  {
    private string playerName;
    public string PlayerName
    {
      get { return playerName; }
      set
      {
        playerName = value;
        if (playerNameValue != null)
          playerNameValue.text = playerName;
      }
    }

    public bool IsMe
    {
      get
      {
        throw new System.NotImplementedException();
        // return ServiceLocator.Get<PlayerController>().ID == PlayerId;
      }
    }

    public string PlayerId { get; set; }

    public PooledPlayerListHandler ListHandler { get; set; }

    [SerializeField]
    protected Text playerNameValue;
  }
}