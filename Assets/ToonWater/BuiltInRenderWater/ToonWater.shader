// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hovl/Transparent/ToonWater"
{
	Properties
	{
		_Water("Water", Color) = (0.4764151,0.730213,1,0.3372549)
		_Fresnel("Fresnel", Float) = 1
		_FogColor("Fog Color", Color) = (0.1839623,0.2900286,1,1)
		_FogDistance("Fog Distance", Float) = 15
		_Foamdistance("Foam distance", Float) = 5
		_NormalMap("NormalMap", 2D) = "bump" {}
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_NormalPower("Normal Power", Float) = 1
		_WavesspeedXY("Waves speed XY", Vector) = (0.01,0,0,0)
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		_FoamSpeed("Foam Speed", Float) = 0.2
		_Foam("Foam", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _NormalMap;
		uniform float4 _WavesspeedXY;
		uniform float4 _NormalMap_ST;
		uniform float _NormalPower;
		uniform float4 _Water;
		uniform float4 _FogColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FogDistance;
		uniform float4 _FoamColor;
		uniform float _Foamdistance;
		uniform float _FoamSpeed;
		uniform sampler2D _Foam;
		uniform float4 _Foam_ST;
		uniform float _Smoothness;
		uniform float _Fresnel;


		float2 voronoihash29( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi29( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash29( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.707 * sqrt(dot( r, r ));
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float time29 = 0.0;
			float2 coords29 = i.uv_texcoord * 0.01;
			float2 id29 = 0;
			float2 uv29 = 0;
			float fade29 = 0.5;
			float voroi29 = 0;
			float rest29 = 0;
			for( int it29 = 0; it29 <4; it29++ ){
			voroi29 += fade29 * voronoi29( coords29, time29, id29, uv29, 0 );
			rest29 += fade29;
			coords29 *= 2;
			fade29 *= 0.5;
			}//Voronoi29
			voroi29 /= rest29;
			float2 appendResult36 = (float2(_WavesspeedXY.x , _WavesspeedXY.y));
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float2 panner34 = ( 1.0 * _Time.y * appendResult36 + uv_NormalMap);
			float3 lerpResult18 = lerp( UnpackNormal( tex2D( _NormalMap, ( uv29 + panner34 ) ) ) , float3( 0,0,1 ) , _NormalPower);
			o.Normal = lerpResult18;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth7 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth7 = abs( ( screenDepth7 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FogDistance ) );
			float clampResult12 = clamp( distanceDepth7 , 0.0 , 1.0 );
			float4 lerpResult9 = lerp( _Water , _FogColor , clampResult12);
			float screenDepth38 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth38 = abs( ( screenDepth38 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Foamdistance ) );
			float clampResult51 = clamp( (1.0 + (distanceDepth38 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float clampResult40 = clamp( ( (1.0 + (distanceDepth38 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) - sin( ( ( clampResult51 - ( _Time.y * _FoamSpeed ) ) * ( 4.0 * UNITY_PI ) ) ) ) , 0.0 , 1.0 );
			float2 appendResult76 = (float2(_WavesspeedXY.z , _WavesspeedXY.w));
			float2 uv_Foam = i.uv_texcoord * _Foam_ST.xy + _Foam_ST.zw;
			float2 panner74 = ( 1.0 * _Time.y * appendResult76 + uv_Foam);
			float temp_output_58_0 = ( clampResult40 * tex2D( _Foam, panner74 ).r );
			float4 lerpResult42 = lerp( lerpResult9 , _FoamColor , temp_output_58_0);
			o.Emission = (lerpResult42).rgb;
			o.Smoothness = _Smoothness;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV4 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode4 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV4, _Fresnel ) );
			float lerpResult5 = lerp( 0.0 , _Water.a , fresnelNode4);
			float clampResult11 = clamp( ( ( lerpResult5 + clampResult12 ) + ( _FoamColor.a * temp_output_58_0 ) ) , 0.0 , 1.0 );
			o.Alpha = clampResult11;
		}

		ENDCG
	}
}
/*ASEBEGIN
Version=18800
307;119;897;990;5878.77;4447.145;8.4872;True;False
Node;AmplifyShaderEditor.RangedFloatNode;44;-2860,790;Inherit;False;Property;_Foamdistance;Foam distance;4;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;38;-2620,710;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;47;-2108,918;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-2108,998;Inherit;False;Property;_FoamSpeed;Foam Speed;10;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;41;-2282.632,710;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1900,886;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;51;-2076,790;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;54;-1724,883;Inherit;False;1;0;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;50;-1753.159,779.6829;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1532,774;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;35;-1625.839,-427.2476;Inherit;False;Property;_WavesspeedXY;Waves speed XY;8;0;Create;True;0;0;0;False;0;False;0.01,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-1521.997,552.0577;Inherit;False;0;60;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;62;-2268.22,525.5138;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;76;-1446.851,673.8101;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinOpNode;52;-1372,774;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;74;-1218.278,552.1425;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1531.844,462.4135;Inherit;False;Property;_FogDistance;Fog Distance;3;0;Create;True;0;0;0;False;0;False;15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1499.175,278.9157;Inherit;False;Property;_Fresnel;Fresnel;1;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;57;-1212,694;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-1220.974,-77.58557;Inherit;False;Property;_Water;Water;0;0;Create;True;0;0;0;False;0;False;0.4764151,0.730213,1,0.3372549;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;7;-1339.437,440.6557;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;36;-1356.819,-396.4626;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1429.679,-543.8665;Inherit;False;0;15;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;60;-986.884,519.4792;Inherit;True;Property;_Foam;Foam;11;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;4;-1356.482,187.0998;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;40;-968.9882,400.6009;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-1226.323,-274.194;Inherit;False;Property;_FogColor;Fog Color;2;0;Create;True;0;0;0;False;0;False;0.1839623,0.2900286,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;5;-900.2554,136.841;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;43;-792.4061,-226.8888;Inherit;False;Property;_FoamColor;Foam Color;9;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;29;-999.9259,-539.3577;Inherit;False;0;1;1;0;4;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0.01;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.ClampOpNode;12;-888.9893,267.464;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;34;-1183.784,-410.3005;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.01,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-664.9673,304.5698;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-678.7257,137.6097;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-479.6741,217.7795;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-742.916,-343.6349;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;9;-730.0272,-28.09776;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-329.5446,143.4248;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;42;-433.4098,-35.32543;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-435.3,-145.6512;Inherit;False;Property;_NormalPower;Normal Power;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-555.7396,-338.0388;Inherit;True;Property;_NormalMap;NormalMap;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;11;-172.036,142.6064;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;-221.9662,-267.4895;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;3;-217.135,-34.54026;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-283.338,66.18384;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;14;32.85882,-75.64163;Float;False;True;-1;2;;0;0;Standard;Hovl/Transparent/ToonWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;38;0;44;0
WireConnection;41;0;38;0
WireConnection;48;0;47;0
WireConnection;48;1;49;0
WireConnection;51;0;41;0
WireConnection;50;0;51;0
WireConnection;50;1;48;0
WireConnection;53;0;50;0
WireConnection;53;1;54;0
WireConnection;62;0;38;0
WireConnection;76;0;35;3
WireConnection;76;1;35;4
WireConnection;52;0;53;0
WireConnection;74;0;75;0
WireConnection;74;2;76;0
WireConnection;57;0;62;0
WireConnection;57;1;52;0
WireConnection;7;0;13;0
WireConnection;36;0;35;1
WireConnection;36;1;35;2
WireConnection;60;1;74;0
WireConnection;4;3;6;0
WireConnection;40;0;57;0
WireConnection;5;1;2;4
WireConnection;5;2;4;0
WireConnection;12;0;7;0
WireConnection;34;0;21;0
WireConnection;34;2;36;0
WireConnection;58;0;40;0
WireConnection;58;1;60;1
WireConnection;10;0;5;0
WireConnection;10;1;12;0
WireConnection;45;0;43;4
WireConnection;45;1;58;0
WireConnection;33;0;29;2
WireConnection;33;1;34;0
WireConnection;9;0;2;0
WireConnection;9;1;8;0
WireConnection;9;2;12;0
WireConnection;39;0;10;0
WireConnection;39;1;45;0
WireConnection;42;0;9;0
WireConnection;42;1;43;0
WireConnection;42;2;58;0
WireConnection;15;1;33;0
WireConnection;11;0;39;0
WireConnection;18;0;15;0
WireConnection;18;2;19;0
WireConnection;3;0;42;0
WireConnection;14;1;18;0
WireConnection;14;2;3;0
WireConnection;14;4;16;0
WireConnection;14;9;11;0
ASEEND*/
//CHKSM=9E5DC367660DE92470928E68825886A5F44662A1