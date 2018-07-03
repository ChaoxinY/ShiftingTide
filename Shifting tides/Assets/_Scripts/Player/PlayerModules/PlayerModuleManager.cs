using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerModuleManager : MonoBehaviour
{
    public List<PlayerModule> AvailableModules = new List<PlayerModule>();
    private List<PlayerModule> ActiveModules = new List<PlayerModule>();

    private void Start()
    {
        ActiveModules.Add(AvailableModules[0]);
        ActiveModules.Add(AvailableModules[1]);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            foreach (PlayerModule module in ActiveModules)
                if (module == AvailableModules[2])
                {
                    module.ModuleRemove();
                    ActiveModules.Remove(module);
                    return;
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

public class PlayerModule : MonoBehaviour
{

    public virtual void ModuleUpdate() { }
    public virtual void ModuleFixedUpdate() { }
    public virtual void ModuleStartUp() { }
    public virtual void ModuleRemove() { }
}