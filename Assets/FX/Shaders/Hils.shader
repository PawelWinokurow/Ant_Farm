// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Hils"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		_Matcap("Matcap", 2D) = "white" {}
		_ScaleUV1("ScaleUV", Float) = 1

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
		Stencil
		{
			Ref 1
			Comp NotEqual
			Pass Keep
			Fail Keep
			ZFail Keep
		}
		
		
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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _ScaleUV1;
			uniform sampler2D _Matcap;
			SamplerState sampler_Matcap;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_color = v.color;
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
				float2 appendResult194 = (float2(WorldPosition.x , WorldPosition.z));
				float4 transform179 = mul(unity_WorldToObject,float4( float3(-1,0,0) , 0.0 ));
				float4 temp_output_136_0 = (i.ase_color*2.0 + -1.0);
				float dotResult137 = dot( transform179 , temp_output_136_0 );
				float4 transform180 = mul(unity_WorldToObject,float4( float3(0,0,1) , 0.0 ));
				float dotResult171 = dot( transform180 , temp_output_136_0 );
				float2 appendResult164 = (float2(dotResult137 , dotResult171));
				float temp_output_183_0 = ( tex2D( _Matcap, (appendResult164*0.5 + 0.5) ).r - 0.5 );
				
				
				finalColor = ( ( ( _Color * tex2D( _MainTex, ( _MainTex_ST.zw + ( _MainTex_ST.xy * appendResult194 * _ScaleUV1 ) ) ) ) + saturate( temp_output_183_0 ) ) * ( 1.0 - saturate( ( -1.0 * temp_output_183_0 ) ) ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18500
985.3334;80;905.3334;729;1318.742;1628.505;2.462073;True;False
Node;AmplifyShaderEditor.VertexColorNode;135;-2650.863,-459.0574;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;138;-1839.669,-502.1556;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;False;-1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;175;-1974.911,-280.5346;Inherit;False;Constant;_Vector1;Vector 1;0;0;Create;True;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldToObjectTransfNode;179;-1621.948,-510.9644;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;136;-2282.092,-539.4202;Inherit;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;180;-1744.595,-247.7073;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;137;-1470.219,-313.5609;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;171;-1466.266,-142.8782;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;164;-1112.477,-81.13117;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;191;-1145.134,-1007.21;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;192;-1306.667,-1409.941;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DynamicAppendNode;194;-944.1353,-968.2098;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;155;-820.101,-78.76862;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureTransformNode;195;-995.0563,-1145.523;Inherit;False;-1;False;1;0;SAMPLER2D;;False;2;FLOAT2;0;FLOAT2;1
Node;AmplifyShaderEditor.RangedFloatNode;193;-1031.55,-782.358;Inherit;False;Property;_ScaleUV1;ScaleUV;3;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-789.2633,-941.4023;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;151;-520.6865,-110.5565;Inherit;True;Property;_Matcap;Matcap;2;0;Create;True;0;0;False;0;False;-1;None;fa9784bf53dee4322b5fd85d1dd1c77a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;183;-367.6127,-387.0674;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;197;-609.3672,-947.5289;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;200;-222.9415,-1183.46;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;-199.1109,-427.1762;Inherit;False;2;2;0;FLOAT;-1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;198;-390.2693,-1001.585;Inherit;True;Property;_TextureSample1;Texture Sample 0;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;-4.941467,-890.4596;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;184;-209.9844,-656.8651;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;187;-132.5147,-523.6728;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;182;-21.24303,-683.9905;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;189;31.93726,-423.0994;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;143.3839,-564.4464;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;134;506.7751,-460.3226;Float;False;True;-1;2;ASEMaterialInspector;100;1;Custom/Hils;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;True;1;False;-1;255;False;-1;255;False;-1;6;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;179;0;138;0
WireConnection;136;0;135;0
WireConnection;180;0;175;0
WireConnection;137;0;179;0
WireConnection;137;1;136;0
WireConnection;171;0;180;0
WireConnection;171;1;136;0
WireConnection;164;0;137;0
WireConnection;164;1;171;0
WireConnection;194;0;191;1
WireConnection;194;1;191;3
WireConnection;155;0;164;0
WireConnection;195;0;192;0
WireConnection;196;0;195;0
WireConnection;196;1;194;0
WireConnection;196;2;193;0
WireConnection;151;1;155;0
WireConnection;183;0;151;1
WireConnection;197;0;195;1
WireConnection;197;1;196;0
WireConnection;186;1;183;0
WireConnection;198;0;192;0
WireConnection;198;1;197;0
WireConnection;199;0;200;0
WireConnection;199;1;198;0
WireConnection;184;0;183;0
WireConnection;187;0;186;0
WireConnection;182;0;199;0
WireConnection;182;1;184;0
WireConnection;189;0;187;0
WireConnection;188;0;182;0
WireConnection;188;1;189;0
WireConnection;134;0;188;0
ASEEND*/
//CHKSM=ED89E0D33A8189D1CB914801150E34F275B2B734