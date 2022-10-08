namespace SimpleX
{
    using System;

    // 柏林噪声
    // 论文：http://www.heathershrewsbury.com/dreu2010/wp-content/uploads/2010/07/AnImageSynthesizer.pdf
    // 参考：http://adrianb.io/2014/08/09/perlinnoise.html
    static class Noise
    {
        private static readonly int[] permutation = new int[] {
            151, 160, 137, 91,  90,  15,  131, 13,  201, 95,  96,  53,  194, 233, 7,   225, 140, 36,
            103, 30,  69,  142, 8,   99,  37,  240, 21,  10,  23,  190, 6,   148, 247, 120, 234, 75,
            0,   26,  197, 62,  94,  252, 219, 203, 117, 35,  11,  32,  57,  177, 33,  88,  237, 149,
            56,  87,  174, 20,  125, 136, 171, 168, 68,  175, 74,  165, 71,  134, 139, 48,  27,  166,
            77,  146, 158, 231, 83,  111, 229, 122, 60,  211, 133, 230, 220, 105, 92,  41,  55,  46,
            245, 40,  244, 102, 143, 54,  65,  25,  63,  161, 1,   216, 80,  73,  209, 76,  132, 187,
            208, 89,  18,  169, 200, 196, 135, 130, 116, 188, 159, 86,  164, 100, 109, 198, 173, 186,
            3,   64,  52,  217, 226, 250, 124, 123, 5,   202, 38,  147, 118, 126, 255, 82,  85,  212,
            207, 206, 59,  227, 47,  16,  58,  17,  182, 189, 28,  42,  223, 183, 170, 213, 119, 248,
            152, 2,   44,  154, 163, 70,  221, 153, 101, 155, 167, 43,  172, 9,   129, 22,  39,  253,
            19,  98,  108, 110, 79,  113, 224, 232, 178, 185, 112, 104, 218, 246, 97,  228, 251, 34,
            242, 193, 238, 210, 144, 12,  191, 179, 162, 241, 81,  51,  145, 235, 249, 14,  239, 107,
            49,  192, 214, 31,  181, 199, 106, 157, 184, 84,  204, 176, 115, 121, 50,  45,  127, 4,
            150, 254, 138, 236, 205, 93,  222, 114, 67,  29,  24,  72,  243, 141, 128, 195, 78,  66,
            215, 61,  156, 180, 151
        };

        private const int permutationMask = 0xff;

        // 获取噪声值（2D)
        public static float Perlin2D(float x, float y)
        {
            return Perlin2D(x, y, 0.01f);
        }

        // 获取噪声值（2D)
        public static float Perlin2D(float x, float y, float t)
        {
            var p = PerlinNoise(x * t, y * t, 0.0f);
            return (p + 1f) * 0.5f;
        }

        // 获取噪声值（3D）
        public static float Perlin(float x, float y, float z)
        {
            return Perlin3D(x, y, z, 0.01f);
        }

        // 获取噪声值（3D）
        public static float Perlin3D(float x, float y, float z, float t)
        {
            var p = PerlinNoise(x * t, y * t, z * t);
            return (p + 1f) * 0.5f;
        }

        // 插值
        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t; ;
        }

        // 平滑
        // 6t^5 − 15t^4 + 10t^3
        private static float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        // 梯度
        // https://mrl.cs.nyu.edu/~perlin/paper445.pdf
        public static float Grad(int hash, float x, float y, float z)
        {
            switch (hash & 0xF)
            {
                case 0x0: return  x + y;
                case 0x1: return -x + y;
                case 0x2: return  x - y;
                case 0x3: return -x - y;
                case 0x4: return  x + z;
                case 0x5: return -x + z;
                case 0x6: return  x - z;
                case 0x7: return -x - z;
                case 0x8: return  y + z;
                case 0x9: return -y + z;
                case 0xA: return  y - z;
                case 0xB: return -y - z;
                case 0xC: return  y + x;
                case 0xD: return -y + z;
                case 0xE: return  y - x;
                case 0xF: return -y - z;
                default: return 0;
            }
        }

        private static float PerlinNoise(float x, float y, float z)
        {
            var X = (int)Math.Floor(x) & permutationMask;
            var Y = (int)Math.Floor(y) & permutationMask;
            var Z = (int)Math.Floor(z) & permutationMask;

            x -= (int)Math.Floor(x);
            y -= (int)Math.Floor(y);
            z -= (int)Math.Floor(z);

            var u = Fade(x);
            var v = Fade(y);
            var w = Fade(z);

            var A  = (permutation[X] + Y)     & permutationMask;
            var B  = (permutation[X + 1] + Y) & permutationMask;
            var AA = (permutation[A] + Z)     & permutationMask;
            var BA = (permutation[B] + Z)     & permutationMask;
            var AB = (permutation[A + 1] + Z) & permutationMask;
            var BB = (permutation[B + 1] + Z) & permutationMask;

            var AAA = permutation[AA];
            var BAA = permutation[BA];
            var ABA = permutation[AB];
            var BBA = permutation[BB];
            var AAB = permutation[AA + 1];
            var BAB = permutation[BA + 1];
            var ABB = permutation[AB + 1];
            var BBB = permutation[BB + 1];

            var x1 = Lerp(Grad(AAA, x, y,     z),     Grad(BAA, x - 1, y,     z),     u);
            var x2 = Lerp(Grad(ABA, x, y - 1, z),     Grad(BBA, x - 1, y - 1, z),     u);
            var x3 = Lerp(Grad(AAB, x, y,     z - 1), Grad(BAB, x - 1, y,     z - 1), u);
            var x4 = Lerp(Grad(ABB, x, y - 1, z - 1), Grad(BBB, x - 1, y - 1, z - 1), u);

            var y1 = Lerp(x1, x2, v);
            var y2 = Lerp(x3, x4, v);

            return Lerp(y1, y2, w);
        }
    }
}
