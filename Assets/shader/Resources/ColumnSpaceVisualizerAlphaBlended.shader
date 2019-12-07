Shader "Unlit/ColumnSpaceVisualizerAlphaBlended"
{
	Properties
	{
		_MainTex ("Texture", 2D)  = "white" {}
		_Color   ("color", Color) = (1, 1, 1, 1)
		
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		ZWrite Off
		Cull   Off
		Blend One OneMinusSrcAlpha // Premultiplied transparency

		LOD 100

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
			float4x4  myTransformation;
			
			v2f vert (appdata v)
			{
				v2f o;
		
				float4 transferedPos      = mul(unity_ObjectToWorld, v.vertex);
				       transferedPos      = mul(myTransformation, transferedPos);
				       o.vertex           = mul(UNITY_MATRIX_VP, transferedPos);
				       o.uv               = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv );
			    col *= _Color;
				return col* col.a;
			}
			ENDCG
		}
	}
}
