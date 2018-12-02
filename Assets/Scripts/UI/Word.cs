using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TriangleSpawner))]
[RequireComponent(typeof(AudioSource))]
public class Word : MonoBehaviour
{
    public Color UntypedColor = Color.gray;

    public Color NegativeColor = Color.red;

    public Color HighlightedColor = Color.green;

    public Color TypedColor = Color.white;

    public TMP_Text UserInputDisplay;

    public AudioClip SuccessSound;

    public AudioClip FailSound;

    public AudioClip CompleteSound;

    private TMP_Text _tmp;

    private int _idx;

    private int _lastWordIdx;

    private string _word;

    private string[] _cached;

    private GameSettings.EDifficulty _cachedDifficulty;

    private AudioSource _audioSource;

    private TriangleSpawner _spawner;
    private void Awake()
    {
        _tmp = GetComponent<TMP_Text>();
        _audioSource = GetComponent<AudioSource>();
        _spawner = GetComponent<TriangleSpawner>();
    }

    private void Start()
    {
        UserInputDisplay.SetText("");
        UserInputDisplay.ForceMeshUpdate();
        GenerateNewWord();
    }

    private void Update()
    {
        if (!GameSession.Instance.CanType)
            return;

        if (!Input.anyKeyDown)
            return;

        if (string.IsNullOrWhiteSpace(Input.inputString))
            return;

        var key = Input.inputString.ToLower().First();
        _audioSource.pitch = Random.Range(0.98f, 1.02f);
        _audioSource.volume = Random.Range(0.95f, 105f);
        // correct key press
        if (key == _word[_idx])
        {
            StopCoroutine(FlashCharacterNegative(_idx));
            UserInputDisplay.SetText(UserInputDisplay.GetParsedText() + Input.inputString.ToUpper().First());
            UserInputDisplay.ForceMeshUpdate();
            // Start new word
            if (_idx >= _word.Length - 2)
            {
                ++_idx;
                MovePointer();
                
                GameSession.Instance.Score += 1;
                GameSession.Instance.FirstCharacter.Add(_word.First());

                GenerateNewWord();
                _audioSource.clip = CompleteSound;
                _audioSource.Play();
            }
            else // go to next character
            {
                ++_idx;
                MovePointer();
                _audioSource.clip = SuccessSound;
                _audioSource.Play();
            }
        }
        else // bad key press
        {
            StartCoroutine(FlashCharacterNegative(_idx));
            _audioSource.clip = FailSound;
            _audioSource.Play();
            GameSession.Instance.BadKeyPenalty();
        }
    }

    /// <summary>
    /// sets the index to the next non vowel and highlights the current index
    /// </summary>
    private void MovePointer()
    {
        while (GameSettings.Instance.Vowels.Contains(_word[_idx]))
        {
            ++_idx;
        }

        HighlightCurrentIndex();
    }

    /// <summary>
    /// Gets the next word and displays it
    /// </summary>
    private void GenerateNewWord()
    {
        _word = GetWord(GameSettings.Instance.Difficulty);
        _idx = 0;
        StartCoroutine(_spawner.SpawnTriangles(_tmp.rectTransform));
        SetupWordDisplay();
    }

