using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Message_Panel_Control : MonoBehaviour
{
    public Text message_t;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear_Files()
    {
        Delete_Object_Files("IMUSim");
        Delete_Object_Files("IMUReal");
        Delete_Object_Files("Data_Process");
    }

    private void Delete_Object_Files(string object_name)
    {
        /// <summary>
        /// 功能：删除指定文件夹下面的文件
        /// </summary>
        /// <returns></returns>
        string DeletePath = Application.streamingAssetsPath + "/SourcesFolder/" + object_name;

        if (!Directory.Exists(DeletePath))
        {
            Directory.CreateDirectory(DeletePath);
        }
        try
        {
            DirectoryInfo dir = new DirectoryInfo(DeletePath);
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            foreach (FileSystemInfo item in files)
            {
                if (item is DirectoryInfo)//判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(item.FullName);
                    subdir.Delete(true);//删除子目录和文件
                }
                else
                {
                    File.Delete(item.FullName);//删除指定文件
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }

}
