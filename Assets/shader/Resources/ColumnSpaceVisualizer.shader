Shader "Unlit/ColumnSpaceVisualizer"
{
	Properties
	{
		_MainTex ("Texture", 2D)  = "white" {}
		_Color   ("color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex   vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv     : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv     : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4    _MainTex_ST;
			float4    _Color;
			uniform float4x4  myTransformation;
			
			v2f vert (appdata v)
			{
				v2f o;
		
				float4 transferedPos      = mul(unity_ObjectToWorld, v.vertex);
				       transferedPos      = mul(myTransformation, transferedPos);
					   transferedPos.xyz /= transferedPos.w;
				       o.vertex           = mul(UNITY_MATRIX_VP, transferedPos);
				       o.uv               = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col* _Color;
			}
			ENDCG
		}
	}
}
