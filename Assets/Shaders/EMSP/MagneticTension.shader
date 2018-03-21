// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33748,y:32704,varname:node_3138,prsc:2|custl-7488-OUT,alpha-8231-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32530,y:32862,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Set,id:5221,x:32706,y:32862,varname:color,prsc:2|IN-7241-RGB;n:type:ShaderForge.SFN_Set,id:3540,x:32706,y:32942,varname:alpha,prsc:2|IN-7241-A;n:type:ShaderForge.SFN_NormalVector,id:6196,x:32530,y:33045,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:4842,x:32530,y:33210,varname:node_4842,prsc:2;n:type:ShaderForge.SFN_Dot,id:9733,x:32712,y:33114,varname:node_9733,prsc:2,dt:1|A-6196-OUT,B-4842-OUT;n:type:ShaderForge.SFN_Set,id:2668,x:32884,y:33114,varname:dotAlpha,prsc:2|IN-9733-OUT;n:type:ShaderForge.SFN_Get,id:7488,x:33521,y:32904,varname:node_7488,prsc:2|IN-5221-OUT;n:type:ShaderForge.SFN_Get,id:4328,x:33320,y:33084,varname:node_4328,prsc:2|IN-3540-OUT;n:type:ShaderForge.SFN_Get,id:1569,x:33142,y:32910,varname:node_1569,prsc:2|IN-2668-OUT;n:type:ShaderForge.SFN_Multiply,id:8231,x:33542,y:33010,varname:node_8231,prsc:2|A-8328-OUT,B-4328-OUT,C-8167-OUT;n:type:ShaderForge.SFN_Power,id:8328,x:33341,y:32943,varname:node_8328,prsc:2|VAL-1569-OUT,EXP-9995-OUT;n:type:ShaderForge.SFN_Slider,id:9995,x:33006,y:33002,ptovrint:False,ptlb:AlphaPower,ptin:_AlphaPower,varname:node_9995,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:4,max:8;n:type:ShaderForge.SFN_Slider,id:8167,x:33173,y:33201,ptovrint:False,ptlb:AlphaMultiplier,ptin:_AlphaMultiplier,varname:node_8167,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;proporder:7241-9995-8167;pass:END;sub:END;*/

Shader "EMSP/MagneticTension" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _AlphaPower ("AlphaPower", Range(1, 8)) = 4
        _AlphaMultiplier ("AlphaMultiplier", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _AlphaPower;
            uniform float _AlphaMultiplier;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
                float3 color = _Color.rgb;
                float3 finalColor = color;
                float dotAlpha = max(0,dot(i.normalDir,viewDirection));
                float alpha = _Color.a;
                return fixed4(finalColor,(pow(dotAlpha,_AlphaPower)*alpha*_AlphaMultiplier));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
