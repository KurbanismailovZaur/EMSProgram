// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33079,y:32792,varname:node_3138,prsc:2|custl-7872-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32695,y:32960,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_NormalVector,id:8789,x:32293,y:32921,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:4457,x:32293,y:33080,varname:node_4457,prsc:2;n:type:ShaderForge.SFN_Dot,id:6364,x:32471,y:32990,varname:node_6364,prsc:2,dt:1|A-8789-OUT,B-4457-OUT;n:type:ShaderForge.SFN_Multiply,id:7872,x:32882,y:33034,varname:node_7872,prsc:2|A-7241-RGB,B-9158-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:9158,x:32695,y:33124,varname:node_9158,prsc:2|IN-6364-OUT,IMIN-2402-OUT,IMAX-6549-OUT,OMIN-9539-OUT,OMAX-9072-OUT;n:type:ShaderForge.SFN_Vector1,id:2402,x:32458,y:33158,varname:node_2402,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:6549,x:32458,y:33225,varname:node_6549,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:9539,x:32301,y:33318,ptovrint:False,ptlb:BlackThresold,ptin:_BlackThresold,varname:node_9539,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:9072,x:32301,y:33422,ptovrint:False,ptlb:WhiteThresolt,ptin:_WhiteThresolt,varname:node_9072,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;proporder:7241-9539-9072;pass:END;sub:END;*/

Shader "Numba/Unlit/Lit/Color" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _BlackThresold ("BlackThresold", Range(0, 1)) = 0
        _WhiteThresolt ("WhiteThresolt", Range(0, 1)) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _BlackThresold;
            uniform float _WhiteThresolt;
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
                float node_2402 = 0.0;
                float3 finalColor = (_Color.rgb*(_BlackThresold + ( (max(0,dot(i.normalDir,viewDirection)) - node_2402) * (_WhiteThresolt - _BlackThresold) ) / (1.0 - node_2402)));
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
