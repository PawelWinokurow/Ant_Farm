// Upgrade NOTE: upgraded instancing buffer 'CustomBlob' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Blob"
{
	Properties
	{
		_U("U", Vector) = (-1,0,0,0)
		_V("V", Vector) = (0,0,1,0)
		_Color("Color", Color) = (0,0,0,0)
		_MainTex("MainTex", 2D) = "white" {}
		_ScaleUV("ScaleUV", Float) = 1
		_Mat("Mat", 2D) = "white" {}
		_Forward("Forward", Range( 0 , 1)) = 0
		_BlinkFX_Color("BlinkFX_Color", Color) = (1,0,0,1)
		_Blink_FX("Blink_FX", Range( 0 , 1)) = 0
		_Tex("Tex", 2D) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			#define ASE_ABSOLUTE_VERTEX_POS 1


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _Tex;
			uniform float4 _Tex_ST;
			uniform float _Forward;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _ScaleUV;
			uniform float4 _Color;
			uniform sampler2D _Mat;
			SamplerState sampler_Mat;
			uniform float3 _U;
			uniform float3 _V;
			uniform float4 _BlinkFX_Color;
			UNITY_INSTANCING_BUFFER_START(CustomBlob)
				UNITY_DEFINE_INSTANCED_PROP(float, _Blink_FX)
#define _Blink_FX_arr CustomBlob
			UNITY_INSTANCING_BUFFER_END(CustomBlob)
			float Sin_Custom127( float Time )
			{
				return smoothstep( 0.0 , 1.0 , abs( (frac( Time )*2.0 -1.0) ));
			}
			
			float Sin_Custom126( float Time )
			{
				return smoothstep( 0.0 , 1.0 , abs( (frac( Time )*2.0 -1.0) ));
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float mulTime55 = _Time.y * 0.5;
				float temp_output_131_0 = ( mulTime55 * 3.0 );
				float Time127 = ( temp_output_131_0 + 0.25 );
				float localSin_Custom127 = Sin_Custom127( Time127 );
				float2 texCoord71 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_113_0 = (tex2Dlod( _Tex, float4( ( _Tex_ST.zw + ( _Tex_ST.xy * texCoord71 * 3.0 ) + ( mulTime55 * 0.3 ) ), 0, 0.0) ).r*2.0 + -1.0);
				float temp_output_112_0 = ( ( ( 0.3 * localSin_Custom127 ) + ( temp_output_113_0 * 0.5 * ( 1.0 - texCoord71.y ) ) ) + 1.0 );
				float Time126 = temp_output_131_0;
				float localSin_Custom126 = Sin_Custom126( Time126 );
				float3 appendResult52 = (float3(temp_output_112_0 , ( ( 0.5 * localSin_Custom126 ) + 1.0 ) , temp_output_112_0));
				float lerpResult96 = lerp( 1.0 , texCoord71.y , ( texCoord71.x * 1.0 ));
				float3 appendResult90 = (float3(0.0 , 0.0 , ( _Forward * lerpResult96 )));
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = ( ( v.vertex.xyz * appendResult52 ) + appendResult90 );
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 appendResult8 = (float2(WorldPosition.x , WorldPosition.z));
				float2 texCoord30 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float4 lerpResult32 = lerp( _Color , ( _Color * 0.5 ) , texCoord30.y);
				float4 transform16 = mul(unity_WorldToObject,float4( _U , 0.0 ));
				float4 normalizeResult21 = normalize( transform16 );
				float4 temp_output_19_0 = (i.ase_color*2.0 + -1.0);
				float4 normalizeResult47 = normalize( temp_output_19_0 );
				float dotResult23 = dot( normalizeResult21 , normalizeResult47 );
				float4 transform17 = mul(unity_WorldToObject,float4( _V , 0.0 ));
				float4 normalizeResult20 = normalize( transform17 );
				float4 normalizeResult48 = normalize( temp_output_19_0 );
				float dotResult22 = dot( normalizeResult20 , normalizeResult48 );
				float2 appendResult24 = (float2(dotResult23 , dotResult22));
				float2 texCoord71 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime55 = _Time.y * 0.5;
				float temp_output_113_0 = (tex2D( _Tex, ( _Tex_ST.zw + ( _Tex_ST.xy * texCoord71 * 3.0 ) + ( mulTime55 * 0.3 ) ) ).r*2.0 + -1.0);
				float temp_output_27_0 = ( tex2D( _Mat, ( (appendResult24*0.5 + 0.5) + ( temp_output_113_0 * 0.3 ) ) ).r - 0.5 );
				float _Blink_FX_Instance = UNITY_ACCESS_INSTANCED_PROP(_Blink_FX_arr, _Blink_FX);
				float4 lerpResult40 = lerp( saturate( ( ( ( tex2D( _MainTex, ( _MainTex_ST.zw + ( _MainTex_ST.xy * appendResult8 * _ScaleUV ) ) ) * lerpResult32 ) + saturate( temp_output_27_0 ) ) * ( 1.0 - saturate( ( -1.0 * temp_output_27_0 ) ) ) ) ) , _BlinkFX_Color , ( _BlinkFX_Color.a * _Blink_FX_Instance ));
				
				
				finalColor = lerpResult40;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	

	
}
/*ASEBEGIN
Version=18500
326;97.33334;998;733.6667;1230.913;979.9821;1;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;106;-1767.849,-13.77307;Inherit;True;Property;_Tex;Tex;9;0;Create;True;0;0;False;0;False;None;27e08f06f539657468481e691046c2ce;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureTransformNode;76;-1507.985,142.4035;Inherit;False;-1;False;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;71;-1495.685,384.5649;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;55;-1555.297,-86.34371;Inherit;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-1358.568,604.3023;Inherit;False;Constant;_Float2;Float 2;11;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;15;-3196.969,-1351.928;Inherit;False;Property;_U;U;0;0;Create;True;0;0;False;0;False;-1,0,0;-1,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VertexColorNode;18;-3610.677,-1159.528;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;14;-3407.91,-847.2419;Inherit;False;Property;_V;V;1;0;Create;True;0;0;False;0;False;0,0,1;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-1227.592,314.7939;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;17;-3184.058,-825.3237;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-1084.47,344.8551;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;16;-2929.916,-1346.413;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;19;-3344.679,-1165;Inherit;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;21;-2728.552,-1058.415;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NormalizeNode;47;-2770.968,-982.9498;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;20;-2888.832,-807.2535;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-948.6548,295.6168;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;48;-2806.303,-887.0409;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;68;-778.1152,202.3361;Inherit;True;Property;_Texture3;Texture3;9;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;22;-2528.853,-768.4584;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;23;-2532.806,-939.1411;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;113;-471.032,194.9654;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-2175.064,-706.7114;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-1880.946,-536.9774;Inherit;False;2;2;0;FLOAT;0.3;False;1;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-3275.84,-1935.518;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;1;-3257.556,-2370.453;Inherit;True;Property;_MainTex;MainTex;3;0;Create;True;0;0;False;0;False;None;7bcbc3cef8578244ca7fc84667b66b79;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ScaleAndOffsetNode;25;-1882.689,-704.3488;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-2899.272,-1896.937;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2848.958,-1666.098;Inherit;False;Property;_ScaleUV;ScaleUV;4;0;Create;False;0;0;False;0;False;1;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureTransformNode;4;-2944.414,-2106.035;Inherit;False;-1;False;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.SimpleAddOpNode;133;-1661.418,-651.4613;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;-1349.682,-135.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-2738.621,-1901.913;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-2006.231,-1564.249;Inherit;False;Constant;_Float3;Float 3;11;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;128;-1044.742,-200.2818;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;28;-2063.561,-1786.343;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;False;0,0,0,0;0.3857143,1,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;46;-1504.409,-713.4959;Inherit;True;Property;_Mat;Mat;5;0;Create;True;0;0;False;0;False;-1;None;fa9784bf53dee4322b5fd85d1dd1c77a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-2558.725,-1908.04;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CustomExpressionNode;127;-927.133,-205.1407;Inherit;False;return smoothstep( 0.0 , 1.0 , abs( (frac( Time )*2.0 -1.0) ))@;1;False;1;True;Time;FLOAT;0;In;;Inherit;False;Sin_Custom;True;False;0;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1806.999,-1597.625;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-2021.786,-1406.327;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;135;-422.4997,402.1232;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-1430.2,-1012.647;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;32;-1554.31,-1633.683;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-524.8428,-29.96448;Inherit;False;2;2;0;FLOAT;0.3;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1261.698,-1052.756;Inherit;False;2;2;0;FLOAT;-1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-220.1791,176.1844;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;126;-959.112,-352.4542;Inherit;False;return smoothstep( 0.0 , 1.0 , abs( (frac( Time )*2.0 -1.0) ))@;1;False;1;True;Time;FLOAT;0;In;;Inherit;False;Sin_Custom;True;False;0;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-2339.627,-1962.096;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;130;-368.7267,-123.5604;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;-326.8947,19.23112;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;33;-1272.572,-1282.445;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-456.4521,-239.9121;Inherit;False;2;2;0;FLOAT;0.5;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;34;-1147.629,-1141.835;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-869.3913,565.8165;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1410.776,-1948.471;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;96;-653.9371,534.6932;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;35;-1030.65,-1048.679;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;100;-246.3942,-233.2325;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-1083.83,-1309.571;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;91;123.5702,-47.64812;Inherit;False;Property;_Forward;Forward;6;0;Create;True;0;0;False;0;False;0;0.138;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;112;-184.9844,15.44685;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-919.2032,-1190.026;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;53;-342.8232,-549.3503;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;52;-36.39542,-339.2644;Inherit;False;FLOAT3;4;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;37;-824.6475,-972.5625;Inherit;False;Property;_BlinkFX_Color;BlinkFX_Color;7;0;Create;True;0;0;False;0;False;1,0,0,1;1,0,0,0.5333334;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;546.7167,-32.64672;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-794.8868,-672.7153;Inherit;False;InstancedProperty;_Blink_FX;Blink_FX;8;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;50;-748.0731,-1197.95;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;88.73086,-550.6288;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;90;601.2582,-316.2773;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-445.8053,-755.7016;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;719.7864,-579.8431;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;40;-411.3151,-1138.233;Inherit;False;3;0;COLOR;0,0,0,1;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;9;870.4589,-997.6209;Float;False;True;-1;2;ASEMaterialInspector;100;1;Custom/Blob;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;1;=;0;Standard;1;Vertex Position,InvertActionOnDeselection;0;0;1;True;False;;False;0
WireConnection;76;0;106;0
WireConnection;72;0;76;0
WireConnection;72;1;71;0
WireConnection;72;2;78;0
WireConnection;17;0;14;0
WireConnection;80;0;55;0
WireConnection;16;0;15;0
WireConnection;19;0;18;0
WireConnection;21;0;16;0
WireConnection;47;0;19;0
WireConnection;20;0;17;0
WireConnection;77;0;76;1
WireConnection;77;1;72;0
WireConnection;77;2;80;0
WireConnection;48;0;19;0
WireConnection;68;0;106;0
WireConnection;68;1;77;0
WireConnection;22;0;20;0
WireConnection;22;1;48;0
WireConnection;23;0;21;0
WireConnection;23;1;47;0
WireConnection;113;0;68;1
WireConnection;24;0;23;0
WireConnection;24;1;22;0
WireConnection;114;0;113;0
WireConnection;25;0;24;0
WireConnection;8;0;2;1
WireConnection;8;1;2;3
WireConnection;4;0;1;0
WireConnection;133;0;25;0
WireConnection;133;1;114;0
WireConnection;131;0;55;0
WireConnection;5;0;4;0
WireConnection;5;1;8;0
WireConnection;5;2;3;0
WireConnection;128;0;131;0
WireConnection;46;1;133;0
WireConnection;6;0;4;1
WireConnection;6;1;5;0
WireConnection;127;0;128;0
WireConnection;88;0;28;0
WireConnection;88;1;87;0
WireConnection;135;0;71;2
WireConnection;27;0;46;1
WireConnection;32;0;28;0
WireConnection;32;1;88;0
WireConnection;32;2;30;2
WireConnection;108;1;127;0
WireConnection;29;1;27;0
WireConnection;134;0;113;0
WireConnection;134;2;135;0
WireConnection;126;0;131;0
WireConnection;7;0;1;0
WireConnection;7;1;6;0
WireConnection;110;0;108;0
WireConnection;110;1;134;0
WireConnection;33;0;27;0
WireConnection;101;1;126;0
WireConnection;34;0;29;0
WireConnection;98;0;71;1
WireConnection;10;0;7;0
WireConnection;10;1;32;0
WireConnection;96;1;71;2
WireConnection;96;2;98;0
WireConnection;35;0;34;0
WireConnection;100;0;101;0
WireConnection;100;1;130;0
WireConnection;36;0;10;0
WireConnection;36;1;33;0
WireConnection;112;0;110;0
WireConnection;39;0;36;0
WireConnection;39;1;35;0
WireConnection;52;0;112;0
WireConnection;52;1;100;0
WireConnection;52;2;112;0
WireConnection;94;0;91;0
WireConnection;94;1;96;0
WireConnection;50;0;39;0
WireConnection;66;0;53;0
WireConnection;66;1;52;0
WireConnection;90;2;94;0
WireConnection;99;0;37;4
WireConnection;99;1;38;0
WireConnection;89;0;66;0
WireConnection;89;1;90;0
WireConnection;40;0;50;0
WireConnection;40;1;37;0
WireConnection;40;2;99;0
WireConnection;9;0;40;0
WireConnection;9;1;89;0
ASEEND*/
//CHKSM=E82705FDA6E68C457842D4AC279E3C93B9AE6955