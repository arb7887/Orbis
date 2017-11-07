Shader "WorldShaderV02"
{
	Properties
	{
		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
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

			float rand(float3 co, float3 scale)
			{
				return frac(sin(dot(co.xyz, scale)) * 43758.5453);
			}

			struct appdata
			{
				float4 vertex : POSITION; //vertex position
				//float3 color: COLOR;
				float3 normal : NORMAL;
			};
			
			//v2f: vertex to fragment (things to be sent from v to f)
			struct v2f
			{
				//float2 uv : TEXCOORD0; //texture coordinate
				//SHADOW_COORDS(1) //shadow data into texcoord1
				float4 vertex : SV_POSITION; //clip space position
				float3 normal : NORMAL; //vertex normal
				//float3 avgnormal : NORMAL;
				//fixed3 color : COLOR0;
				//fixed3 randcol : COLOR3;
				//fixed3 randwater : COLOR4;
				//fixed4 diffuse : COLOR1; //diffuse lighting color
				//fixed3 ambient : COLOR2; //ambient lighting color
			};

			sampler2D _MainTex; //texture we are sampling
			float4 _MainTex_ST;
			//float noise;

			//vShader
			v2f vert (appdata v)
			{
				v2f o;

				
				o.vertex = UnityObjectToClipPos(v.vertex);

				half3 worldNormal = UnityObjectToWorldNormal(v.normal); //get vertex normal in world space
				//half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)); //use dot normal and light direction for diffuse

				//o.diffuse = nl * _LightColor0; //factor in light color for diffuse lighting
				//o.diffuse.rgb += ShadeSH9(half4(worldNormal, 1)); //ambient lighting
				//o.ambient = ShadeSH9(half4(worldNormal, 1));
				o.normal = worldNormal;// *displacement; //new normal
				//o.randcol = 0;
				//o.color = 0;
				//o.avgnormal = 0;
				//o.randwater = 0;
				//TRANSFER_SHADOW(o)
				return o;
			}
			//fShader
			fixed4 frag (v2f i) : SV_Target
			{
				//Find color from texture based on normal length
				float y = -0.38f * length(i.normal); // find the poly color based on the average of the 3 normals
				float2 texPos = float2(0, y);
				//if (length(i.avgnormal) > 2.2f) texPos.x = i.randwater;
				float4 newColor = tex2D(_MainTex, texPos);
				
				// sample the texture and return it
				fixed4 color = fixed4(newColor.rgb, 1.0f);

				//fixed shadow = SHADOW_ATTENUATION(i);
				//fixed3 lighting = i.diffuse * shadow + i.ambient;
				//color.rgb *= lighting; //diffuse lighting
				return color;
			}
			ENDCG
		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
