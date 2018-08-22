using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerModuleManager : MonoBehaviour
{
    public List<PlayerModule> AvailableModules = new List<PlayerModule>();
    public List<PlayerModule> ActiveModules = new List<PlayerModule>();
  
    private void Start()
    {
        ActiveModules.Add(AvailableModules[0]);     
        ActiveModules.Add(AvailableModules[1]);
        ActiveModules.Add(AvailableModules[2]);
        ActiveModules.Add(AvailableModules[3]);
    }

    private void Update()
    {
        foreach (PlayerModule module in ActiveModules)
        {
            if (module.active)
            {
                module.ModuleUpdate();
            }
        }
    }
    private void FixedUpdate()
    {
        foreach (PlayerModule module in ActiveModules)
        {
            if (module.active)
            {
                module.ModuleFixedUpdate();
            }
        }
    }
}

public abstract class PlayerModule : MonoBehaviour
{
    public bool active;
    private int moduleID;

    private void Start()
    {
        Initialize();
        active = true;
    }
    protected virtual void Initialize() { InitializeModuleID(); }
    public abstract void InitializeModuleID();
    public virtual void ModuleUpdate() { }
    public virtual void ModuleFixedUpdate() { }
    public virtual void ModuleStartUp() { }
    public virtual void ModuleRemove() { }
    public int ModuleID { get { return moduleID; }set { moduleID = value; } }
}