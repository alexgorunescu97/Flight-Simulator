Shader "Custom/HeightDependentTint"
{
    Properties
    {
      _LandTex("Land (RGB)", 2D) = "white" {}
      _WaterTex("Water (RGB)", 2D) = "white" {}
      _HeightMin("Height Min", Float) = 0
      _HeightMax("Height Max", Float) = 150
      _ColorMin("Tint Color At Min", Color) = (0,0,0,1)
      _ColorMax("Tint Color At Max", Color) = (1,1,1,1)
    }

        SubShader
      {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _LandTex;
        sampler2D _WaterTex;
        fixed4 _ColorMin;
        fixed4 _ColorMax;
        float _HeightMin;
        float _HeightMax;

        struct Input
        {
          float2 uv_LandTex;
          float2 uv_WaterTex;
          float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
          half4 l = tex2D(_LandTex, IN.uv_LandTex);
          half4 w = tex2D(_WaterTex, IN.uv_WaterTex);
          float h = (IN.worldPos.y - _HeightMin) / (_HeightMax - _HeightMin);
          fixed4 tintColor = lerp(_ColorMax.rgba, _ColorMin.rgba, h);
          if (h < 0.05) {
              o.Albedo = w.rgb * tintColor.rgb;
              o.Alpha = w.a * tintColor.a;
          }
          else {
              o.Albedo = l.rgb * tintColor.rgb;
              o.Alpha = l.a * tintColor.a;
          }
        }
        ENDCG
      }
          Fallback "Diffuse"
}