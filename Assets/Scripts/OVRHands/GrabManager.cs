using AltGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AltGrab {
    public class GrabManager : MonoBehaviour
    {
        public AltOVRGrabber grabber;

        public OVRInput.RawAxis1D grabAxis;

        private void Awake()
        {
            if(grabber == null)
            {
                grabber = GetComponent<AltOVRGrabber>();
            }    
        }

        // Update is called once per frame
        void Update()
        {
            if (grabber.grabbedObject == null && OVRInput.Get(grabAxis) > 0.3f)
            {
                grabber.AttemptGrab();
            } else if(grabber.grabbedObject != null && OVRInput.Get(grabAxis) < 0.3f)
            {
                grabber.AttemptLetGo();
            }

        }
    }
}