Shader "WorldShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			Tags {"LightMode"="ForwardBase"}
			CGPROGRAM
			#pragma vertex vert //use vert function as the vertex shader
			#pragma fragment frag //use frag function as the fragment shader
			#pragma enable_d3d11_debug_symbols
			// make fog work
			//#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			//start of Ashima 3D noise code:
			float3 mod(float3 x, float3 y)
			{
				return x - y * floor(x / y);
			}

			float3 mod289(float3 x)
			{
				return x - floor(x / 289.0) * 289.0;
			}

			float4 mod289(float4 x)
			{
				return x - floor(x / 289.0) * 289.0;
			}

			float4 permute(float4 x)
			{
				return mod289(((x*34.0) + 1.0)*x);
			}

			float4 taylorInvSqrt(float4 r)
			{
				return (float4)1.79284291400159 - r * 0.85373472095314;
			}

			float3 fade(float3 t) {
				return t*t*t*(t*(t*6.0 - 15.0) + 10.0);
			}

			// Classic Perlin noise
			float cnoise(float3 P)
			{
				float3 Pi0 = floor(P); // Integer part for indexing
				float3 Pi1 = Pi0 + (float3)1.0; // Integer part + 1
				Pi0 = mod289(Pi0);
				Pi1 = mod289(Pi1);
				float3 Pf0 = frac(P); // Fractional part for interpolation
				float3 Pf1 = Pf0 - (float3)1.0; // Fractional part - 1.0
				float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
				float4 iy = float4(Pi0.y, Pi0.y, Pi1.y, Pi1.y);
				float4 iz0 = (float4)Pi0.z;
				float4 iz1 = (float4)Pi1.z;

				float4 ixy = permute(permute(ix) + iy);
				float4 ixy0 = permute(ixy + iz0);
				float4 ixy1 = permute(ixy + iz1);

				float4 gx0 = ixy0 / 7.0;
				float4 gy0 = frac(floor(gx0) / 7.0) - 0.5;
				gx0 = frac(gx0);
				float4 gz0 = (float4)0.5 - abs(gx0) - abs(gy0);
				float4 sz0 = step(gz0, (float4)0.0);
				gx0 -= sz0 * (step((float4)0.0, gx0) - 0.5);
				gy0 -= sz0 * (step((float4)0.0, gy0) - 0.5);

				float4 gx1 = ixy1 / 7.0;
				float4 gy1 = frac(floor(gx1) / 7.0) - 0.5;
				gx1 = frac(gx1);
				float4 gz1 = (float4)0.5 - abs(gx1) - abs(gy1);
				float4 sz1 = step(gz1, (float4)0.0);
				gx1 -= sz1 * (step((float4)0.0, gx1) - 0.5);
				gy1 -= sz1 * (step((float4)0.0, gy1) - 0.5);

				float3 g000 = float3(gx0.x,gy0.x,gz0.x);
				float3 g100 = float3(gx0.y,gy0.y,gz0.y);
				float3 g010 = float3(gx0.z,gy0.z,gz0.z);
				float3 g110 = float3(gx0.w,gy0.w,gz0.w);
				float3 g001 = float3(gx1.x,gy1.x,gz1.x);
				float3 g101 = float3(gx1.y,gy1.y,gz1.y);
				float3 g011 = float3(gx1.z,gy1.z,gz1.z);
				float3 g111 = float3(gx1.w,gy1.w,gz1.w);

				float4 norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
				g000 *= norm0.x;
				g010 *= norm0.y;
				g100 *= norm0.z;
				g110 *= norm0.w;

				float4 norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
				g001 *= norm1.x;
				g011 *= norm1.y;
				g101 *= norm1.z;
				g111 *= norm1.w;

				float n000 = dot(g000, Pf0);
				float n100 = dot(g100, float3(Pf1.x, Pf0.y, Pf0.z));
				float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
				float n110 = dot(g110, float3(Pf1.x, Pf1.y, Pf0.z));
				float n001 = dot(g001, float3(Pf0.x, Pf0.y, Pf1.z));
				float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
				float n011 = dot(g011, float3(Pf0.x, Pf1.y, Pf1.z));
				float n111 = dot(g111, Pf1);

				float3 fade_xyz = fade(Pf0);
				float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
				float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
				float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
				return 2.2 * n_xyz;
			}

			// Classic Perlin noise, periodic variant
			float pnoise(float3 P, float3 rep)
			{
				float3 Pi0 = mod(floor(P), rep); // Integer part, modulo period
				float3 Pi1 = mod(Pi0 + (float3)1.0, rep); // Integer part + 1, mod period
				Pi0 = mod289(Pi0);
				Pi1 = mod289(Pi1);
				float3 Pf0 = frac(P); // Fractional part for interpolation
				float3 Pf1 = Pf0 - (float3)1.0; // Fractional part - 1.0
				float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
				float4 iy = float4(Pi0.y, Pi0.y, Pi1.y, Pi1.y);
				float4 iz0 = (float4)Pi0.z;
				float4 iz1 = (float4)Pi1.z;

				float4 ixy = permute(permute(ix) + iy);
				float4 ixy0 = permute(ixy + iz0);
				float4 ixy1 = permute(ixy + iz1);

				float4 gx0 = ixy0 / 7.0;
				float4 gy0 = frac(floor(gx0) / 7.0) - 0.5;
				gx0 = frac(gx0);
				float4 gz0 = (float4)0.5 - abs(gx0) - abs(gy0);
				float4 sz0 = step(gz0, (float4)0.0);
				gx0 -= sz0 * (step((float4)0.0, gx0) - 0.5);
				gy0 -= sz0 * (step((float4)0.0, gy0) - 0.5);

				float4 gx1 = ixy1 / 7.0;
				float4 gy1 = frac(floor(gx1) / 7.0) - 0.5;
				gx1 = frac(gx1);
				float4 gz1 = (float4)0.5 - abs(gx1) - abs(gy1);
				float4 sz1 = step(gz1, (float4)0.0);
				gx1 -= sz1 * (step((float4)0.0, gx1) - 0.5);
				gy1 -= sz1 * (step((float4)0.0, gy1) - 0.5);

				float3 g000 = float3(gx0.x,gy0.x,gz0.x);
				float3 g100 = float3(gx0.y,gy0.y,gz0.y);
				float3 g010 = float3(gx0.z,gy0.z,gz0.z);
				float3 g110 = float3(gx0.w,gy0.w,gz0.w);
				float3 g001 = float3(gx1.x,gy1.x,gz1.x);
				float3 g101 = float3(gx1.y,gy1.y,gz1.y);
				float3 g011 = float3(gx1.z,gy1.z,gz1.z);
				float3 g111 = float3(gx1.w,gy1.w,gz1.w);

				float4 norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
				g000 *= norm0.x;
				g010 *= norm0.y;
				g100 *= norm0.z;
				g110 *= norm0.w;
				float4 norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
				g001 *= norm1.x;
				g011 *= norm1.y;
				g101 *= norm1.z;
				g111 *= norm1.w;

				float n000 = dot(g000, Pf0);
				float n100 = dot(g100, float3(Pf1.x, Pf0.y, Pf0.z));
				float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
				float n110 = dot(g110, float3(Pf1.x, Pf1.y, Pf0.z));
				float n001 = dot(g001, float3(Pf0.x, Pf0.y, Pf1.z));
				float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
				float n011 = dot(g011, float3(Pf0.x, Pf1.y, Pf1.z));
				float n111 = dot(g111, Pf1);

				float3 fade_xyz = fade(Pf0);
				float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
				float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
				float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
				return 2.2 * n_xyz;
			}
			//end of Ashima 3D noise code

			//turbulence method, used to create noise
			float turbulence(float3 p)
			{
				float t = -.5f;

				for (float i = 1.0f; i <= 10.0f; i++)
				{
					float power = pow(2.0f, i);
					t += abs(pnoise(float3(power * p), float3(10.0f, 10.0f, 10.0f)) / power);
				}
				return t;
			}

			float rand(float3 co, float3 scale)
			{
				return frac(sin(dot(co.xyz, scale)) * 43758.5453);
			}

			struct appdata
			{
				float4 vertex : POSITION; //vertex position
				float2 uv : TEXCOORD0; //texture coordinate
				float3 normal : NORMAL;
			};
			
			//v2f: vertex to fragment (things to be sent from v to f)
			struct v2f
			{
				float2 uv : TEXCOORD0; //texture coordinate
				float4 vertex : SV_POSITION; //clip space position
				float3 normal : NORMAL; //vertex normal
				fixed4 diffuse : COLOR0; //diffuse lighting color
			};

			sampler2D _MainTex; //texture we are sampling
			float4 _MainTex_ST;
			float noise;

			//vShader
			v2f vert (appdata v)
			{
				v2f o;

				//changing verticies for terraformation
				float noiseshift = 0.5f; //noise seed to shift the noise map
				noise = -0.7f * turbulence(noiseshift * v.normal); // turbulent noise 
				float posnoise = 2.0 * pnoise(0.04 * v.vertex, float3(100.0f, 100.0f, 100.0f));
				float displacement = -10.0f * noise + posnoise;
				if (displacement < -2.5f)
				{
					displacement = -2.5f;
				}
				float3 newVertex = v.vertex + v.normal * displacement;

				//transform position to clip space (multiply with Model*View*Projection matrix)
				o.vertex = UnityObjectToClipPos(float4(newVertex, 1.0f));

				half3 worldNormal = UnityObjectToWorldNormal(v.normal); //get vertex normal in world space
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)); //use dot normal and light direction for diffuse

				o.uv = v.uv;
				o.diffuse = nl * _LightColor0; //factor in light color for diffuse lighting
				o.diffuse.rgb += ShadeSH9(half4(worldNormal, 1)); //ambient lighting
				o.normal = v.normal * displacement; //new normal
				return o;
			}
			
			//fShader
			fixed4 frag (v2f i) : SV_Target
			{
				//Find color from texture based on normal length
				//float r = .01 * rand(i.vertex, float3(12.9898, 78.233, 151.7182)); //add a bit of randomness
				float y = -0.38f * length(i.normal);// + r;
				float2 texPos = float2(0, y);
				float4 newColor = tex2D(_MainTex, texPos);

				// sample the texture and return it
				fixed4 color = fixed4(newColor.rgb, 1.0f);
				//color *= i.diffuse; //diffuse lighting
				return color;
			}
			ENDCG
		}
	}
}
