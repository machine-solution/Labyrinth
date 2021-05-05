using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    Slider slider_n, slider_k, slider_t, slider_volume;
    Toggle toggle_f_tp;

    public void SetSliders()
    {
        slider_n.SetVal(Base.main.n);
        slider_k.SetVal(Base.main.k);
        slider_t.SetVal(Base.main.t);
        slider_volume.SetVal(Base.main.volume);
        toggle_f_tp.Set(Base.main.f_tp, false);
    }

    public void SaveSettings()
    {
        Base.main.SaveGameSettings(slider_n.val, slider_k.val, slider_t.val, toggle_f_tp.condition);
        Base.main.SetGameSettings(slider_n.GetVal(), slider_k.GetVal(), slider_t.GetVal(), toggle_f_tp.condition);
    }

    [System.Obsolete]
    public void Back()
    {
        if (Base.main.lastScene != "")
        {
            Base.main.currentScene = Base.main.lastScene;
            Base.main.OnScene(Base.main.lastScene);
            Base.main.lastScene = "";
        }
    }

    [System.Obsolete]
    void Start()
    {
        slider_n = GameObject.Find("slider_sol").GetComponent<Slider>();
        slider_k = GameObject.Find("slider_nop").GetComponent<Slider>();
        slider_t = GameObject.Find("slider_not").GetComponent<Slider>();
        slider_volume = GameObject.Find("slider_volume").GetComponent<Slider>();
        toggle_f_tp = GameObject.Find("toggle_teleport").GetComponent<Toggle>();
        GameObject.Find("SaveBut").GetComponent<Button>().click = SaveSettings;
        GameObject.Find("MenuBut").GetComponent<Button>().click = Base.main.OnScene_Menu;
        GameObject.Find("BackBut").GetComponent<Button>().click = Back;

        Base.main.LoadGameSettings();
        Base.main.LoadSoundSettings();
        SetSliders();
        SaveSettings();
    }


    void Update()
    {
        Base.main.SaveSoundSettings(slider_volume.val);
    }
}
