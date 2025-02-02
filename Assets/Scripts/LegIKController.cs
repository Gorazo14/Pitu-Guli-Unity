using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LegIKController : MonoBehaviour
{
    [SerializeField] private PlayerMovment playerMovement;

    [SerializeField] private Transform leftLegTarget;
    [SerializeField] private Transform rightLegTarget;
    [SerializeField] private Transform pelvis;
    [SerializeField] private float jumpLegOffset = -0.2f; // Adjust this to move legs down during jumps

    private Vector3 leftLegStartPos;
    private Vector3 rightLegStartPos;

    void Start()
    {
        // Save original positions
        leftLegStartPos = leftLegTarget.localPosition;
        rightLegStartPos = rightLegTarget.localPosition;
    }

    void Update()
    {
        if (playerMovement.isGrounded)
        {
            // Move legs downward slightly to prevent clipping
            leftLegTarget.localPosition = leftLegStartPos + new Vector3(0, jumpLegOffset, 0);
            rightLegTarget.localPosition = rightLegStartPos + new Vector3(0, jumpLegOffset, 0);
        }
        else
        {
            // Reset leg positions when not jumping
            leftLegTarget.localPosition = leftLegStartPos;
            rightLegTarget.localPosition = rightLegStartPos;
        }
    }
}