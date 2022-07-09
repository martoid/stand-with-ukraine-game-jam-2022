Shader "Sprites/NewUnlitShader"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Fade("FadeDistance", Float) = 0 
    }
    SubShader
    {
        Tags { 	
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 color    : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };


			fixed4 _Color;
            sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
			float _Fade;

			float pretty_sine(float2 uv)
			{
				float end = 1.0f;// pow((1 - abs((uv.y - 0.5f) * 2)), 2);
				float d = (sin(uv.x * 2) - uv.y);
				float d1 = 4.0 * d;
				float d2 = 16.0 * d;
				float sine = sin(uv.x + _Time.y) ;
				float s = lerp(d1, d2, sine + 0.5);
				float result = 1.0 + 0.75 * (-sine + 0.5) - abs(s);
				return result  ;
			}

			struct Pretty_Sine_Data
			{
				float2 uv;
				float3 color;
				float multiplier;
			};

			float4 edge(float2 uv, float2 p0, float2 p1)
			{
				float2 diff = p1 - p0;
				float2 n_diff = normalize(diff);
				float half_diff_length = 0.5 * length(diff);
				float2 uv_x = float2(1.0, 0.0);
				float2 uv_y = float2(0.0, 1.0);

				float cos_a = dot(uv_x, n_diff);
				float sin_a = dot(uv_y, n_diff);
				float2 p = float2(cos_a * uv.x + sin_a * uv.y,
					-sin_a * uv.x + cos_a * uv.y);

				float2 offset = 0.5 * (p0 + p1);
				float2 xn = cos_a * uv_x + sin_a * uv_y;
				float2 yn = -sin_a * uv_x + cos_a * uv_y;

				float2 new_offset = float2(dot(offset, xn), dot(offset, yn));

				p -= new_offset;

				Pretty_Sine_Data data[6];

				data[0].uv =  1.0f * float2(p.x - _Time.y * 0.35f,  p.y);
				data[0].color = float3(0.15, 0.7, 0.1); 
				data[0].multiplier = 0.4f* _Fade;

				data[1].uv = 1.5f *  float2(p.x - _Time.y* 0.5f,  p.y);
				data[1].color = float3(0.7, 0.2, 0.15);
				data[1].multiplier = 0.45f* _Fade;

				data[2].uv = 1.75f *float2(p.x + _Time.y* 0.55f, p.y);
				data[2].color = float3(0.3, 0.7, 1.0);
				data[2].multiplier = 0.5f* _Fade;

				data[3].uv = 2.0f * float2(p.x + _Time.y * 0.65f, (p.y));
				data[3].color = float3(0.4, 0.3, 0.7);
				data[3].multiplier = 0.55f* _Fade;

				data[4].uv = 2.25f * float2((p.x + _Time.y* 0.75f), (p.y));
				data[4].color = float3(1.0f, 0.5f, 2.0f);
				data[4].multiplier = 0.6f* _Fade;

				data[5].uv = 2.5f * float2(p.x - _Time.y* 1.0f,  p.y);
				data[5].color = float3(0.7, 0.2, 0.15);
				data[5].multiplier = 0.65f * _Fade;

				float output_dist = 0.2f;
				float3 output_color = float3(0.0, 0.0, 0.0);

				for (int i = 0; i < 6; ++i)
				{
					float d = pretty_sine(data[i].uv);
					float limit = (p.x < -half_diff_length || p.x > half_diff_length) ? 0.0 : 1.0;
					float cd = limit * data[i].multiplier * clamp(d, 0.0, 1.0);
					output_color += cd * data[i].color;
					output_dist += cd;
				}
				return float4(output_color, 0.0f);
			}

			float edge_f(float2 p, float2 e0, float2 e1)
			{
				float result = (e0.y - e1.y) * p.x + (e1.x - e0.x) * p.y + e0.x * e1.y - e0.y * e1.x;
				return result;
			}

			float4 mainImage(float2 uv)
			{
				const float PI = 3.14159;

				float4 test = edge(uv, float2(0.5f,1.0f), float2(0.5f ,0.0f));

				return test;
			}

			v2f vert(appdata v)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(v.vertex);
				OUT.uv = v.uv;
				OUT.color = v.color * _Color;
#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(v.vertex);
#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}


            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 c = SampleSpriteTexture(i.uv) * i.color;
				
				float alphaX = pow(1 - (2 * abs(i.uv.x - 0.5f)),2);

				float alphaY = pow(1 - (2 * abs(i.uv.x - 0.5f)), 2);

				fixed4 col = mainImage(i.uv) * c.a ;
				col = col.rgba * alphaX *alphaY;
                return col * alphaX * alphaY;
            }			

            ENDCG
        }
    }
}
