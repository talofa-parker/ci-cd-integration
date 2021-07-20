namespace TalofaGames.Utility
{
  using System.Linq;
  using UnityEditor;
  using UnityEngine;

  /// <summary>
  /// Abstract class for making reload-proof singletons out of ScriptableObjects
  /// Returns the asset created on editor, null if there is none
  /// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
  /// </summary>
  /// <typeparam name="T">Type of the singleton</typeparam>
  public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
  {
    static T instance = null;
    public static T Instance
    {
      get
      {
        if (instance != null)
          return instance;

#if UNITY_EDITOR
        //In editor, the player settings preloaded assets work differently, mainly they aren't loaded...
        var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
        foreach (Object asset in preloadedAssets)
        {
          if (asset.GetType() == typeof(T))
          {
            instance = (T)asset;
            break;
          }
        }
#else
        instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
#endif
        return instance;
      }
    }

    public abstract void OnPlay();

    private void OnEnable()
    {
#if UNITY_EDITOR
      EditorApplication.playModeStateChanged += onPlayModeStateChanged;
#else
      OnPlay();
#endif
    }

#if UNITY_EDITOR
    private void onPlayModeStateChanged(PlayModeStateChange state)
    {
      if (state == PlayModeStateChange.EnteredPlayMode)
        OnPlay();
    }
#endif

    private void OnValidate()
    {
      //If we are in the editor, and create this singleton for the first time, 
      //confirm if its in the playersettings as a preloaded asset
#if UNITY_EDITOR
      bool hasTypeSaved = false;
      var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
      foreach (Object asset in preloadedAssets)
      {
        if (asset == null)
          continue;

        hasTypeSaved = (asset.GetType() == typeof(T));
        if (hasTypeSaved) //Stop looping if we have an asset of this type in the player settings
          break;
      }

      //Yikes, lets add this singleton to the player settings for them
      if (hasTypeSaved == false)
      {
        // Add the config asset to the build
        preloadedAssets.Add(this);
        PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        Debug.Log($"<b>Auto added {this} asset to the player settings and saved to disk</b>");
        AssetDatabase.SaveAssets();
      }
#endif
    }
  }
}