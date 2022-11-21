Shader "Custom/Mask(Stencil)" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader {
        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry-1"
        }

        ColorMask 0
        ZWrite off
        ZTest Always

        Stencil {
            Ref 1
            Comp always
            Pass replace
        }

        Pass{}
    }
    FallBack "Diffuse"
}
