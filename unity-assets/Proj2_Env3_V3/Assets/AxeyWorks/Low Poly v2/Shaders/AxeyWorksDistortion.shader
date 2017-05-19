// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "AxeyWorks/Distortion"
{
	Properties
	{
		_MainTex("Diffuse (RGB)", 2D) = "white" {}
		_Color("Main Colour", Color) = (1,0,0,1)
		_SpecColor("Specular Colour", Color) = (1,1,1,1)
		_Shininess("Matte", Float) = 1.0
		_WaveLength("Churn Length", Float) = 0.5
		_WaveHeight("Churn Height", Float) = 0.5
		_WaveSpeed("Churn Speed", Float) = 1.0
		_RandomHeight("Randomise Height", Float) = 0.5
		_RandomSpeed("Randomise Speed", Float) = 0.5
		_CollisionWaveLength("Collision Churn Length", Float) = 2.0
		_AlphaFactor("Alpha factor", Range(0, 1.0)) = 1.0
	}

	CGINCLUDE
		#define UNITY_SETUP_BRDF_INPUT SpecularSetup
	ENDCG

	SubShader
	{
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
				#pragma target 3.0
				#pragma exclude_renderers gles
				#pragma vertex vert
				#pragma geometry geom
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				#pragma multi_compile_fwdbase
				#pragma multi_compile_fog
				#include "UnityCG.cginc"
				#include "HLSLSupport.cginc"
				#include "UnityStandardCore.cginc"

				float rand(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
				}

				float rand2(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
				}

				float _WaveLength;
				float _WaveHeight;
				float _WaveSpeed;
				float _RandomHeight;
				float _RandomSpeed;
				float _CollisionWaveLength;
				vector _CollisionVectors[20];
				
				uniform float _Shininess;
				uniform float _AlphaFactor;

				sampler2D _CameraDepthTexture; 

				struct vFwdBase
				{
					float4 pos                     : SV_POSITION;
					float4 tex                     : TEXCOORD0;
					half3 eyeVec                   : TEXCOORD1;
					half4 tangentToWorldAndParallax[3]   : TEXCOORD2;
					half4 ambientOrLightmapUV         : TEXCOORD5;
					SHADOW_COORDS(6)
					UNITY_FOG_COORDS(7)
					float4 posWorld                  : TEXCOORD8;
					float3 diffuseColor : TEXCOORD9;
					float3 specularColor : TEXCOORD10;
				};

				vFwdBase vert(VertexInput v)
				{
					vFwdBase o;
					UNITY_INITIALIZE_OUTPUT(vFwdBase, o);

					float3 v0 = mul((float3x3)unity_ObjectToWorld, v.vertex).xyz;

					float collPhase = 0.0;

					for (int i = 0; i < 20; i++)
					{
						float distanceToCenter = length(v0.xz - _CollisionVectors[i].xy);
						float waveHeight = _CollisionVectors[i].z;
						float waveState = _CollisionVectors[i].w;

						if (distanceToCenter < _CollisionWaveLength * 10 * waveState)
						{
							collPhase += (waveHeight * ((1.0 - waveState) * distanceToCenter) / (_CollisionWaveLength * 10 * waveState)) * sin(distanceToCenter - (_CollisionWaveLength * 10 * waveState));
						}
					}

					float phase0 = (_WaveHeight)* sin((_Time[1] * _WaveSpeed) + (v0.x * _WaveLength) + (v0.z * _WaveLength) + rand2(v0.xzz));
					float phase0_1 = (_RandomHeight)* sin(cos(rand(v0.xzz) * _RandomHeight * cos(_Time[1] * _RandomSpeed * sin(rand(v0.xxz)))));
					float phase0_2 = (_WaveHeight / 5.0) * sin((_Time[1] * _WaveSpeed * 3.0) + (v0.x * -_WaveLength * 4.0) + (v0.z * _WaveLength * 4.0) + rand2(v0.xzz));

					v0.y += collPhase + phase0 + phase0_1 + phase0_2;

					v.vertex.xyz = mul((float3x3)unity_WorldToObject, v0);

					float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
					#if UNITY_SPECCUBE_BOX_PROJECTION
						o.posWorld = posWorld;
					#endif
					o.pos = v.vertex;
					o.tex = TexCoords(v);
					o.eyeVec = NormalizePerVertexNormal(posWorld.xyz - _WorldSpaceCameraPos);
					float3 normalWorld = UnityObjectToWorldNormal(v.normal);
					#ifdef _TANGENT_TO_WORLD
						float4 tangentWorld = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);

						float3x3 tangentToWorld = CreateTangentToWorldPerVertex(normalWorld, tangentWorld.xyz, tangentWorld.w);
						o.tangentToWorldAndParallax[0].xyz = tangentToWorld[0];
						o.tangentToWorldAndParallax[1].xyz = tangentToWorld[1];
						o.tangentToWorldAndParallax[2].xyz = tangentToWorld[2];
					#else
						o.tangentToWorldAndParallax[0].xyz = 0;
						o.tangentToWorldAndParallax[1].xyz = 0;
						o.tangentToWorldAndParallax[2].xyz = normalWorld;
					#endif

					TRANSFER_SHADOW(o);

					#ifndef LIGHTMAP_OFF
						o.ambientOrLightmapUV.xy = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
						o.ambientOrLightmapUV.zw = 0;
					#elif UNITY_SHOULD_SAMPLE_SH
						#if UNITY_SAMPLE_FULL_SH_PER_PIXEL
							o.ambientOrLightmapUV.rgb = 0;
						#elif (SHADER_TARGET < 30)
							o.ambientOrLightmapUV.rgb = ShadeSH9(half4(normalWorld, 1.0));
						#else
							o.ambientOrLightmapUV.rgb = ShadeSH3Order(half4(normalWorld, 1.0));
						#endif
						#ifdef VERTEXLIGHT_ON
							o.ambientOrLightmapUV.rgb += Shade4PointLights(
								unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
								unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
								unity_4LightAtten0, posWorld, normalWorld);
						#endif
					#endif

					#ifdef DYNAMICLIGHTMAP_ON
						o.ambientOrLightmapUV.zw = v.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
					#endif

					#ifdef _PARALLAXMAP
						TANGENT_SPACE_ROTATION;
						half3 viewDirForParallax = mul(rotation, ObjSpaceViewDir(v.vertex));
						o.tangentToWorldAndParallax[0].w = viewDirForParallax.x;
						o.tangentToWorldAndParallax[1].w = viewDirForParallax.y;
						o.tangentToWorldAndParallax[2].w = viewDirForParallax.z;
					#endif

					UNITY_TRANSFER_FOG(o, o.pos);
					o.diffuseColor = float3(0.0, 0.0, 0.0);
					o.specularColor = float3(0.0, 0.0, 0.0);
					return o;
				}

				[maxvertexcount(3)]
				void geom(triangle vFwdBase IN[3], inout TriangleStream<vFwdBase> triStream)
				{
					vFwdBase o;
					UNITY_INITIALIZE_OUTPUT(vFwdBase, o);

					float3 v0 = IN[0].pos.xyz;
					float3 v1 = IN[1].pos.xyz;
					float3 v2 = IN[2].pos.xyz;

					float3 centerPos = (v0 + v1 + v2) / 3.0;

					float3 vn = normalize(cross(v1 - v0, v2 - v0));

					float4x4 modelMatrix = unity_ObjectToWorld;
					float4x4 modelMatrixInverse = unity_WorldToObject;

					float3 normalDirection = normalize(
						mul(float4(vn, 0.0), modelMatrixInverse).xyz);
					float3 viewDirection = normalize(_WorldSpaceCameraPos
						- mul(modelMatrix, float4(centerPos, 0.0)).xyz);
					float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
					float attenuation = 1.0;

					float3 ambientLighting =
						UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;

					float3 diffuseReflection =
						attenuation * _LightColor0.rgb * _Color.rgb
						* max(0.0, dot(normalDirection, lightDirection));

					float3 specularReflection;
					if (dot(normalDirection, lightDirection) < 0.0)
					{
						specularReflection = float3(0.0, 0.0, 0.0);
					}
					else
					{
						specularReflection = attenuation * _LightColor0.rgb
							* _SpecColor.rgb * pow(max(0.0, dot(
								reflect(-lightDirection, normalDirection),
								viewDirection)), _Shininess);
					}

					o.pos = IN[0].pos;
					o.pos = UnityObjectToClipPos(IN[0].pos);
					o.tex = IN[0].tex;
					o.eyeVec = IN[0].eyeVec;
					o.tangentToWorldAndParallax = IN[0].tangentToWorldAndParallax;
					o.ambientOrLightmapUV = IN[0].ambientOrLightmapUV;
					TRANSFER_SHADOW(o);
					UNITY_TRANSFER_FOG(o, o.pos);
					o.diffuseColor = ambientLighting + diffuseReflection / 4;
					o.specularColor = specularReflection + diffuseReflection * 3 / 4;
					triStream.Append(o);

					o.pos = IN[1].pos;
					o.pos = UnityObjectToClipPos(IN[1].pos);
					o.tex = IN[1].tex;
					o.eyeVec = IN[1].eyeVec;
					o.tangentToWorldAndParallax = IN[1].tangentToWorldAndParallax;
					o.ambientOrLightmapUV = IN[1].ambientOrLightmapUV;
					TRANSFER_SHADOW(o);
					UNITY_TRANSFER_FOG(o, o.pos);
					o.diffuseColor = ambientLighting + diffuseReflection / 4;
					o.specularColor = specularReflection + diffuseReflection * 3 / 4;
					triStream.Append(o);

					o.pos = IN[2].pos;
					o.pos = UnityObjectToClipPos(IN[2].pos);
					o.tex = IN[2].tex;
					o.eyeVec = IN[2].eyeVec;
					o.tangentToWorldAndParallax = IN[2].tangentToWorldAndParallax;
					o.ambientOrLightmapUV = IN[2].ambientOrLightmapUV;
					TRANSFER_SHADOW(o);
					UNITY_TRANSFER_FOG(o, o.pos);
					o.diffuseColor = ambientLighting + diffuseReflection / 4;
					o.specularColor = specularReflection + diffuseReflection * 3 / 4;
					triStream.Append(o);

				}

				half4 frag(vFwdBase IN) : COLOR
				{
					float z1 = tex2Dproj(_CameraDepthTexture, IN.pos); 
					z1 = LinearEyeDepth(z1);
					float z2 = (IN.pos.z);
					z1 = saturate(0.125 * (abs(z2 - z1)));
					half shadowAtten = SHADOW_ATTENUATION(IN);

					return float4((IN.specularColor * 0.8) +
					IN.diffuseColor , saturate(z1*0.1) + _AlphaFactor);
				}
			ENDCG
		}

		Pass
		{
            Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }

			Fog{ Mode Off }
			ZWrite On ZTest Less Cull Off
            Offset [_ShadowBias],[_ShadowBiasSlope]

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				float rand(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
				}

				float rand2(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
				}

				float _WaveLength;
				float _WaveHeight;
				float _WaveSpeed;
				float _RandomHeight;
				float _RandomSpeed;
				float _CollisionWaveLength;
				vector _CollisionVectors[20];

				struct v2f
				{
					V2F_SHADOW_CASTER;
				};
				
				v2f vert(appdata_base v)
				{
					float3 v0 = mul((float3x3)unity_ObjectToWorld, v.vertex).xyz;

					float collPhase = 0.0;

					for (int i = 0; i < 20; i++)
					{
						float distanceToCenter = length(v0.xz - _CollisionVectors[i].xy);
						float waveHeight = _CollisionVectors[i].z;
						float waveState = _CollisionVectors[i].w;

						if (distanceToCenter < _CollisionWaveLength * 10 * waveState)
						{
							collPhase += (waveHeight * ((1.0 - waveState) * distanceToCenter) / (_CollisionWaveLength * 10 * waveState)) * sin(distanceToCenter - (_CollisionWaveLength * 10 * waveState));
						}
					}

					float phase0 = (_WaveHeight)* sin((_Time[1] * _WaveSpeed) + (v0.x * _WaveLength) + (v0.z * _WaveLength) + rand2(v0.xzz));
					float phase0_1 = (_RandomHeight)* sin(cos(rand(v0.xzz) * _RandomHeight * cos(_Time[1] * _RandomSpeed * sin(rand(v0.xxz)))));
					float phase0_2 = (_WaveHeight / 5.0) * sin((_Time[1] * _WaveSpeed * 3.0) + (v0.x * -_WaveLength * 4.0) + (v0.z * _WaveLength * 4.0) + rand2(v0.xzz));

					v0.y += collPhase + phase0 + phase0_1 + phase0_2;

					v.vertex.xyz = mul((float3x3)unity_WorldToObject, v0);

					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}
				
				float4 frag(v2f i) : COLOR
				{
					SHADOW_CASTER_FRAGMENT(i)
				}
			ENDCG
		}
	}
	Fallback "Diffuse"
}