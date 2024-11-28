using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GridRenderer : MonoBehaviour
{
    public int gridSizeX;
    public int gridSizeY;
    public float cellSizeX = 3f;
    public float cellSizeY = 3f;
    public Material gridMaterial;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = gridMaterial;

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(gridSizeX + 1) * (gridSizeY + 1)];
        int[] indices = new int[gridSizeX * (gridSizeY + 1) * 2 + gridSizeY * (gridSizeX + 1) * 2];

        int vert = 0;
        int index = 0;

        for (int y = 0; y <= gridSizeY; y++)
        {
            for (int x = 0; x <= gridSizeX; x++)
            {
                vertices[vert] = new Vector3(x * cellSizeX, 0, y * cellSizeY);

                if (x < gridSizeX)
                {
                    indices[index++] = vert;
                    indices[index++] = vert + 1;
                }

                if (y < gridSizeY)
                {
                    indices[index++] = vert;
                    indices[index++] = vert + gridSizeX + 1;
                }

                vert++;
            }
        }

        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
        meshFilter.mesh = mesh;
    }
}


