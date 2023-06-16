using System;
using Cinemachine;
using UnityEngine;
using Rewired;

namespace Spacebar
{

    [RequireComponent(typeof(CinemachineFreeLook))]
    class CinemachineFreeLookZoom : MonoBehaviour
    {
        private CinemachineFreeLook freelook;
        private CinemachineFreeLook.Orbit[] originalOrbits;

        private Player player; //Rewired player

        [Range(0.0F, 1F)]
        private float zoomPercent = 0.5f;
        public float zoomWheelSensitivity = 100;
        public float minZoom = 0.08f;

        public void Awake()
        {
            freelook = GetComponentInChildren<CinemachineFreeLook>();
            originalOrbits = new CinemachineFreeLook.Orbit[freelook.m_Orbits.Length];

            if (ReInput.players != null)
            {
                player = ReInput.players.GetPlayer(0);
            }

            for (int i = 0; i < freelook.m_Orbits.Length; i++)
            {
                originalOrbits[i].m_Height = freelook.m_Orbits[i].m_Height;
                originalOrbits[i].m_Radius = freelook.m_Orbits[i].m_Radius;
            }
        }

        public void Update()
        {
            zoomPercent -= (player.GetAxis("Look Zoom") / zoomWheelSensitivity);
            if (zoomPercent < minZoom) zoomPercent = minZoom;
            if (zoomPercent > 1) zoomPercent = 1;

            //Debug.Log("Look Zoom: " + zoomPercent);

            for (int i = 0; i < freelook.m_Orbits.Length; i++)
            {
                freelook.m_Orbits[i].m_Height = originalOrbits[i].m_Height * zoomPercent;
                freelook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * zoomPercent;
            }
        }
    }
}