using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace ConnectPy
{
    public class Connect_Python_func : MonoBehaviour
    {
        public static string output;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void RunPythonScript(string file_path,string[] argvs)
        {
            Process p = new Process();
            string path = file_path;
            foreach (string temp in argvs)
            {
                path += " " + temp;
            }
            p.StartInfo.FileName = @"E:\softwa2\anaconda3\python.exe";

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = path;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.BeginOutputReadLine();
            p.OutputDataReceived += new DataReceivedEventHandler(Get_data);
            p.WaitForExit();
        }
        private static void Get_data(object sender, DataReceivedEventArgs eventArgs)
        {
            if (!string.IsNullOrEmpty(eventArgs.Data))
            {
                output = eventArgs.Data;
                print("res="+eventArgs.Data);
            }

        }
    }
}
