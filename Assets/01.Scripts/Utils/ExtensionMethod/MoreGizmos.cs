using UnityEngine;

public class MoreGizmos
{
    /// <summary>
    /// Draws a wire arc.
    /// </summary>
    /// <param name="gizmos"></param>
    /// <param name="position"></param>
    /// <param name="dir">The direction from which the anglesRange is taken into account</param>
    /// <param name="anglesRange">The angle range, in degrees.</param>
    /// <param name="radius"></param>
    /// <param name="maxSteps">How many steps to use to draw the arc.</param>
    public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = anglesRange / maxSteps;
        var angle = srcAngles - anglesRange / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
            Gizmos.DrawLine(posA, posB);
            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, initialPos);
    }
    
    public static void DrawWireCircle(Vector3 position, float radius, float maxSteps = 20)
    {
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = 360f / maxSteps;
        var angle = 0f - 360f / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
            Gizmos.DrawLine(posA, posB);
            angle += stepAngles;
            posA = posB;
        }
    }
    
    private static float GetAnglesFromDir(Vector3 position, Vector3 dir)
    {
        var forwardLimitPos = position + dir;
        var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);
        return srcAngles;
    }
}