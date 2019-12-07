using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrices : MonoBehaviour {

    public enum MatricesType
    {
        Identity, ScaleUniform, ScaleAxis, Mirror,
        NormalBasis, OrthogonalBasis, OrthogonalMatrix,
        Rotation, Translation, 
        projection

    };
    
    public MatricesType matrixType;

    
	
	// Update is called once per frame
	void Update () {

        Matrix4x4 objectToWorld = Matrix4x4.identity;

        switch (matrixType)
        {
            case MatricesType.Identity:

                objectToWorld = CreateMatrix(

                    1f, 0f, 0f, 0f,
                    0f, 1f, 0f, 0f,
                    0f, 0f, 1f, 0f,
                    0f, 0f, 0f, 1f
                );
                break;

            case MatricesType.ScaleUniform:

                 objectToWorld = CreateMatrix(

                    2f, 0f, 0f, 0f,
                    0f, 2f, 0f, 0f,
                    0f, 0f, 2f, 0f,
                    0f, 0f, 0f, 1f
                );

                break;
            
            case MatricesType.ScaleAxis:

                 objectToWorld = CreateMatrix(

                    3f, 0f, 0f, 0f,
                    0f, 1f, 0f, 0f,
                    0f, 0f, 1f, 0f,
                    0f, 0f, 0f, 1f
                );

                break;
            case MatricesType.Mirror:

                 objectToWorld = CreateMatrix(

                   -1f, 0f, 0f, 0f,
                    0f, 1f, 0f, 0f,
                    0f, 0f, 1f, 0f,
                    0f, 0f, 0f, 1f
                );

                break;
            case MatricesType.NormalBasis:
                float  a = 1f / Mathf.Sqrt(2f);
                 objectToWorld = CreateMatrix(
                
                     a, 0f, 0f, 0f,
                     a, 1f, 0f, 0f,
                    0f, 0f, 1f, 0f,
                    0f, 0f, 0f, 1f
                );
                break;

            case MatricesType.OrthogonalBasis:

                

                objectToWorld = CreateMatrix(
                
                    1f,   1f,   1f, 0f,
                   -1f,   1f,   1f, 0f,
                    0f,  -2f,   1f, 0f,
                    0f,   0f,   0f, 1f
                );
                break;

            case MatricesType.OrthogonalMatrix:

                // columnOne   = new Vector3( 1f,  1f,  2f) /      2f;
                // columnTwo   = new Vector3( 1f,  1f, -1f) / sqrt(3);
                // columnThree = new Vector3(-3f, -3f,  0f) / sqrt(6);

                float n1 = Mathf.Sqrt(2f);
                float n2 = Mathf.Sqrt(6f);
                float n3 = Mathf.Sqrt(3f);

                objectToWorld = CreateMatrix(
                
                    1f/n1,   1f/n2,   1f/n3, 0f,
                   -1f/n1,   1f/n2,   1f/n3, 0f,
                    0f,     -2f/n2,   1f/n3, 0f,
                    0f,         0f,      0f, 1f
                );
                break;

            case MatricesType.Rotation:

                // Rotation around X axis

                float theta = 45f;

                objectToWorld = CreateMatrix(

                    1f,                0f,                0f, 0f,
                    0f,  Mathf.Cos(theta), -Mathf.Sin(theta), 0f,
                    0f,  Mathf.Sin(theta),  Mathf.Cos(theta), 0f,
                    0f,                0f,                0f, 1f
                );
                break;

            case MatricesType.Translation:
                

                objectToWorld = CreateMatrix(

                    1f, 0f, 0f, 6f,
                    0f, 1f, 0f, 0f,
                    0f, 0f, 1f, 0f,
                    0f, 0f, 0f, 1f
                );
                break;

            case MatricesType.projection:

                float fov = 90;
                float s   = 1f /                 Mathf.Tan(fov * Mathf.PI 
                                                           /(2f * 180f));
                float f =  3f;
                float n = 0.1f;


                objectToWorld = CreateMatrix(

                    s,  0f,       0f,         1f,
                    0f,  s,       0f,         0f,
                    0f, 0f, -f/(f-n), -f*n/(f-n),
                    0f, 0f,      -1f,         0f
                );
                break;
        }


        float v = Mathf.Sin(Time.time) * 0.5f + 0.5f;

        Matrix4x4 toSet = lerpMatrices(Matrix4x4.identity, objectToWorld, v);

        Shader.SetGlobalMatrix("myTransformation", toSet);


	}

    Matrix4x4 lerpMatrices(Matrix4x4 one, Matrix4x4 two, float value)
    {
        value = Mathf.Clamp01(value);


        Vector4 c1 = Vector4.Lerp( one.GetColumn(0), two.GetColumn(0), value);
        Vector4 c2 = Vector4.Lerp( one.GetColumn(1), two.GetColumn(1), value);
        Vector4 c3 = Vector4.Lerp( one.GetColumn(2), two.GetColumn(2), value);
        Vector4 c4 = Vector4.Lerp( one.GetColumn(3), two.GetColumn(3), value);

        return new Matrix4x4(c1, c2, c3, c4);

    }


    Matrix4x4 CreateMatrix( float a11, float a12, float a13, float a14,
                            float a21, float a22, float a23, float a24,
                            float a31, float a32, float a33, float a34,
                            float a41, float a42, float a43, float a44)
    {
        Vector4 column1 = new Vector4(a11, a21, a31, a41);
        Vector4 column2 = new Vector4(a12, a22, a32, a42);
        Vector4 column3 = new Vector4(a13, a23, a33, a43);
        Vector4 column4 = new Vector4(a14, a24, a34, a44);

        return new Matrix4x4(column1, column2, column3, column4);
    }
}
