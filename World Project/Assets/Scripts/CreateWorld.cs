using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateWorld : MonoBehaviour {
    /* #region PerlinNoise
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
    } */

    #region Noise functions

    public static float Noise(float x)
    {
        var X = Mathf.FloorToInt(x) & 0xff;
        x -= Mathf.Floor(x);
        var u = Fade(x);
        return Lerp(u, Grad(perm[X], x), Grad(perm[X + 1], x - 1)) * 2;
    }

    public static float Noise(float x, float y)
    {
        var X = Mathf.FloorToInt(x) & 0xff;
        var Y = Mathf.FloorToInt(y) & 0xff;
        x -= Mathf.Floor(x);
        y -= Mathf.Floor(y);
        var u = Fade(x);
        var v = Fade(y);
        var A = (perm[X] + Y) & 0xff;
        var B = (perm[X + 1] + Y) & 0xff;
        return Lerp(v, Lerp(u, Grad(perm[A], x, y), Grad(perm[B], x - 1, y)),
                       Lerp(u, Grad(perm[A + 1], x, y - 1), Grad(perm[B + 1], x - 1, y - 1)));
    }

    public static float Noise(Vector2 coord)
    {
        return Noise(coord.x, coord.y);
    }

    public static float Noise(float x, float y, float z)
    {
        var X = Mathf.FloorToInt(x) & 0xff;
        var Y = Mathf.FloorToInt(y) & 0xff;
        var Z = Mathf.FloorToInt(z) & 0xff;
        x -= Mathf.Floor(x);
        y -= Mathf.Floor(y);
        z -= Mathf.Floor(z);
        var u = Fade(x);
        var v = Fade(y);
        var w = Fade(z);
        var A = (perm[X] + Y) & 0xff;
        var B = (perm[X + 1] + Y) & 0xff;
        var AA = (perm[A] + Z) & 0xff;
        var BA = (perm[B] + Z) & 0xff;
        var AB = (perm[A + 1] + Z) & 0xff;
        var BB = (perm[B + 1] + Z) & 0xff;
        return Lerp(w, Lerp(v, Lerp(u, Grad(perm[AA], x, y, z), Grad(perm[BA], x - 1, y, z)),
                               Lerp(u, Grad(perm[AB], x, y - 1, z), Grad(perm[BB], x - 1, y - 1, z))),
                       Lerp(v, Lerp(u, Grad(perm[AA + 1], x, y, z - 1), Grad(perm[BA + 1], x - 1, y, z - 1)),
                               Lerp(u, Grad(perm[AB + 1], x, y - 1, z - 1), Grad(perm[BB + 1], x - 1, y - 1, z - 1))));
    }

    public static float Noise(Vector3 coord)
    {
        return Noise(coord.x, coord.y, coord.z);
    }

    #endregion

    #region fBm functions

    public static float Fbm(float x, int octave)
    {
        var f = 0.0f;
        var w = 0.5f;
        for (var i = 0; i < octave; i++)
        {
            f += w * Noise(x);
            x *= 2.0f;
            w *= 0.5f;
        }
        return f;
    }

    public static float Fbm(Vector2 coord, int octave)
    {
        var f = 0.0f;
        var w = 0.5f;
        for (var i = 0; i < octave; i++)
        {
            f += w * Noise(coord);
            coord *= 2.0f;
            w *= 0.5f;
        }
        return f;
    }

    public static float Fbm(float x, float y, int octave)
    {
        return Fbm(new Vector2(x, y), octave);
    }

    public static float Fbm(Vector3 coord, int octave)
    {
        var f = 0.0f;
        var w = 0.5f;
        for (var i = 0; i < octave; i++)
        {
            f += w * Noise(coord);
            coord *= 2.0f;
            w *= 0.5f;
        }
        return f;
    }

    public static float Fbm(float x, float y, float z, int octave)
    {
        return Fbm(new Vector3(x, y, z), octave);
    }

    #endregion

    #region Private functions

    static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    static float Lerp(float t, float a, float b)
    {
        return a + t * (b - a);
    }

    static float Grad(int hash, float x)
    {
        return (hash & 1) == 0 ? x : -x;
    }

    static float Grad(int hash, float x, float y)
    {
        return ((hash & 1) == 0 ? x : -x) + ((hash & 2) == 0 ? y : -y);
    }

    static float Grad(int hash, float x, float y, float z)
    {
        var h = hash & 15;
        var u = h < 8 ? x : y;
        var v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }

    static int[] perm = {
        151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
        151
    };

    #endregion
    //turbulence method, used to create noise
    float turbulence(Vector3 p)
    {
        float t = -.5f;

        for (float i = 1.0f; i <= 10.0f; i++)
        {
            float power = Mathf.Pow(3.0f, i);
            t += Mathf.Abs(Noise((p * power)) / power);
        }
        return t;
    }
    public GameObject World;
    private float noiseshift = 1.5f; //noise seed to shift the noise map
    // Use this for initialization
    void Start () {
        /*Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Editor/OurWorld.asset", typeof(Mesh));
        if (mesh == null)
        {
            GenWorld();
        }
        else World.GetComponent<MeshFilter>().mesh = mesh; */
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
            float noise = -2.0f * turbulence(noiseshift * normals[i]); // turbulent noise 
            float posnoise = 4.0f * Noise(0.1f * vertices[i]);
            float displacement = -5f * noise + posnoise;
             if (displacement < -3.0f)
            {
                displacement = -3.0f;
            }
            vertices[i] = vertices[i] + normals[i] * displacement;
        }
        WorldMesh.vertices = vertices;
        WorldMesh.RecalculateBounds();
        World.GetComponent<MeshFilter>().mesh = WorldMesh;
        MeshUtility.Optimize(WorldMesh);

        //AssetDatabase.CreateAsset(WorldMesh, "Assets/Editor/OurWorld.asset");
        //AssetDatabase.SaveAssets();
    }
}
