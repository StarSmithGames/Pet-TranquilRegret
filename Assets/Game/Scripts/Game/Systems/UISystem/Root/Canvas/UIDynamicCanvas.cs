using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.UISystem
{
// #if UNITY_EDITOR
// 	[ ExecuteAlways ]
// #endif
    public class UIDynamicCanvas : UICanvas
    {
	    public CanvasScaler Scaler;
        public Transform DialogsRoot;
        
//         private void Awake()
//         {
// #if UNITY_EDITOR
// 	        if ( !Application.isPlaying ) return;
// #endif
// 	        UpdateCanvasScale();
//         }
//
// #if UNITY_EDITOR
//         private void Update() => UpdateCanvasScale();
// #endif

        private void UpdateCanvasScale()
        {
	        Vector2 size = ScreenUtils.GetAspectSizeNormal();
	        float lastMatch = Scaler.matchWidthOrHeight;
	        //0.5f == 9:16
	        //0.75f == 3:4 or 2:3
	        Scaler.matchWidthOrHeight = Mathf.Lerp( 1f, 0.5f, Mathf.InverseLerp( 0.75f, 1f, size.x ) );
	        if ( Scaler.matchWidthOrHeight != lastMatch )
	        {
		        Debug.LogWarning( $"[Dynamic Canvas] Canvas match changed on {Scaler.matchWidthOrHeight}" );
	        }
        }
    }
}