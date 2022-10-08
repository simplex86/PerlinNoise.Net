# PerlinNoise.Net
```
Noise.Perlin2D(x, y)
```
```
Noise.Perlin2D(x, y, t)
```
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
