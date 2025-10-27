using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MoveCommander : MonoBehaviour
{

    // �O���b�h��0.5�P�ʂň����iworld * 2 -> �����O���b�h�j
    private const float GridUnit = 0.5f;

    [SerializeField] private Color lineColor = Color.cyan;
    [SerializeField] private float lineWidth = 0.08f;
    [SerializeField] private float lineHeightOffset = 0.05f; // �n�ʂ�菭����ɕ`�悷�邽�߂̃I�t�Z�b�g

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (line == null) line = gameObject.AddComponent<LineRenderer>();

        // �����ݒ�i�K�v�ɉ����ĕύX�j
        line.useWorldSpace = true;
        line.loop = false;
#if UNITY_2019_1_OR_NEWER
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
#else
        line.SetWidth(lineWidth, lineWidth);
#endif
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = lineColor;
        line.endColor = lineColor;
        ClearPath();
    }

    void Update()
    {

    }

    // ���JAPI: occupiedWorldPositions �� BuildingInstaller ���ŊǗ����Ă��� HashSet<Vector3>
    // �߂�l: �o�H�i���[���h���W�̃��X�g�j�B�o�H��������Ȃ���� null ��Ԃ��B
    public List<Vector3> FindPath(Vector3 goalWorldPos, HashSet<Vector3> buildingPositions)
    {
        // buildingPositions: �����̒��S���W�i���[���h���W�j
        // �����T�C�Y�� 3x3�i�O���b�h�P�ʁj�ŌŒ�Ƃ���

        Vector2Int start = ToGrid(transform.position);
        Vector2Int goal = ToGrid(goalWorldPos);

        // buildingPositions �����L�O���b�h���v�Z�i���S�� ToGrid ���� -1..+1 ��W�J�j
        HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
        if (buildingPositions != null)
        {
            foreach (var b in buildingPositions)
            {
                Vector2Int center = ToGrid(b);
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dz = -1; dz <= 1; dz++)
                    {
                        occupied.Add(new Vector2Int(center.x + dx, center.y + dz));
                    }
                }
            }
        }

        // �ڕW����L����Ă���ꍇ�͎��s�i�K�v�Ȃ��O�I������ǉ��j
        if (occupied.Contains(goal))
            return null;

        List<Node> open = new List<Node>();
        HashSet<Vector2Int> closed = new HashSet<Vector2Int>();

        Node startNode = new Node(start, 0, Heuristic(start, goal), null);
        open.Add(startNode);

        int maxIterations = 10000;
        int iter = 0;

        while (open.Count > 0 && iter++ < maxIterations)
        {
            // open����f�ŏ��̃m�[�h���擾
            open.Sort((a, b) => a.F.CompareTo(b.F));
            Node current = open[0];
            open.RemoveAt(0);

            if (current.Pos == goal)
            {
                return ReconstructPath(current);
            }

            closed.Add(current.Pos);

            foreach (var dir in Neighbors)
            {
                Vector2Int neighborPos = current.Pos + dir;
                if (closed.Contains(neighborPos)) continue;
                if (occupied.Contains(neighborPos)) continue; // ��L�Z���͒ʂ�Ȃ�

                float tentativeG = current.G + 1; // 4�����O���b�h�Ȃ̂ŃR�X�g��1

                Node existing = open.Find(n => n.Pos == neighborPos);
                if (existing == null)
                {
                    Node neighbor = new Node(neighborPos, tentativeG, Heuristic(neighborPos, goal), current);
                    open.Add(neighbor);
                }
                else if (tentativeG < existing.G)
                {
                    existing.G = tentativeG;
                    existing.Parent = current;
                }
            }
        }

        // ������Ȃ�����
        return null;
    }

    // �֗����\�b�h: �o�H���󂯎���Ĉړ����J�n����
    public void MoveTo(Vector3 goalWorldPos, HashSet<Vector3> occupiedWorldPositions, float speed = 2f)
    {
        var path = FindPath(goalWorldPos, occupiedWorldPositions);
        if (path == null || path.Count == 0)
        {
            ClearPath();
            return;
        }

        // �o�H��`��
        DrawPath(path);

        StopAllCoroutines();
        StartCoroutine(MoveAlongPath(path, speed));
    }

    private IEnumerator MoveAlongPath(List<Vector3> path, float speed)
    {
        foreach (var target in path)
        {
            // y �͎��I�u�W�F�N�g�� y ���ێ�
            Vector3 dest = new Vector3(target.x, transform.position.y, target.z);
            while (Vector3.SqrMagnitude(transform.position - dest) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
                yield return null;
            }
        }

        // �ړ��������Ɍo�H�\��������
        ClearPath();
    }

    // �O���b�h�ϊ��i���[���h���W -> �����O���b�h�j
    private Vector2Int ToGrid(Vector3 world)
    {
        int gx = Mathf.RoundToInt(world.x / GridUnit);
        int gz = Mathf.RoundToInt(world.z / GridUnit);
        return new Vector2Int(gx, gz);
    }

    // �O���b�h�ϊ��i�����O���b�h -> ���[���h���W�j
    private Vector3 FromGrid(Vector2Int g, float y = 0f)
    {
        return new Vector3(g.x * GridUnit, y, g.y * GridUnit);
    }

    // �}���n�b�^�������i4�����ړ��ɓK����j
    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // �אڃI�t�Z�b�g�i4�����j
    private static readonly Vector2Int[] Neighbors = new Vector2Int[]
    {
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1)
    };

    private List<Vector3> ReconstructPath(Node node)
    {
        List<Vector3> path = new List<Vector3>();
        Node cur = node;
        float y = transform.position.y;
        while (cur != null)
        {
            Vector3 w = FromGrid(cur.Pos, y);
            path.Add(w);
            cur = cur.Parent;
        }
        path.Reverse();
        return path;
    }

    private void DrawPath(List<Vector3> path)
    {
        if (line == null) return;
        int n = path.Count;
        line.positionCount = n;
        Vector3[] pts = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
            Vector3 p = path[i];
            // �������������グ�ĕ`��i�n�ʂƔ��Ȃ��悤�Ɂj
            pts[i] = new Vector3(p.x, p.y + lineHeightOffset, p.z);
        }
        line.SetPositions(pts);
        line.enabled = true;
    }

    private void ClearPath()
    {
        if (line == null) return;
        line.positionCount = 0;
        line.enabled = false;
    }

    private class Node
    {
        public Vector2Int Pos;
        public float G;
        public float H;
        public Node Parent;
        public float F => G + H;

        public Node(Vector2Int pos, float g, float h, Node parent)
        {
            Pos = pos;
            G = g;
            H = h;
            Parent = parent;
        }
    }
}
