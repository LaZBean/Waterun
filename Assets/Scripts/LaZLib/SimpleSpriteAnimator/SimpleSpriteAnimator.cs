using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class SpriteAnimationSettings
{
    public float lifetime = 1.0f;

    public bool loop = true;
    public bool playOnAwake = true;
    public bool clearSpriteOnEnd = false;
    public SimpleSpriteAnimator.Mode mode = SimpleSpriteAnimator.Mode.SingleRow;

    [Header("[Grid Mode]")]
    public int rowLength = 8;
    public SimpleSpriteAnimator.GridOrienation gridOrientation = SimpleSpriteAnimator.GridOrienation.X;

    public int[] rowPattern = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
}



//[ExecuteInEditMode]
public class SimpleSpriteAnimator : MonoBehaviour {

    [SerializeField] Sprite[] _frames;

    public SpriteAnimationSettings settings;

    public SpriteAnimationPreset[] presets;
    public string presetID;




    [HideInInspector] public SpriteRenderer renderer;
    [HideInInspector] public Image image;


    public float timer;
    float timeToNextFrame;
    public float timeMultiplier = 1f;

    public bool isPlaying = false;
    public bool isAnimationOver = true;
    public bool stopInLastFrame = true;

    int _x, _y;
    public int _curIndx = 0;

    //X and Y
    public int x
    {
        get { return _x; }
        set {
            if (value != _x) {
                _x = value;
                CalculateFrame();
            }
        }
    }

    public int y
    {
        get { return _y; }
        set {
            if (value != _y) {
                _y = value;
                CalculateFrame();
            }
        }
    }


    //=====
    public int curIndx
    {
        get { return _curIndx; }
        set {
            if (_curIndx != value)
            {
                if(isPlaying)
                    isAnimationOver = false;

                if (_curIndx == frames.Length - 2)
                {
                    OnFrameLast();
                }

                if(_curIndx >= frames.Length - 1)
                {
                    //OnAnimationOver();
                }

                _curIndx = value;
                OnFrameChanged();
            }

        }
    }


    //=================

    public enum Mode {
        SingleRow = 0,
        Grid = 1
    }

    public enum GridOrienation {
        X = 0,
        Y = 1
    }


    public Sprite[] frames {
        get { return _frames; }
        set {
            if (frames != value) {
                //timer = 0;
            }
            _frames = value;
        }
    }


    public float frameTime
    {
        get { return settings.lifetime / (1.0f * frames.Length); }
        set { settings.lifetime = frames.Length * value; }
    }


    //=================

    void Awake() {
        Init();

        if(settings.playOnAwake)
            isPlaying = true;
    }

    public void Init()
    {
        renderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
    }
	

	void LateUpdate () {

        if (!Application.isPlaying) return;

        if (!isPlaying || frames.Length == 0)
        {
            if (settings.clearSpriteOnEnd)
            {
                if (renderer != null)
                    renderer.sprite = null;
                else if (image != null)
                    image.sprite = null;
            }
            return;
        }

        Tick(Time.deltaTime);
    }



    //
    public void SetPreset(string animID, bool saveTime=false)
    {
        if (presetID == animID) return;

        foreach (var p in presets)
        {
            if(p.animationID == animID)
            {
                frames = p.sprites;

                settings = p.settings;

                presetID = animID;

                if (!saveTime)
                    timer = 0;
                break;
            }
        }
    }


    //================
    public void Tick(float delta)
    {
        //Timer
        float step = delta * timeMultiplier / (settings.lifetime);
        if((timer + step) >= 1f)
        {
            if (!settings.loop)
                isPlaying = false;
            OnAnimationOver();
        }

        if (!isPlaying && stopInLastFrame)
        {
            //WTF
            CalculateFrame();
        }
        else
        {
            timer = (timer + step) % 1;
            if (timer < 0)
                timer = 1 + timer;

            //Calculate Frame
            CalculateFrame();
        }
        
    }

    void CalculateFrame()
    {
        if (settings.mode == Mode.SingleRow)
        {
            x = Mathf.FloorToInt((timer / 1f) * frames.Length);
            curIndx = x;
        }
        else if (settings.mode == Mode.Grid)
        {
            x = Mathf.FloorToInt((timer / 1f) * settings.rowLength);
            curIndx = y * settings.rowLength + x;
        }
    }


    //Events
    void OnFrameChanged()
    {
        //Render
        if (curIndx >= 0 && curIndx < frames.Length)
        {
            UpdateRenderer(frames[curIndx]);
        }
        else
        {
            UpdateRenderer(null);
        }
    }

    void OnFrameLast()
    {

    }

    void OnAnimationOver()
    {
        isAnimationOver = true;
    }


    //Update Renderer
    void UpdateRenderer(Sprite s)
    {
        if (renderer != null)
            renderer.sprite = s;
        else if (image != null)
            image.sprite = s;
    }



    

    //=================




    public void Emit(){
        settings.loop = false;
        timer = 0;
        isPlaying = true;
	}

    public void Play()
    {
        isPlaying = true;
    }

    public void Pause()
    {
        isPlaying = false;
    }

    public void Stop(){
		isPlaying = false;
        timer = 0;
        CalculateFrame();
	}
}
