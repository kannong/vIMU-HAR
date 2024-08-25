using UnityEngine;
using Uduino;
using Csv_Function;

public class ReceiveIMUValues : MonoBehaviour {

    Vector3 position;
    Vector3 rotation;
    public Vector3 rotationOffset ;
    public float speedFactor = 15.0f;
    public string imuName = "r"; // You should ignore this if there is one IMU.
    private CsvFunction csvf = new CsvFunction("IMUReal");
    private string csv_path;

    void Start () {
        //  UduinoManager.Instance.OnDataReceived += ReadIMU;
        //  Note that here, we don't use the delegate but the Events, assigned in the Inpsector Panel
        string header = "a_x" + "," + "a_y" + "," + "a_z" + "," +
                      "g_x" + "," + "g_y" + "," + "g_z" + ",";
        csv_path = csvf.BinSourcesFolder + "imureal_data.csv";
        csvf.Csv_Init(header, csv_path);
    }

    void Update() { }

    public void ReadIMU (string data, UduinoDevice device) {

        string[] values = data.Split('/');
        if (values.Length == 12&&values[0] == imuName) // Rotation of the first one 
        {
            float ax = float.Parse(values[1]);
            float ay = float.Parse(values[2]);
            float az = float.Parse(values[3]);
            float gx = float.Parse(values[4]);
            float gy = float.Parse(values[5]);
            float gz = float.Parse(values[6]);
            float temp = float.Parse(values[7]);
            float w = float.Parse(values[8]);
            float x = float.Parse(values[9]);
            float y = float.Parse(values[10]);
            float z = float.Parse(values[11]);
            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, new Quaternion(w, y, x, z), Time.deltaTime * speedFactor);
            Debug.Log("ax,ay,az= " + ax + "\t" + ay + "\t" + az + "\t" +
                      " gx,gy,gz= " + gx + "\t" + gy + "\t" + gz + "\t" +
                      "tempature= " + temp);

            // write to csv
            string one_row = ax + "," + ay + "," + az + "," + gx + "," + gy + "," + gz + ",";
            csvf.WriteCsvnew(one_row, csv_path);
        }
        else
        {
            Debug.LogWarning(data);
        }
        this.transform.parent.transform.eulerAngles = rotationOffset;
        //  Log.Debug("The new rotation is : " + transform.Find("IMU_Object").eulerAngles);

    }
}
