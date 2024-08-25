using System;
using System.Runtime.InteropServices;

/*����C#���÷��йܵ�C/C++��DLL��һ�ֶ��嶨��ṹ��ķ�ʽ����Ҫ��Ϊ���ڴ�������LayoutKind����������Sequential��Explicit
Sequential��ʾ˳��洢���ṹ�����������ڴ��ж���˳���ŵ�Explicit��ʾ��ȷ���֣���Ҫ��FieldOffset()����ÿ����Ա��λ���ⶼ��
Ϊ��ʹ�÷��йܵ�ָ��׼���ģ�CharSet=CharSet.Ansi��ʾ���뷽ʽ
*/
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct OpenFileName
{
    public int structSize;       //�ṹ���ڴ��С
    public IntPtr dlgOwner;       //���öԻ���ľ��
    public IntPtr instance;       //����flags��־�����ã�ȷ��instance��˭�ľ���������������
    public string filter;         //��ȡ�ļ��Ĺ��˷�ʽ
    public string customFilter;  //һ����̬������ ���������û�ѡ���ɸѡ��ģʽ
    public int maxCustFilter;     //�������Ĵ�С
    public int filterIndex;                 //ָ��Ļ���������������������ַ�����
    public string file;                  //�洢��ȡ�ļ�·��
    public int maxFile;                     //�洢��ȡ�ļ�·������󳤶� ����256
    public string fileTitle;             //��ȡ���ļ�������չ��
    public int maxFileTitle;                //��ȡ�ļ�����󳤶�
    public string initialDir;            //���Ŀ¼
    public string title;                 //�򿪴��ڵ�����
    public int flags;                       //��ʼ���Ի����һ��λ��־  �������ͺ����ò��Ĺٷ�API
    public short fileOffset;                //�ļ���ǰ�ĳ���
    public short fileExtension;             //��չ��ǰ�ĳ���
    public string defExt;                //Ĭ�ϵ���չ��
    public IntPtr custData;       //���ݸ�lpfnHook��Ա��ʶ�Ĺ����ӳ̵�Ӧ�ó����������
    public IntPtr hook;           //ָ���ӵ�ָ�롣����Flags��Ա����OFN_ENABLEHOOK��־������ó�Ա�������ԡ�
    public string templateName;          //ģ������hInstance��Ա��ʶ�ĶԻ���ģ����Դ������
    public IntPtr reservedPtr;
    public int reservedInt;
    public int flagsEx;                     //�����ڳ�ʼ���Ի����һ��λ��־
}

public class WindowDll
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
}

public class SaveDll
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);

}
