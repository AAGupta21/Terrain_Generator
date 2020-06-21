using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] int Length = 100;
    [SerializeField] int Width = 100;
    [SerializeField] float Amplitude = 5f;
    [SerializeField] float Frequency = 5f;

    [SerializeField] float MaxHeight = 20f;
    [SerializeField] bool IsMaxHeightAmplitude = false;
    [SerializeField] float MinHeight = 0f;

    [SerializeField] private Color[] GroundColorArray = new Color[0];
    [SerializeField] private float[] HeightDetermintOfTerrainArray = new float[0];

    [SerializeField] private GameObject Cube_Prefab = null;

    private float[] ActualValueofTerrainColor = new float[0];

    private bool IsLevelGenerated = false;
    
    private float X_Offset = 0f;
    private float Y_Offset = 0f;

    private void Start()
    {
        if(IsMaxHeightAmplitude)
        {
            MaxHeight = Amplitude;
        }

        ActualValueofTerrainColor = new float[HeightDetermintOfTerrainArray.Length];

        for(int i = 0; i < HeightDetermintOfTerrainArray.Length; i++)
        {
            ActualValueofTerrainColor[i] = (MaxHeight - MinHeight) * HeightDetermintOfTerrainArray[i];
        }

        if(GroundColorArray.Length != HeightDetermintOfTerrainArray.Length)
        {
            Debug.Break();
            Debug.Log("Array Values mismatch!");
        }
    }

    private void Instantiate_LevelField()
    {
        int total_cubes = Length * Width;

        if (!IsLevelGenerated)
        {            
            for (int i = 0; i < total_cubes; i++)
            {
                Instantiate(Cube_Prefab, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f), transform);
            }

            IsLevelGenerated = true;
        }
    }

    public void LevelFieldGenerator()
    {
        Instantiate_LevelField();

        GameObject[] cubeObjects = GameObject.FindGameObjectsWithTag("Cubes");

        for (int x = 0; x < Length; x++)
        {
            for (int y = 0; y < Width; y++)
            {
                int index = x + Length * y;
                cubeObjects[index].transform.position = new Vector3(x, 0f, y);
                cubeObjects[index].GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
    }

    public void Generate_RandomField()
    {
        if(!IsLevelGenerated)
        {
            Instantiate_LevelField();
        }
        
        GameObject[] cubeObjects = GameObject.FindGameObjectsWithTag("Cubes");

        X_Offset = Random.Range(0f, 1000f);
        Y_Offset = Random.Range(0f, 1000f);

        for (int x = 0; x < Length; x++)
        {
            for (int y = 0; y < Width; y++)
            {
                float y_val = Mathf.PerlinNoise(((float)x * Frequency / Length) + X_Offset, ((float)y * Frequency / Width) + Y_Offset);
                
                if(y_val > MaxHeight)
                {
                    y_val = MaxHeight;
                }

                if(y_val < MinHeight)
                {
                    y_val = MinHeight;
                }

                y_val *= Amplitude;

                int index = x + Length * y;
                cubeObjects[index].transform.position = 
                    new Vector3( x, y_val , y);
                cubeObjects[index].GetComponent<MeshRenderer>().material.color = RetColorBasedOnHeight(y_val);
            }
        }
    }

    private Color RetColorBasedOnHeight(float y_val)
    {
        for(int i = 0; i<ActualValueofTerrainColor.Length - 1; i++)
        {
            if(y_val > ActualValueofTerrainColor[i])
            {
                return GroundColorArray[i];
            }
        }

        return GroundColorArray[GroundColorArray.Length - 1];
    }
}