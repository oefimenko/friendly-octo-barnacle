using UnityEngine;

abstract public class Formation {

    protected int width;
    protected int height;

    public Formation () { }

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    public void Calculate (int unitCount, Vector2 unitSize, out Vector2[] offset, out Vector2 size) {
        int baseNumber = unitCount / (Height * Width);
        int remainingUnits = unitCount - (Height * Width * baseNumber);
        int halfWidth = baseNumber * Width / 2;
        int halfHeigth = baseNumber * Height / 2;

        Vector2 correction = new Vector2(
            (baseNumber * Width) % 2 == 0 ? unitSize.x / 2 : 0f,
            (baseNumber * Height) % 2 == 0 ? unitSize.y / 2 : 0f
        );

        offset = new Vector2[unitCount];
        int count = 0;

        for (int i = -halfWidth; i <= halfWidth; i++) {
            if (baseNumber * Width % 2 == 0 && i == 0) continue;
            for (int j = -halfHeigth; j <= halfHeigth; j++) {
                if (baseNumber * Height % 2 == 0 && j == 0) continue;
                offset[count] = GenerateOffset(unitSize, correction, i, j);
                count++;
            }
        }

        size = new Vector2(
           unitSize.x * baseNumber * Width,
            unitSize.y * baseNumber * Height
        );

        int remainingUnitsLine = halfHeigth + 1;
        for (int i = -remainingUnits / 2; i <= remainingUnits / 2; i++) {
            if (remainingUnits % 2 == 0 && i == 0) continue;
            offset[count] = GenerateOffset(unitSize, new Vector2(0, 0), i, remainingUnitsLine);
            count++;
        }
        
    }

   protected Vector2 GenerateOffset (Vector2 unitSize, Vector2 correction, int i, int j) {
        float x = unitSize.x * i - Mathf.Sign(i) * correction.x;
        float y = unitSize.y * j - Mathf.Sign(j) * correction.y;
        return new Vector2(x, y);
    }


    public void Destroy () {

    }
}












