using UnityEngine;

namespace Final_Survivors.Weapons
{
    [RequireComponent(typeof(Animator))]
    public class IKWeapons : MonoBehaviour
    {
        protected Animator animator;

        [SerializeField] private bool ikActive = false;
        [SerializeField] private Transform rightHand = null;
        [SerializeField] private Transform leftHand = null;
        [SerializeField] private Transform lookObj = null;

        //private int far = 200;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK() //a callback for calculating IK
        {
            if (animator)
            {
                if (ikActive) //if the IK is active, set the position and rotation directly to the goal.
                {
                    if (lookObj != null) // Set the look target position, if one has been assigned
                    {
                        animator.SetLookAtWeight(0.5f);
                        animator.SetLookAtPosition(lookObj.position);
                    }
                    /*else // Look At player forward + far
                    {
                        animator.SetLookAtPosition(transform.forward * far);
                    }*/

                    if (rightHand != null)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
                    }

                    if (leftHand != null)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
                    }
                }
                else //if the IK is not active, set the positions and rotations of the hands back to the original positions
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                    animator.SetLookAtWeight(0f);
                }
            }
        }

        public void SetIK(Transform lh, Transform rh)
        {
            leftHand = lh;
            rightHand = rh;
        }

        public void SetIKActive(bool value)
        {
            ikActive = value;
        }
    }
}
