using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class PopupResult : MonoBehaviour
{
    public Animator animator; // Animator component attached to the GameObject
    public string animationName = "Name of the Animation"; // Name of the animation to play
    public float animationDuration = 1f; // Duration of the animation in seconds

    private void Start()
    {

        DelayedPlayAnimation();
    }

    public async void DelayedPlayAnimation()
    {
        await Task.Delay(5000); // Delay for 2000 milliseconds (2 seconds)
        PlayAnimation(); // Call the PlayAnimation function after the delay
    }

    public void PlayAnimation()
    {
        // Check if the animator is assigned and the animation name is not empty
        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            // Play the animation using the animator component
            animator.Play(animationName);
            Debug.Log("Playing animation: " + animationName);
        }
        else
        {
            Debug.LogWarning("Animator or animation name is not set.");
        }
    }
}
