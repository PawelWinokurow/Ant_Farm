// Upgrade NOTE: upgraded instancing buffer 'CustomGround_Base' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Ground_Base"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		_ScaleUV("ScaleUV", Float) = 1
		_BlendTexture("BlendTexture", Range( 0 , 1)) = 0
		_Heigth("Heigth", Float) = 1
		_BlendDetails("BlendDetails", Range( 0 , 1)) = 0.5
		_BlinkFX_Color("BlinkFX_Color", Color) = (1,0,0,1)
		_Blink_FX("Blink_FX", Range( 0 , 1)) = 0

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

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
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

			uniform float4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _ScaleUV;
			uniform float _BlendTexture;
			uniform float _Heigth;
			uniform float _BlendDetails;
			uniform float4 _BlinkFX_Color;
			UNITY_INSTANCING_BUFFER_START(CustomGround_Base)
				UNITY_DEFINE_INSTANCED_PROP(float, _Blink_FX)
#define _Blink_FX_arr CustomGround_Base
			UNITY_INSTANCING_BUFFER_END(CustomGround_Base)

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
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
				float2 appendResult141 = (float2(WorldPosition.x , WorldPosition.z));
				float temp_output_216_0 = max( i.ase_texcoord1.xy.y , 0.0 );
				float4 lerpResult199 = lerp( _Color , ( _Color * tex2D( _MainTex, ( _MainTex_ST.zw + ( _MainTex_ST.xy * appendResult141 * _ScaleUV ) ) ) ) , ( _BlendTexture * saturate( (0.5 + (temp_output_216_0 - 0.0) * (1.0 - 0.5) / (_Heigth - 0.0)) ) ));
				float4 temp_cast_0 = (1.0).xxxx;
				float3 objToWorld186 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
				float4 lerpResult207 = lerp( temp_cast_0 , tex2D( _MainTex, ( _MainTex_ST.zw + ( _MainTex_ST.xy * (objToWorld186).xz * 1.1 ) ) ) , _BlendDetails);
				float4 temp_cast_1 = (saturate( (0.1 + (temp_output_216_0 - 0.0) * (0.0 - 0.1) / (_Heigth - 0.0)) )).xxxx;
				float4 temp_output_225_0 = ( ( ( lerpResult199 * lerpResult207 ) - temp_cast_1 ) * i.ase_color );
				float _Blink_FX_Instance = UNITY_ACCESS_INSTANCED_PROP(_Blink_FX_arr, _Blink_FX);
				float4 lerpResult227 = lerp( temp_output_225_0 , _BlinkFX_Color , ( _BlinkFX_Color.a * _Blink_FX_Instance ));
				
				
				finalColor = saturate( lerpResult227 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18500
1069.333;97.33334;1055.333;717;1939.588;1243.745;1.546656;True;False
Node;AmplifyShaderEditor.TexturePropertyNode;140;-2317.49,-1046.127;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;False;None;09e53d9ff92d3284984c5f65a9c929fe;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.WorldPosInputsNode;139;-2549.957,-698.3959;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureTransformNode;142;-2005.879,-781.7092;Inherit;False;-1;False;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.RangedFloatNode;148;-2042.373,-418.5441;Inherit;False;Property;_ScaleUV;ScaleUV;2;0;Create;True;0;0;False;0;False;1;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;141;-1954.958,-604.3959;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;217;-1151.639,273.6603;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformPositionNode;186;-2287.916,117.3799;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureTransformNode;175;-1942.064,-327.9177;Inherit;False;-1;False;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.SimpleMaxOpNode;216;-852.7628,30.36777;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-1700.798,54.59566;Inherit;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;False;0;False;1.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;185;-1988.506,118.3159;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-1800.086,-577.5884;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;197;-901.4053,452.9112;Inherit;False;Property;_Heigth;Heigth;4;0;Create;True;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;202;-657.6778,-87.82479;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;-1736.271,-123.7967;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;144;-1620.19,-583.715;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;231;-1248.358,-986.0637;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;0.5471698,0.5471698,0.5471698,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;145;-1401.092,-637.7708;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;189;-1283.18,18.3264;Inherit;False;Property;_BlendTexture;BlendTexture;3;0;Create;True;0;0;False;0;False;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;177;-1556.375,-129.9234;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;203;-457.7679,-113.2085;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;190;-1064.668,-547.0609;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;206;-1249.018,-366.0267;Inherit;False;Property;_BlendDetails;BlendDetails;5;0;Create;True;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;233;-1004.377,-743.1439;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;171;-1352.417,-198.398;Inherit;True;Property;_TextureSample1;Texture Sample 1;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;-707.0776,-251.8527;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;207;-837.1393,-454.1249;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;199;-791.011,-846.7244;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;194;-361.8684,181.0778;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;212;-129.5627,183.454;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;209;-446.4826,-616.6628;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;226;59.26347,-312.6599;Inherit;False;Property;_BlinkFX_Color;BlinkFX_Color;6;0;Create;False;0;0;False;0;False;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;229;15.66535,17.88945;Inherit;False;InstancedProperty;_Blink_FX;Blink_FX;7;0;Create;False;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;198;-416.8397,-454.6629;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;169;-209.1927,-194.6389;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;225;51.16618,-538.2726;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;373.7226,-109.5181;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;227;346.7113,-381.8767;Inherit;False;3;0;COLOR;0,0,0,1;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;230;621.299,-494.9502;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;228;465.3494,-641.1785;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;187;789.8187,-494.0927;Float;False;True;-1;2;ASEMaterialInspector;100;1;Custom/Ground_Base;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;142;0;140;0
WireConnection;141;0;139;1
WireConnection;141;1;139;3
WireConnection;175;0;140;0
WireConnection;216;0;217;2
WireConnection;185;0;186;0
WireConnection;143;0;142;0
WireConnection;143;1;141;0
WireConnection;143;2;148;0
WireConnection;202;0;216;0
WireConnection;202;2;197;0
WireConnection;176;0;175;0
WireConnection;176;1;185;0
WireConnection;176;2;211;0
WireConnection;144;0;142;1
WireConnection;144;1;143;0
WireConnection;145;0;140;0
WireConnection;145;1;144;0
WireConnection;177;0;175;1
WireConnection;177;1;176;0
WireConnection;203;0;202;0
WireConnection;233;0;231;0
WireConnection;233;1;145;0
WireConnection;171;0;140;0
WireConnection;171;1;177;0
WireConnection;201;0;189;0
WireConnection;201;1;203;0
WireConnection;207;0;190;0
WireConnection;207;1;171;0
WireConnection;207;2;206;0
WireConnection;199;0;231;0
WireConnection;199;1;233;0
WireConnection;199;2;201;0
WireConnection;194;0;216;0
WireConnection;194;2;197;0
WireConnection;212;0;194;0
WireConnection;209;0;199;0
WireConnection;209;1;207;0
WireConnection;198;0;209;0
WireConnection;198;1;212;0
WireConnection;225;0;198;0
WireConnection;225;1;169;0
WireConnection;232;0;226;4
WireConnection;232;1;229;0
WireConnection;227;0;225;0
WireConnection;227;1;226;0
WireConnection;227;2;232;0
WireConnection;230;0;227;0
WireConnection;228;0;225;0
WireConnection;228;1;227;0
WireConnection;187;0;230;0
ASEEND*/
//CHKSM=07E599540421803E87028CC2F2CC8819931CF599