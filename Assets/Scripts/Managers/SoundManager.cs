
using System.Collections;
using UnityEngine;

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;


    public AudioSource MusicSource { get { return musicSource; } }

    public void PlayMusic(AudioClip clip,float volume)
    {
        musicSource.clip = clip;

        musicSource.volume = volume;

        musicSource.Play();
    }
    public void StopMusic() => musicSource.Stop();
    public void PauseMusic()
    {
        if (musicSource.clip != null)
            musicSource.Pause();
    }

    public void UnPauseMusic()
    {
        if (musicSource.clip != null)
            musicSource.UnPause();
    }

    public void StopMusicSmoothly(float duration)
        =>  StartCoroutine(StopMusicSmoothlyRoutine(duration));

    public void PlaySFX(AudioClip clip, float volume) => sfxSource.PlayOneShot(clip, volume);

    public void PauseSFX()
    {
        if (sfxSource.clip != null)
            sfxSource.Pause();
    }

    public void UnPauseSFX()
    {
        if(sfxSource.clip != null)
            sfxSource.UnPause();
    }


    private IEnumerator StopMusicSmoothlyRoutine(float duration)
    {
        float startPoint = musicSource.volume;

        float time = 0f;

        while(time <= duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            float normalizedVolume = Mathf.Lerp(startPoint, 0, t);

            musicSource.volume = normalizedVolume;

            yield return null;

        }

        musicSource.volume = 0f;

        StopMusic();
    }


}