using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class ArduinoBasic : MonoBehaviour {

    private SerialPort arduino;
    public string port;
    private Thread readThread; // 宣告執行緒
    //[HideInInspector]
    public string readMessage;
    private string errorMessage;
    bool isNewMessage;

    void Start() {

        if (port != "") {
            arduino = new SerialPort(port, 57600);
            arduino.ReadTimeout = 10;
            try {
                arduino.Open();
                readThread = new Thread(new ThreadStart(ArduinoRead));
                readThread.Start();
                Debug.Log("SerialPort Open");
            } catch (System.Exception e) {
                Debug.Log("SerialPort Fail To Open");
                Debug.Log(e);
            }
        }

    }
    void Update() {
        if (isNewMessage) {
            //Debug.Log(readMessage);
        }
        isNewMessage = false;
    }

    private void ArduinoRead() {
        while (arduino.IsOpen) {
            try {

                readMessage = arduino.ReadLine(); // 讀取SerialPort資料並裝入readMessage
                isNewMessage = true;

            } catch (System.Exception e) {
                errorMessage = e.Message;
            }
        }
    }


    public void ArduinoWrite(string message) {
        //Debug.Log(message);
        arduino.Write(message);
    }

    public void OnApplicationQuit() {

        if (arduino != null) {
            if (arduino.IsOpen) {
                arduino.Close();
            }
        }
    }

}