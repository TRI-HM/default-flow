using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonBlueClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float pressDepth = 0.1f; // Độ sâu khi nhấn
    public float speed = 15f; // Tốc độ di chuyển
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    public Animator firstAnimator;  // Animator của đối tượng đầu tiên
    public string firstAnimation = "Jump"; // Tên animation đầu tiên

    public Animator secondAnimator; // Animator của đối tượng thứ hai
    public string secondAnimation = "Spin"; // Tên animation thứ hai

    // Active result when animation finished
    public GameObject resultObject; // Đối tượng kết quả
    private bool isResultActive = false; // Biến kiểm tra trạng thái của đối tượng kết quả


    private void Start()
    {
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
        resultObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetPosition = originalPosition + new Vector3(0, -pressDepth, 0);
        // Nếu có animator đầu tiên, chạy animation đầu tiên
        if (firstAnimator != null && !isResultActive)
        {
            // Kiểm tra xem animation đầu tiên đã chạy chưa
            if (firstAnimator.GetCurrentAnimatorStateInfo(0).IsName(firstAnimation))
            {
                return; // Nếu animation đã chạy, không làm gì cả
            }

            // Nếu animation chưa chạy, bắt đầu chạy animation đầu tiên
            isResultActive = true; // Đánh dấu là đã chạy animation đầu tiên
            firstAnimator.Play(firstAnimation);
            StartCoroutine(WaitForAnimation(firstAnimator, firstAnimation));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetPosition = originalPosition;

    }

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, speed * Time.deltaTime);
    }

    IEnumerator WaitForAnimation(Animator anim, string animationName)
    {
        // Lấy thông tin animation clip
        AnimationClip clip = GetAnimationClip(anim, animationName);
        if (clip != null)
        {
            yield return new WaitForSeconds(clip.length); // Đợi animation chạy xong
        }

        // Sau khi animation đầu tiên kết thúc, chạy animation thứ hai
        // if (secondAnimator != null)
        // {
        //     secondAnimator.Play(secondAnimation);
        // }

        // Kích hoạt đối tượng kết quả
        if (resultObject != null)
        {
            resultObject.SetActive(true);
        }

    }

    AnimationClip GetAnimationClip(Animator anim, string animationName)
    {
        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip;
            }
        }
        return null;
    }
}
