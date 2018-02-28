using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjModelImporter
{
    private List<Texture2D> _textures = new List<Texture2D>();
    private List<string> _textureFileExtensions = new List<string>() { ".jpg", ".png" };


    public GameObject ImportModel(string absolutePathToModelDirectory)
    {
        DirectoryInfo root = new DirectoryInfo(absolutePathToModelDirectory);
        List<string> texturesPathes = new List<string>();
        string objPath = "";
        string mtlPath = "";

        foreach(var file in root.GetFiles())
        {
            string fileExtension = file.Extension;
            if(_textureFileExtensions.Contains(fileExtension))
            {
                texturesPathes.Add(file.FullName);
                continue;
            }

            if(fileExtension == ".obj" || fileExtension == ".OBJ")
            {
                objPath = file.FullName;
                continue;
            }

            if(fileExtension == ".mtl" || fileExtension == ".MTL")
            {
                mtlPath = file.FullName;
            }
        }

        foreach(var path in texturesPathes)
        {
            byte[] texAsBytes = File.ReadAllBytes(path);
            var texture = new Texture2D(1,1);
            if(texture.LoadImage(texAsBytes))
            {
                _textures.Add(texture);
            }          
        }

        string objString = File.ReadAllText(objPath);
        string mtlString = File.ReadAllText(mtlPath);
        
        return ObjImporter.Import(objString, mtlString, _textures.ToArray());
    }

}
