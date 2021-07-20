﻿using TalofaGames.UI.Attributes;
using UnityEngine;

namespace TalofaGames.UI.PanelSystem
{
  public class PanelViewControllerRegisterer : MonoBehaviour
  {
    [SerializeField, Help("This component registers a panel controller with a panel controller provider. The panel provider is used to map PanelType to PanelViewController.")]
    private PanelViewControllerProvider provider;
    [SerializeField]
    private RegisterEvent regsiterOnEvent = RegisterEvent.Start;
    private IPanelViewController controller;

    public enum RegisterEvent
    {
      Awake,
      Start
    }

    private void Awake()
    {
      if (regsiterOnEvent == RegisterEvent.Awake)
      {
        Register();
      }
    }

    private void Start()
    {
      if (regsiterOnEvent == RegisterEvent.Start)
      {
        Register();
      }
    }

    private void Register()
    {
      controller = GetComponent<IPanelViewController>();
      if (controller != null)
      {
        provider.Add(controller);
      }
    }

    private void OnDestroy()
    {
      if (controller != null)
      {
        provider.Remove(controller);
      }
    }
  }
}
