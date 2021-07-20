using UnityEngine;
using TalofaGames.Utility;
using System.Threading.Tasks;

namespace TalofaGames.Bootstrapping
{
  /// <summary>
  /// Bootstrapper handles authenticating the player and loading them into the world scene
  /// </summary>
  [CreateAssetMenu(menuName = "Controllers/BootstrapperController")]
  public class BootstrapperController : ScriptableSingleton<BootstrapperController>
  {
    public int test;
    public override void OnPlay()
    {
    }

    public async void AuthenticateAndLogin()
    {
      Debug.Log($"[<b>{nameof(BootstrapperController)}</b>] Starting to Authenticate and Login...");
      await Task.Delay(3000); // Fake 3 sec delay
    }
  }
}