using IMUSim;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Csv_Function
{

    public class CsvFunction : MonoBehaviour
    {

        //get streamingAssetsPath
        private string binSourcesFolder = Application.streamingAssetsPath + "/SourcesFolder/";
        private bool first = false;

        public CsvFunction(string type) {
            binSourcesFolder += "/" + type + "/";
            if (System.IO.Directory.Exists(binSourcesFolder) == false)
            {
                System.IO.Directory.CreateDirectory(binSourcesFolder);
            }
        }

        public string BinSourcesFolder
        {
            get
            {
                return binSourcesFolder;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Csv_Init(string header,string path)
        {

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (StreamWriter stream = new StreamWriter(path, true, Encoding.UTF8))
                {
                    stream.WriteLine(header);
                }
            }
            else
            {
                using (StreamWriter stream = new StreamWriter(path, false, Encoding.UTF8))
                {
                    stream.WriteLine(header);
                }
            }
        }

        //write csv file
        public void WriteCsvnew(string strs, string path)
        {

            using (StreamWriter stream = new StreamWriter(path, true, Encoding.UTF8))
            {
                stream.WriteLine(strs);
            }
        }
        public void WriteCsvDatasnew(List<string> strs, string path)
        {
            foreach (var item in strs)
            {
                WriteCsvnew(item, path);
            }
        }

        public void WriteCsv(string header,string strs, string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (StreamWriter stream = new StreamWriter(path, true, Encoding.UTF8))
                {
                    stream.WriteLine(header);
                }
            }

            //save as UTF-8, true: append, false: overwrite
            using (StreamWriter stream = new StreamWriter(path, true, Encoding.UTF8))
            {
                stream.WriteLine(strs);
            }

        }

        // close csv file
        public void FileClose(string path)
        {
            StreamWriter stream = new StreamWriter(path, true, Encoding.UTF8);
            stream.Close();
        }
        public void Saveeua_tocsv(Transform[] jnt, string jnt_name)
        {
            // set file path and file name
            string path = BinSourcesFolder + jnt_name + "_data.csv";
            // set header and joint datas
            string header = "";
            string jointdatas = "";
            foreach (var item in jnt)
            {
                header += item.name + "_x" + "," +
                          item.name + "_y" + "," +
                          item.name + "_z" + ",";
                jointdatas += item.transform.localEulerAngles.x.ToString() + "," +
                              item.transform.localEulerAngles.y.ToString() + "," +
                              item.transform.localEulerAngles.z.ToString() + ",";
            }
            // write all datas
            WriteCsv(header, jointdatas, path);
        }

        public void Saveimudata_tocsv(IMU_Sim imusim)
        {
            // set file path and file name
            string path = BinSourcesFolder + imusim.Avater_name + "_" + imusim.Obj.name + "_data.csv";
            // set header and joint datas
            string header = "";
            string datas = "";
            header += imusim.Obj.name + "a_x" + "," +
                      imusim.Obj.name + "a_y" + "," +
                      imusim.Obj.name + "a_z" + "," +
                      imusim.Obj.name + "w_x" + "," +
                      imusim.Obj.name + "w_y" + "," +
                      imusim.Obj.name + "w_z" + "," ;
            datas += imusim.A_x.ToString() + "," +
                     imusim.A_y.ToString() + "," +
                     imusim.A_z.ToString() + "," +
                     imusim.W_x.ToString() + "," +
                     imusim.W_y.ToString() + "," +
                     imusim.W_z.ToString() + ",";
            
            // write all datas
            WriteCsv(header, datas, path);
        }

        public void Saveimudata_tocsvnew(IMU_Sim imusim,bool first)
        {
            // set file path and file name
            string path = BinSourcesFolder + imusim.Avater_name + "_" + imusim.Obj.name + "_data.csv";
            // set header and joint datas
            string header = "";
            string datas = "";
            header += imusim.Obj.name + "a_x" + "," +
                      imusim.Obj.name + "a_y" + "," +
                      imusim.Obj.name + "a_z" + "," +
                      imusim.Obj.name + "w_x" + "," +
                      imusim.Obj.name + "w_y" + "," +
                      imusim.Obj.name + "w_z" + ",";
            datas += imusim.A_x.ToString() + "," +
                     imusim.A_y.ToString() + "," +
                     imusim.A_z.ToString() + "," +
                     imusim.W_x.ToString() + "," +
                     imusim.W_y.ToString() + "," +
                     imusim.W_z.ToString() + ",";
            // first csv init
            if(first)
            {
                Csv_Init(header, path);
            }
            // write all datas
            WriteCsvnew(datas, path);
        }
    }
}
