using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerModuleManager : MonoBehaviour
{
    public List<PlayerModule> AvailableModules = new List<PlayerModule>();
    public List<PlayerModule> ActiveModules = new List<PlayerModule>();

    private bool isAiming = false;
  

    private void Start()
    {
        ActiveModules.Add(AvailableModules[0]);     
        ActiveModules.Add(AvailableModules[1]);
        ActiveModules.Add(AvailableModules[2]);
        ActiveModules.Add(AvailableModules[3]);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //foreach (PlayerModule module in ActiveModules)
            //{
            //    if (module.ModuleID == AvailableModules[2].ModuleID)
            //    {
            //        module.ModuleRemove();
            //        //ActiveModules.Remove(module);
            //        return;
            //    }
            //}
            if (isAiming == false)
            {            
                ActiveModules[2].ModuleStartUp();
                isAiming = true;
                return;
            }
            if (isAiming) {
                foreach (PlayerModule module in ActiveModules)
                {
                    if (module.ModuleID == AvailableModules[2].ModuleID)
                    {
                        module.ModuleRemove();
                        isAiming = false;
                        return;
                    }
                }               
            }
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