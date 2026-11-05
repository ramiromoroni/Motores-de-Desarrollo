using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyVoiceData", menuName = "Audio/Enemy Voice Data")]
public class StoredLines : ScriptableObject
{
    [field: Header("Alertas (Variaciones Aleatorias)")]
    [field: SerializeField] public List<AudioClip> Alertas { get; private set; } = new List<AudioClip>();

    [field: Header("Cámara de Seguridad")]
    [field: SerializeField] public AudioClip CamaraDetectoAlguien { get; private set; }
    [field: SerializeField] public List<AudioClip> CamaraNoHayNadie { get; private set; } = new List<AudioClip>();

    [field: Header("Evasión")]
    [field: SerializeField] public AudioClip JugadorEscapo { get; private set; }
}
