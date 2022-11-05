using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using NUnit.Framework.Internal;
public class ResaveMesh : EditorWindow

{
    static Mesh mesh;
    static ResaveMesh window = null;

    [MenuItem("CustomTools/ResaveMesh")]
    static void Init()
    {
        if (window != null)
        {
            window.Close();
        }

        window = ScriptableObject.CreateInstance(typeof(ResaveMesh)) as ResaveMesh;
        window.ShowUtility();
    }
    void OnGUI()
    {
        int j = 0;
        j += 2;


        if (GUI.Button(new Rect(3, j, position.width - 6, 30), "ResaveMesh!"))
        {
            Resave();
        }
        j += 32;


        if (GUI.Button(new Rect(3, j, position.width - 6, 30), "Test!"))
        {
            Test();
        }
    }

    private void Resave()
    {
        if (!Directory.Exists("Assets/Data/Meshes"))//���� ���������� ���
        {
            Directory.CreateDirectory("Assets//Data/Meshes");//������� ����������
        }

        GameObject SelectedObject = Selection.activeGameObject;
        Mesh meshOriginal = SelectedObject.GetComponent<MeshFilter>().sharedMesh;

        Mesh mesh = new Mesh();
        mesh.name = meshOriginal.name;
        mesh.vertices = meshOriginal.vertices;
        mesh.triangles = meshOriginal.triangles;
        mesh.normals = null;
        mesh.uv = null;
        mesh.colors = null;
        //mesh.normals = meshOriginal.normals;
        //mesh.uv = meshOriginal.uv;
        //mesh.colors = meshOriginal.colors;
        //mesh.boneWeights = meshOriginal.boneWeights;
        //mesh.bindposes = meshOriginal.bindposes;


        AssetDatabase.CreateAsset(mesh, "Assets//Data/Meshes/" + mesh.name + "_clone.asset");//��������� ���
    }

    private void Test()
    {
 
        GameObject SelectedObject = Selection.activeGameObject;
        Mesh meshOriginal = SelectedObject.GetComponent<MeshFilter>().sharedMesh;

        Debug.Log(meshOriginal.normals[0]);

    }
}
