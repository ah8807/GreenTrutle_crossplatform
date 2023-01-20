using System.IO;
using System.Xml.Linq;

namespace GreenTrutle_crossplatform.tools;

public class Save
{
    private string baseFolder;
    public Save()
    {
        baseFolder = Globals.appDataFilePath + "\\GreenTurtle";
        createFolders();
    }

    public virtual bool CheckIfFileExists(string file)
    {
        return File.Exists(baseFolder + "\\" + file);
    }

    public void createFolders()
    {
        CreateFolder(baseFolder);
    }

    private void CreateFolder(string s)
    {
        DirectoryInfo directory = new DirectoryInfo(s);
        if (!directory.Exists)
        {
            directory.Create();
        }
    }

    public void deleteFile(string file)
    {
        File.Delete(file);
    }

    public XDocument GetFile(string file)
    {
        if (CheckIfFileExists(file))
        {
            return XDocument.Load(baseFolder+"\\"+file);
        }

        return null; 
    }

    public virtual void saveFile(XDocument xml, string path)
    {
        xml.Save(baseFolder+"\\"+path);
    }
}