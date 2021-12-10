using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static UnityAction onSceneLoadComplete;

    [SerializeField] private string initialScene;
    [SerializeField] private GameObject transitionCanvas;
    [SerializeField] private GameObject transitionCircle;
    [SerializeField] private GameObject transitionCircleMask;

    [SerializeField] private float waitTimeBetweenScenes = 0.5f;

    [Tooltip("How long a single fading animation plays")] [SerializeField]
    public float transitionTime = 1f;

    [SerializeField] private FadingType defaultFadeInType = FadingType.Opacity;

    private static SceneManager _instance;

    private static bool _transitionInProgress;
    private Image _image;

    private static FadingType _fadeInType;
    private static Vector2 _fadingFillPosition;

    private static RectTransform _circleTransform;
    private static RectTransform _maskTransform;
    private static RectTransform _canvasTransform;

    private Vector2 _circleOutPosition;

    public enum FadingType
    {
        None,
        Opacity,
        Up,
        Right,
        Left,
        Down,
        FillOut,
        FillIn
    }

    private void Awake()
    {
        _image = transitionCircle.GetComponent<Image>();

        _circleTransform = transitionCircle.GetComponent<RectTransform>();
        _maskTransform = transitionCircleMask.GetComponent<RectTransform>();
        _canvasTransform = transitionCanvas.GetComponent<RectTransform>();
    }

    void Start()
    {
        transitionCircle.gameObject.SetActive(true);

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        _fadingFillPosition = new Vector2(0, 0);
        _circleOutPosition = (_canvasTransform.sizeDelta + _circleTransform.sizeDelta) * 0.5f + new Vector2(10, 10);

        _fadeInType = defaultFadeInType;
        StartCoroutine(FirstFadeIn());
    }

    private IEnumerator LoadSceneAdditive(string sceneName, FadingType fadeOutType)
    {
        yield return null;

        if (_transitionInProgress) yield break;
        _transitionInProgress = true;

        if (fadeOutType != FadingType.None)
        {
            FadeOut(fadeOutType);
            yield return new WaitForSeconds(transitionTime);
        }

        for (var i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name != "Main")
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
            }
        }

        yield return new WaitForSeconds(waitTimeBetweenScenes);
        
        var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        loading.allowSceneActivation = true;
        while (!loading.isDone)
        {
            yield return null;
        }

        yield return null;

        var sceneLoader = FindObjectOfType<SceneLoader>();

        yield return new WaitUntil(() => sceneLoader.isReady);

        FadeIn(_fadeInType);

        if (_fadeInType != FadingType.None)
        {
            yield return new WaitForSeconds(transitionTime);
        }

        _fadeInType = defaultFadeInType;
        _transitionInProgress = false;
        onSceneLoadComplete?.Invoke();
    }

    private IEnumerator FirstFadeIn()
    {
        _transitionInProgress = true;
        AddScene(initialScene);
        yield return new WaitForSeconds(waitTimeBetweenScenes);
        FadeIn(_fadeInType);
        yield return new WaitForSeconds(transitionTime);
        _transitionInProgress = false;
        onSceneLoadComplete?.Invoke();
    }

    public static void LoadScene(string sceneName, FadingType fadeOutType)
    {
        _instance.StartCoroutine(_instance.LoadSceneAdditive(sceneName, fadeOutType));
    }

    public static void AddScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public static void RemoveScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
    }

    public void FadeIn(FadingType dir)
    {
        LeanTween.cancel(transitionCircle);
        LeanTween.cancel(transitionCircleMask);

        _maskTransform.sizeDelta = Vector2.zero;
        _circleTransform.localScale = Vector3.one;
        _circleTransform.localPosition = Vector3.zero;
        _maskTransform.localPosition = Vector3.zero;

        var targetCoords = new Vector3();

        Color tempColor;

        switch (dir)
        {
            case FadingType.Up:
                targetCoords = new Vector3(0, _circleOutPosition.y, 0);
                break;
            case FadingType.Right:
                targetCoords = new Vector3(_circleOutPosition.x, 0, 0);
                break;
            case FadingType.Down:
                targetCoords = new Vector3(0, -_circleOutPosition.y, 0);
                break;
            case FadingType.Left:
                targetCoords = new Vector3(-_circleOutPosition.x, 0, 0);
                break;
            case FadingType.None:
                tempColor = _image.color;
                tempColor.a = 0f;
                _image.color = tempColor;
                break;
            case FadingType.Opacity:
                _circleTransform.localPosition = Vector3.zero;
                break;
            case FadingType.FillOut:
                tempColor = _image.color;
                tempColor.a = 1f;
                _image.color = tempColor;

                _maskTransform.sizeDelta = Vector2.zero;
                _maskTransform.localPosition = _fadingFillPosition;
                _circleTransform.localScale = new Vector3(1, 1, 1);
                break;
            case FadingType.FillIn:
                tempColor = _image.color;
                tempColor.a = 1f;
                _image.color = tempColor;

                _maskTransform.sizeDelta = Vector2.zero;
                _maskTransform.localPosition = _fadingFillPosition;
                _circleTransform.localScale = new Vector3(1, 1, 1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }

        switch (dir)
        {
            case FadingType.Up:
            case FadingType.Right:
            case FadingType.Down:
            case FadingType.Left:
                tempColor = _image.color;
                tempColor.a = 1f;
                _image.color = tempColor;

                LeanTween.moveLocal(transitionCircle, targetCoords, transitionTime).setEaseInOutSine();
                break;
            case FadingType.Opacity:
                LeanTween.value(1f, 0f, transitionTime).setEaseInOutSine().setOnUpdate(UpdateFadeCircleAlpha);
                break;
            case FadingType.None:
                break;
            case FadingType.FillOut:
                LeanTween.scale(transitionCircle, Vector3.zero, transitionTime).setEaseInOutSine();
                break;
            case FadingType.FillIn:
                LeanTween.value(_maskTransform.sizeDelta.x, _circleTransform.sizeDelta.x, transitionTime)
                    .setEaseInOutSine()
                    .setOnUpdate(UpdateMaskSizeX);
                LeanTween.value(_maskTransform.sizeDelta.y, _circleTransform.sizeDelta.y, transitionTime)
                    .setEaseInOutSine()
                    .setOnUpdate(UpdateMaskSizeY);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }
    }

    private void UpdateFadeCircleAlpha(float alpha)
    {
        var tempColor = _image.color;
        tempColor.a = alpha;
        _image.color = tempColor;
    }

    public void FadeOut(FadingType dir)
    {
        LeanTween.cancel(transitionCircle);
        LeanTween.cancel(transitionCircleMask);

        _maskTransform.sizeDelta = Vector2.zero;
        _circleTransform.localScale = Vector3.one;
        _circleTransform.localPosition = Vector3.zero;
        _maskTransform.localPosition = Vector3.zero;

        Color tempColor;
        switch (dir)
        {
            case FadingType.Up:
                _circleTransform.localPosition = new Vector3(0, _circleOutPosition.y, 0);
                break;
            case FadingType.Right:
                _circleTransform.localPosition = new Vector3(_circleOutPosition.x, 0, 0);
                break;
            case FadingType.Down:
                _circleTransform.localPosition = new Vector3(0, -_circleOutPosition.y, 0);
                break;
            case FadingType.Left:
                _circleTransform.localPosition = new Vector3(-_circleOutPosition.x, 0, 0);
                break;
            case FadingType.None:
                tempColor = _image.color;
                tempColor.a = 0f;
                _image.color = tempColor;
                break;
            case FadingType.Opacity:
                _circleTransform.localPosition = Vector3.zero;
                tempColor = _image.color;
                tempColor.a = 0f;
                _image.color = tempColor;
                break;
            case FadingType.FillOut:
                tempColor = _image.color;
                tempColor.a = 1f;
                _image.color = tempColor;

                _maskTransform.localPosition = _fadingFillPosition;
                _maskTransform.sizeDelta = Vector2.zero;
                _circleTransform.localScale = new Vector3(0, 0, 1);
                break;
            case FadingType.FillIn:
                tempColor = _image.color;
                tempColor.a = 1f;
                _image.color = tempColor;

                _maskTransform.localPosition = _fadingFillPosition;
                _maskTransform.sizeDelta = _circleTransform.sizeDelta;
                _circleTransform.localScale = new Vector3(1, 1, 1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }

        switch (dir)
        {
            case FadingType.Up:
            case FadingType.Right:
            case FadingType.Down:
            case FadingType.Left:
                tempColor = _image.color;
                tempColor.a = 1f;
                _image.color = tempColor;

                LeanTween.moveLocal(transitionCircle, Vector3.zero, transitionTime).setEaseInOutSine();
                break;
            case FadingType.Opacity:
                LeanTween.value(0f, 1f, transitionTime).setEaseInOutSine().setOnUpdate(UpdateFadeCircleAlpha);
                break;
            case FadingType.None:
                break;
            case FadingType.FillOut:
                LeanTween.scale(transitionCircle, Vector3.one, transitionTime).setEaseInSine();
                break;
            case FadingType.FillIn:
                LeanTween.value(_circleTransform.sizeDelta.x, 0, transitionTime)
                    .setEaseOutSine()
                    .setOnUpdate(UpdateMaskSizeX);
                LeanTween.value(_circleTransform.sizeDelta.y, 0, transitionTime)
                    .setEaseOutSine()
                    .setOnUpdate(UpdateMaskSizeY);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }
    }

    private void UpdateMaskSizeX(float x)
    {
        _maskTransform.sizeDelta = new Vector2(x, _maskTransform.sizeDelta.y);
    }

    private void UpdateMaskSizeY(float y)
    {
        _maskTransform.sizeDelta = new Vector2(_maskTransform.sizeDelta.x, y);
    }

    public static void SetFadeInType(FadingType type)
    {
        if (_transitionInProgress) return;

        _fadeInType = type;
    }

    public static void SetFadingFillPosition(Vector2 viewPortPoint)
    {
        var canvasSize = _canvasTransform.sizeDelta;
        var canvasPoint = new Vector2(viewPortPoint.x * canvasSize.x, viewPortPoint.y * canvasSize.y) -
                          canvasSize * 0.5f;

        var radius = (_circleTransform.sizeDelta.x * 0.15f);

        if (canvasPoint.magnitude > radius)
        {
            canvasPoint = canvasPoint.normalized * radius;
        }

        _fadingFillPosition = canvasPoint;
    }

    public static void SetFadingFillPosition()
    {
        _fadingFillPosition = Vector2.zero;
    }

    public static bool IsTransitionInProgress()
    {
        return _transitionInProgress;
    }
}