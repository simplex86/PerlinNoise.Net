# PerlinNoise.Net
简单一句代码即可获取2D空间的perlin noise

1、使用默认参数获取柏林噪声的随机数
```
Noise.Perlin2D(x, y)
```
2、使用自定义参数t获取柏林噪声的随机数
```
Noise.Perlin2D(x, y, t)
```
## 生成图片
```
private Bitmap CreateNoiseImage()
{
    Bitmap bitmap = new Bitmap(512, 512);

    var t = T();
    for (int y = 0; y < bitmap.Height; y++)
    {
        for (int x = 0; x < bitmap.Width; x++)
        {
            var p = (int)(Noise.Perlin2D(x, y, t) * 255);
            var c = Color.FromArgb(255, p, p, p);
            bitmap.SetPixel(x, y, c);
        }
    }

    return bitmap;
}
```
![perlinnoise_10](https://github.com/simplex86/PerlinNoise.Net/blob/main/Doc/perlinnoise_10.png)
![perlinnoise_50](https://github.com/simplex86/PerlinNoise.Net/blob/main/Doc/perlinnoise_50.png)
![perlinnoise_100](https://github.com/simplex86/PerlinNoise.Net/blob/main/Doc/perlinnoise_100.png)
## 说明
也有支持获取3D空间下perlin noise的函数
```
Noise.Perlin3D
```
但是我**没有测试过**
