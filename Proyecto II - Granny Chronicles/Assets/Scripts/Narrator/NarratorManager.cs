using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class NarratorManager : MonoBehaviour
{
    public static NarratorManager Instance { get; private set; }

    [Tooltip("The amount of time the narrator waits between phrases")][SerializeField]
    private float waitTime = 1.0f;

    private Queue<string> sentences = new Queue<string>();
    private Queue<AudioClip> clips = new Queue<AudioClip>();
    private Queue<NarratorEvent> events = new Queue<NarratorEvent>();

    public string[] sentencesTest = new string[] { };

    [SerializeField] private TMP_Text textDisplay;
    private AudioSource sound;

    private void Awake()
    {
        //Singleton
        /*if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);*/
        //
    }
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        sentencesTest = sentences.ToArray();
        if (Input.GetKeyDown(KeyCode.M))
        {
            CutComment();
        }
    }
    public void StartComment(NarratorComment _comment)
    {
        sentences.Clear();
        clips.Clear();
        events.Clear();
        StopAllCoroutines();

        foreach(string sentence in _comment.sentences) 
        {
            sentences.Enqueue(sentence);
        }
        foreach(AudioClip audioClip in _comment.clips)
        {
            clips.Enqueue(audioClip);
        }
        if(_comment.events.Length != 0)
        {
            foreach (NarratorEvent narratorEvent in _comment.events)
            {
                if (narratorEvent.eventName == "")
                {
                    narratorEvent.active = false;
                }
                events.Enqueue(narratorEvent);
            }
        }

        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if(sentences.Count == 0) 
        {
            EndComment();
            return;
        }

        string currentSentence = sentences.Dequeue();
        AudioClip currentClip = clips.Dequeue();
        NarratorEvent nEvent = events.Dequeue();

        if (nEvent.active)
        {
            if(nEvent.eventReceiver == null)
            {
                nEvent.eventReceiver = FindObjectOfType<Player>();
            }
            nEvent.eventReceiver.Invoke(nEvent.eventName, nEvent.timeOffset);
        }
        

        if (currentClip != null)
        {
            PlayVoice(currentClip);
        }
        //Por si se quiere que las letras salgan todas a la vez
        textDisplay.text = currentSentence;

        // Por si se quiere que las letras salgan de una en una
        //StopAllCoroutines();
        //StartCoroutine(TypeSentence(currentSentence));

        StartCoroutine(WaitForNextSentence());
    }

    void PlayVoice(AudioClip _clip)
    {
        sound.clip = _clip;
        sound.Play();
    }

    void StopVoice()
    {
        sound.Stop();
    }

    IEnumerator WaitForNextSentence()
    {
        do
        {
            yield return null;
        } while (sound.isPlaying);
        yield return new WaitForSeconds(waitTime);
        DisplayNextSentence();
    }
    /*
    IEnumerator TypeSentence(string _sentence)
    {
        textDisplay.text = "";
        foreach(char letter in _sentence.ToCharArray())
        {
            textDisplay.text += letter;
            for(int i = 0; i < letterWaitFrames; i++)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(sentenceWaitTime);
        DisplayNextSentence();
    }*/
    void EndComment()
    {
        textDisplay.text = "";
    }

    public void CutComment()
    {
        sentences.Clear();
        clips.Clear();
        events.Clear();
        StopAllCoroutines();
        textDisplay.text = "";
        StopVoice();
    }
}
