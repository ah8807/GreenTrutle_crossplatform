using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;

namespace GreenTrutle_crossplatform.sound;

public class SoundControl
{
    public float musicV
    {
        get { return _musicV; }
        set
        {
            _musicV = value; 
            adjustMusicVolume(value);
        }
    }
    public float SFXV{
        get { return _SFXV; }
        set
        {
            _SFXV = value; 
            adjustSFXVolume(value);
        }
    }

    public float _SFXV { get; set; }

    private SoundItem soundItem;
    Dictionary<string,SoundItem> soundItems = new Dictionary<string,SoundItem>();
    private float _musicV;

    public SoundControl(string? path)
    {
        
        if (path != null)
        {
            ChangeMusic(path);
        }
        initialize();
    }

    public void initialize()
    {
        addSound("pickUpLettuce","pickupCoin",1);
        XDocument xml = Globals.save.GetFile("settings.xml");
        if (xml == null)
            return;
        List<XElement> objList = (from t in xml.Element("Root").Element("Settings").Descendants("Setting")
            where t.Element("Type").Value == "Slider"
            select t).ToList<XElement>();
        
        foreach (var obj in objList)
        {
            string name = Convert.ToString(obj.Element("Name").Value);
            float value = (Convert.ToSingle(obj.Element("Value").Value));
            switch (name)
            {
                case "Music volume":
                    musicV = value/100f;
                    break;
                case "SFX volume":
                    SFXV = value/100f;
                    break;
            }
        }
    }

    public void addSound(string name, string path, float volume)
    {
        SoundItem sound = new SoundItem(name, path, _SFXV);
        if (!soundItems.TryAdd(name, sound))
            soundItems[name] = sound;
    }

    public void playSound(string name)
    {
        SoundItem? sound;
        soundItems.TryGetValue(name,out sound);
        if (sound != null)
        {
            sound.CreateInstance();
            runSound(sound.sound,sound.instance,sound.volume);
        }

    }
    public void playSoundOnce(string name)
    {
        SoundItem? sound;
        soundItems.TryGetValue(name,out sound);
        if (sound != null)
        {
            runSound(sound.sound,sound.instance,sound.volume);
        }

    }

    private void runSound(SoundEffect sound,SoundEffectInstance instance, float volume)
    {
        instance.Volume = volume;
        instance.Play();
    }
    private void ChangeMusic(string path)
    { 
        soundItem = new SoundItem("music", path, 1f);
        soundItem.CreateInstance();
        if(soundItem.volume!=null)
            adjustMusicVolume(soundItem.volume);
        else
        {
            adjustMusicVolume(1);
        }
        soundItem.instance.Play();
        soundItem.instance.IsLooped = true;
    }

    public virtual void adjustMusicVolume(float percent)
    {
        if (soundItem.instance != null)
        {
            soundItem.volume = percent;
        }
    }
    public virtual void adjustSFXVolume(float percent)
    {
        foreach (var effect in soundItems)
        {
            if (soundItem.instance != null)
            {
                effect.Value.volume = percent;
            }
        }
        playSoundOnce("pickUpLettuce");
    }
    
}