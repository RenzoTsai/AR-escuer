using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NRKernal;

namespace WorldPosition {
    public class GetPosition : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject bodyLockMenu;
        public Text positionText;
        public Text distanceText;
        public GameObject anchor1;
        public GameObject anchor2;
        private GameObject selectedAnchor;
        public int currentAnchorIndex;
        public GameObject LeftArrow;
        public GameObject RightArrow;
        private int lastFrameDistance;
        public GameObject frontMenu;
        public GameObject TurnBackToLeftArrow;
        public GameObject TurnBackToRightArrow;
        public GameObject navigation;
        public AudioSource finishedAudio;

        private static float currentExitDistance;

        void Start()
        {
            selectedAnchor = anchor1;
            LeftArrow.SetActive(true);
            RightArrow.SetActive(false);
            TurnBackToLeftArrow.SetActive(false);
            TurnBackToRightArrow.SetActive(false);
            currentAnchorIndex = 1;
            currentExitDistance = 10000000.0f;
            
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log((Vector3)NRKernal.NRFrame.HeadPose.position);

            bodyLockMenu.transform.position = NRKernal.NRFrame.HeadPose.position;
            bodyLockMenu.transform.rotation = Camera.main.transform.rotation;
            int anchorHeadDistance = (int)Vector3.Distance(Camera.main.transform.position, selectedAnchor.transform.position);
            currentExitDistance = Vector3.Distance(Camera.main.transform.position, anchor2.transform.position);
            SelectAnchor(anchorHeadDistance);
            float eccentricAngle = CalculateAngle(frontMenu.transform.position, selectedAnchor.transform.position, Camera.main.transform.position);
            if (currentAnchorIndex == 2 && Mathf.Abs(eccentricAngle) < 20)
            {
                LeftArrow.SetActive(false);
                RightArrow.SetActive(true);

            }

            if (currentExitDistance < 0.8) {
                finishedAudio.Play();
                navigation.SetActive(false);
            }


            distanceText.text = anchorHeadDistance.ToString() + " m";
            if (eccentricAngle < - 45) 
            {
                TurnBackToLeftArrow.SetActive(true);
                TurnBackToRightArrow.SetActive(false);
            }
            else if (eccentricAngle > 45) 
            {
                TurnBackToLeftArrow.SetActive(false);
                TurnBackToRightArrow.SetActive(true);
            }
            else {
                TurnBackToLeftArrow.SetActive(false);
                TurnBackToRightArrow.SetActive(false);
            }
            positionText.text = "Debug: " + NRKernal.NRFrame.HeadPose.position.ToString() + "\n" + eccentricAngle.ToString();
        }

        float CalculateAngle(Vector3 v1, Vector3 v2, Vector3 n) 
        {
            Vector3 l1 = v1-n;
            Vector3 l2 = v2-n;
            Vector3 l1_2D = new Vector3(l1.x, l1.z, 0);
            Vector3 l2_2D = new Vector3(l2.x, l2.z, 0);
            Vector3 cross = Vector3.Cross(l1_2D, l2_2D.normalized);
            float sign = (cross.z > 0)? -1:1;
            float angle = Vector3.Angle(l1_2D, l2_2D);
            return angle * sign;
        }

        int SelectAnchor(int anchorHeadDistance)
        {
            if (anchorHeadDistance == 0)
            {
                currentAnchorIndex = 2;
                selectedAnchor = anchor2;
                anchor1.SetActive(false);
                anchor2.SetActive(true);
            }
            return currentAnchorIndex;
        }

        public static float GetCurrentExitDistance()
        {
            return currentExitDistance;
        }
    }


}
