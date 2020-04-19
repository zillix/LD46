// Screenspace tutorial: https://www.ronja-tutorials.com/2019/01/20/screenspace-texture.html

Shader "LD46/UnlitScreenSpaceParticle"{
	//show values to edit in inspector
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_Alpha("Alpha", Float) = 1
		_ColorAmt("ColorAmt", Float) = 1
		_Aspect("AspectRatio", Float) = 1
	}

		SubShader{
		//the material is completely non-transparent and is rendered at the same time as the other opaque geometry
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{

			// taken from https://computergraphics.stackexchange.com/questions/4801/alpha-blending-between-two-overlapping-semi-transparent-shapes
			 Stencil {
				Ref 0
				Comp Equal
				Pass IncrSat
				Fail IncrSat
			}

			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"

			//define vertex and fragment shader
			#pragma vertex vert
			#pragma fragment frag

			//texture and transforms of the texture
			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _Alpha;
			float _ColorAmt;
			float _Aspect;

			//the object data that's put into the vertex shader
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			//the data that's used to generate fragments and can be read by the fragment shader
			struct v2f {
				float4 position : SV_POSITION;
				float4 screenPosition : TEXCOORD0;
				fixed4 color : COLOR;
			};

			//the vertex shader
			v2f vert(appdata v) {
				v2f o;
				//convert the vertex positions from object space to clip space so they can be rendered
				o.position = UnityObjectToClipPos(v.vertex);
				o.screenPosition = ComputeScreenPos(o.position);

				// since the particle emitter changes the vertex color,
				// we need to keep that to carry it over
				o.color = v.color;
				return o;
			}

			//the fragment shader
			fixed4 frag(v2f i) : SV_TARGET{
				float2 textureCoordinate = i.screenPosition.xy / i.screenPosition.w;
				float aspect = _ScreenParams.x / _ScreenParams.y;
				textureCoordinate.x = textureCoordinate.x * aspect * _Aspect;
				textureCoordinate = TRANSFORM_TEX(textureCoordinate, _MainTex);
				fixed4 col = tex2D(_MainTex, textureCoordinate);

				// Interpolate linearly between the screen space
				// image and the color that already existed on the particle
				//col.rgb = (i.color.rgb - col.rgb) * _ColorAmt + col.rbg;/*
				col.r = (i.color.r - col.r) * _ColorAmt + col.r;
				col.g = (i.color.g - col.g) * _ColorAmt + col.g;
				col.b = (i.color.b - col.b) * _ColorAmt + col.b;
				col.a = _Alpha;
				return col;
			}

			ENDCG
		}
	}
}