    /// <summary>
    /// highlights the current character index. Updates vertex data
    /// </summary>
    private void HighlightCurrentIndex()
    {
        _tmp.ForceMeshUpdate();
        for (var cIdx = 0; cIdx < _tmp.textInfo.characterCount - 1; ++cIdx)
        {
            var cInfo = _tmp.textInfo.characterInfo[cIdx];

            var matIdx = cInfo.materialReferenceIndex;

            var vertColors = _tmp.textInfo.meshInfo[matIdx].colors32;

            var vertIdx = cInfo.vertexIndex;

            Color color;

            if (GameSettings.Instance.Difficulty >= GameSettings.EDifficulty.Insane)
            {
                color = TypedColor;
            }
            else if (cIdx < _idx)
            {
               color = GameSettings.Instance.Vowels.Contains(char.ToLower(cInfo.character)) ? NegativeColor : TypedColor;
            }
            else if (cIdx == _idx && GameSettings.Instance.Difficulty == GameSettings.EDifficulty.Easy)
            {
                color = HighlightedColor;
            }
            else if (GameSettings.Instance.Difficulty == GameSettings.EDifficulty.Hard)
            {
                color = GameSettings.Instance.Vowels.Contains(char.ToLower(cInfo.character)) ? NegativeColor : TypedColor;
            }
            else
            {
                color = UntypedColor;
            }

            for (var i = vertIdx; i <= vertIdx + 3; ++i)
            {
                vertColors[i] = color;
            }
        }

        _tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    /// <summary>
    /// Sets the current word and moves the pointer
    /// </summary>
    private void SetupWordDisplay()
    {
        StartCoroutine(GoToNewWord(_tmp));
    }

    /// <summary>
    /// grabs the next word based on difficulty
    /// </summary>
    private string GetWord(GameSettings.EDifficulty difficulty)
    {
        if (_cachedDifficulty == difficulty && _cached != null && _cached.Any())
        {
            Debug.Log(_cached.Length);
            int i;
            do
            {
                i = UnityEngine.Random.Range(0, _cached.Length - 1);
            } while (i == _lastWordIdx);

            _lastWordIdx = i;
            return _cached[i];
        }

        var file = GameSettings.Instance.GetDifficultyFile(difficulty);
        _cached = file.text.Split('\n');
        _cachedDifficulty = difficulty;

        return GetWord(difficulty);
    }

    /// <summary>
    /// flashes the character at index red 
    /// </summary>
    private IEnumerator FlashCharacterNegative(int cindex)
    {
        var cInfo = _tmp.textInfo.characterInfo[cindex];

        var matIdx = cInfo.materialReferenceIndex;

        var vertColors = _tmp.textInfo.meshInfo[matIdx].colors32;

        var vertIdx = cInfo.vertexIndex;
        for (var x = 1; x <= 2; ++x)
        {
            HighlightCurrentIndex();
            for (var i = vertIdx; i <= vertIdx + 3; ++i)
            {
                vertColors[i] = NegativeColor;
            }

            _tmp.UpdateVertexData();

            var dt = 0f;
            while (dt < 0.05f)
            {
                dt += Time.deltaTime;
                yield return null;
            }

            HighlightCurrentIndex();
            for (var i = vertIdx; i <= vertIdx + 3; ++i)
            {
                vertColors[i] = HighlightedColor;
            }

            _tmp.UpdateVertexData();

            dt = 0f;
            while (dt < 0.05f)
            {
                dt += Time.deltaTime;
                yield return null;
            }
        }

        HighlightCurrentIndex();
    }

    private IEnumerator GoToNewWord(TMP_Text tmp)
    {
        GameSession.Instance.CanType = false;

        if (GameSession.Instance.Score > 0)
        {
            var txtInfo = tmp.textInfo;

            for (var c = 0; c < txtInfo.characterCount; ++c)
            {
                var cInfo = txtInfo.characterInfo[c];

                var matIdx = cInfo.materialReferenceIndex;

                var vertColors = txtInfo.meshInfo[matIdx].colors32;

                var vertIdx = cInfo.vertexIndex;

                for (var i = vertIdx; i <= vertIdx + 3; ++i)
                {
                    vertColors[i].a = 0;
                    tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    yield return null;
                }

                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return null;
            }
        }

        tmp.SetText(_word);
        UserInputDisplay.SetText("");
        UserInputDisplay.ForceMeshUpdate();
        tmp.ForceMeshUpdate();
        
        var txtInfo2 = tmp.textInfo;

        for (var c = 0; c < txtInfo2.characterCount; ++c)
        {
            var cInfo = txtInfo2.characterInfo[c];

            var matIdx = cInfo.materialReferenceIndex;

            var vertColors = txtInfo2.meshInfo[matIdx].colors32;

            var vertIdx = cInfo.vertexIndex;

            for (var i = vertIdx; i <= vertIdx + 3; ++i)
            {
                vertColors[i].a = 0;
            }

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        var txtInfo3 = tmp.textInfo;
        for (var c = 0; c < txtInfo3.characterCount; ++c)
        {
            var cInfo = txtInfo3.characterInfo[c];

            var matIdx = cInfo.materialReferenceIndex;

            var vertColors = txtInfo3.meshInfo[matIdx].colors32;

            var vertIdx = cInfo.vertexIndex;

            Color color;

            if (GameSettings.Instance.Difficulty < GameSettings.EDifficulty.Hard)
            {
                color = UntypedColor;
            }
            else
            {
                color = TypedColor;
            }

            for (var i = vertIdx; i <= vertIdx + 3; ++i)
            {
                vertColors[i] = color;
                vertColors[i].a = 255;
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return null;
            }

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return null;
        }

        MovePointer();
        if (GameSession.Instance.GameTime > 0)
            GameSession.Instance.CanType = true;
    }
}
