using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("组件")]
    public AudioSource audioSourceBGM;
    public AudioSource audioSourceSFX;
    [Header("音乐片段")]
    public AudioClip audioClipBGM;
    public AudioClip audioClipSFXHover;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioSourceBGM.loop = true;
        audioSourceSFX.loop = false;
    }
    /// <summary>
    /// 点击房间时调用函数,在不同房间播放不同BGM
    /// </summary>
    /// <param name="data"></param>
    public void LoadRoomEvent(object data)
    {
        Room currentRoom = (Room)data;
        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                
                StartCoroutine(DelayAudioPlay(audioClipBGM));
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.ResetRoom:
                
                break;
        }
    }

    IEnumerator DelayAudioPlay(AudioClip clip)
    {
        
        float startVolume = audioSourceBGM.volume;
        for (float t = 0; t < 2; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(startVolume, 0, t / 1);
            yield return null;
        }
        
        audioSourceBGM.clip = clip;
        audioSourceBGM.Play();
        for (float t = 0; t < 2; t += Time.deltaTime)
        {
            audioSourceBGM.volume = Mathf.Lerp(0, startVolume, t / 1);
            yield return null;
        }
    }
/// <summary>
/// 划入按钮播放的音效
/// </summary>
    public void ButtonHover()
    {
        audioSourceSFX.clip = audioClipSFXHover;
        audioSourceSFX.Play();

    }
}
