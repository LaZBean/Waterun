using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class GameUtility {

    public static PropertyInfo[] GetClassProperties(Object o, BindingFlags f = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
    {
        PropertyInfo[] infos = o.GetType().GetProperties(f);
        return infos;
    }

    

    //2-ple array a[x+y*w]
    //3-ple array a[x+y*w+z*w*h]

    public static int Get2DArrayElementID(int x, int y, int w){
        return x + y * w;
    }

    public static int Get3DArrayElementID(int x, int y, int z, int w, int h){
        return x + y * w + z * w * h;
    }

    public static Vector3 DirFromAngle(float angleInDeg){
		return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad),0,Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
	}

    public static bool Vector3Equal(Vector3 a, Vector3 b, double p = 0.0001f)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    public static void Swap<T>(ref T a, ref T b){
        T temp = a;
        a = b;
        b = temp;
    }

    public static Vector2 Clamp(Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
    }

    public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y), Mathf.Clamp(v.z, min.z, max.z));
    }

    public static bool NearlyEqual(float a, float b, float epsilon)
    {
        const float floatNormal = (1 << 23) * float.Epsilon;
        float absA = Mathf.Abs(a);
        float absB = Mathf.Abs(b);
        float diff = Mathf.Abs(a - b);

        if (a == b)
        {
            // Shortcut, handles infinities
            return true;
        }

        if (a == 0.0f || b == 0.0f || diff < floatNormal)
        {
            // a or b is zero, or both are extremely close to it.
            // relative error is less meaningful here
            return diff < (epsilon * floatNormal);
        }

        // use relative error
        return diff / Mathf.Min((absA + absB), float.MaxValue) < epsilon;
    }

    public static bool InBounds(float v, float min, float max)
    {
        return ((min <= v) && (v <= max));
    }

    public static bool InBounds(Vector2 v, Vector2 min, Vector2 max)
    {
        return ((min.x <= v.x) && (v.x <= max.x)) && ((min.y <= v.y) && (v.y <= max.y));
    }

    public static Vector3 MouseWorldPosOnPlane(Camera cam, Vector3 normal, Vector3 point) {
		Vector3 pos = Vector3.zero;

		if (cam == null) {
			Debug.LogError ("Camera = null");
			return pos;
		}

		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Plane hPlane = new Plane(normal, point);
		float distance = 0; 

		if (hPlane.Raycast(ray, out distance)){
			pos = ray.GetPoint(distance);
		}

        return pos;
	}

	public static Vector2 ScreenPosToRectPosNormalize(Vector2 pos ,RectTransform rect){
		Vector2 localpoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, pos, rect.GetComponentInParent<Canvas>().worldCamera, out localpoint);

		Vector2 normalizedPoint = Rect.PointToNormalized(rect.rect, localpoint);

		return normalizedPoint;
	}

	public static Vector3 DirNormalize(Vector3 dir){
		if (dir.sqrMagnitude > 1) {
			dir.Normalize ();
		}
		return dir;
	}


    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t){
        System.Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t){
        System.Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

    public static bool ContainsWord(this string word, string otherword)
    {
        int currentIndex = 0;

        foreach (var character in otherword)
        {
            if ((currentIndex = word.IndexOf(character, currentIndex)) == -1)
                return false;
        }

        return true;
    }


    public static Rect GetTextAnchor(TextAnchor type){
        switch (type){
            case TextAnchor.UpperLeft:      return new Rect(0,1,1,0);
            case TextAnchor.UpperCenter:    return new Rect(0.5f, 1, 0.5f, 1);
            case TextAnchor.UpperRight:     return new Rect(1,1,1,1);
                                                                                 
            case TextAnchor.MiddleLeft:     return new Rect(0, 0.5f, 0, 0.5f);
            case TextAnchor.MiddleCenter:   return new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            case TextAnchor.MiddleRight:    return new Rect(1, 0.5f, 1, 0.5f);
                                                                                 
            case TextAnchor.LowerLeft:      return new Rect(0, 0, 0, 0);
            case TextAnchor.LowerCenter:    return new Rect(0.5f, 0, 0.5f, 0);
            case TextAnchor.LowerRight:     return new Rect(1, 0, 1, 0);

            default: return new Rect(0, 0, 0, 0);
        }
    }

    public static Vector2 GetTextAnchorPivot(TextAnchor type){
        switch (type){
            case TextAnchor.UpperLeft:      return new Vector2(0,    1);
            case TextAnchor.UpperCenter:    return new Vector2(0.5f, 1);
            case TextAnchor.UpperRight:     return new Vector2(1,    1);

            case TextAnchor.MiddleLeft:     return new Vector2(0,    0.5f);
            case TextAnchor.MiddleCenter:   return new Vector2(0.5f, 0.5f);
            case TextAnchor.MiddleRight:    return new Vector2(1,    0.5f);

            case TextAnchor.LowerLeft:      return new Vector2(0,    0);
            case TextAnchor.LowerCenter:    return new Vector2(0.5f, 0);
            case TextAnchor.LowerRight:     return new Vector2(1,    0);

            default: return new Vector2(0, 0);
        }
    }

    //UI
    public static Vector2 CursorPosToUICanvas(RectTransform transform)
    {
        CanvasScaler scaler = transform.GetComponentInParent<CanvasScaler>();
        RectTransform scalerRect = scaler.GetComponent<RectTransform>();

        Vector2 cpos = (Vector2)Input.mousePosition;
        cpos.x = (cpos.x / Screen.width) * scalerRect.sizeDelta.x; //cursorTransform.sizeDelta.x;
        cpos.y = (cpos.y / Screen.height) * scalerRect.sizeDelta.y;

        return cpos;
    }

    public static Vector2 CursorPosOnUIRect(RectTransform transform)
    {
        CanvasScaler scaler = transform.GetComponentInParent<CanvasScaler>();
        RectTransform scalerRect = scaler.GetComponent<RectTransform>();

        Vector2 cpos = (Vector2)Input.mousePosition;
        cpos.x = (cpos.x / Screen.width) * scalerRect.sizeDelta.x; //cursorTransform.sizeDelta.x;
        cpos.y = (cpos.y / Screen.height) * scalerRect.sizeDelta.y;

        return cpos;
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
	{
		Vector2 size= Vector2.Scale(transform.rect.size, transform.lossyScale);
		float x= transform.position.x + transform.anchoredPosition.x;
		float y= Screen.height - transform.position.y - transform.anchoredPosition.y;

		return new Rect(x, y, size.x, size.y);
	}

    public static Vector3 WorldToUIPoint(Vector3 pos)
    {
        if (Camera.main == null)
            return Vector3.zero;
        Vector3 uiPos = Camera.main.WorldToScreenPoint(pos);
        return uiPos;
    }

    public static Vector3 WorldToGUIPoint(Vector3 pos){
		Vector3 guiPos = Camera.main.WorldToScreenPoint (pos);
		guiPos.y = Screen.height - guiPos.y;
		return guiPos;
	}

	public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 )
	{
		screenPosition1.y = Screen.height - screenPosition1.y;
		screenPosition2.y = Screen.height - screenPosition2.y;

		var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
		var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );

		return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
	}

    public static bool OutOfRange(int v, int max, int min=0){       //OUT OF RANGE FOR ARRAYS [INT]
        return (v < min || max < v);
    }

    public static bool OutOfRange(float v, float max, float min = 0){
        return (v < min || max < v);
    }

    public static bool PointInQuad(Vector2 point, Vector2 cubeSize){
        return (OutOfRange(point.x, cubeSize.x) || OutOfRange(point.y, cubeSize.y));
    }

    public static bool PointInCube(Vector3 point, Vector3 cubeSize){
        return (OutOfRange(point.x, cubeSize.x) || OutOfRange(point.y, cubeSize.y) || OutOfRange(point.z, cubeSize.z));
    }


    public static float RandomValue() {
		return Random.Range (0, 1f);
	}

	public static float RoundToXD(float v, int x) {
		float mult = Mathf.Pow(10.0f, (float)x);
		return Mathf.Round(v * mult) / mult;
	}

	public static Vector3 RoundToXDVector3(Vector3 v, int x){
		return new Vector3 (RoundToXD(v.x, x), RoundToXD(v.y, x), RoundToXD(v.z, x));
	}

	public static Vector3 Vector2ToVector3(Vector2 v){
		return new Vector3 (v.x, v.y, 0);
	}

	public static Vector2 Vector3ToVector2(Vector3 v){
		return new Vector2 (v.x, v.y);
	}

    public static Vector2 VectorRotate(Vector2 v, float angle)
    {
        float x = Mathf.Cos(Mathf.PI/180f * angle);
        float y = Mathf.Sin(Mathf.PI/180f * angle);
        return new Vector2(x*v.x - y*v.y, y*v.x + x*v.y);
    }

    public static bool LinesCross(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {

        float denominator = ((b.x - a.x) * (d.y - c.y)) - ((b.y - a.y) * (d.x - c.x));

        if (denominator == 0)
            return false;

        float numerator1 = ((a.y - c.y) * (d.x - c.x)) - ((a.x - c.x) * (d.y - c.y));
        float numerator2 = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));

        if (numerator1 == 0 || numerator2 == 0)
            return false;

        float r = numerator1 / denominator;
        float s = numerator2 / denominator;

        return (r > 0 && r < 1) && (s > 0 && s < 1);
    }

    public static bool PointInPolygon(Vector2 point, Vector2[] polyPoints)
    {

        float xMin = 0;
        foreach (Vector2 p in polyPoints)
            xMin = Mathf.Min(xMin, p.x);

        Vector2 origin = new Vector2(xMin - 0.1f, point.y);
        int intersections = 0;

        for (int i = 0; i < polyPoints.Length; i++)
        {

            Vector2 pA = polyPoints[i];
            Vector2 pB = polyPoints[(i + 1) % polyPoints.Length];

            if (LinesCross(origin, point, pA, pB))
                intersections++;
        }

        return (intersections & 1) == 1;
    }




    public static Color RandomColor() {
		return new Color(Random.Range (0, 1f), Random.Range (0, 1f), Random.Range (0, 1f));
	}

	public static Color ColorFromRGB(float r, float g, float b, float a=1f) {
		return new Color (r, g, b, a);
	}

	public static Color ColorFromRGB(int r, int g, int b, int a=255) {
		return new Color (r/255f, g/255f, b/255f, a/255f);
	}

	public static Color[] ClearColorArray(int n){
		if (n == 0)	return null;
		Color[] cs = new Color[n];
		Color color = new Color (0, 0, 0, 0);
		for (int i = 0; i < n; i++) {
			cs[i] = color;
		}
		return cs;
	}

	public static Color[] SolidColorArray(Color c, int n) {
		if (n == 0) return null;

		Color[] cs = new Color[n];
		for (int i = 0; i < n; i++) {
			cs[i] = c;
		}
		return cs;
	}

	public static Texture2D GetSpriteTexture(Sprite s) {
		int x = (int)s.rect.x;
		int y = (int)s.rect.y;
		int w = (int)s.rect.width;
		int h = (int)s.rect.height;

		Texture2D tex = new Texture2D(w, h);

		Color[] cs = s.texture.GetPixels (x, y, w, h);
		tex.SetPixels (cs);

		tex.filterMode = FilterMode.Point;
		return tex;
	}

    public static bool TextureIsEmpty(Texture2D s)
    {
        for (int x = 0; x < s.width; x++){
            for (int y = 0; y < s.height; y++){
                if (s.GetPixel(x, y).a != 0) return false;
            }
        }
        return true;
    }

    public static bool TextureIsSolid(Texture2D s)
    {
        Color c = s.GetPixel(0,0);
        for (int x = 0; x < s.width; x++){
            for (int y = 0; y < s.height; y++){
                if (s.GetPixel(x, y) != c) return false;
            }
        }
        return true;
    }



    public static int GetDistanceOnGrid(int x1, int y1, int x2, int y2){

        int dx = Mathf.Abs(x2 - x1);
        int dy = Mathf.Abs(y2 - y1);

        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return (int)(Mathf.Sqrt(2) * diagonalSteps + straightSteps);
    }

	public static int GetDistanceOnGrid(Vector2 p1, Vector2 p2){
		return GetDistanceOnGrid((int)p1.x, (int)p1.y, (int)p2.x, (int)p2.y);
	}

    public static int GetDistanceOnGrid(Vector2Int p1, Vector2Int p2){
        return GetDistanceOnGrid(p1.x, p1.y, p2.x, p2.y);
    }



    public static int[] Dem2ToDem1Array(int[,] map){
		int w = map.GetLength(0);
		int h = map.GetLength(1);
		int[] newMap = new int[w*h];

		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				newMap [x + w * y] = map [x, y];	//((y * 16 + z) * 16 + x) Dem3
			}
		}
		return newMap;
	}

	public static Vector3 GridPosToIsometric(int x, int y, int tileSizeX = 32, int tileSizeY = 16, float pixPerUnit = 100f){
		float nx = (x-y) * (tileSizeX / 2f);
		float ny = (x+y) * (tileSizeY / 2f);
		return new Vector3 (nx, ny) / pixPerUnit;
	}

    public static Vector3 GridPosToIsometric(float x, float y, int tileSizeX = 32, int tileSizeY = 16, float pixPerUnit = 100f)
    {
        float nx = (x - y) * (tileSizeX / 2f);
        float ny = (x + y) * (tileSizeY / 2f);
        return new Vector3(nx, ny) / pixPerUnit;
    }


    public static Vector3 IsometricToGridPos(Vector3 pos, int tileSizeX = 32, int tileSizeY = 16, float pixPerUnit = 100f){
        
        float tsX = tileSizeX / pixPerUnit;
        float tsY = tileSizeY / pixPerUnit;

        float nx = (pos.x / tsX + pos.y / tsY) * 1f;
        float ny = (pos.y / tsY - pos.x / tsX) * 1f;
        return new Vector3(nx, ny) ;
    }




    public static Vector2Int WorldToGridPos(Vector2 pos, int tileWidth = 16, int tileHeight = 16, Transform parent = null , float pixPerUnit = 100f){

		Vector3 offset = (parent == null) ? Vector3.zero : parent.transform.position;

		int nx = (int)((pos.x - offset.x) / (tileWidth / pixPerUnit) );
		int ny = (int)((pos.y - offset.y) / (tileHeight / pixPerUnit) );
		return new Vector2Int(nx, ny) ;
	}

    public static Vector3 WorldToGridPos(Vector3 pos, int tileWidth = 16, int tileHeight = 16, int tileDepth=16, Transform parent = null, float pixPerUnit = 100f){

        Vector3 offset = (parent == null) ? Vector3.zero : parent.transform.position;

        float nx = ((pos.x - offset.x) / (tileWidth / pixPerUnit));
        float ny = ((pos.y - offset.y) / (tileHeight / pixPerUnit));
        float nz = ((pos.z - offset.z) / (tileDepth / pixPerUnit));
        return new Vector3(nx, ny, nz);
    }





    //EXTENSIONS
    public enum RoundType{
        Floor = 0,
        Round = 1,
        Ceil = 2
    }
    
    //VECTOR3
    public static Vector3Int ToVector3Int(this Vector3 v, RoundType type = RoundType.Floor){
        Vector3Int p;
        switch (type){
            case RoundType.Round: p = new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z)); break;
            case RoundType.Ceil: p = new Vector3Int(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y), Mathf.CeilToInt(v.z)); break;
            default: p = new Vector3Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z)); break;
        }
        return p;
    }

    public static Vector3 Mul(this Vector3 v, Vector3 v1){
        return new Vector3(v.x * v1.z, v.y * v1.z, v.z * v1.z);
    }

    public static Vector3 RandomVector3(float radius=1f)
    {
        return new Vector3(Random.value * radius, Random.value * radius, Random.value * radius);
    }

    //VECTOR3INT
    public static Vector3 ToVector3(this Vector3Int v){
        return new Vector3(v.x, v.y, v.z);
    }





    public static float DirAngle(Vector2 toVector2, Vector2 fromVector2) {
		float angle = Vector2.Angle(fromVector2, toVector2);
		Vector3 cross = Vector3.Cross(fromVector2, toVector2);
		if (cross.z > 0)	angle = 360 - angle;
		return angle;
	}

    public static float AngleToCamera(Transform t, Camera cam = null){
        if (cam == null) cam = Camera.main;

        float angle = 0;

        Vector3 direction = (cam.transform.position - t.position).normalized;
        angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        angle -= (t.eulerAngles.y);

        while (angle < 0)
            angle += 360;
            
        return angle;
    }

    public static int SideOfCircle(float angle, int sides=8){
		float a = 360f / sides;
		for (int i = 0; i<sides; i++) {
			if (i == 0 && (angle >= 360f-(a / 2f) && angle <= 360f) || (angle < (a / 2) && angle >= 0)) {
				return 0;
			} 
			else {
				if(angle>=((a*i)-(a/2f)) && angle<((a*i)+(a/2))){
					return i;
				}
			}
		}
		return 0;
	}


    public static Vector3 RotateVector2(Vector3 v, float angle){

        float a = Mathf.Deg2Rad * angle;
        float x = Mathf.Cos(a) * v.x - Mathf.Sin(a) * v.y;
        float y = Mathf.Sin(a) * v.x + Mathf.Cos(a) * v.y;

        return new Vector3(x, y);
    }



    public static string ColorToHex(Color32 c){
		return c.r.ToString ("X2") + c.g.ToString ("X2") + c.b.ToString ("X2") + c.a.ToString ("X2");
	}

	public static Color32 HexToColor(string hex){
		hex = hex.Replace ("0x", "");// 0xFFFFFF
		hex = hex.Replace ("#", "");// #FFFFFF
		byte a = 255;
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		}
		return (new Color32(r,g,b,a));
	}



	public static void DestroyAllChildren(Transform parent){
		for (int i = 0; i < parent.childCount; i++) {
			Object.Destroy (parent.GetChild (i).gameObject);
		}
	}

    public static void DisableAllChildren(Transform parent, bool t){
        for (int i = 0; i < parent.childCount; i++){
            parent.GetChild(i).gameObject.SetActive(!t);
        }
    }



    public static void GUIDrawSprite(Rect rect, Sprite sprite, bool flipX=false, bool flipY=false)
    {
        if (Event.current.type == EventType.Repaint)
        {
            Rect c = sprite.rect;
            var tex = sprite.texture;
            c.xMin /= tex.width;
            c.xMax /= tex.width;
            c.yMin /= tex.height;
            c.yMax /= tex.height;
            if(flipX)
                c.width = -c.width;
            if(flipY)
                c.height = -c.height;
            GUI.DrawTextureWithTexCoords(rect, tex, c);
        }
    }

    public static void GUILayoutDrawSprite(Sprite sprite, float width=32, float height=32)
    {
        Rect c = sprite.rect;
        
        Rect rect = GUILayoutUtility.GetRect(width, height);
        //rect.width = width;
        //rect.height = height;
        if (Event.current.type == EventType.Repaint)
        {
            var tex = sprite.texture;
            c.xMin /= tex.width;
            c.xMax /= tex.width;
            c.yMin /= tex.height;
            c.yMax /= tex.height;
            GUI.DrawTextureWithTexCoords(rect, tex, c);
        }
    }

    #if (UNITY_EDITOR)
    public static void EditorGUIDrawCurve(Rect a, Rect b)
    {
        Vector3 startPos = new Vector3(a.x + a.width/2, a.y+a.height/2, 0);
        Vector3 endPos = new Vector3(b.x + b.width / 2, b.y + b.height / 2, 0);

        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;

        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, (i+1)*5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
    #endif

    public static Vector3 LinearBezierCurve(Vector3 a, Vector3 b, float t)
    {
        return a + t * (b - a);
    }

    public static Vector3 QuadraticBezierCurve(Vector3 a, Vector3 b, Vector3 p0, float t)
    {
        return (Mathf.Pow(1-t,2)*a) + (2*(1-t)*t*p0) + (Mathf.Pow(t,2)* b);
    }

    public static Vector3 CubicBezierCurve(Vector3 a, Vector3 b, Vector3 p0, Vector3 p1, float t)
    {
        return (Mathf.Pow(1-t, 3) * a) 
            + (3 * Mathf.Pow(1-t, 2) * t * p0) 
            + (3 * (1-t) * Mathf.Pow(t,2) * p1) 
            + (Mathf.Pow(t, 3) * b);
    }
}




