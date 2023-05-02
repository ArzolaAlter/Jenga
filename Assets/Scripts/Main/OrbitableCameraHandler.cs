using Cinemachine;
using UnityEngine;

namespace Jenga.Main
{
    /// <summary>
    /// Class responsible for allowing the camera to drag around
    /// </summary>
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class OrbitableCameraHandler : JengaMonoBehaviour
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        // Runtime
        private CinemachineFreeLook freeLook;

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            Instances.GameBrain.SelectedGradeChanged += OnSelectedGradeChanged;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            Instances.GameBrain.SelectedGradeChanged -= OnSelectedGradeChanged;
        }

        private void OnSelectedGradeChanged(object sender, JengaTower tower)
        {
            freeLook.Follow = tower.TowerCenter;
            freeLook.LookAt = tower.TowerCenter;
        }

        public float GetAxisCustom(string axisName)
        {
            if (axisName == "Mouse X")
            {
                if (Input.GetMouseButton(0))
                {
                    return UnityEngine.Input.GetAxis("Mouse X");
                }
                else
                {
                    return 0;
                }
            }
            else if (axisName == "Mouse Y")
            {
                if (Input.GetMouseButton(0))
                {
                    return UnityEngine.Input.GetAxis("Mouse Y");
                }
                else
                {
                    return 0;
                }
            }
            return UnityEngine.Input.GetAxis(axisName);
        }

        protected override void Start()
        {
            base.Start();
            CinemachineCore.GetInputAxis = GetAxisCustom;
        }

        private void Awake()
        {
            freeLook = gameObject.GetComponent<CinemachineFreeLook>();
        }
    }
}