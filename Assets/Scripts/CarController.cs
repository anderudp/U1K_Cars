using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject[] wheels;
    public bool fourWheelDrive = true;

    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxBrakeTorque;

    // Update is called once per frame
    void Update()
    {
        bool brakeEngaged = Input.GetKey(KeyCode.Space);
        for (int i = 0; i < 4; i++)
        {
            var col = wheels[i].GetComponent<WheelCollider>();
            if (i < 2) 
            {
                col.steerAngle = maxSteeringAngle * Input.GetAxis("Horizontal");
            }
            if ((!fourWheelDrive && i >= 2) || fourWheelDrive)
            {
                // Ternáris operátor, így működik:
                // X változó = Y állítás ? X értéke ha Y == true : X értéke ha Y == false
                col.motorTorque = brakeEngaged ? 0f : maxMotorTorque * Input.GetAxis("Vertical");
                col.brakeTorque = brakeEngaged ? maxBrakeTorque : 0f;
            }
            Vector3 p;
            Quaternion q;
            col.GetWorldPose(out p, out q);
            wheels[i].transform.rotation = q;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            if (GuiController.currentTime < GuiController.bestTime || GuiController.bestTime == 0f)
            {
                GuiController.bestTime = GuiController.currentTime;
            }
            GuiController.startTime = Time.time;
        }
    }
}
