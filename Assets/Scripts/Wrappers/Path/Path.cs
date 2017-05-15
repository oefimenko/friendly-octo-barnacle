using System.Collections.Generic;
using UnityEngine;

public class Path {

    private int maxLength = 50;
    private float chainLength = 1f;
    private float width = 0.3f;
    private int collapseValue = 5;
    private MeshFilter mf;
    private Mesh mesh;
    private List<Vector3> path = new List<Vector3>();
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> triangles = new List<int>();

    public Path (Vector3 position) {
        GameObject prefab = ResourceManager.Instance.Get("Misc", "Path");
        GameObject gObject = Object.Instantiate(prefab, new Vector3(0, 0), Quaternion.identity);
        mf = gObject.GetComponent<MeshFilter>();
        mesh = new Mesh();
        mf.sharedMesh = mesh;
        mesh.MarkDynamic();
        path.Add(position);
        UpdatePath(GetRectangle(path[0], path[0]));
    }

    public void Update (Vector3 position) {
        float distance = Vector2.Distance(path[path.Count - 1], position);
        if (distance > chainLength && path.Count < maxLength) {
            ExtendOrCollapse(new Vector3(position.x, position.y, 0));
        } else if (path.Count < maxLength) {
            UpdatePath(GetRectangle(path[path.Count - 1], position));
        }
        ApplyMesh();
    }

    public void Complete (Vector3 position) {

    }

    private void ExtendOrCollapse (Vector3 position) {
        int initialValue = path.Count > collapseValue ? path.Count - collapseValue : 0;
        for (int i = initialValue; i < path.Count; i++) {
            if (Vector3.Distance(path[i], position) < chainLength) {
                path.RemoveRange(i + 1, path.Count - i - 1);
                Recalculate();
                break;
            } else if (Vector3.Distance(path[i], position) < Vector3.Distance(path[path.Count - 1], position)) {
                path.RemoveRange(i + 1, path.Count - i - 1);
                path.Add(position);
                Recalculate();
                break;
            } else if (path[i] == path[path.Count - 1]) {
                vertices.RemoveAt(vertices.Count - 2);
                vertices.RemoveAt(vertices.Count - 1);
                ExtendPath(GetRectangle(path[path.Count - 1], position), GetInitialDirection(path.Count - 1));
                path.Add(position);
                break;
            }
        }
    }
    
    private void UpdatePath (Vector3[] newRect) {
        if (vertices.Count == 0) {
            vertices.Add(newRect[0]);
            vertices.Add(newRect[1]);
            AddMeshSection(newRect[2], newRect[3]);
        } else if (vertices.Count == 4) {
            vertices[0] = newRect[0];
            vertices[1] = newRect[1];
            vertices[2] = newRect[2];
            vertices[3] = newRect[3];
        } else {
            vertices[vertices.Count - 2] = newRect[2];
            vertices[vertices.Count - 1] = newRect[3];
        }
    }

    private void Recalculate () {
        // Clear current
        vertices.Clear();
        normals.Clear();
        uvs.Clear();
        triangles.Clear();
        // Add base
        UpdatePath(GetRectangle(path[0], path[0]));
        // Recalculate
        for (int i = 1; i < path.Count; i++) {
            ExtendPath(GetRectangle(path[i - 1], path[i]), GetInitialDirection(i - 1));
        }
        mesh.Clear();
        ApplyMesh();
    }

    private void ExtendPath (Vector3[] newRect, Vector3 initialDirection) {

        SplineBounds spline = GetSplineBounds(newRect, initialDirection);
        OrientedPoint[] points = spline.GetPoints();

        Vector3 basePoint = new Vector3(0, 0, 0);
        Vector3 offsetPoint = new Vector3(0, -width, 0);

        for (int i = 0; i < points.Length; i++) {
            Vector3 a1 = points[i].LocalToWorld(basePoint);
            Vector3 b1 = points[i].LocalToWorld(offsetPoint);
            AddMeshSection(a1, b1);
        }
        AddMeshSection(vertices[vertices.Count - 2], vertices[vertices.Count - 1]);
    }

    private void AddMeshSection (Vector3 endA, Vector3 endB) {
        // Vertices
        vertices.Add(endA);
        vertices.Add(endB);
        // Triangles
        int lastVert = vertices.Count - 1;
        triangles.AddRange(new List<int> {
            lastVert - 3, lastVert - 1, lastVert - 2,
            lastVert - 1, lastVert, lastVert -2
        });
        // Normals 
        int diff = vertices.Count - normals.Count;
        for (int i = 0; i < diff; i++) {
            normals.Add(new Vector3(0, 1, 0));
        }
        // UVs
        for (int i = uvs.Count; i < vertices.Count; i++) {
            uvs.Add(new Vector2(vertices[i].x, vertices[i].y));
        };
    }

    private void ApplyMesh () {
        // Add Arrow Head
        Vector3 lineEndDir = (vertices[vertices.Count - 1] - vertices[vertices.Count - 2]).normalized;
        Vector3 arrowDir = new Vector3(-lineEndDir.y, lineEndDir.x, 0);
        Vector3 midPoint = (vertices[vertices.Count - 1] + vertices[vertices.Count - 2]) / 2f;
        List<Vector3> finalVertices = new List<Vector3>(vertices) {
            midPoint + lineEndDir * width,
            midPoint - lineEndDir * width,
            midPoint + arrowDir * 2 * width,
        };
        List<Vector3> finalNormals = new List<Vector3>(normals) { Vector3.up, Vector3.up, Vector3.up };
        List<Vector2> finalUvs = new List<Vector2>(uvs);
        for (int i = finalUvs.Count; i < finalVertices.Count; i++) {
            finalUvs.Add(
                new Vector2(finalVertices[i].x, finalVertices[i].y)
            );
        }
        List<int> finalTriangles = new List<int>(triangles);
        for (int i = 0; i < 3; i++) finalTriangles.Add(vertices.Count + i);
        // Apply
        mesh.vertices = finalVertices.ToArray();
        mesh.triangles = finalTriangles.ToArray();
        mesh.normals = finalNormals.ToArray();
        mesh.uv = finalUvs.ToArray();
    }

    private Vector3[] GetRectangle (Vector3 start, Vector3 finish) {
        Vector2 direction = finish - start;
        Vector3 offset = new Vector3(-direction.y, direction.x).normalized * 0.5f * width;
        return new Vector3[4] {
            new Vector3 (start.x + offset.x, start.y + offset.y, 0),
            new Vector3 (start.x - offset.x, start.y - offset.y, 0),
            new Vector3 (finish.x + offset.x, finish.y + offset.y, 0),
            new Vector3 (finish.x - offset.x, finish.y - offset.y, 0)
        };
    }

    private SplineBounds GetSplineBounds (Vector3[] rectangle, Vector3 initialDirection) {
        return new SplineBounds(new Vector3[] {
            vertices[vertices.Count - 2],
            vertices[vertices.Count - 1],
            rectangle[rectangle.Length - 2],
            rectangle[rectangle.Length - 1]
        }, initialDirection);
    }

    private Vector3 GetInitialDirection (int pathIndex) {
        int prevIndx = (pathIndex < 1) ? 0 : pathIndex - 1;
        Vector3 initialDirection = path[pathIndex] - path[prevIndx];
        initialDirection.z = 0;
        return initialDirection;
    }

    public void Destroy () {
        mf.mesh = null;
        mesh.Clear();
        GameObject.Destroy(mesh);
        GameObject.Destroy(mf.gameObject);
        mf = null;
        mesh = null;
    }
}