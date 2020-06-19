using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] int Length = 100;
    [SerializeField] int Width = 100;
    [SerializeField] float frequency = 5f;

    [SerializeField] float MaxHeight = 20f;
    [SerializeField] bool IsMaxHeightSameAsFrequency = false;
    [SerializeField] float MinHeight = 0f;

    [SerializeField] float MinForWhite = 0.9f;
    [SerializeField] float MinForGreen = 0.5f;
    [SerializeField] float MinForBrown = 0.4f;
    [SerializeField] float MinForBlue = 0.2f;

    [SerializeField] private GameObject Cube_Prefab = null;

    private bool IsLevelGenerated = false;

    private float MinWhite_Val = 0f;
    private float MinGreen_Val = 0f;
    private float MinBrown_Val = 0f;
    private float MinBlue_Val = 0f;

    private float X_Offset = 0f;
    private float Y_Offset = 0f;

    private void Start()
    {
        if(IsMaxHeightSameAsFrequency)
        {
            MaxHeight = frequency;
        }

        MinWhite_Val = (MaxHeight - MinHeight) * MinForWhite;
        MinGreen_Val = (MaxHeight - MinHeight) * MinForGreen;
        MinBrown_Val = (MaxHeight - MinHeight) * MinForBrown;
        MinBlue_Val = (MaxHeight - MinHeight) * MinForBlue;
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
                float y_val = Mathf.PerlinNoise((float)x / Length + X_Offset, (float)y / Width + Y_Offset);
                
                if(y_val > MaxHeight)
                {
                    y_val = MaxHeight;
                }

                if(y_val < MinHeight)
                {
                    y_val = MinHeight;
                }

                y_val *= frequency;

                int index = x + Length * y;
                cubeObjects[index].transform.position = 
                    new Vector3( x, y_val , y);
                cubeObjects[index].GetComponent<MeshRenderer>().material.color = RetColorBasedOnHeight(y_val);
            }
        }
    }

    private Color RetColorBasedOnHeight(float y_val)
    {
        if(y_val > MinWhite_Val)
        {
            return Color.white;
        }

        if (y_val > MinGreen_Val)
        {
            return Color.green;
        }

        if (y_val > MinBrown_Val)
        {
            return new Color(0.3301887f, 0.2882467f, 0.2601015f);
        }

        if (y_val > MinBlue_Val)
        {
            return Color.blue;
        }

        return Color.black;
    }
}