[System.Serializable]
public struct BasicColor{

	public BasicColor(int r, int g, int b, int a=255){
		_r = (byte)r;
		_g = (byte)g;
		_b = (byte)b;
		_a = (byte)a;
	}

	public BasicColor(float r, float g, float b, float a=1f){
		_r = (byte)((int)(r*255));
		_g = (byte)((int)(g*255));
		_b = (byte)((int)(b*255));
		_a = (byte)((int)(a*255));
	}

	[SerializeField]byte _r;
	[SerializeField]byte _g;
	[SerializeField]byte _b;
	[SerializeField]byte _a;

	public int r{ get{return _r;} set{_r = (byte)value;} }
	public int g{ get{return _g;} set{_g = (byte)value;} }
	public int b{ get{return _b;} set{_b = (byte)value;} }
	public int a{ get{return _a;} set{_a = (byte)value;} }

	public Color ToColor(){
		return GameUtility.ColorFromRGB (_r, _g, _b, _a);
	}

	public static BasicColor FromColor(Color color){
		return new BasicColor (color.r, color.g, color.b, color.a);
	}

	public static BasicColor Random(){
		return FromColor(GameUtility.RandomColor ());
	}

}







[System.Serializable]
public struct RectInt{
	[SerializeField]int _x;
	[SerializeField]int _y;
	[SerializeField]int _w;
	[SerializeField]int _h;

	public RectInt(int x, int y, int w, int h){ 
		_x = x; 
		_y = y; 
		_w = w; 
		_h = h;
	}

	public int x{ get{return _x;} set{_x = value;} }
	public int y{ get{return _y;} set{_y = value;} }
	public int w{ get{return _w;} set{_w = value;} }
	public int h{ get{return _h;} set{_h = value;} }

	public Rect ToRect(){
		return new Rect(_x, _y, _w, _h);
	}

	public static RectInt FromRect(Rect r){
		return new RectInt ((int)r.x, (int)r.y, (int)r.width, (int)r.height);
	}
}
	