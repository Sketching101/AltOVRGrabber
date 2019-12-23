using AltGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AltGrab
{
    public class AltOVRGrabbable : MonoBehaviour
    {
        public AltOVRGrabber grabbedBy;
        public Transform gripTr;
        public Transform gripOffset;

        public bool isGrabbed
        {
            get
            {
                return grabbedBy != null;
            }
        }

        // Start is called before the first frame update
        protected virtual void Awake()
        {
            if(gripOffset == null)
            {
                gripOffset = transform;
                Debug.Log("Grip offset is null");
            }

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {
            if (isGrabbed)
            {
                GrabbedFixedUpdate();
            }
        }

        public virtual void GrabbedFixedUpdate()
        {
            gripTr.position = grabbedBy.gripTransform.position + (gripTr.position - gripOffset.position);
            gripTr.rotation = grabbedBy.gripTransform.rotation * gripOffset.localRotation;
        }

        public virtual void OnGrab()
        {

        }

        public virtual void OnLetGo()
        {
            grabbedBy = null;
        }
    }
}