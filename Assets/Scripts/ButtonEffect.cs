using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using System.Linq;

public class ButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);

    public GameObject findIDInput;
    public TMP_InputField inputField;
    public float slideDuration = 0.5f;
    public Vector3 hiddenPosition = new Vector3(-500f, 0f, 0f);
    public Vector3 shownPosition = new Vector3(0f, 0f, 0f);

    private Coroutine slideCoroutine;
    private Coroutine wipeCoroutine;
    private bool isInputVisible = false;

    private void Start()
    {
        if (findIDInput != null)
        {
            findIDInput.SetActive(false);
            findIDInput.GetComponent<RectTransform>().anchoredPosition = hiddenPosition;
        }
        if (inputField != null)
        {
            inputField.onSubmit.AddListener(OnInputSubmit);
        }
    }

    private void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onSubmit.RemoveListener(OnInputSubmit);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = normalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (findIDInput == null) return;

        var rectTransform = findIDInput.GetComponent<RectTransform>();

        if (!isInputVisible)
        {
            findIDInput.SetActive(true);
            if (slideCoroutine != null) StopCoroutine(slideCoroutine);
            slideCoroutine = StartCoroutine(SlideIn(rectTransform));

            if (inputField != null)
            {
                if (wipeCoroutine != null) StopCoroutine(wipeCoroutine);
                wipeCoroutine = StartCoroutine(WipeInInputField(inputField, 0.5f));
            }
        }
        else
        {
            if (slideCoroutine != null) StopCoroutine(slideCoroutine);
            slideCoroutine = StartCoroutine(SlideOut(rectTransform));

            if (inputField != null)
            {
                if (wipeCoroutine != null) StopCoroutine(wipeCoroutine);
                wipeCoroutine = StartCoroutine(WipeOutInputField(inputField, 0.5f, () => findIDInput.SetActive(false)));
            }
            else
            {
                StartCoroutine(DeactivateAfterDelay(findIDInput, slideDuration));
            }
        }
        isInputVisible = !isInputVisible;
    }

    private IEnumerator SlideIn(RectTransform rectTransform)
    {
        float elapsed = 0f;
        rectTransform.anchoredPosition = hiddenPosition;
        while (elapsed < slideDuration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(hiddenPosition, shownPosition, elapsed / slideDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = shownPosition;
    }

    private IEnumerator SlideOut(RectTransform rectTransform)
    {
        float elapsed = 0f;
        Vector3 start = rectTransform.anchoredPosition;
        while (elapsed < slideDuration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(start, hiddenPosition, elapsed / slideDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = hiddenPosition;
    }

    private IEnumerator WipeInInputField(TMP_InputField inputField, float duration)
    {
        string fullText = inputField.text;
        inputField.text = "";
        int length = fullText.Length;
        float timePerChar = duration / Mathf.Max(1, length);
        for (int i = 1; i <= length; i++)
        {
            inputField.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(timePerChar);
        }
    }

    private IEnumerator WipeOutInputField(TMP_InputField inputField, float duration, System.Action onComplete)
    {
        string fullText = inputField.text;
        int length = fullText.Length;
        float timePerChar = duration / Mathf.Max(1, length);
        for (int i = length; i >= 0; i--)
        {
            inputField.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(timePerChar);
        }
        onComplete?.Invoke();
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    // Hàm xử lý khi bấm Enter trong InputField
    private void OnInputSubmit(string inputText)
    {
        if (int.TryParse(inputText, out int searchID))
        {
            // Tìm ASM_MN trong scene
            ASM_MN asm = FindObjectOfType<ASM_MN>();
            if (asm != null)
            {
                asm.YC4(inputText);
            }
            else
            {
                Debug.LogError("Không tìm thấy ASM_MN trong scene.");
            }
        }
    }
}
