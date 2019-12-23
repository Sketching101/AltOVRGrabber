using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AltGrab {
    public enum Hand
    {
        Left, Right
    }

    public class AltOVRGrabber : MonoBehaviour
    {
        public AltOVRGrabbable grabbedObject;
        public Hand hand;
        private Dictionary<AltOVRGrabbable, int> grabWithinRangeDict;
        private List<AltOVRGrabbable> grabWithinRangeList;

        public static AltOVRGrabber rightGrabber;
        public static AltOVRGrabber leftGrabber;

        public Transform gripTransform;
        public Transform handTransform;


        private bool alreadyUpdated = false;

        protected Quaternion anchorOffsetRotation;
        protected Vector3 anchorOffsetPosition;

        protected Vector3 lastPos;
        protected Quaternion lastRot;

        protected virtual void Awake()
        {
            grabWithinRangeDict = new Dictionary<AltOVRGrabbable, int>();
            grabWithinRangeList = new List<AltOVRGrabbable>();
            if (hand == Hand.Left)
            {
                if (leftGrabber == null)
                    leftGrabber = this;
                else
                    Debug.LogError("Multiple left hands!");
            }
            else if (hand == Hand.Right)
            {
                if (rightGrabber == null)
                    rightGrabber = this;
                else
                    Debug.LogError("Multiple right hands!");
            }


            anchorOffsetPosition = transform.localPosition;
            anchorOffsetRotation = transform.localRotation;

        }

        protected void Update()
        {
            alreadyUpdated = false;
        }

        protected void FixedUpdate()
        {
            HandFixedUpdate();
        }

        // Hands follow the touch anchors by calling MovePosition each frame to reach the anchor.
        // This is done instead of parenting to achieve workable physics. If you don't require physics on
        // your hands or held objects, you may wish to switch to parenting.
        void OnUpdatedAnchors()
        {
            // Don't want to MovePosition multiple times in a frame, as it causes high judder in conjunction
            // with the hand position prediction in the runtime.
            if (alreadyUpdated) return;
            alreadyUpdated = true;

            Vector3 destPos = handTransform.TransformPoint(anchorOffsetPosition);
            Quaternion destRot = handTransform.rotation * anchorOffsetRotation;

            if(destPos != lastPos || destRot != lastRot)
            {
                GetComponent<Rigidbody>().MovePosition(destPos);
                GetComponent<Rigidbody>().MoveRotation(destRot);
            }

            lastPos = transform.position;
            lastRot = transform.rotation;
        }

        protected void HandFixedUpdate()
        {
            OnUpdatedAnchors();
         //   transform.position = handTransform.position;
         //   transform.rotation = handTransform.rotation;
        }

        /// <summary>
        /// Grabs an object, if a grabbable object is within range.
        /// </summary>
        /// <returns>Whether or not an object was successfully grabbed.</returns>
        public bool AttemptGrab()
        {
            if (grabbedObject != null || grabWithinRangeDict.Count == 0)
            {
                OnFailGrab();
                return false;
            } else
            {
                OnGrab();
                return true;
            }
        }

        /// <summary>
        /// Lets go of held object, if an object is held.
        /// </summary>
        /// <returns>Whether or not object was successfully let go</returns>
        public bool AttemptLetGo()
        {
            if (grabbedObject == null)
            {
                return false;
            }
            else
            {
                OnLetGo();
                return true;
            }
        }

        public virtual void OnLetGo()
        {
            grabbedObject.OnLetGo();
            grabbedObject = null;
        }

        /// <summary>
        /// Dummy function
        /// </summary>
        public virtual void OnFailGrab()
        {

        }

        /// <summary>
        /// Action on grab
        /// </summary>
        public virtual void OnGrab()
        {
            int count = GetFirstFull();
            if(count >= grabWithinRangeList.Count)
            {
                Debug.LogError("ERROR! Should never get an index greater than the size of the list");
            }
            grabbedObject = grabWithinRangeList[count];
            if (grabWithinRangeDict.ContainsKey(grabbedObject))
            {

            } else
            {
                Debug.LogError("Grabbing nothing!");
            }
            grabbedObject.grabbedBy = this;

            Debug.Log("OnGrab()");

            // Call grabbed objects OnGrab()
            grabbedObject.OnGrab();
        }

        public void AddGrabbable(AltOVRGrabbable grab)
        {
            if(!grabWithinRangeDict.ContainsKey(grab))
            {
                int count = GetFirstEmpty();

                if (count == grabWithinRangeList.Count)
                    grabWithinRangeList.Add(grab);
                else
                    grabWithinRangeList[count] = grab;

                grabWithinRangeDict.Add(grab, count);
            }
        }

        public void RemoveGrabbable(AltOVRGrabbable grab)
        {
            if (grabWithinRangeDict.ContainsKey(grab))
            {
                int idx = grabWithinRangeDict[grab];
                grabWithinRangeDict.Remove(grab);
                grabWithinRangeList[idx] = null;
            }
        }

        public int GetFirstEmpty()
        {
            int j;
            for(j = 0; j < grabWithinRangeList.Count; j++)
            {
                if(grabWithinRangeList[j] == null)
                {
                    break;
                }
            }
            return j;
        }

        public int GetFirstFull()
        {
            int j;
            for (j = 0; j < grabWithinRangeList.Count; j++)
            {
                if (grabWithinRangeList[j] != null)
                {
                    break;
                }
            }
            return j;
        }
    }
}