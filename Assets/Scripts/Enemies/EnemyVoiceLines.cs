using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyVoiceEmitter : MonoBehaviour
{
    //Almacen de lineas de voz
    [SerializeField] private StoredLines voiceData;

    private AudioSource _audioSource;

    [SerializeField] private AudioSource voiceSource;

    private void Awake()
    {
        voiceSource.playOnAwake = false;
        voiceSource.spatialBlend = 1.0f;
    }

    //Metodos que va a llamar el enemigo
    public void PlayAlerta() => PlayRandomClip(voiceData?.Alertas);
    public void PlayCamaraDetecto() => PlaySingleClip(voiceData?.CamaraDetectoAlguien);
    public void PlayCamaraFalsaAlarma() => PlayRandomClip(voiceData?.CamaraNoHayNadie);
    public void PlayJugadorEscapo() => PlaySingleClip(voiceData?.JugadorEscapo);

    private void PlaySingleClip(AudioClip clip)
    {
        if (clip == null) return;

        // Evita repetir una voz mientras todavía está sonando.
        if (voiceSource.isPlaying) return;

        voiceSource.PlayOneShot(clip);
    }

    private void PlayRandomClip(List<AudioClip> clips)
    {
        if (clips == null || clips.Count == 0) return;

        //Elige un índice al azar entre las variaciones disponibles (para cuando no detecta a nadie en la camara o ve a un enemigo)
        int randomIndex = Random.Range(0, clips.Count);
        PlaySingleClip(clips[randomIndex]);
    }
}
