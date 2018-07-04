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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (PlayerModule module in ActiveModules)
            {
                if (module.ModuleID == AvailableModules[2].ModuleID)
                {
                    module.ModuleRemove();
                    ActiveModules.Remove(module);
                    return;
                }
            }
            ActiveModules.Add(AvailableModules[2]);
        }
        foreach (PlayerModule module in ActiveModules)
        {
            module.ModuleUpdate();
        }
    }
    private void FixedUpdate()
    {
        foreach (PlayerModule module in ActiveModules)
        {
            module.ModuleFixedUpdate();
        }
    }
}

public abstract class PlayerModule : MonoBehaviour
{
    private int moduleID;
    private void Start()
    {
        Initialize();
    }
    protected virtual void Initialize() { InitializeModuleID(); }
    public abstract void InitializeModuleID();
    public virtual void ModuleUpdate() { }
    public virtual void ModuleFixedUpdate() { }
    public virtual void ModuleStartUp() { }
    public virtual void ModuleRemove() { }
    public int ModuleID { get { return moduleID; }set { moduleID = value; } }
}