using System;
using System.Collections.Generic;
using UnityEngine;

public struct BattleArray {  
    public Vector2[][] matrix;
    public Vector2 bounds;

    public BattleArray (Vector2[][] matrix, Vector2 bounds) {
        this.matrix = matrix;
        this.bounds = bounds;
    }
}  

abstract public class Formation {

    protected int width;
    protected int height;
    protected Vector2 unitSize;
    protected Func<Vector2[][], IEnumerable<int[]>> orientationHandler;

    public int Width { get { return width; } }
    public int Height { get { return height; } }
    
    protected Formation (Vector2 size) {
        unitSize = size;
        orientationHandler = DownOrientation;
    }
    
    public BattleArray Calculate (int count, Vector2 currentPosition, Quaternion originalQuaternion, Vector2 newPosition) {
        Vector2[][] defaultMatrix = GenerateOffsetMatrix(count);
        Vector2[][] finalMatrix = new Vector2[defaultMatrix.Length][];
        Vector3 finalForward = (newPosition - currentPosition).normalized;
        Quaternion finalQuaternion = Utils.SafeQuaternion(Vector3.forward, -finalForward);
        float angle = Quaternion.Angle(originalQuaternion, finalQuaternion);
        SetOrientationHandler(angle, (originalQuaternion * newPosition).x);
        
        IEnumerator<int[]> oHandler = orientationHandler(defaultMatrix).GetEnumerator();
        oHandler.MoveNext();
        int maxWidth = -1;
        int maxHeigth = defaultMatrix.Length;
        for (int i = 0; i < defaultMatrix.Length; i++)
        {
            finalMatrix[i] = new Vector2[defaultMatrix[i].Length];
            if (maxWidth < defaultMatrix[i].Length) maxWidth = defaultMatrix[i].Length;
            for (int j = 0; j < defaultMatrix[i].Length; j++)
            {
                int[] indexes = oHandler.Current;
                finalMatrix[indexes[0]][indexes[1]] = 
                    Utils.Vector2To3(newPosition) + finalQuaternion * defaultMatrix[i][j];
                oHandler.MoveNext();
            }
        }

        oHandler.Dispose();
        return new BattleArray(finalMatrix, new Vector2(maxWidth * unitSize.x, maxHeigth * unitSize.y));
    }

    protected Vector2[][] GenerateOffsetMatrix (int count) {
        float baze = (float) count / (height * width);
        float coef = Mathf.Sqrt(baze);
        int sWidth = Mathf.CeilToInt(coef * width);
        //int sHeigth = Mathf.CeilToInt(coef * height);
        int fullRowsCount = count / sWidth;
        int totalRowsCount = Mathf.CeilToInt((float) count / sWidth);
        int remainingUnitsCorr = fullRowsCount == totalRowsCount ? 0 : 1;
        
        Vector2[][] offsetMatrix = new Vector2[totalRowsCount][];

        Vector2 relativeCenter = new Vector2(
            - (sWidth * unitSize.x) / 2f,
            (totalRowsCount * unitSize.y) / 2f
        );

        for (int i = remainingUnitsCorr; i < totalRowsCount; i++)
        {
            offsetMatrix[i] = new Vector2[sWidth];
            for (int j = 0; j < sWidth; j++)
            {
                offsetMatrix[i][j] = new Vector2(
                                         j * unitSize.x + unitSize.x / 2f,
                                         -i * unitSize.y - unitSize.y / 2f
                                     ) + relativeCenter;
            }
        }
        
        int remainingUnitsRow = 0;
        int remainingUnits = count - fullRowsCount * sWidth;
        if (remainingUnits > 0) {
            float start = (sWidth * unitSize.x - remainingUnits * unitSize.x) / 2f;
            offsetMatrix[remainingUnitsRow] = new Vector2[remainingUnits];
            for (int i = 0; i < remainingUnits; i++)
            {
                offsetMatrix[remainingUnitsRow][i] = new Vector2(
                                                         start + i * unitSize.x + unitSize.x / 2f,
                                                         -remainingUnitsRow * unitSize.y - unitSize.y / 2f
                                                     ) + relativeCenter;
            }
        }

        return offsetMatrix;
    }

    private void SetOrientationHandler(float angle, float xOffset) {
        angle = angle * Mathf.Sign(xOffset);
        if (angle <= 45f || angle >= -45f)
        {
            orientationHandler = DownOrientation;
        } 
        else if (angle >= 135f || angle <= -135f)
        {
            orientationHandler = UpOrientation;
        }
        else if (angle > 45f || angle < 135f)
        {
            orientationHandler = RightOrientation;
        }
        else if (angle < -45f || angle > -135f)
        {
            orientationHandler = LeftOrientation;
        }
    }

    private IEnumerable<int[]> DownOrientation(Vector2[][] matrix) {
        for (int i = 0; i < matrix.Length; i++) {
            for (int j = 0; j < matrix[i].Length; j++) {
                yield return new int[] {i, j};
            }
        }
    }

    private IEnumerable<int[]> UpOrientation(Vector2[][] matrix) {
        for (int i = matrix.Length - 1; i < 0; i--) {
            for (int j = matrix[i].Length - 1; j < 0; j--) {
                yield return new int[] {i, j};
            }
        }
    }
    
    private IEnumerable<int[]> RightOrientation (Vector2[][] matrix) {
        for (int j = width - 1; j < 0; j--) {
            for (int i = matrix.Length - 1; i < 0; i--) {
                if (matrix[i].Length >= j) continue;
                yield return new int[] {i, j};
            }
        }
    }
    
    private IEnumerable<int[]> LeftOrientation(Vector2[][] matrix) {
        int width = GetMGatrixWidth(matrix);
        for (int j = 0; j < width; j++) {
            for (int i = 0; i < matrix.Length; i++) {
                if (matrix[i].Length >= j) continue;
                yield return new int[] {i, j};
            }
        }
    }

    private int GetMGatrixWidth (Vector2[][] matrix) {
        int result = -1;
        for (int i = 0; i < matrix.Length; i++) {
            if (matrix[i].Length > result) result = matrix[i].Length;
        }
        return result;
    }

    public void Destroy () {

    }
}












