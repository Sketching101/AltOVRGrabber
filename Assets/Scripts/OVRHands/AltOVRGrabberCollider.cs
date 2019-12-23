using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AltGrab
{
    [RequireComponent(typeof(Collider))]
    public class AltOVRGrabberCollider : MonoBehaviour
    {
        public AltOVRGrabber grabber;

        protected virtual void OnTriggerEnter(Collider other)
        {
            AltOVRGrabbableCollider altGrabbable;
            if((altGrabbable = other.GetComponent<AltOVRGrabbableCollider>()) != null)
            {
                grabber.AddGrabbable(altGrabbable.grabbable);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            AltOVRGrabbableCollider altGrabbable;
            if ((altGrabbable = other.GetComponent<AltOVRGrabbableCollider>()) != null)
            {
                grabber.RemoveGrabbable(altGrabbable.grabbable);
            }
        }
    }
}