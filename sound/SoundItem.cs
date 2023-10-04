using System.ComponentModel;
using Microsoft.Xna.Framework.Audio;

namespace GreenTrutle_crossplatform.sound;

public class SoundItem
{
    public float volume
    {
        get { return _volume;}
        set
        {
            _volume = value;
            if(instance==null)
                CreateInstance();
            instance.Volume = _volume;
        }
    }
    public string name;
    public SoundEffect sound;
    public SoundEffectInstance instance;
    private float _volume;

    public SoundItem(string name, string path,float volume)
    {
        this.name = name;
        this.sound = Globals.game.Content.Load<SoundEffect>(path);
        this.volume = volume;
        CreateInstance();
    }

    public virtual void CreateInstance()
    {
        instance = sound.CreateInstance();
        instance.Volume = volume;

    }
}