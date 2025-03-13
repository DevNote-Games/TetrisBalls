using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    public Shader paintShader; // ������ ��� ��������� ������
    private Material paintMaterial;
    private RenderTexture paintTexture;

    private void Start()
    {
        // ������� ����� RenderTexture ��� �������� ������
        paintTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
        paintTexture.Create();

        // ������� �������� ��� ��������� �����
        paintMaterial = new Material(paintShader);

        // ������������� �������� �� �������� �������
        GetComponent<Renderer>().material.SetTexture("_PaintTex", paintTexture);
    }

    public void Paint(Vector3 worldPosition, float size)
    {
        // ������������ ������� ���������� � UV �������
        Vector2 uv = WorldToUV(worldPosition);

        // ������� ��������� RenderTexture
        RenderTexture temp = RenderTexture.GetTemporary(paintTexture.width, paintTexture.height, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(paintTexture, temp); // �������� ������� ��������

        // ����������� ��������� ��������� ��� ���������
        paintMaterial.SetVector("_UV", uv);
        paintMaterial.SetFloat("_Size", size);

        // ������ ����� ���� ������
        Graphics.Blit(temp, paintTexture, paintMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }

    private Vector2 WorldToUV(Vector3 worldPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(worldPosition + Vector3.up * 5f, Vector3.down, out hit))
        {
            return hit.textureCoord; // �������� UV-���������� ���������
        }
        return Vector2.zero;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                print("Paint");
                Paint(hit.point, 0.05f);
            }
        }
    }
}

