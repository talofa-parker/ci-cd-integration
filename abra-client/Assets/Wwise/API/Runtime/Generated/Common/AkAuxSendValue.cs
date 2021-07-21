#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.

//------------------------------------------------------------------------------

// <auto-generated />

//

// This file was automatically generated by SWIG (http://www.swig.org).

// Version 3.0.12

//

// Do not make changes to this file unless you know what you are doing--modify

// the SWIG interface file instead.

//------------------------------------------------------------------------------





public class AkAuxSendValue : global::System.IDisposable {

  private global::System.IntPtr swigCPtr;

  protected bool swigCMemOwn;



  internal AkAuxSendValue(global::System.IntPtr cPtr, bool cMemoryOwn) {

    swigCMemOwn = cMemoryOwn;

    swigCPtr = cPtr;

  }



  internal static global::System.IntPtr getCPtr(AkAuxSendValue obj) {

    return (obj == null) ? global::System.IntPtr.Zero : obj.swigCPtr;

  }



  internal virtual void setCPtr(global::System.IntPtr cPtr) {

    Dispose();

    swigCPtr = cPtr;

  }



  ~AkAuxSendValue() {

    Dispose();

  }



  public virtual void Dispose() {

    lock(this) {

      if (swigCPtr != global::System.IntPtr.Zero) {

        if (swigCMemOwn) {

          swigCMemOwn = false;

          AkSoundEnginePINVOKE.CSharp_delete_AkAuxSendValue(swigCPtr);

        }

        swigCPtr = global::System.IntPtr.Zero;

      }

      global::System.GC.SuppressFinalize(this);

    }

  }



  public ulong listenerID { set { AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_listenerID_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_listenerID_get(swigCPtr); } 

  }



  public uint auxBusID { set { AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_auxBusID_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_auxBusID_get(swigCPtr); } 

  }



  public float fControlValue { set { AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_fControlValue_set(swigCPtr, value); }  get { return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_fControlValue_get(swigCPtr); } 

  }



  public void Set(ulong listener, uint id, float value) { AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_Set(swigCPtr, listener, id, value); }



  public bool IsSame(ulong listener, uint id) { return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_IsSame(swigCPtr, listener, id); }



  public void Set(UnityEngine.GameObject listener, uint id, float value)

  {

    var listener_id = AkSoundEngine.GetAkGameObjectID(listener);

    AkSoundEngine.PreGameObjectAPICall(listener, listener_id);



    Set(listener_id, id, value);

  }



  public bool IsSame(UnityEngine.GameObject listener, uint id)

  {

    var listener_id = AkSoundEngine.GetAkGameObjectID(listener);

    AkSoundEngine.PreGameObjectAPICall(listener, listener_id);



    return IsSame(listener_id, id);

  }



  public static int GetSizeOf() { return AkSoundEnginePINVOKE.CSharp_AkAuxSendValue_GetSizeOf(); }



}

#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_WIIU || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
