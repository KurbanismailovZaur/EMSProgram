// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Numba/UI/VerticalGradient"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Color0("Color0", Color) = (1,1,1,1)
		_Color1("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Color3("Color3", Color) = (1,1,1,1)
		_Color4("Color4", Color) = (1,1,1,1)
		_Color5("Color5", Color) = (1,1,1,1)
		_Color6("Color6", Color) = (1,1,1,1)
		_Color7("Color7", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _Color0;
			uniform float4 _Color1;
			uniform float4 _Color2;
			uniform float4 _Color3;
			uniform float4 _Color4;
			uniform float4 _Color5;
			uniform float4 _Color6;
			uniform float4 _Color7;
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv21 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float uvUp37 = (uv21).y;
				float uvUpScaled41 = ( uvUp37 * 7 );
				float clampResult3_g1 = clamp( uvUpScaled41 , (float)0 , (float)1 );
				float4 lerpResult50 = lerp( _Color0 , _Color1 , clampResult3_g1);
				float clampResult3_g2 = clamp( ( uvUpScaled41 - (float)1 ) , (float)0 , (float)1 );
				float4 lerpResult52 = lerp( lerpResult50 , _Color2 , clampResult3_g2);
				float clampResult3_g3 = clamp( ( uvUpScaled41 - (float)2 ) , (float)0 , (float)1 );
				float4 lerpResult63 = lerp( lerpResult52 , _Color3 , clampResult3_g3);
				float clampResult3_g4 = clamp( ( uvUpScaled41 - (float)3 ) , (float)0 , (float)1 );
				float4 lerpResult68 = lerp( lerpResult63 , _Color4 , clampResult3_g4);
				float clampResult3_g5 = clamp( ( uvUpScaled41 - (float)4 ) , (float)0 , (float)1 );
				float4 lerpResult74 = lerp( lerpResult68 , _Color5 , clampResult3_g5);
				float clampResult3_g6 = clamp( ( uvUpScaled41 - (float)5 ) , (float)0 , (float)1 );
				float4 lerpResult80 = lerp( lerpResult74 , _Color6 , clampResult3_g6);
				float clampResult3_g7 = clamp( ( uvUpScaled41 - (float)6 ) , (float)0 , (float)1 );
				float4 lerpResult86 = lerp( lerpResult80 , _Color7 , clampResult3_g7);
				
				half4 color = lerpResult86;
				
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15201
7;29;1426;824;-133.8823;-468.2293;2.221215;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1239.42,463.9239;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;22;-1006.12,458.0535;Float;True;False;True;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;40;-416.3943,554.7405;Float;False;Constant;_Int3;Int 3;3;0;Create;True;0;0;False;0;7;0;0;1;INT;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-439.6036,458.1082;Float;False;37;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-734.8148,458.0315;Float;False;uvUp;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-203.3943,453.7404;Float;False;2;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;304.4995,677.1022;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-47.39429,457.7405;Float;False;uvUpScaled;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;243.6054,423.051;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;55;351.4995,794.1022;Float;False;Constant;_Int6;Int 6;3;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.FunctionNode;60;462.1898,427.7014;Float;False;Clamp01;-1;;1;8fc3713ce69d8e241adc2b06fa7af50e;0;1;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;56;549.4149,719.7014;Float;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;65;623.1506,1017.026;Float;False;Constant;_Int4;Int 4;3;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;576.1507,900.0259;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;49;458.9612,36.92035;Float;False;Property;_Color0;Color0;0;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;51;455.9612,229.9203;Float;False;Property;_Color1;Color1;1;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;66;821.0657,942.6251;Float;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;835.7332,1123.871;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;61;711.035,720.0677;Float;False;Clamp01;-1;;2;8fc3713ce69d8e241adc2b06fa7af50e;0;1;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;70;886.0371,1244.176;Float;False;Constant;_Int5;Int 5;3;0;Create;True;0;0;False;0;3;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;53;705.4995,501.1022;Float;False;Property;_Color2;Color2;2;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;50;757.9612,343.9203;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;79;1142.658,1465.957;Float;False;Constant;_Int7;Int 7;3;0;Create;True;0;0;False;0;4;0;0;1;INT;0
Node;AmplifyShaderEditor.FunctionNode;67;982.6847,942.9913;Float;False;Clamp01;-1;;3;8fc3713ce69d8e241adc2b06fa7af50e;0;1;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;75;1092.354,1345.652;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;62;977.1495,724.0261;Float;False;Property;_Color3;Color3;3;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;52;1019.645,502.677;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;71;1080.648,1166.47;Float;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;85;1393.559,1688.255;Float;False;Constant;_Int8;Int 8;3;0;Create;True;0;0;False;0;5;0;0;1;INT;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;1343.255,1567.95;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;63;1274.149,705.0259;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;73;1235.432,946.5714;Float;False;Property;_Color4;Color4;4;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;76;1337.269,1388.251;Float;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;72;1242.267,1166.837;Float;False;Clamp01;-1;;4;8fc3713ce69d8e241adc2b06fa7af50e;0;1;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;1601.956,1781.148;Float;False;41;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;90;1652.26,1901.453;Float;False;Constant;_Int9;Int 9;3;0;Create;True;0;0;False;0;6;0;0;1;INT;0
Node;AmplifyShaderEditor.LerpOp;68;1533.731,928.8711;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;82;1588.171,1610.549;Float;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;77;1498.888,1388.618;Float;False;Clamp01;-1;;5;8fc3713ce69d8e241adc2b06fa7af50e;0;1;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;78;1494.353,1169.652;Float;False;Property;_Color5;Color5;5;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;74;1790.352,1150.652;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;84;1744.255,1391.95;Float;False;Property;_Color6;Color6;6;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;88;1846.872,1823.747;Float;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;83;1749.789,1610.917;Float;False;Clamp01;-1;;6;8fc3713ce69d8e241adc2b06fa7af50e;0;1;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;89;2008.49,1824.115;Float;False;Clamp01;-1;;7;8fc3713ce69d8e241adc2b06fa7af50e;0;1;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;80;2041.254,1372.95;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;91;2002.956,1605.148;Float;False;Property;_Color7;Color7;7;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;86;2299.956,1586.148;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;9;2537.775,1584.858;Float;False;True;2;Float;ASEMaterialInspector;0;3;Numba/UI;5056123faa0c79b47ab6ad7e8bf059a4;0;0;Default;2;True;2;SrcAlpha;OneMinusSrcAlpha;0;One;Zero;False;True;Off;False;False;False;False;False;True;5;Queue=Transparent;IgnoreProjector=True;RenderType=Transparent;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;0;0;False;False;False;False;False;False;False;False;False;True;2;0;0;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;22;0;21;0
WireConnection;37;0;22;0
WireConnection;39;0;38;0
WireConnection;39;1;40;0
WireConnection;41;0;39;0
WireConnection;60;2;46;0
WireConnection;56;0;54;0
WireConnection;56;1;55;0
WireConnection;66;0;64;0
WireConnection;66;1;65;0
WireConnection;61;2;56;0
WireConnection;50;0;49;0
WireConnection;50;1;51;0
WireConnection;50;2;60;0
WireConnection;67;2;66;0
WireConnection;52;0;50;0
WireConnection;52;1;53;0
WireConnection;52;2;61;0
WireConnection;71;0;69;0
WireConnection;71;1;70;0
WireConnection;63;0;52;0
WireConnection;63;1;62;0
WireConnection;63;2;67;0
WireConnection;76;0;75;0
WireConnection;76;1;79;0
WireConnection;72;2;71;0
WireConnection;68;0;63;0
WireConnection;68;1;73;0
WireConnection;68;2;72;0
WireConnection;82;0;81;0
WireConnection;82;1;85;0
WireConnection;77;2;76;0
WireConnection;74;0;68;0
WireConnection;74;1;78;0
WireConnection;74;2;77;0
WireConnection;88;0;87;0
WireConnection;88;1;90;0
WireConnection;83;2;82;0
WireConnection;89;2;88;0
WireConnection;80;0;74;0
WireConnection;80;1;84;0
WireConnection;80;2;83;0
WireConnection;86;0;80;0
WireConnection;86;1;91;0
WireConnection;86;2;89;0
WireConnection;9;0;86;0
ASEEND*/
//CHKSM=EA891352A2E587C1E66ED83FA4C533314A637ECA