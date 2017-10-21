using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorld : MonoBehaviour {
    #region PerlinNoise
    //Andrew Baron
    //Ashima 3D Noise - Unity C# Conversion
    Vector3 mul(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
    Vector4 mul(Vector4 a, Vector4 b)
    {
        return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    }

    Vector3 add(Vector3 a, float b)
    {
        return new Vector3(a.x + b, a.y + b, a.z + b);
    }
    Vector4 add(Vector4 a, float b)
    {
        return new Vector4(a.x + b, a.y + b, a.z + b, a.w + b);
    }

    Vector3 sub(Vector3 a, float b)
    {
        return new Vector3(a.x - b, a.y - b, a.z - b);
    }
    Vector4 sub(Vector4 a, float b)
    {
        return new Vector4(a.x - b, a.y - b, a.z - b, a.w - b);
    }

    Vector3 div(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    Vector3 floor(Vector3 a)
    {
        return new Vector3(Mathf.Floor(a.x), Mathf.Floor(a.y), Mathf.Floor(a.z));
    }
    Vector4 floor(Vector4 a)
    {
        return new Vector4(Mathf.Floor(a.x), Mathf.Floor(a.y), Mathf.Floor(a.z), Mathf.Floor(a.w));
    }

    Vector3 frac(Vector3 a)
    {
        return floor(a) - a;
    }

    float dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    Vector3 abs(Vector3 a)
    {
        return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    }
    Vector4 abs(Vector4 a)
    {
        return new Vector4(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z), Mathf.Abs(a.w));
    }

    Vector4 step(Vector4 edge, Vector4 x)
    {
        Vector4 result = new Vector4(0.0f, 0.0f);
        if (x.x < edge.x) result.x = 0.0f;
        else result.x = 1.0f;
        if (x.y < edge.y) result.y = 0.0f;
        else result.y = 1.0f;
        if (x.z < edge.z) result.z = 0.0f;
        else result.z = 1.0f;
        if (x.w < edge.w) result.w = 0.0f;
        else result.w = 1.0f;
        return result;
    }

    //start of Ashima 3D noise code:
    Vector3 mod(Vector3 x, Vector3 y)
    {
        return x - mul(y, floor(div(x, y)));
    }

    Vector3 mod289(Vector3 x)
    {
        return x - floor(x / 289.0f) * 289.0f;
    }

    Vector4 mod289(Vector4 x)
    {
        return x - floor(x / 289.0f) * 289.0f;
    }

    Vector4 permute(Vector4 x)
    {
        return mod289(mul(add((x * 34.0f), 1.0f), x));
    }

    Vector4 taylorInvSqrt(Vector4 r)
    {
        return new Vector4(1.79284291400159f, 1.79284291400159f, 1.79284291400159f, 1.79284291400159f) - r * 0.85373472095314f;
    }

    Vector3 fade(Vector3 t)
    {
        return mul(t, mul(t, mul(t, (mul(t, (add(sub(t * 6.0f, 15.0f), 10.0f)))))));
    }
    // Classic Perlin noise, periodic variant
    float pnoise(Vector3 P, Vector3 rep)
    {
        Vector3 Pi0 = mod(floor(P), rep); // Integer part, modulo period
        Vector3 Pi1 = mod(Pi0 + new Vector3(1.0f, 1.0f, 1.0f), rep); // Integer part + 1, mod period
        Pi0 = mod289(Pi0);
        Pi1 = mod289(Pi1);
        Vector3 Pf0 = frac(P); // Fractional part for interpolation
        Vector3 Pf1 = Pf0 - new Vector3(1.0f, 1.0f, 1.0f); // Fractional part - 1.0
        Vector4 ix = new Vector4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
        Vector4 iy = new Vector4(Pi0.y, Pi0.y, Pi1.y, Pi1.y);
        Vector4 iz0 = new Vector4(Pi0.z, Pi0.z, Pi0.z, Pi0.z);
        Vector4 iz1 = new Vector4(Pi1.z, Pi1.z, Pi1.z, Pi1.z);

        Vector4 ixy = permute(permute(ix) + iy);
        Vector4 ixy0 = permute(ixy + iz0);
        Vector4 ixy1 = permute(ixy + iz1);

        Vector4 gx0 = ixy0 / 7.0f;
        Vector4 gy0 = sub(frac(floor(gx0) / 7.0f), 0.5f);
        gx0 = frac(gx0);
        Vector4 gz0 = new Vector4(0.5f, 0.5f, 0.5f, 0.5f) - abs(gx0) - abs(gy0);
        Vector4 sz0 = step(gz0, new Vector4(0.0f, 0.0f));
        gx0 -= mul(sz0, (sub(step(new Vector4(0.0f, 0.0f), gx0), 0.5f)));
        gy0 -= mul(sz0, (sub(step(new Vector4(0.0f, 0.0f), gy0), 0.5f)));

        Vector4 gx1 = ixy1 / 7.0f;
        Vector4 gy1 = sub(frac(floor(gx1) / 7.0f), 0.5f);
        gx1 = frac(gx1);
        Vector4 gz1 = new Vector4(0.5f, 0.5f, 0.5f, 0.5f) - abs(gx1) - abs(gy1);
        Vector4 sz1 = step(gz1, new Vector4(0.0f, 0.0f));
        gx1 -= mul(sz1, (sub(step(new Vector4(0.0f, 0.0f), gx1), 0.5f)));
        gy1 -= mul(sz1, (sub(step(new Vector4(0.0f, 0.0f), gy1), 0.5f)));

        Vector3 g000 = new Vector3(gx0.x, gy0.x, gz0.x);
        Vector3 g100 = new Vector3(gx0.y, gy0.y, gz0.y);
        Vector3 g010 = new Vector3(gx0.z, gy0.z, gz0.z);
        Vector3 g110 = new Vector3(gx0.w, gy0.w, gz0.w);
        Vector3 g001 = new Vector3(gx1.x, gy1.x, gz1.x);
        Vector3 g101 = new Vector3(gx1.y, gy1.y, gz1.y);
        Vector3 g011 = new Vector3(gx1.z, gy1.z, gz1.z);
        Vector3 g111 = new Vector3(gx1.w, gy1.w, gz1.w);

        Vector4 norm0 = taylorInvSqrt(new Vector4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
        g000 *= norm0.x;
        g010 *= norm0.y;
        g100 *= norm0.z;
        g110 *= norm0.w;
        Vector4 norm1 = taylorInvSqrt(new Vector4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
        g001 *= norm1.x;
        g011 *= norm1.y;
        g101 *= norm1.z;
        g111 *= norm1.w;

        float n000 = dot(g000, Pf0);
        float n100 = dot(g100, new Vector3(Pf1.x, Pf0.y, Pf0.z));
        float n010 = dot(g010, new Vector3(Pf0.x, Pf1.y, Pf0.z));
        float n110 = dot(g110, new Vector3(Pf1.x, Pf1.y, Pf0.z));
        float n001 = dot(g001, new Vector3(Pf0.x, Pf0.y, Pf1.z));
        float n101 = dot(g101, new Vector3(Pf1.x, Pf0.y, Pf1.z));
        float n011 = dot(g011, new Vector3(Pf0.x, Pf1.y, Pf1.z));
        float n111 = dot(g111, Pf1);

        Vector3 fade_xyz = fade(Pf0);
        Vector4 n_z = Vector4.Lerp(new Vector4(n000, n100, n010, n110), new Vector4(n001, n101, n011, n111), fade_xyz.z);
        Vector2 n_yz = Vector2.Lerp(new Vector2(n_z.x, n_z.y), new Vector2(n_z.z, n_z.w), fade_xyz.y);
        float n_xyz = Mathf.Lerp(n_yz.x, n_yz.y, fade_xyz.x);
        return 2.2f * n_xyz;
    }
    //turbulence method, used to create noise
    float turbulence(Vector3 p)
    {
        float t = -.5f;

        for (float i = 1.0f; i <= 10.0f; i++)
        {
            float power = Mathf.Pow(2.0f, i);
            t += Mathf.Abs(pnoise((p * power), new Vector3(10.0f, 10.0f, 10.0f)) / power);
        }
        return t;
    }
    #endregion
    public GameObject World;
    private float noiseshift = 0.5f; //noise seed to shift the noise map
    // Use this for initialization
    void Start () {
        GenWorld();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void GenWorld()
    {
        Mesh WorldMesh = World.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = WorldMesh.vertices;
        Vector3[] normals = WorldMesh.normals;
        for(int i = 0; i < vertices.Length; i++)
        {
            //changing verticies for terraformation
            float noise = -0.7f * turbulence(noiseshift * normals[i]); // turbulent noise 
            float posnoise = 2.0f * pnoise(0.04f * vertices[i], new Vector3(100.0f, 100.0f, 100.0f));
            float displacement = -1.0f * noise + posnoise;
            if (displacement < -2.5f)
            {
                displacement = -2.5f;
            }
            vertices[i] = vertices[i] + normals[i] * displacement;
        }
        WorldMesh.vertices = vertices;
        WorldMesh.RecalculateBounds();
        World.GetComponent<MeshFilter>().mesh = WorldMesh;

    }
}
