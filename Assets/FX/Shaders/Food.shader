// Upgrade NOTE: upgraded instancing buffer 'CustomFood' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Food"
{
	Properties
	{
		_Mult("Mult", Float) = 1
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
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _Mult;
			uniform float4 _BlinkFX_Color;
			UNITY_INSTANCING_BUFFER_START(CustomFood)
				UNITY_DEFINE_INSTANCED_PROP(float, _Blink_FX)
#define _Blink_FX_arr CustomFood
			UNITY_INSTANCING_BUFFER_END(CustomFood)

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_color = v.color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
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
				float2 texCoord190 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_220_0 = ( texCoord190.y - 0.5 );
				float _Blink_FX_Instance = UNITY_ACCESS_INSTANCED_PROP(_Blink_FX_arr, _Blink_FX);
				float4 lerpResult225 = lerp( ( ( i.ase_color + saturate( ( 0.5 * temp_output_220_0 ) ) ) * ( 1.0 - saturate( ( -0.3 * temp_output_220_0 ) ) ) * _Mult ) , _BlinkFX_Color , ( _BlinkFX_Color.a * _Blink_FX_Instance ));
				
				
				finalColor = saturate( lerpResult225 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18500
1584.221;52.58053;866;903;2152.289;1282.789;1.721858;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;190;-2403.125,-517.2972;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;220;-2000.376,-511.9448;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;221;-1972.411,-686.2755;Inherit;False;2;2;0;FLOAT;0.5;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;217;-1764.522,-506.492;Inherit;False;2;2;0;FLOAT;-0.3;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;218;-1765.278,-648.5502;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;214;-1854.633,-761.9332;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;210;-2283.763,-1171.462;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;227;-1191.219,-334.4065;Inherit;False;InstancedProperty;_Blink_FX;Blink_FX;2;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;219;-1563.188,-571.748;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;223;-1402.783,-445.39;Inherit;False;Property;_Mult;Mult;0;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;224;-1228.173,-600.0908;Inherit;False;Property;_BlinkFX_Color;BlinkFX_Color;1;0;Create;True;0;0;False;0;False;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;215;-1654.006,-808.8679;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-1422.027,-717.0569;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;229;-951.0061,-540.0942;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;225;-878.8328,-766.1014;Inherit;False;3;0;COLOR;0,0,0,1;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;228;-647.1002,-760.4912;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;134;-482.6693,-769.204;Float;False;True;-1;2;ASEMaterialInspector;100;1;Custom/Food;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;1;False;-1;255;False;-1;255;False;-1;6;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;220;0;190;2
WireConnection;221;1;220;0
WireConnection;217;1;220;0
WireConnection;218;0;217;0
WireConnection;214;0;221;0
WireConnection;219;0;218;0
WireConnection;215;0;210;0
WireConnection;215;1;214;0
WireConnection;216;0;215;0
WireConnection;216;1;219;0
WireConnection;216;2;223;0
WireConnection;229;0;224;4
WireConnection;229;1;227;0
WireConnection;225;0;216;0
WireConnection;225;1;224;0
WireConnection;225;2;229;0
WireConnection;228;0;225;0
WireConnection;134;0;228;0
ASEEND*/
//CHKSM=6AD5B4DFD30C2D17D37831B22EF438FAF4C833FF